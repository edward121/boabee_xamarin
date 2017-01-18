using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text.Method;
using Android.Views;
using Android.Widget;

using Android.Views.Animations;

using Android.Graphics;
using System.Globalization;
using Android.Text;

using SQLite;
using BoaBeePCL;

namespace Leadbox
{
	public class ContactsListAdapter : BaseAdapter<DBlocalContact>
    {
		private List<DBlocalContact> _items;
        private Activity _context;
		EventHandler clickEdit = null;
		EventHandler clickCustomer = null;

		public ContactsListAdapter(Activity context, List<DBlocalContact> items, EventHandler _clickEdit, EventHandler _clickCustomer)
            : base()
        {
            _context = context;
            _items = items;
			clickEdit = _clickEdit;
			clickCustomer = _clickCustomer;
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
			View pendingView = convertView ?? LayoutInflater.From (_context).Inflate (Resource.Layout.ListItemContact, parent, false);
			Typeface font = Typeface.CreateFromAsset (Application.Context.Assets, "HelveticaNeueLTStd-Lt.otf");
			// what happens if there is nothing in this position?

			var item = _items [position];


			TextView text = pendingView.FindViewById<TextView> (Resource.Id.textViewDisplayName);
			TextView editCustomers = pendingView.FindViewById<TextView> (Resource.Id.editContact);

			text.SetTypeface (font, TypefaceStyle.Normal);
			editCustomers.SetTypeface (font, TypefaceStyle.Normal);
			editCustomers.Visibility = ViewStates.Visible;

			if (string.IsNullOrEmpty (item.company)) {
                text.Text = item.firstname + " " + item.lastname;
			} else {
                text.Text = item.firstname + " " + item.lastname + " (" + item.company + ")";
			}

			check_valid (item, text, editCustomers);

			text.Tag = position;
			text.Click -= clickCustomer;
			text.Click += clickCustomer;

			editCustomers.Tag = position;
			editCustomers.Click -= clickEdit;
			editCustomers.Click += clickEdit;

			return pendingView;
        }


        public override long GetItemId(int position)
        {
            return position;
        }

        public override int Count
        {
            get { return _items.Count; }
        }

		public override DBlocalContact this[int position]
        {
            get { return _items[position]; }
        }

		public override void NotifyDataSetChanged ()
		{
			base.NotifyDataSetChanged ();
		}

		public override void NotifyDataSetInvalidated ()
		{
			base.NotifyDataSetInvalidated ();
		}
        ///////////////////////////////////////////WARNING BADGE/////////////////////////////////////////////////////////////////       
        void check_valid(DBlocalContact contact, TextView _text, TextView _editText)
		{
			//Console.WriteLine ("Barcod every customer = " + contact.barcode);
            if (string.IsNullOrEmpty (contact.lastname) &&
                string.IsNullOrEmpty (contact.firstname) &&
			   string.IsNullOrEmpty (contact.company)) {
                if (!string.IsNullOrEmpty (contact.uid)) {
                    _text.Text = "badge"+" "+contact.uid;
                    Console.WriteLine (contact.uid);
					_editText.Visibility = ViewStates.Gone;
					return;
				}
			}

            if (string.IsNullOrEmpty (contact.lastname) &&
                string.IsNullOrEmpty (contact.firstname)) {
                if (!string.IsNullOrEmpty (contact.uid)) {
                    _text.Text = "badge"+" "+contact.uid;
                    Console.WriteLine (contact.uid);
					_editText.Visibility = ViewStates.Gone;
					return;
				}
			}

		}
			
    }
}