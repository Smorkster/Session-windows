using System;
using System.Collections.Generic;

namespace Session_windows
{
	/// <summary>
	/// Description of Settings.
	/// </summary>
	public class Settings
	{
		List<Session> sessions;
		List<ProcessInfo> sessionProcesses;
		string[] dockedUndocked;
		bool startSysTray;
		public Session savedSession;
		public List<ProcessInfo> sessionProcessList;

		public Settings()
		{
			sessions = new List<Session>();
			sessionProcesses = new List<ProcessInfo>();
			dockedUndocked = new string[2];
			startSysTray = false;
		}

		/// <summary>
		/// Should the application start in systray or with the form open
		/// </summary>
		public bool StartInSysTray {
			get { return startSysTray; }
			set {
				startSysTray = value;
			}
		}

		/// <summary>
		/// String for which session to be used when docked
		/// </summary>
		public string Docked {
			get { return dockedUndocked[0]; }
			set {
				dockedUndocked[0] = value;
			}
		}

		/// <summary>
		/// String for which session to be used when undocked
		/// </summary>
		public string Undocked {
			get { return dockedUndocked[1]; }
			set {
				dockedUndocked[1] = value;
			}
		}

		/// <summary>
		/// Returns number of saved sessions
		/// </summary>
		public int NumberOfSessions { get { return sessions.Count; } }

		/// <summary>
		/// Searched and returns a session based on the name
		/// </summary>
		/// <param name="sessionName">Name of session to search for</param>
		/// <returns>The session being searched for</returns>
		public Session getSession(string sessionName)
		{
			return sessions.Find(x => x.SessionName.Equals(sessionName));
		}

		/// <summary>
		/// Returns the session based on its index in the sessionlist
		/// </summary>
		/// <param name="index">Index of session</param>
		/// <returns>The session at index</returns>
		public Session getSession(int index)
		{
			return sessions[index];
		}

		/// <summary>
		/// Gets the index of a session in the sessionlist
		/// </summary>
		/// <param name="sessionName">Name of the session to return for</param>
		/// <returns>The index of the session in the sessionlist</returns>
		public int getSessionIndex(string sessionName)
		{
			return sessions.FindIndex(x => x.SessionName.Equals(sessionName));
		}

		/// <summary>
		/// Returns an stringarray of the entire sessionlist
		/// </summary>
		/// <returns>Stringarray of sessions</returns>
		public string[] getSessionList()
		{
			string[] sessionList = new string[NumberOfSessions];
			for (int i = 0; i < sessions.Count; i++) {
				sessionList[i] = sessions[i].SessionName;
			}
			
			return sessionList;
		}

		/// <summary>
		/// Saves a session
		/// </summary>
		/// <param name="session">Session to be saved</param>
		public void saveSession(Session session)
		{
			int sessionIndex = sessions.FindIndex(x => x.SessionName.Equals(session.SessionName));
			sessions[sessionIndex].Plist = session.Plist;
		}

		/// <summary>
		/// Saves a session to the given sessionname
		/// </summary>
		/// <param name="sessionName">Session to be saved</param>
		/// <param name="session">Session to be saved</param>
		public void saveSession(string sessionName, Session session)
		{
			int sessionIndex = sessions.FindIndex(x => x.SessionName.Equals(sessionName));
			sessions[sessionIndex].Plist = session.Plist;
		}

		/// <summary>
		/// Adds a session to the settings. This after the user have saved a new session
		/// </summary>
		/// <param name="sessionToAdd">The session to add</param>
		public void addSession(Session sessionToAdd)
		{
			if (sessions == null)
				sessions = new List<Session>();
			sessions.Add(sessionToAdd);
		}

		/// <summary>
		/// Deletes a session from the settings
		/// </summary>
		/// <param name="sessionName">Session to delete</param>
		public void deleteSession(string sessionName)
		{
			int sessionIndex = sessions.FindIndex(x => x.SessionName.Equals(sessionName));
			sessions.RemoveAt(sessionIndex);
		}
	}
}
