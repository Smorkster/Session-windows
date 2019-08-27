using System;
using System.Runtime.InteropServices;

namespace Session_windows
{
	/// <summary>
	/// Description of Process.
	/// </summary>
	public class ProcessInfo
	{
		public delegate void StartingEventHandler(ProcessInfo p);
		public delegate void TerminatingEventHandler(ProcessInfo p);
		
		public int ProcessID { get { return id; } set { id = value; } }
		public int XTopCoordinate { get { return xTopCoordinate; } set { xTopCoordinate = value; } }
		public int YTopCoordinate { get { return yTopCoordinate; } set { yTopCoordinate = value; } }
		public IntPtr WindowHandle { get { return handle; } }
		public int Height { get { return height; } set { height = value; } }
		public int Width { get { return width; } set { width = value; } }
		public string ProcessName { get { return processName; } set { processName = value; } }
		public int WindowPlacement { get { return windowPlacement; } set { windowPlacement = value; } }

		int xTopCoordinate, yTopCoordinate;
		int height, width;
		int id;
		int windowPlacement;
		IntPtr handle;
		string processName;

		/// <summary>
		/// Struct for windowlayout coordinates
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		public struct RECT
		{
			public int left;
			public int top;
			public int right;
			public int bottom;
		}

		public ProcessInfo()
		{
		}

		public ProcessInfo(IntPtr phandle, int i, string pname, int xTop, int yTop, int width, int height, int placement)
		{
			id = i;
			handle = phandle;
			processName = pname;
			xTopCoordinate = xTop;
			yTopCoordinate = yTop;
			this.height = height;
			this.width = width;
			windowPlacement = placement;
		}
	}
}
