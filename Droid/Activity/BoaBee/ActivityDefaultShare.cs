using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V4.View;
using Android.Support.V4.App;
using Android.Content.PM;
using Android.Views.Animations;
using Android.Graphics;
using Java.Lang;
using SlidingTabLayoutTutorial;
using Android.Support.V7.App;
//using sToolbar = Android.Support.V7.Widget.Toolbar;
using Android.Graphics.Drawables;
using BoaBeePCL;
using System.Runtime.InteropServices;


namespace Leadbox
{
	[Activity (Label = "ActivityDefaultShare", 
	ScreenOrientation = ScreenOrientation.Portrait, 
	Theme = "@style/ActivityTheme", 
	WindowSoftInputMode = SoftInput.AdjustResize)]
	
	public class ActivityDefaultShare : Activity
	{  
		ListView listfolders;
		ListView listfiles;
		LinearLayout folders;
		LinearLayout files;
		TextView folderUp;
		TextView btnShowcontactPopup;
		TextView countFile;
		Animation fadeIn;
		Animation fadeOut;
		RelativeLayout popUpconract;
		ListView listitems;
		List<DBfileTO> string_find = new List<DBfileTO>();
		List<DBfileTO> _list_for_add_popup = new List<DBfileTO>();
		List<DBfileTO> l_folder;
		List<DBfileTO> l_files;
		List<DBfileTO> tt;
		LinearLayout tapzoneopenfile;
		TextView txtFilesNo;
		//TabsAdapter tabsAdapter;
		List<DBDefaultFileTO> list_popup_temp_default;
        List<DBfileTO> listActiveFiles;
        List<DBfileTO> allFiles;
        bool firststart = true;


		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			list_popup_temp_default = DBLocalDataStore.GetInstance().GetDefaultFileTO();

			SetContentView(Resource.Layout.ActivityDefaultShare);
			Typeface font = Typeface.CreateFromAsset (Application.Context.Assets, "HelveticaNeueLTStd-Lt.otf");
			var txtPlashka = FindViewById<TextView>(Resource.Id.textPlashka);
			var linearForTap = FindViewById<LinearLayout> (Resource.Id.linearLayoutForTap);
			var btnExit = FindViewById<ImageView> (Resource.Id.buttonExit);
			var btnDone = FindViewById<ImageView> (Resource.Id.btnSync);

			countFile = FindViewById<TextView>(Resource.Id.countFilesdefault);
			listitems = FindViewById<ListView> (Resource.Id.listViewApps);
			folders = FindViewById<LinearLayout> (Resource.Id.linearLayoutFolders);
			files = FindViewById<LinearLayout> (Resource.Id.linearLayoutFiles);
			folderUp = FindViewById<TextView> (Resource.Id.nameFolderUp);
			btnShowcontactPopup = FindViewById<TextView> (Resource.Id.ShowContacts);
			popUpconract = FindViewById<RelativeLayout> (Resource.Id.popRelative);
			tapzoneopenfile = FindViewById<LinearLayout> (Resource.Id.tapzoneblock);
			var btnUp = FindViewById<LinearLayout> (Resource.Id.linearLayoutUp);
			txtFilesNo = FindViewById<TextView> (Resource.Id.textFilesNo);
			listfolders = FindViewById<ListView> (Resource.Id.listFolders);
			listfiles = FindViewById<ListView> (Resource.Id.listFiles);
//			txtFilesNo = FindViewById<TextView> (Resource.Id.textFilesNo);

			files.Visibility = ViewStates.Gone;
			folders.Visibility = ViewStates.Visible;
			folderUp.SetTypeface (font, TypefaceStyle.Bold);
			txtFilesNo.Visibility = ViewStates.Gone;
			txtFilesNo.SetTypeface (font, TypefaceStyle.Normal);
			txtPlashka.SetTypeface (font, TypefaceStyle.Normal);
			countFile.SetTypeface(font, TypefaceStyle.Normal);
			btnShowcontactPopup.SetTypeface (font, TypefaceStyle.Normal);

            allFiles = DBLocalDataStore.GetInstance().GetAllLocalFiles();
            listActiveFiles = DBLocalDataStore.GetInstance().GetAllLocalFiles().Where(s => s.activeFile).ToList();

			tapzoneopenfile.Click += (sender, e) => 
				Console.WriteLine("tap zone");

			linearForTap.Click += (object sender, EventArgs e) => {
				fadeOut = new AlphaAnimation (1f, 0f);
				fadeOut.Duration = 400;
				popUpconract.Visibility = ViewStates.Gone;
				popUpconract.StartAnimation(fadeOut);
			};

			tt = DBLocalDataStore.GetInstance ().GetAllLocalFiles ();
			tt.Sort(delegate(DBfileTO name1, DBfileTO name2)
				{ return name1.name.CompareTo(name2.name); });
			
			//foreach(var item in list_popup_temp_default)
			//{
			//	foreach(var items in tt)
			//	{
			//		if (item.uuid == items.uuid)
			//		{
			//			//SaveAndLoad.files_selected.Add(items);
			//		}

			//	}
			//}

			if (DBLocalDataStore.GetInstance().GetShowLocalFiles()) {
				txtFilesNo.Text = "No documentation available for this app. \nGo to your dashboard to add documentation";
				txtFilesNo.Visibility = ViewStates.Visible;
				listfolders.Visibility = ViewStates.Gone;
				listfiles.Visibility = ViewStates.Gone;
			}
//
//			l_folder = tt.Where (s => s.folderUuid == "_empty_").ToList ();
//			l_files = tt.Where (s => s.folderUuid == "_empty_").ToList ();



			l_folder = tt.Where (s => s.folderUuid == "_empty_" && s.fileType.ToUpper() == "folder".ToUpper()).ToList ();
			l_files = tt.Where (s => s.folderUuid == "_empty_").ToList ();

			listfolders.Adapter = new FoldersListAdapter (this, l_folder);

				btnUp.Visibility = ViewStates.Gone;
				//linearForTap.Visibility = ViewStates.Gone;

            listfolders.Adapter = new FilesListAdapter(this, l_files, click_add_file, click_item_file,firststart);
			//if (flag_folders)
			listfolders.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
//				files.Visibility = ViewStates.Visible;
//				folders.Visibility = ViewStates.Gone;
                List<DBfileTO> oldfiles = new List<DBfileTO>();
                oldfiles = l_files;
                l_files = tt.Where(s => s.folderUuid == l_files[e.Position].uuid).ToList();
                if (l_files.Count == 0)
                {
                    if (oldfiles[e.Position].fileType == "folder")
                    {
                        btnUp.Visibility = ViewStates.Visible;
                        txtFilesNo.Visibility = ViewStates.Visible;
                        txtFilesNo.Text = "No files";
                        listfolders.Adapter = new FilesListAdapter(this, l_files, click_add_file, click_item_file);//listfiles
                        try
                        {
                            string_find.Add(oldfiles[e.Position]);
                            folderUp.Text = oldfiles[e.Position].name;
                        }
                        catch { }
                    }
                    else {
                        l_files = oldfiles;
                    }
                }
                else {
                    //if (folderUp.Text != oldfiles[e.Position].name && folderUp.Text != "Go to Up"){
                    btnUp.Visibility = ViewStates.Visible;
                    txtFilesNo.Visibility = ViewStates.Gone;
                    try
                    {
                        string_find.Add(oldfiles[e.Position]);
                        folderUp.Text = oldfiles[e.Position].name;
                    }
                    catch { }
                    listfolders.Adapter = new FilesListAdapter(this, l_files, click_add_file, click_item_file);
                    //}

                }

			};
			btnUp.Click += (object sender, EventArgs e) => {
				txtFilesNo.Visibility = ViewStates.Gone;
				if (string_find.Count > 1){
					l_files = tt.Where(s => s.folderUuid == string_find[string_find.Count-1].folderUuid).ToList();
					_list_for_add_popup.Clear();
					_list_for_add_popup = l_files;
					var popup_files_temp = new List<DBpopupfileTO>();
					list_popup_temp_default.ForEach(s=>popup_files_temp.Add(new DBpopupfileTO(s)));

					listfolders.Adapter = new FilesListAdapter(this, l_files, click_add_file, click_item_file);
					string_find.RemoveAt(string_find.Count-1);
					folderUp.Text = string_find[string_find.Count-1].name;
					//folderUp.Tag = tt.Where(s =).ToList()[0].uuid;
				}
				else{
					string_find.Clear();
//					files.Visibility = ViewStates.Gone;
//					folders.Visibility = ViewStates.Visible;
//					listfolders.Adapter = new FoldersListAdapter (this, l_folder);
//

					l_files = tt.Where (s => s.folderUuid == "_empty_").ToList ();
					l_folder = tt.Where (s => s.folderUuid == "_empty_" && s.fileType.ToUpper() == "folder".ToUpper()).ToList ();

					listfolders.Adapter = new FoldersListAdapter(this, l_folder);

					listfolders.Adapter = new FilesListAdapter(this, l_files, click_add_file, click_item_file);
					btnUp.Visibility = ViewStates.Gone;
					//linearForTap.Visibility = ViewStates.Gone;
				}

			};

			btnExit.Click += (object sender, EventArgs e) => 
				{
					var list = DBLocalDataStore.GetInstance().GetAllLocalFiles();
                    for (int i = 0; i < list.Count; i++)
                    {
                        list[i].activeFile = false;
                        list[i].isDefault = false;
                        DBLocalDataStore.GetInstance().UpdateLocalFile(list[i]);
                    }
                    Finish();
				};

			btnDone.Click += (sender, e) => {
                //				
                var list = DBLocalDataStore.GetInstance().GetAllLocalFiles();
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].activeFile)
                    {
                        list[i].isDefault = true;
                    }
                    else {
                        list[i].isDefault = false;
                    }
                    DBLocalDataStore.GetInstance().UpdateLocalFile(list[i]);
                }
                Finish();
			};


			btnShowcontactPopup.Click += (object sender, EventArgs e) => {
                
                if (DBLocalDataStore.GetInstance().GetAllLocalFiles().Where(s => s.activeFile).ToList().Count() > 0){
					if (popUpconract.Visibility == ViewStates.Gone)
					{
						fadeIn = new AlphaAnimation (0f, 1f);
						fadeIn.Duration = 400;
						popUpconract.Visibility = ViewStates.Visible;
						popUpconract.StartAnimation(fadeIn);
					}
					else {
						fadeOut = new AlphaAnimation (1f, 0f);
						fadeOut.Duration = 400;
						popUpconract.Visibility = ViewStates.Gone;
						popUpconract.StartAnimation(fadeOut);
					}
                    linearForTap.Visibility = ViewStates.Visible;
				}
			};


		}


		void click_item_file(object sender, EventArgs e)
		{
			var linearForTap = FindViewById<LinearLayout> (Resource.Id.linearLayoutForTap);
			var btnUp = FindViewById<LinearLayout> (Resource.Id.linearLayoutUp);
			try
			{
				int id = 0;
				if (sender is TextView)
				{
					id = Convert.ToInt32( ((TextView) sender).Tag);
				}
				else if(sender is ImageView)
				{
					id = Convert.ToInt32( ((ImageView) sender).Tag);
				}
				if (l_files[id].fileType == "folder")
				{
					string_find.Add(l_files[id]);
					folderUp.Text = l_files[id].name;
					folderUp.Tag = l_files[id].uuid;

					l_files = tt.Where(s => s.folderUuid == l_files[id].uuid).ToList();
					_list_for_add_popup.Clear();
					_list_for_add_popup = l_files;
					var popup_files_temp = new List<DBpopupfileTO>();
					list_popup_temp_default.ForEach(s=>popup_files_temp.Add(new DBpopupfileTO(s)));

                    listfolders.Adapter = new FilesListAdapter(this, l_files, click_add_file, click_item_file,firststart);
                    firststart = false;
					if (l_files.Count == 0){
						btnUp.Visibility = ViewStates.Visible;
						linearForTap.Visibility = ViewStates.Visible;
						txtFilesNo.Visibility = ViewStates.Visible;
						txtFilesNo.Text = "No files";
					}
					else{
						btnUp.Visibility = ViewStates.Visible;
						linearForTap.Visibility = ViewStates.Visible;
						txtFilesNo.Visibility = ViewStates.Gone;
					}

				}
				else{
					tapzoneopenfile.Visibility = ViewStates.Visible;
					if (l_files[id].fileType == "url")
					{
						var uri = Android.Net.Uri.Parse (l_files[id].localpath);
						var intent = new Intent (Intent.ActionView, uri); 
						StartActivity (intent);
						return;
					}
					else if(l_files[id].fileType == "pdf" )
					{
						var _activity = new Intent(this, typeof(ActivityDisplayPdf));
						_activity.PutExtra("id_form_overview", l_files[id].localpath);
						StartActivity(_activity);
					}
					else if("image" == l_files[id].mimeType.Split("/".ToCharArray())[0])
					{
						var _activity = new Intent(this, typeof(ActivityDisplayImage));
						_activity.PutExtra("id_form_overview", l_files[id].localpath);
						StartActivity(_activity);
					}
//					Android.Net.Uri _File = Android.Net.Uri.FromFile (new Java.IO.File (l_files[id].localpath));
//					Intent file_Intent = new Intent (Intent.ActionView);
//					file_Intent.SetDataAndType (_File, l_files[id].mimeType);
//					file_Intent.SetFlags (ActivityFlags.NoHistory);
//					StartActivity (file_Intent);
				}
			}
			catch(Java.Lang.Exception ex){
				Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
			}
		}

		void click_add_file(object sender, EventArgs e)
		{
			
			scaleView(btnShowcontactPopup, 1f, 1f, 1f, 1.1f, 0f, 50f);
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
                //popUpconract = _rootview.FindViewById<RelativeLayout> (Resource.Id.popRelative);
                //btnShowcontactPopup.Text = ;
                //text in bottom
                ((ImageView)sender).SetImageResource(Resource.Drawable.addButton);
            }
            else {
                l_files[id].activeFile = false;
                DBLocalDataStore.GetInstance().UpdateLocalFile(l_files[id]);
                ((ImageView)sender).SetImageResource(Resource.Drawable.addBattonout);
            }
            listActiveFiles = DBLocalDataStore.GetInstance().GetAllLocalFiles().Where(s => s.activeFile).ToList();
            countFile.Text = listActiveFiles.Count + "";
            //scaleView(btnShowcontactPopup, 1f, 1f, 1f, 1.1f, 0f, 50f);
            //scaleImage((ImageView)sender, 1f, 1.2f, 1f, 1.2f, 50f, 50f);
            //listfolders.Adapter = new FilesListAdapter(Activity, l_files, click_add_file, click_item_file);
            listitems.Adapter = new FilesPopupListAdapter(this, listActiveFiles, click_delete_popup);
		}

		void click_delete_popup(object sender, EventArgs e)
		{

             string id = "";

            if (sender is ImageView)
            {
                id = (string)((ImageView)sender).Tag;
            }
            try
            {
                var fileforDelete = DBLocalDataStore.GetInstance().GetAllLocalFiles().Find(s => s.uuid == id);
                for (int i = 0; i < l_files.Count; i++)
                {
                    if (l_files[i].uuid == id)
                    {
                        l_files[i].activeFile = false;
                    }
                }
                fileforDelete.activeFile = false;
                DBLocalDataStore.GetInstance().UpdateLocalFile(fileforDelete);
                var listactivefilesforpopup = DBLocalDataStore.GetInstance().GetAllLocalFiles().Where(s => s.activeFile).ToList();
                listitems.Adapter = new FilesPopupListAdapter(this, listactivefilesforpopup, click_delete_popup);
                countFile.Text = listactivefilesforpopup.Count + "";
                scaleImage((ImageView)sender, 1f, 1.2f, 1f, 1.2f, 50f, 50f);

                scaleView(btnShowcontactPopup, 1f, 1f, 1f, 1.1f, 0f, 50f);

                listfolders.Adapter = new FilesListAdapter(this, l_files, click_add_file, click_item_file);

                ((ImageView)sender).SetImageResource(Resource.Drawable.addBattonout);
                SaveAndLoad.files_selected.RemoveAll(s => s.uuid == id);
                //var fileforDelete = l_files.Where(s => s.uuid == id).ToList();
                //fileforDelete[0].activeFile = false;
                //DBLocalDataStore.GetInstance().UpdateLocalFile(fileforDelete[0]);
                //listActiveFile = FragmentShareScreen.getActiveFiles(l_files);
                //listitems.Adapter = new FilesPopupListAdapter(Activity, listActiveFile, click_delete_popup);
                //btnShowcontactPopup.Text = listActiveFile.Count + "";
                //scaleImage((ImageView)sender, 1f, 1.2f, 1f, 1.2f, 50f, 50f);

                //scaleView(btnShowcontactPopup, 1f, 1f, 1f, 1.1f, 0f, 50f);
                //listfolders.Adapter = new FilesListAdapter(Activity, l_files, click_add_file, click_item_file);

                //((ImageView)sender).SetImageResource(Resource.Drawable.addBattonout);
                //SaveAndLoad.files_selected.RemoveAll(s => s.uuid == id);
                tt = DBLocalDataStore.GetInstance().GetAllLocalFiles();
                tt.Sort(delegate (DBfileTO name1, DBfileTO name2)
                { return name1.name.CompareTo(name2.name); });
            }
            catch {
                
            }
			

		}

		public override void Finish ()
		{
			base.Finish ();
		}

		public override void OnBackPressed ()
		{
			base.OnBackPressed ();
		}

		protected override void OnResume()
		{
            base.OnResume();
			tapzoneopenfile.Visibility = ViewStates.Gone;
//			var dfs = DBLocalDataStore.GetInstance ().GetDefaultFileTO ();
			List <DBfileTO> lc = new List<DBfileTO>();
            lc = DBLocalDataStore.GetInstance().GetAllLocalFiles().Where(s => s.activeFile).ToList();
            listitems.Adapter = new FilesPopupListAdapter (this, lc, click_delete_popup);
			countFile.Text = lc.Count + "";

			if (popUpconract.Visibility == ViewStates.Visible) {
				fadeOut = new AlphaAnimation (1f, 0f);
				fadeOut.Duration = 400;
				popUpconract.Visibility = ViewStates.Gone;
				popUpconract.StartAnimation (fadeOut);
			}
		}
		public void scaleImage(ImageView v,  float startScalex, float endScalex, float startScaley, float endScaley, float pivotX,float pivotY) {
			Animation anim = new ScaleAnimation (
				startScalex, endScalex, 
				startScaley,endScaley,
				pivotX,pivotY);

			anim.Duration = 100;

			v.StartAnimation(anim);
			anim.AnimationEnd += (sender, e) => {
				v.ScaleX=1.2f;
				v.ScaleY=1.2f;
				scaleImage2(v);

			};
		}
		public void scaleImage2(ImageView v){//ImageView v,  float startScalex, float endScalex, float startScaley, float endScaley, float pivotX,float pivotY) {
			Animation anim2 = new ScaleAnimation (
				1.2f, 1f, 
				1.2f,1f,
				50f,50f);

			//	scaleImage((ImageView)sender, 1f, 1.3f, 1f, 1.3f, 20f, 10f);
			anim2.Duration = 100;

			v.StartAnimation(anim2);
			anim2.AnimationEnd += (sender, e) => {
				v.ScaleX=1f;
				v.ScaleY=1f;

				//listfiles.Adapter = new FilesListAdapter(Activity, l_files, click_add_file, click_item_file, list_popup_temp_default);

			};
		}
		public void scaleView(TextView v,  float startScalex, float endScalex, float startScaley, float endScaley, float pivotX,float pivotY) {
			Animation anim = new ScaleAnimation (
				startScalex, endScalex, 
				startScaley,endScaley,pivotX,pivotY);
			anim.Duration = 400;
			v.StartAnimation(anim);
			anim.AnimationEnd += (sender, e) =>
				{
					v.TextScaleX =1.1f;
					scaleView2(v);
				};
		}
		public void scaleView2(TextView v) {
			Animation anim2 = new ScaleAnimation (
				1f, 1f, 
				1.1f,1f,
				0f,50f);
			anim2.Duration = 400;
			v.StartAnimation(anim2);
			anim2.AnimationEnd += (sender, e) =>
				{
					v.TextScaleX =1f;
				};
		}
	}
}

