using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Session_windows
{

	static class Program
	{
		static NotifyIcon icon;
		static List<Session> sessions;

		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			using (icon = new NotifyIcon()) {
				icon.Icon = new Icon(Resources.ICON, 32, 32);
				icon.ContextMenuStrip = new ContextMenuStrip();
				icon.DoubleClick += doubleClick;
				icon.Visible = true;
				readFile();
				icon.ContextMenuStrip.Items.Add("-");
				icon.ContextMenuStrip.Items.AddRange(new [] {
					new ToolStripMenuItem("Show form", null, menuShow_Click),
					new ToolStripMenuItem("Exit", null, (s, e) => Application.Exit()),
				});
				SystemEvents.DisplaySettingsChanged += screensizeChanged;
				Application.Run();
				icon.Visible = false;
			}
		}

		/// <summary>
		/// Read XML-file, gather all sessions and return as list
		/// </summary>
		/// <returns>List of all sessions saved</returns>
		static List<Session> readFile()
		{
			FileHandler fl = new FileHandler();
			sessions = new List<Session>();
			sessions = fl.read();
			ToolStripMenuItem sessionMenu = new ToolStripMenuItem("Sessions >");

			for (int i = 0; i < sessions.Count; i++) {
				sessionMenu.DropDownItems.Add(new ToolStripMenuItem(sessions[i].SessionName, null, sessionSelected));
			}
			icon.ContextMenuStrip.Items.Add(sessionMenu);

			return sessions;
		}

		/// <summary>
		/// Menuitem Show was clicked in the contextmenu of the systemtray.
		/// </summary>
		static void menuShow_Click(object sender, EventArgs e)
		{
			Form m = new MainForm(sessions);
			m.Show();
		}

		/// <summary>
		/// A sessionname was clicked in the contextmenu
		/// Use the windowsettings of that session
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		static void sessionSelected(object sender, EventArgs e)
		{
			string name = (sender as ToolStripMenuItem).Text;
			Session session = sessions.Find(x => x.SessionName.Equals(name));

			foreach (ProcessInfo pi in session.Plist) {
				new WindowLayout().setLayout(pi);
			}
		}

		/// <summary>
		/// Systrayicon was doubleclicked, open form
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic Eventargs</param>
		static void doubleClick(object sender, EventArgs e)
		{
			Form m = new MainForm(sessions);
			m.Show();
		}

		/// <summary>
		/// The arrea of the screen have changed, probably from being docked/undocked
		/// Use the sessionsettings specified by the user for the layout
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic Eventargs</param>
		static void screensizeChanged(object sender, EventArgs e)
		{
			FileHandler fl = new FileHandler();
			int dockedStatus;
			Session dockedSession;

			dockedStatus = Screen.AllScreens.Length == 1 ? 1 : 0;
			dockedSession = sessions.Find(x => x.SessionName.Equals(fl.getDockedSessions()[dockedStatus]));
			foreach (ProcessInfo process in dockedSession.Plist) {
				new WindowLayout().setLayout(process);
			}
		}
	}
}
