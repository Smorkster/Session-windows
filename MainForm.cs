using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Session_windows
{
	public partial class MainForm : Form
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetWindowRect(IntPtr hWnd, ref RECT Rect);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int Width, int Height, bool Repaint);

		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);
		
		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool SetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

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

		internal enum ShowWindowCommands : int
		{
			SW_SHOWNORMAL = 1, // Displayed
			SW_SHOWMINIMIZED = 2, // Minimized
			SW_MAXIMIZE = 3, // Maximized
		}

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        List<ProcessInfo> pList;
        List<Session> sList;
        IEnumerable<Process> processes;
        int MaxWindowSizeX = SystemInformation.VirtualScreen.Height, MaxWindowSizeY = SystemInformation.VirtualScreen.Width;
        ProcessInfo currentProcess;
        FileHandler fh;

        public MainForm(List<Session> l)
        {
			InitializeComponent();

			lbProcesses.SelectedIndexChanged -= lbProcesses_SelectedIndexChanged;
			fh = new FileHandler();
			//sList = fh.read();
			sList = l;
			populateSessions();
			lbProcesses.SelectedIndexChanged += lbProcesses_SelectedIndexChanged;
			
			
        }

        public void populate()
        {
        	if (pList == null)
        	{
	        	processes = Process.GetProcesses().Where(p => p.MainWindowHandle != IntPtr.Zero &&
	        	                                              p.ProcessName != "explorer" &&
	        	                                              p.MainWindowTitle != "" &&
	        	                                              p.ProcessName != "iexplore");
        		pList = new List<ProcessInfo>();
	            foreach (Process p in processes)
	            {
	                IntPtr handle = p.MainWindowHandle;
	                WINDOWPLACEMENT placement = new WINDOWPLACEMENT();
	                placement.length = Marshal.SizeOf(placement);
	                RECT Rect = new RECT();
	                if (GetWindowRect(handle, ref Rect))
	                {
	                	GetWindowPlacement(p.Handle, ref placement);
	                	IntPtr h = p.Handle;
	                	string pn = p.ProcessName;
	                	string mwt;
						mwt = p.MainWindowTitle.Equals("") ? "Process: " + p.ProcessName : p.MainWindowTitle;
	                	int i = p.Id;
	                	int l = Rect.left;
	                	int t = Rect.top;
	                	int r = Rect.right;
	                	int b = Rect.bottom;
	                	int pl = (int) placement.showCmd;
	                	pList.Add(new ProcessInfo(h, i, pn, mwt, l, t, r, b, pl));
	                	lbProcesses.Items.Add(p.MainWindowTitle);
	                }
	            }
            } else
        	{
        		foreach(ProcessInfo item in pList)
        		{
        			lbProcesses.Items.Add(item.MainWindowTitle);
        		}
        	}
        }

		void populateSessions()
		{
			if(sList != null)
			{
				foreach(Session s in sList)
				{
					lbSessions.Items.Add(s.SessionName);
				}
			}
		}
        void on()
	    {
			txtX.TextChanged += txtX_TextChanged;
			txtY.TextChanged += txtY_TextChanged;
			txtWidth.TextChanged += txtWidth_TextChanged;
			txtHeight.TextChanged += txtHeight_TextChanged;
			cbMinimized.SelectedIndexChanged += cbMinimized_SelectedIndexChanged;
        }
        void off()
        {
			txtX.TextChanged -= txtX_TextChanged;
			txtY.TextChanged -= txtY_TextChanged;
			txtWidth.TextChanged -= txtWidth_TextChanged;
			txtHeight.TextChanged -= txtHeight_TextChanged;
			cbMinimized.SelectedIndexChanged -= cbMinimized_SelectedIndexChanged;
        }

		void clearTextBoxes()
		{
			txtHeight.Text = "";
			txtWidth.Text = "";
			txtX.Text = "";
			txtY.Text = "";
			txtProcess.Text = "";
			txtId.Text = "";
			cbMinimized.SelectedIndex = -1;
			lbProcesses.Items.Clear();
		}

		void redrawWindows(ProcessInfo process)
		{
			Process p = Process.GetProcessById(process.ID);

			if(currentProcess != null)
			{
				try
				{
					txtId.Text = p.Id.ToString();
					int h = p.Id;
					MoveWindow(p.MainWindowHandle, process.Xcoordinate, process.Ycoordinate, process.Width, process.Height, false);

					WINDOWPLACEMENT wp = new WINDOWPLACEMENT();
					wp.length = Marshal.SizeOf(wp);
					GetWindowPlacement(p.MainWindowHandle, ref wp);
					switch(cbMinimized.SelectedIndex)
					{
						case 0:
							wp.showCmd = ShowWindowCommands.SW_SHOWNORMAL;
							break;
						case 1:
							wp.showCmd = ShowWindowCommands.SW_SHOWMINIMIZED;
							break;
						case 2:
							wp.showCmd = ShowWindowCommands.SW_MAXIMIZE;
							break;
					}

					SetWindowPlacement(p.MainWindowHandle, ref wp);
				} catch (ArgumentException err)
				{
					MessageBox.Show("No process with id " + process.ID + " is running.");
				}
			}
		}
		void btnGetProcesses_Click(object sender, EventArgs e)
		{
			off();
			clearTextBoxes();
			pList = null;
			currentProcess = null;
			populate();
			on();
		}

		void btnSetProcessInfo_Click(object sender, EventArgs e)
		{
			int h = int.Parse(txtHeight.Text);
			int	w = int.Parse(txtWidth.Text);
			int xc = int.Parse(txtX.Text);
			int yc = int.Parse(txtY.Text);
			int m = cbMinimized.SelectedIndex-1;
			string pn = txtProcess.Text;

			currentProcess.Height = h;
			currentProcess.Width = w;
			currentProcess.ProcessName = pn;
			currentProcess.Xcoordinate = xc;
			currentProcess.Ycoordinate = yc;
			currentProcess.Minimized = m;

			if(sList != null)
			{
				int si = sList.FindIndex(x => x.SessionName.Equals(lbSessions.SelectedItem));
				int pi = sList[si].Plist.FindIndex(x => x.ID == int.Parse(txtId.Text));
				sList[si].Plist[pi].Height = currentProcess.Height;
				sList[si].Plist[pi].Width = currentProcess.Width;
				sList[si].Plist[pi].ProcessName = currentProcess.ProcessName;
				sList[si].Plist[pi].Xcoordinate = currentProcess.Xcoordinate;
				sList[si].Plist[pi].Ycoordinate = currentProcess.Ycoordinate;
				sList[si].Plist[pi].Minimized = currentProcess.Minimized;
			} else {
				pList[pList.FindIndex(x => x.ID == currentProcess.ID)].Height = currentProcess.Height;
				pList[pList.FindIndex(x => x.ID == currentProcess.ID)].Width = currentProcess.Width;
				pList[pList.FindIndex(x => x.ID == currentProcess.ID)].ProcessName = currentProcess.ProcessName;
				pList[pList.FindIndex(x => x.ID == currentProcess.ID)].Xcoordinate = currentProcess.Xcoordinate;
				pList[pList.FindIndex(x => x.ID == currentProcess.ID)].Minimized = currentProcess.Minimized;
				pList[pList.FindIndex(x => x.ID == currentProcess.ID)].Ycoordinate = currentProcess.Ycoordinate;
			}
			redrawWindows(currentProcess);
		}

		void lbProcesses_SelectedIndexChanged(object sender, EventArgs e)
		{
			if(lbProcesses.SelectedIndex == -1)
			{
				currentProcess = null;
			} else if (!lbProcesses.SelectedItem.ToString().StartsWith("   "))
			{
				ttInfo.Active = false;
				currentProcess = pList.Find(x => x.MainWindowTitle.Equals(lbProcesses.SelectedItem.ToString()));
        		off();
        		txtX.Text = currentProcess.Xcoordinate.ToString();
        		txtY.Text = currentProcess.Ycoordinate.ToString();
        		txtHeight.Text = currentProcess.Height.ToString();
        		txtWidth.Text = currentProcess.Width.ToString();
        		txtProcess.Text = currentProcess.ProcessName;
        		txtId.Text = currentProcess.ID.ToString();
        		cbMinimized.SelectedIndex = -1;
        		on();

			}
		}

		void lbProcesses_MouseDown(object sender, MouseEventArgs e)
		{
			int index = lbProcesses.IndexFromPoint(e.Location);
			
			if(index == -1)
			{
				lbProcesses.SelectedIndex = -1;
				off();
				clearTextBoxes();
				currentProcess = null;
			}
		}
		
		void txtX_TextChanged(object sender, EventArgs e)
		{
			if(currentProcess != null)
			{
				if(txtX.Text.Length == 0)
					txtX.Text = "0";
				if(int.Parse(txtX.Text) > SystemInformation.VirtualScreen.Width)
					txtX.Text = string.Format("{0}",SystemInformation.VirtualScreen.Width - 20);
				currentProcess.Xcoordinate = int.Parse(txtX.Text);
			}
		}
		void txtY_TextChanged(object sender, EventArgs e)
		{
			if(currentProcess != null)
			{
				if(txtY.Text.Length == 0)
					txtY.Text = "0";
				if(int.Parse(txtY.Text) > SystemInformation.VirtualScreen.Height)
					txtY.Text = string.Format("{0}",SystemInformation.VirtualScreen.Height - 20);
				currentProcess.Ycoordinate = int.Parse(txtY.Text);
			}
		}
		void txtWidth_TextChanged(object sender, EventArgs e)
		{
			if(currentProcess != null)
			{
				if(txtWidth.Text.Length == 0 || int.Parse(txtWidth.Text) == 0)
				{
					txtWidth.Text = currentProcess.Width.ToString();
					ttInfo.Show("Window can't be 0 pixels wide", txtWidth, 0, 18);
				} else {
					currentProcess.Width = int.Parse(txtWidth.Text);
				}
			}
		}
		void txtHeight_TextChanged(object sender, EventArgs e)
		{
			if(currentProcess != null)
			{
				if(txtHeight.Text.Length == 0)
				{
					txtHeight.Text = currentProcess.Height.ToString();
					ttInfo.Show("Window can't be 0 pixels high", txtHeight, 0, 18);
				} else {
					currentProcess.Height = int.Parse(txtHeight.Text);
				}
			}
		}
		void btnSave_Click(object sender, EventArgs e)
		{
			cmenuSave.Show(btnSave, 80, 23);
		}
		void btnSetSettings_Click(object sender, EventArgs e)
		{
			foreach(ProcessInfo p in pList) {
				redrawWindows(p);
			}
		}
		void cmenuNew_Click(object sender, EventArgs e)
		{
			SessionName sn = new SessionName();

			sn.ShowDialog();
			if(sn.DialogResult == DialogResult.OK)
			{
				if(sList == null)
					sList = new List<Session>();
				sList.Add(new Session(sn.getName(), pList));
				lbSessions.Items.Add(sn.getName());
			}
			fh.write(sList);
		}
		void cmenuMarked_Click(object sender, EventArgs e)
		{
			Session sn;
			SelectSession nsm = new SelectSession(sList);
			nsm.ShowDialog();
			
			if(lbSessions.SelectedIndex == -1)
				sn = nsm.getSession();
			else
				sn = sList.Find(x => x.SessionName.Equals(lbSessions.SelectedItem.ToString()));
			int i = sList.IndexOf(sn);
			sList[i].Plist = pList;
			fh.write(sList);
		}
		void cbMinimized_SelectedIndexChanged(object sender, EventArgs e)
		{
			currentProcess.Minimized = cbMinimized.SelectedIndex;
		}
		void lbSessions_SelectedIndexChanged(object sender, EventArgs e)
		{
			Session s;

			if(lbSessions.SelectedIndex != -1)
			{
				s = new Session(sList.Find(x => x.SessionName.Equals(lbSessions.SelectedItem.ToString())));
				clearTextBoxes();
				pList = s.Plist;
				currentProcess = null;
				populate();
			}
		}
		void cbMinimized_Click(object sender, EventArgs e)
		{
			cbMinimized.DroppedDown = true;
		}
    }
}
