using Session_windows.Library;
using Session_windows.Models;
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Session_windows
{
	/// <summary>
	/// Class for setting the layout of a windowform 
	/// </summary>
	public static class WindowLayout
	{
		/// <summary>
		/// Is there an open window with provided handle
		/// </summary>
		/// <param name="handle">Windowhandle to verify if window is open</param>
		/// <returns>True if window is open, else false</returns>
		static bool IsOpenWindow(IntPtr handle)
		{
			IntPtr open = IntPtr.Zero;

			NativeMethods.EnumWindows(delegate (IntPtr hWnd, int lParam)
			{
				if (hWnd == handle)
				{ open = handle; return false; }
				return true;
			}, IntPtr.Zero);

			if (open == IntPtr.Zero)
				return false;
			return true;
		}

		/// <summary>
		/// Take the processinfo specified by the user,
		/// find all open windows of the same processname,
		/// apply the settings from the processinfo
		/// </summary>
		/// <param name="pInfo">Information of how the window is to be shown</param>
		internal static void SetLayout(ProcessInfo pInfo)
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
			catch (ArgumentException e)
			{
				MessageBox.Show("No process with id " + pInfo.ProcessID + " is running.\n" + e.Message);
			}
		}
	}
}
