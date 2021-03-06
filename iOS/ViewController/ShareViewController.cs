// This file has been autogenerated from a class added in the UI designer.

using System;
using System.Linq;
using System.Collections.Generic;

using CoreGraphics;

using Foundation;
using UIKit;

using BoaBeePCL;
using BoaBeeLogic;

namespace BoaBee.iOS
{
	public partial class ShareViewController : BoaBeeBasePageContentViewController, IUITableViewDataSource, IUITableViewDelegate, IUIDocumentInteractionControllerDelegate
	{
        private List<DBfileTO> currentFolder;
        private List<DBfileTO> allFiles;
        private DBfileTO parentFolder = null;

		private List<DBfileTO> addedFiles = new List<DBfileTO>();

		public ShareViewController (IntPtr handle) : base (handle)
		{
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            NSUserDefaults.StandardUserDefaults.SetValueForKey(NSNumber.FromInt32((int)ProspectLastScreen.Share), (NSString)"ProspectLastScreen");
            NSUserDefaults.StandardUserDefaults.Synchronize();

            this.allFiles = DBLocalDataStore.GetInstance().GetAllLocalFiles();

            DBFileToComparer comparer = new DBFileToComparer();
            this.allFiles.Sort(comparer);

            this.currentFolder = this.allFiles.Where(s => s.folderUuid == "_empty_").ToList();

            foreach (var file in this.allFiles)
            {
                if (file.isDefault || file.activeFile)
                {
                    this.addedFiles.Add(file);
                    file.activeFile = true;
                    DBLocalDataStore.GetInstance().UpdateLocalFile(file);
                }
            }

            this.folderTableView.EstimatedRowHeight = 53;
            this.folderTableView.RowHeight = UITableView.AutomaticDimension;

            if (this.allFiles.Count == 0)
            {
                this.noDocumentationLabel.Hidden = false;
                this.folderTableView.Hidden = true;
            }
            else
            {
                this.noDocumentationLabel.Hidden = true;
                this.folderTableView.Hidden = false;
            }

            this.filesOverviewButton.TitleLabel.TranslatesAutoresizingMaskIntoConstraints = false;
        }

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

            UILabel countLabel = this.filesOverviewButton.TitleLabel;
            this.filesOverviewButton.SetTitle(string.Format("{0}", this.addedFiles.Count), UIControlState.Normal);
			this.folderTableView.ReloadData();
		}

		partial void crossButtonClick(UIButton sender)
        {
            UIAlertController alert = UIAlertController.Create("Warning", "Your current work will be lost.", UIAlertControllerStyle.Alert);
            alert.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Default, null));
            alert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Destructive, (obj) =>
            {
                var selectedContacts = DBLocalDataStore.GetInstance().GetLocalContacts().Where(c => c.activeContact).ToList();
                selectedContacts.ForEach((c) =>
                {
                    c.activeContact = false;
                    DBLocalDataStore.GetInstance().UpdateLocalContact(c);
                });

                this.addedFiles.ForEach((f) =>
                {
                    f.activeFile = false;
                    DBLocalDataStore.GetInstance().UpdateLocalFile(f);
                });

                DBLocalDataStore.GetInstance().resetAnswers();

                this.DismissViewController(true, null);
            }));

            Alert.PresentAlert(alert, true, null);
        }

		partial void tickButtonClick (UIButton sender)
		{
            try
            {
                OfflineLogic.prepareSync();

                this.DismissViewController(true, null);
            }
            catch (Exception e)
            {
                new UIAlertView("Error", e.Message, null, "OK", null).Show();
            }
		}

		partial void filesOverviewButtonClick (UIButton sender)
		{
			if (this.addedFiles.Count > 0)
			{
				this.PerformSegue("toFilesOverview", null);
			}
		}

		partial void plusButtonClick (UIKit.UIButton sender, UIKit.UIEvent @event)
        {
			UITouch touch = (UITouch)@event.TouchesForView(sender).AnyObject;
			CGPoint touchLocation = touch.LocationInView(this.folderTableView);

			NSIndexPath indexPath = this.folderTableView.IndexPathForRowAtPoint(touchLocation);

			DBfileTO selectedFile = this.currentFolder[indexPath.Row];

            if (!selectedFile.activeFile)
            {
                selectedFile.activeFile = true;
                DBLocalDataStore.GetInstance().UpdateLocalFile(selectedFile);

                this.addedFiles.Add(selectedFile);

                sender.Selected = true;
            }
            else
            {
                selectedFile.activeFile = false;
                DBLocalDataStore.GetInstance().UpdateLocalFile(selectedFile);

                int removeIndex = this.addedFiles.FindIndex(s => s.uuid.Equals(selectedFile.uuid));
                this.addedFiles.RemoveAt(removeIndex);

                sender.Selected = false;
            }

            this.filesOverviewButton.SetTitle(string.Format("{0}", this.addedFiles.Count), UIControlState.Normal);

			UILabel countLabel = this.filesOverviewButton.TitleLabel;

			ViewBouncer.bounceViewWithScaleFactor(countLabel, 1.35f);
			UIImageView plusImage = sender.ImageView;
			ViewBouncer.bounceViewWithScaleFactor(plusImage, 1.2f);
        }

        [Export("numberOfSectionsInTableView:")]
        public nint NumberOfSections(UITableView tableView)
        {
            return 2;
        }

        public nint RowsInSection(UITableView tableView, nint section)
        {
            if (section == 0)
            {
                if (this.parentFolder != null)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            return this.currentFolder.Count;
        }

        [Export("tableView:viewForFooterInSection:")]
        public  UIView GetViewForFooter(UITableView tableView, nint section)
        {
            return null;
        }

        [Export("tableView:viewForHeaderInSection:")]
        public  UIView GetViewForHeader(UITableView tableView, nint section)
        {
            return null;
        }

        [Export("tableView:heightForFooterInSection:")]
        public nfloat GetHeightForFooter(UITableView tableView, nint section)
        {
            return new nfloat(0.001);
        }

        [Export("tableView:heightForHeaderInSection:")]
        public nfloat GetHeightForHeader(UITableView tableView, nint section)
        {
            return new nfloat(0.001);
        }

        public UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            if (indexPath.Section == 0)
            {
                BackCell cell = (BackCell)tableView.DequeueReusableCell("BackCell", indexPath);

                cell.initWithParentName(this.parentFolder.name);
                return cell;
            }
            else
            {
				if (this.currentFolder[indexPath.Row].fileType.Equals("folder"))
				{
					DBfileTO folder = this.currentFolder[indexPath.Row];

					FolderCell cell = (FolderCell)tableView.DequeueReusableCell("FolderCell", indexPath);

					cell.initWithFolderName(folder.name);
					return cell;
				}
                else
                {
                    DBfileTO file = this.currentFolder[indexPath.Row];

                    FileCell cell = (FileCell)tableView.DequeueReusableCell("FileCell", indexPath);

					bool isFileAdded = this.addedFiles.Any(s=> s.uuid.Equals(file.uuid));

					cell.initWithFile(file, isFileAdded);
                    return cell;
                }
            }
        }

        [Export("tableView:didSelectRowAtIndexPath:")]
        public void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            tableView.DeselectRow(indexPath, true);

            if (indexPath.Section == 0)
            {
                this.currentFolder = this.allFiles.Where(s => s.folderUuid == parentFolder.folderUuid).ToList();
                try
                {
                    this.parentFolder = this.allFiles.Where(s => s.uuid == currentFolder[0].folderUuid).ToList().First();
                }
                catch
                {
//                    tableView.BackgroundColor = UIColor.Black;
                    this.parentFolder = null;

					this.tableViewTopConstraint.Constant = 12;
					this.View.LayoutIfNeeded();
                }

                tableView.ReloadData();
            }
            else
            {
				DBfileTO selectedItem = this.currentFolder[indexPath.Row];

				if (selectedItem.fileType.Equals("folder"))
				{
					this.parentFolder = selectedItem;
					this.currentFolder = this.allFiles.Where(s => s.folderUuid == parentFolder.uuid).ToList();

					tableView.ReloadData();

//					tableView.BackgroundColor = UIColor.FromRGB(0x1b, 0x1b, 0x1b);

					this.tableViewTopConstraint.Constant = 0;
					this.View.LayoutIfNeeded();
				}
				else if (selectedItem.fileType.Equals("url"))
				{
					bool success = UIApplication.SharedApplication.OpenUrl(new NSUrl(selectedItem.localpath));
					if (!success)
					{
						success = UIApplication.SharedApplication.OpenUrl(new NSUrl(string.Format("http://{0}", selectedItem.localpath)));
						if (!success)
						{
							success = UIApplication.SharedApplication.OpenUrl(new NSUrl(string.Format("https://{0}", selectedItem.localpath)));
							if (!success)
							{
								new UIAlertView(null, "Could not preview selected item", null, "OK", null).Show();
							}
						}
					}
				}
				else
				{
					string documentsDirectory = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
					string filePath = System.IO.Path.Combine(documentsDirectory, selectedItem.localpath);

					NSUrl fileURL = NSUrl.FromFilename(filePath);

					bool success;

					if (selectedItem.fileType.Equals("webloc"))
					{
						NSUrl wwwURL = NSUrl.FromString(selectedItem.localpath);

						success = UIApplication.SharedApplication.OpenUrl(wwwURL);
						if (!success)
						{
							new UIAlertView(null, "Could not preview selected item", null, "OK", null).Show();
						}
					}
					else
					{
						var docController = UIDocumentInteractionController.FromUrl(fileURL);

						docController.Delegate = this;
						docController.Name = selectedItem.name;

						success = docController.PresentPreview(true);
						if (!success)
						{
							new UIAlertView(null, "Could not preview selected item", null, "OK", null).Show();
						}
					}
				}
            }
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            if (segue.Identifier.Equals("toFilesOverview"))
            {
                FilesOverviewViewController target = (FilesOverviewViewController)segue.DestinationViewController;
                target.addedFiles = this.addedFiles;
            }
        }

		[Export("documentInteractionControllerViewControllerForPreview:")]
		public UIViewController ViewControllerForPreview(UIDocumentInteractionController controller)
		{
			return this;
		}

		[Export("documentInteractionControllerWillBeginPreview:")]
		public void WillBeginPreview(UIDocumentInteractionController controller)
		{
			UIApplication.SharedApplication.SetStatusBarStyle (UIStatusBarStyle.Default, true);
		}

		[Export("documentInteractionControllerDidEndPreview:")]
		public void DidEndPreview(UIDocumentInteractionController controller)
		{
			UIApplication.SharedApplication.SetStatusBarStyle (UIStatusBarStyle.LightContent, true);
		}
	}
}
