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
using System.Data.Objects.SqlClient;


namespace ReferalDB.Controllers
{
    public class IntakeAssessmentController : Controller
    {
        //
        // GET: /Intake Assessment/
        public clsSession sess = null;
        LetterTray getLetterTray = new LetterTray();
        LetterGenerationViewModel LetterGeneration = new LetterGenerationViewModel();
        public ActionResult Index()
        {
            return View();
        }

        //Schedule Appointment for Parent Tour
        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult ParantScreenShedule()
        {
            ClsCommon getCommon = new ClsCommon();
            sess = (clsSession)Session["UserSession"];
            if (sess != null)
                ViewBag.permission = getCommon.setPermission();
            ScheduleParentScreeningViewModel shAssmtShe = new ScheduleParentScreeningViewModel();

            bool InActivePresent = getCommon.checkInactive();
            shAssmtShe.ChkAll = getCommon.getCheckListMultiple("PT", "Check");
            shAssmtShe.Comment = getCommon.getRevCmt("PT");
            //shAssmtShe.CallLog = getCommon.getRevCallLog("PT", shAssmtShe.Comment.academicReviewId);
            shAssmtShe.CallLog = getCommon.getRevMultipleCallLog("PT", shAssmtShe.Comment.academicReviewId);
            if (shAssmtShe.CallLog.Count == 0)
            {
                CommonCallLogViewModel defaultmodel = new CommonCallLogViewModel();
                defaultmodel.StaffName = sess.UserName;
                shAssmtShe.CallLog.Add(defaultmodel);
            }
            if (shAssmtShe.Comment.AproveInt == true)
            {
                shAssmtShe.approvedStatus = 1;
            }
            return View("SchedulePrntScreening", shAssmtShe);
        }

        //[ActiveSession]
        [HttpPost, ValidateInput(false)]
        [MultiButton(MatchFormKey = "Save", MatchFormValue = "Save")]
        public ActionResult SavePrntScrmin(ScheduleParentScreeningViewModel model)
        {
            try
            {
                MelmarkDBEntities objData = new MelmarkDBEntities();
                ClsCommon updateCommon = new ClsCommon();
                foreach (var item in model.CallLog)
                {
                    DateTime dtcalldate = new DateTime();
                    DateTime dtappntdate = new DateTime();
                    if (item.CallDateShow != null)
                        dtcalldate = DateTime.ParseExact(item.CallDateShow, "MM'/'dd'/'yyyy", System.Globalization.CultureInfo.CurrentCulture);
                    if (item.CallTimeShow == null && item.CallDateShow != null)
                        item.CallTimeShow = "00:00AM";
                    if (item.CallDateShow != null)
                    {
                        item.CallTime = dtcalldate.Add(TimeSpan.Parse(amPmTo24hourConverter(item.CallTimeShow)));
                    }

                    if (item.AppntDateShow != null)
                        dtappntdate = DateTime.ParseExact(item.AppntDateShow, "MM'/'dd'/'yyyy", System.Globalization.CultureInfo.CurrentCulture);
                    if (item.AppntTimeShow == null && item.AppntDateShow != null)
                        item.AppntTimeShow = "00:00AM";
                    if (item.AppntDateShow != null)
                    {
                        item.AppntTime = dtappntdate.Add(TimeSpan.Parse(amPmTo24hourConverter(item.AppntTimeShow)));
                    }
                }

                model.Comment = updateCommon.UpdateOrInsertRevCmt("PT", model.Comment.academicReviewId, model.Comment.Comments, "Y", model.Comment.IsPresent, model.Comment.AproveInt, "PT");
                model.CallLog = updateCommon.UpdateOrInsertMultipleCallLog("PT", model.Comment.academicReviewId, "Y", model.CallLog, model.Comment.QstatusId);
                //model.CallLog = updateCommon.UpdateOrInsertCallLog("PT", model.Comment.academicReviewId, model.CallLog.AppntTime, "Y", model.CallLog.CallTime, model.CallLog.CallLogId, model.CallLog.Conversation, model.CallLog.NameOfContact, model.CallLog.StaffName, model.CallLog.IsPresent, model.Comment.QstatusId);
                model.ChkAll = updateCommon.UpdateOrInsertMulCheckList(model.ChkAll, model.Comment.academicReviewId, "PT", model.Comment.QstatusId, "Check");
                string returnVal = clsGeneral.sucessMsgWithoutQuote("Data Successfully Saved");

                //return Content("<script type='text/javascript'>$.get('/IntakeAssessment/ParantScreenShedule', function (result) {" +
                //    "$('#LoadQueue').html(result);" +
                //    "$('#tdMsg').html('" + returnVal + "');" +
                //"});</script>");

                return Content(returnVal);
            }
            catch (Exception e)
            {
                string returnVal = clsGeneral.failedMsgWithoutQuote(e.Message);
                // return Content("<script type='text/javascript'>$.get('/IntakeAssessment/ParantScreenShedule', function (result) {" +
                //    "$('#LoadQueue').html(result);" +
                //    "$('#tdMsg').html('" + returnVal + "');" +
                //"});</script>");
                return Content(returnVal);
            }
            // ViewBag.Chkmsg = returnVal;
            // return View("SchedulePrntScreening");
            //    }


            //catch (Exception e)
            //{
            //    string returnVal = clsGeneral.failedMsg(e.Message);
            //    ViewBag.Chkmsg = returnVal;
            //    return View("SchedulePrntScreening");
            //}  
        }


        //Schedule Appointment For Parents Tour
        //[ActiveSession]
        [HttpPost, ValidateInput(false)]
        [MultiButton(MatchFormKey = "Save", MatchFormValue = "Submit")]
        public ActionResult SavePrntScrmin(ScheduleParentScreeningViewModel model, string a)
        {
            string returnVal = "";
            bool success = false;
            ClsCommon updateCommon = new ClsCommon();
            sess = (clsSession)Session["UserSession"];
            try
            {
                if (sess != null)
                {
                    int qId = updateCommon.getQueueId("PT");
                    // returnVal = updateCommon.IsSubmit(qId);

                    //if (returnVal == "")
                    //{
                    MelmarkDBEntities objData = new MelmarkDBEntities();

                    foreach (var item in model.CallLog)
                    {
                        DateTime dtcalldate = new DateTime();
                        DateTime dtappntdate = new DateTime();
                        if (item.CallDateShow != null)
                            dtcalldate = DateTime.ParseExact(item.CallDateShow, "MM'/'dd'/'yyyy", System.Globalization.CultureInfo.CurrentCulture);
                        if (item.CallTimeShow == null && item.CallDateShow != null)
                            item.CallTimeShow = "00:00AM";
                        if (item.CallDateShow != null)
                        {
                            item.CallTime = dtcalldate.Add(TimeSpan.Parse(amPmTo24hourConverter(item.CallTimeShow)));
                        }

                        if (item.AppntDateShow != null)
                            dtappntdate = DateTime.ParseExact(item.AppntDateShow, "MM'/'dd'/'yyyy", System.Globalization.CultureInfo.CurrentCulture);
                        if (item.AppntTimeShow == null && item.AppntDateShow != null)
                            item.AppntTimeShow = "00:00AM";
                        if (item.AppntDateShow != null)
                        {
                            item.AppntTime = dtappntdate.Add(TimeSpan.Parse(amPmTo24hourConverter(item.AppntTimeShow)));
                        }
                    }

                    if (model.ChkAll != null && model.ChkAll.Count > 0)
                    {
                        success = updateCommon.getCheckedCheckListMul(model.ChkAll);

                        //if (success == true)
                        //{
                        model.Comment = updateCommon.UpdateOrInsertRevCmt("PT", model.Comment.academicReviewId, model.Comment.Comments, "N", model.Comment.IsPresent, model.Comment.AproveInt, "PS");
                        model.CallLog = updateCommon.UpdateOrInsertMultipleCallLog("PT", model.Comment.academicReviewId, "N", model.CallLog, model.Comment.QstatusId);
                        //model.CallLog = updateCommon.UpdateOrInsertCallLog("PT", model.Comment.academicReviewId, model.CallLog.AppntTime, "N", model.CallLog.CallTime, model.CallLog.CallLogId, model.CallLog.Conversation, model.CallLog.NameOfContact, model.CallLog.StaffName, model.CallLog.IsPresent, model.Comment.QstatusId);
                        //model.enginLetterList = updateCommon.UpdateOrInsertCheckList(model.enginLetterList, model.Comment.academicReviewId, "PT", model.Comment.QstatusId, "Check");
                        model.ChkAll = updateCommon.UpdateOrInsertMulCheckList(model.ChkAll, model.Comment.academicReviewId, "PT", model.Comment.QstatusId, "Check");

                        if (model.Comment.AproveInt == true)
                        {


                            int qIdNext = updateCommon.getQueueId("PS");
                            returnVal = "success*" + sess.ReferralId + "_" + qId;
                        }
                        else
                        {
                            //insert Letter
                            int QUeueid = updateCommon.getQueueId("PT");
                            var LetterName = objData.LetterEngines.Where(x => x.QueueId == QUeueid && x.ApproveStatus == false && x.SchoolId == sess.SchoolId && x.LetterType == "Letter").ToList();
                            if (LetterName.Count > 0)
                                getLetterTray.insertLetter("PT", sess.ReferralId, LetterName[0].LetterEngineId, "Parent");
                            LetterGeneration.LetterLists = getLetterTray.getLetterList("");

                            int qIdInActive = updateCommon.getQueueId("IL");
                            returnVal = "success*" + sess.ReferralId + "_" + qIdInActive;


                            var studentPer = objData.StudentPersonals.Where(x => x.StudentPersonalId == sess.ReferralId).SingleOrDefault();
                            studentPer.InactiveList = true;
                            studentPer.WaitingList = false;
                            objData.SaveChanges();
                        }
                        //}

                        //else
                        //{
                        //    model.Comment = updateCommon.UpdateOrInsertRevCmt("PT", model.Comment.academicReviewId, model.Comment.Comments, "Y", model.Comment.IsPresent, model.Comment.AproveInt, "PS");
                        //    model.CallLog = updateCommon.UpdateOrInsertMultipleCallLog("PT", model.Comment.academicReviewId, "Y", model.CallLog, model.Comment.QstatusId);
                        //    model.ChkAll = updateCommon.UpdateOrInsertMulCheckList(model.ChkAll, model.Comment.academicReviewId, "PT", model.Comment.QstatusId, "Check");
                        //    returnVal = clsGeneral.warningMsg("Checklist items are not completed so data submission is not possible.");
                        //}
                    }
                    else
                    {
                        model.Comment = updateCommon.UpdateOrInsertRevCmt("PT", model.Comment.academicReviewId, model.Comment.Comments, "N", model.Comment.IsPresent, model.Comment.AproveInt, "PS");
                        model.CallLog = updateCommon.UpdateOrInsertMultipleCallLog("PT", model.Comment.academicReviewId, "N", model.CallLog, model.Comment.QstatusId);
                        //model.CallLog = updateCommon.UpdateOrInsertCallLog("PT", model.Comment.academicReviewId, model.CallLog.AppntTime, "N", model.CallLog.CallTime, model.CallLog.CallLogId, model.CallLog.Conversation, model.CallLog.NameOfContact, model.CallLog.StaffName, model.CallLog.IsPresent, model.Comment.QstatusId);
                        //model.enginLetterList = updateCommon.UpdateOrInsertCheckList(model.enginLetterList, model.Comment.academicReviewId, "PT", model.Comment.QstatusId, "Check");
                        model.ChkAll = updateCommon.UpdateOrInsertMulCheckList(model.ChkAll, model.Comment.academicReviewId, "PT", model.Comment.QstatusId, "Check");

                        if (model.Comment.AproveInt == true)
                        {


                            int qIdNext = updateCommon.getQueueId("PS");
                            returnVal = "success*" + sess.ReferralId + "_" + qId;
                        }
                        else
                        {
                            //insert Letter
                            int QUeueid = updateCommon.getQueueId("PT");
                            var LetterName = objData.LetterEngines.Where(x => x.QueueId == QUeueid && x.ApproveStatus == false && x.SchoolId == sess.SchoolId && x.LetterType == "Letter").ToList();
                            if (LetterName.Count > 0)
                                getLetterTray.insertLetter("PT", sess.ReferralId, LetterName[0].LetterEngineId, "Parent");
                            LetterGeneration.LetterLists = getLetterTray.getLetterList("");

                            int qIdInActive = updateCommon.getQueueId("IL");
                            returnVal = "success*" + sess.ReferralId + "_" + qIdInActive;


                            var studentPer = objData.StudentPersonals.Where(x => x.StudentPersonalId == sess.ReferralId).SingleOrDefault();
                            studentPer.InactiveList = true;
                            objData.SaveChanges();
                        }
                    }

                }
                else
                {
                    returnVal = clsGeneral.failedMsg("Session not available.");

                }
                return Content(returnVal);
            }
            catch (Exception e)
            {
                returnVal = clsGeneral.failedMsg(e.Message);
                return Content(returnVal);
            }
        }

        //Tour Parent Screening
        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult TourParentScreening()
        {
            ClsCommon CommonCls = new ClsCommon();
            sess = (clsSession)Session["UserSession"];
            if (sess != null)
                ViewBag.permission = CommonCls.setPermission();
            TourParentScreeningViewModel shAssmtShe = new TourParentScreeningViewModel();

            bool InActivePresent = CommonCls.checkInactive();
            //shAssmtShe.enginLetterList = CommonCls.getCheckList("PS", "Check");
            shAssmtShe.ChkAll = CommonCls.getCheckListMultiple("PS", "Check");
            shAssmtShe.Comment = CommonCls.getRevCmt("PS");
            return View("TourParentScreening", shAssmtShe);
        }

        //[ActiveSession]
        [HttpPost, ValidateInput(false)]
        [MultiButton(MatchFormKey = "SaveTourParentScreening", MatchFormValue = "Save")]
        public ActionResult TourParentScreeningSubmit(ScheduleParentScreeningViewModel model)
        {
            try
            {
                //if (model.ChkAll != null) //Commented as part of List 3 - Task #10 by TL
                //{
                //    foreach (var val in model.ChkAll)
                //    {
                //        foreach (var v in val.chkList)
                //        {
                //            v.AssignMultiId = "NA";
                //        }
                //    } 
                //}  //because added assign in the menu so if it is not commented alwasy write "NA" to Database
                TourParentScreeningViewModel shAssmtShe = new TourParentScreeningViewModel();
                ClsCommon updateCommon = new ClsCommon();
                model.Comment = updateCommon.UpdateOrInsertRevCmt("PS", model.Comment.academicReviewId, model.Comment.Comments, "Y", model.Comment.IsPresent, model.Comment.AproveInt, "PS");
                model.ChkAll = updateCommon.UpdateOrInsertMulCheckList(model.ChkAll, model.Comment.academicReviewId, "PS", model.Comment.QstatusId, "Check");

                string returnVal = clsGeneral.sucessMsg("Data Successfully Saved");

                return Content(returnVal);

            }
            catch (Exception e)
            {
                string returnVal = clsGeneral.failedMsg(e.Message);
                return Content(returnVal);
            }
        }

        //[ActiveSession]
        [HttpPost, ValidateInput(false)]
        [MultiButton(MatchFormKey = "SaveTourParentScreening", MatchFormValue = "Submit")]
        public ActionResult TourParentScreeningSubmit(ScheduleParentScreeningViewModel model, string a)
        {
            MelmarkDBEntities objData = new MelmarkDBEntities();
            string returnVal = "";
            bool success = false;

            ClsCommon updateCommon = new ClsCommon();
            sess = (clsSession)Session["UserSession"];
            try
            {
                if (sess != null)
                {
                    int qId = updateCommon.getQueueId("PS");
                    // returnVal = updateCommon.IsSubmit(qId);

                    //if (returnVal == "")
                    //{
                    TourParentScreeningViewModel shAssmtShe = new TourParentScreeningViewModel();
                    if (model.ChkAll != null && model.ChkAll.Count > 0)
                    {
                        foreach (var val in model.ChkAll)
                        {
                            foreach (var v in val.chkList)
                            {
                                v.AssignMultiId = "NA";
                            }
                        }
                        success = updateCommon.getCheckedCheckListMul(model.ChkAll);


                        //if (success == true)
                        //{
                        model.Comment = updateCommon.UpdateOrInsertRevCmt("PS", model.Comment.academicReviewId, model.Comment.Comments, "N", model.Comment.IsPresent, model.Comment.AproveInt, "CA");
                        //  model.enginLetterList = updateCommon.UpdateOrInsertCheckList(model.enginLetterList, model.Comment.academicReviewId, "PS", model.Comment.QstatusId, "Check");
                        model.ChkAll = updateCommon.UpdateOrInsertMulCheckList(model.ChkAll, model.Comment.academicReviewId, "PS", model.Comment.QstatusId, "Check");
                        sess = (clsSession)Session["UserSession"];
                        if (model.Comment.AproveInt == true)
                        {
                            int qIdNext = updateCommon.getQueueId("CA");
                            returnVal = "success*" + sess.ReferralId + "_" + qId;


                        }
                        else
                        {
                            //insert Letter
                            int QUeueid = updateCommon.getQueueId("PS");
                            var LetterName = objData.LetterEngines.Where(x => x.QueueId == QUeueid && x.ApproveStatus == false && x.SchoolId == sess.SchoolId && x.LetterType == "Letter").ToList();
                            if (LetterName.Count > 0)
                                getLetterTray.insertLetter("PS", sess.ReferralId, LetterName[0].LetterEngineId, "Parent");
                            LetterGeneration.LetterLists = getLetterTray.getLetterList("");

                            int qIdInActive = updateCommon.getQueueId("IL");
                            returnVal = "success*" + sess.ReferralId + "_" + qIdInActive;


                            var studentPer = objData.StudentPersonals.Where(x => x.StudentPersonalId == sess.ReferralId).SingleOrDefault();
                            studentPer.InactiveList = true;
                            studentPer.WaitingList = false;
                            objData.SaveChanges();
                        }
                        //}
                        //else
                        //{
                        //    model.Comment = updateCommon.UpdateOrInsertRevCmt("PS", model.Comment.academicReviewId, model.Comment.Comments, "Y", model.Comment.IsPresent, model.Comment.AproveInt, "PS");
                        //    model.enginLetterList = updateCommon.UpdateOrInsertCheckList(model.enginLetterList, model.Comment.academicReviewId, "PS", model.Comment.QstatusId, "Check");
                        //    model.ChkAll = updateCommon.UpdateOrInsertMulCheckList(model.ChkAll, model.Comment.academicReviewId, "PS", model.Comment.QstatusId, "Check");
                        //    returnVal = clsGeneral.warningMsg("Checklist items are not completed so data submission is not possible.");
                        //}
                    }
                    else
                    {
                        model.Comment = updateCommon.UpdateOrInsertRevCmt("PS", model.Comment.academicReviewId, model.Comment.Comments, "N", model.Comment.IsPresent, model.Comment.AproveInt, "CA");
                        //  model.enginLetterList = updateCommon.UpdateOrInsertCheckList(model.enginLetterList, model.Comment.academicReviewId, "PS", model.Comment.QstatusId, "Check");
                        model.ChkAll = updateCommon.UpdateOrInsertMulCheckList(model.ChkAll, model.Comment.academicReviewId, "PS", model.Comment.QstatusId, "Check");
                        sess = (clsSession)Session["UserSession"];
                        if (model.Comment.AproveInt == true)
                        {
                            int qIdNext = updateCommon.getQueueId("CA");
                            returnVal = "success*" + sess.ReferralId + "_" + qId;


                        }
                        else
                        {
                            //insert Letter
                            int QUeueid = updateCommon.getQueueId("PS");
                            var LetterName = objData.LetterEngines.Where(x => x.QueueId == QUeueid && x.ApproveStatus == false && x.SchoolId == sess.SchoolId && x.LetterType == "Letter").ToList();
                            if (LetterName.Count > 0)
                                getLetterTray.insertLetter("PS", sess.ReferralId, LetterName[0].LetterEngineId, "Parent");
                            LetterGeneration.LetterLists = getLetterTray.getLetterList("");

                            int qIdInActive = updateCommon.getQueueId("IL");
                            returnVal = "success*" + sess.ReferralId + "_" + qIdInActive;


                            var studentPer = objData.StudentPersonals.Where(x => x.StudentPersonalId == sess.ReferralId).SingleOrDefault();
                            studentPer.InactiveList = true;
                            objData.SaveChanges();
                        }
                    }
                    //    else
                    //    {
                    //        model.Comment = updateCommon.UpdateOrInsertRevCmt("PS", model.Comment.academicReviewId, model.Comment.Comments, "Y", model.Comment.IsPresent, model.Comment.AproveInt, "PS");
                    //        //model.enginLetterList = updateCommon.UpdateOrInsertCheckList(model.enginLetterList, model.Comment.academicReviewId, "PS", model.Comment.QstatusId, "Check");
                    //        model.ChkAll = updateCommon.UpdateOrInsertMulCheckList(model.ChkAll, model.Comment.academicReviewId, "PS", model.Comment.QstatusId, "Check");
                    //        returnVal = clsGeneral.warningMsg("Checklist items are not assigned..");
                    //    }

                    //}


                }
                else
                {
                    returnVal = clsGeneral.failedMsg("Session not available.");
                }
                return Content(returnVal);
            }
            catch (Exception e)
            {
                returnVal = clsGeneral.failedMsg(e.Message);
                return Content(returnVal);
            }
        }





        //Clinical/Academic Review
        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult AcademicReviewIntakeAssessment()
        {
            ClsCommon CommonCls = new ClsCommon();
            sess = (clsSession)Session["UserSession"];
            if (sess != null)
                ViewBag.permission = CommonCls.setPermission();
            AcademicReviewIntakeAssessmentViewModel shAssmtShe = new AcademicReviewIntakeAssessmentViewModel();

            shAssmtShe.ChkAll = CommonCls.getCheckListMultiple("CA", "Check");
            shAssmtShe.Comment = CommonCls.getRevCmt("CA");
            return View("AcademicReviewIntakeAssessment", shAssmtShe);
        }

        //[ActiveSession]
        [HttpPost, ValidateInput(false)]
        [MultiButton(MatchFormKey = "SaveAcademicReviewIntake", MatchFormValue = "Save")]
        public ActionResult AcademicReviewIntakeAssessmentSubmit(AcademicReviewIntakeAssessmentViewModel model)
        {
            try
            {
                AcademicReviewIntakeAssessmentViewModel shAssmtShe = new AcademicReviewIntakeAssessmentViewModel();
                ClsCommon updateCommon = new ClsCommon();

                if (model.ChkAll != null)
                {
                    foreach (var val in model.ChkAll)
                    {
                        foreach (var v in val.chkList)
                        {
                            v.AssignMultiId = "NA";
                        }
                    }
                }

                model.Comment = updateCommon.UpdateOrInsertRevCmt("CA", model.Comment.academicReviewId, model.Comment.Comments, "Y", model.Comment.IsPresent, model.Comment.AproveInt, "IA");
                model.ChkAll = updateCommon.UpdateOrInsertMulCheckList(model.ChkAll, model.Comment.academicReviewId, "CA", model.Comment.QstatusId, "Check");
                string returnVal = clsGeneral.sucessMsg("Data Successfully Saved");

                return Content(returnVal);
            }
            catch (Exception e)
            {
                string returnVal = clsGeneral.failedMsg(e.Message);
                return Content(returnVal);
            }
        }

        //[ActiveSession]
        [HttpPost, ValidateInput(false)]
        [MultiButton(MatchFormKey = "SaveAcademicReviewIntake", MatchFormValue = "Submit")]
        public ActionResult AcademicReviewIntakeAssessmentSubmit(AcademicReviewIntakeAssessmentViewModel model, string a)
        {
            MelmarkDBEntities objData = new MelmarkDBEntities();
            string returnVal = "";
            ClsCommon updateCommon = new ClsCommon();
            bool success = false;
            sess = (clsSession)Session["UserSession"];
            try
            {
                if (sess != null)
                {
                    int qId = updateCommon.getQueueId("CA");
                    // returnVal = updateCommon.IsSubmit(qId);
                    //if (returnVal == "")
                    //{
                    AcademicReviewIntakeAssessmentViewModel shAssmtShe = new AcademicReviewIntakeAssessmentViewModel();

                    if (model.ChkAll.Count > 0)
                    {
                        foreach (var val in model.ChkAll)
                        {
                            foreach (var v in val.chkList)
                            {
                                v.AssignMultiId = "NA";
                            }
                        }
                        success = updateCommon.getCheckedCheckListMul(model.ChkAll);
                        //if (success == true)
                        //{
                        model.Comment = updateCommon.UpdateOrInsertRevCmt("CA", model.Comment.academicReviewId, model.Comment.Comments, "N", model.Comment.IsPresent, model.Comment.AproveInt, "SA");
                        model.ChkAll = updateCommon.UpdateOrInsertMulCheckList(model.ChkAll, model.Comment.academicReviewId, "CA", model.Comment.QstatusId, "Check");
                        sess = (clsSession)Session["UserSession"];
                        if (model.Comment.AproveInt == true)
                        {
                            int qIdNext = updateCommon.getQueueId("SA");
                            returnVal = "success*" + sess.ReferralId + "_" + qId;
                        }
                        else
                        {
                            //insert Letter
                            int QUeueid = updateCommon.getQueueId("CA");
                            var LetterName = objData.LetterEngines.Where(x => x.QueueId == QUeueid && x.ApproveStatus == false && x.SchoolId == sess.SchoolId && x.LetterType == "Letter").ToList();
                            if (LetterName.Count > 0)
                                getLetterTray.insertLetter("CA", sess.ReferralId, LetterName[0].LetterEngineId, "Parent");
                            LetterGeneration.LetterLists = getLetterTray.getLetterList("");

                            int qIdInActive = updateCommon.getQueueId("IL");
                            returnVal = "success*" + sess.ReferralId + "_" + qIdInActive;

                            var studentPer = objData.StudentPersonals.Where(x => x.StudentPersonalId == sess.ReferralId).SingleOrDefault();
                            studentPer.InactiveList = true;
                            studentPer.WaitingList = false;
                            objData.SaveChanges();
                        }
                        //}
                        //else
                        //{
                        //    model.Comment = updateCommon.UpdateOrInsertRevCmt("CA", model.Comment.academicReviewId, model.Comment.Comments, "Y", model.Comment.IsPresent, model.Comment.AproveInt, "IA");
                        //    model.ChkAll = updateCommon.UpdateOrInsertMulCheckList(model.ChkAll, model.Comment.academicReviewId, "CA", model.Comment.QstatusId, "Check");
                        //    returnVal = clsGeneral.warningMsg("Checklist items are not completed so data submission is not possible.");
                        //}
                    }
                    else
                    {
                        model.Comment = updateCommon.UpdateOrInsertRevCmt("CA", model.Comment.academicReviewId, model.Comment.Comments, "N", model.Comment.IsPresent, model.Comment.AproveInt, "SA");
                        model.ChkAll = updateCommon.UpdateOrInsertMulCheckList(model.ChkAll, model.Comment.academicReviewId, "CA", model.Comment.QstatusId, "Check");
                        sess = (clsSession)Session["UserSession"];
                        if (model.Comment.AproveInt == true)
                        {
                            int qIdNext = updateCommon.getQueueId("SA");
                            returnVal = "success*" + sess.ReferralId + "_" + qId;
                        }
                        else
                        {
                            //insert Letter
                            int QUeueid = updateCommon.getQueueId("CA");
                            var LetterName = objData.LetterEngines.Where(x => x.QueueId == QUeueid && x.ApproveStatus == false && x.SchoolId == sess.SchoolId && x.LetterType == "Letter").ToList();
                            if (LetterName.Count > 0)
                                getLetterTray.insertLetter("CA", sess.ReferralId, LetterName[0].LetterEngineId, "Parent");
                            LetterGeneration.LetterLists = getLetterTray.getLetterList("");

                            int qIdInActive = updateCommon.getQueueId("IL");
                            returnVal = "success*" + sess.ReferralId + "_" + qIdInActive;

                            var studentPer = objData.StudentPersonals.Where(x => x.StudentPersonalId == sess.ReferralId).SingleOrDefault();
                            studentPer.InactiveList = true;
                            objData.SaveChanges();
                        }
                        // returnVal = clsGeneral.warningMsg("Checklist items are not assigned..");
                    }

                    //}


                }
                else
                {
                    returnVal = clsGeneral.failedMsg("Session not available");
                }
                return Content(returnVal);
            }
            catch (Exception e)
            {
                returnVal = clsGeneral.failedMsg(e.Message);
                return Content(returnVal);
            }
        }


        //Shedule Appoinment for Intake Assessment
        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult ScheduleAppointment()
        {
            ClsCommon getCommon = new ClsCommon();
            sess = (clsSession)Session["UserSession"];
            if (sess != null)
                ViewBag.permission = getCommon.setPermission();
            scheduleAppointmentIntakeAssessmentViewModel shAssmtShe = new scheduleAppointmentIntakeAssessmentViewModel();

            shAssmtShe.Comment = getCommon.getRevCmt("SA");
            shAssmtShe.ChkAll = getCommon.getCheckListMultiple("SA", "Check");
            shAssmtShe.CallLog = getCommon.getRevMultipleCallLog("SA", shAssmtShe.Comment.academicReviewId);
            if (shAssmtShe.CallLog.Count == 0)
            {
                CommonCallLogViewModel defaultmodel = new CommonCallLogViewModel();
                defaultmodel.StaffName = sess.UserName;
                shAssmtShe.CallLog.Add(defaultmodel);
            }
            if (shAssmtShe.Comment.AproveInt == true)
            {
                shAssmtShe.approvedStatus = 1;
            }
            return View("ScheduleAppointment", shAssmtShe);
        }

        //[ActiveSession]
        [HttpPost, ValidateInput(false)]
        [MultiButton(MatchFormKey = "SaveScheduleAppointment", MatchFormValue = "Save")]
        public ActionResult ScheduleAppointmentSubmit(scheduleAppointmentIntakeAssessmentViewModel model)
        {


            try
            {
                MelmarkDBEntities objData = new MelmarkDBEntities();
                ClsCommon updateCommon = new ClsCommon();
                //if (model.ChkAll != null) // Commented pasrt of List 3 - Task #25
                //{
                //    foreach (var val in model.ChkAll)
                //    {
                //        foreach (var v in val.chkList)
                //        {
                //            v.AssignMultiId = "NA";
                //        }
                //    }
                //}  //because added assign in the menu so if it is not commented alwasy write "NA" to Database
                foreach (var item in model.CallLog)
                {
                    DateTime dtcalldate = new DateTime();
                    DateTime dtappntdate = new DateTime();
                    if (item.CallDateShow != null)
                        dtcalldate = DateTime.ParseExact(item.CallDateShow, "MM'/'dd'/'yyyy", System.Globalization.CultureInfo.CurrentCulture);
                    if (item.CallTimeShow == null && item.CallDateShow != null)
                        item.CallTimeShow = "00:00AM";
                    if (item.CallDateShow != null)
                    {
                        item.CallTime = dtcalldate.Add(TimeSpan.Parse(amPmTo24hourConverter(item.CallTimeShow)));
                    }

                    if (item.AppntDateShow != null)
                        dtappntdate = DateTime.ParseExact(item.AppntDateShow, "MM'/'dd'/'yyyy", System.Globalization.CultureInfo.CurrentCulture);
                    if (item.AppntTimeShow == null && item.AppntDateShow != null)
                        item.AppntTimeShow = "00:00AM";
                    if (item.AppntDateShow != null)
                    {
                        item.AppntTime = dtappntdate.Add(TimeSpan.Parse(amPmTo24hourConverter(item.AppntTimeShow)));
                    }
                }
                model.Comment = updateCommon.UpdateOrInsertRevCmt("SA", model.Comment.academicReviewId, model.Comment.Comments, "Y", model.Comment.IsPresent, model.Comment.AproveInt, "SA");
                model.ChkAll = updateCommon.UpdateOrInsertMulCheckList(model.ChkAll, model.Comment.academicReviewId, "SA", model.Comment.QstatusId, "Check");
                model.CallLog = updateCommon.UpdateOrInsertMultipleCallLog("SA", model.Comment.academicReviewId, "Y", model.CallLog, model.Comment.QstatusId);
                string returnVal = clsGeneral.sucessMsgWithoutQuote("Data Successfully Saved");

                //return Content("<script type='text/javascript'>$.get('/IntakeAssessment/ScheduleAppointment', function (result) {" +
                //    "$('#LoadQueue').html(result);" +
                //    "$('#tdMsg').html('" + returnVal + "');" +
                //"});</script>");

                return Content(returnVal);
            }
            catch (Exception e)
            {
                string returnVal = clsGeneral.failedMsgWithoutQuote(e.Message);
                //return Content("<script type='text/javascript'>$.get('/IntakeAssessment/ScheduleAppointment', function (result) {" +
                //    "$('#LoadQueue').html(result);" +
                //    "$('#tdMsg').html('" + returnVal + "');" +
                //"});</script>");
                return Content(returnVal);
            }

            //     ViewBag.Chkmsg = returnVal;
            //        return View("ScheduleAppointment");


            //}
            //catch (Exception e)
            //{
            //    string returnVal = clsGeneral.failedMsg(e.Message);
            //    ViewBag.Chkmsg = returnVal;
            //    return View("ScheduleAppointment");
            //}

        }

        //[ActiveSession]
        [HttpPost, ValidateInput(false)]
        [MultiButton(MatchFormKey = "SaveScheduleAppointment", MatchFormValue = "Submit")]
        public ActionResult ScheduleAppointmentSubmit(scheduleAppointmentIntakeAssessmentViewModel model, string a)
        {
            string returnVal = "";
            bool success = false;
            ClsCommon updateCommon = new ClsCommon();



            sess = (clsSession)Session["UserSession"];
            try
            {
                if (sess != null)
                {
                    int qId = updateCommon.getQueueId("SA");
                    // returnVal = updateCommon.IsSubmit(qId);
                    //if (returnVal == "")
                    //{
                    MelmarkDBEntities objData = new MelmarkDBEntities();
                    foreach (var item in model.CallLog)
                    {
                        DateTime dtcalldate = new DateTime();
                        DateTime dtappntdate = new DateTime();
                        if (item.CallDateShow != null)
                            dtcalldate = DateTime.ParseExact(item.CallDateShow, "MM'/'dd'/'yyyy", System.Globalization.CultureInfo.CurrentCulture);
                        if (item.CallTimeShow == null && item.CallDateShow != null)
                            item.CallTimeShow = "00:00AM";
                        if (item.CallDateShow != null)
                        {
                            item.CallTime = dtcalldate.Add(TimeSpan.Parse(amPmTo24hourConverter(item.CallTimeShow)));
                        }

                        if (item.AppntDateShow != null)
                            dtappntdate = DateTime.ParseExact(item.AppntDateShow, "MM'/'dd'/'yyyy", System.Globalization.CultureInfo.CurrentCulture);
                        if (item.AppntTimeShow == null && item.AppntDateShow != null)
                            item.AppntTimeShow = "00:00AM";
                        if (item.AppntDateShow != null)
                        {
                            item.AppntTime = dtappntdate.Add(TimeSpan.Parse(amPmTo24hourConverter(item.AppntTimeShow)));
                        }
                    }
                    if (model.ChkAll.Count > 0)
                    {
                        // foreach (var val in model.ChkAll) // Commented pasrt of List 3 - Task #25
                        // {
                            // foreach (var v in val.chkList)
                            // {
                                // v.AssignMultiId = "NA";
                            // }
                        // } //because added assign in the menu so if it is not commented alwasy write "NA" to Database

                        success = updateCommon.getCheckedCheckListMul(model.ChkAll);
                        //if (success == true)
                        //{

                        model.Comment = updateCommon.UpdateOrInsertRevCmt("SA", model.Comment.academicReviewId, model.Comment.Comments, "N", model.Comment.IsPresent, model.Comment.AproveInt, "IE");
                        model.CallLog = updateCommon.UpdateOrInsertMultipleCallLog("SA", model.Comment.academicReviewId, "N", model.CallLog, model.Comment.QstatusId);
                        model.ChkAll = updateCommon.UpdateOrInsertMulCheckList(model.ChkAll, model.Comment.academicReviewId, "SA", model.Comment.QstatusId, "Check");
                        sess = (clsSession)Session["UserSession"];
                        if (model.Comment.AproveInt == true)
                        {


                            int qIdNext = updateCommon.getQueueId("IE");
                            returnVal = "success*" + sess.ReferralId + "_" + qId;
                        }
                        else
                        {
                            ////insert Letter
                            //int QUeueid = updateCommon.getQueueId("SA");
                            //var LetterName = objData.LetterEngines.Where(x => x.QueueId == QUeueid && x.ApproveStatus == false && x.SchoolId == sess.SchoolId && x.LetterType == "Letter").ToList();
                            //if (LetterName.Count > 0)
                            //    getLetterTray.insertLetter("SA", sess.ReferralId, LetterName[0].LetterEngineId, "Parent");
                            //LetterGeneration.LetterLists = getLetterTray.getLetterList("");

                            int qIdInActive = updateCommon.getQueueId("IL");
                            returnVal = "success*" + sess.ReferralId + "_" + qIdInActive;


                            var studentPer = objData.StudentPersonals.Where(x => x.StudentPersonalId == sess.ReferralId).SingleOrDefault();
                            studentPer.InactiveList = true;
                            studentPer.WaitingList = false;
                            objData.SaveChanges();
                        }
                        //}
                        //else
                        //{
                        //    model.Comment = updateCommon.UpdateOrInsertRevCmt("SA", model.Comment.academicReviewId, model.Comment.Comments, "Y", model.Comment.IsPresent, model.Comment.AproveInt, "SA");
                        //    model.CallLog = updateCommon.UpdateOrInsertMultipleCallLog("SA", model.Comment.academicReviewId, "Y", model.CallLog, model.Comment.QstatusId);
                        //    model.ChkAll = updateCommon.UpdateOrInsertMulCheckList(model.ChkAll, model.Comment.academicReviewId, "SA", model.Comment.QstatusId, "Check");
                        //    returnVal = clsGeneral.warningMsg("Checklist items are not completed so data submission is not possible.");
                        //}
                    }
                    else
                    {
                        model.Comment = updateCommon.UpdateOrInsertRevCmt("SA", model.Comment.academicReviewId, model.Comment.Comments, "N", model.Comment.IsPresent, model.Comment.AproveInt, "IE");
                        model.CallLog = updateCommon.UpdateOrInsertMultipleCallLog("SA", model.Comment.academicReviewId, "N", model.CallLog, model.Comment.QstatusId);
                        model.ChkAll = updateCommon.UpdateOrInsertMulCheckList(model.ChkAll, model.Comment.academicReviewId, "SA", model.Comment.QstatusId, "Check");
                        sess = (clsSession)Session["UserSession"];
                        if (model.Comment.AproveInt == true)
                        {


                            int qIdNext = updateCommon.getQueueId("IE");
                            returnVal = "success*" + sess.ReferralId + "_" + qId;
                        }
                        else
                        {
                            ////insert Letter
                            //int QUeueid = updateCommon.getQueueId("SA");
                            //var LetterName = objData.LetterEngines.Where(x => x.QueueId == QUeueid && x.ApproveStatus == false && x.SchoolId == sess.SchoolId && x.LetterType == "Letter").ToList();
                            //if (LetterName.Count > 0)
                            //    getLetterTray.insertLetter("SA", sess.ReferralId, LetterName[0].LetterEngineId, "Parent");
                            //LetterGeneration.LetterLists = getLetterTray.getLetterList("");

                            int qIdInActive = updateCommon.getQueueId("IL");
                            returnVal = "success*" + sess.ReferralId + "_" + qIdInActive;


                            var studentPer = objData.StudentPersonals.Where(x => x.StudentPersonalId == sess.ReferralId).SingleOrDefault();
                            studentPer.InactiveList = true;
                            objData.SaveChanges();
                        }
                    }
                    //else
                    //{
                    //    foreach (var item in model.CallLog)
                    //    {
                    //        DateTime dtcalldate = new DateTime();
                    //        DateTime dtappntdate = new DateTime();
                    //        if (item.CallDateShow != null)
                    //            dtcalldate = DateTime.ParseExact(item.CallDateShow, "MM'/'dd'/'yyyy", System.Globalization.CultureInfo.CurrentCulture);
                    //        if (item.CallTimeShow == null && item.CallDateShow != null)
                    //            item.CallTimeShow = "00:00AM";
                    //        if (item.CallDateShow != null)
                    //        {
                    //            item.CallTime = dtcalldate.Add(TimeSpan.Parse(amPmTo24hourConverter(item.CallTimeShow)));
                    //        }

                    //        if (item.AppntDateShow != null)
                    //            dtappntdate = DateTime.ParseExact(item.AppntDateShow, "MM'/'dd'/'yyyy", System.Globalization.CultureInfo.CurrentCulture);
                    //        if (item.AppntTimeShow == null && item.AppntDateShow != null)
                    //            item.AppntTimeShow = "00:00AM";
                    //        if (item.AppntDateShow != null)
                    //        {
                    //            item.AppntTime = dtappntdate.Add(TimeSpan.Parse(amPmTo24hourConverter(item.AppntTimeShow)));
                    //        }

                    //    }
                    //    model.Comment = updateCommon.UpdateOrInsertRevCmt("SA", model.Comment.academicReviewId, model.Comment.Comments, "Y", model.Comment.IsPresent, model.Comment.AproveInt, "SA");
                    //    model.CallLog = updateCommon.UpdateOrInsertMultipleCallLog("SA", model.Comment.academicReviewId, "Y", model.CallLog, model.Comment.QstatusId);
                    //    model.ChkAll = updateCommon.UpdateOrInsertMulCheckList(model.ChkAll, model.Comment.academicReviewId, "SA", model.Comment.QstatusId, "Check");
                    //    returnVal = clsGeneral.warningMsg("Checklist items are not assigned..");
                    //}

                    //}
                }
                else
                {
                    returnVal = clsGeneral.failedMsg("Session not available");

                }
            }
            catch (Exception e)
            {
                returnVal = clsGeneral.failedMsg(e.Message);
                return Content(returnVal);
            }
            return Content(returnVal);
        }


        private string amPmTo24hourConverter(string time)
        {
            string[] Alltime = time.Split(':');
            int h = Convert.ToInt16(Alltime[0]);
            string startAMPM = Alltime[1].Substring(2, 2);
            string m = Alltime[1].Substring(0, 2);
            if (startAMPM == "PM")
            {
                if (h != 12)
                {
                    h += 12;
                }

            }
            else if (startAMPM == "AM")
            {
                if (h == 12)
                {
                    h = 00;
                }
            }
            return (h.ToString() + ":" + m.ToString());
        }

        //Shedule Parent Interview
        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult ParantSheduleInteview()
        {
            ClsCommon getCommon = new ClsCommon();
            sess = (clsSession)Session["UserSession"];
            if (sess != null)
                ViewBag.permission = getCommon.setPermission();
            SchedulePrntInterviewViewModel shAssmtShe = new SchedulePrntInterviewViewModel();


            shAssmtShe.Comment = getCommon.getRevCmt("SI");
            //shAssmtShe.CallLog = getCommon.getRevCallLog("SI", shAssmtShe.Comment.academicReviewId);
            shAssmtShe.CallLog = getCommon.getRevMultipleCallLog("SI", shAssmtShe.Comment.academicReviewId);
            if (shAssmtShe.CallLog.Count == 0)
            {
                CommonCallLogViewModel defaultmodel = new CommonCallLogViewModel();
                defaultmodel.StaffName = sess.UserName;
                shAssmtShe.CallLog.Add(defaultmodel);
            }
            shAssmtShe.ChkAll = getCommon.getCheckListMultiple("SI", "Check");
            return View("SchedulePrntInterviewv", shAssmtShe);
        }

        //[ActiveSession]
        [HttpPost, ValidateInput(false)]
        [MultiButton(MatchFormKey = "SaveScheduleParntInterview", MatchFormValue = "Save")]
        public ActionResult ParantSheduleInteviewSave(SchedulePrntInterviewViewModel model)
        {
            try
            {
                MelmarkDBEntities objData = new MelmarkDBEntities();
                ClsCommon updateCommon = new ClsCommon();
                if (model.ChkAll != null)
                {
                    foreach (var val in model.ChkAll)
                    {
                        foreach (var v in val.chkList)
                        {
                            v.AssignMultiId = "NA";
                        }
                    }
                }
                foreach (var item in model.CallLog)
                {
                    DateTime dtcalldate = new DateTime();
                    DateTime dtappntdate = new DateTime();
                    if (item.CallDateShow != null)
                        dtcalldate = DateTime.ParseExact(item.CallDateShow, "MM'/'dd'/'yyyy", System.Globalization.CultureInfo.CurrentCulture);
                    if (item.CallTimeShow == null && item.CallDateShow != null)
                        item.CallTimeShow = "00:00AM";
                    if (item.CallDateShow != null)
                    {
                        item.CallTime = dtcalldate.Add(TimeSpan.Parse(amPmTo24hourConverter(item.CallTimeShow)));
                    }

                    if (item.AppntDateShow != null)
                        dtappntdate = DateTime.ParseExact(item.AppntDateShow, "MM'/'dd'/'yyyy", System.Globalization.CultureInfo.CurrentCulture);
                    if (item.AppntTimeShow == null && item.AppntDateShow != null)
                        item.AppntTimeShow = "00:00AM";
                    if (item.AppntDateShow != null)
                    {
                        item.AppntTime = dtappntdate.Add(TimeSpan.Parse(amPmTo24hourConverter(item.AppntTimeShow)));
                    }
                }
                model.Comment = updateCommon.UpdateOrInsertRevCmt("SI", model.Comment.academicReviewId, model.Comment.Comments, "Y", model.Comment.IsPresent, model.Comment.AproveInt, "PT");
                //model.CallLog = updateCommon.UpdateOrInsertCallLog("SI", model.Comment.academicReviewId, model.CallLog.AppntTime, "Y", model.CallLog.CallTime, model.CallLog.CallLogId, model.CallLog.Conversation, model.CallLog.NameOfContact, model.CallLog.StaffName, model.CallLog.IsPresent, model.Comment.QstatusId);
                model.CallLog = updateCommon.UpdateOrInsertMultipleCallLog("SI", model.Comment.academicReviewId, "Y", model.CallLog, model.Comment.QstatusId);
                model.ChkAll = updateCommon.UpdateOrInsertMulCheckList(model.ChkAll, model.Comment.academicReviewId, "SI", model.Comment.QstatusId, "Check");
                string returnVal = clsGeneral.sucessMsgWithoutQuote("Data Successfully Saved");

                //return Content("<script type='text/javascript'>$.get('/IntakeAssessment/ParantSheduleInteview', function (result) {" +
                //    "$('#LoadQueue').html(result);" +
                //    "$('#tdMsg').html('" + returnVal + "');" +
                //"});</script>");


                return Content(returnVal);
            }
            catch (Exception e)
            {
                string returnVal = clsGeneral.failedMsgWithoutQuote(e.Message);
                //return Content("<script type='text/javascript'>$.get('/IntakeAssessment/ParantSheduleInteview', function (result) {" +
                //    "$('#LoadQueue').html(result);" +
                //    "$('#tdMsg').html('" + returnVal + "');" +
                //"});</script>");
                return Content(returnVal);
            }
        }

        //[ActiveSession]
        [HttpPost, ValidateInput(false)]
        [MultiButton(MatchFormKey = "SaveScheduleParntInterview", MatchFormValue = "Submit")]
        public ActionResult ParantSheduleInteviewSubmit(SchedulePrntInterviewViewModel model)
        {
            string returnVal = "";
            bool success = false;
            sess = (clsSession)Session["UserSession"];

            ClsCommon updateCommon = new ClsCommon();
            try
            {
                if (sess != null)
                {
                    int qId = updateCommon.getQueueId("SI");
                    //returnVal = updateCommon.IsSubmit(qId);

                    //if (returnVal == "")
                    //{
                    MelmarkDBEntities objData = new MelmarkDBEntities();
                    foreach (var item in model.CallLog)
                    {
                        DateTime dtcalldate = new DateTime();
                        DateTime dtappntdate = new DateTime();
                        if (item.CallDateShow != null)
                            dtcalldate = DateTime.ParseExact(item.CallDateShow, "MM'/'dd'/'yyyy", System.Globalization.CultureInfo.CurrentCulture);
                        if (item.CallTimeShow == null && item.CallDateShow != null)
                            item.CallTimeShow = "00:00AM";
                        if (item.CallDateShow != null)
                        {
                            item.CallTime = dtcalldate.Add(TimeSpan.Parse(amPmTo24hourConverter(item.CallTimeShow)));
                        }

                        if (item.AppntDateShow != null)
                            dtappntdate = DateTime.ParseExact(item.AppntDateShow, "MM'/'dd'/'yyyy", System.Globalization.CultureInfo.CurrentCulture);
                        if (item.AppntTimeShow == null && item.AppntDateShow != null)
                            item.AppntTimeShow = "00:00AM";
                        if (item.AppntDateShow != null)
                        {
                            item.AppntTime = dtappntdate.Add(TimeSpan.Parse(amPmTo24hourConverter(item.AppntTimeShow)));
                        }
                    }
                    if (model.ChkAll.Count > 0)
                    {
                        foreach (var val in model.ChkAll)
                        {
                            foreach (var v in val.chkList)
                            {
                                v.AssignMultiId = "NA";
                            }
                        }

                        success = updateCommon.getCheckedCheckListMul(model.ChkAll);
                        //if (success == true)
                        //{
                        model.Comment = updateCommon.UpdateOrInsertRevCmt("SI", model.Comment.academicReviewId, model.Comment.Comments, "N", model.Comment.IsPresent, model.Comment.AproveInt, "PI");
                        //model.CallLog = updateCommon.UpdateOrInsertCallLog("SI", model.Comment.academicReviewId, model.CallLog.AppntTime, "N", model.CallLog.CallTime, model.CallLog.CallLogId, model.CallLog.Conversation, model.CallLog.NameOfContact, model.CallLog.StaffName, model.CallLog.IsPresent, model.Comment.QstatusId);
                        model.CallLog = updateCommon.UpdateOrInsertMultipleCallLog("SI", model.Comment.academicReviewId, "N", model.CallLog, model.Comment.QstatusId);
                        model.ChkAll = updateCommon.UpdateOrInsertMulCheckList(model.ChkAll, model.Comment.academicReviewId, "SI", model.Comment.QstatusId, "Check");
                        sess = (clsSession)Session["UserSession"];
                        if (model.Comment.AproveInt == true)
                        {


                            int qIdNext = updateCommon.getQueueId("PI");
                            returnVal = "success*" + sess.ReferralId + "_" + qId;
                        }
                        else
                        {
                            //insert Letter
                            int QUeueid = updateCommon.getQueueId("SI");
                            var LetterName = objData.LetterEngines.Where(x => x.QueueId == QUeueid && x.ApproveStatus == false && x.SchoolId == sess.SchoolId && x.LetterType == "Letter").ToList();
                            if (LetterName.Count > 0)
                                getLetterTray.insertLetter("SI", sess.ReferralId, LetterName[0].LetterEngineId, "Parent");
                            LetterGeneration.LetterLists = getLetterTray.getLetterList("");

                            int qIdInActive = updateCommon.getQueueId("IL");
                            returnVal = "success*" + sess.ReferralId + "_" + qIdInActive;

                            var studentPer = objData.StudentPersonals.Where(x => x.StudentPersonalId == sess.ReferralId).SingleOrDefault();
                            studentPer.InactiveList = true;
                            studentPer.WaitingList = false;
                            objData.SaveChanges();
                        }
                        //}
                        //else
                        //{

                        //    model.Comment = updateCommon.UpdateOrInsertRevCmt("SI", model.Comment.academicReviewId, model.Comment.Comments, "Y", model.Comment.IsPresent, model.Comment.AproveInt, "SI");
                        //    //model.CallLog = updateCommon.UpdateOrInsertCallLog("SI", model.Comment.academicReviewId, model.CallLog.AppntTime, "N", model.CallLog.CallTime, model.CallLog.CallLogId, model.CallLog.Conversation, model.CallLog.NameOfContact, model.CallLog.StaffName, model.CallLog.IsPresent, model.Comment.QstatusId);
                        //    model.CallLog = updateCommon.UpdateOrInsertMultipleCallLog("SI", model.Comment.academicReviewId, "Y", model.CallLog, model.Comment.QstatusId);
                        //    model.ChkAll = updateCommon.UpdateOrInsertMulCheckList(model.ChkAll, model.Comment.academicReviewId, "SI", model.Comment.QstatusId, "Check");
                        //    returnVal = clsGeneral.warningMsg("Checklist items are not completed so data submission is not possible.");
                        //}
                    }
                    else
                    {
                        model.Comment = updateCommon.UpdateOrInsertRevCmt("SI", model.Comment.academicReviewId, model.Comment.Comments, "N", model.Comment.IsPresent, model.Comment.AproveInt, "PI");
                        //model.CallLog = updateCommon.UpdateOrInsertCallLog("SI", model.Comment.academicReviewId, model.CallLog.AppntTime, "N", model.CallLog.CallTime, model.CallLog.CallLogId, model.CallLog.Conversation, model.CallLog.NameOfContact, model.CallLog.StaffName, model.CallLog.IsPresent, model.Comment.QstatusId);
                        model.CallLog = updateCommon.UpdateOrInsertMultipleCallLog("SI", model.Comment.academicReviewId, "N", model.CallLog, model.Comment.QstatusId);
                        model.ChkAll = updateCommon.UpdateOrInsertMulCheckList(model.ChkAll, model.Comment.academicReviewId, "SI", model.Comment.QstatusId, "Check");
                        sess = (clsSession)Session["UserSession"];
                        if (model.Comment.AproveInt == true)
                        {


                            int qIdNext = updateCommon.getQueueId("PI");
                            returnVal = "success*" + sess.ReferralId + "_" + qId;
                        }
                        else
                        {
                            //insert Letter
                            int QUeueid = updateCommon.getQueueId("SI");
                            var LetterName = objData.LetterEngines.Where(x => x.QueueId == QUeueid && x.ApproveStatus == false && x.SchoolId == sess.SchoolId && x.LetterType == "Letter").ToList();
                            if (LetterName.Count > 0)
                                getLetterTray.insertLetter("SI", sess.ReferralId, LetterName[0].LetterEngineId, "Parent");
                            LetterGeneration.LetterLists = getLetterTray.getLetterList("");

                            int qIdInActive = updateCommon.getQueueId("IL");
                            returnVal = "success*" + sess.ReferralId + "_" + qIdInActive;

                            var studentPer = objData.StudentPersonals.Where(x => x.StudentPersonalId == sess.ReferralId).SingleOrDefault();
                            studentPer.InactiveList = true;
                            objData.SaveChanges();
                        }
                    }
                }
                else
                {
                    returnVal = clsGeneral.failedMsg("Session not available");

                }

                return Content(returnVal);
            }
            catch (Exception e)
            {
                returnVal = clsGeneral.failedMsg(e.Message);
                return Content(returnVal);
            }
        }



        //Accadamic Parent Interview
        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        [clsSessionActive(Result = true)]
        public ActionResult ParentInterView()
        {
            ClsCommon CommonCls = new ClsCommon();
            sess = (clsSession)Session["UserSession"];
            if (sess != null)
                ViewBag.permission = CommonCls.setPermission();
            ParentInterviewViewModel shAssmtShe = new ParentInterviewViewModel();

            try
            {
                shAssmtShe.ChkAll = CommonCls.getCheckListMultiple("PI", "Check");
                shAssmtShe.Comment = CommonCls.getRevCmt("PI");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View("ParentInterView", shAssmtShe);
        }

        //[ActiveSession]
        [HttpPost, ValidateInput(false)]
        [MultiButton(MatchFormKey = "SaveParentInterview", MatchFormValue = "Save")]
        public ActionResult ParentInterviewSubmit(ParentInterviewViewModel model)
        {

            try
            {
                ParentInterviewViewModel shAssmtShe = new ParentInterviewViewModel();
                ClsCommon updateCommon = new ClsCommon();
                model.Comment = updateCommon.UpdateOrInsertRevCmt("PI", model.Comment.academicReviewId, model.Comment.Comments, "Y", model.Comment.IsPresent, model.Comment.AproveInt, "IA");
                model.ChkAll = updateCommon.UpdateOrInsertMulCheckList(model.ChkAll, model.Comment.academicReviewId, "PI", model.Comment.QstatusId, "Check");
                //   model.enginLetterList0 = updateCommon.UpdateOrInsertCheckList(model.enginLetterList0, model.Comment.academicReviewId, "PI", model.Comment.QstatusId, "Check");
                //   model.enginLetterList1 = updateCommon.UpdateOrInsertCheckList(model.enginLetterList1, model.Comment.academicReviewId, "PI", model.Comment.QstatusId, "Check1");
                //   model.enginLetterList2 = updateCommon.UpdateOrInsertCheckList(model.enginLetterList2, model.Comment.academicReviewId, "PI", model.Comment.QstatusId, "Check2");
                string returnVal = clsGeneral.sucessMsg("Data Successfully Saved");
                return Content(returnVal);
            }
            catch (Exception e)
            {
                string returnVal = clsGeneral.failedMsg(e.Message);
                return Content(returnVal);
            }
        }

        //[ActiveSession]
        [HttpPost, ValidateInput(false)]
        [MultiButton(MatchFormKey = "SaveParentInterview", MatchFormValue = "Submit")]
        public ActionResult ParentInterviewSubmit(ParentInterviewViewModel model, string a)
        {
            MelmarkDBEntities objData = new MelmarkDBEntities();
            string returnVal = "";
            bool success = false;

            ClsCommon updateCommon = new ClsCommon();
            sess = (clsSession)Session["UserSession"];
            try
            {
                if (sess != null)
                {
                    int qId = updateCommon.getQueueId("PI");
                    //returnVal = updateCommon.IsSubmit(qId);

                    //if (returnVal == "")
                    //{
                    ParentInterviewViewModel shAssmtShe = new ParentInterviewViewModel();
                    if (model.ChkAll != null)
                    {
                        if (model.ChkAll.Count > 0)
                        {
                            success = updateCommon.getCheckedCheckListMul(model.ChkAll);

                            //if (success == true)
                            //{
                            model.Comment = updateCommon.UpdateOrInsertRevCmt("PI", model.Comment.academicReviewId, model.Comment.Comments, "N", model.Comment.IsPresent, model.Comment.AproveInt, "RS");
                            model.ChkAll = updateCommon.UpdateOrInsertMulCheckList(model.ChkAll, model.Comment.academicReviewId, "PI", model.Comment.QstatusId, "Check");
                            //  model.enginLetterList0 = updateCommon.UpdateOrInsertCheckList(model.enginLetterList0, model.Comment.academicReviewId, "PI", model.Comment.QstatusId, "Check");
                            //model.enginLetterList1 = updateCommon.UpdateOrInsertCheckList(model.enginLetterList1, model.Comment.academicReviewId, "PI", model.Comment.QstatusId, "Check1");
                            // model.enginLetterList2 = updateCommon.UpdateOrInsertCheckList(model.enginLetterList2, model.Comment.academicReviewId, "PI", model.Comment.QstatusId, "Check2");
                            sess = (clsSession)Session["UserSession"];

                            if (model.Comment.AproveInt == true)
                            {
                                int qIdNext = updateCommon.getQueueId("RS");
                                returnVal = "success*" + sess.ReferralId + "_" + qId;
                            }
                            else
                            {
                                //insert Letter
                                int QUeueid = updateCommon.getQueueId("PI");
                                var LetterName = objData.LetterEngines.Where(x => x.QueueId == QUeueid && x.ApproveStatus == false && x.SchoolId == sess.SchoolId && x.LetterType == "Letter").ToList();
                                if (LetterName.Count > 0)
                                    getLetterTray.insertLetter("PI", sess.ReferralId, LetterName[0].LetterEngineId, "Parent");
                                LetterGeneration.LetterLists = getLetterTray.getLetterList("");

                                int qIdInActive = updateCommon.getQueueId("IL");
                                returnVal = "success*" + sess.ReferralId + "_" + qIdInActive;


                                var studentPer = objData.StudentPersonals.Where(x => x.StudentPersonalId == sess.ReferralId).SingleOrDefault();
                                studentPer.InactiveList = true;
                                studentPer.WaitingList = false;
                                objData.SaveChanges();
                            }
                        }
                        //    else
                        //    {
                        //        model.Comment = updateCommon.UpdateOrInsertRevCmt("PI", model.Comment.academicReviewId, model.Comment.Comments, "Y", model.Comment.IsPresent, model.Comment.AproveInt, "IA");
                        //        model.ChkAll = updateCommon.UpdateOrInsertMulCheckList(model.ChkAll, model.Comment.academicReviewId, "PI", model.Comment.QstatusId, "Check");
                        //        //  model.enginLetterList0 = updateCommon.UpdateOrInsertCheckList(model.enginLetterList0, model.Comment.academicReviewId, "PI", model.Comment.QstatusId, "Check");
                        //        //  model.enginLetterList1 = updateCommon.UpdateOrInsertCheckList(model.enginLetterList1, model.Comment.academicReviewId, "PI", model.Comment.QstatusId, "Check1");
                        //        //   model.enginLetterList2 = updateCommon.UpdateOrInsertCheckList(model.enginLetterList2, model.Comment.academicReviewId, "PI", model.Comment.QstatusId, "Check2");
                        //        returnVal = clsGeneral.warningMsg("Checklist items are not completed so data submission is not possible.");
                        //    }
                        //}
                    }
                    else
                    {
                        model.Comment = updateCommon.UpdateOrInsertRevCmt("PI", model.Comment.academicReviewId, model.Comment.Comments, "N", model.Comment.IsPresent, model.Comment.AproveInt, "RS");
                        model.ChkAll = updateCommon.UpdateOrInsertMulCheckList(model.ChkAll, model.Comment.academicReviewId, "PI", model.Comment.QstatusId, "Check");
                        //  model.enginLetterList0 = updateCommon.UpdateOrInsertCheckList(model.enginLetterList0, model.Comment.academicReviewId, "PI", model.Comment.QstatusId, "Check");
                        //model.enginLetterList1 = updateCommon.UpdateOrInsertCheckList(model.enginLetterList1, model.Comment.academicReviewId, "PI", model.Comment.QstatusId, "Check1");
                        // model.enginLetterList2 = updateCommon.UpdateOrInsertCheckList(model.enginLetterList2, model.Comment.academicReviewId, "PI", model.Comment.QstatusId, "Check2");
                        sess = (clsSession)Session["UserSession"];

                        if (model.Comment.AproveInt == true)
                        {
                            int qIdNext = updateCommon.getQueueId("RS");
                            returnVal = "success*" + sess.ReferralId + "_" + qId;
                        }
                        else
                        {
                            //insert Letter
                            int QUeueid = updateCommon.getQueueId("PI");
                            var LetterName = objData.LetterEngines.Where(x => x.QueueId == QUeueid && x.ApproveStatus == false && x.SchoolId == sess.SchoolId && x.LetterType == "Letter").ToList();
                            if (LetterName.Count > 0)
                                getLetterTray.insertLetter("PI", sess.ReferralId, LetterName[0].LetterEngineId, "Parent");
                            LetterGeneration.LetterLists = getLetterTray.getLetterList("");

                            int qIdInActive = updateCommon.getQueueId("IL");
                            returnVal = "success*" + sess.ReferralId + "_" + qIdInActive;


                            var studentPer = objData.StudentPersonals.Where(x => x.StudentPersonalId == sess.ReferralId).SingleOrDefault();
                            studentPer.InactiveList = true;
                            objData.SaveChanges();
                        }
                    }
                    //else
                    //{
                    //    model.Comment = updateCommon.UpdateOrInsertRevCmt("PI", model.Comment.academicReviewId, model.Comment.Comments, "Y", model.Comment.IsPresent, model.Comment.AproveInt, "IA");
                    //    model.ChkAll = updateCommon.UpdateOrInsertMulCheckList(model.ChkAll, model.Comment.academicReviewId, "PI", model.Comment.QstatusId, "Check");
                    //    returnVal = clsGeneral.warningMsg("Checklist items are not assigned..");
                    //}
                    //}
                    //else
                    //{
                    //    model.Comment = updateCommon.UpdateOrInsertRevCmt("PI", model.Comment.academicReviewId, model.Comment.Comments, "Y", model.Comment.IsPresent, model.Comment.AproveInt, "IA");
                    //    model.ChkAll = updateCommon.UpdateOrInsertMulCheckList(model.ChkAll, model.Comment.academicReviewId, "PI", model.Comment.QstatusId, "Check");
                    //    returnVal = clsGeneral.warningMsg("Checklist items are not assigned..");
                    //}

                    //}

                }
                else
                {
                    returnVal = clsGeneral.failedMsg("Session not available");
                    //return Content(returnVal);
                }
                return Content(returnVal);
            }
            catch (Exception e)
            {
                returnVal = clsGeneral.failedMsg(e.Message);
                return Content(returnVal);
            }
        }


        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult userList(CommanUserViewModel academicModel, string userIdz = "", string ChkListId = "", string ChkCounti = "", string ChkCountj = "")
        {
            sess = (clsSession)Session["UserSession"];
            ClsCommon getcheck = new ClsCommon();
            academicModel.userList = getcheck.getUserList(sess.SchoolId);
            academicModel.userIdz = userIdz;
            academicModel.CheckListId = ChkListId;
            academicModel.ChkCountj = ChkCountj;
            academicModel.ChkCounti = ChkCounti;
            return View("userList", academicModel);
        }

        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult userListParentInterview(CommanUserViewModel academicModel, string userIdz = "", string ChkListId = "", int ChkCount = 0)
        {
            sess = (clsSession)Session["UserSession"];
            ClsCommon getcheck = new ClsCommon();
            academicModel.userList = getcheck.getUserList(sess.SchoolId);
            academicModel.userIdz = userIdz;
            academicModel.CheckListId = ChkListId;
            academicModel.ChkCount = ChkCount;
            return View("userListForParentInterview", academicModel);
        }


        //Intake Assessment
        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult ViewIntakeAssessment(ListIntakeAssessment model)
        {
            ClsCommon getcheck = new ClsCommon();
            sess = (clsSession)Session["UserSession"];
            if (sess != null)
            {
                ViewBag.permission = getcheck.setPermission();
                model.GetCheckLists(sess.ReferralId, sess.SchoolId);
                int Id = getcheck.getQueueStatusIdIfSubmitted(sess.ReferralId, "IE");
                if (Id > 0)
                {
                    model.isSubmit = true;
                }
            }
            return View(model);
        }

        //[ActiveSession]
        [HttpPost, ValidateInput(false)]
        [MultiButton(MatchFormKey = "SaveIntakeAsmnt", MatchFormValue = "Save")]
        public ActionResult SaveIntake(ListIntakeAssessment model)
        {
            sess = (clsSession)Session["UserSession"];
            string result = "";
            if (sess != null)
            {
                result = model.SaveData(sess.ReferralId, sess.SchoolId, "Y", sess.LoginId, "Save");
                if (result == "Success")
                    result = clsGeneral.sucessMsg("Successfully Saved");
                else
                    result = clsGeneral.failedMsg(result);
                //ViewData["Chkmsg"] = result;
            }
            model.GetCheckLists(sess.ReferralId, sess.SchoolId);
            //return View("ViewIntakeAssessment", model);
            return Content(result);
        }

        //[ActiveSession]
        [HttpPost, ValidateInput(false)]
        [MultiButton(MatchFormKey = "SaveIntakeAsmnt", MatchFormValue = "Submit")]
        public ActionResult SaveIntake(ListIntakeAssessment model, string a)
        {
            ClsCommon clsComm = new ClsCommon();
            MelmarkDBEntities objData = new MelmarkDBEntities();
            sess = (clsSession)Session["UserSession"];
            string result = "";
            if (sess != null)
            {
                int qId = clsComm.getQueueId("IE");
                //result = clsComm.IsSubmit(qId);

                //if (result == "")
                //{
                result = model.SaveData(sess.ReferralId, sess.SchoolId, "N", sess.LoginId, "Submit");
                //if (result == "Success" || result== "Assessment Checklist not complete...")
                //{
                int qIdNext = clsComm.getQueueId("AT");
                //insert Letter
                int QUeueid = clsComm.getQueueId("IE");
                var LetterName = objData.LetterEngines.Where(x => x.QueueId == QUeueid && x.ApproveStatus == model.Approved && x.SchoolId == sess.SchoolId && x.LetterType == "Letter").ToList();
                if (LetterName.Count > 0)
                    getLetterTray.insertLetter("IE", sess.ReferralId, LetterName[0].LetterEngineId, "Parent");
                LetterGeneration.LetterLists = getLetterTray.getLetterList("");

                int Id = clsComm.getQueueStatusIdIfSubmitted(sess.ReferralId, "IE");
                if (Id > 0)
                {
                    model.isSubmit = true;
                }

                if (model.Approved != true)
                {


                    int qIdInActive = clsComm.getQueueId("IL");

                    var studentPer = objData.StudentPersonals.Where(x => x.StudentPersonalId == sess.ReferralId).SingleOrDefault();
                    studentPer.InactiveList = true;
                    studentPer.WaitingList = false;
                    objData.SaveChanges();

                    return Content("success*" + sess.ReferralId + "_" + qIdInActive);


                }
                else
                {

                    return Content("success*" + sess.ReferralId + "_" + qId);

                }
                //}



                //}
            }
            //return View("ViewIntakeAssessment", model);
            return Content(result);
        }


        //Admission Review
        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult AdmissionReviewTeam(AdmissionReviewViewModel model)
        {
            sess = (clsSession)Session["UserSession"];
            ClsCommon getcheck = new ClsCommon();
            if (sess != null)
            {
                int QueueId = getcheck.getQueueId("RT");
                // string retVal = getcheck.IsSubmit(QueueId);
                //if (retVal == "")
                //  {
                ViewBag.permission = getcheck.setPermission();
                model.GetAdmissionReviewTeam(sess.ReferralId, sess.SchoolId, sess.LoginId);
                // }
            }
            return View(model);
        }


        //[ActiveSession]
        [HttpPost, ValidateInput(false)]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        [MultiButton(MatchFormKey = "SaveAdmsnReview", MatchFormValue = "Save")]
        public ActionResult SaveAdmission(AdmissionReviewViewModel model)
        {
            sess = (clsSession)Session["UserSession"];
            string result = "";
            if (sess != null)
            {
                result = model.SaveData(sess.ReferralId, sess.SchoolId, "Y", sess.LoginId);
                if (result == "Success")
                    result = clsGeneral.sucessMsg("Successfully Saved");
                else
                    result = clsGeneral.failedMsg(result);
                //ViewData["Chkmsg"] = result;
            }
            //return View("AdmissionReviewTeam", model);
            return Content(result);
        }

        //[ActiveSession]
        [HttpPost, ValidateInput(false)]
        [MultiButton(MatchFormKey = "SaveAdmsnReview", MatchFormValue = "Submit")]
        public ActionResult SaveAdmission(AdmissionReviewViewModel model, string a)
        {
            MelmarkDBEntities objData = new MelmarkDBEntities();
            ClsCommon clsComm = new ClsCommon();
            sess = (clsSession)Session["UserSession"];
            string result = "";
            if (sess != null)
            {
                int qId = clsComm.getQueueId("AT");
               // result = clsComm.IsSubmit(qId);

                //if (result == "")
                //{
                result = model.SaveData(sess.ReferralId, sess.SchoolId, "N", sess.LoginId);
                // if (result == "Success")
                //{
                if (model.Approved == true)
                {
                    int qIdNext = clsComm.getQueueId("SI");
                    return Content("success*" + sess.ReferralId + "_" + qId);
                }
                else
                {
                    //insert Letter
                    int QUeueid = clsComm.getQueueId("AT");
                    var LetterName = objData.LetterEngines.Where(x => x.QueueId == QUeueid && x.ApproveStatus == false && x.SchoolId == sess.SchoolId && x.LetterType == "Letter").ToList();
                    if (LetterName.Count > 0)
                        getLetterTray.insertLetter("AT", sess.ReferralId, LetterName[0].LetterEngineId, "Parent");
                    LetterGeneration.LetterLists = getLetterTray.getLetterList("");

                    int qIdInActive = clsComm.getQueueId("IL");

                    var studentPer = objData.StudentPersonals.Where(x => x.StudentPersonalId == sess.ReferralId).SingleOrDefault();
                    studentPer.InactiveList = true;
                    studentPer.WaitingList = false;
                    objData.SaveChanges();

                    return Content("success*" + sess.ReferralId + "_" + qIdInActive);
                }
                //}
                // ViewData["Chkmsg"] = clsGeneral.failedMsg(result);

                //}
            }
            //return View("AdmissionReviewTeam", model);
            return Content(result);
        }

        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        [HttpPost, ValidateInput(false)]
        [MultiButton(MatchFormKey = "SaveAdmsnReview", MatchFormValue = "Send")]
        public ActionResult SaveAdmission(AdmissionReviewViewModel model, string a, string b)
        {
            sess = (clsSession)Session["UserSession"];
            ClsCommon comm = new ClsCommon();
            string result = "";
            if (sess != null)
            {
                int QueueId = comm.getQueueId("RT");
                //string retVal = comm.IsSubmit(QueueId);
                //if (retVal == "")
                //{
                result = model.SendNotification(sess.ReferralId, sess.SchoolId, sess.LoginId);
                //}
                //else
                //{
                //    result = clsGeneral.failedMsg("Notification cant be sent unless Referral team Assign is submitted");
                //}
                //ViewData["Chkmsg"] = result;


            }
            //return View("AdmissionReviewTeam", model);
            return Content(result);
        }



        //IEP singOff
        [HttpGet, ValidateInput(false)]
        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult IEPsignoff(IEPsignoffModel model, string msg = "")
        {
            ViewBag.Chkmsg = msg;
            ViewBag.Flag = 0;
            ClsCommon getcheck = new ClsCommon();
            sess = (clsSession)Session["UserSession"];
            if (sess != null)
                ViewBag.permission = getcheck.setPermission();
            return View(model);
        }

        [HttpPost, ValidateInput(false)]
        [MultiButton(MatchFormKey = "SaveIEP", MatchFormValue = "Save")]
        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult SaveIEP(IEPsignoffModel model)
        {
            MelmarkDBEntities objData = new MelmarkDBEntities();
            ClsCommon clsComm = new ClsCommon();
            ViewBag.Chkmsg = "";
            ViewBag.Flag = 1;
            sess = (clsSession)Session["UserSession"];
            if (sess != null)
            {
                string result = "";
                int Qid = clsComm.getQueueId("IP");
                //bool AccptanceDraftMode = clsComm.AcceptanceSubQDraftMode(Qid);
                //if (AccptanceDraftMode == true)
                //{
                result = model.Save("Y", sess.ReferralId, sess.SchoolId, sess.LoginId, model.IEPLists);
                //}
                //else
                //{
                //    result = clsGeneral.warningMsg("Data cannot be Saved. One queue is already in drafted mode.");
                //}

                ViewBag.Chkmsg = result;
                //model.GetConsents(sess.ReferralId, sess.SchoolId);
            }
            return View("IEPsignoff", model);
        }

        [HttpPost, ValidateInput(false)]
        [MultiButton(MatchFormKey = "SaveIEP", MatchFormValue = "Submit")]
        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult SaveIEP(IEPsignoffModel model, string a)
        {
            ViewBag.Flag = 0;
            MelmarkDBEntities objData = new MelmarkDBEntities();
            ClsCommon clsComm = new ClsCommon();
            string result = "";
            sess = (clsSession)Session["UserSession"];
            if (sess != null)
            {

                int qId = clsComm.getQueueId("IP");
               // result = clsComm.IsSubmit(qId);

                //if (result == "")
                //{

                int Qid = clsComm.getQueueId("IP");
                bool AccptanceDraftMode = clsComm.AcceptanceSubQDraftMode(Qid);
                //if (AccptanceDraftMode == true)
                //{
                result = model.Save("N", sess.ReferralId, sess.SchoolId, sess.LoginId, model.IEPLists);
                if (result == clsGeneral.sucessMsg("Successfully Saved"))
                {
                    ////insert Letter
                    //int QUeueid = clsComm.getQueueId("IP");
                    //var LetterName = objData.LetterEngines.Where(x => x.QueueId == QUeueid && x.SchoolId == sess.SchoolId && x.LetterType == "Letter").ToList();
                    //if (LetterName.Count > 0)
                    //    getLetterTray.insertLetter("IP", sess.ReferralId, LetterName[0].LetterEngineId, "Parent");
                    //LetterGeneration.LetterLists = getLetterTray.getLetterList("");

                    int qIdNext = clsComm.getQueueId("PM");
                    return Content("success*" + sess.ReferralId + "_" + Qid);
                }
                //}
                //else
                //{
                //    result = clsGeneral.warningMsg("Data cannot be Submitted. One queue is already in drafted mode.");
                //}

                //model.GetConsents(sess.ReferralId, sess.SchoolId);


                //}
            }
            ViewBag.Chkmsg = result;
            return View("IEPsignoff", model);
        }

        [HttpPost, ValidateInput(false)]
        [MultiButton(MatchFormKey = "SaveIEP", MatchFormValue = "Add")]
        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult SaveIEP(IEPsignoffModel model, HttpPostedFileBase Upfile)
        {
            ViewBag.Chkmsg = "";
            ViewBag.Flag = 1;
            sess = (clsSession)Session["UserSession"];
            if (sess != null)
            {
                if (Upfile != null && Upfile.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(Upfile.FileName);

                    //string path = System.Web.Configuration.WebConfigurationManager.AppSettings["filesLocation"].ToString();
                    //string path = @"D:\SavedDocs\";
                    //if (!Directory.Exists(path))
                    //{
                    //    Directory.CreateDirectory(path);
                    //}

                    int Docid = model.FileUpload(sess.ReferralId, sess.SchoolId, model.DocumentName, fileName, sess.LoginId);
                    clsDocumentasBinary objBinary = new clsDocumentasBinary();
                    objBinary.SaveBinaryFiles(sess.SchoolId, sess.ReferralId, model.DocumentName, sess.LoginId, Upfile, "Referal", "IEP", Docid);
                    //if (id > 0)
                    //    Upfile.SaveAs(path + id + "-" + fileName);
                }
                model.GetConsents(sess.ReferralId, sess.SchoolId);
            }
            return View("IEPsignoff", model);
        }


        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public void DownloadConsent(int id)
        {
            string filePath = "";
            string result = "";
            sess = (clsSession)Session["UserSession"];
            MelmarkDBEntities objData = new MelmarkDBEntities();
            if (sess != null)
            {

                var binDoc = objData.binaryFiles.Where(x => x.DocId == id && x.SchoolId == sess.SchoolId).ToList();

                var DocList = (from Doc in binDoc
                               select new DocumentListWithBinary
                               {
                                   DocId = Doc.BinaryId,
                                   DocName = Doc.DocumentName,
                                   ContentType = Doc.ContentType.ToString(),
                                   Data = Doc.Data,
                               }).SingleOrDefault();


                ShowDocument(DocList.DocName, DocList.Data, DocList.ContentType);
                //result = model.DownloadDoc(id, sess.ReferralId);

                //string[] Filename = result.Split('/');
                //filePath = Filename[Filename.Length - 1];
                //if (result.Contains(".jpg") || result.Contains(".jpeg") || result.Contains(".png"))
                //    return File(result, System.Net.Mime.MediaTypeNames.Image.Jpeg);
                //else if (result.Contains(".gif"))
                //{
                //    return File(result, System.Net.Mime.MediaTypeNames.Image.Gif);
                //}
                //else if (result.Contains(".tiff"))
                //{
                //    return File(result, System.Net.Mime.MediaTypeNames.Image.Tiff);
                //}
                //else if (result.Contains(".pdf"))
                //{
                //    return File(result, System.Net.Mime.MediaTypeNames.Application.Pdf);
                //}
                //else if (result.Contains(".doc"))
                //{
                //    return File(result, System.Net.Mime.MediaTypeNames.Application.Rtf, "Document.doc");

                //}

            }
            //return File(result, Server.UrlEncode(result));

        }

        private void ShowDocument(string fileName, byte[] fileContent, string ContentType)
        {
            ClsCommon cmn = new ClsCommon();
            string Ext = cmn.getFileExtention(ContentType);
            fileName = fileName + Ext;
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = ContentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
            Response.BinaryWrite(fileContent);
            Response.Flush();
            Response.End();

        }

        //Contract
        [HttpGet, ValidateInput(false)]
        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult Contract(ContractViewModel model, string msg = "")
        {
            ViewBag.Chkmsg = msg;
            ViewBag.Flag = 0;
            ClsCommon getcheck = new ClsCommon();
            sess = (clsSession)Session["UserSession"];
            if (sess != null)
            {
                ViewBag.permission = getcheck.setPermission();
                model.GetConsents(sess.ReferralId, sess.SchoolId);
            }
            return View(model);
        }

        [HttpPost, ValidateInput(false)]
        [MultiButton(MatchFormKey = "SaveContract", MatchFormValue = "Save")]
        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult SaveContract(ContractViewModel model)
        {
            //    var Email = model.Email;
            //    var Phone = model.Phone;
            //    bool Result = true;
            //    bool pResult = true;
            string result = "";

            //    if (Email == null && Email != "")
            //    {
            //        Result = clsGeneral.isValidEmail(Email);
            //    }
            //    if (Phone == null && Phone != "")
            //    {
            //        pResult = clsGeneral.IsPhoneNumber(Phone);
            //    }



            //    if (Result == true && pResult == true)
            //    {
            ViewBag.Chkmsg = "";
            ViewBag.Flag = 1;
            sess = (clsSession)Session["UserSession"];
            ClsCommon clsComm = new ClsCommon();
            if (sess != null)
            {

                int Qid = clsComm.getQueueId("CT");
                bool AccptanceDraftMode = clsComm.AcceptanceSubQDraftMode(Qid);
                //if (AccptanceDraftMode == true)
                //{
                model.Emailhidden = model.Email;
                model.Phonehidden = model.Phone;
                result = model.Save("Y", sess.ReferralId, sess.SchoolId, sess.LoginId, model.ContractLists);
                //}
                //else
                //{
                //    result = clsGeneral.warningMsg("Data cannot be Saved. One queue is already in drafted mode.");
                //}
                ViewBag.Chkmsg = result;
                //model.GetConsents(sess.ReferralId, sess.SchoolId);
                model.Email = model.Emailhidden;
                model.Phone = model.Phonehidden;
            }
            return View("Contract", model);
            //}
            //else
            //{
            //    if (Result == false)
            //    {
            //        result = clsGeneral.warningMsg("Invalid Email..");
            //        return View();
            //    }
            //    else if (pResult == false)
            //    {
            //        result = clsGeneral.warningMsg("Invalid Phone number.. Format is (xxx)xxx-xxxx");
            //        return View();
            //    }
            //}
            //return View();
        }

        [HttpPost, ValidateInput(false)]
        [MultiButton(MatchFormKey = "SaveContract", MatchFormValue = "Submit")]
        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult SaveContract(ContractViewModel model, string a)
        {
            ViewBag.Flag = 0;
            MelmarkDBEntities objData = new MelmarkDBEntities();
            ClsCommon clsComm = new ClsCommon();
            string result = "";
            sess = (clsSession)Session["UserSession"];
            if (sess != null)
            {

                int qId = clsComm.getQueueId("CT");
               // result = clsComm.IsSubmit(qId);

                //if (result == "")
                //{
                int Qid = qId;
                bool AccptanceDraftMode = clsComm.AcceptanceSubQDraftMode(Qid);
                //if (AccptanceDraftMode == true)
                //{
                result = model.Save("N", sess.ReferralId, sess.SchoolId, sess.LoginId, model.ContractLists);
                if (result == clsGeneral.sucessMsg("Successfully Saved"))
                {
                    ////insert Letter
                    //int QUeueid = clsComm.getQueueId("CT");
                    //var LetterName = objData.LetterEngines.Where(x => x.QueueId == QUeueid && x.SchoolId == sess.SchoolId && x.LetterType == "Letter").ToList();
                    //if (LetterName.Count > 0)
                    //    getLetterTray.insertLetter("CT", sess.ReferralId, LetterName[0].LetterEngineId, "Parent");
                    //LetterGeneration.LetterLists = getLetterTray.getLetterList("");

                    int qIdNext = clsComm.getQueueId("DC");
                    return Content("success*" + sess.ReferralId + "_" + qId);
                }
                //}
                //else
                //{
                //    result = clsGeneral.warningMsg("Data cannot be Submitted. One queue is already in drafted mode.");
                //}
                ViewBag.Chkmsg = result;
                model.GetConsents(sess.ReferralId, sess.SchoolId);

            }
            // }
            ViewBag.Chkmsg = result;
            return View("Contract", model);
        }

        [HttpPost, ValidateInput(false)]
        [MultiButton(MatchFormKey = "SaveContract", MatchFormValue = "Add")]
        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult SaveContract(ContractViewModel model, HttpPostedFileBase Upfile)
        {
            ViewBag.Chkmsg = "";
            ViewBag.Flag = 1;
            sess = (clsSession)Session["UserSession"];
            if (sess != null)
            {
                model.Emailhidden = model.Email;
                model.Phonehidden = model.Phone;
                string result = model.Save("Y", sess.ReferralId, sess.SchoolId, sess.LoginId, model.ContractLists);
                result = "";
                ViewBag.Chkmsg = result;
                if (Upfile != null && Upfile.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(Upfile.FileName);
                    //string path = System.Web.Configuration.WebConfigurationManager.AppSettings["filesLocation"].ToString();
                    //string path = @"D:\SavedDocs\";
                    //if (!Directory.Exists(path))
                    //{
                    //    Directory.CreateDirectory(path);
                    //}

                    int Docid = model.FileUpload(sess.ReferralId, sess.SchoolId, model.DocumentName, fileName, sess.LoginId);

                    clsDocumentasBinary objBinary = new clsDocumentasBinary();
                    objBinary.SaveBinaryFiles(sess.SchoolId, sess.ReferralId, model.DocumentName, sess.LoginId, Upfile, "Referal", "Contract", Docid);
                    ////if (id > 0)
                    ////    Upfile.SaveAs(path + id + "-" + fileName);
                }
                model.GetConsents(sess.ReferralId, sess.SchoolId);
                model.Email = model.Emailhidden;
                model.Phone = model.Phonehidden;
            }
            return View("Contract", model);
        }

        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public void DownloadContract(int id)
        {
            string filePath = "";
            string result = "";
            MelmarkDBEntities objData = new MelmarkDBEntities();
            sess = (clsSession)Session["UserSession"];
            if (sess != null)
            {

                var binDoc = objData.binaryFiles.Where(x => x.DocId == id && x.SchoolId == sess.SchoolId).ToList();

                var DocList = (from Doc in binDoc
                               select new DocumentListWithBinary
                               {
                                   DocId = Doc.BinaryId,
                                   DocName = Doc.DocumentName,
                                   ContentType = Doc.ContentType.ToString(),
                                   Data = Doc.Data,
                               }).SingleOrDefault();


                ShowDocument(DocList.DocName, DocList.Data, DocList.ContentType);
                //    result = model.DownloadDoc(id, sess.ReferralId);

                //    string[] Filename = result.Split('/');
                //    filePath = Filename[Filename.Length - 1];
                //    if (result.Contains(".jpg") || result.Contains(".jpeg") || result.Contains(".png"))
                //        return File(result, System.Net.Mime.MediaTypeNames.Image.Jpeg);
                //    else if (result.Contains(".gif"))
                //    {
                //        return File(result, System.Net.Mime.MediaTypeNames.Image.Gif);
                //    }
                //    else if (result.Contains(".tiff"))
                //    {
                //        return File(result, System.Net.Mime.MediaTypeNames.Image.Tiff);
                //    }
                //    else if (result.Contains(".pdf"))
                //    {
                //        return File(result, System.Net.Mime.MediaTypeNames.Application.Pdf);
                //    }
                //    else if (result.Contains(".doc"))
                //    {
                //        return File(result, System.Net.Mime.MediaTypeNames.Application.Rtf, "Document.doc");

                //    }

                //}
                //return File(result, Server.UrlEncode(result));
            }

        }



        //Shedule Pre Admission Meeting
        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult SchedulePreAdmissionMeeting()
        {
            PreAdmissionMeetingViewModel Meeting = new PreAdmissionMeetingViewModel();
            ClsCommon updateCommon = new ClsCommon();
            sess = (clsSession)Session["UserSession"];
            if (sess != null)
            {
                ViewBag.permission = updateCommon.setPermission();
                MelmarkDBEntities objData = new MelmarkDBEntities();
                QstatusDetails qDetails = updateCommon.getQueueStatusId(sess.ReferralId, "SM");
                int QstatusId = qDetails.QueueStatusId;


                if (QstatusId > 0)
                {
                    int QstatusIds = updateCommon.getQueueStatusIdIfSubmitted(sess.ReferralId, "SM");
                    if (QstatusIds > 0)
                    {
                        Meeting.IsSubmit = true;
                    }
                }

                var CallLogVar = (from x in objData.ref_CallLogs
                                  where (x.QueueStatusId == QstatusId && x.StudentId == sess.ReferralId)
                                  select new CommonCallLogViewModel
                                  {
                                      academicReviewId = x.AcReviewId,
                                      AppntTime = x.AppointmentTime,
                                      CallLogId = x.CallLogId,
                                      CallTime = x.CallTime,
                                      Conversation = x.Conversation,
                                      Draft = x.Draft,
                                      NameOfContact = x.Nameofcontact,
                                      SchoolId = x.SchoolId,
                                      StaffName = x.StaffName,
                                      StudentId = x.StudentId,
                                      type = x.Type

                                  }).ToList();
                IList<CommonCallLogViewModel> defaltNullVal = new List<CommonCallLogViewModel>();

                //Checkin weather the Value Present or not in table
                if (CallLogVar.Count > 0)
                {
                    //setting IsPresent Flag
                    foreach (var item in CallLogVar)
                    {
                        item.IsPresent = true;
                        //item.AppntTimeShow = (item.AppntTime == null) ? "" : item.AppntTime.Value.ToString("MM'/'dd'/'yyyy");
                        item.AppntDateShow = (item.AppntTime == null) ? "" : item.AppntTime.Value.ToString("MM'/'dd'/'yyyy");
                        item.AppntTimeShow = (item.AppntTime == null) ? "" : item.AppntTime.Value.ToString("HH':'mm");
                        item.CallDateShow = (item.CallTime == null) ? "" : item.CallTime.Value.ToString("MM'/'dd'/'yyyy");
                        item.CallTimeShow = (item.CallTime == null) ? "" : item.CallTime.Value.ToString("HH':'mm");
                        item.IsSubmit = updateCommon.checkIfSubmitted(sess.ReferralId, "SM");
                    }
                    Meeting.CallLog = CallLogVar;
                    return View("ShedulePreAdmissionMeeting", Meeting);
                }
                else
                {
                    //setting IsPresent Flag
                    //defaltNullVal.IsPresent = false;
                    //defaltNullVal.academicReviewId = 0;
                    CommonCallLogViewModel defaultmodel = new CommonCallLogViewModel();
                    defaultmodel.StaffName = sess.UserName;
                    Meeting.CallLog.Add(defaultmodel);
                    //Meeting.CallLog = defaltNullVal;
                    return View("ShedulePreAdmissionMeeting", Meeting);
                }

            }
            else
            {
                IList<CommonCallLogViewModel> defaltNullVal = new List<CommonCallLogViewModel>();
                Meeting.CallLog = defaltNullVal;
                return View("ShedulePreAdmissionMeeting", Meeting);
            }


        }

        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        [HttpPost, ValidateInput(false)]
        [MultiButton(MatchFormKey = "SaveSchedulePreAdmissionMeeting", MatchFormValue = "Save")]
        public ActionResult SchedulePreAdmissionMeetingSave(PreAdmissionMeetingViewModel model)
        {
            try
            {
                sess = (clsSession)Session["UserSession"];
                ClsCommon objclsCommon = new ClsCommon();
                if (sess != null)
                {
                    string returnVal = "";
                    ClsCommon updateCommon = new ClsCommon();
                    MelmarkDBEntities objData = new MelmarkDBEntities();
                    int Qid = updateCommon.getQueueId("SM");
                    
                    foreach (var item in model.CallLog)
                    {
                        DateTime dtcalldate = new DateTime();
                        DateTime dtappntdate = new DateTime();
                        if (item.CallDateShow != null)
                            dtcalldate = DateTime.ParseExact(item.CallDateShow, "MM'/'dd'/'yyyy", System.Globalization.CultureInfo.CurrentCulture);
                        if (item.CallTimeShow == null && item.CallDateShow != null)
                            item.CallTimeShow = "00:00AM";
                        if (item.CallDateShow != null)
                        {
                            item.CallTime = dtcalldate.Add(TimeSpan.Parse(amPmTo24hourConverter(item.CallTimeShow)));
                        }

                        if (item.AppntDateShow != null)
                            dtappntdate = DateTime.ParseExact(item.AppntDateShow, "MM'/'dd'/'yyyy", System.Globalization.CultureInfo.CurrentCulture);
                        if (item.AppntTimeShow == null && item.AppntDateShow != null)
                            item.AppntTimeShow = "00:00AM";
                        if (item.AppntDateShow != null)
                        {
                            item.AppntTime = dtappntdate.Add(TimeSpan.Parse(amPmTo24hourConverter(item.AppntTimeShow)));
                        }
                    }

                    int qStatusId = updateCommon.insertQstatus("SM", "Y");
                    model.CallLog = updateCommon.UpdateOrInsertMultipleCallLogPre(model.CallLog, qStatusId);
                    
                    returnVal = clsGeneral.sucessMsgWithoutQuote("Data Successfully Saved");
                    //}
                    //else
                    //{
                    //    returnVal = clsGeneral.warningMsg("Data cannot be Saved. One queue is already in drafted mode.");
                    //}
                    //    return Content("<script type='text/javascript'>$.get('/IntakeAssessment/SchedulePreAdmissionMeeting', function (result) {" +
                    //    "$('#LoadQueue').html(result);" +
                    //    "$('#tdMsg').html('" + returnVal + "');" +
                    //"});</script>");
                    return Content(returnVal);

                }
                else
                {
                    string returnVal = clsGeneral.failedMsgWithoutQuote("Sorry session expired");
                    //    return Content("<script type='text/javascript'>$.get('/IntakeAssessment/SchedulePreAdmissionMeeting', function (result) {" +
                    //    "$('#LoadQueue').html(result);" +
                    //    "$('#tdMsg').html('" + returnVal + "');" +
                    //"});</script>");
                    return Content(returnVal);
                }
            }
            catch (Exception e)
            {
                string returnVal = clsGeneral.failedMsgWithoutQuote(e.Message);
                //return Content("<script type='text/javascript'>$.get('/IntakeAssessment/SchedulePreAdmissionMeeting', function (result) {" +
                //    "$('#LoadQueue').html(result);" +
                //    "$('#tdMsg').html('" + returnVal + "');" +
                //"});</script>");
                return Content(returnVal);
            }
        }




        [HttpPost, ValidateInput(false)]
        [MultiButton(MatchFormKey = "SaveSchedulePreAdmissionMeeting", MatchFormValue = "Submit")]
        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult SchedulePreAdmissionMeetingSave(PreAdmissionMeetingViewModel model, string a)
        {
            try
            {
                sess = (clsSession)Session["UserSession"];
                ClsCommon updateCommon = new ClsCommon();
                if (sess != null)
                {
                    string returnVal = "";
                    int qIds = updateCommon.getQueueId("SM");
                    //  returnVal = updateCommon.IsSubmit(qIds);
                    //if (returnVal == "")
                    //{


                    MelmarkDBEntities objData = new MelmarkDBEntities();
                    int Qid = updateCommon.getQueueId("SM");
                    foreach (var item in model.CallLog)
                    {
                        DateTime dtcalldate = new DateTime();
                        DateTime dtappntdate = new DateTime();
                        if (item.CallDateShow != null)
                            dtcalldate = DateTime.ParseExact(item.CallDateShow, "MM'/'dd'/'yyyy", System.Globalization.CultureInfo.CurrentCulture);
                        if (item.CallTimeShow == null && item.CallDateShow != null)
                            item.CallTimeShow = "00:00AM";
                        if (item.CallDateShow != null)
                        {
                            item.CallTime = dtcalldate.Add(TimeSpan.Parse(amPmTo24hourConverter(item.CallTimeShow)));
                        }

                        if (item.AppntDateShow != null)
                            dtappntdate = DateTime.ParseExact(item.AppntDateShow, "MM'/'dd'/'yyyy", System.Globalization.CultureInfo.CurrentCulture);
                        if (item.AppntTimeShow == null && item.AppntDateShow != null)
                            item.AppntTimeShow = "00:00AM";
                        if (item.AppntDateShow != null)
                        {
                            item.AppntTime = dtappntdate.Add(TimeSpan.Parse(amPmTo24hourConverter(item.AppntTimeShow)));
                        }
                    }
                    
                    int qId = updateCommon.getQueueId("SM");
                    int qProcess = updateCommon.getProcessId();
                    var qStatusRow = objData.ref_QueueStatus.Where(x => x.StudentPersonalId == sess.ReferralId && x.QueueId == qId && x.CurrentStatus == true && x.QueueProcess == qProcess).ToList();
                    int qStatusId = updateCommon.insertQstatus("SM", "Y");
                    model.CallLog = updateCommon.UpdateOrInsertMultipleCallLogPre(model.CallLog, qStatusId);
                    
                    model.IsSubmit = updateCommon.checkIfSubmitted(sess.ReferralId, "SM");
                    sess = (clsSession)Session["UserSession"];
                    int qIdNext = updateCommon.getQueueId("IP");
                    int QProcess = updateCommon.getProcessId();
                    updateCommon.insertNewStatus("SM", "IP", sess.ReferralId);

                    ////insert Letter
                    //int QUeueid = updateCommon.getQueueId("SM");
                    //var LetterName = objData.LetterEngines.Where(x => x.QueueId == QUeueid && x.SchoolId == sess.SchoolId && x.LetterType == "Letter").ToList();
                    //if (LetterName.Count > 0)
                    //    getLetterTray.insertLetter("SM", sess.ReferralId, LetterName[0].LetterEngineId, "Parent");
                    //LetterGeneration.LetterLists = getLetterTray.getLetterList("");

                    int QIdNext = updateCommon.getQueueId("IP");
                    returnVal = "success*" + sess.ReferralId + "_" + qId;
                    return Content(returnVal);
                    //}
                    //else
                    //{
                    //    returnVal = clsGeneral.failedMsg("Data cannot be Submitted. One queue is already in drafted mode.");
                    //    return Content(returnVal);
                    //}

                    //}
                    //else
                    //{
                    //    return Content(returnVal);
                    //}
                }
                else
                {
                    string returnVal = clsGeneral.failedMsg("Sorry session expired");
                    return Content(returnVal);
                }
            }
            catch (Exception e)
            {
                string returnVal = clsGeneral.failedMsg(e.Message);
                return Content(returnVal);
            }
        }



        //Referal Summary
        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult ReferalSumaryView()
        {
            ClsCommon CommonCls = new ClsCommon();
            sess = (clsSession)Session["UserSession"];
            if (sess != null)
                ViewBag.permission = CommonCls.setPermission();
            ReferalSummaryViewModel shAssmtShe = new ReferalSummaryViewModel();

            bool InActivePresent = CommonCls.checkInactive();
            //shAssmtShe.enginLetterList = CommonCls.getCheckList("RS", "Check");
            shAssmtShe.ChkAll = CommonCls.getCheckListMultiple("RS", "Check");
            shAssmtShe.Comment = CommonCls.getRevCmt("RS");
            return View("ReferalSummary", shAssmtShe);
        }


        [HttpPost, ValidateInput(false)]
        [MultiButton(MatchFormKey = "SaveReferalSummary", MatchFormValue = "Save")]
        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult ReferalSumarySubmit(ReferalSummaryViewModel model)
        {
            string returnVal = "";
            try
            {
                ClsCommon updateCommon = new ClsCommon();
                int Qid = updateCommon.getQueueId("RS");
                //bool AccptanceDraftMode = updateCommon.AcceptanceSubQDraftMode(Qid);
                //if (AccptanceDraftMode == true)
                //{
                if (model.ChkAll != null)
                {
                    foreach (var val in model.ChkAll)
                    {
                        foreach (var v in val.chkList)
                        {
                            v.AssignMultiId = "NA";
                        }
                    }
                }
                ReferalSummaryViewModel shAssmtShe = new ReferalSummaryViewModel();

                model.Comment = updateCommon.UpdateOrInsertRevCmt("RS", model.Comment.academicReviewId, model.Comment.Comments, "Y", model.Comment.IsPresent, true, "RS");
                //model.enginLetterList = updateCommon.UpdateOrInsertCheckList(model.enginLetterList, model.Comment.academicReviewId, "RS", model.Comment.QstatusId, "Check");
                model.ChkAll = updateCommon.UpdateOrInsertMulCheckList(model.ChkAll, model.Comment.academicReviewId, "RS", model.Comment.QstatusId, "Check");
                returnVal = clsGeneral.sucessMsg("Data Successfully Saved");
                //}
                //else
                //{
                //    returnVal = clsGeneral.warningMsg("Data cannot be Saved. One queue is already in drafted mode.");
                //}
                return Content(returnVal);


            }
            catch (Exception e)
            {
                returnVal = clsGeneral.failedMsg(e.Message);
                return Content(returnVal);
            }
        }



        [HttpPost, ValidateInput(false)]
        [MultiButton(MatchFormKey = "SaveReferalSummary", MatchFormValue = "Submit")]
        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult ReferalSumarySubmit(ReferalSummaryViewModel model, string a)
        {
            MelmarkDBEntities objData = new MelmarkDBEntities();
            string returnVal = "";
            bool success = false;
            sess = (clsSession)Session["UserSession"];
            ClsCommon updateCommon = new ClsCommon();

            try
            {
                if (sess != null)
                {

                    int qId = updateCommon.getQueueId("RS");
                    //returnVal = updateCommon.IsSubmit(qId);

                    //if (returnVal == "")
                    //{
                    ReferalSummaryViewModel shAssmtShe = new ReferalSummaryViewModel();

                    int Qid = updateCommon.getQueueId("RS");
                    //bool AccptanceDraftMode = updateCommon.AcceptanceSubQDraftMode(Qid);
                    //if (AccptanceDraftMode == true)
                    //{

                    if (model.ChkAll != null && model.ChkAll.Count > 0)
                    {
                        foreach (var val in model.ChkAll)
                        {
                            foreach (var v in val.chkList)
                            {
                                v.AssignMultiId = "NA";
                            }
                        }
                        success = updateCommon.getCheckedCheckListMul(model.ChkAll);

                        //bool success = updateCommon.getCheckedCheckList(model.enginLetterList);
                        //if (success == true)
                        //{
                        model.Comment = updateCommon.UpdateOrInsertRevCmt("RS", model.Comment.academicReviewId, model.Comment.Comments, "N", model.Comment.IsPresent, true, "SM");
                        //model.enginLetterList = updateCommon.UpdateOrInsertCheckList(model.enginLetterList, model.Comment.academicReviewId, "RS", model.Comment.QstatusId, "Check");
                        model.ChkAll = updateCommon.UpdateOrInsertMulCheckList(model.ChkAll, model.Comment.academicReviewId, "RS", model.Comment.QstatusId, "Check");
                        sess = (clsSession)Session["UserSession"];

                        ////insert Letter
                        //int QUeueid = updateCommon.getQueueId("RS");
                        //var LetterName = objData.LetterEngines.Where(x => x.QueueId == QUeueid && x.SchoolId == sess.SchoolId && x.LetterType == "Letter").ToList();
                        //if (LetterName.Count > 0)
                        //    getLetterTray.insertLetter("RS", sess.ReferralId, LetterName[0].LetterEngineId, "Parent");
                        //LetterGeneration.LetterLists = getLetterTray.getLetterList("");

                        int QIdNext = updateCommon.getQueueId("SM");
                        returnVal = "success*" + sess.ReferralId + "_" + Qid;
                        //}
                        //else
                        //{
                        //    model.Comment = updateCommon.UpdateOrInsertRevCmt("RS", model.Comment.academicReviewId, model.Comment.Comments, "Y", model.Comment.IsPresent, true, "RS");
                        //    //model.enginLetterList = updateCommon.UpdateOrInsertCheckList(model.enginLetterList, model.Comment.academicReviewId, "RS", model.Comment.QstatusId, "Check");
                        //    model.ChkAll = updateCommon.UpdateOrInsertMulCheckList(model.ChkAll, model.Comment.academicReviewId, "RS", model.Comment.QstatusId, "Check");
                        //    returnVal = clsGeneral.warningMsg("Checklist items are not completed so data submission is not possible.");
                        //}
                    }
                    else
                    {
                        model.Comment = updateCommon.UpdateOrInsertRevCmt("RS", model.Comment.academicReviewId, model.Comment.Comments, "N", model.Comment.IsPresent, true, "SM");
                        //model.enginLetterList = updateCommon.UpdateOrInsertCheckList(model.enginLetterList, model.Comment.academicReviewId, "RS", model.Comment.QstatusId, "Check");
                        model.ChkAll = updateCommon.UpdateOrInsertMulCheckList(model.ChkAll, model.Comment.academicReviewId, "RS", model.Comment.QstatusId, "Check");
                        sess = (clsSession)Session["UserSession"];

                        ////insert Letter
                        //int QUeueid = updateCommon.getQueueId("RS");
                        //var LetterName = objData.LetterEngines.Where(x => x.QueueId == QUeueid && x.SchoolId == sess.SchoolId && x.LetterType == "Letter").ToList();
                        //if (LetterName.Count > 0)
                        //    getLetterTray.insertLetter("RS", sess.ReferralId, LetterName[0].LetterEngineId, "Parent");
                        //LetterGeneration.LetterLists = getLetterTray.getLetterList("");

                        int QIdNext = updateCommon.getQueueId("SM");
                        returnVal = "success*" + sess.ReferralId + "_" + Qid;
                    }
                    //    else
                    //    {
                    //        model.Comment = updateCommon.UpdateOrInsertRevCmt("RS", model.Comment.academicReviewId, model.Comment.Comments, "Y", model.Comment.IsPresent, true, "RS");
                    //        //model.enginLetterList = updateCommon.UpdateOrInsertCheckList(model.enginLetterList, model.Comment.academicReviewId, "RS", model.Comment.QstatusId, "Check");
                    //        model.ChkAll = updateCommon.UpdateOrInsertMulCheckList(model.ChkAll, model.Comment.academicReviewId, "RS", model.Comment.QstatusId, "Check");
                    //        returnVal = clsGeneral.warningMsg("Checklist items are not assigned..");
                    //    }
                    //}
                    //else
                    //{
                    //    returnVal = clsGeneral.warningMsg("Data cannot be Submitted. One queue is already in drafted mode.");
                    //}
                    //}
                }
                else
                {
                    returnVal = clsGeneral.failedMsg("Session not available");
                    return Content(returnVal);
                }
                return Content(returnVal);
            }
            catch (Exception e)
            {
                returnVal = clsGeneral.failedMsg(e.Message);
                return Content(returnVal);
            }
        }


        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public List<SelectListItem> FillDropStateList()
        {
            MelmarkDBEntities objData = new MelmarkDBEntities();
            List<SelectListItem> StateList = new List<SelectListItem>();

            StateList = (from objstate in objData.LookUps
                         where objstate.LookupType == "State"
                         select new SelectListItem
                         {
                             Text = objstate.LookupName,
                             Value = SqlFunctions.StringConvert((double)objstate.LookupId).Trim()
                         }).ToList();
            return StateList;
        }


        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public List<SelectListItem> FillDropServices()
        {
            MelmarkDBEntities objData = new MelmarkDBEntities();
            List<SelectListItem> ServiceList = new List<SelectListItem>();
            ServiceList.Add(new SelectListItem() { Text = "Day", Value = "Day" });
            ServiceList.Add(new SelectListItem() { Text = "Res", Value = "Res" });
            ServiceList.Add(new SelectListItem() { Text = "None", Value = "None" });

            return ServiceList;
        }

        public List<SelectListItem> GetFundingList(int SchoolId)  //--- 22Sep2020 - List 3 - Task #2 - (Start) ---//
        {
            List<SelectListItem> result = new List<SelectListItem>();
            result.Add(new SelectListItem
            {
                Text = "---Select---",
                Value = ""
            });
            MelmarkDBEntities dbobj = new MelmarkDBEntities();
            var data = (from look in dbobj.LookUps
                        where look.LookupType == "fundingsource" && look.SchoolId == SchoolId
                        select new SelectListItem
                        {
                            Text = look.LookupName,
                            Value = SqlFunctions.StringConvert((decimal)look.LookupId).Trim(),
                        }).ToList();
            foreach (var item in data)
            {
                result.Add(item);
            }
            return result;
        } //--- 22Sep2020 - List 3 - Task #2 - (End) ---//

        //29-Oct-2020 List 3 task #26 Start-----
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult MedicalReview()
        {
            ClsCommon CommonCls = new ClsCommon();
            sess = (clsSession)Session["UserSession"];
            if (sess != null)
                ViewBag.permission = CommonCls.setPermission();
            MedicalReviewViewModel shAssmtShe = new MedicalReviewViewModel();

            bool InActivePresent = CommonCls.checkInactive();
            //shAssmtShe.enginLetterList = CommonCls.getCheckList("MR", "Check");
            shAssmtShe.ChkAll = CommonCls.getCheckListMultiple("MR", "Check");
            shAssmtShe.Comment = CommonCls.getRevCmt("MR");
            return View("MedicalReview", shAssmtShe);
        }
        //[ActiveSession]
        [HttpPost, ValidateInput(false)]
        [MultiButton(MatchFormKey = "SaveMedicalReview", MatchFormValue = "Save")]
        public ActionResult MedicalReviewSubmit(ScheduleParentScreeningViewModel model)
        {
            try
            {
                MedicalReviewViewModel shAssmtShe = new MedicalReviewViewModel();
                ClsCommon updateCommon = new ClsCommon();
                model.Comment = updateCommon.UpdateOrInsertRevCmt("MR", model.Comment.academicReviewId, model.Comment.Comments, "Y", model.Comment.IsPresent, model.Comment.AproveInt, "MR");
                model.ChkAll = updateCommon.UpdateOrInsertMulCheckList(model.ChkAll, model.Comment.academicReviewId, "MR", model.Comment.QstatusId, "Check");

                string returnVal = clsGeneral.sucessMsg("Data Successfully Saved");

                return Content(returnVal);

            }
            catch (Exception e)
            {
                string returnVal = clsGeneral.failedMsg(e.Message);
                return Content(returnVal);
            }
        }

        //[ActiveSession]
        [HttpPost, ValidateInput(false)]
        [MultiButton(MatchFormKey = "SaveMedicalReview", MatchFormValue = "Submit")]
        public ActionResult MedicalReviewSubmit(ScheduleParentScreeningViewModel model, string a)
        {
            MelmarkDBEntities objData = new MelmarkDBEntities();
            string returnVal = "";
            bool success = false;

            ClsCommon updateCommon = new ClsCommon();
            sess = (clsSession)Session["UserSession"];
            try
            {
                if (sess != null)
                {
                    int qId = updateCommon.getQueueId("MR");
                    MedicalReviewViewModel shAssmtShe = new MedicalReviewViewModel();
                    if (model.ChkAll != null && model.ChkAll.Count > 0)
                    {
                        foreach (var val in model.ChkAll)
                        {
                            foreach (var v in val.chkList)
                            {
                                v.AssignMultiId = "NA";
                            }
                        }
                        success = updateCommon.getCheckedCheckListMul(model.ChkAll);


                        //if (success == true)
                        //{
                        model.Comment = updateCommon.UpdateOrInsertRevCmt("MR", model.Comment.academicReviewId, model.Comment.Comments, "N", model.Comment.IsPresent, model.Comment.AproveInt, "CA");
                        //  model.enginLetterList = updateCommon.UpdateOrInsertCheckList(model.enginLetterList, model.Comment.academicReviewId, "PS", model.Comment.QstatusId, "Check");
                        model.ChkAll = updateCommon.UpdateOrInsertMulCheckList(model.ChkAll, model.Comment.academicReviewId, "MR", model.Comment.QstatusId, "Check");
                        sess = (clsSession)Session["UserSession"];
                        if (model.Comment.AproveInt == true)
                        {
                            int qIdNext = updateCommon.getQueueId("CA");
                            returnVal = "success*" + sess.ReferralId + "_" + qId;


                        }
                        else
                        {
                            //insert Letter
                            int QUeueid = updateCommon.getQueueId("MR");
                            var LetterName = objData.LetterEngines.Where(x => x.QueueId == QUeueid && x.ApproveStatus == false && x.SchoolId == sess.SchoolId && x.LetterType == "Letter").ToList();
                            if (LetterName.Count > 0)
                                getLetterTray.insertLetter("MR", sess.ReferralId, LetterName[0].LetterEngineId, "Parent");
                            LetterGeneration.LetterLists = getLetterTray.getLetterList("");

                            int qIdInActive = updateCommon.getQueueId("IL");
                            returnVal = "success*" + sess.ReferralId + "_" + qIdInActive;


                            var studentPer = objData.StudentPersonals.Where(x => x.StudentPersonalId == sess.ReferralId).SingleOrDefault();
                            studentPer.InactiveList = true;
                            studentPer.WaitingList = false;
                            objData.SaveChanges();
                        }
                    }
                    else
                    {
                        model.Comment = updateCommon.UpdateOrInsertRevCmt("MR", model.Comment.academicReviewId, model.Comment.Comments, "N", model.Comment.IsPresent, model.Comment.AproveInt, "CA");
                        //  model.enginLetterList = updateCommon.UpdateOrInsertCheckList(model.enginLetterList, model.Comment.academicReviewId, "PS", model.Comment.QstatusId, "Check");
                        model.ChkAll = updateCommon.UpdateOrInsertMulCheckList(model.ChkAll, model.Comment.academicReviewId, "MR", model.Comment.QstatusId, "Check");
                        sess = (clsSession)Session["UserSession"];
                        if (model.Comment.AproveInt == true)
                        {
                            int qIdNext = updateCommon.getQueueId("CA");
                            returnVal = "success*" + sess.ReferralId + "_" + qId;


                        }
                        else
                        {
                            //insert Letter
                            int QUeueid = updateCommon.getQueueId("MR");
                            var LetterName = objData.LetterEngines.Where(x => x.QueueId == QUeueid && x.ApproveStatus == false && x.SchoolId == sess.SchoolId && x.LetterType == "Letter").ToList();
                            if (LetterName.Count > 0)
                                getLetterTray.insertLetter("MR", sess.ReferralId, LetterName[0].LetterEngineId, "Parent");
                            LetterGeneration.LetterLists = getLetterTray.getLetterList("");

                            int qIdInActive = updateCommon.getQueueId("IL");
                            returnVal = "success*" + sess.ReferralId + "_" + qIdInActive;


                            var studentPer = objData.StudentPersonals.Where(x => x.StudentPersonalId == sess.ReferralId).SingleOrDefault();
                            studentPer.InactiveList = true;
                            objData.SaveChanges();
                        }
                    }
                }
                else
                {
                    returnVal = clsGeneral.failedMsg("Session not available.");
                }
                return Content(returnVal);
            }
            catch (Exception e)
            {
                returnVal = clsGeneral.failedMsg(e.Message);
                return Content(returnVal);
            }
        }

        //29-Oct-2020 List 3 task #26 END-----
    }
}
