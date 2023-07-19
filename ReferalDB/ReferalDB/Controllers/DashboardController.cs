using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using ReferalDB.Models;
using BuisinessLayer;
using ReferalDB.CommonClass;
using System.IO;
using System.Configuration;
using System.Web.Configuration;

namespace ReferalDB.Controllers
{
    public class DashboardController : Controller
    {

        // GET: /Dashboard/
        public NoteModel Objnte = null;
        public MelmarkDBEntities objData = null;
        public clsSession sess = null;
        ref_Notes Ntobj = new ref_Notes();





        //public ActionResult Index()
        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult Dashboard()
        {
            string title = System.Configuration.ConfigurationManager.AppSettings["Server"].ToString();
            if (title == "PA")
                ViewBag.Title = "Melmark Pennsylvania";
            else
                ViewBag.Title = "Melmark New England";
            setSession();
            sess = (clsSession)Session["UserSession"];
            sess.ReferralId = 0;
            ClsCommon getCommon = new ClsCommon();
            if (sess != null)
                ViewBag.permission = getCommon.setPermission();
            //ReferralDashboardModel returnModel = new ReferralDashboardModel();
            //if (sess != null)
            //{
            //    returnModel = ReferralDashboardModel.BindQueueList(sess.SchoolId);
            //}
            return View("MainPanel");//, returnModel);
        }

        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult GetTitle()
        {
            string title = System.Configuration.ConfigurationManager.AppSettings["Server"].ToString();
            if (title == "PA")
                title = "Melmark Pennsylvania";
            else
                title = "Melmark New England";
            return Json(title, JsonRequestBehavior.AllowGet);
        }
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult Dashboard_refMode()
        {
            setSession();
            sess = (clsSession)Session["UserSession"];
            ClsCommon getCommon = new ClsCommon();
            if (sess != null)
                ViewBag.permission = getCommon.setPermission();
            ViewBag.Title = GetTitleReport();
            //ReferralDashboardModel returnModel = new ReferralDashboardModel();
            //if (sess != null)
            //{
            //    returnModel = ReferralDashboardModel.BindQueueList(sess.SchoolId);
            //}
            return View("MainPanel_refMode");//, returnModel);
        }


        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public string GetTitleReport()
        {
            string title = System.Configuration.ConfigurationManager.AppSettings["Server"].ToString();
            if (title == "PA")
                title = "Melmark Pennsylvania";
            else
                title = "Melmark New England";
            return title;

        }
        //public void TestDigitals()
        //{
        //    DbFunctions obj = new DbFunctions();
        //    obj.SignDocument(0, 0);
        //}


        private void setSession()
        {
            sess = new clsSession();
            // sess = (clsSession)Session["UserSession"];
            MelmarkDBEntities Objdata = new MelmarkDBEntities();

            if (Session["Values"] != null)
            {
                string Values = Session["Values"].ToString();
                string[] arValues = Values.Split('#');
                sess.LoginId = Convert.ToInt16(arValues[1]);
                sess.SchoolId = Convert.ToInt16(arValues[0]);
                //sess.UserName = Convert.ToString(arValues[3]);

                if (sess.LoginId != 0)              //set the remaining session used in the Referral DB
                {
                    var Role = (from Objrole in Objdata.Roles
                                join objrgp in Objdata.RoleGroups on Objrole.RoleId equals objrgp.RoleId
                                select new
                                {
                                    RoleId = Objrole.RoleId,
                                    Roledesc = Objrole.RoleDesc,
                                    SchoolId = Objrole.SchoolId
                                }).ToList();
                    var Usr = (from Objrole in Role
                               from Objusr in Objdata.Users
                               where Objusr.UserId == sess.LoginId
                               select new
                               {
                                   Objrole.RoleId,
                                   Objrole.Roledesc,
                                   Objusr.UserId,
                                   Objusr.UserFName,
                                   Objusr.UserLName,
                                   Objusr.Gender
                               }).ToList();

                    if (Usr == null) return;
                    if (Usr.Count() > 0)
                    {

                        sess.IsLogin = true;
                        sess.LoginTime = (DateTime.Now.ToShortTimeString()).ToString();
                        sess.LoginId = Convert.ToInt32(Usr[0].UserId);
                        sess.UserName = Convert.ToString(Usr[0].UserLName + "," + Usr[0].UserFName);
                        sess.RoleId = Convert.ToInt32(Usr[0].RoleId);
                        sess.Gender = Convert.ToString(Usr[0].Gender);
                        sess.RoleName = Convert.ToString(Usr[0].Roledesc);
                        sess.SessionID = Session.SessionID.ToString();
                        sess.ReferralId = 0;
                    }
                }

                Session["UserSession"] = sess;
            }

        }

        //To load Reports

        // Comment out Because the url to report is directly given to the client side of report Icon.

        //[OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        //public ActionResult LoadReport()
        //{
        //    string path = WebConfigurationManager.AppSettings["ReportPathLoad"].ToString();
        //    return Redirect(path);
        //}

        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult DefaultDashboard(string Name)
        {
            MelmarkDBEntities Objdata = new MelmarkDBEntities();
            ViewBag.queueName = "";
            string SType = "Referral", Queue = "";
            string[] Queues = Name.Split(',');
            int clientQueueId = 0;
            Queue = Queues[1];

            if (Queues[0] == "CL")
            {

                var clientQueue = Objdata.ref_Queue.Where(x => x.QueueType == "CL").SingleOrDefault();
                clientQueueId = clientQueue.QueueId;
                Name = clientQueueId.ToString() + "," + Queues[1];
            }

            if (Queue == "Client List")
            {
                SType = "Client";
            }

            sess = (clsSession)Session["UserSession"];
            ReferralDashboardModel returnModel = new ReferralDashboardModel();
            ClsCommon getCommon = new ClsCommon();
            if (sess != null)
            {
                ViewBag.permission = getCommon.setPermission();
                //sess.ReferralId = 0;
                ViewBag.queueName = Queue;
                returnModel = ReferralDashboardModel.BindDashboard(sess.SchoolId, Name, SType);
            }
            return View("Dashboard", returnModel);
            //return Content("bnehoidjsfasdasd");
        }

        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult GetLeftMenu()
        {
            sess = (clsSession)Session["UserSession"];
            ReferralDashboardModel returnModel = new ReferralDashboardModel();
            ClsCommon getCommon = new ClsCommon();
            if (sess != null)
            {
                ViewBag.permission = getCommon.setPermission();
                returnModel = ReferralDashboardModel.BindQueueList(sess.SchoolId);
            }
            return View("DashboardMenu", returnModel);
        }

        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult GetLeftMenu_refMode()
        {
            sess = (clsSession)Session["UserSession"];
            ReferralDashboardModel returnModel = new ReferralDashboardModel();
            ClsCommon getCommon = new ClsCommon();
            if (sess != null)
            {
                ViewBag.permission = getCommon.setPermission();
                returnModel = ReferralDashboardModel.BindQueueList(sess.SchoolId);
            }
            return View("DashboardMenu_refMode", returnModel);
        }

        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult BindNotes()
        {
            sess = (clsSession)Session["UserSession"];
            NoteModel returnModel = new NoteModel();
            if (sess != null)
            {
                returnModel = NoteModel.BindNote(sess.SchoolId);
            }
            return View("Notes", returnModel);
        }

        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult BindActivity()
        {
            sess = (clsSession)Session["UserSession"];
            ReferralDashboardModel returnModel = new ReferralDashboardModel();
            if (sess != null)
            {
                returnModel = ReferralDashboardModel.BindActivity(sess.SchoolId, sess.ReferralId);
            }
            return View("DashboardActivity", returnModel);
        }

        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult SaveNotes(string id)
        {
            Objnte = new NoteModel();
            objData = new MelmarkDBEntities();
            sess = (clsSession)Session["UserSession"];
            if (sess != null)
            {
                //////Save notes//////
                Ntobj.SchoolId = sess.SchoolId;
                Ntobj.StudentPersonalId = sess.ReferralId;
                Ntobj.Notes = id;
                Ntobj.CreatedBy = sess.LoginId;
                Ntobj.CreatedOn = DateTime.Now;
                Ntobj.ModifiedBy = sess.LoginId;
                Ntobj.ModifiedOn = DateTime.Now;
                objData.ref_Notes.Add(Ntobj);
                objData.SaveChanges();
                Objnte = NoteModel.BindNote(sess.SchoolId);
            }
            return View("Notes", Objnte);
        }

        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult ListDashboards(string argument = "*", bool bSort = false, string Data = "0*k")
        {

            sess = (clsSession)Session["UserSession"];
            SearchModel search = new SearchModel();
            ReferralDashboardModel bindObj = new ReferralDashboardModel();
            if (sess != null)
            {
                search.SearchArgument = argument;
                search.SortStatus = bSort;
                search.PagingArgument = Data;
                ViewBag.curval = "";
                ViewBag.flage = "";
                ViewBag.SearchArg = "";
                ViewBag.itemCount = 0;
                if ((bSort == false) && (argument != "*"))
                {
                    ViewBag.SearchArg = argument;
                }
                ViewBag.SortArg = argument;
                bindObj = ReferralDashboardModel.fillDashboard(search, sess.SchoolId);
                ViewBag.flage = search.flag;
                ViewBag.curval = search.perPage;
                ViewBag.itemCount = search.itemCount;
            }
            return View("Dashboard", bindObj);

        }

        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public string SelectReferral(string Refid)// Refid is a combination of Referralid_CurrentQueueId
        {
            objData = new MelmarkDBEntities();
            ClsCommon com = new ClsCommon();
            string[] refData = Refid.Split('_');
            int refid = Convert.ToInt32(refData[0]);
            string Result = "";
            string IsQueue = "true";
            int masterId = 0;
            sess = (clsSession)Session["UserSession"];
            if (sess != null)
            {
                int processid = 0;
                sess.ReferralId = refid;
                if (Convert.ToInt32(refData[1]) == -1) //Active Process is -1 indicates the process is not in Queue Ie, it is in (Active or inactive or waiting)
                {
                    processid = ReferralStatus(refid);
                }
                else
                {
                    if (refData[1] == "999")
                    {
                        int CurrentId = com.getProcessIdForStudent(refid);
                        object obj = com.getQueueIdCurrent(CurrentId, refid);
                        if (obj != null) refData[1] = com.getQueueIdCurrent(CurrentId, refid).ToString();
                    }
                    processid = Convert.ToInt32(refData[1]);
                    if (processid > 0)
                    {
                        var QueueStatus = objData.ref_Queue.Where(objque => objque.QueueId == processid).SingleOrDefault();
                        if (QueueStatus.QueueType != "CL")
                        {
                            if (QueueStatus.MasterId == 0)
                            {
                                IsQueue = "false";
                            }
                            else
                            {
                                masterId = QueueStatus.MasterId;
                            }
                        }
                        else
                        {
                            IsQueue = "true";
                        }
                    }
                }
                sess.CurrentProcessId = processid;
                string QType = "", QName = "";
                if (processid == 0)
                {
                    QType = "AR";
                    QName = "Academic Review";
                }
                else
                {
                    var queuetype = objData.ref_Queue.Where(x => x.QueueId == processid).ToList();
                    QType = queuetype[0].QueueType;
                    QName = queuetype[0].QueueName;
                }
                Result = QType + "_" + QName + "_" + processid + "_" + IsQueue + "_" + masterId;
            }
            return Result;
        }

        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public int ReferralStatus(int Refid)
        {
            objData = new MelmarkDBEntities();
            ref_QueueStatus QStatus = new ref_QueueStatus();
            int NewQueueId = 0;
            string QType = "";
            var stdPer = objData.StudentPersonals.Where(x => x.StudentPersonalId == Refid).SingleOrDefault();
            if (Convert.ToBoolean(Session["OldStatus"]) == true)
            {
                QType = "IL";
            }
            else
            {
                var Queueids = objData.ref_QueueStatus.Where(x => x.StudentPersonalId == Refid && x.CurrentStatus == true).ToList();
                int QueueId = Convert.ToInt32(Queueids[0].QueueId);
                var QueueType = objData.ref_Queue.Where(x => x.QueueId == QueueId).ToList();
                QType = QueueType[0].QueueType;
            }

            var ProcessStatus = objData.ref_QueueStatus.Where(x => x.StudentPersonalId == Refid && x.CurrentStatus == true).ToList();
            int QueueProcessId = ProcessStatus[0].QueueProcess;
            QStatus = objData.ref_QueueStatus.Where(x => x.StudentPersonalId == Refid && x.CurrentStatus == true).First();
            bool IsFund = false;

            if (QType == "IL")
            {
                QStatus.Draft = "N";
                QStatus.CurrentStatus = false;
                QStatus.ModifiedBy = sess.LoginId;
                QStatus.ModifiedOn = DateTime.Now;
                QStatus.EndDate = DateTime.Now;
                objData.SaveChanges();
                int IL = 0, WL = 0, AL = 0, NA = 0, CL = 0;
                var NewQIds = objData.ref_Queue.Where(x => x.QueueType == "NA").ToList();
                NA = Convert.ToInt32(NewQIds[0].QueueId);
                NewQIds = objData.ref_Queue.Where(x => x.QueueType == "AV").ToList();
                AL = Convert.ToInt32(NewQIds[0].QueueId);
                NewQIds = objData.ref_Queue.Where(x => x.QueueType == "WL").ToList();
                WL = Convert.ToInt32(NewQIds[0].QueueId);
                NewQIds = objData.ref_Queue.Where(x => x.QueueType == "IL").ToList();
                IL = Convert.ToInt32(NewQIds[0].QueueId);
                NewQIds = objData.ref_Queue.Where(x => x.QueueType == "CL").ToList();
                CL = Convert.ToInt32(NewQIds[0].QueueId);



                var QStatusList = objData.ref_QueueStatus.Where(x => x.StudentPersonalId == Refid).ToList();
                foreach (var item in QStatusList)
                {
                    if (item.QueueId != NA && item.QueueId != AL && item.QueueId != WL && item.QueueId != CL && item.QueueId != IL)
                    {
                        QStatus = new ref_QueueStatus();
                        QStatus = objData.ref_QueueStatus.Where(x => x.StudentPersonalId == Refid && x.QueueStatusId == item.QueueStatusId).SingleOrDefault();
                        QStatus.Draft = "Y";
                        objData.SaveChanges();
                    }
                }

                var NewQueueIds = objData.ref_Queue.Where(x => x.QueueType == "AR").ToList();
                NewQueueId = Convert.ToInt32(NewQueueIds[0].QueueId);

                QStatus = new ref_QueueStatus();
                QStatus = objData.ref_QueueStatus.Where(x => x.StudentPersonalId == Refid && x.QueueId == NewQueueId).First();
                QStatus.Draft = "Y";
                QStatus.CurrentStatus = true;
                QStatus.ModifiedBy = sess.LoginId;
                QStatus.ModifiedOn = DateTime.Now;
                QStatus.EndDate = DateTime.Now;
                objData.SaveChanges();

                //ref_QueueStatus QUEUEStatus = new ref_QueueStatus();
                //QUEUEStatus.CreatedBy = sess.LoginId;
                //QUEUEStatus.CreatedOn = DateTime.Now;
                //QUEUEStatus.Draft = "Y";
                //QUEUEStatus.CurrentStatus = true;
                //QUEUEStatus.StartDate = DateTime.Now;
                //QUEUEStatus.EndDate = DateTime.Now;
                //QUEUEStatus.QueueId = NewQueueId;
                //QUEUEStatus.QueueProcess = QueueProcessId + 1;
                //QUEUEStatus.SchoolId = sess.SchoolId;
                //QUEUEStatus.StudentPersonalId = sess.ReferralId;
                //objData.ref_QueueStatus.Add(QUEUEStatus);
                //objData.SaveChanges();
            }
            else
            {
                if (QType == "AV")
                {
                    var NewQueueIds = objData.ref_Queue.Where(x => x.QueueType == "MO").ToList();
                    NewQueueId = Convert.ToInt32(NewQueueIds[0].QueueId);
                    QStatus.Draft = "N";
                    QStatus.CurrentStatus = false;
                    objData.SaveChanges();
                }
                else if (QType == "WL")
                {
                    var NewQueueIds = objData.ref_Queue.Where(x => x.QueueType == "FV").ToList();
                    var NewQueueIdFund = Convert.ToInt32(NewQueueIds[0].QueueId);
                    var qStatusUpdate = objData.ref_QueueStatus.Where(x => x.StudentPersonalId == sess.ReferralId && x.QueueId == NewQueueIdFund && x.QueueProcess == QStatus.QueueProcess).OrderByDescending(x => x.QueueStatusId).FirstOrDefault();
                    qStatusUpdate.Draft = "Y";
                    qStatusUpdate.CurrentStatus = true;
                    objData.SaveChanges();

                    IsFund = true;
                    NewQueueIds = objData.ref_Queue.Where(x => x.QueueType == "WL").ToList();
                    NewQueueId = Convert.ToInt32(NewQueueIds[0].QueueId);
                    qStatusUpdate = objData.ref_QueueStatus.Where(x => x.StudentPersonalId == sess.ReferralId && x.QueueId == NewQueueId && x.QueueProcess == QStatus.QueueProcess).OrderByDescending(x => x.QueueStatusId).FirstOrDefault();
                    qStatusUpdate.Draft = "N";
                    qStatusUpdate.CurrentStatus = false;
                    objData.SaveChanges();

                    NewQueueId = NewQueueIdFund;
                }
                if (IsFund == false)
                {
                    var qStatusRow = objData.ref_QueueStatus.Where(x => x.StudentPersonalId == sess.ReferralId && x.QueueId == NewQueueId && x.QueueProcess == QStatus.QueueProcess).ToList();
                    if (qStatusRow.Count == 0)
                    {
                        QStatus.Draft = "N";
                        QStatus.CurrentStatus = false;
                        QStatus.ModifiedBy = sess.LoginId;
                        QStatus.ModifiedOn = DateTime.Now;
                        QStatus.EndDate = DateTime.Now;
                        objData.SaveChanges();
                        ref_QueueStatus QUEUEStatus = new ref_QueueStatus();
                        QUEUEStatus.CreatedBy = sess.LoginId;
                        QUEUEStatus.CreatedOn = DateTime.Now;
                        QUEUEStatus.Draft = "Y";
                        QUEUEStatus.CurrentStatus = true;
                        QUEUEStatus.StartDate = DateTime.Now;
                        QUEUEStatus.EndDate = DateTime.Now;
                        QUEUEStatus.QueueId = NewQueueId;
                        QUEUEStatus.QueueProcess = QStatus.QueueProcess;
                        QUEUEStatus.SchoolId = sess.SchoolId;
                        QUEUEStatus.StudentPersonalId = sess.ReferralId;
                        objData.ref_QueueStatus.Add(QUEUEStatus);
                        objData.SaveChanges();
                    }
                    else
                    {
                        //   qStatusRow[qStatusRow.Count - 1].Draft = "Y";
                        qStatusRow[qStatusRow.Count - 1].CurrentStatus = true;
                        objData.SaveChanges();
                    }
                }
            }

            return NewQueueId;
        }

        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public string ResetReferralID()
        {
            string Result = "false";
            sess = (clsSession)Session["UserSession"];
            if (sess != null)
            {
                sess.ReferralId = 0;
                Result = "true";
            }
            return Result;
        }


        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public string selectReferalStatus()
        {
            string Result = "false";
            string QType = "";

            ReferalDB.AppFunctions.Other_Functions OF = new AppFunctions.Other_Functions();
            if (OF.setPermission() == "false")
            {
                return "Client";
            }

            sess = (clsSession)Session["UserSession"];
            if (sess != null)
            {
                objData = new MelmarkDBEntities();
                ref_QueueStatus QStatus = new ref_QueueStatus();
                if (sess.ReferralId > 0)
                {
                    var stdPer = objData.StudentPersonals.Where(x => x.StudentPersonalId == sess.ReferralId).SingleOrDefault();
                    QType = stdPer.StudentType;

                }
            }
            return QType;
        }


        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public string ProcessStatus(int QueueId)
        {
            objData = new MelmarkDBEntities();
            ClsCommon Common = new ClsCommon();
            sess = (clsSession)Session["UserSession"];
            string Result = "NA";
            int processId = 0;
            int QueueProcss = 0;
            if (sess != null)
            {

                var process = objData.ref_QueueStatus.Where(x => x.CurrentStatus == true && x.StudentPersonalId == sess.ReferralId).ToList();
                if (process.Count > 0)
                {
                    processId = process[0].QueueId;
                    QueueProcss = process[0].QueueProcess;
                }


                if (processId > 0)
                    sess.CurrentProcessId = processId;
                if (sess.CurrentProcessId != 0)
                {
                    if (QueueId > sess.CurrentProcessId)
                    {
                        var SelectedQueueStatus = objData.ref_Queue.Where(x => x.QueueId == QueueId).ToList();
                        if (SelectedQueueStatus.Count > 0)
                        {
                            int prevQId = SelectedQueueStatus[0].PrevQueueId;
                            if (prevQId != 0)
                            {
                                var Qname = objData.ref_Queue.Where(x => x.QueueId == prevQId).ToList();
                                string QueueName = Qname[0].QueueName;
                                var prevQueueStatus = objData.ref_QueueStatus.Where(x => x.QueueId == prevQId && x.CurrentStatus == false && x.StudentPersonalId == sess.ReferralId && x.QueueProcess == QueueProcss).ToList();
                                if (prevQueueStatus.Count == 0)
                                    Result = QueueName + " is not submitted";
                            }
                        }
                    }
                    //var NewQueueStatus = objData.ref_QueueStatus.Where(x => x.QueueId == sess.CurrentProcessId && x.CurrentStatus == true && x.StudentPersonalId == sess.ReferralId).ToList();
                    //if (NewQueueStatus.Count > 0)
                    //{
                    //    string DraftStatus = NewQueueStatus[0].Draft;
                    //    var Qname = objData.ref_Queue.Where(x => x.QueueId == sess.CurrentProcessId).ToList();
                    //    string QueueName = Qname[0].QueueName;
                    //    var QueueType = objData.ref_Queue.Where(x => x.QueueId == QueueId).ToList();
                    //    string QType = QueueType[0].QueueType;
                    //    if (QType != "NA")
                    //    {

                    //        var NotQueue = objData.ref_Queue.Where(objque => objque.QueueType == "WL" || objque.QueueType == "AV" || objque.QueueType == "IL").ToList().Select(objque => objque.QueueId);

                    //        if (QueueId > sess.CurrentProcessId || NotQueue.Contains(sess.CurrentProcessId) == true)
                    //        {
                    //            Result = QueueName + " is not submitted";
                    //        }

                    //    }
                    //}
                }
            }
            Result = "NA";
            return Result;
        }

        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult SearchAndFilterDashboard(string Name)
        {
            sess = (clsSession)Session["UserSession"];
            ReferralDashboardModel returnModel = new ReferralDashboardModel();
            ClsCommon getCommon = new ClsCommon();
            clsDashboard objclsDashbrd = new clsDashboard();
            ViewBag.Sort = "";
            if (sess != null)
            {
                ViewBag.permission = getCommon.setPermission();
                string[] SearchArgs = Name.Split('_');
                ViewBag.Sort = SearchArgs[6];
                returnModel.DataContent = objclsDashbrd.FillDashboard(sess.SchoolId, SearchArgs[0], SearchArgs[1], SearchArgs[2], SearchArgs[3], SearchArgs[4], SearchArgs[5], SearchArgs[6], SearchArgs[7], SearchArgs[8]);
            }
            return View("Dashboard", returnModel);
            //return Content("bnehoidjsfasdasd");
        }

        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public string SelectQueueName(int QueueId)
        {
            objData = new MelmarkDBEntities();
            var Queue = objData.ref_Queue.Where(objque => objque.QueueId == QueueId).SingleOrDefault();
            return Queue.QueueName;
        }

        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public string SelectQueueType(int QueueId)
        {
            objData = new MelmarkDBEntities();
            var Queue = objData.ref_Queue.Where(objque => objque.QueueId == QueueId).SingleOrDefault();
            return Queue.QueueType;
        }

        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public int SelectQueueID(string QueueType)
        {
            objData = new MelmarkDBEntities();
            var Queue = objData.ref_Queue.Where(objque => objque.QueueType == QueueType).SingleOrDefault();
            return Queue.QueueId;
        }


        //Notification
        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult GetNotifications(NotificationModel model)
        {
            sess = (clsSession)Session["UserSession"];
            if (sess != null)
            {
                model.GetNotifications(sess.LoginId, sess.SchoolId);
            }
            return View(model);
        }

        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public string GetTabId(int QueueId)
        {
            string Tabid = "";
            objData = new MelmarkDBEntities();
            var Queue = objData.ref_Queue.Where(objque => objque.QueueId == QueueId).SingleOrDefault();
            if (Queue.MasterId == 0)
            {
                Tabid = QueueId + "," + Queue.QueueName;

            }
            else
            {
                var tab = objData.ref_Queue.Where(objque => objque.QueueId == Queue.MasterId).SingleOrDefault();
                Tabid = tab.QueueId + "," + tab.QueueName;
            }
            return Tabid;
        }


        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public string GetUserName()
        {
            sess = (clsSession)Session["UserSession"];
            string UName = "";
            if (sess != null)
            {
                UName = sess.UserName;
            }
            return UName;
        }

        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public string GetReferralName()
        {
            sess = (clsSession)Session["UserSession"];
            string RName = "";
            if (sess != null)
            {
                objData = new MelmarkDBEntities();
                var Refname = objData.StudentPersonals.Where(objref => objref.StudentPersonalId == sess.ReferralId).SingleOrDefault();
                if (Refname != null)
                    RName = Refname.LastName + "," + Refname.FirstName;
            }
            return RName;
        }

        public string MakeClientImmediately(int studId)
        {
            string school = ConfigurationManager.ConnectionStrings["dbConnectionString"].ToString();
            sess = (clsSession)Session["UserSession"];
            string RName = "";
            if (sess != null)
            {
                objData = new MelmarkDBEntities();

                var LocalId = objData.StudentPersonals.Max(objsp => objsp.ClientId).Value;
                
                var Refname = objData.StudentPersonals.Where(objref => objref.StudentPersonalId == sess.ReferralId).SingleOrDefault();
                if (Refname.StudentType == "Referral")
                {
                    if (Refname != null)
                    {

                        if (LocalId == 0)
                        {
                            if (sess.SchoolId == 1)
                                Refname.ClientId = 10000;
                            else
                                Refname.ClientId = 50000;
                        }
                        else
                            Refname.ClientId = LocalId + 1;

                        Refname.StudentType = "Client";
                    }

                    int result = objData.SaveChanges();
                    if (result > 0)
                    {
                        return "Success";
                    }
                }
                else
                {
                    return "isClient";
                }
            }

            return "Failed";
        }


        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public string DeleteReferral(int Id)
        {
            string Result = "";
            try
            {
                objData = new MelmarkDBEntities();
                StudentPersonal stupernl = (from x in objData.StudentPersonals where x.StudentPersonalId == Id select x).First();
                stupernl.StudentType = "Expired";
                objData.SaveChanges();


                ref_QueueStatus QueueStatus = (from x in objData.ref_QueueStatus where x.StudentPersonalId == Id select x).First();
                QueueStatus.CurrentStatus = false;
                objData.SaveChanges();
                Result = "Success";
            }
            catch (Exception)
            {
                Result = "Failed";
            }
            return Result;
        }

        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public string ServerDetails()
        {
            sess = (clsSession)Session["UserSession"];
            string Server = "";
            if (sess != null)
            {
                if (sess.SchoolId == 1)
                {
                    Server = "NE";
                }
                else if (sess.SchoolId == 2)
                {
                    Server = "PA";
                }
            }

            return Server;
        }


        #region Auto Complete

        public JsonResult AutoCompleteCountry(string term)
        {

            ClsCommon clsComn = new ClsCommon();
            IList<StudentSearchDetails> val = clsComn.GetStudentSearch(term);

            if (val != null)
            {

                var result = (from r in val
                              where r.ReferralName.ToLower().Contains(term.ToLower())
                              select new { r.ReferralName, r.ReferralId });
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(null, JsonRequestBehavior.AllowGet);

        }

        public JsonResult AutoCompleteRefsearch(string term)
        {

            ClsCommon clsComn = new ClsCommon();
            IList<StudentSearchDetails> val = clsComn.GetStudentSearch_ref(term);
            string[] args = term.Split('$');

            if (args.Length > 1)
            {
                term = args[0].ToString();
            }
            if (val != null)
            {

                var result = (from r in val
                              where r.ReferralName.ToLower().Contains(term.ToLower())
                              select new { r.ReferralName, r.ReferralName_short });
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(null, JsonRequestBehavior.AllowGet);

        }

        public JsonResult AutoCompleteStaffName(string term)
        {

            ClsCommon clsComn = new ClsCommon();
            IList<StaffSearchDetails> val = clsComn.GetStaffList(term);
            if (val != null)
            {

                var result = (from r in val
                              where (r.UserName.ToLower().Contains(term.ToLower()))
                              select new { r.UserName }).Distinct();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(null, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AutoCompleteContactName(string term)
        {

            ClsCommon clsComn = new ClsCommon();
            IList<ContactNameSearchDetails> val = clsComn.GetContactNameList(term);
            if (val != null)
            {

                var result = (from r in val
                              where (r.ContactName.ToLower().Contains(term.ToLower()))
                              select new { r.ContactName, r.ContactId }).Distinct();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(null, JsonRequestBehavior.AllowGet);
        }

        #endregion



        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public string changeRefQueuestatus(string type)
        {


            sess = (clsSession)Session["UserSession"];

            MelmarkDBEntities objData = new MelmarkDBEntities();
            StudentPersonal SPObj = new StudentPersonal();
            ClsCommon updateCommon = new ClsCommon();

            var StudPersonal = objData.StudentPersonals.Where(x => x.StudentPersonalId == sess.ReferralId).SingleOrDefault();
            Session["OldStatus"] = null;
            switch (type)
            {

                case "MI":
                    StudPersonal.InactiveList = true;
                    StudPersonal.WaitingList = false;
                    break;
                case "MTW":
                    StudPersonal.WaitingList = true;
                    StudPersonal.InactiveList = false;
                    break;
                case "RFW":
                    StudPersonal.WaitingList = false;
                    StudPersonal.InactiveList = false;
                    break;
                case "MA":
                    Session["OldStatus"] = true;
                    StudPersonal.InactiveList = false;
                    StudPersonal.WaitingList = false;
                    break;
            }
            try
            {
                objData.SaveChanges();
                return "Succes";
            }
            catch (Exception exp)
            {
                throw exp;
                return "Failed";
            }
        }

    }
}
