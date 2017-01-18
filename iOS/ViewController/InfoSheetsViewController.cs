// This file has been autogenerated from a class added in the UI designer.

using System;

using Foundation;
using UIKit;
using System.Collections.Generic;
using BoaBeePCL;
using System.Linq;
using CoreGraphics;
using Newtonsoft.Json;

namespace BoaBee.iOS
{
	public partial class InfoSheetsViewController : UIViewController, IUITableViewDataSource, IUITableViewDelegate
	{
		public InfoSheetsViewController (IntPtr handle) : base (handle)
		{
		}

        private List<DBSyncRequest> syncRequests;
        private List<DBlocalContact> contactsMet;

		private NSObject kbFrameChangeNoificationToken;

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

            this.contactsMet = new List<DBlocalContact>();
            this.syncRequests = DBLocalDataStore.GetInstance().getSyncRequests().ToList();

            syncRequests.ForEach((r) =>
            {
                SyncContext context = JsonConvert.DeserializeObject<SyncContext>(r.serializedSyncContext);
                context.forms.ForEach((f) =>
                {
                    var contact = DBLocalDataStore.GetInstance().GetLocalContactsByUID(f.contactUid);
                    this.contactsMet.Add(contact);
                });
            });

            infoSheetsLabel.Text = this.contactsMet.Count.ToString() + " INFO SHEETS";

			this.contactsTableView.EstimatedRowHeight = 44;
			this.contactsTableView.RowHeight = UITableView.AutomaticDimension;

			UIStringAttributes placeholderAttributes = new UIStringAttributes();
			placeholderAttributes.ForegroundColor = UIColor.FromRGB(0x71, 0x71, 0x71);

			NSAttributedString searchPlaceholder = new NSAttributedString (this.searchTextField.Placeholder, placeholderAttributes);

			this.searchTextField.AttributedPlaceholder = searchPlaceholder;
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

            if (!this.IsMovingToParentViewController)
            {
                this.contactsMet = new List<DBlocalContact>();
                this.syncRequests = DBLocalDataStore.GetInstance().getSyncRequests().ToList();

                syncRequests.ForEach((r) =>
                {
                    SyncContext context = JsonConvert.DeserializeObject<SyncContext>(r.serializedSyncContext);
                    context.forms.ForEach((f) =>
                    {
                        var contact = DBLocalDataStore.GetInstance().GetLocalContactsByUID(f.contactUid);
                        this.contactsMet.Add(contact);
                    });
                });
            }

			this.contactsTableView.ReloadData();
		}

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);

			this.kbFrameChangeNoificationToken = UIKeyboard.Notifications.ObserveWillChangeFrame(kbFrameChangeCallback);
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);

			if (this.kbFrameChangeNoificationToken != null)
			{
				this.kbFrameChangeNoificationToken.Dispose();
				this.kbFrameChangeNoificationToken = null;
			}
		}

		private void kbFrameChangeCallback (object sender, UIKeyboardEventArgs args)
		{
			CGRect beginFrame = args.FrameBegin;
			CGRect endFrame = args.FrameEnd;

			UIView.Animate(args.AnimationDuration, 
				() =>
				{
					this.bottomButtonsConstraint.Constant += (beginFrame.Y - endFrame.Y);
					this.View.LayoutIfNeeded();
				});
		}

		partial void searchTextFieldEditingChanged (UITextField sender)
		{
			this.contactsTableView.ReloadData();
		}

		partial void searchTextFieldEditingDidBegin (UITextField sender)
		{

		}

		partial void searchTextFieldEditingDidEnd (UITextField sender)
		{

		}

		partial void cancelButtonClick (UIButton sender)
		{
			this.NavigationController.PopViewController(true);
		}

		public nint RowsInSection(UITableView tableView, nint section)
		{
            var request = this.syncRequests[(int)section];
            SyncContext context = JsonConvert.DeserializeObject<SyncContext>(request.serializedSyncContext);

            if (this.searchTextField.Text.Length > 0)
            {
                var listMatched = context.forms.Where(f => this.contactsMet.Where(s => string.Join(" ", new string[] { s.firstname, s.lastname, s.company }).ToLower().Contains(this.searchTextField.Text.ToLower())).ToList().Find(c => c.uid.Equals(f.contactUid)) != null).ToList();
                var count = listMatched.Count;
                return count;
            }

            return context.forms.Count;
        }

        [Export("numberOfSectionsInTableView:")]
        public nint NumberOfSections(UITableView tableView)
        {
            return this.syncRequests.Count;
        }

		public UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			ContactInfoSheetCell cell = (ContactInfoSheetCell)tableView.DequeueReusableCell("ContactCell", indexPath);

            var request = this.syncRequests[indexPath.Section];
            SyncContext context = JsonConvert.DeserializeObject<SyncContext>(request.serializedSyncContext);

            DBlocalContact contact = null;
			if (this.searchTextField.Text.Length > 0)
			{
                contact = DBLocalDataStore.GetInstance().GetLocalContactsByUID(context.forms.Where(f => this.contactsMet.Where(s => string.Join(" ", new string[] { s.firstname, s.lastname, s.company }).ToLower().Contains(this.searchTextField.Text.ToLower())).ToList().Find(c => c.uid.Equals(f.contactUid)) != null).ToList()[indexPath.Row].contactUid);
			}
			else
			{
                contact = DBLocalDataStore.GetInstance().GetLocalContactsByUID(context.forms[indexPath.Row].contactUid);
			}

            cell.initWithContact(contact, DateTime.Parse(context.forms[indexPath.Row].startdate));
			return cell;
		}

		[Export("tableView:didSelectRowAtIndexPath:")]
		public void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			tableView.DeselectRow(indexPath, true);

            var request = this.syncRequests[indexPath.Section];

            Console.Error.WriteLine("Saved request is: {0}", request.serializedSyncContext);

            SyncContext context = JsonConvert.DeserializeObject<SyncContext>(request.serializedSyncContext);

            DBlocalContact contact = null;

            AnsweredForm form = null;
            if (this.searchTextField.Text.Length > 0)
            {
                form = context.forms.Where(f => this.contactsMet.Where(s => string.Join(" ", new string[] { s.firstname, s.lastname, s.company }).ToLower().Contains(this.searchTextField.Text.ToLower())).ToList().Find(c => c.uid.Equals(f.contactUid)) != null).ToList()[indexPath.Row];
            }
            else
            {
                form = context.forms[indexPath.Row];
            }

            contact = DBLocalDataStore.GetInstance().GetLocalContactsByUID(form.contactUid);
            NSMutableDictionary dict = new NSMutableDictionary();
            dict.Add((NSString)"request", (NSString)JsonConvert.SerializeObject(request));
            dict.Add((NSString)"contactUID", (NSString)form.contactUid);

            Console.Error.WriteLine(string.Join(" ", contact.firstname, contact.lastname));

            this.PerformSegue("toInfoSheetDetails", dict);
		}

		public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
		{
			if (segue.Identifier.Equals("toInfoSheetDetails"))
			{
				InfoSheetViewController target = (InfoSheetViewController)segue.DestinationViewController;

				NSDictionary dict = (NSDictionary)sender;

                DBSyncRequest request = JsonConvert.DeserializeObject<DBSyncRequest>(dict["request"].ToString());
                string contactUID = dict["contactUID"].ToString();

                target.request = request;
                target.contactUID = contactUID;
            }
		}
	}
}
