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
	public class OverviewSharesListAdapter : BaseAdapter<DBlocalContact>
    {
		private List<DBlocalContact> _items;
        private Activity _context;



		public OverviewSharesListAdapter(Activity context, List<DBlocalContact> items)
            : base()
        {
            _context = context;
            _items = items;
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
			View pendingView = convertView ?? LayoutInflater.From (_context).Inflate (Resource.Layout.item_text_select, parent, false);
			Typeface font = Typeface.CreateFromAsset (Application.Context.Assets, "HelveticaNeueLTStd-Lt.otf");
			// what happens if there is nothing in this position?

			var item = _items [position];


			TextView text = pendingView.FindViewById<TextView> (Resource.Id.textSpin);

			text.SetTypeface (font, TypefaceStyle.Normal);

			if (string.IsNullOrEmpty(item.company))
                text.Text = item.firstname + " " + item.lastname;
			else
                text.Text = item.firstname + " " + item.lastname + " (" + item.company + ")";
			
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

		public override void NotifyDataSetChanged ()
		{
			base.NotifyDataSetChanged ();
		}

		public override void NotifyDataSetInvalidated ()
		{
			base.NotifyDataSetInvalidated ();
		}
			
    }
}