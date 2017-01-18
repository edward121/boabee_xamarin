using System;
using SQLite;

namespace BoaBeePCL
{
	[Table("DBShareFileContact")]
	public class DBShareFileContact
	{
		[PrimaryKey, AutoIncrement, Column("_id"), Unique]
		public int _id { get; set; }

		public string uuid_contact { get; set; }
		public string uuid_file { get; set; }

		public DBShareFileContact ()
		{
		}
	}
}

