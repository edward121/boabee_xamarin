using System;

namespace BoaBeePCL
{
	public class CampaignAppUpdateRequestResult
	{
		public bool success { get; set; }
		public string error { get; set; }
		public DBBasicAuthority[] profiles { get; set; }
		public FolderTO[] folders { get; set; }
		public FormDefinition[] forms { get; set; }
		public FormDefinition defaultForm { get; set; }
		public string appType { get; set; }
		public FileTO welcomeScreenImage {get; set; }
		public FileTO finishedScreenImage { get; set; }
        public string campaignReference { get; set; }

		public CampaignAppUpdateRequestResult ()
		{
//			profiles = new DBBasicAuthority[]{ };
//			folders = new FolderTO[]{ };
//			forms = new FormDefinition[]{ };
		}
	}
}

