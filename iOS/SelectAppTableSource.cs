using System;
using System.Collections.Generic;

using Foundation;

using UIKit;

namespace BoaBee.iOS
{
	public class SelectAppTableSource : UITableViewSource
	{
		public List<string> _tableItems { get; private set; }

		private readonly string appCellIdentifier = @"appCell";

		public SelectAppTableSource ()
		{
		}

		public SelectAppTableSource (List<string> tableItems)
		{
			_tableItems = tableItems;
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			string tableItem = _tableItems [indexPath.Row];
			AppCell appCell = (AppCell)tableView.DequeueReusableCell (this.appCellIdentifier);

			if (appCell == null)
			{
				appCell = new AppCell (new IntPtr ());
			}

			appCell.appName = tableItem;
			appCell.initCell ();
			return appCell;
		}

//		public override void WillDisplay (UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
//		{
//			
//			((AppCell)cell).ContentView.LayoutIfNeeded();
//		}

		public override nint RowsInSection (UITableView tableview, nint section)
		{
			return _tableItems.Count;
		}

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			tableView.DeselectRow (indexPath, true);

			NSDictionary notificationUserInfo = NSDictionary.FromObjectAndKey (indexPath, new NSString ("indexPath"));

			NSNotificationCenter.DefaultCenter.PostNotificationName ("SelectAppViewControllerRowSelected", null, notificationUserInfo);
		}
	}
}

