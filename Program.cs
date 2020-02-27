using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;
using Session_windows.Library;
using Session_windows.Models;

namespace Session_windows
{
	class Program
	{
		static readonly Mutex mutex = new Mutex(true, "SW");
		[STAThread]
		static void Main()
		{
			// Use mutex to check if application is already running. If so, bring form to front
			if (mutex.WaitOne(TimeSpan.Zero, true))
			{
				Properties.Settings.Default.Test = true;
				Properties.Settings.Default.SettingsFile = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\WindowSession.xml";
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);

				new ApplicationControls();
			}
			else
			{
				NativeMethods.PostMessage(
							   (IntPtr)NativeMethods.Hwnd_Broadcast,
							   NativeMethods.WM_SHOWME,
							   IntPtr.Zero,
							   IntPtr.Zero);
			}
			mutex.ReleaseMutex();
		}
	}

	class ApplicationControls : IDisposable
	{
		/// <summary>
		/// Holds the number of screens currently connected to the computer
		/// </summary>
		static int currentScreenWidth = Screen.AllScreens.Length;
		/// <summary>
		/// A formobject for the mainwindow
		/// </summary>
		Form mainform = null;
		/// <summary>
		/// The settingsobject for all information
		/// </summary>
		Settings settings;
		/// <summary>
		/// Trayicon-object
		/// </summary>
		internal static NotifyIcon trayicon;

		internal ApplicationControls()
		{
			SystemEvents.DisplaySettingsChanged += ScreensizeChanged;
			Application.ApplicationExit += ApplicationClosing;

			trayicon = new NotifyIcon
			{
				Icon = new Icon(Resources.ICON, 32, 32),
				ContextMenuStrip = new ContextMenuStrip(),
				Text = "No saved session loaded",
				Visible = true
			};
			trayicon.DoubleClick += Trayicon_DoubleClick;

			settings = new FileHandler().Read();

			if (settings.GetNumberOfSessions() > 0)
			{
				ToolStripMenuItem sessionsMenu = new ToolStripMenuItem("Apply session...");
				foreach (Session session in settings.GetSessionList())
				{
					sessionsMenu.DropDownItems.Add(new ToolStripMenuItem(session.SessionName, null, SessionSelected));
				}
				trayicon.ContextMenuStrip.Items.Add(sessionsMenu);
			}
			trayicon.ContextMenuStrip.Items.Add("-");
			trayicon.ContextMenuStrip.Items.AddRange(new[] {
					new ToolStripMenuItem("Show form", null, MenuShow_Click),
					new ToolStripMenuItem("Exit", null, MenuCloseApplication_Click),
				});

			if (!settings.StartInSysTray)
			{
				mainform = new MainForm(ref settings);
				mainform.Show();
			}

			Application.Run();
		}

		/// <summary>
		/// Eventhandler for when the application is closing
		/// Save to file and dispose of trayicon
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void ApplicationClosing(object sender, EventArgs e)
		{
			settings.SaveToFile();
			trayicon.Dispose();
		}

		/// <summary>
		/// Application is closing, dispose memory
		/// </summary>
		void IDisposable.Dispose()
		{
			Dispose();
		}

		void Dispose() { }

		/// <summary>
		/// Close the application from systemtray
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void MenuCloseApplication_Click(object sender, EventArgs e)
		{
			settings.AllowClosing = true;
			Application.Exit();
		}

		/// <summary>
		/// Menuitem Show was clicked in the contextmenu of the systemtray.
		/// </summary>
		void MenuShow_Click(object sender, EventArgs e)
		{
			ShowForm();
		}

		/// <summary>
		/// The arrea of the screen have changed, probably from being docked/undocked
		/// Use the sessionsettings specified by the user for the layout
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic Eventargs</param>
		void ScreensizeChanged(object sender, EventArgs e)
		{
			if (Screen.AllScreens.Length != currentScreenWidth)
			{
				if (Screen.AllScreens.Length == 1)
					settings.ActiveSession = settings.UndockedSession;
				else
					settings.ActiveSession = settings.DockedSession;

				settings.ApplyActiveSession();
				currentScreenWidth = Screen.AllScreens.Length;
			}
		}

		/// <summary>
		/// A sessionname was clicked in the contextmenu in systemtray
		/// Use the settings of that session
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void SessionSelected(object sender, EventArgs e)
		{
			settings.ActiveSession = (sender as ToolStripMenuItem).Text;
			settings.ApplyActiveSession();
		}

		/// <summary>
		/// Opens the mainform
		/// If mainform hasn't been loaded, create a new object
		/// </summary>
		void ShowForm()
		{
			if (mainform == null)
				mainform = new MainForm(ref settings);
			mainform.Show();
			mainform.WindowState = FormWindowState.Normal;
			mainform.Activate();
		}

		/// <summary>
		/// Systrayicon was doubleclicked, open form
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic Eventargs</param>
		void Trayicon_DoubleClick(object sender, EventArgs e)
		{
			ShowForm();
		}
	}
}
