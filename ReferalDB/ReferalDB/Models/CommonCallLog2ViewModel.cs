using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReferalDB.Models;
using DataLayer;
using System.Data.Objects.SqlClient;
using BuisinessLayer;
using ReferalDB.CommonClass;
using System.IO;

namespace ReferalDB.Models
{
    public class CommonCallLog2ViewModel
    {
        public virtual int CallLogId2 { get; set; }
        public int academicReviewId2 { get; set; }
        public int StudentId2 { get; set; }
        public int SchoolId2 { get; set; }
        public virtual IList<SelectListItem> RelationshipList { get; set; }
        public string Relationship2 { get; set; }
        public string ReferralName2 { get; set; }
        public string StaffName2 { get; set; }
        public DateTime? CallTime2 { get; set; }
        public string CallDateShow2 { get; set; }
        public string CallTimeShow2{ get; set; }
        public string NameOfContact2 { get; set; }
        public DateTime? AppntTime2{ get; set; }
        public string AppntDateShow2 { get; set; }
        public string AppntTimeShow2 { get; set; }
        public string Conversation2 { get; set; }
        public string type2{ get; set; }
        public string Draft2 { get; set; }
        public bool IsPresent2 { get; set; }
        public bool IsSubmit2 { get; set; }
        public IEnumerable<SelectListItem> ContactlogTypeList { get; set; }
        public string ContactlogType { get; set; }
        
        public CommonCallLog2ViewModel()
        {
            RelationshipList = new List<SelectListItem>();
            ContactlogTypeList = FillContactLog();
           
        }
       // GlobalData MetaData = new GlobalData();
        public virtual IList<CallList> CallLists { get; set; }
        public static clsSession sess = null;
       // int page,int pageSize
        
        
       public IList<CallList> getcallDetails(int callLogId)
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

            retunmodel = (from call in objData.ref_CallLogs
                          where (call.StudentId == sess.ReferralId && call.SchoolId == sess.SchoolId && call.CallLogId == callLogId )
                          select new CallList
                          {
                              CallLogId = call.CallLogId,
                              ContactName = call.Nameofcontact,
                              Relationship = "",
                              Calltime = call.CallTime,
                              Callflag = call.CallFlag,
                              Conversation = call.Conversation

                          }).ToList();
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
        //public CallLogModel()
        //{
        //    CallLists = new List<CallList>();
        //    pageModel = new PagingModel();
        //}

       public IEnumerable<SelectListItem> FillContactLog()
       {
           List<SelectListItem> obj = new List<SelectListItem>();
           MelmarkDBEntities objData = new MelmarkDBEntities();
           var lookup = objData.LookUps.Where(x => x.LookupType == "Calllog Type").ToList();
           foreach (var lookupdata in lookup)
           {
               obj.Add(new SelectListItem { Text = lookupdata.LookupName, Value = lookupdata.LookupId.ToString() });
           }
           return obj;
       }
    

    public class CallList
    {
        
        public virtual int CallLogId { get; set; }
        public virtual string ContactName { get; set; }
        public virtual string Relationship { get; set; }
        public virtual DateTime? Calltime { get; set; }
        public virtual int? Callflag { get; set; }
        public virtual string Conversation { get; set; }
    }
    }
}