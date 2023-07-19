using DataLayer;
using BuisinessLayer;
using ReferalDB.CommonClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReferalDB.Controllers;
using System.Web.Mvc;
using ReferalDBApplicant.Classes;

namespace ReferalDB.Models
{
    public class ContractViewModel
    {
        public clsSession session = null;
        public ClsCommon clsComm = null;
        public MelmarkDBEntities objData = null;
        public virtual string Comments { get; set; }
        public virtual string flag { get; set; }
        public int? StateId { get; set; }
        public string DistrictId { get; set; }
        public string Services { get; set; }
        public string ContactName { get; set; }
        public string ContactAddress { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Phonehidden { get; set; }
        public string Emailhidden { get; set; }
        public string CostShare { get; set; }
        public string NoOfHours { get; set; }
		public virtual string EndDate { get; set; }
        public virtual bool isSubmit { get; set; }
        public virtual string DocumentName { get; set; }
        public IList<SelectListItem> StateDetails { get; set; }
        public IList<SelectListItem> ServiceDetails { get; set; }
        public IList<SelectListItem> FundingSourceList { get; set; } //--- 22Sep2020 - List 3 - Task #2 ---//        
        public int? FundingSourceId { get; set; } //--- 22Sep2020 - List 3 - Task #2 ---//
        public virtual IList<DocumentDownloadViewModel> ContractLists { get; set; }
        public ContractViewModel()
        {
            //ContractLists = new List<Contract>();
            session = (clsSession)HttpContext.Current.Session["UserSession"];
            ContractLists = new List<DocumentDownloadViewModel>();
            ContractLists = GetConsents(session.ReferralId, session.SchoolId);
            StateDetails = new List<SelectListItem>();
            ServiceDetails = new List<SelectListItem>();
            IntakeAssessmentController intake = new IntakeAssessmentController();
            StateDetails = intake.FillDropStateList();
            ServiceDetails = intake.FillDropServices();
            FundingSourceList = intake.GetFundingList(session.SchoolId); //--- 22Sep2020 - List 3 - Task #2 ---//    
        }
        public IList<DocumentDownloadViewModel> GetConsents(int StudentId, int SchoolId)
        {
            int QueueStatus = 0;
            objData = new MelmarkDBEntities();
            clsComm = new ClsCommon();
            ref_Queue que = new ref_Queue();
            clsReferral objRef = new clsReferral(); //--- 02Oct2020 - List 3 - Task #2 ---//
            que = objData.ref_Queue.Where(obj => obj.QueueName == "Contract").SingleOrDefault();
            if (que != null)
            {
                //int QueueStatusId = 0;
                int QueueId = que.QueueId;
                string QueueType = que.QueueType;
              
                //QueueStatus = clsComm.insertQstatus(QueueType, "Y");
                QstatusDetails qDetails = clsComm.getQueueStatusId(StudentId, QueueType);
                QueueStatus = qDetails.QueueStatusId;
                isSubmit = false;
                if (QueueStatus == 0)
                {
                    QueueStatus = clsComm.getQueueStatusIdIfSubmitted(StudentId, QueueType);
                    qDetails.DraftStatus = "N";
                }
                //ref_QueueStatus tempQueue = new ref_QueueStatus();
                //tempQueue = objData.ref_QueueStatus.Where(obj => obj.StudentPersonalId == StudentId && obj.SchoolId == SchoolId && obj.QueueId == QueueId).SingleOrDefault();
                if (QueueStatus > 0)
                {
                    int QueueStatuss = clsComm.getQueueStatusIdIfSubmitted(StudentId, QueueType);
                    if (QueueStatuss > 0)
                    {
                        isSubmit = true;
                    }
                   
                    ref_ConsentMeeting tempPm = new ref_ConsentMeeting();
                    tempPm = objData.ref_ConsentMeeting.Where(obj => obj.StudentPersonalId == StudentId && obj.QueueStatusId == QueueStatus).SingleOrDefault();
                    if (tempPm != null)
                    {
                        flag = qDetails.DraftStatus;
                        Comments = tempPm.Comments;
                    }
                    try
                    {

                        //--- 02Oct2020 - List 3 - Task #2 -(Start)--//
                        int? LookUpID = objRef.GetFundingSourceID(session.ReferralId, session.SchoolId);
                        if (LookUpID != null)
                        {
                            FundingSourceId = LookUpID;
                        }
                        else
                        {
                            FundingSourceId = 0;
                        }
                        //--- 02Oct2020 - List 3 - Task #2 -(Start)--//
                    }
                    catch (Exception ex)
                    {
                        ClsErrorLog erorLog = new ClsErrorLog();
                        erorLog.WriteToLog(ex.ToString());
                    }

                    ref_ContractDetails objcont = new ref_ContractDetails();
                    objcont = objData.ref_ContractDetails.Where(obj => obj.StudentPersonalId == StudentId && obj.QueueStatusId == QueueStatus).SingleOrDefault();
                    if (objcont != null)
                    {
                        ContactName = objcont.ContactPersonName;
                        CostShare = objcont.CostShare;
                        ContactAddress = objcont.Address;
                        DistrictId = objcont.DistrictId;
                        if (Emailhidden == null)
                            Email = objcont.Email;
                        else
                            Email = Emailhidden;

                        Fax = objcont.Fax;

                        if (Phonehidden == null)
                            Phone = objcont.Phone;
                        else
                            Phone = Phonehidden;

                        Services = objcont.Services;
                        StateId = objcont.StateId;
                        NoOfHours = objcont.NoOfHours;
						EndDate = objcont.EndDate != null ? ((DateTime)objcont.EndDate).ToString("MM/dd/yyyy") : "" ;
                        EndDate = EndDate.Replace("-", "/");      
                    }
                }


            }




            ContractLists = (from objDoc in objData.Documents

                             join objLookup in objData.LookUps on objDoc.DocumentType equals objLookup.LookupId
                             where (objLookup.LookupType == "Document Type" && objLookup.LookupName == "Contract" && (objDoc.QueueStatusId == QueueStatus || objDoc.QueueStatusId == 0) && objDoc.Status == true && objDoc.StudentPersonalId == StudentId)
                             select new DocumentDownloadViewModel
                             {
                                 IEPId = objDoc.DocumentId,
                                 IEPName = objDoc.DocumentName,
                                 IEPPath = objDoc.DocumentPath,
                                 Verified = objDoc.Varified
                             }).OrderBy(t => t.IEPName).ToList();
            //if (ContractLists != null)
            //{
            //    foreach (var item in ContractLists)
            //    {
            //        var signedby = (from objDoc in objData.Documents
            //                        join objParent in objData.Parents on objDoc.SignedBy equals objParent.ParentID
            //                        where (objDoc.DocumentId == item.IEPId)
            //                        select new
            //                        {
            //                            SignedBy = objParent.Lname + " ," + objParent.Fname
            //                        }).SingleOrDefault();
            //        if (signedby != null) item.SignedBy = signedby.SignedBy;
            //        var signedon = (from objDoc in objData.Documents
            //                        join objParent in objData.Parents on objDoc.SignedBy equals objParent.ParentID
            //                        where (objDoc.DocumentId == item.IEPId)
            //                        select new
            //                        {
            //                            SignedOn = objDoc.SignedOn
            //                        }).SingleOrDefault();
            //        if (signedon != null) item.SignedOn = signedon.SignedOn;
            //    }
            //    return ContractLists;
            //}
            return ContractLists;
        }
        //public string Save(string Draft, int StudentId, int SchoolId, int UserId)
        //{
        //    string result = "";
        //    clsComm = new ClsCommon();
        //    objData = new MelmarkDBEntities();
        //    ref_Queue que = new ref_Queue();
        //    que = objData.ref_Queue.Where(obj => obj.QueueId==23).SingleOrDefault();
        //    if (que != null)
        //    {
        //        int QueueStatusId = 0;
        //        int QueueId = que.QueueId;
        //        string QueueType = que.QueueType;
        //        int qIdNext = clsComm.getQueueId("DC");
        //        int QProcess = clsComm.getProcessId();
        //        if (Draft == "Y")
        //            QueueStatusId = clsComm.insertQstatus(QueueType, Draft);
        //        else if (Draft == "N")
        //        {
        //            clsComm.insertNewStatus(QueueType, "DC", StudentId);
        //            var qStatusRow1 = objData.ref_QueueStatus.Where(x => x.StudentPersonalId == StudentId && x.QueueId == qIdNext && x.CurrentStatus == true && x.QueueProcess == QProcess).ToList();
        //            QueueStatusId = qStatusRow1.Count > 0 ? qStatusRow1[0].QueueStatusId : 0;
        //        }

        //        if (QueueStatusId > 0)
        //        {
        //            ref_ConsentMeeting tempPm = new ref_ConsentMeeting();
        //            tempPm = objData.ref_ConsentMeeting.Where(obj => obj.StudentPersonalId == StudentId && obj.QueueStatusId == QueueStatusId).SingleOrDefault();
        //            if (tempPm != null)
        //            {
        //                tempPm.Comments = Comments;
        //                tempPm.QueueId = 23;
        //                tempPm.ModifiedBy = UserId;
        //                tempPm.ModifiedOn = DateTime.Now;
        //                objData.SaveChanges();
        //            }
        //            else
        //            {
        //                ref_ConsentMeeting pm = new ref_ConsentMeeting();
        //                pm.QueueStatusId = QueueStatusId;
        //                pm.Comments = Comments;
        //                pm.QueueId = 23;
        //                pm.StudentPersonalId = StudentId;
        //                pm.CreatedBy = UserId;
        //                pm.CreatedOn = DateTime.Now;
        //                pm.ModifiedBy = UserId;
        //                pm.ModifiedOn = DateTime.Now;
        //                objData.ref_ConsentMeeting.Add(pm);
        //                objData.SaveChanges();
        //            }
        //        }
        //    }
        //    return result;
        //}

        public string Save(string Draft, int StudentId, int SchoolId, int UserId, IList<DocumentDownloadViewModel> ContractLists)
        {
            string result = clsGeneral.sucessMsg("Successfully Saved");
            try
            {
                //bool Result = true;
                //bool PhnResult = true;
                clsComm = new ClsCommon();
                objData = new MelmarkDBEntities();
                ref_Queue que = new ref_Queue();
                session = (clsSession)HttpContext.Current.Session["UserSession"];
                que = objData.ref_Queue.Where(obj => obj.QueueName == "Contract").SingleOrDefault();
                //if (Email != null)
                //{
                //    Result = clsGeneral.isValidEmail(Email);
                //}
                //if (Phone != null)
                //{
                //    PhnResult = clsGeneral.IsPhoneNumber(Phone);
                //}


                //if (PhnResult == true && Result == true)
                //{
                //    if (Result == true)
                //    {
                if (que != null)
                {
                    int QueueStatusId = 0;
                    int QueueId = que.QueueId;
                    string QueueType = que.QueueType;

                    QstatusDetails qDetails = clsComm.getQueueStatusId(StudentId, QueueType);
                    QueueStatusId = qDetails.QueueStatusId;

                    //--- 02Oct2020 - List 3 - Task #2 -(Start)--//
                    StudentPersonal stdprs = new StudentPersonal();
                    stdprs = objData.StudentPersonals.Where(obj => obj.StudentPersonalId == StudentId).SingleOrDefault();
                    if (stdprs != null)
                    {
                        stdprs.FundingSource = FundingSourceId;
                        objData.SaveChanges();
                    }
                    else
                    {
                        StudentPersonal stdprs2 = new StudentPersonal();
                        stdprs2.FundingSource = FundingSourceId;
                        objData.StudentPersonals.Add(stdprs2);
                        objData.SaveChanges();
                    }
                    //--- 02Oct2020 - List 3 - Task #2 -(End)--//


                    if (QueueStatusId > 0)
                    {
                        ref_ConsentMeeting tempPm = new ref_ConsentMeeting();
                        tempPm = objData.ref_ConsentMeeting.Where(obj => obj.StudentPersonalId == StudentId && obj.QueueStatusId == QueueStatusId).SingleOrDefault();
                        if (tempPm != null)
                        {
                            tempPm.Comments = Comments;
                            tempPm.ModifiedBy = UserId;
                            tempPm.ModifiedOn = DateTime.Now;
                            objData.SaveChanges();
                        }
                        else
                        {
                            ref_ConsentMeeting pm = new ref_ConsentMeeting();
                            pm.QueueStatusId = QueueStatusId;
                            pm.Comments = Comments;
                            pm.StudentPersonalId = StudentId;
                            pm.CreatedBy = UserId;
                            pm.CreatedOn = DateTime.Now;
                            pm.ModifiedBy = UserId;
                            pm.ModifiedOn = DateTime.Now;
                            objData.ref_ConsentMeeting.Add(pm);
                            objData.SaveChanges();
                        }

                        ref_ContractDetails objcon = new ref_ContractDetails();
                        objcon = objData.ref_ContractDetails.Where(obj => obj.StudentPersonalId == StudentId && obj.QueueStatusId == QueueStatusId).SingleOrDefault();
                        if (objcon != null)
                        {
                            objcon.ContactPersonName = ContactName;
                            objcon.CostShare = CostShare;
                            objcon.Address = ContactAddress;
                            objcon.DistrictId = DistrictId;
                            //if (Result == true)
                            objcon.Email = Email;
                            objcon.Fax = Fax;
                            //if (PhnResult == true)
                            objcon.Phone = Phone;
                            objcon.Services = Services;
                            objcon.StateId = StateId;
                            objcon.ModifiedBy = UserId;
                            objcon.ModifiedOn = DateTime.Now;
                            objcon.NoOfHours = NoOfHours;
                            if (EndDate != null)
                            {
                                objcon.EndDate = DateTime.ParseExact(EndDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            }
                            objData.SaveChanges();
                        }
                        else
                        {
                            ref_ContractDetails objcontract = new ref_ContractDetails();
                            objcontract.QueueStatusId = QueueStatusId;
                            objcontract.SchoolId = session.SchoolId;
                            objcontract.StudentPersonalId = session.ReferralId;
                            objcontract.ContactPersonName = ContactName;
                            objcontract.CostShare = CostShare;
                            objcontract.Address = ContactAddress;
                            objcontract.DistrictId = DistrictId;
                            objcontract.Email = Email;
                            objcontract.Fax = Fax;
                            objcontract.Phone = Phone;
                            objcontract.Services = Services;
                            objcontract.StateId = StateId;
                            objcontract.CreatedBy = UserId;
                            objcontract.CreatedOn = DateTime.Now;
                            objcontract.ModifiedBy = UserId;
                            objcontract.ModifiedOn = DateTime.Now;
                            objcontract.NoOfHours = NoOfHours;
                            if (EndDate != null)
                            {
                                objcontract.EndDate = DateTime.ParseExact(EndDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            }
                            objData.ref_ContractDetails.Add(objcontract);
                            objData.SaveChanges();
                        }
                       
                        if (ContractLists != null)
                        {
                            foreach (var item in ContractLists)
                            {
                                // Document tblDoc = new Document();
                                var tblDoc = objData.Documents.Where(obj => obj.Status == true && obj.DocumentId == item.IEPId).ToList();
                                tblDoc[0].Varified = item.Verified;
                                tblDoc[0].ModifiedBy = UserId;
                                tblDoc[0].ModifiedOn = DateTime.Now;
                                objData.SaveChanges();
                                var tblDocs = objData.binaryFiles.Where(obj => obj.SchoolId == session.SchoolId && obj.DocId == item.IEPId && obj.StudentId == session.ReferralId).ToList();
                                tblDocs[0].Varified = item.Verified;
                                objData.SaveChanges();
                            }
                        }

                        int qIdNext = clsComm.getQueueId("DC");
                        int QProcess = clsComm.getProcessId();
                        if (Draft == "Y")
                            QueueStatusId = clsComm.insertQstatus(QueueType, Draft);
                        else if (Draft == "N")
                        {
                            QueueStatusId = clsComm.insertQstatus(QueueType, Draft);
                            //clsComm.insertNewStatus(QueueType, "DC", StudentId);
                            var qStatusRow1 = objData.ref_QueueStatus.Where(x => x.StudentPersonalId == StudentId && x.QueueId == qIdNext && x.CurrentStatus == true && x.QueueProcess == QProcess).ToList();
                            //QueueStatusId = qStatusRow1.Count > 0 ? qStatusRow1[0].QueueStatusId : 0;
                        }


                    }
                    else result = clsGeneral.failedMsg("Saving Failed");
                }



            }
            catch (Exception ex)
            {
                result = clsGeneral.failedMsg("Saving Failed." + ex.Message);
            }
            return result;
        }

        public int FileUpload(int StudentId, int SchoolId, string DocName, string DocPath, int UserId)
        {
            int rtrnval = -1;
            objData = new MelmarkDBEntities();
            LookUp lookup = new LookUp();
            ClsCommon getcheck = new ClsCommon();
            QstatusDetails qDetails = getcheck.getQueueStatusId(session.ReferralId, "CT");
            int QstatusId = qDetails.QueueStatusId;
            if (QstatusId == 0)
            {
                QstatusId = getcheck.getQueueStatusIdIfSubmitted(session.ReferralId, "CT");

            }

            lookup = objData.LookUps.Where(obj => obj.LookupType == "Document Type" && obj.LookupName == "Contract").SingleOrDefault();
            if (lookup != null)
            {
                Document tblDoc = new Document();
                tblDoc.DocumentName = DocName;
                tblDoc.DocumentType = lookup.LookupId;
                tblDoc.DocumentPath = DocPath;
                tblDoc.SchoolId = SchoolId;
                tblDoc.QueueStatusId = QstatusId;
                tblDoc.StudentPersonalId = StudentId;
                tblDoc.Status = true;
                tblDoc.UserType = "Staff";
                tblDoc.CreatedBy = UserId;
                tblDoc.CreatedOn = DateTime.Now;
                objData.Documents.Add(tblDoc);
                objData.SaveChanges();
                rtrnval = tblDoc.DocumentId;
            }
            return rtrnval;
        }
        public string DownloadDoc(string documentId, int StudentId)
        {
            objData = new MelmarkDBEntities();
            Document ObjDoc = new Document();
            string dirpath = (AppDomain.CurrentDomain.BaseDirectory + "Files/Documents".ToString()).Replace('\\', '/');
            dirpath = @"D:\SavedDocs\" + documentId;

            //ObjDoc = objData.Documents.Where(objDocument => objDocument.DocumentId == documentId && objDocument.StudentPersonalId == StudentId).SingleOrDefault();
            //if (ObjDoc != null)
            //{
            //    var documentPath = dirpath + ObjDoc.DocumentId + "-" + ObjDoc.DocumentPath;
            //    return documentPath.Replace('\\', '/');
            //}

            return dirpath;
        }

    }
    //public class Contract
    //{
    //    public virtual string ContractName { get; set; }
    //    public virtual string SignedBy { get; set; }
    //    public virtual DateTime? SignedOn { get; set; }
    //    public virtual int ContractId { get; set; }
    //    public virtual string ContractPath { get; set; }
    //}
}