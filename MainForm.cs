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

		/// <summary>
		/// A list of processes that is currently running, that has a visible window
		/// </summary>
		IEnumerable<Process> runningProcesses;
		/// <summary>
		/// Contains the currently marked process in the list 
		/// </summary>
		ProcessInfo markedProcess;
		/// <summary>
		/// Object containing all settings and sessions
		/// </summary>
		Settings settings;
		/// <summary>
		/// Name of the session currently shown in application
		/// </summary>
		String activeSession;

		/// <summary>
		/// Entrypoint of mainform, called from systrayicon
		/// Fill listbox of sessions from data in the XML-file
		/// </summary>
		/// <param name="s">All settings read from XML-file</param>
		public MainForm(ref Settings s)
		{
			InitializeComponent();

			eventsOff();
			if (s != null) {
				this.settings = s;
				checkboxStart.Checked = settings.StartInSysTray;
				populateSessions();
			}
			activeSession = "current";
			populateProcessList(activeSession, "");
			Text = "Session window version " + System.Reflection.Assembly.GetExecutingAssembly()
				.GetName().Version;
			eventsOn();
		}

		IEnumerable<Process> getListOfRunningProcesses()
		{
			return Process.GetProcesses()
					.Where(p => p.MainWindowHandle != IntPtr.Zero &&
						p.ProcessName != "iexplore" &&
						p.Id != Process.GetCurrentProcess().Id)
					.OrderBy(p => p.ProcessName);
		}

		/// <summary>
		/// Update processlist with windowinfo from currently running processes
		/// </summary>
		public void populateProcessList(string sessionName, string excludedProcess)
		{
			lvProcesses.Items.Clear();

			List<ListViewItem> items = new List<ListViewItem>();
			// No saved session was chosen. Load information from the currently running processes.
			if(sessionName.Equals("current"))
			{
				groupBoxProcesses.Text = "Currently running processes";
				settings.currentlyRunningProcesses.Plist = new List<ProcessInfo>();
				btnDeleteLoadedSession.Enabled = false;
				bool explorerAdded = false;

				runningProcesses = getListOfRunningProcesses();
				foreach (Process process in runningProcesses) {
					if (!process.ProcessName.Equals(excludedProcess) && !settings.isExcludedApp(process.ProcessName)) {
						if (process.ProcessName.Equals("explorer")) {
							if (explorerAdded) {
								continue;
							} else {
								explorerAdded = true;
							}
						}

						IntPtr handle = process.MainWindowHandle;
						WINDOWPLACEMENT placement = new WINDOWPLACEMENT();
						placement.length = Marshal.SizeOf(placement);
						RECT Rect = new RECT();
						if (GetWindowRect(handle, ref Rect)) {
							GetWindowPlacement(process.MainWindowHandle, ref placement);
							IntPtr mainWindowHandle = process.MainWindowHandle;
							string processName = process.ProcessName;
							string mainWindowTitle = process.MainWindowTitle.Equals("") ? "Process: " + process.ProcessName : process.MainWindowTitle;
							int processID = process.Id;
							int xTopCoordinate = Rect.left;
							int yTopCoordinate = Rect.top;
							int xBottomCoordinate = Rect.right;
							int yBottomCoordinate = Rect.bottom;
							int width = xBottomCoordinate - xTopCoordinate;
							int height = yBottomCoordinate - yTopCoordinate;
							int windowPlacement = (int)placement.showCmd;

							settings.currentlyRunningProcesses.Plist.Add(new ProcessInfo(mainWindowHandle, processID, processName, xTopCoordinate, yTopCoordinate, width, height, windowPlacement));
							ListViewItem lvi = new ListViewItem(new [] { processID.ToString(), process.ProcessName});
							items.Add(lvi);
					    }
					}
				}
			} else {
				groupBoxProcesses.Text = "Session: " + activeSession;
				btnDeleteLoadedSession.Enabled = true;
				var session = settings.getSession(activeSession);

				foreach(ProcessInfo pInfo in session.Plist)
				{
					if (!settings.isExcludedApp(pInfo.ProcessName))
					{
						Process[] processes = Process.GetProcessesByName(pInfo.ProcessName);
						ListViewItem lvi;
						if(!processes.Any())
						{
							lvi = new ListViewItem(new [] {
								"(not started)",
								pInfo.ProcessName
							});
							lvi.ForeColor = Color.Red;
							lvi.Font = new Font(Font, FontStyle.Italic);
						} else {
							lvi = new ListViewItem(new [] {
								pInfo.ProcessID.ToString(),
								pInfo.ProcessName
							});
						}
						items.Add(lvi);
					}
				}
			}
			ListViewItem[] processesToAdd = items.ToArray();
			lvProcesses.Items.AddRange(processesToAdd);
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
			lvProcesses.SelectedIndexChanged += lvProcesses_SelectedIndexChanged;
			txtX.TextChanged += txtX_TextChanged;
			txtY.TextChanged += txtY_TextChanged;
			txtWidth.TextChanged += txtWidth_TextChanged;
			txtHeight.TextChanged += txtHeight_TextChanged;
			checkboxStart.CheckedChanged += checkboxStart_CheckedChanged;
			checkboxTaskbar.CheckedChanged += checkboxTaskbar_CheckedChanged;
			comboboxWindowPlacement.SelectedIndexChanged += comboboxWindowPlacement_SelectedIndexChanged;
			comboboxDocked.SelectedIndexChanged += comboboxDocked_SelectedIndexChanged;
			comboboxUndocked.SelectedIndexChanged += comboboxUndocked_SelectedIndexChanged;
		}

		/// <summary>
		/// Turn off eventhandlers to avoid messages from being shown unnecessarily
		/// </summary>
		void eventsOff()
		{
			lvProcesses.SelectedIndexChanged -= lvProcesses_SelectedIndexChanged;
			txtX.TextChanged -= txtX_TextChanged;
			txtY.TextChanged -= txtY_TextChanged;
			txtWidth.TextChanged -= txtWidth_TextChanged;
			txtHeight.TextChanged -= txtHeight_TextChanged;
			checkboxStart.CheckedChanged -= checkboxStart_CheckedChanged;
			checkboxTaskbar.CheckedChanged -= checkboxTaskbar_CheckedChanged;
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
		/// A process is to be deleted from the session
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void deleteProcess(object sender, EventArgs e)
		{
			if (activeSession.Equals("current"))
			{
				settings.currentlyRunningProcesses.deleteProcessFromSession(markedProcess);
			} else {
				settings.getSession(activeSession).deleteProcessFromSession(markedProcess);
				settings.saveSession(activeSession, settings.getSession(activeSession));
				new FileHandler().write(ref settings);
			}
			eventsOff();
			clearTextBoxes();
			populateProcessList(activeSession, markedProcess.ProcessName);
			eventsOn();
		}

		/// <summary>
		/// A new application have been select to be excluded from processing
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void excludeApplication(object sender, EventArgs e)
		{
			settings.updateExcludedApplications(markedProcess.ProcessName);
			populateProcessList(activeSession, null);
			new FileHandler().write(ref settings);
		}


		/// <summary>
		/// Handles the event of a menuitem, in contextmenu for sessions, have been clicked.
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
					eventsOff();
					activeSession = (sender as ToolStripItem).Text;
					populateProcessList(activeSession, "");
					populateDockedLists();
					checkboxTaskbar.Checked = settings.getSession(activeSession).TaskbarVisible;
					eventsOn();
					break;
				case "delete":
					if(activeSession.Equals((sender as ToolStripItem).Text))
					{
						populateProcessList("current", "");
						populateDockedLists();
					}
					settings.deleteSession((sender as ToolStripItem).Text);
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
		/// Clear list of processes, clear boxes of info and collect info for the active session
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void btnGetRunningProcesses_Click(object sender, EventArgs e)
		{
			eventsOff();
			clearTextBoxes();
			lvProcesses.Items.Clear();
			activeSession = "current";
			markedProcess = null;
			populateProcessList(activeSession, "");
			btnDeleteLoadedSession.Enabled = false;
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
		/// New layoutinfo have been entered, use for the process' window. Does not save to session to file.
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void btnSetProcessInfo_Click(object sender, EventArgs e)
		{
			markedProcess.Height = int.Parse(txtHeight.Text);
			markedProcess.Width = int.Parse(txtWidth.Text);
			markedProcess.ProcessName = txtProcess.Text;
			markedProcess.XTopCoordinate = int.Parse(txtX.Text);
			markedProcess.YTopCoordinate = int.Parse(txtY.Text);
			markedProcess.WindowPlacement = comboboxWindowPlacement.SelectedIndex + 1;

			ProcessInfo tempProcess = new ProcessInfo(Process.GetProcessById(markedProcess.ProcessID).MainWindowHandle,
				                          markedProcess.ProcessID,
				                          markedProcess.ProcessName,
				                          markedProcess.XTopCoordinate,
				                          markedProcess.YTopCoordinate,
				                          markedProcess.Width,
				                          markedProcess.Height,
				                          markedProcess.WindowPlacement);
			if (activeSession.Equals("current"))
		    {
		    	settings.currentlyRunningProcesses.updateProcess(tempProcess);
		    } else {
		    	settings.getSession(activeSession).updateProcess(tempProcess);
		    }

			if (markedProcess.ProcessID != 0)
				new WindowLayout().setLayout(tempProcess);
		}

		/// <summary>
		/// Information about a process have been updated, save to session and file
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void btnSaveProcessInfo_Click(object sender, EventArgs e)
		{
			markedProcess.Height = int.Parse(txtHeight.Text);
			markedProcess.Width = int.Parse(txtWidth.Text);
			markedProcess.ProcessName = txtProcess.Text;
			markedProcess.XTopCoordinate = int.Parse(txtX.Text);
			markedProcess.YTopCoordinate = int.Parse(txtY.Text);
			markedProcess.WindowPlacement = comboboxWindowPlacement.SelectedIndex + 1;

			ProcessInfo tempProcess = new ProcessInfo(Process.GetProcessById(markedProcess.ProcessID).MainWindowHandle,
				                          markedProcess.ProcessID,
				                          markedProcess.ProcessName,
				                          markedProcess.XTopCoordinate,
				                          markedProcess.YTopCoordinate,
				                          markedProcess.Width,
				                          markedProcess.Height,
				                          markedProcess.WindowPlacement);
			if (activeSession.Equals("current"))
		    {
		    	settings.currentlyRunningProcesses.updateProcess(tempProcess);
		    } else {
		    	settings.getSession(activeSession).updateProcess(tempProcess);
		    	settings.saveSession(activeSession, settings.getSession(activeSession));
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
		/// Open a contextmenu of saved sessions, clicking on the name deletes that session
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void btnDeleteLoadedSession_Click(object sender, EventArgs e)
		{
			settings.deleteSession(activeSession);
		}

		/// <summary>
		/// Creates a new session from the processes shown in lvProcesses and their configurations
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void btnCreateNewSession_Click(object sender, EventArgs e)
		{
			NewSessionName newSessionName = new NewSessionName(settings.getSessionList());
			newSessionName.ShowDialog();

			if (newSessionName.DialogResult == DialogResult.OK) {
				settings.addSession(new Session(newSessionName.getName(), settings.currentlyRunningProcesses.Plist));

				conmSessions.Items.Add(newSessionName.getName());
				comboboxUndocked.Items.Add(newSessionName.getName());
				comboboxDocked.Items.Add(newSessionName.getName());
				new FileHandler().write(ref settings);
			}
		}

		/// <summary>
		/// The listbox for processes have been clicked
		/// ListViewItem where the click occured?
		/// Yes: Get the info of the process specified by the ListViewItem, enter it in the textboxes, and set markedProcess
		/// No: Clear markedProcess and empty the textboxes
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void lvProcesses_SelectedIndexChanged(object sender, EventArgs e)
		{
			markedProcess = null;
			if (lvProcesses.SelectedItems.Count == 0) {
				btnWinInfo.Enabled = false;
			} else {
				btnWinInfo.Enabled = true;
				ttInfo.Active = false;
				string selectedProcess = lvProcesses.SelectedItems[0].SubItems[1].Text;
				if (activeSession.Equals("current")) {
					markedProcess = settings.currentlyRunningProcesses.getProcess(selectedProcess);
				} else {
					markedProcess = settings.getSession(activeSession).getProcess(selectedProcess);
				}

				eventsOff();
				if (markedProcess.ProcessID == 0) {
					txtProcess.Text = markedProcess.ProcessName;
					txtId.Text = "No process for " + markedProcess.ProcessName + " is running";
					txtId.Font = new Font(Font, FontStyle.Italic);
					txtId.BackColor = Color.Tomato;
				} else {
					txtId.Font = new Font(Font, FontStyle.Regular);
					txtId.BackColor = Control.DefaultBackColor;
					switch (markedProcess.WindowPlacement) {
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
					if (markedProcess.WindowPlacement == 2 || markedProcess.WindowPlacement == 3) {
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
					txtX.Text = markedProcess.XTopCoordinate.ToString();
					txtY.Text = markedProcess.YTopCoordinate.ToString();
					txtHeight.Text = markedProcess.Height.ToString();
					txtWidth.Text = markedProcess.Width.ToString();
					txtProcess.Text = markedProcess.ProcessName;
					txtId.Text = markedProcess.ProcessID.ToString();
					btnSetProcessInfoLayout.Enabled = true;
				}
				eventsOn();
			}
		}

		/// <summary>
		/// Check if a click in lvProcesses occured over an item
		/// If there are no ListViewItem where the click occured, clear boxes of information and clear markedProcess
		/// If the right mousebutton was clicked, open contextmenu for removal of process
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
				markedProcess = null;
			}
			if (e.Button == MouseButtons.Right) {
				conmenuProcessAction.Show(Cursor.Position);
				conmenuProcessAction.Visible = true;
			}
		}

		/// <summary>
		/// Text for window left most X-coordinate have changed
		/// Check if the coordinate is within visible range
		/// If coordinate is missing or too close to screenedge, use more visible coordinate 
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void txtX_TextChanged(object sender, EventArgs e)
		{
			if (markedProcess != null) {
				if (txtX.Text.Length == 0)
					txtX.Text = "0";
				if (int.Parse(txtX.Text) > SystemInformation.VirtualScreen.Width)
					txtX.Text = string.Format("{0}", SystemInformation.VirtualScreen.Width - 20);
				markedProcess.XTopCoordinate = int.Parse(txtX.Text);
			}
		}

		/// <summary>
		/// Text for window upper most Y-coordinate have changed
		/// Check if the coordinate is within visible range
		/// If coordinate is missing or too close to screenedge, use more visible coordinate 
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void txtY_TextChanged(object sender, EventArgs e)
		{
			if (markedProcess != null) {
				if (txtY.Text.Length == 0)
					txtY.Text = "0";
				if (int.Parse(txtY.Text) > SystemInformation.VirtualScreen.Height)
					txtY.Text = string.Format("{0}", SystemInformation.VirtualScreen.Height - 20);
				markedProcess.YTopCoordinate = int.Parse(txtY.Text);
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
			if (markedProcess != null) {
				if (txtHeight.Text.Length == 0) {
					txtHeight.Text = markedProcess.Height.ToString();
					ttInfo.Show("Window can't be 0 pixels high", txtHeight, 0, 18);
				} else {
					markedProcess.Height = int.Parse(txtHeight.Text);
				}
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
			if (markedProcess != null) {
				if (txtWidth.Text.Length == 0 || int.Parse(txtWidth.Text) == 0) {
					txtWidth.Text = markedProcess.Width.ToString();
					ttInfo.Show("Window can't be 0 pixels wide", txtWidth, 0, 18);
				} else {
					markedProcess.Width = int.Parse(txtWidth.Text);
				}
			}
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
		/// Rolldown the list when clicked
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void comboboxMinimized_Click(object sender, EventArgs e)
		{
			comboboxWindowPlacement.DroppedDown = true;
		}

		/// <summary>
		/// The setting for how the process' window is shown, have changed
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void comboboxWindowPlacement_SelectedIndexChanged(object sender, EventArgs e)
		{
			markedProcess.WindowPlacement = comboboxWindowPlacement.SelectedIndex + 1;
			if (activeSession.Equals("current")) {
				settings.currentlyRunningProcesses.updateProcess(markedProcess);
			} else {
				settings.getSession(activeSession).updateProcess(markedProcess);
			}

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
			new FileHandler().write(ref settings);
		}

		/// <summary>
		/// Setting for the visibility of taskbar in the saved session
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void checkboxTaskbar_CheckedChanged(object sender, EventArgs e)
		{
			settings.getSession(activeSession).TaskbarVisible = checkboxTaskbar.Checked;
			new FileHandler().write(ref settings);
		}

		/// <summary>
		/// Open a menu with active windows
		/// Clicking on an item, opens a window with information about the process and window
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="ea">Generic EventArgs</param>
		void btnWinInfo_Click(object sender, EventArgs ea)
		{
			conmsActiveWindows.Items.Clear();

			IEnumerable<Process> listProcesses = getListOfRunningProcesses();
			foreach (Process p in listProcesses) {
				IntPtr handle = p.MainWindowHandle;
				IntPtr h = p.MainWindowHandle;
				string pn = p.ProcessName;
				string mwt = p.MainWindowTitle.Equals("") ? "Process: " + p.ProcessName : "Process: " + p.ProcessName + " - " + p.MainWindowTitle;
				int i = p.Id;
				conmsActiveWindows.Items.Add(mwt + "(" + i + ")", null, (s, e) => activewindowEventHandler(s, e, i));
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
			settings.getSession(activeSession).useSession();
		}

		/// <summary>
		/// Remove the process from session
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void btnRemoveProcess_Click(object sender, EventArgs e)
		{
			deleteProcess(null, null);
		}

		/// <summary>
		/// Open a form with list of all excluded applications
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void btnExcludedApplications_Click(object sender, EventArgs e)
		{
			ExcludedApplications ex = new ExcludedApplications(settings.getExcludedApps());
			DialogResult ans = ex.ShowDialog();
			if (ans == DialogResult.OK)
			{
				settings.replaceExcludedApplicationsList(ex.getExcludedApplications());
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
			Application.Exit();
		}

		/// <summary>
		/// The mainform is minimized, hide the form, i.e. "minimize to tray"
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void MainForm_Resize(object sender, EventArgs e)
		{
			if(WindowState == FormWindowState.Minimized){
				Hide();
			}
		}
	}
}
