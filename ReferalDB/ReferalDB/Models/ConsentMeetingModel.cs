using BuisinessLayer;
using DataLayer;
using ReferalDB.CommonClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReferalDB.Models
{
    public class ConsentMeetingModel
    {
        public clsSession session = null;
        public ClsCommon clsComm = null;
        public MelmarkDBEntities objData = null;
        public virtual string Comments { get; set; }
        public virtual string flag { get; set; }
        public virtual bool isSubmit { get; set; }
        public virtual string DocumentName { get; set; }
        //public virtual IEnumerable<ConsentForms> ConsentLists { get; set; }
        public virtual IList<DocumentDownloadViewModel> ConsentLists { get; set; }
        public ConsentMeetingModel()
        {
            //ConsentLists = new List<ConsentForms>();
            session = (clsSession)HttpContext.Current.Session["UserSession"];
            ConsentLists = new List<DocumentDownloadViewModel>();
            ConsentLists = GetConsents(session.ReferralId, session.SchoolId);
        }
        public IList<DocumentDownloadViewModel> GetConsents(int StudentId, int SchoolId)
        {
            clsComm = new ClsCommon();
            objData = new MelmarkDBEntities();
            ref_Queue que = new ref_Queue();
            int QueueStatus = 0;
            que = objData.ref_Queue.Where(obj => obj.QueueName == "Consent Meeting").SingleOrDefault();
            if (que != null)
            {
                //int QueueStatusId = 0;
                int QueueId = que.QueueId;
                string QueueType = que.QueueType;

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





            ConsentLists = (from objDoc in objData.Documents
                            join objLookup in objData.LookUps on objDoc.DocumentType equals objLookup.LookupId
                            join objBinary in objData.binaryFiles on objDoc.DocumentId equals objBinary.DocId
                            where (objLookup.LookupType == "Document Type" && objLookup.LookupName == "Consent" && (objDoc.QueueStatusId == QueueStatus || objDoc.QueueStatusId == 0) && objDoc.Status == true && objDoc.StudentPersonalId == StudentId)
                            select new DocumentDownloadViewModel
                            {
                                IEPId = (int)objBinary.BinaryId,
                                IEPName = objDoc.DocumentName,
                                IEPPath = objDoc.DocumentPath,
                                Verified = objDoc.Varified
                            }).OrderBy(t => t.IEPName).ToList();

            return ConsentLists;
        }

        public string Save(string Draft, int StudentId, int SchoolId, int UserId, IList<DocumentDownloadViewModel> ConsentLists)
        {
            string result = clsGeneral.sucessMsg("Successfully Saved");
            try
            {
                clsComm = new ClsCommon();
                objData = new MelmarkDBEntities();
                ref_Queue que = new ref_Queue();
                que = objData.ref_Queue.Where(obj => obj.QueueName == "Consent Meeting").SingleOrDefault();
                if (que != null)
                {
                    int QueueStatusId = 0;
                    int QueueId = que.QueueId;
                    string QueueType = que.QueueType;

                    QstatusDetails qDetails = clsComm.getQueueStatusId(StudentId, QueueType);
                    QueueStatusId = qDetails.QueueStatusId;



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

                        if (ConsentLists != null)
                        {
                            foreach (var item in ConsentLists)
                            {
                                //Document tblDoc = new Document();
                                var docId = objData.binaryFiles.Where(x => x.BinaryId == item.IEPId).ToList();
                                if (docId.Count > 0)
                                {
                                    int docIds = (int)docId[0].DocId;
                                    var tblDoc = objData.Documents.Where(obj => obj.Status == true && obj.DocumentId == docIds).ToList();
                                    tblDoc[0].Varified = item.Verified;
                                    tblDoc[0].ModifiedBy = UserId;
                                    tblDoc[0].ModifiedOn = DateTime.Now;
                                    objData.SaveChanges();
                                    var tblDocs = objData.binaryFiles.Where(obj => obj.SchoolId == session.SchoolId && obj.DocId == docIds && obj.StudentId == session.ReferralId).ToList();
                                    tblDocs[0].Varified = item.Verified;
                                    objData.SaveChanges();
                                }
                            }
                        }

                        int qIdNext = clsComm.getQueueId("CT");
                        int QProcess = clsComm.getProcessId();
                        if (Draft == "Y")
                            QueueStatusId = clsComm.insertQstatus(QueueType, Draft);
                        else if (Draft == "N")
                        {
                            clsComm.insertNewStatus(QueueType, "CT", StudentId);
                            var qStatusRow1 = objData.ref_QueueStatus.Where(x => x.StudentPersonalId == StudentId && x.QueueId == qIdNext && x.CurrentStatus == true && x.QueueProcess == QProcess).ToList();
                            QueueStatusId = qStatusRow1.Count > 0 ? qStatusRow1[0].QueueStatusId : 0;
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
            LookUp lookup = new LookUp();

            ClsCommon getcheck = new ClsCommon();
            QstatusDetails qDetails = getcheck.getQueueStatusId(session.ReferralId, "CM");
            int QstatusId = qDetails.QueueStatusId;
            if (QstatusId == 0)
            {
                QstatusId = getcheck.getQueueStatusIdIfSubmitted(session.ReferralId, "CM");

            }
            lookup = objData.LookUps.Where(obj => obj.LookupType == "Document Type" && obj.LookupName == "Consent").SingleOrDefault();
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
    //public class ConsentForms
    //{
    //    public virtual string ConsentName { get; set; }
    //    public virtual string SignedBy { get; set; }
    //    public virtual DateTime? SignedOn { get; set; }
    //    public virtual int ConsentId { get; set; }
    //    public virtual string ConsentPath { get; set; }
    //}
}