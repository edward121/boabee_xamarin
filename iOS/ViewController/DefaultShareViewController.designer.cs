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
	[Register ("DefaultShareViewController")]
	partial class DefaultShareViewController
	{
		[Outlet]
		UIKit.UIButton filesOverviewButton { get; set; }

		[Outlet]
		UIKit.UITableView foldersTableView { get; set; }

		[Outlet]
		UIKit.UILabel noDocumentationLabel { get; set; }

		[Action ("crossButtonClick:")]
		partial void crossButtonClick (UIKit.UIButton sender);

		[Action ("filesOverviewButtonClick:")]
		partial void filesOverviewButtonClick (UIKit.UIButton sender);

		[Action ("plusButtonClick:forEvent:")]
		partial void plusButtonClick (UIKit.UIButton sender, UIKit.UIEvent @event);

		[Action ("tickButtonClick:")]
		partial void tickButtonClick (UIKit.UIButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (foldersTableView != null) {
				foldersTableView.Dispose ();
				foldersTableView = null;
			}

			if (filesOverviewButton != null) {
				filesOverviewButton.Dispose ();
				filesOverviewButton = null;
			}

			if (noDocumentationLabel != null) {
				noDocumentationLabel.Dispose ();
				noDocumentationLabel = null;
			}
		}
	}
}
