using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Session_windows
{
	/// <summary>
	/// Description of WindowInfo.
	/// </summary>
	public partial class WindowInfo : Form
	{
		[DllImport("user32.dll")]
		static extern bool GetWindowRect(IntPtr hWnd, ref RECT Rect);

		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

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
		/// Struct for windowcoordinates
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		public struct RECT
		{
			public int left;
			public int top;
			public int right;
			public int bottom;
			public int placement;
			public string placementName;
		}
		WINDOWPLACEMENT placement;

		public WindowInfo(int processID)
		{
			InitializeComponent();
			Process process = Process.GetProcessById(processID);
			RECT pos = position(process.MainWindowHandle);
			Text += " for "+process.ProcessName;

			label1.Text = "Processname: " + process.ProcessName+"\n";
			//label1.Text += "Process file name: "+process.MainModule.FileName+"\n";
			label1.Text += "MainWindowTitle: " + (process.MainWindowTitle.Equals("") ? "Process: " + process.ProcessName : process.MainWindowTitle)+"\n";
			label1.Text += "ProcessID: " + processID+"\n";
			label1.Text += "X coordinate top: " + pos.left+"\n";
			label1.Text += "Y coordinate top: " + pos.top+"\n";
			label1.Text += "X coordinate bottom: " + pos.right + " (width " + (pos.right - pos.left) + ")"+"\n";
			label1.Text += "Y coordinate bottom: " + pos.bottom + " (height " + (pos.bottom - pos.top) + ")"+"\n";
			label1.Text += "WindowPlacement: " + pos.placement + " (" + pos.placementName + ")"+"\n";
			label1.Text += "Starttime: " + process.StartTime+"\n";
			label1.Text += "Processor time: " + process.TotalProcessorTime+"\n";
			label1.Text += "User time: " + process.UserProcessorTime+"\n";
			label1.Text += "Privileged time: " + process.PrivilegedProcessorTime+"\n";
			label1.Text += "BasePriority: " + process.BasePriority + "(" + prio(process.BasePriority) + ")"+"\n";
			label1.Text += "MaxWorkingSet: " + ((double)process.MaxWorkingSet).ToFileSize()+"\n";
			label1.Text += "MinWorkingSet: " + ((double)process.MinWorkingSet).ToFileSize()+"\n";
			label1.Text += "Working Set: " + ((double)process.WorkingSet64).ToFileSize()+"\n";
			label1.Text += "Non-paged Memory Size: " + ((double)process.NonpagedSystemMemorySize64).ToFileSize()+"\n";
			label1.Text += "Paged Memory Size: " + ((double)process.PagedMemorySize64).ToFileSize()+"\n";
			label1.Text += "Paged System Memory Size: " + ((double)process.PagedSystemMemorySize64).ToFileSize()+"\n";
			label1.Text += "Peak Paged Memory Size: " + ((double)process.PeakPagedMemorySize64).ToFileSize()+"\n";
			label1.Text += "Peak Virtual Memory Size: " + ((double)process.PeakVirtualMemorySize64).ToFileSize()+"\n";
			label1.Text += "Peak Working Set: " + ((double)process.PeakWorkingSet64).ToFileSize()+"\n";
			label1.Text += "Virtual Memory Size: "+((double)process.VirtualMemorySize64).ToFileSize()+"\n";
			label1.Text += "SessionID: " + process.SessionId + "\n";
			label1.Text += "Startinfo: " + "\n    UserName: " + process.StartInfo.UserName+"\n"+
				"    FileName: "+process.StartInfo.FileName+"\n"+
				"    WorkingDirectory: "+process.StartInfo.WorkingDirectory+"\n";
			label1.Text += "Thread 1 - ID: " + process.Threads[0].Id;
		}
		RECT position(IntPtr id)
		{
			RECT Rect = new RECT();

			placement.length = Marshal.SizeOf(placement);
			if (GetWindowRect(id, ref Rect)) {
				GetWindowPlacement(id, ref placement);
				Rect.placement = (int)placement.showCmd;
			}
			switch (Rect.placement) {
				case 0:
					Rect.placementName = "SW_NORMAL";
					break;
				case 1:
					Rect.placementName = "SW_SHOWNORMAL";
					break;
				case 2:
					Rect.placementName = "SW_SHOWMINIMIZED";
					break;
				case 3:
					Rect.placementName = "SW_MAXIMIZE";
					break;
				case 4:
					Rect.placementName = "SW_SHOWNOACTIVATE";
					break;
				case 5:
					Rect.placementName = "SW_SHOW";
					break;
				case 6:
					Rect.placementName = "SW_MINIMIZE";
					break;
				case 7:
					Rect.placementName = "SW_SHOWMINNOACTIVE";
					break;
				case 8:
					Rect.placementName = "SW_SHOWNA";
					break;
				case 9:
					Rect.placementName = "SW_RESTORE";
					break;
			}
			return Rect;
		}

		string prio(int basePriority)
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

	}
}
