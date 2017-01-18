namespace BoaBeePCL
{
    public static class ServerURLs
    {
        public static readonly string baseURL = @"https://services.boabee.com";

        public static string authURL 
        {
            get { return baseURL + @"/services/auth/login";}
        }

        public static string contactsURL
        {
            get { return baseURL + @"/services/sales/customers";}
        }

        public static string contactsURLNew
        {
            get { return baseURL + @"/services/sync/device"; }
        }

        public static string contactLookupURL
        {
            get { return baseURL + @"/services/sync/lookup"; }
        }

        public static string filesAndFormsURL
        {
            get { return baseURL + @"/services/update/app";}
        }
        public static string profilesURL
        {
            get { return baseURL + @"/services/auth/profiles"; }
        }
    }
}

