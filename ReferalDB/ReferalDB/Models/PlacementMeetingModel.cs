using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer;
using BuisinessLayer;
using ReferalDB.CommonClass;

namespace ReferalDB.Models
{
    public class PlacementMeetingModel
    {
        public clsSession session = null;
        public ClsCommon clsComm = null;
        public MelmarkDBEntities objData = null;
        public virtual DateTime? StartDate { get; set; }
        public virtual int? Ratio1 { get; set; }
        public virtual int? Ratio2 { get; set; }
        public virtual string Comments { get; set; }
        public virtual string GetDate { get; set; }
        public virtual string flag { get; set; }

        public virtual string DocumentName { get; set; }
        //public virtual IEnumerable<Agreements> AgreementLists { get; set; }
        public virtual IList<DocumentDownloadViewModel> AgreementLists { get; set; }

        public PlacementMeetingModel()
        {
            //AgreementLists = new List<Agreements>();
            session = (clsSession)HttpContext.Current.Session["UserSession"];
            AgreementLists = new List<DocumentDownloadViewModel>();
            AgreementLists = GetAgreements(session.ReferralId, session.SchoolId);
        }

        public IList<DocumentDownloadViewModel> GetAgreements(int StudentId, int SchoolId)
        {
            clsComm = new ClsCommon();
            objData = new MelmarkDBEntities();
            ref_Queue que = new ref_Queue();
            int QueueStatus = 0;
            que = objData.ref_Queue.Where(obj => obj.QueueName == "Placement Meeting").SingleOrDefault();
            if (que != null)
            {
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
                    ref_PlacementMeeting tempPm = new ref_PlacementMeeting();
                    tempPm = objData.ref_PlacementMeeting.Where(obj => obj.StudentPersonalId == StudentId && obj.QueueStatusId == QueueStatus).SingleOrDefault();
                    if (tempPm != null)
                    {
                        flag = qDetails.DraftStatus;
                        Comments = tempPm.Comments;
                        Ratio1 = tempPm.RatioNumerator;
                        Ratio2 = tempPm.RatioDenominator;
                        StartDate = tempPm.StartDate;
                        if (tempPm.StartDate != null) GetDate = tempPm.StartDate.Value.ToShortDateString();
                    }
                }
            }

            //AgreementLists = (from objDoc in objData.binaryFiles
            //                  //  join objStatus in objData.IEPExportStatus on objDoc.DocumentId equals objStatus.BinaryId
            //                  where (objDoc.type == "Referal" && objDoc.ModuleName == "PlacementMeeting" && objDoc.SchoolId == SchoolId && objDoc.StudentId == StudentId)
            //                  select new DocumentDownloadViewModel
            //                  {
            //                      IEPId = objDoc.BinaryId,
            //                      IEPName = objDoc.DocumentName,
            //                      IEPPath = "",
            //                      Verified = objDoc.Varified,
            //                  }).OrderBy(t => t.IEPName).ToList();

            //return AgreementLists;


            AgreementLists = (from objDoc in objData.Documents                             
                              join objLookup in objData.LookUps on objDoc.DocumentType equals objLookup.LookupId
                              where (objLookup.LookupType == "Document Type" && objLookup.LookupName == "Placement Agreement" && (objDoc.QueueStatusId == QueueStatus || objDoc.QueueStatusId == 0) && objDoc.Status == true && objDoc.StudentPersonalId == StudentId)
                              select new DocumentDownloadViewModel
                              {
                                  IEPId = objDoc.DocumentId,
                                  IEPName = objDoc.DocumentName,
                                  IEPPath = objDoc.DocumentPath,
                                  Verified = objDoc.Varified
                              }).OrderBy(t => t.IEPName).ToList();


            return AgreementLists;
            //if (AgreementLists != null)
            //{
            //    foreach (var item in AgreementLists)
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
            //    return AgreementLists;
            //}
            //return AgreementLists;
        }

        public int FileUpload(int StudentId, int SchoolId, string DocName, string DocPath, int UserId)
        {
            int rtrnval = -1;
            objData = new MelmarkDBEntities();
            LookUp lookup = new LookUp();

            ClsCommon getcheck = new ClsCommon();
            QstatusDetails qDetails = getcheck.getQueueStatusId(session.ReferralId, "PM");
            int QstatusId = qDetails.QueueStatusId;
            if (QstatusId == 0)
            {
                QstatusId = getcheck.getQueueStatusIdIfSubmitted(session.ReferralId, "PM");

            }


            lookup = objData.LookUps.Where(obj => obj.LookupType == "Document Type" && obj.LookupName == "Placement Agreement").SingleOrDefault();
            if (lookup != null)
            {
                Document tblDoc = new Document();
                tblDoc.DocumentName = DocName;
                tblDoc.DocumentType = lookup.LookupId;
                tblDoc.DocumentPath = DocPath;
                tblDoc.SchoolId = SchoolId;
                tblDoc.StudentPersonalId = StudentId;
                tblDoc.Status = true;
                tblDoc.QueueStatusId = QstatusId;
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
        public string Save(string Draft, int StudentId, int SchoolId, int UserId, IList<DocumentDownloadViewModel> AgreementLists)
        {
            string result = clsGeneral.sucessMsg("Successfully Saved");
            try
            {
                clsComm = new ClsCommon();
                objData = new MelmarkDBEntities();
                ref_Queue que = new ref_Queue();
                que = objData.ref_Queue.Where(obj => obj.QueueName == "Placement Meeting").SingleOrDefault();
                if (que != null)
                {
                    int QueueStatusId = 0;
                    int QueueId = que.QueueId;
                    string QueueType = que.QueueType;

                    QstatusDetails qDetails = clsComm.getQueueStatusId(StudentId, QueueType);
                    QueueStatusId = qDetails.QueueStatusId;


                    if (QueueStatusId > 0)
                    {
                        ref_PlacementMeeting tempPm = new ref_PlacementMeeting();
                        tempPm = objData.ref_PlacementMeeting.Where(obj => obj.StudentPersonalId == StudentId && obj.QueueStatusId == QueueStatusId).SingleOrDefault();
                        if (tempPm != null)
                        {
                            tempPm.RatioNumerator = Ratio1;
                            tempPm.RatioDenominator = Ratio2;
                            tempPm.Comments = Comments;
                            tempPm.StartDate = StartDate;
                            tempPm.ModifiedBy = UserId;
                            tempPm.ModifiedOn = DateTime.Now;
                            objData.SaveChanges();
                        }
                        else
                        {
                            ref_PlacementMeeting pm = new ref_PlacementMeeting();
                            pm.QueueStatusId = QueueStatusId;
                            pm.RatioNumerator = Ratio1;
                            pm.RatioDenominator = Ratio2;
                            pm.Comments = Comments;
                            pm.StudentPersonalId = StudentId;
                            pm.StartDate = StartDate;
                            pm.CreatedBy = UserId;
                            pm.CreatedOn = DateTime.Now;
                            pm.ModifiedBy = UserId;
                            pm.ModifiedOn = DateTime.Now;
                            objData.ref_PlacementMeeting.Add(pm);
                            objData.SaveChanges();
                        }

                        if (AgreementLists != null)
                        {
                            foreach (var item in AgreementLists)
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

                        int qIdNext = clsComm.getQueueId("CM");
                        int QProcess = clsComm.getProcessId();
                        if (Draft == "Y")
                            QueueStatusId = clsComm.insertQstatus(QueueType, Draft);
                        else if (Draft == "N")
                        {
                            clsComm.insertNewStatus(QueueType, "CM", StudentId);
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
    }

    //public class Agreements
    //{
    //    public virtual string AgreementName { get; set; }
    //    public virtual string SignedBy { get; set; }
    //    public virtual DateTime? SignedOn { get; set; }
    //    public virtual int AgreementId { get; set; }
    //    public virtual string AgreementPath { get; set; }
    //}
}