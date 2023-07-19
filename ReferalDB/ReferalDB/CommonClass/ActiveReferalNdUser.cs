using ReferalDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReferalDB.CommonClass
{
    public class ActiveReferalNdUser
    {
        public string ReferralName;
        public string QueueName;
        public string UserName;
        public string ImageUrl;
        public string Gender;
        public string AssignDate;
        public string CheckListName;
        public string QueueId;
        public string ReferralId;
    }
    public class StudentSearchDetails
    {
        public virtual PagingModel pageModel { get; set; }
        public int ReferralId { get; set; }
        public string QueueId { get; set; }
        public string ReferralName { get; set; }
        public string Gender { get; set; }
        public string AdmissionDate { get; set; }
        public string BirthDate { get; set; }
        public string LastQueue { get; set; }
        public bool? FundingVerification { get; set; }
        public bool WaitingList { get; set; }
        public bool InactiveList { get; set; }
        public string StudentType { get; set; }
        public string ReferralName_short { get; set; }
    }

    public class StaffSearchDetails
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
    }

    public class ContactNameSearchDetails {

        public int ContactId { get; set; }
        public string ContactName { get; set; }
    }
}