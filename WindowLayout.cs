using Session_windows.Library;
using Session_windows.Models;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Session_windows
{
	/// <summary>
	/// Class for setting the layout of a windowform 
	/// </summary>
	public static class WindowLayout
	{
		/// <summary>
		/// Take the processinfo specified by the user,
		/// find all open windows of the same processname,
		/// apply the settings from the processinfo
		/// </summary>
		/// <param name="pInfo">Information of how the window is to be shown</param>
		internal static bool SetLayout(ProcessInfo pInfo)
		{
			try
			{
				Process p = Process.GetProcessById(pInfo.ProcessID);
				NativeMethods.WINDOWPLACEMENT wp = new NativeMethods.WINDOWPLACEMENT();
				wp.length = Marshal.SizeOf(wp);
				NativeMethods.GetWindowPlacement(p.MainWindowHandle, ref wp);
				switch (pInfo.WindowPlacement)
				{
					case 3:
						wp.showCmd = NativeMethods.ShowWindowCommands.SW_MAXIMIZE;
						break;
					case 2:
						wp.showCmd = NativeMethods.ShowWindowCommands.SW_SHOWMINIMIZED;
						break;
					default:
						wp.showCmd = NativeMethods.ShowWindowCommands.SW_SHOWNORMAL;
						break;
				}
				NativeMethods.SetWindowPlacement(pInfo.MainWindowHandle, ref wp);
				if (wp.showCmd == NativeMethods.ShowWindowCommands.SW_SHOWNORMAL)
					NativeMethods.MoveWindow(pInfo.MainWindowHandle, pInfo.XTopCoordinate, pInfo.YTopCoordinate, pInfo.Width, pInfo.Height, true);
			}
			catch (ArgumentException)
			{
				return false;
			}

			return true;
		}
	}
}
