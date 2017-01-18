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
using System.Timers;

namespace Leadbox
{
    public class OverviewContactsListAdapter : BaseAdapter<contactData>
    {
        private List<contactData> _items;
        private Activity _context;
		bool flag;


        public OverviewContactsListAdapter(Activity context, List<contactData> items, bool _flag)
            : base()
        {
            _context = context;
            _items = items;
			flag = _flag;
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
			View pendingView = convertView ?? LayoutInflater.From (_context).Inflate (Resource.Layout.item_text_overviewshare, parent, false);
			Typeface font = Typeface.CreateFromAsset (Application.Context.Assets, "HelveticaNeueLTStd-Lt.otf");
			// what happens if there is nothing in this position?

			var item = _items [position];


			TextView text = pendingView.FindViewById<TextView> (Resource.Id.textSpin);
			TextView date = pendingView.FindViewById<TextView> (Resource.Id.textDate);

			text.SetTypeface (font, TypefaceStyle.Normal);
			date.SetTypeface (font, TypefaceStyle.Normal);
			date.Visibility = ViewStates.Gone;

            if (string.IsNullOrEmpty(item.contact.company))
                text.Text = item.contact.firstname + " " + item.contact.lastname;
			else
                text.Text = item.contact.firstname + " " + item.contact.lastname + " (" + item.contact.company + ")";
			if (flag) {
				date.Visibility = ViewStates.Visible;
                string[] forstr1 = item.date.Split('T');
                string[] forstr2 = item.date.Split('T');
                forstr2 = forstr2[1].Split('+');
                var str1 = string.Format ("{0:yyyy-MM-dd}", forstr1[0]);
                var str2 = string.Format ("{0:H:mm:ss}", forstr2[0]);

				date.Text = str1 + "\n" + str2;
			} else {
				date.Visibility = ViewStates.Gone;
			}
			check_valid (item, text);
			
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

        public override contactData this[int position]
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

        void check_valid(contactData contact, TextView _text)
		{
            Console.WriteLine ("Barcod every customer = " + contact.contact.uid);
            if (string.IsNullOrEmpty (contact.contact.lastname) &&
                string.IsNullOrEmpty (contact.contact.firstname) &&
				string.IsNullOrEmpty (contact.contact.company)) {
                if (!string.IsNullOrEmpty (contact.contact.uid)) {
					_text.Text = "badge "+" "+contact.contact.uid;
					Console.WriteLine (contact.contact.uid);
					return;
				}
			}

            if (string.IsNullOrEmpty (contact.contact.lastname) &&
                string.IsNullOrEmpty (contact.contact.firstname)) {
				if (!string.IsNullOrEmpty (contact.contact.uid)) {
					_text.Text = "badge "+" "+contact.contact.uid;
					Console.WriteLine (contact.contact.uid);
					return;
				}
			}

		}
			
    }
}