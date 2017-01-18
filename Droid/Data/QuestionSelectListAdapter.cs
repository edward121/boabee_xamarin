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
	public class QuestionSelectListAdapter : BaseAdapter<DBFormDefinition>
    {
		private List<DBFormDefinition> _items;
        private Activity _context;



		public QuestionSelectListAdapter(Activity context, List<DBFormDefinition> items)
            : base()
        {
            _context = context;
            _items = items;
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
			View pendingView = convertView ?? LayoutInflater.From (_context).Inflate (Resource.Layout.ListItemApp, parent, false);
			Typeface font = Typeface.CreateFromAsset (Application.Context.Assets, "HelveticaNeueLTStd-Lt.otf");
			// what happens if there is nothing in this position?

			var item = _items [position];


			TextView text = pendingView.FindViewById<TextView> (Resource.Id.textViewDisplayName);

			text.SetTypeface (font, TypefaceStyle.Normal);

			text.Text = item.objectName;

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

		public override DBFormDefinition this[int position]
        {
            get { return _items[position]; }
        }



			
    }
}