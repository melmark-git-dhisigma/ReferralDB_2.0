using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReferalDB.CommonClass;
using BuisinessLayer;
using System.Globalization;

namespace ReferalDB.Models
{
    public class ListIntakeAssessment
    {
        public clsSession sess = null;
        ClsCommon clsComm = null;
        public MelmarkDBEntities objData = null;
        public virtual IList<Assessments> ListAssessments { get; set; }
        public virtual string Comments { get; set; }
        public virtual bool Approved { get; set; }
        public virtual bool isSubmit { get; set; }
        public ListIntakeAssessment()
        {
            ListAssessments = new List<Assessments>();
        }
        public void GetCheckLists(int StudentId, int SchoolId)
        {
            clsComm = new ClsCommon();
            objData = new MelmarkDBEntities();
            ref_Queue que = new ref_Queue();
            que = objData.ref_Queue.Where(obj => obj.QueueType == "IE").SingleOrDefault();

            if (que != null)
            {
                int QueueId = que.QueueId;
                string QueueType = que.QueueType;
                int QueueStatus = 0;
                //QueueStatus = clsComm.insertQstatus(QueueType, "Y");
                QstatusDetails qDetails = clsComm.getQueueStatusId(StudentId, QueueType);
                QueueStatus = qDetails.QueueStatusId;
                isSubmit = false;
                if (QueueStatus == 0)
                {
                    QueueStatus = clsComm.getQueueStatusIdIfSubmitted(StudentId, QueueType);
                    if (QueueStatus > 0)
                    {
                        isSubmit = true;
                    }
                }
                if (QueueStatus > 0)
                {
                    int QueueStatuss = clsComm.getQueueStatusIdIfSubmitted(StudentId, QueueType);
                    if (QueueStatuss > 0)
                    {
                        isSubmit = true;
                    }
                    ref_ReviewComments rCommnts = new ref_ReviewComments();
                    rCommnts = objData.ref_ReviewComments.Where(obj => obj.StudentId == StudentId && obj.SchoolId == SchoolId && obj.QueueStatusId == QueueStatus && obj.Type == QueueType).SingleOrDefault();
                    if (rCommnts != null)
                    {
                        Comments = rCommnts.Comments;
                        Approved = rCommnts.ApproveInd;
                    }
                    else
                        Approved = true;
                    ListAssessments = (from ltrengine in objData.ref_Checklist
                                       where (ltrengine.QueueId == QueueId && ltrengine.SchoolId == SchoolId && ltrengine.ChecklistHeaderId == 0)
                                       select new Assessments
                                       {
                                           Name = ltrengine.ChecklistName,
                                           Id = ltrengine.ChecklistId
                                       }).ToList();
                    if (ListAssessments != null)
                    {
                        foreach (var item in ListAssessments)
                        {
                            item.chkLists = (from ltrEngineItem in objData.ref_Checklist
                                             where (ltrEngineItem.ChecklistHeaderId == item.Id && ltrEngineItem.SchoolId == SchoolId)
                                             select new CheckLists
                                             {
                                                 Name = ltrEngineItem.ChecklistName,
                                                 Id = ltrEngineItem.ChecklistId
                                             }).ToList();
                            if (item.chkLists != null)
                            {
                                foreach (var chItem in item.chkLists)
                                {
                                    var chkVals = objData.ref_IntakeAssessment.Where(obj => obj.CheckListId == chItem.Id && obj.ReferalPersonalId == StudentId && obj.SchoolId == SchoolId && obj.QueueStatusId == QueueStatus).OrderByDescending(obj => obj.CreatedOn).FirstOrDefault();
                                    if (chkVals != null)
                                    {
                                        if (chkVals.IntakeDate != null)
                                            chItem.Date = chkVals.IntakeDate.Value.ToShortDateString();
                                        chItem.Comment = chkVals.Comments;
                                        chItem.Prsnt = chkVals.Present;
                                        chItem.NotPresent = chkVals.NonPresent;
                                        chItem.Emerging = chkVals.Emerging;
                                        if (chItem.Date != null) chItem.ReturnDate = chItem.Date;
                                    }
                                }
                            }
                        }
                    }


                }
            }
        }
        public string SaveData(int StudentId, int SchoolId, string Draft, int UserId, string Type)
        {
            string result = "Success";
            clsComm = new ClsCommon();
            sess = (clsSession)HttpContext.Current.Session["UserSession"];
            objData = new MelmarkDBEntities();
            ref_Queue que = new ref_Queue();
            try
            {
                que = objData.ref_Queue.Where(obj => obj.QueueType == "IE").SingleOrDefault();
                if (que != null)
                {
                    int QueueStatusId = 0;
                    int QueueId = que.QueueId;
                    string QueueType = que.QueueType;
                    QstatusDetails qDetails = clsComm.getQueueStatusId(StudentId, QueueType);
                    QueueStatusId = qDetails.QueueStatusId;
                    //ref_QueueStatus tempQueue = new ref_QueueStatus();
                    //tempQueue = objData.ref_QueueStatus.Where(obj => obj.StudentPersonalId == StudentId && obj.CurrentStatus == true && obj.SchoolId == SchoolId && obj.QueueId == QueueId).SingleOrDefault();
                    //if (tempQueue != null)
                    string qNextType = "IE";

                    //QueueStatusId = clsComm.insertQstatus(QueueType, Draft);
                    if (QueueStatusId > 0)
                    {
                        CommonAccRevComntsViewModel commModel = new CommonAccRevComntsViewModel();
                        ref_ReviewComments review = new ref_ReviewComments();
                        review = objData.ref_ReviewComments.Where(obj => obj.QueueStatusId == QueueStatusId && obj.StudentId == StudentId && obj.SchoolId == SchoolId && obj.Type == QueueType).SingleOrDefault();
                        if (review == null)
                        {
                            commModel = clsComm.UpdateOrInsertRevCmt(QueueType, 0, Comments, Draft, false, Approved, "AT");
                        }
                        else
                        {
                            commModel = clsComm.UpdateOrInsertRevCmt(QueueType, review.AcReviewId, Comments, Draft, true, Approved, "AT");
                        }
                        if (commModel != null)
                        {
                            int AcReviewId = commModel.academicReviewId;
                            if (AcReviewId > 0)
                            {
                                if (ListAssessments.Count > 0)
                                {
                                    int ChkComplteCnt = 0;
                                    //foreach (var Listitem in ListAssessments)
                                    //{
                                    //    foreach (var chkitem in Listitem.chkLists)
                                    //    {
                                    //        if (chkitem.Date == null && chkitem.Present == null)
                                    //        {
                                    //            ChkComplteCnt++;
                                    //        }
                                    //    }
                                    //}
                                    if (ChkComplteCnt == 0 && Type == "Submit")
                                    {
                                        foreach (var Listitem in ListAssessments)
                                        {
                                            foreach (var chkitem in Listitem.chkLists)
                                            {
                                                ref_IntakeAssessment refIA = new ref_IntakeAssessment();
                                                refIA = objData.ref_IntakeAssessment.Where(obj => obj.ReferalPersonalId == StudentId && obj.SchoolId == SchoolId && obj.CheckListId == chkitem.Id && obj.AcReviewId == AcReviewId).SingleOrDefault();
                                                if (refIA != null)
                                                {
                                                    DateTime dt = new DateTime();
                                                    if (chkitem.Date != null)
                                                    {
                                                        dt = DateTime.ParseExact(chkitem.Date, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                                                        refIA.IntakeDate = dt;
                                                    }
                                                    refIA.CheckListId = chkitem.Id;
                                                    if (chkitem.Present == "Present")
                                                    { refIA.Present = true; refIA.NonPresent = false; refIA.Emerging = false; }
                                                    else if (chkitem.Present == "NotPresent")
                                                    { refIA.Present = false; refIA.NonPresent = true; refIA.Emerging = false; }
                                                    else if (chkitem.Present == "Emerging")
                                                    { refIA.Present = false; refIA.NonPresent = false; refIA.Emerging = true; }
                                                    else
                                                    { refIA.Present = false; refIA.NonPresent = false; refIA.Emerging = false; }
                                                    refIA.Comments = chkitem.Comment;
                                                    refIA.Draft = Draft;
                                                    refIA.ModifiedBy = UserId;
                                                    refIA.ModifiedOn = DateTime.Now;
                                                    objData.SaveChanges();
                                                }
                                                else
                                                {
                                                    refIA = new ref_IntakeAssessment();
                                                    DateTime dt = new DateTime();
                                                    if (chkitem.Date != null)
                                                    {
                                                        dt = DateTime.ParseExact(chkitem.Date, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                                                        refIA.IntakeDate = dt;
                                                    }
                                                    refIA.QueueStatusId = QueueStatusId;
                                                    refIA.ReferalPersonalId = StudentId;
                                                    refIA.SchoolId = SchoolId;
                                                    refIA.CheckListId = chkitem.Id;
                                                    if (chkitem.Present == "Present")
                                                    { refIA.Present = true; refIA.NonPresent = false; refIA.Emerging = false; }
                                                    else if (chkitem.Present == "NotPresent")
                                                    { refIA.Present = false; refIA.NonPresent = true; refIA.Emerging = false; }
                                                    else if (chkitem.Present == "Emerging")
                                                    { refIA.Present = false; refIA.NonPresent = false; refIA.Emerging = true; }
                                                    else
                                                    { refIA.Present = false; refIA.NonPresent = false; refIA.Emerging = false; }
                                                    refIA.Comments = chkitem.Comment;
                                                    refIA.Draft = Draft;
                                                    refIA.AcReviewId = AcReviewId;
                                                    refIA.CreatedBy = UserId;
                                                    refIA.CreatedOn = DateTime.Now;
                                                    objData.ref_IntakeAssessment.Add(refIA);
                                                    objData.SaveChanges();
                                                }

                                            }
                                        }
                                        if (Draft == "N" && Approved == true)
                                        {
                                            clsComm.insertNewStatus("IE", "AT", sess.ReferralId);
                                        }
                                        else if (Draft == "N" && Approved == false)
                                        {
                                            clsComm.MakeQstatusInactive(QueueStatusId);
                                            int qid = clsComm.getQueueId("IL");
                                            sess.CurrentProcessId = qid;
                                        }
                                    }
                                    else if (Type == "Save")
                                    {
                                        foreach (var Listitem in ListAssessments)
                                        {
                                            foreach (var chkitem in Listitem.chkLists)
                                            {
                                                ref_IntakeAssessment refIA = new ref_IntakeAssessment();
                                                refIA = objData.ref_IntakeAssessment.Where(obj => obj.ReferalPersonalId == StudentId && obj.SchoolId == SchoolId && obj.CheckListId == chkitem.Id && obj.AcReviewId == AcReviewId).SingleOrDefault();
                                                if (refIA != null)
                                                {
                                                    DateTime dt = new DateTime();
                                                    if (chkitem.Date != null)
                                                    {
                                                        dt = DateTime.ParseExact(chkitem.Date, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                                                        refIA.IntakeDate = dt;
                                                    }
                                                    refIA.CheckListId = chkitem.Id;
                                                    if (chkitem.Present == "Present")
                                                    { refIA.Present = true; refIA.NonPresent = false; refIA.Emerging = false; }
                                                    else if (chkitem.Present == "NotPresent")
                                                    { refIA.Present = false; refIA.NonPresent = true; refIA.Emerging = false; }
                                                    else if (chkitem.Present == "Emerging")
                                                    { refIA.Present = false; refIA.NonPresent = false; refIA.Emerging = true; }
                                                    else
                                                    { refIA.Present = false; refIA.NonPresent = false; refIA.Emerging = false; }
                                                    refIA.Comments = chkitem.Comment;
                                                    refIA.Draft = Draft;
                                                    refIA.ModifiedBy = UserId;
                                                    refIA.ModifiedOn = DateTime.Now;
                                                    objData.SaveChanges();
                                                }
                                                else
                                                {
                                                    refIA = new ref_IntakeAssessment();
                                                    DateTime dt = new DateTime();
                                                    if (chkitem.Date != null)
                                                    {
                                                        dt = DateTime.ParseExact(chkitem.Date, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                                                        refIA.IntakeDate = dt;
                                                    }
                                                    refIA.QueueStatusId = QueueStatusId;
                                                    refIA.ReferalPersonalId = StudentId;
                                                    refIA.SchoolId = SchoolId;
                                                    refIA.CheckListId = chkitem.Id;
                                                    if (chkitem.Present == "Present")
                                                    { refIA.Present = true; refIA.NonPresent = false; refIA.Emerging = false; }
                                                    else if (chkitem.Present == "NotPresent")
                                                    { refIA.Present = false; refIA.NonPresent = true; refIA.Emerging = false; }
                                                    else if (chkitem.Present == "Emerging")
                                                    { refIA.Present = false; refIA.NonPresent = false; refIA.Emerging = true; }
                                                    else
                                                    { refIA.Present = false; refIA.NonPresent = false; refIA.Emerging = false; }
                                                    refIA.Comments = chkitem.Comment;
                                                    refIA.Draft = Draft;
                                                    refIA.AcReviewId = AcReviewId;
                                                    refIA.CreatedBy = UserId;
                                                    refIA.CreatedOn = DateTime.Now;
                                                    objData.ref_IntakeAssessment.Add(refIA);
                                                    objData.SaveChanges();
                                                }

                                            }
                                        }
                                    }
                                    else
                                    {
                                        result = "Assessment Checklist not complete...";
                                    }
                                }
                                else
                                    result = "Assessment Checklist not available...";
                            }
                            else result = "Saving Failed....";
                        }
                        else result = "Saving Failed....";

                        //int qIdNext = clsComm.getQueueId("AT");
                        //int QProcess = clsComm.getProcessId();
                        //if (Draft == "Y")
                        //    QueueStatusId = clsComm.insertQstatus(QueueType, Draft);
                        //else if (Draft == "N")
                        //{
                        //    qNextType = "AT";
                        //    clsComm.insertNewStatus(QueueType, "AT", StudentId);
                        //    var qStatusRow1 = objData.ref_QueueStatus.Where(x => x.StudentPersonalId == StudentId && x.QueueId == qIdNext && x.CurrentStatus == true && x.QueueProcess == QProcess).ToList();
                        //    QueueStatusId = qStatusRow1.Count > 0 ? qStatusRow1[0].QueueStatusId : 0;
                        //}
                    }
                    else result = "Saving Failed....";
                }
                else result = "Saving Failed....";
            }
            catch (Exception ex)
            {
                result = "Saving Failed. " + ex.Message;
            }
            return result;
        }
    }
    public class CheckLists
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Date { get; set; }
        public virtual string ReturnDate { get; set; }
        public virtual string Present { get; set; }
        public virtual bool? NotPresent { get; set; }
        public virtual bool? Emerging { get; set; }
        public virtual bool? Prsnt { get; set; }
        public virtual string Comment { get; set; }
    }
    public class Assessments
    {
        public virtual IList<CheckLists> chkLists { get; set; }
        public virtual string Name { get; set; }
        public virtual int Id { get; set; }
        public Assessments()
        {
            chkLists = new List<CheckLists>();
        }
    }
}