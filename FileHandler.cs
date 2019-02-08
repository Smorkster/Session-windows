using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
		FileInfo fi = new FileInfo(@"H:\WindowSession.xml");

		/// <summary>
		/// Collects all process- and sessioninformation to a string and writes to file
		/// </summary>
		/// <param name="settings">All settings for sessions and processes</param>
		public void write (ref Settings settings)
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
							
							pWinPl.Value = p.WindowPlacement.ToString();
							xc.Value = p.XTopCoordinate.ToString();
							yc.Value = p.YTopCoordinate.ToString();
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
			int loop = 0;
			if (File.Exists(fi.FullName) && !IsFileLocked(fi))
			{
				try
				{
					XDocument xmlDoc = XDocument.Load(fi.FullName);
					Process process = null;
		
					foreach (XElement sessionXMLElement in xmlDoc.Descendants("session"))
					{
						Session savedSession = new Session();
						savedSession.SessionName = sessionXMLElement.Attribute("name").Value;
						List<ProcessInfo> processList = new List<ProcessInfo>();
						foreach (XElement pInfo in sessionXMLElement.Descendants("process"))
						{
							IntPtr handle;
							int processID;
							string windowTitle;

							Process[] ps = Process.GetProcessesByName(pInfo.Value);
							if (!ps.Any()) {
								handle = IntPtr.Zero;
								processID = 0;
								windowTitle = "";
							} else {
								try{
									process = ps.First(p => p.MainWindowHandle != IntPtr.Zero);
									handle = process.MainWindowHandle;
									processID = process.Id;
									windowTitle = process.MainWindowTitle;
								} catch {
									handle = IntPtr.Zero;
									processID = ps.First().Id;
									windowTitle = "";
								}
							}
							int xCoordinate = int.Parse(pInfo.Attribute("xcoor").Value);
							int yCoordinate = int.Parse(pInfo.Attribute("ycoor").Value);
							int width = int.Parse(pInfo.Attribute("width").Value);
							int height = int.Parse(pInfo.Attribute("height").Value);
							int windowPlacement = int.Parse(pInfo.Attribute("winplacement").Value);
							processList.Add(new ProcessInfo(handle,
								processID,
								pInfo.Value,
								xCoordinate,
								yCoordinate,
								width,
								height,
								windowPlacement));
							savedSession.Plist = processList;
							loop += 1;
						}
						settings.addSession(savedSession);
						process = null;
					}
					settings.Docked = xmlDoc.Root.Attribute("docked").Value;
					settings.Undocked = xmlDoc.Root.Attribute("undocked").Value;
					settings.StartInSysTray = Convert.ToBoolean(int.Parse(xmlDoc.Root.Attribute("startsystray").Value));

					return settings;
				} catch (Exception e)
				{
					MessageBox.Show("Something wrong when reading XML ("+loop+"):\n" + e.Message, "");
					return null;
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
				return true;
			} finally
			{
				if (stream != null)
					stream.Close();
			}
			return false; // File is not locked
		}
	}
}
