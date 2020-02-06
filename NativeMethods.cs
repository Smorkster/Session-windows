using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Session_windows
{
	internal static class NativeMethods
	{
		/// <summary>
		/// Broadcast message to all top-level windows
		/// </summary>
		internal const int Hwnd_Broadcast = 0xffff;

		/// <summary>
		/// Message to show mainform
		/// </summary>
		internal static readonly int WM_SHOWME = RegisterWindowMessage("WM_SHOWME");

		/// <summary>
		/// Places a message in the message queue associated with the thread that created the specified window
		/// </summary>
		/// <param name="hwnd">Handle to the window whose window procedure is to receive the message</param>
		/// <param name="msg">The message to be posted</param>
		/// <param name="wparam">Additional message-specific information</param>
		/// <param name="lparam">Additional message-specific information</param>
		/// <returns>If the function succeeds, the return value is nonzero</returns>
		[DllImport("user32")]
		internal static extern bool PostMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);

		/// <summary>
		/// Defines a window message, guaranteed to be unique throughout the system
		/// </summary>
		/// <param name="message">Message to be sent</param>
		/// <returns>If successfully registered, the return value is a message identifier in the range 0xC000 through 0xFFFF. If function fails, return value is zero</returns>
		[DllImport("user32", CharSet = CharSet.Unicode)]
		internal static extern int RegisterWindowMessage(string message);
		
		/// <summary>
		/// Gets the dimensions of the bounding rectangle of the specified window
		/// </summary>
		/// <param name="hWnd">Handle of the window</param>
		/// <param name="Rect">Pointer to a RECT structure that receives the screen coordinates of the upper-left and lower-right corners of the window</param>
		/// <returns>Nonzero if function succeeds, zero if function fails</returns>
		[DllImport("user32.dll", SetLastError = true)]
		internal static extern bool GetWindowRect(IntPtr hWnd, ref RECT Rect);

		/// <summary>
		/// Gets the show state and the restored, minimized, and maximized positions of the specified window
		/// </summary>
		/// <param name="hWnd">Handle to the window</param>
		/// <param name="lpwndpl">Pointer to the WINDOWPLACEMENT structure that receives the show state and position information</param>
		/// <returns>Nonzero if function succeeds, zero if function fails</returns>
		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

		/// <summary>
		/// Enumerates all top-level windows on the screen by passing the handle to each window, in turn, to an application-defined callback function
		/// </summary>
		/// <param name="enumFunc">Pointer to an application-defined callback function</param>
		/// <param name="lParam">An application-defined value to be passed to the callback function.</param>
		/// <returns>Nonzero if function succeeds. Zero if function fails</returns>
		[DllImport("user32.dll")]
		internal static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);

		/// <summary>
		/// Moves and sizes an open windowform
		/// </summary>
		/// <param name="hWnd">Handle for the window to operate on</param>
		/// <param name="X">New X-coordinate of top left corner</param>
		/// <param name="Y">New Y-coordinate of top left corner</param>
		/// <param name="Width">New width of window</param>
		/// <param name="Height">New height of window</param>
		/// <param name="Repaint">Whether the window is to be repainted. If TRUE, the window receives a message. If FALSE, no repainting of any kind occurs. Applies to client area, nonclient area and any part of the parent window uncovered as a result of moving a child window.</param>
		/// <returns></returns>
		[DllImport("user32.dll", SetLastError = true)]
		internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int Width, int Height, bool Repaint);

		/// <summary>
		/// Sets the show state and the restored, minimized, and maximized positions of the specified window.
		/// </summary>
		/// <param name="hWnd">Handle to window</param>
		/// <param name="lpwndpl">Pointer to WINDOWPLACEMENT structure, specifying the new show state and window positions</param>
		/// <returns>Nonzero if function succeeds. Zero if function fails</returns>
		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool SetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

		/// <summary>
		/// Receives top-level window handles
		/// This is an application-defined callback function
		/// </summary>
		/// <param name="hWnd">Handle to top-level window</param>
		/// <param name="lParam">Application-defined value given in EnumWindows or EnumDesktopWindows</param>
		/// <returns>To continue enumeration, the callback function must return TRUE; to stop enumeration, it must return FALSE.</returns>
		internal delegate bool EnumWindowsProc(IntPtr hWnd, int lParam);

		/// <summary>
		/// Struct for a windows placement
		/// </summary>
		[Serializable]
		[StructLayout(LayoutKind.Sequential)]
		internal struct WINDOWPLACEMENT
		{
			internal int length;
			internal int flags;
			internal ShowWindowCommands showCmd;
			internal Point ptMinPosition;
			internal Point ptMaxPosition;
			internal Rectangle rcNormalPosition;
		}

		/// <summary>
		/// Struct for windowcoordinates
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		internal struct RECT
		{
			internal int left;
			internal int top;
			internal int right;
			internal int bottom;
			internal int placement;
			internal string placementName;
			internal string placementComment;
		}

		/// <summary>
		/// Enumeration of available windowcommands for WINDOWPLACEMENT
		/// </summary>
		internal enum ShowWindowCommands
		{
			/// <summary>
			/// Activates and displays the window. If window is minimized or
			/// maximized, the system restores it to its original size and position.
			/// An application should specify this flag when displaying the window
			/// for the first time.
			/// </summary>
			SW_SHOWNORMAL = 1,
			/// <summary>
			/// Activates the window and displays it as a minimized window
			/// </summary>
			SW_SHOWMINIMIZED = 2,
			/// <summary>
			/// Maximizes the specified window
			/// </summary>
			SW_MAXIMIZE = 3,
		}
	}
}
