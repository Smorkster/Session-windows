using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Session_windows
{
	/// <summary>
	/// Description of Settings.
	/// </summary>
	internal class Settings
	{
		/// <summary>
		/// List of saved sessions
		/// </summary>
		List<Session> sessions;
		/// <summary>
		/// A list of applications that are ignored
		/// </summary>
		List<string> excludedApps;
		/// <summary>
		/// Holds a list of currently running processes
		/// This list is not saved in any session
		/// </summary>
		internal Session currentlyRunningProcesses;

		/// <summary>
		/// Basic onstruct for settings.
		/// Creates an empty settings-object.
		/// </summary>
		internal Settings()
		{
			Properties.Settings.Default.ActiveSession = "current";
			DockedUndocked = new string[2];
			StartInSysTray = false;

			currentlyRunningProcesses = new Session("current", new List<ProcessInfo>());
			excludedApps = new List<string>();
			SessionNamesAutoComplete = new AutoCompleteStringCollection();
			sessions = new List<Session>();
		}

		/// <summary>
		/// Should the application start in systray or with the form open
		/// </summary>
		internal bool StartInSysTray { get; set; }

		/// <summary>
		/// String for which session to be used when docked
		/// </summary>
		internal string DockedSession { get { return DockedUndocked[0]; } set { DockedUndocked[0] = value; } }

		/// <summary>
		/// String for which session to be used when undocked
		/// </summary>
		internal string UndockedSession { get { return DockedUndocked[1]; } set { DockedUndocked[1] = value; } }

		/// <summary>
		/// Returns number of saved sessions
		/// </summary>
		internal int NumberOfSessions { get { return sessions.Count; } }

		/// <summary>
		/// Holds which session is to be used when computer is docked vs undocked
		/// </summary>
		internal string[] DockedUndocked { get; set; }

		/// <summary>
		/// Returns a list of the saved session names
		/// </summary>
		internal AutoCompleteStringCollection SessionNamesAutoComplete { get; }

		/// <summary>
		/// Name and address of where to read/write the settings
		/// Saved in application properties
		/// </summary>
		internal FileInfo SettingsFile { get { return new FileInfo(Properties.Settings.Default.SettingsFile); } set { Properties.Settings.Default.SettingsFile = value.FullName; } }

		/// <summary>
		/// Allow closing of application
		/// </summary>
		internal bool AllowClosing { get { return Properties.Settings.Default.AllowClosing; } set { Properties.Settings.Default.AllowClosing = value; } }

		/// <summary>
		/// Evenhandler delegate for changes in applicationproperty ActiveSession
		/// </summary>
		internal delegate void ActiveSessionHandler();

		/// <summary>
		/// Eventhandler to handle changes in applicationproperty ActiveSession
		/// </summary>
		internal event ActiveSessionHandler ActiveSession_Changed;

		/// <summary>
		/// Name of the currently loaded session
		/// Saved in application properties
		/// </summary>
		internal string ActiveSession { get { return Properties.Settings.Default.ActiveSession; } set { Properties.Settings.Default.ActiveSession = value;
				if (ActiveSession_Changed != null) ActiveSession_Changed(); } }

		/// <summary>
		/// Boolean for if application is in testmode
		/// Saved in application properties
		/// </summary>
		internal bool Test { get { return Properties.Settings.Default.Test; } set { Properties.Settings.Default.Test = value; } }

		/// <summary>
		/// Adds a session to the settings. This after the user have saved a new session
		/// </summary>
		/// <param name="sessionToAdd">The session to add</param>
		internal void AddSession(Session sessionToAdd)
		{
			if (sessions == null)
				sessions = new List<Session>();
			sessions.Add(sessionToAdd);
			SessionNamesAutoComplete.Add(sessionToAdd.SessionName);
		}

		/// <summary>
		/// Deletes a session from the settings
		/// </summary>
		/// <param name="sessionName">Session to delete</param>
		internal void DeleteSession(string sessionName)
		{
			sessions.RemoveAt(sessions.FindIndex(x => x.SessionName.Equals(sessionName)));
			SessionNamesAutoComplete.Remove(sessionName);
		}

		/// <summary>
		/// Return a list of applications that is excluded from settings
		/// </summary>
		/// <returns>List of excluded applications</returns>
		internal List<string> GetExcludedApps()
		{
			return excludedApps;
		}

		/// <summary>
		/// Get how many sessions have been saved
		/// </summary>
		/// <returns>Number of saved sessions</returns>
		internal int GetNumberOfSessions()
		{
			return sessions.Count;
		}

		/// <summary>
		/// Checks if the named process is in list of excluded applications
		/// </summary>
		/// <param name="processName">Name of process to check if excluded</param>
		/// <returns>If application is excluded or not</returns>
		internal bool IsExcludedApp(string processName)
		{
			return excludedApps.Contains(processName);
		}

		/// <summary>
		/// A new application is to be added to exclusionlist
		/// </summary>
		/// <param name="appToExclude">Name of application to be added</param>
		internal void UpdateExcludedApplications(string appToExclude)
		{
			excludedApps.Add(appToExclude);
		}

		/// <summary>
		/// Rewrite the whole list of applications
		/// </summary>
		/// <param name="appsList">List of applications to exclude</param>
		internal void ReplaceExcludedApplicationsList(List<string> appsList)
		{
			excludedApps.Clear();
			foreach (string app in appsList)
			{
				excludedApps.Add(app);
			}
		}

		/// <summary>
		/// Returns the session based on its index in the sessionlist
		/// </summary>
		/// <param name="index">Index of session</param>
		/// <returns>The session at index</returns>
		internal Session GetSession(int index)
		{
			return sessions[index];
		}

		/// <summary>
		/// Searched and returns a session based on the name
		/// </summary>
		/// <param name="sessionName">Name of session to search for</param>
		/// <returns>The session being searched for</returns>
		internal Session GetSession(string sessionName)
		{
			return sessions.Find(x => x.SessionName.Equals(sessionName));
		}

		/// <summary>
		/// Returns a list with the names of sessions in the sessionlist
		/// </summary>
		/// <returns>Stringarray of sessions</returns>
		internal List<Session> GetSessionList()
		{
			return sessions;
		}

		/// <summary>
		/// Saves a session to the given sessionname
		/// </summary>
		/// <param name="sessionName">Name of session to be saved</param>
		/// <param name="session">Session to be saved</param>
		internal void SaveSession(string sessionName, Session session)
		{
			int sessionIndex = sessions.FindIndex(x => x.SessionName.Equals(sessionName));
			sessions[sessionIndex].Plist = session.Plist;
		}

		/// <summary>
		/// Writes the settings to file
		/// </summary>
		internal void SaveToFile()
		{
			var s = MemberwiseClone() as Settings;
			new FileHandler().Write(ref s);
		}

		/// <summary>
		/// Applies the settings for the currently active session
		/// Do this in alphabetical order of plist
		/// </summary>
		internal void ApplyActiveSession()
		{
			GetSession(Properties.Settings.Default.ActiveSession).UseSession();
		}

		/// <summary>
		/// Applies the settings for the currently active session
		/// The settings is applied in order of system z-order
		/// </summary>
		internal void ApplyActiveSession(List<KeyValuePair<IntPtr, int>> handles)
		{
			GetSession(Properties.Settings.Default.ActiveSession).UseSession(handles);
		}
	}
}
