using System;
using BoaBeePCL;

namespace BoaBeePCL
{
	public class AnsweredForm
	{
		public Answer[] answers { get; set; }
		public string enddate { get; set; }
		public string name { get; set; }
		public string startdate { get; set; }
		public DeviceUser user { get; set; }
        public string contactUid { get; set; }
	}
}

