using System;
using System.Collections.Generic;
using SQLite;

namespace BoaBeePCL
{
    public partial class DBLocalDataStore
    {
        public void ClearContactsPopupById(DBpopupContact _item)
        {
            try
            {
                SQLiteConnection connection = GetConnection();
                connection.CreateTable<DBpopupContact>();
                //connection.Query<DBpopupContact> ("DELETE FROM DBpopupContact WHERE _id = '?'", Id);
                connection.Delete(_item);
            }
            catch//(Exception ex)
            {
                //              Console.WriteLine (ex.Message);
            }
        }

        public void AddLocalContactPopup(DBpopupContact item)
        {
            SQLiteConnection connection = GetConnection();
            connection.CreateTable<DBpopupContact>();
            DBlocalContact lc = new DBlocalContact();
            lc.company = item.company;
            lc.email = item.email;
            lc.firstname = item.firstName;
            lc.lastname = item.lastName;
            lc.phone = item.phone;
            //lc.source = item.source;
            lc.city = item.city;
            lc.zip = item.zip;
            lc.street = item.street;
            lc.country = item.country;
            //lc.uuid = item.uuid;
            //AddLocalContact (lc);
            connection.Insert(item);
        }

        public List<DBpopupContact> GetLocalContactsPopup()
        {
            List<DBpopupContact> profile = new List<DBpopupContact>();
            try
            {
                SQLiteConnection connection = GetConnection();
                var table = connection.Table<DBpopupContact>();
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

              public int GetLocalContactsContainsPopup(DBpopupContact item)
              {
                  List<DBpopupContact> profile = new List<DBpopupContact>();
                  try
                  {
                      SQLiteConnection connection = GetConnection();
                      var table = connection.Table<DBpopupContact>();
                      //var bas = table.First();

                      foreach (var pp in table)
                      {
                          //profile.Add(pp);
                          if (!string.IsNullOrEmpty(item.email) && pp.email == item.email)
                          {
                              profile.Add(pp);
                          }
                          if (!string.IsNullOrEmpty(item.barcode) && pp.barcode == item.barcode){
                              profile.Add(pp);
                          }

                      }

                      //return profile.Contains(item);

                  }
                  catch //(Exception ex)
                  {
        //                Console.WriteLine (ex.Message);
                      // not sure what to do about logging on the phone
                  }


                  return profile.Count;
              }

              public int GetLocalContactsContains(DBpopupContact item)
              {
                  List<DBlocalContact> profile = new List<DBlocalContact>();
                  try
                  {
                      SQLiteConnection connection = GetConnection();
                      var table = connection.Table<DBlocalContact>();
                      //var bas = table.First();
                      table = table.Skip(item._id);
                      foreach (var pp in table)
                      {
                          //profile.Add(pp);
                          if (pp.email == item.email)
                          {

                              profile.Add(pp);
                          }

                      }

                      //return profile.Contains(item);

                  }
                  catch //(Exception ex)
                  {
        //                Console.WriteLine (ex.Message);
                      // not sure what to do about logging on the phone
                  }


                  return profile.Count;
              }

        public void UpdatePopupContact (DBpopupContact item)
        {
          SQLiteConnection connection = GetConnection ();
          connection.CreateTable<DBpopupContact> ();
          connection.Update (item);
        }

        public void AddContactsInFile(List<DBpopupContact> items, string _uuid)
        {
          foreach (var i in items)
          {
              var sh = new DBShareFileContact ();
              sh.uuid_file = _uuid;
              sh.uuid_contact = i.uuid;
              AddShare (sh);
          }
        }

        public void ClearAllContactsPopup()
        {
          SQLiteConnection connection = GetConnection();
          //connection.CreateTable<DBlocalContact>();
          connection.DropTable<DBpopupContact> ();
        }

              public DBlocalContact GetLocalContactsPopupById(int _uuid)
              {
                  try
                  {
                      SQLiteConnection connection = GetConnection();
                      var table = connection.Table<DBpopupContact>().Where(sim => sim.uuid == _uuid.ToString());
                      //var bas = table.First();

                      foreach (var pp in table)
                      {
                          return new DBlocalContact{
                              firstname = pp.firstName,
                              lastname = pp.lastName,
                              email = pp.email,
                              company = pp.company,
                              phone = pp.phone,
                              city = pp.city,
                              zip = pp.zip,
                              street = pp.street,
                              country = pp.country
                          };
                      }
                  }
                  catch //(Exception ex)
                  {
        //                Console.WriteLine (ex.Message);
                      // not sure what to do about logging on the phone
                  }
                  return null;
              }

        public void AddLocalFilesPopup(DBpopupfileTO item)
        {
          SQLiteConnection connection = GetConnection();
          connection.CreateTable<DBpopupfileTO>();
          connection.Insert(item);
        }

              public List<DBpopupfileTO> GetLocalFilesPopup()
              {
                  List<DBpopupfileTO> profile = new List<DBpopupfileTO>();
                  try
                  {
                      SQLiteConnection connection = GetConnection();
                      var table = connection.Table<DBpopupfileTO>();
                      //var bas = table.First();

                      foreach (var pp in table)
                      {
                          profile.Add(pp);
                      }
                  }
                  catch //(Exception ex)
                  {
        //                Console.WriteLine (ex.Message);
                      // not sure what to do about logging on the phone
                  }
                  return profile;
              }

              public int GetLocalFilesContainsPopup(DBpopupfileTO item)
              {
                  List<DBpopupfileTO> profile = new List<DBpopupfileTO>();
                  try
                  {
                      SQLiteConnection connection = GetConnection();
                      var table = connection.Table<DBpopupfileTO>();
                      //var bas = table.First();

                      foreach (var pp in table)
                      {
                          //profile.Add(pp);
                          if (pp.uuid == item.uuid)
                          {
                              profile.Add(pp);
                          }

                      }

                      //return profile.Contains(item);

                  }
                  catch //(Exception ex)
                  {
        //                Console.WriteLine (ex.Message);
                      // not sure what to do about logging on the phone
                  }


                  return profile.Count;
              }

        public void AddFilesInContact(List<DBpopupfileTO> items, string _uuid)
        {
          foreach (var i in items)
          {
              var sh = new DBShareFileContact ();
              sh.uuid_file = i.uuid;
              sh.uuid_contact = _uuid;
              AddShare (sh);
          }
        }

              public void ClearAllFilesPopup()
              {
                  SQLiteConnection connection = GetConnection();
                  //connection.CreateTable<DBlocalContact>();
                  connection.DropTable<DBpopupfileTO> ();
              }

              public void ClearFilesPopupById(string uuid)
              {
                  try{
                      SQLiteConnection connection = GetConnection();
                      connection.CreateTable<DBpopupfileTO>();
        //                var tab = connection.Table<DBpopupfileTO>();
                      connection.Query<DBpopupfileTO> ("DELETE FROM DBpopupfileTO WHERE uuid = ?", uuid);
                  }
                  catch//(Exception ex)
                  {
        //                Console.WriteLine (ex.Message);
                  }
                  //connection.Delete<DBpopupfileTO>(connection.Table<DBpopupfileTO>().Where(v=>v.email.Equals(email)));// .Table<DBpopupContact>().Where(v=>v.email == "");//  .DropTable<DBpopupContact> ();
              }

              public void ClearDefaultFileTO(DBDefaultFileTO item = null)
              {
                  SQLiteConnection connection = GetConnection();
                  connection.CreateTable<DBDefaultFileTO>();
        //            try
        //            {
                      if (item == null)
                      {
                          connection.DropTable<DBDefaultFileTO> ();
                          return;
                      }
                  connection.Delete (item);
        //            }
        //            catch{
        //            }

              }

        public List<DBDefaultFileTO> GetDefaultFileTO()
        {
          List<DBDefaultFileTO> items = new List<DBDefaultFileTO>();
          SQLiteConnection connection = GetConnection();
          connection.CreateTable<DBDefaultFileTO>();
          var table = connection.Table<DBDefaultFileTO>();

          foreach (DBDefaultFileTO item in table)
          {
              items.Add(item);
          }

          return items;
        }

        public void AddDefaultFileTO(DBDefaultFileTO item)
        {
          SQLiteConnection connection = GetConnection();
          connection.CreateTable<DBDefaultFileTO>();
          connection.Insert (item);
        }

              public List<OrderType> GetAllContactsFromShare(List<DBOverviewContacts> _list_share)
              {
                  //var _list_share = new List<DBOverviewContacts> ();
                  var _list_ordertype = new List<OrderType> ();
                  try{
                      //_list_share = DBLocalDataStore.GetInstance().GetOverwievContacts(-1, "new").FindAll(s=>s.isfiles == true);
                      //_list_share = (from contact in _list_share where contact.isfiles == true select contact);

                      if (_list_share.Count == 0) return _list_ordertype;

                      foreach (var ls in _list_share)
                      {
                          var _order = new OrderType();
                          var outCustomers = new CustomerType ();
                          outCustomers.company = ls.company;
                          outCustomers.firstname = ls.firstName;
                          outCustomers.lastname = ls.lastName;
                          outCustomers.email = ls.email;
                          outCustomers.phone = ls.phone;
                          //outCustomers.source = ls.source;
                          outCustomers.zip = ls.zip;
                          outCustomers.street = ls.street;
                          outCustomers.city = ls.city;
                          outCustomers.country = ls.country;
                          //if (!string.IsNullOrEmpty(ls.barcode)){
                          //  outCustomers.badge = new BrandTappBadge {
                          //      properties = new IBadgeProperties{ barcode = ls.barcode },
                          //      type = "barcode"
                          //  };
                          //}
                          //2015-09-08T13:34:45+06:00

                          _order.created = string.Format("{0:yyyy-MM-ddTH:mm:sszzz}", new DateTimeOffset(ls.datetime));//string.Format("{0:yyyy-MM-ddTH:mm:sszzz}", dto);
        //                    Console.WriteLine(_order.created);
                          _order.customer = outCustomers;
                          _order.creator = GetUserMail();
                          _order.profile = GetSelectProfile ().shortName;

                          var _list_share_files = GetOverwievFiles(ls.session, "new");
                          var _line = new List<OrderLineType>();

                          foreach (var _lf in _list_share_files)
                          {
                              var _linet = new OrderLineType();
                              _linet.item = _lf.uuid;
                              _linet.itemDescription = _lf.name;
                              _line.Add(_linet);
                          }
                          _order.orderLine = _line.ToArray();
                          if (_list_share_files.Count != 0)
                              _list_ordertype.Add(_order);
                      }

                  }
                  catch//(Exception ex)
                  {
        //                Console.WriteLine (ex.Message);
                  }
                  return _list_ordertype;
              }

              public void AddOverwievContact(DBOverviewContacts item)
              {
                  SQLiteConnection connection = GetConnection();
                  connection.CreateTable<DBOverviewContacts>();

                  //if (flag_added)
                  connection.Insert(item);
              }

              public void UpdateOverwievContact(DBOverviewContacts item)
              {
                  SQLiteConnection connection = GetConnection();
                  connection.CreateTable<DBOverviewContacts>();
                  connection.Update(item);
              }

              public List<DBOverviewContacts> GetOverwievContacts(int _session, string _status)
              {
                  List<DBOverviewContacts> profile = new List<DBOverviewContacts>();
                  try
                  {
                      SQLiteConnection connection = GetConnection();
                      TableQuery<DBOverviewContacts> table = null;

                      if (_session > 0 && _status != ""){
                          table = connection.Table<DBOverviewContacts> ()
                              .Where (o => o.session == _session && o.status == _status);
                      }
                      else if (_session < 0 && _status == ""){
                          table = connection.Table<DBOverviewContacts> ();
                      }
                      else if (_session > 0 && _status == ""){
                          table = connection.Table<DBOverviewContacts> ().Where (o => o.session == _session);
                      }
                      else if (_session < 0 && _status != ""){
                          table = connection.Table<DBOverviewContacts> ().Where (o => o.status == _status);
                      }


                      foreach (var pp in table)
                      {
                          profile.Add(pp);
                      }
                  }
                  catch //(Exception ex)
                  {
        //                Console.WriteLine (ex.Message);
                      // not sure what to do about logging on the phone
                  }
                  return profile;
              }

        public void ClearAllOverwievContacts()
        {
          SQLiteConnection connection = GetConnection();
          //connection.CreateTable<DBlocalContact>();
          connection.DropTable<DBOverviewContacts> ();
        }

              public int GetLastSession()
              {
                  int session = 0;
                  try
                  {
                      SQLiteConnection connection = GetConnection();
                      var table = connection.Table<DBOverviewContacts> ();
                      var el = table.Count() - 1;
                      session = table.ElementAt (el).session;

                  }
                  catch //(Exception ex)
                  {
        //                Console.WriteLine (ex.Message);
                      // not sure what to do about logging on the phone
                  }
        //            Console.WriteLine ("session = " + session);
                  return session;
              }

              public void UpdateOverwievFile(DBOverviewFileTO item)
              {
                  SQLiteConnection connection = GetConnection();
                  connection.CreateTable<DBOverviewFileTO>();
                  connection.Update(item);
              }

              public void AddOverwievFile(DBOverviewFileTO item)
              {
                  bool flag_added = true;

                  SQLiteConnection connection = GetConnection();
                  connection.CreateTable<DBOverviewFileTO>();
                  var table = connection.Table<DBOverviewFileTO>();

                  foreach (var pp in table)
                  {
        //                Console.WriteLine (pp.name + ", " + pp.session);

                  }

                  if (flag_added)
                      connection.Insert(item);
              }

              public List<DBOverviewFileTO> GetOverwievFiles(int _session, string _status)
              {
                  List<DBOverviewFileTO> profile = new List<DBOverviewFileTO>();
                  try
                  {
                      SQLiteConnection connection = GetConnection();
                      TableQuery<DBOverviewFileTO> table = null;

                      if (_session > 0 && _status != ""){
                          table = connection.Table<DBOverviewFileTO> ()
                              .Where (o => o.session == _session && o.status == _status);
                      }
                      else if (_session < 0 && _status == ""){
                          table = connection.Table<DBOverviewFileTO> ();
                      }
                      else if (_session > 0 && _status == ""){
                          table = connection.Table<DBOverviewFileTO> ().Where (o => o.session == _session);
                      }
                      else if (_session < 0 && _status != ""){
                          table = connection.Table<DBOverviewFileTO> ().Where (o => o.status == _status);
                      }
                      //var bas = table.First();

                      foreach (var pp in table)
                      {
                          profile.Add(pp);
                      }
                  }
                  catch //(Exception ex)
                  {
        //                Console.WriteLine (ex.Message);
                      // not sure what to do about logging on the phone
                  }
                  return profile;
              }

        public void ClearAllOverwievFiles()
        {
          SQLiteConnection connection = GetConnection();
          //connection.CreateTable<DBlocalContact>();
          connection.DropTable<DBOverviewFileTO> ();
        }

              public int GetLastSessionOrders()
              {
                  int session = 0;
                  try
                  {
                      SQLiteConnection connection = GetConnection();
                      var table = connection.Table<DBOverviewFileTO> ();
                      var el = table.Count() - 1;
                      session = table.ElementAt (el).session;

                  }
                  catch //(Exception ex)
                  {
        //                Console.WriteLine (ex.Message);
                      // not sure what to do about logging on the phone
                  }
        //            Console.WriteLine ("session = " + session);
                  return session;
              }

              public void AddOverwievAnswer(DBOverviewQuestionAnswer item)
              {
                  bool flag_added = true;

                  SQLiteConnection connection = GetConnection();
                  connection.CreateTable<DBOverviewQuestionAnswer>();
                  var table = connection.Table<DBOverviewQuestionAnswer>();

                  foreach (var pp in table)
                  {
        //                Console.WriteLine (pp.question + ", " + pp.session);

                  }

                  if (flag_added)
                      connection.Insert(item);
              }

                public void UpdateOverwievAnswer(DBOverviewQuestionAnswer item)
                {
                    SQLiteConnection connection = GetConnection();
                    connection.CreateTable<DBOverviewQuestionAnswer>();
                    connection.Update(item);
                }

              public List<DBOverviewQuestionAnswer> GetOverwievAnswers(int _session, string _status)
              {
                  List<DBOverviewQuestionAnswer> profile = new List<DBOverviewQuestionAnswer>();
                  try
                  {
                      SQLiteConnection connection = GetConnection();
                      TableQuery<DBOverviewQuestionAnswer> table = null;

                      if (_session > 0 && _status != ""){
                          table = connection.Table<DBOverviewQuestionAnswer> ()
                              .Where (o => o.session == _session && o.status == _status);
                      }
                      else if (_session < 0 && _status == ""){
                          table = connection.Table<DBOverviewQuestionAnswer> ();
                      }
                      else if (_session > 0 && _status == ""){
                          table = connection.Table<DBOverviewQuestionAnswer> ().Where (o => o.session == _session);
                      }
                      else if (_session < 0 && _status != ""){
                          table = connection.Table<DBOverviewQuestionAnswer> ().Where (o => o.status == _status);
                      }
                      //var bas = table.First();

                      foreach (var pp in table)
                      {
                          profile.Add(pp);
                      }
                  }
                  catch //(Exception ex)
                  {
        //                Console.WriteLine (ex.Message);
                      // not sure what to do about logging on the phone
                  }
                  return profile;
              }

              public int GetLastSessionForms()
              {
                  int session = 0;
                  try
                  {
                      SQLiteConnection connection = GetConnection();
                      var table = connection.Table<DBOverviewQuestionAnswer> ();
                      var el = table.Count() - 1;
                      session = table.ElementAt (el).session;

                  }
                  catch //(Exception ex)
                  {
        //                Console.WriteLine (ex.Message);
                      // not sure what to do about logging on the phone
                  }
        //            Console.WriteLine ("session = " + session);
                  return session;
              }

              public void ClearAllOverwievAnswers()
              {
                  SQLiteConnection connection = GetConnection();
                  //connection.CreateTable<DBlocalContact>();
                  connection.DropTable<DBOverviewQuestionAnswer> ();
              }
    }

    [Table("DBOverviewQuestionAnswer")]
    public class DBOverviewQuestionAnswer
    {
        [PrimaryKey, AutoIncrement, Column("_id"), Unique]
        public int Id { get; set; }

        public string question { get; set; }
        //public string options { get; set; }
        //public int? maxLength { get; set; }
        public bool required { get; set; }
        public string name_question { get; set; }
        public string type_question { get; set; } // ['string' or 'datetime' or 'date' or 'integer'],
        //public string uuidgroup { get; set; }

        public string answer { get; set; }
        public string name_answer { get; set; }
        public string type_answer { get; set; }

        public DateTime datetime { get; set; }

        public int session { get; set; }// numbers, how match tap on FLAG, ex. - 1,2,3...
        public string status { get; set; }//"new" or "server"
    }

    [Table("DBOverviewFileTO")]
    public class DBOverviewFileTO
    {
        [PrimaryKey, AutoIncrement, Column("_id"), Unique]
        public int Id { get; set; }

        public string uuid { get; set; }

        public string name { get; set; }
        public string fileType { get; set; }
        public string md5 { get; set; }
        public string downloadUrl { get; set; }
        public string mimeType { get; set; }
        public string folderUuid { get; set; }
        public string localpath { get; set; }

        public DateTime datetime { get; set; }

        public int session { get; set; }// numbers, how match tap on FLAG, ex. - 1,2,3...
        public string status { get; set; }//"new" or "server"
    }

    [Table("DBOverviewContacts")]
    public class DBOverviewContacts
    {
        [PrimaryKey, AutoIncrement, Column("_id"), Unique]
        public int uuid { get; set; }

        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string company { get; set; }
        public string phone { get; set; }
        public string source { get; set; }
        public string barcode { get; set; }

        public string jobTitle { get; set; }

        public DateTime datetime { get; set; }

        public string externalReference { get; set; }
        public string externalCompanyReference { get; set; }
        public string internalContactName { get; set; }
        public string internalContactEmail { get; set; }
        public string prefix { get; set; }
        public string mobile { get; set; }
        public string fax { get; set; }
        public string vat { get; set; }
        public string function { get; set; }
        public string level { get; set; }
        public string department { get; set; }
        public string street { get; set; }
        public string city { get; set; }
        public string zip { get; set; }
        public string country { get; set; }

        public int session { get; set; }// numbers, how match tap on FLAG, ex - 1,2,3...
        public string status { get; set; }//"new" or "server"

        public bool isfiles { get; set; }
        public bool isanswers { get; set; }
        public bool isSentToWebhook { get; set; }
    }

    [Table("DBDefaultFileTO")]
    public class DBDefaultFileTO
    {
        [PrimaryKey, AutoIncrement, Column("_id"), Unique]
        public int Id { get; set; }

        public string uuid { get; set; }
        public string name { get; set; }
        public string fileType { get; set; }
        public string md5 { get; set; }
        public string downloadUrl { get; set; }
        public string mimeType { get; set; }
        public string folderUuid { get; set; }


        public DBDefaultFileTO()
        {
        }

        public DBDefaultFileTO(DBfileTO source)
        {
            this.uuid = source.uuid;
            this.name = source.name;
            this.fileType = source.fileType;
            this.md5 = source.md5;
            this.downloadUrl = source.downloadUrl;
            this.mimeType = source.mimeType;
            this.folderUuid = source.folderUuid;
        }

        public DBDefaultFileTO(DBpopupfileTO popUpFile)
        {
            this.uuid = popUpFile.uuid;
            this.name = popUpFile.name;
            this.fileType = popUpFile.fileType;
            this.md5 = popUpFile.md5;
            this.downloadUrl = popUpFile.downloadUrl;
            this.mimeType = popUpFile.mimeType;
            this.folderUuid = popUpFile.folderUuid;
        }
    }

    [Table("DBpopupfileTO")]
    public class DBpopupfileTO
    {
        //[PrimaryKey,Column("uuid")]
        public string uuid { get; set; }
        public string name { get; set; }
        public string fileType { get; set; }
        public string md5 { get; set; }
        public string downloadUrl { get; set; }
        public string mimeType { get; set; }
        public string folderUuid { get; set; }


        public DBpopupfileTO()
        {
        }

        public DBpopupfileTO(DBDefaultFileTO source)
        {
            this.uuid = source.uuid;
            this.name = source.name;
            this.fileType = source.fileType;
            this.md5 = source.md5;
            this.downloadUrl = source.downloadUrl;
            this.mimeType = source.mimeType;
            this.folderUuid = source.folderUuid;
        }
    }

    [Table("DBpopupContact")]
    public class DBpopupContact
    {
        [PrimaryKey, AutoIncrement, Column("_id"), Unique]
        public int _id { get; set; }

        public string uuid { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string company { get; set; }
        public string phone { get; set; }
        public string source { get; set; }
        public string barcode { get; set; }

        public string jobTitle { get; set; }

        /*Kiosk fields*/
        public string externalReference { get; set; }
        public string externalCompanyReference { get; set; }
        public string internalContactName { get; set; }
        public string internalContactEmail { get; set; }
        public string prefix { get; set; }
        public string mobile { get; set; }
        public string fax { get; set; }
        public string vat { get; set; }
        public string function { get; set; }
        public string level { get; set; }
        public string department { get; set; }
        public string street { get; set; }
        public string city { get; set; }
        public string zip { get; set; }
        public string country { get; set; }
        /*KioskFields*/

        public DateTime date_time { get; set; }

        public DBpopupContact()
        {
        }

        public DBpopupContact(DBlocalContact lc)
        {
            firstName = lc.firstname;
            lastName = lc.lastname;
            email = lc.email;
            company = lc.company;
            phone = lc.phone;
            jobTitle = lc.jobtitle;
            //source = lc.source;
            street = lc.street;
            zip = lc.zip;
            city = lc.city;
            country = lc.country;
        }
    }
}