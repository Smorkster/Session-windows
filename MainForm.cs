using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Session_windows
{
	public partial class MainForm : Form
	{
		/// <summary>
		/// A list of processes that is currently running, that has an open window
		/// </summary>
		List<ProcessInfo> runningProcesses;

		/// <summary>
		/// Contains the currently marked process in the list
		/// </summary>
		ProcessInfo markedProcess;

		/// <summary>
		/// Object containing all settings and sessions
		/// </summary>
		Settings settings;

		/// <summary>
		/// Fill listbox of sessions from data in the XML-file
		/// </summary>
		/// <param name="s">All settings read from XML-file</param>
		internal MainForm(ref Settings s)
		{
			InitializeComponent();

			EventsOff();
			settings = s;
			checkboxStart.Checked = settings.StartInSysTray;
			PopulateSessionsList();
			PopulateDockedLists();
			settings.ActiveSession_Changed += new Settings.ActiveSessionHandler(LoadSession);
			EventsOn();

			if (settings.ActiveSession == null)
				settings.ActiveSession = "current";
			else
				LoadSession(settings.ActiveSession);

			Text = $"Session window version {System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}";
			if (settings.Test)
				Text = Text + " [In testmode]";
		}

		/// <summary>
		/// Form is closing, check if form should minimize or application close 
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic FormClosingEventArgs</param>
		void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (!settings.AllowClosing)
			{
				e.Cancel = true;
				Hide();
			}
		}

		/// <summary>
		/// When the mainform is minimized, hide the form
		/// When the mainform is brought up, enable eventhandler for ActiveSession
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void MainForm_Resize(object sender, EventArgs e)
		{
			switch (WindowState)
			{
				case FormWindowState.Minimized:
					settings.ActiveSession_Changed -= new Settings.ActiveSessionHandler(LoadSession);
					Hide();
					break;
				default:
					settings.ActiveSession_Changed += new Settings.ActiveSessionHandler(LoadSession);
					break;
			}
		}

		/// <summary>
		/// Eventhandler for activewindows-menu
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		/// <param name="i">ProcessID of window to show info about</param>
		static void ActiveWindow_EventHandler(object sender, EventArgs e, int processId)
		{
			WindowInfo wi = new WindowInfo(processId);
			wi.Show();
		}

		/// <summary>
		/// Add the chosen process to current session
		/// </summary>
		/// <param name="pInfo">ProcessInfo to be added</param>
		void AddProcess(ProcessInfo pInfo)
		{
			settings.GetSession(settings.ActiveSession).AddProcessToSession(pInfo);
			btnUpdateSettings.Enabled = true;
			PopulateProcessList(settings.ActiveSession, "", pInfo);
		}

		/// <summary>
		/// Remove information of process
		/// Called after click in listbox where no listboxitem is located
		/// </summary>
		void ClearTextBoxes()
		{
			txtHeight.Text = txtWidth.Text = txtX.Text = txtY.Text = txtProcess.Text = txtId.Text = txtMainWindowHandle.Text = "";
			txtId.Font = txtMainWindowHandle.Font = new Font(Font, FontStyle.Regular);
			txtId.BackColor = txtMainWindowHandle.BackColor = DefaultBackColor;
			comboboxWindowPlacement.SelectedIndex = -1;
			btnSetProcessInfoLayout.Enabled = false;
			gbWindowSettings.Enabled = false;
		}

		/// <summary>
		/// A process is to be deleted from the session
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void DeleteProcess(object sender, EventArgs e)
		{
			if (settings.ActiveSession.Equals("current"))
			{
				settings.currentlyRunningProcesses.DeleteProcessFromSession(markedProcess);
			}
			else
			{
				settings.GetSession(settings.ActiveSession).DeleteProcessFromSession(markedProcess);
				btnUpdateSettings.Enabled = true;
			}
			EventsOff();
			ClearTextBoxes();
			PopulateProcessList(settings.ActiveSession, markedProcess.ProcessName, null);
			EventsOn();
		}

		/// <summary>
		/// Turn off eventhandlers to avoid messages from being shown unnecessarily
		/// </summary>
		void EventsOff()
		{
			lvProcesses.SelectedIndexChanged -= LvProcesses_SelectedIndexChanged;
			txtX.TextChanged -= TxtNumber_TextChanged;
			txtY.TextChanged -= TxtNumber_TextChanged;
			txtWidth.TextChanged -= TxtNumber_TextChanged;
			txtHeight.TextChanged -= TxtNumber_TextChanged;
			checkboxStart.CheckedChanged -= CheckboxStart_CheckedChanged;
			checkboxSessionVisibleInTaskbar.CheckedChanged -= CheckboxTaskbar_CheckedChanged;
			comboboxWindowPlacement.SelectedIndexChanged -= ComboboxWindowPlacement_SelectedIndexChanged;
			comboboxDocked.SelectedIndexChanged -= ComboboxDocked_SelectedIndexChanged;
			comboboxUndocked.SelectedIndexChanged -= ComboboxUndocked_SelectedIndexChanged;
		}

		/// <summary>
		/// Turn on the eventhandlers to enable messages being shown 
		/// </summary>
		void EventsOn()
		{
			lvProcesses.SelectedIndexChanged += LvProcesses_SelectedIndexChanged;
			txtX.TextChanged += TxtNumber_TextChanged;
			txtY.TextChanged += TxtNumber_TextChanged;
			txtWidth.TextChanged += TxtNumber_TextChanged;
			txtHeight.TextChanged += TxtNumber_TextChanged;
			checkboxStart.CheckedChanged += CheckboxStart_CheckedChanged;
			checkboxSessionVisibleInTaskbar.CheckedChanged += CheckboxTaskbar_CheckedChanged;
			comboboxWindowPlacement.SelectedIndexChanged += ComboboxWindowPlacement_SelectedIndexChanged;
			comboboxDocked.SelectedIndexChanged += ComboboxDocked_SelectedIndexChanged;
			comboboxUndocked.SelectedIndexChanged += ComboboxUndocked_SelectedIndexChanged;
		}

		/// <summary>
		/// A new application have been select to be excluded from processing
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void ExcludeApplication(object sender, EventArgs e)
		{
			settings.UpdateExcludedApplications(markedProcess.ProcessName);
			PopulateProcessList(settings.ActiveSession, "", null);
			settings.SaveToFile();
		}

		/// <summary>
		/// Get all currently running processes
		/// Filter out explorer and processes without an open window and return list
		/// </summary>
		/// <returns>List of processes with open window</returns>
		List<ProcessInfo> GetListOfRunningProcesses()
		{
			List<ProcessInfo> pInfoList = new List<ProcessInfo>();

			foreach (Process process in Process.GetProcesses()
					.Where(x => x.MainWindowHandle != IntPtr.Zero &&
						x.ProcessName != "iexplore" &&
						x.Id != Process.GetCurrentProcess().Id)
					.OrderBy(p => p.ProcessName))
			{
				pInfoList.Add(new ProcessInfo(process));
			}
			return pInfoList;
		}

		/// <summary>
		/// ActiveSession have changed
		/// </summary>
		internal void LoadSession()
		{
			LoadSession(settings.ActiveSession);
		}

		/// <summary>
		/// Load a saved session from settings
		/// If sessionname is 'current' load running processes instead
		/// </summary>
		/// <param name="sessionName">Name of session to load</param>
		void LoadSession(string sessionName)
		{
			EventsOff();
			ClearTextBoxes();
			if (sessionName.Equals("current"))
			{
				gbSessionSettings.Text = "No session loaded";
				gbSessionSettings.Enabled = false;
				checkboxSessionVisibleInTaskbar.Checked = false;
				btnAddRunningProcess.Enabled = false;
				btnDeleteLoadedSession.Enabled = false;
				btnDeleteLoadedSession.Text = "Delete session ...";
			}
			else
			{
				gbSessionSettings.Text = "Settings for session '" + settings.ActiveSession + "'";
				gbSessionSettings.Enabled = true;
				checkboxSessionVisibleInTaskbar.Checked = settings.GetSession(settings.ActiveSession).TaskbarVisible;
				btnAddRunningProcess.Enabled = true;
				btnDeleteLoadedSession.Enabled = true;
				btnDeleteLoadedSession.Text = "Delete session '" + sessionName + "'";
			}
			markedProcess = null;
			btnUpdateSettings.Enabled = false;
			PopulateProcessList(settings.ActiveSession, "", null);
			EventsOn();
		}

		/// <summary>
		/// Fills the dropdownlist for choosing what session to be used when docked/undocked
		/// </summary>
		void PopulateDockedLists()
		{
			comboboxDocked.Items.Clear();
			comboboxUndocked.Items.Clear();

			foreach (Session session in settings.GetSessionList())
			{
				comboboxDocked.Items.Add(session.SessionName);
				comboboxUndocked.Items.Add(session.SessionName);
			}

			// Checks which session is selected for docked and undocked, then select that in the comboboxes.
			if (settings.DockedSession == null || settings.DockedSession.Equals(""))
			{ comboboxDocked.SelectedIndex = -1; }
			else
			{
				comboboxDocked.SelectedItem = settings.DockedSession;
			}

			if (settings.UndockedSession == null || settings.UndockedSession.Equals(""))
			{ comboboxUndocked.SelectedIndex = -1; }
			else
			{
				comboboxUndocked.SelectedItem = settings.UndockedSession;
			}
		}

		/// <summary>
		/// Fill processlist with windowinfo from currently running processes or loaded session
		/// </summary>
		void PopulateProcessList(string sessionName, string excludedProcess, ProcessInfo pInfoToSelect)
		{
			lvProcesses.Items.Clear();
			List<ListViewItem> processListForListView = new List<ListViewItem>();
			ListViewItem lvi;

			if (sessionName.Equals("current"))
			{// Load information from the currently running processes
				groupBoxProcesses.Text = "Currently running processes";
				if (settings.currentlyRunningProcesses.Plist.Any())
					settings.currentlyRunningProcesses.Plist.Clear();
				bool explorerAdded = false;

				runningProcesses = GetListOfRunningProcesses();
				foreach (ProcessInfo pInfo in runningProcesses)
				{
					if (!pInfo.ProcessName.Equals(excludedProcess) && !settings.IsExcludedApp(pInfo.ProcessName))
					{
						if (pInfo.ProcessName.Equals("explorer"))
						{
							if (explorerAdded)
							{
								continue;
							}
							else
							{
								explorerAdded = true;
							}
						}

						settings.currentlyRunningProcesses.Plist.Add(pInfo);
						lvi = new ListViewItem(new[] { pInfo.ProcessID.ToString(), pInfo.ProcessName });
						processListForListView.Add(lvi);
					}
				}
			}
			else
			{// Load processes from saved session
				groupBoxProcesses.Text = "Session: " + settings.ActiveSession;
				Session session = settings.GetSession(settings.ActiveSession);
				IEnumerable<Process> runningProcesses = Process.GetProcesses().Where(x => x.MainWindowHandle != IntPtr.Zero);

				foreach (ProcessInfo pInfo in session.Plist)
				{
					if (!settings.IsExcludedApp(pInfo.ProcessName))
					{
						try
						{
							Process p = runningProcesses.First(x => x.ProcessName.Equals(pInfo.ProcessName));
							lvi = new ListViewItem(new[] {
								p.Id.ToString(),
								p.ProcessName
							});
							if (pInfo.ProcessID == 0)
								pInfo.ProcessID = p.Id;
							if (pInfo.MainWindowHandle == IntPtr.Zero)
								pInfo.MainWindowHandle = p.MainWindowHandle;
						}
						catch
						{
							lvi = new ListViewItem(new[] {
								"(not started)",
								pInfo.ProcessName
							})
							{
								ForeColor = Color.Red,
								Font = new Font(Font, FontStyle.Italic)
							};
							pInfo.ProcessID = 0;
							pInfo.MainWindowHandle = IntPtr.Zero;
						}
						lvi.Name = pInfo.ProcessID.ToString();
						processListForListView.Add(lvi);
					}
				}
			}
			lvProcesses.Items.AddRange(processListForListView.ToArray());
			if (pInfoToSelect != null)
			{
				lvProcesses.Items[pInfoToSelect.ProcessID.ToString()].Selected = true;
			}
		}

		/// <summary>
		/// Fill the contextmenu for sessionlist with all saved sessions
		/// </summary>
		void PopulateSessionsList()
		{
			conmSessions.Items.Clear();
			if (settings.GetNumberOfSessions() > 0)
			{
				btnLoadSession.Enabled = true;
				btnLoadSession.Text = "Load session ->";

				foreach (Session session in settings.GetSessionList())
				{
					conmSessions.Items.Add(session.SessionName, null, SessionMenu_EventHandler);
				}
				conmSessions.Items.Add("-");
				ToolStripMenuItem menuItem_SessionMenu = new ToolStripMenuItem("Currently running processes", null, SessionMenu_EventHandler)
				{
					ToolTipText = "Unloads '" + settings.ActiveSession + "' and lists the currently running processes with an open window"
				};
				conmSessions.Items.Add(menuItem_SessionMenu);
			}
			else
			{
				btnLoadSession.Enabled = false;
				btnLoadSession.Text = "No saved sessions";
			}
		}

		/// <summary>
		/// Session in menu have been clicked
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void SessionMenu_EventHandler(object sender, EventArgs e)
		{
			if ((sender as ToolStripMenuItem).Text.Contains("Current"))
			{
				settings.ActiveSession = "current";
			}
			else
			{
				settings.ActiveSession = (sender as ToolStripMenuItem).Text;
			}
		}

		/// <summary>
		/// Shows form if it is minimized
		/// If form is not minimized, bring to front
		/// </summary>
		void ShowForm()
		{
			if (WindowState == FormWindowState.Minimized)
			{
				WindowState = FormWindowState.Normal;
			}

			Activate();
		}

		/// <summary>
		/// Handle systemwide windowmessages
		/// </summary>
		/// <param name="m">Message</param>
		protected override void WndProc(ref Message m)
		{
			if (m.Msg == NativeMethods.WM_SHOWME)
			{
				ShowForm();
			}
			base.WndProc(ref m);
		}

		/// <summary>
		/// Applies the settings of the loaded session
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void BtnApplySession_Click(object sender, EventArgs e)
		{
			settings.ApplyActiveSession();
		}

		/// <summary>
		/// Closes the application
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void BtnClose_Click(object sender, EventArgs e)
		{
			settings.AllowClosing = true;
			Application.Exit();
		}

		/// <summary>
		/// Creates a new session from the processes shown in lvProcesses and their configurations
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void BtnCreateNewSession_Click(object sender, EventArgs e)
		{
			NewSessionName newSessionName = new NewSessionName(settings.SessionNamesAutoComplete);
			newSessionName.ShowDialog();

			if (newSessionName.DialogResult == DialogResult.OK)
			{
				settings.SaveToFile();
				settings.AddSession(new Session(newSessionName.GetName(), settings.currentlyRunningProcesses.Plist));

				settings.ActiveSession = newSessionName.GetName();
				PopulateDockedLists();
				PopulateSessionsList();
			}
			newSessionName.Dispose();
		}

		/// <summary>
		/// Delete the currently loaded session
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void BtnDeleteLoadedSession_Click(object sender, EventArgs e)
		{
			settings.DeleteSession(settings.ActiveSession);
			settings.ActiveSession = "current";
			PopulateDockedLists();
			PopulateSessionsList();
			btnUpdateSettings.Enabled = true;
		}

		/// <summary>
		/// Open a form with list of all excluded applications
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void BtnExcludedApplications_Click(object sender, EventArgs e)
		{
			ExcludedApplications excludedForm = new ExcludedApplications(settings.GetExcludedApps());
			DialogResult returnAction = excludedForm.ShowDialog();
			if (returnAction == DialogResult.OK)
			{
				settings.ReplaceExcludedApplicationsList(excludedForm.GetExcludedApplications());
				settings.SaveToFile();
			}
			excludedForm.Dispose();
		}

		/// <summary>
		/// Gets position and size of an active window and enters this into textboxes
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void BtnGetActiveWindow_Click(object sender, EventArgs e)
		{
			Process p = Process.GetProcessesByName(markedProcess.ProcessName).First(x => x.MainWindowHandle != IntPtr.Zero);
			NativeMethods.RECT pos = new NativeMethods.RECT();
			NativeMethods.GetWindowRect(p.MainWindowHandle, ref pos);
			txtHeight.Text = (pos.bottom - pos.top).ToString();
			txtWidth.Text = (pos.right - pos.left).ToString();
			txtX.Text = pos.left.ToString();
			txtY.Text = pos.top.ToString();
			btnUpdateProcess.Enabled = true;
			p.Dispose();
		}

		/// <summary>
		/// Show info about button when mouse is hovering
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void BtnListRunningProcesses_MouseHover(object sender, EventArgs e)
		{
			ttInfo.Active = true;
			ttInfo.Show("Show a list of processes (not saved) with open, visible windows.", (Button)sender, 0, 18);
		}

		/// <summary>
		/// Clear list of processes, clear boxes of info and collect info for the active session
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="ea">Generic EventArgs</param>
		void BtnAddRunningProcess_Click(object sender, EventArgs ea)
		{
			conmsActiveWindows.Items.Clear();

			foreach (ProcessInfo pInfo in GetListOfRunningProcesses())
			{
				if (settings.GetSession(settings.ActiveSession).Plist.Find(x => x.ProcessName.Equals(pInfo.ProcessName)) == null)
				{
					string windowTitle = pInfo.MainWindowTitle.Equals("") ? "Process: " + pInfo.ProcessName : "Process: " + pInfo.ProcessName;
					conmsActiveWindows.Items.Add(new ToolStripMenuItem(windowTitle, null, (s, e) => AddProcess(pInfo)));
				}
			}
			conmsActiveWindows.Show(btnAddRunningProcess, 0, 18);
		}

		/// <summary>
		/// Open a contextmenu, displaying all saved sessions, for loading a session
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void BtnLoadSession_Click(object sender, EventArgs e)
		{
			if (btnUpdateSettings.Enabled)
			{
				if (!settings.ActiveSession.Equals("current"))
				{
					DialogResult returnAction = MessageBox.Show("There are unsaved changes in session ('" + settings.ActiveSession + "')\r\nDo you want to save?", "", MessageBoxButtons.YesNo);
					if (returnAction == DialogResult.Yes)
					{
						BtnUpdateSettings_Click(null, null);
					}
					else
					{
						btnUpdateSettings.Enabled = false;
					}
				}
			}
			conmSessions.Show(btnLoadSession, 0, 18);
		}

		/// <summary>
		/// Information for a process is to be saved to session
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void BtnUpdateProcess_Click(object sender, EventArgs e)
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
			if (settings.ActiveSession.Equals("current"))
			{
				settings.currentlyRunningProcesses.UpdateProcess(tempProcess);
			}
			else
			{
				settings.GetSession(settings.ActiveSession).UpdateProcess(tempProcess);
			}
			btnUpdateProcess.Enabled = false;
			btnUpdateSettings.Enabled = true;
		}

		/// <summary>
		/// Remove the process from session
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void BtnRemoveProcess_Click(object sender, EventArgs e)
		{
			DeleteProcess(null, null);
		}

		/// <summary>
		/// Information about a process have been updated, save to session and file
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void BtnUpdateSettings_Click(object sender, EventArgs e)
		{
			settings.SaveToFile();
			btnUpdateSettings.Enabled = false;
		}

		/// <summary>
		/// New layoutinfo have been entered, use for the process' window. Does not save to session to file.
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void BtnSetProcessInfoLayout_Click(object sender, EventArgs e)
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
			if (settings.ActiveSession.Equals("current"))
			{
				settings.currentlyRunningProcesses.UpdateProcess(tempProcess);
			}
			else
			{
				settings.GetSession(settings.ActiveSession).UpdateProcess(tempProcess);
			}

			if (markedProcess.ProcessID != 0)
				new WindowLayout().SetLayout(tempProcess);
		}

		/// <summary>
		/// Open a menu with active windows
		/// Clicking on an item, opens a window with information about the process and window
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="ea">Generic EventArgs</param>
		void BtnWinInfo_Click(object sender, EventArgs ea)
		{
			conmsActiveWindows.Items.Clear();

			List<ProcessInfo> listProcesses = GetListOfRunningProcesses();
			foreach (ProcessInfo pInfo in listProcesses)
			{
				conmsActiveWindows.Items.Add(pInfo.ProcessName + " (ProcessId: " + pInfo.ProcessID + ")", null, (s, e) => ActiveWindow_EventHandler(s, e, pInfo.ProcessID));
			}
			conmsActiveWindows.Show(btnWinInfo, 0, 18);
		}

		/// <summary>
		/// User wants the application to start minimized to the notificationarea
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void CheckboxStart_CheckedChanged(object sender, EventArgs e)
		{
			settings.StartInSysTray = checkboxStart.Checked;
			btnUpdateSettings.Enabled = true;
		}

		/// <summary>
		/// Setting for the visibility of taskbar in the saved session
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void CheckboxTaskbar_CheckedChanged(object sender, EventArgs e)
		{
			settings.GetSession(settings.ActiveSession).TaskbarVisible = checkboxSessionVisibleInTaskbar.Checked;
			btnUpdateSettings.Enabled = true;
		}

		/// <summary>
		/// User have selected which session is to be used when the computer is docked (have more than one screen connected)
		/// Remove that session from the other combobox to avoid it being used
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void ComboboxDocked_SelectedIndexChanged(object sender, EventArgs e)
		{
			settings.DockedSession = comboboxDocked.SelectedItem.ToString();
			btnUpdateSettings.Enabled = true;
		}

		/// <summary>
		/// Rolldown the list when clicked
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void ComboboxMinimized_Click(object sender, EventArgs e)
		{
			comboboxWindowPlacement.DroppedDown = true;
		}

		/// <summary>
		/// User have selected which session is to be used when the computer is not docked (only one screen connected)
		/// Remove that session from the other combobox to avoid it being used
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void ComboboxUndocked_SelectedIndexChanged(object sender, EventArgs e)
		{
			settings.UndockedSession = comboboxUndocked.SelectedItem.ToString();
			btnUpdateSettings.Enabled = true;
		}

		/// <summary>
		/// The setting for how the window is shown, have changed
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void ComboboxWindowPlacement_SelectedIndexChanged(object sender, EventArgs e)
		{
			markedProcess.WindowPlacement = comboboxWindowPlacement.SelectedIndex + 1;
			btnUpdateProcess.Enabled = true;

			if (comboboxWindowPlacement.SelectedIndex == 0)
			{
				txtX.Enabled = true;
				txtY.Enabled = true;
				txtWidth.Enabled = true;
				txtHeight.Enabled = true;
			}
			else
			{
				txtX.Enabled = false;
				txtY.Enabled = false;
				txtWidth.Enabled = false;
				txtHeight.Enabled = false;
			}
		}

		/// <summary>
		/// The mousepointer is no longer over the control.
		/// Hide the tooltip
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void Control_MouseLeave(object sender, EventArgs e)
		{
			ttInfo.Active = false;
		}

		/// <summary>
		/// Check if a click in lvProcesses occured over an item
		/// If there are no ListViewItem where the click occured, clear boxes of information and clear markedProcess
		/// If the right mousebutton was clicked, open contextmenu for removal of process
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic MouseEventArgs</param>
		void LvProcesses_MouseDown(object sender, MouseEventArgs e)
		{
			ListViewItem item = lvProcesses.GetItemAt(e.X, e.Y);

			if (item == null)
			{
				lvProcesses.SelectedIndices.Clear();
				btnRemoveProcess.Enabled = false;
				EventsOff();
				ClearTextBoxes();
				EventsOn();
				markedProcess = null;
				gbWindowSettings.Enabled = false;
			}
			if (e.Button == MouseButtons.Right)
			{
				conmenuProcessAction.Show(Cursor.Position);
				conmenuProcessAction.Visible = true;
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
		void LvProcesses_SelectedIndexChanged(object sender, EventArgs e)
		{
			markedProcess = null;
			if (lvProcesses.SelectedItems.Count != 0)
			{
				EventsOff();
				ClearTextBoxes();
				gbWindowSettings.Enabled = true;
				ttInfo.Active = false;
				string selectedProcess = lvProcesses.SelectedItems[0].SubItems[1].Text;

				if (settings.ActiveSession.Equals("current"))
				{
					markedProcess = settings.currentlyRunningProcesses.GetProcess(selectedProcess);
					btnRemoveProcess.Text = "Remove process from list";
					gbWindowSettings.Text = "Window information of active process";
				}
				else
				{
					markedProcess = settings.GetSession(settings.ActiveSession).GetProcess(selectedProcess);
					btnRemoveProcess.Text = "Remove process from '" + settings.ActiveSession + "'";
					gbWindowSettings.Text = "Window information saved in session";
				}
				btnRemoveProcess.Enabled = true;
				txtProcess.Text = markedProcess.ProcessName;
				txtId.Text = markedProcess.ProcessID.ToString();
				txtMainWindowHandle.Text = markedProcess.MainWindowHandle.ToString();

				if (markedProcess.MainWindowHandle == IntPtr.Zero)
				{
					btnGetActiveWindow.Enabled = false;
					btnSetProcessInfoLayout.Enabled = false;
					if (markedProcess.ProcessID == 0)
					{
						txtId.Text = "No process for " + markedProcess.ProcessName + " is running";
						txtMainWindowHandle.Text = "0";
						txtId.Font = txtMainWindowHandle.Font = new Font(Font, FontStyle.Italic);
						txtId.BackColor = txtMainWindowHandle.BackColor = Color.Tomato;
					}
					else
					{
						txtMainWindowHandle.Text = "No open/visible window for " + markedProcess.ProcessName;
						txtMainWindowHandle.Font = new Font(Font, FontStyle.Italic);
						txtMainWindowHandle.BackColor = Color.Tomato;
					}
				}
				else
				{
					btnGetActiveWindow.Enabled = true;
					btnSetProcessInfoLayout.Enabled = true;
					txtId.Font = txtMainWindowHandle.Font = new Font(Font, FontStyle.Regular);
					txtId.BackColor = txtMainWindowHandle.BackColor = DefaultBackColor;
				}

				if (markedProcess.WindowPlacement == 2 || markedProcess.WindowPlacement == 3)
				{
					txtHeight.Enabled = false;
					txtWidth.Enabled = false;
					txtY.Enabled = false;
					txtX.Enabled = false;
				}
				else
				{
					txtHeight.Enabled = true;
					txtWidth.Enabled = true;
					txtY.Enabled = true;
					txtX.Enabled = true;
				}
				comboboxWindowPlacement.SelectedIndex = markedProcess.WindowPlacement - 1;
				txtX.Text = markedProcess.XTopCoordinate.ToString();
				txtY.Text = markedProcess.YTopCoordinate.ToString();
				txtHeight.Text = markedProcess.Height.ToString();
				txtWidth.Text = markedProcess.Width.ToString();
				EventsOn();
			}
		}

		/// <summary>
		/// Tooltip shown for textbox for coordinates when mouse is hovering
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void TxtCoordinate_MouseHover(object sender, EventArgs e)
		{
			ttInfo.Active = true;
			ttInfo.Show("Coordinate of the upper left cornet", (TextBox)sender, 0, 18);
		}

		/// <summary>
		/// Text for window height have changed
		/// If no width is entered, use previous height
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void TxtHeight_TextChanged()
		{
			if (markedProcess != null)
			{
				if (txtHeight.Text.Length == 0)
				{
					txtHeight.Text = markedProcess.Height.ToString();
					ttInfo.Show("Window can't be 0 pixels high", txtHeight, 0, 18);
				}
				else
				{
					markedProcess.Height = int.Parse(txtHeight.Text);
					btnUpdateProcess.Enabled = true;
				}
			}
		}

		/// <summary>
		/// A textbox for numbers have changed
		/// Check if text only contains numbers, decode which textbox and call it's function
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void TxtNumber_TextChanged(object sender, EventArgs e)
		{
			string newNumber = (sender as TextBox).Text;
			char name = (sender as TextBox).Name[3];
			if (int.TryParse(newNumber, out int number))
			{
				switch (name)
				{
					case 'Y':
						TxtY_TextChanged();
						break;
					case 'X':
						TxtX_TextChanged();
						break;
					case 'H':
						TxtHeight_TextChanged();
						break;
					case 'W':
						TxtWidth_TextChanged();
						break;
				}
			}
			else
			{
				ttInfo.Show("Only numbers are allowed", (sender as TextBox), 0, 18);
				switch (name)
				{
					case 'Y':
						txtY.Text = markedProcess.YTopCoordinate.ToString();
						break;
					case 'X':
						txtX.Text = markedProcess.XTopCoordinate.ToString();
						break;
					case 'H':
						txtHeight.Text = markedProcess.Height.ToString();
						break;
					case 'W':
						txtWidth.Text = markedProcess.Width.ToString();
						break;
				}
			}
		}

		/// <summary>
		/// Text for window width have changed
		/// If no width is entered, use previous width
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void TxtWidth_TextChanged()
		{
			if (markedProcess != null)
			{
				if (txtWidth.Text.Length == 0 || int.Parse(txtWidth.Text) == 0)
				{
					txtWidth.Text = markedProcess.Width.ToString();
					ttInfo.Show("Window can't be 0 pixels wide", txtWidth, 0, 18);
				}
				else
				{
					markedProcess.Width = int.Parse(txtWidth.Text);
					btnUpdateProcess.Enabled = true;
				}
			}
		}

		/// <summary>
		/// Text for window left most X-coordinate have changed
		/// Check if the coordinate is within visible range
		/// If coordinate is missing or too close to screenedge, use more visible coordinate 
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void TxtX_TextChanged()
		{
			if (markedProcess != null)
			{
				if (txtX.Text.Length == 0)
				{
					txtX.Text = markedProcess.XTopCoordinate.ToString();
					return;
				}
				else if (int.Parse(txtX.Text) > SystemInformation.VirtualScreen.Width)
				{
					txtX.Text = string.Format("{0}", SystemInformation.VirtualScreen.Width - 20);
					ttInfo.Show("Number would put window outside screen. Setting it within screen.", txtY, 0, 18);
				}
				markedProcess.XTopCoordinate = int.Parse(txtX.Text);
				btnUpdateProcess.Enabled = true;
			}
		}

		/// <summary>
		/// Text for window upper most Y-coordinate have changed
		/// Check if the coordinate is within visible range
		/// If coordinate is missing or too close to screenedge, use more visible coordinate 
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void TxtY_TextChanged()
		{
			if (markedProcess != null)
			{
				if (txtY.Text.Length == 0)
				{
					txtY.Text = markedProcess.YTopCoordinate.ToString();
					return;
				}
				else if (int.Parse(txtY.Text) > SystemInformation.VirtualScreen.Height)
				{
					txtY.Text = string.Format("{0}", SystemInformation.VirtualScreen.Height - 20);
					ttInfo.Show("Number would put window outside screen. Setting it within screen.", txtY, 0, 18);
				}
				markedProcess.YTopCoordinate = int.Parse(txtY.Text);
				btnUpdateProcess.Enabled = true;
			}
		}
	}
}
