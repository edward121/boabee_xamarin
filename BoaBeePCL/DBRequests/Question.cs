using System;
using System.Collections.Generic;

namespace BoaBeePCL
{
	public class Question
	{
		public string question { get; set; }
		public List<string> options { get; set; }
		public int? maxLength { get; set; }
		public bool required { get; set; }
		public string name { get; set; }
		public string type { get; set; } // ['string' or 'datetime' or 'date' or 'integer'],
		public int? questionNumber  { get; set; }
//		public object additionalProperties { get; set; }

		public Question ()
		{
			//options = new string[]{ };
		}
	}
}

