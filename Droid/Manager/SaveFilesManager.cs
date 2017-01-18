using System;
using System.IO;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Java.Security.Acl;
using System.Threading;
//using Android.Net;
using Android.App;
using Android.Widget;
using Android.Content;
using System.Security.AccessControl;
using BoaBeePCL;
using System.Reflection;
using System.Linq;

namespace Leadbox
{
	public class SaveFilesManager
	{

		System.Timers.Timer t;
		private static SaveFilesManager _instance;
		private static readonly object _lockCreate = new object();
		/// <summary>
		/// The count all files.
		/// </summary>
		private int count_all_files;
		/// <summary>
		/// The count need to download files.
		/// </summary>
		private int count_need_files;
		List<DBfileTO> _list_files_need_download;
		ConnectManager.StatusDownload lg;
        string _path_app_dir = "";// Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
		public bool check =true; 
		private long freememory;

		private Activity _activ;

		private SaveFilesManager() {
		}

		public static SaveFilesManager GetInstance()
		{
			lock (_lockCreate)
			{
				if (_instance == null)
				{
					_instance = new SaveFilesManager();
				}
				return _instance;
			}
		}

		/// <summary>
		/// Sets the URL path.
		/// </summary>
		/// <returns>The URL path.</returns>
		/// <param name="_dir">Directory which need to save.</param>
		public void SetUrlPath()
		{
			string libraryPath = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			//var documents = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath;//dir of sdcard(local, in phone)
			var directoryname = Path.Combine (libraryPath, "BoaBee");
			//directoryname = Path.Combine (directoryname, _dir);
			if (!Directory.Exists (directoryname)) {
				Directory.CreateDirectory (directoryname);
				_path_app_dir = directoryname;
				return;
			
			}
			_path_app_dir = directoryname;
			//return directoryname;
		}

		public int GetCountAllFiles(FolderTO[] items)
		{
			int _count = 0;
			foreach (var i in items) {
				_count += i.files.Length;
				_count += GetCountAllFiles (i.folders);
			}
			return _count;
		}

		public List<Java.IO.File> GetAllPath()
		{
			var listdir = new List<Java.IO.File> ();
			//var directories = Directory.EnumerateFileSystemEntries(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath);
			var f_d = new  Java.IO.File (Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, "BoaBee"));
			if (!f_d.Exists())
				return null;
			
			foreach (var directory in f_d.ListFiles()) {
				Console.WriteLine (directory.Name);
				listdir.Add (directory);
			}
			return listdir;
		}

		public Java.IO.File GetParentByDir(string _path_dir)
		{
			var _dir = new Java.IO.File (_path_dir);
			//Console.WriteLine (_dir.ParentFile.Name);
			if (_dir.ParentFile == null)
				return null;

			return _dir.ParentFile;
		}

		public List<Java.IO.File> GetAllPathByDir(string _dir)
		{
			var listdir = new List<Java.IO.File> ();
			var __dir = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, "BoaBee");
			var directories = new  Java.IO.File(Path.Combine(__dir, _dir));

			foreach (var directory in directories.ListFiles()) {
				Console.WriteLine(directory);
				listdir.Add (directory);

			}
			return listdir;
		}

		public void CreateAllFolder(ConnectManager.StatusDownload _lh, ConnectManager.StatusDownload dai, Activity _context)
		{
			lg = _lh;
			_activ = _context;
			_list_files_need_download = DBLocalDataStore.GetInstance ().GetOnlyLocalFiles ();
			count_need_files = _list_files_need_download.Count;
			SetUrlPath ();
			if (count_need_files > 0)
				downloadAllFiles (_list_files_need_download, dai);
			else {
				lg (true, "FINISH");
				 SaveFilesManager.GetInstance ().downloadFileAppType (dai);
				//_activ.StartActivity (typeof(ActivitySelectQuestion));
				//_activ.Finish ();
			}

		}

		public void downloadAllFiles (List<DBfileTO> _items, ConnectManager.StatusDownload dai)
		{
			count_all_files = 0;
			//int result = 0;
			Console.WriteLine ("Files to be need to download = " + _list_files_need_download.Count);
			try {
				for (int a = 0; a < _items.Count; a++) {
					if (string.IsNullOrEmpty (_items [a].localpath)) {
						string localFilename = _items [a].md5 + "." + _items [a].fileType;
						if (File.Exists (Path.Combine (_path_app_dir, localFilename))) {
							_items [a].localpath = Path.Combine (_path_app_dir, localFilename);
							DBLocalDataStore.GetInstance ().UpdateLocalFile (_items [a]);
                            var tt = DBLocalDataStore.GetInstance().GetAllLocalFiles();
                            var filesnull = tt.Where(s => s.localpath == null).ToList();

							Console.WriteLine ("===[BOABEE] Sendign file: [" + _items [a].name + "] to " + _items [a].localpath);
							count_all_files++;
							lg (true, "UP");
						} else {
							Console.WriteLine ("Number file need download = " + a);
							downloadFile (_items [a], a);
						}


					} else {
						if (!string.IsNullOrEmpty (_items [a].localpath) && !File.Exists (_items [a].localpath)) {
							Console.WriteLine ("Number file need download (File not exist) = " + a);
							downloadFile (_items [a], a);
							//return;
						} else {
							lg (true, "UP");
						}

					}


				}
			} catch (TaskCanceledException ex) {
				Console.WriteLine ("Downloaded canceled");
				lg (false, "Downloaded canceled");
			} catch (Exception ex) {
				Console.WriteLine (ex.Message);
				lg (false, "Downloaded canceled");
			} 
				downloadFileAppType (dai);
		}

		public void downloadFile (DBfileTO item, int _position)
		{
			try{

				var webClient = new WebClient ();
				webClient.DownloadDataCompleted += (s, e) => {
					var _klll = new Java.IO.File(_path_app_dir);
					string documentsPath = _path_app_dir;
					//string localFilename = item.name + "__" + item.folderUuid;
					string localFilename = item.md5 + "." + item.fileType;
					string localPath = Path.Combine (documentsPath, localFilename);
					freememory = _klll.FreeSpace;
					var documents = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;

					if (e.Error == null){
						if(e.Result.LongLength < freememory)
						{
							File.WriteAllBytes(localPath, e.Result);
							new Java.IO.File(localPath).SetReadable(true);
							Console.WriteLine ("count_all_files = " + count_all_files);
							item.localpath = localPath;
							DBLocalDataStore.GetInstance ().UpdateLocalFile (item);
							Console.WriteLine ("===[BOABEE] Sendign file: [" + item.name + "] to " + localPath);
							//CheckAllFilesDownloaded();
							count_all_files++;
							lg (true, "UP");
							Console.WriteLine("freememory = " + freememory);
						}
						else if(e.Result.LongLength < new Java.IO.File(documents).FreeSpace)
						{
							File.WriteAllBytes(Path.Combine (documents, localFilename), e.Result);
							Console.WriteLine ("count_all_files = " + count_all_files);
							item.localpath = localPath;
							DBLocalDataStore.GetInstance ().UpdateLocalFile (item);
							Console.WriteLine ("===[BOABEE] Sendign file: [" + item.name + "] to " + Path.Combine (documents, localFilename));
							//CheckAllFilesDownloaded();
							count_all_files++;
							lg (true, "UP");
						}
						else
						{
							Console.WriteLine ("count_all_files = " + count_all_files);
							item.localpath = localPath;
							DBLocalDataStore.GetInstance ().UpdateLocalFile (item);
							Console.WriteLine ("===[BOABEE] Sendign file: [" + item.name + "] to " + localPath);
							count_all_files++;
							lg (true, "UP");
						}
					}
					else{
						Console.WriteLine (e.Error);
						lg (false, "Downloaded canceled.\nPlease try again later.");
						return;
					}
				};

				var url = new Uri (item.downloadUrl);
				Console.WriteLine ("Download file start is - " + item.name);
				webClient.DownloadDataAsync (url);
			}
			catch (TargetInvocationException ex) {
				Console.WriteLine (ex.Message);
				lg (false, "Downloaded canceled.\nPlease try again later.");
			}
			catch (TargetParameterCountException ex) {
				Console.WriteLine (ex.Message);
				lg (false, "Downloaded canceled.\nPlease try again later.");
				//_activ.Finish();

			}
			catch (TaskCanceledException ex) {
				Console.WriteLine (ex.Message);
				lg (false, "Downloaded canceled.\nPlease try again later.");
			}
			catch(Exception ex) {
				Console.WriteLine (ex.Message);
				lg (false, "Downloaded canceled.\nPlease try again later.");
			}
		}
        public void bildingPatchImageKiosk(DBAppInfo item,string localPath, DownloadDataCompletedEventArgs e,ConnectManager.StatusDownload statusLog)
		{
			File.WriteAllBytes(localPath, e.Result);
			new Java.IO.File(localPath).SetReadable(true);
			Console.WriteLine ("count_all_files kiosk = " + count_all_files);
			if(localPath == "localPathFinish")
			{
				item.finishedImageLocalPath = localPath;
			}
			else if(localPath == "localPathWelcom")
			{
				item.welcomeImageLocalPath = localPath;
			}
			DBLocalDataStore.GetInstance ().UpdateLocalFileAppType (item);
			//Console.WriteLine ("===[BOABEE] Sendign file: [" + item.name + "] to " + localPath);
			//CheckAllFilesDownloaded();
			count_all_files++;
			statusLog (true, "UP");
			Console.WriteLine("freememory = " + freememory);
		}
		public void downloadFileAppType (ConnectManager.StatusDownload statusLog)
		{
            DBAppInfo item = DBLocalDataStore.GetInstance().GetAppInfo();
			if (item.appType == "prospect" || item.appType == null) {
				return;
			}
			try
			{
				var webClient = new WebClient ();
				webClient.DownloadDataCompleted += (s, e) => {
					var _klll = new Java.IO.File(_path_app_dir);
                    string documentsPath = _path_app_dir; //Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

					if(e.Error == null)
					{ 
						if(check == true)
						{
						string localFilename = item.welcomeImageMD5 + "." + item.welcomeImageFileType;
							string destPath = System.IO.Path.Combine (documentsPath, localFilename);

							File.WriteAllBytes(destPath, e.Result);

							item.welcomeImageLocalPath = string.Format(documentsPath,"{0}.{1}", item.welcomeImageMD5, item.welcomeImageFileType);

                        Console.WriteLine ("===[BOABEE] Sendign file KIOSk: [" + item.welcomeImageMD5 + "] to " + item.welcomeImageLocalPath);


							check = false;
							var url = new Uri (item.finishedImageURL);
							webClient.DownloadDataAsync (url);
						}
						else if(check == false)
						{
							string localFilename = item.finishedImageMD5 + "." + item.finishedImageFileType;
							string destPath = System.IO.Path.Combine (documentsPath, localFilename);
							File.WriteAllBytes(destPath, e.Result);
							statusLog (true, "finished");
							item.finishedImageLocalPath = string.Format(documentsPath,"{0}.{1}", item.finishedImageMD5, item.finishedImageFileType);
							Console.WriteLine ("===[BOABEE] Sendign file KIOSk: [" + item.finishedImageMD5 + "] to " + item.finishedImageLocalPath);



						}
                        DBLocalDataStore.GetInstance().SetAppInfo(item);
					}
					else
					{
						
					}
				};

				    var urlk = new Uri (item.welcomeImageURL);
					webClient.DownloadDataAsync (urlk);

				//Console.WriteLine ("Download file start is - " + item.name);
			
			}
			catch (TargetInvocationException ex) {
				Console.WriteLine (ex.Message);
				statusLog (false, "Downloaded canceled.\nPlease try again later.");
			}
			catch (TargetParameterCountException ex) {
				Console.WriteLine (ex.Message);
				statusLog (false, "Downloaded canceled.\nPlease try again later.");
				//_activ.Finish();

			}
			catch (TaskCanceledException ex) {
				Console.WriteLine (ex.Message);
				statusLog (false, "Downloaded canceled.\nPlease try again later.");
			}
			catch(Exception ex) {
				Console.WriteLine (ex.Message);
				statusLog (false, "Downloaded canceled.\nPlease try again later.");
			}
		}
		public void DeleteAllFields(Java.IO.File _file)
		{
			if (_file.Exists ()) {
				if (_file.IsDirectory) {
					Console.WriteLine ("Files in dir = " + _file.ListFiles ().Length);

					foreach (Java.IO.File child in _file.ListFiles()) {
						DeleteAllFields (child);
					}

					if (_file.Exists() && _file.ListFiles ().Length == 0)
						Directory.Delete (_file.Path);

					return;
				}
				Console.WriteLine ("File is deleted - " + _file.Name);
				_file.Delete ();
			}
		}


	}
}

