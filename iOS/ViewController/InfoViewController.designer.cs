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
	[Register ("InfoViewController")]
	partial class InfoViewController
	{
		[Outlet]
		UIKit.NSLayoutConstraint bottomButtonsConstraint { get; set; }

		[Outlet]
		UIKit.UIButton contactsOverviewButton { get; set; }

		[Outlet]
		UIKit.UILabel noQuestionsLabel { get; set; }

		[Outlet]
		UIKit.UITableView questionsTableView { get; set; }

		[Action ("crossButtonClick:")]
		partial void crossButtonClick (UIKit.UIButton sender);

		[Action ("nextButtonClick:")]
		partial void nextButtonClick (UIKit.UIButton sender);

		[Action ("overviewButtonClick:")]
		partial void overviewButtonClick (UIKit.UIButton sender);

		[Action ("tickButtonClick:")]
		partial void tickButtonClick (UIKit.UIButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (bottomButtonsConstraint != null) {
				bottomButtonsConstraint.Dispose ();
				bottomButtonsConstraint = null;
			}

			if (contactsOverviewButton != null) {
				contactsOverviewButton.Dispose ();
				contactsOverviewButton = null;
			}

			if (noQuestionsLabel != null) {
				noQuestionsLabel.Dispose ();
				noQuestionsLabel = null;
			}

			if (questionsTableView != null) {
				questionsTableView.Dispose ();
				questionsTableView = null;
			}
		}
	}
}
