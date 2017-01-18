using System;

namespace BoaBeePCL
{
	public class BrandTappSecurityProfile
	{
		public string objectId { get; set; }
		public string objectName { get; set; }
		public string uuid { get; set; }
		public bool hidden { get; set; }
		public BrandTappMetadata metadata { get; set; }
		public BasicAcl acl { get; set; }
		public OrganizationMetadata organization { get; set; }
		public int objectState { get; set; }
		public DBBasicAuthority consumerAuthority { get; set; }
		public DBBasicAuthority contributorAuthority { get; set; }
		public string[] consumerMembers { get; set; }
		public string[] contributorMembers { get; set; }
		public string[] applicationRoles { get; set; }

		public BrandTappSecurityProfile ()
		{
		}
	}
}

