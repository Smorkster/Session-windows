using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Session_windows
{
	public partial class MainForm : Form
	{
			[DllImport("user32.dll", SetLastError = true)]
		static extern bool GetWindowRect(IntPtr hWnd, ref RECT Rect);

			[DllImport("user32.dll", SetLastError = true)]
			[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

		/// <summary>
		/// Enumeration of available windowcommands for WINDOWPLACEMENT
		/// </summary>
		internal enum ShowWindowCommands
		{
			SW_SHOWNORMAL = 1,
			// Displayed
			SW_SHOWMINIMIZED = 2,
			// Minimized
			SW_MAXIMIZE = 3,
			// Maximized
		}

		/// <summary>
		/// Struct for Windowplacement of windows
		/// </summary>
			[Serializable]
			[StructLayout(LayoutKind.Sequential)]
		internal struct WINDOWPLACEMENT
		{
			public int length;
			public int flags;
			public ShowWindowCommands showCmd;
			public Point ptMinPosition;
			public Point ptMaxPosition;
			public Rectangle rcNormalPosition;
		}

		/// <summary>
		/// Struct for windowcoordinates
		/// </summary>
			[StructLayout(LayoutKind.Sequential)]
		public struct RECT
		{
			public int left;
			public int top;
			public int right;
			public int bottom;
		}

		IEnumerable<Process> activeProcesses;
		ProcessInfo currentProcess;
		Settings settings;
		Session activeSession = new Session("current", new List<ProcessInfo>());

		/// <summary>
		/// Entrypoint of mainform, called from systrayicon
		/// Fill listbox of sessions from data in the XML-file
		/// </summary>
		/// <param name="s">All settings read from XML-file</param>
		public MainForm(ref Settings s)
		{
			InitializeComponent();
			lvProcesses.SelectedIndexChanged -= lvProcesses_SelectedIndexChanged;

			if (s != null) {
				this.settings = s;
				checkboxStart.Checked = settings.StartInSysTray;
				populateSessions();
			}
			populateProcessList("current", "");
			Text = "Session window version " + System.Reflection.Assembly.GetExecutingAssembly()
				.GetName().Version;

			lvProcesses.SelectedIndexChanged += lvProcesses_SelectedIndexChanged;
		}

		/// <summary>
		/// Update processlist with windowinfo from currently running processes
		/// </summary>
		public void populateProcessList(string sessionName, string excludedProcess)
		{
			lvProcesses.Items.Clear();

			// No saved session was chosen. Load information from the currently running processes.
			if(sessionName.Equals("current"))
			{
				groupBoxProcesses.Text = "Currently running processes";
				activeSession.Plist = new List<ProcessInfo>();
				btnGetRunningProcesses.Enabled = false;
				btnDeleteSession.Enabled = false;

				activeProcesses = Process.GetProcesses()
					.Where(p => p.MainWindowHandle != IntPtr.Zero &&
						p.ProcessName != "iexplore" &&
						p.Id != Process.GetCurrentProcess().Id)
					.OrderBy(p => p.ProcessName);
				foreach (Process activeProcess in activeProcesses) {
					if (!activeProcess.ProcessName.Equals(excludedProcess)) {
						IntPtr handle = activeProcess.MainWindowHandle;
						WINDOWPLACEMENT placement = new WINDOWPLACEMENT();
						placement.length = Marshal.SizeOf(placement);
						RECT Rect = new RECT();
						if (GetWindowRect(handle, ref Rect)) {
							GetWindowPlacement(activeProcess.MainWindowHandle, ref placement);
							IntPtr mainWindowHandle = activeProcess.MainWindowHandle;
							string processName = activeProcess.ProcessName;
							string mainWindowTitle = activeProcess.MainWindowTitle.Equals("") ? "Process: " + activeProcess.ProcessName : activeProcess.MainWindowTitle;
							int processID = activeProcess.Id;
							int xTopCoordinate = Rect.left;
							int yTopCoordinate = Rect.top;
							int xBottomCoordinate = Rect.right;
							int yBottomCoordinate = Rect.bottom;
							int width = xBottomCoordinate - xTopCoordinate;
							int height = yBottomCoordinate - yTopCoordinate;
							int windowPlacement = (int)placement.showCmd;

							activeSession.Plist.Add(new ProcessInfo(mainWindowHandle, processID, processName, xTopCoordinate, yTopCoordinate, width, height, windowPlacement));
							lvProcesses.Items.Add(new ListViewItem(new [] {
								processID.ToString(),
								activeProcess.ProcessName
							}));
						}
					}
				}
			} else {
				groupBoxProcesses.Text = "Processes saved in session '" + activeSession.SessionName + "'.";
				btnGetRunningProcesses.Enabled = true;
				btnDeleteSession.Enabled = true;

				foreach(ProcessInfo pInfo in activeSession.Plist)
				{
					Process[] processes = Process.GetProcessesByName(pInfo.ProcessName);
					if(!processes.Any())
					{
						ListViewItem lvi = new ListViewItem(new [] {
							"(not started)",
							pInfo.ProcessName
						});
						lvi.ForeColor = Color.Red;
						lvi.Font = new Font(Font, FontStyle.Italic);
						lvProcesses.Items.Add(lvi);
					} else {
						lvProcesses.Items.Add(new ListViewItem(new [] {
							pInfo.ProcessID.ToString(),
							pInfo.ProcessName
						}));
					}
					pInfo.WatchProcess();
					pInfo.Started += new ProcessInfo.StartingEventHandler(processStarting_EventHandler);
					pInfo.Terminated += new ProcessInfo.TerminatingEventHandler(processTerminated_EventHandler);
				}
			}
		}

		/// <summary>
		/// Fill the contextmenu for sessions
		/// </summary>
		void populateSessions()
		{
			string[] sessionList = settings.getSessionList();
			if (sessionList != null) {
				foreach (string session in sessionList) {
					conmSessions.Items.Add(session, null, sessionMenu_EventHandler);
				}
				populateDockedLists();

				// Checks which session is selected for docked and undocked, then display that in the comboboxes.
				comboboxDocked.SelectedIndex = settings.Docked.Equals("") ? -1 : comboboxDocked.FindStringExact(settings.Docked);
				comboboxUndocked.SelectedIndex = settings.Undocked.Equals("") ? -1 : comboboxUndocked.FindStringExact(settings.Undocked);
			}
		}

		/// <summary>
		/// Fills the dropdownlist for choosing what settings to be used when docked/undocked
		/// </summary>
		void populateDockedLists()
		{
			comboboxUndocked.Items.Clear();
			comboboxDocked.Items.Clear();
			foreach (string s in settings.getSessionList()) {
				comboboxDocked.Items.Add(s);
				comboboxUndocked.Items.Add(s);
			}
		}

		/// <summary>
		/// Turn on the eventhandlers to enable messages being shown 
		/// </summary>
		void eventsOn()
		{
			txtX.TextChanged += txtX_TextChanged;
			txtY.TextChanged += txtY_TextChanged;
			txtWidth.TextChanged += txtWidth_TextChanged;
			txtHeight.TextChanged += txtHeight_TextChanged;
			checkboxStart.CheckedChanged += checkboxStart_CheckedChanged;
			comboboxWindowPlacement.SelectedIndexChanged += comboboxWindowPlacement_SelectedIndexChanged;
			comboboxDocked.SelectedIndexChanged += comboboxDocked_SelectedIndexChanged;
			comboboxUndocked.SelectedIndexChanged += comboboxUndocked_SelectedIndexChanged;
		}

		/// <summary>
		/// Turn off eventhandlers to avoid messages from being shown unnecessarily
		/// </summary>
		void eventsOff()
		{
			txtX.TextChanged -= txtX_TextChanged;
			txtY.TextChanged -= txtY_TextChanged;
			txtWidth.TextChanged -= txtWidth_TextChanged;
			txtHeight.TextChanged -= txtHeight_TextChanged;
			checkboxStart.CheckedChanged -= checkboxStart_CheckedChanged;
			comboboxWindowPlacement.SelectedIndexChanged -= comboboxWindowPlacement_SelectedIndexChanged;
			comboboxDocked.SelectedIndexChanged -= comboboxDocked_SelectedIndexChanged;
			comboboxUndocked.SelectedIndexChanged -= comboboxUndocked_SelectedIndexChanged;
		}

		/// <summary>
		/// Remove information of process
		/// Called after click in listbox where no listboxitem is located
		/// </summary>
		void clearTextBoxes()
		{
			txtHeight.Text = "";
			txtWidth.Text = "";
			txtX.Text = "";
			txtY.Text = "";
			txtProcess.Text = "";
			txtId.Text = "";
			comboboxWindowPlacement.SelectedIndex = -1;
			btnSetProcessInfoLayout.Enabled = false;
		}

		/// <summary>
		/// Handles the event of a menuitem in contextmenu for sessions, have been clicked.
		/// Get the buttoncontrol calling the contextmenu and act according to its tag.
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void sessionMenu_EventHandler(object sender, EventArgs e)
		{
			string ownerButtonFunction = ((sender as ToolStripItem).Owner as ContextMenuStrip).SourceControl.Tag.ToString();

			switch (ownerButtonFunction)
			{
				case "load":
					foreach(ProcessInfo p in activeSession.Plist)
					{
						p.Dispose();
					}
					activeSession = settings.getSession((sender as ToolStripItem).Text);
					string sessionname = (sender as ToolStripItem).Text;
					populateProcessList(sessionname, "");
					populateDockedLists();
					break;
				case "delete":
					foreach(ProcessInfo p in activeSession.Plist)
					{
						p.Dispose();
					}
					if(activeSession.SessionName.Equals((sender as ToolStripItem).Text))
					{
						populateProcessList("current", "");
						populateDockedLists();
					}
					settings.deleteSession((sender as ToolStripItem).Text);
					new FileHandler().write(ref settings);
					break;
				case "save":
					settings.saveSession((sender as ToolStripItem).Text, activeSession);
					new FileHandler().write(ref settings);
					break;
			}
		}

		/// <summary>
		/// Eventhandler for activewindows menu
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		/// <param name="i">ProcessID of window to show info about</param>
		static void activewindowEventHandler(object sender, EventArgs e, int i)
		{
			WindowInfo wi = new WindowInfo(i);
			wi.Show();
		}

		/// <summary>
		/// Eventhandler for if a process in lvProcesses, shown as not started, have started
		/// Change text apperance and insert processID
		/// </summary>
		/// <param name="p">ProcessInfo for the process that have started</param>
		void processStarting_EventHandler(ProcessInfo p)
		{
			ListViewItem i = lvProcesses.FindItemWithText(p.ProcessName);
			i.ForeColor = Color.Black;
			i.Font = new Font(Font, FontStyle.Regular);
			i.SubItems[0].Text = p.ProcessID.ToString();
			lvProcesses.Items[i.Index] = i;
			//MessageBox.Show(i.Index.ToString(),"");
			//MessageBox.Show(p.ProcessName, "");
		}

		/// <summary>
		/// Eventhandler for if a process in lvProcesses, have terminated
		/// Change text apperance and remove processID
		/// </summary>
		/// <param name="p">ProcessInfo for the process that terminated</param>
		void processTerminated_EventHandler(ProcessInfo p)
		{
			ListViewItem i = lvProcesses.Items.Cast<ListViewItem>().FirstOrDefault(x => x.SubItems[0].Text.Equals(p.ProcessID.ToString()));
			i.ForeColor = Color.Red;
			i.Font = new Font(Font, FontStyle.Italic);
			i.SubItems[0].Text = "(not started)";
			lvProcesses.Items[i.Index] = i;
		}

		/// <summary>
		/// Clear list of processes, clear boxes of info and collect info for the active session
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void btnGetRunningProcesses_Click(object sender, EventArgs e)
		{
			eventsOff();
			clearTextBoxes();
			lvProcesses.Items.Clear();
			activeSession.Plist = new List<ProcessInfo>();
			activeSession.SessionName = "current";
			currentProcess = null;
			populateProcessList(activeSession.SessionName, "");
			btnCreateNewSession.Enabled = true;
			eventsOn();
		}

		/// <summary>
		/// Show info about button when mouse is hovering
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void btnGetProcesses_MouseHover(object sender, EventArgs e)
		{
			ttInfo.Active = true;
			ttInfo.Show("List processes with open, visible windows.", (Button)sender, 0, 18);
		}

		/// <summary>
		/// The mousepointer is no longer over the control.
		/// Hide the tooltip
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void control_MouseLeave(object sender, EventArgs e)
		{
			ttInfo.Active = false;
		}

		/// <summary>
		/// New layoutinfo have been entered, set information to the processes. Does not save to session.
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void btnSetProcessInfo_Click(object sender, EventArgs e)
		{
			currentProcess.Height = int.Parse(txtHeight.Text);
			currentProcess.Width = int.Parse(txtWidth.Text);
			currentProcess.ProcessName = txtProcess.Text;
			currentProcess.XTopCoordinate = int.Parse(txtX.Text);
			currentProcess.YTopCoordinate = int.Parse(txtY.Text);
			currentProcess.WindowPlacement = comboboxWindowPlacement.SelectedIndex + 1;

			ProcessInfo tempProcess = new ProcessInfo(Process.GetProcessById(currentProcess.ProcessID).MainWindowHandle,
				                          currentProcess.ProcessID,
				                          currentProcess.ProcessName,
				                          currentProcess.XTopCoordinate,
				                          currentProcess.YTopCoordinate,
				                          currentProcess.Width,
				                          currentProcess.Height,
				                          currentProcess.WindowPlacement);
			activeSession.updateProcess(txtProcess.Text, tempProcess);
			if (currentProcess.ProcessID != 0)
				new WindowLayout().setLayout(tempProcess);
		}

		/// <summary>
		/// Information about a process have been updated, save it to settings
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void btnSaveProcessInfo_Click(object sender, EventArgs e)
		{
			currentProcess.Height = int.Parse(txtHeight.Text);
			currentProcess.Width = int.Parse(txtWidth.Text);
			currentProcess.ProcessName = txtProcess.Text;
			currentProcess.XTopCoordinate = int.Parse(txtX.Text);
			currentProcess.YTopCoordinate = int.Parse(txtY.Text);
			currentProcess.WindowPlacement = comboboxWindowPlacement.SelectedIndex + 1;

			ProcessInfo tempProcess = new ProcessInfo(Process.GetProcessById(currentProcess.ProcessID).MainWindowHandle,
				                          currentProcess.ProcessID,
				                          currentProcess.ProcessName,
				                          currentProcess.XTopCoordinate,
				                          currentProcess.YTopCoordinate,
				                          currentProcess.Width,
				                          currentProcess.Height,
				                          currentProcess.WindowPlacement);
			activeSession.updateProcess(txtProcess.Text, tempProcess);

			if (!activeSession.SessionName.Equals("current")) {
				settings.saveSession(activeSession);
				new FileHandler().write(ref settings);
			}
		}

		/// <summary>
		/// Open a contextmenu, displaying all saved sessions, for loading a session
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void btnLoadSession_Click(object sender, EventArgs e)
		{
			conmSessions.Show(btnLoadSession, new Point(118, 23));
		}

		/// <summary>
		/// Open a contextmenu for saving the current session to an existing name
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void btnSaveSession_Click(object sender, EventArgs e)
		{
			conmSessions.Show(btnSaveSession, new Point(118, 23));
		}

		/// <summary>
		/// Open a contextmenu of saved sessions, clicking on the name deletes that session
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void btnDeleteSession_Click(object sender, EventArgs e)
		{
			conmSessions.Show(btnDeleteSession, new Point(118,23));
		}

		/// <summary>
		/// Creates a new session from the processes shown in lvProcesses and their configurations
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void btnCreateNewSession_Click(object sender, EventArgs e)
		{
			NewSessionName newSessionName = new NewSessionName();
			newSessionName.ShowDialog();

			if (newSessionName.DialogResult == DialogResult.OK) {
				settings.addSession(new Session(newSessionName.getName(), activeSession.Plist));

				conmSessions.Items.Add(newSessionName.getName());
				comboboxUndocked.Items.Add(newSessionName.getName());
				comboboxDocked.Items.Add(newSessionName.getName());
			}
			new FileHandler().write(ref settings);
		}

		/// <summary>
		/// Use specified layoutinformation for all processes
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void btnSetSettings_Click(object sender, EventArgs e)
		{
			foreach (ProcessInfo p in settings.sessionProcessList) {
				new WindowLayout().setLayout(p);
			}
		}

		/// <summary>
		/// The listbox for processes have been clicked
		/// Listbox item at clickpoint?
		/// Yes: Fetch the info of the process specified by the listboxitem and enter it in the textboxes
		/// No: Clear currentProcess and empty the textboxes
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void lvProcesses_SelectedIndexChanged(object sender, EventArgs e)
		{
			currentProcess = null;
			if (lvProcesses.SelectedItems.Count == 0) {
				btnWinInfo.Enabled = false;
			} else {
				btnWinInfo.Enabled = true;
				ttInfo.Active = false;
				string selectedProcess = lvProcesses.SelectedItems[0].SubItems[1].Text;
				currentProcess = activeSession.Plist.Find(x => x.ProcessName.Equals(selectedProcess));
				eventsOff();

				if (currentProcess.ProcessID == 0) {
					txtProcess.Text = currentProcess.ProcessName;
					txtId.Text = "No process for " + currentProcess.ProcessName + " is running";
				} else {
					switch (currentProcess.WindowPlacement) {
						case 3:
							comboboxWindowPlacement.SelectedIndex = 2;
							break;
						case 2:
							comboboxWindowPlacement.SelectedIndex = 1;
							break;
						default:
							comboboxWindowPlacement.SelectedIndex = 0;
							break;
					}
					if (currentProcess.WindowPlacement == 2 || currentProcess.WindowPlacement == 3) {
						txtHeight.Enabled = false;
						txtWidth.Enabled = false;
						txtY.Enabled = false;
						txtX.Enabled = false;
					} else {
						txtHeight.Enabled = true;
						txtWidth.Enabled = true;
						txtY.Enabled = true;
						txtX.Enabled = true;
					}
					txtX.Text = currentProcess.XTopCoordinate.ToString();
					txtY.Text = currentProcess.YTopCoordinate.ToString();
					txtHeight.Text = currentProcess.Height.ToString();
					txtWidth.Text = currentProcess.Width.ToString();
					txtProcess.Text = currentProcess.ProcessName;
					txtId.Text = currentProcess.ProcessID.ToString();
					btnSetProcessInfoLayout.Enabled = true;
				}
				eventsOn();
			}
		}

		/// <summary>
		/// Check if a click in lvProcesses occured over an item
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic MouseEventArgs</param>
		void lvProcesses_MouseDown(object sender, MouseEventArgs e)
		{
			ListViewItem item = lvProcesses.GetItemAt(e.X, e.Y);

			if (item == null) {
				lvProcesses.SelectedIndices.Clear();
				eventsOff();
				clearTextBoxes();
				currentProcess = null;
			}
			if (e.Button == MouseButtons.Right) {
				conmenuDeleteProcess.Show(Cursor.Position);
				conmenuDeleteProcess.Visible = true;
			}
		}

		/// <summary>
		/// Text for window left most X-coordinate have changed
		/// Check if the coordinate is within visible range
		/// If to close to the edge, use more visible coordinate 
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void txtX_TextChanged(object sender, EventArgs e)
		{
			if (currentProcess != null) {
				if (txtX.Text.Length == 0)
					txtX.Text = "0";
				if (int.Parse(txtX.Text) > SystemInformation.VirtualScreen.Width)
					txtX.Text = string.Format("{0}", SystemInformation.VirtualScreen.Width - 20);
				currentProcess.XTopCoordinate = int.Parse(txtX.Text);
			}
		}

		/// <summary>
		/// Text for window upper most Y-coordinate have changed
		/// Check if the coordinate is within visible range
		/// If to close to the edge, use more visible coordinate 
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void txtY_TextChanged(object sender, EventArgs e)
		{
			if (currentProcess != null) {
				if (txtY.Text.Length == 0)
					txtY.Text = "0";
				if (int.Parse(txtY.Text) > SystemInformation.VirtualScreen.Height)
					txtY.Text = string.Format("{0}", SystemInformation.VirtualScreen.Height - 20);
				currentProcess.YTopCoordinate = int.Parse(txtY.Text);
			}
		}

		/// <summary>
		/// Text for window width have changed
		/// If no width is entered, use previous width
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void txtWidth_TextChanged(object sender, EventArgs e)
		{
			if (currentProcess != null) {
				if (txtWidth.Text.Length == 0 || int.Parse(txtWidth.Text) == 0) {
					txtWidth.Text = currentProcess.Width.ToString();
					ttInfo.Show("Window can't be 0 pixels wide", txtWidth, 0, 18);
				} else {
					currentProcess.Width = int.Parse(txtWidth.Text);
				}
			}
		}

		/// <summary>
		/// Text for window height have changed
		/// If no width is entered, use previous height
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void txtHeight_TextChanged(object sender, EventArgs e)
		{
			if (currentProcess != null) {
				if (txtHeight.Text.Length == 0) {
					txtHeight.Text = currentProcess.Height.ToString();
					ttInfo.Show("Window can't be 0 pixels high", txtHeight, 0, 18);
				} else {
					currentProcess.Height = int.Parse(txtHeight.Text);
				}
			}
		}

		/// <summary>
		/// Removes a process from lvProcesses and from the current session
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void contextmenuDeleteProcess_Click(object sender, EventArgs e)
		{
			activeSession.deleteProcessFromSession(currentProcess);
			if (!activeSession.SessionName.Equals("current")) {
				settings.saveSession(activeSession);
				new FileHandler().write(ref settings);
			}
			eventsOff();
			clearTextBoxes();
			populateProcessList(activeSession.SessionName, currentProcess.ProcessName);
			eventsOn();
		}

		/// <summary>
		/// The setting for how the process' window is show, have changed
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void comboboxWindowPlacement_SelectedIndexChanged(object sender, EventArgs e)
		{
			currentProcess.WindowPlacement = comboboxWindowPlacement.SelectedIndex + 1;
			activeSession.updateProcess(currentProcess.ProcessName, currentProcess);
			if (comboboxWindowPlacement.SelectedIndex != 0) {
				txtX.Enabled = false;
				txtY.Enabled = false;
				txtWidth.Enabled = false;
				txtHeight.Enabled = false;
			} else {
				txtX.Enabled = true;
				txtY.Enabled = true;
				txtWidth.Enabled = true;
				txtHeight.Enabled = true;
			}
		}

		/// <summary>
		/// Rolldown the list when clicked
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void comboboxMinimized_Click(object sender, EventArgs e)
		{
			comboboxWindowPlacement.DroppedDown = true;
		}

		/// <summary>
		/// User have selected which session is to be used when the computer is docked (have more than one screen connected)
		/// Remove that session from the other combobox to avoid it being used
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void comboboxDocked_SelectedIndexChanged(object sender, EventArgs e)
		{
			settings.Docked = comboboxDocked.SelectedItem.ToString();
			new FileHandler().write(ref settings);
		}

		/// <summary>
		/// User have selected which session is to be used when the computer is not docked (only one screen connected)
		/// Remove that session from the other combobox to avoid it being used
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void comboboxUndocked_SelectedIndexChanged(object sender, EventArgs e)
		{
			settings.Undocked = comboboxUndocked.SelectedItem.ToString();
			new FileHandler().write(ref settings);
		}

		/// <summary>
		/// Tooltip shown for textbox for coordinates when mouse is hovering
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void txtCoordinate_MouseHover(object sender, EventArgs e)
		{
			ttInfo.Active = true;
			ttInfo.Show("Coordinate of the upper left cornet", (TextBox)sender, 0, 18);
		}

		/// <summary>
		/// User wants the application to start minimized to the notificationarea
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void checkboxStart_CheckedChanged(object sender, EventArgs e)
		{
			settings.StartInSysTray = checkboxStart.Checked;
			btnSaveSession.Enabled = true;
		}

		/// <summary>
		/// Open a menu with active windows
		/// Clicking on an item, opens a window with information about the process and window
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="ea">Generic EventArgs</param>
		void btnWinInfo_Click(object sender, EventArgs ea)
		{
			if (conmsActiveWindows.Items.Count < 1) {
				IEnumerable<Process> listProcesses = Process.GetProcesses()
					.Where(p => p.MainWindowHandle != IntPtr.Zero)
					.OrderBy(p => p.ProcessName);
				foreach (Process p in listProcesses) {
					IntPtr handle = p.MainWindowHandle;
					IntPtr h = p.MainWindowHandle;
					string pn = p.ProcessName;
					string mwt = p.MainWindowTitle.Equals("") ? "Process: " + p.ProcessName : "Process: " + p.ProcessName + " - " + p.MainWindowTitle;
					int i = p.Id;
					conmsActiveWindows.Items.Add(mwt + "(" + i + ")", null, (s, e) => activewindowEventHandler(s, e, i));
				}
			}
			conmsActiveWindows.Show(Cursor.Position);
		}

		/// <summary>
		/// Terminates the application
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void btnClose_Click(object sender, EventArgs e)
		{
			
			Application.Exit();
		}

		/// <summary>
		/// Applies the settings of the loaded session
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void btnApplySession_Click(object sender, EventArgs e)
		{
			foreach(ProcessInfo pI in activeSession.Plist)
			{
				new WindowLayout().setLayout(pI);
			}
		}

		/// <summary>
		/// When the form is closing, dispose of the notifyicon
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			Program.icon.Dispose();
		}

		/// <summary>
		/// The mainform is minimized, hide the form, i.e. "close to tray"
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void MainForm_Resize(object sender, EventArgs e)
		{
			if(this.WindowState == FormWindowState.Minimized){
				this.Hide();
			}
		}
	}
}
