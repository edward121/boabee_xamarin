using System;
namespace BoaBeePCL
{
    public class LookupContext
    {
        public RequestData context;
        public string contactUid;

        public LookupContext()
        {
        }

        public LookupContext(string contactUid)
        {
            this.contactUid = contactUid;
        }
    }
}