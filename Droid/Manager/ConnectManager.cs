using System;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using Android.Drm;
using Android.App.Admin;
using Android.OS;
using Android.Net;
using Android.App;
using Android.Content;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.Util;
using Android.Provider;
using BoaBeePCL;
using System.Linq;

namespace Leadbox
{
	public class ConnectManager
	{
		private static ConnectManager _instance;
		private static object _lockCreate = new object();
		public delegate void LogHandler(string message);
		public delegate void StatusDownload(bool status, string message);
		//int countConnection = 0;

		public ConnectManager ()
		{
		}

		public static ConnectManager GetInstance()
		{
			lock (_lockCreate)
			{
				if (_instance == null)
				{
					_instance = new ConnectManager();
				}
			}
			return _instance;
		}

		public enum NetworkState
		{
			Unknown,
			ConnectedWifi,
			ConnectedData,
			Disconnected
		}

		public NetworkState stateNet()
		{
			var connectivityManager = (ConnectivityManager)Application.Context.GetSystemService (Context.ConnectivityService);
			var activeNetworkInfo = connectivityManager.ActiveNetworkInfo;
			var _state = NetworkState.Unknown;
			if (activeNetworkInfo != null && activeNetworkInfo.IsConnectedOrConnecting) {
				// Now that we know it's connected, determine if we're on WiFi or something else.
				_state = activeNetworkInfo.Type == ConnectivityType.Wifi ? NetworkState.ConnectedWifi : NetworkState.ConnectedData;
			}

			return _state;
		}

		public bool StateNet()
		{
			if (stateNet() == ConnectManager.NetworkState.ConnectedWifi || stateNet() == ConnectManager.NetworkState.ConnectedData)
				return true;

			return false;
		}


		public async void updateProfiles (LogHandler loghandler)
		{
			//ExchengeInfo receivedData;
			var jsonUser = JsonConvert.SerializeObject (DBLocalDataStore.GetInstance().GetLocalUserInfo());

			var httpWebRequest = (HttpWebRequest)WebRequest.Create ("http://services.boabee.com:80/services/auth/login");
			httpWebRequest.ContentType = "application/json";
			httpWebRequest.Method = "POST";
			httpWebRequest.ServicePoint.ConnectionLimit = 3;
			//httpWebRequest.ServicePoint.Expect100Continue = false;
			httpWebRequest.Timeout = 10000;

			using (var streamWriter = new StreamWriter (httpWebRequest.GetRequestStream ())) {

				streamWriter.Write (jsonUser);
			}
			try{
				//var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse ();

				Task<WebResponse> task = Task.Factory.FromAsync(
					httpWebRequest.BeginGetResponse,
					asyncResult => httpWebRequest.EndGetResponse(asyncResult),
					(object)null);
				await task.ContinueWith(t => updateProfilesFromResponse(t.Result));
				loghandler("Profiles is update.");
			}
			catch(Exception ex)
			{
				loghandler("Error connection.");
				Console.WriteLine (ex.Message);
			}
		}

		string updateProfilesFromResponse (WebResponse result)
		{
			var httpResponse = result.GetResponseStream ();
			using (var streamReader = new StreamReader (httpResponse)) {
				var result1 = streamReader.ReadToEnd ();

				var receivedData = JsonConvert.DeserializeObject<ExchengeInfo> (result1);

				DBLocalDataStore.GetInstance().ClearBasicAuthority();
				foreach(var rd in receivedData.profiles)
				{
					DBLocalDataStore.GetInstance().AddBasicAuthority(rd);
				}

				return result1;
			}
		}
		public async void GetContacts (LogHandler loghandler)
		{
            //CustomerSyncResult receivedData;

            BoaBeePCL.SyncContext scr = new BoaBeePCL.SyncContext ();
			var user = DBLocalDataStore.GetInstance ().GetLocalUserInfo ();
            scr.context = new RequestData ();
            //scr.contacts = new CustomerType[]{};
			var output = scr.context; 
			output.password = user.password;
			output.username = user.username;
			output.profile = DBLocalDataStore.GetInstance ().GetSelectProfile ().shortName;

			var jsonUser = JsonConvert.SerializeObject (scr);
			try{

				var httpWebRequest = (HttpWebRequest)WebRequest.Create ("http://services.boabee.com:80/services/sales/customers");
				httpWebRequest.ContentType = "application/json";
				httpWebRequest.Method = "POST";
				httpWebRequest.Timeout = 10000;

				using (var streamWriter = new StreamWriter (httpWebRequest.GetRequestStream ())) {

					streamWriter.Write (jsonUser);
				}
				Task<WebResponse> task = Task.Factory.FromAsync(
					httpWebRequest.BeginGetResponse,
					asyncResult => httpWebRequest.EndGetResponse(asyncResult),
					(object)null);
				await task.ContinueWith(t => GetContactsFromResponse(t.Result, loghandler));

			}
			catch(Exception ex)
			{
				loghandler (ex.Message);
				Console.WriteLine (ex.Message);
			}
		}

		string GetContactsFromResponse (WebResponse result, LogHandler loghandler)
		{
			var httpResponse = result.GetResponseStream ();
			using (var streamReader = new StreamReader (httpResponse)) {
				var result1 = streamReader.ReadToEnd ();

				var receivedData = JsonConvert.DeserializeObject<CustomerSyncResult> (result1);
                var contacts = receivedData.contacts;
				DBLocalDataStore.GetInstance().ClearAllContacts();
				//DBLocalDataStore.GetInstance().ClearAllShare();
				foreach (var contact in contacts)
				{
					var localContact = new DBlocalContact ();
                    localContact.uid = contact.uid;
					localContact.company = contact.company;
					localContact.email = contact.email;
                    localContact.firstname = contact.firstname;
                    localContact.lastname = contact.lastname;
					localContact.phone = contact.phone;
                    localContact.jobtitle = contact.jobtitle;
					//localContact.uuid = contact.uuid;
					//if (contact. != null) {
					//	Console.WriteLine ("Barcode = true");
					//	if (contact.badge.properties != null) {
					//		localContact.barcode = contact.badge.properties.barcode;
					//		Console.WriteLine ("Barcode = " + localContact.barcode);
					//	}
					//}
					Console.WriteLine ("Add contact");
					DBLocalDataStore.GetInstance ().AddLocalContact (localContact);

				}
				loghandler("Contacts update from server");
			}

			return "";
		}
			
		public void SetContactToServer (DBpopupContact contact, LogHandler loghandler)
		{

			CustomerSyncResult receivedData;
			BoaBeePCL.SyncContext scr = new BoaBeePCL.SyncContext ();
			var user = DBLocalDataStore.GetInstance ().GetLocalUserInfo ();
			scr.context = new RequestData ();
            //scr.contacts = new CustomerType[1];
			var outContext = scr.context;
            //scr.contacts [0] = new CustomerType ();
            var outCustomers = scr.contacts [0];
			outContext.password = user.password;
			outContext.username = user.username;
			outContext.profile = DBLocalDataStore.GetInstance ().GetSelectProfile ().shortName;

			outCustomers.company = contact.company;
			outCustomers.firstname = contact.firstName;
			outCustomers.lastname = contact.lastName;
			outCustomers.email = contact.email;
			outCustomers.phone = contact.phone;
			


			var jsonUser = JsonConvert.SerializeObject (scr);
			try{
				
			var httpWebRequest = (HttpWebRequest)WebRequest.Create ("http://services.boabee.com:80/services/sales/customers");
			httpWebRequest.ContentType = "application/json";
			httpWebRequest.Method = "POST";
			httpWebRequest.ServicePoint.ConnectionLimit = 3;
			//httpWebRequest.ServicePoint.Expect100Continue = false;
			httpWebRequest.Timeout = 10000;
			//httpWebRequest


			using (var streamWriter = new StreamWriter (httpWebRequest.GetRequestStream() )) {

				streamWriter.WriteAsync (jsonUser);
			}

				var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();// .GetResponse ();
				using (var streamReader = new StreamReader (httpResponse.GetResponseStream ())) {
					var result = streamReader.ReadToEnd ();
					Console.WriteLine(result);
					receivedData = JsonConvert.DeserializeObject<CustomerSyncResult> (result);
					Console.WriteLine("Set data");
					loghandler("Contacts status send to server = " + receivedData.success);
				}
			}
			catch(Exception ex)
			{
				loghandler (ex.Message);
				Console.WriteLine (ex.Message);
				//SetContactToServer (contact, loghandler);
			}
		}
		public void SetWebHook (DBlocalContact contact, LogHandler loghandler,string https)
		{
			CustomerSyncResult receivedData;
			BoaBeePCL.SyncContext scr = new BoaBeePCL.SyncContext ();
			var user = DBLocalDataStore.GetInstance ().GetLocalUserInfo ();
			scr.context = new RequestData ();
            //scr.contacts = new CustomerType[1];
			var outContext = scr.context;
            //scr.contacts [0] = new CustomerType ();
            var outCustomers = scr.contacts [0];
			outContext.password = user.password;
			outContext.username = user.username;
			outContext.profile = DBLocalDataStore.GetInstance ().GetSelectProfile ().shortName;

			outCustomers.company = contact.company;
            outCustomers.firstname = contact.firstname;
            outCustomers.lastname = contact.lastname;
			outCustomers.email = contact.email;
			outCustomers.phone = contact.phone;
		


			var jsonUser = JsonConvert.SerializeObject (scr);
			try{

				var httpWebRequest = (HttpWebRequest)WebRequest.Create (https);
				httpWebRequest.ContentType = "application/json";
				httpWebRequest.Method = "POST";
				httpWebRequest.ServicePoint.ConnectionLimit = 3;
				//httpWebRequest.ServicePoint.Expect100Continue = false;
				httpWebRequest.Timeout = 10000;
				//httpWebRequest


				using (var streamWriter = new StreamWriter (httpWebRequest.GetRequestStream() )) {

					streamWriter.WriteAsync (jsonUser);
				}

				var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();// .GetResponse ();
				using (var streamReader = new StreamReader (httpResponse.GetResponseStream ())) {
					var result = streamReader.ReadToEnd ();
					Console.WriteLine(result);
					receivedData = JsonConvert.DeserializeObject<CustomerSyncResult> (result);
					Console.WriteLine("Set data");
					loghandler("Contacts status send to server = " + receivedData.success);
				}
			}
			catch(Exception ex)
			{
				loghandler (ex.Message);
				Console.WriteLine (ex.Message);
				//SetContactToServer (contact, loghandler);
			}
		}
		//List<DBlocalContact> contacts, 
		public void SetAllContactsToServer (List<DBOverviewContacts> contacts_over, StatusDownload loghandler)
		{
			if (!StateNet ()) {
				loghandler (false, "Access to internet is failed.");
				return;
			}



            CustomerSyncResult receivedData;
			BoaBeePCL.SyncContext scr = new BoaBeePCL.SyncContext ();
			var user = DBLocalDataStore.GetInstance ().GetLocalUserInfo ();
			scr.context = new RequestData ();
            //scr.contacts = new CustomerType[contacts_over.Count];
			var outContext = scr.context;
			//scr.customers = new CustomerType[];
			outContext.password = user.password;
			outContext.username = user.username;
			outContext.profile = DBLocalDataStore.GetInstance ().GetSelectProfile ().shortName;
            outContext.tags = new string[] { user.tags };

			for (int i = 0; i < contacts_over.Count; i++) {
				
				var outCustomers = new CustomerType ();
				outCustomers.company = contacts_over[i].company;
				outCustomers.firstname = contacts_over[i].firstName;
				outCustomers.lastname = contacts_over[i].lastName;
				outCustomers.email = contacts_over[i].email;
				outCustomers.phone = contacts_over[i].phone;
                outCustomers.jobtitle = contacts_over[i].jobTitle;

	///////////////////////////////////////////WARNING BADGE/////////////////////////////////////////////////////////////////		
    //            if (!string.IsNullOrEmpty(contacts_over [i].barcode)) {
    //                outCustomers.uid = new BrandTappBadge ();
				//	outCustomers.badge.properties = new IBadgeProperties ();
				//	outCustomers.badge.properties.barcode = contacts_over [i].barcode;
				//	outCustomers.badge.type = "barcode";
				//}

                //scr.contacts [i] = outCustomers;
			}
				
			var jsonUser = JsonConvert.SerializeObject (scr);
			try{
				var httpWebRequest = (HttpWebRequest)WebRequest.Create ("http://services.boabee.com:80/services/sales/customers");
				httpWebRequest.ContentType = "application/json";
				httpWebRequest.Method = "POST";
				httpWebRequest.ServicePoint.ConnectionLimit = 3;
				//httpWebRequest.ServicePoint.Expect100Continue = false;
				httpWebRequest.Timeout = 10000;
				//httpWebRequest


				using (var streamWriter = new StreamWriter (httpWebRequest.GetRequestStream() )) {

					streamWriter.WriteAsync (jsonUser);
				}

				var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();// .GetResponse ();
				using (var streamReader = new StreamReader (httpResponse.GetResponseStream ())) {
					var result = streamReader.ReadToEnd ();
					Console.WriteLine(result);
					receivedData = JsonConvert.DeserializeObject<CustomerSyncResult> (result);
                    var contacts_received = receivedData.contacts;
					//DBLocalDataStore.GetInstance().ClearAllContacts();
					//here is new send customers from sync
					var local_contact_list = DBLocalDataStore.GetInstance().GetLocalContacts();
					//DBLocalDataStore.GetInstance().ClearAllContacts();
					//return;

					
					/*
					foreach (var contact in contacts_received)
					{
						//EMAIL, NO BARCODE
						if (!string.IsNullOrWhiteSpace(contact.email) && contact.badge == null){
							Console.WriteLine("EMAIL, NO BARCODE");
							var _local_contact = local_contact_list.Find(s=>s.email == contact.email);
							if (_local_contact != null){
								//YES
								if (_local_contact.source != "server") {
									_local_contact.source = "server";
									_local_contact.uuid = contact.uuid;
									DBLocalDataStore.GetInstance ().UpdateLocalContact(_local_contact);
								}
							} else {
								//NO
								var localContact_ = new DBlocalContact ();
								localContact_.uuid = contact.uuid;
								localContact_.company = contact.company;
								localContact_.email = contact.email;
								localContact_.firstName = contact.firstname;
								localContact_.lastName = contact.lastname;
								localContact_.phone = contact.phone;
								localContact_.source = contact.source;
								DBLocalDataStore.GetInstance ().AddLocalContact (localContact_);
							}
							//BARCODE, NO EMAIL
						} else if (string.IsNullOrWhiteSpace(contact.email) && contact.badge != null){
							Console.WriteLine("BARCODE, NO EMAIL");
							var _local_contact = local_contact_list.Find(s=>s.barcode == contact.badge.properties.barcode);
							if (_local_contact != null){
								//YES
								if (_local_contact.source != "server") {
									_local_contact.source = "server";
									_local_contact.uuid = contact.uuid;
									DBLocalDataStore.GetInstance ().UpdateLocalContact(_local_contact);
								}
							} else {
								//NOOOOOOOOOOOOOOO
								var localContact_ = new DBlocalContact ();
								localContact_.uuid = contact.uuid;
								localContact_.company = contact.company;
								localContact_.email = contact.email;
								localContact_.firstName = contact.firstname;
								localContact_.lastName = contact.lastname;
								localContact_.phone = contact.phone;
								localContact_.source = contact.source;
								localContact_.barcode = contact.badge.properties.barcode;
								DBLocalDataStore.GetInstance ().AddLocalContact (localContact_);
							}
							//EMAIL AND BARCODE
						} else if (!string.IsNullOrWhiteSpace(contact.email) && contact.badge != null){
							Console.WriteLine("EMAIL AND BARCODE");
							local_contact_list = DBLocalDataStore.GetInstance().GetLocalContacts();
							var _local_contact_barcode = local_contact_list.Find(s=>s.barcode == contact.badge.properties.barcode);
							var _local_contact = local_contact_list.Find(s=>s.email == contact.email);
							if (_local_contact != null){
								//YES
								if (_local_contact.source != "server") {
									_local_contact.source = "server";
									_local_contact.uuid = contact.uuid;
									_local_contact.barcode = contact.badge.properties.barcode;
									DBLocalDataStore.GetInstance ().UpdateLocalContact(_local_contact);
								}

								if (_local_contact_barcode != null){
									DBLocalDataStore.GetInstance ().DeleteLocalContact(_local_contact_barcode);
								}
							} else {
								//NO
								if (_local_contact_barcode != null){
									//YES
									_local_contact_barcode.company = contact.company;
									_local_contact_barcode.email = contact.email;
									_local_contact_barcode.firstName = contact.firstname;
									_local_contact_barcode.lastName = contact.lastname;
									_local_contact_barcode.phone = contact.phone;
									_local_contact_barcode.source = contact.source;
									DBLocalDataStore.GetInstance ().UpdateLocalContact(_local_contact_barcode);
								}
								else{
									//NO
									var localContact = new DBlocalContact ();
									localContact.company = contact.company;
									localContact.email = contact.email;
									localContact.firstName = contact.firstname;
									localContact.lastName = contact.lastname;
									localContact.phone = contact.phone;
									localContact.source = contact.source;
									localContact.barcode = contact.badge.properties.barcode;
									DBLocalDataStore.GetInstance ().AddLocalContact (localContact);
								}
							}



						}
						

						/*var localContact = new DBlocalContact ();
						localContact.company = contact.company;
						localContact.email = contact.email;
						localContact.firstName = contact.firstname;
						localContact.lastName = contact.lastname;
						localContact.phone = contact.phone;
						localContact.source = contact.source;
						if (contact.badge != null) {
							Console.WriteLine ("Barcode = true");
							if (contact.badge.properties != null) {
								localContact.barcode = contact.badge.properties.barcode;
								Console.WriteLine ("Barcode = " + localContact.barcode);
							}
						}
						Console.WriteLine ("Add contact");
						DBLocalDataStore.GetInstance ().AddLocalContact (localContact);

					}*/
					Console.WriteLine("Set data");
					loghandler(true, "Contacts status send to server = " + receivedData.success);
				}
			}
			catch(Exception ex)
			{
				loghandler (false, ex.Message);
				Console.WriteLine (ex.Message);
				//SetContactToServer (contact, loghandler);
			}
		}


		public async void GetUpdateApp (StatusDownload loghandler, Activity _context)
		{

            //CampaignAppUpdateRequestResult receivedData;
			BoaBeePCL.SyncContext scr = new BoaBeePCL.SyncContext ();
			var user = DBLocalDataStore.GetInstance ().GetLocalUserInfo ();
            scr.context = new RequestData ();
            //scr.contacts = new CustomerType[]{};
			var output = scr.context; 
			output.password = user.password;
			output.username = user.username;
			output.profile = DBLocalDataStore.GetInstance ().GetSelectProfile ().shortName;

			var jsonUser = JsonConvert.SerializeObject (scr.context);
			try{

				var httpWebRequest = (HttpWebRequest)WebRequest.Create ("http://services.boabee.com:80/services/update/app");
				httpWebRequest.ContentType = "application/json";
				httpWebRequest.Method = "POST";
				httpWebRequest.ServicePoint.ConnectionLimit = 3;
				//httpWebRequest.ServicePoint.Expect100Continue = false;
				httpWebRequest.Timeout = 10000;

				using (var streamWriter = new StreamWriter (httpWebRequest.GetRequestStream ())) {

					streamWriter.Write (jsonUser);
				}

				Task<WebResponse> task = Task.Factory.FromAsync(
					httpWebRequest.BeginGetResponse,
					asyncResult => httpWebRequest.EndGetResponse(asyncResult),
					(object)null);
				await task.ContinueWith(t => GetUpdateAppFromResponse(t.Result, loghandler, _context));

				//loghandler("Customers update from server");
			}
			catch(Exception ex)
			{
				loghandler (false, ex.Message);
				Console.WriteLine (ex.Message);
			}
		}

		string GetUpdateAppFromResponse (WebResponse result, StatusDownload loghandler, Activity _context)
		{
			var httpResponse = result.GetResponseStream ();
			using (var streamReader = new StreamReader (httpResponse)) {
				var result1 = streamReader.ReadToEnd ();
				Console.WriteLine(result);
				var receivedData = JsonConvert.DeserializeObject<CampaignAppUpdateRequestResult> (result1);
				//receivedData.forms = JsonConvert.DeserializeObject<FormDefinition[]> (result);
				if (receivedData.defaultForm != null)
					DBLocalDataStore.GetInstance().AddLocalFormDefinitions(new []{receivedData.defaultForm});
				else
					DBLocalDataStore.GetInstance().AddLocalFormDefinitions(receivedData.forms);

				DBLocalDataStore.GetInstance ().ClearAllFiles ();
				DBLocalDataStore.GetInstance ().ClearAllFolders ();
				DBLocalDataStore.GetInstance ().AddLocalFolders (receivedData.folders);
                DBAppInfo AppInfo = new DBAppInfo();

				AppInfo.appType = (receivedData.appType != null) ? receivedData.appType.ToLower() : null;

				AppInfo.welcomeImageURL = (receivedData.welcomeScreenImage != null) ? receivedData.welcomeScreenImage.downloadUrl : null;
				AppInfo.finishedImageURL = (receivedData.finishedScreenImage != null) ? receivedData.finishedScreenImage.downloadUrl : null;

				AppInfo.welcomeImageFileType = (receivedData.welcomeScreenImage != null) ? receivedData.welcomeScreenImage.fileType : null;
				AppInfo.finishedImageFileType = (receivedData.finishedScreenImage != null) ? receivedData.finishedScreenImage.fileType : null;

				AppInfo.welcomeImageLocalPath = null;
				AppInfo.finishedImageLocalPath = null;

				AppInfo.welcomeImageMD5 = (receivedData.welcomeScreenImage != null) ? receivedData.welcomeScreenImage.md5 : null;
				AppInfo.finishedImageMD5 = (receivedData.finishedScreenImage != null) ? receivedData.finishedScreenImage.md5 : null;

                AppInfo.campaignReference = receivedData.campaignReference;

                DBLocalDataStore.GetInstance().SetAppInfo(AppInfo);

				//DBLocalDataStore.GetInstance()
				//SaveFilesManager.GetInstance ().DeleteAllFields (new Java.IO.File(Path.Combine (Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, "BoaBee")));
				if (receivedData.folders.Length == 0)
					DBLocalDataStore.GetInstance ().ShowLocalFiles (true);
				else 
					DBLocalDataStore.GetInstance ().ShowLocalFiles (false);
				

				_context.RunOnUiThread (()=>{
					Console.WriteLine("Get data");
					loghandler(true, "App update from server = " + receivedData.success);
				});

			}
			return "";
		}

		public async void GetOnlyQuestions (StatusDownload loghandler)
		{

			//CampaignAppUpdateRequestResult receivedData;

			BoaBeePCL.SyncContext scr = new BoaBeePCL.SyncContext ();
			var user = DBLocalDataStore.GetInstance ().GetLocalUserInfo ();
			scr.context = new RequestData ();
          //  scr.contacts = new CustomerType[]{};
			var output = scr.context; 
			output.password = user.password;
			output.username = user.username;
			output.profile = DBLocalDataStore.GetInstance ().GetSelectProfile ().shortName;

			var jsonUser = JsonConvert.SerializeObject (scr.context);
			try{

				var httpWebRequest = (HttpWebRequest)WebRequest.Create ("http://services.boabee.com:80/services/update/app");
				httpWebRequest.ContentType = "application/json";
				httpWebRequest.Method = "POST";
				httpWebRequest.ServicePoint.ConnectionLimit = 3;
				//httpWebRequest.ServicePoint.Expect100Continue = false;
				httpWebRequest.Timeout = 10000;

				using (var streamWriter = new StreamWriter (httpWebRequest.GetRequestStream ())) {

					streamWriter.Write (jsonUser);
				}

				Task<WebResponse> task = Task.Factory.FromAsync(
					httpWebRequest.BeginGetResponse,
					asyncResult => httpWebRequest.EndGetResponse(asyncResult),
					(object)null);
				await task.ContinueWith(t => GetOnlyQuestionsFromResponse(t.Result, loghandler));

				//loghandler("Customers update from server");
			}
			catch(Exception ex)
			{
				//loghandler (false, ex.Message);
				Console.WriteLine (ex.Message);
			}
		}

		string GetOnlyQuestionsFromResponse (WebResponse result, StatusDownload loghandler)
		{
			var httpResponse = result.GetResponseStream ();
			using (var streamReader = new StreamReader (httpResponse)) {
				var result1 = streamReader.ReadToEnd ();
				Console.WriteLine(result1);
				var receivedData = JsonConvert.DeserializeObject<CampaignAppUpdateRequestResult> (result1);
				//receivedData.forms = JsonConvert.DeserializeObject<FormDefinition[]> (result);

				DBLocalDataStore.GetInstance().AddLocalFormDefinitions(receivedData.forms);

				Console.WriteLine("Get data");
				loghandler(true, "App update from server = " + receivedData.success);
			}
			return "";
		}


		public void SetForms (StatusDownload loghandler)
		{
			List<AnsweredForm> _form = new List<AnsweredForm> ();
			ExchengeInfo receivedData;

			AnsweredFormsSyncRequest scr = new AnsweredFormsSyncRequest ();
			var user = DBLocalDataStore.GetInstance ().GetLocalUserInfo ();
			scr.context = new RequestData ();
			//scr.forms = new AnsweredForm[]{};
			var output = scr.context; 
			output.password = user.password;
			output.username = user.username;
			output.profile = DBLocalDataStore.GetInstance ().GetSelectProfile ().shortName;
//			foreach (var form in scr.forms) {
			var _list_customers = DBLocalDataStore.GetInstance ().GetOverwievContacts(-1, "new_send").FindAll(s=>s.isanswers == true);
			var list_number_session = new List<int> ();
			var prof = DBLocalDataStore.GetInstance ().GetLocalFormDefinitions ().Find(s=>s.uuid == DBLocalDataStore.GetInstance ().GetSelectedQuestionPosition());


			foreach (var custom in _list_customers) {
				var _list_answ = DBLocalDataStore.GetInstance ().GetOverwievAnswers (custom.session, "new");
				if (_list_answ.Count != 0) {
					list_number_session.Add (custom.session);
				}
				var _listanswer = new List<Answer> ();
				foreach (var answ in _list_answ) {
					if (answ.status == "new")
					_listanswer.Add (new Answer{
						name = answ.name_question,
						type = answ.type_question,
						answer = answ.answer
					});
				}
                ///////////////////////////////////////////WARNING BADGE/////////////////////////////////////////////////////////////////       
                //foreach (var answ in _list_answ) {
                _form.Add (new AnsweredForm {
					name = prof.objectName,
					enddate = string.Format("{0:yyyy-MM-ddTH:mm:sszzz}", new DateTimeOffset(custom.datetime)),
					startdate = string.Format("{0:yyyy-MM-ddTH:mm:sszzz}", new DateTimeOffset(custom.datetime)),
					user = new DeviceUser{username = user.username, profile = output.profile},
					//customer = new CustomerType{
					//	company = custom.company,
						
					//	email = custom.email,
					//	firstname = custom.firstName,
					//	lastname = custom.lastName,
     //                   jobtitle = custom.jobTitle,
     //                   uid = custom.barcode
					//},
					//answers = _listanswer.ToArray()
				});
				//}
			}

			scr.forms = _form.ToArray ();

			var jsonUser = JsonConvert.SerializeObject (scr);
			Console.WriteLine(jsonUser);
			try{

				var httpWebRequest = (HttpWebRequest)WebRequest.Create ("http://services.boabee.com:80/services/sales/forms");
				httpWebRequest.ContentType = "application/json";
				httpWebRequest.Method = "POST";
				httpWebRequest.ServicePoint.ConnectionLimit = 3;
				//httpWebRequest.ServicePoint.Expect100Continue = false;
				httpWebRequest.Timeout = 10000;

				using (var streamWriter = new StreamWriter (httpWebRequest.GetRequestStream ())) {

					streamWriter.Write (jsonUser);
				}
				var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse ();
				using (var streamReader = new StreamReader (httpResponse.GetResponseStream ())) {
					var result = streamReader.ReadToEnd ();
					Console.WriteLine(result);
					receivedData = JsonConvert.DeserializeObject<ExchengeInfo> (result);
                    if (receivedData.success){
                    //if (receivedData.success.ToUpper() == "True".ToUpper()){
						var _list_answ = DBLocalDataStore.GetInstance ().GetOverwievAnswers (-1, "new");

						foreach (var answ in _list_answ){
							if (list_number_session.Exists(s=>s == answ.session)){
								answ.status = "server";
								DBLocalDataStore.GetInstance().UpdateOverwievAnswer(answ);
							}
						}
						loghandler(true, "Set forms and answers to server = " + receivedData.success);
					}
					//receivedData.forms = JsonConvert.DeserializeObject<FormDefinition[]> (result);
					Console.WriteLine("Get data");
					//loghandler(true, "Set forms and answers to server = " + receivedData.success);
				}
			}
			catch(Exception ex)
			{
				loghandler (false, ex.Message);
				Console.WriteLine (ex.Message);
			}
		}

		public void SetFormsForUpdate (StatusDownload loghandler)
		{
			List<AnsweredForm> _form = new List<AnsweredForm> ();
			ExchengeInfo receivedData;

			AnsweredFormsSyncRequest scr = new AnsweredFormsSyncRequest ();
			var user = DBLocalDataStore.GetInstance ().GetLocalUserInfo ();
			scr.context = new RequestData ();
			//scr.forms = new AnsweredForm[]{};
			var output = scr.context; 
			output.password = user.password;
			output.username = user.username;
			output.profile = DBLocalDataStore.GetInstance ().GetSelectProfile ().shortName;
			//			foreach (var form in scr.forms) {
			var _list_customers = DBLocalDataStore.GetInstance ().GetOverwievContacts(-1, "updateForAnswer").FindAll(s=>s.isanswers == true);
			var list_number_session = new List<int> ();
			var prof = DBLocalDataStore.GetInstance ().GetLocalFormDefinitions ().Find(s=>s.uuid == DBLocalDataStore.GetInstance ().GetSelectedQuestionPosition());


			foreach (var custom in _list_customers) {
				var _list_answ = DBLocalDataStore.GetInstance ().GetOverwievAnswers (custom.session, "update");
				if (_list_answ.Count != 0) {
					list_number_session.Add (custom.session);
				}
				var _listanswer = new List<Answer> ();
				foreach (var answ in _list_answ) {
					if (answ.status == "update")
						_listanswer.Add (new Answer{
							name = answ.name_question,
							type = answ.type_question,
							answer = answ.answer
						});
				}

				//foreach (var answ in _list_answ) {
				_form.Add (new AnsweredForm {
					name = prof.objectName,
					enddate = string.Format("{0:yyyy-MM-ddTH:mm:sszzz}", new DateTimeOffset(custom.datetime)),
					startdate = string.Format("{0:yyyy-MM-ddTH:mm:sszzz}", new DateTimeOffset(custom.datetime)),
					user = new DeviceUser{username = user.username, profile = output.profile},
					//customer = new CustomerType{
					//	company = custom.company,
					//	//created = string.Format("{0:yyyy-MM-ddTH:mm:sszzz}", new DateTimeOffset(custom.datetime)),
					//	email = custom.email,
					//	firstname = custom.firstName,
					//	lastname = custom.lastName,
					//	jobtitle = custom.jobTitle,
						//badge = new BrandTappBadge{properties = new IBadgeProperties{barcode = custom.barcode}, type = "barcode"}
					//},
					answers = _listanswer.ToArray()
				});
				//}
			}

			scr.forms = _form.ToArray ();

			var jsonUser = JsonConvert.SerializeObject (scr);
			Console.WriteLine(jsonUser);
			try{

				var httpWebRequest = (HttpWebRequest)WebRequest.Create ("http://services.boabee.com:80/services/sales/forms");
				httpWebRequest.ContentType = "application/json";
				httpWebRequest.Method = "POST";
				httpWebRequest.ServicePoint.ConnectionLimit = 3;
				//httpWebRequest.ServicePoint.Expect100Continue = false;
				httpWebRequest.Timeout = 10000;

				using (var streamWriter = new StreamWriter (httpWebRequest.GetRequestStream ())) {

					streamWriter.Write (jsonUser);
				}
				var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse ();
				using (var streamReader = new StreamReader (httpResponse.GetResponseStream ())) {
					var result = streamReader.ReadToEnd ();
					Console.WriteLine(result);
					receivedData = JsonConvert.DeserializeObject<ExchengeInfo> (result);
                    if (receivedData.success){
                    //if (receivedData.success.ToUpper() == "True".ToUpper()){
						var _list_answ = DBLocalDataStore.GetInstance ().GetOverwievAnswers (-1, "update");

						foreach (var answ in _list_answ){
							if (list_number_session.Any(s=>s == answ.session)){
								answ.status = "updateserver";
								Console.WriteLine("update answer");
								DBLocalDataStore.GetInstance().UpdateOverwievAnswer(answ);
							}
						}
						loghandler(true, "Set forms and answers to server = " + receivedData.success);
					}
					//receivedData.forms = JsonConvert.DeserializeObject<FormDefinition[]> (result);
					Console.WriteLine("Set forms and answers to server = " + receivedData.success);
					//loghandler(true, "Set forms and answers to server = " + receivedData.success);
				}
			}
			catch(Exception ex)
			{
				loghandler (false, ex.Message);
				Console.WriteLine (ex.Message);
			}
		}

		public async void SetOrdersContext (bool flag_send_message, StatusDownload loghandler)
		{
			SalesOrderContext receivedData = new SalesOrderContext ();
			var user = DBLocalDataStore.GetInstance ().GetLocalUserInfo ();
			var _context = receivedData.context = new RequestData ();
			_context.profile = DBLocalDataStore.GetInstance ().GetSelectProfile ().shortName;
			_context.password = user.password;
			_context.username = user.username;
			var share_customers = DBLocalDataStore.GetInstance ().GetOverwievContacts (-1, "new_send");
			if (flag_send_message) {
				var list_contacts = share_customers.FindAll (s => s.isfiles == true);
				receivedData.orders = DBLocalDataStore.GetInstance ().GetAllContactsFromShare (list_contacts).ToArray ();
			} else {
                  	
                var _list_ordertype = new List<OrderType> ();
				foreach (var ls in share_customers) {
					var _order = new OrderType ();
					var outCustomers = new CustomerType ();
					outCustomers.company = ls.company;
					outCustomers.firstname = ls.firstName;
					outCustomers.lastname = ls.lastName;
					outCustomers.email = ls.email;
					outCustomers.phone = ls.phone;
					
                    outCustomers.jobtitle = ls.jobTitle;
                    outCustomers.uid = ls.barcode;
					//2015-09-08T13:34:45+06:00

					_order.created = string.Format ("{0:yyyy-MM-ddTH:mm:sszzz}", new DateTimeOffset (ls.datetime));//string.Format("{0:yyyy-MM-ddTH:mm:sszzz}", dto);
					//Console.WriteLine(_order.created);
					_order.customer = outCustomers;
					_order.creator = DBLocalDataStore.GetInstance ().GetUserMail ();
					_order.profile = DBLocalDataStore.GetInstance ().GetSelectProfile ().shortName;
					_list_ordertype.Add (_order);
				}
				receivedData.orders = _list_ordertype.ToArray ();
			}

			var jsonUser = JsonConvert.SerializeObject (receivedData);
			Console.WriteLine (jsonUser);
			try{
				var httpWebRequest = (HttpWebRequest)WebRequest.Create ("http://services.boabee.com:80/services/sales/orders");
				httpWebRequest.ContentType = "application/json";
				httpWebRequest.Method = "POST";
				httpWebRequest.ServicePoint.ConnectionLimit = 3;
				//httpWebRequest.ServicePoint.Expect100Continue = false;
				httpWebRequest.Timeout = 10000;

				using (var streamWriter = new StreamWriter (httpWebRequest.GetRequestStream ())) {

					streamWriter.Write (jsonUser);
				}

				Task<WebResponse> task = Task.Factory.FromAsync(
					httpWebRequest.BeginGetResponse,
					asyncResult => httpWebRequest.EndGetResponse(asyncResult),
					(object)null);
				await task.ContinueWith(t => SetOrdersContextFromResponse(t.Result, share_customers, loghandler));

				//loghandler("Customers update from server");
			}
			catch(Exception ex)
			{
				loghandler (false, ex.Message);
				Console.WriteLine (ex.Message);
			}


		}

		object SetOrdersContextFromResponse (WebResponse result, List<DBOverviewContacts> share_customers, StatusDownload loghandler)
		{
			var httpResponse = result.GetResponseStream ();
			using (var streamReader = new StreamReader (httpResponse)) {
				var result1 = streamReader.ReadToEnd ();
				Console.WriteLine(result1);
				var receivedData = JsonConvert.DeserializeObject<ExchengeInfo> (result1);
				//receivedData.forms = JsonConvert.DeserializeObject<FormDefinition[]> (result);
                if (receivedData.success){
                //if (receivedData.success.ToUpper () == "True".ToUpper ()) {
					
					var list_order = DBLocalDataStore.GetInstance ().GetOverwievFiles (-1, "new");
					foreach(var order in list_order){
						if (share_customers.Exists (s => s.session == order.session)) {
							order.status = "server";
							DBLocalDataStore.GetInstance ().UpdateOverwievFile (order);
						}
					}
					loghandler (true, "All orders send to server.\n(This message for test)");

				} else {
					loghandler (false, "Error send to server");
				}
				//Console.WriteLine("Get data");
				//loghandler("App update from server = " + receivedData.success);
			}
			return "";
		}

	}
}

