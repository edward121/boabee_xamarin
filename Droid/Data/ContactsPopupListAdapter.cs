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
using BoaBeePCL;
using SQLite;

namespace Leadbox
{
    public class ContactsPopupListAdapter : BaseAdapter<DBlocalContact>
    {
        private List<DBlocalContact> _items;
        private Activity _context;
		EventHandler delete_item;


        public ContactsPopupListAdapter(Activity context, List<DBlocalContact> items, EventHandler _delete_item)
            : base()
        {
            _context = context;
            _items = items;
			delete_item = _delete_item;
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View pendingView = convertView ?? LayoutInflater.From (_context).Inflate (Resource.Layout.ListItemContactDialog, parent, false);
            Typeface font = Typeface.CreateFromAsset (Application.Context.Assets, "HelveticaNeueLTStd-Lt.otf");
            // what happens if there is nothing in this position?

            var item = _items [position];

            var btnDelete = pendingView.FindViewById<ImageView> (Resource.Id.imageBtnDelete);
            TextView text = pendingView.FindViewById<TextView> (Resource.Id.textViewDisplayName);

            text.SetTypeface (font, TypefaceStyle.Normal);
                if (string.IsNullOrEmpty(item.company))
                    text.Text = item.firstname + " " + item.lastname;
                else
                    text.Text = item.firstname + " " + item.lastname + " (" + item.company + ")";

                check_valid(item, text);

                btnDelete.Tag = position;
                btnDelete.Click -= delete_item;
                btnDelete.Click += delete_item;

                //			pendingView.Click -= (object sender, EventArgs e) => {
                //				_context.StartActivity(typeof(ActivityIdetifyScreen));
                //			};
                //
                //			pendingView.Click += (object sender, EventArgs e) => {
                //				_context.StartActivity(typeof(ActivityIdetifyScreen));
                //			};
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

        void check_valid(DBlocalContact contact, TextView _text)
		{
            Console.WriteLine ("Barcod every customer = " + contact.uid);
            if (string.IsNullOrEmpty (contact.lastname) &&
                string.IsNullOrEmpty (contact.firstname) &&
				string.IsNullOrEmpty (contact.company)) {
                if (!string.IsNullOrEmpty (contact.uid)) {
                    _text.Text = "badge "+contact.uid;
                    Console.WriteLine (contact.uid);
					return;
				}
			}

            if (string.IsNullOrEmpty (contact.lastname) &&
                string.IsNullOrEmpty (contact.firstname)) {
                if (!string.IsNullOrEmpty (contact.uid)) {
                    _text.Text = "badge "+ contact.uid;
                    Console.WriteLine (contact.uid);
					return;
				}
			}

		}

			
    }
}