using System;
using FireSharp.Config;
using FireSharp;
using FireSharp.Response;
using System.Threading.Tasks;
using BoaBeePCL;

namespace FireBase
{
    public class FirebaseManager
    {
        public delegate void EventHandlerFirebase();
        public delegate void EventExceptionHandlerFirebase(Exception e);
        public delegate void EventHandlerFirebaseValue<T>(T value_returned);
        public delegate void EventHandlerFirebaseValue(FirebaseResponse value_returned);

        private static FirebaseManager _instance;
        private static object _lockCreate = new object();

        public static FirebaseManager GetInstance()
        {
            lock (_lockCreate)
            {
                if (_instance == null)
                {
                    _instance = new FirebaseManager();

                }
            }
            return _instance;
        }

        public FirebaseManager()
        {
        }

        private FirebaseClient Init(string basePath)
        {
            FirebaseConfig config = new FirebaseConfig
            {
                AuthSecret = "y9YqBtgM1l5U05RysYuKGBlzQr83Vob95H7KseqN",
                BasePath = basePath
            };
            return new FirebaseClient(config);
        }
        public string GetObjectByNameJson(string name_database)
        {
            FirebaseResponse response = Init("https://boabeefirebase-f2491.firebaseio.com").Get(name_database);
            string version = response.ResultAs<string>();
            return version;

        }
    }
}