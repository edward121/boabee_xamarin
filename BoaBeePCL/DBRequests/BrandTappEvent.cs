using System;

namespace BoaBeePCL
{
	public class BrandTappEvent
	{
		public string objectId { get; set; }
		public string objectName { get; set; }
		public string uuid { get; set; }
		public bool hidden { get; set; }
		public BrandTappMetadata metadata { get; set; }
		public BasicAcl acl { get; set; }
		public OrganizationMetadata organization { get; set; }
		public int objectState { get; set; }
		public int eventId { get; set; }
		public string eventLocation { get; set; }
		public GisCoordinates eventGisLocation { get; set; }
		public string startDate { get; set; }
		public string endDate { get; set; }
		public string timeZone { get; set; }
		public BrandTappUser[] eventManagers { get; set; }
		public BrandTappUser[] customerManagers { get; set; }
//		scanners (array[Entry«string,BrandTappEventDevice»], optional),
//		printers (array[Entry«string,BrandTappEventDevice»], optional),
//		topics (array[Entry«string,BrandTappEventTopic»], optional)

		public BrandTappEvent ()
		{
		}
	}
}

