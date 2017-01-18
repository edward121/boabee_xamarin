using System;
using SQLite;

namespace BoaBeePCL
{
    public class DBContactToServer
    {
        private const string valueForUnchanged = "notchangednotchanged";

        [PrimaryKey, AutoIncrement, Column("_id"), Unique]
        public int Id { get; set; }

        public string city { get; set; }
        public string company { get; set; }
        public string country { get; set; }
        public string department { get; set; }
        public string email { get; set; }
        public string externalCompanyReference { get; set; }
        public string externalReference { get; set; }
        public string fax { get; set; }
        public string firstname { get; set; }
        public string function { get; set; }
        public string jobtitle { get; set; }
        public string lastname { get; set; }
        public string level { get; set; }
        public string mobile { get; set; }
        public string phone { get; set; }
        public string prefix { get; set; }
        public string street { get; set; }
        public string uid { get; set; }
        public string vat { get; set; }
        public string zip { get; set; }

        private DateTime dateCity{ get; set; }
        private DateTime dateCompany { get; set; }
        private DateTime dateCountry { get; set; }
        private DateTime dateDepartment { get; set; }
        private DateTime dateEmail { get; set; }
        private DateTime dateExternalCompanyReference { get; set; }
        private DateTime dateExternalReference { get; set; }
        private DateTime dateFax { get; set; }
        private DateTime dateFirstname { get; set; }
        private DateTime dateFunction { get; set; }
        private DateTime dateJobtitle { get; set; }
        private DateTime dateLastname { get; set; }
        private DateTime dateLevel { get; set; }
        private DateTime dateMobile { get; set; }
        private DateTime datePhone { get; set; }
        private DateTime datePrefix { get; set; }
        private DateTime dateStreet { get; set; }
        private DateTime dateVat { get; set; }
        private DateTime dateZip { get; set; }

        public DBContactToServer()
        {
        }

        public DBContactToServer(bool fillWithDefaultValues = false)
        {
            if (fillWithDefaultValues)
            {
                this.city = valueForUnchanged;
                this.company = valueForUnchanged;
                this.country = valueForUnchanged;
                this.department = valueForUnchanged;
                this.email = valueForUnchanged;
                this.externalCompanyReference = valueForUnchanged;
                this.externalReference = valueForUnchanged;
                this.fax = valueForUnchanged;
                this.firstname = valueForUnchanged;
                this.function = valueForUnchanged;
                this.jobtitle = valueForUnchanged;
                this.lastname = valueForUnchanged;
                this.level = valueForUnchanged;
                this.mobile = valueForUnchanged;
                this.phone = valueForUnchanged;
                this.prefix = valueForUnchanged;
                this.street = valueForUnchanged;
                this.vat = valueForUnchanged;
                this.zip = valueForUnchanged;
            }
        }

        public DBContactToServer(DBlocalContact c, bool isNewContact = false)
        {
            if (c == null)
            {
                throw new ArgumentNullException("Contact must not be null");
            }
            //if (string.IsNullOrWhiteSpace(c.uid))
            //{
            //    throw new ArgumentException("Contact UID must not be null or empty");
            //}

            this.uid = c.uid;

            this.city                       = c.city != null || !isNewContact                       ? c.city                        : valueForUnchanged;
            this.company                    = c.company != null || !isNewContact                    ? c.company                     : valueForUnchanged;
            this.country                    = c.country != null || !isNewContact                    ? c.country                     : valueForUnchanged;
            this.department                 = c.department != null || !isNewContact                 ? c.department                  : valueForUnchanged;
            this.email                      = c.email != null || !isNewContact                      ? c.email                       : valueForUnchanged;
            this.externalCompanyReference   = c.externalCompanyReference != null || !isNewContact   ? c.externalCompanyReference    : valueForUnchanged;
            this.externalReference          = c.externalReference != null || !isNewContact          ? c.externalReference           : valueForUnchanged;
            this.fax                        = c.fax != null || !isNewContact                        ? c.fax                         : valueForUnchanged;
            this.firstname                  = c.firstname != null || !isNewContact                  ? c.firstname                   : valueForUnchanged;
            this.function                   = c.function != null || !isNewContact                   ? c.function                    : valueForUnchanged;
            this.jobtitle                   = c.jobtitle != null || !isNewContact                   ? c.jobtitle                    : valueForUnchanged;
            this.lastname                   = c.lastname != null || !isNewContact                   ? c.lastname                    : valueForUnchanged;
            this.level                      = c.level != null || !isNewContact                      ? c.level                       : valueForUnchanged;
            this.mobile                     = c.mobile != null || !isNewContact                     ? c.mobile                      : valueForUnchanged;
            this.phone                      = c.phone != null || !isNewContact                      ? c.phone                       : valueForUnchanged;
            this.prefix                     = c.prefix != null || !isNewContact                     ? c.prefix                      : valueForUnchanged;
            this.street                     = c.street != null || !isNewContact                     ? c.street                      : valueForUnchanged;
            this.vat                        = c.vat != null || !isNewContact                        ? c.vat                         : valueForUnchanged;
            this.zip                        = c.zip != null || !isNewContact                        ? c.zip                         : valueForUnchanged;
        }

        public static DBContactToServer operator &(DBContactToServer c1, DBContactToServer c2)
        {
            if (c1 == null)
            {
                return c2;
            }
            if (c2 == null)
            {
                return c1;
            }

            c1.city =                       c1.dateCity< c2.dateCity                                            ? c2.city                       : c1.city;
            c1.company =                    c1.dateCompany < c2.dateCompany                                     ? c2.company                    : c1.company;
            c1.country =                    c1.dateCountry < c2.dateCountry                                     ? c2.country                    : c1.country;
            c1.department =                 c1.dateDepartment < c2.dateDepartment                               ? c2.department                 : c1.department;
            c1.email =                      c1.dateEmail < c2.dateEmail                                         ? c2.email                      : c1.email;
            c1.externalCompanyReference =   c1.dateExternalCompanyReference < c2.dateExternalCompanyReference   ? c2.externalCompanyReference   : c1.externalCompanyReference;
            c1.externalReference =          c1.dateExternalReference < c2.dateExternalReference                 ? c2.externalReference          : c1.externalReference;
            c1.fax =                        c1.dateFax < c2.dateFax                                             ? c2.fax                        : c1.fax;
            c1.firstname =                  c1.dateFirstname < c2.dateFirstname                                 ? c2.firstname                  : c1.firstname;
            c1.function =                   c1.dateFunction < c2.dateFunction                                   ? c2.function                   : c1.function;
            c1.jobtitle =                   c1.dateJobtitle < c2.dateJobtitle                                   ? c2.jobtitle                   : c1.jobtitle;
            c1.lastname =                   c1.dateLastname < c2.dateLastname                                   ? c2.lastname                   : c1.lastname;
            c1.level =                      c1.dateLevel < c2.dateLevel                                         ? c2.level                      : c1.level;
            c1.mobile =                     c1.dateMobile < c2.dateMobile                                       ? c2.mobile                     : c1.mobile;
            c1.phone =                      c1.datePhone < c2.datePhone                                         ? c2.phone                      : c1.phone;      
            c1.prefix =                     c1.datePrefix < c2.datePrefix                                       ? c2.prefix                     : c1.prefix;
            c1.street =                     c1.dateStreet < c2.dateStreet                                       ? c2.street                     : c1.street;
            c1.vat =                        c1.dateVat < c2.dateVat                                             ? c2.vat                        : c1.vat;
            c1.zip =                        c1.dateZip < c2.dateZip                                             ? c2.zip                        : c1.zip;

            c1.dateCity =                       c1.dateCity < c2.dateCity                                           ? c2.dateCity                       : c1.dateCity;
            c1.dateCompany =                    c1.dateCompany < c2.dateCompany                                     ? c2.dateCompany                    : c1.dateCompany;
            c1.dateCountry =                    c1.dateCountry < c2.dateCountry                                     ? c2.dateCountry                    : c1.dateCountry;
            c1.dateDepartment=                  c1.dateDepartment < c2.dateDepartment                               ? c2.dateDepartment                 : c1.dateDepartment;
            c1.dateEmail =                      c1.dateEmail < c2.dateEmail                                         ? c2.dateEmail                      : c1.dateEmail;
            c1.dateExternalCompanyReference =   c1.dateExternalCompanyReference < c2.dateExternalCompanyReference   ? c2.dateExternalCompanyReference   : c1.dateExternalCompanyReference;
            c1.dateExternalReference =          c1.dateExternalReference < c2.dateExternalReference                 ? c2.dateExternalReference          : c1.dateExternalReference;
            c1.dateFax =                        c1.dateFax < c2.dateFax                                             ? c2.dateFax                        : c1.dateFax;
            c1.dateFirstname =                  c1.dateFirstname < c2.dateFirstname                                 ? c2.dateFirstname                  : c1.dateFirstname;
            c1.dateFunction =                   c1.dateFunction < c2.dateFunction                                   ? c2.dateFunction                   : c1.dateFunction;
            c1.dateJobtitle =                   c1.dateJobtitle < c2.dateJobtitle                                   ? c2.dateJobtitle                   : c1.dateJobtitle;
            c1.dateLastname =                   c1.dateLastname < c2.dateLastname                                   ? c2.dateLastname                   : c1.dateLastname;
            c1.dateLevel =                      c1.dateLevel < c2.dateLevel                                         ? c2.dateLevel                      : c1.dateLevel;
            c1.dateMobile =                     c1.dateMobile < c2.dateMobile                                       ? c2.dateMobile                     : c1.dateMobile;
            c1.datePhone =                      c1.datePhone < c2.datePhone                                         ? c2.datePhone                      : c1.datePhone;
            c1.datePrefix =                     c1.datePrefix < c2.datePrefix                                       ? c2.datePrefix                     : c1.datePrefix;
            c1.dateStreet=                      c1.dateStreet < c2.dateStreet                                       ? c2.dateStreet                     : c1.dateStreet;
            c1.dateVat =                        c1.dateVat < c2.dateVat                                             ? c2.dateVat                        : c1.dateVat;
            c1.dateZip =                        c1.dateZip < c2.dateZip                                             ? c2.dateZip                        : c1.dateZip;


            return c1;
        }

        public static DBContactToServer operator -(DBContactToServer c1, DBlocalContact c2)
        {
            if (c1 == null)
            {
                return null;
            }
            if (c2 == null)
            {
                return c1;
            }

            DateTime now = DateTime.Now;

            c1.dateCity =                       c1.city != c2.city                                          ? now                           : DateTime.MinValue;
            c1.dateCompany =                    c1.company != c2.company                                    ? now                           : DateTime.MinValue;
            c1.dateCountry =                    c1.country != c2.country                                    ? now                           : DateTime.MinValue;
            c1.dateDepartment =                 c1.department != c2.department                              ? now                           : DateTime.MinValue;
            c1.dateEmail =                      c1.email != c2.email                                        ? now                           : DateTime.MinValue;
            c1.dateExternalCompanyReference =   c1.externalCompanyReference != c2.externalCompanyReference  ? now                           : DateTime.MinValue;
            c1.dateExternalReference =          c1.externalReference != c2.externalReference                ? now                           : DateTime.MinValue;
            c1.dateFax =                        c1.fax != c2.fax                                            ? now                           : DateTime.MinValue;
            c1.dateFirstname =                  c1.firstname != c2.firstname                                ? now                           : DateTime.MinValue;
            c1.dateFunction =                   c1.function != c2.function                                  ? now                           : DateTime.MinValue;
            c1.dateJobtitle =                   c1.jobtitle != c2.jobtitle                                  ? now                           : DateTime.MinValue;
            c1.dateLastname =                   c1.lastname != c2.lastname                                  ? now                           : DateTime.MinValue;
            c1.dateLevel =                      c1.level != c2.level                                        ? now                           : DateTime.MinValue;
            c1.dateMobile =                     c1.mobile != c2.mobile                                      ? now                           : DateTime.MinValue;
            c1.datePhone =                      c1.phone != c2.phone                                        ? now                           : DateTime.MinValue;
            c1.datePrefix =                     c1.prefix != c2.prefix                                      ? now                           : DateTime.MinValue;
            c1.dateStreet =                     c1.street != c2.street                                      ? now                           : DateTime.MinValue;
            c1.dateVat =                        c1.vat != c2.vat                                            ? now                           : DateTime.MinValue;
            c1.dateZip =                        c1.zip != c2.zip                                            ? now                           : DateTime.MinValue;

            c1.city =                       c1.city != c2.city                                              ? c2.city                       : valueForUnchanged;
            c1.company =                    c1.company != c2.company                                        ? c2.company                    : valueForUnchanged;
            c1.country =                    c1.country != c2.country                                        ? c2.country                    : valueForUnchanged;
            c1.department =                 c1.department != c2.department                                  ? c2.department                 : valueForUnchanged;
            c1.email =                      c1.email != c2.email                                            ? c2.email                      : valueForUnchanged;
            c1.externalCompanyReference =   c1.externalCompanyReference != c2.externalCompanyReference      ? c2.externalCompanyReference   : valueForUnchanged;
            c1.externalReference =          c1.externalReference != c2.externalReference                    ? c2.externalReference          : valueForUnchanged;
            c1.fax =                        c1.fax != c2.fax                                                ? c2.fax                        : valueForUnchanged;
            c1.firstname =                  c1.firstname != c2.firstname                                    ? c2.firstname                  : valueForUnchanged;
            c1.function =                   c1.function != c2.function                                      ? c2.function                   : valueForUnchanged;
            c1.jobtitle =                   c1.jobtitle != c2.jobtitle                                      ? c2.jobtitle                   : valueForUnchanged;
            c1.lastname =                   c1.lastname != c2.lastname                                      ? c2.lastname                   : valueForUnchanged;
            c1.level =                      c1.level != c2.level                                            ? c2.level                      : valueForUnchanged;
            c1.mobile =                     c1.mobile != c2.mobile                                          ? c2.mobile                     : valueForUnchanged;
            c1.phone =                      c1.phone != c2.phone                                            ? c2.phone                      : valueForUnchanged;
            c1.prefix =                     c1.prefix != c2.prefix                                          ? c2.prefix                     : valueForUnchanged;
            c1.street =                     c1.street != c2.street                                          ? c2.street                     : valueForUnchanged;
            c1.vat =                        c1.vat != c2.vat                                                ? c2.vat                        : valueForUnchanged;
            c1.zip =                        c1.zip != c2.zip                                                ? c2.zip                        : valueForUnchanged;

            return c1;  
        }
        public DBContactToServer newContact(DBContactToServer c1)
        {
            throw new InvalidOperationException("Deprecated");
            //if (c1 == null)
            //{
            //    return null;
            //}

            //c1.city =                       c1.city                     != null ? c1.city : valueForUnchanged;
            //c1.company =                    c1.company                  != null ? c1.company : valueForUnchanged;
            //c1.country =                    c1.country                  != null ? c1.country : valueForUnchanged;
            //c1.department =                 c1.department               != null ? c1.department : valueForUnchanged;
            //c1.email =                      c1.email                    != null ? c1.email : valueForUnchanged;
            //c1.externalCompanyReference =   c1.externalCompanyReference != null ? c1.externalCompanyReference : valueForUnchanged;
            //c1.externalReference =          c1.externalReference        != null ? c1.externalReference : valueForUnchanged;
            //c1.fax =                        c1.fax                      != null ? c1.fax : valueForUnchanged;
            //c1.firstname =                  c1.firstname                != null ? c1.firstname : valueForUnchanged;
            //c1.function =                   c1.function                 != null ? c1.function : valueForUnchanged;
            //c1.jobtitle =                   c1.jobtitle                 != null ? c1.jobtitle : valueForUnchanged;
            //c1.lastname =                   c1.lastname                 != null ? c1.lastname : valueForUnchanged;
            //c1.level =                      c1.level                    != null ? c1.level : valueForUnchanged;
            //c1.mobile =                     c1.mobile                   != null ? c1.mobile : valueForUnchanged;
            //c1.phone =                      c1.phone                    != null ? c1.phone : valueForUnchanged;
            //c1.prefix =                     c1.prefix                   != null ? c1.prefix : valueForUnchanged;
            //c1.street =                     c1.street                   != null ? c1.street : valueForUnchanged;
            //c1.vat =                        c1.vat                      != null ? c1.vat : valueForUnchanged;
            //c1.zip =                        c1.zip                      != null ? c1.zip : valueForUnchanged;
           
            //return c1;
        }
    }
}

