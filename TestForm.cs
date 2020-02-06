using System;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Windows.Forms;

namespace Session_windows
{
	internal class Test : IDisposable
	{
		// Definition of the delegates
		internal delegate void StartedEventHandler(object sender, EventArgs e);
		internal delegate void TerminatedEventHandler(object sender, EventArgs e);

		internal StartedEventHandler Started = null;
		internal TerminatedEventHandler Terminated = null;
		ManagementEventWatcher watcher;

		internal Test(string app)
		{
			string pol = "2";
			ManagementScope scope = new ManagementScope(@"root\CIMV2");
			EventQuery queryWatch = new EventQuery("SELECT *" +
				"  FROM __InstanceOperationEvent " +
				"WITHIN  " + pol +
				" WHERE TargetInstance ISA 'Win32_Process' " +
				"   AND TargetInstance.Name = '" + app + "'");

			// create the watcher and start to listen
			watcher = new ManagementEventWatcher(scope, queryWatch);
			watcher.EventArrived += new EventArrivedEventHandler(this.OnEventArrived);
			watcher.Start();
		}

		public void Dispose()
		{
			watcher.Dispose();
		}

		void OnEventArrived(object sender, EventArrivedEventArgs e)
		{
			string eventName = e.NewEvent.ClassPath.ClassName;
			if (eventName.CompareTo("__InstanceCreationEvent") == 0)
			{
				if (Started != null)
					Started(this, e);
			}
			else
			{
				if (Terminated != null)
					Terminated(this, e);
			}
		}

		void testGet(string app)
		{
			SelectQuery query = new SelectQuery("SELECT *" +
				"  FROM Win32_Process" +
				" where name = '" + app + "'");
			ManagementScope scope = new ManagementScope(@"root\CIMV2");
			ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);
			ManagementObjectCollection processes = searcher.Get();

			foreach (ManagementObject mo in processes)
			{
				if (mo["Name"].ToString().Contains(app))
				{
					int i = int.Parse(mo["ProcessId"].ToString());
				}
			}
		}
	}

	internal partial class TestForm : Form
	{
		string app = "notepad++.exe";
		internal TestForm()
		{
			InitializeComponent();
//			Process p = Process.GetProcessesByName("notepad++").Where(x => x.MainWindowHandle != IntPtr.Zero).First();
			Test t = new Test(app);
			t.Started += new Test.StartedEventHandler(teststart);
			t.Terminated += new Test.TerminatedEventHandler(testterminated);
		}

		void testterminated(object sender, EventArgs e)
		{
			if (textBox1.IsAccessible)
			{
				Action action = () => textBox1.Text += DateTime.Now.TimeOfDay + " - Terminated\r\n\t" + app + "\r\n";
				textBox1.BeginInvoke(action);
			}
		}

		void teststart(object sender, EventArgs e)
		{
			Action action = () => textBox1.Text += DateTime.Now.TimeOfDay + " - Started\r\n\t" + app + "\r\n";
			textBox1.BeginInvoke(action);
		}
	}
}
