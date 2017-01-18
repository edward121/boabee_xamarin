using System;

namespace BoaBeePCL
{
	public class DBAppInfo
	{
		public string appType { get; set; }

		public string welcomeImageURL {get; set; }
		public string finishedImageURL { get; set; }

		public string welcomeImageLocalPath {get; set; }
		public string finishedImageLocalPath { get; set; }

		public string welcomeImageFileType {get; set; }
		public string finishedImageFileType {get; set; }

		public string welcomeImageMD5 {get; set; }
		public string finishedImageMD5 {get; set; }

        public string campaignReference { get; set; }

		public DBAppInfo()
		{
		}
	}
}