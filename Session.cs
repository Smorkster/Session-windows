using System;
using System.Collections.Generic;

namespace Session_windows
{
	/// <summary>
	/// Description of Session.
	/// </summary>
	public class Session
	{
		List<ProcessInfo> plist;
		string sessionName;

		public Session(){}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="n">Name of session</param>
		/// <param name="l">List of processes in session</param>
		public Session(string n, List<ProcessInfo> l)
		{
			plist = l;
			sessionName = n;
		}

		/// <summary>
		/// Contructor
		/// </summary>
		/// <param name="s">Session to be used</param>
		public Session(Session s)
		{
			plist = s.Plist;
			sessionName = s.SessionName;
		}

		public string SessionName { get { return sessionName; } set { sessionName = value; } }
		public List<ProcessInfo> Plist { get { return plist; } set { plist = value; } }

		/// <summary>
		/// Deletes a process from the processlist
		/// </summary>
		/// <param name="process">Processname</param>
		public void deleteProcessFromSession(ProcessInfo process)
		{
			int processIndex = plist.FindIndex(x => x.ProcessID == process.ProcessID);
			plist.RemoveAt(processIndex);
		}

		/// <summary>
		/// Adds a process to the session
		/// </summary>
		/// <param name="process"></param>
		public void addProcessToSession(ProcessInfo process)
		{
			plist.Add(process);
		}

		/// <summary>
		/// Updates the information about the process
		/// </summary>
		/// <param name="processName">Name of the process to be updated</param>
		/// <param name="process">An object containing the new information</param>
		public void updateProcess(string processName, ProcessInfo process)
		{
			int processIndex = plist.FindIndex(x => x.ProcessName.Equals(processName));
			plist[processIndex] = process;
		}
	}
}
