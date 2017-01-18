using System;

namespace BoaBeePCL
{
	public class DBQuestion
	{
		public string question { get; set; }
		public string options { get; set; }
		public int? maxLength { get; set; }
		public bool required { get; set; }
		public string name { get; set; }
		public string type { get; set; } // ['string' or 'datetime' or 'date' or 'integer'],
		public string uuidgroup { get; set; }

//		public int? questionNumber  { get; set; }
//		public object additionalProperties { get; set; }

		public DBQuestion ()
		{
			//options = new string[]{ };
		}
	}
}

