using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace Session_windows.Models
{
	/// <summary>
	/// Description of Process.
	/// </summary>
	internal class ProcessInfo
	{
		/// <summary>
		/// Height of the mainwindow
		/// </summary>
		internal int Height { get; set; }
		/// <summary>
		/// Handle of the mainwindow for the process
		/// </summary>
		internal IntPtr MainWindowHandle { get; set; }
		/// <summary>
		/// Title of the mainwindow
		/// </summary>
		internal string MainWindowTitle { get; set; }
		/// <summary>
		/// Unique id of the process
		/// </summary>
		internal int ProcessID { get; set; }
		/// <summary>
		/// Name of the process
		/// </summary>
		internal string ProcessName { get; set; }
		/// <summary>
		/// Width of the mainwindow
		/// </summary>
		internal int Width { get; set; }
		/// <summary>
		/// Windowplacement of the mainwindow
		/// This is represented by an int that is interpreted with showCmd in windowplacement-enumeration
		/// </summary>
		internal int WindowPlacement { get; set; }
		/// <summary>
		/// X-coordinate of the bottomright corner
		/// </summary>
		readonly int XBottomCoordinate;
		/// <summary>
		/// Y-coordinate of the bottomright corner
		/// </summary>
		readonly int YBottomCoordinate;
		/// <summary>
		/// X-coordinate of the topleft corner of the mainwindow
		/// </summary>
		internal int XTopCoordinate { get; set; }
		/// <summary>
		/// Y-coordinate of the topleft corner of the mainwindow
		/// </summary>
		internal int YTopCoordinate { get; set; }

		internal ProcessInfo() { }

		/// <summary>
		/// Construct to create an ProcessInfo-object from an temporary Process-object
		/// </summary>
		/// <param name="p">Process to get information from</param>
		internal ProcessInfo(Process p)
		{
			MainWindowHandle = p.MainWindowHandle;
			NativeMethods.WINDOWPLACEMENT placement = new NativeMethods.WINDOWPLACEMENT();
			placement.length = Marshal.SizeOf(placement);
			NativeMethods.RECT Rect = new NativeMethods.RECT();

			if (NativeMethods.GetWindowRect(MainWindowHandle, ref Rect))
			{
				NativeMethods.GetWindowPlacement(MainWindowHandle, ref placement);
				WindowPlacement = (int)placement.showCmd;
				IntPtr mainWindowHandle = MainWindowHandle;

				ProcessName = p.ProcessName;
				MainWindowTitle = p.MainWindowTitle.Equals("") ? "Process: " + p.ProcessName : p.MainWindowTitle;
				ProcessID = p.Id;
				XTopCoordinate = Rect.left;
				YTopCoordinate = Rect.top;
				XBottomCoordinate = Rect.right;
				YBottomCoordinate = Rect.bottom;
				Width = XBottomCoordinate - XTopCoordinate;
				Height = YBottomCoordinate - YTopCoordinate;
			}
		}

		/// <summary>
		/// Create a ProcessInfo-object from a temporary ProcessInfo-object
		/// </summary>
		/// <param name="p"></param>
		internal ProcessInfo(ProcessInfo p)
		{
			ProcessID = p.ProcessID;
			MainWindowHandle = p.MainWindowHandle;
			ProcessName = p.ProcessName;
			XTopCoordinate = p.XTopCoordinate;
			YTopCoordinate = p.YTopCoordinate;
			Height = p.Height;
			Width = p.Width;
			WindowPlacement = p.WindowPlacement;
		}

		/// <summary>
		/// Construct to create an object with all needed information
		/// </summary>
		/// <param name="mainwindowhandle">Handle of the mainwindow</param>
		/// <param name="processId">Unique id of the process</param>
		/// <param name="processname">Name of the process</param>
		/// <param name="xTop">X-coordinate of the topleft corner</param>
		/// <param name="yTop">Y-coordinate of the topleft corner</param>
		/// <param name="width">Width of mainwindow</param>
		/// <param name="height">Height of mainwindow</param>
		/// <param name="placement">Windowplacement of mainwindow</param>
		internal ProcessInfo(IntPtr mainwindowhandle, int processId, string processname, int xTop, int yTop, int width, int height, int placement)
		{
			ProcessID = processId;
			MainWindowHandle = mainwindowhandle;
			ProcessName = processname;
			XTopCoordinate = xTop;
			YTopCoordinate = yTop;
			Height = height;
			Width = width;
			WindowPlacement = placement;
		}

		/// <summary>
		/// Updates the MainWindowHandle for this ProcessInfo
		/// </summary>
		internal void UpdateMainWindowHandle()
		{
			Process[] pList = Process.GetProcessesByName(ProcessName);
			if (pList.Length > 0)
			{
				MainWindowHandle = pList.Where(x => x.MainWindowHandle != IntPtr.Zero).First().MainWindowHandle;
			}
		}
	}
}
