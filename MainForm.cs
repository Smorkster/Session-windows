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
		static extern bool GetWindowRect (IntPtr hWnd, ref RECT Rect);

		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool GetWindowPlacement (IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

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

		/// <summary>
		/// Entrypoint of mainform, called from systrayicon
		/// Fill listbox of sessions from data in the XML-file
		/// </summary>
		/// <param name="settings">All settings read from XML-file</param>
		public MainForm(ref Settings s)
		{
			InitializeComponent();
			this.settings = s;
			lbProcesses.SelectedIndexChanged -= lbProcesses_SelectedIndexChanged;
			populateSessions();
			checkboxStart.Checked = settings.StartInSysTray;
			lbProcesses.SelectedIndexChanged += lbProcesses_SelectedIndexChanged;
			Text = "Session window version " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
		}

		/// <summary>
		/// Take data from current processlist and fill the listbox
		/// If processlist is empty (pList), create list from currently running processes
		/// </summary>
		public void populateProcesses ()
		{
			if (settings.currentProcessesList == null) {
				activeProcesses = Process.GetProcesses().Where(p => p.MainWindowHandle != IntPtr.Zero &&
				p.ProcessName != "iexplore" &&
				p.Id != Process.GetCurrentProcess().Id);
				settings.currentProcessesList = new List<ProcessInfo>();
				foreach (Process p in activeProcesses) {
					IntPtr handle = p.MainWindowHandle;
					WINDOWPLACEMENT placement = new WINDOWPLACEMENT();
					placement.length = Marshal.SizeOf(placement);
					RECT Rect = new RECT();
					if (GetWindowRect(handle, ref Rect)) {
						GetWindowPlacement(p.MainWindowHandle, ref placement);
						IntPtr h = p.MainWindowHandle;
						string pn = p.ProcessName;
						string mwt;
						mwt = p.MainWindowTitle.Equals("") ? "Process: " + p.ProcessName : p.MainWindowTitle;
						int i = p.Id;
						int l = Rect.left;
						int t = Rect.top;
						int r = Rect.right;
						int b = Rect.bottom;
						int pl = (int)placement.showCmd;
						settings.currentProcessesList.Add(new ProcessInfo(h, i, pn, mwt, l, t, r, b, pl));
						lbProcesses.Items.Add(p.ProcessName);
					}
				}
			} else {
				foreach (ProcessInfo item in settings.currentProcessesList) {
					lbProcesses.Items.Add(item.ProcessName);
				}
			}
		}

		/// <summary>
		/// Fill the listbox for sessions
		/// </summary>
		void populateSessions ()
		{
			string[] sessionList = settings.getSessionList();
			if (sessionList != null) {
				off();
				foreach (string s in sessionList) {
					lbSessions.Items.Add(s);
				}
				populateDockedLists();

				string temp = settings.Docked;
				if (!string.IsNullOrEmpty(temp)) {
					if (settings.Docked.Equals(settings.Undocked)) {
						comboboxUndocked.SelectedIndex = comboboxDocked.SelectedIndex = comboboxDocked.FindStringExact(settings.Docked);
					} else if (settings.Docked.Equals("")) {
						comboboxUndocked.SelectedIndex = comboboxDocked.SelectedIndex = -1;
					} else {
						comboboxUndocked.SelectedIndex = comboboxUndocked.FindStringExact(settings.Undocked);
						comboboxDocked.SelectedIndex = comboboxDocked.FindStringExact(settings.Docked);
					}
				}
				on();
			}
		}

		void populateDockedLists ()
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
		void on ()
		{
			txtX.TextChanged += txtX_TextChanged;
			txtY.TextChanged += txtY_TextChanged;
			txtWidth.TextChanged += txtWidth_TextChanged;
			txtHeight.TextChanged += txtHeight_TextChanged;
			comboboxMinimized.SelectedIndexChanged += comboboxMinimized_SelectedIndexChanged;
			comboboxDocked.SelectedIndexChanged += comboboxDocked_SelectedIndexChanged;
			comboboxUndocked.SelectedIndexChanged += comboboxUndocked_SelectedIndexChanged;
		}

		/// <summary>
		/// Turn off eventhandlers to avoid messages from being shown unnecessarily
		/// </summary>
		void off ()
		{
			txtX.TextChanged -= txtX_TextChanged;
			txtY.TextChanged -= txtY_TextChanged;
			txtWidth.TextChanged -= txtWidth_TextChanged;
			txtHeight.TextChanged -= txtHeight_TextChanged;
			comboboxMinimized.SelectedIndexChanged -= comboboxMinimized_SelectedIndexChanged;
			comboboxDocked.SelectedIndexChanged -= comboboxDocked_SelectedIndexChanged;
			comboboxUndocked.SelectedIndexChanged -= comboboxUndocked_SelectedIndexChanged;
		}

		/// <summary>
		/// Remove information of process
		/// Called after click in listbox where no listboxitem is located
		/// </summary>
		void clearTextBoxes ()
		{
			txtHeight.Text = "";
			txtWidth.Text = "";
			txtX.Text = "";
			txtY.Text = "";
			txtProcess.Text = "";
			txtId.Text = "";
			comboboxMinimized.SelectedIndex = -1;
		}

		/// <summary>
		/// Apply new windowlayout of specified process
		/// </summary>
		/// <param name="process">Process specified with layoutinfo</param>
		void redrawWindows (ProcessInfo process)
		{
			if (process != null) {
				new WindowLayout().setLayout(process);
			}
		}

		/// <summary>
		/// Clear list of processes, clear boxes of info and collect info on currently running processes
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void btnGetProcesses_Click (object sender, EventArgs e)
		{
			off();
			clearTextBoxes();
			lbProcesses.Items.Clear();
			settings.currentProcessesList = null;
			currentProcess = null;
			populateProcesses();
			on();
		}

		/// <summary>
		/// New layoutinfo have been entered, input this to the processlist
		/// and to the processlist of the session
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void btnSetProcessInfo_Click (object sender, EventArgs e)
		{
			currentProcess.Height = int.Parse(txtHeight.Text);
			currentProcess.Width = int.Parse(txtWidth.Text);
			currentProcess.ProcessName = txtProcess.Text;
			currentProcess.Xcoordinate = int.Parse(txtX.Text);
			currentProcess.Ycoordinate = int.Parse(txtY.Text);
			currentProcess.Minimized = comboboxMinimized.SelectedIndex + 1;

			if (settings.currentSession != null && lbSessions.SelectedIndex != -1) {
				ProcessInfo tempProcess = new ProcessInfo(Process.GetProcessById(currentProcess.ID).MainWindowHandle,
				                                          currentProcess.ID,
				                                          currentProcess.ProcessName,
				                                          currentProcess.MainWindowTitle,
				                                          currentProcess.Xcoordinate,
				                                          currentProcess.Ycoordinate,
				                                          currentProcess.Xcoordinatelow,
				                                          currentProcess.Ycoordinatelow,
				                                          currentProcess.Minimized);
				if (!settings.updateProcess(currentProcess.ProcessName, tempProcess)) {
					settings.addProcessToSession(lbSessions.SelectedItem.ToString(), tempProcess);
				}
			}

			redrawWindows(currentProcess);
		}

		/// <summary>
		/// Open a contextmenu for saving the current session
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void btnSave_Click (object sender, EventArgs e)
		{
			conmenuSave.Show(btnSave, 80, 23);
		}

		/// <summary>
		/// Use specified layoutinformation for all processes
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void btnSetSettings_Click (object sender, EventArgs e)
		{
			foreach (ProcessInfo p in settings.currentProcessesList) {
				redrawWindows(p);
			}
		}

		/// <summary>
		/// Listbox for sessions have been clicked
		/// Load the processes of the session to the listbox
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void lbSessions_SelectedIndexChanged (object sender, EventArgs e)
		{
			if (lbSessions.SelectedIndex != -1) {
				settings.setCurrentSession(lbSessions.SelectedItem.ToString());
				clearTextBoxes();
				lbProcesses.Items.Clear();
				settings.currentProcessesList = null;
				settings.currentProcessesList = settings.currentSession.Plist;
				populateProcesses();
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
		void lbProcesses_SelectedIndexChanged (object sender, EventArgs e)
		{
			if (lbProcesses.SelectedIndex == -1) {
				currentProcess = null;
			} else if (!lbProcesses.SelectedItem.ToString().StartsWith("   ")) {
				ttInfo.Active = false;
				currentProcess = settings.currentProcessesList.Find(x => x.ProcessName.Equals(lbProcesses.SelectedItem.ToString()));
				off();

				if (currentProcess.ID == 0) {
					txtProcess.Text = currentProcess.ProcessName;
					txtId.Text = "No process for " + currentProcess.ProcessName + " is running";
					txtX.ReadOnly = true;
					txtY.ReadOnly = true;
					txtHeight.ReadOnly = true;
					txtWidth.ReadOnly = true;
					comboboxMinimized.Enabled = false;
				} else {
					txtX.Text = currentProcess.Xcoordinate.ToString();
					txtX.ReadOnly = false;
					txtY.Text = currentProcess.Ycoordinate.ToString();
					txtY.ReadOnly = false;
					txtHeight.Text = currentProcess.Height.ToString();
					txtHeight.ReadOnly = false;
					txtWidth.Text = currentProcess.Width.ToString();
					txtWidth.ReadOnly = false;
					txtProcess.Text = currentProcess.ProcessName;
					txtId.Text = currentProcess.ID.ToString();
					switch (currentProcess.Minimized) {
						case 3:
							comboboxMinimized.SelectedIndex = 2;
							break;
						case 2:
							comboboxMinimized.SelectedIndex = 1;
							break;
						default:
							comboboxMinimized.SelectedIndex = 0;
							break;
					}
					comboboxMinimized.Enabled = true;
				}
				on();
			}
		}

		/// <summary>
		/// Used to check if a click in the listbox occured over a listbox item
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic MouseEventArgs</param>
		void lbProcesses_MouseDown (object sender, MouseEventArgs e)
		{
			int index = lbProcesses.IndexFromPoint(e.Location);

			if (index == -1) {
				lbProcesses.SelectedIndex = -1;
				off();
				clearTextBoxes();
				currentProcess = null;
			}
			if (e.Button == MouseButtons.Right) {
				lbProcesses.SelectedIndex = index;
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
		void txtX_TextChanged (object sender, EventArgs e)
		{
			if (currentProcess != null) {
				if (txtX.Text.Length == 0)
					txtX.Text = "0";
				if (int.Parse(txtX.Text) > SystemInformation.VirtualScreen.Width)
					txtX.Text = string.Format("{0}", SystemInformation.VirtualScreen.Width - 20);
				currentProcess.Xcoordinate = int.Parse(txtX.Text);
			}
		}

		/// <summary>
		/// Text for window upper most Y-coordinate have changed
		/// Check if the coordinate is within visible range
		/// If to close to the edge, use more visible coordinate 
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void txtY_TextChanged (object sender, EventArgs e)
		{
			if (currentProcess != null) {
				if (txtY.Text.Length == 0)
					txtY.Text = "0";
				if (int.Parse(txtY.Text) > SystemInformation.VirtualScreen.Height)
					txtY.Text = string.Format("{0}", SystemInformation.VirtualScreen.Height - 20);
				currentProcess.Ycoordinate = int.Parse(txtY.Text);
			}
		}

		/// <summary>
		/// Text for window width have changed
		/// If no width is entered, use previous width
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void txtWidth_TextChanged (object sender, EventArgs e)
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
		void txtHeight_TextChanged (object sender, EventArgs e)
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
		/// User wants to save the session as new
		/// Open dialog for specifying sessionname
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void contextmenuSaveNewSession_Click (object sender, EventArgs e)
		{
			NewSessionName newsession = new NewSessionName();

			newsession.ShowDialog();
			if (newsession.DialogResult == DialogResult.OK) {
				settings.addSession(new Session(newsession.getName(), settings.currentProcessesList));

				lbSessions.Items.Add(newsession.getName());
				comboboxUndocked.Items.Add(newsession.getName());
				comboboxDocked.Items.Add(newsession.getName());
				settings.currentSession = settings.getSession(newsession.getName());
				lbSessions.SelectedIndex = lbSessions.FindString(newsession.getName());
			}
			new FileHandler().write(settings);
		}

		/// <summary>
		/// User wants to save the current session
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void contextmenuSaveMarkedSession_Click (object sender, EventArgs e)
		{
			settings.updateSession();
			new FileHandler().write(settings);
		}

		void contextmenuDeleteProcess_Click (object sender, EventArgs e)
		{
			settings.deleteProcessFromSession(settings.currentSession.SessionName, currentProcess);
			settings.currentSession = settings.getSession(settings.currentSession.SessionName);
			settings.currentProcessesList = settings.currentSession.Plist;
			populateProcesses();
			new FileHandler().write(settings);
		}

		/// <summary>
		/// The setting for how the window is show, have changed
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void comboboxMinimized_SelectedIndexChanged (object sender, EventArgs e)
		{
			currentProcess.Minimized = comboboxMinimized.SelectedIndex;
			settings.updateProcess(currentProcess.ProcessName, currentProcess);
			new FileHandler().write(settings);
		}

		/// <summary>
		/// Rolldown the list when clicked
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void comboboxMinimized_Click (object sender, EventArgs e)
		{
			comboboxMinimized.DroppedDown = true;
		}

		/// <summary>
		/// User have selected which session is to be used when the computer is docked (have more than one screen connected)
		/// Remove that session from the other combobox to avoid it being used
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void comboboxDocked_SelectedIndexChanged (object sender, EventArgs e)
		{
			settings.Docked = comboboxDocked.SelectedItem.ToString();
			new FileHandler().write(settings);
		}

		/// <summary>
		/// User have selected which session is to be used when the computer is not docked (only one screen connected)
		/// Remove that session from the other combobox to avoid it being used
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void comboboxUndocked_SelectedIndexChanged (object sender, EventArgs e)
		{
			settings.Undocked = comboboxUndocked.SelectedItem.ToString();
			new FileHandler().write(settings);
		}

		/// <summary>
		/// Tooltip shown for textbox for coordinates when mouse is hovering
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void txtCoordinate_MouseHover (object sender, EventArgs e)
		{
			ttInfo.Active = true;
			ttInfo.Show("Coordinate of the upper left cornet", (TextBox)sender, 0, 18);
		}

		/// <summary>
		/// Show info about button when mouse is hovering
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void btnGetProcesses_MouseHover (object sender, EventArgs e)
		{
			ttInfo.Active = true;
			ttInfo.Show("List processes with open, visible windows.", (Button)sender, 0, 18);
		}

		/// <summary>
		/// Show info about button when mouse is hovering
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void btnSave_MouseHover (object sender, EventArgs e)
		{
			ttInfo.Active = true;
			ttInfo.Show("Save the current settings for visible windows.\nAs new or as the currently marked session.", (Button)sender, 0, 18);
		}

		/// <summary>
		/// Check if the list of sessions is empty. If so, disable menuitem for "Marked" 
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic CancelEventArgs</param>
		void contextmenuSave_Opening (object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (lbSessions.Items.Count == 0)
				conmenuSave.Items[1].Enabled = false;
		}

		/// <summary>
		/// User wants the application to start minimized to the notificationarea
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void checkboxStart_CheckedChanged (object sender, EventArgs e)
		{
			settings.StartInSysTray = checkboxStart.Checked;
			new FileHandler().write(settings);
		}
	}
}
