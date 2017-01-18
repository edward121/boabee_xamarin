using System;

namespace BoaBeePCL
{
	public class CustomerSyncResult
	{
        public bool success { get; set;}
        public string error { get; set;}
		public DBBasicAuthority[] profiles { get; set; }
		public CustomerType[] contacts { get; set; }


		public CustomerSyncResult ()
		{
		}
	}
}

