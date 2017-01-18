using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using System.Threading.Tasks;
using System.Xml;
using BoaBeePCL;
using System.Linq;
using System.Diagnostics;

namespace BoaBeePCL
{
	public partial class DBLocalDataStore
	{
		private static DBLocalDataStore _instance;
		private static object _lockCreate = new object();
		private static string _pathName;
		private DBLocalDataStore()
		{

		}
		public static DBLocalDataStore GetInstance()
		{
			lock (_lockCreate)
			{
				if (_instance == null)
				{
					_instance = new DBLocalDataStore();
				}
				return _instance;
			}
		}
		public void SetPath(string pathName)
		{
			_pathName = pathName;
		}

		public string GetPath()
		{
			return _pathName;
		}

		private SQLiteConnection GetConnection()
		{
            SQLiteConnection connection = new SQLiteConnection(_pathName);
            connection.BusyTimeout = new TimeSpan(0, 0, 3);
            return connection;
		}

        public List<DBSyncRequest> getSyncRequests()
        {
            try
            {
                List<DBSyncRequest> sync = new List<DBSyncRequest>();
                SQLiteConnection connection = GetConnection();
                var table = connection.Table<DBSyncRequest>();
                foreach (var pp in table)
                {
                    sync.Add(pp);
                }
                return sync;
            }
            catch
            {
                return new List<DBSyncRequest>();
            }
        }
       

        public void updateSyncReqest(DBSyncRequest answer)
        {
            SQLiteConnection connection = GetConnection();
            connection.CreateTable<DBSyncRequest>();
            connection.Update(answer);
        }
        public void addDBVersion(DBVersion item)
        {
            SQLiteConnection connection = GetConnection();
            connection.CreateTable<DBVersion>();
            connection.Insert(item);
        }
        public List<DBVersion> getVersion()
        {
            try
            {
                SQLiteConnection connection = GetConnection();
                var table = connection.Table<DBVersion>();

                return table.ToList();
            }
            catch
            {
                return null;
            }
        }
        public void SetImportDataBase(DBImportDataBase ImportDataBase)
        {
            SQLiteConnection connection = GetConnection();
            connection.DropTable<DBImportDataBase>();
            connection.CreateTable<DBImportDataBase>();
            connection.Insert(ImportDataBase);
        }

        public DBImportDataBase GetImportDataBase()
        {
            SQLiteConnection connection = GetConnection();
            connection.CreateTable<DBImportDataBase>();
            var table = connection.Table<DBImportDataBase>();
            var value = table.FirstOrDefault();
            return value;
        }
        public void SetCountHomeScreen(DBHomeScreenCounts HomeScreenCounts)
        {
            SQLiteConnection connection = GetConnection();
            connection.DropTable<DBHomeScreenCounts>();
            connection.CreateTable<DBHomeScreenCounts>();
            connection.Insert(HomeScreenCounts);
        }

        public DBHomeScreenCounts GetCountHomeScreen()
        {
            DBHomeScreenCounts value = null;
            SQLiteConnection connection = GetConnection();
            connection.CreateTable<DBHomeScreenCounts>();
            var table = connection.Table<DBHomeScreenCounts>();
            value = table.FirstOrDefault();
            return value;
        }

        public void addSyncRequest(DBSyncRequest item)
        {
            SQLiteConnection connection = GetConnection();
            connection.CreateTable<DBSyncRequest>();
            connection.Insert(item);
        }

        public void clearSyncRequests()
        {
            SQLiteConnection connection = GetConnection();
            connection.DropTable<DBSyncRequest>();
        }

        public void deleteSyncRequest(DBSyncRequest request)
        {
            SQLiteConnection connection = GetConnection();
            connection.CreateTable<DBSyncRequest>();
            connection.Delete(request);
        }

        public void resetAnswers()
        {
            SQLiteConnection connection = GetConnection();
            connection.DropTable<DBAnswer>();
            connection.CreateTable<DBAnswer>();

            var lq = DBLocalDataStore.GetInstance().GetLocalQuestions(DBLocalDataStore.GetInstance().GetSelectedQuestionPosition());
            foreach (var question in lq)
            {
                DBAnswer answer = new DBAnswer();
                answer.question_name = question.name;
                answer.question = question.question;
                if (question.options.Length != 0)
                {
                    answer.answer = "select a value";
                }
                else
                {
                    answer.answer = "";
                }
                connection.Insert(answer);
            }
        }

        public List<DBAnswer> GetDefaultAnswers()
        {
            List<DBAnswer> answers = new List<DBAnswer>();
            var lq = DBLocalDataStore.GetInstance().GetLocalQuestions(DBLocalDataStore.GetInstance().GetSelectedQuestionPosition());
            foreach (var question in lq)
            {
                DBAnswer answer = new DBAnswer();
                answer.question_name = question.name;
                answer.question = question.question;
                if (question.options.Length != 0)
                {
                    answer.answer = "select a value";
                }
                else
                {
                    answer.answer = "";
                }
                answers.Add(answer);
            }

            return answers;
        }

        public void updateAnswer(DBAnswer answer)
        {
            SQLiteConnection connection = GetConnection();
            connection.CreateTable<DBAnswer>();
            connection.Update(answer);
        }

        public List<DBAnswer> getAnswers()
        {
            try //if updated from an earlier version, table will not exist
            {
                SQLiteConnection connection = GetConnection();
                var table = connection.Table<DBAnswer>();

                return table.ToList();
            }
            catch
            {
                return null;
            }
        }
        public void cleardefoultfiles()
        {
            var listdefoult = GetAllLocalFiles().Where(s => s.isDefault).ToList();
            if (listdefoult.Count != 0)
            {
                for (int i = 0; i < listdefoult.Count; i++)
                {
                    listdefoult[i].isDefault = false;
                    UpdateLocalFile(listdefoult[i]);
                }

            }
        
        }

		public void SetQuestionBackgroundColor(DBColor color)
		{
			SQLiteConnection connection = GetConnection();
			connection.DropTable<DBQuestionBackgroundColor>();
			connection.CreateTable<DBQuestionBackgroundColor>();
			DBQuestionBackgroundColor fontColor = new DBQuestionBackgroundColor(color);
			connection.Insert(fontColor);
		}

		public DBColor GetQuestionBackgroundColor()
		{
			DBColor value = null;
			SQLiteConnection connection = GetConnection();
			connection.CreateTable<DBQuestionBackgroundColor>();
			var table = connection.Table<DBQuestionBackgroundColor>();
			value = table.FirstOrDefault();

			if (value == null)
			{
				value = new DBColor(0xFF, 0xFF, 0xFF);
			}

			return value;
		}

		public void SetQuestionFontColor(DBColor color)
		{
			SQLiteConnection connection = GetConnection();
			connection.DropTable<DBQuestionFontColor>();
			connection.CreateTable<DBQuestionFontColor>();
			DBQuestionFontColor fontColor = new DBQuestionFontColor(color);
			connection.Insert(fontColor);
		}

		public DBColor GetQuestionFontColor()
		{
			DBColor value = null;
			SQLiteConnection connection = GetConnection();
			connection.CreateTable<DBQuestionFontColor>();
			var table = connection.Table<DBQuestionFontColor>();
			value = table.FirstOrDefault();

			if (value == null)
			{
				value = new DBColor(0, 0, 0);
			}

			return value;
		}

		public void SetAnswerFontColor(DBColor color)
		{
			SQLiteConnection connection = GetConnection();
			connection.DropTable<DBAnswerFontColor>();
			connection.CreateTable<DBAnswerFontColor>();
			DBAnswerFontColor fontColor = new DBAnswerFontColor(color);
			connection.Insert(fontColor);
		}

		public DBColor GetAnswerFontColor()
		{
			DBColor value = null;
			SQLiteConnection connection = GetConnection();
			connection.CreateTable<DBAnswerFontColor>();
			var table = connection.Table<DBAnswerFontColor>();
			value = table.FirstOrDefault();

			if (value == null)
			{
				value = new DBColor(0xED, 0xCD, 0x00);
			}

			return value;
		}

		public void SetAnswerBackgroundColor(DBColor color)
		{
			SQLiteConnection connection = GetConnection();
			connection.DropTable<DBAnswerBackgroundColor>();
			connection.CreateTable<DBAnswerBackgroundColor>();
			DBAnswerBackgroundColor fontColor = new DBAnswerBackgroundColor(color);
			connection.Insert(fontColor);
		}

		public DBColor GetAnswerBackgroundColor()
		{
			DBColor value = null;
			SQLiteConnection connection = GetConnection();
			connection.CreateTable<DBAnswerBackgroundColor>();
			var table = connection.Table<DBAnswerBackgroundColor>();
			value = table.FirstOrDefault();

			if (value == null)
			{
				value = new DBColor(0xF1, 0xF1, 0xF1);
			}

			return value;
		}

		public DBQuestionFontSize GetQuestionFontSize()
		{
			DBQuestionFontSize size = null;
			SQLiteConnection connection = GetConnection();
			connection.CreateTable<DBQuestionFontSize>();
			var table = connection.Table<DBQuestionFontSize>();
			size = table.FirstOrDefault();

			if (size == null)
			{
				size = new DBQuestionFontSize((int)FontSize.Medium);
			}

			return size;
		}

		public void SetQuestionFontSize(DBQuestionFontSize size)
		{
			SQLiteConnection connection = GetConnection();
			connection.DropTable<DBQuestionFontSize>();
			connection.CreateTable<DBQuestionFontSize>();
			connection.Insert(size);
		}

		public DBAnswerFontSize GetAnswerFontSize()
		{
			DBAnswerFontSize size = null;
			SQLiteConnection connection = GetConnection();
			connection.CreateTable<DBAnswerFontSize>();
			var table = connection.Table<DBAnswerFontSize>();
			size = table.FirstOrDefault();

			if (size == null)
			{
				size = new DBAnswerFontSize((int)FontSize.Medium);
			}

			return size;
		}

		public void SetAnswerFontSize(DBAnswerFontSize size)
		{
			SQLiteConnection connection = GetConnection();
			connection.DropTable<DBAnswerFontSize>();
			connection.CreateTable<DBAnswerFontSize>();
			connection.Insert(size);
		}

		public List<DBBasicAuthority> GetProfiles()
		{
			List<DBBasicAuthority> profile = new List<DBBasicAuthority>();
			try
			{
				SQLiteConnection connection = GetConnection();
				var table = connection.Table<DBBasicAuthority>();
				//var bas = table.First();

				foreach (var pp in table)
				{
					DBBasicAuthority ba = new DBBasicAuthority ();
					ba.authorityType = pp.authorityType;
					ba.shortName = pp.shortName;
					ba.fullName = pp.fullName;
					ba.displayName = pp.displayName;

					profile.Add(ba);
				}
			}
			catch //(Exception ex)
			{
//				Console.WriteLine (ex.Message);
				// not sure what to do about logging on the phone
			}
			return profile;
		}

		public void ClearBasicAuthority()
		{
			SQLiteConnection connection = GetConnection();
			connection.CreateTable<DBBasicAuthority>();
			//connection.Query<DBBasicAuthority>("delete from DeviceInfo");
			connection.DropTable<DBBasicAuthority> ();
		}

		public string GetUserMail()
		{
			string email = null;
			SQLiteConnection connection = GetConnection();
			var table = connection.Table<DBUserLoginRequest>();
			foreach (var t in table)
				email = t.username;

			return email;
		}

		public void AddBasicAuthority(DBBasicAuthority item)
		{
			SQLiteConnection connection = GetConnection();
			connection.CreateTable<DBBasicAuthority>();
			connection.Insert(item);
		}

		public void AddSelectProfile(string displayName)
		{
			ClearSelectProfile ();
			SQLiteConnection connection = GetConnection();
			var sp = new DBSelectProfile ();
			var ba = connection.Table<DBBasicAuthority>().Where(v => v.displayName.ToUpper().Equals(displayName.ToUpper()));
			foreach (var b in ba) {
				sp.authorityType = b.authorityType;
				sp.displayName = b.displayName;
				sp.fullName = b.fullName;
				sp.shortName = b.shortName;
			}
			connection.CreateTable<DBSelectProfile>();
			connection.Insert(sp);
		}

		public void ClearSelectProfile()
		{
			SQLiteConnection connection = GetConnection();
			connection.CreateTable<DBSelectProfile>();
			//connection.Query<DBBasicAuthority>("delete from DeviceInfo");
			connection.DropTable<DBSelectProfile> ();
		}

		public DBSelectProfile GetSelectProfile()
		{
			DBSelectProfile selectprofile = null;
			try
			{
				SQLiteConnection connection = GetConnection();
				connection.CreateTable<DBSelectProfile>();
				var table = connection.Table<DBSelectProfile>();

				selectprofile = table.FirstOrDefault();
				/*foreach (var pp in table)
				{
					selectprofile = new DBSelectProfile();
					selectprofile.authorityType = pp.authorityType;
					selectprofile.displayName = pp.displayName;
					selectprofile.fullName = pp.fullName;
					selectprofile.shortName = pp.shortName;
				}*/
			}
			catch //(Exception ex)
			{
				
//				Console.WriteLine (ex.Message);
				// not sure what to do about logging on the phone
			}
			return selectprofile;
		}

		public DBSelectProfile GetSelectProfile(Action<Exception> exceptionCallback, Action successCallback)
		{
			DBSelectProfile selectprofile = null;
			try
			{
				SQLiteConnection connection = GetConnection();
				connection.CreateTable<DBSelectProfile>();
				var table = connection.Table<DBSelectProfile>();

				selectprofile = table.FirstOrDefault();
				/*foreach (var pp in table)
				{
					selectprofile = new DBSelectProfile();
					selectprofile.authorityType = pp.authorityType;
					selectprofile.displayName = pp.displayName;
					selectprofile.fullName = pp.fullName;
					selectprofile.shortName = pp.shortName;
				}*/
			}
			catch (Exception ex)
			{
				if (exceptionCallback != null)
				{
					exceptionCallback(ex);
				}
				return null;
//				Console.WriteLine (ex.Message);
				// not sure what to do about logging on the phone
			}
			if (successCallback != null)
			{
				successCallback();
			}
			return selectprofile;
		}

        public void SetAppInfo(DBAppInfo appInfo)
		{
			SQLiteConnection connection = GetConnection();
			connection.DropTable<DBAppInfo>();
			connection.CreateTable<DBAppInfo>();
			connection.Insert(appInfo);
		}

		public void ClearAppInfo()
		{
			SQLiteConnection connection = GetConnection();
			connection.DropTable<DBAppInfo>();
		}

		public DBAppInfo GetAppInfo()
		{
			DBAppInfo value = null;
			SQLiteConnection connection = GetConnection();
			connection.CreateTable<DBAppInfo>();
			var table = connection.Table<DBAppInfo>();
			value = table.FirstOrDefault();
			return value;
		}

        public void SetAppSettings(DBAppSettings appSettings)
        {
            SQLiteConnection connection = GetConnection();
            connection.DropTable<DBAppSettings>();
            connection.CreateTable<DBAppSettings>();
            connection.Insert(appSettings);
        }

        public void ClearAppSettings()
        {
            SQLiteConnection connection = GetConnection();
            connection.DropTable<DBAppSettings>();
        }

        public DBAppSettings GetAppSettings()
        {
            SQLiteConnection connection = GetConnection();
            connection.CreateTable<DBAppSettings>();
            var table = connection.Table<DBAppSettings>();
            var value = table.FirstOrDefault();
            return value;
        }

        public void SetKioskSettings(DBKioskSettings kioskSettings)
		{
			SQLiteConnection connection = GetConnection();
			connection.DropTable<DBKioskSettings>();
			connection.CreateTable<DBKioskSettings>();
			connection.Insert(kioskSettings);
		}

		public void ClearKioskSettings()
		{
			SQLiteConnection connection = GetConnection();
			connection.DropTable<DBKioskSettings>();
		}

		public DBKioskSettings GetKioskSettings()
		{
			SQLiteConnection connection = GetConnection();
			connection.CreateTable<DBKioskSettings>();
			var table = connection.Table<DBKioskSettings>();
			var value = table.FirstOrDefault();
			return value;
		}

		public DBUserAutologin GetShouldAutologin()
		{
			DBUserAutologin value = null;
			SQLiteConnection connection = GetConnection();
//            Debug.WriteLine(connection.IsInTransaction.ToString());
			connection.CreateTable<DBUserAutologin>();
			var table = connection.Table<DBUserAutologin> ();
			value = table.FirstOrDefault();
			if (value == null)
			{
				value = new DBUserAutologin();
				value.shouldAutologin = false;
			}
			return value;
		}

		public void SetShouldAutologin(DBUserAutologin value)
		{
			SQLiteConnection connection = GetConnection();
			connection.DropTable<DBUserAutologin>();
			connection.CreateTable<DBUserAutologin>();
			connection.Insert(value);
		}

		public void AddUserInfo(DBUserLoginRequest infoUser)
		{
			ClearUserInfo ();
			SQLiteConnection connection = GetConnection();
			connection.CreateTable<DBUserLoginRequest>();
			connection.Insert(infoUser);
		}

		public void ClearUserInfo()
		{
			SQLiteConnection connection = GetConnection();
			connection.CreateTable<DBUserLoginRequest>();
			//connection.Query<DBBasicAuthority>("delete from DeviceInfo");
			connection.DropTable<DBUserLoginRequest> ();
		}

		public DBUserLoginRequest GetLocalUserInfo()
		{
			DBUserLoginRequest user = null;
			try
			{
				SQLiteConnection connection = GetConnection();
				var table = connection.Table<DBUserLoginRequest>();
                //var bas = table.First();

                user = table.FirstOrDefault();

				//foreach (var pp in table)
				//{
				//	user = new DBUserLoginRequest();
				//	user.username = pp.username;
				//	user.password = pp.password;
				//}
			}
			catch //(Exception ex)
			{
//				Console.WriteLine (ex.Message);
				// not sure what to do about logging on the phone
			}
			return user;
		}

		public void AddExcengeInfo(ExchengeInfo item)
		{
			SQLiteConnection connection = GetConnection();
			//connection.DropTable<ExchengeInfo> ();
			connection.CreateTable<ExchengeInfo>();
			ExchengeInfo pmt = new ExchengeInfo();
			pmt.success = item.success;
			pmt.error = item.error;
			pmt.profiles = item.profiles;

			connection.Insert(pmt);
		}

		public void ClearAllContacts()
		{
			SQLiteConnection connection = GetConnection();
			connection.DropTable<DBlocalContact> ();
		}

        public void ClearAllContactsToServer()
        {
            SQLiteConnection connection = GetConnection();
            connection.DropTable<DBContactToServer>();
        }
        public void RemoveContactToServer(DBContactToServer item)
        {
            SQLiteConnection connection = GetConnection();
            connection.CreateTable<DBContactToServer>();
            connection.Delete(item);
        }

		public void AddLocalContact(DBlocalContact item)
		{
			SQLiteConnection connection = GetConnection();
			connection.CreateTable<DBlocalContact>();
			//if (string.IsNullOrEmpty (item.company) &&
			//	string.IsNullOrEmpty (item.email) &&
			//	string.IsNullOrEmpty (item.firstname) &&
   //             string.IsNullOrEmpty (item.lastname) &&
			//	string.IsNullOrEmpty (item.barcode)&&
			//	string.IsNullOrEmpty (item.street) &&
			//	string.IsNullOrEmpty (item.city) &&
			//	string.IsNullOrEmpty (item.zip) &&
			//    string.IsNullOrEmpty (item.country)) 
			//	return;
//			Console.WriteLine ("Email = " + item.email + ", barcode = " + item.barcode);
			connection.Insert(item);
		}
        public void AddContactToServer(DBContactToServer item)
        {
            SQLiteConnection connection = GetConnection();
            connection.CreateTable<DBContactToServer>();
            connection.Insert(item);
        }

        public DBlocalContact GetLocalContactsByUID(string uid)
		{
			try
			{
				SQLiteConnection connection = GetConnection();
                var table = connection.Table<DBlocalContact>().Where(sim => sim.uid.ToLower().Equals(uid.ToLower()));
                var contact = table.FirstOrDefault();
                return contact;
			}
			catch// (Exception ex)
			{
//				Console.WriteLine (ex.Message);
				// not sure what to do about logging on the phone
			}
			return null;
		}

		public DBlocalContact GetLocalContactsById(int _id)
		{
			try
			{
				SQLiteConnection connection = GetConnection();
				var table = connection.Table<DBlocalContact>().Where(sim => sim.Id == _id);
				//var bas = table.First();

				foreach (var pp in table)
				{
//					return new DBlocalContact{
//						uuid = pp.uuid,
//						firstName = pp.firstName,
//						lastName = pp.lastName,
//						email = pp.email,
//						company = pp.company,
//						phone = pp.phone,
//						source = pp.source
//					};
					return pp;
				}
			}
			catch //(Exception ex)
			{
//				Console.WriteLine (ex.Message);
				// not sure what to do about logging on the phone
			}
			return null;
		}
        public void UpdateContactToServer(DBContactToServer item)
        {
            SQLiteConnection connection = GetConnection();
            connection.CreateTable<DBContactToServer>();
            connection.Update(item);
        }
		public void UpdateLocalContact(DBlocalContact item)
		{
			SQLiteConnection connection = GetConnection();
			connection.CreateTable<DBlocalContact>();
			connection.Update(item);
		}

		public void DeleteLocalContact(DBlocalContact item)
		{
			SQLiteConnection connection = GetConnection();
			connection.CreateTable<DBlocalContact>();
			connection.Delete<DBlocalContact>(item.Id);
		}

		public List<DBlocalContact> GetLocalContacts()
		{
			List<DBlocalContact> profile = new List<DBlocalContact>();
			try
			{
				SQLiteConnection connection = GetConnection();
				var table = connection.Table<DBlocalContact>();
				//var bas = table.First();

				foreach (var pp in table)
				{
					profile.Add(pp);
				}
			}
			catch //(Exception ex)
			{
//				Console.WriteLine (ex.Message);
				// not sure what to do about logging on the phone
			}
			return profile;
		}

        public List<DBContactToServer> GetContactsToServer()
        {
            List<DBContactToServer> profile = new List<DBContactToServer>();
            try
            {
                SQLiteConnection connection = GetConnection();
                var table = connection.Table<DBContactToServer>();
                //var bas = table.First();

                foreach (var pp in table)
                {
                    profile.Add(pp);
                }
            }
            catch //(Exception ex)
            {
                //              Console.WriteLine (ex.Message);
                // not sure what to do about logging on the phone
            }
            return profile;
        }

        public DBContactToServer GetContactsToServerByUID(string uid)
        {
            DBContactToServer profile = null;
            try
            {
                SQLiteConnection connection = GetConnection();
                var table = connection.Table<DBContactToServer>().Where(sim => sim.uid.ToLower().Equals(uid.ToLower()));
                profile = table.FirstOrDefault();
            }
            catch //(Exception ex)
            {
                //              Console.WriteLine (ex.Message);
                // not sure what to do about logging on the phone
            }
            return profile;
        }

		public List<DBlocalContact> GetLocalContactsNeedSetServer()
		{
			List<DBlocalContact> profile = new List<DBlocalContact>();
//			try
//			{
//				SQLiteConnection connection = GetConnection();
//				var table = connection.Table<DBlocalContact>();
//				//var bas = table.First();

//				foreach (var pp in table)
//				{
//					if (pp.source == "manual" || pp.source == "scannedlocal")
//						profile.Add(pp);
//				}
//			}
//			catch// (Exception ex)
//			{
////				Console.WriteLine (ex.Message);
//				// not sure what to do about logging on the phone
//			}
			return profile;
		}

		public void AddLocalFormDefinitions(FormDefinition[] items){
			SQLiteConnection connection = GetConnection ();
			connection.DropTable<DBFormDefinition> ();
			connection.DropTable<DBQuestion> ();
			connection.CreateTable<DBFormDefinition> ();
			foreach (var item in items){
				var _form = new DBFormDefinition {
					objectId = item.objectId,
					objectName = item.objectName,
					objectState = item.objectState,
					uuid = item.uuid,
					hidden = item.hidden
				};

				connection.Insert (_form);
//				Console.WriteLine ("Form added = " + _form.objectName);
				AddLocalQuestion (item);
			}
		}

		public void AddLocalFormDefinitions(FormDefinition[] items, string default_uuid = null){
			SQLiteConnection connection = GetConnection ();
			connection.DropTable<DBFormDefinition> ();
			connection.DropTable<DBQuestion> ();
			connection.CreateTable<DBFormDefinition> ();
			foreach (var item in items){
				var _form = new DBFormDefinition {
					objectId = item.objectId,
					objectName = item.objectName,
					objectState = item.objectState,
					uuid = item.uuid,
					hidden = item.hidden,
					isDefault = false
				};

				if (default_uuid.ToLower().Equals(item.uuid.ToLower()))
				{
					_form.isDefault = true;
				}

				connection.Insert (_form);
				//				Console.WriteLine ("Form added = " + _form.objectName);
				AddLocalQuestion (item);
			}
		}

		public void SetSelectedFormDefinitions(string _uuid_group){
			SQLiteConnection connection = GetConnection();
			connection.DropTable<SelectedQuestionPosition> ();
			connection.CreateTable<SelectedQuestionPosition> ();
			connection.Insert (new SelectedQuestionPosition{uuid_group = _uuid_group});
			//SaveAndLoad.GetInstance ().DeleteFile ();
		}

		public string GetSelectedQuestionPosition()
		{
			string return_i = "";
			SQLiteConnection connection = GetConnection();
			connection.CreateTable<SelectedQuestionPosition> ();
			var table = connection.Table<SelectedQuestionPosition>();
			foreach (var i in table) {
				return_i = i.uuid_group;
			}
			return return_i;
		}

		public List<DBFormDefinition> GetLocalFormDefinitions()
		{
			List<DBFormDefinition> forms = new List<DBFormDefinition>();
			try
			{
				SQLiteConnection connection = GetConnection();
				var table = connection.Table<DBFormDefinition>();

				foreach (var pp in table)
				{
					forms.Add(pp);
				}
			}
			catch
			{
				
			}
			return forms;
		}

		public DBFormDefinition GetLocalDefaultFormDefinition()
		{
			try
			{
				SQLiteConnection connection = GetConnection();
				var table = connection.Table<DBFormDefinition>();

				foreach (var pp in table)
				{
					if (pp.isDefault)
					{
						return pp;
					}
				}
			}
			catch
			{
				
			}
			return null;
		}

		public void AddLocalQuestion(FormDefinition item)
		{
			SQLiteConnection connection = GetConnection ();
			//connection.DropTable<DBQuestion> ();
			connection.CreateTable<DBQuestion> ();

			foreach (var tt in item.questions) {
				//for (var i = 0; i < tt.options.Count; i++) {
				var dbq = new DBQuestion ();
				dbq.question = tt.question;
				dbq.name = tt.name;
				dbq.required = tt.required;
				dbq.type = tt.type;
				dbq.maxLength = tt.maxLength;
				dbq.uuidgroup = item.uuid;

				if (tt.options != null) {
					string str = "";
					foreach (var st in tt.options) {
						str = str + "_,_" + st;


					}
					dbq.options = str;
				}
				connection.Insert (dbq);
//				Console.WriteLine ("Question added = " + dbq.name);
			}
		}

		public List<DBQuestion> GetLocalQuestions(string _uuid_group)
		{
			List<DBQuestion> profile = new List<DBQuestion>();
			try
			{
				SQLiteConnection connection = GetConnection();
				var table = connection.Table<DBQuestion>().Where(ob => ob.uuidgroup == _uuid_group);
				//var bas = table.First();

				foreach (var pp in table)
				{
					profile.Add(pp);
				}
			}
			catch //(Exception ex)
			{
//				Console.WriteLine (ex.Message);
				// not sure what to do about logging on the phone
			}
			return profile;
		}

		public void AddLocalAnswers(List<Answer> items)
		{
			SQLiteConnection connection = GetConnection();
			connection.DropTable<Answer>();
			connection.CreateTable<Answer>();
			foreach(var it in items){
				connection.Insert(it);
			}


		}

		public List<Answer> GetLocalAnswers()
		{
			List<Answer> profile = new List<Answer>();
			try
			{
				SQLiteConnection connection = GetConnection();
				var table = connection.Table<Answer>();
				//var bas = table.First();

				foreach (var pp in table)
				{
					profile.Add(pp);
				}
			}
			catch //(Exception ex)
			{
//				Console.WriteLine (ex.Message);
				// not sure what to do about logging on the phone
			}
			return profile;
		}

		public void AddLocalFolders(FolderTO[] items)
		{
			SQLiteConnection connection = GetConnection();


			//ClearAllFiles ();
			//ClearAllFolders ();

			//connection.DropTable<DBfolderTO>();
			connection.CreateTable<DBfileTO>();
			foreach(var it in items)
			{
				if (items.Length != 1)
				{
					var local_file = GetOnlyLocalFileByUUID(it.uuid);
					var f = new DBfileTO();
					f.fileType = it.fileType;
					f.name = it.name;
					f.uuid = it.uuid;
					f.folderUuid = "_empty_";
					if (local_file == null)
						connection.Insert(f);

					AddLocalFolders(it.folders, it.uuid);
					AddLocalFiles(it.files, it.uuid);
					AddLocalUrls(it.urls, it.uuid);
				}
				else
				{
					AddLocalFolders(it.folders, "_empty_");
					AddLocalFiles(it.files, "_empty_");
					AddLocalUrls(it.urls, "_empty_");
				}
			}


		}

		public void AddLocalFolders(FolderTO[] items, string folderUuid)
		{
			SQLiteConnection connection = GetConnection();
			//ClearAllFiles ();
			//ClearAllFolders ();
			//connection.DropTable<DBfolderTO>();
			connection.CreateTable<DBfileTO>();
			foreach(var it in items)
			{
				var local_file = GetOnlyLocalFileByUUID (it.uuid);

				var f = new DBfileTO ();
				f.fileType = it.fileType;
				f.name = it.name;
				f.uuid = it.uuid;
				f.folderUuid = folderUuid;
				if (local_file == null)
					connection.Insert(f);
				
//				Console.WriteLine ("field add = " + f.name);
				AddLocalFiles (it.files, it.uuid);
				AddLocalFolders (it.folders, it.uuid);
				AddLocalUrls (it.urls, it.uuid);
			}
		}

		public List<DBfileTO> GetLocalFolders()
		{
			List<DBfileTO> profile = new List<DBfileTO>();
			try
			{
				SQLiteConnection connection = GetConnection();
				var table = connection.Table<DBfileTO>();
				//var bas = table.First();

				foreach (var pp in table)
				{
					var f = new DBfileTO();
					f.fileType = pp.fileType;
					f.name = pp.name;
					f.uuid = pp.uuid;
					profile.Add(f);
				}
			}
			catch// (Exception ex)
			{
//				Console.WriteLine (ex.Message);
				// not sure what to do about logging on the phone
			}
			return profile;
		}

		public void AddLocalFiles(FileTO[] items, string folderUuid)
		{
			SQLiteConnection connection = GetConnection();
			//connection.DropTable<DBfileTO>();
			connection.CreateTable<DBfileTO>();
			foreach(var it in items){
				var file_here_uuid = GetOnlyLocalFileByUUID(it.uuid);
//				var file_here = GetOnlyLocalFileByMD5 (it.md5);
				var dbf = new DBfileTO ();
				dbf.downloadUrl = it.downloadUrl;
				dbf.fileType = it.fileType;
				dbf.folderUuid = folderUuid;
				dbf.md5 = it.md5;
				dbf.mimeType = it.mimeType;
				dbf.name = it.name;
				dbf.uuid = it.uuid;

				if (file_here_uuid == null) 
				{
					connection.Insert (dbf);

//					Console.WriteLine ("file add = " + dbf.name + " stat = ");
				}

			}


		}

		public void UpdateLocalFile(DBfileTO item)
		{
			try{
				SQLiteConnection connection = GetConnection();

				connection.CreateTable<DBfileTO>();

				connection.Update(item);
			}
			catch//(Exception ex)
			{
//				Console.WriteLine ("EXEPTION UPDATE FILE: " + ex.Message);
			}
		}
		public void UpdateLocalFileAppType(DBAppInfo item)
		{
			try{
				SQLiteConnection connection = GetConnection();

				connection.CreateTable<DBAppInfo>();

				connection.Update(item);
			}
			catch//(Exception ex)
			{
				//				Console.WriteLine ("EXEPTION UPDATE FILE: " + ex.Message);
			}
		}

		public List<DBfileTO> GetAllLocalFiles()
		{
			List<DBfileTO> profile = new List<DBfileTO>();
			SQLiteConnection connection = GetConnection();
			connection.CreateTable<DBfileTO>();
			var table = connection.Table<DBfileTO>();
			//var bas = table.First();

			foreach (var pp in table)
			{
				profile.Add(pp);
			}
			return profile;
		}

		public List<DBfileTO> GetOnlyLocalFiles(bool includeWeblocFiles = true)
		{
			List<DBfileTO> profile = new List<DBfileTO>();
			SQLiteConnection connection = GetConnection();
			connection.CreateTable<DBfileTO>();
			TableQuery<DBfileTO> table = null;
			if (includeWeblocFiles)
			{
				table = connection.Table<DBfileTO>().Where(s => s.fileType != "folder" && s.fileType != "url");
			}
			else
			{
				table = connection.Table<DBfileTO>().Where(s => s.fileType != "folder" && s.fileType != "url" && s.fileType != "webloc");
			}
			//var bas = table.First();

			foreach (var pp in table)
			{
				profile.Add(pp);
			}
			return profile;
		}

		public DBfileTO GetOnlyLocalFileByMD5(string _md5)
		{
			DBfileTO profile = null;
			SQLiteConnection connection = GetConnection();
			connection.CreateTable<DBfileTO>();
			var table = connection.Table<DBfileTO>().Where(s => s.md5 == _md5);
			//var bas = table.First();

			foreach (var pp in table)
			{
				profile= pp;
			}
			return profile;
		}

		public List<DBfileTO> GetOnlyLocalFilesByMD5(string _md5)
		{
			SQLiteConnection connection = GetConnection();
			connection.CreateTable<DBfileTO>();
			var table = connection.Table<DBfileTO>().Where(s => s.md5 == _md5);
			List<DBfileTO> files = new List<DBfileTO>();

			foreach (var pp in table)
			{
				files.Add(pp);
			}
			return files;
		}

		public DBfileTO GetOnlyLocalFileByUUID(string uuid)
		{
			DBfileTO profile = null;
			SQLiteConnection connection = GetConnection();
			connection.CreateTable<DBfileTO>();
			var table = connection.Table<DBfileTO>().Where(s => s.uuid == uuid);
			//var bas = table.First();

			foreach (var pp in table)
			{
				profile= pp;
			}
			return profile;
		}

		public List<DBfileTO> GetOnlyLocalFilesWithOutlocalPath()
		{
			List<DBfileTO> profile = new List<DBfileTO>();
			SQLiteConnection connection = GetConnection();
			connection.CreateTable<DBfileTO>();
			var table = connection.Table<DBfileTO> ().Where(s => s.fileType != "folder" && s.fileType != "url");
			try{
				foreach (var pp in table)
				{
					if (pp.localpath == null || pp.localpath == "")
						profile.Add(pp);
				}
			}
			catch//(Exception ex) 
			{
//				Console.WriteLine (ex.Message);
			}
			return profile;
		}

		public List<FileTO> GetLocalFiles(string folderUuid)
		{
			List<FileTO> profile = new List<FileTO>();
			try
			{
				SQLiteConnection connection = GetConnection();
				var table = connection.Table<DBfileTO>().Where(v => v.folderUuid.Equals(folderUuid));
				//var bas = table.First();

				foreach (var pp in table)
				{
					var ft = new FileTO();
					ft.downloadUrl = pp.downloadUrl;
					ft.fileType = pp.fileType;
					ft.md5 = pp.md5;
					ft.mimeType = pp.mimeType;
					ft.name = pp.name;
					ft.uuid = pp.uuid;

					profile.Add(ft);
				}
			}
			catch //(Exception ex)
			{
//				Console.WriteLine (ex.Message);
				// not sure what to do about logging on the phone
			}
			return profile;
		}

		public void ClearAllFolders()
		{
			SQLiteConnection connection = GetConnection();
			//connection.CreateTable<DBlocalContact>();
			connection.DropTable<DBfolderTO> ();
		}

		public void ClearAllFiles()
		{
			SQLiteConnection connection = GetConnection();
			//connection.CreateTable<DBlocalContact>();
			connection.DropTable<DBfileTO> ();
		}

		public void AddLocalUrls(UrlTO[] items, string folderUuid)
		{
			SQLiteConnection connection = GetConnection();
			//connection.DropTable<DBfileTO>();
			connection.CreateTable<DBfileTO>();
			foreach(var it in items){
				var local_file = GetOnlyLocalFileByUUID (it.uuid);
				var dbf = new DBfileTO ();
				dbf.localpath = it.link;
				dbf.fileType = it.fileType;
				dbf.folderUuid = folderUuid;
				//dbf.md5 = it.md5;
				//dbf.mimeType = it.mimeType;
				dbf.name = it.name;
				dbf.uuid = it.uuid;
				if (local_file == null)
					connection.Insert(dbf);

//				Console.WriteLine ("LINK add = " + dbf.name);

			}


		}

		public List<DBUrlTO> GetLocalUrls(string folderUuid)
		{
			List<DBUrlTO> profile = new List<DBUrlTO>();
			try
			{
				SQLiteConnection connection = GetConnection();
				var table = connection.Table<DBUrlTO>().Where(v => v.folderUuid.Equals(folderUuid));
				//var bas = table.First();

				foreach (var pp in table)
				{
					profile.Add(pp);
				}
			}
			catch //(Exception ex)
			{
//				Console.WriteLine (ex.Message);
				// not sure what to do about logging on the phone
			}
			return profile;
		}

		public void AddShare(DBShareFileContact item)
		{
			SQLiteConnection connection = GetConnection();
			connection.CreateTable<DBShareFileContact>();
			connection.Insert(item);
		}

		public List<DBShareFileContact> GetAllShare()
		{
			var _list_share = new List<DBShareFileContact> ();
			try{
				SQLiteConnection connection = GetConnection();
				connection.CreateTable<DBShareFileContact>();
				var table = connection.Table<DBShareFileContact>();
				foreach (var t in table)
				{
					_list_share.Add(t);
				}
				
			}
			catch//(Exception ex) 
			{
//				Console.WriteLine (ex.Message);
			}
			return _list_share;
		}

		public List<DBfileTO> GetShareFilesById(int _id)
		{
			var _list_files = new List<DBfileTO> ();
			try{
				SQLiteConnection connection = GetConnection();
				connection.CreateTable<DBShareFileContact>();
				var table = connection.Table<DBShareFileContact>().Where(ob => ob.uuid_contact == _id.ToString());
				foreach (var t in table)
				{
					connection.CreateTable<DBfileTO>();
					var _table = connection.Table<DBfileTO>().Where(ob => ob.uuid == t.uuid_file);

					foreach (var _t in _table)
					{
						_list_files.Add(_t);
					}
				}

			}
			catch//Exception ex)
			{
//				Console.WriteLine (ex.Message);
			}
			return _list_files;
		}



		public void ClearAllShare()
		{
			SQLiteConnection connection = GetConnection();
			connection.CreateTable<DBShareFileContact>();
			connection.DropTable<DBShareFileContact> ();
		}

		public void ShowLocalFiles(bool flag)
		{
			SQLiteConnection connection = GetConnection();
			connection.DropTable<DBShowLocalFiles> ();
			connection.CreateTable<DBShowLocalFiles>();
			try{
				//var table = connection.Table<DBShowLocalFiles> ();
				//Console.WriteLine (table.Count());
				//if (table.Count() != 0)
					//connection.DropTable<DBShowLocalFiles> ();
			}
			catch//(Exception ex)
			{
//				Console.WriteLine (ex.Message);
			}
			var _show = new DBShowLocalFiles ();
			_show.show = flag;
			connection.Insert(_show);
		}

		public bool GetShowLocalFiles()
		{
			try{
				SQLiteConnection connection = GetConnection();
				connection.CreateTable<DBShowLocalFiles>();
				var table = connection.Table<DBShowLocalFiles> ();

				return table.First ().show;
			}
			catch//(Exception ex)
			{
//				Console.WriteLine (ex.Message);
			}

			return false;
		}

		public void SetFlagFullData (GetFullData item)
		{
			SQLiteConnection connection = GetConnection();
			connection.DropTable<GetFullData>();
			connection.CreateTable<GetFullData>();
			connection.Insert(item);
		}

		public GetFullData GetFlagFullData ()
		{
			GetFullData flag = null;
			SQLiteConnection connection = GetConnection();
			connection.CreateTable<GetFullData>();
			var table = connection.Table<GetFullData> ();
			flag = table.FirstOrDefault();
			return flag;
		}
	}
}
