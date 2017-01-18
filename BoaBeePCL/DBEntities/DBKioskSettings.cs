using System;

namespace BoaBeePCL
{
	public class DBKioskSettings
	{
		public string kioskTitle { get; set; }

		public bool badgePrinting {get; set; }
		public string badgePrintingWebhook { get; set; }

		public DBKioskSettings()
		{
		}
	}
}

