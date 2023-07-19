using DataLayer;
using ReferalDB.CommonClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReferalDB.Models
{
    public class NotificationModel
    {
        ClsCommon clsComm = null;
        public MelmarkDBEntities objData = null;
        public virtual IEnumerable<Notifications> Notifications { get; set; }
        public static clsSession sess = null;
        public NotificationModel()
        {
            Notifications = new List<Notifications>();
        }
        public void GetNotifications(int UserId,int SchoolId)
        {
            objData = new MelmarkDBEntities();
            sess=(clsSession)HttpContext.Current.Session["UserSession"];
            if (sess != null)
            {
                if (sess.ReferralId > 0)
                {
                    Notifications = (from objNoti in objData.ref_Notification
                                     join objRef in objData.StudentPersonals on objNoti.ReferalId equals objRef.StudentPersonalId
                                     join objQueue in objData.ref_Queue on objNoti.QueueId equals objQueue.QueueId
                                     join objTeam in objData.ReviewTeams on objNoti.TeamId equals objTeam.TeamId
                                     join objTeamMbrs in objData.TeamMembers on objTeam.TeamId equals objTeamMbrs.TeamId
                                     where (objTeam.ActiveInd == "A" && objTeamMbrs.UserId == UserId && objTeamMbrs.SchoolId == SchoolId && objRef.StudentPersonalId==sess.ReferralId)
                                     select new Notifications
                                     {
                                         Id = objNoti.NotificationId,
                                         Message = objNoti.Comments,
                                         QueueName = objQueue.QueueName,
                                         ReferalName = objRef.LastName + ", " + objRef.FirstName,
                                         Date = objNoti.Date
                                     }).ToList();
                }
                else
                {
                    Notifications = (from objNoti in objData.ref_Notification
                                     join objRef in objData.StudentPersonals on objNoti.ReferalId equals objRef.StudentPersonalId
                                     join objQueue in objData.ref_Queue on objNoti.QueueId equals objQueue.QueueId
                                     join objTeam in objData.ReviewTeams on objNoti.TeamId equals objTeam.TeamId
                                     join objTeamMbrs in objData.TeamMembers on objTeam.TeamId equals objTeamMbrs.TeamId
                                     where (objTeam.ActiveInd == "A" && objTeamMbrs.UserId == UserId && objTeamMbrs.SchoolId == SchoolId)
                                     select new Notifications
                                     {
                                         Id = objNoti.NotificationId,
                                         Message = objNoti.Comments,
                                         QueueName = objQueue.QueueName,
                                         ReferalName = objRef.LastName + ", " + objRef.FirstName,
                                         Date = objNoti.Date
                                     }).ToList();
                }
            }
        }
    }
    public class Notifications
    {
        public virtual int Id { get; set; }
        public virtual string ReferalName { get; set; }
        public virtual string QueueName { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual string Message { get; set; }
    }
}