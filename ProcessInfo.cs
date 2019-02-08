using System;
using System.Management;
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
		// WMI event watcher
		ManagementEventWatcher watcher;
		
		public int ProcessID { get { return id; } set { id = value; } }
		public int XTopCoordinate { get { return xTopCoordinate; } set { xTopCoordinate = value; } }
		public int YTopCoordinate { get { return yTopCoordinate; } set { yTopCoordinate = value; } }
		public IntPtr WindowHandle { get { return handle; } }
		public int Height { get { return height; } set { height = value; } }
		public int Width { get { return width; } set { width = value; } }
		public string ProcessName { get { return processName; } set { processName = value; } }
		public int WindowPlacement { get { return windowPlacement; } set { windowPlacement = value; } }
		public StartingEventHandler Started = null;
		public TerminatingEventHandler Terminated = null;

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

		/// <summary>
		/// Create a WMI-eventwatcher to monitor if the process have started or terminated
		/// </summary>
		public void WatchProcess()
		{
			string scope = @"\\.\root\CIMV2";
			string pol = "1";
			string queryString = 
				"SELECT *" +
				"  FROM __InstanceOperationEvent " +
				"WITHIN  " + pol +
				" WHERE TargetInstance ISA 'Win32_Process' " +
				"   AND TargetInstance.Name = '" + processName + ".exe'";

			// create the watcher and start to listen
			watcher = new ManagementEventWatcher(scope, queryString);
			watcher.EventArrived += OnEventArrived;			
			watcher.Start();
		}

		/// <summary>
		/// Dispose of this process' eventwatcher
		/// </summary>
		public void Dispose()
		{
			if (watcher != null) {
				watcher.Stop();
				watcher.Dispose();
			}
		}

		/// <summary>
		/// An managementevent have occured, check if a process have started or terminated
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArrivedEventArgs</param>
		void OnEventArrived(object sender, EventArrivedEventArgs e)
		{
			try {
				string eventName = e.NewEvent.ClassPath.ClassName;

				if (eventName.CompareTo("__InstanceCreationEvent") == 0) {
					// Process started
					if (Started != null) {
				        ManagementBaseObject targetInstance = (ManagementBaseObject)e.NewEvent.Properties["TargetInstance"].Value;
						ProcessID = int.Parse(targetInstance.Properties["ProcessID"].Value.ToString());
						Started(this);
					}
				} else if (eventName.CompareTo("__InstanceDeletionEvent") == 0) {
					// Process terminated
					if (Terminated != null)
						Terminated(this);
				}				
			} catch (Exception ex) {
				System.Diagnostics.Debug.WriteLine(ex.Message);
			}
		}
	}
}
