﻿
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

		public Session()
		{
		}

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
	}
}
