
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
using Java.Util;
using Android.Support.V4.View;
using Android.Opengl;
using System.Threading;
using BoaBeePCL;
using System.Runtime.InteropServices;
using Android.Renderscripts;
using Org.W3c.Dom.LS;
using Dalvik.SystemInterop;
using BoaBeeLogic;

namespace Leadbox
{
	public class FragmentShareScreen : Android.Support.V4.App.Fragment
	{
		public ViewPager _viewPager{ get; set; }
		ListView listfolders;
		List<string> temper;
		ListView listfiles;
		LinearLayout folders;
		LinearLayout files;
		TextView folderUp;
		Animation fadeIn;
		Animation fadeOut;
		RelativeLayout popUpconract;
		ListView listitems;
		List<DBfileTO> string_find = new List<DBfileTO>();
		List<DBfileTO> _list_for_add_popup = new List<DBfileTO>();
	//	List<> list_popup = new List<>();
		List<DBfileTO> l_folder;
		List<DBfileTO> l_files;
		List<DBfileTO> tt;
		List<DBfileTO> tts;
		LinearLayout tapzoneopenfile;
		View _rootview;
		List<DBpopupfileTO> list_popup_temp;
		TextView txtFilesNo;
		List<DBDefaultFileTO> list_popup_temp_default;
        List<DBlocalContact> localcontacts;
        public List<DBfileTO> listActiveFile;
        TextView countfiles;
        LinearLayout btnUp;
        public int countFiles = DBLocalDataStore.GetInstance().GetAllLocalFiles().Where(s => s.activeFile).Count();

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			tt = DBLocalDataStore.GetInstance ().GetAllLocalFiles ();
            for (int i = 0; i < tt.Count; i++) {
                if (tt[i].isDefault)
                {
                    tt[i].activeFile = true;
                    DBLocalDataStore.GetInstance().UpdateLocalFile(tt[i]);
                }
            }
            tt.Sort(delegate (DBfileTO name1, DBfileTO name2)
                    { return name1.name.CompareTo(name2.name); });
            list_popup_temp = DBLocalDataStore.GetInstance().GetLocalFilesPopup();

			if (DBLocalDataStore.GetInstance().GetLocalFilesPopup().Count == 0)
			{
				var list_default_share = DBLocalDataStore.GetInstance().GetDefaultFileTO();
				foreach (var share in list_default_share)
				{
					DBLocalDataStore.GetInstance().AddLocalFilesPopup(new DBpopupfileTO(share));
					var item = tt.Find(s=>s.uuid == share.uuid);

					if (item != null)
						SaveAndLoad.files_selected.Add(item);
				}
			}
			foreach (var share in list_popup_temp)
			{
				var item = tt.Find(s=>s.uuid == share.uuid);
				bool checkTemp = SaveAndLoad.files_selected.Any(s=>s.uuid == item.uuid);
				if(!checkTemp)
				{
					SaveAndLoad.files_selected.Add(item);
				}
			}
            localcontacts = DBLocalDataStore.GetInstance().GetLocalContacts();
            localcontacts = FragmentClassifyScreen.CheckActiveContacts(localcontacts);
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{

		    
			list_popup_temp_default = DBLocalDataStore.GetInstance().GetDefaultFileTO();
			Typeface font = Typeface.CreateFromAsset (Application.Context.Assets, "HelveticaNeueLTStd-Lt.otf");
			_rootview = inflater.Inflate (Resource.Layout.Activity_share_screen , container, false);
			popUpconract = _rootview.FindViewById<RelativeLayout> (Resource.Id.popRelative);
			var linearForTap = _rootview.FindViewById<LinearLayout> (Resource.Id.linearLayoutForTap);
            var btnExit = _rootview.FindViewById<TextView> (Resource.Id.cancelButton);
            var titletext = _rootview.FindViewById<TextView>(Resource.Id.textView1);
            var btnSync = _rootview.FindViewById<TextView> (Resource.Id.ShowContacts);
			tapzoneopenfile = _rootview.FindViewById<LinearLayout> (Resource.Id.tapzoneblock);
			listitems = _rootview.FindViewById<ListView> (Resource.Id.listViewApps);
			folders = _rootview.FindViewById<LinearLayout> (Resource.Id.linearLayoutFolders);
			files = _rootview.FindViewById<LinearLayout> (Resource.Id.linearLayoutFiles);
			//var line_between = _rootview.FindViewById<LinearLayout> (Resource.Id.linearLayout6);
			folderUp = _rootview.FindViewById<TextView> (Resource.Id.nameFolderUp);
			btnUp = _rootview.FindViewById<LinearLayout> (Resource.Id.linearLayoutUp);
			txtFilesNo = _rootview.FindViewById<TextView> (Resource.Id.textFilesNo);
			listfolders = _rootview.FindViewById<ListView> (Resource.Id.listFolders);
			listfiles = _rootview.FindViewById<ListView> (Resource.Id.listFiles);
            countfiles = _rootview.FindViewById<TextView>(Resource.Id.countFiles);
            var infoText = _rootview.FindViewById<TextView>(Resource.Id.textView2);
            
            countfiles.Text = countFiles.ToString();

  			files.Visibility = ViewStates.Gone;
			folders.Visibility = ViewStates.Visible;
			folderUp.SetTypeface (font, TypefaceStyle.Bold);
			txtFilesNo.Visibility = ViewStates.Gone;
            btnSync.SetTypeface (font, TypefaceStyle.Normal);
            infoText.SetTypeface(font, TypefaceStyle.Normal);
			txtFilesNo.SetTypeface (font, TypefaceStyle.Normal);
            titletext.SetTypeface(font, TypefaceStyle.Normal);
//			files.SetTypeface (font, TypefaceStyle.Normal);
//			folders.SetTypeface (font, TypefaceStyle.Normal);
            var listactivefilesforpopup = DBLocalDataStore.GetInstance().GetAllLocalFiles().Where(s => s.activeFile).ToList();
            listitems.Adapter = new FilesPopupListAdapter(Activity, listactivefilesforpopup, click_delete_popup);
			tapzoneopenfile.Click += (sender, e) => 
				Console.WriteLine("tap zone");

			linearForTap.Click += (object sender, EventArgs e) => {
				fadeOut = new AlphaAnimation (1f, 0f);
				fadeOut.Duration = 400;
				popUpconract.Visibility = ViewStates.Gone;
				popUpconract.StartAnimation(fadeOut);
			};
            countfiles.Click += delegate {
                ActivitySelectedContacts.FromShareScreen = true;
                Activity.StartActivity(typeof(ActivitySelectedContacts));
            };

			var str23 = Android.OS.Environment.ExternalStorageDirectory;
			Console.WriteLine (str23);

			tt = DBLocalDataStore.GetInstance ().GetAllLocalFiles ();
			//tts.Sort();
			tt.Sort(delegate(DBfileTO name1, DBfileTO name2)
				{ return name1.name.CompareTo(name2.name); });
			
//			foreach(var item in list_popup_temp_default)
//			{
//				foreach(var items in tt)
//				{
//					if (item == items.uuid)
//					{
//						SaveAndLoad.files_selected.Add(items);
//					}
//
//				}
//			}

			if (DBLocalDataStore.GetInstance().GetShowLocalFiles()) {
				txtFilesNo.Text = "No documentation available for this app. \nGo to your dashboard to add documentation";
				txtFilesNo.Visibility = ViewStates.Visible;
				listfolders.Visibility = ViewStates.Gone;
				listfiles.Visibility = ViewStates.Gone;
			}
            var filesnull = tt.Where(s => s.localpath == null).ToList();
			l_folder = tt.Where (s => s.folderUuid == "_empty_" && s.fileType.ToUpper() == "folder".ToUpper()).ToList ();
			l_files = tt.Where (s => s.folderUuid == "_empty_").ToList ();

			//listfolders.Adapter = new FoldersListAdapter(Activity, l_folder);

				btnUp.Visibility = ViewStates.Gone;
				//line_between.Visibility = ViewStates.Gone;

			listfolders.Adapter = new FilesListAdapter(Activity, l_files, click_add_file, click_item_file,true);


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
                            listfolders.Adapter = new FilesListAdapter(Activity, l_files, click_add_file, click_item_file);//listfiles
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
                            listfolders.Adapter = new FilesListAdapter(Activity, l_files, click_add_file, click_item_file);
                    //}

                }
			};
			btnUp.Click += (object sender, EventArgs e) => {
				txtFilesNo.Visibility = ViewStates.Gone;
				if (string_find.Count > 1){
					
//					files.Visibility = ViewStates.Gone;
//					folders.Visibility = ViewStates.Visible;
//					btnUp.Visibility = ViewStates.Gone;
//					line_between.Visibility = ViewStates.Gone;
//
					l_files = tt.Where(s => s.folderUuid == string_find[string_find.Count-1].folderUuid).ToList();
					_list_for_add_popup.Clear();
					_list_for_add_popup = l_files;
				//	listfolders.Adapter = new FoldersListAdapter(Activity, l_folder);

					listfolders.Adapter = new FilesListAdapter(Activity, l_files, click_add_file, click_item_file);
					string_find.RemoveAt(string_find.Count-1);
					folderUp.Text = string_find[string_find.Count-1].name;
					//folderUp.Tag = tt.Where(s =).ToList()[0].uuid;
				}
				else{
					string_find.Clear();
//					files.Visibility = ViewStates.Gone;
//					folders.Visibility = ViewStates.Visible;

					l_files = tt.Where (s => s.folderUuid == "_empty_").ToList ();
					l_folder = tt.Where (s => s.folderUuid == "_empty_" && s.fileType.ToUpper() == "folder".ToUpper()).ToList ();

					listfolders.Adapter = new FoldersListAdapter(Activity, l_folder);

					listfolders.Adapter = new FilesListAdapter(Activity, l_files, click_add_file, click_item_file);
					//listfolders.Adapter = new FoldersListAdapter (Activity,l_files);
					btnUp.Visibility = ViewStates.Gone;
					//line_between.Visibility = ViewStates.Gone;
                }

			}; 

			btnExit.Click += (object sender, EventArgs e) => 
			{
				AlertDialog.Builder builder = new AlertDialog.Builder (Activity, Resource.Style.TransparentProgressDialog);
				AlertDialog ad = builder.Create ();
				ad.SetMessage ("Your current work will be lost.");
				ad.SetTitle ("Warning:");
				ad.SetButton ("Cancel",(s, ev) => {
				});
				ad.SetButton2("Ok",(s, ev) => {
					StartActivity (new Intent (Activity, typeof(ActivityHomescreen)));
					SaveAndLoad.GetInstance ().DeleteFile ();
					DBLocalDataStore.GetInstance().ClearAllContactsPopup();
					DBLocalDataStore.GetInstance().ClearAllFilesPopup();
					SaveAndLoad.files_selected.Clear();
                    BoaBeeLogic.OfflineLogic.ClearDataSelected();
					Activity.Finish();
				});
                ad.Show ();
			};
			

            btnSync.Click += (object sender, EventArgs e) => {
                if (localcontacts.Count == 0){
					Toast.MakeText (Activity, "Please select at least one contact", ToastLength.Short).Show ();
					_viewPager.SetCurrentItem(0, true);
					return;
				}
                DBLocalDataStore.GetInstance().resetAnswers();
                var answer2 = SaveAndLoad.GetInstance().GetAllAnswers();
                var list = DBLocalDataStore.GetInstance().GetLocalQuestions(DBLocalDataStore.GetInstance().GetSelectedQuestionPosition());
                if (DBLocalDataStore.GetInstance().GetLocalQuestions(DBLocalDataStore.GetInstance().GetSelectedQuestionPosition()).Count != 0)
                {
                    for (int i = 0; i < answer2.Count; i++)
                    {
                        DBAnswer answer = new DBAnswer();
                        answer.question = list[i].question;
                        answer.answer = answer2[i] == "_,___" ? "" : answer2[i];
                        answer.Id = i + 1;
                        DBLocalDataStore.GetInstance().updateAnswer(answer);
                    }
                }
                try
                {
                    OfflineLogic.prepareSync();
                }
                catch
                {
                    Toast.MakeText(Activity, "You did not complete all mandatory fields in the info screen. Please correct.", ToastLength.Short).Show();
                    ActivityIdentifyClassifyShare.viewPager.Visibility = ViewStates.Visible;
                    ActivityIdentifyClassifyShare.viewPagerShare.Visibility = ViewStates.Gone;
                    var prefs1 = Application.Context.GetSharedPreferences("MyApp", FileCreationMode.Private);
                    var prefEditor1 = prefs1.Edit();
                    prefEditor1.PutInt("ScrennDestroy", 1);
                    prefEditor1.Commit();
                    return;
                }


                SaveAndLoad.GetInstance ().DeleteFile ();
                OfflineLogic.ClearDataSelected();
                ActivityBadgeScanning.fromHomeScreen = false;
                var prefs = Application.Context.GetSharedPreferences("MyApp", FileCreationMode.Private);
                var prefEditor = prefs.Edit();
                prefEditor.PutInt("ScrennDestroy", 1);
                prefEditor.Commit();
				Activity.Finish();
			};

			
            listactivefilesforpopup = DBLocalDataStore.GetInstance().GetAllLocalFiles().Where(s => s.activeFile).ToList();
            listitems.Adapter = new FilesPopupListAdapter(Activity, listactivefilesforpopup, click_delete_popup);



            return _rootview;
			//return base.OnCreateView (inflater, container, savedInstanceState);
		}

		bool check_required(List<string> str1, List<DBQuestion> qs, int _session, DateTime date_now)
		{
			//var que = new List<DBQuestion> ();
			//var changeque = new List<DBQuestion> ();
			List<bool> true_answer = new List<bool>();
			List<bool> false_answer = new List<bool>();
			List<DBOverviewQuestionAnswer> _list_answer = new List<DBOverviewQuestionAnswer>();
			List<DBQuestion> que = new List<DBQuestion> ();

			for (int i = 0; i < str1.Count; i++) {
				var s = str1 [i];
				if (!s.ToUpper ().Contains ("select a value".ToUpper ()) && !s.ToUpper ().Contains("_,___")) {
					false_answer.Add (false);
				}

				if (qs [i].required) {
					if (s.ToUpper ().Contains ("select a value".ToUpper ()) || s.ToUpper () == "_,___") {
						true_answer.Add (false);
						que.Add (qs[i]);
					} else {
						que.Add (null);
						true_answer.Add (true);
						_list_answer.Add (new DBOverviewQuestionAnswer{
							question = qs[i].question,
							required = qs[i].required,
							name_question = qs[i].name,
							answer = s,
							type_question = qs[i].type,
							datetime = date_now,
							status = "new",
							session = _session
						});
					}
				} else {
					que.Add (null);
					if (s.ToUpper ().Contains ("select a value".ToUpper ()) || s.ToUpper () == "_,___") {
						Console.WriteLine ("answer is empty and not required");
						//true_answer.Add (false);
					} else {
						//true_answer.Add (true);
						_list_answer.Add (new DBOverviewQuestionAnswer{
							question = qs[i].question,
							required = qs[i].required,
							name_question = qs[i].name,
							answer = s,
							type_question = qs[i].type,
							datetime = date_now,
							status = "new",
							session = _session
						});
					}
				}
			}

			if (false_answer.Count != 0) {
				true_answer = true_answer.Where (o => o == false).ToList();
				if (true_answer.Count != 0) {
					var __view = (FragmentClassifyScreen)_viewPager.Adapter.InstantiateItem(_viewPager, 1);
					var listQuestions = __view.View.FindViewById<ListView> (Resource.Id.listQuestions);
					listQuestions.Adapter = new QuestionsListAdapter(Activity, qs, que);
					_viewPager.SetCurrentItem(1, true);
					Toast.MakeText(Activity, "You did not complete all mandatory fields in the classify screen. Please correct.", ToastLength.Short).Show();
					return false;
				}
				//return true;
			} else {
				return true;
			}

			foreach (var la in _list_answer) {
				DBLocalDataStore.GetInstance ().AddOverwievAnswer (la);
			}
			return true;
		}

		void click_item_file(object sender, EventArgs e)
		{
			var btnUp = _rootview.FindViewById<LinearLayout> (Resource.Id.linearLayoutUp);//linearLayoutUp
			//var line_between = _rootview.FindViewById<LinearLayout> (Resource.Id.linearLayout6);

			try{
				int id = 0;
				if (sender is TextView)
				{
					//paymentId = Convert.ToString(((ImageButton) sender).Tag);
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
					listfolders.Adapter = new FilesListAdapter(Activity, l_files, click_add_file, click_item_file);
					if (l_files.Count == 0)
					{
						btnUp.Visibility = ViewStates.Visible;
						//line_between.Visibility = ViewStates.Visible;

						txtFilesNo.Visibility = ViewStates.Visible;
						txtFilesNo.Text = "No files";
					}
					else
					{
						btnUp.Visibility = ViewStates.Visible;
						//line_between.Visibility = ViewStates.Visible;
						txtFilesNo.Visibility = ViewStates.Gone;
//						files.Visibility = ViewStates.Visible;
//						folders.Visibility = ViewStates.Gone;
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
						var _activity = new Intent(Activity, typeof(ActivityDisplayPdf));
						_activity.PutExtra("id_form_overview", l_files[id].localpath);
						StartActivity(_activity);
					}
					else if("image" == l_files[id].mimeType.Split("/".ToCharArray())[0])
					{
						var _activity = new Intent(Activity, typeof(ActivityDisplayImage));
						_activity.PutExtra("id_form_overview", l_files[id].localpath);
						StartActivity(_activity);
					}
//					PdfFixedDocument pdffile = new PdfFixedDocument(l_files[id].localpath);
//					pdffile.DisplayMode = PdfDisplayMode.UseOC;
					//					pdffile.Save(l_files[id].localpath);
//					PdfOutputIntent temp_temp = new PdfOutputIntent();
//					temp_temp.Type = PdfOutputIntentType.Unknown;
//					temp_temp
//					//temp_temp.
//

//					Android.Net.Uri _File = Android.Net.Uri.FromFile (new Java.IO.File (l_files[id].localpath));
//					Intent file_Intent = new Intent (Intent.ActionPaste);
//					file_Intent.SetDataAndType (_File, l_files[id].mimeType);
//					file_Intent.SetFlags (ActivityFlags.NoHistory | ActivityFlags.GrantReadUriPermission);
//					StartActivity (file_Intent);
				}
			}
			catch(Exception ex){
				Toast.MakeText(Activity, ex.Message, ToastLength.Long).Show();
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
                //popUpconract = _rootview.FindViewById<RelativeLayout> (Resource.Id.popRelative);
                //btnShowcontactPopup.Text = ;
                //text in bottom
                ((ImageView)sender).SetImageResource(Resource.Drawable.addButton);
                countFiles++;
                countfiles.Text = countFiles.ToString();
            }
            else {
                l_files[id].activeFile = false;
                DBLocalDataStore.GetInstance().UpdateLocalFile(l_files[id]);
                ((ImageView)sender).SetImageResource(Resource.Drawable.addBattonout);
                countFiles--;
                countfiles.Text = countFiles.ToString();
            }
            var listactivefilesforpopup = DBLocalDataStore.GetInstance().GetAllLocalFiles().Where(s => s.activeFile).ToList();
            listitems.Adapter = new FilesPopupListAdapter(Activity, listactivefilesforpopup, click_delete_popup);
		}

		void receivedM(string m)
		{
			Toast.MakeText(Activity, m, ToastLength.Long).Show();
		}
		void click_delete_popup(object sender, EventArgs e)
		{
			//var lc = DBLocalDataStore.GetInstance ().GetLocalFilesPopup ();
			string id = "";

			if (sender is ImageView)
			{
				id = (string)((ImageView) sender).Tag;
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
                listitems.Adapter = new FilesPopupListAdapter(Activity, listactivefilesforpopup, click_delete_popup);
                scaleImage((ImageView)sender, 1f, 1.2f, 1f, 1.2f, 50f, 50f);


                listfolders.Adapter = new FilesListAdapter(Activity, l_files, click_add_file, click_item_file);

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
            }catch{}
		}

		public override void OnResume ()
		{
			base.OnResume ();
            localcontacts = DBLocalDataStore.GetInstance().GetLocalContacts();
            localcontacts = FragmentClassifyScreen.CheckActiveContacts(localcontacts);
			tapzoneopenfile.Visibility = ViewStates.Gone;
            var listactivefilesforpopup = DBLocalDataStore.GetInstance().GetAllLocalFiles().Where(s => s.activeFile).ToList();
            countfiles.Text = listactivefilesforpopup.Count.ToString();
            countFiles = listactivefilesforpopup.Count;
            listitems.Adapter = new FilesPopupListAdapter (Activity, listactivefilesforpopup, click_delete_popup);
            if (ActivitySelectedContacts.FromShareScreen)
            {
                ActivitySelectedContacts.FromShareScreen = false;
                tt = DBLocalDataStore.GetInstance().GetAllLocalFiles();
                tt.Sort(delegate (DBfileTO name1, DBfileTO name2)
                    { return name1.name.CompareTo(name2.name); });
                l_files = tt.Where(s => s.folderUuid == "_empty_").ToList();

                if (popUpconract.Visibility == ViewStates.Visible)
                {
                    fadeOut = new AlphaAnimation(1f, 0f);
                    fadeOut.Duration = 400;
                    popUpconract.Visibility = ViewStates.Gone;
                    popUpconract.StartAnimation (fadeOut);
                }
                btnUp.Visibility = ViewStates.Gone;
                listfolders.Adapter = new FilesListAdapter(Activity, l_files, click_add_file, click_item_file, true);

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
				//listfiles.Adapter = new FilesListAdapter(Activity, l_files, click_add_file, click_item_file, list_popup_temp);

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
        void ClearActiveContacts()
        {
            localcontacts = DBLocalDataStore.GetInstance().GetLocalContacts();
            for (int i = 0; i < localcontacts.Count; i++)
            {
                if (localcontacts[i].activeContact == true)
                {
                    localcontacts[i].activeContact = false;
                    DBLocalDataStore.GetInstance().UpdateLocalContact(localcontacts[i]);
                }
            }
        }

	}

}

