using System;
using System.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices; // Added for COM release and COMException
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;

namespace ClearRecentLinks
{
	public partial class Form1 : Form
	{
		private Thread taskThread = null;
		private readonly Object myLock = new Object();

#if DEBUG
		private const int STARTUP_DELAY_SECONDS = 3;
#else
		private const int STARTUP_DELAY_SECONDS = 30;
#endif

		public Form1()
		{
			InitializeComponent();

			Thread.CurrentThread.Name = "Application thread"; // useful for debugging

			txtRunInterval.Text = ConfigurationManager.AppSettings["RunIntervalMinutes"];
			this.txtRunInterval.TextChanged += new System.EventHandler(this.txtRunInterval_TextChanged);

			LoadListbox();

			RestartTaskThread();
		}

		private void LoadListbox()
		{
			foreach (string x in ConfigurationManager.AppSettings["RemoveThese"].Split('|'))
			{
				listBox1.Items.Add(x);
			}
			listBox1.Sorted = true; // perform a sort
		}

		private void RestartTaskThread()
		{
			if (taskThread != null && taskThread.IsAlive)
			{
				System.Diagnostics.Debug.WriteLine("Aborting existing worker thread...");
				taskThread.Abort();
				taskThread = null;
			}

			labelStatus.Text = string.Format("Worker thread startup delay ({0} seconds)...", STARTUP_DELAY_SECONDS);

			taskThread = new Thread(ThreadTask);
            taskThread.SetApartmentState(ApartmentState.STA); // CRITICAL: Required for Shell32 COM interop
			taskThread.IsBackground = true;
			taskThread.Name = "Worker thread: Clear Recent/JumpList items";
			taskThread.Start();
		}

		private void ThreadTask()
		{
			try
			{
				// initial startup delay
				System.Diagnostics.Debug.WriteLine("Worker thread startup delay...");

				Thread.Sleep(1000 * STARTUP_DELAY_SECONDS);

				do
				{
					RunTask();

					Thread.Sleep(1000 * 60 * Int32.Parse(ConfigurationManager.AppSettings["RunIntervalMinutes"]));
				} while (true);
			}
			catch (ThreadAbortException)
			{
				// Thread restarted or stopped; do nothing
			}
		}

		private void btnRunNow_Click(object sender, EventArgs e)
		{
			RunTask();
		}

		private void RunTask()
		{
			if (Monitor.TryEnter(myLock, 1000))
			{
				Object shell = null;

				try
				{
					System.Diagnostics.Debug.WriteLine(string.Format("Enter RunTask() for thread named \"{0}\"", Thread.CurrentThread.Name));
					UpdateStatusText("Running...");

					// get the path for c:\users\USER
					string userProfile = System.Environment.GetEnvironmentVariable("USERPROFILE");

					// get some stuff from app.config
					string recentLinksPath = Path.Combine(userProfile, ConfigurationManager.AppSettings["RecentLinkPath"]);
					string jumpListPath = Path.Combine(userProfile, ConfigurationManager.AppSettings["JumpListPath"]);
					string JumpListFileExtension = ConfigurationManager.AppSettings["JumpListFileExtension"];

					System.Diagnostics.Debug.WriteLine("userProfile=" + userProfile);
					System.Diagnostics.Debug.WriteLine("RecentLinkPath=" + recentLinksPath);
					System.Diagnostics.Debug.WriteLine("JumpListPath=" + jumpListPath);
					System.Diagnostics.Debug.WriteLine("JumpListFileExtension=" + JumpListFileExtension);

					// get list of patterns we want to delete links for
					var removalPatterns = new List<string>();
					foreach (string x in ConfigurationManager.AppSettings["RemoveThese"].Split('|'))
					{
						string s = x.Trim().ToLower();
						if (!String.IsNullOrEmpty(s))
							removalPatterns.Add(s);
					}

					// files to be deleted later
					var filesToDelete = new List<string>();

					// process "Recent" links
					// I'm using Shell32 for this entire operation (including enumerating the files) 
					// because I need a shell32 instance to get the link target path, so may as well 
					// instantiate it once and use it all the way through.
					// Sorry it's so ugly.
					Type shell32Type = Type.GetTypeFromProgID("Shell.Application");
					shell = Activator.CreateInstance(shell32Type);
					Shell32.Folder s32Folder = (Shell32.Folder)shell32Type.InvokeMember("NameSpace", System.Reflection.BindingFlags.InvokeMethod, null, shell, new object[] { recentLinksPath });

                    if (s32Folder != null)
                    {
                        foreach (Shell32.FolderItem2 item in s32Folder.Items())
                        {
                            try
                            {
                                if (item.IsLink)
                                {
                                    Shell32.ShellLinkObject link = null;
                                    try
                                    {
                                        link = (Shell32.ShellLinkObject)item.GetLink;
                                    }
                                    catch (Exception)
                                    {
                                        // Unable to resolve link interface
                                    }

                                    if (link != null)
                                    {
                                        string targetPath = null;
                                        try
                                        {
                                            targetPath = link.Target?.Path;
                                        }
                                        catch (Exception)
                                        {
                                            // Inaccessible or broken target path
                                        }

                                        if (!String.IsNullOrEmpty(targetPath))
                                        {
                                            string linkTarget = targetPath.ToLower();
                                            foreach (string x in removalPatterns)
                                            {
                                                if (linkTarget.Contains(x))
                                                {
                                                    filesToDelete.Add(item.Path);
                                                    break;
                                                }
                                            }
                                        }

                                        Marshal.FinalReleaseComObject(link);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine("Error evaluating item: " + ex.Message);
                            }
                            finally
                            {
                                if (item != null)
                                    Marshal.FinalReleaseComObject(item);
                            }
                        }

                        Marshal.FinalReleaseComObject(s32Folder);
                    }

                    // Process Jump List files
                    if (Directory.Exists(jumpListPath))
                    {
                        foreach (string linkFile in Directory.GetFiles(jumpListPath, JumpListFileExtension))
                        {
                            try
                            {
                                string fileContents = File.ReadAllText(linkFile);
                                foreach (string x in removalPatterns)
                                {
                                    if (fileContents.ToLower().Contains(x))
                                    {
                                        filesToDelete.Add(linkFile);
                                        break;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine("Error reading jump list file: " + ex.Message);
                            }
                        }
                    }

                    // Perform deletions
                    foreach (string file in filesToDelete)
                    {
                        try
                        {
                            System.Diagnostics.Debug.WriteLine(string.Format("Deleting file: {0}", file));
                            File.Delete(file);
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine(string.Format("exception: {0}", ex.Message));
                        }
                    }

					// debug
					//string message = "files to delete";
					//foreach (string file in filesToDelete)
					//{
					//	message += Environment.NewLine + file;
					//}
					//MessageBox.Show(message);

					// do the registry
					int regValueCount = CleanRegistry(removalPatterns);

					// put a status message on the UI
					UpdateStatusText(string.Format("{0} {1} : Deleted {2} files, {3} registry values.", 
						DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString(), filesToDelete.Count, regValueCount));

					System.Diagnostics.Debug.WriteLine(string.Format("RunTask() done for thread \"{0}\"", Thread.CurrentThread.Name));
				}
				catch (Exception ex)
				{
                    System.Diagnostics.Debug.WriteLine("RunTask unhandled exception: " + ex.Message);
				}
				finally
				{
					if (shell != null)
                    {
                        Marshal.FinalReleaseComObject(shell);
                    }

					Monitor.Exit(myLock);
				}
			}
			else
			{
				System.Diagnostics.Debug.WriteLine(string.Format("Thread named \"{0}\" was unable to enter RunTask", Thread.CurrentThread.Name));
			}
		}

		private int CleanRegistry(List<string> removalPatterns)
		{
			// Computer\HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\TypedPaths
			int deletedCount = 0;
			string keyName = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\TypedPaths";
			using (RegistryKey key = Registry.CurrentUser.OpenSubKey(keyName, true))
			{
				if (key != null)
				{
					var vNames = key.GetValueNames();
					foreach (var valueName in vNames)
					{
						foreach (string x in removalPatterns)
                        {
							if (Convert.ToString(key.GetValue(valueName)).ToLower().Contains(x))
							{
								key.DeleteValue(valueName);
								deletedCount++;
								break;
							}
						}
					}

					// rename them remaining values else they won't be visible
					// e.g. if value names are  url1, url2, url4, url5    then url4 and url5 will not be visible in Explorer droplist
					if (deletedCount > 0)
					{
						vNames = key.GetValueNames();
						for (int i = 0; i < vNames.Length; i++)
                        {
							string desiredName = string.Format("url{0}", i + 1);
							string currentName = vNames[i];
							if (currentName != desiredName)
                            {
								// according to microsoft docs you cannot programmatically rename a registry key,
								// you have to create a new one and delete the old one
								//string debug = string.Format("{0} --> {1}", currentName, desiredName);
								//Console.WriteLine(debug);
								key.SetValue(desiredName, key.GetValue(currentName));
								key.DeleteValue(currentName);
							}

						}

					}

				}
			}

			return deletedCount;
		}

        private void UpdateStatusText(string text)
        {
            if (labelStatus.InvokeRequired)
            {
                labelStatus.Invoke((MethodInvoker)delegate
                {
                    labelStatus.Text = text;
                });
            }
            else
            {
                labelStatus.Text = text;
            }
        }

		private void listBox1_KeyUp(object sender, KeyEventArgs e)
		{
			// allow user to delete from listbox by using the delete key
			if (e.KeyCode == Keys.Delete && listBox1.SelectedIndex > -1)
			{
				listBox1.Items.RemoveAt(listBox1.SelectedIndex);
				SaveSettings();
			}
		}

		private void txtRunInterval_TextChanged(object sender, EventArgs e)
		{
			Int32 foo;
			if (Int32.TryParse(txtRunInterval.Text, out foo))
			{
				SaveSettings();
			}
			else
			{
				MessageBox.Show("Run interval is not valid, resetting to previous value.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				txtRunInterval.Text = ConfigurationManager.AppSettings["RunIntervalMinutes"];
			}
		}

		private void btnAddDirectory_Click(object sender, EventArgs e)
		{
			FolderBrowserDialog dlg = new FolderBrowserDialog();
			DialogResult result = dlg.ShowDialog();
			if (result == DialogResult.OK)
			{
				listBox1.Items.Add(dlg.SelectedPath);
				SaveSettings();
			}
		}

		private void btnAddFilename_Click(object sender, EventArgs e)
		{
			var form = new AddFilenameDlg();
			if (form.ShowDialog() == DialogResult.OK && !String.IsNullOrEmpty(form.PartialFilename))
			{
				listBox1.Items.Add(form.PartialFilename);
				SaveSettings();
			}
		}

		private void SaveSettings()
		{
			// convert list to a pipe-delimited string
			var sb = new System.Text.StringBuilder();
			foreach (string entry in listBox1.Items)
			{
				sb.AppendFormat("{0}|", entry);
			}
			string removeThese = sb.ToString().TrimEnd('|');

			// write to app.config
			Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
			config.AppSettings.Settings.Remove("RemoveThese");
			config.AppSettings.Settings.Add("RemoveThese", removeThese);
			config.AppSettings.Settings.Remove("RunIntervalMinutes");
			config.AppSettings.Settings.Add("RunIntervalMinutes", txtRunInterval.Text);
			config.Save(ConfigurationSaveMode.Modified);
			
			// restart worker thread
			RestartTaskThread();
		}
	}
}