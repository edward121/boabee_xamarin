using System;
namespace BoaBeePCL
{
    public class LookupResult
    {
        public CustomerType contact { get; set; }
        public string error { get; set; }
        public DBBasicAuthority profiles { get; set; }
        public bool success { get; set; }
    }
}
