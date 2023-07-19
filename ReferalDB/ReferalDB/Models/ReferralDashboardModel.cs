using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer;
using System.Data.Objects.SqlClient;
using ReferalDB.CommonClass;
using ReferalDBApplicant.Classes;

namespace ReferalDB.Models
{

    public class ReferralDashboardModel
    {
        public static MelmarkDBEntities objData = null;
        public IList<DashboardModelClass> DataContent { get; set; }
        public IList<SubQueueModelClass> SubQueueList { get; set; }
        public IList<QueueModelClass> QueueList { get; set; }
        public IList<ActivityModelClass> ActivityList { get; set; }
        public IList<NotificationClass> NotificationList { get; set; }
        public static clsSession sess = null;


        public ReferralDashboardModel()
        {
            DataContent = new List<DashboardModelClass>();
            QueueList = new List<QueueModelClass>();
            SubQueueList = new List<SubQueueModelClass>();
            ActivityList = new List<ActivityModelClass>();
            NotificationList = new List<NotificationClass>();
        }

        public static ReferralDashboardModel BindDashboard(int Schoolid, string Queuename, string SType)
        {
            sess = (clsSession)HttpContext.Current.Session["UserSession"];
            objData = new MelmarkDBEntities();
            clsReferral objClsRef = new clsReferral();
            ClsCommon objCommon = new ClsCommon();
            IList<DashboardModelClass> retunmodel = new List<DashboardModelClass>();
            IList<DashboardModelClass> modellist = new List<DashboardModelClass>();
            ReferralDashboardModel ObjRef = new ReferralDashboardModel();
            string[] Queues = Queuename.Split(',');
            int MasterId = 0;
            string lastStepName = "";
            int studentId = 0;
            string CurrentQId = "";
            int queueid = Convert.ToInt32(Queues[0].ToString());

            if (SType == "Client")
            {


                retunmodel = (from objref in objData.Client_Dashboard(queueid, Schoolid, SType)
                              select new DashboardModelClass
                              {
                                  ReferralId = objref.ReferralId,
                                  ReferralName = objref.ReferralName,
                                  ReferralDob = objref.BirthDate,
                                  ReferralGender = objref.Gender,
                                  ApplicationDate = objref.Appdate,
                                  CompletedStep = objref.LastCompleted,
                                  CompletedBy = objref.CompletedBy,
                                  ImageUrl = objClsRef.GetStudentImage(objref.ReferralId),
                                  ActiveProcess = Convert.ToString(objref.ActiveProcess),
                                  PermissionStatus=sess.IsApproved,
                                  Perc = 100
                              }).ToList();
                if (retunmodel.Count > 0)
                {
                    foreach (var items in retunmodel)
                    {
                        modellist.Add(items);
                    }
                }
            }
            else
            {
                //if (Queues[1] != "Referral")
                //{
                //    int LastQId = objCommon.getQueueId("DC");
                //    var currentProcessQIds = objData.ref_QueueStatus.Where(x => x.SchoolId == Schoolid && x.CurrentStatus == true).ToList();
                //    if (currentProcessQIds.Count > 0)
                //    {
                //        foreach (var item in currentProcessQIds)
                //        {
                //            if (item.QueueId <= LastQId)
                //            {
                //                studentId = item.StudentPersonalId;
                //                CurrentQId = Convert.ToString(item.QueueId);
                //                var masterId = objData.ref_Queue.Where(x => x.SchoolId == Schoolid && x.QueueId == item.QueueId).ToList();
                //                if (masterId.Count > 0)
                //                    MasterId = masterId[0].MasterId;
                //                if (queueid < MasterId)
                //                {
                //                    var lastStep = objData.ref_Queue.Where(x => x.SchoolId == Schoolid && x.MasterId == queueid).ToList();
                //                    if (lastStep.Count > 0)
                //                        lastStepName = lastStep[lastStep.Count - 1].QueueName;
                //                    retunmodel = (from objref in objData.StudentPersonals
                //                                  join objUser in objData.Users on objref.CreatedBy equals objUser.UserId
                //                                  where objref.StudentPersonalId == studentId
                //                                  select new DashboardModelClass
                //                                  {
                //                                      ReferralId = objref.StudentPersonalId,
                //                                      ReferralName = objref.LastName + "," + objref.FirstName,
                //                                      ReferralDob = objref.BirthDate,
                //                                      ReferralGender = objref.Gender,
                //                                      ApplicationDate = objref.CreatedOn,
                //                                      CompletedStep = lastStepName,
                //                                      CompletedBy = objUser.UserLName + "," + objUser.UserFName,
                //                                      ImageUrl = objref.ImageUrl,
                //                                      ActiveProcess = CurrentQId,
                //                                      PermissionStatus=sess.IsApproved,
                //                                      Perc = 100
                //                                  }).ToList();
                //                    if (retunmodel.Count > 0)
                //                    {
                //                        foreach (var items in retunmodel)
                //                        {
                //                            if (items.ReferralGender == "1")
                //                                items.ReferralGender = "Male";
                //                            else
                //                                items.ReferralGender = "Female";
                //                            modellist.Add(items);
                //                        }
                //                    }
                //                }
                //            }
                //        }
                //    }
                //}
                retunmodel = (from objref in objData.Referral_Dashboard(queueid, Schoolid, SType)
                              select new DashboardModelClass
                              {
                                  ReferralId = objref.ReferralId,
                                  ReferralName = objref.ReferralName,
                                  ReferralDob = objref.BirthDate,
                                  ReferralGender = objref.Gender,
                                  ApplicationDate = objref.Appdate,
                                  CompletedStep = objref.LastCompleted,
                                  CompletedBy = objref.CompletedBy,
                                  ImageUrl = objClsRef.GetStudentImage(objref.ReferralId),
                                  PermissionStatus=sess.IsApproved,
                                  ActiveProcess = Convert.ToString(objref.ActiveProcess),
                                  QueueType=objref.QueueType,
                                  Perc = objref.Percentage
                              }).ToList();
                if (retunmodel.Count > 0)
                {
                    foreach (var items in retunmodel)
                    {
                        modellist.Add(items);
                    }
                }
            }

            ObjRef.DataContent = modellist;
            return ObjRef;

        }

        public static ReferralDashboardModel BindActivity(int SchoolId, int ReferralId)
        {
            objData = new MelmarkDBEntities();
            IList<ActivityModelClass> ActModel = new List<ActivityModelClass>();
            ReferralDashboardModel ObjRef = new ReferralDashboardModel();
            ActModel = (from x in objData.StudentPersonals
                        join y in objData.ref_QueueStatus
                        on x.StudentPersonalId equals y.StudentPersonalId
                        join z in objData.ref_Queue
                        on y.QueueId equals z.QueueId
                        join a in objData.Users
                        on y.CreatedBy equals a.UserId
                        where x.StudentType == "Referral" && x.SchoolId == SchoolId && x.StudentPersonalId == ReferralId
                        orderby y.QueueStatusId descending
                        select new ActivityModelClass
                        {
                            ReferralId = x.StudentPersonalId,
                            ReferralName = x.LastName + " , " + x.FirstName,
                            CurrentStep = z.QueueName,
                            UserName = a.UserLName + " , " + a.UserFName,
                            RefUrl = x.ImageUrl,
                            RefGender = x.Gender,
                            Status = y.Draft,
                            QueueStatusID = y.QueueStatusId,
                            CreatedOn = y.CreatedOn,
                            ModifiedOn = y.ModifiedOn
                        }).ToList();
            ObjRef.ActivityList = ActModel;
            //ObjRef.ActiveCount = (from x in objData.StudentPersonals
            //                      join y in objData.ref_QueueStatus
            //                      on x.StudentPersonalId equals y.StudentPersonalId
            //                      join z in objData.ref_Queue
            //                      on y.QueueId equals z.QueueId
            //                      join a in objData.Users
            //                      on y.CreatedBy equals a.UserId
            //                      where x.StudentType == "Referral" && x.StudentPersonalId == ReferralId && x.SchoolId == SchoolId
            //                      orderby y.QueueStatusId descending
            //                      select new ActivityModelClass
            //                      {
            //                          ReferralId = x.StudentPersonalId,
            //                          ReferralName = x.LastName + " , " + x.FirstName,
            //                          CurrentStep = z.QueueName,
            //                          UserName = a.UserLName + " , " + a.UserFName,
            //                          RefUrl = x.ImageUrl,
            //                          RefGender = x.Gender
            //                      }).Count();
            return ObjRef;
        }

        public static ReferralDashboardModel BindQueueList(int SchoolId)
        {
            objData = new MelmarkDBEntities();
            ClsCommon commonClass = new ClsCommon();
            IList<QueueModelClass> Queumdl = new List<QueueModelClass>();
            IList<SubQueueModelClass> subqueuemdl = new List<SubQueueModelClass>();
            IList<NotificationClass> notiModel = new List<NotificationClass>();
            ReferralDashboardModel ObjRef = new ReferralDashboardModel();

            Queumdl = (from q in objData.ref_Queue
                       where q.MasterId == 0 //&& q.SchoolId == SchoolId
                       orderby q.QueueId
                       select new QueueModelClass
                       {
                           QueueId = q.QueueId,
                           QueueName = q.QueueName,
                           QueueType = q.QueueType
                       }).ToList();

            subqueuemdl = (from sq in objData.ref_Queue
                           where sq.MasterId != 0 //&& sq.SchoolId == SchoolId
                           orderby sq.SortOrder
                           select new SubQueueModelClass
                           {
                               QueueId = sq.QueueId,
                               MasterId = sq.MasterId,
                               QueueName = sq.QueueName,
                               QueueType = sq.QueueType,
                               PrevQueueId = sq.PrevQueueId,
                               SortOrder = sq.SortOrder
                           }).ToList();

            notiModel = (from objnoti in objData.Referral_Notifications(SchoolId)
                         select new NotificationClass
                         {
                             QueueId = objnoti.QueueId,
                             AttentionNeeded = objnoti.AttentionNeeded,
                             Newqueue = (int)objnoti.New,
                             RefTotal = (int)objnoti.Total

                         }).ToList();
            var retunmodel = (from objref in objData.Client_Dashboard(commonClass.getQueueId("CL"), SchoolId, "Client")
                              select new DashboardModelClass
                              {
                                  ReferralId = objref.ReferralId,
                                  ReferralName = objref.ReferralName,
                                  ReferralDob = objref.BirthDate,
                                  ReferralGender = objref.Gender,
                                  ApplicationDate = objref.Appdate,
                                  CompletedStep = objref.LastCompleted,
                                  CompletedBy = objref.CompletedBy,
                                  ActiveProcess = Convert.ToString(objref.ActiveProcess),
                                  Perc = objref.Percentage
                              }).ToList();
            if (retunmodel.Count > 0)
            {
                int count = retunmodel.Count;
                int Qid = commonClass.getQueueId("CL");
                foreach (var item in notiModel)
                {
                    if (item.QueueId == Qid)
                        item.AttentionNeeded = count;
                }
            }
            ClsCommon com=new ClsCommon();
            int QueueId=com.getQueueId("CL");
            foreach (var item in notiModel)
            {
                if (item.QueueId == QueueId)
                {
                    item.Newqueue = com.getClientListCount(SchoolId, QueueId);
                }
            }


            ObjRef.NotificationList = notiModel;
            ObjRef.QueueList = Queumdl;
            ObjRef.SubQueueList = subqueuemdl;
            return ObjRef;

        }



        public static ReferralDashboardModel fillDashboard(SearchModel objDashbrdSearch, int Schoolid)
        {

            IList<ReferralDashboardModel> retunmodel = new List<ReferralDashboardModel>();

            string pageArg = objDashbrdSearch.PagingArgument;
            const int pagesize = 10;
            string[] datasplit = pageArg.Split('*');
            string way = datasplit[1];
            int dec = 0;
            string flag = "";
            int page = Convert.ToInt32(datasplit[0]);
            objData = new MelmarkDBEntities();
            IList<DashboardModelClass> result = new List<DashboardModelClass>();
            if (way == "n")
            {
                page++;
                objDashbrdSearch.PagingArgument = page.ToString();
                var SelectRefDashboard = from c in objData.StudentPersonals where c.StudentType == "Referral" && c.SchoolId == Schoolid orderby c.StudentPersonalId select c;
                result = (from u in SelectRefDashboard
                          from a in objData.ref_QueueStatus
                                           .Where(x => u.StudentPersonalId == x.StudentPersonalId && u.StudentType == "Referral")
                                           .DefaultIfEmpty()
                          from s in objData.ref_Queue
                                           .Where(x => a.QueueStatusId == x.QueueId)
                                           .DefaultIfEmpty()
                          from t in objData.Users
                                           .Where(x => a.CreatedBy == x.UserId)
                                           .DefaultIfEmpty()

                          select new DashboardModelClass
                          {
                              ReferralId = u.StudentPersonalId,
                              ReferralName = u.LastName + " , " + u.FirstName,
                              ReferralDob = u.BirthDate,
                              ReferralGender = u.Gender,
                              ApplicationDate = u.CreatedOn,
                              CompletedStep = s.QueueName,
                              CompletedBy = t.UserLName + " , " + t.UserFName,
                              ActiveProcess = s.QueueName
                          }).ToList();

                if (objDashbrdSearch.SortStatus == false)
                {
                    result = (from u in SelectRefDashboard.
                              Where(x => objDashbrdSearch.SearchArgument == null ||
                        x.LastName.StartsWith(objDashbrdSearch.SearchArgument))
                              from a in objData.ref_QueueStatus
                                               .Where(x => u.StudentPersonalId == x.StudentPersonalId && u.StudentType == "Referral")
                                               .DefaultIfEmpty()
                              from s in objData.ref_Queue
                                               .Where(x => a.QueueStatusId == x.QueueId)
                                               .DefaultIfEmpty()
                              from t in objData.Users
                                               .Where(x => a.CreatedBy == x.UserId)
                                               .DefaultIfEmpty()


                              select new DashboardModelClass
                              {
                                  ReferralId = u.StudentPersonalId,
                                  ReferralName = u.LastName + " , " + u.FirstName,
                                  ReferralDob = u.BirthDate,
                                  ReferralGender = u.Gender,
                                  ApplicationDate = u.CreatedOn,
                                  CompletedStep = s.QueueName,
                                  CompletedBy = t.UserLName + " , " + t.UserFName,
                                  ActiveProcess = s.QueueName
                              }).ToList();

                }
                else
                {
                    result = DashboardSearchList(objDashbrdSearch, result);
                }

                if (result.Count == pagesize)
                {
                    dec = 1;
                    flag = "<>";
                }
                else
                {
                    flag = "<";
                }
            }
            else if (way == "p")
            {
                page--;
                objDashbrdSearch.PagingArgument = page.ToString();
                var SelectRefDashboard = from c in objData.StudentPersonals where c.StudentType == "Referral" && c.SchoolId == Schoolid orderby c.StudentPersonalId select c;
                result = (from u in SelectRefDashboard
                          from a in objData.ref_QueueStatus
                                           .Where(x => u.StudentPersonalId == x.StudentPersonalId && u.StudentType == "Referral")
                                           .DefaultIfEmpty()
                          from s in objData.ref_Queue
                                           .Where(x => a.QueueStatusId == x.QueueId)
                                           .DefaultIfEmpty()
                          from t in objData.Users
                                           .Where(x => a.CreatedBy == x.UserId)
                                           .DefaultIfEmpty()

                          select new DashboardModelClass
                          {
                              ReferralId = u.StudentPersonalId,
                              ReferralName = u.LastName + " , " + u.FirstName,
                              ReferralDob = u.BirthDate,
                              ReferralGender = u.Gender,
                              ApplicationDate = u.CreatedOn,
                              CompletedStep = s.QueueName,
                              CompletedBy = t.UserLName + " , " + t.UserFName,
                              ActiveProcess = s.QueueName
                          }).ToList();
                if (objDashbrdSearch.SortStatus == false)
                {
                    result = (from u in SelectRefDashboard.
                             Where(x => objDashbrdSearch.SearchArgument == null ||
                       x.LastName.StartsWith(objDashbrdSearch.SearchArgument))
                              from a in objData.ref_QueueStatus
                                               .Where(x => u.StudentPersonalId == x.StudentPersonalId && u.StudentType == "Referral")
                                               .DefaultIfEmpty()
                              from s in objData.ref_Queue
                                               .Where(x => a.QueueStatusId == x.QueueId)
                                               .DefaultIfEmpty()
                              from t in objData.Users
                                               .Where(x => a.CreatedBy == x.UserId)
                                               .DefaultIfEmpty()


                              select new DashboardModelClass
                              {
                                  ReferralId = u.StudentPersonalId,
                                  ReferralName = u.LastName + " , " + u.FirstName,
                                  ReferralDob = u.BirthDate,
                                  ReferralGender = u.Gender,
                                  ApplicationDate = u.CreatedOn,
                                  CompletedStep = s.QueueName,
                                  CompletedBy = t.UserLName + " , " + t.UserFName,
                                  ActiveProcess = s.QueueName
                              }).ToList();





                }
                else
                {
                    result = DashboardSearchList(objDashbrdSearch, result);
                }
                flag = ">";

                if (result.Count == pagesize)
                {

                    dec = 1;
                    flag = "<>";
                    if (page == 0)
                    {
                        flag = ">";
                    }
                }
                else
                {

                    flag = ">";
                }
            }
            else
            {
                objDashbrdSearch.PagingArgument = page.ToString();
                try
                {
                    var SelectRefDashboard = from c in objData.StudentPersonals where c.StudentType == "Referral" && c.SchoolId == Schoolid orderby c.StudentPersonalId select c;
                    result = (from u in SelectRefDashboard
                              from a in objData.ref_QueueStatus
                                               .Where(x => u.StudentPersonalId == x.StudentPersonalId && u.StudentType == "Referral")
                                               .DefaultIfEmpty()
                              from s in objData.ref_Queue
                                               .Where(x => a.QueueStatusId == x.QueueId)
                                               .DefaultIfEmpty()
                              from t in objData.Users
                                               .Where(x => a.CreatedBy == x.UserId)
                                               .DefaultIfEmpty()

                              select new DashboardModelClass
                              {
                                  ReferralId = u.StudentPersonalId,
                                  ReferralName = u.LastName + " , " + u.FirstName,
                                  ReferralDob = u.BirthDate,
                                  ReferralGender = u.Gender,
                                  ApplicationDate = u.CreatedOn,
                                  CompletedStep = s.QueueName,
                                  CompletedBy = t.UserLName + " , " + t.UserFName,
                                  ActiveProcess = s.QueueName
                              }).ToList();
                }
                catch
                {

                }
                if (objDashbrdSearch.SortStatus == false)
                {
                    if (objDashbrdSearch.SearchArgument != "*")
                    {
                        var SelectRefDashboard = from c in objData.StudentPersonals where c.StudentType == "Referral" && c.SchoolId == Schoolid orderby c.StudentPersonalId select c;
                        result = (from u in SelectRefDashboard.
                             Where(x => objDashbrdSearch.SearchArgument == null ||
                       x.LastName.StartsWith(objDashbrdSearch.SearchArgument))
                                  from a in objData.ref_QueueStatus
                                                   .Where(x => u.StudentPersonalId == x.StudentPersonalId && u.StudentType == "Referral")
                                                   .DefaultIfEmpty()
                                  from s in objData.ref_Queue
                                                   .Where(x => a.QueueStatusId == x.QueueId)
                                                   .DefaultIfEmpty()
                                  from t in objData.Users
                                                   .Where(x => a.CreatedBy == x.UserId)
                                                   .DefaultIfEmpty()


                                  select new DashboardModelClass
                                  {
                                      ReferralId = u.StudentPersonalId,
                                      ReferralName = u.LastName + " , " + u.FirstName,
                                      ReferralDob = u.BirthDate,
                                      ReferralGender = u.Gender,
                                      ApplicationDate = u.CreatedOn,
                                      CompletedStep = s.QueueName,
                                      CompletedBy = t.UserLName + " , " + t.UserFName,
                                      ImageUrl = u.ImageUrl,
                                      ActiveProcess = s.QueueName
                                  }).ToList();

                    }
                }
                else
                {
                    result = DashboardSearchList(objDashbrdSearch, result);
                }
                result = result.Skip(Convert.ToInt32(objDashbrdSearch.PagingArgument) * (10 - 1)).Take(10).ToList();
                if (result.Count == pagesize)
                {
                    dec = 1;
                    flag = ">";
                }

            }
            objDashbrdSearch.flag = flag;
            objDashbrdSearch.perPage = page;
            //string gender = "";
            //if (result != null && result.Count > 0)
            //{

            //    for (int i = 0; i < result.Count - dec; i++)
            //    {
            //        objDashbrdSearch.itemCount = 1;
            //        DashboardModelClass model = new DashboardModelClass
            //        {
            //            ReferralId = result[i].ReferralId,
            //            ReferralName= result[i].ReferralName,
            //            ReferralDob = result[i].ReferralDob,
            //            ReferralGender = result[i].ReferralGender,
            //            ApplicationDate = result[i].ApplicationDate,
            //            CompletedStep = result[i].CompletedStep,
            //            CompletedBy = result[i].CompletedBy,
            //            NumberOfCompletion = "",
            //            ImageUrl = result[i].ImageUrl,
            //            ActiveProcess = result[i].ActiveProcess
            //        };
            //        TimeSpan tempDatetime;
            //        try
            //        {
            //            tempDatetime = DateTime.Now - (DateTime)result[i].ReferralDob;
            //        }
            //        catch
            //        {
            //            tempDatetime = new TimeSpan();
            //        }


            //    }
            //}
            ReferralDashboardModel refobj = new ReferralDashboardModel();
            refobj.DataContent = result;

            return refobj;
        }


        private static IList<DashboardModelClass> DashboardSearchList(SearchModel objdashbrdSearch, IList<DashboardModelClass> result)
        {
            IList<DashboardModelClass> results = new List<DashboardModelClass>();

            switch (objdashbrdSearch.SearchArgument)
            {
                case "Age":
                    results = result.OrderByDescending(p => p.ReferralDob).ToList();
                    results = results.Skip(Convert.ToInt32(objdashbrdSearch.PagingArgument) * (10 - 1)).Take(10).ToList();
                    break;
                case "DateOfBirth":
                    results = result.OrderByDescending(p => p.ReferralDob).ToList();
                    results = results.Skip(Convert.ToInt32(objdashbrdSearch.PagingArgument) * (10 - 1)).Take(10).ToList();
                    break;
                default:
                    results = result.Skip(Convert.ToInt32(objdashbrdSearch.PagingArgument) * (10 - 1)).Take(10).ToList();
                    break;

            }
            return results;
        }
    }




    public class DashboardModelClass
    {
        public int ReferralId { get; set; }
        public string ReferralName { get; set; }
        public DateTime? ReferralDob { get; set; }
        public string ReferralGender { get; set; }
        public DateTime? ApplicationDate { get; set; }
        public string CompletedStep { get; set; }
        public string CompletedBy { get; set; }
        public double? Perc { get; set; }
        public string ImageUrl { get; set; }
        public string ActiveProcess { get; set; }
        public string QueueType { get; set; }
        public string PermissionStatus { get; set; }
        public int age { get; set; }
    }

    public class SubQueueModelClass
    {
        public int QueueId { get; set; }
        public string QueueName { get; set; }
        public int MasterId { get; set; }
        public string QueueType { get; set; }
        public int PrevQueueId { get; set; }
        public int? SortOrder { get; set; }
    }

    public class QueueModelClass
    {
        public int QueueId { get; set; }
        public string QueueName { get; set; }
        public string QueueType { get; set; }
    }

    public class ActivityModelClass
    {
        public int ReferralId { get; set; }
        public string SType { get; set; }
        public string ReferralName { get; set; }
        public string RefGender { get; set; }
        public string RefUrl { get; set; }
        public string CurrentStep { get; set; }
        public string UserName { get; set; }
        public string Status { get; set; }
        public int QueueStatusID { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }


    public class NotificationClass
    {
        public int QueueId { get; set; }
        public int? AttentionNeeded { get; set; }
        public int Newqueue { get; set; }
        public int RefTotal { get; set; }
    }



}