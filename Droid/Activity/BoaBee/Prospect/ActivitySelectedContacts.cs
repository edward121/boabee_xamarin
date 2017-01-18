
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Android.Views.Animations;
using Android.Support.V4.View;
using System.Threading;
using BoaBeePCL;
using Android.Support.V4.Content;
using Android;
using Android.Content.PM;
using Android.Support.V4.App;

namespace Leadbox
{
    [Activity(Label = "ActivitySelectedContacts", ScreenOrientation = ScreenOrientation.Portrait, Theme = "@style/ActivityTheme", WindowSoftInputMode = SoftInput.StateAlwaysHidden)]
    public class ActivitySelectedContacts : Activity
    {
        List<DBlocalContact> ActiveContacts;
        ListView listview;
        public List<DBfileTO> l_files;
        public static bool FromShareScreen = false;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ActivitySelectedContacts);
            Typeface font = Typeface.CreateFromAsset(Application.Context.Assets, "HelveticaNeueLTStd-Lt.otf");
            var btnCancel = FindViewById<TextView>(Resource.Id.buttonCancel);
            listview = FindViewById<ListView>(Resource.Id.listViewApps);
            var filter = FindViewById<EditText>(Resource.Id.lookUpFilter);
            var titletext = FindViewById<TextView>(Resource.Id.titleSelectedContacts);
            btnCancel.SetTypeface(font, TypefaceStyle.Normal);
            titletext.SetTypeface(font, TypefaceStyle.Normal);
            filter.SetTypeface(font, TypefaceStyle.Normal);
            btnCancel.Click += delegate
            {
                Finish();
            };


            if (!FromShareScreen)
            {
                ActiveContacts = DBLocalDataStore.GetInstance().GetLocalContacts().Where(s => s.activeContact).ToList();
                listview.Adapter = new ContactsPopupListAdapter(this, ActiveContacts, click_delete_popup);
                filter.TextChanged += (sender, e) =>
                {
                    var ActiveContactsFilter = ActiveContacts.Where(s =>
                                              (s.firstname + " " + s.lastname + " (" + s.company + ")").ToUpper()
                    .Contains(filter.Text.ToUpper()) || s.uid != null && s.email == null && ("badge " + s.uid).Contains(filter.Text)).ToList();
                    listview.Adapter = new ContactsPopupListAdapter(this, ActiveContactsFilter, click_delete_popup);

                };
            }
            else {
                titletext.Text = "SELECTED FILES";
                l_files = DBLocalDataStore.GetInstance().GetAllLocalFiles().Where(s => s.activeFile).ToList();
                listview.Adapter = new FilesListAdapter(this, l_files, click_add_file, click_item_file, true);
                filter.TextChanged += (sender, e) =>
                {
                    var ActiveContactsFilter = l_files.Where(s =>
                                                             (s.name).ToUpper()
                                                             .Contains(filter.Text.ToUpper())).ToList();
                    listview.Adapter = new FilesListAdapter(this, ActiveContactsFilter, click_add_file, click_item_file, true);
                };
            }
        }
        void click_add_file(object sender, EventArgs e)
        {
            scaleImage((ImageView)sender, 1f, 1.2f, 1f, 1.2f, 50f, 50f);
            int id = 0;
            if (sender is ImageView)
            {
                id = Convert.ToInt32(((ImageView)sender).Tag);
            }
            if (!l_files[id].activeFile)
            {
                l_files[id].activeFile = true;
                DBLocalDataStore.GetInstance().UpdateLocalFile(l_files[id]);
                ((ImageView)sender).SetImageResource(Resource.Drawable.addButton);
            }
            else {
                l_files[id].activeFile = false;
                DBLocalDataStore.GetInstance().UpdateLocalFile(l_files[id]);
                ((ImageView)sender).SetImageResource(Resource.Drawable.addBattonout);
            }
            var listactivefilesforpopup = DBLocalDataStore.GetInstance().GetAllLocalFiles().Where(s => s.activeFile).ToList();
            listview.Adapter = new FilesListAdapter(this, l_files, click_add_file, click_item_file, true);
        }

        void click_delete_popup(object sender, EventArgs e)
        {
            int id = 0;
            if (sender is ImageView)
            {
                //paymentId = Convert.ToString(((ImageButton) sender).Tag);
                id = Convert.ToInt32(((ImageView)sender).Tag);
            }

            ActiveContacts[id].activeContact = false;
            DBLocalDataStore.GetInstance().UpdateLocalContact(ActiveContacts[id]);
            ActiveContacts = DBLocalDataStore.GetInstance().GetLocalContacts().Where(s => s.activeContact).ToList();
            listview.Adapter = new ContactsPopupListAdapter(this, ActiveContacts, click_delete_popup);
        }

        void click_item_file(object sender, EventArgs e)
        {
            var line_between = FindViewById<LinearLayout> (Resource.Id.linearLayout6);

            try
            {
                
                int id = 0;
                if (sender is TextView)
                {
                    //paymentId = Convert.ToString(((ImageButton) sender).Tag);
                    id = Convert.ToInt32(((TextView)sender).Tag);
                }
                else if (sender is ImageView)
                {
                    id = Convert.ToInt32(((ImageView)sender).Tag);
                }

                    if (l_files[id].fileType == "url")
                    {
                        var uri = Android.Net.Uri.Parse(l_files[id].localpath);
                        var intent = new Intent(Intent.ActionView, uri);
                        StartActivity(intent);
                        return;
                    }
                    else if (l_files[id].fileType == "pdf")
                    {
                        var _activity = new Intent(this, typeof(ActivityDisplayPdf));
                        _activity.PutExtra("id_form_overview", l_files[id].localpath);
                        StartActivity(_activity);
                    }
                    else if ("image" == l_files[id].mimeType.Split("/".ToCharArray())[0])
                    {
                        var _activity = new Intent(this, typeof(ActivityDisplayImage));
                        _activity.PutExtra("id_form_overview", l_files[id].localpath);
                        StartActivity(_activity);
                    }

            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
            }

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
