using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Session_windows
{
	class Program
	{
		static Mutex mutex = new Mutex(true, "SW");
		[STAThread]
		static void Main()
		{
			// Use mutex to check if application is already running. If so, bring form to front
			if (mutex.WaitOne(TimeSpan.Zero, true))
			{
				Properties.Settings.Default.Test = false;
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
		/// Delegate to filter which windows to include
		/// </summary>
		/// <param name="hWnd">Generic IntPtr</param>
		/// <param name="lParam">Generic IntPtr</param>
		/// <returns>True to continue enumeration, false to stop</returns>
		delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);
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

			if (settings.Test)
			{
				trayicon.ContextMenuStrip.Items.Add(new ToolStripMenuItem("Application is in testmode") { BackColor = Color.Red, ForeColor = Color.White });
				trayicon.ContextMenuStrip.Items.Add(new ToolStripMenuItem("Test", null, MenuTestItem_Click));
				trayicon.ContextMenuStrip.Items.Add("-");
			}
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
			Dispose(true);
		}

		void Dispose(bool disposing)
		{
		}

		/// <summary>
		/// Enumerate open windows and return their z-order
		/// </summary>
		/// <param name="windowHandles">Filter for open windows</param>
		/// <returns>Z-ordered KeyValuePair-list [handle, z-index] of windows, ordered in decending z-order</returns>
		List<KeyValuePair<IntPtr, int>> EnumerateWindows(List<IntPtr> windowHandles)
		{
			List<KeyValuePair<IntPtr, int>> z = new List<KeyValuePair<IntPtr, int>>();
			List<IntPtr> listOfOpenWindows = new List<IntPtr>();

			List<int> t = new List<int>();
			var numRemaining = windowHandles.Count;
			NativeMethods.EnumWindows((wnd, param) =>
			{
				listOfOpenWindows.Add(wnd);
				return true;
			}, IntPtr.Zero);

			foreach (IntPtr handle in windowHandles)
			{
				int index = listOfOpenWindows.IndexOf(handle);
				z.Add(new KeyValuePair<IntPtr, int>(handle, index));
			}
			return z.OrderByDescending(x => x.Value).ToList();
		}

		/// <summary>
		/// List processes, saved in a sessions, sorted in z-order
		/// </summary>
		/// <returns>KeyValuePair list of z-ordered handles</returns>
		List<KeyValuePair<IntPtr, int>> GetWindowsZorder()
		{
			if (settings.ActiveSession != null && !settings.ActiveSession.Equals("current"))
			{
				List<IntPtr> handles = settings.GetSession(settings.ActiveSession).GetWindowHandles();
				List<KeyValuePair<IntPtr, int>> enumeratedWindows = EnumerateWindows(handles);
				return enumeratedWindows;
			}
			return null;
		}

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

				settings.ApplyActiveSession(GetWindowsZorder());
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
			settings.ApplyActiveSession(GetWindowsZorder());
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

		/// <summary>
		/// Menuitem for testing have been clicked
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		static void MenuTestItem_Click(object sender, EventArgs e)
		{
			new TestForm().ShowDialog();
		}
	}
}
