using System;
using System.Collections.Generic;

using Foundation;

using BoaBeePCL;

namespace BoaBee.iOS
{
	public static class DownloadManager
	{
		public static NSUrlSession currentDownloadSession = null;

		public static void downloadFilesForApp(string appName, bool isKiosk = false)
		{
			DownloadSession session = new DownloadSession (appName/*, URLs*/);
			session.startSession (isKiosk);
		}

        public static void downloadFilesForApp(bool isKiosk = false)
        {
            DownloadSession session = new DownloadSession();
            session.startSession(isKiosk);
        }
	}
}

