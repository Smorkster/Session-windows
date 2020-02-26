using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Session_windows.Models
{
	/// <summary>
	/// Description of Session.
	/// </summary>
	public class Session
	{
		/// <summary>
		/// Timeout, in seconds, for the ballontip when session is applied
		/// </summary>
		readonly int balloonTipTimeout = 5;

		/// <summary>
		/// Retrieves a handle to the top-level window whose class name and window name match the specified strings
		/// </summary>
		/// <param name="strClassName">Class name or a class atom created by a previous call to the RegisterClass or RegisterClassEx function</param>
		/// <param name="strWindowName">Window name</param>
		/// <returns>Handle to the window that has the specified class name and window name</returns>
		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		internal static extern IntPtr FindWindow(string strClassName, string strWindowName);

		/// <summary>
		/// Sends an appbar message to the system
		/// </summary>
		/// <param name="dwMessage">Appbar message value to send (see https://docs.microsoft.com/en-us/windows/win32/api/shellapi/nf-shellapi-shappbarmessage)</param>
		/// <param name="pData">Pointer to an APPBARDATA structure</param>
		/// <returns>A message-dependent value</returns>
		[DllImport("shell32.dll")]
		internal static extern uint SHAppBarMessage(uint dwMessage, ref APPBARDATA pData);

		/// <summary>
		/// Information about a system appbar message
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		internal struct APPBARDATA : IDisposable
		{
			internal uint cbSize;
			internal IntPtr hWnd;
			internal uint uCallbackMessage;
			internal uint uEdge;
			Rectangle rc;
			internal int lParam;
			public void Dispose() { }
		}

		internal Session() { }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="name">Name of session</param>
		/// <param name="l">List of processes in session</param>
		internal Session(string name, List<ProcessInfo> l)
		{
			SessionName = name;
			Plist = l;
			TaskbarVisible = true;
		}

		/// <summary>
		/// Name of this session
		/// </summary>
		internal string SessionName { get; set; }
		/// <summary>
		/// List of processes, with their location and size information, saved in this session
		/// </summary>
		internal List<ProcessInfo> Plist { get; set; }
		/// <summary>
		/// Bool for if the taskbar is to be visible when this session is active
		/// </summary>
		internal bool TaskbarVisible { get; set; }

		/// <summary>
		/// Adds a process to the session
		/// </summary>
		/// <param name="process"></param>
		internal void AddProcessToSession(ProcessInfo process)
		{
			Plist.Add(process);
		}

		/// <summary>
		/// Deletes a process from the processlist
		/// </summary>
		/// <param name="process">Processname</param>
		internal void DeleteProcessFromSession(ProcessInfo process)
		{
			int processIndex = Plist.FindIndex(x => x.ProcessID == process.ProcessID);
			Plist.RemoveAt(processIndex);
		}

		/// <summary>
		/// Searches for, and returns a process by given process name
		/// </summary>
		/// <param name="processName">Name of process to return</param>
		/// <returns>Returns the process in the session with same name</returns>
		internal ProcessInfo GetProcess(string processName)
		{
			return new ProcessInfo(Plist.Find(x => x.ProcessName.Equals(processName)));
		}

		/// <summary>
		/// Return a windowhandle for the windows of each process in plist
		/// </summary>
		/// <returns>Array of windowhandles</returns>
		internal List<IntPtr> GetWindowHandles()
		{
			List<IntPtr> handles = new List<IntPtr>();

			if (SessionName.Equals("current"))
			{
				return null;
			}
			else
			{
				foreach (ProcessInfo pi in Plist)
				{
					if (pi.MainWindowHandle != IntPtr.Zero && pi.ProcessID != 0)

						handles.Add(pi.MainWindowHandle);
				}
				return handles;
			}
		}

		/// <summary>
		/// Set the Windows taskbar to AutoHide or AlwaysOnTop
		/// </summary>
		void SetTaskbarState()
		{
			APPBARDATA msgData = new APPBARDATA();
			msgData.cbSize = (uint)Marshal.SizeOf(msgData);
			if ((msgData.hWnd = FindWindow("Shell_TrayWnd", null)) == IntPtr.Zero)
				msgData.hWnd = FindWindow("System_TrayWnd", null);

			if (TaskbarVisible)
				msgData.lParam = 0x02;
			else
				msgData.lParam = 0x01;
			SHAppBarMessage(0x0a, ref msgData);

		}

		/// <summary>
		/// Updates the information about the process
		/// </summary>
		/// <param name="process">An object containing the new information</param>
		internal void UpdateProcess(ProcessInfo process)
		{
			int processIndex = Plist.FindIndex(x => x.ProcessName.Equals(process.ProcessName));
			Plist[processIndex] = process;
		}

		/// <summary>
		/// Apply saved settings for the session and taskbar
		/// </summary>
		internal void UseSession()
		{
			foreach (ProcessInfo info in Plist)
			{
				new WindowLayout().SetLayout(info);
			}

			SetTaskbarState();
			ApplicationControls.trayicon.ShowBalloonTip(balloonTipTimeout, "Session windows", "Session '" + SessionName + "' is now loaded", System.Windows.Forms.ToolTipIcon.Info);
			ApplicationControls.trayicon.Text = "Session '" + SessionName + "' loaded";
		}

		/// <summary>
		/// Apply saved settings for the session and taskbar
		/// Apply the settings for windows by reversed z-order
		/// </summary>
		/// <param name="handles">List of KeyValuePairs with handles of open windows that have saved settings in this session</param>
		internal void UseSession(List<KeyValuePair<IntPtr, int>> handles)
		{
			foreach (KeyValuePair<IntPtr, int> item in handles)
			{
				new WindowLayout().SetLayout(Plist.Find(p => p.MainWindowHandle.Equals(item.Key)));
			}

			SetTaskbarState();
			ApplicationControls.trayicon.ShowBalloonTip(balloonTipTimeout, "Session windows", "Session '" + SessionName + "' is now loaded", System.Windows.Forms.ToolTipIcon.Info);
			ApplicationControls.trayicon.Text = "Session '" + SessionName + "' loaded";
		}
	}
}
