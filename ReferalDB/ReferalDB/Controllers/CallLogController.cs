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
using System.Web.Services;
namespace ReferalDB.Controllers
{
    public class CallLogController : Controller
    {
        //
        // GET: /CallLog/
        public clsSession sess = null;
        public ActionResult Index()
        {
            return View();
        }
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult CallLog(CommonCallLogViewModel model)
        {
            sess = (clsSession)Session["UserSession"];
            MelmarkDBEntities objData = new MelmarkDBEntities();
            IList<LookUp> Relation = new List<LookUp>();
            IList<LookUp> CalltypeLookup = new List<LookUp>();
            SelectListItem onesele = new SelectListItem { Text = "-- Select --", Value = "" };
            SelectListItem oneselecall = new SelectListItem { Text = "-- Select --", Value = "" };
            IList<SelectListItem> relationSelecteditem = new List<SelectListItem>();
            IList<SelectListItem> callTypeSelectedItem = new List<SelectListItem>();
            relationSelecteditem.Add(onesele);
            callTypeSelectedItem.Add(oneselecall);
            try
            {
                Relation = objData.LookUps.Where(x => x.LookupType == "Relationship").OrderBy(x => x.LookupName).ToList();
            }
            catch (Exception ex)
            {
            }
            try
            {
                CalltypeLookup = objData.LookUps.Where(x => x.LookupType == "Calllog Type").OrderBy(x => x.LookupName).ToList();

            }
            catch(Exception ex)
            {

            }
            var relationSelecteditemsub = (from Relationship in Relation select new SelectListItem { Text = Relationship.LookupName, Value = Relationship.LookupId.ToString() }).ToList();
            foreach (SelectListItem sele in relationSelecteditemsub)
            {
                relationSelecteditem.Add(sele);
            }

            var cntactType = (from ContactlogType in CalltypeLookup select new SelectListItem { Text = ContactlogType.LookupName, Value = ContactlogType.LookupId.ToString() }).ToList();
            foreach (SelectListItem sele in cntactType)
            {
                callTypeSelectedItem.Add(sele);
            }
           


            model.RelationshipList = relationSelecteditem;
            model.ContactlogTypeList = callTypeSelectedItem;
            model.CallDateShow = DateTime.Now.Date.ToString("MM/dd/yyyy");
            model.CallTimeShow = DateTime.Now.ToString("hh:mmtt");
            model.StaffName = sess.UserName;
            return View(model);
        }

        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult CallLog2(CommonCallLog2ViewModel model, int CallLogid = 0)
        {
            sess = (clsSession)Session["UserSession"];
            MelmarkDBEntities objData = new MelmarkDBEntities();
            IList<LookUp> Relation = new List<LookUp>();

            ReferalDB.AppFunctions.Other_Functions OF = new ReferalDB.AppFunctions.Other_Functions();
            ViewBag.permission = OF.setPermission();

            SelectListItem onesele = new SelectListItem { Text = "-- Select --", Value = "" };
            IList<SelectListItem> relationSelecteditem = new List<SelectListItem>();
            relationSelecteditem.Add(onesele);
            try
            {
                Relation = objData.LookUps.Where(x => x.LookupType == "Relationship").OrderBy(x => x.LookupName).ToList();
            }
            catch (Exception ex)
            {
            }
            var relationSelecteditemsub = (from Relationship in Relation select new SelectListItem { Text = Relationship.LookupName, Value = Relationship.LookupId.ToString() }).ToList();
            foreach (SelectListItem sele in relationSelecteditemsub)
            {
                relationSelecteditem.Add(sele);
            }
            model.RelationshipList = relationSelecteditem;
            if (CallLogid == 0)
            {
                model.CallDateShow2 = DateTime.Now.Date.ToString("MM/dd/yyyy").Replace("-", "/");
                model.CallTimeShow2 = DateTime.Now.ToString("hh:mmtt");
                model.StaffName2 = sess.UserName;
                if (sess.ReferralId > 0)
                {
                    var Refname = objData.StudentPersonals.Where(x => x.StudentPersonalId == sess.ReferralId).SingleOrDefault();
                    model.ReferralName2 = Refname.LastName + "," + Refname.FirstName;
                }
            }
            else
            {
                Session["CallLogId"] = CallLogid;
                model.CallLogId2 = CallLogid;
                var returnModel = objData.ref_CallLogs.Where(x => x.CallLogId == CallLogid).SingleOrDefault();
                model.CallDateShow2 = (returnModel.CallTime == null) ? null : Convert.ToDateTime(returnModel.CallTime).ToString("MM/dd/yyyy").Replace("-","/");
                model.CallTimeShow2 = (returnModel.CallTime == null) ? null : Convert.ToDateTime(returnModel.CallTime).ToString("hh:mmtt");
                model.StaffName2 = returnModel.StaffName;
                var Refname = objData.StudentPersonals.Where(x => x.StudentPersonalId == returnModel.StudentId).SingleOrDefault();
                model.ReferralName2 = Refname.LastName + "," + Refname.FirstName;
                model.Conversation2 = returnModel.Conversation;
                model.ContactlogType =Convert.ToString(returnModel.CallFlag);
                model.NameOfContact2 = returnModel.Nameofcontact;
            }
            return View(model);
        }

        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult SaveCallLog(CommonCallLogViewModel model)
        {

            string result = "";


            ViewBag.Chkmsg = "";
            ViewBag.Flag = 1;
            sess = (clsSession)Session["UserSession"];
            ClsCommon clsComm = new ClsCommon();
            if (sess != null)
            {

                result = clsComm.Save(sess.ReferralId, sess.SchoolId, sess.LoginId, model);

                ViewBag.Chkmsg = result;

            }
            return View("CallLog", model);
        }

        // Call log with ajax   
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        [MultiButton(MatchFormKey = "SaveCallLog", MatchFormValue = "Save")]
        public ActionResult SaveCallLog2_2(CommonCallLog2ViewModel model)
        {
            sess = (clsSession)Session["UserSession"];
            MelmarkDBEntities objData = new MelmarkDBEntities();
            ref_CallLogs Objcalllog = new ref_CallLogs();
            DateTime dtcalldate = new DateTime();
            ClsCommon ObjCls = new ClsCommon();
            if (sess != null)
            {
                int Type =Convert.ToInt32(model.ContactlogType);
                if (model.Relationship2 == "")
                    Objcalllog.RelationshipId = 0;
                else
                    Objcalllog.RelationshipId = Convert.ToInt32(model.Relationship2);
                Objcalllog.StudentId = sess.ReferralId;
                Objcalllog.SchoolId = sess.SchoolId;
                Objcalllog.StaffName = model.StaffName2;
                Objcalllog.CallFlag = Type;
                if (model.CallDateShow2 != null)
                    dtcalldate = DateTime.ParseExact(model.CallDateShow2, "MM'/'dd'/'yyyy", System.Globalization.CultureInfo.CurrentCulture);
                if (model.CallTimeShow2 == null && model.CallDateShow2 != null)
                    model.CallTimeShow2 = "00:00AM";
                if (model.CallDateShow2 != null)
                {
                    model.CallTime2 = dtcalldate.Add(TimeSpan.Parse(ObjCls.amPmTo24hourConverter(model.CallTimeShow2)));
                }
                Objcalllog.CallTime = model.CallTime2;
                Objcalllog.Nameofcontact = model.NameOfContact2;
                Objcalllog.AppointmentTime = model.AppntTime2;
                Objcalllog.Conversation = model.Conversation2;
                Objcalllog.CreatedBy = sess.LoginId;
                Objcalllog.CreatedOn = DateTime.Now;
                Objcalllog.Draft = "Y";
                Objcalllog.Type = "NA";
                Objcalllog.AcReviewId = 0;
                Objcalllog.QueueStatusId = 0;
                objData.ref_CallLogs.Add(Objcalllog);
                objData.SaveChanges();
                model.CallLogId2 = 0;


            }
            return Content("Success*" + sess.ReferralId.ToString());
        }


        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        [MultiButton(MatchFormKey = "SaveCallLog", MatchFormValue = "Update")]
        public ActionResult SaveCallLog2_2(CommonCallLog2ViewModel model,string a)
        {
            sess = (clsSession)Session["UserSession"];
            MelmarkDBEntities objData = new MelmarkDBEntities();
            ref_CallLogs Objcalllog = new ref_CallLogs();
            DateTime dtcalldate = new DateTime();
            ClsCommon ObjCls = new ClsCommon();
            if (sess != null)
            {
                if (Session["CallLogId"] != null)
                {
                    int callLogid = Convert.ToInt32(Session["CallLogId"]);
                    Objcalllog = objData.ref_CallLogs.Where(x => x.CallLogId == callLogid).SingleOrDefault();
                    int Type = Convert.ToInt32(model.ContactlogType);
                    Objcalllog.StudentId = sess.ReferralId;
                    Objcalllog.SchoolId = sess.SchoolId;
                    Objcalllog.StaffName = model.StaffName2;
                    Objcalllog.CallFlag = Type;
                    if (model.CallDateShow2 != null)
                        dtcalldate = DateTime.ParseExact(model.CallDateShow2, "MM'/'dd'/'yyyy", System.Globalization.CultureInfo.CurrentCulture);
                    if (model.CallTimeShow2 == null && model.CallDateShow2 != null)
                        model.CallTimeShow2 = "00:00AM";
                    if (model.CallDateShow2 != null)
                    {
                        model.CallTime2 = dtcalldate.Add(TimeSpan.Parse(ObjCls.amPmTo24hourConverter(model.CallTimeShow2)));
                    }
                    Objcalllog.CallTime = model.CallTime2;
                    Objcalllog.Nameofcontact = model.NameOfContact2;
                    Objcalllog.AppointmentTime = model.AppntTime2;
                    Objcalllog.Conversation = model.Conversation2;
                    objData.SaveChanges();
                }


            }
            return Content("Success*" + sess.ReferralId.ToString());
        }

        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public string SaveCallLog2(string value)
        {
            sess = (clsSession)Session["UserSession"];
            CommonCallLogViewModel model = new CommonCallLogViewModel();
            if ((value != null) || (value != ""))
            {
                string[] datas = value.Split('|');
                model.ReferralName = datas[0].ToString();
                model.NameOfContact = datas[1].ToString();
                model.Relationship = datas[2].ToString();
                model.CallDateShow = datas[3].ToString();
                model.CallTimeShow = datas[4].ToString();
                model.StaffName = datas[5].ToString();
                model.StudentId = sess.ReferralId;
                model.ContactlogType = Convert.ToString(datas[7]);
                model.Conversation = datas[8].ToString();
            }
            string result = "";


            ViewBag.Chkmsg = "";
            ViewBag.Flag = 1;
            sess = (clsSession)Session["UserSession"];
            ClsCommon clsComm = new ClsCommon();
            if (sess != null)
            {

                result = clsComm.Save(sess.ReferralId, sess.SchoolId, sess.LoginId, model);

                ViewBag.Chkmsg = result;

            }
            return result;
        }
        //end





        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult CallLogView(CommonCallLogViewModel model)
        {
            MelmarkDBEntities objData = new MelmarkDBEntities();
            CommonCallLogViewModel call = new CommonCallLogViewModel();
            model.CallLists = call.getCallLog();
            return View(model);
        }
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult callDetailsSelect(CommonCallLogViewModel Model, int callLogId = 0)
        {
            ClsCommon clsComn = new ClsCommon();
            sess = (clsSession)Session["UserSession"];
            CommonCallLogViewModel call = new CommonCallLogViewModel();
            if (callLogId != 0)

                Model.CallLists = call.getcallDetails(callLogId);
            //Model.Search = "YES";
            return View("callDetailsCon", Model);
        }
    }
}
