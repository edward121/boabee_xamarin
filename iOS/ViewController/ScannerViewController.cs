// This file has been autogenerated from a class added in the UI designer.

using System;
using System.Drawing;

using Foundation;
using UIKit;

using CoreGraphics;

using ScanditSDK;
using BoaBeePCL;
using BoaBeeLogic;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BoaBee.iOS
{
    public partial class ScannerViewController : UIViewController
    {
        private SIBarcodePicker barcodePicker;
        private OverlayControllerDelegate scanDelegate;

        private LoadingOverlay overlay;

        private int selectedContactsCount = 0;

        public ScannerViewController (IntPtr handle) : base (handle)
		{
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            NSUserDefaults.StandardUserDefaults.SetValueForKey(NSNumber.FromInt32((int)ProspectLastScreen.Scan), (NSString)"ProspectLastScreen");
            NSUserDefaults.StandardUserDefaults.Synchronize();

            this.barcodePicker = new SIBarcodePicker("npj2MAMeXf41wjXjkbrussTn/5lgnOcGB7OqWk6vAQA", SICameraFacingDirection.Back);

            this.scanDelegate = new OverlayControllerDelegate(this.barcodePicker, this);
            this.barcodePicker.OverlayController.Delegate = this.scanDelegate;
            this.barcodePicker.OverlayController.ShowToolBar(false);
            this.barcodePicker.OverlayController.ShowSearchBar(false);

            SizeF size = new SizeF((float)this.cameraUnderlay.Frame.Width, (float)this.cameraUnderlay.Frame.Height);
            this.barcodePicker.Size = size;
            this.barcodePicker.View.Bounds = new CGRect(0, 0, size.Width, size.Height);
            this.barcodePicker.View.Frame = new CGRect(0, 0, size.Width, size.Height);
            this.cameraUnderlay.AddSubview(this.barcodePicker.View);

            this.scanDelegate.onBeginCheck += () =>
            {
                this.View.AddSubview(overlay);
            };
            this.scanDelegate.onEndCheck += () =>
            {
                this.overlay.RemoveFromSuperview();
            };

            #if __SIMULATOR__
            {
                UILongPressGestureRecognizer longPress = new UILongPressGestureRecognizer((recognizer) =>
                {
                    if (recognizer.State == UIGestureRecognizerState.Began)
                    {
                        Console.Error.WriteLine("Long tap!");

                        //format example: "[["barcode","5160017139930041470033",0],["Name","Gert Stalmans",1]]"
                        Random r = new Random();
                        string randomUID = Guid.NewGuid().ToString().Replace("-", "");
                        string randomName = string.Format("Random Name {0}", r.Next());

                        string randomBarcode = string.Format("[[\"barcode\",\"{0}\",0],[\"Name\",\"{1}\",1]]", randomUID, randomName);

                        NSDictionary barcodeDict = NSDictionary.FromObjectsAndKeys(new NSObject[] {(NSString)"QR",(NSString)randomBarcode }, new NSObject[] { (NSString)"symbology", (NSString)"barcode" });
                        this.scanDelegate.DidScanBarcode(null, barcodeDict);
                    }
                });

                longPress.NumberOfTouchesRequired = 2;
                longPress.MinimumPressDuration = 2;

                this.barcodePicker.View.AddGestureRecognizer(longPress);
            }
            #endif

            this.badgesCountButton.TitleLabel.TranslatesAutoresizingMaskIntoConstraints = false;

            this.selectedContactsCount = DBLocalDataStore.GetInstance().GetLocalContacts().Where(c => c.activeContact).Count();
            if (this.selectedContactsCount > 0)
            {
                this.singleMultiButton.Hidden = true;

                this.badgesCountButton.SetTitle(selectedContactsCount.ToString(), UIControlState.Normal);
                this.badgesCountButton.Hidden = false;

                this.nextButton.Hidden = false;

                this.scanDelegate.isMultiscan = true;
            }
		}

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            this.overlay = new LoadingOverlay((RectangleF)this.View.Frame, "Checking for more details...");

            this.InnerWillAppear();
        }

        private void InnerWillAppear()
        {
            this.selectedContactsCount = DBLocalDataStore.GetInstance().GetLocalContacts().Where(c => c.activeContact).Count();

            if (this.selectedContactsCount > 0)
            {
                this.singleMultiButton.Hidden = true;
                this.badgesCountButton.Hidden = false;

                this.nextButton.Hidden = false;
            }
            else
            {
                this.singleMultiButton.Hidden = false;
                this.badgesCountButton.Hidden = true;

                this.nextButton.Hidden = true;
            }
        }

        private async void InnerDidAppear()
        {
            this.badgesCountButton.SetTitle(this.selectedContactsCount.ToString(), UIControlState.Normal);

            ViewBouncer.bounceViewWithScaleFactor(this.badgesCountButton.TitleLabel, 1.35f);

            await Task.Delay(1000);
            this.barcodePicker.StartScanning();
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            this.InnerDidAppear();
        }

        public void afterScan(DBlocalContact c)
        {
            c.source = ContactSource.Scanned;
            OfflineLogic.createOrUpdateContact(c);

            if (!this.scanDelegate.isMultiscan)
            {
                this.toQuestions();
            }
            else
            {
                var poppedViews = this.NavigationController.PopToViewController(this, true);
                if (poppedViews == null || poppedViews.Length == 0)
                {
                    this.InnerWillAppear();
                    this.InnerDidAppear();
                }
            }
        }

        public void afterScan()
        {
            var poppedViews = this.NavigationController.PopToViewController(this, true);
            if (poppedViews == null || poppedViews.Length == 0)
            {
                this.InnerWillAppear();
                this.InnerDidAppear();
            }
        }

        public override void DismissViewController(bool animated, System.Action completionHandler)
        {
            if (this.barcodePicker != null)
            {
                this.barcodePicker.StopScanning();
                this.barcodePicker.Dispose();
                this.barcodePicker = null;
            }
            if (this.scanDelegate != null)
            {
                this.scanDelegate.Dispose();
                this.scanDelegate = null;
            }
            GC.Collect();

            base.DismissViewController(animated, completionHandler);
        }

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);

            if (this.barcodePicker != null)
            {
                barcodePicker.StopScanning();
            }
            GC.Collect();
		}

        partial void badgesCountButtonClick(UIButton sender)
        {
            if (selectedContactsCount > 0)
            {
                this.PerformSegue("toContactsOverview", null);
            }
        }

        partial void singleMultiButtonClick(UIButton sender)
        {
            if (this.scanDelegate.isMultiscan)
            {
                this.scanDelegate.isMultiscan = false;
                sender.Selected = false;
                UIImage normalImage = UIImage.FromBundle("SingleScan");
                sender.SetImage(normalImage, UIControlState.Normal);
                this.nextButton.Hidden = true;
            }
            else
            {
                this.scanDelegate.isMultiscan = true;
                sender.Selected = true;
                UIImage normalImage = UIImage.FromBundle("MultiScan");
                sender.SetImage(normalImage, UIControlState.Normal);
                this.nextButton.Hidden = false;
            }
        }

        partial void lookupButtonClick(UIButton sender)
        {
            this.PerformSegue("toSelectContact", null);
        }

        partial void manualButtonClick(UIButton sender)
        {
            this.PerformSegue("toManualAdd", null);
        }

        partial void nextButtonClick(UIButton sender)
        {
            this.toQuestions();
        }

        partial void closeButtonClick(UIButton sender)
        {
            UIAlertController alert = UIAlertController.Create("Warning", "Your current work will be lost.", UIAlertControllerStyle.Alert);
            alert.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Default, null));
            alert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Destructive, (obj) =>
            {
                OfflineLogic.ClearDataSelected(false);

                this.DismissViewController(true, null);
            }));

            Alert.PresentAlert(alert, true, null);
        }

		partial void buttonClick (UIButton sender)
		{
			if (this.scanDelegate.isMultiscan)
			{
				this.DismissViewController(true, null);
			}
			else
			{
				this.scanDelegate.isMultiscan = true;
				sender.SetTitle("Cancel", UIControlState.Normal);
			}
		}

        private void toQuestions()
        {
            //this.NavigationController.PopToViewController(this, true);

            var selectedContacts = DBLocalDataStore.GetInstance().GetLocalContacts().Where(c => c.activeContact);
            if (selectedContacts.Count() > 0)
            {
                string formDefinitionUUID = DBLocalDataStore.GetInstance().GetSelectedQuestionPosition();
                if (DBLocalDataStore.GetInstance().GetLocalQuestions(formDefinitionUUID).Count > 0)
                {
                    this.PerformSegue("toInfoViewController", null);
                }
                else
                {
                    Console.Error.WriteLine("No questions available");
                    if (DBLocalDataStore.GetInstance().GetAllLocalFiles().Count > 0)
                    {
                        this.PerformSegue("toShare", null);
                    }
                    else
                    {
                        Console.Error.WriteLine("No files available");

                        OfflineLogic.prepareSync();
                        this.DismissViewController(true, null);
                    }
                }
            }
            else
            {
                UIAlertController alertController = UIAlertController.Create(null, "Please scan or select at least 1 contact", UIAlertControllerStyle.Alert);
                alertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));

                this.PresentViewController(alertController, true, null);
            }
        }

        private void prospectDidSelectContact()
        {
            Console.Error.WriteLine("prospectDidSelectContact");
            //if (!this.scanDelegate.isMultiscan)
            {
                this.toQuestions();
            }
        }

        private void prospectDidCreateContact()
        {
            Console.Error.WriteLine("prospectDidCreateContact");
            //if (!this.scanDelegate.isMultiscan)
            {
                this.toQuestions();
            }
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            if (segue.Identifier.Equals("toSelectContact"))
            {
                SelectContactViewController target = (SelectContactViewController)segue.DestinationViewController;
                target.onProspectSelectContact += this.prospectDidSelectContact;
            }
            if (segue.Identifier.Equals("toManualAdd"))
            {
                NewContactRootViewController target = (NewContactRootViewController)segue.DestinationViewController;
                target.onProspectCreateContact += this.prospectDidCreateContact;
            }
        }

		public class OverlayControllerDelegate : SIOverlayControllerDelegate
		{
			private SIBarcodePicker picker;
            private ScannerViewController presentingViewController;

            private DBlocalContact scannedContact;

			public bool isMultiscan = false;

            public delegate void spinnerDelegate();
            public event spinnerDelegate onBeginCheck;
            public event spinnerDelegate onEndCheck;

			public OverlayControllerDelegate(SIBarcodePicker picker, UIViewController presentingViewController) 
			{
				this.picker = picker;
                this.presentingViewController = (ScannerViewController)presentingViewController;
                scannedContact = null;
            }

            public override async void DidScanBarcode(SIOverlayController overlayController, NSDictionary barcode)
            {
                Console.WriteLine("barcode scanned: {0}, '{1}'", barcode["symbology"], barcode["barcode"]);

                GC.Collect();
                picker.StopScanning();

                string barcodeValue = ((NSString)barcode["barcode"]).ToString();
                string symbology = ((NSString)barcode["symbology"]).ToString();

                ////-- TEST
                //this.finishScan();
                //return;
                ////--

                try
                {
                    if (this.onBeginCheck != null)
                    {
                        this.onBeginCheck();
                    }
                    string returnedMessage = null;
                    this.scannedContact = await OfflineLogic.didScanBarcode(barcodeValue, symbology, (message) =>
                    {
                        Console.Error.WriteLine("Lookup status: {0}", message);
                        returnedMessage = message;
                    });
                    Console.Error.WriteLine("Contact DB ID:{0}", this.scannedContact.Id);

                    DBAppSettings appSettings = DBLocalDataStore.GetInstance().GetAppSettings();
                    if (appSettings == null)
                    {
                        appSettings = new DBAppSettings();
                        appSettings.instantContactCheck = true;
                        DBLocalDataStore.GetInstance().SetAppSettings(appSettings);
                    }

                    if (appSettings.instantContactCheck)
                    {
                        EditScannedContactRootViewController editContactViewController = (EditScannedContactRootViewController)this.presentingViewController.Storyboard.InstantiateViewController("EditScannedContactRootViewController");
                        editContactViewController.message = returnedMessage;
                        editContactViewController.contact = this.scannedContact;
                        editContactViewController.onCommitEdit += this.presentingViewController.afterScan;

                        this.presentingViewController.NavigationController.PushViewController(editContactViewController, true);
                    }
                    else
                    {
                        this.presentingViewController.afterScan(this.scannedContact);
                    }
                }
                catch (Exception e)
                {
                    this.presentingViewController.afterScan();
                    new UIAlertView(null, e.Message, null, "OK", null).Show();
                }
                finally
                {
                    if (this.onEndCheck != null)
                    {
                        this.onEndCheck();
                    }
                }
            }

            private void finishScan()
            {
                if (this.isMultiscan)
                {
                    this.PerformSelector(new ObjCRuntime.Selector("reactivateCamera"), null, 1.5);
                }
                else
                {
                    this.presentingViewController.DismissViewController(true, null);
                }
            }

            public override void DidCancel (SIOverlayController overlayController, NSDictionary status) 
			{

			}

			public override void DidManualSearch (SIOverlayController overlayController, string text) 
			{

			}

			[Export("reactivateCamera")]
			private void reactivateCamera()
			{
                this.scannedContact = null;
				picker.StartScanning();
			}
		}
	}
}