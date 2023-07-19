using ReferalDB.AppFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace ReferalDB.Models
{
    public class ContactModel
    {
        
        public virtual int Id { get; set; }
        public virtual IEnumerable<SelectListItem> FirstNamePrefixList { get; set; }
        public virtual string FirstNamePrefix { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string MiddleName { get; set; }
        public virtual string ContactFor { get; set; }
        public virtual IEnumerable<SelectListItem> LastNameSuffixList { get; set; }
        public virtual string LastNameSuffix { get; set; }
        public virtual IEnumerable<SelectListItem> RelationList { get; set; }
        public virtual int Relation { get; set; }
        public virtual string Spouse { get; set; }
        public virtual string UserID { get; set; }
        public virtual string PrimaryLanguage { get; set; }
        public virtual IEnumerable<SelectListItem> HomeAddressTypeList { get; set; }
        public virtual int HomeAddressTypeId { get; set; }
        public virtual IEnumerable<SelectListItem> WorkAddressTypeList { get; set; }
        public virtual int WorkAddressTypeId { get; set; }
        public virtual IEnumerable<SelectListItem> OtherAddressTypeList { get; set; }
        public virtual int OtherAddressTypeId { get; set; }
        public virtual string HomeAddressLine1 { get; set; }
        public virtual string HomeAddressLine2 { get; set; }
        public virtual string HomeAddressLine3 { get; set; }
        public virtual string ContactFlag { get; set; }
        public virtual IEnumerable<SelectListItem> HomeCountryList { get; set; }
        public virtual int HomeCountry { get; set; }
        public virtual string HomeCounty { get; set; }
        public virtual IEnumerable<SelectListItem> HomeStateList { get; set; }
        public virtual int? HomeState { get; set; }
        public virtual string HomeCity { get; set; }
        public virtual string HomeStreet { get; set; }
        public virtual int? HomeNumber { get; set; }
       // public virtual IEnumerable<SelectListItem> ZipList { get; set; }
        public virtual string HomeZip { get; set; }
        public virtual string HomePhone { get; set; }
        public virtual string HomeMobilePhone { get; set; }
        public virtual string HomeWorkPhone { get; set; }
        public virtual string HomeFax { get; set; }
        public virtual string HomeEmail { get; set; }
        public virtual string HomeWorkEmail { get; set; }

        public virtual string WorkAddressLine1 { get; set; }
        public virtual string WorkAddressLine2 { get; set; }
        public virtual string WorkAddressLine3 { get; set; }
        public virtual IEnumerable<SelectListItem> WorkCountryList { get; set; }
        public virtual int WorkCountry { get; set; }
        public virtual IEnumerable<SelectListItem> WorkStateList { get; set; }
        public virtual int? WorkState { get; set; }
        public virtual string WorkCity { get; set; }
        // public virtual IEnumerable<SelectListItem> ZipList { get; set; }
        public virtual string WorkZip { get; set; }
        public virtual string WorkHomePhone { get; set; }
        public virtual string WorkMobilePhone { get; set; }
        public virtual string WorkPhone { get; set; }
        public virtual string WorkFax { get; set; }
        public virtual string WorkHomeEmail { get; set; }
        public virtual string WorkEmail { get; set; }
        public virtual string WorkCounty { get; set; }

        public virtual string OtherAddressLine1 { get; set; }
        public virtual string OtherAddressLine2 { get; set; }
        public virtual string OtherAddressLine3 { get; set; }
        public virtual IEnumerable<SelectListItem> OtherCountryList { get; set; }
        public virtual int OtherCountry { get; set; }
        public virtual IEnumerable<SelectListItem> OtherStateList { get; set; }
        public virtual int? OtherState { get; set; }
        public virtual string OtherCity { get; set; }
        // public virtual IEnumerable<SelectListItem> ZipList { get; set; }
        public virtual string OtherZip { get; set; }
        public virtual string OtherHomePhone { get; set; }
        public virtual string OtherMobilePhone { get; set; }
        public virtual string OtherWorkPhone { get; set; }
        public virtual string OtherFax { get; set; }
        public virtual string OtherHomeEmail { get; set; }
        public virtual string OtherWorkEmail { get; set; }
        public virtual string OtherCounty { get; set; }

        public virtual string ImageUrl { get; set; }
        
        public virtual string WorkExtension { get; set; }
        public virtual string HomeExtension { get; set; }
        public virtual string OtherExtension { get; set; }
        public virtual string OtherExtension2 { get; set; }
        public virtual string OtherExtension3 { get; set; }

        public IEnumerable<checkBoxViewModel> checkbox { get; set; }

        public IEnumerable<string> getcheked { get; set; }
        Other_Functions objFuns = new Other_Functions();
        public ContactModel()
        {
            checkbox = new List<checkBoxViewModel>();
            FirstNamePrefixList = new List<SelectListItem>();
            LastNameSuffixList = new List<SelectListItem>();
            RelationList = new List<SelectListItem>();
            HomeCountryList = new List<SelectListItem>();
            HomeStateList = new List<SelectListItem>();
            WorkCountryList = new List<SelectListItem>();
            WorkStateList = new List<SelectListItem>();
            OtherCountryList = new List<SelectListItem>();
            OtherStateList = new List<SelectListItem>();
            HomeAddressTypeList = objFuns.getAddressTypes();
            getcheked = new List<string>();
        }
    }
    public class checkBoxViewModel
    {
        public string name { get; set; }
        public bool check { get; set; }
    }
    public class contactFirstName {

        public string username { get; set; }
        public int Usercount { get; set; }
    }
}