using System.Collections.Generic;
using SQLite;

namespace BoaBeePCL
{
	[Table("DBlocalContact")]
	public class DBlocalContact
	{
		[PrimaryKey, AutoIncrement, Column("_id"), Unique]
		public int Id { get; set;}

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
        public bool activeContact { get; set;}
        public bool useInRequest { get; set; }


        public ContactSource source { get; set; }


        public DBlocalContact()
        {
        }

        public ContactSource getSource()
        {
            return this.source;
        }

        public void setSource(ContactSource source)
        {
            this.source = source;
        }

        public bool hasOnlyUID()
        {
            if (string.IsNullOrWhiteSpace(this.firstname) &&
                string.IsNullOrWhiteSpace(this.lastname) &&
                string.IsNullOrWhiteSpace(this.email) &&
                string.IsNullOrWhiteSpace(this.company) &&
                string.IsNullOrWhiteSpace(this.phone) &&
                string.IsNullOrWhiteSpace(this.jobtitle) &&
                string.IsNullOrWhiteSpace(this.street) &&
                string.IsNullOrWhiteSpace(this.city) &&
                string.IsNullOrWhiteSpace(this.country) &&
                string.IsNullOrWhiteSpace(this.zip))
            {
                return true;
            }

            return false;
        }

        public bool hasOnlyUIDAndName()
        {
            if (!string.IsNullOrWhiteSpace(this.firstname) &&
                !string.IsNullOrWhiteSpace(this.lastname) &&
                string.IsNullOrWhiteSpace(this.email) &&
                string.IsNullOrWhiteSpace(this.company) &&
                string.IsNullOrWhiteSpace(this.phone) &&
                string.IsNullOrWhiteSpace(this.jobtitle) &&
                string.IsNullOrWhiteSpace(this.street) &&
                string.IsNullOrWhiteSpace(this.city) &&
                string.IsNullOrWhiteSpace(this.country) &&
                string.IsNullOrWhiteSpace(this.zip))
            {
                return true;
            }

            return false;
        }

        public DBlocalContact(CustomerType serverContact)
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



        /// <summary>
        /// overwrites not empty fields in <c>c1</c> with not empty values of <c>c2</c>.
        /// </summary>
        /// <param name="c1">The first <see cref="BoaBeePCL.DBlocalContact"/> to add.</param>
        /// <param name="c2">The second <see cref="BoaBeePCL.DBlocalContact"/> to add.</param>
        /// <returns>The <see cref="T:BoaBeePCL.DBlocalContact"/> with updated fields.</returns>
        public static DBlocalContact operator ^(DBlocalContact c1, DBlocalContact c2)
        {
            if (c1 == null)
            {
                return null;
            }
            if (c2 == null)
            {
                return c1;
            }

            c1.city                     = !string.IsNullOrEmpty(c1.city)                     ? c2.city                       : c1.city;
            c1.company                  = !string.IsNullOrEmpty(c1.company)                  ? c2.company                    : c1.company;
            c1.country                  = !string.IsNullOrEmpty(c1.country)                  ? c2.country                    : c1.country;
            c1.department               = !string.IsNullOrEmpty(c1.department)               ? c2.department                 : c1.department;
            c1.email                    = !string.IsNullOrEmpty(c1.email)                    ? c2.email                      : c1.email;
            c1.externalCompanyReference = !string.IsNullOrEmpty(c1.externalCompanyReference) ? c2.externalCompanyReference   : c1.externalCompanyReference;
            c1.externalReference        = !string.IsNullOrEmpty(c1.externalReference)        ? c2.externalReference          : c1.externalReference;
            c1.fax                      = !string.IsNullOrEmpty(c1.fax)                      ? c2.fax                        : c1.fax;
            c1.firstname                = !string.IsNullOrEmpty(c1.firstname)                ? c2.firstname                  : c1.firstname;
            c1.function                 = !string.IsNullOrEmpty(c1.function)                 ? c2.function                   : c1.function;
            c1.jobtitle                 = !string.IsNullOrEmpty(c1.jobtitle)                 ? c2.jobtitle                   : c1.jobtitle;
            c1.lastname                 = !string.IsNullOrEmpty(c1.lastname)                 ? c2.lastname                   : c1.lastname;
            c1.level                    = !string.IsNullOrEmpty(c1.level)                    ? c2.level                      : c1.level;
            c1.mobile                   = !string.IsNullOrEmpty(c1.mobile)                   ? c2.mobile                     : c1.mobile;
            c1.phone                    = !string.IsNullOrEmpty(c1.phone)                    ? c2.phone                      : c1.phone;
            c1.prefix                   = !string.IsNullOrEmpty(c1.prefix)                   ? c2.prefix                     : c1.prefix;
            c1.street                   = !string.IsNullOrEmpty(c1.street)                   ? c2.street                     : c1.street;
            c1.vat                      = !string.IsNullOrEmpty(c1.vat)                      ? c2.vat                        : c1.vat;
            c1.zip                      = !string.IsNullOrEmpty(c1.zip)                      ? c2.zip                        : c1.zip;

            return c1;
        }
        public static DBlocalContact operator *(DBlocalContact c1, DBlocalContact c2)
        {
            if (c1 == null)
            {
                return null;
            }
            if (c2 == null)
            {
                return c1;
            }

            if (!string.IsNullOrWhiteSpace(c2.uid))
            {

                c1.city = c2.city;
                c1.company = c2.company;
                c1.country = c2.country;
                c1.department = c2.department;
                c1.email = c2.email;
                c1.externalCompanyReference = c2.externalCompanyReference;
                c1.externalReference = c2.externalReference;
                c1.fax = c2.fax;
                c1.firstname = c2.firstname;
                c1.function = c2.function;
                c1.jobtitle = c2.jobtitle;
                c1.lastname = c2.lastname;
                c1.level = c2.level;
                c1.mobile = c2.mobile;
                c1.phone = c2.phone;
                c1.prefix = c2.prefix;
                c1.street = c2.street;
                c1.vat = c2.vat ;
                c1.zip =  c2.zip ;
            }
            return c1;
        }
        /// <summary>
        /// overwrites all fields in <c>c1</c> with not empty values of <c>c2</c>.
        /// </summary>
        /// <param name="c1">The first <see cref="BoaBeePCL.DBlocalContact"/> to add.</param>
        /// <param name="c2">The second <see cref="BoaBeePCL.DBlocalContact"/> to add.</param>
        /// <returns>The <see cref="T:BoaBeePCL.DBlocalContact"/> with updated fields.</returns>
        public static DBlocalContact operator &(DBlocalContact c1, DBlocalContact c2)
        {
            if (c1 == null)
            {
                return null;
            }
            if (c2 == null)
            {
                return c1;
            }

            c1.city                     = string.IsNullOrEmpty(c2.city)                     ? c1.city                       : c2.city;
            c1.company                  = string.IsNullOrEmpty(c2.company)                  ? c1.company                    : c2.company;
            c1.country                  = string.IsNullOrEmpty(c2.country)                  ? c1.country                    : c2.country;
            c1.department               = string.IsNullOrEmpty(c2.department)               ? c1.department                 : c2.department;
            c1.email                    = string.IsNullOrEmpty(c2.email)                    ? c1.email                      : c2.email;
            c1.externalCompanyReference = string.IsNullOrEmpty(c2.externalCompanyReference) ? c1.externalCompanyReference   : c2.externalCompanyReference;
            c1.externalReference        = string.IsNullOrEmpty(c2.externalReference)        ? c1.externalReference          : c2.externalReference;
            c1.fax                      = string.IsNullOrEmpty(c2.fax)                      ? c1.fax                        : c2.fax;
            c1.firstname                = string.IsNullOrEmpty(c2.firstname)                ? c1.firstname                  : c2.firstname;
            c1.function                 = string.IsNullOrEmpty(c2.function)                 ? c1.function                   : c2.function;
            c1.jobtitle                 = string.IsNullOrEmpty(c2.jobtitle)                 ? c1.jobtitle                   : c2.jobtitle;
            c1.lastname                 = string.IsNullOrEmpty(c2.lastname)                 ? c1.lastname                   : c2.lastname;
            c1.level                    = string.IsNullOrEmpty(c2.level)                    ? c1.level                      : c2.level;
            c1.mobile                   = string.IsNullOrEmpty(c2.mobile)                   ? c1.mobile                     : c2.mobile;
            c1.phone                    = string.IsNullOrEmpty(c2.phone)                    ? c1.phone                      : c2.phone;
            c1.prefix                   = string.IsNullOrEmpty(c2.prefix)                   ? c1.prefix                     : c2.prefix;
            c1.street                   = string.IsNullOrEmpty(c2.street)                   ? c1.street                     : c2.street;
            c1.vat                      = string.IsNullOrEmpty(c2.vat)                      ? c1.vat                        : c2.vat;
            c1.zip                      = string.IsNullOrEmpty(c2.zip)                      ? c1.zip                        : c2.zip;

            return c1;
        }

        /// <summary>
        /// overwrites empty fields in <c>c1</c> with values of <c>c2</c>.
        /// </summary>
        /// <param name="c1">The first <see cref="BoaBeePCL.DBlocalContact"/> to add.</param>
        /// <param name="c2">The second <see cref="BoaBeePCL.DBlocalContact"/> to add.</param>
        /// <returns>The <see cref="T:BoaBeePCL.DBlocalContact"/> with overwritten empty fields.</returns>
        public static DBlocalContact operator +(DBlocalContact c1, DBlocalContact c2)
        {
            if (c1 == null)
            {
                return null;
            }
            if (c2 == null)
            {
                return c1;
            }

            c1.city                     = string.IsNullOrEmpty(c1.city)                     ? c2.city                       : c1.city;
            c1.company                  = string.IsNullOrEmpty(c1.company)                  ? c2.company                    : c1.company;
            c1.country                  = string.IsNullOrEmpty(c1.country)                  ? c2.country                    : c1.country;
            c1.department               = string.IsNullOrEmpty(c1.department)               ? c2.department                 : c1.department;
            c1.email                    = string.IsNullOrEmpty(c1.email)                    ? c2.email                      : c1.email;
            c1.externalCompanyReference = string.IsNullOrEmpty(c1.externalCompanyReference) ? c2.externalCompanyReference   : c1.externalCompanyReference;
            c1.externalReference        = string.IsNullOrEmpty(c1.externalReference)        ? c2.externalReference          : c1.externalReference;
            c1.fax                      = string.IsNullOrEmpty(c1.fax)                      ? c2.fax                        : c1.fax;
            c1.firstname                = string.IsNullOrEmpty(c1.firstname)                ? c2.firstname                  : c1.firstname;
            c1.function                 = string.IsNullOrEmpty(c1.function)                 ? c2.function                   : c1.function;
            c1.jobtitle                 = string.IsNullOrEmpty(c1.jobtitle)                 ? c2.jobtitle                   : c1.jobtitle;
            c1.lastname                 = string.IsNullOrEmpty(c1.lastname)                 ? c2.lastname                   : c1.lastname;
            c1.level                    = string.IsNullOrEmpty(c1.level)                    ? c2.level                      : c1.level;
            c1.mobile                   = string.IsNullOrEmpty(c1.mobile)                   ? c2.mobile                     : c1.mobile;
            c1.phone                    = string.IsNullOrEmpty(c1.phone)                    ? c2.phone                      : c1.phone;
            c1.prefix                   = string.IsNullOrEmpty(c1.prefix)                   ? c2.prefix                     : c1.prefix;
            c1.street                   = string.IsNullOrEmpty(c1.street)                   ? c2.street                     : c1.street;
            c1.vat                      = string.IsNullOrEmpty(c1.vat)                      ? c2.vat                        : c1.vat;
            c1.zip                      = string.IsNullOrEmpty(c1.zip)                      ? c2.zip                        : c1.zip;

            return c1;
        }

        public void updateFromServerContact(CustomerType serverContact)
        {
            this.city = string.IsNullOrEmpty(this.city) ? serverContact.city : this.city;
            this.company = string.IsNullOrEmpty(this.company) ? serverContact.company : this.company;
            this.country = string.IsNullOrEmpty(this.country) ? serverContact.country : this.country;
            this.department = string.IsNullOrEmpty(this.department) ? serverContact.department : this.department;
            this.email = string.IsNullOrEmpty(this.email) ? serverContact.email : this.email;
            this.externalCompanyReference = string.IsNullOrEmpty(this.externalCompanyReference) ? serverContact.externalCompanyReference : this.externalCompanyReference;
            this.externalReference = string.IsNullOrEmpty(this.externalReference) ? serverContact.externalReference : this.externalReference;
            this.fax = string.IsNullOrEmpty(this.fax) ? serverContact.fax : this.fax;
            this.firstname = string.IsNullOrEmpty(this.firstname) ? serverContact.firstname : this.firstname;
            this.function = string.IsNullOrEmpty(this.function) ? serverContact.function : this.function;
            this.jobtitle = string.IsNullOrEmpty(this.jobtitle) ? serverContact.jobtitle : this.jobtitle;
            this.lastname = string.IsNullOrEmpty(this.lastname) ? serverContact.lastname : this.lastname;
            this.level = string.IsNullOrEmpty(this.level) ? serverContact.level : this.level;
            this.mobile = string.IsNullOrEmpty(this.mobile) ? serverContact.mobile : this.mobile;
            this.phone = string.IsNullOrEmpty(this.phone) ? serverContact.phone : this.phone;
            this.prefix = string.IsNullOrEmpty(this.prefix) ? serverContact.prefix : this.prefix;
            this.street = string.IsNullOrEmpty(this.street) ? serverContact.street : this.street;
            this.vat = string.IsNullOrEmpty(this.vat) ? serverContact.vat : this.vat;
            this.zip = string.IsNullOrEmpty(this.zip) ? serverContact.zip : this.zip;
        }
	}
}

