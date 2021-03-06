// This file has been autogenerated from a class added in the UI designer.

using System;
using System.Collections.Generic;

using Foundation;
using UIKit;

using BoaBeePCL;

namespace BoaBee.iOS
{
    public partial class SelectQuestionsViewController : UIViewController, IUITableViewDataSource, IUITableViewDelegate
	{
        private List<DBFormDefinition> formDefinitions = new List<DBFormDefinition>();

		public SelectQuestionsViewController (IntPtr handle) : base (handle)
		{
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.formDefinitions = DBLocalDataStore.GetInstance().GetLocalFormDefinitions();

            this.formDefinitions.Sort(delegate(DBFormDefinition x, DBFormDefinition y)
                {
                    return x.objectName.CompareTo(y.objectName);
                });
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            if (this.questionnaireTableView.Frame.Height >= this.questionnaireTableView.ContentSize.Height)
            {
                this.questionnaireTableView.ScrollEnabled = false;
                this.questionnaireTableView.Bounces = false;
            }
            else
            {
                this.questionnaireTableView.ScrollEnabled = true;
                this.questionnaireTableView.Bounces = true;
            }
        }

        partial void closeButtonClick (UIKit.UIButton sender)
        {
            this.DismissViewController(true, null);
        }

        public nint RowsInSection(UITableView tableView, nint section)
        {
            return this.formDefinitions.Count;
        }

        public UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            SelectQuestionsCell cell = (SelectQuestionsCell)tableView.DequeueReusableCell("cell", indexPath);

            return cell;
        }

        [Export("tableView:willDisplayCell:forRowAtIndexPath:")]
        public void WillDisplay(UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
        {
            ((SelectQuestionsCell)cell).initWithFormDefinition(this.formDefinitions[indexPath.Row]);
        }

        [Export("tableView:didSelectRowAtIndexPath:")]
        public void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            tableView.DeselectRow(indexPath, true);

            DBLocalDataStore.GetInstance().SetSelectedFormDefinitions(this.formDefinitions[indexPath.Row].uuid);

			DBLocalDataStore.GetInstance().resetAnswers();

            this.PerformSegue("toMyWork", null);
        }
	}
}
