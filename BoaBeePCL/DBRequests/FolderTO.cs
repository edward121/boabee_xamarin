using System;
using System.Runtime.CompilerServices;

namespace BoaBeePCL
{
	public class FolderTO
	{
		public string uuid { get; set; }
		public string name { get; set; }
		public string fileType { get; set; }
		public FileTO[] files { get; set; }
		public FolderTO[] folders { get; set; }
		public UrlTO[] urls { get; set; }

//		public FolderTO ()
//		{
//		}
	}
}

