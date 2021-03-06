// This file has been autogenerated from a class added in the UI designer.

using System;
using System.Collections.Generic;

using Foundation;
using UIKit;

using BoaBeePCL;
using Newtonsoft.Json;
using System.Drawing;
using BoaBeeLogic;

namespace BoaBee.iOS
{
	public partial class SelectAppViewController : UIViewController
	{
		//Filled from login view
		public List<DBBasicAuthority> dbAppEntities { private get; set; }

		public bool isFromLogin;

		public bool shouldUpdateProfiles = true;

		private List<string> selectAppNames { get; set; } = new List<string>();

		private NSObject notificationToken;

		private UINavigationController lastAppNavigation = null;

		private LoadingOverlay overlay = null;

		private bool isAllFilesLoaded = false;

		public SelectAppViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			this.selectAppTableView.EstimatedRowHeight = 44;
			this.selectAppTableView.RowHeight = UITableView.AutomaticDimension;

			if (this.isFromLogin)
			{
				//Prepare app names for view into table
				if (this.dbAppEntities == null)
				{
					return;
				}

				this.dbAppEntities.Sort(delegate(DBBasicAuthority x, DBBasicAuthority y)
				{
                    return x.displayName.CompareTo(y.displayName);
				});

				foreach (DBBasicAuthority appEntity in this.dbAppEntities)
				{
					this.selectAppNames.Add (appEntity.displayName);
				}

				//Load data into table
				this.selectAppTableView.Source = null;
				this.selectAppTableView.ClearsContextBeforeDrawing = true;
				this.selectAppTableView.Source = new SelectAppTableSource (this.selectAppNames);
				this.selectAppTableView.ReloadData ();

				this.View.Alpha = 0;
				DBBasicAuthority lastApp = DBLocalDataStore.GetInstance().GetSelectProfile();
				if (lastApp != null)
				{
                    var appInfo = DBLocalDataStore.GetInstance().GetAppInfo();

					this.isAllFilesLoaded = NSUserDefaults.StandardUserDefaults.BoolForKey("AlLFilesDownloaded");
					if (this.isAllFilesLoaded)
					{
						UINavigationController restoreAppNavigationController;
						if (appInfo != null)
						{
                            //#warning temporary disabled kiosk
							if (appInfo.appType.Equals("kiosk"))
							{
								restoreAppNavigationController = (UINavigationController)this.Storyboard.InstantiateViewController("LastKioskNavigationController");
							}
							else
							{
								restoreAppNavigationController = (UINavigationController)this.Storyboard.InstantiateViewController("LastAppNavigationController");
							}

                            //restoreAppNavigationController = (UINavigationController)this.Storyboard.InstantiateViewController("LastAppNavigationController");
						}
						else
						{
							restoreAppNavigationController = (UINavigationController)this.Storyboard.InstantiateViewController("LastAppNavigationController");
						}

						lastAppNavigation = restoreAppNavigationController;

						this.PresentViewController(restoreAppNavigationController, false, () =>
						{
							this.View.Alpha = 1;
							this.isFromLogin = false;
						});
					}
					else
					{
						UINavigationController loadingVC = (UINavigationController)this.Storyboard.InstantiateViewController("AppLoadingNavigationController");
						this.PresentViewController(loadingVC, false, () =>
						{
							this.View.Alpha = 1;
							this.isFromLogin = false;
						});
					}
				}
				else
				{
					this.View.Alpha = 1;
				}
			}
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

//			this.selectAppTableView.ReloadData ();

			//subscribe tableSource selection callback notification
			this.notificationToken = NSNotificationCenter.DefaultCenter.AddObserver (new NSString ("SelectAppViewControllerRowSelected"), new Action<NSNotification> (rowSelectedCallback));
		}

        public override async void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            if (!this.isFromLogin && this.shouldUpdateProfiles)
            {
                var user = DBLocalDataStore.GetInstance().GetLocalUserInfo();

                this.overlay = new LoadingOverlay((RectangleF)this.View.Frame, "Loading Data...");
                this.View.AddSubview(overlay);

                await NetworkRequests.performAuth(user, (success, messageTitle, message, list) =>
                {
                    this.InvokeOnMainThread(() =>
                    {
                        this.overlay.RemoveFromSuperview();

                        if (!success)
                        {
                            UIAlertController alertController = UIAlertController.Create(messageTitle, message, UIAlertControllerStyle.Alert);
                            alertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, (UIAlertAction obj) =>
                            {
                                this.selectAppSwitchUserBtnTouchUpInside(null);
                            }));

                            this.PresentViewController(alertController, true, null);
                        }
                        else
                        {
                            this.dbAppEntities = list;

                            this.dbAppEntities.Sort(delegate (DBBasicAuthority x, DBBasicAuthority y)
                            {
                                return x.displayName.CompareTo(y.displayName);
                            });

                            this.selectAppNames.Clear();
                            foreach (DBBasicAuthority appEntity in this.dbAppEntities)
                            {
                                this.selectAppNames.Add(appEntity.displayName);
                            }

                            this.selectAppTableView.ClearsContextBeforeDrawing = true;
                            this.selectAppTableView.Source = new SelectAppTableSource(this.selectAppNames);
                            this.selectAppTableView.ReloadData();
                        }
                    });
                });
            }
            this.shouldUpdateProfiles = true;
        }

        public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);

			//unsubscribe tableSource selection callback notification
			if (this.notificationToken != null)
			{
				this.notificationToken.Dispose();
			}
		}

		private async void rowSelectedCallback(NSNotification notification)
		{
			NSIndexPath selectedAppIndexPath = (NSIndexPath)notification.UserInfo.ObjectForKey (new NSString ("indexPath"));

			if (await Reachability.isConnected())
			{
				DBBasicAuthority selectedApp = this.dbAppEntities[selectedAppIndexPath.Row];
				DBBasicAuthority lastApp = DBLocalDataStore.GetInstance().GetSelectProfile();

				if (lastApp != null && lastApp.displayName.Equals(selectedApp.displayName))
				{
					this.isFromLogin = false;
					this.PerformSegue("toAppLoading", null);
					return;
				}

				DBLocalDataStore.GetInstance().ClearAllFiles();
				DBLocalDataStore.GetInstance().ClearAllFolders();

				DBLocalDataStore.GetInstance().ClearAppInfo();
                DBLocalDataStore.GetInstance().resetAnswers();
                DBLocalDataStore.GetInstance().clearSyncRequests();
                DBLocalDataStore.GetInstance().ClearAllContactsToServer();

				DBLocalDataStore.GetInstance().AddSelectProfile(selectedApp.displayName);

				this.isFromLogin = false;
				this.PerformSegue("toAppLoading", null);
			}
			else
			{
				new UIAlertView(null, "No internet access, try again later.", null, "OK", null).Show();
			}
		}

		partial void selectAppCancelBtnTouchUpInside (Foundation.NSObject sender)
		{
			if (lastAppNavigation != null)
			{
				this.PresentViewController(lastAppNavigation, true, null);
			}
		}

		partial void selectAppSwitchUserBtnTouchUpInside (Foundation.NSObject sender)
		{
			var shouldAutologin = new DBUserAutologin();
			shouldAutologin.shouldAutologin = false;
			DBLocalDataStore.GetInstance().SetShouldAutologin(shouldAutologin);

//			DBLocalDataStore.GetInstance().ClearUserInfo();
			DBLocalDataStore.GetInstance().ClearSelectProfile();

			this.NavigationController.PopViewController(true);
		}

		public override void PrepareForSegue (UIStoryboardSegue segue, NSObject sender)
		{
            string segueID = segue.Identifier;

            if (segueID.Equals ("toAppLoading") || segueID.Equals ("toLastSelectedApp"))
			{
				lastAppNavigation = (UINavigationController)segue.DestinationViewController;
			}
		}
	}
}
