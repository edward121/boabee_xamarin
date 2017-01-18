using System;
using System.ComponentModel;

namespace BoaBeePCL
{
	public class  CustomerType
	{
        private const string defaultValue = "notchangednotchanged";

        [DefaultValue(defaultValue)]
        public string city { get; set; }
        [DefaultValue(defaultValue)]
        public string company { get; set; }
        [DefaultValue(defaultValue)]
        public string country { get; set; }
        [DefaultValue(defaultValue)]
        public string department { get; set; }
        [DefaultValue(defaultValue)]
        public string email { get; set; }
        [DefaultValue(defaultValue)]
        public string externalCompanyReference { get; set; }
        [DefaultValue(defaultValue)]
        public string externalReference { get; set; }
        [DefaultValue(defaultValue)]
        public string fax { get; set; }
        [DefaultValue(defaultValue)]
        public string firstname { get; set; }
        [DefaultValue(defaultValue)]
        public string function { get; set; }
        [DefaultValue(defaultValue)]
        public string jobtitle { get; set; }
        [DefaultValue(defaultValue)]
        public string lastname { get; set; }
        [DefaultValue(defaultValue)]
        public string level { get; set; }
        [DefaultValue(defaultValue)]
        public string mobile { get; set; }
        [DefaultValue(defaultValue)]
        public string phone { get; set; }
        [DefaultValue(defaultValue)]
        public string prefix { get; set; }
        [DefaultValue(defaultValue)]
        public string street { get; set; }
        [DefaultValue(defaultValue)]
        public string uid { get; set; }
        [DefaultValue(defaultValue)]
        public string vat { get; set; }
        [DefaultValue(defaultValue)]
        public string zip { get; set; }



        public CustomerType()
        {
        }

        public CustomerType(DBlocalContact contact)
        {
            this.city                       = contact.city;
            this.company                    = contact.company;
            this.country                    = contact.country;
            this.department                 = contact.department;
            this.email                      = contact.email;
            this.externalCompanyReference   = contact.externalCompanyReference;
            this.externalReference          = contact.externalReference;
            this.fax                        = contact.fax;
            this.firstname                  = contact.firstname;
            this.function                   = contact.function;
            this.jobtitle                   = contact.jobtitle;
            this.lastname                   = contact.lastname;
            this.level                      = contact.level;
            this.mobile                     = contact.mobile;
            this.phone                      = contact.phone;
            this.prefix                     = contact.prefix;
            this.street                     = contact.street;
            this.uid                        = contact.uid;
            this.vat                        = contact.vat;
            this.zip                        = contact.zip;
        }

        public CustomerType(DBContactToServer serverContact)
        {
            this.city                       = serverContact.city;
            this.company                    = serverContact.company;
            this.country                    = serverContact.country;
            this.department                 = serverContact.department;
            this.email                      = serverContact.email;
            this.externalCompanyReference   = serverContact.externalCompanyReference;
            this.externalReference          = serverContact.externalReference;
            this.fax                        = serverContact.fax;
            this.firstname                  = serverContact.firstname;
            this.function                   = serverContact.function;
            this.jobtitle                   = serverContact.jobtitle;
            this.lastname                   = serverContact.lastname;
            this.level                      = serverContact.level;
            this.mobile                     = serverContact.mobile;
            this.phone                      = serverContact.phone;
            this.prefix                     = serverContact.prefix;
            this.street                     = serverContact.street;
            this.uid                        = serverContact.uid;
            this.vat                        = serverContact.vat;
            this.zip                        = serverContact.zip;
        }
    }
}

