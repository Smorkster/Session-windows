using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Session_windows
{
	/// <summary>
	/// 
	/// </summary>
	public class WindowLayout
	{
		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool GetWindowPlacement (IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

		[DllImport("user32.dll", SetLastError = true)]
		static extern bool MoveWindow (IntPtr hWnd, int X, int Y, int Width, int Height, bool Repaint);

		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool SetWindowPlacement (IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

		private delegate bool EnumWindowsProc(IntPtr hWnd, int lParam);

		[DllImport("USER32.DLL")]
		static extern bool EnumWindows (EnumWindowsProc enumFunc, int lParam);

		[DllImport("USER32.DLL")]
		static extern int GetWindowText (IntPtr hWnd, StringBuilder lpString, int nMaxCount);
		[DllImport("user32.dll", SetLastError = true)]
		static extern uint GetWindowThreadProcessId (IntPtr hWnd, out uint processId);

		[DllImport("USER32.DLL")]
		static extern int GetWindowTextLength (IntPtr hWnd);
	
		[DllImport("USER32.DLL")]
		static extern bool IsWindowVisible (IntPtr hWnd);

		[DllImport("USER32.DLL")]
		static extern IntPtr GetShellWindow ();

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
		/// Take the processinfo specified by the user,
		/// find all open windows of the same processname,
		/// apply the settings from the processinfo
		/// </summary>
		/// <param name="process">Information of how the window is to be shown</param>
		public void setLayout (ProcessInfo process)
		{
			try
			{
				foreach (KeyValuePair<IntPtr, string> window in GetOpenWindows())
				{
					IntPtr handle = window.Key;
					uint id;
					string title = window.Value;

					GetWindowThreadProcessId(handle, out id);
					Process p = Process.GetProcessById((int)id);
					if (p.ProcessName == process.ProcessName)
					{
						MoveWindow(handle, process.Xcoordinate, process.Ycoordinate, process.Width, process.Height, true);

						WINDOWPLACEMENT wp = new WINDOWPLACEMENT();
						wp.length = Marshal.SizeOf(wp);
						GetWindowPlacement(p.MainWindowHandle, ref wp);
						switch (process.Minimized)
						{
							case 3:
								wp.showCmd = ShowWindowCommands.SW_MAXIMIZE;
								break;
							case 2:
								wp.showCmd = ShowWindowCommands.SW_SHOWMINIMIZED;
								break;
							default:
								wp.showCmd = ShowWindowCommands.SW_SHOWNORMAL;
								break;
						}
						SetWindowPlacement(handle, ref wp);
					}
				}
			} catch (ArgumentException e)
			{
				MessageBox.Show("No process with id " + process.ID + " is running.\n" + e.Message);
			}
		}

		/// <summary>
		/// Get a key/value pair dictionary of all the currently open windows
		/// </summary>
		/// <returns>Key/value dictionary of the open windows</returns>
		public static IDictionary<IntPtr, string> GetOpenWindows ()
		{
			IntPtr shellWindow = GetShellWindow();
			Dictionary<IntPtr, string> windows = new Dictionary<IntPtr, string>();

			EnumWindows(delegate(IntPtr hWnd, int lParam)
			{
				if (hWnd == shellWindow)
					return true;
				if (!IsWindowVisible(hWnd))
					return true;

				int length = GetWindowTextLength(hWnd);
				if (length == 0)
					return true;

				StringBuilder builder = new StringBuilder(length);
				GetWindowText(hWnd, builder, length + 1);

				windows[hWnd] = builder.ToString();
				return true;

			}, 0);

			return windows;
		}
	}
}
