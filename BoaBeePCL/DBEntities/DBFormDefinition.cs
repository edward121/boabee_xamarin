using System;
using System.Collections.Generic;

namespace BoaBeePCL
{
	public class DBFormDefinition
	{
		public string objectId { get; set; }
		public string objectName { get; set; }
		public string uuid { get; set; }
		public bool hidden { get; set; }
		//public BrandTappMetadata metadata { get; set; }
		//public BasicAcl acl { get; set; }
		//public OrganizationMetadata organization { get; set; }
		public int? objectState { get; set; }
		//public List<Question> questions { get; set; }
		public bool isDefault {get; set;}
	}
}

