using System;

namespace BoaBeePCL
{
	public class OrderType
	{

//		public string objectId { get; set; }
//		public string objectName { get; set; }
//		public string uuid { get; set; }
//		public bool hidden { get; set; }
//		public BrandTappMetadata metadata { get; set; }
//		public BasicAcl acl { get; set; }
//		public OrganizationMetadata organization { get; set; }
//		public int objectState { get; set; }

		public string created { get; set; }
		public string creator { get; set; }

//		public int amountReduction { get; set; }
//		public int percentReduction { get; set; }
//		public string currency { get; set; }
//		public string note { get; set; }
//		public int totalAmount { get; set; }
//		public int totalItems { get; set; }
//		public int orderStatus { get; set; }
		public string profile { get; set; }

		public CustomerType customer { get; set; }

//		public int objectStatus { get; set; }
//		public string organizationUuid { get; set; }
//		public string sentEmailUuid { get; set; }
//		public string campaignUuid { get; set; }
//		public string campaignAppUuid { get; set; }
		public OrderLineType[] orderLine { get; set; }

	}
}

