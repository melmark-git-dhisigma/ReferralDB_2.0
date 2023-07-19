using DataLayer;
using BuisinessLayer;
using ReferalDB.CommonClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReferalDB.Models
{
    public class IEPsignoffModel
    {
        public clsSession session = null;
        public ClsCommon clsComm = null;
        public MelmarkDBEntities objData = null;
        public virtual string Comments { get; set; }
        public virtual string flag { get; set; }
        public virtual bool isSubmit { get; set; }
        public virtual string DocumentName { get; set; }
        public virtual IList<DocumentDownloadViewModel> IEPLists { get; set; }
        //public virtual IEnumerable<IEPsignoff> IEPLists { get; set; }
        public IEPsignoffModel()
        {
            //IEPLists = new List<IEPsignoff>();
            session = (clsSession)HttpContext.Current.Session["UserSession"];
            IEPLists = new List<DocumentDownloadViewModel>();
            IEPLists = GetConsents(session.ReferralId, session.SchoolId);
        }
        public IList<DocumentDownloadViewModel> GetConsents(int StudentId, int SchoolId)
        {
            objData = new MelmarkDBEntities();
            clsComm = new ClsCommon();
            ref_Queue que = new ref_Queue();
            int QueueStatus = 0;

            que = objData.ref_Queue.Where(obj => obj.QueueName == "PreAdmission/IEP signoff").SingleOrDefault();
            if (que != null)
            {
                //int QueueStatusId = 0;
                int QueueId = que.QueueId;
                string QueueType = que.QueueType;
                isSubmit = false;
                //QueueStatus = clsComm.insertQstatus(QueueType, "Y");
                QstatusDetails qDetails = clsComm.getQueueStatusId(StudentId, QueueType);
                QueueStatus = qDetails.QueueStatusId;
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
                }
            }

            // MelmarkDBEntities objData = new MelmarkDBEntities();

            //IEPLists = (from objDoc in objData.binaryFiles
            //            //  join objStatus in objData.IEPExportStatus on objDoc.DocumentId equals objStatus.BinaryId
            //            where (objDoc.type == "Referal" && objDoc.ModuleName == "IEPsignoff" && objDoc.SchoolId == SchoolId && objDoc.StudentId == StudentId)
            //            select new DocumentDownloadViewModel
            //            {
            //                IEPId = objDoc.BinaryId,
            //                IEPName = objDoc.DocumentName,
            //                IEPPath = "",
            //                Verified = objDoc.Varified
            //            }).OrderBy(t => t.IEPName).ToList();



            //return IEPLists;


            IEPLists = (from objDoc in objData.Documents
                  
                        join objLookup in objData.LookUps on objDoc.DocumentType equals objLookup.LookupId
                        where (objLookup.LookupType == "Document Type" && objLookup.LookupName == "IEP" && (objDoc.QueueStatusId == QueueStatus || objDoc.QueueStatusId == 0) && objDoc.Status == true && objDoc.StudentPersonalId == StudentId)
                        select new DocumentDownloadViewModel
                        {
                            IEPId = objDoc.DocumentId,
                            IEPName = objDoc.DocumentName,
                            IEPPath = objDoc.DocumentPath,
                            Verified = objDoc.Varified
                        }).OrderBy(t => t.IEPName).ToList();
            //if (IEPLists != null)
            //{
            //    foreach (var item in IEPLists)
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
            //    return IEPLists;
            //}
            return IEPLists;
        }
        //public string Save(string Draft, int StudentId, int SchoolId, int UserId)
        //{
        //    string result = "";
        //    clsComm = new ClsCommon();
        //    objData = new MelmarkDBEntities();
        //    ref_Queue que = new ref_Queue();
        //    que = objData.ref_Queue.Where(obj => obj.QueueId==20).SingleOrDefault();
        //    if (que != null)
        //    {
        //        int QueueStatusId = 0;
        //        int QueueId = que.QueueId;
        //        string QueueType = que.QueueType;
        //        int qIdNext = clsComm.getQueueId("PM");
        //        int QProcess = clsComm.getProcessId();
        //        if (Draft == "Y")
        //            QueueStatusId = clsComm.insertQstatus(QueueType, Draft);
        //        else if (Draft == "N")
        //        {
        //            clsComm.insertNewStatus(QueueType, "PM", StudentId);
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
        //                tempPm.QueueId = 20;
        //                tempPm.ModifiedBy = UserId;
        //                tempPm.ModifiedOn = DateTime.Now;
        //                objData.SaveChanges();
        //            }
        //            else
        //            {
        //                ref_ConsentMeeting pm = new ref_ConsentMeeting();
        //                pm.QueueStatusId = QueueStatusId;
        //                pm.Comments = Comments;
        //                pm.QueueId = 20;
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
        public string Save(string Draft, int StudentId, int SchoolId, int UserId, IList<DocumentDownloadViewModel> IEPLists)
        {
            string result = clsGeneral.sucessMsg("Successfully Saved");
            try
            {
                clsComm = new ClsCommon();
                objData = new MelmarkDBEntities();
                ref_Queue que = new ref_Queue();
                que = objData.ref_Queue.Where(obj => obj.QueueType == "IP").SingleOrDefault();
                if (que != null)
                {
                    int QueueStatusId = 0;
                    int QueueId = que.QueueId;
                    string QueueType = que.QueueType;

                    QstatusDetails qDetails = clsComm.getQueueStatusId(StudentId, QueueType);
                    QueueStatusId = qDetails.QueueStatusId;
                    //int processId = clsComm.getProcessId();
                    //ref_QueueStatus tempQueue = new ref_QueueStatus();
                    //tempQueue = objData.ref_QueueStatus.Where(obj => obj.StudentPersonalId == StudentId && obj.SchoolId == SchoolId && obj.QueueId == QueueId && obj.QueueProcess == processId).SingleOrDefault();


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

                        //IList< Document> DocType=new List<Document>();
                        // var LookupType=objData.LookUps.Where(obj => obj.LookupType=="Document Type" && obj.LookupName== "IEP").ToList();
                        // if(LookupType.Count>0)
                        //     DocType=objData.Documents.Where(obj =>obj.Status == true && obj.StudentPersonalId == StudentId && obj.DocumentType==LookupType[0].LookupId).ToList();
                        // if (DocType.Count>0)
                        // {

                        // }
                        //GetConsents(StudentId,SchoolId);
                        if (IEPLists != null)
                        {
                            foreach (var item in IEPLists)
                            {
                                // Document tblDoc = new Document();
                                var tblDoc = objData.Documents.Where(obj => obj.Status == true && obj.DocumentId == item.IEPId).ToList();
                                if (tblDoc.Count > 0)
                                {
                                    tblDoc[0].Varified = item.Verified;
                                    tblDoc[0].ModifiedBy = UserId;
                                    tblDoc[0].ModifiedOn = DateTime.Now;
                                    objData.SaveChanges();
                                    var tblDocs = objData.binaryFiles.Where(obj => obj.SchoolId == session.SchoolId && obj.DocId == item.IEPId && obj.StudentId == session.ReferralId).ToList();
                                    tblDocs[0].Varified = item.Verified;
                                    objData.SaveChanges();
                                }
                            }
                        }



                        int qIdNext = clsComm.getQueueId("PM");
                        int QProcess = clsComm.getProcessId();
                        if (Draft == "Y")
                            QueueStatusId = clsComm.insertQstatus(QueueType, Draft);
                        else if (Draft == "N")
                        {
                            QueueStatusId = clsComm.insertQstatus(QueueType, Draft);
                            //clsComm.insertNewStatus(QueueType, "PM", StudentId);
                            var qStatusRow1 = objData.ref_QueueStatus.Where(x => x.StudentPersonalId == StudentId && x.QueueId == qIdNext && x.CurrentStatus == true && x.QueueProcess == QProcess).ToList();
                            //QueueStatusId = qStatusRow1.Count > 0 ? qStatusRow1[0].QueueStatusId : 0;
                        }
                    }
                    else clsGeneral.failedMsg("Saving Failed");
                }
                else clsGeneral.failedMsg("Saving Failed");
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
            ClsCommon getcheck = new ClsCommon();
            QstatusDetails qDetails = getcheck.getQueueStatusId(session.ReferralId, "IP");
            int QstatusId = qDetails.QueueStatusId;
            if (QstatusId == 0)
            {
                QstatusId = getcheck.getQueueStatusIdIfSubmitted(session.ReferralId, "IP");

            }
            LookUp lookup = new LookUp();
            lookup = objData.LookUps.Where(obj => obj.LookupType == "Document Type" && obj.LookupName == "IEP").SingleOrDefault();
            if (lookup != null)
            {
                Document tblDoc = new Document();
                tblDoc.DocumentName = DocName;
                tblDoc.DocumentType = lookup.LookupId;
                tblDoc.DocumentPath = DocPath;
                tblDoc.SchoolId = SchoolId;
                tblDoc.StudentPersonalId = StudentId;
                tblDoc.QueueStatusId = QstatusId;
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

            ////ObjDoc = objData.Documents.Where(objDocument => objDocument.DocumentId == documentId && objDocument.StudentPersonalId == StudentId).SingleOrDefault();
            ////if (ObjDoc != null)
            ////{
            ////    var documentPath = dirpath + ObjDoc.DocumentId + "-" + ObjDoc.DocumentPath;
            ////    return documentPath.Replace('\\', '/');
            ////}

            return dirpath;
        }

    }
    //public class IEPsignoff
    //{
    //    public virtual string IEPName { get; set; }
    //    public virtual bool? Verified { get; set; }
    //    public virtual string SignedBy { get; set; }
    //    public virtual DateTime? SignedOn { get; set; }
    //    public virtual int IEPId { get; set; }
    //    public virtual string IEPPath { get; set; }
    //}
}