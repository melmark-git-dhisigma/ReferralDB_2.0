using DataLayer;
using ReferalDBApplicant.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace ReferalDB.Models
{
    public class StdDetailsViewModel
    {
        public int StudentPersonalId { get; set; }
        public string studentPersonalName { get; set; }
        public string Gender { get; set; }
        public string BirthDate { get; set; }
        public string AdmissionDate { get; set; }
        public string ImageUrl { get; set; }
        public string PlaceOfBirth { get; set; }
        public string ApplicationDate { get; set; }
        public string StateOfBirth { get; set; }
        public string currentQueue { get; set; }
        public bool FVqueueStatus { get; set; }
        public string CountryOfBirth { get; set; }
        public string CountryOfCitizenship { get; set; }
        public string MaritalStatus { get; set; }
        public string PrimaryLanguage { get; set; }
        public string Height { get; set; }
        public string Weight { get; set; }
        public string Address { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Apartment { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public int? State { get; set; }
        public DateTime? LetterDate { get; set; }
        public string LetterDateString { get; set; }
        public string GenderNum { get; set; }
        public string Status { get; set; }
        public string Diagnosis { get; set; }
        public IList<QueueStatus> QueueList { get; set; }
        public IList<ReferralQueueStatus> ReferralQueueList { get; set; }
        public IEnumerable<SelectListItem> StateList { get; set; }
        public IEnumerable<SelectListItem> GenderList { get; set; }
        public IList<CommonCallLog2ViewModel> CallLog { get; set; }
        public virtual IList<CallList> CallLists { get; set; }
        public bool? fl_FA { get; set; }
        public bool fl_WL { get; set; }
        public bool fl_AT { get; set; }
        public IList<SelectListItem> FundingSourceList { get; set; } //--- 22Sep2020 - List 3 - Task #2 ---//        
        public int? FundingSourceId { get; set; } //--- 22Sep2020 - List 3 - Task #2 ---//

        public StdDetailsViewModel()
        {
            QueueList = new List<QueueStatus>();
            ReferralQueueList = new List<ReferralQueueStatus>();
            clsReferral clsRf = new clsReferral();
            StateList = clsRf.FillState(1);
            GenderList = clsRf.FillGender();
            CallLog = new List<CommonCallLog2ViewModel>();
            FundingSourceList = clsRf.GetFundingList(); //--- 22Sep2020 - List 3 - Task #2 ---//    
        }

        public string Search { get; set; }
        public string SearchName { get; set; }
        public static clsSession sess = null;

        public IList<CallList> getCallLog()
        {
            sess = (clsSession)HttpContext.Current.Session["UserSession"];
            CommonCallLogViewModel listModel = new CommonCallLogViewModel();
            MelmarkDBEntities objData = new MelmarkDBEntities();
            // listModel.pageModel.CurrentPageIndex = page;
            // listModel.pageModel.PageSize = pageSize;
            DateTime now = DateTime.Now;
            IList<CallList> retunmodel = new List<CallList>();
            //try
            //{
            var calllogmodel = objData.ref_CallLogs.Where(x => x.StudentId == sess.ReferralId && x.SchoolId == sess.SchoolId).ToList();
            var lookupModel = objData.LookUps.Where(x => x.LookupType == "Calllog Type").ToList();


            retunmodel = (from call in calllogmodel
                          join lookups in lookupModel on call.CallFlag equals lookups.LookupId into callLookup
                          from callloglookup in callLookup.DefaultIfEmpty()
                          select new CallList
                          {
                              CallLogId = call.CallLogId,
                              ContactName = call.Nameofcontact,
                              Relationship = "",
                              Calltime = call.CallTime,
                              Conversation = call.Conversation,
                              Staffname = call.StaffName,
                              Callflag = (callloglookup == null ? String.Empty : callloglookup.LookupName)

                          }).OrderByDescending(x => x.CallLogId).ToList();
            
            
            if (retunmodel != null)
            {
                foreach (var item in retunmodel)
                {
                    var RelationId = objData.ref_CallLogs.Where(objref => objref.StudentId == sess.ReferralId && objref.SchoolId == sess.SchoolId && objref.CallLogId == item.CallLogId)
                                .Select(objref => objref.RelationshipId).Single();

                    if (RelationId > 0)
                    {

                        var data = (from call in objData.ref_CallLogs
                                    join lukup in objData.LookUps on RelationId equals lukup.LookupId
                                    where (call.StudentId == sess.ReferralId && call.CallLogId == item.CallLogId)
                                    select new CallList
                                    {
                                        Relationship = lukup.LookupName,
                                    }).SingleOrDefault();
                        item.Relationship = data.Relationship;

                    }


                }
            }






           
            return retunmodel;


        }

    }


    public class ReferralQueueStatus
    {
        public int QueueID { get; set; }
        public string QueueStatus { get; set; }
        public string QueueDate { get; set; }
        public int ReferralId { get; set; }
        public string QueueType { get; set; }
    }

    public class CallList
    {

        public virtual int CallLogId { get; set; }
        public virtual string ContactName { get; set; }
        public virtual string Relationship { get; set; }
        public virtual DateTime? Calltime { get; set; }        
        public virtual string Callflag { get; set; }
        public virtual string Conversation { get; set; }
        public virtual string Staffname { get; set; }
    }

}