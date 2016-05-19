using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace Session_windows
{
	/// <summary>
	/// Description of FileHandler.
	/// </summary>
	public class FileHandler
	{
		List<Session> slist = new List<Session>();

		public void write(List<Session> sL)
		{
			FileInfo fi = new FileInfo(@"H:\WindowSession.xml");
			string dataText = "";
			StreamWriter writer = new StreamWriter((fi.FullName), false, System.Text.Encoding.UTF8);

			foreach(Session s in sL)
			{
				foreach(ProcessInfo p in s.Plist)
				{
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
			try
			{
//				if(!IsFileLocked(fi))
//				{
					writer.WriteLine("<?xml version=\"1.0\"?>\r\n<sessions>");
					writer.Write(dataText);
					writer.WriteLine("</sessions>");
					writer.Flush();
					writer.Close();
//				}
			} catch (IOException e)
			{
				MessageBox.Show("Something went wrong while writing configfile:\r\n\r\n" + e.Message, "", MessageBoxButtons.OK);
			}
		}

		public List<Session> read()
		{
			FileInfo fi = new FileInfo(@"H:\WindowSession.xml");
			XmlReader xRead = new XmlTextReader(@"H:\WindowSession.xml");;
			XmlDocument doc = new XmlDocument();

			if(File.Exists(fi.FullName) && !IsFileLocked(fi))
			{
				try
				{
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
	
					for(int i = 0; i < sessionname.Count;i++)
					{
						processes = Process.GetProcessesByName(processname[i].InnerText);
						IntPtr h;
						int id;
						string n, wt;
						if(processes.Length == 0)
						{
							h = IntPtr.Zero;
							id = 0;
							n = "";
							wt = "";
						}
						else
						{
							h = processes[0].MainWindowHandle;
							id = processes[0].Id;
							n = processes[0].ProcessName;
							wt = processes[0].MainWindowTitle;
						}
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
						if(sessionindex == -1)
						{
							List<ProcessInfo> p = new List<ProcessInfo>();
							slist.Add(new Session(sessionname[i].InnerText, p));
							sessionindex = 0;
						} else {
							if(slist[sessionindex].Plist == null)
								slist[sessionindex].Plist = new List<ProcessInfo>();
						}
						slist[sessionindex].Plist.Add(pi);
					}
					return slist;
				} catch (Exception e)
				{
					MessageBox.Show("Something wrong when reading XML:\n" + e.Message, "");
					xRead.Close();
				}
			}

			return null;
		}
		
		protected virtual bool IsFileLocked(FileInfo file)
		{
		    FileStream stream = null;

		    try
		    {
		        stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
		    } catch (IOException)  {
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
