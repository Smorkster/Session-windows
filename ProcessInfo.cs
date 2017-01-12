using System;
using System.Runtime.InteropServices;

namespace Session_windows
{
	/// <summary>
	/// Description of Process.
	/// </summary>
	public class ProcessInfo
	{
		int xCoordinate, yCoordinate;
		int height, width;
		int id;
		IntPtr handle;
		string processName, mainwindowtitle;
		int windowPlacement;

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

		public ProcessInfo(IntPtr phandle, int i, string pname, string ptitle, int left, int top, int width, int height, int placement)
		{
			id = i;
			handle = phandle;
			processName = pname;
			mainwindowtitle = ptitle;
			xCoordinate = left;
			yCoordinate = top;
			this.height = height;
			this.width = width;
			windowPlacement = placement;
		}

		// disable ConvertToAutoProperty
		public int ID { get { return id; } set { id = value; } }
		public int Xcoordinate { get { return xCoordinate; } set { xCoordinate = value; } }
		public int Ycoordinate { get { return yCoordinate; } set { yCoordinate = value; } }
		public IntPtr Handle { get { return handle; } }
		public int Height { get { return height; } set { height = value; } }
		public int Width { get { return width; } set { width = value; } }
		public string ProcessName { get { return processName; } set { processName = value; } }
		public string MainWindowTitle { get { return mainwindowtitle; } }
		public int Minimized { get { return windowPlacement; } set { windowPlacement = value; } }
	}
}
