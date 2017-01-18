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
	[Register ("KioskQuestionsViewController")]
	partial class KioskQuestionsViewController
	{
		[Outlet]
		UIKit.NSLayoutConstraint bottomButtonsConstraint { get; set; }

		[Outlet]
		UIKit.UIView bottomButtonsContainerView { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint bottomTableViewConstraint { get; set; }

		[Outlet]
		UIKit.UILabel kioskTitleLabel { get; set; }

		[Outlet]
		UIKit.UITableView questionsTableView { get; set; }

		[Action ("finishButtonClicked:")]
		partial void finishButtonClicked (UIKit.UIButton sender);

		[Action ("previousButtonClicked:")]
		partial void previousButtonClicked (UIKit.UIButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (kioskTitleLabel != null) {
				kioskTitleLabel.Dispose ();
				kioskTitleLabel = null;
			}

			if (questionsTableView != null) {
				questionsTableView.Dispose ();
				questionsTableView = null;
			}

			if (bottomButtonsContainerView != null) {
				bottomButtonsContainerView.Dispose ();
				bottomButtonsContainerView = null;
			}

			if (bottomButtonsConstraint != null) {
				bottomButtonsConstraint.Dispose ();
				bottomButtonsConstraint = null;
			}

			if (bottomTableViewConstraint != null) {
				bottomTableViewConstraint.Dispose ();
				bottomTableViewConstraint = null;
			}
		}
	}
}
