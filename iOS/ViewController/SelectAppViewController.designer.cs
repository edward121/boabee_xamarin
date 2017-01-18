// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace BoaBee.iOS
{
	[Register ("SelectAppViewController")]
	partial class SelectAppViewController
	{
		[Outlet]
		UIKit.UILabel headerLabel { get; set; }

		[Outlet]
		UIKit.UIButton selectAppCancelBtn { get; set; }

		[Outlet]
		UIKit.UIButton selectAppSwitchUserBtn { get; set; }

		[Outlet]
		UIKit.UITableView selectAppTableView { get; set; }

		[Action ("selectAppCancelBtnTouchUpInside:")]
		partial void selectAppCancelBtnTouchUpInside (Foundation.NSObject sender);

		[Action ("selectAppSwitchUserBtnTouchUpInside:")]
		partial void selectAppSwitchUserBtnTouchUpInside (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (headerLabel != null) {
				headerLabel.Dispose ();
				headerLabel = null;
			}

			if (selectAppCancelBtn != null) {
				selectAppCancelBtn.Dispose ();
				selectAppCancelBtn = null;
			}

			if (selectAppSwitchUserBtn != null) {
				selectAppSwitchUserBtn.Dispose ();
				selectAppSwitchUserBtn = null;
			}

			if (selectAppTableView != null) {
				selectAppTableView.Dispose ();
				selectAppTableView = null;
			}
		}
	}
}
