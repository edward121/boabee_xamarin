using System;
using Foundation;

namespace BoaBee.iOS
{
	public static class DateFormatter
	{
		public static string formatDateTime(NSDate date)
		{
			NSDateFormatter formatter = new NSDateFormatter();
			formatter.DateFormat = "dd MMM yyyy HH:mm";
			formatter.TimeZone = NSTimeZone.SystemTimeZone;

			return formatter.ToString(date);
		}

		public static string formatDate(NSDate date)
		{
			NSDateFormatter formatter = new NSDateFormatter();
			formatter.DateFormat = "dd MMM yyyy";
			formatter.TimeZone = NSTimeZone.SystemTimeZone;

			return formatter.ToString(date);
		}

		public static NSDate parseDate(string date)
		{
			NSDateFormatter formatter = new NSDateFormatter();
			formatter.DateFormat = "dd MMM yyyy";
			formatter.TimeZone = NSTimeZone.SystemTimeZone;

			return formatter.Parse(date);
		}

		public static NSDate parseDateTime(string dateTime)
		{
			NSDateFormatter formatter = new NSDateFormatter();
			formatter.DateFormat = "dd MMM yyyy HH:mm";
			formatter.TimeZone = NSTimeZone.SystemTimeZone;
			
			return formatter.Parse(dateTime);
		}
	}
}

