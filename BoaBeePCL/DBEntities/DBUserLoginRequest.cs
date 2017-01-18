using System.Collections.Generic;

namespace BoaBeePCL
{   
    public class DBUserLoginRequest
    {
            public string username { get; set; }
            public string password { get; set; }
            public string tags { get; set; }
            public bool invalidPassword { get; set; }
    }
}
