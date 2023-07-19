using BuisinessLayer;
using DataLayer;
using ReferalDB.CommonClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReferalDB.Models
{
    public class AdmissionReviewViewModel
    {
        public clsSession sess = null;
        ClsCommon clsComm = null;
        public MelmarkDBEntities objData = null;
        public virtual IList<AdmissionReviewTeams> ListAdmsnReviewTeam { get; set; }
        public virtual bool Approved { get; set; }
        public virtual bool isSubmit { get; set; }
        public AdmissionReviewViewModel()
        {
            ListAdmsnReviewTeam = new List<AdmissionReviewTeams>();
        }

        public void GetAdmissionReviewTeam(int StudentId, int SchoolId, int UserId)
        {
            clsComm = new ClsCommon();
            objData = new MelmarkDBEntities();
            ref_Queue que = new ref_Queue();
            que = objData.ref_Queue.Where(obj => obj.QueueName == "Admission Review Team").SingleOrDefault();
            if (que != null)
            {
                int QueueId = que.QueueId;
                string QueueType = que.QueueType;
                int processId = clsComm.getProcessId();
                int QueueStatus = 0;
                //QueueStatus = clsComm.insertQstatus(QueueType, "Y");
                QstatusDetails qDetails = clsComm.getQueueStatusId(StudentId, QueueType);
                QueueStatus = qDetails.QueueStatusId;
                isSubmit = false;
                if (QueueStatus > 0)
                {
                   int QueueStatuss = clsComm.getQueueStatusIdIfSubmitted(StudentId, QueueType);
                    if (QueueStatuss > 0)
                    {
                        isSubmit = true;
                    }
                }
                if (QueueStatus > 0)
                {
                    ref_ReviewComments rCommnts = new ref_ReviewComments();
                    rCommnts = objData.ref_ReviewComments.Where(obj => obj.StudentId == StudentId && obj.SchoolId == SchoolId && obj.QueueStatusId == QueueStatus && obj.Type == QueueType).SingleOrDefault();
                    if (rCommnts != null)
                    {
                        Approved = rCommnts.ApproveInd;
                    }
                    else
                        Approved = true;
                    //ListAdmsnReviewTeam = (from objTeam in objData.ReviewTeams
                    //                       join objTeamRef in objData.ref_TeamReferrals on objTeam.TeamId equals objTeamRef.TeamId
                    //                       join objTeamMbrs in objData.TeamMembers on objTeam.TeamId equals objTeamMbrs.TeamId
                    //                       join objusers in objData.Users on objTeamMbrs.UserId equals objusers.UserId
                    //                       where (objTeamRef.StudentPersonalId == StudentId && objTeamRef.SchoolId == SchoolId)
                    //                       select new AdmissionReviewTeams
                    //                       {
                    //                           Id = objTeamMbrs.MemberId,
                    //                           Name = objusers.UserFName + " " + objusers.UserLName
                    //                       }).ToList();
                    ListAdmsnReviewTeam = (from objTeam in objData.ref_TeamAssign
                                           join objTeamMbrs in objData.TeamMembers on objTeam.TeamId equals objTeamMbrs.TeamId
                                           join objtm in objData.ReviewTeams on objTeam.TeamId equals objtm.TeamId
                                           join objusers in objData.Users on objTeamMbrs.UserId equals objusers.UserId
                                           where (objTeam.ReferalId == StudentId && objTeam.ProcessId == processId && objTeam.Complete == true)
                                           select new AdmissionReviewTeams
                                           {
                                               TeamId = objTeamMbrs.TeamId,
                                               Id = objTeamMbrs.MemberId,
                                               Name = objusers.UserFName + " " + objusers.UserLName
                                           }).ToList();

                    if (ListAdmsnReviewTeam != null)
                    {
                        foreach (var item in ListAdmsnReviewTeam)
                        {
                            var rslt = objData.ref_ReviewTeamDecision.Where(obj => obj.TeamMemberId == item.Id & obj.ReferalId == StudentId && obj.TeamId == item.TeamId).SingleOrDefault();
                            if (rslt != null)
                            {
                                item.Comments = rslt.Comments;
                                item.AcceptInd = rslt.Decision;
                            }
                        }
                    }
                }
            }
        }
        public string SaveData(int StudentId, int SchoolId, string Draft, int UserId)
        {
            string result = "Success";
            sess = (clsSession)HttpContext.Current.Session["UserSession"];
            clsComm = new ClsCommon();
            objData = new MelmarkDBEntities();
            ref_Queue que = new ref_Queue();
            try
            {
                que = objData.ref_Queue.Where(obj => obj.QueueName == "Admission Review Team").SingleOrDefault();
                if (que != null)
                {
                    int QueueStatusId = 0;
                    int QueueId = que.QueueId;
                    int processId = clsComm.getProcessId();
                    string QueueType = que.QueueType;
                    QstatusDetails qDetails = clsComm.getQueueStatusId(StudentId, QueueType);
                    QueueStatusId = qDetails.QueueStatusId;
                    //ref_QueueStatus tempQueue = new ref_QueueStatus();
                    //tempQueue = objData.ref_QueueStatus.Where(obj => obj.StudentPersonalId == StudentId && obj.CurrentStatus == true && obj.SchoolId == SchoolId && obj.QueueId == QueueId).SingleOrDefault();
                    //if (tempQueue != null)
                    string qNextType = "AT";

                    //QueueStatusId = clsComm.insertQstatus(QueueType, Draft);
                    if (QueueStatusId > 0)
                    {
                        //int teamId = 0;
                        //var team = objData.ref_TeamReferrals.Where(obj => obj.StudentPersonalId == StudentId && obj.SchoolId == SchoolId && obj.ActiveInd == "A").SingleOrDefault();
                        var team = objData.ref_TeamAssign.Where(obj => obj.ReferalId == StudentId && obj.ProcessId == processId && obj.Complete == true).ToList();
                        //if (team != null)
                        //    teamId = team.TeamId;

                        //if (teamId > 0)
                       //if (team.Count > 0)
                       // {
                            CommonAccRevComntsViewModel commModel = new CommonAccRevComntsViewModel();
                            ref_ReviewComments review = new ref_ReviewComments();
                            review = objData.ref_ReviewComments.Where(obj => obj.QueueStatusId == QueueStatusId && obj.StudentId == StudentId && obj.SchoolId == SchoolId && obj.Type == QueueType).SingleOrDefault();
                            if (review == null)
                            {
                                commModel = clsComm.UpdateOrInsertRevCmt(QueueType, 0, "", Draft, false, Approved, "SI");
                            }
                            else
                            {
                                commModel = clsComm.UpdateOrInsertRevCmt(QueueType, review.AcReviewId, "", Draft, true, Approved, "SI");
                            }
                            if (commModel != null)
                            {

                                foreach (var item in ListAdmsnReviewTeam)
                                {
                                    ref_ReviewTeamDecision decsn = new ref_ReviewTeamDecision();
                                    decsn = objData.ref_ReviewTeamDecision.Where(obj => obj.ReferalId == StudentId && obj.TeamMemberId == item.Id && obj.TeamId == item.TeamId).SingleOrDefault();
                                    if (decsn != null)
                                    {
                                        if (item.Decision == "Accept")
                                        { decsn.Decision = true; }
                                        else if (item.Decision == "Reject")
                                        { decsn.Decision = false; }
                                        decsn.Comments = item.Comments;
                                        decsn.ModifiedBy = UserId;
                                        decsn.ModifiedOn = DateTime.Now;
                                        objData.SaveChanges();
                                    }
                                    else
                                    {
                                        decsn = new ref_ReviewTeamDecision();
                                        decsn.TeamId = item.TeamId;
                                        decsn.TeamMemberId = item.Id;
                                        decsn.ReferalId = StudentId;
                                        if (item.Decision == "Accept")
                                        { decsn.Decision = true; }
                                        else if (item.Decision == "Reject")
                                        { decsn.Decision = false; }
                                        decsn.Comments = item.Comments;
                                        decsn.CreatedBy = UserId;
                                        decsn.CreatedOn = DateTime.Now;
                                        objData.ref_ReviewTeamDecision.Add(decsn);
                                        objData.SaveChanges();
                                    }
                                }
                                if (Draft == "N" && Approved == true)
                                {
                                   // clsComm.insertNewStatus("AT", "SI", sess.ReferralId);
                                }
                                else if (Draft == "N" && Approved == false)
                                {
                                    clsComm.MakeQstatusInactive(QueueStatusId);
                                    int qid = clsComm.getQueueId("IL");
                                    sess.CurrentProcessId = qid;
                                }
                            }
                            else result = "Saving Failed....";

                        //}
                        //else result = "No Admission Review Team Found..!!";
                        //int qIdNext = clsComm.getQueueId("SI");
                        //int QProcess = clsComm.getProcessId();
                        //if (Draft == "Y")
                        //    QueueStatusId = clsComm.insertQstatus(QueueType, Draft);
                        //else if (Draft == "N")
                        //{
                        //    qNextType = "SI";
                        //    clsComm.insertNewStatus(QueueType, "SI", StudentId);
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
                result = ex.Message;
            }
            return result;
        }
        public string SendNotification(int StudentId, int SchoolId, int UserId)
        {
            string result = clsGeneral.sucessMsg("Successfully Sent");
            objData = new MelmarkDBEntities();
            clsComm = new ClsCommon();
            int processId = clsComm.getProcessId();
            //objData.ref_TeamAssign.Where(obj => obj.ReferalId == StudentId && obj.ProcessId == processId && obj.Complete == true).ToList();
            var team = (from objTeam in objData.ref_TeamAssign
                        join objtm in objData.ReviewTeams on objTeam.TeamId equals objtm.TeamId
                        where (objTeam.ReferalId == StudentId && objTeam.ProcessId == processId && objTeam.Complete == true)
                        select new 
                        {
                            TeamId = objTeam.TeamId
                        }).ToList();
            if (team.Count > 0)
            {
                try
                {
                    var Referralname = objData.StudentPersonals.Where(objref => objref.StudentPersonalId == StudentId).SingleOrDefault();
                    ref_Queue que = new ref_Queue();
                    que = objData.ref_Queue.Where(obj => obj.QueueName == "Admission Review Team").SingleOrDefault();

                    foreach (var t in team)
                    {
                        int teamid = (int)t.TeamId;
                        var notification = objData.ref_Notification.Where(obj => obj.QueueId == que.QueueId && obj.ReferalId == StudentId && obj.TeamId == teamid).SingleOrDefault();
                        ref_Notification notify = new ref_Notification();
                        if (notification != null)
                        {
                            notification.Comments = "You have been selected for review of the referral " + Referralname.LastName + "," + Referralname.FirstName;
                            notification.Date = DateTime.Now;
                            notification.QueueId = que.QueueId;
                            notification.ReferalId = StudentId;
                            notification.TeamId = teamid;
                            notification.ModifiedBy = UserId;
                            notification.ModifiedOn = DateTime.Now;
                            objData.SaveChanges();
                            result = clsGeneral.sucessMsg("Already Sent notification..");
                        }
                        else
                        {
                            notify.Comments = "You have been selected for review of the referral " + Referralname.LastName + "," + Referralname.FirstName;
                            notify.Date = DateTime.Now;
                            notify.QueueId = que.QueueId;
                            notify.ReferalId = StudentId;
                            notify.TeamId = teamid;
                            notify.CreatedBy = UserId;
                            notify.CreatedOn = DateTime.Now;
                            objData.ref_Notification.Add(notify);
                            objData.SaveChanges();

                        }
                    }
                }
                catch (Exception ex)
                {
                    result = clsGeneral.failedMsg(ex.Message);
                }
            }
            else
            {
                result = clsGeneral.failedMsg("Review Team does not exist");
            }
            return result;
        }
    }
    public class AdmissionReviewTeams
    {
        public virtual int TeamId { get; set; }
        public virtual string Name { get; set; }
        public virtual int Id { get; set; }
        public virtual string Decision { get; set; }
        public virtual bool? AcceptInd { get; set; }
        public virtual string Comments { get; set; }
    }
}