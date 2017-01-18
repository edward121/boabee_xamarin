using System;
using System.Collections.Generic;

namespace BoaBeePCL
{
	public class SyncContext
	{
		public RequestData context { get; set; }
        public List<CustomerType> contacts { get; set; }
        public List<AnsweredForm> forms { get; set; }
        public List<DBOrder> orders { get; set; }

		public SyncContext ()
		{
		}
	}
    public class UpdateForms
    { 
        public RequestData context { get; set; }
        public List<AnsweredForm> forms { get; set; }
    }
    public class SyncContextNokia
    { 
        public RequestData context { get; set; }
        public CustomerType contact { get; set; }
    }
}

