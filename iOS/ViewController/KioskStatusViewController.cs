// This file has been autogenerated from a class added in the UI designer.

using System;

using Foundation;
using UIKit;
using BoaBeePCL;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using BoaBeeLogic;
using System.Threading.Tasks;

namespace BoaBee.iOS
{
	public partial class KioskStatusViewController : UIViewController, IUIAlertViewDelegate
	{
		private NSTimer syncTimer;
		private NSTimer webhookTimer;

		public KioskStatusViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			DBSelectProfile selectedProfile = DBLocalDataStore.GetInstance ().GetSelectProfile ();

			this.appNameLabel.Text = selectedProfile.displayName;
			this.userNameLabel.Text = DBLocalDataStore.GetInstance().GetUserMail();
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

            NetworkRequests.onFinishSync += this.syncFinishCallback;
            NetworkRequests.onFailSync += this.syncFailCallback;
            NetworkRequests.onBeginSync += this.syncBeginCallback;

			this.updateView();
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);

            NetworkRequests.onFinishSync -= this.syncFinishCallback;
            NetworkRequests.onFailSync -= this.syncFailCallback;
            NetworkRequests.onBeginSync -= this.syncBeginCallback;

			if (this.syncTimer != null)
			{
				this.syncTimer.Invalidate();
				this.syncTimer = null;
			}
			if (this.webhookTimer != null)
			{
				this.webhookTimer.Invalidate();
				this.webhookTimer = null;
			}
		}

        private void syncFailCallback()
        {
            UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
        }

        private void syncFinishCallback()
        {
            if (this.syncTimer != null)
            {
                this.syncTimer.Invalidate();
                this.syncTimer = null;
            }

            UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
            this.updateView();
        }

        private void syncBeginCallback()
        {
            UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;

        }

		private void updateView()
		{
			var unsentSyncRequests = DBLocalDataStore.GetInstance().getSyncRequests().Where(s => !s.isSent).ToList();

            this.InvokeOnMainThread(async () =>
            {
                if (unsentSyncRequests.Any())
                {
                    this.statusLabel.Text = "Not all your work is safely in the cloud.\nCheck your internet connection and use the SYNC MANUALLY button if necessary.";
                    this.statusLabel.TextColor = UIColor.FromRGB(0xFF, 0x00, 0x00);

//                    if (await Reachability.isConnected())
//                    {
//                        await NetworkRequests.SyncDataServer();
//                    }
//                    else
//                    {
//                        if (this.syncTimer == null)
//                        {
//                            #warning 60 sec
//                            this.syncTimer = NSTimer.CreateRepeatingScheduledTimer(6, this.timerAction);
//                        }
//                    }
                }
                else
                {
                    this.statusLabel.Text = "All your work is safely stored in the cloud now.\nIt is available in the report via your dashboard.";
                    this.statusLabel.TextColor = UIColor.FromRGB(0xED, 0xCD, 0x00);
                }
                await Task.Delay(1);
            });
		}

		private void timerAction(NSTimer timer)
		{
			Console.Error.WriteLine("Kiosk settings Sync timer fired");

            //if (await Reachability.isConnected())
            //{
            //    this.syncTimer.Invalidate();
            //    this.syncTimer = null;

            //    await NetworkRequests.SyncDataSersver();
            //}
		}

//		private async void webhookTimerAction(NSTimer timer)
//		{
//			Console.Error.WriteLine("Kiosk settings webhook timer fired");
//
//			if (await Reachability.isWebhookAvailable())
//			{
//				var newContactsNotPrinted = DBLocalDataStore.GetInstance().GetOverwievContacts(-1, "new").Where(c=> !c.isSentToWebhook).ToList();
//
//				if (SyncManager.webhookSyncThread == null && newContactsNotPrinted.Count > 0)
//				{
//					SyncManager.webhookSyncThread = new Thread(new ParameterizedThreadStart(SyncManager.syncWebhook));
//					SyncManager.webhookSyncThread.Start(newContactsNotPrinted);
//				}
//
//				this.webhookTimer.Invalidate();
//				this.webhookTimer = null;
//			}
//		}

        partial void resetWorkbuttonClick (UIButton sender)
		{
			UIAlertView alert = new UIAlertView(null, "Are you sure remove all data from MY WORK?", null, "NO", new string[] {"YES"});
			alert.Delegate = this;
			alert.Tag = new nint(AlertTagEnum.resetWork);
			alert.Show();
		}

		async partial void syncManuallyButtonClick (UIButton sender)
		{
			var unsentSyncRequests = DBLocalDataStore.GetInstance().getSyncRequests().Where(s => !s.isSent).ToList();

            if (unsentSyncRequests.Any())
            {
                if (await Reachability.isConnected())
                {
                    await NetworkRequests.SyncDataServer();
                }
                else
                {
                    UIAlertController alert = UIAlertController.Create(null, "There is no internet connection available.\nPlease try again later", UIAlertControllerStyle.Alert);
                    alert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
                    this.PresentViewController(alert, true, null);

                    if (this.syncTimer == null)
                    {
                        //#warning 60 sec
                        //this.syncTimer = NSTimer.CreateRepeatingScheduledTimer(60, this.timerAction);
                    }
                }
            }
            else
            {
                UIAlertController alert = UIAlertController.Create(null, "All your work is safely stored in the cloud", UIAlertControllerStyle.Alert);
                alert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
                this.PresentViewController(alert, true, null);
            }
		}

		[Export("alertView:clickedButtonAtIndex:")]
		public async void alertClicked(UIAlertView alertview, nint buttonIndex)
		{
			const int alertTagSwitchAppSynced = AlertTagEnum.switchAppSynced;
			const int alertTagSwitchAppUnsynced = AlertTagEnum.switchAppUnsynced;
			const int alertTagResetWork = AlertTagEnum.resetWork;

			switch (alertview.Tag)
			{
				case alertTagResetWork:
					{
						if (buttonIndex == 1)
						{
							DBLocalDataStore.GetInstance().clearSyncRequests();
                            this.updateView();
						}
						break;
					}

				case alertTagSwitchAppSynced:
					{
						if (buttonIndex == 1)
						{
							if (await Reachability.isConnected())
							{
								this.DismissViewController(true, null);
							}
							else
							{
								new UIAlertView(null, "There is no internet connection available.\nPlease try again later.", null, "OK", null).Show();
							}
						}
						break;	
					}
				case alertTagSwitchAppUnsynced:
					{
                        if (await Reachability.isConnected())
                        {
                            await NetworkRequests.SyncDataServer();
                        }
						else
						{
							new UIAlertView(null, "There is no internet connection available.\nPlease try again later.", null, "OK", null).Show();
						}
//						if (await Reachability.isWebhookAvailable())
//						{
//							var newOverviewContactsNotPrinted = DBLocalDataStore.GetInstance().GetOverwievContacts(-1, "new").Where(c => !c.isSentToWebhook).ToList();
//							SyncManager.syncWebhook(newOverviewContactsNotPrinted);
//						}
//						else
//						{
//							new UIAlertView(null, "There is no webhook connection available.\nPlease try again later.", null, "OK", null).Show();
//						}
						break;
					}
			}
		}

		partial void closeButtonClicked (UIButton sender)
		{
			this.NavigationController.PopViewController(true);
		} 

		partial void switchAppInvisibleTouchUpInside (UIButton sender)
		{
			const int alertTagSwitchAppSynced = AlertTagEnum.switchAppSynced;
            const int alertTagSwitchAppUnsynced = AlertTagEnum.switchAppUnsynced;

            var unsentSyncRequests = DBLocalDataStore.GetInstance().getSyncRequests().Where(s => !s.isSent).ToList();

            if (unsentSyncRequests.Any())
            {
                UIAlertView alert = new UIAlertView(null, "Not all your work is safely stored in the cloud.\nLet's send it now to the cloud.", null, "OK", null);
                alert.Delegate = this;

                alert.Tag = alertTagSwitchAppUnsynced;

                alert.Show();
            }
            else
            {
                UIAlertView alert = new UIAlertView(null, "All your work is safely stored in the cloud.\nWhen you switch to another app setup you will start with a clean work-status.", null, "CANCEL", new string[] { "CONTINUE" });
                alert.Delegate = this;

                alert.Tag = alertTagSwitchAppSynced;

                alert.Show();
            }
		}

		partial void kioskSettingsInvisibleTouchUpInside (UIButton sender)
		{
			this.PerformSegue("toKioskSettings", null);
		}
			
		partial void defaultSharingButtonClick (UIButton sender)
		{
			this.PerformSegue("toDefaultSharing", null);
		}
	}
}