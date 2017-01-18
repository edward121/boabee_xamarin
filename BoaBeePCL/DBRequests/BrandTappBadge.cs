using System;

namespace BoaBeePCL
{
	public class BrandTappBadge
	{
		public string objectId { get; set; }
		public string objectName { get; set; }
		public string uuid { get; set; }
		public bool hidden { get; set; }
		public BrandTappMetadata metadata { get; set; }
		public BasicAcl acl { get; set; }
		public OrganizationMetadata organization { get; set; }
		public int objectState { get; set; }
		public string type { get; set; } //['nfc' or 'ibeacon' or 'barcode' or 'rfid'],
		public IBadgeProperties properties { get; set; }
		public CustomerType customer { get; set; }
		public BrandTappEvent events { get; set; }
		public string campaignUuid { get; set; }

		public BrandTappBadge ()
		{
			type = "barcode";
		}
	}
}

