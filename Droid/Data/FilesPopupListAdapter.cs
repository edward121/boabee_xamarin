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
	public class FilesPopupListAdapter : BaseAdapter<DBfileTO>
    {
		private List<DBfileTO> _items;
        private Activity _context;
		EventHandler delete_item;


		public FilesPopupListAdapter(Activity context, List<DBfileTO> items, EventHandler _delete_item)
            : base()
        {
            _context = context;
            _items = items;
			delete_item = _delete_item;
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View pendingView = convertView ?? LayoutInflater.From(_context).Inflate(Resource.Layout.ListItemContactDialog, parent, false);
            Typeface font = Typeface.CreateFromAsset(Application.Context.Assets, "HelveticaNeueLTStd-Lt.otf");
            // what happens if there is nothing in this position?

            var item = _items[position];


            TextView text = pendingView.FindViewById<TextView>(Resource.Id.textViewDisplayName);
            var btnDelete = pendingView.FindViewById<ImageView>(Resource.Id.imageBtnDelete);

			text.SetTypeface (font, TypefaceStyle.Normal);
			text.Text = item.name;
            btnDelete.Tag = item.uuid;
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

		public override DBfileTO this[int position]
        {
            get { return _items[position]; }
        }



			
    }
}