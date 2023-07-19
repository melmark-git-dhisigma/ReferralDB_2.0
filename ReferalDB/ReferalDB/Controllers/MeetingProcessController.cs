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

namespace ReferalDB.Controllers
{
    public class MeetingProcessController : Controller
    {
        //
        // GET: /AcceptanceProcess/
        public clsSession sess = null;
        LetterTray getLetterTray = new LetterTray();
        LetterGenerationViewModel LetterGeneration = new LetterGenerationViewModel();
        public ActionResult Index()
        {
            return View();
        }

        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult DocumentChecklist()
        {
            ClsCommon getCommon = new ClsCommon();
            sess = (clsSession)Session["UserSession"];
            if (sess != null)
                ViewBag.permission = getCommon.setPermission();
            DocumentChecklistViewModel DocChk = new DocumentChecklistViewModel();            
            DocChk.ChkAll = getCommon.getCheckListMultiple("DC", "Check");
            DocChk.Comment = getCommon.getRevCmt("DC");
            return View("DocumentCheckList", DocChk);
        }

        //[ActiveSession]
        [HttpPost, ValidateInput(false)]
        [MultiButton(MatchFormKey = "Save", MatchFormValue = "Save")]
        public ActionResult DocumentChecklistSave(DocumentChecklistViewModel model)
        {
            try
            {
                MelmarkDBEntities objData = new MelmarkDBEntities();
                ClsCommon updateCommon = new ClsCommon();
                string returnVal = "";
                int Qid = updateCommon.getQueueId("DC");
                bool AccptanceDraftMode = updateCommon.AcceptanceSubQDraftMode(Qid);                
                model.Comment = updateCommon.UpdateOrInsertRevCmt("DC", model.Comment.academicReviewId, model.Comment.Comments, "Y", model.Comment.IsPresent, model.Comment.AproveInt, "DC");                
                model.ChkAll = updateCommon.UpdateOrInsertMulCheckList(model.ChkAll, model.Comment.academicReviewId, "DC", model.Comment.QstatusId, "Check");
                returnVal = clsGeneral.sucessMsg("Data Successfully Saved");                
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
        [MultiButton(MatchFormKey = "Save", MatchFormValue = "Submit")]
        public ActionResult DocumentChecklistSubmit(DocumentChecklistViewModel model)
        {
            string returnVal = "";
            try
            {
                MelmarkDBEntities objData = new MelmarkDBEntities();
                ClsCommon updateCommon = new ClsCommon();
                Placement objPlacement = new Placement();
                sess = (clsSession)Session["UserSession"];
                Session["QueueList"] = null;
                DateTime? placementdate = null;                
                if (sess != null)
                {
                    if (model.ChkAll != null)

                    {
                        if (model.ChkAll.Count > 0)
                        {
                            bool success = updateCommon.getCheckedCheckListMul(model.ChkAll);
                            int QUeueid = updateCommon.getQueueId("DC");
                            var LetterName = objData.LetterEngines.Where(x => x.QueueId == QUeueid && x.ApproveStatus == true && x.SchoolId == sess.SchoolId && x.LetterType == "Letter").ToList();
                            if (LetterName.Count > 0)
                                getLetterTray.insertLetter("DC", sess.ReferralId, LetterName[0].LetterEngineId, "Parent");
                            LetterGeneration.LetterLists = getLetterTray.getLetterList("");
                            model.Comment = updateCommon.UpdateOrInsertRevCmt("DC", model.Comment.academicReviewId, model.Comment.Comments, "N", model.Comment.IsPresent, true, "DC");
                            model.ChkAll = updateCommon.UpdateOrInsertMulCheckList(model.ChkAll, model.Comment.academicReviewId, "DC", model.Comment.QstatusId, "Check");
                            if (Session["QueueList"] != null && Session["QueueList"] != "")
                            {
                                returnVal = Session["QueueList"].ToString();
                            }
                            else
                            { 
                                var placement = objData.ref_PlacementMeeting.Where(x => x.StudentPersonalId == sess.ReferralId).ToList();
                                if (placement.Count > 0)
                                {
                                    placementdate = placement[0].StartDate;
                                }
                                var matchopen = objData.ref_MatchOpening.Where(x => x.StudentId == sess.ReferralId && x.Draft == "N").ToList();
                                if (matchopen.Count > 0)
                                {
                                    objPlacement.StudentPersonalId = sess.ReferralId;
                                    objPlacement.SchoolId = sess.SchoolId;
                                    objPlacement.Status = 1;                                    
                                    objPlacement.StartDate = placementdate;
                                    objPlacement.Department = matchopen[0].DepartmentId;
                                    objPlacement.PlacementType = matchopen[0].Program;
                                    objPlacement.CreatedBy = sess.LoginId;
                                    objPlacement.CreatedOn = System.DateTime.Now;
                                    objData.Placements.Add(objPlacement);
                                    objData.SaveChanges();
                                }


                                // Add here

                                var s = objData.StudentPersonals.Where(m => m.StudentPersonalId == sess.ReferralId).SingleOrDefault();
                                var maxClientId = objData.StudentPersonals.Max(m => m.ClientId);
                                if (maxClientId != null)
                                {
                                    maxClientId = maxClientId + 1;
                                    s.ClientId = maxClientId;
                                }
                                else
                                {
                                    maxClientId = 50000;
                                    s.ClientId = maxClientId;
                                }
                                objData.SaveChanges();
                                returnVal = "success";
                            }
                        }
                    }
                    else
                    {

                        model.Comment = updateCommon.UpdateOrInsertRevCmt("DC", model.Comment.academicReviewId, model.Comment.Comments, "N", model.Comment.IsPresent, true, "DC");
                        model.ChkAll = updateCommon.UpdateOrInsertMulCheckList(model.ChkAll, model.Comment.academicReviewId, "DC", model.Comment.QstatusId, "Check");
                        if (Session["QueueList"] != null && Session["QueueList"].ToString() != "")
                        {
                            returnVal = Session["QueueList"].ToString();
                        }
                        else
                        {
                            var placement = objData.ref_PlacementMeeting.Where(x => x.StudentPersonalId == sess.ReferralId).ToList();
                            if (placement.Count > 0)
                            {
                                placementdate = placement[0].StartDate;
                            }

                            var matchopen = objData.ref_MatchOpening.Where(x => x.StudentId == sess.ReferralId && x.Draft == "N").ToList();
                            if (matchopen.Count > 0)
                            {
                                objPlacement.StudentPersonalId = sess.ReferralId;
                                objPlacement.SchoolId = sess.SchoolId;
                                objPlacement.Status = 1;
                                objPlacement.StartDate = placementdate;
                                objPlacement.Department = matchopen[0].DepartmentId;
                                objPlacement.PlacementType = matchopen[0].Program;
                                objPlacement.CreatedBy = sess.LoginId;
                                objPlacement.CreatedOn = System.DateTime.Now;
                                objData.Placements.Add(objPlacement);
                                objData.SaveChanges();
                            }


                            // Add here

                            var s = objData.StudentPersonals.Where(m => m.StudentPersonalId == sess.ReferralId).SingleOrDefault();
                            var maxClientId = objData.StudentPersonals.Max(m => m.ClientId);
                            if (maxClientId != null)
                            {
                                maxClientId = maxClientId + 1;
                                s.ClientId = maxClientId;
                            }
                            else
                            {
                                maxClientId = 50000;
                                s.ClientId = maxClientId;
                            }
                            objData.SaveChanges();
                            returnVal = "success";
                        }
                    }
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
            return View("userListDocChk", academicModel);
        }

        /// Meeting Progress Actions ///
        //[ActiveSession]
        [HttpGet, ValidateInput(false)]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult MeetingView(MeetingModel model, string msg = "")
        {
            ClsCommon getCommon = new ClsCommon();
            sess = (clsSession)Session["UserSession"];
            if (sess != null)
                ViewBag.permission = getCommon.setPermission();
            ViewBag.Chkmsg = msg;
            ViewBag.Flag = 0;
            return View(model);
        }
        [HttpPost, ValidateInput(false)]

        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        [MultiButton(MatchFormKey = "SaveMeeting", MatchFormValue = "Save")]
        public ActionResult Save(MeetingModel model)
        {
            ViewBag.Chkmsg = "";
            ViewBag.Flag = 1;
            MelmarkDBEntities objData = new MelmarkDBEntities();
            ClsCommon clsComm = new ClsCommon();
            sess = (clsSession)Session["UserSession"];
            if (sess != null)
            {
                string result = "";
                int Qid = 0;
                if (model.getMeetingType == "placement")
                {
                    Qid = clsComm.getQueueId("PM");
                    bool AccptanceDraftMode = clsComm.AcceptanceSubQDraftMode(Qid);
                    result = model.SavePlc("Y", sess.ReferralId, sess.SchoolId, sess.LoginId, model.AgreementLists);
                }
                else if (model.getMeetingType == "consent")
                {
                    Qid = clsComm.getQueueId("CM");
                    bool AccptanceDraftMode = clsComm.AcceptanceSubQDraftMode(Qid);
                    result = model.SaveCon("Y", sess.ReferralId, sess.SchoolId, sess.LoginId, model.ConsentLists);
                }
                ViewBag.Chkmsg = result;                
            }
            return View("MeetingView", model);
        }

        //[ActiveSession]
        [HttpPost, ValidateInput(false)]
        [MultiButton(MatchFormKey = "SaveMeeting", MatchFormValue = "Submit")]
        public ActionResult Save(MeetingModel model, string a)
        {
            ViewBag.Flag = 0;
            ClsCommon clsComm = new ClsCommon();
            MelmarkDBEntities objData = new MelmarkDBEntities();
            sess = (clsSession)Session["UserSession"];
            string result = "";
            if (sess != null)
            {
                int qId = 0;
                int Qid = 0;
                if (model.getMeetingType == "placement")
                {
                    qId = clsComm.getQueueId("PM");
                    Qid = clsComm.getQueueId("PM");
                    result = model.SavePlc("N", sess.ReferralId, sess.SchoolId, sess.LoginId, model.AgreementLists);
                    if (result == clsGeneral.sucessMsg("Successfully Saved"))
                    {
                        int qIdNext = clsComm.getQueueId("CM");
                        return Content("success*" + sess.ReferralId + "_" + Qid);
                    }
                }
                else if (model.getMeetingType == "consent")
                {
                    qId = clsComm.getQueueId("CM");
                    Qid = clsComm.getQueueId("CM");
                    bool AccptanceDraftMode = clsComm.AcceptanceSubQDraftMode(Qid);
                    result = model.SaveCon("N", sess.ReferralId, sess.SchoolId, sess.LoginId, model.ConsentLists);
                    if (result == clsGeneral.sucessMsg("Successfully Saved"))
                    {
                        int qIdNext = clsComm.getQueueId("CT");
                        return Content("success*" + sess.ReferralId + "_" + Qid);
                    }
                }
            }
            ViewBag.Chkmsg = result;
            return View("MeetingView", model);
        }

        //[ActiveSession]
        [HttpPost, ValidateInput(false)]
        [MultiButton(MatchFormKey = "SaveMeeting", MatchFormValue = "Add")]
        public ActionResult Save(MeetingModel model, HttpPostedFileBase Upfile)
        {
            ViewBag.Chkmsg = "";
            ViewBag.Flag = 1;
            sess = (clsSession)Session["UserSession"];
            if (sess != null)
            {
                if (model.getMeetingType == "placement")
                {
                    string result = model.SavePlc("Y", sess.ReferralId, sess.SchoolId, sess.LoginId, model.AgreementLists);
                    result = "";
                    ViewBag.Chkmsg = result;
                }
                if (Upfile != null && Upfile.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(Upfile.FileName);
                    string docname = "";
                    if (model.getMeetingType == "placement")
                    {
                        docname = model.DocumentName1;
                    }
                    else if (model.getMeetingType == "consent")
                    {
                        docname = model.DocumentName2;
                    }
                    else
                    {
                        docname = "SampleDoc" + Upfile.FileName;
                    }

                    clsDocumentasBinary objBinary = new clsDocumentasBinary();
                    if (model.getMeetingType == "placement")
                    {
                        int Docid = model.FileUploadPM(sess.ReferralId, sess.SchoolId, docname, fileName, sess.LoginId);
                        objBinary.SaveBinaryFiles(sess.SchoolId, sess.ReferralId, docname, sess.LoginId, Upfile, "Referal", "Placement Agreement", Docid);
                    }
                    else if (model.getMeetingType == "consent")
                    {
                        int Docid = model.FileUploadCM(sess.ReferralId, sess.SchoolId, docname, fileName, sess.LoginId);
                        objBinary.SaveBinaryFiles(sess.SchoolId, sess.ReferralId, docname, sess.LoginId, Upfile, "Referal", "Consent", Docid);
                    }
                }
                model.GetAgreements(sess.ReferralId, sess.SchoolId);
                model.GetConsents(sess.ReferralId, sess.SchoolId);
            }
            return View("MeetingView", model);
        }

        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public void DownloadDoc(int id)
        {
            MelmarkDBEntities objData = new MelmarkDBEntities();
            string filePath = "";
            string result = "";
            sess = (clsSession)Session["UserSession"];
            if (sess != null)
            {
                var binDoc = objData.binaryFiles.Where(x => x.DocId == id && x.SchoolId == sess.SchoolId).ToList();

                if (binDoc.Count > 0)
                {
                    var DocList = (from Doc in binDoc
                                   select new DocumentListWithBinary
                                   {
                                       DocId = Doc.BinaryId,
                                       DocName = Doc.DocumentName,
                                       ContentType = Doc.ContentType.ToString(),
                                       Data = Doc.Data,
                                   }).SingleOrDefault();
                    ShowDocument(DocList.DocName, DocList.Data, DocList.ContentType);
                }

            }            
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
        // include in project
        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public void DownloadConsent(int id)
        {
            sess = (clsSession)Session["UserSession"];
            MelmarkDBEntities objData = new MelmarkDBEntities();
            if (sess != null)
            {
                var binDoc = objData.binaryFiles.Where(x => x.BinaryId == id && x.SchoolId == sess.SchoolId).ToList();
                if (binDoc.Count > 0)
                {
                    var DocList = (from Doc in binDoc
                                   select new DocumentListWithBinary
                                   {
                                       DocId = (int)Doc.DocId,
                                       DocName = Doc.DocumentName,
                                       ContentType = Doc.ContentType.ToString(),
                                       Data = Doc.Data,
                                   }).SingleOrDefault();
                    ShowDocument(DocList.DocName, DocList.Data, DocList.ContentType);
                }
            }            
        }

    }
}
