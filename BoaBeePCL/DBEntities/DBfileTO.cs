using System;
using System.Runtime.CompilerServices;
using SQLite;

namespace BoaBeePCL
{
	[Table("dbfileto")]
	public class DBfileTO
	{
		[PrimaryKey,Column("_id")/*, AutoIncrement*/]
		//public int Id { get; set; }
		public string uuid { get; set; }

		public string name { get; set; }
		public string fileType { get; set; }
		public string md5 { get; set; }
		public string downloadUrl { get; set; }
		public string mimeType { get; set; }
		public string folderUuid { get; set; }
		public string localpath { get; set; }
        public bool activeFile { get; set; }
        public bool isDefault { get; set; }

//		public FileTO ()
//		{
//		}
	}
}

