using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using Microsoft.Win32;

namespace Session_windows
{

	static class Program
	{
		public static NotifyIcon icon;
		static int currentScreenWidth;
		static Settings settings = new Settings();
		static Form m = null;

		[STAThread]
		static void Main()
		{
			currentScreenWidth = Screen.AllScreens.Length;
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			SystemEvents.DisplaySettingsChanged += screensizeChanged;

			using (icon = new NotifyIcon()) {
				icon.Icon = new Icon(Resources.ICON, 32, 32);
				icon.ContextMenuStrip = new ContextMenuStrip();
				icon.DoubleClick += doubleClick;
				icon.Visible = true;
				readFile();
				icon.ContextMenuStrip.Items.Add("-");
				icon.ContextMenuStrip.Items.AddRange(new [] {
					new ToolStripMenuItem("Show form", null, menuShow_Click),
					new ToolStripMenuItem("Exit", null, closeApplication),
				});
				SystemEvents.DisplaySettingsChanged += screensizeChanged;
				if (settings != null) {
					if (!settings.StartInSysTray) {
						m = new MainForm(ref settings);
						m.Show();
					}
				} else {
					m = new MainForm(ref settings);
					m.Show();
				}
				Application.Run();
			}
		}

		/// <summary>
		/// Read XML-file, gather all sessions and add them as menuitems for the notifyicon
		/// </summary>
		static void readFile()
		{
			FileHandler fl = new FileHandler();

			if (File.Exists(@"H:\WindowSession - Test.xml")) {
				try {
					settings = fl.read();

					ToolStripMenuItem sessionMenu = new ToolStripMenuItem("Sessions >");

					if (settings != null) {
						for (int i = 0; i < settings.NumberOfSessions; i++) {
							sessionMenu.DropDownItems.Add(new ToolStripMenuItem(settings.getSession(i).SessionName, null, sessionSelected));
						}
						icon.ContextMenuStrip.Items.Add(sessionMenu);
					}
				} catch (XmlException e) {
					MessageBox.Show("Error while reading XML-file:\nAt: " + e.LineNumber + "\n\n" + e.Message, "");
				}
			} else {
				MessageBox.Show("WindowSession.xml was not found", "");
				
			}
		}

		/// <summary>
		/// Menuitem Show was clicked in the contextmenu of the systemtray.
		/// </summary>
		static void menuShow_Click(object sender, EventArgs e)
		{
			if (m != null)
				m.BringToFront();
			else {
				m = new MainForm(ref settings);
				m.Show();
			}
		}

		static void closeApplication(object sender, EventArgs e)
		{
			Program.icon.Dispose();
			Application.Exit();
		}

		/// <summary>
		/// A sessionname was clicked in the contextmenu
		/// Use the settings of that session
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		static void sessionSelected(object sender, EventArgs e)
		{
			string name = (sender as ToolStripMenuItem).Text;
			settings.getSession(name).useSession();
		}

		/// <summary>
		/// Systrayicon was doubleclicked, open form
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic Eventargs</param>
		static void doubleClick(object sender, EventArgs e)
		{
			if (m != null)
				m.BringToFront();
			else {
				m = new MainForm(ref settings);
				m.Show();
			}
		}

		/// <summary>
		/// The arrea of the screen have changed, probably from being docked/undocked
		/// Use the sessionsettings specified by the user for the layout
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic Eventargs</param>
		static void screensizeChanged(object sender, EventArgs e)
		{
			if (Screen.AllScreens.Length != currentScreenWidth) {
				if (Screen.AllScreens.Length == 1)
					settings.getSession(settings.Undocked).useSession();
				else
					settings.getSession(settings.Docked).useSession();
				currentScreenWidth = Screen.AllScreens.Length;
			}
		}
	}
}
