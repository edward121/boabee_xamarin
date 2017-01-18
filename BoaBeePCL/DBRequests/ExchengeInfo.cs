using System;

namespace BoaBeePCL
{
    public class ExchengeInfo
    {
        public bool success { get; set;}
		public string error { get; set;}
		public DBBasicAuthority[] profiles { get; set; }

		public ExchengeInfo ()
		{
		}
    }
}
