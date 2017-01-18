using System;
using SQLite;

namespace BoaBeePCL
{
	[Table("DBUserAutologin")]
	public class DBUserAutologin
	{
		[PrimaryKey,Column("_id"), AutoIncrement]
		public int id { get; set; }

		public bool shouldAutologin { get; set; }

		public DBUserAutologin()
		{
			
		}
	}
}

