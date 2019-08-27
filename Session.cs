using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Session_windows
{
	/// <summary>
	/// Description of Session.
	/// </summary>
	public class Session
	{
		List<ProcessInfo> plist;
		string sessionName;
		bool taskbarVisible;

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr FindWindow(string strClassName, string strWindowName);

		[DllImport("shell32.dll")]
		public static extern UInt32 SHAppBarMessage(UInt32 dwMessage, ref APPBARDATA pData);

		public enum AppBarMessages
		{
		    New              = 0x00,
		    Remove           = 0x01,
		    QueryPos         = 0x02,
		    SetPos           = 0x03,
		    GetState         = 0x04,
		    GetTaskBarPos    = 0x05,
		    Activate         = 0x06,
		    GetAutoHideBar   = 0x07,
		    SetAutoHideBar   = 0x08,
		    WindowPosChanged = 0x09,
		    SetState         = 0x0a
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct APPBARDATA
		{
		    public UInt32 cbSize;
		    public IntPtr hWnd;
		    public UInt32 uCallbackMessage;
		    public UInt32 uEdge;
		    public Rectangle rc;
		    public Int32 lParam;
		}

		public enum AppBarStates
		{
		    AutoHide    = 0x01,
		    AlwaysOnTop = 0x02
		}

		public Session(){}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="n">Name of session</param>
		/// <param name="l">List of processes in session</param>
		public Session(string n, List<ProcessInfo> l)
		{
			plist = l;
			sessionName = n;
		}

		/// <summary>
		/// Contructor
		/// </summary>
		/// <param name="s">Session to be used</param>
		public Session(Session s)
		{
			plist = s.Plist;
			sessionName = s.SessionName;
		}

		public string SessionName { get { return sessionName; } set { sessionName = value; } }
		public List<ProcessInfo> Plist { get { return plist; } set { plist = value; } }
		public bool TaskbarVisible { get { return taskbarVisible; }  set { taskbarVisible = value;}}

		/// <summary>
		/// Set the Taskbar State option
		/// </summary>
		/// <param name="option">AppBarState to activate</param>
		static void setTaskbarState(AppBarStates option)
		{
		    APPBARDATA msgData = new APPBARDATA();
		    msgData.cbSize = (UInt32)Marshal.SizeOf(msgData);
			msgData.hWnd = FindWindow("System_TrayWnd", null);
			if (msgData.hWnd == IntPtr.Zero)
			    msgData.hWnd = FindWindow("Shell_TrayWnd", null);
			msgData.lParam = (Int32)(option);
		    SHAppBarMessage((UInt32)AppBarMessages.SetState, ref msgData);
		}

		/// <summary>
		/// Adds a process to the session
		/// </summary>
		/// <param name="process"></param>
		public void addProcessToSession(ProcessInfo process)
		{
			plist.Add(process);
		}

		/// <summary>
		/// Searches for, and returns a process by given process name
		/// </summary>
		/// <param name="processName">Name of process to return</param>
		/// <returns>Returns the process in the session with same name</returns>
		public ProcessInfo getProcess(string processName)
		{
			return plist.Find(x => x.ProcessName.Equals(processName));
		}

		/// <summary>
		/// Deletes a process from the processlist
		/// </summary>
		/// <param name="process">Processname</param>
		public void deleteProcessFromSession(ProcessInfo process)
		{
			int processIndex = plist.FindIndex(x => x.ProcessID == process.ProcessID);
			plist.RemoveAt(processIndex);
		}

		/// <summary>
		/// Updates the information about the process
		/// </summary>
		/// <param name="process">An object containing the new information</param>
		public void updateProcess(ProcessInfo process)
		{
			int processIndex = plist.FindIndex(x => x.ProcessName.Equals(process.ProcessName));
			plist[processIndex] = process;
		}

		/// <summary>
		/// Apply saved settings for the session and taskbar
		/// </summary>
		public void useSession()
		{
			foreach(ProcessInfo info in Plist)
			{
				new WindowLayout().setLayout(info);
			}

			if (taskbarVisible)
				setTaskbarState(AppBarStates.AlwaysOnTop);
			else
				setTaskbarState(AppBarStates.AutoHide);
			Program.icon.ShowBalloonTip(8, "Session windows", "Session '" + sessionName + "' is now loaded", System.Windows.Forms.ToolTipIcon.Info);
		}
	}
}
