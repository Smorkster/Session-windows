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
		bool changed;
		public Session currentSession;
		public List<ProcessInfo> currentProcessesList;

		public Settings()
		{
			sessions = new List<Session>();
			sessionProcesses = new List<ProcessInfo>();
			dockedUndocked = new string[2];
			startSysTray = false;
		}

		// disable ConvertToAutoProperty
		public bool StartInSysTray {
			get { return startSysTray; }
			set {
				startSysTray = value;
				Changed = true;
			}
		}
		public string Docked {
			get { return dockedUndocked[0]; }
			set {
				dockedUndocked[0] = value;
				Changed = true;
			}
		}
		public string Undocked {
			get { return dockedUndocked[1]; }
			set {
				dockedUndocked[1] = value;
				Changed = true;
			}
		}
		public bool Changed {
			get { return changed; }
			set { changed = value; }
		}
		public int NumberOfSessions { get { return sessions.Count; } }

		public Session getSession (string sessionName)
		{
			return sessions.Find(x => x.SessionName.Equals(sessionName));
		}
		
		public Session getSession (int index)
		{
			return sessions[index];
		}

		public int getSessionIndex (string sessionName)
		{
			return sessions.FindIndex(x => x.SessionName.Equals(sessionName));
		}

		public string[] getSessionList ()
		{
			string[] sessionList = new string[NumberOfSessions];
			for (int i = 0; i < sessions.Count; i++) {
				sessionList[i] = sessions[i].SessionName;
			}
			
			return sessionList;
		}

		public ProcessInfo getProcessInfo (string sessionName, string processName)
		{
			int sessionIndex = sessions.FindIndex(x => x.SessionName.Equals(sessionName));
			return sessions[sessionIndex].Plist.Find(x => x.ProcessName.Equals(processName));
		}

		public void setCurrentSession (string sessionName)
		{
			int sessionIndex = sessions.FindIndex(x => x.SessionName.Equals(sessionName));
			currentSession = sessions.Find(x => x.SessionName.Equals(sessionName));
		}

		public void setCurrentProcesses (List<ProcessInfo> processList)
		{
			currentProcessesList = processList;
		}
		
		public void addSession (Session sessionToAdd)
		{
			if (sessions == null)
				sessions = new List<Session>();
			sessions.Add(sessionToAdd);
			Changed = true;
		}

		public void addProcessToSession (string sessionName, ProcessInfo process)
		{
			int sessionIndex = sessions.FindIndex(x => x.SessionName.Equals(sessionName));
			sessions[sessionIndex].Plist.Add(process);
			Changed = true;
		}

		public void deleteProcessFromSession (string sessionName, ProcessInfo process)
		{
			int sessionIndex = sessions.FindIndex(x => x.SessionName.Equals(sessionName));
			int processIndex = sessions[sessionIndex].Plist.FindIndex(x => x.ID == process.ID);
			sessions[sessionIndex].Plist.RemoveAt(processIndex);
			Changed = true;
		}
		
		public void deleteSession (string sessionName)
		{
			int sessionIndex = sessions.FindIndex(x => x.SessionName.Equals(sessionName));
			sessions.RemoveAt(sessionIndex);
			Changed = true;
		}

		public bool updateProcess (string processName, ProcessInfo newProcessInfo)
		{
			int sessionIndex = sessions.FindIndex(x => x.SessionName.Equals(currentSession.SessionName));
			int processIndex = sessions[sessionIndex].Plist.FindIndex(x => x.ProcessName.Equals(processName));

			if (processIndex != -1) {
				sessions[sessionIndex].Plist[processIndex] = currentSession.Plist[processIndex] = newProcessInfo;
				Changed = true;
				return true;
			}
			return false;
		}

		public void updateSession ()
		{
			int sessionIndex = sessions.FindIndex(x => x.SessionName.Equals(currentSession.SessionName));
			sessions[sessionIndex] = currentSession;
			Changed = true;
		}
	}
}
