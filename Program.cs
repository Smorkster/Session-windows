using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Session_windows
{

	sealed class Program
	{
		[STAThread]
		static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			
			using (NotifyIcon icon = new NotifyIcon())
			{

				icon.Icon = new Icon("../../icon.ico");
				icon.ContextMenu = new ContextMenu(new [] {
				                                   	//new MenuItem("Show form", (s, e) => {new MainForm().Show();}),
				                                   	new MenuItem("Show form", (s, e) => menuShow_Click()),
				                                   	new MenuItem("Exit", (s, e) => { Application.Exit(); }),
				                                   	});
				icon.DoubleClick += doubleClick;
				icon.Visible = true;
				Application.Run();
				icon.Visible = false;
			}
		}

		static List<Session> readFile()
		{
			FileHandler fl = new FileHandler();
			List<Session> ss = new List<Session>();
			ss = fl.read();

			return ss;
		}

		static void menuShow_Click()
		{
			Form m = new MainForm(readFile());
			m.Show();
		}

		static void doubleClick(object sender, EventArgs e)
		{
			Form m = new MainForm (readFile());
			m.Show();
		}
		static void screensizeChanged(object sender, EventArgs e)
		{
			throw new NotImplementedException();
		}
	}
}
