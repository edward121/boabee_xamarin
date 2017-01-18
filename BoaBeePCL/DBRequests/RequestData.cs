using System;

namespace BoaBeePCL
{
	public class RequestData
	{
		public string username { get; set; }
		public string password { get; set; }
		public string appname { get; set; }
		public string appversion { get; set; }
		public string customer { get; set; }
		public string locale { get; set; }
		public string devicemodel { get; set; }
		public string osversion { get; set; }
		public string pushId { get; set; }
		public string profile { get; set; }
		public string devicename { get; set; }
		public string[] profiles { get; set; }
        public string[] tags { get; set; }
        public string campaignReference { get; set; }
    }
}

