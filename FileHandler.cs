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
		FileInfo settingsFile = new FileInfo(@"H:\WindowSession.xml");

		/// <summary>
		/// Collects all process- and sessioninformation to a string and writes to file
		/// </summary>
		/// <param name="settings">All settings for sessions and processes</param>
		public void write (ref Settings settings)
		{
			try
			{
				if (!IsFileLocked(settingsFile))
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
					
					List<string> excludedApps = settings.getExcludedApps();

					XmlNode xmlExcludedAppsList = xmlDoc.CreateElement("excludedAppsList");
					foreach (string app in excludedApps)
					{
						XmlNode xmlExcludedApp = xmlDoc.CreateElement("excludedApp");
						xmlExcludedApp.InnerText = app;
						xmlExcludedAppsList.AppendChild(xmlExcludedApp);
					}
					rootNode.AppendChild(xmlExcludedAppsList);

					foreach (string s in settings.getSessionList())
					{
						Session session = settings.getSession(s);
						XmlNode sessionNode = xmlDoc.CreateElement("session");
						XmlAttribute attributeSessionName = xmlDoc.CreateAttribute("name");
						XmlAttribute attributeTaskbarVisible = xmlDoc.CreateAttribute("taskbarVisible");

						attributeSessionName.Value = session.SessionName;
						sessionNode.Attributes.Append(attributeSessionName);
						attributeTaskbarVisible.Value = session.TaskbarVisible.ToString();
						sessionNode.Attributes.Append(attributeTaskbarVisible);
						foreach (ProcessInfo p in session.Plist)
						{
							if (!settings.isExcludedApp(p.ProcessName))
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
						}
						rootNode.AppendChild(sessionNode);
					}
					xmlDoc.Save(@"H:\WindowSession.xml");
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
			string loop="";
			if (File.Exists(settingsFile.FullName) && !IsFileLocked(settingsFile))
			{
				XDocument xmlDoc;
				Process process;
				List<string> excludeList;
				Session savedSession;

				try
				{
					xmlDoc = XDocument.Load(settingsFile.FullName);
					process = null;
					excludeList = new List<string>();
		
					foreach (XElement sessionXMLElement in xmlDoc.Descendants("session"))
					{
						savedSession = new Session();
						savedSession.SessionName = sessionXMLElement.Attribute("name").Value;
						if(sessionXMLElement.Attribute("taskbarVisible") != null)
							savedSession.TaskbarVisible = sessionXMLElement.Attribute("taskbarVisible").Value == "True";
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
							loop = sessionXMLElement.Attribute("name").Value + " " + pInfo.Value;
						}
						settings.addSession(savedSession);
						process = null;
						loop = "Done";
					}

					XElement xmlExcludedAppsList = xmlDoc.Descendants("excludedAppsList").First();
					foreach (XElement xmlExcludedApp in xmlExcludedAppsList.Descendants("excludedApp"))
					{
						excludeList.Add(xmlExcludedApp.Value);
					}

				} catch (Exception e)
				{
					MessageBox.Show("Something wrong when reading XML (" + loop + "):\n" + e.Message, "");
					return null;
				}

				settings.Docked = xmlDoc.Root.Attribute("docked").Value;
				settings.Undocked = xmlDoc.Root.Attribute("undocked").Value;
				settings.StartInSysTray = Convert.ToBoolean(int.Parse(xmlDoc.Root.Attribute("startsystray").Value));
				settings.replaceExcludedApplicationsList(excludeList);

				return settings;
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
