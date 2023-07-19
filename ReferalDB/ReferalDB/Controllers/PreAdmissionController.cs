using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReferalDB.Models;
using BuisinessLayer;
using DataLayer;
using ReferalDB.CommonClass;
using System.IO;
using System.Web.Configuration;
using ReferalDB.AppFunctions;
using System.Data.Objects.SqlClient;


namespace ReferalDB.Controllers
{
    public class PreAdmissionController : Controller
    {
        //
        // GET: /PreAdmission/
        clsGeneral objGeneral = new clsGeneral();
        ClsCommon getcheck = new ClsCommon();
        LetterTray getLetterTray = new LetterTray();
        MelmarkDBEntities objData = new MelmarkDBEntities();
        LetterGenerationViewModel LetterGeneration = new LetterGenerationViewModel();
        public clsSession session = null;
        // int currentStatus = 0;

        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult AcademicReviews(AcademicReviewModel academicReviewModel)
        {
            //currentStatus = getcheck.getQueueStatusId(1, 1);
            session = (clsSession)Session["UserSession"];
            if (session != null)
            {
                ViewBag.permission = getcheck.setPermission();
                academicReviewModel.commonACCREV = getcheck.getRevCmt("AR");
                if (academicReviewModel.commonACCREV.academicReviewId != 0)
                    academicReviewModel.approvedStatus = academicReviewModel.commonACCREV.AproveInt == true ? 1 : 2;
                else
                    academicReviewModel.approvedStatus = 0;
                //academicReviewModel.engineLetter = getcheck.getCheckList("AR", "Check");
                academicReviewModel.ChkAll = getcheck.getCheckListMultiple("AR", "Check");
                return View("AcademicReviews", academicReviewModel);
            }
            else
                return View("AcademicReviews", academicReviewModel);
        }


        [HttpPost, ValidateInput(false)]
        [MultiButton(MatchFormKey = "Action", MatchFormValue = "Save")]
        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult AcademicReviewSave(AcademicReviewModel academicReviewModel)
        {
            try
            {
                ClsCommon updateCommon = new ClsCommon();
                session = (clsSession)Session["UserSession"];
                if (session != null)
                {
                    academicReviewModel.commonACCREV.AproveInt = academicReviewModel.approvedStatus == 1 ? true : false;
                    academicReviewModel.commonACCREV.Draft = "Y";
                    academicReviewModel.commonACCREV = getcheck.UpdateOrInsertRevCmt("AR", academicReviewModel.commonACCREV.academicReviewId, academicReviewModel.commonACCREV.Comments, academicReviewModel.commonACCREV.Draft, academicReviewModel.commonACCREV.IsPresent, academicReviewModel.commonACCREV.AproveInt, "AR");
                    //academicReviewModel.engineLetter = getcheck.UpdateOrInsertCheckList(academicReviewModel.engineLetter, academicReviewModel.commonACCREV.academicReviewId, "AR", academicReviewModel.commonACCREV.QstatusId, "Check");
                    academicReviewModel.ChkAll = updateCommon.UpdateOrInsertMulCheckList(academicReviewModel.ChkAll, academicReviewModel.commonACCREV.academicReviewId, "AR", academicReviewModel.commonACCREV.QstatusId, "Check");
                    string returnVal = clsGeneral.sucessMsg("Data Successfully Saved");
                    return Content(returnVal);
                }
                else
                {
                    string returnVal = clsGeneral.failedMsg("Session not available");
                    return Content(returnVal);
                }

            }
            catch (Exception e)
            {
                string returnVal = clsGeneral.failedMsg(e.Message);
                return Content(returnVal);
            }

        }


        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        [HttpPost, ValidateInput(false)]
        [MultiButton(MatchFormKey = "Action", MatchFormValue = "Submit")]
        public ActionResult AcademicReviewSubmit(AcademicReviewModel academicReviewModel)
        {
            string returnVal = "";
            bool success = false;
            int QidNext = getcheck.getQueueId("CR");
            int QidInactive = getcheck.getQueueId("IL");
            try
            {
                ClsCommon updateCommon = new ClsCommon();

                session = (clsSession)Session["UserSession"];
                if (session != null)
                {

                    academicReviewModel.commonACCREV.AproveInt = academicReviewModel.approvedStatus == 1 ? true : false;

                    if (academicReviewModel.ChkAll.Count > 0)
                    {
                        success = updateCommon.getCheckedCheckListMul(academicReviewModel.ChkAll);

                        //if (success == true)
                        //{
                            int Qid = getcheck.getQueueId("AR");
                            if (academicReviewModel.commonACCREV.AproveInt == false)
                            {
                                Qid = getcheck.getQueueId("AR");
                                var LetterName = objData.LetterEngines.Where(x => x.QueueId == Qid && x.ApproveStatus == false && x.SchoolId == session.SchoolId && x.LetterType == "Letter").ToList();
                                if (LetterName.Count > 0)
                                    getLetterTray.insertLetter("AR", session.ReferralId, LetterName[0].LetterEngineId, "Parent");
                                LetterGeneration.LetterLists = getLetterTray.getLetterList("");
                                var studentPer = objData.StudentPersonals.Where(x => x.StudentPersonalId == session.ReferralId).SingleOrDefault();
                                studentPer.InactiveList = true;
                                studentPer.WaitingList = false;
                                objData.SaveChanges();

                            }
                            academicReviewModel.commonACCREV.Draft = "N";
                            academicReviewModel.commonACCREV = getcheck.UpdateOrInsertRevCmt("AR", academicReviewModel.commonACCREV.academicReviewId, academicReviewModel.commonACCREV.Comments, academicReviewModel.commonACCREV.Draft, academicReviewModel.commonACCREV.IsPresent, academicReviewModel.commonACCREV.AproveInt, "CR");
                            academicReviewModel.ChkAll = updateCommon.UpdateOrInsertMulCheckList(academicReviewModel.ChkAll, academicReviewModel.commonACCREV.academicReviewId, "AR", academicReviewModel.commonACCREV.QstatusId, "Check");
                            /// academicReviewModel.commonACCREV.AproveInt = academicReviewModel.approvedStatus == 1 ? true : false;
                            if (academicReviewModel.commonACCREV.AproveInt == true)
                                returnVal = "success*" + session.ReferralId + "_" + Qid;
                            else
                                returnVal = "success*" + session.ReferralId + "_" + QidInactive;
                        //}
                        //else
                        //{
                        //    academicReviewModel.commonACCREV.Draft = "Y";
                        //    academicReviewModel.commonACCREV = getcheck.UpdateOrInsertRevCmt("AR", academicReviewModel.commonACCREV.academicReviewId, academicReviewModel.commonACCREV.Comments, academicReviewModel.commonACCREV.Draft, academicReviewModel.commonACCREV.IsPresent, academicReviewModel.commonACCREV.AproveInt, "AR");
                        //    academicReviewModel.ChkAll = updateCommon.UpdateOrInsertMulCheckList(academicReviewModel.ChkAll, academicReviewModel.commonACCREV.academicReviewId, "AR", academicReviewModel.commonACCREV.QstatusId, "Check");
                        //    returnVal = clsGeneral.warningMsg("Checklist items are not completed. So data submission is not possible.");
                        //}
                    }
                    else
                    {
                        academicReviewModel.commonACCREV.Draft = "N";
                        academicReviewModel.commonACCREV = getcheck.UpdateOrInsertRevCmt("AR", academicReviewModel.commonACCREV.academicReviewId, academicReviewModel.commonACCREV.Comments, academicReviewModel.commonACCREV.Draft, academicReviewModel.commonACCREV.IsPresent, academicReviewModel.commonACCREV.AproveInt, "AR");
                        academicReviewModel.ChkAll = updateCommon.UpdateOrInsertMulCheckList(academicReviewModel.ChkAll, academicReviewModel.commonACCREV.academicReviewId, "AR", academicReviewModel.commonACCREV.QstatusId, "Check");

                        int Qid = getcheck.getQueueId("AR");
                        if (academicReviewModel.commonACCREV.AproveInt == true)
                            returnVal = "success*" + session.ReferralId + "_" + Qid;
                        else
                            returnVal = "success*" + session.ReferralId + "_" + QidInactive;
                        //returnVal = clsGeneral.warningMsg("No Checklist Item Found.");
                        //    academicReviewModel.commonACCREV.Draft = "Y";
                        //    academicReviewModel.commonACCREV = getcheck.UpdateOrInsertRevCmt("AR", academicReviewModel.commonACCREV.academicReviewId, academicReviewModel.commonACCREV.Comments, academicReviewModel.commonACCREV.Draft, academicReviewModel.commonACCREV.IsPresent, academicReviewModel.commonACCREV.AproveInt, "AR");
                        //    academicReviewModel.ChkAll = updateCommon.UpdateOrInsertMulCheckList(academicReviewModel.ChkAll, academicReviewModel.commonACCREV.academicReviewId, "AR", academicReviewModel.commonACCREV.QstatusId, "Check");
                        //    returnVal = clsGeneral.warningMsg("Checklist items are not assigned.");
                    }





                    return Content(returnVal);

                }
                else
                {
                    returnVal = clsGeneral.failedMsg("Session not available");
                    return Content(returnVal);
                }

            }
            catch (Exception e)
            {
                returnVal = clsGeneral.failedMsg(e.Message);
                return Content(returnVal);
            }

        }



        //show userlist
        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult userList(CommanUserViewModel academicModel, string userIdz = "", string ChkListId = "", string ChkCounti = "", string ChkCountj = "")
        {
            session = (clsSession)Session["UserSession"];
            ClsCommon getcheck = new ClsCommon();
            academicModel.userList = getcheck.getUserList(session.SchoolId);
            academicModel.userIdz = userIdz;
            academicModel.CheckListId = ChkListId;
            academicModel.ChkCountj = ChkCountj;
            academicModel.ChkCounti = ChkCounti;
            return View("userList", academicModel);
        }
        //public ActionResult userList(CommanUserViewModel academicModel, string userIdz = "", string ChkListId = "")
        //{
        //    session = (clsSession)Session["UserSession"];
        //    if (session != null)
        //    {
        //        ClsCommon getcheck = new ClsCommon();
        //        academicModel.userList = getcheck.getUserList(session.SchoolId);
        //        academicModel.userIdz = userIdz;
        //        academicModel.CheckListId = ChkListId;
        //        return View(academicModel);
        //    }
        //    else
        //        return View(academicModel);
        //}



        //clinicalReview
        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult ClinicalReview(AcademicReviewModel academicReviewModel)
        {
            session = (clsSession)Session["UserSession"];
            if (session != null)
            {
                ViewBag.permission = getcheck.setPermission();
                academicReviewModel.commonACCREV = getcheck.getRevCmt("CR");
                if (academicReviewModel.commonACCREV.academicReviewId != 0)
                    academicReviewModel.approvedStatus = academicReviewModel.commonACCREV.AproveInt == true ? 1 : 2;
                else
                    academicReviewModel.approvedStatus = 0;
                academicReviewModel.ChkAll = getcheck.getCheckListMultiple("CR", "Check");
                return View("ClinicalReview", academicReviewModel);
            }
            else
                return View("ClinicalReview", academicReviewModel);
        }


        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        [HttpPost, ValidateInput(false)]
        [MultiButton(MatchFormKey = "ClinicalAction", MatchFormValue = "Save")]
        public ActionResult ClinicalReviewSave(AcademicReviewModel academicReviewModel)
        {
            try
            {
                ClsCommon updateCommon = new ClsCommon();
                session = (clsSession)Session["UserSession"];
                if (session != null)
                {
                    academicReviewModel.commonACCREV.AproveInt = academicReviewModel.approvedStatus == 1 ? true : false;
                    academicReviewModel.commonACCREV.Draft = "Y";
                    academicReviewModel.commonACCREV = getcheck.UpdateOrInsertRevCmt("CR", academicReviewModel.commonACCREV.academicReviewId, academicReviewModel.commonACCREV.Comments, academicReviewModel.commonACCREV.Draft, academicReviewModel.commonACCREV.IsPresent, academicReviewModel.commonACCREV.AproveInt, "CR");
                    academicReviewModel.ChkAll = updateCommon.UpdateOrInsertMulCheckList(academicReviewModel.ChkAll, academicReviewModel.commonACCREV.academicReviewId, "CR", academicReviewModel.commonACCREV.QstatusId, "Check");
                    //academicReviewModel.engineLetter = getcheck.UpdateOrInsertCheckList(academicReviewModel.engineLetter, academicReviewModel.commonACCREV.academicReviewId, "CR", academicReviewModel.commonACCREV.QstatusId, "Check");

                    string returnVal = clsGeneral.sucessMsg("Data Successfully Saved");
                    return Content(returnVal);
                }
                else
                {
                    string returnVal = clsGeneral.failedMsg("Session not available");
                    return Content(returnVal);
                }

            }
            catch (Exception e)
            {
                string returnVal = clsGeneral.failedMsg(e.Message);
                return Content(returnVal);
            }

        }


        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        [HttpPost, ValidateInput(false)]
        [MultiButton(MatchFormKey = "ClinicalAction", MatchFormValue = "Submit")]
        public ActionResult ClinicalReviewSubmit(AcademicReviewModel academicReviewModel)
        {
            string returnVal = "";
            bool success = false;
            int QidNext = getcheck.getQueueId("RT");
            int QidInactive = getcheck.getQueueId("IL");
            try
            {
                session = (clsSession)Session["UserSession"];
                ClsCommon updateCommon = new ClsCommon();
                if (session != null)
                {
                    int qId = updateCommon.getQueueId("CR");

                    academicReviewModel.commonACCREV.AproveInt = academicReviewModel.approvedStatus == 1 ? true : false;
                    if (academicReviewModel.ChkAll.Count > 0)
                    {
                        success = updateCommon.getCheckedCheckListMul(academicReviewModel.ChkAll);
                        //if (success == true)
                        //{
                            if (academicReviewModel.commonACCREV.AproveInt == false)
                            {
                                int Qid = getcheck.getQueueId("CR");
                                var LetterName = objData.LetterEngines.Where(x => x.QueueId == Qid && x.ApproveStatus == false && x.SchoolId == session.SchoolId && x.LetterType == "Letter").ToList();
                                if (LetterName.Count > 0)
                                    getLetterTray.insertLetter("CR", session.ReferralId, LetterName[0].LetterEngineId, "Parent");
                                LetterGeneration.LetterLists = getLetterTray.getLetterList("");
                                var studentPer = objData.StudentPersonals.Where(x => x.StudentPersonalId == session.ReferralId).SingleOrDefault();
                                studentPer.InactiveList = true;
                                objData.SaveChanges();
                            }
                            academicReviewModel.commonACCREV.Draft = "N";
                            academicReviewModel.commonACCREV = getcheck.UpdateOrInsertRevCmt("CR", academicReviewModel.commonACCREV.academicReviewId, academicReviewModel.commonACCREV.Comments, academicReviewModel.commonACCREV.Draft, academicReviewModel.commonACCREV.IsPresent, academicReviewModel.commonACCREV.AproveInt, "RT");
                            academicReviewModel.ChkAll = updateCommon.UpdateOrInsertMulCheckList(academicReviewModel.ChkAll, academicReviewModel.commonACCREV.academicReviewId, "CR", academicReviewModel.commonACCREV.QstatusId, "Check");
                            //academicReviewModel.engineLetter = getcheck.UpdateOrInsertCheckList(academicReviewModel.engineLetter, academicReviewModel.commonACCREV.academicReviewId, "CR", academicReviewModel.commonACCREV.QstatusId, "Check");
                            if (academicReviewModel.commonACCREV.AproveInt == true)
                                returnVal = "success*" + session.ReferralId + "_" + qId;
                            else
                                returnVal = "success*" + session.ReferralId + "_" + QidInactive;
                        //}
                        //else
                        //{
                        //    academicReviewModel.commonACCREV.Draft = "Y";
                        //    academicReviewModel.commonACCREV = getcheck.UpdateOrInsertRevCmt("CR", academicReviewModel.commonACCREV.academicReviewId, academicReviewModel.commonACCREV.Comments, academicReviewModel.commonACCREV.Draft, academicReviewModel.commonACCREV.IsPresent, academicReviewModel.commonACCREV.AproveInt, "CR");
                        //    academicReviewModel.ChkAll = updateCommon.UpdateOrInsertMulCheckList(academicReviewModel.ChkAll, academicReviewModel.commonACCREV.academicReviewId, "CR", academicReviewModel.commonACCREV.QstatusId, "Check");
                        //    //academicReviewModel.engineLetter = getcheck.UpdateOrInsertCheckList(academicReviewModel.engineLetter, academicReviewModel.commonACCREV.academicReviewId, "CR", academicReviewModel.commonACCREV.QstatusId, "Check"); returnVal = clsGeneral.warningMsg("Data Successfully saved.Please assign all the checklists");
                        //    returnVal = clsGeneral.warningMsg("Checklist items are not completed so data submission is not possible.");
                        //}

                    }
                    else
                    {
                        academicReviewModel.commonACCREV.Draft = "N";
                        academicReviewModel.commonACCREV = getcheck.UpdateOrInsertRevCmt("CR", academicReviewModel.commonACCREV.academicReviewId, academicReviewModel.commonACCREV.Comments, academicReviewModel.commonACCREV.Draft, academicReviewModel.commonACCREV.IsPresent, academicReviewModel.commonACCREV.AproveInt, "CR");
                        academicReviewModel.ChkAll = updateCommon.UpdateOrInsertMulCheckList(academicReviewModel.ChkAll, academicReviewModel.commonACCREV.academicReviewId, "CR", academicReviewModel.commonACCREV.QstatusId, "Check");
                        if (academicReviewModel.commonACCREV.AproveInt == true)
                            returnVal = "success*" + session.ReferralId + "_" + qId;
                        else
                            returnVal = "success*" + session.ReferralId + "_" + QidInactive;
                        //    academicReviewModel.commonACCREV.Draft = "Y";
                        //    academicReviewModel.commonACCREV = getcheck.UpdateOrInsertRevCmt("CR", academicReviewModel.commonACCREV.academicReviewId, academicReviewModel.commonACCREV.Comments, academicReviewModel.commonACCREV.Draft, academicReviewModel.commonACCREV.IsPresent, academicReviewModel.commonACCREV.AproveInt, "CR");
                        //    academicReviewModel.ChkAll = updateCommon.UpdateOrInsertMulCheckList(academicReviewModel.ChkAll, academicReviewModel.commonACCREV.academicReviewId, "CR", academicReviewModel.commonACCREV.QstatusId, "Check");
                        //    //academicReviewModel.engineLetter = getcheck.UpdateOrInsertCheckList(academicReviewModel.engineLetter, academicReviewModel.commonACCREV.academicReviewId, "CR", academicReviewModel.commonACCREV.QstatusId, "Check"); returnVal = clsGeneral.warningMsg("Data Successfully saved.Please assign all the checklists");
                        //    returnVal = clsGeneral.warningMsg("Checklist items are not assigned.");

                    }

                    if (academicReviewModel.commonACCREV.AproveInt == false)
                    {

                    }

                    return Content(returnVal);


                }
                else
                {
                    returnVal = clsGeneral.failedMsg("Session not available");
                    return Content(returnVal);
                }

            }
            catch (Exception e)
            {
                returnVal = clsGeneral.failedMsg(e.Message);
                return Content(returnVal);
            }

        }



        //funding verification
        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        [HttpGet, ValidateInput(false)]
        public ActionResult FundingVerification(AcademicReviewModel academicReviewModel, string msg = "")
        {
            ViewBag.Chkmsg = msg;
            ViewBag.Flag = 0;
            session = (clsSession)Session["UserSession"];
            if (session != null)
            {
                ViewBag.permission = getcheck.setPermission();
                academicReviewModel.commonACCREV = getcheck.getRevCmt("FV");
                if (academicReviewModel.commonACCREV.academicReviewId != 0)
                {

                    academicReviewModel.approvedStatus = academicReviewModel.commonACCREV.AproveInt == true ? 1 : 2;
                }
                else
                    academicReviewModel.approvedStatus = 0;

                academicReviewModel.commonCallLog = getcheck.getRevMultipleCallLog("FV", academicReviewModel.commonACCREV.academicReviewId);
                if (academicReviewModel.commonCallLog.Count == 0)
                {
                    CommonCallLogViewModel defaultmodel = new CommonCallLogViewModel();
                    defaultmodel.StaffName = session.UserName;
                    academicReviewModel.commonCallLog.Add(defaultmodel);
                }
                //academicReviewModel.commonCallLog.CallTime = DateTime.ParseExact(academicReviewModel.commonCallLog.CallTimeShow, "MM/dd/yyyy", System.Globalization.CultureInfo.CurrentCulture);
                //academicReviewModel.commonCallLog.AppntTime = DateTime.ParseExact(academicReviewModel.commonCallLog.AppntTimeShow, "MM/dd/yyyy", System.Globalization.CultureInfo.CurrentCulture);
                academicReviewModel.DocList = GetDocList(session.ReferralId, session.SchoolId, "FV");
                academicReviewModel.ChkAll = getcheck.getCheckListMultiple("FV", "Check");

                return View(academicReviewModel);
            }
            else
                return View(academicReviewModel);
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


        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        [HttpPost, ValidateInput(false)]
        [MultiButton(MatchFormKey = "ActionFunding", MatchFormValue = "Save")]
        public ActionResult FundingVerification(AcademicReviewModel academicReviewModel)
        {
            ViewBag.Chkmsg = "";
            ViewBag.Flag = 1;
            //var checklists = getcheck.getCheckList("FV");
            try
            {
                MelmarkDBEntities objData = new MelmarkDBEntities();
                ClsCommon updateCommon = new ClsCommon();
                session = (clsSession)Session["UserSession"];
                if (session != null)
                {
                    foreach (var item in academicReviewModel.commonCallLog)
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
                    academicReviewModel.commonACCREV.AproveInt = academicReviewModel.approvedStatus == 1 ? true : false;
                    academicReviewModel.commonACCREV.Draft = "Y";
                    // academicReviewModel.commonCallLog.academicReviewId = academicReviewModel.commonACCREV.academicReviewId;

                    academicReviewModel.commonACCREV = getcheck.UpdateOrInsertRevCmt("FV", academicReviewModel.commonACCREV.academicReviewId, academicReviewModel.commonACCREV.Comments, academicReviewModel.commonACCREV.Draft, academicReviewModel.commonACCREV.IsPresent, academicReviewModel.commonACCREV.AproveInt, "FV");
                    academicReviewModel.ChkAll = updateCommon.UpdateOrInsertMulCheckList(academicReviewModel.ChkAll, academicReviewModel.commonACCREV.academicReviewId, "FV", academicReviewModel.commonACCREV.QstatusId, "Check");
                    //academicReviewModel.engineLetter = getcheck.UpdateOrInsertCheckList(academicReviewModel.engineLetter, academicReviewModel.commonACCREV.academicReviewId, "FV", academicReviewModel.commonACCREV.QstatusId, "Check");
                    //academicReviewModel.commonCallLog = getcheck.UpdateOrInsertCallLog("FV", academicReviewModel.commonACCREV.academicReviewId, academicReviewModel.commonCallLog.AppntTime, academicReviewModel.commonACCREV.Draft, academicReviewModel.commonCallLog.CallTime, academicReviewModel.commonCallLog.CallLogId, academicReviewModel.commonCallLog.Conversation, academicReviewModel.commonCallLog.NameOfContact, academicReviewModel.commonCallLog.StaffName, academicReviewModel.commonCallLog.IsPresent, academicReviewModel.commonACCREV.QstatusId);
                    academicReviewModel.commonCallLog = getcheck.UpdateOrInsertMultipleCallLog("FV", academicReviewModel.commonACCREV.academicReviewId, "Y", academicReviewModel.commonCallLog, academicReviewModel.commonACCREV.QstatusId);
                    //academicReviewModel.DocList = GetDocList(session.ReferralId, session.SchoolId);
                    if (academicReviewModel.DocList != null)
                    {
                        foreach (var item in academicReviewModel.DocList)
                        {
                            // Document tblDoc = new Document();
                            var tblDoc = objData.binaryFiles.Where(obj => obj.SchoolId == session.SchoolId && obj.BinaryId == item.IEPId && obj.StudentId == session.ReferralId).ToList();
                            tblDoc[0].Varified = item.Verified;
                            objData.SaveChanges();
                        }
                    }
                    string returnVal = clsGeneral.sucessMsg("Data Successfully Saved");
                    ViewBag.Chkmsg = returnVal;
                    return View(academicReviewModel);
                }
                else
                {
                    string returnVal = clsGeneral.failedMsg("Session not available");
                    ViewBag.Chkmsg = returnVal;
                    return View(academicReviewModel);
                }

            }
            catch (Exception e)
            {
                string returnVal = clsGeneral.failedMsg(e.Message);
                ViewBag.Chkmsg = returnVal;
                return View(academicReviewModel);
            }
        }


        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        [HttpPost, ValidateInput(false)]
        [MultiButton(MatchFormKey = "ActionFunding", MatchFormValue = "Submit")]
        public ActionResult FundingVerification(AcademicReviewModel academicReviewModel, string b, string a)
        {
            ViewBag.Flag = 0;
            string returnVal = "";
            bool success = false;
            try
            {
                MelmarkDBEntities objData = new MelmarkDBEntities();
                ClsCommon updateCommon = new ClsCommon();
                session = (clsSession)Session["UserSession"];
                if (session != null)
                {
                    int qId = updateCommon.getQueueId("FV");
                    //returnVal = updateCommon.IsSubmit(qId);

                    //if (returnVal == "")
                    //{

                    academicReviewModel.commonACCREV.AproveInt = academicReviewModel.approvedStatus == 1 ? true : false;
                    foreach (var item in academicReviewModel.commonCallLog)
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


                    if (academicReviewModel.DocList != null)
                    {
                        foreach (var item in academicReviewModel.DocList)
                        {
                            // Document tblDoc = new Document();
                            var tblDoc = objData.binaryFiles.Where(obj => obj.SchoolId == session.SchoolId && obj.BinaryId == item.IEPId && obj.StudentId == session.ReferralId).ToList();
                            tblDoc[0].Varified = item.Verified;
                            objData.SaveChanges();
                        }
                    }
                    //academicReviewModel.commonCallLog.academicReviewId = academicReviewModel.commonACCREV.academicReviewId;
                    if (academicReviewModel.ChkAll.Count > 0)
                    {
                        success = updateCommon.getCheckedCheckListMul(academicReviewModel.ChkAll);

                        //if (success == true)
                        //{
                            //if (academicReviewModel.commonACCREV.AproveInt == true)
                            //{
                            int Queueid = getcheck.getQueueId("FV");
                            var LetterName = objData.LetterEngines.Where(x => x.QueueId == Queueid && x.ApproveStatus == academicReviewModel.commonACCREV.AproveInt && x.SchoolId == session.SchoolId && x.LetterType == "Letter").ToList();
                            if (LetterName.Count > 0)
                                getLetterTray.insertLetter("FV", session.ReferralId, LetterName[0].LetterEngineId, "Parent");
                            LetterGeneration.LetterLists = getLetterTray.getLetterList("");

                            //}
                            string type = "";
                            academicReviewModel.commonACCREV.Draft = "N";
                            if (academicReviewModel.commonACCREV.AproveInt == true)
                            {
                                type = "AV";

                                var studentPer = objData.StudentPersonals.Where(x => x.StudentPersonalId == session.ReferralId).SingleOrDefault();
                                studentPer.FundingVerification = true;
                                objData.SaveChanges();
                            }
                            else
                            {
                                type = "WL";

                                var studentPer = objData.StudentPersonals.Where(x => x.StudentPersonalId == session.ReferralId).SingleOrDefault();
                                studentPer.WaitingList = true;
                                studentPer.FundingVerification = false;
                                objData.SaveChanges();
                            }
                            int Qid = getcheck.getQueueId(type);
                            academicReviewModel.commonACCREV = getcheck.UpdateOrInsertRevCmt("FV", academicReviewModel.commonACCREV.academicReviewId, academicReviewModel.commonACCREV.Comments, academicReviewModel.commonACCREV.Draft, academicReviewModel.commonACCREV.IsPresent, academicReviewModel.commonACCREV.AproveInt, type);
                            academicReviewModel.ChkAll = updateCommon.UpdateOrInsertMulCheckList(academicReviewModel.ChkAll, academicReviewModel.commonACCREV.academicReviewId, "FV", academicReviewModel.commonACCREV.QstatusId, "Check");
                            //academicReviewModel.engineLetter = getcheck.UpdateOrInsertCheckList(academicReviewModel.engineLetter, academicReviewModel.commonACCREV.academicReviewId, "FV", academicReviewModel.commonACCREV.QstatusId, "Check");
                            //academicReviewModel.commonCallLog = getcheck.UpdateOrInsertCallLog("FV", academicReviewModel.commonACCREV.academicReviewId, academicReviewModel.commonCallLog.AppntTime, academicReviewModel.commonACCREV.Draft, academicReviewModel.commonCallLog.CallTime, academicReviewModel.commonCallLog.CallLogId, academicReviewModel.commonCallLog.Conversation, academicReviewModel.commonCallLog.NameOfContact, academicReviewModel.commonCallLog.StaffName, academicReviewModel.commonCallLog.IsPresent, academicReviewModel.commonACCREV.QstatusId);
                            academicReviewModel.commonCallLog = getcheck.UpdateOrInsertMultipleCallLog("FV", academicReviewModel.commonACCREV.academicReviewId, academicReviewModel.commonACCREV.Draft, academicReviewModel.commonCallLog, academicReviewModel.commonACCREV.QstatusId);
                            return Content("success*" + session.ReferralId + "_" + Queueid);
                        //}
                        //else
                        //{
                        //    academicReviewModel.commonACCREV.Draft = "Y";
                        //    academicReviewModel.commonACCREV = getcheck.UpdateOrInsertRevCmt("FV", academicReviewModel.commonACCREV.academicReviewId, academicReviewModel.commonACCREV.Comments, academicReviewModel.commonACCREV.Draft, academicReviewModel.commonACCREV.IsPresent, academicReviewModel.commonACCREV.AproveInt, "FV");
                        //    academicReviewModel.ChkAll = updateCommon.UpdateOrInsertMulCheckList(academicReviewModel.ChkAll, academicReviewModel.commonACCREV.academicReviewId, "FV", academicReviewModel.commonACCREV.QstatusId, "Check");
                        //    //academicReviewModel.engineLetter = getcheck.UpdateOrInsertCheckList(academicReviewModel.engineLetter, academicReviewModel.commonACCREV.academicReviewId, "FV", academicReviewModel.commonACCREV.QstatusId, "Check");
                        //    //academicReviewModel.commonCallLog = getcheck.UpdateOrInsertCallLog("FV", academicReviewModel.commonACCREV.academicReviewId, academicReviewModel.commonCallLog.AppntTime, academicReviewModel.commonACCREV.Draft, academicReviewModel.commonCallLog.CallTime, academicReviewModel.commonCallLog.CallLogId, academicReviewModel.commonCallLog.Conversation, academicReviewModel.commonCallLog.NameOfContact, academicReviewModel.commonCallLog.StaffName, academicReviewModel.commonCallLog.IsPresent, academicReviewModel.commonACCREV.QstatusId);
                        //    academicReviewModel.commonCallLog = getcheck.UpdateOrInsertMultipleCallLog("FV", academicReviewModel.commonACCREV.academicReviewId, academicReviewModel.commonACCREV.Draft, academicReviewModel.commonCallLog, academicReviewModel.commonACCREV.QstatusId);
                        //    returnVal = clsGeneral.warningMsg("Checklist items are not completed so data submission is not possible.");
                        //}
                    }
                    else
                    {
                        string type = "";
                        academicReviewModel.commonACCREV.Draft = "N";
                        if (academicReviewModel.commonACCREV.AproveInt == true)
                        {
                            type = "AV";

                            var studentPer = objData.StudentPersonals.Where(x => x.StudentPersonalId == session.ReferralId).SingleOrDefault();
                            studentPer.FundingVerification = true;
                            objData.SaveChanges();
                        }
                        else
                        {
                            type = "WL";

                            var studentPer = objData.StudentPersonals.Where(x => x.StudentPersonalId == session.ReferralId).SingleOrDefault();
                            studentPer.WaitingList = true;
                            studentPer.FundingVerification = false;
                            objData.SaveChanges();
                        }
                        int Qid = getcheck.getQueueId(type);
                        int Queueid = getcheck.getQueueId("FV");
                        academicReviewModel.commonACCREV = getcheck.UpdateOrInsertRevCmt("FV", academicReviewModel.commonACCREV.academicReviewId, academicReviewModel.commonACCREV.Comments, academicReviewModel.commonACCREV.Draft, academicReviewModel.commonACCREV.IsPresent, academicReviewModel.commonACCREV.AproveInt, type);
                        academicReviewModel.ChkAll = updateCommon.UpdateOrInsertMulCheckList(academicReviewModel.ChkAll, academicReviewModel.commonACCREV.academicReviewId, "FV", academicReviewModel.commonACCREV.QstatusId, "Check");
                        academicReviewModel.commonCallLog = getcheck.UpdateOrInsertMultipleCallLog("FV", academicReviewModel.commonACCREV.academicReviewId, academicReviewModel.commonACCREV.Draft, academicReviewModel.commonCallLog, academicReviewModel.commonACCREV.QstatusId);
                        return Content("success*" + session.ReferralId + "_" + Queueid); 
                        

                    }
                    //academicReviewModel.DocList = GetDocList(session.ReferralId, session.SchoolId);


                    ViewBag.Chkmsg = returnVal;
                    return View("FundingVerification", academicReviewModel);

                    // }
                    //else
                    //{
                    //    ViewBag.Chkmsg = returnVal;
                    //    return View("FundingVerification", academicReviewModel);
                    //}
                }
                else
                {

                    returnVal = clsGeneral.failedMsg("Session not available");
                    ViewBag.Chkmsg = returnVal;
                    return View("FundingVerification", academicReviewModel);
                }

            }
            catch (Exception e)
            {
                returnVal = clsGeneral.failedMsg(e.Message);
                ViewBag.Chkmsg = returnVal;
                return View("FundingVerification", academicReviewModel);
            }
        }

        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public string RemoveCallLog(int CallLogId)
        {
            string Result = "";
            try
            {
                objData = new MelmarkDBEntities();
                var DeleteCallLog = objData.ref_CallLogs.Where(x => x.CallLogId == CallLogId).First();
                objData.ref_CallLogs.Remove(DeleteCallLog);
                objData.SaveChanges();
                Result = "Success";
            }
            catch (Exception Ex)
            {
                Result = "Failed";
            }
            return Result;
        }


        //match opening
        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult MatchOpening(int id=0)
        {
            string school = WebConfigurationManager.AppSettings["Server"];
            ClsCommon objFuns = new ClsCommon();
            session = (clsSession)Session["UserSession"];
            if (session != null)
            {
                ViewBag.school = school;
                ViewBag.permission = objFuns.setPermission();
            }
            AddMatchOpeningViewModel model = new AddMatchOpeningViewModel();

            model.PlacementTypeList = objFuns.getType("Placement Type");
            model.PrimaryNurseList = objFuns.getUserType("PRIMARY NURSE");
            model.UnitClerkList = objFuns.getUserType("UNIT CLERK");
            model.BehaviorAnalystList = objFuns.getUserType("BEHAVIOR ANALYST");
            model.DepartmentList = objFuns.getType("Department");            

            model.PlacementDepartmentList = model.GetPlacementDepartment(session.SchoolId);
            model.PlacementReasonList = model.GetPlacementReason(session.SchoolId);
            model.LocationList = model.GetLocationList();
            model.listPlacement = model.fillPlacement(1, 5);
            try
            {

                model.FundingSourceList = objFuns.getType("fundingsource"); //--- 22Sep2020 - List 3 - Task #2 ---//
                model.FundingSourceList = model.GetFundingList(session.SchoolId); //--- 22Sep2020 - List 3 - Task #2 ---//            
                model = objFuns.bindPlacementFundid(id, model); //--- 02Oct2020 - List 3 - Task #2 ---//
            }
            catch (Exception ex)
            {
                ClsErrorLog erorLog = new ClsErrorLog();
                erorLog.WriteToLog(ex.ToString());
            }
            if (id > 0)
            {
                model = objFuns.bindPlacement(id, model);
            }
            else
            {
                model.iSSubmitted = getcheck.checkIfSubmitted(session.ReferralId, "MO");
            }
            return View(model);

        }


        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        [HttpPost, ValidateInput(false)]
        [MultiButton(MatchFormKey = "ActionMatchOpening", MatchFormValue = "Submit")]        
        public ActionResult MatchOpeningSave(AddMatchOpeningViewModel matchOpening,string a="")
        {
            MelmarkDBEntities objData = new MelmarkDBEntities();
            try
            {
                session = (clsSession)Session["UserSession"];
                if (session != null)
                {
                    string result = "";
                    ClsCommon objFuns = new ClsCommon();

                    //result = objFuns.SavePlacementData(matchOpening);
                    string returnVal = "";
                    int qId = getcheck.getQueueId("MO");
                   // returnVal = getcheck.IsSubmit(qId);

                    //if (returnVal == "")
                    //{
                    //Adding or updating row in QStatus Table With CurrentId QID
                    ref_QueueStatus qStatus = new ref_QueueStatus();
                    qId = getcheck.getQueueId("MO");
                    int qIdNext = getcheck.getQueueId("PT");
                    int queueStatusId = 0;
                    int qProcess = getcheck.getProcessId();

                    // getcheck.insertNewStatus("MO", "PT", session.ReferralId);

                    queueStatusId = getcheck.getQueueStatusIdCurrent(qId, qProcess, session.ReferralId);

                    //update or insert in MatchOpening table
                    ref_MatchOpening refMatchOpening = new ref_MatchOpening();
                   // refMatchOpening.Comments = matchOpening.comments;
                    refMatchOpening.CreatedBy = session.LoginId;
                    refMatchOpening.CreatedOn = System.DateTime.Now;
                    refMatchOpening.DepartmentId = matchOpening.PlacementDepartmentId;
                    refMatchOpening.Draft = "N";
                    refMatchOpening.Program = matchOpening.PlacementType;
                    refMatchOpening.SchoolId = session.SchoolId;
                    refMatchOpening.StudentId = session.ReferralId;
                    refMatchOpening.QueueStatusId = queueStatusId;
                    var matchOpenlist = objData.ref_MatchOpening.Where(x => x.StudentId == session.ReferralId).ToList();
                    if (matchOpenlist.Count == 0)
                    {
                        objData.ref_MatchOpening.Add(refMatchOpening);
                        objData.SaveChanges();
                    }
                    else
                    {
                        matchOpenlist[0].DepartmentId = matchOpening.PlacementDepartmentId;
                        matchOpenlist[0].Program = matchOpening.PlacementType;
                        matchOpenlist[0].ModifiedBy = session.LoginId;
                        matchOpenlist[0].ModifiedOn = System.DateTime.Now;
                        objData.SaveChanges();
                    }
                    ref_QueueStatus refqueue = objData.ref_QueueStatus.Where(x => x.StudentPersonalId == session.ReferralId && x.QueueId == qId).SingleOrDefault();
                    StudentPersonal studper = objData.StudentPersonals.Where(x => x.StudentPersonalId == session.ReferralId).SingleOrDefault();
                    if (refqueue != null)
                    {
                        refqueue.Draft = "N";
                        objData.SaveChanges();
                    }
                    if (studper != null)
                    {
                        studper.PlacementStatus  = "A";
                        objData.SaveChanges();
                    }
                    var qStatusLatest = objData.ref_QueueStatus.Where(x => x.StudentPersonalId == session.ReferralId && x.CurrentStatus == true && x.QueueProcess == qProcess).ToList();
                    session.CurrentProcessId = qStatusLatest[0].QueueId;
                    matchOpening.iSSubmitted = getcheck.checkIfSubmitted(session.ReferralId, "MO");                    

                    //insert Letter
                    int QUeueid = getcheck.getQueueId("MO");
                    var LetterName = objData.LetterEngines.Where(x => x.QueueId == QUeueid && x.SchoolId == session.SchoolId && x.LetterType == "Letter").ToList();
                    if (LetterName.Count > 0)
                        getLetterTray.insertLetter("MO", session.ReferralId, LetterName[0].LetterEngineId, "Parent");
                    LetterGeneration.LetterLists = getLetterTray.getLetterList("");

                    returnVal = "success*" + session.ReferralId + "_" + QUeueid;
                    //string returnVal = clsGeneral.sucessMsg("Data Successfully Saved");
                    return Content(returnVal);
                    //}
                    //else
                    //{
                    //    return Content(returnVal);
                    //}
                }
                else
                {
                    string returnVal = clsGeneral.failedMsg("Session not available");
                    return Content(returnVal);
                }

            }
            catch (Exception e)
            {
                string returnVal = clsGeneral.failedMsg(e.Message);
                return Content(returnVal);
            }
        }

        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        [HttpPost, ValidateInput(false)]
        [MultiButton(MatchFormKey = "ActionMatchOpening", MatchFormValue = "Save")]
        public ActionResult MatchOpeningSave(AddMatchOpeningViewModel model, int page = 1, int pageSize = 5)
        {
            string result = "";
            ClsCommon objFuns = new ClsCommon();

            result = objFuns.SavePlacementData(model);

            if (result == "No Client Selected")
            {
                TempData["notice"] = "No Client Selected";
            }
            model = new AddMatchOpeningViewModel();
            model.listPlacement = model.fillPlacement(page,pageSize);
            return RedirectToAction("MatchOpening");

        }

        

        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        [HttpPost, ValidateInput(false)]
        [MultiButton(MatchFormKey = "ActionMatchOpening", MatchFormValue = "Update")]
        public ActionResult MatchOpeningUpdate(AddMatchOpeningViewModel model,int id)
        {
            model.Id = id;
            string result = "";
            session = (clsSession)Session["UserSession"];
            ClsCommon objFuns = new ClsCommon();
            result = objFuns.SavePlacementData(model);
            return RedirectToAction("MatchOpening");

        }

        public IEnumerable<SelectListItem> getType(string type)
        {
            MelmarkDBEntities objData = new MelmarkDBEntities();
            IList<LookUp> PlacementTypeData = new List<LookUp>();
            SelectListItem onesele = new SelectListItem { Text = "-- Select --", Value = "" };
            IList<SelectListItem> placementTypeSelecteditem = new List<SelectListItem>();
            placementTypeSelecteditem.Add(onesele);
            PlacementTypeData = objData.LookUps.Where(objLookUp => objLookUp.LookupType == type).ToList();
            var placementSelecteditemsub = (from placementType in PlacementTypeData select new SelectListItem { Text = placementType.LookupName, Value = placementType.LookupId.ToString() }).ToList();
            foreach (SelectListItem sele in placementSelecteditemsub)
            {
                placementTypeSelecteditem.Add(sele);
            }
            return placementTypeSelecteditem;
        }

        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        private IEnumerable<SelectListItem> GetDepartmentList()
        {
            MelmarkDBEntities objData = new MelmarkDBEntities();
            session = (clsSession)Session["UserSession"];
            if (session != null)
            {
                if (session.SchoolId == 1)
                {
                    var departments = objData.LookUps.Where(x => x.LookupType == "MatchOpening" && x.SchoolId == session.SchoolId).ToList();
                    return (from department in departments
                            select new SelectListItem
                            {
                                Text = department.LookupName,
                                Value = department.LookupId.ToString()
                            }).ToList();
                }
                else
                {
                    var departments = objData.LookUps.Where(x => x.LookupType == "Department" && x.SchoolId == session.SchoolId).ToList();
                    return (from department in departments
                            select new SelectListItem
                            {
                                Text = department.LookupName,
                                Value = department.LookupId.ToString()
                            }).ToList();
                }

            }
            else
                return null;
        }
        //LetterTray View

        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult LetterTray(string QType)
        {
            MelmarkDBEntities objData = new MelmarkDBEntities();
            AcademicReviewModel academicReviewModel = new AcademicReviewModel();

            ReferalDB.AppFunctions.Other_Functions OF = new ReferalDB.AppFunctions.Other_Functions();
            ViewBag.permission = OF.setPermission();

            int Qid = getcheck.getQueueId(QType);
            session = (clsSession)Session["UserSession"];
            if (session != null)
            {
                var engineLetterItems = (from x in objData.ref_LetterTrayAssign
                                         join y in objData.ref_LetterTrayValues on x.LetterTrayId equals y.LetterTrayId
                                         join z in objData.LetterEngines on y.LetterId equals z.LetterEngineId
                                         where (x.LetterUserId == session.LoginId && y.QueueId == Qid)
                                         select new LetterTrayViewModel
                                         {
                                             LetterItem = y.TrayValue,
                                             LetterName = z.LetterEngineName
                                         }).ToList();

                //var trayId=objData.ref_LetterTrayAssign.Single                
                academicReviewModel.LetterList = engineLetterItems;
                return View(academicReviewModel);
            }
            else
                return View(academicReviewModel);

        }


        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult LetterGenerationView(LetterGenerationViewModel LetterGeneration, string Name)
        {
            session = (clsSession)Session["UserSession"]; //---- List 3 - Task #30 [20-Oct-2020] ---//
            ClsCommon getCommon = new ClsCommon(); //---- List 3 - Task #30 [20-Oct-2020] ---//
            LetterGeneration = new LetterGenerationViewModel();
            LetterTray letterTray = new LetterTray();
            if (session != null) //---- List 3 - Task #30 [20-Oct-2020] - (Start) ---//
            {
                ViewBag.permission = getCommon.setPermission();
                LetterGeneration.QueueItems = FillDropQueueList();
            }//---- List 3 - Task #30 [20-Oct-2020] - (End) ---//

            ReferalDB.AppFunctions.Other_Functions OF = new ReferalDB.AppFunctions.Other_Functions();
            ViewBag.permission = OF.setPermission();

            if (Name != null && Name != "")
            {
                if (Name.EndsWith("_Search"))
                {
                    LetterGeneration.refFlag = "Search";
                }
                else if (Name == "_")
                {
                    LetterGeneration.refFlag = "No Search";
                }
                else
                    LetterGeneration.refFlag = "Search";
            }
            LetterGeneration.LetterLists = letterTray.getLetterList(Name);
            return View(LetterGeneration);
        }
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult LetterGenerationAll(LetterGenerationViewModel LetterGeneration)
        {
            LetterGeneration = new LetterGenerationViewModel();
            LetterTray letterTray = new LetterTray();
            LetterGeneration.refFlag = "Search";
            LetterGeneration.LetterLists = letterTray.getLetterAll();
            return View("LetterGenerationView", LetterGeneration);
        }

        //Letter Generation Page Save
        [HttpPost, ValidateInput(false)]
        [MultiButton(MatchFormKey = "ActionLetterGeneration", MatchFormValue = "Send")]
        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult LetterGenerationViewSave(LetterGenerationViewModel LetterGeneration)
        {
            MelmarkDBEntities objData = new MelmarkDBEntities();
            //var LetterTrayValues = objData.ref_LetterTrayValues.ToList();

            foreach (var LetterTray in LetterGeneration.LetterLists)
            {
                ref_LetterTrayValues LetterTrayValues = new ref_LetterTrayValues();
                if (LetterTray.status == true && LetterTray.SentOn == null)
                {
                    LetterTrayValues = objData.ref_LetterTrayValues.Single(x => x.LetterTrayId == LetterTray.LetterTrayId);
                    LetterTrayValues.Status = true;
                    LetterTrayValues.ModifiedOn = System.DateTime.Now;
                    LetterTrayValues.ModifiedBy = 1;
                    objData.SaveChanges();
                }
            }
            string returnVal = clsGeneral.sucessMsg("Letter Successfully Sent...");
            return Content(returnVal);
        }

        //---- List 3 - Task #30 [20-Oct-2020] - (Start) ---//
        //Letter Save to Tray
        [HttpPost, ValidateInput(false)]
        [MultiButton(MatchFormKey = "LetterSaveToTray", MatchFormValue = "Add")]
        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult LetterGenerationViewSave(LetterGenerationViewModel LetterGeneration2, int pageid = 3)
        {
            MelmarkDBEntities objData = new MelmarkDBEntities();

            if (LetterGeneration2.QueueTypeId != null)
            {
                int Getid = Convert.ToInt32(LetterGeneration2.QueueTypeId);
                AddLettertoTray(Getid);
            }
            string returnVal = clsGeneral.sucessMsg("Letter Successfully Added to Tray...");
            return Content(returnVal);
        }
        //---- List 3 - Task #30 [20-Oct-2020] - (End) ---//

        //Letter View
        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult LetterTrayView(int LetterTrayId)
        {
            MelmarkDBEntities objData = new MelmarkDBEntities();
            AcademicReviewModel academicReviewModel = new AcademicReviewModel();
            //session = (clsSession)Session["UserSession"];
            //if (session != null)
            //{
            var LetterItems = (from x in objData.ref_LetterTrayValues
                               join z in objData.LetterEngines on x.LetterId equals z.LetterEngineId
                               //join y in objData.LetterEngineItems on x.LetterId equals y.LetterEngineItemId
                               //join LetterEngin in objData.LetterEngines on y.LetterEngineId equals LetterEngin.LetterEngineId
                               where (x.LetterTrayId == LetterTrayId)
                               select new LetterTrayViewModel
                               {
                                   LetterItem = x.TrayValue,
                                   LetterName = z.LetterEngineName

                               }).ToList();

            //var trayId=objData.ref_LetterTrayAssign.Single

            academicReviewModel.LetterList = LetterItems;
            return View(academicReviewModel);
            //}
            //else
            //    return View(academicReviewModel);

        }


        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult AssignTeam(TeamAssignViewModel TeamAssign = null)
        {
            session = (clsSession)Session["UserSession"];
            MelmarkDBEntities objData = new MelmarkDBEntities();
            if (session != null)
            {
                ViewBag.permission = getcheck.setPermission();
                //var team=objData.ReviewTeams.Where(x=>x.ActiveInd=="A").ToList();
                ClsCommon cmnClass = new ClsCommon();
                QstatusDetails qDetails = cmnClass.getQueueStatusId(session.ReferralId, "RT");
                bool isSubmit = false;
                int QstatusId = qDetails.QueueStatusId;
                if (QstatusId > 0)
                {
                    int QstatusIds = cmnClass.getQueueStatusIdIfSubmitted(session.ReferralId, "RT");
                    if (QstatusIds > 0)
                    {
                        isSubmit = true;

                    }

                }



                TeamAssign.TeamUsers =
                    (from x in objData.ReviewTeams
                     where (x.ActiveInd == "A")
                     select new ReferalDB.Models.TeamAssignViewModel.teamUserDetails
                     {
                         TeamName = x.TeamName,
                         TeamId = x.TeamId
                     }).ToList();

                if (TeamAssign.TeamUsers.Count > 0)
                {
                    foreach (var tm in TeamAssign.TeamUsers)
                    {

                        var val = (from x in objData.TeamMembers
                                   join y in objData.Users on x.UserId equals y.UserId
                                   where (x.TeamId == tm.TeamId)
                                   select new ReferalDB.Models.TeamAssignViewModel.teamUserDetails
                                   {

                                       UserNames = y.UserLName + "," + y.UserFName

                                   }).ToList();
                        if (val.Count > 0)
                        {
                            foreach (var tVal in val)
                            {
                                tm.UserNames += tVal.UserNames + ";";
                            }

                        }
                        if (QstatusId > 0)
                        {
                            var TeamAssignd = objData.ref_TeamAssign.Where(x => x.QueueStatusId == QstatusId).ToList();
                            if (TeamAssignd.Count > 0)
                            {
                                foreach (var tmAsgnd in TeamAssignd)
                                {
                                    if (tmAsgnd.TeamId == tm.TeamId)
                                    {
                                        tm.TeamAssignId = tmAsgnd.TeamAssignId;
                                        if (tmAsgnd.Complete == true)
                                        {
                                            tm.Complete = true;
                                        }
                                        else
                                        {
                                            tm.Complete = false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                TeamAssign.iSSubmitted = isSubmit;
                return View("TeamAssign", TeamAssign);
            }
            else
                return View("TeamAssign", TeamAssign);
        }

        [HttpPost, ValidateInput(false)]
        [MultiButton(MatchFormKey = "SaveTeamAssign", MatchFormValue = "Submit")]
        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult AssignTeamsubmit(TeamAssignViewModel Model = null)
        {
            string returnVal = "";
            int count = 0;
            session = (clsSession)Session["UserSession"];
            ClsCommon cmnClass = new ClsCommon();
            MelmarkDBEntities objData = new MelmarkDBEntities();
            if (session != null)
            {
                bool IsSucess = false;
                foreach (var val in Model.TeamUsers)
                {
                    if (val.checkListval == "True")
                    {
                        IsSucess = true;
                        break;
                    }
                }
                if (IsSucess == false)
                {
                    returnVal = clsGeneral.failedMsg("At least one team required");
                    return Content(returnVal);
                }
                int qId = cmnClass.getQueueId("RT");
                //returnVal = cmnClass.IsSubmit(qId);

                QstatusDetails qDetails = cmnClass.getQueueStatusId(session.ReferralId, "RT");
                bool isSubmit = false;
                int QstatusId = qDetails.QueueStatusId;
                if (QstatusId == 0)
                {
                    QstatusId = cmnClass.getQueueStatusIdIfSubmitted(session.ReferralId, "RT");
                    if (QstatusId > 0)
                    {
                        isSubmit = true;
                    }
                    else
                    {
                        QstatusId = cmnClass.insertQstatus("RT", "N");
                    }
                }


                foreach (var val in Model.TeamUsers)
                {
                    if (QstatusId > 0)
                    {
                        var TeamAssignd = objData.ref_TeamAssign.Where(x => x.QueueStatusId == QstatusId).ToList();
                        if (TeamAssignd.Count > 0)
                        {
                            foreach (var tmAsgnd in TeamAssignd)
                            {
                                if (tmAsgnd.TeamId == val.TeamId)
                                {
                                    val.TeamAssignId = tmAsgnd.TeamAssignId;

                                }
                            }
                        }
                    }
                    var isPresentVal = objData.ref_TeamAssign.Where(x => x.QueueStatusId == QstatusId && x.TeamAssignId == val.TeamAssignId).ToList();
                    if (isPresentVal.Count > 0)
                    {
                        val.IsPresent = true;
                        val.TeamAssignId = isPresentVal[0].TeamAssignId;
                    }
                }

                foreach (var item in Model.TeamUsers)
                {
                    if (item.checkListval == "True" || item.checkListval == "true")
                    {
                        count++;
                    }
                }
                if (count > 0)
                {
                    foreach (var val in Model.TeamUsers)
                    {
                        if (val.IsPresent == true)
                        {
                            var Updaterow = objData.ref_TeamAssign.Where(x => x.TeamAssignId == val.TeamAssignId).ToList();
                            if (Updaterow.Count > 0)
                            {
                                Updaterow[0].ModifiedOn = System.DateTime.Now;
                                Updaterow[0].ModifiedBy = session.LoginId;
                                if (val.checkListval == "True" || val.checkListval == "true")
                                {
                                    Updaterow[0].Complete = true;
                                }
                                else
                                {
                                    Updaterow[0].Complete = false;
                                }
                                objData.SaveChanges();
                            }

                        }
                        else
                        {
                            ref_TeamAssign newTeamAssign = new ref_TeamAssign();
                            newTeamAssign.QueueStatusId = QstatusId;
                            newTeamAssign.CreatedOn = System.DateTime.Now;
                            newTeamAssign.CreatedBy = session.LoginId;
                            newTeamAssign.TeamId = val.TeamId;
                            newTeamAssign.ReferalId = session.ReferralId;
                            ClsCommon getCmn = new ClsCommon();
                            newTeamAssign.ProcessId = getCmn.getProcessId();

                            if (val.checkListval == "True" || val.checkListval == "true")
                            {
                                newTeamAssign.Complete = true;
                            }
                            else
                            {
                                newTeamAssign.Complete = false;
                            }
                            objData.ref_TeamAssign.Add(newTeamAssign);
                            objData.SaveChanges();
                        }
                    }
                    cmnClass.insertNewStatus("RT", "FV", session.ReferralId);
                    Model.iSSubmitted = isSubmit;

                    //insert Letter
                    int QUeueid = getcheck.getQueueId("RT");
                    var LetterName = objData.LetterEngines.Where(x => x.QueueId == QUeueid && x.SchoolId == session.SchoolId && x.LetterType == "Letter").ToList();
                    if (LetterName.Count > 0)
                        getLetterTray.insertLetter("RT", session.ReferralId, LetterName[0].LetterEngineId, "Parent");
                    LetterGeneration.LetterLists = getLetterTray.getLetterList("");

                    int inQnxtId = cmnClass.getQueueId("FV");
                    returnVal = "success*" + session.ReferralId + '_' + QUeueid;
                }
                else
                {
                    returnVal = clsGeneral.failedMsg("Please assign atleast one team..");
                }

            }
            else
            {

                returnVal = clsGeneral.failedMsg("Session not available");
            }
            return Content(returnVal);
        }

        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult DeletePlacementDetails(int id)
        {
            session = (clsSession)Session["UserSession"];
            Other_Functions objFuns = new Other_Functions();
            objFuns.deletePlacement(session.ReferralId, id);

            return RedirectToAction("MatchOpening");

        }



        [HttpPost, ValidateInput(false)]
        [MultiButton(MatchFormKey = "SaveTeamAssign", MatchFormValue = "Save")]
        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult AssignTeamSave(string a, TeamAssignViewModel Model = null)
        {
            string returnVal = "";
            session = (clsSession)Session["UserSession"];
            ClsCommon cmnClass = new ClsCommon();
            MelmarkDBEntities objData = new MelmarkDBEntities();
            if (session != null)
            {
                QstatusDetails qDetails = cmnClass.getQueueStatusId(session.ReferralId, "RT");
                bool isSubmit = false;
                int QstatusId = qDetails.QueueStatusId;
                if (QstatusId > 0)
                {
                    int QstatusIds = cmnClass.getQueueStatusIdIfSubmitted(session.ReferralId, "RT");
                    if (QstatusIds > 0)
                    {
                        isSubmit = true;
                    }
                    else
                    {
                        QstatusId = cmnClass.insertQstatus("RT", "Y");
                    }
                }


                foreach (var val in Model.TeamUsers)
                {
                    if (QstatusId > 0)
                    {
                        var TeamAssignd = objData.ref_TeamAssign.Where(x => x.QueueStatusId == QstatusId).ToList();
                        if (TeamAssignd.Count > 0)
                        {
                            foreach (var tmAsgnd in TeamAssignd)
                            {
                                if (tmAsgnd.TeamId == val.TeamId)
                                {
                                    val.TeamAssignId = tmAsgnd.TeamAssignId;

                                }
                            }
                        }
                    }
                    var isPresentVal = objData.ref_TeamAssign.Where(x => x.QueueStatusId == QstatusId && x.TeamAssignId == val.TeamAssignId).ToList();
                    if (isPresentVal.Count > 0)
                    {
                        val.IsPresent = true;
                        val.TeamAssignId = isPresentVal[0].TeamAssignId;
                    }
                }

                foreach (var val in Model.TeamUsers)
                {
                    if (val.IsPresent == true)
                    {
                        var Updaterow = objData.ref_TeamAssign.Where(x => x.TeamAssignId == val.TeamAssignId).ToList();
                        if (Updaterow.Count > 0)
                        {
                            Updaterow[0].ModifiedOn = System.DateTime.Now;
                            Updaterow[0].ModifiedBy = session.LoginId;
                            Updaterow[0].ReferalId = session.ReferralId;
                            ClsCommon getCmn = new ClsCommon();
                            Updaterow[0].ProcessId = getCmn.getProcessId();
                            if (val.checkListval == "True" || val.checkListval == "true")
                            {
                                Updaterow[0].Complete = true;
                            }
                            else
                            {
                                Updaterow[0].Complete = false;
                            }
                            objData.SaveChanges();
                        }

                    }
                    else
                    {

                        ref_TeamAssign newTeamAssign = new ref_TeamAssign();
                        newTeamAssign.QueueStatusId = QstatusId;
                        newTeamAssign.CreatedOn = System.DateTime.Now;
                        newTeamAssign.CreatedBy = session.LoginId;
                        newTeamAssign.TeamId = val.TeamId;
                        newTeamAssign.ReferalId = session.ReferralId;
                        ClsCommon getCmn = new ClsCommon();
                        newTeamAssign.ProcessId = getCmn.getProcessId();
                        if (val.checkListval == "True" || val.checkListval == "true")
                        {
                            newTeamAssign.Complete = true;
                        }
                        else
                        {
                            newTeamAssign.Complete = false;
                        }
                        objData.ref_TeamAssign.Add(newTeamAssign);
                        objData.SaveChanges();
                        val.TeamAssignId = newTeamAssign.TeamAssignId;
                    }
                }
                Model.iSSubmitted = isSubmit;
                returnVal = clsGeneral.sucessMsg("Data Successfully Saved");
            }
            else
            {

                returnVal = clsGeneral.failedMsg("Session not available");
            }
            return Content(returnVal);
        }

        public int FileUpload(int StudentId, int SchoolId, string DocName, string DocPath, int UserId, string queryType)
        {
            int rtrnval = -1;
            MelmarkDBEntities objData = new MelmarkDBEntities();
            QstatusDetails qDetails = getcheck.getQueueStatusId(session.ReferralId, queryType);
            int QstatusId = qDetails.QueueStatusId;
            if (QstatusId == 0)
            {
                QstatusId = getcheck.getQueueStatusIdIfSubmitted(session.ReferralId, queryType);

            }
            LookUp lookup = new LookUp();
            lookup = objData.LookUps.Where(obj => obj.LookupType == "Document Type" && obj.LookupName == "Funding").SingleOrDefault();
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
                tblDoc.CreatedOn = System.DateTime.Now;
                objData.Documents.Add(tblDoc);
                objData.SaveChanges();
                rtrnval = tblDoc.DocumentId;
            }




            return rtrnval;
        }




        [HttpPost, ValidateInput(false)]
        [MultiButton(MatchFormKey = "ActionFunding", MatchFormValue = "Add")]
        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult Save(AcademicReviewModel academicReviewModel, HttpPostedFileBase Upfile)
        {
            ViewBag.Chkmsg = "";
            ViewBag.Flag = 1;
            ClsCommon updateCommon = new ClsCommon();
            session = (clsSession)Session["UserSession"];
            if (session != null)
            {
                if (Upfile != null && Upfile.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(Upfile.FileName);
                    //string path = System.Web.Configuration.WebConfigurationManager.AppSettings["filesLocation"].ToString();
                    //string path = AppDomain.CurrentDomain.BaseDirectory + "SavedDocs\\";
                    //if (!Directory.Exists(path))
                    //{
                    //    Directory.CreateDirectory(path);
                    //}

                    int Docid = FileUpload(session.ReferralId, session.SchoolId, academicReviewModel.DocumentName, fileName, session.LoginId, "FV");
                    clsDocumentasBinary DocBinary = new clsDocumentasBinary();
                    DocBinary.SaveBinaryFiles(session.SchoolId, session.ReferralId, academicReviewModel.DocumentName, session.LoginId, Upfile, "Referal", "Funding", Docid);

                    //if (id > 0)
                    //    Upfile.SaveAs(path + id + "-" + fileName);
                }

                academicReviewModel.DocList = GetDocList(session.ReferralId, session.SchoolId, "FV");


                foreach (var item in academicReviewModel.commonCallLog)
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
                academicReviewModel.commonACCREV.AproveInt = academicReviewModel.approvedStatus == 1 ? true : false;
                academicReviewModel.commonACCREV.Draft = "Y";
                // academicReviewModel.commonCallLog.academicReviewId = academicReviewModel.commonACCREV.academicReviewId;

                academicReviewModel.commonACCREV = getcheck.UpdateOrInsertRevCmt("FV", academicReviewModel.commonACCREV.academicReviewId, academicReviewModel.commonACCREV.Comments, academicReviewModel.commonACCREV.Draft, academicReviewModel.commonACCREV.IsPresent, academicReviewModel.commonACCREV.AproveInt, "FV");
                academicReviewModel.ChkAll = updateCommon.UpdateOrInsertMulCheckList(academicReviewModel.ChkAll, academicReviewModel.commonACCREV.academicReviewId, "FV", academicReviewModel.commonACCREV.QstatusId, "Check");
                //academicReviewModel.engineLetter = getcheck.UpdateOrInsertCheckList(academicReviewModel.engineLetter, academicReviewModel.commonACCREV.academicReviewId, "FV", academicReviewModel.commonACCREV.QstatusId, "Check");
                //academicReviewModel.commonCallLog = getcheck.UpdateOrInsertCallLog("FV", academicReviewModel.commonACCREV.academicReviewId, academicReviewModel.commonCallLog.AppntTime, academicReviewModel.commonACCREV.Draft, academicReviewModel.commonCallLog.CallTime, academicReviewModel.commonCallLog.CallLogId, academicReviewModel.commonCallLog.Conversation, academicReviewModel.commonCallLog.NameOfContact, academicReviewModel.commonCallLog.StaffName, academicReviewModel.commonCallLog.IsPresent, academicReviewModel.commonACCREV.QstatusId);
                academicReviewModel.commonCallLog = getcheck.UpdateOrInsertMultipleCallLog("FV", academicReviewModel.commonACCREV.academicReviewId, "Y", academicReviewModel.commonCallLog, academicReviewModel.commonACCREV.QstatusId);
                //academicReviewModel.DocList = GetDocList(session.ReferralId, session.SchoolId);
            }
            return View("FundingVerification", academicReviewModel);
        }


        public string SaveFundingDocs(AcademicReviewModel academicReviewModel, HttpPostedFileBase Upfile)
        {
            ViewBag.Chkmsg = "";
            ViewBag.Flag = 1;
            session = (clsSession)Session["UserSession"];
            if (session != null)
            {
                if (Upfile != null && Upfile.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(Upfile.FileName);
                    //string path = System.Web.Configuration.WebConfigurationManager.AppSettings["filesLocation"].ToString();
                    string path = AppDomain.CurrentDomain.BaseDirectory + "SavedDocs\\";
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    int id = (Directory.GetFiles(path).Length) + 1;
                    Upfile.SaveAs(path + id + "-" + fileName);
                    //int id = FileUpload(session.ReferralId, session.SchoolId, academicReviewModel.DocumentName, fileName, session.LoginId);
                    //if (id > 0)
                    //    Upfile.SaveAs(path + id + "-" + fileName);
                }
                academicReviewModel.DocList = GetDocList(session.ReferralId, session.SchoolId, "FV");
            }
            return "";
        }


        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public void DownloadFundingDoc(int id)
        {
            string filePath = "";
            string result = "";
            session = (clsSession)Session["UserSession"];
            if (session != null)
            {
                var binDoc = objData.binaryFiles.Where(x => x.BinaryId == id && x.SchoolId == session.SchoolId).ToList();

                var DocList = (from Doc in binDoc
                               select new DocumentListWithBinary
                               {
                                   DocId = Doc.BinaryId,
                                   DocName = Doc.DocumentName,
                                   ContentType = Doc.ContentType.ToString(),
                                   Data = Doc.Data,
                               }).SingleOrDefault();


                ShowDocument(DocList.DocName, DocList.Data, DocList.ContentType);
                //    result = DownloadDoc(id, session.ReferralId);

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

            }
            // return File(result, Server.UrlEncode(result));
        }

        public IList<DocumentDownloadViewModel> GetDocList(int StudentId, int SchoolId, string querytype)
        {
            MelmarkDBEntities objData = new MelmarkDBEntities();
            QstatusDetails qDetails = getcheck.getQueueStatusId(session.ReferralId, querytype);
            int QstatusId = qDetails.QueueStatusId;
            if (QstatusId == 0)
            {
                QstatusId = getcheck.getQueueStatusIdIfSubmitted(session.ReferralId, querytype);

            }



            var DocLists = (from objDoc in objData.binaryFiles
                            join docQStatus in objData.Documents on objDoc.DocId equals docQStatus.DocumentId
                            where (objDoc.type == "Referal" && objDoc.ModuleName == "Funding" && objDoc.SchoolId == SchoolId && (docQStatus.QueueStatusId == QstatusId || docQStatus.QueueStatusId == 0) && objDoc.StudentId == StudentId)

                            select new DocumentDownloadViewModel
                            {
                                IEPId = objDoc.BinaryId,
                                IEPName = objDoc.DocumentName,
                                IEPPath = "",
                                Verified = objDoc.Varified,
                            }).OrderBy(t => t.IEPName).ToList();

            return DocLists;
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



        public string DownloadDoc(string documentId, int StudentId)
        {
            Document ObjDoc = new Document();
            string dirpath = (AppDomain.CurrentDomain.BaseDirectory + "Files/Documents".ToString()).Replace('\\', '/');
            dirpath = AppDomain.CurrentDomain.BaseDirectory + "SavedDocs\\" + documentId;

            return dirpath;
        }

        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult userListAssignTeam(CommanUserViewModel academicModel, int TeamIdz = 0)
        {
            session = (clsSession)Session["UserSession"];
            MelmarkDBEntities objData = new MelmarkDBEntities();
            var val = (from x in objData.TeamMembers
                       join y in objData.Users on x.UserId equals y.UserId
                       where (x.TeamId == TeamIdz)
                       select new
                       {
                           UserName = y.UserLName + "," + y.UserFName
                       }).ToList();
            userNames usrNm = new userNames();
            usrNm.TeamId = TeamIdz;
            int i = 0;
            //usrNm.UserNames = val.ToList();
            foreach (var singleUsr in val)
            {
                usrNm.UserNames.Add(singleUsr.UserName);
                i++;
            }
            return View("AssignUserList", usrNm);
        }


        //---- List 3 - Task #30 [20-Oct-2020] - (Start) ---//
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public List<SelectListItem> FillDropQueueList()
        {
            objData = new MelmarkDBEntities();
            List<SelectListItem> QueueList = new List<SelectListItem>();

            QueueList = (from objqueue in objData.ref_Queue
                         where objqueue.MasterId != 0 && (objqueue.QueueType != "RT" && objqueue.QueueType != "MO" &&
                         objqueue.QueueType != "SM" && objqueue.QueueType != "IP" && objqueue.QueueType != "PM" && objqueue.QueueType != "CM" && objqueue.QueueType != "CT" && objqueue.QueueType != "SA" && objqueue.QueueType != "PI" && objqueue.QueueType != "RS" && objqueue.QueueType != "DC" && objqueue.QueueType != "PT")
                         select new SelectListItem
                         {
                             Text = objqueue.QueueName,
                             Value = SqlFunctions.StringConvert((double)objqueue.QueueId).Trim()
                         }).ToList();
            return QueueList;
        }

        public int AddLettertoTray(int getQid)
        {
            MelmarkDBEntities objData1 = new MelmarkDBEntities();
            session = (clsSession)Session["UserSession"];
            int Qid = getQid;
            string Qtype = getcheck.getQueueType(Qid);
            if (Qtype != null)
            {
                //var LetterName = objData.LetterEngines.Where(x => x.QueueId == Qid && x.ApproveStatus == false && x.SchoolId == session.SchoolId && x.LetterType == "Letter").ToList();
                var LetterName = objData.LetterEngines.Where(x => x.QueueId == Qid && x.SchoolId == session.SchoolId && x.LetterType == "Letter").ToList();
                if (LetterName.Count > 0)
                {
                    getLetterTray.insertLetter(Qtype, session.ReferralId, LetterName[0].LetterEngineId, "Parent");
                }
                LetterGeneration.LetterLists = getLetterTray.getLetterList("");
                var studentPer = objData.StudentPersonals.Where(x => x.StudentPersonalId == session.ReferralId).SingleOrDefault();
                //studentPer.InactiveList = true; //Comented fro not deactivaing student
                //studentPer.WaitingList = false; //Comented fro not deactivaing student
                objData.SaveChanges();
            }
            return 0;
        }

        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public int RemoveLetter(string LetterTrayIds)
        {
            MelmarkDBEntities objData1 = new MelmarkDBEntities();
            session = (clsSession)Session["UserSession"];
            string[] IDs = LetterTrayIds.Split(',');
            if (IDs != null)
            {
                int GetLetterid = Convert.ToInt32(IDs[0]);
                int GetLetterQueueid = Convert.ToInt32(IDs[1]);
                var GetLetter = objData.ref_LetterTrayValues.Where(x => x.LetterTrayId == GetLetterid && x.QueueId == GetLetterQueueid).FirstOrDefault();
                if (GetLetter != null)
                {
                    string gQtype = getcheck.getQueueType(GetLetterQueueid);
                    string SetStdIdtoFalse = "-" + GetLetter.StudentPersonalId.ToString(); //Modfied as Part of List 3 - #33
                    GetLetter.StudentPersonalId = Convert.ToInt32(SetStdIdtoFalse); //Modfied as Part of List 3 - #33
                    if (gQtype == "OTH") //Modfied as Part of List 3 - #33
                    {
                        string SetLetterIdtoFalse = "-" + GetLetter.LetterId.ToString(); //Modfied as Part of List 3 - #33
                        GetLetter.LetterId = Convert.ToInt32(SetLetterIdtoFalse); //Modfied as Part of List 3 - #33
                    } //Modfied as Part of List 3 - #33
                    //objData.ref_LetterTrayValues.Remove(GetLetter);
                    objData.SaveChanges();
                }
            }
            return 0;
        }
        //---- List 3 - Task #30 [20-Oct-2020] - (End) ---//
    }
}
