using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReferalDB.Models
{
    public class StdDetailsQuickUpdateViewModel
    {
        public int StudentPersonalId { get; set; }
        public string studentPersonalName { get; set; }
        public string Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime? AdmissionDate { get; set; }
        public string ImageUrl { get; set; }
        public string PlaceOfBirth { get; set; }
        public DateTime? ApplicationDate { get; set; }
        public string StateOfBirth { get; set; }
        public string currentQueue { get; set; }
        public bool FVqueueStatus { get; set; }
        public string CountryOfBirth { get; set; }
        public string CountryOfCitizenship { get; set; }
        public string MaritalStatus { get; set; }
        public string PrimaryLanguage { get; set; }
        public string Height { get; set; }
        public string Weight { get; set; }
        public string AddressAppartment { get; set; }
        public string AddressStreet { get; set; }
        public string AddressCity { get; set; }
        public DateTime? LetterDate { get; set; }
        public string LetterDateString { get; set; }
        public IList<QueueStatus> QueueList { get; set; }
        public IList<ReferralQueueStatus_QuickUpdate> ReferralQueueList { get; set; }

        public StdDetailsQuickUpdateViewModel()
        {
            QueueList = new List<QueueStatus>();
            ReferralQueueList = new List<ReferralQueueStatus_QuickUpdate>();
        }

        public string Search { get; set; }
        public string SearchName { get; set; }
        
    }
    public class ReferralQueueStatus_QuickUpdate
    {
        public int QueueID { get; set; }
        public string QueueStatus { get; set; }
        public string QueueDate { get; set; }
        public int ReferralId { get; set; }
        public string QueueType { get; set; }
    }
}