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

		/// <summary>
		/// Entrypoint of mainform, called from systrayicon
		/// Fill listbox of sessions from data in the XML-file
		/// </summary>
		/// <param name="settings">All settings read from XML-file</param>
		public MainForm(ref Settings s)
		{
			InitializeComponent();
			this.settings = s;
			lvProcesses.SelectedIndexChanged -= lvProcesses_SelectedIndexChanged;
			checkboxStart.Checked = settings.StartInSysTray;
			populateSessions();
			populateProcesses();
			lvProcesses.SelectedIndexChanged += lvProcesses_SelectedIndexChanged;
			Text = "Session window version " + System.Reflection.Assembly.GetExecutingAssembly()
				.GetName().Version;
			btnSaveSession.Enabled = false;
		}

		/// <summary>
		/// Update current processlist with windowinfo from currently running processes
		/// </summary>
		public void populateProcesses()
		{
			if (settings.sessionProcessList == null) {
				activeProcesses = Process.GetProcesses()
					.Where(p => p.MainWindowHandle != IntPtr.Zero &&
						p.ProcessName != "iexplore" &&
						p.Id != Process.GetCurrentProcess().Id)
					.OrderBy(p => p.ProcessName);
				settings.sessionProcessList = new List<ProcessInfo>();
				foreach (Process p in activeProcesses) {
					IntPtr handle = p.MainWindowHandle;
					WINDOWPLACEMENT placement = new WINDOWPLACEMENT();
					placement.length = Marshal.SizeOf(placement);
					RECT Rect = new RECT();
					if (GetWindowRect(handle, ref Rect)) {
						GetWindowPlacement(p.MainWindowHandle, ref placement);
						IntPtr h = p.MainWindowHandle;
						string pn = p.ProcessName;
						string mwt = p.MainWindowTitle.Equals("") ? "Process: " + p.ProcessName : p.MainWindowTitle;
						int i = p.Id;
						int l = Rect.left;
						int t = Rect.top;
						int r = Rect.right;
						int b = Rect.bottom;
						int pl = (int)placement.showCmd;
						settings.sessionProcessList.Add(new ProcessInfo(h, i, pn, mwt, l, t, (r - l), (b - t), pl));
						string[] item = new string[2];
						item[0] = i.ToString();
						item[1] = p.ProcessName;
						lvProcesses.Items.Add(new ListViewItem(item));
					}
				}
			} else {
				foreach (ProcessInfo p in settings.sessionProcessList) {
					string[] li = new string [2];
					li[0] = p.ID.ToString();
					li[1] = p.ProcessName;
					lvProcesses.Items.Add(new ListViewItem(li));
				}
			}
		}

		/// <summary>
		/// Fill the listbox for sessions
		/// </summary>
		void populateSessions()
		{
			string[] sessionList = settings.getSessionList();
			if (sessionList != null) {
				eventsOff();
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
				eventsOn();
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
			comboboxMinimized.SelectedIndexChanged += comboboxMinimized_SelectedIndexChanged;
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
			comboboxMinimized.SelectedIndexChanged -= comboboxMinimized_SelectedIndexChanged;
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
			comboboxMinimized.SelectedIndex = -1;
			btnSetProcessInfo.Enabled = false;
		}

		/// <summary>
		/// Eventhandler for activewindows menu
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		/// <param name="i">ProcessID of window to show info about</param>
		static void awEH(object sender, EventArgs e, int i)
		{
			WindowInfo wi = new WindowInfo(i);
			wi.Show();
		}

		/// <summary>
		/// Clear list of processes, clear boxes of info and collect info on currently running processes
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void btnGetProcesses_Click(object sender, EventArgs e)
		{
			eventsOff();
			clearTextBoxes();
			lvProcesses.Items.Clear();
			settings.sessionProcessList = null;
			currentProcess = null;
			populateProcesses();
			btnCreateNew.Enabled = true;
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
		/// New layoutinfo have been entered, input this to the processlist
		/// and to the processlist of the session
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void btnSetProcessInfo_Click(object sender, EventArgs e)
		{
			currentProcess.Height = int.Parse(txtHeight.Text);
			currentProcess.Width = int.Parse(txtWidth.Text);
			currentProcess.ProcessName = txtProcess.Text;
			currentProcess.Xcoordinate = int.Parse(txtX.Text);
			currentProcess.Ycoordinate = int.Parse(txtY.Text);
			currentProcess.Minimized = comboboxMinimized.SelectedIndex + 1;

			if (settings.savedSession != null && lbSessions.SelectedIndex != -1) {
				ProcessInfo tempProcess = new ProcessInfo(Process.GetProcessById(currentProcess.ID).MainWindowHandle,
					                          currentProcess.ID,
					                          currentProcess.ProcessName,
					                          currentProcess.MainWindowTitle,
					                          currentProcess.Xcoordinate,
					                          currentProcess.Ycoordinate,
					                          currentProcess.Width,
					                          currentProcess.Height,
					                          currentProcess.Minimized);
				if (!settings.updateProcess(currentProcess.ProcessName, tempProcess)) {
					settings.addProcessToSession(lbSessions.SelectedItem.ToString(), tempProcess);
				}
			}

			btnSaveSession.Enabled = true;
			if (currentProcess.ID != 0)
				new WindowLayout().setLayout(currentProcess);
		}

		/// <summary>
		/// Open a contextmenu for saving the current session
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void btnSaveSession_Click(object sender, EventArgs e)
		{
			if (settings.savedSession != null)
				settings.updateSession();
			new FileHandler().write(settings);
		}

		/// <summary>
		/// Creates a new session
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void btnCreateNew_Click(object sender, EventArgs e)
		{
			NewSessionName newsession = new NewSessionName();

			newsession.ShowDialog();
			if (newsession.DialogResult == DialogResult.OK) {
				settings.addSession(new Session(newsession.getName(), settings.sessionProcessList));

				lbSessions.Items.Add(newsession.getName());
				comboboxUndocked.Items.Add(newsession.getName());
				comboboxDocked.Items.Add(newsession.getName());
				settings.savedSession = settings.getSession(newsession.getName());
				lbSessions.SelectedIndex = lbSessions.FindString(newsession.getName());
			}
			new FileHandler().write(settings);
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
			if (lvProcesses.SelectedItems.Count == 0) {
				currentProcess = null;
			} else {
				ttInfo.Active = false;
				string selected = lvProcesses.SelectedItems[0].SubItems[1].Text;
				currentProcess = settings.sessionProcessList.Find(x => x.ProcessName.Equals(selected));
				eventsOff();

				if (currentProcess.ID == 0) {
					txtProcess.Text = currentProcess.ProcessName;
					txtId.Text = "No process for " + currentProcess.ProcessName + " is running";
				} else {
					txtX.Text = currentProcess.Xcoordinate.ToString();
					txtY.Text = currentProcess.Ycoordinate.ToString();
					txtHeight.Text = currentProcess.Height.ToString();
					txtWidth.Text = currentProcess.Width.ToString();
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
					btnSetProcessInfo.Enabled = true;
				}
				eventsOn();
			}
		}


		/// <summary>
		/// Listbox for sessions have been clicked
		/// Load the processes of the session to the listbox
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic MouseEventArgs</param>
		void lbSessions_MouseDown(object sender, MouseEventArgs e)
		{
			int index = lbSessions.IndexFromPoint(e.Location);
			eventsOff();
			lvProcesses.Items.Clear();
			btnCreateNew.Enabled = false;
			clearTextBoxes();
			if (index != -1) {
				var t = lbSessions.SelectedItem.ToString();
				settings.setSavedSession(t);
				settings.sessionProcessList = null;
				settings.sessionProcessList = settings.savedSession.Plist;
				currentProcess = null;
				populateProcesses();
				btnCreateNew.Enabled = true;
			}
			eventsOn();
		}

		/// <summary>
		/// Used to check if a click in the listview occured over an item
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
				//lvProcesses.SelectedIndices = item;
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
		void txtY_TextChanged(object sender, EventArgs e)
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
		/// Removes a process from lbProcesses and from the current session
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void contextmenuDeleteProcess_Click(object sender, EventArgs e)
		{
			settings.deleteProcessFromSession(settings.savedSession.SessionName, currentProcess);
			settings.savedSession = settings.getSession(settings.savedSession.SessionName);
			settings.sessionProcessList = settings.savedSession.Plist;
			eventsOff();
			clearTextBoxes();
			populateProcesses();
			eventsOn();
			new FileHandler().write(settings);
		}

		/// <summary>
		/// The setting for how the window is show, have changed
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void comboboxMinimized_SelectedIndexChanged(object sender, EventArgs e)
		{
			int temp = comboboxMinimized.SelectedIndex;
			currentProcess.Minimized = comboboxMinimized.SelectedIndex + 1;
			settings.updateProcess(currentProcess.ProcessName, currentProcess);
			if (settings.savedSession != null)
				new FileHandler().write(settings);
		}

		/// <summary>
		/// Rolldown the list when clicked
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void comboboxMinimized_Click(object sender, EventArgs e)
		{
			comboboxMinimized.DroppedDown = true;
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
			btnSaveSession.Enabled = true;
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
			btnSaveSession.Enabled = true;
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
					conmsActiveWindows.Items.Add(mwt + "(" + i + ")", null, (s, e) => awEH(s, e, i));
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
	}
}
