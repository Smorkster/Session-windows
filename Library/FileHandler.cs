using Session_windows.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Schema;

namespace Session_windows.Library
{
	/// <summary>
	/// Object for filehandling, i.e. writing and reading
	/// </summary>
	class FileHandler
	{
		string XMLErrors = "";
		internal FileHandler() { }

		/// <summary>
		/// Create an XmlDocument for reading the input-file
		/// </summary>
		/// <returns>XmlDocument containing settings from the input-file</returns>
		XmlDocument CreateStream_In()
		{
			try
			{
				if (new FileInfo(Properties.Settings.Default.SettingsFile).Length > 0)
				{
					XmlSchemaSet xmlSet = Schema.GetSchemaSet();
					XmlReaderSettings settings = new XmlReaderSettings();
					ValidationEventHandler validationEventHandler = new ValidationEventHandler(XmlValidationHandler);
					XmlReader xmlReader;

					xmlSet.ValidationEventHandler += validationEventHandler;
					settings.Schemas.Add(xmlSet);
					settings.Schemas.Compile();
					settings.ValidationEventHandler += validationEventHandler;
					settings.ValidationType = ValidationType.Schema;

					using (xmlReader = XmlReader.Create(Properties.Settings.Default.SettingsFile, settings))
					{
						XmlDocument document = new XmlDocument();
						document.Load(xmlReader);
						return document;
					}
				}
				else
				{
					throw new Exception("File empty");
				}
			}
			catch (FileNotFoundException)
			{
				MessageBox.Show("File not found");
			}
			catch (FileLoadException)
			{
				MessageBox.Show("Can't access file");
			}
			catch (IOException e)
			{
				MessageBox.Show(e.Message);
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message);
			}
			return null;
		}

		/// <summary>
		/// Verify file location and accesibility, then return a streamwriter for the file
		/// </summary>
		/// <returns>Streamwriter for settings-file</returns>
		StreamWriter CreateStream_Out()
		{
			try
			{
				return new StreamWriter(Properties.Settings.Default.SettingsFile);
			}
			catch (UnauthorizedAccessException)
			{
				Properties.Settings.Default.SettingsFile += ".BackUp";
				MessageBox.Show("Don't have access to file.\r\nNew file '" + Properties.Settings.Default.SettingsFile + "'");
				return new StreamWriter(Properties.Settings.Default.SettingsFile);
			}
			catch (FileNotFoundException)
			{
				MessageBox.Show("File not found");
				return null;
			}
			catch (FileLoadException)
			{
				MessageBox.Show("Can't access file");
				return null;
			}
			catch (IOException e)
			{
				MessageBox.Show(e.Message);
				return null;
			}
		}

		/// <summary>
		/// Read the XML-file and sort the processes per session
		/// </summary>
		/// <returns>All settings read from XML-file</returns>
		internal Settings Read()
		{
			List<string> excludeList;
			Process process;
			Process[] ps;
			ProcessInfo temppInfo;
			Session savedSession;
			Settings settings = new Settings();
			string sessionLoop = "";
			XmlDocument xmlDoc;

			if ((xmlDoc = CreateStream_In()) == null)
			{
				return settings;
			}
			excludeList = new List<string>();

			foreach (XmlNode sessionXMLElement in xmlDoc.SelectNodes("//session"))
			{
				List<ProcessInfo> processList = new List<ProcessInfo>();
				foreach (XmlNode pInfo in sessionXMLElement.SelectNodes("process"))
				{
					process = null;
					ps = Process.GetProcessesByName(pInfo.InnerText);
					temppInfo = new ProcessInfo();

					try
					{
						if (pInfo.InnerText.Equals("explorer"))
						{
							process = ps.First(p => p.MainWindowHandle != IntPtr.Zero || !p.MainWindowTitle.Equals(""));
						}
						else
						{
							process = ps.First(p => p.MainWindowHandle != IntPtr.Zero);
						}
						temppInfo.MainWindowHandle = process.MainWindowHandle;
						temppInfo.ProcessID = process.Id;
					}
					catch
					{
						if (ps.Count() == 0)
						{
							temppInfo.MainWindowHandle = IntPtr.Zero;
							temppInfo.ProcessID = 0;
						}
						else
						{
							temppInfo.MainWindowHandle = IntPtr.Zero;
							temppInfo.ProcessID = ps[0].Id;
						}
					}
					temppInfo.ProcessName = pInfo.InnerText;
					try { temppInfo.XTopCoordinate = int.Parse(pInfo.Attributes["xcoor"].Value); } catch { }
					try { temppInfo.YTopCoordinate = int.Parse(pInfo.Attributes["ycoor"].Value); } catch { }
					try { temppInfo.Width = int.Parse(pInfo.Attributes["width"].Value); } catch { }
					try { temppInfo.Height = int.Parse(pInfo.Attributes["height"].Value); } catch { }
					try { temppInfo.WindowPlacement = int.Parse(pInfo.Attributes["winplacement"].Value); } catch { }

					sessionLoop = "session: " + sessionXMLElement.Attributes["name"].Value + ", process:" + temppInfo.ProcessName;
					processList.Add(temppInfo);
					ps = null;
				}
				savedSession = new Session()
				{
					SessionName = sessionXMLElement.Attributes["name"].Value,
					Plist = processList
				};

				try { savedSession.TaskbarVisible = sessionXMLElement.Attributes["taskbarVisible"].Value == "True"; } catch { }

				settings.AddSession(savedSession);
				sessionLoop = "session '" + sessionXMLElement.Attributes["name"].Value + "' is done";
			}

			foreach (XmlNode xmlExcludedApp in xmlDoc.SelectNodes("//excludedApp"))
			{
				excludeList.Add(xmlExcludedApp.InnerText);
			}
			settings.ReplaceExcludedApplicationsList(excludeList);

			try { settings.DockedSession = xmlDoc.ChildNodes[1].Attributes["docked"].Value; } catch { }
			try { settings.UndockedSession = xmlDoc.ChildNodes[1].Attributes["undocked"].Value; } catch { }
			try { settings.StartInSysTray = Convert.ToBoolean(int.Parse(xmlDoc.ChildNodes[1].Attributes["startsystray"].Value)); } catch { }

			settings.SettingsFile = new FileInfo(Properties.Settings.Default.SettingsFile);

			if (!XMLErrors.Equals(""))
				MessageBox.Show(XMLErrors);
			return settings;
		}

		/// <summary>
		/// Collects all process- and sessioninformation and writes to an XML-file
		/// </summary>
		/// <param name="settings">All settings for sessions and processes</param>
		internal void Write(ref Settings settings)
		{
			StreamWriter writer;
			if ((writer = CreateStream_Out()) != null)
			{
				XmlDocument xmlDoc = new XmlDocument();
				XmlNode rootNode = xmlDoc.CreateElement("sessions");
				XmlAttribute docked = xmlDoc.CreateAttribute("docked");
				XmlAttribute undocked = xmlDoc.CreateAttribute("undocked");
				XmlAttribute startsystray = xmlDoc.CreateAttribute("startsystray");

				docked.Value = settings.DockedSession;
				undocked.Value = settings.UndockedSession;
				startsystray.Value = settings.StartInSysTray ? string.Format("1") : string.Format("0");
				rootNode.Attributes.Append(docked);
				rootNode.Attributes.Append(undocked);
				rootNode.Attributes.Append(startsystray);
				xmlDoc.AppendChild(rootNode);

				List<string> excludedApps = settings.GetExcludedApps();

				XmlNode xmlExcludedAppsList = xmlDoc.CreateElement("excludedAppsList");
				foreach (string app in excludedApps)
				{
					XmlNode xmlExcludedApp = xmlDoc.CreateElement("excludedApp");
					xmlExcludedApp.InnerText = app;
					xmlExcludedAppsList.AppendChild(xmlExcludedApp);
				}
				rootNode.AppendChild(xmlExcludedAppsList);

				foreach (Session session in settings.GetSessionList())
				{
					XmlNode sessionNode = xmlDoc.CreateElement("session");
					XmlAttribute attributeSessionName = xmlDoc.CreateAttribute("name");
					XmlAttribute attributeTaskbarVisible = xmlDoc.CreateAttribute("taskbarVisible");

					attributeSessionName.Value = session.SessionName;
					sessionNode.Attributes.Append(attributeSessionName);
					attributeTaskbarVisible.Value = session.TaskbarVisible.ToString();
					sessionNode.Attributes.Append(attributeTaskbarVisible);
					foreach (ProcessInfo p in session.Plist)
					{
						if (!settings.IsExcludedApp(p.ProcessName))
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

				xmlDoc.Save(writer);
				writer.Close();
			}
		}

		/// <summary>
		/// Eventhandler for validationerrors in XML-file
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic ValidationEventArgs</param>
		void XmlValidationHandler(object sender, ValidationEventArgs e)
		{
			XMLErrors += " * Error reading XML-file. Errormessage:\r\n\t" + e.Message + "\r\n\tAt line " + e.Exception.LineNumber + "\r\n\r\n";
		}
	}
}
