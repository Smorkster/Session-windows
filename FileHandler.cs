using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace Session_windows
{
	/// <summary>
	/// Object for filehandling, i.e. writing and reading
	/// </summary>
	public class FileHandler
	{
		FileInfo fi = new FileInfo(@"H:\WindowSession - Test.xml");

		/// <summary>
		/// Collects all process- and sessioninformation to a string and writes to file
		/// </summary>
		/// <param name="settings">All settings for sessions and processes</param>
		public void write (Settings settings)
		{
			try
			{
				if (!IsFileLocked(fi))
				{
					XmlDocument xmlDoc = new XmlDocument();
					XmlNode rootNode = xmlDoc.CreateElement("sessions");
					XmlAttribute docked = xmlDoc.CreateAttribute("docked");
					XmlAttribute undocked = xmlDoc.CreateAttribute("undocked");
					XmlAttribute startsystray = xmlDoc.CreateAttribute("startsystray");
					
					docked.Value = settings.Docked;
					undocked.Value = settings.Undocked;
					startsystray.Value = settings.StartInSysTray ? string.Format("1") : string.Format("0");
					rootNode.Attributes.Append(docked);
					rootNode.Attributes.Append(undocked);
					rootNode.Attributes.Append(startsystray);
					xmlDoc.AppendChild(rootNode);
					
					foreach (string s in settings.getSessionList())
					{
						Session session = settings.getSession(s);
						XmlNode sessionNode = xmlDoc.CreateElement("session");
						XmlAttribute attributeSName = xmlDoc.CreateAttribute("name");
						attributeSName.Value = session.SessionName;
						sessionNode.Attributes.Append(attributeSName);
						foreach (ProcessInfo p in session.Plist)
						{
							XmlNode processNode = xmlDoc.CreateElement("process");
							XmlAttribute pWinPl = xmlDoc.CreateAttribute("winplacement"),
							xc = xmlDoc.CreateAttribute("xcoor"),
							yc = xmlDoc.CreateAttribute("ycoor"),
							width = xmlDoc.CreateAttribute("width"),
							height = xmlDoc.CreateAttribute("height");
							
							pWinPl.Value = p.Minimized.ToString();
							xc.Value = p.Xcoordinate.ToString();
							yc.Value = p.Ycoordinate.ToString();
							width.Value = p.Width.ToString();
							height.Value = p.Height.ToString();
							
							processNode.Attributes.Append(pWinPl);
							processNode.Attributes.Append(xc);
							processNode.Attributes.Append(yc);
							processNode.Attributes.Append(width);
							processNode.Attributes.Append(height);
							
							processNode.InnerText = p.ProcessName;
							
							sessionNode.AppendChild(processNode);
						}
						rootNode.AppendChild(sessionNode);
					}
					xmlDoc.Save(@"H:\WindowSession - Test.xml");
				} else
				{
					MessageBox.Show("Can't open file for writing. Might be open in another process", "Error opening XML-file");
				}
			} catch (IOException e)
			{
				MessageBox.Show("Something went wrong while writing configfile:\r\n\r\n" + e.Message, "", MessageBoxButtons.OK);
			}
		}

		/// <summary>
		/// Read the XML-file and sort the processes per session
		/// </summary>
		/// <returns>List of sessions with processes</returns>
		public Settings read ()
		{
			Settings settings = new Settings();
			if (File.Exists(fi.FullName) && !IsFileLocked(fi))
			{
				try
				{
					XDocument xmlDoc = XDocument.Load(@"H:\WindowSession - Test.xml");
					Process[] processes;
		
					foreach (XElement se in xmlDoc.Descendants("session"))
					{
						Session session = new Session();
						session.SessionName = se.Attribute("name").Value;
						List<ProcessInfo> pl = new List<ProcessInfo>();
						foreach (XElement pi in se.Descendants("process"))
						{
							processes = Process.GetProcessesByName(pi.Value);
							IntPtr h;
							int id;
							string wt;
							if (processes.Length == 0)
							{
								h = IntPtr.Zero;
								id = 0;
								wt = "";
							} else
							{
								h = processes[0].MainWindowHandle;
								id = processes[0].Id;
								wt = processes[0].MainWindowTitle;
							}
							int x = int.Parse(pi.Attribute("xcoor").Value);
							int y = int.Parse(pi.Attribute("ycoor").Value);
							int wi = int.Parse(pi.Attribute("width").Value);
							int he = int.Parse(pi.Attribute("height").Value);
							int w = int.Parse(pi.Attribute("winplacement").Value);
							pl.Add(new ProcessInfo(h,
								id,
								pi.Value,
								wt,
								x,
								y,
								wi,
								he,
								w));
							session.Plist = pl;
						}
						settings.addSession(session);
					}
					settings.Docked = xmlDoc.Root.Attribute("docked").Value;
					settings.Undocked = xmlDoc.Root.Attribute("undocked").Value;
					settings.StartInSysTray = Convert.ToBoolean(int.Parse(xmlDoc.Root.Attribute("startsystray").Value));

					return settings;
				} catch (Exception e)
				{
					MessageBox.Show("Something wrong when reading XML:\n" + e.Message, "");
				}
			}

			return null;
		}

		/// <summary>
		/// Checks if a file is locked in any way or not
		/// </summary>
		/// <param name="file">File to be checked</param>
		/// <returns>True if file is locked, otherwise false</returns>
		bool IsFileLocked (FileInfo file)
		{
			FileStream stream = null;

			try
			{
				stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
			} catch (IOException)
			{
				//the file is unavailable because it is:
				//still being written to
				//or being processed by another thread
				//or does not exist (has already been processed)
				return true;
			} finally
			{
				if (stream != null)
					stream.Close();
			}

			//file is not locked
			return false;
		}
	}
}
