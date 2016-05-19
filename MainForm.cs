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
		static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int Width, int Height, bool Repaint);

		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);
		
		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool SetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

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

		List<ProcessInfo> pList;
		List<Session> sList;
		IEnumerable<Process> processes;
		int currentSessionIndex, currentProcessIndex;
		ProcessInfo currentProcess;
		FileHandler fh;
		string[] dockedSession;

		/// <summary>
		/// Entrypoint of mainform, called from systrayicon
		/// Fill listbox of sessions from data in the XML-file
		/// </summary>
		/// <param name="l">List of processessions</param>
		public MainForm(List<Session> l)
		{
			InitializeComponent();

			lbProcesses.SelectedIndexChanged -= lbProcesses_SelectedIndexChanged;
			fh = new FileHandler();
			sList = l;
			dockedSession = fh.getDockedSessions();
			populateSessions();
			lbProcesses.SelectedIndexChanged += lbProcesses_SelectedIndexChanged;
		}

		/// <summary>
		/// Take data from current processlist and fill the listbox
		/// If processlist is empty (pList), create list from currently running processes
		/// </summary>
		public void populateProcesses()
		{
			if (pList == null) {
				processes = Process.GetProcesses().Where(p => p.MainWindowHandle != IntPtr.Zero &&
				p.ProcessName != "iexplore" &&
				p.Id != Process.GetCurrentProcess().Id);
				pList = new List<ProcessInfo>();
				foreach (Process p in processes) {
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
						pList.Add(new ProcessInfo(h, i, pn, mwt, l, t, r, b, pl));
						lbProcesses.Items.Add(p.ProcessName);
					}
				}
			} else {
				foreach (ProcessInfo item in pList) {
					lbProcesses.Items.Add(item.ProcessName);
				}
			}
		}

		/// <summary>
		/// Fill the listbox for sessions from data in XML-file
		/// </summary>
		void populateSessions()
		{
			if (sList != null) {
				off();
				foreach (Session s in sList) {
					lbSessions.Items.Add(s.SessionName);
				}
				populateDockedLists();

				if (dockedSession != null) {
					if (dockedSession[0].Equals(dockedSession[1])) {
						cbUndocked.SelectedIndex = cbDocked.SelectedIndex = cbDocked.FindStringExact(dockedSession[0]);
					} else if (dockedSession.Equals("")) {
						cbUndocked.SelectedIndex = cbDocked.SelectedIndex = -1;
					} else {
						cbUndocked.SelectedIndex = cbUndocked.FindStringExact(dockedSession[1]);
						cbDocked.SelectedIndex = cbDocked.FindStringExact(dockedSession[0]);
					}
				}
				on();
			}
		}

		void populateDockedLists()
		{
			if (sList != null) {
				cbUndocked.Items.Clear();
				cbDocked.Items.Clear();
				foreach (Session s in sList) {
					cbDocked.Items.Add(s.SessionName);
					cbUndocked.Items.Add(s.SessionName);
				}
			}
		}

		/// <summary>
		/// Turn on the eventhandlers to enable messages being shown 
		/// </summary>
		void on()
		{
			txtX.TextChanged += txtX_TextChanged;
			txtY.TextChanged += txtY_TextChanged;
			txtWidth.TextChanged += txtWidth_TextChanged;
			txtHeight.TextChanged += txtHeight_TextChanged;
			cbMinimized.SelectedIndexChanged += cbMinimized_SelectedIndexChanged;
			cbDocked.SelectedIndexChanged += cbDocked_SelectedIndexChanged;
			cbUndocked.SelectedIndexChanged += cbUndocked_SelectedIndexChanged;
		}

		/// <summary>
		/// Turn off eventhandlers to avoid messages from being shown unnecessarily
		/// </summary>
		void off()
		{
			txtX.TextChanged -= txtX_TextChanged;
			txtY.TextChanged -= txtY_TextChanged;
			txtWidth.TextChanged -= txtWidth_TextChanged;
			txtHeight.TextChanged -= txtHeight_TextChanged;
			cbMinimized.SelectedIndexChanged -= cbMinimized_SelectedIndexChanged;
			cbDocked.SelectedIndexChanged -= cbDocked_SelectedIndexChanged;
			cbUndocked.SelectedIndexChanged -= cbUndocked_SelectedIndexChanged;
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
			cbMinimized.SelectedIndex = -1;
		}

		/// <summary>
		/// Apply new windowlayout of specified process
		/// </summary>
		/// <param name="process">Process specified with layoutinfo</param>
		void redrawWindows(ProcessInfo process)
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
		void btnGetProcesses_Click(object sender, EventArgs e)
		{
			off();
			clearTextBoxes();
			lbProcesses.Items.Clear();
			pList = null;
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
		void btnSetProcessInfo_Click(object sender, EventArgs e)
		{
			int processIndex, sessionIndex;
			currentProcess.Height = int.Parse(txtHeight.Text);
			currentProcess.Width = int.Parse(txtWidth.Text);
			currentProcess.ProcessName = txtProcess.Text;
			currentProcess.Xcoordinate = int.Parse(txtX.Text);
			currentProcess.Ycoordinate = int.Parse(txtY.Text);
			currentProcess.Minimized = cbMinimized.SelectedIndex + 1;

			if (sList != null && lbSessions.SelectedIndex != -1) {
				sessionIndex = sList.FindIndex(x => x.SessionName.Equals(lbSessions.SelectedItem));
				processIndex = sList[sessionIndex].Plist.FindIndex(x => x.ID == int.Parse(txtId.Text));
				if (processIndex != -1) {
					sList[sessionIndex].Plist[processIndex].Height = currentProcess.Height;
					sList[sessionIndex].Plist[processIndex].Width = currentProcess.Width;
					sList[sessionIndex].Plist[processIndex].ProcessName = currentProcess.ProcessName;
					sList[sessionIndex].Plist[processIndex].Xcoordinate = currentProcess.Xcoordinate;
					sList[sessionIndex].Plist[processIndex].Ycoordinate = currentProcess.Ycoordinate;
					sList[sessionIndex].Plist[processIndex].Minimized = currentProcess.Minimized;
				} else {
					ProcessInfo pInfo = new ProcessInfo(Process.GetProcessById(currentProcess.ID).MainWindowHandle,
						                    currentProcess.ID,
						                    currentProcess.ProcessName,
						                    currentProcess.MainWindowTitle,
						                    currentProcess.Xcoordinate,
						                    currentProcess.Ycoordinate,
						                    currentProcess.Xcoordinatelow,
						                    currentProcess.Ycoordinatelow,
						                    currentProcess.Minimized);
					sList[sessionIndex].Plist.Add(pInfo);
				}
			}
			processIndex = pList.FindIndex(x => x.ID == currentProcess.ID);
			pList[processIndex].Height = currentProcess.Height;
			pList[processIndex].Width = currentProcess.Width;
			pList[processIndex].ProcessName = currentProcess.ProcessName;
			pList[processIndex].Xcoordinate = currentProcess.Xcoordinate;
			pList[processIndex].Minimized = currentProcess.Minimized;
			pList[processIndex].Ycoordinate = currentProcess.Ycoordinate;

			redrawWindows(currentProcess);
		}

		/// <summary>
		/// Open a contextmenu for saving the current session
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void btnSave_Click(object sender, EventArgs e)
		{
			conmenuSave.Show(btnSave, 80, 23);
		}

		/// <summary>
		/// Use specified layoutinformation for all processes
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void btnSetSettings_Click(object sender, EventArgs e)
		{
			foreach (ProcessInfo p in pList) {
				redrawWindows(p);
			}
		}

		/// <summary>
		/// Listbox for sessions have been clicked
		/// Load the processes of the session to the listbox
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void lbSessions_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (lbSessions.SelectedIndex != -1) {
				currentSessionIndex = sList.FindIndex(x => x.SessionName.Equals(lbSessions.SelectedItem.ToString()));
				currentProcessIndex = -1;
				clearTextBoxes();
				lbProcesses.Items.Clear();
				currentProcess = null;

				pList = new Session(sList.Find(x => x.SessionName.Equals(lbSessions.SelectedItem.ToString()))).Plist;
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
		void lbProcesses_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (lbProcesses.SelectedIndex == -1) {
				currentProcess = null;
			} else if (!lbProcesses.SelectedItem.ToString().StartsWith("   ")) {
				ttInfo.Active = false;
				currentProcess = pList.Find(x => x.ProcessName.Equals(lbProcesses.SelectedItem.ToString()));
				currentProcessIndex = pList.FindIndex(x => x.ProcessName.Equals(lbProcesses.SelectedItem.ToString()));
				off();
				if (currentProcess.ID == 0) {
					txtProcess.Text = currentProcess.ProcessName;
					txtId.Text = "No process for " + currentProcess.ProcessName + " is running";
					txtX.ReadOnly = true;
					txtY.ReadOnly = true;
					txtHeight.ReadOnly = true;
					txtWidth.ReadOnly = true;
					cbMinimized.Enabled = false;
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
							cbMinimized.SelectedIndex = 2;
							break;
						case 2:
							cbMinimized.SelectedIndex = 1;
							break;
						default:
							cbMinimized.SelectedIndex = 0;
							break;
					}
					cbMinimized.Enabled = true;
				}
				on();
			}
		}

		/// <summary>
		/// Used to check if a click in the listbox occured over a listbox item
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic MouseEventArgs</param>
		void lbProcesses_MouseDown(object sender, MouseEventArgs e)
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
		/// User wants to save the session as new
		/// Open dialog for specifying sessionname
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void conmenuNew_Click(object sender, EventArgs e)
		{
			SessionName sn = new SessionName();

			sn.ShowDialog();
			if (sn.DialogResult == DialogResult.OK) {
				if (sList == null)
					sList = new List<Session>();
				sList.Add(new Session(sn.getName(), pList));
				lbSessions.Items.Add(sn.getName());
				cbUndocked.Items.Add(sn.getName());
				cbDocked.Items.Add(sn.getName());
			}
			fh.write(sList, dockedSession);
		}

		/// <summary>
		/// User wants to save the current session
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void conmenuMarked_Click(object sender, EventArgs e)
		{
			Session sessionName;
			SelectSession userSpecifiedSession = new SelectSession(sList);
			
			if (lbSessions.SelectedItem.ToString().Equals("")) {
				userSpecifiedSession.ShowDialog();
				sessionName = userSpecifiedSession.getSession();
			} else
				sessionName = sList.Find(x => x.SessionName.Equals(lbSessions.SelectedItem.ToString()));
			int i = sList.IndexOf(sessionName);
			sList[i].Plist = pList;
			fh.write(sList, dockedSession);
		}

		void conmenuDelete_Click(object sender, EventArgs e)
		{
			int processIndex = pList.FindIndex(x => x.ID == int.Parse(txtId.Text));

			pList.RemoveAt(processIndex);
			processIndex = sList[currentSessionIndex].Plist.FindIndex(x => x.ID == int.Parse(txtId.Text));
			sList[currentSessionIndex].Plist.RemoveAt(processIndex);
			populateSessions();
		}

		/// <summary>
		/// The setting for how the window is show, have changed
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void cbMinimized_SelectedIndexChanged(object sender, EventArgs e)
		{
			currentProcess.Minimized = cbMinimized.SelectedIndex;
		}

		/// <summary>
		/// Rolldown the list when clicked
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void cbMinimized_Click(object sender, EventArgs e)
		{
			cbMinimized.DroppedDown = true;
		}

		/// <summary>
		/// User have selected which session is to be used when the computer is docked (have more than one screen connected)
		/// Remove that session from the other combobox to avoid it being used
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void cbDocked_SelectedIndexChanged(object sender, EventArgs e)
		{
			dockedSession[0] = cbDocked.SelectedItem.ToString();
			fh.write(sList, dockedSession);
		}

		/// <summary>
		/// User have selected which session is to be used when the computer is not docked (only one screen connected)
		/// Remove that session from the other combobox to avoid it being used
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void cbUndocked_SelectedIndexChanged(object sender, EventArgs e)
		{
			dockedSession[1] = cbUndocked.SelectedItem.ToString();
			fh.write(sList, dockedSession);
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
		/// Show info about button when mouse is hovering
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void btnSave_MouseHover(object sender, EventArgs e)
		{
			ttInfo.Active = true;
			ttInfo.Show("Save the current settings for visible windows.\nAs new or as the currently marked session.", (Button)sender, 0, 18);
		}

		/// <summary>
		/// Check if the list of sessions is empty. If so, disable menuitem for "Marked" 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void conmenuSave_Opening(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (lbSessions.Items.Count == 0)
				conmenuSave.Items[1].Enabled = false;
		}
	}
}
