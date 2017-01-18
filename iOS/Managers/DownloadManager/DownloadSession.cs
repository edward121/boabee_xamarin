using BoaBeePCL;

using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;

using Foundation;

using UIKit;
//using System.Security.Policy;
using System.Security.Policy;

namespace BoaBee.iOS
{
	public class DownloadSession  : NSUrlSessionDownloadDelegate
	{
		private Dictionary<string, string> URLs = new Dictionary<string, string>();
        private IDictionary<string, string> finishedURLs = new Dictionary<string, string>();
		private List<DBfileTO> localFiles;
		private int retryCount = 0;

		private bool isInvalidated;

		public DownloadSession (string sessionID)
		{
			//this.sessionID = sessionID;
		}

        public DownloadSession()
        {

        }

		public void startSession(bool isKiosk)
		{
			DBSelectProfile selectedProfile = DBLocalDataStore.GetInstance().GetSelectProfile();
			Console.Error.WriteLine("downloading files for {0} profile", selectedProfile.displayName);

			NSUrlSessionConfiguration configuration = NSUrlSessionConfiguration.DefaultSessionConfiguration;

			NSUrlSession session = NSUrlSession.FromConfiguration (configuration, this, NSOperationQueue.CurrentQueue);

			if (DownloadManager.currentDownloadSession != null) 
			{
				DownloadManager.currentDownloadSession.InvalidateAndCancel ();
			}
			DownloadManager.currentDownloadSession = session;

			this.localFiles = DBLocalDataStore.GetInstance ().GetOnlyLocalFiles(false);
            DBFileToEqualityComparer comparer = new DBFileToEqualityComparer();
            this.localFiles = this.localFiles.Distinct(comparer).ToList();

			if (isKiosk)
			{
                DBAppInfo appInfo = DBLocalDataStore.GetInstance().GetAppInfo();
				NSFileManager fileManager = NSFileManager.DefaultManager;

				var documentsDirectory = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
				string destPath = System.IO.Path.Combine (documentsDirectory, string.Format("{0}.{1}", appInfo.welcomeImageMD5, appInfo.welcomeImageFileType));

				if (!fileManager.FileExists(destPath))
				{
					if (!string.IsNullOrWhiteSpace(appInfo.welcomeImageURL))
					{
						Console.Error.WriteLine("Added welcome file for kiosk to download list");

						URLs.Add(appInfo.welcomeImageURL, "Welcome");
					}
				}
				else
				{
					appInfo.welcomeImageLocalPath = string.Format("{0}.{1}", appInfo.welcomeImageMD5, appInfo.welcomeImageFileType);
				}

				destPath = System.IO.Path.Combine (documentsDirectory, string.Format("{0}.{1}", appInfo.finishedImageMD5, appInfo.finishedImageFileType));

				if (!fileManager.FileExists(destPath))
				{
					if (!string.IsNullOrWhiteSpace(appInfo.finishedImageURL))
					{
						Console.Error.WriteLine("Added finished file for kiosk to download list");

						URLs.Add(appInfo.finishedImageURL, "Finished");
					}
				}
				else
				{
					appInfo.finishedImageLocalPath = string.Format("{0}.{1}", appInfo.finishedImageMD5, appInfo.finishedImageFileType);
				}

				DBLocalDataStore.GetInstance().SetAppInfo(appInfo);
			}

			foreach (DBfileTO file in this.localFiles)
			{
				var documentsDirectory = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
				string destPath = System.IO.Path.Combine (documentsDirectory, string.Format("{0}.{1}", file.md5, file.fileType));

				NSFileManager fileManager = NSFileManager.DefaultManager;
				if (!fileManager.FileExists(destPath) && !string.IsNullOrWhiteSpace(file.downloadUrl))
				{
					//if (!URLs.ContainsKey(file.downloadUrl))
					{
						Console.Error.WriteLine("Added file {0} \nwith URL {1} \nand type {2}\nto download list", file.name, file.downloadUrl, file.fileType);

						URLs.Add(file.downloadUrl, file.fileType);
					}
				}
				else
				{
					if (string.IsNullOrWhiteSpace(file.downloadUrl))
					{
						Console.Error.WriteLine("No download URL for \"{0}.{1}\"", file.name, file.fileType);
					}
					else
					{
						file.localpath = System.IO.Path.GetFileName(destPath);
						DBLocalDataStore.GetInstance().UpdateLocalFile(file);
					}
				}
			}

			Console.Error.WriteLine ("downloading {0} URLs", this.URLs.Count);

			int percentValue = 0;
			if (this.localFiles.Count != 0)
			{
				percentValue = (int)(Math.Floor(50 + (50 - ((double)this.URLs.Count / this.localFiles.Count) * 50)));
			} 
			else
			{
				percentValue = 100;
			}

			string percentValueString = percentValue.ToString () + "%";
			NSNotificationCenter.DefaultCenter.PostNotificationName ("AppLoadingProgressDidUpdate", null, NSDictionary.FromObjectAndKey((NSString)percentValueString, (NSString)"NewValue"));

			if (this.URLs.Count > 0)
			{
				foreach (KeyValuePair<string, string> URL in this.URLs)
				{
					//create task & launch it
					this.downloadFile(session, URL);
				}
			} 
			else
			{
				NSNotificationCenter.DefaultCenter.PostNotificationName ("AppLoadingProgressDidUpdate", null, NSDictionary.FromObjectAndKey((NSString)"100%", (NSString)"NewValue"));
				NSNotificationCenter.DefaultCenter.PostNotificationName("AppLoadingDidFinish", null);
			}
		}

		private void downloadFile(NSUrlSession session, KeyValuePair<string, string> URL)
		{
			NSUrlSessionDownloadTask task = session.CreateDownloadTask(new NSUrl(URL.Key), (NSUrl location, NSUrlResponse response, NSError error) => 
			{
				if (error != null)
				{
					if (error.LocalizedDescription.Contains("timed out"))
					{
						Console.Error.WriteLine("timed out");
						this.downloadFile(session, URL);
						return;
					}

					this.retryCount++;
					Console.Error.WriteLine("{0} while downloading {1}", error.LocalizedDescription, URL.Key);
					Console.Error.WriteLine("Retry count = {0}", this.retryCount);

					if (this.retryCount > 5)
					{
						if (!this.isInvalidated)
						{
							this.isInvalidated = true;
							session.InvalidateAndCancel();
							this.InvokeOnMainThread(()=>
							{
								NSNotificationCenter.DefaultCenter.PostNotificationName("AppLoadingDidFail", null, NSDictionary.FromObjectAndKey((NSString)error.LocalizedDescription, (NSString)"error"));
							});
						}
					}
					else
					{
						this.downloadFile(session, URL);
					}

					return;
				}

                this.finishedURLs.Add(URL);
                int filesLeft = this.URLs.Count - this.finishedURLs.Count;
                Console.Error.WriteLine (filesLeft.ToString () + " files until finish");

				int newValue = 0;
				if (this.localFiles.Count != 0)
				{
                    newValue = (int)(Math.Floor(50 + (50 - ((double)filesLeft / this.localFiles.Count) * 50)));
				} 
				else
				{
					newValue = 100;
				}

				string newValueString = newValue.ToString() + "%";
				NSNotificationCenter.DefaultCenter.PostNotificationName ("AppLoadingProgressDidUpdate", null, NSDictionary.FromObjectAndKey((NSString)newValueString, (NSString)"NewValue"));

				NSData tmpFileData = NSData.FromUrl (location);

				Console.Error.WriteLine("File extension is {0}", URL.Value);
				string extension = URL.Value;

				this.moveTemporaryFileToDocuments (tmpFileData, extension);

                //if (this.URLs.Count == 0) 
                if (this.URLs.ToList().TrueForAll(kvp => this.finishedURLs.Contains(kvp)))
				{
					session.InvalidateAndCancel ();

					NSNotificationCenter.DefaultCenter.PostNotificationName("AppLoadingDidFinish", null);
				}
			});

			Console.Error.WriteLine("Queued URL {0} \nand type {1}\nto download", URL.Key, URL.Value);

			task.Resume ();
		}

		public override void DidWriteData (NSUrlSession session, NSUrlSessionDownloadTask downloadTask, long bytesWritten, long totalBytesWritten, long totalBytesExpectedToWrite)
		{

		}

		public override void DidCompleteWithError (NSUrlSession session, NSUrlSessionTask task, NSError error)
		{
			
		}

		public override void DidFinishDownloading (NSUrlSession session, NSUrlSessionDownloadTask downloadTask, NSUrl location)
		{
			
		}

		private void moveTemporaryFileToDocuments(NSData fileData, string extension_)
		{
			string md5 = this.getMD5 (fileData);
			string extension = extension_;

			var documentsDirectory = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
			string destPath = System.IO.Path.Combine (documentsDirectory, string.Format("{0}.{1}", md5, extension));

			if (extension.StartsWith("Welcome"))
			{
                DBAppInfo appInfo = DBLocalDataStore.GetInstance().GetAppInfo();
				extension = appInfo.welcomeImageFileType;

				destPath = System.IO.Path.Combine (documentsDirectory, string.Format("{0}.{1}", md5, extension));
				appInfo.welcomeImageMD5 = md5;
				appInfo.welcomeImageLocalPath = string.Format("{0}.{1}", md5, extension.ToLower());

				Console.Error.WriteLine("Updating welcome image for kiosk");

				DBLocalDataStore.GetInstance().SetAppInfo(appInfo);
			}
			else if (extension.StartsWith("Finished"))
			{
                DBAppInfo appInfo = DBLocalDataStore.GetInstance().GetAppInfo();
				extension = appInfo.finishedImageFileType;

				destPath = System.IO.Path.Combine (documentsDirectory, string.Format("{0}.{1}", md5, extension));
				appInfo.finishedImageMD5 = md5;
				appInfo.finishedImageLocalPath = string.Format("{0}.{1}", md5, extension.ToLower());

				Console.Error.WriteLine("Updating finished image for kiosk");

				DBLocalDataStore.GetInstance().SetAppInfo(appInfo);
			}
			else
			{
				List<DBfileTO> dbFiles = DBLocalDataStore.GetInstance().GetOnlyLocalFilesByMD5(md5);
				foreach (var file in dbFiles)
				{
					file.localpath = string.Format("{0}.{1}", md5, extension.ToLower());
					Console.Error.WriteLine("Updating local path for file {0}", file.name);

					DBLocalDataStore.GetInstance ().UpdateLocalFile (file);
				}
			}

			NSFileManager fileManager = NSFileManager.DefaultManager;
			if (fileManager.CreateFile(destPath, fileData, (NSDictionary)null))
			{
				Console.Error.WriteLine("Successfully saved file");
			}
		}

		private string getMD5(NSData data)
		{
			string md5Hash = string.Empty;
			MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider ();
			try
			{
				md5Hash = BitConverter.ToString(md5Hasher.ComputeHash (data.ToArray ()));
				md5Hash = md5Hash.Replace("-", "").ToLower();
			}
			catch
			{
				md5Hash = string.Empty;
			}
			return md5Hash;
		}
	}
}

