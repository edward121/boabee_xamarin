using System;

namespace BoaBeePCL
{
	public class BrandTappUser
	{
		public string objectId { get; set; }
		public string objectName { get; set; }
		public string uuid { get; set; }
		public bool hidden { get; set; }
		public BrandTappMetadata metadata { get; set; }
		public BasicAcl acl { get; set; }
		public OrganizationMetadata organization { get; set; }
		public int objectState { get; set; }
		public BrandTappSecurityProfile[] consumerProfiles { get; set; }
		public BrandTappSecurityProfile[] contributorProfiles { get; set; }
		public string[] applicationRoles { get; set; }
		public bool showFirstLoginWizardOnNextLogin { get; set; }
		public bool showTermsAndConditionsWizardOnNextLogin { get; set; }
		public bool showPasswordResetWizardOnNextLogin { get; set; }
		public string lastTermsAndConditionAcceptedDate { get; set; }
		public string userName { get; set; }
		public bool enabled { get; set; }
		public string firstName { get; set; }
		public string lastName { get; set; }
		public string jobtitle { get; set; }
		public string telephone { get; set; }
		public string mobile { get; set; }
		public string email { get; set; }
		public string password { get; set; }
		public string companyaddress1 { get; set; }
		public string companyaddress2 { get; set; }
		public string companyaddress3 { get; set; }
		public string companypostcode { get; set; }
		public string companytelephone { get; set; }
		public string companyfax { get; set; }

		public BrandTappUser ()
		{
		}
	}
}

