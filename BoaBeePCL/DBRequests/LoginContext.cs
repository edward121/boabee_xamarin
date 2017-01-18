namespace BoaBeePCL
{
    public class LoginContext
    {
        public string username { get; set; }
        public string password { get; set; }
        public string[] tags { get; set; }

        public LoginContext(DBUserLoginRequest user)
        {
            this.username = user.username;
            this.password = user.password;
            this.tags = new string[] { user.tags };
        }

        public LoginContext()
        {
        }
    }
}

