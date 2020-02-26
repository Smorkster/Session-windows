using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Session_windows
{
	/// <summary>
	/// Defines a window showing as much info as possible about a process and its mainwindow     
	/// </summary>
	internal partial class WindowInfo : Form
	{
		/// <summary>
		/// Placement of window
		/// </summary>
		NativeMethods.WINDOWPLACEMENT placement;
		/// <summary>
		/// Timer for updateinterval of textboxes
		/// </summary>
		Timer timer;
		/// <summary>
		/// Process-object
		/// </summary>
		Process process;

		/// <summary>
		/// Enumeration of available windowcommands for WINDOWPLACEMENT
		/// </summary>
		internal enum ShowWindowCommands
		{
			SW_HIDE = 0,
			SW_SHOWNORMAL = 1,
			// Displayed
			SW_SHOWMINIMIZED = 2,
			// Minimized
			SW_MAXIMIZE = 3,
			// Maximized
			SW_SHOWNOACTIVATE = 4,
			SW_SHOW = 5,
			SW_MINIMIZE = 6,
			SW_SHOWMINNOACTIVE = 7,
			SW_SHOWNA = 8,
			SW_RESTORE = 9
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
		/// Construct
		/// </summary>
		/// <param name="processID">ID of the process to show</param>
		internal WindowInfo(int processId)
		{
			InitializeComponent();
			process = Process.GetProcessById(processId);
			NativeMethods.RECT windowRectangle = Position(process.MainWindowHandle);
			Text += " for " + process.ProcessName;
			IntPtr processHandle = process.Handle;
			timer = new Timer();

			timer.Tick += TickOccured;
			timer.Interval = 100;
			timer.Start();

			txtProcessName.Text = process.ProcessName;
			try {
				txtMainModuleFileName.Text = process.MainModule.FileName;
			} catch (Exception e) {
				txtMainModuleFileName.Font = new Font(Font, FontStyle.Italic);
				txtMainModuleFileName.Text = e.Message;
			}
			txtMainWindowTitle.Text = process.MainWindowTitle.Equals("") ? process.ProcessName : process.MainWindowTitle;
			txtProcessID.Text = processId.ToString();
			txtXCoordinateTop.Text = windowRectangle.left.ToString();
			txtYCoordinateTop.Text = windowRectangle.top.ToString();
			txtXCoordinateBottom.Text = windowRectangle.right + " (width " + (windowRectangle.right - windowRectangle.left) + ")";
			txtYCoordinateBottom.Text = windowRectangle.bottom + " (height " + (windowRectangle.bottom - windowRectangle.top) + ")";
			txtWindowPlacement.Text = windowRectangle.placement + " (" + windowRectangle.placementName + ")";
			txtWindowPlacementComment.Text = windowRectangle.placementComment;
			txtStarttime.Text = process.StartTime.ToLongDateString() + " "+ process.StartTime.ToLongTimeString();
			txtBasePriority.Text = process.BasePriority + " (" + Prio(process.BasePriority) + ")";
			txtMaxWorkingSet.Text = ((double)process.MaxWorkingSet).ToString();
			txtMinWorkingSet.Text = ((double)process.MinWorkingSet).ToString();
			txtWorkingSet.Text = ((double)process.WorkingSet64).ToString();
			txtNonPagedMemorySize.Text = ((double)process.NonpagedSystemMemorySize64).ToString();
			txtPagedMemorySize.Text = ((double)process.PagedMemorySize64).ToString();
			txtPagedSystemMemorySize.Text = ((double)process.PagedSystemMemorySize64).ToString();
			txtPeakPagedMemorySize.Text = ((double)process.PeakPagedMemorySize64).ToString();
			txtPeakVirtualMemorySize.Text = ((double)process.PeakVirtualMemorySize64).ToString();
			txtPeakWorkingSet.Text = ((double)process.PeakWorkingSet64).ToString();
			txtVirtualMemorySize.Text = ((double)process.VirtualMemorySize64).ToString();
			txtSessionID.Text = process.SessionId.ToString();
			txtStartinfoUserName.Text = process.StartInfo.UserName;
			txtStartinfoFileName.Text = process.StartInfo.FileName;
			txtStartinfoWorkingDirectory.Text = process.StartInfo.WorkingDirectory;
			txtThread1ID.Text = process.Threads[0].Id.ToString();
			ActiveControl = txtProcessName;
		}

		#region Operational methods
		/// <summary>
		/// Gets the window placementstyle, size and position 
		/// </summary>
		/// <param name="windowHandle">Handle of window to check</param>
		/// <returns>Windowplacement-style of target window</returns>
		internal NativeMethods.RECT Position(IntPtr windowHandle)
		{
			NativeMethods.RECT windowRect = new NativeMethods.RECT();

			placement.length = Marshal.SizeOf(placement);
			if (NativeMethods.GetWindowRect(windowHandle, ref windowRect))
			{
				NativeMethods.GetWindowPlacement(windowHandle, ref placement);
				windowRect.placement = (int)placement.showCmd;
			}
			switch (windowRect.placement)
			{
				case 0:
					windowRect.placementName = "SW_HIDE";
					windowRect.placementComment = "Hides the window and activates another window.";
					break;
				case 1:
					windowRect.placementName = "SW_SHOWNORMAL";
					windowRect.placementComment = "Activates and displays a window. If the window is minimized or maximized, the system restores it to its original size and position. \n\rAn application should specify this flag when displaying the window for the first time.";
					break;
				case 2:
					windowRect.placementName = "SW_SHOWMINIMIZED";
					windowRect.placementComment = "Activates the window and displays it as a minimized window.";
					break;
				case 3:
					windowRect.placementName = "SW_SHOWMAXIMIZED";
					windowRect.placementComment = "Activates the window and displays it as a maximized window.";
					break;
				case 4:
					windowRect.placementName = "SW_SHOWNOACTIVATE";
					windowRect.placementComment = "Displays a window in its most recent size and position.";
					break;
				case 5:
					windowRect.placementName = "SW_SHOW";
					windowRect.placementComment = "Activates the window and displays it in its current size and position.";
					break;
				case 6:
					windowRect.placementName = "SW_MINIMIZE";
					windowRect.placementComment = "Minimizes the specified window and activates the next top-level window in the z-order.";
					break;
				case 7:
					windowRect.placementName = "SW_SHOWMINNOACTIVE";
					windowRect.placementComment = "Displays the window as a minimized window.";
					break;
				case 8:
					windowRect.placementName = "SW_SHOWNA";
					windowRect.placementComment = "Displays the window in its current size and position.";
					break;
				case 9:
					windowRect.placementName = "SW_RESTORE";
					windowRect.placementComment = "Activates and displays the window. If the window is minimized or maximized, the system restores it to its original size and position.\n\rAn application should specify this flag when restoring a minimized window.";
					break;
			}
			return windowRect;
		}

		/// <summary>
		/// Returns a description for the working base priority of a process
		/// </summary>
		/// <param name="basePriority">BasePriority to describe</param>
		/// <returns>Description of basepriority</returns>
		string Prio(int basePriority)
		{
			string r = "";
			switch (basePriority) {
				case 4:
					r = "Idle";
					break;
				case 6:
					r = "Below normal";
					break;
				case 8:
					r = "Normal";
					break;
				case 10:
					r = "Above normal";
					break;
				case 13:
					r = "High";
					break;
				case 24:
					r = "RealTime";
					break;
			}
			return r;
		}
		#endregion

		#region Event methods
		/// <summary>
		/// Timeintervall for timer have looped
		/// Update textboxes at each tick
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void TickOccured(object sender, EventArgs e)
		{
			TimeSpan running = DateTime.Now - process.StartTime;

			txtTimeRunning.Text = running.Days + " days, " + running.Hours + " hours, " + running.Minutes + " minutes, " + running.Seconds + " seconds";
			txtProcessorTime.Text = process.TotalProcessorTime.Days + " days, " + process.TotalProcessorTime.Hours + " hours, " + process.TotalProcessorTime.Minutes + " minutes, " + process.TotalProcessorTime.Seconds + " seconds, " + process.TotalProcessorTime.Milliseconds + " milliseconds";
			txtUserTime.Text = process.UserProcessorTime.Days + " days, " + process.UserProcessorTime.Hours + " hours, " + process.UserProcessorTime.Minutes + " minutes, " + process.UserProcessorTime.Seconds + " seconds, " + process.UserProcessorTime.Milliseconds + " milliseconds";
			txtPrivilegedTime.Text = process.PrivilegedProcessorTime.Days + " days, " + process.PrivilegedProcessorTime.Hours + " hours, " + process.PrivilegedProcessorTime.Minutes + " minutes, " + process.PrivilegedProcessorTime.Seconds + " seconds, " + process.PrivilegedProcessorTime.Milliseconds + " milliseconds";
		}

		/// <summary>
		/// Form is closing
		/// Stop timer and decouple the event for ticks
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic FormClosingEventArgs</param>
		void WindowInfo_FormClosing(object sender, FormClosingEventArgs e)
		{
			timer.Stop();
			timer.Tick -= TickOccured;
			timer.Dispose();
			Dispose();
		}

		/// <summary>
		/// Catches if the Escape-key is pressed, then close the form
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic KeyEventArgs</param>
		void WindowInfo_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode.Equals(Keys.Escape))
				Close();
		}
		#endregion
	}
}