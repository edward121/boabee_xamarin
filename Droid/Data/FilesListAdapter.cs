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
	public class FilesListAdapter : BaseAdapter<DBfileTO>
	{   View pendingView ;
		private List<DBfileTO> _items;
        private Activity _context;
		EventHandler clickAdd;
		EventHandler clickFile;
		TextView tipeFile;
        bool firstwithDefoult = false;

		public FilesListAdapter(Activity context, List<DBfileTO> items, EventHandler _clickAdd, EventHandler _clickFile, bool firstwithDefoult = false)
            : base()
        {
            _context = context;
            _items = items;
			clickAdd = _clickAdd;
			clickFile = _clickFile;
            this.firstwithDefoult = firstwithDefoult;
		}

        public override View GetView(int position, View convertView, ViewGroup parent)
		{

			Typeface font = Typeface.CreateFromAsset (Application.Context.Assets, "HelveticaNeueLTStd-Lt.otf");



			var item = _items [position];
			if (item.fileType == "folder")
			{

				pendingView = LayoutInflater.From (_context).Inflate (Resource.Layout.ListItemShareFolders, parent, false);
				TextView field_text = pendingView.FindViewById<TextView>(Resource.Id.textViewDisplayName);
				ImageView image = pendingView.FindViewById<ImageView>(Resource.Id.imageView1);

				field_text.SetTypeface(font, TypefaceStyle.Normal);

				field_text.Text = item.name;
				field_text.Tag = position;
				field_text.Click -= clickFile;
				field_text.Click += clickFile;
				image.Tag = position;
				image.Click -= clickFile;
				image.Click += clickFile;
				return pendingView;
			}
			else
			{
				pendingView = LayoutInflater.From(_context).Inflate(Resource.Layout.ListItemShareFiles, parent, false);

				tipeFile = pendingView.FindViewById<TextView>(Resource.Id.typeFile);
				var addToList = pendingView.FindViewById<ImageView>(Resource.Id.addListSelected);
				TextView text = pendingView.FindViewById<TextView>(Resource.Id.textViewDisplayName);
                
				//pendingView = convertView ?? LayoutInflater.From (_context).Inflate (Resource.Layout.ListItemShareFiles, parent, false);
				//	ImageView image = pendingView.FindViewById<ImageView>(Resource.Id.imageView1);
				text.SetTypeface(font, TypefaceStyle.Normal);
				tipeFile.SetTypeface(font, TypefaceStyle.Normal);
    
				text.Text = item.name;
				text.Tag = position;
				text.Click -= clickFile;
				text.Click += clickFile;


				tipeFile.Tag = position;
				tipeFile.Click -= clickFile;
				tipeFile.Click += clickFile;
				try
				{
				
					switch (item.fileType)
					{
						case "pdf":
							tipeFile.Text = item.fileType + "   |";
							break;
						case "jpg":
							tipeFile.Text = item.fileType + "   |";
							break;
						case "png":
							tipeFile.Text = item.fileType + "   |";
							break;
						case "txt":
							tipeFile.Text = item.fileType + "   |";
							break;
						case "url":
							tipeFile.Text = "www" + "   |";
							break;
						default : 
							tipeFile.Text = item.fileType + "   |";
							break;
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
				}


				addToList.Tag = position;
                if (item.isDefault && firstwithDefoult)
                {
                    item.activeFile = true;
                    DBLocalDataStore.GetInstance().UpdateLocalFile(item);
                }
           
                if (item.activeFile)
				{
					addToList.SetImageResource(Resource.Drawable.addButton);
				}
				else
				{
					addToList.SetImageResource(Resource.Drawable.addBattonout);
				}

				addToList.Click -= clickAdd;
				addToList.Click += clickAdd;

                addToList.Click += delegate {
                    scaleImage(addToList, 1f, 1.2f, 1f, 1.2f, 50f, 50f);
                };


//			pendingView.Click -= (object sender, EventArgs e) => {
//				_context.StartActivity(typeof(ActivityIdetifyScreen));
//			};
//
//			pendingView.Click += (object sender, EventArgs e) => {
//				_context.StartActivity(typeof(ActivityIdetifyScreen));
//			};

				return pendingView;
			}
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

        public void scaleImage(ImageView v, float startScalex, float endScalex, float startScaley, float endScaley, float pivotX, float pivotY)
        {
            Animation anim = new ScaleAnimation(
                startScalex, endScalex,
                startScaley, endScaley,
                pivotX, pivotY);

            anim.Duration = 100;

            v.StartAnimation(anim);
            anim.AnimationEnd += (sender, e) =>
            {
                v.ScaleX = 1.2f;
                v.ScaleY = 1.2f;
                scaleImage2(v);

            };
        }
        public void scaleImage2(ImageView v)
        {//ImageView v,  float startScalex, float endScalex, float startScaley, float endScaley, float pivotX,float pivotY) {
            Animation anim2 = new ScaleAnimation(
                1.2f, 1f,
                1.2f, 1f,
                50f, 50f);

            //  scaleImage((ImageView)sender, 1f, 1.3f, 1f, 1.3f, 20f, 10f);
            anim2.Duration = 100;

            v.StartAnimation(anim2);
            anim2.AnimationEnd += (sender, e) =>
            {
                v.ScaleX = 1f;
                v.ScaleY = 1f;
                //listfiles.Adapter = new FilesListAdapter(Activity, l_files, click_add_file, click_item_file, list_popup_temp);

            };
        }
			
    }
}