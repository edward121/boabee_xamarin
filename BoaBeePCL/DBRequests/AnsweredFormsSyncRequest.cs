using System;

namespace BoaBeePCL
{
	public class AnsweredFormsSyncRequest
	{
		public RequestData context { get; set; }
		public AnsweredForm[] forms { get; set; }

		public AnsweredFormsSyncRequest ()
		{
		}
	}
}

