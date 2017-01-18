using System;
using SQLite;

namespace BoaBeePCL
{
	[Table("DBShowLocalFiles")]
	public class DBShowLocalFiles
	{
		public bool show {get; set; }
	}
}

