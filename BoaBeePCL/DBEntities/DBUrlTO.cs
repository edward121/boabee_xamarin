using System;
using SQLite;

namespace BoaBeePCL
{
	[Table("dburlto")]
	public class DBUrlTO
	{
		[PrimaryKey,Column("_id")]
		public string uuid { get; set; }

		public string name { get; set; }
		public string fileType { get; set; }
		public string link { get; set; }
		public string folderUuid { get; set; }
	}
}

