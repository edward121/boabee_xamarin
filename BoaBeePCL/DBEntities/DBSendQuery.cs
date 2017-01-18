using System;
using SQLite;

namespace BoaBeePCL
{
	public class DBSendQuery
	{
		[PrimaryKey, AutoIncrement, Column("_id"), Unique]
		public string uuid { get; set;}

		public string query { get; set;}
		public int session { get; set;}

	}
}

