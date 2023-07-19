using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReferalDB.Models
{
    public class LetterTrayDetailsViewModel
    {
        public string ReferralFirstName { get; set; }
        public string ReferralLastName { get; set; }
        public string FatherFirstName { get; set; }
        public string FatherLastName { get; set; }
        public string MotherFirstName { get; set; }
        public string MotherLastName { get; set; }
        public string BirthDate { get; set; }
        public string ApplicationDate { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string GuardianFirstName { get; set; }
        public string GuardianLastName { get; set; }
        public string DayDate { get; set; }
        public string DistrictName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Zip { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Fax { get; set; }
        public string RecieveLetterDate { get; set; }
    }
}