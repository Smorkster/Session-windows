using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace Session_windows
{
	/// <summary>
	/// Object for filehandling, i.e. writing and reading
	/// </summary>
	public class FileHandler
	{
		List<Session> slist = new List<Session>();
		FileInfo fi = new FileInfo(@"H:\WindowSession.xml");
		XmlDocument doc = new XmlDocument();

		/// <summary>
		/// Collects all process- and sessioninformation to a string and writes to file
		/// </summary>
		/// <param name="settings">All settings for sessions and processes</param>
		public void write (Settings settings)
		{
			string dataText = "";
			StreamWriter writer;

			for (int i = 0; i < settings.NumberOfSessions; i++) {
				Session s = settings.getSession(i);
				foreach (ProcessInfo p in s.Plist) {
					dataText = dataText + string.Format("<session><sessionname>{0}</sessionname><processname>{1}</processname><winplacement>{2}</winplacement><xcoor>{3}</xcoor><ycoor>{4}</ycoor><xlowcoor>{5}</xlowcoor><ylowcoor>{6}</ylowcoor></session>\r\n",
					                                    s.SessionName,
					                                    p.ProcessName,
					                                    p.Minimized,
					                                    p.Xcoordinate,
					                                    p.Ycoordinate,
					                                    p.Xcoordinatelow,
					                                    p.Ycoordinatelow);
				}
			}
			try {
				if (!IsFileLocked(fi)) {
					writer = new StreamWriter((fi.FullName), false, System.Text.Encoding.UTF8);
					writer.WriteLine("<?xml version=\"1.0\"?>\r\n<sessions>");
					writer.Write(dataText);
					writer.WriteLine("<docked>" + settings.Docked + "</docked>");
					writer.WriteLine("<undocked>" + settings.Undocked + "</undocked>");
					writer.WriteLine("<start>" + (settings.StartInSysTray ? "true" : "false") + "</start>");
					writer.WriteLine("</sessions>");
					writer.Flush();
					writer.Close();
				} else {
					MessageBox.Show("Can't open file for writing. Might be open in another process", "Error opening XML-file");
				}
			} catch (IOException e) {
				MessageBox.Show("Something went wrong while writing configfile:\r\n\r\n" + e.Message, "", MessageBoxButtons.OK);
			}
		}

		/// <summary>
		/// Read the XML-file and sort the processes per session
		/// </summary>
		/// <returns>List of sessions with processes</returns>
		public List<Session> read (string Caller)
		{
			XmlTextReader xRead = new XmlTextReader(@"H:\WindowSession.xml");

			if (File.Exists(fi.FullName) && !IsFileLocked(fi)) {
				try {
					doc.Load(xRead);

					XmlNode root = doc.DocumentElement;
					XmlNodeList processname = doc.GetElementsByTagName("processname"),
					xcoor = doc.GetElementsByTagName("xcoor"),
					ycoor = doc.GetElementsByTagName("ycoor"),
					xlowcoor = doc.GetElementsByTagName("xlowcoor"),
					ylowcoor = doc.GetElementsByTagName("ylowcoor"),
					winplacement = doc.GetElementsByTagName("winplacement"),
					sessionname = doc.GetElementsByTagName("sessionname");

					xRead.Close();

					Process[] processes;

					for (int i = 0; i < sessionname.Count; i++) {
						processes = Process.GetProcessesByName(processname[i].InnerText);
						IntPtr h;
						int id;
						string n, wt;
						if (processes.Length == 0) {
							h = IntPtr.Zero;
							id = 0;
							wt = "";
						} else {
							h = processes[0].MainWindowHandle;
							id = processes[0].Id;
							wt = processes[0].MainWindowTitle;
						}
						n = processname[i].InnerText;
						int xc = int.Parse(xcoor[i].InnerText);
						int yx = int.Parse(ycoor[i].InnerText);
						int xlc = int.Parse(xlowcoor[i].InnerText);
						int ylc = int.Parse(ylowcoor[i].InnerText);
						int wp = int.Parse(winplacement[i].InnerText);
						ProcessInfo pi = new ProcessInfo(h,
						                                 id,
						                                 n,
						                                 wt,
						                                 xc,
						                                 yx,
						                                 xlc,
						                                 ylc,
						                                 wp);
						int sessionindex = slist.FindIndex(x => x.SessionName.Equals(sessionname[i].InnerText));
						if (sessionindex == -1) {
							List<ProcessInfo> p = new List<ProcessInfo>();
							slist.Add(new Session(sessionname[i].InnerText, p));
							sessionindex = slist.FindIndex(x => x.SessionName.Equals(sessionname[i].InnerText));
						} else {
							if (slist[sessionindex].Plist == null || slist[sessionindex].Plist.Count == 0)
								slist[sessionindex].Plist = new List<ProcessInfo>();
						}
						slist[sessionindex].Plist.Add(pi);
					}
					return slist;
				} catch (Exception e) {
					MessageBox.Show("Something wrong when reading XML (" + Caller + "):\n" + e.Message, "");
					xRead.Close();
				}
			}

			return null;
		}

		/// <summary>
		/// Read XML-elements for what sessions is specified to be used when computer is docked vs. undocked
		/// </summary>
		/// <returns>Two set stringarray of the sessionnames</returns>
		public string[] getDockedSessions ()
		{
			string[] specifiedSessions = new string[2];
			XmlTextReader xRead = new XmlTextReader(@"H:\WindowSession.xml");

			try {
				doc.Load(xRead);

				XmlNodeList undocked = doc.GetElementsByTagName("undocked"),
				docked = doc.GetElementsByTagName("docked");
				xRead.Close();
				specifiedSessions[0] = docked[0].InnerText;
				specifiedSessions[1] = undocked[0].InnerText;
				return specifiedSessions;
			} catch (Exception e) {
				MessageBox.Show("Something wrong when reading XML:\n" + e.Message, "");
				xRead.Close();
			}

			return new []{ "", "" };
		}

		/// <summary>
		/// Read XML-file for the the setting if the application is to be started in
		/// notificationarean or with form shown
		/// </summary>
		/// <returns>True if started in notificationarea</returns>
		public bool getStart ()
		{
			XmlTextReader xRead = new XmlTextReader(@"H:\WindowSession.xml");

			try {
				doc.Load(xRead);

				XmlNodeList start = doc.GetElementsByTagName("start");
				xRead.Close();

				if (start.Count <= 0)
					return false;

				var temp = start[0].InnerText;
				return start[0].InnerText.Equals("true");
			} catch (Exception e) {
				MessageBox.Show("Something wrong when reading XML:\n" + e.Message, "");
				xRead.Close();
			}

			return false;
		}

		/// <summary>
		/// Checks if a file is locked in any way or not
		/// </summary>
		/// <param name="file">File to be checked</param>
		/// <returns>True if file is locked, otherwise false</returns>
		bool IsFileLocked (FileInfo file)
		{
			FileStream stream = null;

			try {
				stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
			} catch (IOException) {
				//the file is unavailable because it is:
				//still being written to
				//or being processed by another thread
				//or does not exist (has already been processed)
				return true;
			} finally {
				if (stream != null)
					stream.Close();
			}

			//file is not locked
			return false;
		}
	}
}
