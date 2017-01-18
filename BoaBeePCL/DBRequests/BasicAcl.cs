using System;
using BoaBeePCL;

namespace BoaBeePCL
{
	public class BasicAcl
	{
		public DBBasicAuthority[] contributors { get; set; }
		public DBBasicAuthority[] consumers { get; set; }
		public bool writeAccessForConsumers { get; set; }

		public BasicAcl ()
		{
		}
	}
}

