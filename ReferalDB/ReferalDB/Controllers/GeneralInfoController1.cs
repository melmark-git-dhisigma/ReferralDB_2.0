using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer;
using System.Web.Mvc;
using System.Xml;
using System.Web.UI.HtmlControls;
using ReferalDB.Models;
using ReferalDBApplicant.Classes;
using System.IO;
using ReferalDB.CommonClass;
using System.Web.Configuration;
using System.Web.UI;

namespace ReferalDB.Controllers
{
    public class GeneralInfoController : Controller
    {
        //
        // GET: /GeneralInfo/

        public clsSession sess = null;

        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult Index()
        {
            // clsSessionTab sesClsObj = new clsSessionTab();
            //    Session["sesData"] = sesClsObj;
            ClsCommon getCommon = new ClsCommon();
            int imageSize = Convert.ToInt32(WebConfigurationManager.AppSettings["imageSize"].ToString());
            ViewBag.imageSize = imageSize;
            ViewBag.permission = getCommon.setPermission();
            return View("GeneralInfoData");
        }

        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult GeneralInfoData(GenInfoModel genModel = null)
        {
            return RedirectToAction("XmlResult");
        }

        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult Section1(int id, bool isAlert = false)
        {
            ClsErrorLog error = new ClsErrorLog();
            bool tabSaved = false;
            int studentId = 0;
            clsReferral clsRef = new clsReferral();
            sess = (clsSession)Session["UserSession"];

            // clsSessionTab sesClsObj = (clsSessionTab)Session["sesData"];
            bool valid = false;

            if (sess != null)
            {
                error.WriteToLog("Student Id : " + sess.ReferralId);
                if (sess.ReferralId != 0)
                {
                    studentId = sess.ReferralId;
                }
                else
                {
                    studentId = 0;
                }
            }
            else
            {
                studentId = 0;
            }

            if (studentId > 0)
            {

                sess.ReferralId = studentId;                                  // set session Id to studentId
                GenInfoModel model = new GenInfoModel();
                model = clsRef.LoadStudentData(studentId, id);
                TempData["LoadStudentData"] = model;

                if (id == 1)
                {
                    sess.SessTab1 = studentId;
                }
                if (id == 2)
                {
                    tabSaved = IsTabSaved(studentId, "Tab2");

                    if (tabSaved == true)
                    {
                        sess.SessTab2 = studentId;
                    }
                    else
                    {
                        sess.SessTab2 = 0;
                    }

                }
                if (id == 3)
                {

                    tabSaved = IsTabSaved(studentId, "Tab3");
                    error.WriteToLog("Tab Saved: " + tabSaved);
                    error.WriteToLog("Student Id: " + studentId);
                    if (tabSaved == true)
                    {
                        sess.SessTab3 = studentId;
                    }
                    else
                    {
                        sess.SessTab3 = 0;
                    }


                    //valid = IsSessionTab3(studentId);
                    //if (valid == true)
                    //{
                    //    sess.SessTab3 = studentId;
                    //}
                    //else
                    //{
                    //    sess.SessTab3 = 0;
                    //}
                }

                if (id == 4)
                {
                    tabSaved = IsTabSaved(studentId, "Tab4");

                    if (tabSaved == true)
                    {
                        sess.SessTab4 = studentId;
                    }
                    else
                    {
                        sess.SessTab4 = 0;
                    }

                    //valid = IsSessionTab3(studentId);
                    //if (valid == true)
                    //{
                    //    sess.SessTab4 = studentId;
                    //}
                    //else
                    //{
                    //    sess.SessTab4 = 0;
                    //}
                }
                if (id == 5)
                {
                    tabSaved = IsTabSaved(studentId, "Tab5");

                    if (tabSaved == true)
                    {
                        sess.SessTab5 = studentId;
                    }
                    else
                    {
                        sess.SessTab5 = 0;
                    }
                }
                if (id == 6)
                {
                    tabSaved = IsTabSaved(studentId, "Tab6");

                    if (tabSaved == true)
                    {
                        sess.SessTab6 = studentId;
                    }
                    else
                    {
                        sess.SessTab6 = 0;
                    }
                }
                if (id == 7)
                {
                    tabSaved = IsTabSaved(studentId, "Tab7");

                    if (tabSaved == true)
                    {
                        sess.SessTab7 = studentId;
                    }
                    else
                    {
                        sess.SessTab7 = 0;
                    }
                }
                if (id == 8)
                {
                    sess.SessTab8 = 0;
                }

                clsReferral cls = new clsReferral();
                bool flag = cls.findNewAppFlag(sess.ReferralId);
                if (flag == true)
                    ViewBag.html = XmlResult(model, id);
                else
                    ViewBag.html = XmlResultOld(model, id);


            }
            else
            {
                GenInfoModel model = new GenInfoModel();
                clsReferral cls = new clsReferral();
                bool flag = cls.findNewAppFlag(sess.ReferralId);
                if (flag == true)
                    ViewBag.html = XmlResult(model, id);
                else
                    ViewBag.html = XmlResultOld(model, id);

                //{
                //    ViewBag.html += " <div id='popupDiv'>Data Saved Successfully</div>";
                //}
            }
            return View();

        }


        [HttpPost, ValidateInput(false)]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult DocumentSave(DocumentDownloadViewModel model, HttpPostedFileBase fileUpldDocName)
        {
            sess = (clsSession)Session["UserSession"];
            MelmarkDBEntities objData = new MelmarkDBEntities();
            ClsCommon clsCommon = new ClsCommon();
            ViewBag.File = 0;
            if (sess != null)
            {
                if (fileUpldDocName != null)
                {
                    string Result = clsCommon.GetFileType(fileUpldDocName.ContentType);
                    if (Result != "")
                    {
                        UploadDoccuments(sess.ReferralId, model, fileUpldDocName, sess.SchoolId, sess.LoginId);
                    }
                    else
                    {
                        return Content("Invalid file format");
                    }
                }
                else
                {
                    return Content("No file selected");
                }

            }

            return Content("Saved Successfully");

        }

        public void UploadDoccuments(int studentPersnlId, DocumentDownloadViewModel model, HttpPostedFileBase upldFile, int schoolId, int loginId)
        {
            clsReferral objRef = new clsReferral();
            clsDocumentasBinary objBinary = new clsDocumentasBinary();
            try
            {
                if (upldFile != null && upldFile.ContentLength > 0 && model.DocumentType != "")
                {

                    string fileName = Path.GetFileName(upldFile.FileName);
                    string Name = objBinary.LookName(Convert.ToInt32(model.DocumentType));

                    int Docid = objRef.FileUpload(studentPersnlId, schoolId, loginId, model.DocumentName, model.OtherFDocType, fileName, Convert.ToInt32(model.DocumentType));
                    if (Docid > 0)

                        objBinary.SaveBinaryFiles(sess.SchoolId, sess.ReferralId, model.DocumentName, sess.LoginId, upldFile, "Referal_upld", Name, Docid);


                    //  upldFile.SaveAs(path + id + "-" + fileName);
                }
            }
            catch (Exception e)
            {

            }
        }



        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult SaveGeneralDataOld(GenInfoModel model, string type, FormCollection formdata, HttpPostedFileBase fileUpldDocName, HttpPostedFileBase fileUploadPhotoName)
        {
            int id = 0;
            int sveId = 0;
            string status = "";
            byte[] xmlData = null;
            clsReferral clsRf = new clsReferral();
            ActionResult returnResult = null;
            ClsCommon clsCommon = new ClsCommon();
            LetterTray clsletter = new LetterTray();
            sess = (clsSession)Session["UserSession"];
            MelmarkDBEntities objData = new MelmarkDBEntities();
            ApplicantUploadDownload clsUpld = new ApplicantUploadDownload();
            // clsSessionTab sesClsObj = (clsSessionTab)Session["sesData"];
            string xmlPath = Server.MapPath("../XML/RefInfoCustom.xml");
            TempData["type"] = type;

            if (sess != null)
            {

                if (model.TabId == 1)
                {
                    int tempId = 0;
                    tempId = Convert.ToInt32(Session["RefId"]);
                    ClsErrorLog errlog = new ClsErrorLog();
                    errlog.WriteToLog("sessRefId: " + sess.ReferralId);
                    errlog.WriteToLog("tempId: " + tempId);
                    errlog.WriteToLog("status: " + status);
                    errlog.WriteToLog("type: " + type);
                    if (sess.SessTab1 > 0 || tempId > 0)
                    {
                        status = "update";
                        sveId = Convert.ToInt32(sess.SessTab1);
                        errlog.WriteToLog("sveId: " + sveId);
                        if (sveId != 0)
                        {
                            TempData["EditId"] = id = clsRf.SaveGeneralDataOld(model, type, sveId, status, sess.LoginId, sess.SchoolId);
                        }
                        else
                            TempData["EditId"] = id = clsRf.SaveGeneralDataOld(model, type, tempId, status, sess.LoginId, sess.SchoolId);
                        sess.ReferralId = id;
                        sess.SessTab1 = id;
                    }
                    else
                    {
                        status = "save";
                        TempData["EditId"] = id = clsRf.SaveGeneralDataOld(model, type, sveId, status, sess.LoginId, sess.SchoolId);

                        xmlData = clsRf.SaveAsBlob(xmlPath);   //Initial full xml doccument saving to database
                        clsRf.SaveXmlToDB(xmlData, id);

                        sess.ReferralId = id;                    // Insert the new referal Id in Q Status
                        clsCommon.insertQstatus("NA", "N");
                        clsCommon.insertQstatus("AR", "Y");
                        int Qid = clsCommon.getQueueId("NA");
                        var LetterName = objData.LetterEngines.Where(x => x.QueueId == Qid && x.ApproveStatus == true && x.SchoolId == sess.SchoolId && x.LetterType == "Letter").ToList();
                        if (LetterName.Count > 0)
                        {
                            clsletter.insertLetter("NA", sess.ReferralId, LetterName[0].LetterEngineId, "Parent");
                        }
                        sess.SessTab1 = id;
                        Session["RefId"] = id;
                        sess.ReferralId = id;
                    }
                    if (fileUploadPhotoName != null)
                    {
                        string Result = clsCommon.GetFileAllowed(Path.GetExtension(fileUploadPhotoName.FileName));
                        if (Result != "")
                        {
                            UploadStudentPhoto(id, model, fileUploadPhotoName);
                        }
                        else
                        {
                            return Content("Invalid file");
                        }
                    }

                    xmlData = SaveCustomXmlOld(formdata, id, model.TabId);
                    clsRf.SaveXmlToDB(xmlData, id);

                    returnResult = RedirectToAction("SectionRedirect", new { tabID = model.TabId });

                }
                else if (model.TabId == 2)
                {
                    if (Session["RefId"] != null)
                    {
                        int tempId = 0;
                        tempId = Convert.ToInt32(Session["RefId"]);
                        ClsErrorLog errlog = new ClsErrorLog();
                        errlog.WriteToLog("sessRefId: " + sess.ReferralId);
                        errlog.WriteToLog("tempId: " + tempId);
                        errlog.WriteToLog("status: " + status);
                        errlog.WriteToLog("type: " + type);
                        errlog.WriteToLog("sveId: " + sveId);
                        if (sess.SessTab2 > 0)
                        {
                            status = "update";
                            sveId = Convert.ToInt32(sess.SessTab2);
                            errlog.WriteToLog("sveId: " + sveId);
                            if (sveId != 0)
                            {
                                TempData["EditId"] = id = clsRf.SaveGeneralDataOld(model, type, sveId, status, sess.LoginId, sess.SchoolId);
                            }
                            else
                                TempData["EditId"] = id = clsRf.SaveGeneralDataOld(model, type, tempId, status, sess.LoginId, sess.SchoolId);
                            sess.ReferralId = id;
                            clsRf.SaveCurrentTab(id, "Tab2");
                        }
                        else
                        {
                            status = "save";
                            TempData["EditId"] = id = clsRf.SaveGeneralDataOld(model, type, tempId, status, sess.LoginId, sess.SchoolId);
                            clsRf.SaveCurrentTab(id, "Tab2");

                            sess.SessTab2 = id;
                            Session["RefId"] = id;
                            sess.ReferralId = id;
                        }
                        //if (Session["RefId"] != null)
                        //{
                        //    if (sess.SessTab2 > 0)
                        //    {
                        //        status = "update";
                        //        sveId = Convert.ToInt32(sess.SessTab2);
                        //        TempData["EditId"] = id = clsRf.SaveGeneralData(model, type, sveId, status, sess.LoginId, sess.SchoolId);
                        //    }
                        //    else
                        //    {
                        //        if (Session["RefId"] != null)
                        //        {
                        //            sveId = Convert.ToInt32(Session["RefId"]);
                        //        }
                        //        status = "save";
                        //        TempData["EditId"] = id = clsRf.SaveGeneralData(model, type, sveId, status, sess.LoginId, sess.SchoolId);
                        //        clsRf.SaveCurrentTab(id, "Tab2");
                        //        sess.SessTab2 = id;
                        //    }

                        //    xmlData = SaveCustomXml(formdata, id, model.TabId);
                        //    clsRf.SaveXmlToDB(xmlData, id);

                        //    returnResult = RedirectToAction("SectionRedirect", new { tabID = model.TabId });
                        //}
                        //else
                        //    returnResult = View();
                        xmlData = SaveCustomXmlOld(formdata, id, model.TabId);
                        clsRf.SaveXmlToDB(xmlData, id);

                        returnResult = RedirectToAction("SectionRedirect", new { tabID = model.TabId });
                    }
                    else
                        returnResult = View();

                }
                else if (model.TabId == 3)
                {
                    ClsErrorLog errlog = new ClsErrorLog();
                    errlog.WriteToLog("sessRefId: " + sess.ReferralId);
                    bool itabSaved = IsTabSaved(sess.ReferralId, "Tab3");
                    errlog.WriteToLog("itabSaved: " + itabSaved);
                    errlog.WriteToLog("sessRefId: " + sess.ReferralId);

                    errlog.WriteToLog("status: " + status);
                    errlog.WriteToLog("type: " + type);

                    if (itabSaved)
                    {
                        sess.SessTab3 = sess.ReferralId;
                    }
                    if (Session["RefId"] != null)
                    {
                        int tempId = 0;
                        tempId = Convert.ToInt32(Session["RefId"]);
                        errlog.WriteToLog("tempId: " + tempId);
                        errlog.WriteToLog("tempId: " + sess.SessTab3);
                        if (sess.SessTab3 > 0)
                        {
                            status = "update";

                            sveId = Convert.ToInt32(sess.SessTab3);
                            errlog.WriteToLog("sveId: " + sveId);
                            if (sveId != 0)
                            {
                                TempData["EditId"] = id = clsRf.SaveGeneralDataOld(model, type, sveId, status, sess.LoginId, sess.SchoolId);
                            }
                            else
                                TempData["EditId"] = id = clsRf.SaveGeneralDataOld(model, type, tempId, status, sess.LoginId, sess.SchoolId);
                            errlog.WriteToLog("EditId: " + id);
                            sess.SessTab3 = id;
                            clsRf.SaveCurrentTab(id, "Tab3");
                        }
                        else
                        {
                            status = "save";
                            TempData["EditId"] = id = clsRf.SaveGeneralDataOld(model, type, tempId, status, sess.LoginId, sess.SchoolId);
                            clsRf.SaveCurrentTab(id, "Tab3");

                            sess.SessTab3 = id;
                            Session["RefId"] = id;
                            sess.ReferralId = id;
                        }
                        xmlData = SaveCustomXmlOld(formdata, id, model.TabId);
                        clsRf.SaveXmlToDB(xmlData, id);

                        returnResult = RedirectToAction("SectionRedirect", new { tabID = model.TabId });
                    }
                    else
                        returnResult = View();
                    //if (Session["RefId"] != null)
                    //{
                    //    if (sess.SessTab3 > 0)
                    //    {
                    //        status = "update";
                    //        sveId = Convert.ToInt32(sess.SessTab3);
                    //        TempData["EditId"] = id = clsRf.SaveGeneralData(model, type, sveId, status, sess.LoginId, sess.SchoolId);
                    //    }
                    //    else
                    //    {
                    //        if (Session["RefId"] != null)
                    //        {
                    //            sveId = Convert.ToInt32(Session["RefId"]);
                    //        }
                    //        status = "save";
                    //        TempData["EditId"] = id = clsRf.SaveGeneralData(model, type, sveId, status, sess.LoginId, sess.SchoolId);
                    //        clsRf.SaveCurrentTab(id, "Tab3");
                    //        sess.SessTab3 = id;
                    //    }

                    //    xmlData = SaveCustomXml(formdata, id, model.TabId);
                    //    clsRf.SaveXmlToDB(xmlData, id);

                    //    returnResult = RedirectToAction("SectionRedirect", new { tabID = model.TabId });
                    //}
                    //else returnResult = View();
                }
                else if (model.TabId == 4)
                {
                    if (Session["RefId"] != null)
                    {
                        int tempId = 0;
                        tempId = Convert.ToInt32(Session["RefId"]);
                        ClsErrorLog errlog = new ClsErrorLog();
                        errlog.WriteToLog("sessRefId: " + sess.ReferralId);
                        errlog.WriteToLog("tempId: " + tempId);
                        errlog.WriteToLog("status: " + status);
                        errlog.WriteToLog("type: " + type);

                        if (sess.SessTab4 > 0)
                        {
                            status = "update";
                            sveId = Convert.ToInt32(sess.SessTab4);
                            errlog.WriteToLog("sveId: " + sveId);
                            if (sveId != 0)
                            {
                                TempData["EditId"] = id = clsRf.SaveGeneralDataOld(model, type, sveId, status, sess.LoginId, sess.SchoolId);
                            }
                            else
                                TempData["EditId"] = id = clsRf.SaveGeneralDataOld(model, type, tempId, status, sess.LoginId, sess.SchoolId);
                            sess.SessTab4 = id;
                            clsRf.SaveCurrentTab(id, "Tab4");
                        }
                        else
                        {
                            status = "save";
                            TempData["EditId"] = id = clsRf.SaveGeneralDataOld(model, type, tempId, status, sess.LoginId, sess.SchoolId);
                            clsRf.SaveCurrentTab(id, "Tab4");

                            sess.SessTab4 = id;
                            Session["RefId"] = id;
                            sess.ReferralId = id;
                        }
                        xmlData = SaveCustomXmlOld(formdata, id, model.TabId);
                        clsRf.SaveXmlToDB(xmlData, id);

                        returnResult = RedirectToAction("SectionRedirect", new { tabID = model.TabId });
                    }
                    else
                        returnResult = View();
                    //if (Session["RefId"] != null)
                    //{
                    //    if (sess.SessTab4 > 0)
                    //    {
                    //        status = "update";
                    //        sveId = Convert.ToInt32(sess.SessTab4);
                    //        TempData["EditId"] = id = clsRf.SaveGeneralData(model, type, sveId, status, sess.LoginId, sess.SchoolId);
                    //    }
                    //    else
                    //    {
                    //        if (Session["RefId"] != null)
                    //        {
                    //            sveId = Convert.ToInt32(Session["RefId"]);
                    //        }
                    //        status = "save";
                    //        TempData["EditId"] = id = clsRf.SaveGeneralData(model, type, sveId, status, sess.LoginId, sess.SchoolId);
                    //        clsRf.SaveCurrentTab(id, "Tab4");
                    //        sess.SessTab4 = id;
                    //    }

                    //    xmlData = SaveCustomXml(formdata, id, model.TabId);
                    //    clsRf.SaveXmlToDB(xmlData, id);

                    //    returnResult = RedirectToAction("SectionRedirect", new { tabID = model.TabId });
                    //}
                    //else returnResult = View();
                }
                else if (model.TabId == 5)
                {
                    if (Session["RefId"] != null)
                    {
                        bool tabSaved = false;
                        int tempId = 0;
                        tempId = Convert.ToInt32(Session["RefId"]);
                        ClsErrorLog errlog = new ClsErrorLog();
                        errlog.WriteToLog("sessRefId: " + sess.ReferralId);
                        errlog.WriteToLog("tempId: " + tempId);
                        errlog.WriteToLog("status: " + status);
                        errlog.WriteToLog("type: " + type);

                        if (sess.SessTab5 > 0)
                        {
                            status = "update";
                            sveId = Convert.ToInt32(sess.SessTab5);
                            errlog.WriteToLog("sveId: " + sveId);
                            if (sveId != 0)
                            {
                                TempData["EditId"] = id = clsRf.SaveGeneralDataOld(model, type, sveId, status, sess.LoginId, sess.SchoolId);
                            }
                            else
                                TempData["EditId"] = id = clsRf.SaveGeneralDataOld(model, type, tempId, status, sess.LoginId, sess.SchoolId);
                            sess.SessTab5 = id;
                            clsRf.SaveCurrentTab(id, "Tab5");
                        }
                        else
                        {

                            status = "save";
                            TempData["EditId"] = id = clsRf.SaveGeneralDataOld(model, type, tempId, status, sess.LoginId, sess.SchoolId);
                            clsRf.SaveCurrentTab(id, "Tab5");
                            tabSaved = IsTabSaved(id, "Tab5");
                            if (tabSaved == true)
                                status = "update";
                            else
                                status = "save";
                            sess.SessTab5 = id;
                            Session["RefId"] = id;
                            sess.ReferralId = id;
                        }
                        xmlData = SaveCustomXmlOld(formdata, id, model.TabId);
                        clsRf.SaveXmlToDB(xmlData, id);

                        returnResult = RedirectToAction("SectionRedirect", new { tabID = model.TabId });
                    }
                    else
                        returnResult = View();

                    //if (Session["RefId"] != null)
                    //{

                    //    if (sess.SessTab5 > 0)
                    //    {
                    //        sveId = Convert.ToInt32(sess.SessTab5);
                    //        if (Session["RefId"] != null)
                    //        {
                    //            TempData["EditId"] = id = sveId;
                    //            xmlData = SaveCustomXml(formdata, id, model.TabId);
                    //            clsRf.SaveXmlToDB(xmlData, id);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        if (Session["RefId"] != null)
                    //        {
                    //            sveId = Convert.ToInt32(Session["RefId"]);
                    //            TempData["EditId"] = id = sveId;
                    //            xmlData = SaveCustomXml(formdata, id, model.TabId);
                    //            clsRf.SaveXmlToDB(xmlData, id);
                    //            clsRf.SaveCurrentTab(id, "Tab5");
                    //            sess.SessTab5 = id;
                    //        }


                    //    }
                    //    returnResult = RedirectToAction("SectionRedirect", new { tabID = model.TabId });
                    //}
                    //else returnResult = View();
                }
                else if (model.TabId == 6)
                {
                    if (Session["RefId"] != null)
                    {
                        bool tabSaved = false;
                        int tempId = 0;
                        tempId = Convert.ToInt32(Session["RefId"]);
                        ClsErrorLog errlog = new ClsErrorLog();
                        errlog.WriteToLog("sessRefId: " + sess.ReferralId);
                        errlog.WriteToLog("tempId: " + tempId);
                        errlog.WriteToLog("status: " + status);
                        errlog.WriteToLog("type: " + type);

                        if (sess.SessTab6 > 0)
                        {
                            status = "update";
                            sveId = Convert.ToInt32(sess.SessTab6);
                            errlog.WriteToLog("sveId: " + sveId);
                            if (sveId != 0)
                            {
                                TempData["EditId"] = id = clsRf.SaveGeneralDataOld(model, type, sveId, status, sess.LoginId, sess.SchoolId);
                            }
                            else
                                TempData["EditId"] = id = clsRf.SaveGeneralDataOld(model, type, tempId, status, sess.LoginId, sess.SchoolId);
                            sess.SessTab6 = id;
                            clsRf.SaveCurrentTab(id, "Tab6");
                        }
                        else
                        {
                            status = "save";
                            TempData["EditId"] = id = clsRf.SaveGeneralDataOld(model, type, tempId, status, sess.LoginId, sess.SchoolId);
                            clsRf.SaveCurrentTab(id, "Tab6");

                            tabSaved = IsTabSaved(id, "Tab6");
                            if (tabSaved == true)
                                status = "update";
                            else
                                status = "save";

                            sess.SessTab6 = id;
                            Session["RefId"] = id;
                            sess.ReferralId = id;
                        }
                        xmlData = SaveCustomXmlOld(formdata, id, model.TabId);
                        clsRf.SaveXmlToDB(xmlData, id);

                        returnResult = RedirectToAction("SectionRedirect", new { tabID = model.TabId });
                    }
                    else
                        returnResult = View();
                    //if (Session["RefId"] != null)
                    //{
                    //    if (sess.SessTab6 > 0)
                    //    {
                    //        sveId = Convert.ToInt32(sess.SessTab6);
                    //        if (Session["RefId"] != null)
                    //        {
                    //            TempData["EditId"] = id = sveId;
                    //            xmlData = SaveCustomXml(formdata, id, model.TabId);
                    //            clsRf.SaveXmlToDB(xmlData, id);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        if (Session["RefId"] != null)
                    //        {
                    //            sveId = Convert.ToInt32(Session["RefId"]);
                    //            TempData["EditId"] = id = sveId;
                    //            xmlData = SaveCustomXml(formdata, id, model.TabId);
                    //            clsRf.SaveXmlToDB(xmlData, id);
                    //            clsRf.SaveCurrentTab(id, "Tab6");
                    //            sess.SessTab6 = id;
                    //        }

                    //        // TempData["EditId"] = id = clsRf.SaveGeneralData(model, type, sveId, status);
                    //        //  sesClsObj.SessTab5 = id;
                    //    }

                    //    returnResult = RedirectToAction("SectionRedirect", new { tabID = model.TabId });
                    //}
                    //else returnResult = View();
                }
                else if (model.TabId == 7)
                {
                    if (Session["RefId"] != null)
                    {
                        bool tabSaved = false;
                        int tempId = 0;
                        tempId = Convert.ToInt32(Session["RefId"]);
                        ClsErrorLog errlog = new ClsErrorLog();
                        errlog.WriteToLog("sessRefId: " + sess.ReferralId);
                        errlog.WriteToLog("tempId: " + tempId);
                        errlog.WriteToLog("status: " + status);
                        errlog.WriteToLog("type: " + type);

                        if (sess.SessTab7 > 0)
                        {
                            status = "update";
                            sveId = Convert.ToInt32(sess.SessTab7);
                            errlog.WriteToLog("sveId: " + sveId);
                            if (sveId != 0)
                            {
                                TempData["EditId"] = id = clsRf.SaveGeneralDataOld(model, type, sveId, status, sess.LoginId, sess.SchoolId);
                            }
                            else
                                TempData["EditId"] = id = clsRf.SaveGeneralDataOld(model, type, tempId, status, sess.LoginId, sess.SchoolId);
                            sess.SessTab7 = id;
                            clsRf.SaveCurrentTab(id, "Tab7");
                        }
                        else
                        {
                            status = "save";
                            TempData["EditId"] = id = clsRf.SaveGeneralDataOld(model, type, tempId, status, sess.LoginId, sess.SchoolId);
                            clsRf.SaveCurrentTab(id, "Tab7");

                            tabSaved = IsTabSaved(id, "Tab7");
                            if (tabSaved == true)
                                status = "update";
                            else
                                status = "save";

                            sess.SessTab7 = id;
                            Session["RefId"] = id;
                            sess.ReferralId = id;
                        }
                        xmlData = SaveCustomXmlOld(formdata, id, model.TabId);
                        clsRf.SaveXmlToDB(xmlData, id);

                        returnResult = RedirectToAction("SectionRedirect", new { tabID = model.TabId });
                    }
                    else
                        returnResult = View();
                    //if (Session["RefId"] != null)
                    //{

                    //    if (sess.SessTab7 > 0)
                    //    {
                    //        sveId = Convert.ToInt32(sess.SessTab7);
                    //        if (Session["RefId"] != null)
                    //        {
                    //            TempData["EditId"] = id = sveId;
                    //            xmlData = SaveCustomXml(formdata, id, model.TabId);
                    //            clsRf.SaveXmlToDB(xmlData, id);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        if (Session["RefId"] != null)
                    //        {
                    //            sveId = Convert.ToInt32(Session["RefId"]);
                    //            TempData["EditId"] = id = sveId;
                    //            xmlData = SaveCustomXml(formdata, id, model.TabId);
                    //            clsRf.SaveXmlToDB(xmlData, id);
                    //            clsRf.SaveCurrentTab(id, "Tab7");
                    //            sess.SessTab7 = id;
                    //        }

                    //        // TempData["EditId"] = id = clsRf.SaveGeneralData(model, type, sveId, status);
                    //        //  sesClsObj.SessTab5 = id;
                    //    }

                    //    returnResult = RedirectToAction("SectionRedirect", new { tabID = model.TabId });
                    //}
                    //else returnResult = View();

                }
                else if (model.TabId == 8)
                {
                    if (Session["RefId"] != null)
                    {
                        //ClsErrorLog errlog = new ClsErrorLog();
                        //errlog.WriteToLog("sessRefId: " + sess.ReferralId);
                        //errlog.WriteToLog("status: " + status);
                        //errlog.WriteToLog("type: " + type);

                        if (sess.SessTab8 > 0)
                        {
                            //sveId = Convert.ToInt32(sesClsObj.SessTab8);
                            //if (Session["RefId"] != null)
                            //{
                            //    TempData["EditId"] = id = sveId;
                            //    xmlData = SaveCustomXml(formdata, id, model.TabId);
                            //    clsRf.SaveXmlToDB(xmlData, id);
                            //}
                        }
                        else
                        {
                            if (Session["RefId"] != null)
                            {
                                if (fileUpldDocName != null)
                                {
                                    sveId = Convert.ToInt32(Session["RefId"]);
                                    //errlog.WriteToLog("sveId: " + sveId);
                                    TempData["EditId"] = id = sveId;
                                    string Result = clsCommon.GetFileType(fileUpldDocName.ContentType);
                                    if (Result != "")
                                    {
                                        UploadDoccuments(id, model, fileUpldDocName, sess.SchoolId, sess.LoginId);
                                    }
                                    else
                                    {
                                        return Content("Invalid file format");
                                    }

                                    //   xmlData = SaveCustomXml(formdata, id, model.TabId);
                                    //   clsRf.SaveXmlToDB(xmlData, id);
                                    sess.SessTab8 = id;
                                }
                                else
                                {
                                    return Content("No file selected");
                                }
                            }

                            // TempData["EditId"] = id = clsRf.SaveGeneralData(model, type, sveId, status);
                            //  sesClsObj.SessTab5 = id;
                        }

                        returnResult = RedirectToAction("SectionRedirect", new { tabID = model.TabId });
                    }
                    else returnResult = View();
                }

                //  return View();
            }
            // return RedirectToAction("SectionRedirect", model);           

            return returnResult;
        }


        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public string XmlResultOld(GenInfoModel genModel, int Index)
        {
            // clsSessionTab sesClsObj = null;
            clsReferral objClsRef = new clsReferral();
            sess = (clsSession)Session["UserSession"];

            if (TempData["datatomethod"] != null)
            {
                genModel = (GenInfoModel)TempData["datatomethod"];
                //  sesClsObj = (clsSessionTab)Session["sesData"];
            }
            else if (TempData["LoadStudentData"] != null)
            {
                genModel = (GenInfoModel)TempData["LoadStudentData"];
                //sesClsObj = (clsSessionTab)Session["sesData"];
                if (sess != null)
                {
                    if (sess.ReferralId != 0)
                    {
                        TempData["EditId"] = Session["RefId"] = sess.ReferralId;

                    }
                }

            }
            int isUpated = 0;
            if (Index == 1 && sess != null)
            {
                if (sess.SessTab1 > 0) isUpated = 1; else isUpated = 0;

            }
            if (Index == 2 && sess != null)
            {
                if (sess.SessTab2 > 0) isUpated = 1; else isUpated = 0;

            }
            if (Index == 3 && sess != null)
            {
                if (sess.SessTab3 > 0) isUpated = 1; else isUpated = 0;

            }
            if (Index == 4 && sess != null)
            {
                if (sess.SessTab4 > 0) isUpated = 1; else isUpated = 0;

            }
            if (Index == 5 && sess != null)
            {
                if (sess.SessTab5 > 0) isUpated = 1; else isUpated = 0;

            }
            if (Index == 6 && sess != null)
            {
                if (sess.SessTab6 > 0) isUpated = 1; else isUpated = 0;

            }
            if (Index == 7 && sess != null)
            {
                if (sess.SessTab7 > 0) isUpated = 1; else isUpated = 0;

            }
            if (Index == 8 && sess != null)
            {
                if (sess.SessTab8 > 0) isUpated = 1; else isUpated = 0;

            }


            XmlDocument xdocCustom = new XmlDocument();
            xdocCustom.Load(Server.MapPath("~/XML/RefInfoCustom.xml"));
            XmlNodeList xSectionsCustom = xdocCustom.SelectNodes("/ReferralTemplate/Sections/Section[@name]");
            XmlNode xSectnCustom = xSectionsCustom[Index - 1];
            XmlNodeList rowsCust = xSectnCustom.ChildNodes[0].ChildNodes;



            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(Server.MapPath("~/XML/RefInfoDB.xml"));
            XmlNodeList xSections = xdoc.SelectNodes("/ReferralTemplate/Sections/Section[@name]");


            string html = "";
            int countCheck = 0;
            int widthCell = 23;
            int tdwidth = 10;
            // int formid = 0;
            XmlNode xSectn = xSections[Index - 1];
            //foreach (XmlNode xSectn in xSections)
            //{
            // formid++;
            string TableIds = "tblData" + Index;
            html += " <form method='post' id='frm" + Index + "' enctype='multipart/form-data' action='../GeneralInfo/SaveGeneralDataOld?type=" + xSectn.Attributes["name"].Value + "'><div><span class='headPanel'  onclick='getColapse(\"" + TableIds + "\")'>" + xSectn.Attributes["name"].Value + "</span><div class='divSubs'><table id='" + TableIds + "' class='clsTabDiv' style='float:left;width=100%;'>";
            XmlNodeList rows = xSectn.ChildNodes[0].ChildNodes;
            foreach (XmlNode row in rows)
            {
                html += "<tr style='width:100%;'>";
                foreach (XmlNode cell in row.ChildNodes)
                {
                    string id = cell.Attributes["id"].Value;

                    string type = cell.Attributes["type"].Value;
                    string label = cell.Attributes["label"].Value;
                    string colspan = cell.Attributes["colspan"].Value;
                    if (type == "label")
                        html += "<td class='tdtext' colspan=" + colspan + " style='width:" + tdwidth + "%;'>" + label + "</td>";

                    else if (type == "StudentPhoto")
                    {
                        string imagePath = "";

                        if (sess.ReferralId != 0)
                        {
                            imagePath = objClsRef.GetStudentImage(sess.ReferralId);
                            if (imagePath != null && imagePath != "")
                            {
                                html += "<td colspan=" + colspan + "><img id='imgStudPhoto' height='150px' width='150px' src=data:image/gif;base64," + imagePath + "></td>";
                            }
                        }
                    }

                    else if (type == "input[text]")
                    {
                        string maxLength = cell.Attributes["MaxLength"].Value;
                        string textValue = cell.Attributes["modelName"].Value;
                        string widthXml = cell.Attributes["width"] != null ? cell.Attributes["width"].Value : "";
                        if (widthXml != "")
                        {
                            widthXml = "width:" + widthXml;
                            widthXml += "% !important";
                        }
                        object value = null;
                        try
                        {
                            string[] split = textValue.Split('.');
                            if (split.Length > 1)
                            {
                                if (split[0] == "objRelationMthr")
                                    value = genModel.objRelationMthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationMthr, null);
                                if (split[0] == "objRelationFthr")
                                    value = genModel.objRelationFthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationFthr, null);
                                if (split[0] == "objRelationClose")
                                    if (genModel.objRelationClose != null) value = genModel.objRelationClose.GetType().GetProperty(split[1]).GetValue(genModel.objRelationClose, null);
                                if (split[0] == "objPhysicianDetails")
                                    if (genModel.objPhysicianDetails != null) value = genModel.objPhysicianDetails.GetType().GetProperty(split[1]).GetValue(genModel.objPhysicianDetails, null);
                                if (split[0] == "objInsuranceDetails")
                                    value = genModel.objInsuranceDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDetails, null);
                                if (split[0] == "objInsuranceSecDetails")
                                    value = genModel.objInsuranceSecDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceSecDetails, null);
                                if (split[0] == "objInsuranceDentalDetails")
                                    value = genModel.objInsuranceDentalDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDentalDetails, null);
                                if (split[0] == "objRelationLegalGuardian")
                                    value = genModel.objRelationLegalGuardian.GetType().GetProperty(split[1]).GetValue(genModel.objRelationLegalGuardian, null);
                                if (split[0] == "objRelationEmergncyContact")
                                    value = genModel.objRelationEmergncyContact.GetType().GetProperty(split[1]).GetValue(genModel.objRelationEmergncyContact, null);
                                if (split[0] == "objclsUpld")
                                    value = genModel.objclsUpld.GetType().GetProperty(split[1]).GetValue(genModel.objclsUpld, null);
                            }
                            else
                            {
                                value = genModel.GetType().GetProperty(textValue.ToString()).GetValue(genModel, null);
                            }
                        }
                        catch
                        {

                        }
                        finally
                        {
                        }

                        //html += "<td colspan=" + colspan + " style='width: "+widthCell+"%;'><span class='fontwhite'>*</span><input id='" + id + "' name='" + textValue.ToString() + "' value='" + value + "' maxlength='" + maxLength + "' type='text' /></td>";
                        html += "<td colspan=" + colspan + " style='width: " + widthCell + "%;'><span class='fontwhite'>*<span><input id='" + id + "' name='" + textValue.ToString() + "' style='" + widthXml + "' value='" + Server.HtmlEncode(Convert.ToString(value)) + "' maxlength='" + maxLength + "' type='text' /></td>";
                    }


                    else if (type == "input[textvalidate]")
                    {
                        string textValue = cell.Attributes["modelName"].Value;
                        string className = cell.Attributes["className"].Value;
                        string maxLength = cell.Attributes["MaxLength"].Value;
                        object value = null;
                        try
                        {

                            string[] split = textValue.Split('.');
                            if (split.Length > 1)
                            {
                                if (split[0] == "objRelationMthr")
                                    value = genModel.objRelationMthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationMthr, null);
                                if (split[0] == "objRelationFthr")
                                    value = genModel.objRelationFthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationFthr, null);
                                if (split[0] == "objRelationClose")
                                    if (genModel.objRelationClose != null) value = genModel.objRelationClose.GetType().GetProperty(split[1]).GetValue(genModel.objRelationClose, null);
                                if (split[0] == "objPhysicianDetails")
                                    if (genModel.objPhysicianDetails != null) value = genModel.objPhysicianDetails.GetType().GetProperty(split[1]).GetValue(genModel.objPhysicianDetails, null);
                                if (split[0] == "objInsuranceDetails")
                                    value = genModel.objInsuranceDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDetails, null);
                                if (split[0] == "objInsuranceSecDetails")
                                    value = genModel.objInsuranceSecDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceSecDetails, null);
                                if (split[0] == "objInsuranceDentalDetails")
                                    value = genModel.objInsuranceDentalDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDentalDetails, null);
                                if (split[0] == "objRelationLegalGuardian")
                                    value = genModel.objRelationLegalGuardian.GetType().GetProperty(split[1]).GetValue(genModel.objRelationLegalGuardian, null);
                                if (split[0] == "objRelationEmergncyContact")
                                    value = genModel.objRelationEmergncyContact.GetType().GetProperty(split[1]).GetValue(genModel.objRelationEmergncyContact, null);
                                if (split[0] == "objclsUpld")
                                    value = genModel.objclsUpld.GetType().GetProperty(split[1]).GetValue(genModel.objclsUpld, null);
                            }
                            else
                            {

                                value = genModel.GetType().GetProperty(textValue.ToString()).GetValue(genModel, null);
                            }
                        }
                        catch
                        {

                        }

                        if (className == "validate[required]")
                            html += "<td colspan=" + colspan + " style='width: " + widthCell + "%;'><span class='validate'>*<span><input id='" + id + "' name='" + textValue.ToString() + "' value='" + Server.HtmlEncode(Convert.ToString(value)) + "' class='" + className + "' maxlength='" + maxLength + "' type='text' /></td>";
                        else if (className == "validate[required] namefield")
                            html += "<td colspan=" + colspan + " style='width: " + widthCell + "%;'><span class='validate'>*<span><input id='" + id + "' name='" + textValue.ToString() + "' value='" + Server.HtmlEncode(Convert.ToString(value)) + "' class='" + className + "' maxlength='" + maxLength + "' type='text' onpaste='PreventDef(event)' /></td>";
                        else
                            html += "<td colspan=" + colspan + " style='width: " + widthCell + "%;'><span class='fontwhite'>*<span><input id='" + id + "' name='" + textValue.ToString() + "' value='" + Server.HtmlEncode(Convert.ToString(value)) + "' class='" + className + "' maxlength='" + maxLength + "' type='text' /></td>";
                    }

                    else if (type == "input[textfunction]")
                    {
                        string maxLength = cell.Attributes["MaxLength"].Value;
                        string textValue = cell.Attributes["modelName"].Value;
                        string funName = cell.Attributes["onkeypress"].Value;

                        //if (textValue == "RefZipCode")
                        //{
                        //    classname = "validate[required]";
                        //}

                        object value = null;
                        try
                        {

                            string[] split = textValue.Split('.');
                            if (split.Length > 1)
                            {
                                if (split[0] == "objRelationMthr")
                                    value = genModel.objRelationMthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationMthr, null);
                                if (split[0] == "objRelationFthr")
                                    value = genModel.objRelationFthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationFthr, null);
                                if (split[0] == "objRelationClose")
                                    if (genModel.objRelationClose != null) value = genModel.objRelationClose.GetType().GetProperty(split[1]).GetValue(genModel.objRelationClose, null);
                                if (split[0] == "objPhysicianDetails")
                                    if (genModel.objPhysicianDetails != null) value = genModel.objPhysicianDetails.GetType().GetProperty(split[1]).GetValue(genModel.objPhysicianDetails, null);
                                if (split[0] == "objInsuranceDetails")
                                    value = genModel.objInsuranceDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDetails, null);
                                if (split[0] == "objInsuranceSecDetails")
                                    value = genModel.objInsuranceSecDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceSecDetails, null);
                                if (split[0] == "objInsuranceDentalDetails")
                                    value = genModel.objInsuranceDentalDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDentalDetails, null);
                                if (split[0] == "objRelationLegalGuardian")
                                    value = genModel.objRelationLegalGuardian.GetType().GetProperty(split[1]).GetValue(genModel.objRelationLegalGuardian, null);
                                if (split[0] == "objRelationEmergncyContact")
                                    value = genModel.objRelationEmergncyContact.GetType().GetProperty(split[1]).GetValue(genModel.objRelationEmergncyContact, null);
                                if (split[0] == "objclsUpld")
                                    value = genModel.objclsUpld.GetType().GetProperty(split[1]).GetValue(genModel.objclsUpld, null);
                            }
                            else
                            {
                                value = genModel.GetType().GetProperty(textValue.ToString()).GetValue(genModel, null);
                            }
                        }
                        catch
                        {

                        }


                        if (textValue == "RefZipCode")
                        {
                            html += "<td colspan=" + colspan + " style='width: " + widthCell + "%;'><span class='fontwhite'>*<span><input id='" + id + "' name='" + textValue.ToString() + "' value='" + Server.HtmlEncode(Convert.ToString(value)) + "' maxlength='" + maxLength + "'  onkeypress='" + funName + "' type='text' /></td>";
                        }
                        else html += "<td colspan=" + colspan + " style='width: " + widthCell + "%;'><span class='fontwhite'>*<span><input id='" + id + "' name='" + textValue.ToString() + "' value='" + Server.HtmlEncode(Convert.ToString(value)) + "' maxlength='" + maxLength + "' onkeypress='" + funName + "' type='text' /></td>";


                    }

                    else if (type == "input[textKeyType]")
                    {
                        string maxLength = cell.Attributes["MaxLength"].Value;
                        string textValue = cell.Attributes["modelName"].Value;
                        string funName = cell.Attributes["onkeypress"].Value;
                        string keyName = cell.Attributes["key"].Value;
                        string width = cell.Attributes["width"].Value;
                        object value = null;
                        try
                        {
                            string[] split = textValue.Split('.');
                            if (split.Length > 1)
                            {
                                if (split[0] == "objRelationMthr")
                                    value = genModel.objRelationMthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationMthr, null);
                                if (split[0] == "objRelationFthr")
                                    value = genModel.objRelationFthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationFthr, null);
                                if (split[0] == "objRelationClose")
                                    if (genModel.objRelationClose != null) value = genModel.objRelationClose.GetType().GetProperty(split[1]).GetValue(genModel.objRelationClose, null);
                                if (split[0] == "objPhysicianDetails")
                                    if (genModel.objPhysicianDetails != null) value = genModel.objPhysicianDetails.GetType().GetProperty(split[1]).GetValue(genModel.objPhysicianDetails, null);
                                if (split[0] == "objInsuranceDetails")
                                    value = genModel.objInsuranceDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDetails, null);
                                if (split[0] == "objInsuranceSecDetails")
                                    value = genModel.objInsuranceSecDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceSecDetails, null);
                                if (split[0] == "objInsuranceDentalDetails")
                                    value = genModel.objInsuranceDentalDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDentalDetails, null);
                                if (split[0] == "objRelationLegalGuardian")
                                    value = genModel.objRelationLegalGuardian.GetType().GetProperty(split[1]).GetValue(genModel.objRelationLegalGuardian, null);
                                if (split[0] == "objRelationEmergncyContact")
                                    value = genModel.objRelationEmergncyContact.GetType().GetProperty(split[1]).GetValue(genModel.objRelationEmergncyContact, null);
                                if (split[0] == "objclsUpld")
                                    value = genModel.objclsUpld.GetType().GetProperty(split[1]).GetValue(genModel.objclsUpld, null);
                            }
                            else
                            {
                                value = genModel.GetType().GetProperty(textValue.ToString()).GetValue(genModel, null);
                            }
                        }
                        catch
                        {

                        }

                        html += "<td colspan=" + colspan + " style='width: " + widthCell + "%;'><span class='fontwhite'>*<span><input id='" + id + "' name='" + textValue.ToString() + "' value='" + Server.HtmlEncode(Convert.ToString(value)) + "' style='width: " + width + ";margin-right:0px;' maxlength='" + maxLength + "' onkeypress='" + funName + "' type='text' /><span style='color:#595959;'>" + keyName + "</span></td>";

                    }

                    else if (type == "input[multitext]")
                    {
                        string textValue = cell.Attributes["modelName"].Value;
                        string width = cell.Attributes["Width"].Value;
                        object value = null;
                        try
                        {
                            string[] split = textValue.Split('.');
                            if (split.Length > 1)
                            {
                                if (split[0] == "objRelationMthr")
                                    value = genModel.objRelationMthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationMthr, null);
                                if (split[0] == "objRelationFthr")
                                    value = genModel.objRelationFthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationFthr, null);
                                if (split[0] == "objRelationClose")
                                    if (genModel.objRelationClose != null) value = genModel.objRelationClose.GetType().GetProperty(split[1]).GetValue(genModel.objRelationClose, null);
                                if (split[0] == "objPhysicianDetails")
                                    if (genModel.objPhysicianDetails != null) value = genModel.objPhysicianDetails.GetType().GetProperty(split[1]).GetValue(genModel.objPhysicianDetails, null);
                                if (split[0] == "objInsuranceDetails")
                                    value = genModel.objInsuranceDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDetails, null);
                                if (split[0] == "objInsuranceSecDetails")
                                    value = genModel.objInsuranceSecDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceSecDetails, null);
                                if (split[0] == "objInsuranceDentalDetails")
                                    value = genModel.objInsuranceDentalDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDentalDetails, null);
                                if (split[0] == "objRelationLegalGuardian")
                                    value = genModel.objRelationLegalGuardian.GetType().GetProperty(split[1]).GetValue(genModel.objRelationLegalGuardian, null);

                                if (split[0] == "objRelationEmergncyContact")
                                    value = genModel.objRelationEmergncyContact.GetType().GetProperty(split[1]).GetValue(genModel.objRelationEmergncyContact, null);

                                if (split[0] == "objclsUpld")
                                    value = genModel.objclsUpld.GetType().GetProperty(split[1]).GetValue(genModel.objclsUpld, null);
                            }
                            else
                            {
                                value = genModel.GetType().GetProperty(textValue.ToString()).GetValue(genModel, null);
                            }
                        }
                        catch
                        {

                        }

                        html += "<td colspan=" + colspan + " style='width: " + widthCell + "%;'><span class='fontwhite'>*<span><textarea id='" + id + "' name='" + textValue.ToString() + "' value='" + Server.HtmlEncode(Convert.ToString(value)) + "' type='text' style='width:" + width + ";' rows='3'>" + Server.HtmlEncode(Convert.ToString(value)) + "</textarea></td>";
                    }
                    else if (type == "input[textDate]")
                    {
                        object value = null;
                        string textValue = cell.Attributes["modelName"].Value;

                        try
                        {
                            string[] split = textValue.Split('.');
                            if (split.Length > 1)
                            {
                                if (split[0] == "objRelationMthr")
                                    value = genModel.objRelationMthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationMthr, null);
                                if (split[0] == "objRelationFthr")
                                    value = genModel.objRelationFthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationFthr, null);
                                if (split[0] == "objRelationClose")
                                    if (genModel.objRelationClose != null) value = genModel.objRelationClose.GetType().GetProperty(split[1]).GetValue(genModel.objRelationClose, null);
                                if (split[0] == "objPhysicianDetails")
                                    if (genModel.objPhysicianDetails != null) value = genModel.objPhysicianDetails.GetType().GetProperty(split[1]).GetValue(genModel.objPhysicianDetails, null);
                                if (split[0] == "objInsuranceDetails")
                                    value = genModel.objInsuranceDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDetails, null);
                                if (split[0] == "objInsuranceSecDetails")
                                    value = genModel.objInsuranceSecDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceSecDetails, null);
                                if (split[0] == "objInsuranceDentalDetails")
                                    value = genModel.objInsuranceDentalDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDentalDetails, null);
                                if (split[0] == "objRelationLegalGuardian")
                                    value = genModel.objRelationLegalGuardian.GetType().GetProperty(split[1]).GetValue(genModel.objRelationLegalGuardian, null);
                                if (split[0] == "objRelationEmergncyContact")
                                    value = genModel.objRelationEmergncyContact.GetType().GetProperty(split[1]).GetValue(genModel.objRelationEmergncyContact, null);

                                if (split[0] == "objclsUpld")
                                    value = genModel.objclsUpld.GetType().GetProperty(split[1]).GetValue(genModel.objclsUpld, null);
                            }
                            else
                            {
                                value = genModel.GetType().GetProperty(textValue.ToString()).GetValue(genModel, null);
                            }
                        }
                        catch
                        {


                        }

                        if (textValue == "RefDate" || textValue == "RefDOB")
                        {
                            html += "<td colspan=" + colspan + " style='width: " + widthCell + "%;'><span class='validate'>*<span><input id='" + id + "' name='" + textValue.ToString() + "' class='validate[required] datepicker' onpaste='PreventDef(event)' onkeypress='return false' value='" + Server.HtmlEncode(Convert.ToString(value)) + "' type='text' /></td>";
                        }
                        else
                            html += "<td colspan=" + colspan + " style='width: " + widthCell + "%;'><span class='fontwhite'>*<span><input id='" + id + "' name='" + textValue.ToString() + "' class='datepicker' onpaste='PreventDef(event)'  onkeypress='return false' value='" + Server.HtmlEncode(Convert.ToString(value)) + "' type='text' /></td>";
                    }

                    //else if (type == "input[button]")
                    //{
                    //    if (isUpated == 1)
                    //    {
                    //        html += "<td colspan=" + colspan + " style='width: 20%; text-align:right;'><input id='" + id + "' value='Update' type='submit' class='NFButton' /></td>";
                    //    }
                    //    else
                    //    {
                    //        html += "<td colspan=" + colspan + " style='width: 20%; text-align:right;'><input id='" + id + "' value='" + label + "' type='submit' class='NFButton' /></td>";
                    //    }

                    //    //html += "<td colspan=" + colspan + " style='width: 20%; text-align:right;'><input id='" + id + "' value='" + (isUpated == 1 ? "update" : "save") + "' type='submit' class='NFButton' /></td>";
                    //}
                    else if (type == "input[file]")
                    {
                        string nameFile = cell.Attributes["name"].Value.ToString();

                        html += "<td colspan=" + colspan + " style='width: " + widthCell + "%;'><span class='fontwhite'>*<span><input id='" + id + "' name='" + nameFile + "' type='file' onchange ='selectFileImage(this);' /></td>";


                    }
                    else if (type == "input[check]")
                    {
                        countCheck = Convert.ToInt32(cell.Attributes["Count"].Value);
                        string[] idSplit = id.Split('|');
                        string[] lblSplit = label.Split('|');
                        html += "<td colspan=" + colspan + " style='width: " + widthCell + "%;'>";
                        for (int i = 0; i < countCheck; i++)
                        {
                            html += "<input id='" + idSplit[i] + "' type='checkbox'>" + lblSplit[i] + "</input><br>";
                        }
                        html += "</td>";

                    }
                    else if (type == "lblBold")
                    {
                        html += "<td class='lblBold' colspan=" + colspan + " style='width: 100%;'>" + label + "</td>";
                    }
                    else if (type == "input[checkVt]")
                    {
                        countCheck = Convert.ToInt32(cell.Attributes["Count"].Value);
                        string[] idSpl = id.Split('|');
                        string[] lblsplit = label.Split('|');
                        html += "<td colspan=" + colspan + " style='width:" + widthCell + "%;'>";
                        for (int i = 0; i < countCheck; i++)
                        {
                            html += "<input id='" + idSpl[i] + "' type='checkbox'>" + lblsplit[i] + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</input>";
                        }
                        html += "</td>";

                    }


                    else if (type == "input[radio]")
                    {
                        IEnumerable<SelectListItem> listMar = new List<SelectListItem>();
                        if (label == "MaritalStatus")
                        {
                            string textValue = cell.Attributes["modelName"].Value;
                            listMar = objClsRef.FillMaritalStatus();
                            if (textValue != "")
                            {
                                html += "<td colspan=" + colspan + " style='" + widthCell + "%;'>";
                                foreach (SelectListItem item in listMar)
                                {
                                    if (genModel.objRelationMthr != null)
                                    {
                                        if (genModel.objRelationMthr.txtMaritalStatus.ToString() == item.Value)
                                        {
                                            html += "<input id='" + item.Value + "' type='radio' value='" + item.Value + "' name='" + textValue + "' checked><label>" + item.Text + "</label></input>";
                                        }
                                        else
                                        {
                                            html += "<input id='" + item.Value + "' type='radio' value='" + item.Value + "' name='" + textValue + "'><label>" + item.Text + "</label></input>";
                                        }
                                    }
                                }
                                html += "</td>";
                            }
                            else
                            {
                                html += "<td colspan=" + colspan + " style='width: " + widthCell + "%;'>";

                                foreach (SelectListItem item in listMar)
                                {
                                    html += "<input id='" + item.Value + "' type='radio' value='" + item.Value + "' name='" + textValue + "'><label>" + item.Text + "</label></input>";
                                }
                                html += "</td>";
                            }

                        }

                    }


                    else if (type == "input[dropRel]")
                    {
                        IEnumerable<SelectListItem> ListData = new List<SelectListItem>();

                        if (label == "Relation Status")
                        {
                            ListData = objClsRef.FillDropList("Relationship");
                            string textValue = cell.Attributes["modelName"].Value;
                            html += "<td colspan=" + colspan + " style='width: " + widthCell + "%;'><span class='fontwhite'>*<span><select id='" + id + "' name='" + textValue.ToString() + "' class=''>";
                            html += "<option value=''>Select " + label + "</option>";

                            foreach (SelectListItem Item in ListData)
                            {
                                if (textValue == "objRelationClose.closeRelation")
                                {
                                    if (genModel.objRelationClose.closeRelation.ToString() == Item.Value)
                                    {
                                        html += "<option value='" + Item.Value + "' selected='true'>" + Item.Text + "</option>";
                                    }
                                    else
                                    {
                                        html += "<option value='" + Item.Value + "'>" + Item.Text + "</option>";
                                    }
                                }
                            }

                        }
                    }


                    else if (type == "input[dropDocType]")
                    {
                        IEnumerable<SelectListItem> ListData = new List<SelectListItem>();

                        if (label == "DocumentType")
                        {
                            ListData = objClsRef.FillDoccumentType();
                            string textValue = cell.Attributes["modelName"].Value;
                            html += "<td colspan=" + colspan + " style='width: " + widthCell + "%;'><span class='validate'>*<span><select id='" + id + "' name='" + textValue.ToString() + "' class='validate[required]'>";
                            html += "<option value='' selected='true'>Select " + label + "</option>";
                            foreach (SelectListItem Item in ListData)
                            {
                                if (textValue == "objclsUpld.DocType")
                                {
                                    if (genModel.objclsUpld.DocType.ToString() != null)
                                    {
                                        html += "<option value='" + Item.Value + "'>" + Item.Text + "</option>";
                                    }
                                    else
                                    {
                                        html += "<option value='" + Item.Value + "'>" + Item.Text + "</option>";
                                    }
                                }
                            }

                        }
                    }

                    else if (type == "input[drop]")
                    {
                        IEnumerable<SelectListItem> listData = new List<SelectListItem>();
                        IEnumerable<SelectListItem> listDataState = new List<SelectListItem>();
                        if (label == "Country")
                        {
                            listData = objClsRef.FillDropList("Country");
                        }

                        if (label == "Country")
                        {
                            string textValue = cell.Attributes["modelName"].Value;
                            string stateVal = "0";

                            if (textValue == "RefCountry")
                            {
                                html += "<td colspan=" + colspan + " style='width: " + widthCell + "%;'><span class='fontwhite'>*<span><select id='" + id + "' name='" + textValue.ToString() + "' class='ddlRefCountry' >";// onchange='GetState(this.id," + stateVal + ")'
                                //html += "<option value=''>Select " + label + "</option>";
                            }
                            else
                            {
                                html += "<td colspan=" + colspan + " style='width: " + widthCell + "%;'><span class='fontwhite'>*<span><select id='" + id + "' name='" + textValue.ToString() + "' class='ddlOtherCountry' >";//onchange='GetState(this.id," + stateVal + ")'
                                //html += "<option value=0>Select " + label + "</option>";
                            }



                            foreach (SelectListItem list in listData)
                            {

                                if (textValue == "objRelationMthr.txtCountry")
                                {
                                    if (genModel.objRelationMthr != null)
                                    {
                                        if (genModel.objRelationMthr.txtCountry.ToString() == list.Value)
                                        {
                                            html += "<option value='" + list.Value + "' selected='true'>" + list.Text + "</option>";

                                        }
                                        else
                                        {
                                            html += "<option value='" + list.Value + "'>" + list.Text + "</option>";
                                        }
                                    }
                                    else
                                    {
                                        html += "<option value='" + list.Value + "'>" + list.Text + "</option>";
                                    }
                                }
                                else if (textValue == "objRelationFthr.txtCountry")
                                {
                                    if (genModel.objRelationFthr.txtCountry != null)
                                    {
                                        if (genModel.objRelationFthr.txtCountry.ToString() == list.Value)
                                        {
                                            html += "<option value='" + list.Value + "' selected='true'>" + list.Text + "</option>";
                                        }
                                        else
                                        {
                                            html += "<option value='" + list.Value + "'>" + list.Text + "</option>";
                                        }
                                    }
                                    else
                                    {
                                        html += "<option value='" + list.Value + "'>" + list.Text + "</option>";
                                    }
                                }
                                else if (textValue == "objRelationClose.txtCountry")
                                {
                                    if (genModel.objRelationClose != null)
                                    {
                                        if (genModel.objRelationClose.txtCountry != null)
                                        {
                                            if (genModel.objRelationClose.txtCountry.ToString() == list.Value)
                                            {
                                                html += "<option value='" + list.Value + "' selected='true'>" + list.Text + "</option>";
                                            }
                                            else
                                            {
                                                html += "<option value='" + list.Value + "'>" + list.Text + "</option>";
                                            }
                                        }
                                        else
                                        {
                                            html += "<option value='" + list.Value + "'>" + list.Text + "</option>";
                                        }
                                    }
                                }
                                else if (textValue == "objPhysicianDetails.txtCountry")
                                {
                                    if (genModel.objPhysicianDetails != null)
                                    {
                                        //if (genModel.objPhysicianDetails.txtCountry != null)
                                        //{
                                        if (genModel.objPhysicianDetails.txtCountry.ToString() == list.Value)
                                        {
                                            html += "<option value='" + list.Value + "' selected='true'>" + list.Text + "</option>";
                                        }
                                        else
                                        {
                                            html += "<option value='" + list.Value + "'>" + list.Text + "</option>";
                                        }
                                        //}
                                    }
                                    else
                                    {
                                        html += "<option value='" + list.Value + "'>" + list.Text + "</option>";
                                    }
                                }
                                else if (textValue == "objInsuranceDetails.txtCountry")
                                {
                                    if (genModel.objInsuranceDetails != null)
                                    {
                                        //if (genModel.objInsuranceDetails.txtCountry != null)
                                        //{
                                        if (genModel.objInsuranceDetails.txtCountry.ToString() == list.Value)
                                        {
                                            html += "<option value='" + list.Value + "' selected='true'>" + list.Text + "</option>";
                                        }
                                        else
                                        {
                                            html += "<option value='" + list.Value + "'>" + list.Text + "</option>";
                                        }
                                        // }
                                    }
                                    else
                                    {
                                        html += "<option value='" + list.Value + "'>" + list.Text + "</option>";
                                    }
                                }
                                else if (textValue == "objInsuranceSecDetails.txtCountry")
                                {
                                    if (genModel.objInsuranceDetails != null)
                                    {
                                        //if (genModel.objInsuranceSecDetails.txtCountry != null)
                                        //{
                                        if (genModel.objInsuranceSecDetails.txtCountry.ToString() == list.Value)
                                        {
                                            html += "<option value='" + list.Value + "' selected='true'>" + list.Text + "</option>";
                                        }
                                        else
                                        {
                                            html += "<option value='" + list.Value + "'>" + list.Text + "</option>";
                                        }
                                        //}
                                    }
                                    else
                                    {
                                        html += "<option value='" + list.Value + "'>" + list.Text + "</option>";
                                    }
                                }
                                else if (textValue == "objInsuranceDentalDetails.txtCountry")
                                {
                                    if (genModel.objInsuranceDetails != null)
                                    {
                                        //if (genModel.objInsuranceDentalDetails.txtCountry != null)
                                        //{
                                        if (genModel.objInsuranceDentalDetails.txtCountry.ToString() == list.Value)
                                        {
                                            html += "<option value='" + list.Value + "' selected='true'>" + list.Text + "</option>";
                                        }
                                        else
                                        {
                                            html += "<option value='" + list.Value + "'>" + list.Text + "</option>";
                                        }
                                        // }
                                    }
                                    else
                                    {
                                        html += "<option value='" + list.Value + "'>" + list.Text + "</option>";
                                    }
                                }

                                else if (textValue == "objRelationLegalGuardian.txtCountry")
                                {
                                    if (genModel.objRelationLegalGuardian.txtCountry != null)
                                    {
                                        if (genModel.objRelationLegalGuardian.txtCountry.ToString() == list.Value)
                                        {
                                            html += "<option value='" + list.Value + "' selected='true'>" + list.Text + "</option>";
                                        }
                                        else
                                        {
                                            html += "<option value='" + list.Value + "'>" + list.Text + "</option>";
                                        }
                                    }
                                    else
                                    {
                                        html += "<option value='" + list.Value + "'>" + list.Text + "</option>";
                                    }
                                }
                                else if (textValue == "objRelationEmergncyContact.txtCountry")
                                {
                                    if (genModel.objRelationEmergncyContact.txtCountry != null)
                                    {
                                        if (genModel.objRelationEmergncyContact.txtCountry.ToString() == list.Value)
                                        {
                                            html += "<option value='" + list.Value + "' selected='true'>" + list.Text + "</option>";
                                        }
                                        else
                                        {
                                            html += "<option value='" + list.Value + "'>" + list.Text + "</option>";
                                        }
                                    }
                                    else
                                    {
                                        html += "<option value='" + list.Value + "'>" + list.Text + "</option>";
                                    }
                                }
                                else if (textValue == "RefCntryBirth")
                                {
                                    if (genModel.RefCntryBirth != null)
                                    {
                                        if (genModel.RefCntryBirth.ToString() == list.Value)
                                        {
                                            html += "<option value='" + list.Value + "' selected='true'>" + list.Text + "</option>";
                                        }
                                        else
                                        {
                                            html += "<option value='" + list.Value + "'>" + list.Text + "</option>";
                                        }
                                    }
                                    else
                                    {
                                        html += "<option value='" + list.Value + "'>" + list.Text + "</option>";
                                    }
                                }
                                else
                                {
                                    if (genModel.RefCountry.ToString() == list.Value)
                                    {
                                        html += "<option value='" + list.Value + "' selected='true'>" + list.Text + "</option>";

                                    }
                                    else
                                    {
                                        html += "<option value='" + list.Value + "'>" + list.Text + "</option>";
                                    }
                                }
                            }

                            html += "</select></td>";
                        }
                        else if (label == "State")
                        {

                            string textValue = cell.Attributes["modelName"].Value;

                            if (textValue == "RefState")
                            {
                                html += "<td colspan=" + colspan + " style='width: " + widthCell + "%;'><span class='fontwhite'>*<span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
                                html += "<option value=0>Select " + label + "</option>";
                                int countryId = Convert.ToInt32(genModel.RefCountry);
                                listDataState = objClsRef.FillState(countryId);

                                foreach (SelectListItem listSt in listDataState)
                                {
                                    if (textValue == "RefState")
                                    {
                                        if (genModel.RefState.ToString() == listSt.Value)
                                        {
                                            html += "<option value='" + listSt.Value + "' selected='true'>" + listSt.Text + "</option>";
                                        }
                                        else
                                        {
                                            html += "<option value='" + listSt.Value + "'>" + listSt.Text + "</option>";
                                        }
                                    }
                                }
                                html += "</select></td>";
                            }

                            else if (textValue == "objRelationMthr.txtState")
                            {
                                int countryId = Convert.ToInt32(genModel.objRelationMthr.txtCountry);
                                listDataState = objClsRef.FillState(countryId);
                                html += "<td colspan=" + colspan + " style='width: " + widthCell + "%;'><span class='fontwhite'>*<span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
                                html += "<option value=0>Select " + label + "</option>";
                                foreach (SelectListItem listSt in listDataState)
                                {
                                    if (textValue == "objRelationMthr.txtState")
                                    {
                                        if (genModel.objRelationMthr.txtState.ToString() == listSt.Value)
                                        {
                                            html += "<option value='" + listSt.Value + "' selected='true'>" + listSt.Text + "</option>";
                                        }
                                        else
                                        {
                                            html += "<option value='" + listSt.Value + "'>" + listSt.Text + "</option>";
                                        }
                                    }
                                }
                                html += "</select></td>";
                            }

                            else if (textValue == "objRelationFthr.txtState")
                            {
                                int countryId = Convert.ToInt32(genModel.objRelationFthr.txtCountry);
                                listDataState = objClsRef.FillState(countryId);
                                html += "<td colspan=" + colspan + " style='width: " + widthCell + "%;'><span class='fontwhite'>*<span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
                                html += "<option value=0>Select " + label + "</option>";
                                foreach (SelectListItem listSt in listDataState)
                                {
                                    if (textValue == "objRelationFthr.txtState")
                                    {
                                        if (genModel.objRelationFthr.txtState.ToString() == listSt.Value)
                                        {
                                            html += "<option value='" + listSt.Value + "' selected='true'>" + listSt.Text + "</option>";
                                        }
                                        else
                                        {
                                            html += "<option value='" + listSt.Value + "'>" + listSt.Text + "</option>";
                                        }
                                    }
                                }
                                html += "</select></td>";
                            }

                            else if (textValue == "objRelationClose.txtState")
                            {
                                if (genModel.objRelationClose != null)
                                {
                                    int countryId = Convert.ToInt32(genModel.objRelationClose.txtCountry);
                                    listDataState = objClsRef.FillState(countryId);
                                    html += "<td colspan=" + colspan + " style='width: " + widthCell + "%;'><span style='color: white;'>*<span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
                                    html += "<option value=0>Select " + label + "</option>";
                                    foreach (SelectListItem listSt in listDataState)
                                    {
                                        if (textValue == "objRelationClose.txtState")
                                        {
                                            if (genModel.objRelationClose.txtState.ToString() == listSt.Value)
                                            {
                                                html += "<option value='" + listSt.Value + "' selected='true'>" + listSt.Text + "</option>";
                                            }
                                            else
                                            {
                                                html += "<option value='" + listSt.Value + "'>" + listSt.Text + "</option>";
                                            }
                                        }
                                    }
                                }
                                html += "</select></td>";
                            }

                            else if (textValue == "objRelationLegalGuardian.txtState")
                            {
                                int countryId = Convert.ToInt32(genModel.objRelationLegalGuardian.txtCountry);
                                listDataState = objClsRef.FillState(countryId);
                                html += "<td colspan=" + colspan + " style='width: " + widthCell + "%;'><span class='fontwhite'>*<span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
                                html += "<option value=0>Select " + label + "</option>";
                                foreach (SelectListItem listSt in listDataState)
                                {
                                    if (textValue == "objRelationLegalGuardian.txtState")
                                    {
                                        if (genModel.objRelationLegalGuardian.txtState.ToString() == listSt.Value)
                                        {
                                            html += "<option value='" + listSt.Value + "' selected='true'>" + listSt.Text + "</option>";
                                        }
                                        else
                                        {
                                            html += "<option value='" + listSt.Value + "'>" + listSt.Text + "</option>";
                                        }
                                    }
                                }
                                html += "</select></td>";
                            }

                            else if (textValue == "objRelationEmergncyContact.txtState")
                            {
                                int countryId = Convert.ToInt32(genModel.objRelationEmergncyContact.txtCountry);
                                listDataState = objClsRef.FillState(countryId);
                                html += "<td colspan=" + colspan + " style='width:" + widthCell + "%;'><span class='fontwhite'>*<span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
                                html += "<option value=0>Select " + label + "</option>";
                                foreach (SelectListItem listSt in listDataState)
                                {
                                    if (textValue == "objRelationEmergncyContact.txtState")
                                    {
                                        if (genModel.objRelationEmergncyContact.txtState.ToString() == listSt.Value)
                                        {
                                            html += "<option value='" + listSt.Value + "' selected='true'>" + listSt.Text + "</option>";
                                        }
                                        else
                                        {
                                            html += "<option value='" + listSt.Value + "'>" + listSt.Text + "</option>";
                                        }
                                    }
                                }
                                html += "</select></td>";
                            }

                            else if (textValue == "RefStateBirth")
                            {
                                int countryId = Convert.ToInt32(genModel.RefCntryBirth);
                                listDataState = objClsRef.FillState(countryId);
                                html += "<td colspan=" + colspan + " style='width: " + widthCell + "%;'><span class='fontwhite'>*<span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
                                html += "<option value=0>Select " + label + "</option>";
                                foreach (SelectListItem listSt in listDataState)
                                {
                                    if (textValue == "RefStateBirth")
                                    {
                                        if (genModel.RefStateBirth.ToString() == listSt.Value)
                                        {
                                            html += "<option value='" + listSt.Value + "' selected='true'>" + listSt.Text + "</option>";
                                        }
                                        else
                                        {
                                            html += "<option value='" + listSt.Value + "'>" + listSt.Text + "</option>";
                                        }
                                    }
                                }
                                html += "</select></td>";
                            }

                            else if (textValue == "objPhysicianDetails.txtState")
                            {
                                int countryId = 0;
                                if (genModel.objPhysicianDetails != null) countryId = Convert.ToInt32(genModel.objPhysicianDetails.txtCountry);
                                if (countryId != 0) listDataState = objClsRef.FillState(countryId);
                                html += "<td colspan=" + colspan + " style='width: " + widthCell + "%;'><span class='fontwhite'>*<span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
                                html += "<option value=0>Select " + label + "</option>";
                                foreach (SelectListItem listSt in listDataState)
                                {
                                    if (textValue == "objPhysicianDetails.txtState")
                                    {
                                        if (genModel.objPhysicianDetails.txtState.ToString() == listSt.Value)
                                        {
                                            html += "<option value='" + listSt.Value + "' selected='true'>" + listSt.Text + "</option>";
                                        }
                                        else
                                        {
                                            html += "<option value='" + listSt.Value + "'>" + listSt.Text + "</option>";
                                        }
                                    }
                                }
                                html += "</select></td>";
                            }


                            else if (textValue == "objInsuranceDetails.txtState")
                            {
                                int countryId = 0;
                                if (genModel.objInsuranceDetails != null) countryId = Convert.ToInt32(genModel.objInsuranceDetails.txtCountry);
                                listDataState = objClsRef.FillState(countryId);
                                html += "<td colspan=" + colspan + " style='width: " + widthCell + "%;'><span class='fontwhite'>*<span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
                                html += "<option value=0>Select " + label + "</option>";
                                foreach (SelectListItem listSt in listDataState)
                                {
                                    if (textValue == "objInsuranceDetails.txtState")
                                    {
                                        if (genModel.objInsuranceDetails.txtState.ToString() == listSt.Value)
                                        {
                                            html += "<option value='" + listSt.Value + "' selected='true'>" + listSt.Text + "</option>";
                                        }
                                        else
                                        {
                                            html += "<option value='" + listSt.Value + "'>" + listSt.Text + "</option>";
                                        }
                                    }
                                }
                                html += "</select></td>";
                            }

                            else if (textValue == "objInsuranceSecDetails.txtState")
                            {
                                int countryId = 0;
                                if (genModel.objInsuranceSecDetails != null) countryId = Convert.ToInt32(genModel.objInsuranceSecDetails.txtCountry);
                                listDataState = objClsRef.FillState(countryId);
                                html += "<td colspan=" + colspan + " style='width: " + widthCell + "%;'><span class='fontwhite'>*<span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
                                html += "<option value=0>Select " + label + "</option>";
                                foreach (SelectListItem listSt in listDataState)
                                {
                                    if (textValue == "objInsuranceSecDetails.txtState")
                                    {
                                        if (genModel.objInsuranceSecDetails.txtState.ToString() == listSt.Value)
                                        {
                                            html += "<option value='" + listSt.Value + "' selected='true'>" + listSt.Text + "</option>";
                                        }
                                        else
                                        {
                                            html += "<option value='" + listSt.Value + "'>" + listSt.Text + "</option>";
                                        }
                                    }
                                }
                                html += "</select></td>";
                            }


                            else if (textValue == "objInsuranceDentalDetails.txtState")
                            {
                                int countryId = 0;
                                if (genModel.objInsuranceSecDetails != null) countryId = Convert.ToInt32(genModel.objInsuranceDentalDetails.txtCountry);
                                listDataState = objClsRef.FillState(countryId);
                                html += "<td colspan=" + colspan + " style='width: " + widthCell + "%;'><span class='fontwhite'>*<span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
                                html += "<option value=0>Select " + label + "</option>";
                                foreach (SelectListItem listSt in listDataState)
                                {
                                    if (textValue == "objInsuranceDentalDetails.txtState")
                                    {
                                        if (genModel.objInsuranceDentalDetails.txtState.ToString() == listSt.Value)
                                        {
                                            html += "<option value='" + listSt.Value + "' selected='true'>" + listSt.Text + "</option>";
                                        }
                                        else
                                        {
                                            html += "<option value='" + listSt.Value + "'>" + listSt.Text + "</option>";
                                        }
                                    }
                                }
                                html += "</select></td>";
                            }

                            else
                            {
                                html += "<td colspan=" + colspan + " style='width: " + widthCell + "%;'><span class='fontwhite'>*<span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
                                html += "<option value=0>Select " + label + "</option>";
                                html += "</select></td>";
                            }



                        }
                        else if (label == "Gender")
                        {
                            string textValue = cell.Attributes["modelName"].Value;

                            html += "<td colspan=" + colspan + " style='width: " + widthCell + "%;'><span class='validate'>*<span><select id='" + id + "' name='" + textValue.ToString() + "' class='validate[required]'>";
                            if (genModel.RefGender == "1")
                            {
                                html += "<option value=''>Select Gender</option><option selected='true' value=1>Male</option><option value=2>Female</option>";
                            }
                            else if (genModel.RefGender == "2")
                            {
                                html += "<option value=''>Select Gender</option><option value=1>Male</option><option selected='true' value=2>Female</option>";
                            }
                            else
                            {
                                html += "<option value=''>Select Gender</option><option value=1>Male</option><option value=2>Female</option>";
                            }


                            html += "</select></td>";

                        }
                        else
                        {
                            string textValue = cell.Attributes["modelName"].Value;

                            html += "<td colspan=" + colspan + " style='width: " + widthCell + "%;'><span class='fontwhite'>*<span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass2'>";
                            html += "<option value=0>Select " + label + "</option>";
                            html += "</select></td>";
                        }
                    }

                    else if (type == "td")
                    {
                        html += "<td colspan=" + colspan + " style='width:" + tdwidth + "%;'></td>";
                    }


                }
                html += "</tr>";
            }

            if (TempData["EditId"] == null)
            {

                foreach (XmlNode rowcust in rowsCust)
                {
                    html += "<tr>";
                    foreach (XmlNode cellCust in rowcust.ChildNodes)
                    {
                        string id = cellCust.Attributes["id"].Value;
                        string type = cellCust.Attributes["type"].Value;
                        string label = cellCust.Attributes["label"].Value;
                        string colspan = cellCust.Attributes["colspan"].Value;
                        string widthXml = cellCust.Attributes["width"] != null ? cellCust.Attributes["width"].Value : "";
                        if (widthXml != "")
                        {
                            widthXml = "width:" + widthXml;
                            widthXml += "% !important";
                        }
                        if (type == "label")
                            html += "<td class='tdtext' colspan=" + colspan + " style='width: 10%;'>" + label + "</td>";
                        else if (type == "input[text]")
                        {
                            //html += "<td colspan=" + colspan + " style='width: " + widthCell + "%;'><span class='fontwhite'>*<span><input id='" + id + "' name='" + id + "' type='text' /></td>";
                            html += "<td colspan=" + colspan + " style='width: " + widthCell + "%;'><span class='fontwhite'>*<span><input id='" + id + "'  style='" + widthXml + "' name='" + id + "' type='text' /></td>";

                        }
                        else if (type == "input[textfunction]")
                        {
                            string maxLength = cellCust.Attributes["Maxlength"].Value;
                            string funName = cellCust.Attributes["onkeypress"].Value;
                            html += "<td colspan=" + colspan + " style='width: " + widthCell + "%;'><span class='fontwhite'>*<span><input id='" + id + "' name='" + id + "' maxlength='" + maxLength + "' onkeypress='" + funName + "' type='text' /></td>";
                        }

                        else if (type == "input[textvalidate]")
                        {
                            string className = cellCust.Attributes["className"].Value;
                            html += "<td colspan=" + colspan + " style='width: " + widthCell + "%;'><span class='fontwhite'>*<span><input id='" + id + "' name='" + id + "' class='" + className + "' type='text' /></td>";
                        }

                        else if (type == "input[textdate]")
                        {
                            html += "<td colspan=" + colspan + " style='width: " + widthCell + "%;'><span class='fontwhite'>*<span><input id='" + id + "' onkeypress='return false' name='" + id + "' type='text' class='datepicker' onpaste='PreventDef(event)'  /></td>";
                        }

                        else if (type == "input[multitext]")
                        {
                            string width = cellCust.Attributes["Width"].Value;
                            html += "<td colspan=" + colspan + " style='width: " + width + "%;'><span class='fontwhite'>*<span><textarea id='" + id + "' name='" + id + "' type='text' style='width:" + width + ";' rows='3'></textarea></td>";
                        }
                        else if (type == "input[checkVt]")
                        {

                            countCheck = Convert.ToInt32(cellCust.Attributes["Count"].Value);
                            string[] idSpl = id.Split('|');
                            string[] lblsplit = label.Split('|');
                            html += "<td colspan=" + colspan + " style='width:" + widthCell + "%;'>";
                            for (int i = 0; i < countCheck; i++)
                            {
                                html += "<input id='" + idSpl[i] + "' type='checkbox' value='unchecked' name='" + idSpl[i] + "' onclick='GetSelected(this);' !checked><span>" + lblsplit[i] + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>";
                            }
                            html += "</td>";

                        }

                        else if (type == "input[drop]")
                        {
                            countCheck = Convert.ToInt32(cellCust.Attributes["Count"].Value);
                            string[] lblsplit = label.Split('|');
                            if (label == "Early Childhood (Day School, ages 3-10)|School Age (Day School, ages 11-22)|Residential")
                            {
                                html += "<td colspan=" + colspan + " style='width:" + widthCell + "%;'><span class='fontwhite'>*<span><select id='" + id + "' name='" + id + "' class='drpClass1'><option value=0>-----Select.....</option>";
                                for (int i = 0; i < countCheck; i++)
                                {
                                    html += "<option value='" + lblsplit[i] + "'>" + lblsplit[i] + "</option>";

                                    //html += "<input id='" + id + "' type='checkbox' value='unchecked' name='" + idSpl[i] + "' onclick='GetSelected(this);' !checked><span>" + lblsplit[i] + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>";

                                }
                                html += "</select></td>";

                            }
                            else if (id == "drpControl" && countCheck == 4)
                            {
                                html += "<td colspan=" + colspan + " style='width:" + widthCell + "%;'><span class='fontwhite'>*<span><select id='" + id + "' name='" + id + "' class='drpClass3'><option value=0>-----Select.....</option>";
                                for (int i = 0; i < countCheck; i++)
                                {
                                    html += "<option value='" + lblsplit[i] + "'>" + lblsplit[i] + "</option>";

                                    //html += "<input id='" + id + "' type='checkbox' value='unchecked' name='" + idSpl[i] + "' onclick='GetSelected(this);' !checked><span>" + lblsplit[i] + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>";

                                }
                                html += "</select></td>";
                            }
                            else
                            {
                                {
                                    html += "<td colspan=" + colspan + " style='width:" + widthCell + "%;'><span class='fontwhite'>*<span><select id='" + id + "' name='" + id + "' class='drpClass3'><option value=0>-----Select.....</option>";
                                    for (int i = 0; i < countCheck; i++)
                                    {
                                        html += "<option value='" + lblsplit[i] + "'>" + lblsplit[i] + "</option>";

                                        //html += "<input id='" + id + "' type='checkbox' value='unchecked' name='" + idSpl[i] + "' onclick='GetSelected(this);' !checked><span>" + lblsplit[i] + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>";

                                    }
                                    html += "</select></td>";
                                }

                            }

                        }

                        //else if (type == "input[radiosingleselect]")
                        //{
                        //    string name = cellCust.Attributes["name"].Value.ToString();
                        //    countCheck = Convert.ToInt32(cellCust.Attributes["Count"].Value);
                        //    string[] idSpl = id.Split('|');
                        //    string[] lblsplit = label.Split('|');
                        //    html += "<td colspan=" + colspan + " style='width:20%;'>";
                        //    for (int i = 0; i < countCheck; i++)
                        //    {
                        //        html += "<input id='" + idSpl[i] + "' type='radio' value='unchecked' name='" + name + "' onclick='GetSelected(this);' !checked><span>" + lblsplit[i] + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>";
                        //    }
                        //    html += "</td>";

                        //}

                        else if (type == "input[checkNolbl]")
                        {

                            countCheck = Convert.ToInt32(cellCust.Attributes["Count"].Value);
                            string[] idSpl = id.Split('|');
                            string[] lblsplit = label.Split('|');
                            html += "<td colspan=" + colspan + " style='width:" + widthCell + "%;'>";
                            for (int i = 0; i < countCheck; i++)
                            {
                                html += "<input id='" + idSpl[i] + "' type='checkbox' value='unchecked' name='" + idSpl[i] + "' onclick='GetSelected(this);' !checked>";
                            }
                            html += "</td>";

                        }
                        else if (type == "input[file]")
                        {
                            string nameFile = cellCust.Attributes["name"].Value.ToString();
                            html += "<td colspan=" + colspan + " style='width: " + widthCell + "%;'><span class='validate'>*</span><input id='" + id + "' name='" + nameFile + "' type='file'/></td>";
                        }
                        else if (type == "input[check]")
                        {
                            countCheck = Convert.ToInt32(cellCust.Attributes["Count"].Value);
                            string[] idSplit = id.Split('|');
                            string[] lblSplit = label.Split('|');
                            html += "<td colspan=" + colspan + " style='width: " + widthCell + "%;'>";
                            for (int i = 0; i < countCheck; i++)
                            {
                                html += "<input id='" + idSplit[i] + "' type='checkbox'>" + lblSplit[i] + "</input><br>";
                            }
                            html += "</td>";

                        }
                        else if (type == "lblBold")
                        {
                            html += "<td class='lblBold' colspan=" + colspan + " style='width: " + tdwidth + "%;'>" + label + "</td>";
                        }
                        else if (type == "td")
                        {
                            html += "<td colspan=" + colspan + " style='width:" + tdwidth + "%;'></td>";
                        }


                    }
                    html += "</tr>";
                }
            }
            else
            {
                //if (Index == 5) isUpated = 1;
                //if (Index == 6) isUpated = 1;
                //if (Index == 7) isUpated = 1; 



                XmlDocument xdocCustomEdit = new XmlDocument();
                int studentId = Convert.ToInt32(TempData["EditId"]);
                xdocCustomEdit = objClsRef.LoadXmlfromBlob(studentId);

                // xdocCustomEdit.Load(Server.MapPath("~/XML/Data.xml"));
                XmlNodeList xSectionsCustomEdit = xdocCustomEdit.SelectNodes("/ReferralTemplate/Sections/Section[@name]");
                XmlNode xSectnCustomEdit = xSectionsCustomEdit[Index - 1];
                XmlNodeList rowsCustEdit = null;
                if (xSectnCustomEdit != null)
                {
                    rowsCustEdit = xSectnCustomEdit.ChildNodes[0].ChildNodes;

                    foreach (XmlNode rowcustEdit in rowsCustEdit)
                    {
                        html += "<tr>";
                        foreach (XmlNode cellCustEdit in rowcustEdit.ChildNodes)
                        {
                            string id = cellCustEdit.Attributes["id"].Value;
                            string type = cellCustEdit.Attributes["type"].Value;
                            string label = cellCustEdit.Attributes["label"].Value;
                            string colspan = cellCustEdit.Attributes["colspan"].Value;
                            if (type == "label")
                                html += "<td class='tdtext' colspan=" + colspan + " style='width: 10%;'>" + label + "</td>";
                            else if (type == "input[text]")
                            {
                                html += "<td colspan=" + colspan + " style='width: " + widthCell + "%;'><span class='fontwhite'>*<span><input id='" + id + "' name='" + id + "' type='text' value='" + Server.HtmlEncode(cellCustEdit.InnerText) + "' /></td>";
                            }

                            else if (type == "input[textfunction]")
                            {
                                string funName = cellCustEdit.Attributes["onkeypress"].Value;
                                if (id == "txtlblSchoolzipcode" || id == "txtlblSchoolzipcodeResp" || id == "txtDentallblPhysicianZipcode" || id == "txtEyelblPhysicianZipcode" || id == "txtDentallblPhysicianZipcode" || id == "txtOtherlblPhysicianZipcode" || id == "txtlblPhysicianZipcode" || id == "txtlblPhysicianZipcodeEar")
                                {
                                    html += "<td colspan=" + colspan + " style='width: " + widthCell + "%;'><span class='fontwhite'>*<span><input id='" + id + "' name='" + id + "' value='" + Server.HtmlEncode(Convert.ToString(cellCustEdit.InnerText)) + "'  Maxlength='5'  onkeypress='" + funName + "' type='text' /></td>";
                                }
                                else
                                {
                                    html += "<td colspan=" + colspan + " style='width: " + widthCell + "%;'><span class='fontwhite'>*<span><input id='" + id + "' name='" + id + "' value='" + Server.HtmlEncode(Convert.ToString(cellCustEdit.InnerText)) + "' onkeypress='" + funName + "' type='text' /></td>";
                                }
                            }

                            else if (type == "input[textvalidate]")
                            {
                                string className = cellCustEdit.Attributes["className"].Value;
                                html += "<td colspan=" + colspan + " style='width: " + widthCell + "%;'><span class='fontwhite'>*<span><input id='" + id + "' name='" + id + "' value='" + Server.HtmlEncode(Convert.ToString(cellCustEdit.InnerText)) + "' class='" + className + "' type='text' /></td>";
                            }
                            else if (type == "input[multitext]")
                            {
                                string width = cellCustEdit.Attributes["Width"].Value;
                                html += "<td colspan=" + colspan + " style='width: " + widthCell + "%;'><span class='fontwhite'>*<span><textarea id='" + id + "' name='" + id + "' type='text' style='width:" + width + ";' rows='3' value='" + Server.HtmlEncode(Convert.ToString(cellCustEdit.InnerText)) + "'>" + cellCustEdit.InnerText + "</textarea></td>";
                            }
                            else if (type == "input[textdate]")
                            {
                                html += "<td colspan=" + colspan + " style='width: " + widthCell + "%;'><span class='fontwhite'>*<span><input id='" + id + "' value='" + Server.HtmlEncode(Convert.ToString(cellCustEdit.InnerText)) + "' onkeypress='return false'  name='" + id + "' type='text' class='datepicker' onpaste='PreventDef(event)' /></td>";
                            }

                            else if (type == "input[drop]")
                            {
                                string result = cellCustEdit.InnerText.ToString();
                                countCheck = Convert.ToInt32(cellCustEdit.Attributes["Count"].Value);
                                string[] lblsplit = label.Split('|');
                                if (id == "drpControl" && countCheck != 4 && countCheck != 3)
                                {
                                    html += "<td colspan=" + colspan + " style='width:" + widthCell + "%;'><span class='fontwhite'>*<span><select id='" + id + "' name='" + id + "' class='drpClass'><option value=0>-----Select.....</option>";
                                    for (int i = 0; i < countCheck; i++)
                                    {
                                        if (lblsplit[i] == result)
                                        {
                                            html += "<option value='" + lblsplit[i] + "' selected='true'>" + lblsplit[i] + "</option>";
                                        }
                                        else
                                        {
                                            html += "<option value='" + lblsplit[i] + "'>'" + lblsplit[i] + "'</option>";
                                        }

                                        //html += "<input id='" + id + "' type='checkbox' value='unchecked' name='" + idSpl[i] + "' onclick='GetSelected(this);' !checked><span>" + lblsplit[i] + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>";

                                    }
                                    html += "</select></td>";
                                }
                                else
                                {
                                    html += "<td colspan=" + colspan + " style='width:" + widthCell + "%;'><span class='fontwhite'>*<span><select id='" + id + "' name='" + id + "' class='drpClass2'><option value=0>-----Select.....</option>";
                                    for (int i = 0; i < countCheck; i++)
                                    {
                                        if (lblsplit[i] == result)
                                        {
                                            html += "<option value='" + lblsplit[i] + "' selected='true'>" + lblsplit[i] + "</option>";
                                        }
                                        else
                                        {
                                            html += "<option value='" + lblsplit[i] + "'>'" + lblsplit[i] + "'</option>";
                                        }

                                        //html += "<input id='" + id + "' type='checkbox' value='unchecked' name='" + idSpl[i] + "' onclick='GetSelected(this);' !checked><span>" + lblsplit[i] + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>";

                                    }
                                    html += "</select></td>";
                                }

                            }

                            else if (type == "input[file]")
                            {
                                string nameFile = cellCustEdit.Attributes["name"].Value.ToString();
                                html += "<td colspan=" + colspan + " style='width: " + widthCell + "%;'><span class='validate'>*<span><input id='" + id + "' name='" + nameFile + "' type='file' /></td>";
                            }
                            else if (type == "input[check]")
                            {
                                countCheck = Convert.ToInt32(cellCustEdit.Attributes["Count"].Value);
                                string[] idSplit = id.Split('|');
                                string[] lblSplit = label.Split('|');
                                html += "<td colspan=" + colspan + " style='width: " + widthCell + "%;'>";
                                for (int i = 0; i < countCheck; i++)
                                {
                                    html += "<input id='" + idSplit[i] + "' type='checkbox'>" + lblSplit[i] + "</input><br>";
                                }
                                html += "</td>";

                            }
                            else if (type == "input[checkVt]")
                            {
                                string[] splitInnerTxt = cellCustEdit.InnerText.Split('|');
                                countCheck = Convert.ToInt32(cellCustEdit.Attributes["Count"].Value);
                                string[] idSpl = id.Split('|');
                                string[] lblsplit = label.Split('|');
                                html += "<td colspan=" + colspan + " style='width:" + widthCell + "%;'>";
                                for (int i = 0; i < countCheck; i++)
                                {
                                    if (splitInnerTxt[i] == "checked")
                                    {
                                        html += "<input id='" + idSpl[i] + "' type='checkbox' value='checked' name='" + idSpl[i] + "' onclick='GetSelected(this);' checked><span>" + lblsplit[i] + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>";
                                    }
                                    else
                                    {
                                        html += "<input id='" + idSpl[i] + "' type='checkbox' value='unchecked' name='" + idSpl[i] + "' onclick='GetSelected(this);'><span>" + lblsplit[i] + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>";
                                    }
                                }
                                html += "</td>";

                            }


                            //else if (type == "input[radiosingleselect]")
                            //{
                            //    string[] splitInnerTxt = cellCustEdit.InnerText.Split('|');
                            //    countCheck = Convert.ToInt32(cellCustEdit.Attributes["Count"].Value);
                            //    string name = cellCustEdit.Attributes["name"].Value.ToString();
                            //    string[] idSpl = id.Split('|');
                            //    string[] lblsplit = label.Split('|');
                            //    html += "<td colspan=" + colspan + " style='width:20%;'>";
                            //    for (int i = 0; i < countCheck; i++)
                            //    {
                            //        if (splitInnerTxt[i] == "checked")
                            //        {
                            //            html += "<input id='" + idSpl[i] + "' type='radio' value='checked' name='" + name + "' onclick='GetSelected(this);' selected=true><span>" + lblsplit[i] + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>";
                            //        }
                            //        else
                            //        {
                            //            html += "<input id='" + idSpl[i] + "' type='radio' value='unchecked' name='" + name + "' onclick='GetSelected(this);'><span>" + lblsplit[i] + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>";
                            //        }
                            //    }
                            //    html += "</td>";

                            //}

                            else if (type == "input[checkNolbl]")
                            {
                                string[] splitInnerTxt = cellCustEdit.InnerText.Split('|');
                                countCheck = Convert.ToInt32(cellCustEdit.Attributes["Count"].Value);
                                string[] idSpl = id.Split('|');
                                string[] lblsplit = label.Split('|');
                                html += "<td colspan=" + colspan + " style='width:" + widthCell + "%;'>";
                                for (int i = 0; i < countCheck; i++)
                                {
                                    if (splitInnerTxt[i] == "checked")
                                    {
                                        html += "<input id='" + idSpl[i] + "' type='checkbox' value='checked' name='" + idSpl[i] + "' onclick='GetSelected(this);' checked>";
                                    }
                                    else
                                    {
                                        html += "<input id='" + idSpl[i] + "' type='checkbox' value='unchecked' name='" + idSpl[i] + "' onclick='GetSelected(this);'>";
                                    }
                                }
                                html += "</td>";

                            }
                            else if (type == "lblBold")
                            {
                                html += "<td class='lblBold' colspan=" + colspan + " style='width: " + widthCell + "%;'>" + label + "</td>";
                            }
                            else if (type == "label")
                            {
                                html += "<td class='tdtext' colspan=" + colspan + " style='width: " + tdwidth + "%;'>" + label + "</td>";
                            }
                            else if (type == "td")
                            {
                                html += "<td colspan=" + colspan + " style='width:" + tdwidth + "%;'></td>";
                            }

                        }
                        html += "</tr>";
                    }
                }
            }
            MelmarkDBEntities objData = new MelmarkDBEntities();
            ClsCommon getcheck = new ClsCommon();
            int QId = getcheck.getQueueId("CL");
            var IsClient = objData.ref_QueueStatus.Where(x => x.SchoolId == sess.SchoolId && x.StudentPersonalId == sess.ReferralId && x.QueueId == QId).ToList();
            string permission = getcheck.setPermission();
            if (Index == 1)
            {

                if (permission == "true")
                {
                    if (IsClient.Count == 0)
                    {
                        if (isUpated == 1)
                        {
                            html += "<td colspan= 3 style='width: " + widthCell + "%; text-align:right;'><input id='btnGenSave' value='Update' type='submit' class='NFButton' /></td>";
                        }
                        else
                        {
                            html += "<td colspan=3 style='width: " + widthCell + "%; text-align:right;'><input id='btnGenSave' value='Save' type='submit' class='NFButton' /></td>";
                        }
                    }
                    else
                    {
                        if (isUpated == 1)
                        {
                            html += "<td colspan= 3 style='width: " + widthCell + "%; text-align:right;'><input id='btnGenSave' value='Update' type='submit' class='NFButton' style='display:none;'/></td>";
                        }
                        else
                        {
                            html += "<td colspan=3 style='width: " + widthCell + "%; text-align:right;'><input id='btnGenSave' value='Save' type='submit' class='NFButton' style='display:none;'/></td>";
                        }
                    }
                }
                else
                {
                    if (isUpated == 1)
                    {
                        html += "<td colspan= 3 style='width: " + widthCell + "%; text-align:right;'><input id='btnGenSave' value='Update' type='submit' class='NFButton' style='display:none;'/></td>";
                    }
                    else
                    {
                        html += "<td colspan=3 style='width: " + widthCell + "%; text-align:right;'><input id='btnGenSave' value='Save' type='submit' class='NFButton' style='display:none;'/></td>";
                    }
                }

                //html += "<td colspan=" + colspan + " style='width: 20%; text-align:right;'><input id='" + id + "' value='" + (isUpated == 1 ? "update" : "save") + "' type='submit' class='NFButton' /></td>";
            }
            else if (Index == 2)
            {
                if (permission == "true")
                {
                    if (IsClient.Count == 0)
                    {
                        if (isUpated == 1)
                        {
                            html += "<td colspan= 3 style='width: " + widthCell + "%; text-align:right;'><input id='btnFamDataSave' value='Update' type='submit' class='NFButton' /></td>";
                        }
                        else
                        {
                            html += "<td colspan=3 style='width: " + widthCell + "%; text-align:right;'><input id='btnFamDataSave' value='Save' type='submit' class='NFButton' /></td>";
                        }
                    }
                    else
                    {
                        if (isUpated == 1)
                        {
                            html += "<td colspan= 3 style='width: " + widthCell + "%; text-align:right;'><input id='btnFamDataSave' value='Update'  style='display:none;' type='submit' class='NFButton' /></td>";
                        }
                        else
                        {
                            html += "<td colspan=3 style='width: " + widthCell + "%; text-align:right;'><input id='btnFamDataSave' value='Save'  style='display:none;' type='submit' class='NFButton' /></td>";
                        }
                    }
                }
                else
                {
                    if (isUpated == 1)
                    {
                        html += "<td colspan= 3 style='width: " + widthCell + "%; text-align:right;'><input id='btnFamDataSave' value='Update' type='submit' class='NFButton' /></td>";
                    }
                    else
                    {
                        html += "<td colspan=3 style='width: " + widthCell + "%; text-align:right;'><input id='btnFamDataSave' value='Save' type='submit' class='NFButton' /></td>";
                    }
                }
            }

            else if (Index == 3)
            {
                if (isUpated == 1)
                {
                    html += "<td colspan= 3 style='width: " + widthCell + "%; text-align:right;'><input id='btnBirthDetSave' value='Update' type='submit' class='NFButton' /></td>";   //no rows on top
                }
                else
                {
                    html += "<td colspan=3 style='width: " + widthCell + "%; text-align:right;'><input id='btnBirthDetSave' value='Save' type='submit' class='NFButton' /></td>";
                }
            }
            else if (Index == 4)
            {
                if (isUpated == 1)
                {
                    html += "<td colspan= 3 style='width: " + widthCell + "%; text-align:right;'><input id='btnPersnlHistrySave' value='Update' type='submit' class='NFButton' /></td>";
                }
                else
                {
                    html += "<td colspan=3 style='width: " + widthCell + "%; text-align:right;'><input id='btnPersnlHistrySave' value='Save' type='submit'  class='NFButton' /></td>";
                }
            }
            else if (Index == 5)
            {
                if (isUpated == 1)
                {
                    html += "<td colspan= 3 style='width: " + widthCell + "%; text-align:right;'><input id='btnRecretionSave' value='Update' type='submit' class='NFButton' /></td>";
                }
                else
                {
                    html += "<td colspan=3 style='width: " + widthCell + "%; text-align:right;'><input id='btnRecretionSave' value='Save' type='submit' class='NFButton' /></td>";
                }
            }
            else if (Index == 6)
            {
                if (isUpated == 1)
                {
                    html += "<td colspan= 3 style='width: " + widthCell + "%; text-align:right;'><input id='btnPresentSkillsSave' value='Update' type='submit' class='NFButton' /></td>";
                }
                else
                {
                    html += "<td colspan=3 style='width: " + widthCell + "%; text-align:right;'><input id='btnPresentSkillsSave' value='Save' type='submit' class='NFButton' /></td>";
                }
            }
            else if (Index == 7)
            {
                if (isUpated == 1)
                {
                    html += "<td colspan= 3 style='width: " + widthCell + "%; text-align:right;'><input id='btnFundDataSave' value='Update' type='submit' class='NFButton' /></td>";
                }
                else
                {
                    html += "<td colspan=3 style='width: " + widthCell + "%; text-align:right;'><input id='btnFundDataSave' value='Save' type='submit' class='NFButton' /></td>";
                }
            }
            else if (Index == 8)
            {
                if (isUpated == 1)
                {
                    html += "<td style='width: " + widthCell + "%; text-align:right;'><a id='lnk' href='#' style='display:none;' onclick='ShowDocuments()'  >View Items</a></td> <td style='width: " + widthCell + "%; text-align:right;'><input id='btnSaveDoccuments' value='Save' type='submit' class='NFButton' /></td>";
                }
                else
                {
                    html += "<td style='width: " + widthCell + "%; text-align:right;'><a id='lnk' href='#' style='display:none;' onclick='ShowDocuments()' >View Items</a></td> <td style='width: " + widthCell + "%; text-align:right;'><input id='btnSaveDoccuments' value='Save' type='submit' class='NFButton' /></td>";
                }
            }


            html += "<tr><td><input id='hidSec" + Index + "' class='tabcls' value='" + Index + "' name='TabId' type='hidden'/></td></tr>";
            html += "</table></div></div></form>";
            genModel.html = html;

            //}
            return html;

            //return View("GeneralInfoData", genModel);


        }


        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public byte[] SaveCustomXmlOld(FormCollection formdata, int stId, int Index)
        {
            // stId = 0;   
            sess = (clsSession)Session["UserSession"];
            if (stId == 0)
                stId = Convert.ToInt32(sess.ReferralId);
            int countCheck = 0;
            byte[] retXmlByte = null;
            string formVal = "";
            string html = "";
            clsReferral objClsRef = new clsReferral();
            XmlDocument xdocCustom = new XmlDocument();
            xdocCustom = objClsRef.LoadXmlfromBlob(stId);
            //  xdocCustom.Load(Server.MapPath("~/XML/RefInfoCustom.xml"));
            XmlNodeList xSectionsCustom = xdocCustom.SelectNodes("/ReferralTemplate/Sections/Section[@name]");
            XmlNode xSectnCustom = xSectionsCustom[Index - 1];
            XmlNodeList rowsCust = xSectnCustom.ChildNodes[0].ChildNodes;


            foreach (XmlNode rowcust in rowsCust)
            {
                //html += "<tr>";
                foreach (XmlNode cellCust in rowcust.ChildNodes)
                {
                    string id = cellCust.Attributes["id"].Value;
                    string typeD = cellCust.Attributes["type"].Value;
                    string label = cellCust.Attributes["label"].Value;
                    string colspan = cellCust.Attributes["colspan"].Value;
                    // if (type == "label")
                    //html += "<td class='tdtext' colspan=" + colspan + " style='width: 20%;'>" + label + "</td>";

                    if (typeD == "input[text]")
                    {
                        formVal = formdata[id].ToString();

                        cellCust.InnerText = formVal;
                    }
                    else if (typeD == "input[textfunction]")
                    {
                        formVal = formdata[id].ToString();

                        cellCust.InnerText = formVal;
                    }
                    else if (typeD == "input[textvalidate]")
                    {
                        formVal = formdata[id].ToString();

                        cellCust.InnerText = formVal;
                    }
                    else if (typeD == "input[textdate]")
                    {
                        formVal = formdata[id].ToString();

                        cellCust.InnerText = formVal;
                    }
                    else if (typeD == "input[multitext]")
                    {
                        formVal = formdata[id].ToString();

                        cellCust.InnerText = formVal;
                    }
                    else if (typeD == "input[checkVt]")
                    {
                        countCheck = Convert.ToInt32(cellCust.Attributes["Count"].Value);
                        string[] idSpl = id.Split('|');
                        string[] lblsplit = label.Split('|');
                        cellCust.InnerText = "";
                        for (int i = 0; i < countCheck; i++)
                        {
                            try
                            {
                                if (formdata[idSpl[i]] != null) formVal = formdata[idSpl[i]].ToString();
                                if (formVal == "checked")
                                {
                                    cellCust.InnerText += "checked|";
                                }
                                else
                                {
                                    cellCust.InnerText += "unchecked|";
                                }

                            }
                            catch
                            {
                                cellCust.InnerText += "unchecked|";
                            }

                        }
                        cellCust.InnerText = cellCust.InnerText.Substring(0, cellCust.InnerText.Length - 1);


                    }

                    else if (typeD == "input[drop]")
                    {
                        countCheck = Convert.ToInt32(cellCust.Attributes["Count"].Value);
                        string[] lblsplit = label.Split('|');
                        cellCust.InnerText = "";
                        for (int i = 0; i < countCheck; i++)
                        {
                            formVal = formdata[id].ToString();
                            if (id == "drplbltowelclothproperly" || id == "drplbltowelclothproperly")
                            {

                                if (formVal.Contains(","))
                                {
                                    string[] ar = formVal.Split(',');
                                    if (ar != null) formVal = ar[0];
                                }
                            }


                            cellCust.InnerText = formVal;

                        }


                    }


                    //else if (typeD == "input[radiosingleselect]")
                    //{
                    //    countCheck = Convert.ToInt32(cellCust.Attributes["Count"].Value);
                    //    string name = cellCust.Attributes["name"].Value.ToString();
                    //    string[] idSpl = id.Split('|');
                    //    string[] lblsplit = label.Split('|');
                    //    cellCust.InnerText = "";
                    //    for (int i = 0; i < countCheck; i++)
                    //    {
                    //        try
                    //        {
                    //            formVal = formdata[idSpl[i]].ToString();
                    //            if (formVal == "checked")
                    //            {
                    //                cellCust.InnerText += "checked|";
                    //            }
                    //            else
                    //            {
                    //                cellCust.InnerText += "unchecked|";
                    //            }

                    //        }
                    //        catch
                    //        {
                    //            cellCust.InnerText += "unchecked|";
                    //        }

                    //    }
                    //    cellCust.InnerText = cellCust.InnerText.Substring(0, cellCust.InnerText.Length - 1);


                    //}

                    else if (typeD == "input[checkNolbl]")
                    {
                        countCheck = Convert.ToInt32(cellCust.Attributes["Count"].Value);
                        string[] idSpl = id.Split('|');
                        string[] lblsplit = label.Split('|');
                        cellCust.InnerText = "";
                        for (int i = 0; i < countCheck; i++)
                        {
                            try
                            {
                                formVal = formdata[idSpl[i]].ToString();
                                if (formVal == "checked")
                                {
                                    cellCust.InnerText += "checked|";
                                }
                                else
                                {
                                    cellCust.InnerText += "unchecked|";
                                }

                            }
                            catch
                            {
                                cellCust.InnerText += "unchecked|";
                            }

                        }
                        cellCust.InnerText = cellCust.InnerText.Substring(0, cellCust.InnerText.Length - 1);

                        //html += "<td colspan=" + colspan + " style='width:20%;'>";
                        //for (int i = 0; i < countCheck; i++)
                        //{
                        //    html += "<input id='" + idSpl[i] + "' type='checkbox'>" + lblsplit[i] + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</input>";
                        //}
                        //html += "</td>";

                    }



                }
                html += "</tr>";
                formVal = "";
            }

            xdocCustom.PreserveWhitespace = true;
            string virtualPath = Server.MapPath("../XML/Data" + stId + ".xml");
            xdocCustom.Save(virtualPath);

            retXmlByte = objClsRef.SaveAsBlob(virtualPath);

            if (retXmlByte != null)
            {
                System.IO.File.Delete(virtualPath);
            }


            return retXmlByte;

        }



        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public void ResetSessionTab()
        {
            sess = (clsSession)Session["UserSession"];
            if (sess != null)
            {
                sess.SessTab1 = 0;
                sess.SessTab2 = 0;
                sess.SessTab3 = 0;
                sess.SessTab4 = 0;
                sess.SessTab5 = 0;
                sess.SessTab6 = 0;
                sess.SessTab7 = 0;
                sess.SessTab8 = 0;
                Session["RefId"] = 0;

            }

        }

        public bool IsTabSaved(int studentId, string TabName)
        {
            MelmarkDBEntities objData = new MelmarkDBEntities();
            bool valid = false;

            var result = (from TabDef in objData.TabDefinitions
                          where (TabDef.TabName == TabName & TabDef.StudentId == studentId)
                          select new
                          {
                              TabId = TabDef.TabId
                          }).ToList();
            if (result.Count > 0)
            {

                valid = true;

            }
            else valid = false;


            return valid;
        }

        public bool IsSessionTab3(int studentId)
        {
            MelmarkDBEntities objData = new MelmarkDBEntities();
            bool valid = false;


            var result = (from MedicalInsrnce in objData.MedicalAndInsurances
                          where (MedicalInsrnce.StudentPersonalId == studentId)
                          select new
                          {
                              phyId = MedicalInsrnce.MedicalInsuranceId

                          }).SingleOrDefault();


            if (result != null)
            {
                valid = true;
            }
            else valid = false;
            return valid;
        }

        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult SaveGeneralData(GenInfoModel model, string type, FormCollection formdata, HttpPostedFileBase fileUpldDocName, HttpPostedFileBase fileUploadPhotoName)
        {
            int id = 0;
            int sveId = 0;
            string status = "";
            byte[] xmlData = null;
            clsReferral clsRf = new clsReferral();
            ActionResult returnResult = null;
            ClsCommon clsCommon = new ClsCommon();
            LetterTray clsletter = new LetterTray();
            sess = (clsSession)Session["UserSession"];
            MelmarkDBEntities objData = new MelmarkDBEntities();
            ApplicantUploadDownload clsUpld = new ApplicantUploadDownload();
            // clsSessionTab sesClsObj = (clsSessionTab)Session["sesData"];
            string xmlPath = Server.MapPath("../XML/NewRefInfoNE.xml");
            TempData["type"] = type;

            if (sess != null)
            {

                if (model.TabId == 1)
                {
                    int tempId = 0;
                    tempId = Convert.ToInt32(Session["RefId"]);
                    ClsErrorLog errlog = new ClsErrorLog();
                    errlog.WriteToLog("sessRefId: " + sess.ReferralId);
                    errlog.WriteToLog("tempId: " + tempId);
                    errlog.WriteToLog("status: " + status);
                    errlog.WriteToLog("type: " + type);
                    if (sess.SessTab1 > 0 || tempId > 0)
                    {
                        status = "update";
                        sveId = Convert.ToInt32(sess.SessTab1);
                        errlog.WriteToLog("sveId: " + sveId);
                        if (sveId != 0)
                        {
                            TempData["EditId"] = id = clsRf.SaveGeneralData(model, type, sveId, status, sess.LoginId, sess.SchoolId);
                        }
                        else
                            TempData["EditId"] = id = clsRf.SaveGeneralData(model, type, tempId, status, sess.LoginId, sess.SchoolId);
                        sess.ReferralId = id;
                        sess.SessTab1 = id;
                    }
                    else
                    {
                        status = "save";
                        TempData["EditId"] = id = clsRf.SaveGeneralData(model, type, sveId, status, sess.LoginId, sess.SchoolId);

                        xmlData = clsRf.SaveAsBlob(xmlPath);   //Initial full xml doccument saving to database
                        clsRf.SaveXmlToDB(xmlData, id);

                        sess.ReferralId = id;                    // Insert the new referal Id in Q Status
                        clsCommon.insertQstatus("NA", "N");
                        clsCommon.insertQstatus("AR", "Y");
                        int Qid = clsCommon.getQueueId("NA");
                        var LetterName = objData.LetterEngines.Where(x => x.QueueId == Qid && x.ApproveStatus == true && x.SchoolId == sess.SchoolId && x.LetterType == "Letter").ToList();
                        if (LetterName.Count > 0)
                        {
                            clsletter.insertLetter("NA", sess.ReferralId, LetterName[0].LetterEngineId, "Parent");
                        }
                        sess.SessTab1 = id;
                        Session["RefId"] = id;
                        sess.ReferralId = id;
                    }

                    UploadStudentPhoto(id, model, fileUploadPhotoName);
                    xmlData = SaveCustomXml(formdata, id, model.TabId);
                    clsRf.SaveXmlToDB(xmlData, id);

                    returnResult = RedirectToAction("SectionRedirect", new { tabID = model.TabId });

                }
                else if (model.TabId == 2)
                {
                    if (Session["RefId"] != null)
                    {
                        int tempId = 0;
                        tempId = Convert.ToInt32(Session["RefId"]);
                        ClsErrorLog errlog = new ClsErrorLog();
                        errlog.WriteToLog("sessRefId: " + sess.ReferralId);
                        errlog.WriteToLog("tempId: " + tempId);
                        errlog.WriteToLog("status: " + status);
                        errlog.WriteToLog("type: " + type);
                        errlog.WriteToLog("sveId: " + sveId);
                        if (sess.SessTab2 > 0)
                        {
                            status = "update";
                            sveId = Convert.ToInt32(sess.SessTab2);
                            errlog.WriteToLog("sveId: " + sveId);
                            if (sveId != 0)
                            {
                                TempData["EditId"] = id = clsRf.SaveGeneralData(model, type, sveId, status, sess.LoginId, sess.SchoolId);
                            }
                            else
                                TempData["EditId"] = id = clsRf.SaveGeneralData(model, type, tempId, status, sess.LoginId, sess.SchoolId);
                            sess.ReferralId = id;
                            clsRf.SaveCurrentTab(id, "Tab2");
                        }
                        else
                        {
                            status = "save";
                            TempData["EditId"] = id = clsRf.SaveGeneralData(model, type, tempId, status, sess.LoginId, sess.SchoolId);
                            clsRf.SaveCurrentTab(id, "Tab2");

                            sess.SessTab2 = id;
                            Session["RefId"] = id;
                            sess.ReferralId = id;
                        }
                        //if (Session["RefId"] != null)
                        //{
                        //    if (sess.SessTab2 > 0)
                        //    {
                        //        status = "update";
                        //        sveId = Convert.ToInt32(sess.SessTab2);
                        //        TempData["EditId"] = id = clsRf.SaveGeneralData(model, type, sveId, status, sess.LoginId, sess.SchoolId);
                        //    }
                        //    else
                        //    {
                        //        if (Session["RefId"] != null)
                        //        {
                        //            sveId = Convert.ToInt32(Session["RefId"]);
                        //        }
                        //        status = "save";
                        //        TempData["EditId"] = id = clsRf.SaveGeneralData(model, type, sveId, status, sess.LoginId, sess.SchoolId);
                        //        clsRf.SaveCurrentTab(id, "Tab2");
                        //        sess.SessTab2 = id;
                        //    }

                        //    xmlData = SaveCustomXml(formdata, id, model.TabId);
                        //    clsRf.SaveXmlToDB(xmlData, id);

                        //    returnResult = RedirectToAction("SectionRedirect", new { tabID = model.TabId });
                        //}
                        //else
                        //    returnResult = View();
                        xmlData = SaveCustomXml(formdata, id, model.TabId);
                        clsRf.SaveXmlToDB(xmlData, id);

                        returnResult = RedirectToAction("SectionRedirect", new { tabID = model.TabId });
                    }
                    else
                        returnResult = View();

                }
                else if (model.TabId == 3)
                {
                    ClsErrorLog errlog = new ClsErrorLog();
                    errlog.WriteToLog("sessRefId: " + sess.ReferralId);
                    bool itabSaved = IsTabSaved(sess.ReferralId, "Tab3");
                    errlog.WriteToLog("itabSaved: " + itabSaved);
                    errlog.WriteToLog("sessRefId: " + sess.ReferralId);

                    errlog.WriteToLog("status: " + status);
                    errlog.WriteToLog("type: " + type);

                    if (itabSaved)
                    {
                        sess.SessTab3 = sess.ReferralId;
                    }
                    if (Session["RefId"] != null)
                    {
                        int tempId = 0;
                        tempId = Convert.ToInt32(Session["RefId"]);
                        errlog.WriteToLog("tempId: " + tempId);
                        errlog.WriteToLog("tempId: " + sess.SessTab3);
                        if (sess.SessTab3 > 0)
                        {
                            status = "update";

                            sveId = Convert.ToInt32(sess.SessTab3);
                            errlog.WriteToLog("sveId: " + sveId);
                            if (sveId != 0)
                            {
                                TempData["EditId"] = id = clsRf.SaveGeneralData(model, type, sveId, status, sess.LoginId, sess.SchoolId);
                            }
                            else
                                TempData["EditId"] = id = clsRf.SaveGeneralData(model, type, tempId, status, sess.LoginId, sess.SchoolId);
                            errlog.WriteToLog("EditId: " + id);
                            sess.SessTab3 = id;
                            clsRf.SaveCurrentTab(id, "Tab3");
                        }
                        else
                        {
                            status = "save";
                            TempData["EditId"] = id = clsRf.SaveGeneralData(model, type, tempId, status, sess.LoginId, sess.SchoolId);
                            clsRf.SaveCurrentTab(id, "Tab3");

                            sess.SessTab3 = id;
                            Session["RefId"] = id;
                            sess.ReferralId = id;
                        }
                        xmlData = SaveCustomXml(formdata, id, model.TabId);
                        clsRf.SaveXmlToDB(xmlData, id);

                        returnResult = RedirectToAction("SectionRedirect", new { tabID = model.TabId });
                    }
                    else
                        returnResult = View();
                    //if (Session["RefId"] != null)
                    //{
                    //    if (sess.SessTab3 > 0)
                    //    {
                    //        status = "update";
                    //        sveId = Convert.ToInt32(sess.SessTab3);
                    //        TempData["EditId"] = id = clsRf.SaveGeneralData(model, type, sveId, status, sess.LoginId, sess.SchoolId);
                    //    }
                    //    else
                    //    {
                    //        if (Session["RefId"] != null)
                    //        {
                    //            sveId = Convert.ToInt32(Session["RefId"]);
                    //        }
                    //        status = "save";
                    //        TempData["EditId"] = id = clsRf.SaveGeneralData(model, type, sveId, status, sess.LoginId, sess.SchoolId);
                    //        clsRf.SaveCurrentTab(id, "Tab3");
                    //        sess.SessTab3 = id;
                    //    }

                    //    xmlData = SaveCustomXml(formdata, id, model.TabId);
                    //    clsRf.SaveXmlToDB(xmlData, id);

                    //    returnResult = RedirectToAction("SectionRedirect", new { tabID = model.TabId });
                    //}
                    //else returnResult = View();
                }
                else if (model.TabId == 4)
                {
                    if (Session["RefId"] != null)
                    {
                        int tempId = 0;
                        tempId = Convert.ToInt32(Session["RefId"]);
                        ClsErrorLog errlog = new ClsErrorLog();
                        errlog.WriteToLog("sessRefId: " + sess.ReferralId);
                        errlog.WriteToLog("tempId: " + tempId);
                        errlog.WriteToLog("status: " + status);
                        errlog.WriteToLog("type: " + type);

                        if (sess.SessTab4 > 0)
                        {
                            status = "update";
                            sveId = Convert.ToInt32(sess.SessTab4);
                            errlog.WriteToLog("sveId: " + sveId);
                            if (sveId != 0)
                            {
                                TempData["EditId"] = id = clsRf.SaveGeneralData(model, type, sveId, status, sess.LoginId, sess.SchoolId);
                            }
                            else
                                TempData["EditId"] = id = clsRf.SaveGeneralData(model, type, tempId, status, sess.LoginId, sess.SchoolId);
                            sess.SessTab4 = id;
                            clsRf.SaveCurrentTab(id, "Tab4");
                        }
                        else
                        {
                            status = "save";
                            TempData["EditId"] = id = clsRf.SaveGeneralData(model, type, tempId, status, sess.LoginId, sess.SchoolId);
                            clsRf.SaveCurrentTab(id, "Tab4");

                            sess.SessTab4 = id;
                            Session["RefId"] = id;
                            sess.ReferralId = id;
                        }
                        xmlData = SaveCustomXml(formdata, id, model.TabId);
                        clsRf.SaveXmlToDB(xmlData, id);

                        returnResult = RedirectToAction("SectionRedirect", new { tabID = model.TabId });
                    }
                    else
                        returnResult = View();
                    //if (Session["RefId"] != null)
                    //{
                    //    if (sess.SessTab4 > 0)
                    //    {
                    //        status = "update";
                    //        sveId = Convert.ToInt32(sess.SessTab4);
                    //        TempData["EditId"] = id = clsRf.SaveGeneralData(model, type, sveId, status, sess.LoginId, sess.SchoolId);
                    //    }
                    //    else
                    //    {
                    //        if (Session["RefId"] != null)
                    //        {
                    //            sveId = Convert.ToInt32(Session["RefId"]);
                    //        }
                    //        status = "save";
                    //        TempData["EditId"] = id = clsRf.SaveGeneralData(model, type, sveId, status, sess.LoginId, sess.SchoolId);
                    //        clsRf.SaveCurrentTab(id, "Tab4");
                    //        sess.SessTab4 = id;
                    //    }

                    //    xmlData = SaveCustomXml(formdata, id, model.TabId);
                    //    clsRf.SaveXmlToDB(xmlData, id);

                    //    returnResult = RedirectToAction("SectionRedirect", new { tabID = model.TabId });
                    //}
                    //else returnResult = View();
                }
                else if (model.TabId == 5)
                {
                    if (Session["RefId"] != null)
                    {
                        bool tabSaved = false;
                        int tempId = 0;
                        tempId = Convert.ToInt32(Session["RefId"]);
                        ClsErrorLog errlog = new ClsErrorLog();
                        errlog.WriteToLog("sessRefId: " + sess.ReferralId);
                        errlog.WriteToLog("tempId: " + tempId);
                        errlog.WriteToLog("status: " + status);
                        errlog.WriteToLog("type: " + type);

                        if (sess.SessTab5 > 0)
                        {
                            status = "update";
                            sveId = Convert.ToInt32(sess.SessTab5);
                            errlog.WriteToLog("sveId: " + sveId);
                            if (sveId != 0)
                            {
                                TempData["EditId"] = id = clsRf.SaveGeneralData(model, type, sveId, status, sess.LoginId, sess.SchoolId);
                            }
                            else
                                TempData["EditId"] = id = clsRf.SaveGeneralData(model, type, tempId, status, sess.LoginId, sess.SchoolId);
                            sess.SessTab5 = id;
                            clsRf.SaveCurrentTab(id, "Tab5");
                        }
                        else
                        {

                            status = "save";
                            TempData["EditId"] = id = clsRf.SaveGeneralData(model, type, tempId, status, sess.LoginId, sess.SchoolId);
                            clsRf.SaveCurrentTab(id, "Tab5");
                            tabSaved = IsTabSaved(id, "Tab5");
                            if (tabSaved == true)
                                status = "update";
                            else
                                status = "save";
                            sess.SessTab5 = id;
                            Session["RefId"] = id;
                            sess.ReferralId = id;
                        }
                        xmlData = SaveCustomXml(formdata, id, model.TabId);
                        clsRf.SaveXmlToDB(xmlData, id);

                        returnResult = RedirectToAction("SectionRedirect", new { tabID = model.TabId });
                    }
                    else
                        returnResult = View();

                    //if (Session["RefId"] != null)
                    //{

                    //    if (sess.SessTab5 > 0)
                    //    {
                    //        sveId = Convert.ToInt32(sess.SessTab5);
                    //        if (Session["RefId"] != null)
                    //        {
                    //            TempData["EditId"] = id = sveId;
                    //            xmlData = SaveCustomXml(formdata, id, model.TabId);
                    //            clsRf.SaveXmlToDB(xmlData, id);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        if (Session["RefId"] != null)
                    //        {
                    //            sveId = Convert.ToInt32(Session["RefId"]);
                    //            TempData["EditId"] = id = sveId;
                    //            xmlData = SaveCustomXml(formdata, id, model.TabId);
                    //            clsRf.SaveXmlToDB(xmlData, id);
                    //            clsRf.SaveCurrentTab(id, "Tab5");
                    //            sess.SessTab5 = id;
                    //        }


                    //    }
                    //    returnResult = RedirectToAction("SectionRedirect", new { tabID = model.TabId });
                    //}
                    //else returnResult = View();
                }
                else if (model.TabId == 6)
                {
                    if (Session["RefId"] != null)
                    {
                        bool tabSaved = false;
                        int tempId = 0;
                        tempId = Convert.ToInt32(Session["RefId"]);
                        ClsErrorLog errlog = new ClsErrorLog();
                        errlog.WriteToLog("sessRefId: " + sess.ReferralId);
                        errlog.WriteToLog("tempId: " + tempId);
                        errlog.WriteToLog("status: " + status);
                        errlog.WriteToLog("type: " + type);

                        if (sess.SessTab6 > 0)
                        {
                            status = "update";
                            sveId = Convert.ToInt32(sess.SessTab6);
                            errlog.WriteToLog("sveId: " + sveId);
                            if (sveId != 0)
                            {
                                TempData["EditId"] = id = clsRf.SaveGeneralData(model, type, sveId, status, sess.LoginId, sess.SchoolId);
                            }
                            else
                                TempData["EditId"] = id = clsRf.SaveGeneralData(model, type, tempId, status, sess.LoginId, sess.SchoolId);
                            sess.SessTab6 = id;
                            clsRf.SaveCurrentTab(id, "Tab6");
                        }
                        else
                        {
                            status = "save";
                            TempData["EditId"] = id = clsRf.SaveGeneralData(model, type, tempId, status, sess.LoginId, sess.SchoolId);
                            clsRf.SaveCurrentTab(id, "Tab6");

                            tabSaved = IsTabSaved(id, "Tab6");
                            if (tabSaved == true)
                                status = "update";
                            else
                                status = "save";

                            sess.SessTab6 = id;
                            Session["RefId"] = id;
                            sess.ReferralId = id;
                        }
                        xmlData = SaveCustomXml(formdata, id, model.TabId);
                        clsRf.SaveXmlToDB(xmlData, id);

                        returnResult = RedirectToAction("SectionRedirect", new { tabID = model.TabId });
                    }
                    else
                        returnResult = View();
                    //if (Session["RefId"] != null)
                    //{
                    //    if (sess.SessTab6 > 0)
                    //    {
                    //        sveId = Convert.ToInt32(sess.SessTab6);
                    //        if (Session["RefId"] != null)
                    //        {
                    //            TempData["EditId"] = id = sveId;
                    //            xmlData = SaveCustomXml(formdata, id, model.TabId);
                    //            clsRf.SaveXmlToDB(xmlData, id);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        if (Session["RefId"] != null)
                    //        {
                    //            sveId = Convert.ToInt32(Session["RefId"]);
                    //            TempData["EditId"] = id = sveId;
                    //            xmlData = SaveCustomXml(formdata, id, model.TabId);
                    //            clsRf.SaveXmlToDB(xmlData, id);
                    //            clsRf.SaveCurrentTab(id, "Tab6");
                    //            sess.SessTab6 = id;
                    //        }

                    //        // TempData["EditId"] = id = clsRf.SaveGeneralData(model, type, sveId, status);
                    //        //  sesClsObj.SessTab5 = id;
                    //    }

                    //    returnResult = RedirectToAction("SectionRedirect", new { tabID = model.TabId });
                    //}
                    //else returnResult = View();
                }
                else if (model.TabId == 7)
                {
                    if (Session["RefId"] != null)
                    {
                        bool tabSaved = false;
                        int tempId = 0;
                        tempId = Convert.ToInt32(Session["RefId"]);
                        ClsErrorLog errlog = new ClsErrorLog();
                        errlog.WriteToLog("sessRefId: " + sess.ReferralId);
                        errlog.WriteToLog("tempId: " + tempId);
                        errlog.WriteToLog("status: " + status);
                        errlog.WriteToLog("type: " + type);

                        if (sess.SessTab7 > 0)
                        {
                            status = "update";
                            sveId = Convert.ToInt32(sess.SessTab7);
                            errlog.WriteToLog("sveId: " + sveId);
                            if (sveId != 0)
                            {
                                TempData["EditId"] = id = clsRf.SaveGeneralData(model, type, sveId, status, sess.LoginId, sess.SchoolId);
                            }
                            else
                                TempData["EditId"] = id = clsRf.SaveGeneralData(model, type, tempId, status, sess.LoginId, sess.SchoolId);
                            sess.SessTab7 = id;
                            clsRf.SaveCurrentTab(id, "Tab7");
                        }
                        else
                        {
                            status = "save";
                            TempData["EditId"] = id = clsRf.SaveGeneralData(model, type, tempId, status, sess.LoginId, sess.SchoolId);
                            clsRf.SaveCurrentTab(id, "Tab7");

                            tabSaved = IsTabSaved(id, "Tab7");
                            if (tabSaved == true)
                                status = "update";
                            else
                                status = "save";

                            sess.SessTab7 = id;
                            Session["RefId"] = id;
                            sess.ReferralId = id;
                        }
                        xmlData = SaveCustomXml(formdata, id, model.TabId);
                        clsRf.SaveXmlToDB(xmlData, id);

                        returnResult = RedirectToAction("SectionRedirect", new { tabID = model.TabId });
                    }
                    else
                        returnResult = View();
                    //if (Session["RefId"] != null)
                    //{

                    //    if (sess.SessTab7 > 0)
                    //    {
                    //        sveId = Convert.ToInt32(sess.SessTab7);
                    //        if (Session["RefId"] != null)
                    //        {
                    //            TempData["EditId"] = id = sveId;
                    //            xmlData = SaveCustomXml(formdata, id, model.TabId);
                    //            clsRf.SaveXmlToDB(xmlData, id);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        if (Session["RefId"] != null)
                    //        {
                    //            sveId = Convert.ToInt32(Session["RefId"]);
                    //            TempData["EditId"] = id = sveId;
                    //            xmlData = SaveCustomXml(formdata, id, model.TabId);
                    //            clsRf.SaveXmlToDB(xmlData, id);
                    //            clsRf.SaveCurrentTab(id, "Tab7");
                    //            sess.SessTab7 = id;
                    //        }

                    //        // TempData["EditId"] = id = clsRf.SaveGeneralData(model, type, sveId, status);
                    //        //  sesClsObj.SessTab5 = id;
                    //    }

                    //    returnResult = RedirectToAction("SectionRedirect", new { tabID = model.TabId });
                    //}
                    //else returnResult = View();

                }
                else if (model.TabId == 8)
                {
                    if (Session["RefId"] != null)
                    {
                        //ClsErrorLog errlog = new ClsErrorLog();
                        //errlog.WriteToLog("sessRefId: " + sess.ReferralId);
                        //errlog.WriteToLog("status: " + status);
                        //errlog.WriteToLog("type: " + type);

                        if (sess.SessTab8 > 0)
                        {
                            //sveId = Convert.ToInt32(sesClsObj.SessTab8);
                            //if (Session["RefId"] != null)
                            //{
                            //    TempData["EditId"] = id = sveId;
                            //    xmlData = SaveCustomXml(formdata, id, model.TabId);
                            //    clsRf.SaveXmlToDB(xmlData, id);
                            //}
                        }
                        else
                        {
                            if (Session["RefId"] != null)
                            {
                                if (fileUpldDocName != null)
                                {
                                    sveId = Convert.ToInt32(Session["RefId"]);
                                    //errlog.WriteToLog("sveId: " + sveId);
                                    TempData["EditId"] = id = sveId;
                                    string Result = clsCommon.GetFileType(fileUpldDocName.ContentType);
                                    if (Result != "")
                                    {
                                        UploadDoccuments(id, model, fileUpldDocName, sess.SchoolId, sess.LoginId);
                                    }
                                    else
                                    {
                                        return Content("Invalid file format");
                                    }

                                    //   xmlData = SaveCustomXml(formdata, id, model.TabId);
                                    //   clsRf.SaveXmlToDB(xmlData, id);
                                    sess.SessTab8 = id;
                                }
                                else
                                {
                                    return Content("No file selected");
                                }
                            }

                            // TempData["EditId"] = id = clsRf.SaveGeneralData(model, type, sveId, status);
                            //  sesClsObj.SessTab5 = id;
                        }

                        returnResult = RedirectToAction("SectionRedirect", new { tabID = model.TabId });
                    }
                    else returnResult = View();
                }

                //  return View();
            }
            // return RedirectToAction("SectionRedirect", model);           

            return returnResult;
        }

        public void UploadDoccuments(int studentPersnlId, GenInfoModel model, HttpPostedFileBase upldFile, int schoolId, int loginId)
        {
            clsReferral objRef = new clsReferral();
            clsDocumentasBinary objBinary = new clsDocumentasBinary();
            try
            {
                if (upldFile != null && upldFile.ContentLength > 0 && model.objclsUpld.DocType != 0)
                {

                    string fileName = Path.GetFileName(upldFile.FileName);
                    string Name = objBinary.LookName(model.objclsUpld.DocType);

                    int Docid = objRef.FileUpload(studentPersnlId, schoolId, loginId, model.objclsUpld.DocName, model.objclsUpld.OtherName, fileName, model.objclsUpld.DocType);
                    if (Docid > 0)

                        objBinary.SaveBinaryFiles(sess.SchoolId, sess.ReferralId, model.objclsUpld.DocName, sess.LoginId, upldFile, "Referal_upld", Name, Docid);


                    //  upldFile.SaveAs(path + id + "-" + fileName);
                }
            }
            catch (Exception e)
            {

            }
        }



        public void UploadStudentPhoto(int StudentPersnlId, GenInfoModel model, HttpPostedFileBase upldStdPhoto)
        {
            MelmarkDBEntities objData = new MelmarkDBEntities();
            clsReferral objRef = new clsReferral();
            if (upldStdPhoto != null && upldStdPhoto.ContentLength > 0)
            {
                byte[] fileBytes = new byte[upldStdPhoto.ContentLength];



                int byteCount = upldStdPhoto.InputStream.Read(fileBytes, 0, (int)upldStdPhoto.ContentLength);

                int id = objRef.StudentUpldPhoto(StudentPersnlId, fileBytes);

            }

            var Sp = objData.StudentPersonals.Where(objStudent => objStudent.StudentPersonalId == StudentPersnlId).SingleOrDefault();
            if (Sp != null)
            {
                if (Sp.ImageUrl == null)
                {
                    if (upldStdPhoto == null)
                    {
                        string dirpath = (AppDomain.CurrentDomain.BaseDirectory + "\\RefferalPhoto\\referal.gif".ToString()).Replace('\\', '/');
                        FileInfo fileInfo = new FileInfo(dirpath);
                        byte[] data = new byte[fileInfo.Length];
                        using (FileStream fs = fileInfo.OpenRead())
                        {
                            fs.Read(data, 0, data.Length);
                        }
                        int id = objRef.StudentUpldPhoto(StudentPersnlId, data);

                    }
                }
            }


            //clsReferral objRef = new clsReferral();
            //if (upldStdPhoto != null && upldStdPhoto.ContentLength > 0)
            //{
            //    string fileName = Path.GetFileName(upldStdPhoto.FileName);
            //    string path = AppDomain.CurrentDomain.BaseDirectory + "RefferalPhoto\\";
            //    if (!Directory.Exists(path))
            //    {
            //        Directory.CreateDirectory(path);
            //    }                
            //    int id = objRef.StudentUpldPhoto(StudentPersnlId, fileName);
            //    if (id > 0)
            //        upldStdPhoto.SaveAs(path + id + "-" + fileName);
            //}
        }



        //public void UploadStudentPhoto(int StudentPersnlId, GenInfoModel model, HttpPostedFileBase upldStdPhoto)
        //{
        //    clsReferral objRef = new clsReferral();
        //    if (upldStdPhoto != null && upldStdPhoto.ContentLength > 0)
        //    {
        //        string fileName = Path.GetFileName(upldStdPhoto.FileName);
        //        string path = AppDomain.CurrentDomain.BaseDirectory + "RefferalPhoto\\";
        //        if (!Directory.Exists(path))
        //        {
        //            Directory.CreateDirectory(path);
        //        }
        //        int id = objRef.StudentUpldPhoto(StudentPersnlId, fileName);
        //        if (id > 0)
        //            upldStdPhoto.SaveAs(path + id + "-" + fileName);
        //    }
        //}

        //[OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        //public ActionResult SectionRedirect(GenInfoModel model)
        //{
        //    string type = "";
        //    if (TempData["type"] != null)
        //    {
        //        type = TempData["type"].ToString();
        //    }
        //    clsReferral clsRf = new clsReferral();
        //    //int editId = Convert.ToInt32(TempData["EditId"]);
        //    //TempData["datatomethod"] = clsRf.EditData(editId, type);
        //    return RedirectToAction("Section1", new { id = model.TabId, isAlert = true });

        //}


        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult SectionRedirect(int tabID)
        {
            string type = "";
            if (TempData["type"] != null)
            {
                type = TempData["type"].ToString();
            }
            clsReferral clsRf = new clsReferral();
            //int editId = Convert.ToInt32(TempData["EditId"]);
            //TempData["datatomethod"] = clsRf.EditData(editId, type);
            return RedirectToAction("Section1", new { id = tabID, isAlert = true });

        }

        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult GetStates(int countryId)
        {
            clsReferral clsRf = new clsReferral();
            var dataRes = clsRf.FillState(countryId);
            return Json(dataRes, JsonRequestBehavior.AllowGet);

        }


        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public string XmlResult(GenInfoModel genModel, int Index)
        {
            // clsSessionTab sesClsObj = null;
            clsReferral objClsRef = new clsReferral();
            sess = (clsSession)Session["UserSession"];

            if (TempData["datatomethod"] != null)
            {
                genModel = (GenInfoModel)TempData["datatomethod"];
                //  sesClsObj = (clsSessionTab)Session["sesData"];
            }
            else if (TempData["LoadStudentData"] != null)
            {
                genModel = (GenInfoModel)TempData["LoadStudentData"];
                //sesClsObj = (clsSessionTab)Session["sesData"];
                if (sess != null)
                {
                    if (sess.ReferralId != 0)
                    {
                        TempData["EditId"] = Session["RefId"] = sess.ReferralId;

                    }
                }

            }
            int isUpated = 0;
            if (Index == 1 && sess != null)
            {
                if (sess.SessTab1 > 0) isUpated = 1; else isUpated = 0;

            }
            if (Index == 2 && sess != null)
            {
                if (sess.SessTab2 > 0) isUpated = 1; else isUpated = 0;

            }
            if (Index == 3 && sess != null)
            {
                if (sess.SessTab3 > 0) isUpated = 1; else isUpated = 0;

            }
            if (Index == 4 && sess != null)
            {
                if (sess.SessTab4 > 0) isUpated = 1; else isUpated = 0;

            }
            if (Index == 5 && sess != null)
            {
                if (sess.SessTab5 > 0) isUpated = 1; else isUpated = 0;

            }
            if (Index == 6 && sess != null)
            {
                if (sess.SessTab6 > 0) isUpated = 1; else isUpated = 0;

            }
            if (Index == 7 && sess != null)
            {
                if (sess.SessTab7 > 0) isUpated = 1; else isUpated = 0;

            }
            if (Index == 8 && sess != null)
            {
                if (sess.SessTab8 > 0) isUpated = 1; else isUpated = 0;

            }






            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(Server.MapPath("~/XML/NewRefInfoNE.xml"));
            XmlNodeList xSections = xdoc.SelectNodes("/ReferralTemplate/Sections/Section[@name]");


            string html = "";

            int countCheck = 0;
            int widthCell = 25;
            int tdwidth = 8;
            if (TempData["EditId"] == null)
            {
                // int formid = 0;
                XmlNode xSectn = xSections[Index - 1];
                //foreach (XmlNode xSectn in xSections)
                //{
                // formid++;
                int count;
                string TableIds = "tblData" + Index;
                html += " <form method='post' id='frm" + Index + "' enctype='multipart/form-data' action='../GeneralInfo/SaveGeneralData?type=" + xSectn.Attributes["name"].Value + "'><div><span class='headPanel'  onclick='getColapse(\"" + TableIds + "\")'>" + xSectn.Attributes["name"].Value + "</span><div class='divSubs'><table id='" + TableIds + "' class='clsTabDiv' style='float:left;width=100%'> <tr style='width:100%'><td style='width:100%'><table style='width:100%;border:none;'>";
                XmlNodeList rows = xSectn.ChildNodes[0].ChildNodes;
                foreach (XmlNode row in rows)
                {
                    count = 0;
                    html += "<tr style='width:100%'>";
                    foreach (XmlNode cell in row.ChildNodes)
                    {
                        count++;
                        string id = cell.Attributes["id"].Value;
                        string type = cell.Attributes["type"].Value;
                        string label = cell.Attributes["label"].Value;
                        string colspan = cell.Attributes["colspan"].Value;
                        if (type == "label")
                        {
                            if (count == 3 || count == 5)
                            {
                                if (label == "Age" || label == "Gender")
                                    html += "<td class='tdtext' colspan=" + colspan + ">&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp" + label + "</td>";
                                else
                                    html += "<td class='tdtext' colspan=" + colspan + ">&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp" + label + "</td>";
                            }
                            html += "<td class='tdtext' colspan=" + colspan + " > " + label + "</td>";
                        }

                        else if (type == "header")
                        {
                            html += "<td class='subhead' colspan=" + colspan + ">" + label + "</td>";
                        }
                        //else if (type == "notes")
                        //    html += "<td class='notes-text' colspan=" + colspan + ">" + label + "</td>";
                        else if (type == "TableBreak")
                        {
                            html += "</tr></table><table style='width:100%;border:none;'>"; break;
                        }
                        else if (type == "TableBreakColumn4")
                        {
                            // tabType = type;
                            html += "</tr></table><table style='width:100%;border:none;table-layout:auto !important;'>"; break;
                        }
                        else if (type == "StudentPhoto")
                        {
                            string imagePath = "";

                            if (sess.ReferralId != 0)
                            {
                                imagePath = objClsRef.GetStudentImage(sess.ReferralId);
                                if (imagePath != null && imagePath != "")
                                {
                                    html += "<td colspan=" + colspan + "><img id='imgStudPhoto' height='150px' width='150px' src=data:image/gif;base64," + imagePath + "></td>";
                                }
                            }
                        }

                        else if (type == "input[text]")
                        {
                            bool val = objClsRef.IsModel(cell);
                            string widthXml = cell.Attributes["width"] != null ? cell.Attributes["width"].Value : "";
                            if (widthXml != "")
                            {
                                widthXml = "width:" + widthXml;
                                widthXml += "% !important";
                            }
                            if (val)
                            {
                                string maxLength = cell.Attributes["MaxLength"].Value;
                                string textValue = cell.Attributes["modelName"].Value;

                                object value = null;
                                try
                                {
                                    string[] split = textValue.Split('.');
                                    if (split.Length > 1)
                                    {
                                        if (split[0] == "objRelationMthr")
                                            value = genModel.objRelationMthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationMthr, null);
                                        if (split[0] == "objRelationFthr")
                                            value = genModel.objRelationFthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationFthr, null);
                                        if (split[0] == "objRelationClose")
                                            if (genModel.objRelationClose != null) value = genModel.objRelationClose.GetType().GetProperty(split[1]).GetValue(genModel.objRelationClose, null);
                                        if (split[0] == "objPhysicianDetails")
                                            if (genModel.objPhysicianDetails != null) value = genModel.objPhysicianDetails.GetType().GetProperty(split[1]).GetValue(genModel.objPhysicianDetails, null);
                                        if (split[0] == "objInsuranceDetails")
                                            value = genModel.objInsuranceDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDetails, null);
                                        if (split[0] == "objInsuranceSecDetails")
                                            value = genModel.objInsuranceSecDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceSecDetails, null);
                                        if (split[0] == "objInsuranceDentalDetails")
                                            value = genModel.objInsuranceDentalDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDentalDetails, null);
                                        if (split[0] == "objRelationLegalGuardian")
                                            value = genModel.objRelationLegalGuardian.GetType().GetProperty(split[1]).GetValue(genModel.objRelationLegalGuardian, null);
                                        if (split[0] == "objRelationEmergncyContact")
                                            value = genModel.objRelationEmergncyContact.GetType().GetProperty(split[1]).GetValue(genModel.objRelationEmergncyContact, null);
                                        if (split[0] == "objclsUpld")
                                            value = genModel.objclsUpld.GetType().GetProperty(split[1]).GetValue(genModel.objclsUpld, null);
                                    }
                                    else
                                    {
                                        value = genModel.GetType().GetProperty(textValue.ToString()).GetValue(genModel, null);
                                    }
                                }
                                catch
                                {

                                }
                                finally
                                {
                                }

                                //html += "<td colspan=" + colspan + " style='width: "+widthCell+"%;'><span class='fontwhite'>*<span><input id='" + id + "' name='" + textValue.ToString() + "' value='" + value + "' maxlength='" + maxLength + "' type='text' /></td>";
                                html += "<td colspan=" + colspan + " ><span class='fontwhite'>*</span><input id='" + id + "' name='" + textValue.ToString() + "' style='" + widthXml + "' value='" + Server.HtmlEncode(Convert.ToString(value)) + "' maxlength='" + maxLength + "' type='text' /></td>";
                            }
                            else
                            {
                                if (TempData["EditId"] == null)
                                {
                                    if (label == "Duration of Labor")
                                    {
                                        string funName = cell.Attributes["onkeypress"].Value;
                                        html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + id + "'  style='width:80px !important; margin-right:1px !important;' type='text' value='" + cell.InnerText + "' />Hours</td>";
                                    }
                                    else if (label == "Adopted age" || label == "Age when Applicant" || label == "Age of Onset")
                                    {
                                        string funName = cell.Attributes["onkeypress"].Value;
                                        html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + id + "'  style='width:167px !important;margin-right:1px !important;' onkeypress='" + funName + "' type='text' value='" + cell.InnerText + "' /></td>";
                                    }
                                    else if (label == " ")
                                    {
                                        string funName = cell.Attributes["onkeypress"].Value;
                                        html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + id + "'  style='width:80px !important;margin-right:1px !important; ' onkeypress='" + funName + "' type='text' value='" + cell.InnerText + "' />LBS</td>";
                                    }
                                    else
                                        html += "<td colspan=" + colspan + " ><span class='fontwhite'>*</span><input id='" + id + "' name='" + id + "' type='text' value='" + Server.HtmlEncode(cell.InnerText) + "' /></td>";
                                }
                                else
                                    if (label == "Duration of Labor")
                                    {
                                        string funName = cell.Attributes["onkeypress"].Value;
                                        html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + id + "'  style='width:80px !important; margin-right:1px !important;' onkeypress='" + funName + "' type='text' value='" + cell.InnerText + "' />Hours</td>";
                                    }
                                    else if (label == "Adopted age" || label == "Age when Applicant" || label == "Age of Onset")
                                    {
                                        string funName = cell.Attributes["onkeypress"].Value;
                                        html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + id + "'  style='width:167px !important;margin-right:1px !important;' onkeypress='" + funName + "'  type='text' value='" + cell.InnerText + "' />YRS</td>";
                                    }
                                    else if (label == "Birth WeightLBS")
                                    {
                                        string funName = cell.Attributes["onkeypress"].Value;
                                        html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + id + "'  style='width:80px !important;margin-right:1px !important; ' onkeypress='" + funName + "' type='text' value='" + cell.InnerText + "' />LBS</td>";
                                    }
                                    else
                                        html += "<td colspan=" + colspan + " ><span class='fontwhite'>*</span><input id='" + id + "' name='" + id + "' style='" + widthXml + "'  type='text' value='" + Server.HtmlEncode(cell.InnerText) + "' /></td>";
                            }
                        }


                        else if (type == "input[textvalidate]")
                        {
                            bool val = objClsRef.IsModel(cell);

                            if (val)
                            {
                                string className = cell.Attributes["className"].Value;
                                string textValue = cell.Attributes["modelName"].Value;
                                //string className = cell.Attributes["className"].Value;
                                string maxLength = cell.Attributes["MaxLength"].Value;
                                object value = null;
                                try
                                {

                                    string[] split = textValue.Split('.');
                                    if (split.Length > 1)
                                    {
                                        if (split[0] == "objRelationMthr")
                                            value = genModel.objRelationMthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationMthr, null);
                                        if (split[0] == "objRelationFthr")
                                            value = genModel.objRelationFthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationFthr, null);
                                        if (split[0] == "objRelationClose")
                                            if (genModel.objRelationClose != null) value = genModel.objRelationClose.GetType().GetProperty(split[1]).GetValue(genModel.objRelationClose, null);
                                        if (split[0] == "objPhysicianDetails")
                                            if (genModel.objPhysicianDetails != null) value = genModel.objPhysicianDetails.GetType().GetProperty(split[1]).GetValue(genModel.objPhysicianDetails, null);
                                        if (split[0] == "objInsuranceDetails")
                                            value = genModel.objInsuranceDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDetails, null);
                                        if (split[0] == "objInsuranceSecDetails")
                                            value = genModel.objInsuranceSecDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceSecDetails, null);
                                        if (split[0] == "objInsuranceDentalDetails")
                                            value = genModel.objInsuranceDentalDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDentalDetails, null);
                                        if (split[0] == "objRelationLegalGuardian")
                                            value = genModel.objRelationLegalGuardian.GetType().GetProperty(split[1]).GetValue(genModel.objRelationLegalGuardian, null);
                                        if (split[0] == "objRelationEmergncyContact")
                                            value = genModel.objRelationEmergncyContact.GetType().GetProperty(split[1]).GetValue(genModel.objRelationEmergncyContact, null);
                                        if (split[0] == "objclsUpld")
                                            value = genModel.objclsUpld.GetType().GetProperty(split[1]).GetValue(genModel.objclsUpld, null);
                                    }
                                    else
                                    {

                                        value = genModel.GetType().GetProperty(textValue.ToString()).GetValue(genModel, null);
                                    }
                                }
                                catch
                                {

                                }

                                if (className == "validate[required]" || className == "validate[required] namefield")
                                    html += "<td colspan=" + colspan + " ><span class='validate'>*</span><input id='" + id + "' name='" + textValue.ToString() + "' value='" + Server.HtmlEncode(Convert.ToString(value)) + "'  onpaste='PreventDef(event)' class='" + className + "' maxlength='" + maxLength + "' type='text' /></td>";

                                else
                                    html += "<td colspan=" + colspan + " ><span class='fontwhite'>*</span><input id='" + id + "' name='" + textValue.ToString() + "' value='" + Server.HtmlEncode(Convert.ToString(value)) + "'  onpaste='PreventDef(event)' class='" + className + "' maxlength='" + maxLength + "' type='text' /></td>";
                            }
                            else
                            {
                                string className = cell.Attributes["className"].Value;
                                if (className == "validate[required]" || className == "validate[required] namefield")
                                    html += "<td colspan=" + colspan + "><span class='validate'>*</span><input id='" + id + "' name='" + id + "' value='" + cell.InnerText + "' class='" + className + "' onpaste='PreventDef(event)' type='text' /></td>";
                                else
                                    html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + id + "' type='text' value='" + cell.InnerText + "'  class='" + className + "' onpaste='PreventDef(event)' /></td>";
                            }
                        }

                        else if (type == "input[textfunction]")
                        {
                            bool val = objClsRef.IsModel(cell);
                            if (val)
                            {
                                string maxLength = cell.Attributes["MaxLength"].Value;
                                string textValue = cell.Attributes["modelName"].Value;
                                string funName = cell.Attributes["onkeypress"].Value;

                                //if (textValue == "RefZipCode")
                                //{
                                //    classname = "validate[required]";
                                //}

                                object value = null;
                                try
                                {

                                    string[] split = textValue.Split('.');
                                    if (split.Length > 1)
                                    {
                                        if (split[0] == "objRelationMthr")
                                            value = genModel.objRelationMthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationMthr, null);
                                        if (split[0] == "objRelationFthr")
                                            value = genModel.objRelationFthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationFthr, null);
                                        if (split[0] == "objRelationClose")
                                            if (genModel.objRelationClose != null) value = genModel.objRelationClose.GetType().GetProperty(split[1]).GetValue(genModel.objRelationClose, null);
                                        if (split[0] == "objPhysicianDetails")
                                            if (genModel.objPhysicianDetails != null) value = genModel.objPhysicianDetails.GetType().GetProperty(split[1]).GetValue(genModel.objPhysicianDetails, null);
                                        if (split[0] == "objInsuranceDetails")
                                            value = genModel.objInsuranceDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDetails, null);
                                        if (split[0] == "objInsuranceSecDetails")
                                            value = genModel.objInsuranceSecDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceSecDetails, null);
                                        if (split[0] == "objInsuranceDentalDetails")
                                            value = genModel.objInsuranceDentalDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDentalDetails, null);
                                        if (split[0] == "objRelationLegalGuardian")
                                            value = genModel.objRelationLegalGuardian.GetType().GetProperty(split[1]).GetValue(genModel.objRelationLegalGuardian, null);
                                        if (split[0] == "objRelationEmergncyContact")
                                            value = genModel.objRelationEmergncyContact.GetType().GetProperty(split[1]).GetValue(genModel.objRelationEmergncyContact, null);
                                        if (split[0] == "objclsUpld")
                                            value = genModel.objclsUpld.GetType().GetProperty(split[1]).GetValue(genModel.objclsUpld, null);
                                    }
                                    else
                                    {
                                        value = genModel.GetType().GetProperty(textValue.ToString()).GetValue(genModel, null);
                                    }
                                }
                                catch
                                {

                                }


                                if (textValue == "RefZipCode")
                                {
                                    html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + textValue.ToString() + "' value='" + Server.HtmlEncode(Convert.ToString(value)) + "' maxlength='" + maxLength + "'  onkeypress='" + funName + "' type='text' /></td>";
                                }
                                else html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + textValue.ToString() + "' value='" + Server.HtmlEncode(Convert.ToString(value)) + "' maxlength='" + maxLength + "' onkeypress='" + funName + "' type='text' /></td>";


                            }
                            else
                            {
                                string funName = cell.Attributes["onkeypress"].Value;
                                html += "<td colspan=" + colspan + " ><span class='fontwhite'>*</span><input id='" + id + "' name='" + id + "' type='text' value='" + Server.HtmlEncode(cell.InnerText) + "' onkeypress='" + funName + "' /></td>";
                            }
                        }

                        else if (type == "input[textKeyType]")
                        {
                            bool val = objClsRef.IsModel(cell);
                            if (val)
                            {
                                string maxLength = cell.Attributes["MaxLength"].Value;
                                string textValue = cell.Attributes["modelName"].Value;
                                string funName = cell.Attributes["onkeypress"].Value;
                                string keyName = cell.Attributes["key"].Value;
                                string width = cell.Attributes["width"].Value;
                                object value = null;
                                try
                                {
                                    string[] split = textValue.Split('.');
                                    if (split.Length > 1)
                                    {
                                        if (split[0] == "objRelationMthr")
                                            value = genModel.objRelationMthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationMthr, null);
                                        if (split[0] == "objRelationFthr")
                                            value = genModel.objRelationFthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationFthr, null);
                                        if (split[0] == "objRelationClose")
                                            if (genModel.objRelationClose != null) value = genModel.objRelationClose.GetType().GetProperty(split[1]).GetValue(genModel.objRelationClose, null);
                                        if (split[0] == "objPhysicianDetails")
                                            if (genModel.objPhysicianDetails != null) value = genModel.objPhysicianDetails.GetType().GetProperty(split[1]).GetValue(genModel.objPhysicianDetails, null);
                                        if (split[0] == "objInsuranceDetails")
                                            value = genModel.objInsuranceDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDetails, null);
                                        if (split[0] == "objInsuranceSecDetails")
                                            value = genModel.objInsuranceSecDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceSecDetails, null);
                                        if (split[0] == "objInsuranceDentalDetails")
                                            value = genModel.objInsuranceDentalDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDentalDetails, null);
                                        if (split[0] == "objRelationLegalGuardian")
                                            value = genModel.objRelationLegalGuardian.GetType().GetProperty(split[1]).GetValue(genModel.objRelationLegalGuardian, null);
                                        if (split[0] == "objRelationEmergncyContact")
                                            value = genModel.objRelationEmergncyContact.GetType().GetProperty(split[1]).GetValue(genModel.objRelationEmergncyContact, null);
                                        if (split[0] == "objclsUpld")
                                            value = genModel.objclsUpld.GetType().GetProperty(split[1]).GetValue(genModel.objclsUpld, null);
                                    }
                                    else
                                    {
                                        value = genModel.GetType().GetProperty(textValue.ToString()).GetValue(genModel, null);
                                    }
                                }
                                catch
                                {

                                }

                                html += "<td colspan=" + colspan + " ><span class='fontwhite'>*</span><input id='" + id + "' name='" + textValue.ToString() + "' value='" + Server.HtmlEncode(Convert.ToString(value)) + "' style='width: " + width + ";margin-right:0px;' maxlength='" + maxLength + "' onkeypress='" + funName + "' type='text' /><span style='color:#595959;'>" + keyName + "</span></td>";

                            }
                            else
                                html += "<td colspan=" + colspan + " ><span class='fontwhite'>*</span><input id='" + id + "' name='" + id + "' type='text' value='" + Server.HtmlEncode(cell.InnerText) + "' /></td>";
                        }
                        else if (type == "input[textdate]")
                        {
                            html += "<td colspan=" + colspan + " ><span class='fontwhite'>*</span><input id='" + id + "' value='" + Server.HtmlEncode(Convert.ToString(cell.InnerText)) + "'  name='" + id + "' type='text' class='datepicker' onkeypress='return false' /></td>";
                        }

                        else if (type == "input[multitext]")
                        {
                            bool val = objClsRef.IsModel(cell);
                            string width = cell.Attributes["Width"].Value;
                            if (val)
                            {
                                string textValue = cell.Attributes["modelName"].Value;
                                // string width = cell.Attributes["Width"].Value;
                                object value = null;
                                try
                                {
                                    string[] split = textValue.Split('.');
                                    if (split.Length > 1)
                                    {
                                        if (split[0] == "objRelationMthr")
                                            value = genModel.objRelationMthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationMthr, null);
                                        if (split[0] == "objRelationFthr")
                                            value = genModel.objRelationFthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationFthr, null);
                                        if (split[0] == "objRelationClose")
                                            if (genModel.objRelationClose != null) value = genModel.objRelationClose.GetType().GetProperty(split[1]).GetValue(genModel.objRelationClose, null);
                                        if (split[0] == "objPhysicianDetails")
                                            if (genModel.objPhysicianDetails != null) value = genModel.objPhysicianDetails.GetType().GetProperty(split[1]).GetValue(genModel.objPhysicianDetails, null);
                                        if (split[0] == "objInsuranceDetails")
                                            value = genModel.objInsuranceDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDetails, null);
                                        if (split[0] == "objInsuranceSecDetails")
                                            value = genModel.objInsuranceSecDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceSecDetails, null);
                                        if (split[0] == "objInsuranceDentalDetails")
                                            value = genModel.objInsuranceDentalDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDentalDetails, null);
                                        if (split[0] == "objRelationLegalGuardian")
                                            value = genModel.objRelationLegalGuardian.GetType().GetProperty(split[1]).GetValue(genModel.objRelationLegalGuardian, null);

                                        if (split[0] == "objRelationEmergncyContact")
                                            value = genModel.objRelationEmergncyContact.GetType().GetProperty(split[1]).GetValue(genModel.objRelationEmergncyContact, null);

                                        if (split[0] == "objclsUpld")
                                            value = genModel.objclsUpld.GetType().GetProperty(split[1]).GetValue(genModel.objclsUpld, null);
                                    }
                                    else
                                    {
                                        value = genModel.GetType().GetProperty(textValue.ToString()).GetValue(genModel, null);
                                    }
                                }
                                catch
                                {

                                }

                                html += "<td colspan=" + colspan + " ><span class='fontwhite'>*</span><textarea id='" + id + "' name='" + textValue.ToString() + "' value='" + Server.HtmlEncode(Convert.ToString(value)) + "' type='text' style='width:" + width + ";' rows='3'>" + Server.HtmlEncode(Convert.ToString(value)) + "</textarea></td>";
                            }
                            else
                                html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><textarea id='" + id + "' name='" + id + "' type='text' value='" + Server.HtmlEncode(Convert.ToString(cell.InnerText)) + "' style='width:" + width + ";' rows='3'>" + Server.HtmlEncode(Convert.ToString(cell.InnerText)) + "</textarea></td>";
                        }
                        else if (type == "input[textDate]")
                        {
                            bool val = objClsRef.IsModel(cell);
                            if (val)
                            {
                                object value = null;
                                string textValue = cell.Attributes["modelName"].Value;

                                try
                                {
                                    string[] split = textValue.Split('.');
                                    if (split.Length > 1)
                                    {
                                        if (split[0] == "objRelationMthr")
                                            value = genModel.objRelationMthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationMthr, null);
                                        if (split[0] == "objRelationFthr")
                                            value = genModel.objRelationFthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationFthr, null);
                                        if (split[0] == "objRelationClose")
                                            if (genModel.objRelationClose != null) value = genModel.objRelationClose.GetType().GetProperty(split[1]).GetValue(genModel.objRelationClose, null);
                                        if (split[0] == "objPhysicianDetails")
                                            if (genModel.objPhysicianDetails != null) value = genModel.objPhysicianDetails.GetType().GetProperty(split[1]).GetValue(genModel.objPhysicianDetails, null);
                                        if (split[0] == "objInsuranceDetails")
                                            value = genModel.objInsuranceDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDetails, null);
                                        if (split[0] == "objInsuranceSecDetails")
                                            value = genModel.objInsuranceSecDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceSecDetails, null);
                                        if (split[0] == "objInsuranceDentalDetails")
                                            value = genModel.objInsuranceDentalDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDentalDetails, null);
                                        if (split[0] == "objRelationLegalGuardian")
                                            value = genModel.objRelationLegalGuardian.GetType().GetProperty(split[1]).GetValue(genModel.objRelationLegalGuardian, null);
                                        if (split[0] == "objRelationEmergncyContact")
                                            value = genModel.objRelationEmergncyContact.GetType().GetProperty(split[1]).GetValue(genModel.objRelationEmergncyContact, null);

                                        if (split[0] == "objclsUpld")
                                            value = genModel.objclsUpld.GetType().GetProperty(split[1]).GetValue(genModel.objclsUpld, null);
                                    }
                                    else
                                    {
                                        value = genModel.GetType().GetProperty(textValue.ToString()).GetValue(genModel, null);
                                    }
                                }
                                catch
                                {


                                }

                                if (textValue == "RefDate" || textValue == "RefDOB")
                                {
                                    html += "<td colspan=" + colspan + " ><span class='validate'>*<span><input id='" + id + "' name='" + textValue.ToString() + "' class='validate[required] datepicker' onkeypress='return false' value='" + Server.HtmlEncode(Convert.ToString(value)) + "' type='text' /></td>";
                                }
                                else
                                    html += "<td colspan=" + colspan + " ><span class='fontwhite'>*<span><input id='" + id + "' name='" + textValue.ToString() + "' class='datepicker' onkeypress='return false' value='" + Server.HtmlEncode(Convert.ToString(value)) + "' type='text' /></td>";
                            }
                            else
                                html += "<td colspan=" + colspan + " ><span class='fontwhite'>*<span><input id='" + id + "' name='" + id + "'  class='datepicker' onkeypress='return false' value='" + Server.HtmlEncode(cell.InnerText) + "' type='text' /></td>";
                        }

                        //else if (type == "input[button]")
                        //{
                        //    if (isUpated == 1)
                        //    {
                        //        html += "<td colspan=" + colspan + " style='width: 20%; text-align:right;'><input id='" + id + "' value='Update' type='submit' class='NFButton' /></td>";
                        //    }
                        //    else
                        //    {
                        //        html += "<td colspan=" + colspan + " style='width: 20%; text-align:right;'><input id='" + id + "' value='" + label + "' type='submit' class='NFButton' /></td>";
                        //    }

                        //    //html += "<td colspan=" + colspan + " style='width: 20%; text-align:right;'><input id='" + id + "' value='" + (isUpated == 1 ? "update" : "save") + "' type='submit' class='NFButton' /></td>";
                        //}
                        else if (type == "input[file]")
                        {
                            string name = cell.Attributes["label"].Value.ToString();
                            string nameFile = cell.Attributes["name"].Value.ToString();
                            if (name == "Upload Photo")
                                html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + nameFile + "' type='file' style='width:160px !important' /></td>";
                            else
                                html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + nameFile + "' type='file' style='width:210px !important' /></td>";

                        }
                        else if (type == "input[check]")
                        {
                            countCheck = Convert.ToInt32(cell.Attributes["Count"].Value);
                            string[] idSplit = id.Split('|');
                            string[] lblSplit = label.Split('|');
                            html += "<td colspan=" + colspan + " >";
                            for (int i = 0; i < countCheck; i++)
                            {
                                html += "<input id='" + idSplit[i] + "' type='checkbox'>" + lblSplit[i] + "</input><br>";
                            }
                            html += "</td>";

                        }
                        else if (type == "lblBold")
                        {
                            html += "<td class='lblBold' colspan=" + colspan + " style='width: 100%;'>" + label + "</td>";
                        }
                        else if (type == "input[checkVt]")
                        {
                            countCheck = Convert.ToInt32(cell.Attributes["Count"].Value);
                            string[] idSpl = id.Split('|');
                            string[] lblsplit = label.Split('|');
                            html += "<td colspan=" + colspan + " >";
                            for (int i = 0; i < countCheck; i++)
                            {
                                html += "<input id='" + idSpl[i] + "' type='checkbox' value='unchecked' name='" + idSpl[i] + "' onclick='GetSelected(this);' !checked><span>" + lblsplit[i] + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>";
                            }
                            html += "</td>";

                        }


                        else if (type == "input[radio]")
                        {
                            bool val = objClsRef.IsModel(cell);
                            if (val)
                            {
                                IEnumerable<SelectListItem> listMar = new List<SelectListItem>();
                                if (label == "MaritalStatus")
                                {
                                    string textValue = cell.Attributes["modelName"].Value;
                                    listMar = objClsRef.FillMaritalStatus();
                                    if (textValue == "objRelationMthr.txtMaritalStatus")
                                    {
                                        html += "<td colspan=" + colspan + " >";
                                        foreach (SelectListItem item in listMar)
                                        {
                                            if (genModel.objRelationMthr.txtMaritalStatus.ToString() == item.Value)
                                            {
                                                html += "<input id='" + item.Value + "' type='radio' value='" + item.Value + "' name='" + textValue + "' checked><label>" + item.Text + "</label></input>";
                                            }
                                            else
                                            {
                                                html += "<input id='" + item.Value + "' type='radio' value='" + item.Value + "' name='" + textValue + "'><label>" + item.Text + "</label></input>";
                                            }
                                        }
                                        html += "</td>";

                                    }

                                    else if (textValue == "objRelationFthr.txtMaritalStatus")
                                    {
                                        html += "<td colspan=" + colspan + " >";
                                        foreach (SelectListItem item in listMar)
                                        {
                                            if (genModel.objRelationFthr.txtMaritalStatus.ToString() == item.Value)
                                            {
                                                html += "<input id='" + item.Value + "' type='radio' value='" + item.Value + "' name='" + textValue + "' checked><label>" + item.Text + "</label></input>";
                                            }
                                            else
                                            {
                                                html += "<input id='" + item.Value + "' type='radio' value='" + item.Value + "' name='" + textValue + "'><label>" + item.Text + "</label></input>";
                                            }
                                        }
                                        html += "</td>";

                                    }


                                    else
                                    {
                                        html += "<td colspan=" + colspan + " >";

                                        foreach (SelectListItem item in listMar)
                                        {
                                            html += "<input id='" + item.Value + "' type='radio' value='" + item.Value + "' name='" + textValue + "'><label>" + item.Text + "</label></input>";
                                        }
                                        html += "</td>";
                                    }

                                }

                            }
                            else
                            {
                                string name = cell.Attributes["name"].Value.ToString();
                                countCheck = Convert.ToInt32(cell.Attributes["Count"].Value);
                                string[] idSpl = id.Split('|');
                                string[] lblsplit = label.Split('|');
                                html += "<td colspan=" + colspan + " >";
                                for (int i = 0; i < countCheck; i++)
                                {
                                    html += "<input id='" + idSpl[i] + "' type='radio' value='unchecked' name='" + name + "' onclick='GetSelected(this);' !checked><span>" + lblsplit[i] + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>";
                                }
                                html += "</td>";
                            }
                        }

                        else if (type == "input[dropRel]")
                        {
                            IEnumerable<SelectListItem> ListData = new List<SelectListItem>();
                            bool val = objClsRef.IsModel(cell);
                            if (val)
                            {

                                if (label == "Relation Status")
                                {
                                    ListData = objClsRef.FillDropList("Relationship");
                                    string textValue = cell.Attributes["modelName"].Value;
                                    html += "<td colspan=" + colspan + " ><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class=''>";
                                    html += "<option value=''>Select " + label + "</option>";

                                    foreach (SelectListItem Item in ListData)
                                    {
                                        if (textValue == "objRelationClose.closeRelation")
                                        {
                                            if (genModel.objRelationClose.closeRelation.ToString() == Item.Value)
                                            {
                                                html += "<option value='" + Item.Value + "' selected='true'>" + Item.Text + "</option>";
                                            }
                                            else
                                            {
                                                html += "<option value='" + Item.Value + "'>" + Item.Text + "</option>";
                                            }
                                        }

                                        else if (textValue == "objRelationLegalGuardian.LgRelationName")
                                        {
                                            if (genModel.objRelationLegalGuardian.LgRelationName.ToString() == Item.Value)
                                            {
                                                html += "<option value='" + Item.Value + "' selected='true'>" + Item.Text + "</option>";
                                            }
                                            else
                                            {
                                                html += "<option value='" + Item.Value + "'>" + Item.Text + "</option>";
                                            }
                                        }

                                        else if (textValue == "objRelationEmergncyContact.EmRelationName")
                                        {
                                            if (genModel.objRelationEmergncyContact.EmRelationName.ToString() == Item.Value)
                                            {
                                                html += "<option value='" + Item.Value + "' selected='true'>" + Item.Text + "</option>";
                                            }
                                            else
                                            {
                                                html += "<option value='" + Item.Value + "'>" + Item.Text + "</option>";
                                            }
                                        }
                                    }

                                   }
                                }
                            
                            else
                            {

                            }
                        }


                        else if (type == "input[dropDocType]")
                        {
                            IEnumerable<SelectListItem> ListData = new List<SelectListItem>();
                            bool val = objClsRef.IsModel(cell);
                            if (val)
                            {

                                if (label == "DocumentType")
                                {
                                    ListData = objClsRef.FillDoccumentType();
                                    string textValue = cell.Attributes["modelName"].Value;
                                    html += "<td colspan=" + colspan + " ><span class='validate'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='validate[required]'>";
                                    html += "<option value='' selected='true'>Select " + label + "</option>";
                                    foreach (SelectListItem Item in ListData)
                                    {
                                        if (textValue == "objclsUpld.DocType")
                                        {
                                            if (genModel.objclsUpld.DocType.ToString() == Item.Value)
                                            {
                                                html += "<option value='" + Item.Value + "' selected='true'>" + Item.Text + "</option>";
                                            }
                                            else
                                            {
                                                html += "<option value='" + Item.Value + "'>" + Item.Text + "</option>";
                                            }
                                        }
                                    }

                                }
                            }
                            else
                            {
                            }
                        }

                        else if (type == "input[drop]")
                        {
                            IEnumerable<SelectListItem> listData = new List<SelectListItem>();
                            IEnumerable<SelectListItem> listDataState = new List<SelectListItem>();
                            if (label == "Country")
                            {
                                listData = objClsRef.FillDropList("Country");
                            }
                            bool val = objClsRef.IsModel(cell);
                            if (val)
                            {

                                if (label == "Country")
                                {
                                    string textValue = cell.Attributes["modelName"].Value;
                                    string stateVal = "0";

                                    if (textValue == "RefCountry")
                                    {
                                        html += "<td colspan=" + colspan + " ><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='ddlRefCountry' >";// onchange='GetState(this.id," + stateVal + ")'
                                        //html += "<option value=''>Select " + label + "</option>";
                                    }
                                    else
                                    {
                                        html += "<td colspan=" + colspan + " ><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='ddlOtherCountry' >";//onchange='GetState(this.id," + stateVal + ")'
                                        //html += "<option value=0>Select " + label + "</option>";
                                    }



                                    foreach (SelectListItem list in listData)
                                    {

                                        if (textValue == "objRelationMthr.txtCountry")
                                        {
                                            if (genModel.objRelationMthr != null)
                                            {
                                                if (genModel.objRelationMthr.txtCountry.ToString() == list.Value)
                                                {
                                                    html += "<option value='" + list.Value + "' selected='true'>" + list.Text + "</option>";

                                                }
                                                else
                                                {
                                                    html += "<option value='" + list.Value + "'>" + list.Text + "</option>";
                                                }
                                            }
                                            else
                                            {
                                                html += "<option value='" + list.Value + "'>" + list.Text + "</option>";
                                            }
                                        }
                                        else if (textValue == "objRelationFthr.txtCountry")
                                        {
                                            if (genModel.objRelationFthr.txtCountry != null)
                                            {
                                                if (genModel.objRelationFthr.txtCountry.ToString() == list.Value)
                                                {
                                                    html += "<option value='" + list.Value + "' selected='true'>" + list.Text + "</option>";
                                                }
                                                else
                                                {
                                                    html += "<option value='" + list.Value + "'>" + list.Text + "</option>";
                                                }
                                            }
                                            else
                                            {
                                                html += "<option value='" + list.Value + "'>" + list.Text + "</option>";
                                            }
                                        }
                                        else if (textValue == "objRelationClose.txtCountry")
                                        {
                                            if (genModel.objRelationClose != null)
                                            {
                                                if (genModel.objRelationClose.txtCountry != null)
                                                {
                                                    if (genModel.objRelationClose.txtCountry.ToString() == list.Value)
                                                    {
                                                        html += "<option value='" + list.Value + "' selected='true'>" + list.Text + "</option>";
                                                    }
                                                    else
                                                    {
                                                        html += "<option value='" + list.Value + "'>" + list.Text + "</option>";
                                                    }
                                                }
                                                else
                                                {
                                                    html += "<option value='" + list.Value + "'>" + list.Text + "</option>";
                                                }
                                            }
                                        }
                                        else if (textValue == "objPhysicianDetails.txtCountry")
                                        {
                                            if (genModel.objPhysicianDetails != null)
                                            {
                                                //if (genModel.objPhysicianDetails.txtCountry != null)
                                                //{
                                                if (genModel.objPhysicianDetails.txtCountry.ToString() == list.Value)
                                                {
                                                    html += "<option value='" + list.Value + "' selected='true'>" + list.Text + "</option>";
                                                }
                                                else
                                                {
                                                    html += "<option value='" + list.Value + "'>" + list.Text + "</option>";
                                                }
                                                //}
                                            }
                                            else
                                            {
                                                html += "<option value='" + list.Value + "'>" + list.Text + "</option>";
                                            }
                                        }
                                        else if (textValue == "objInsuranceDetails.txtCountry")
                                        {
                                            if (genModel.objInsuranceDetails != null)
                                            {
                                                //if (genModel.objInsuranceDetails.txtCountry != null)
                                                //{
                                                if (genModel.objInsuranceDetails.txtCountry.ToString() == list.Value)
                                                {
                                                    html += "<option value='" + list.Value + "' selected='true'>" + list.Text + "</option>";
                                                }
                                                else
                                                {
                                                    html += "<option value='" + list.Value + "'>" + list.Text + "</option>";
                                                }
                                                // }
                                            }
                                            else
                                            {
                                                html += "<option value='" + list.Value + "'>" + list.Text + "</option>";
                                            }
                                        }
                                        else if (textValue == "objInsuranceSecDetails.txtCountry")
                                        {
                                            if (genModel.objInsuranceDetails != null)
                                            {
                                                //if (genModel.objInsuranceSecDetails.txtCountry != null)
                                                //{
                                                if (genModel.objInsuranceSecDetails.txtCountry.ToString() == list.Value)
                                                {
                                                    html += "<option value='" + list.Value + "' selected='true'>" + list.Text + "</option>";
                                                }
                                                else
                                                {
                                                    html += "<option value='" + list.Value + "'>" + list.Text + "</option>";
                                                }
                                                //}
                                            }
                                            else
                                            {
                                                html += "<option value='" + list.Value + "'>" + list.Text + "</option>";
                                            }
                                        }
                                        else if (textValue == "objInsuranceDentalDetails.txtCountry")
                                        {
                                            if (genModel.objInsuranceDetails != null)
                                            {
                                                //if (genModel.objInsuranceDentalDetails.txtCountry != null)
                                                //{
                                                if (genModel.objInsuranceDentalDetails.txtCountry.ToString() == list.Value)
                                                {
                                                    html += "<option value='" + list.Value + "' selected='true'>" + list.Text + "</option>";
                                                }
                                                else
                                                {
                                                    html += "<option value='" + list.Value + "'>" + list.Text + "</option>";
                                                }
                                                // }
                                            }
                                            else
                                            {
                                                html += "<option value='" + list.Value + "'>" + list.Text + "</option>";
                                            }
                                        }

                                        else if (textValue == "objRelationLegalGuardian.txtCountry")
                                        {
                                            if (genModel.objRelationLegalGuardian.txtCountry != null)
                                            {
                                                if (genModel.objRelationLegalGuardian.txtCountry.ToString() == list.Value)
                                                {
                                                    html += "<option value='" + list.Value + "' selected='true'>" + list.Text + "</option>";
                                                }
                                                else
                                                {
                                                    html += "<option value='" + list.Value + "'>" + list.Text + "</option>";
                                                }
                                            }
                                            else
                                            {
                                                html += "<option value='" + list.Value + "'>" + list.Text + "</option>";
                                            }
                                        }
                                        else if (textValue == "objRelationEmergncyContact.txtCountry")
                                        {
                                            if (genModel.objRelationEmergncyContact.txtCountry != null)
                                            {
                                                if (genModel.objRelationEmergncyContact.txtCountry.ToString() == list.Value)
                                                {
                                                    html += "<option value='" + list.Value + "' selected='true'>" + list.Text + "</option>";
                                                }
                                                else
                                                {
                                                    html += "<option value='" + list.Value + "'>" + list.Text + "</option>";
                                                }
                                            }
                                            else
                                            {
                                                html += "<option value='" + list.Value + "'>" + list.Text + "</option>";
                                            }
                                        }
                                        else if (textValue == "RefCntryBirth")
                                        {
                                            if (genModel.RefCntryBirth != null)
                                            {
                                                if (genModel.RefCntryBirth.ToString() == list.Value)
                                                {
                                                    html += "<option value='" + list.Value + "' selected='true'>" + list.Text + "</option>";
                                                }
                                                else
                                                {
                                                    html += "<option value='" + list.Value + "'>" + list.Text + "</option>";
                                                }
                                            }
                                            else
                                            {
                                                html += "<option value='" + list.Value + "'>" + list.Text + "</option>";
                                            }
                                        }
                                        else
                                        {
                                            if (genModel.RefCountry.ToString() == list.Value)
                                            {
                                                html += "<option value='" + list.Value + "' selected='true'>" + list.Text + "</option>";

                                            }
                                            else
                                            {
                                                html += "<option value='" + list.Value + "'>" + list.Text + "</option>";
                                            }
                                        }
                                    }

                                    html += "</select></td>";
                                }
                                else if (label == "State")
                                {

                                    string textValue = cell.Attributes["modelName"].Value;

                                    if (textValue == "RefState")
                                    {
                                        html += "<td colspan=" + colspan + " ><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
                                        html += "<option value=0>Select " + label + "</option>";
                                        int countryId = Convert.ToInt32(genModel.RefCountry);
                                        listDataState = objClsRef.FillState(countryId);

                                        foreach (SelectListItem listSt in listDataState)
                                        {
                                            if (textValue == "RefState")
                                            {
                                                if (genModel.RefState.ToString() == listSt.Value)
                                                {
                                                    html += "<option value='" + listSt.Value + "' selected='true'>" + listSt.Text + "</option>";
                                                }
                                                else
                                                {
                                                    html += "<option value='" + listSt.Value + "'>" + listSt.Text + "</option>";
                                                }
                                            }
                                        }
                                        html += "</select></td>";
                                    }

                                    else if (textValue == "objRelationMthr.txtState")
                                    {
                                        int countryId = Convert.ToInt32(genModel.objRelationMthr.txtCountry);
                                        listDataState = objClsRef.FillState(countryId);
                                        html += "<td colspan=" + colspan + " ><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
                                        html += "<option value=0>Select " + label + "</option>";
                                        foreach (SelectListItem listSt in listDataState)
                                        {
                                            if (textValue == "objRelationMthr.txtState")
                                            {
                                                if (genModel.objRelationMthr.txtState.ToString() == listSt.Value)
                                                {
                                                    html += "<option value='" + listSt.Value + "' selected='true'>" + listSt.Text + "</option>";
                                                }
                                                else
                                                {
                                                    html += "<option value='" + listSt.Value + "'>" + listSt.Text + "</option>";
                                                }
                                            }
                                        }
                                        html += "</select></td>";
                                    }

                                    else if (textValue == "objRelationFthr.txtState")
                                    {
                                        int countryId = Convert.ToInt32(genModel.objRelationFthr.txtCountry);
                                        listDataState = objClsRef.FillState(countryId);
                                        html += "<td colspan=" + colspan + " ><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
                                        html += "<option value=0>Select " + label + "</option>";
                                        foreach (SelectListItem listSt in listDataState)
                                        {
                                            if (textValue == "objRelationFthr.txtState")
                                            {
                                                if (genModel.objRelationFthr.txtState.ToString() == listSt.Value)
                                                {
                                                    html += "<option value='" + listSt.Value + "' selected='true'>" + listSt.Text + "</option>";
                                                }
                                                else
                                                {
                                                    html += "<option value='" + listSt.Value + "'>" + listSt.Text + "</option>";
                                                }
                                            }
                                        }
                                        html += "</select></td>";
                                    }

                                    else if (textValue == "objRelationClose.txtState")
                                    {
                                        if (genModel.objRelationClose != null)
                                        {
                                            int countryId = Convert.ToInt32(genModel.objRelationClose.txtCountry);
                                            listDataState = objClsRef.FillState(countryId);
                                            html += "<td colspan=" + colspan + " ><span style='color: white;'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
                                            html += "<option value=0>Select " + label + "</option>";
                                            foreach (SelectListItem listSt in listDataState)
                                            {
                                                if (textValue == "objRelationClose.txtState")
                                                {
                                                    if (genModel.objRelationClose.txtState.ToString() == listSt.Value)
                                                    {
                                                        html += "<option value='" + listSt.Value + "' selected='true'>" + listSt.Text + "</option>";
                                                    }
                                                    else
                                                    {
                                                        html += "<option value='" + listSt.Value + "'>" + listSt.Text + "</option>";
                                                    }
                                                }
                                            }
                                        }
                                        html += "</select></td>";
                                    }

                                    else if (textValue == "objRelationLegalGuardian.txtState")
                                    {
                                        int countryId = Convert.ToInt32(genModel.objRelationLegalGuardian.txtCountry);
                                        listDataState = objClsRef.FillState(countryId);
                                        html += "<td colspan=" + colspan + " ><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
                                        html += "<option value=0>Select " + label + "</option>";
                                        foreach (SelectListItem listSt in listDataState)
                                        {
                                            if (textValue == "objRelationLegalGuardian.txtState")
                                            {
                                                if (genModel.objRelationLegalGuardian.txtState.ToString() == listSt.Value)
                                                {
                                                    html += "<option value='" + listSt.Value + "' selected='true'>" + listSt.Text + "</option>";
                                                }
                                                else
                                                {
                                                    html += "<option value='" + listSt.Value + "'>" + listSt.Text + "</option>";
                                                }
                                            }
                                        }
                                        html += "</select></td>";
                                    }

                                    else if (textValue == "objRelationEmergncyContact.txtState")
                                    {
                                        int countryId = Convert.ToInt32(genModel.objRelationEmergncyContact.txtCountry);
                                        listDataState = objClsRef.FillState(countryId);
                                        html += "<td colspan=" + colspan + " ><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
                                        html += "<option value=0>Select " + label + "</option>";
                                        foreach (SelectListItem listSt in listDataState)
                                        {
                                            if (textValue == "objRelationEmergncyContact.txtState")
                                            {
                                                if (genModel.objRelationEmergncyContact.txtState.ToString() == listSt.Value)
                                                {
                                                    html += "<option value='" + listSt.Value + "' selected='true'>" + listSt.Text + "</option>";
                                                }
                                                else
                                                {
                                                    html += "<option value='" + listSt.Value + "'>" + listSt.Text + "</option>";
                                                }
                                            }
                                        }
                                        html += "</select></td>";
                                    }

                                    else if (textValue == "RefStateBirth")
                                    {
                                        int countryId = objClsRef.countryID();
                                        listDataState = objClsRef.FillState(countryId);
                                        html += "<td colspan=" + colspan + " ><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
                                        html += "<option value=0>Select " + label + "</option>";
                                        foreach (SelectListItem listSt in listDataState)
                                        {
                                            if (textValue == "RefStateBirth")
                                            {
                                                if (genModel.RefStateBirth.ToString() == listSt.Value)
                                                {
                                                    html += "<option value='" + listSt.Value + "' selected='true'>" + listSt.Text + "</option>";
                                                }
                                                else
                                                {
                                                    html += "<option value='" + listSt.Value + "'>" + listSt.Text + "</option>";
                                                }
                                            }
                                        }
                                        html += "</select></td>";
                                    }

                                    else if (textValue == "objPhysicianDetails.txtState")
                                    {
                                        int countryId = 0;
                                        if (genModel.objPhysicianDetails != null) countryId = Convert.ToInt32(genModel.objPhysicianDetails.txtCountry);
                                        if (countryId != 0) listDataState = objClsRef.FillState(countryId);
                                        html += "<td colspan=" + colspan + " ><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
                                        html += "<option value=0>Select " + label + "</option>";
                                        foreach (SelectListItem listSt in listDataState)
                                        {
                                            if (textValue == "objPhysicianDetails.txtState")
                                            {
                                                if (genModel.objPhysicianDetails.txtState.ToString() == listSt.Value)
                                                {
                                                    html += "<option value='" + listSt.Value + "' selected='true'>" + listSt.Text + "</option>";
                                                }
                                                else
                                                {
                                                    html += "<option value='" + listSt.Value + "'>" + listSt.Text + "</option>";
                                                }
                                            }
                                        }
                                        html += "</select></td>";
                                    }


                                    else if (textValue == "objInsuranceDetails.txtState")
                                    {
                                        int countryId = 0;
                                        if (genModel.objInsuranceDetails != null) countryId = Convert.ToInt32(genModel.objInsuranceDetails.txtCountry);
                                        listDataState = objClsRef.FillState(countryId);
                                        html += "<td colspan=" + colspan + " ><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
                                        html += "<option value=0>Select " + label + "</option>";
                                        foreach (SelectListItem listSt in listDataState)
                                        {
                                            if (textValue == "objInsuranceDetails.txtState")
                                            {
                                                if (genModel.objInsuranceDetails.txtState.ToString() == listSt.Value)
                                                {
                                                    html += "<option value='" + listSt.Value + "' selected='true'>" + listSt.Text + "</option>";
                                                }
                                                else
                                                {
                                                    html += "<option value='" + listSt.Value + "'>" + listSt.Text + "</option>";
                                                }
                                            }
                                        }
                                        html += "</select></td>";
                                    }

                                    else if (textValue == "objInsuranceSecDetails.txtState")
                                    {
                                        int countryId = 0;
                                        if (genModel.objInsuranceSecDetails != null) countryId = Convert.ToInt32(genModel.objInsuranceSecDetails.txtCountry);
                                        listDataState = objClsRef.FillState(countryId);
                                        html += "<td colspan=" + colspan + " ><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
                                        html += "<option value=0>Select " + label + "</option>";
                                        foreach (SelectListItem listSt in listDataState)
                                        {
                                            if (textValue == "objInsuranceSecDetails.txtState")
                                            {
                                                if (genModel.objInsuranceSecDetails.txtState.ToString() == listSt.Value)
                                                {
                                                    html += "<option value='" + listSt.Value + "' selected='true'>" + listSt.Text + "</option>";
                                                }
                                                else
                                                {
                                                    html += "<option value='" + listSt.Value + "'>" + listSt.Text + "</option>";
                                                }
                                            }
                                        }
                                        html += "</select></td>";
                                    }


                                    else if (textValue == "objInsuranceDentalDetails.txtState")
                                    {
                                        int countryId = 0;
                                        if (genModel.objInsuranceSecDetails != null) countryId = Convert.ToInt32(genModel.objInsuranceDentalDetails.txtCountry);
                                        listDataState = objClsRef.FillState(countryId);
                                        html += "<td colspan=" + colspan + " ><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
                                        html += "<option value=0>Select " + label + "</option>";
                                        foreach (SelectListItem listSt in listDataState)
                                        {
                                            if (textValue == "objInsuranceDentalDetails.txtState")
                                            {
                                                if (genModel.objInsuranceDentalDetails.txtState.ToString() == listSt.Value)
                                                {
                                                    html += "<option value='" + listSt.Value + "' selected='true'>" + listSt.Text + "</option>";
                                                }
                                                else
                                                {
                                                    html += "<option value='" + listSt.Value + "'>" + listSt.Text + "</option>";
                                                }
                                            }
                                        }
                                        html += "</select></td>";
                                    }

                                    else
                                    {
                                        html += "<td colspan=" + colspan + " ><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
                                        html += "<option value=0>Select " + label + "</option>";
                                        html += "</select></td>";
                                    }



                                }
                                else if (label == "Gender")
                                {
                                    string textValue = cell.Attributes["modelName"].Value;

                                    html += "<td colspan=" + colspan + " ><span class='validate'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='validate[required]'>";
                                    if (genModel.RefGender == "1")
                                    {
                                        html += "<option value=''>Select Gender</option><option selected='true' value=1>Male</option><option value=2>Female</option>";
                                    }
                                    else if (genModel.RefGender == "2")
                                    {
                                        html += "<option value=''>Select Gender</option><option value=1>Male</option><option selected='true' value=2>Female</option>";
                                    }
                                    else
                                    {
                                        html += "<option value=''>Select Gender</option><option value=1>Male</option><option value=2>Female</option>";
                                    }


                                    html += "</select></td>";

                                }
                                else
                                {
                                    string textValue = cell.Attributes["modelName"].Value;

                                    html += "<td colspan=" + colspan + " ><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass2'>";
                                    html += "<option value=0>Select " + label + "</option>";
                                    html += "</select></td>";
                                }
                            }
                            else if (type == "td")
                            {
                                html += "<td colspan=" + colspan + " ></td>";
                            }

                            else
                            {

                                countCheck = Convert.ToInt32(cell.Attributes["Count"].Value);
                                string[] lblsplit = label.Split('|');
                                html += "<td colspan=" + colspan + " ><select id='" + id + "' name='" + id + "' class='drpClass1'><option value=0>-----Select.....</option>";
                                for (int i = 0; i < countCheck; i++)
                                {
                                    html += "<option value='" + lblsplit[i] + "'>" + lblsplit[i] + "</option>";

                                    //html += "<input id='" + id + "' type='checkbox' value='unchecked' name='" + idSpl[i] + "' onclick='GetSelected(this);' !checked><span>" + lblsplit[i] + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>";

                                }
                                html += "</select></td>";
                            }
                        }


                    }
                    html += "</tr>";
                }
            }



            else
            {
                //if (Index == 5) isUpated = 1;
                //if (Index == 6) isUpated = 1;
                //if (Index == 7) isUpated = 1; 



                XmlDocument xdocCustomEdit = new XmlDocument();
                int studentId = Convert.ToInt32(TempData["EditId"]);
                xdocCustomEdit = objClsRef.LoadXmlfromBlob(studentId);

                // xdocCustomEdit.Load(Server.MapPath("~/XML/Data.xml"));
                XmlNodeList xSectionsCustomEdit = xdocCustomEdit.SelectNodes("/ReferralTemplate/Sections/Section[@name]");
                XmlNode xSectnCustomEdit = xSectionsCustomEdit[Index - 1];
                XmlNodeList rowsCustEdit = null;
                int count = 0;
                rowsCustEdit = xSectnCustomEdit.ChildNodes[0].ChildNodes;
                string TableIds = "tblData" + Index;
                string tabType = "";
                html += " <form method='post' id='frm" + Index + "' enctype='multipart/form-data' action='../GeneralInfo/SaveGeneralData?type=" + xSectnCustomEdit.Attributes["name"].Value + "'><div><span class='headPanel'  onclick='getColapse(\"" + TableIds + "\")'>" + xSectnCustomEdit.Attributes["name"].Value + "</span><div class='divSubs'><table id='" + TableIds + "' class='clsTabDiv' style='float:left;width:100%'> <tr style='width:100%'><td style='width:100%'><table style='width:100%;border:none;table-layout:fixed;'>";
                foreach (XmlNode row in rowsCustEdit)
                {
                    count = 0;
                    html += "<tr style='width:100%'>";
                    foreach (XmlNode cell in row.ChildNodes)
                    {
                        count++;
                        string id = cell.Attributes["id"].Value;
                        string type = cell.Attributes["type"].Value;
                        string label = cell.Attributes["label"].Value;
                        string colspan = cell.Attributes["colspan"].Value;
                        if (type == "label")
                        {
                            if (count != 1 && Index != 4)
                            {
                                //if (label == "Age" || label == "Gender")
                                //    html += "<td style='padding-left:35px;' class='tdtext' colspan=" + colspan + ">" + label + "</td>";
                                // else
                                html += "<td style='padding-left:35px;' class='tdtext' colspan=" + colspan + ">" + label + "</td>";
                            }
                            else
                            {
                                if (label == "Marital Status")
                                    html += "<td  class='tdtext' style='width:120px !important' colspan=" + colspan + ">" + label + "</td>";
                                else
                                    html += "<td  class='tdtext' colspan=" + colspan + ">" + label + "</td>";
                            }
                        }
                        //else if (type == "notes")
                        //    html += "<td class='notes-text' colspan=" + colspan + ">" + label + "</td>";
                        else if (type == "header")
                            html += "<td class='subhead' colspan=" + colspan + ">" + label + "</td>";
                        else if (type == "TableBreak")
                        {
                            tabType = type;
                            html += "</tr></table><table style='width:100%;border:none;table-layout:fixed !important;'>"; break;
                        }

                        else if (type == "StudentPhoto")
                        {
                            string imagePath = "";

                            if (sess.ReferralId != 0)
                            {
                                imagePath = objClsRef.GetStudentImage(sess.ReferralId);
                                if (imagePath != null && imagePath != "")
                                {
                                    html += "<td colspan=" + colspan + "><img id='imgStudPhoto' height='150px' width='150px' src=data:image/gif;base64," + imagePath + "></td>";
                                }
                            }
                        }

                        else if (type == "input[text]")
                        {
                            bool val = objClsRef.IsModel(cell);
                            string widthXml = cell.Attributes["width"] != null ? cell.Attributes["width"].Value : "";
                            if (widthXml != "")
                            {
                                widthXml = "width:" + widthXml;
                                widthXml += "% !important";
                            }
                            if (val)
                            {
                                string maxLength = cell.Attributes["MaxLength"].Value;
                                string textValue = cell.Attributes["modelName"].Value;

                                object value = null;
                                try
                                {
                                    string[] split = textValue.Split('.');
                                    if (split.Length > 1)
                                    {
                                        if (split[0] == "objRelationMthr")
                                            value = genModel.objRelationMthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationMthr, null);
                                        if (split[0] == "objRelationFthr")
                                            value = genModel.objRelationFthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationFthr, null);
                                        if (split[0] == "objRelationClose")
                                            if (genModel.objRelationClose != null) value = genModel.objRelationClose.GetType().GetProperty(split[1]).GetValue(genModel.objRelationClose, null);
                                        if (split[0] == "objPhysicianDetails")
                                            if (genModel.objPhysicianDetails != null) value = genModel.objPhysicianDetails.GetType().GetProperty(split[1]).GetValue(genModel.objPhysicianDetails, null);
                                        if (split[0] == "objInsuranceDetails")
                                            value = genModel.objInsuranceDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDetails, null);
                                        if (split[0] == "objInsuranceSecDetails")
                                            value = genModel.objInsuranceSecDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceSecDetails, null);
                                        if (split[0] == "objInsuranceDentalDetails")
                                            value = genModel.objInsuranceDentalDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDentalDetails, null);
                                        if (split[0] == "objRelationLegalGuardian")
                                            value = genModel.objRelationLegalGuardian.GetType().GetProperty(split[1]).GetValue(genModel.objRelationLegalGuardian, null);
                                        if (split[0] == "objRelationEmergncyContact")
                                            value = genModel.objRelationEmergncyContact.GetType().GetProperty(split[1]).GetValue(genModel.objRelationEmergncyContact, null);
                                        if (split[0] == "objclsUpld")
                                            value = genModel.objclsUpld.GetType().GetProperty(split[1]).GetValue(genModel.objclsUpld, null);
                                    }
                                    else
                                    {
                                        value = genModel.GetType().GetProperty(textValue.ToString()).GetValue(genModel, null);
                                    }
                                }
                                catch
                                {

                                }
                                finally
                                {
                                }

                                //html += "<td colspan=" + colspan + " style='width: "+widthCell+"%;'><span class='fontwhite'>*<span><input id='" + id + "' name='" + textValue.ToString() + "' value='" + value + "' maxlength='" + maxLength + "' type='text' /></td>";
                                html += "<td colspan=" + colspan + " ><span class='fontwhite'>*</span><input id='" + id + "' name='" + textValue.ToString() + "' style='" + widthXml + "' value='" + Server.HtmlEncode(Convert.ToString(value)) + "' maxlength='" + maxLength + "' type='text' /></td>";
                            }
                            else
                                if (label == "Duration of Labor")
                                {
                                    string funName = cell.Attributes["onkeypress"].Value;
                                    html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + id + "'  style='width:80px !important;margin-right:1px !important;' onkeypress='" + funName + "'; onpaste='PreventDef(event)'  type='text' value='" + cell.InnerText + "' />Hours</td>";
                                }
                                else if (label == "Adopted age" || label == "Age when Applicant" || label == "Age of Onset")
                                {
                                    string funName = cell.Attributes["onkeypress"].Value;
                                    html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + id + "'  style='width:80px !important;margin-right:1px !important;' onkeypress='" + funName + "'; onpaste='PreventDef(event)'  type='text' value='" + cell.InnerText + "' />YRS</td>";
                                }
                                else if (label == "Birth WeightLBS")
                                {
                                    string funName = cell.Attributes["onkeypress"].Value;
                                    html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + id + "'  style='width:80px !important;margin-right:1px !important; ' onkeypress='" + funName + "' onpaste='PreventDef(event)' type='text' value='" + cell.InnerText + "' />LBS</td>";
                                }
                                else
                                    html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' style='" + widthXml + "' name='" + id + "' type='text' value='" + Server.HtmlEncode(cell.InnerText) + "' /></td>";
                        }


                        else if (type == "input[textvalidate]")
                        {
                            bool val = objClsRef.IsModel(cell);

                            if (val)
                            {
                                string className = cell.Attributes["className"].Value;
                                string textValue = cell.Attributes["modelName"].Value;
                                // string className = cell.Attributes["className"].Value;
                                string maxLength = cell.Attributes["MaxLength"].Value;
                                object value = null;
                                try
                                {

                                    string[] split = textValue.Split('.');
                                    if (split.Length > 1)
                                    {
                                        if (split[0] == "objRelationMthr")
                                            value = genModel.objRelationMthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationMthr, null);
                                        if (split[0] == "objRelationFthr")
                                            value = genModel.objRelationFthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationFthr, null);
                                        if (split[0] == "objRelationClose")
                                            if (genModel.objRelationClose != null) value = genModel.objRelationClose.GetType().GetProperty(split[1]).GetValue(genModel.objRelationClose, null);
                                        if (split[0] == "objPhysicianDetails")
                                            if (genModel.objPhysicianDetails != null) value = genModel.objPhysicianDetails.GetType().GetProperty(split[1]).GetValue(genModel.objPhysicianDetails, null);
                                        if (split[0] == "objInsuranceDetails")
                                            value = genModel.objInsuranceDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDetails, null);
                                        if (split[0] == "objInsuranceSecDetails")
                                            value = genModel.objInsuranceSecDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceSecDetails, null);
                                        if (split[0] == "objInsuranceDentalDetails")
                                            value = genModel.objInsuranceDentalDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDentalDetails, null);
                                        if (split[0] == "objRelationLegalGuardian")
                                            value = genModel.objRelationLegalGuardian.GetType().GetProperty(split[1]).GetValue(genModel.objRelationLegalGuardian, null);
                                        if (split[0] == "objRelationEmergncyContact")
                                            value = genModel.objRelationEmergncyContact.GetType().GetProperty(split[1]).GetValue(genModel.objRelationEmergncyContact, null);
                                        if (split[0] == "objclsUpld")
                                            value = genModel.objclsUpld.GetType().GetProperty(split[1]).GetValue(genModel.objclsUpld, null);
                                    }
                                    else
                                    {

                                        value = genModel.GetType().GetProperty(textValue.ToString()).GetValue(genModel, null);
                                    }
                                }
                                catch
                                {

                                }

                                if (className == "validate[required]" || className == "validate[required] namefield")
                                    html += "<td colspan=" + colspan + " ><span class='validate'>*</span><input id='" + id + "' name='" + textValue.ToString() + "' value='" + Server.HtmlEncode(Convert.ToString(value)) + "' class='" + className + "' onpaste='PreventDef(event)' maxlength='" + maxLength + "' type='text' /></td>";

                                else
                                    html += "<td colspan=" + colspan + " ><span class='fontwhite'>*</span><input id='" + id + "' name='" + textValue.ToString() + "' value='" + Server.HtmlEncode(Convert.ToString(value)) + "' class='" + className + "' onpaste='PreventDef(event)' maxlength='" + maxLength + "' type='text' /></td>";
                            }
                            else
                            {
                                string className = cell.Attributes["className"].Value;
                                if (className == "validate[required]" || className == "validate[required] namefield")
                                    html += "<td colspan=" + colspan + "><span class='validate'>*</span><input id='" + id + "' name='" + id + "' value='" + cell.InnerText + "' class='" + className + "' onpaste='PreventDef(event)' type='text' /></td>";
                                else
                                    html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + id + "' type='text' value='" + cell.InnerText + "'  class='" + className + "' onpaste='PreventDef(event)' /></td>";
                            }
                        }

                        else if (type == "input[textfunction]")
                        {
                            bool val = objClsRef.IsModel(cell);
                            string maxLength = cell.Attributes["MaxLength"].Value;
                            string funName = cell.Attributes["onkeypress"].Value;
                            if (val)
                            {

                                string textValue = cell.Attributes["modelName"].Value;


                                //if (textValue == "RefZipCode")
                                //{
                                //    classname = "validate[required]";
                                //}

                                object value = null;
                                try
                                {

                                    string[] split = textValue.Split('.');
                                    if (split.Length > 1)
                                    {
                                        if (split[0] == "objRelationMthr")
                                            value = genModel.objRelationMthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationMthr, null);
                                        if (split[0] == "objRelationFthr")
                                            value = genModel.objRelationFthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationFthr, null);
                                        if (split[0] == "objRelationClose")
                                            if (genModel.objRelationClose != null) value = genModel.objRelationClose.GetType().GetProperty(split[1]).GetValue(genModel.objRelationClose, null);
                                        if (split[0] == "objPhysicianDetails")
                                            if (genModel.objPhysicianDetails != null) value = genModel.objPhysicianDetails.GetType().GetProperty(split[1]).GetValue(genModel.objPhysicianDetails, null);
                                        if (split[0] == "objInsuranceDetails")
                                            value = genModel.objInsuranceDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDetails, null);
                                        if (split[0] == "objInsuranceSecDetails")
                                            value = genModel.objInsuranceSecDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceSecDetails, null);
                                        if (split[0] == "objInsuranceDentalDetails")
                                            value = genModel.objInsuranceDentalDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDentalDetails, null);
                                        if (split[0] == "objRelationLegalGuardian")
                                            value = genModel.objRelationLegalGuardian.GetType().GetProperty(split[1]).GetValue(genModel.objRelationLegalGuardian, null);
                                        if (split[0] == "objRelationEmergncyContact")
                                            value = genModel.objRelationEmergncyContact.GetType().GetProperty(split[1]).GetValue(genModel.objRelationEmergncyContact, null);
                                        if (split[0] == "objclsUpld")
                                            value = genModel.objclsUpld.GetType().GetProperty(split[1]).GetValue(genModel.objclsUpld, null);
                                    }
                                    else
                                    {
                                        value = genModel.GetType().GetProperty(textValue.ToString()).GetValue(genModel, null);
                                    }
                                }
                                catch
                                {

                                }


                                if (textValue == "RefZipCode")
                                {
                                    html += "<td colspan=" + colspan + " ><span class='fontwhite'>*</span><input id='" + id + "' name='" + textValue.ToString() + "' value='" + Server.HtmlEncode(Convert.ToString(value)) + "' maxlength='" + maxLength + "' onpaste='PreventDef(event)'  onkeypress='" + funName + "' type='text' /></td>";
                                }
                                else html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + textValue.ToString() + "' value='" + Server.HtmlEncode(Convert.ToString(value)) + "' maxlength='" + maxLength + "' onpaste='PreventDef(event)' onkeypress='" + funName + "' type='text' /></td>";


                            }
                            else
                            {
                                // string funName = cell.Attributes["onkeypress"].Value;
                                html += "<td colspan=" + colspan + " ><span class='fontwhite'>*</span><input id='" + id + "' name='" + id + "' type='text' value='" + Server.HtmlEncode(cell.InnerText) + "' onpaste='PreventDef(event)'  onkeypress='" + funName + "' maxlength='" + maxLength + "' /></td>";
                            }
                        }

                        else if (type == "input[textKeyType]")
                        {
                            bool val = objClsRef.IsModel(cell);
                            if (val)
                            {
                                string maxLength = cell.Attributes["MaxLength"].Value;
                                string textValue = cell.Attributes["modelName"].Value;
                                string funName = cell.Attributes["onkeypress"].Value;
                                string keyName = cell.Attributes["key"].Value;
                                string width = cell.Attributes["width"].Value;
                                object value = null;
                                try
                                {
                                    string[] split = textValue.Split('.');
                                    if (split.Length > 1)
                                    {
                                        if (split[0] == "objRelationMthr")
                                            value = genModel.objRelationMthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationMthr, null);
                                        if (split[0] == "objRelationFthr")
                                            value = genModel.objRelationFthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationFthr, null);
                                        if (split[0] == "objRelationClose")
                                            if (genModel.objRelationClose != null) value = genModel.objRelationClose.GetType().GetProperty(split[1]).GetValue(genModel.objRelationClose, null);
                                        if (split[0] == "objPhysicianDetails")
                                            if (genModel.objPhysicianDetails != null) value = genModel.objPhysicianDetails.GetType().GetProperty(split[1]).GetValue(genModel.objPhysicianDetails, null);
                                        if (split[0] == "objInsuranceDetails")
                                            value = genModel.objInsuranceDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDetails, null);
                                        if (split[0] == "objInsuranceSecDetails")
                                            value = genModel.objInsuranceSecDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceSecDetails, null);
                                        if (split[0] == "objInsuranceDentalDetails")
                                            value = genModel.objInsuranceDentalDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDentalDetails, null);
                                        if (split[0] == "objRelationLegalGuardian")
                                            value = genModel.objRelationLegalGuardian.GetType().GetProperty(split[1]).GetValue(genModel.objRelationLegalGuardian, null);
                                        if (split[0] == "objRelationEmergncyContact")
                                            value = genModel.objRelationEmergncyContact.GetType().GetProperty(split[1]).GetValue(genModel.objRelationEmergncyContact, null);
                                        if (split[0] == "objclsUpld")
                                            value = genModel.objclsUpld.GetType().GetProperty(split[1]).GetValue(genModel.objclsUpld, null);
                                    }
                                    else
                                    {
                                        value = genModel.GetType().GetProperty(textValue.ToString()).GetValue(genModel, null);
                                    }
                                }
                                catch
                                {

                                }

                                html += "<td colspan=" + colspan + " ><span class='fontwhite'>*</span><input id='" + id + "' name='" + textValue.ToString() + "' value='" + Server.HtmlEncode(Convert.ToString(value)) + "' style='width: " + width + ";margin-right:0px;' maxlength='" + maxLength + "' onkeypress='" + funName + "' type='text' /><span style='color:#595959;'>" + keyName + "</span></td>";

                            }
                            else
                                html += "<td colspan=" + colspan + " ><span class='fontwhite'>*</span><input id='" + id + "' name='" + id + "' type='text' value='" + Server.HtmlEncode(cell.InnerText) + "' /></td>";
                        }
                        else if (type == "input[textdate]")
                        {
                            html += "<td colspan=" + colspan + " ><span class='fontwhite'>*</span><input id='" + id + "' value='" + Server.HtmlEncode(Convert.ToString(cell.InnerText)) + "'  name='" + id + "' type='text' class='datepicker' onkeypress='return false' /></td>";
                        }


                        else if (type == "input[multitext]")
                        {
                            bool val = objClsRef.IsModel(cell);
                            string width = cell.Attributes["Width"].Value;
                            if (val)
                            {
                                string textValue = cell.Attributes["modelName"].Value;

                                object value = null;
                                try
                                {
                                    string[] split = textValue.Split('.');
                                    if (split.Length > 1)
                                    {
                                        if (split[0] == "objRelationMthr")
                                            value = genModel.objRelationMthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationMthr, null);
                                        if (split[0] == "objRelationFthr")
                                            value = genModel.objRelationFthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationFthr, null);
                                        if (split[0] == "objRelationClose")
                                            if (genModel.objRelationClose != null) value = genModel.objRelationClose.GetType().GetProperty(split[1]).GetValue(genModel.objRelationClose, null);
                                        if (split[0] == "objPhysicianDetails")
                                            if (genModel.objPhysicianDetails != null) value = genModel.objPhysicianDetails.GetType().GetProperty(split[1]).GetValue(genModel.objPhysicianDetails, null);
                                        if (split[0] == "objInsuranceDetails")
                                            value = genModel.objInsuranceDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDetails, null);
                                        if (split[0] == "objInsuranceSecDetails")
                                            value = genModel.objInsuranceSecDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceSecDetails, null);
                                        if (split[0] == "objInsuranceDentalDetails")
                                            value = genModel.objInsuranceDentalDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDentalDetails, null);
                                        if (split[0] == "objRelationLegalGuardian")
                                            value = genModel.objRelationLegalGuardian.GetType().GetProperty(split[1]).GetValue(genModel.objRelationLegalGuardian, null);

                                        if (split[0] == "objRelationEmergncyContact")
                                            value = genModel.objRelationEmergncyContact.GetType().GetProperty(split[1]).GetValue(genModel.objRelationEmergncyContact, null);

                                        if (split[0] == "objclsUpld")
                                            value = genModel.objclsUpld.GetType().GetProperty(split[1]).GetValue(genModel.objclsUpld, null);
                                    }
                                    else
                                    {
                                        value = genModel.GetType().GetProperty(textValue.ToString()).GetValue(genModel, null);
                                    }
                                }
                                catch
                                {

                                }

                                html += "<td colspan=" + colspan + " ><span class='fontwhite'>*</span><textarea id='" + id + "' name='" + textValue.ToString() + "' value='" + Server.HtmlEncode(Convert.ToString(value)) + "' type='text' style='width:" + width + ";' rows='3'>" + Server.HtmlEncode(Convert.ToString(value)) + "</textarea></td>";
                            }
                            else
                                html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><textarea id='" + id + "' name='" + id + "' type='text' value='" + Server.HtmlEncode(Convert.ToString(cell.InnerText)) + "' style='width:" + width + ";' rows='3'>" + Server.HtmlEncode(Convert.ToString(cell.InnerText)) + "</textarea></td>";
                        }
                        else if (type == "input[textDate]")
                        {
                            bool val = objClsRef.IsModel(cell);
                            if (val)
                            {
                                object value = null;
                                string textValue = cell.Attributes["modelName"].Value;

                                try
                                {
                                    string[] split = textValue.Split('.');
                                    if (split.Length > 1)
                                    {
                                        if (split[0] == "objRelationMthr")
                                            value = genModel.objRelationMthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationMthr, null);
                                        if (split[0] == "objRelationFthr")
                                            value = genModel.objRelationFthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationFthr, null);
                                        if (split[0] == "objRelationClose")
                                            if (genModel.objRelationClose != null) value = genModel.objRelationClose.GetType().GetProperty(split[1]).GetValue(genModel.objRelationClose, null);
                                        if (split[0] == "objPhysicianDetails")
                                            if (genModel.objPhysicianDetails != null) value = genModel.objPhysicianDetails.GetType().GetProperty(split[1]).GetValue(genModel.objPhysicianDetails, null);
                                        if (split[0] == "objInsuranceDetails")
                                            value = genModel.objInsuranceDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDetails, null);
                                        if (split[0] == "objInsuranceSecDetails")
                                            value = genModel.objInsuranceSecDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceSecDetails, null);
                                        if (split[0] == "objInsuranceDentalDetails")
                                            value = genModel.objInsuranceDentalDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDentalDetails, null);
                                        if (split[0] == "objRelationLegalGuardian")
                                            value = genModel.objRelationLegalGuardian.GetType().GetProperty(split[1]).GetValue(genModel.objRelationLegalGuardian, null);
                                        if (split[0] == "objRelationEmergncyContact")
                                            value = genModel.objRelationEmergncyContact.GetType().GetProperty(split[1]).GetValue(genModel.objRelationEmergncyContact, null);

                                        if (split[0] == "objclsUpld")
                                            value = genModel.objclsUpld.GetType().GetProperty(split[1]).GetValue(genModel.objclsUpld, null);
                                    }
                                    else
                                    {
                                        value = genModel.GetType().GetProperty(textValue.ToString()).GetValue(genModel, null);
                                    }
                                }
                                catch
                                {


                                }

                                if (textValue == "RefDate" || textValue == "RefDOB")
                                {
                                    html += "<td colspan=" + colspan + " ><span class='validate'>*</span><input id='" + id + "' name='" + textValue.ToString() + "' class='validate[required] datepicker' onkeypress='return false' value='" + Server.HtmlEncode(Convert.ToString(value)) + "' type='text' /></td>";
                                }
                                else
                                    html += "<td colspan=" + colspan + " ><span class='fontwhite'>*</span><input id='" + id + "' name='" + textValue.ToString() + "' class='datepicker' onkeypress='return false' value='" + Server.HtmlEncode(Convert.ToString(value)) + "' type='text' /></td>";
                            }
                            else
                                html += "<td colspan=" + colspan + " ><span class='fontwhite'>*</span><input id='" + id + "' name='" + id + "'  class='datepicker' onkeypress='return false' value='" + Server.HtmlEncode(cell.InnerText) + "' type='text' /></td>";
                        }

                        //else if (type == "input[button]")
                        //{
                        //    if (isUpated == 1)
                        //    {
                        //        html += "<td colspan=" + colspan + " style='width: 20%; text-align:right;'><input id='" + id + "' value='Update' type='submit' class='NFButton' /></td>";
                        //    }
                        //    else
                        //    {
                        //        html += "<td colspan=" + colspan + " style='width: 20%; text-align:right;'><input id='" + id + "' value='" + label + "' type='submit' class='NFButton' /></td>";
                        //    }

                        //    //html += "<td colspan=" + colspan + " style='width: 20%; text-align:right;'><input id='" + id + "' value='" + (isUpated == 1 ? "update" : "save") + "' type='submit' class='NFButton' /></td>";
                        //}
                        else if (type == "input[file]")
                        {
                            string name = cell.Attributes["label"].Value.ToString();
                            string nameFile = cell.Attributes["name"].Value.ToString();
                            if (name == "Upload Photo")
                                html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + nameFile + "' type='file' style='width:160px !important' /></td>";
                            else
                                html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + nameFile + "' type='file' style='width:210px !important' /></td>";


                        }
                        else if (type == "input[check]")
                        {
                            countCheck = Convert.ToInt32(cell.Attributes["Count"].Value);
                            string[] idSplit = id.Split('|');
                            string[] lblSplit = label.Split('|');
                            html += "<td colspan=" + colspan + " >";
                            for (int i = 0; i < countCheck; i++)
                            {
                                html += "<input id='" + idSplit[i] + "' type='checkbox'>" + lblSplit[i] + "</input><br>";
                            }
                            html += "</td>";

                        }
                        else if (type == "lblBold")
                        {
                            html += "<td class='lblBold' colspan=" + colspan + " style='width: 100%;'>" + label + "</td>";
                        }
                        else if (type == "input[checkVt]")
                        {
                            string[] splitInnerTxt = cell.InnerText.Split('|');
                            countCheck = Convert.ToInt32(cell.Attributes["Count"].Value);
                            string[] idSpl = id.Split('|');
                            string[] lblsplit = label.Split('|');
                            bool val = objClsRef.IsCheckName(cell);
                            if (val)
                            {
                                string name = cell.Attributes["name"].Value;
                                if (count == 3 && name == "Illnesses")
                                {
                                    html += "<td colspan=" + colspan + " style='padding-left:35px;' >";
                                }
                            }
                            else
                                html += "<td colspan=" + colspan + " >";
                            for (int i = 0; i < countCheck; i++)
                            {
                                if (splitInnerTxt[i] == "checked")
                                {
                                    html += "<input id='" + idSpl[i] + "' type='checkbox' value='checked' name='" + idSpl[i] + "' onclick='GetSelected(this);' checked><span>" + lblsplit[i] + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>";
                                }
                                else
                                {
                                    html += "<input id='" + idSpl[i] + "' type='checkbox' value='unchecked' name='" + idSpl[i] + "' onclick='GetSelected(this);'><span>" + lblsplit[i] + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>";
                                }
                            }
                            html += "</td>";

                        }


                        else if (type == "input[radio]")
                        {
                            bool val = objClsRef.IsModel(cell);
                            if (val)
                            {
                                IEnumerable<SelectListItem> listMar = new List<SelectListItem>();
                                if (label == "MaritalStatus")
                                {
                                    string textValue = cell.Attributes["modelName"].Value;
                                    listMar = objClsRef.FillMaritalStatus();
                                    if (textValue == "objRelationMthr.txtMaritalStatus")
                                    {
                                        html += "<td colspan=" + colspan + " >";
                                        foreach (SelectListItem item in listMar)
                                        {
                                            if (genModel.objRelationMthr.txtMaritalStatus.ToString() == item.Value)
                                            {
                                                html += "<input id='" + item.Value + "' type='radio' value='" + item.Value + "' name='" + textValue + "' checked><label>" + item.Text + "</label></input>";
                                            }
                                            else
                                            {
                                                html += "<input id='" + item.Value + "' type='radio' value='" + item.Value + "' name='" + textValue + "'><label>" + item.Text + "</label></input>";
                                            }
                                        }
                                        html += "</td>";

                                    }
                                    else if (textValue == "objRelationFthr.txtMaritalStatus")
                                    {
                                        html += "<td colspan=" + colspan + " >";
                                        foreach (SelectListItem item in listMar)
                                        {
                                            if (genModel.objRelationFthr.txtMaritalStatus.ToString() == item.Value)
                                            {
                                                html += "<input id='" + item.Value + "' type='radio' value='" + item.Value + "' name='" + textValue + "' checked><label>" + item.Text + "</label></input>";
                                            }
                                            else
                                            {
                                                html += "<input id='" + item.Value + "' type='radio' value='" + item.Value + "' name='" + textValue + "'><label>" + item.Text + "</label></input>";
                                            }
                                        }
                                        html += "</td>";

                                    }



                                    else
                                    {
                                        html += "<td colspan=" + colspan + " >";

                                        foreach (SelectListItem item in listMar)
                                        {
                                            html += "<input id='" + item.Value + "' type='radio' value='" + item.Value + "' name='" + textValue + "'><label>" + item.Text + "</label></input>";
                                        }
                                        html += "</td>";
                                    }

                                }

                            }
                            else
                            {
                                string name = cell.Attributes["name"].Value.ToString();
                                countCheck = Convert.ToInt32(cell.Attributes["Count"].Value);
                                string[] splitInnerTxt = cell.InnerText.Split('|');
                                string[] idSpl = id.Split('|');
                                string[] lblsplit = label.Split('|');
                                string vals = cell.Attributes["value"].Value.ToString();
                                html += "<td colspan=" + colspan + ">";
                                for (int i = 0; i < countCheck; i++)
                                {
                                    if (splitInnerTxt[i] == "checked")
                                    {
                                        html += "<input id='" + idSpl[i] + "' type='radio' value='checked' name='" + name + "' checked><span>" + lblsplit[i] + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>";
                                    }
                                    else
                                        html += "<input id='" + idSpl[i] + "' type='radio' value='unchecked' name='" + name + "' !checked><span>" + lblsplit[i] + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>";
                                }
                                html += "</td>";
                            }
                        }

                        else if (type == "input[dropRel]")
                        {
                            IEnumerable<SelectListItem> ListData = new List<SelectListItem>();
                            bool val = objClsRef.IsModel(cell);
                            if (val)
                            {

                                if (label == "Relation Status")
                                {
                                    ListData = objClsRef.FillDropList("Relationship");
                                    string textValue = cell.Attributes["modelName"].Value;
                                    html += "<td colspan=" + colspan + " ><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class=''>";
                                    html += "<option value=''>Select " + label + "</option>";

                                    foreach (SelectListItem Item in ListData)
                                    {
                                        if (textValue == "objRelationClose.closeRelation")
                                        {
                                            if (genModel.objRelationClose.closeRelation.ToString() == Item.Value)
                                            {
                                                html += "<option value='" + Item.Value + "' selected='true'>" + Item.Text + "</option>";
                                            }
                                            else
                                            {
                                                html += "<option value='" + Item.Value + "'>" + Item.Text + "</option>";
                                            }
                                        }

                                        else if (textValue == "objRelationLegalGuardian.LgRelationName")
                                        {
                                            if (genModel.objRelationLegalGuardian.LgRelationName.ToString() == Item.Value)
                                            {
                                                html += "<option value='" + Item.Value + "' selected='true'>" + Item.Text + "</option>";
                                            }
                                            else
                                            {
                                                html += "<option value='" + Item.Value + "'>" + Item.Text + "</option>";
                                            }
                                        }

                                        else if (textValue == "objRelationEmergncyContact.EmRelationName")
                                        {
                                            if (genModel.objRelationEmergncyContact.EmRelationName.ToString() == Item.Value)
                                            {
                                                html += "<option value='" + Item.Value + "' selected='true'>" + Item.Text + "</option>";
                                            }
                                            else
                                            {
                                                html += "<option value='" + Item.Value + "'>" + Item.Text + "</option>";
                                            }
                                        }
                                     }

                                    }
                                }
                            
                            else
                            {

                            }
                        }


                        else if (type == "input[dropDocType]")
                        {
                            IEnumerable<SelectListItem> ListData = new List<SelectListItem>();
                            bool val = objClsRef.IsModel(cell);
                            if (val)
                            {

                                if (label == "DocumentType")
                                {
                                    ListData = objClsRef.FillDoccumentType();
                                    string textValue = cell.Attributes["modelName"].Value;
                                    html += "<td colspan=" + colspan + " ><span class='validate'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='validate[required]'>";
                                    html += "<option value='' selected='true'>Select " + label + "</option>";
                                    foreach (SelectListItem Item in ListData)
                                    {
                                        if (textValue == "objclsUpld.DocType")
                                        {
                                            if (genModel.objclsUpld.DocType.ToString() == Item.Value)
                                            {
                                                html += "<option value='" + Item.Value + "' selected='true'>" + Item.Text + "</option>";
                                            }
                                            else
                                            {
                                                html += "<option value='" + Item.Value + "'>" + Item.Text + "</option>";
                                            }
                                        }
                                    }

                                }
                            }
                            else
                            {
                            }
                        }

                        else if (type == "input[drop]")
                        {
                            IEnumerable<SelectListItem> listData = new List<SelectListItem>();
                            IEnumerable<SelectListItem> listDataState = new List<SelectListItem>();
                            if (label == "Country")
                            {
                                listData = objClsRef.FillDropList("Country");
                            }
                            bool val = objClsRef.IsModel(cell);
                            if (val)
                            {

                                if (label == "Country")
                                {
                                    string textValue = cell.Attributes["modelName"].Value;
                                    string stateVal = "0";

                                    if (textValue == "RefCountry")
                                    {
                                        html += "<td colspan=" + colspan + " ><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='ddlRefCountry' >";// onchange='GetState(this.id," + stateVal + ")'
                                        //html += "<option value=''>Select " + label + "</option>";
                                    }
                                    else
                                    {
                                        html += "<td colspan=" + colspan + " ><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='ddlOtherCountry' >";//onchange='GetState(this.id," + stateVal + ")'
                                        //html += "<option value=0>Select " + label + "</option>";
                                    }



                                    foreach (SelectListItem list in listData)
                                    {

                                        if (textValue == "objRelationMthr.txtCountry")
                                        {
                                            if (genModel.objRelationMthr != null)
                                            {
                                                if (genModel.objRelationMthr.txtCountry.ToString() == list.Value)
                                                {
                                                    html += "<option value='" + list.Value + "' selected='true'>" + list.Text + "</option>";

                                                }
                                                else
                                                {
                                                    html += "<option value='" + list.Value + "'>" + list.Text + "</option>";
                                                }
                                            }
                                            else
                                            {
                                                html += "<option value='" + list.Value + "'>" + list.Text + "</option>";
                                            }
                                        }
                                        else if (textValue == "objRelationFthr.txtCountry")
                                        {
                                            if (genModel.objRelationFthr.txtCountry != null)
                                            {
                                                if (genModel.objRelationFthr.txtCountry.ToString() == list.Value)
                                                {
                                                    html += "<option value='" + list.Value + "' selected='true'>" + list.Text + "</option>";
                                                }
                                                else
                                                {
                                                    html += "<option value='" + list.Value + "'>" + list.Text + "</option>";
                                                }
                                            }
                                            else
                                            {
                                                html += "<option value='" + list.Value + "'>" + list.Text + "</option>";
                                            }
                                        }
                                        else if (textValue == "objRelationClose.txtCountry")
                                        {
                                            if (genModel.objRelationClose != null)
                                            {
                                                if (genModel.objRelationClose.txtCountry != null)
                                                {
                                                    if (genModel.objRelationClose.txtCountry.ToString() == list.Value)
                                                    {
                                                        html += "<option value='" + list.Value + "' selected='true'>" + list.Text + "</option>";
                                                    }
                                                    else
                                                    {
                                                        html += "<option value='" + list.Value + "'>" + list.Text + "</option>";
                                                    }
                                                }
                                                else
                                                {
                                                    html += "<option value='" + list.Value + "'>" + list.Text + "</option>";
                                                }
                                            }
                                        }
                                        else if (textValue == "objPhysicianDetails.txtCountry")
                                        {
                                            if (genModel.objPhysicianDetails != null)
                                            {
                                                //if (genModel.objPhysicianDetails.txtCountry != null)
                                                //{
                                                if (genModel.objPhysicianDetails.txtCountry.ToString() == list.Value)
                                                {
                                                    html += "<option value='" + list.Value + "' selected='true'>" + list.Text + "</option>";
                                                }
                                                else
                                                {
                                                    html += "<option value='" + list.Value + "'>" + list.Text + "</option>";
                                                }
                                                //}
                                            }
                                            else
                                            {
                                                html += "<option value='" + list.Value + "'>" + list.Text + "</option>";
                                            }
                                        }
                                        else if (textValue == "objInsuranceDetails.txtCountry")
                                        {
                                            if (genModel.objInsuranceDetails != null)
                                            {
                                                //if (genModel.objInsuranceDetails.txtCountry != null)
                                                //{
                                                if (genModel.objInsuranceDetails.txtCountry.ToString() == list.Value)
                                                {
                                                    html += "<option value='" + list.Value + "' selected='true'>" + list.Text + "</option>";
                                                }
                                                else
                                                {
                                                    html += "<option value='" + list.Value + "'>" + list.Text + "</option>";
                                                }
                                                // }
                                            }
                                            else
                                            {
                                                html += "<option value='" + list.Value + "'>" + list.Text + "</option>";
                                            }
                                        }
                                        else if (textValue == "objInsuranceSecDetails.txtCountry")
                                        {
                                            if (genModel.objInsuranceDetails != null)
                                            {
                                                //if (genModel.objInsuranceSecDetails.txtCountry != null)
                                                //{
                                                if (genModel.objInsuranceSecDetails.txtCountry.ToString() == list.Value)
                                                {
                                                    html += "<option value='" + list.Value + "' selected='true'>" + list.Text + "</option>";
                                                }
                                                else
                                                {
                                                    html += "<option value='" + list.Value + "'>" + list.Text + "</option>";
                                                }
                                                //}
                                            }
                                            else
                                            {
                                                html += "<option value='" + list.Value + "'>" + list.Text + "</option>";
                                            }
                                        }
                                        else if (textValue == "objInsuranceDentalDetails.txtCountry")
                                        {
                                            if (genModel.objInsuranceDetails != null)
                                            {
                                                //if (genModel.objInsuranceDentalDetails.txtCountry != null)
                                                //{
                                                if (genModel.objInsuranceDentalDetails.txtCountry.ToString() == list.Value)
                                                {
                                                    html += "<option value='" + list.Value + "' selected='true'>" + list.Text + "</option>";
                                                }
                                                else
                                                {
                                                    html += "<option value='" + list.Value + "'>" + list.Text + "</option>";
                                                }
                                                // }
                                            }
                                            else
                                            {
                                                html += "<option value='" + list.Value + "'>" + list.Text + "</option>";
                                            }
                                        }

                                        else if (textValue == "objRelationLegalGuardian.txtCountry")
                                        {
                                            if (genModel.objRelationLegalGuardian.txtCountry != null)
                                            {
                                                if (genModel.objRelationLegalGuardian.txtCountry.ToString() == list.Value)
                                                {
                                                    html += "<option value='" + list.Value + "' selected='true'>" + list.Text + "</option>";
                                                }
                                                else
                                                {
                                                    html += "<option value='" + list.Value + "'>" + list.Text + "</option>";
                                                }
                                            }
                                            else
                                            {
                                                html += "<option value='" + list.Value + "'>" + list.Text + "</option>";
                                            }
                                        }
                                        else if (textValue == "objRelationEmergncyContact.txtCountry")
                                        {
                                            if (genModel.objRelationEmergncyContact.txtCountry != null)
                                            {
                                                if (genModel.objRelationEmergncyContact.txtCountry.ToString() == list.Value)
                                                {
                                                    html += "<option value='" + list.Value + "' selected='true'>" + list.Text + "</option>";
                                                }
                                                else
                                                {
                                                    html += "<option value='" + list.Value + "'>" + list.Text + "</option>";
                                                }
                                            }
                                            else
                                            {
                                                html += "<option value='" + list.Value + "'>" + list.Text + "</option>";
                                            }
                                        }
                                        else if (textValue == "RefCntryBirth")
                                        {
                                            if (genModel.RefCntryBirth != null)
                                            {
                                                if (genModel.RefCntryBirth.ToString() == list.Value)
                                                {
                                                    html += "<option value='" + list.Value + "' selected='true'>" + list.Text + "</option>";
                                                }
                                                else
                                                {
                                                    html += "<option value='" + list.Value + "'>" + list.Text + "</option>";
                                                }
                                            }
                                            else
                                            {
                                                html += "<option value='" + list.Value + "'>" + list.Text + "</option>";
                                            }
                                        }
                                        else
                                        {
                                            if (genModel.RefCountry.ToString() == list.Value)
                                            {
                                                html += "<option value='" + list.Value + "' selected='true'>" + list.Text + "</option>";

                                            }
                                            else
                                            {
                                                html += "<option value='" + list.Value + "'>" + list.Text + "</option>";
                                            }
                                        }
                                    }

                                    html += "</select></td>";
                                }
                                else if (label == "State")
                                {

                                    string textValue = cell.Attributes["modelName"].Value;

                                    if (textValue == "RefState")
                                    {
                                        html += "<td colspan=" + colspan + " ><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
                                        html += "<option value=0>Select " + label + "</option>";
                                        int countryId = Convert.ToInt32(genModel.RefCountry);
                                        listDataState = objClsRef.FillState(countryId);

                                        foreach (SelectListItem listSt in listDataState)
                                        {
                                            if (textValue == "RefState")
                                            {
                                                if (genModel.RefState.ToString() == listSt.Value)
                                                {
                                                    html += "<option value='" + listSt.Value + "' selected='true'>" + listSt.Text + "</option>";
                                                }
                                                else
                                                {
                                                    html += "<option value='" + listSt.Value + "'>" + listSt.Text + "</option>";
                                                }
                                            }
                                        }
                                        html += "</select></td>";
                                    }

                                    else if (textValue == "objRelationMthr.txtState")
                                    {
                                        int countryId = Convert.ToInt32(genModel.objRelationMthr.txtCountry);
                                        listDataState = objClsRef.FillState(countryId);
                                        html += "<td colspan=" + colspan + " ><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
                                        html += "<option value=0>Select " + label + "</option>";
                                        foreach (SelectListItem listSt in listDataState)
                                        {
                                            if (textValue == "objRelationMthr.txtState")
                                            {
                                                if (genModel.objRelationMthr.txtState.ToString() == listSt.Value)
                                                {
                                                    html += "<option value='" + listSt.Value + "' selected='true'>" + listSt.Text + "</option>";
                                                }
                                                else
                                                {
                                                    html += "<option value='" + listSt.Value + "'>" + listSt.Text + "</option>";
                                                }
                                            }
                                        }
                                        html += "</select></td>";
                                    }

                                    else if (textValue == "objRelationFthr.txtState")
                                    {
                                        int countryId = Convert.ToInt32(genModel.objRelationFthr.txtCountry);
                                        listDataState = objClsRef.FillState(countryId);
                                        html += "<td colspan=" + colspan + " ><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
                                        html += "<option value=0>Select " + label + "</option>";
                                        foreach (SelectListItem listSt in listDataState)
                                        {
                                            if (textValue == "objRelationFthr.txtState")
                                            {
                                                if (genModel.objRelationFthr.txtState.ToString() == listSt.Value)
                                                {
                                                    html += "<option value='" + listSt.Value + "' selected='true'>" + listSt.Text + "</option>";
                                                }
                                                else
                                                {
                                                    html += "<option value='" + listSt.Value + "'>" + listSt.Text + "</option>";
                                                }
                                            }
                                        }
                                        html += "</select></td>";
                                    }

                                    else if (textValue == "objRelationClose.txtState")
                                    {
                                        if (genModel.objRelationClose != null)
                                        {
                                            int countryId = Convert.ToInt32(genModel.objRelationClose.txtCountry);
                                            listDataState = objClsRef.FillState(countryId);
                                            html += "<td colspan=" + colspan + " ><span style='color: white;'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
                                            html += "<option value=0>Select " + label + "</option>";
                                            foreach (SelectListItem listSt in listDataState)
                                            {
                                                if (textValue == "objRelationClose.txtState")
                                                {
                                                    if (genModel.objRelationClose.txtState.ToString() == listSt.Value)
                                                    {
                                                        html += "<option value='" + listSt.Value + "' selected='true'>" + listSt.Text + "</option>";
                                                    }
                                                    else
                                                    {
                                                        html += "<option value='" + listSt.Value + "'>" + listSt.Text + "</option>";
                                                    }
                                                }
                                            }
                                        }
                                        html += "</select></td>";
                                    }

                                    else if (textValue == "objRelationLegalGuardian.txtState")
                                    {
                                        int countryId = Convert.ToInt32(genModel.objRelationLegalGuardian.txtCountry);
                                        listDataState = objClsRef.FillState(countryId);
                                        html += "<td colspan=" + colspan + " ><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
                                        html += "<option value=0>Select " + label + "</option>";
                                        foreach (SelectListItem listSt in listDataState)
                                        {
                                            if (textValue == "objRelationLegalGuardian.txtState")
                                            {
                                                if (genModel.objRelationLegalGuardian.txtState.ToString() == listSt.Value)
                                                {
                                                    html += "<option value='" + listSt.Value + "' selected='true'>" + listSt.Text + "</option>";
                                                }
                                                else
                                                {
                                                    html += "<option value='" + listSt.Value + "'>" + listSt.Text + "</option>";
                                                }
                                            }
                                        }
                                        html += "</select></td>";
                                    }

                                    else if (textValue == "objRelationEmergncyContact.txtState")
                                    {
                                        int countryId = Convert.ToInt32(genModel.objRelationEmergncyContact.txtCountry);
                                        listDataState = objClsRef.FillState(countryId);
                                        html += "<td colspan=" + colspan + " ><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
                                        html += "<option value=0>Select " + label + "</option>";
                                        foreach (SelectListItem listSt in listDataState)
                                        {
                                            if (textValue == "objRelationEmergncyContact.txtState")
                                            {
                                                if (genModel.objRelationEmergncyContact.txtState.ToString() == listSt.Value)
                                                {
                                                    html += "<option value='" + listSt.Value + "' selected='true'>" + listSt.Text + "</option>";
                                                }
                                                else
                                                {
                                                    html += "<option value='" + listSt.Value + "'>" + listSt.Text + "</option>";
                                                }
                                            }
                                        }
                                        html += "</select></td>";
                                    }

                                    else if (textValue == "RefStateBirth")
                                    {
                                        int countryId = objClsRef.countryID();
                                        listDataState = objClsRef.FillState(countryId);
                                        html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
                                        html += "<option value=0>Select " + label + "</option>";
                                        foreach (SelectListItem listSt in listDataState)
                                        {
                                            if (textValue == "RefStateBirth")
                                            {
                                                if (genModel.RefStateBirth.ToString() == listSt.Value)
                                                {
                                                    html += "<option value='" + listSt.Value + "' selected='true'>" + listSt.Text + "</option>";
                                                }
                                                else
                                                {
                                                    html += "<option value='" + listSt.Value + "'>" + listSt.Text + "</option>";
                                                }
                                            }
                                        }
                                        html += "</select></td>";
                                    }

                                    else if (textValue == "objPhysicianDetails.txtState")
                                    {
                                        int countryId = 0;
                                        if (genModel.objPhysicianDetails != null) countryId = Convert.ToInt32(genModel.objPhysicianDetails.txtCountry);
                                        if (countryId != 0) listDataState = objClsRef.FillState(countryId);
                                        html += "<td colspan=" + colspan + " ><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
                                        html += "<option value=0>Select " + label + "</option>";
                                        foreach (SelectListItem listSt in listDataState)
                                        {
                                            if (textValue == "objPhysicianDetails.txtState")
                                            {
                                                if (genModel.objPhysicianDetails.txtState.ToString() == listSt.Value)
                                                {
                                                    html += "<option value='" + listSt.Value + "' selected='true'>" + listSt.Text + "</option>";
                                                }
                                                else
                                                {
                                                    html += "<option value='" + listSt.Value + "'>" + listSt.Text + "</option>";
                                                }
                                            }
                                        }
                                        html += "</select></td>";
                                    }


                                    else if (textValue == "objInsuranceDetails.txtState")
                                    {
                                        int countryId = 0;
                                        if (genModel.objInsuranceDetails != null) countryId = Convert.ToInt32(genModel.objInsuranceDetails.txtCountry);
                                        listDataState = objClsRef.FillState(countryId);
                                        html += "<td colspan=" + colspan + " ><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
                                        html += "<option value=0>Select " + label + "</option>";
                                        foreach (SelectListItem listSt in listDataState)
                                        {
                                            if (textValue == "objInsuranceDetails.txtState")
                                            {
                                                if (genModel.objInsuranceDetails.txtState.ToString() == listSt.Value)
                                                {
                                                    html += "<option value='" + listSt.Value + "' selected='true'>" + listSt.Text + "</option>";
                                                }
                                                else
                                                {
                                                    html += "<option value='" + listSt.Value + "'>" + listSt.Text + "</option>";
                                                }
                                            }
                                        }
                                        html += "</select></td>";
                                    }

                                    else if (textValue == "objInsuranceSecDetails.txtState")
                                    {
                                        int countryId = 0;
                                        if (genModel.objInsuranceSecDetails != null) countryId = Convert.ToInt32(genModel.objInsuranceSecDetails.txtCountry);
                                        listDataState = objClsRef.FillState(countryId);
                                        html += "<td colspan=" + colspan + " ><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
                                        html += "<option value=0>Select " + label + "</option>";
                                        foreach (SelectListItem listSt in listDataState)
                                        {
                                            if (textValue == "objInsuranceSecDetails.txtState")
                                            {
                                                if (genModel.objInsuranceSecDetails.txtState.ToString() == listSt.Value)
                                                {
                                                    html += "<option value='" + listSt.Value + "' selected='true'>" + listSt.Text + "</option>";
                                                }
                                                else
                                                {
                                                    html += "<option value='" + listSt.Value + "'>" + listSt.Text + "</option>";
                                                }
                                            }
                                        }
                                        html += "</select></td>";
                                    }


                                    else if (textValue == "objInsuranceDentalDetails.txtState")
                                    {
                                        int countryId = 0;
                                        if (genModel.objInsuranceSecDetails != null) countryId = Convert.ToInt32(genModel.objInsuranceDentalDetails.txtCountry);
                                        listDataState = objClsRef.FillState(countryId);
                                        html += "<td colspan=" + colspan + " ><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
                                        html += "<option value=0>Select " + label + "</option>";
                                        foreach (SelectListItem listSt in listDataState)
                                        {
                                            if (textValue == "objInsuranceDentalDetails.txtState")
                                            {
                                                if (genModel.objInsuranceDentalDetails.txtState.ToString() == listSt.Value)
                                                {
                                                    html += "<option value='" + listSt.Value + "' selected='true'>" + listSt.Text + "</option>";
                                                }
                                                else
                                                {
                                                    html += "<option value='" + listSt.Value + "'>" + listSt.Text + "</option>";
                                                }
                                            }
                                        }
                                        html += "</select></td>";
                                    }

                                    else
                                    {
                                        html += "<td colspan=" + colspan + " ><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
                                        html += "<option value=0>Select " + label + "</option>";
                                        html += "</select></td>";
                                    }



                                }
                                else if (label == "Gender")
                                {
                                    string textValue = cell.Attributes["modelName"].Value;

                                    html += "<td colspan=" + colspan + " ><span class='validate'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='validate[required]'>";
                                    if (genModel.RefGender == "1")
                                    {
                                        html += "<option value=''>Select Gender</option><option selected='true' value=1>Male</option><option value=2>Female</option>";
                                    }
                                    else if (genModel.RefGender == "2")
                                    {
                                        html += "<option value=''>Select Gender</option><option value=1>Male</option><option selected='true' value=2>Female</option>";
                                    }
                                    else
                                    {
                                        html += "<option value=''>Select Gender</option><option value=1>Male</option><option value=2>Female</option>";
                                    }


                                    html += "</select></td>";

                                }
                                else
                                {
                                    string textValue = cell.Attributes["modelName"].Value;

                                    html += "<td colspan=" + colspan + " ><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass2'>";
                                    html += "<option value=0>Select " + label + "</option>";
                                    html += "</select></td>";
                                }
                            }
                            else if (type == "td")
                            {
                                html += "<td colspan=" + colspan + " ></td>";
                            }

                            else
                            {
                                string result = cell.InnerText.ToString();
                                countCheck = Convert.ToInt32(cell.Attributes["Count"].Value);
                                string[] lblsplit = label.Split('|');
                                html += "<td colspan=" + colspan + " ><span class='fontwhite'>*</span><select id='" + id + "' name='" + id + "' class='drpClass1'><option value=0>-----Select.....</option>";
                                for (int i = 0; i < countCheck; i++)
                                {
                                    if (lblsplit[i] == result)
                                    {
                                        html += "<option value='" + lblsplit[i] + "' selected='true'>" + lblsplit[i] + "</option>";
                                    }
                                    else
                                    {
                                        html += "<option value='" + lblsplit[i] + "'>" + lblsplit[i] + "</option>";
                                    }

                                    //html += "<input id='" + id + "' type='checkbox' value='unchecked' name='" + idSpl[i] + "' onclick='GetSelected(this);' !checked></span>" + lblsplit[i] + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>";

                                }
                                html += "</select></td>";
                            }
                        }


                    }
                    html += "</tr>";

                }

            }
            MelmarkDBEntities objData = new MelmarkDBEntities();
            ClsCommon getcheck = new ClsCommon();
            int QId = getcheck.getQueueId("CL");
            var IsClient = objData.ref_QueueStatus.Where(x => x.SchoolId == sess.SchoolId && x.StudentPersonalId == sess.ReferralId && x.QueueId == QId).ToList();
            string permission = getcheck.setPermission();
            if (Index == 1)
            {

                if (permission == "true")
                {
                    if (IsClient.Count == 0)
                    {
                        if (isUpated == 1)
                        {
                            html += "<td colspan= 3 ; text-align:right;'><input id='btnGenSave' value='Update' type='submit' class='NFButton' /></td>";
                        }
                        else
                        {
                            html += "<td colspan=3  text-align:right;'><input id='btnGenSave' value='Save' type='submit' class='NFButton' /></td>";
                        }
                    }
                    else
                    {
                        if (isUpated == 1)
                        {
                            html += "<td colspan= 3  text-align:right;'><input id='btnGenSave' value='Update' type='submit' class='NFButton' style='display:none;'/></td>";
                        }
                        else
                        {
                            html += "<td colspan=3  text-align:right;'><input id='btnGenSave' value='Save' type='submit' class='NFButton' style='display:none;'/></td>";
                        }
                    }
                }
                else
                {
                    if (isUpated == 1)
                    {
                        html += "<td colspan= 3  text-align:right;'><input id='btnGenSave' value='Update' type='submit' class='NFButton' style='display:none;'/></td>";
                    }
                    else
                    {
                        html += "<td colspan=3  text-align:right;'><input id='btnGenSave' value='Save' type='submit' class='NFButton' style='display:none;'/></td>";
                    }
                }

                //html += "<td colspan=" + colspan + " style='width: 20%; text-align:right;'><input id='" + id + "' value='" + (isUpated == 1 ? "update" : "save") + "' type='submit' class='NFButton' /></td>";
            }
            else if (Index == 2)
            {
                if (permission == "true")
                {
                    if (IsClient.Count == 0)
                    {
                        if (isUpated == 1)
                        {
                            html += "<td colspan= 3  text-align:right;'><input id='btnFamDataSave' value='Update' type='submit' class='NFButton' /></td>";
                        }
                        else
                        {
                            html += "<td colspan=3  text-align:right;'><input id='btnFamDataSave' value='Save' type='submit' class='NFButton' /></td>";
                        }
                    }
                    else
                    {
                        if (isUpated == 1)
                        {
                            html += "<td colspan= 3  text-align:right;'><input id='btnFamDataSave' value='Update'  style='display:none;' type='submit' class='NFButton' /></td>";
                        }
                        else
                        {
                            html += "<td colspan=3  text-align:right;'><input id='btnFamDataSave' value='Save'  style='display:none;' type='submit' class='NFButton' /></td>";
                        }
                    }
                }
                else
                {
                    if (isUpated == 1)
                    {
                        html += "<td colspan= 3  text-align:right;'><input id='btnFamDataSave' value='Update' type='submit' class='NFButton' /></td>";
                    }
                    else
                    {
                        html += "<td colspan=3  text-align:right;'><input id='btnFamDataSave' value='Save' type='submit' class='NFButton' /></td>";
                    }
                }
            }

            else if (Index == 3)
            {
                if (isUpated == 1)
                {
                    html += "<td colspan= 3  text-align:right;'><input id='btnBirthDetSave' value='Update' type='submit' class='NFButton' /></td>";   //no rows on top
                }
                else
                {
                    html += "<td colspan=3  text-align:right;'><input id='btnBirthDetSave' value='Save' type='submit' class='NFButton' /></td>";
                }
            }
            else if (Index == 4)
            {
                if (isUpated == 1)
                {
                    html += "<td colspan= 3  text-align:right;'><input id='btnPersnlHistrySave' value='Update' type='submit' class='NFButton' /></td>";
                }
                else
                {
                    html += "<td colspan=3  text-align:right;'><input id='btnPersnlHistrySave' value='Save' type='submit'  class='NFButton' /></td>";
                }
            }
            else if (Index == 5)
            {
                if (isUpated == 1)
                {
                    html += "<td colspan= 3  text-align:right;'><input id='btnRecretionSave' value='Update' type='submit' class='NFButton' /></td>";
                }
                else
                {
                    html += "<td colspan=3  text-align:right;'><input id='btnRecretionSave' value='Save' type='submit' class='NFButton' /></td>";
                }
            }
            else if (Index == 6)
            {
                if (isUpated == 1)
                {
                    html += "<td colspan= 3  text-align:right;'><input id='btnPresentSkillsSave' value='Update' type='submit' class='NFButton' /></td>";
                }
                else
                {
                    html += "<td colspan=3  text-align:right;'><input id='btnPresentSkillsSave' value='Save' type='submit' class='NFButton' /></td>";
                }
            }
            else if (Index == 7)
            {
                if (isUpated == 1)
                {
                    html += "<td colspan= 3  text-align:right;'><input id='btnFundDataSave' value='Update' type='submit' class='NFButton' /></td>";
                }
                else
                {
                    html += "<td colspan=3  text-align:right;'><input id='btnFundDataSave' value='Save' type='submit' class='NFButton' /></td>";
                }
            }
            else if (Index == 8)
            {
                if (isUpated == 1)
                {
                    html += "<td  text-align:right;'><a id='lnk' href='#' style='display:none;'  onclick='ShowDocuments()'  >View Items</a></td> <td  text-align:right;'><input id='btnSaveDoccuments' value='Save' type='submit' class='NFButton' /></td>";
                }
                else
                {
                    html += "<td  text-align:right;'><a id='lnk' href='#' style='display:none;' onclick='ShowDocuments()' >View Items</a></td> <td  text-align:right;'><input id='btnSaveDoccuments' value='Save' type='submit' class='NFButton' /></td>";
                }
            }


            html += "<tr><td><input id='hidSec" + Index + "' class='tabcls' value='" + Index + "' name='TabId' type='hidden'/></td></tr>";
            html += "</table></div></div></form>";
            genModel.html = html;

            //}
            return html;

            //return View("GeneralInfoData", genModel);


        }

        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public byte[] SaveCustomXml(FormCollection formdata, int stId, int Index)
        {
            // stId = 0;   
            sess = (clsSession)Session["UserSession"];
            if (stId == 0)
                stId = Convert.ToInt32(sess.ReferralId);
            int countCheck = 0;
            byte[] retXmlByte = null;
            string formVal = "";
            string html = "";
            clsReferral objClsRef = new clsReferral();
            XmlDocument xdocCustom = new XmlDocument();
            xdocCustom = objClsRef.LoadXmlfromBlob(stId);
            //  xdocCustom.Load(Server.MapPath("~/XML/RefInfoCustom.xml"));
            XmlNodeList xSectionsCustom = xdocCustom.SelectNodes("/ReferralTemplate/Sections/Section[@name]");
            XmlNode xSectnCustom = xSectionsCustom[Index - 1];
            XmlNodeList rowsCust = xSectnCustom.ChildNodes[0].ChildNodes;


            foreach (XmlNode rowcust in rowsCust)
            {
                //html += "<tr>";
                foreach (XmlNode cellCust in rowcust.ChildNodes)
                {
                    bool val = objClsRef.IsModel(cellCust);
                    if (!val)
                    {
                        string id = cellCust.Attributes["id"].Value;
                        string typeD = cellCust.Attributes["type"].Value;
                        string label = cellCust.Attributes["label"].Value;
                        string colspan = cellCust.Attributes["colspan"].Value;
                        // if (type == "label")
                        //html += "<td class='tdtext' colspan=" + colspan + " style='width: 20%;'>" + label + "</td>";

                        if (typeD == "input[text]")
                        {
                            formVal = formdata[id].ToString();

                            cellCust.InnerText = formVal;
                        }
                        else if (typeD == "input[textfunction]")
                        {
                            formVal = formdata[id].ToString();

                            cellCust.InnerText = formVal;
                        }
                        else if (typeD == "input[textvalidate]")
                        {
                            formVal = formdata[id].ToString();

                            cellCust.InnerText = formVal;
                        }
                        else if (typeD == "input[textdate]")
                        {
                            formVal = formdata[id].ToString();

                            cellCust.InnerText = formVal;
                        }
                        else if (typeD == "input[multitext]")
                        {
                            formVal = formdata[id].ToString();

                            cellCust.InnerText = formVal;
                        }
                        else if (typeD == "input[checkVt]")
                        {
                            countCheck = Convert.ToInt32(cellCust.Attributes["Count"].Value);
                            string[] idSpl = id.Split('|');
                            string[] lblsplit = label.Split('|');
                            cellCust.InnerText = "";
                            for (int i = 0; i < countCheck; i++)
                            {
                                try
                                {
                                    formVal = formdata[idSpl[i]].ToString();
                                    if (formVal == "checked")
                                    {
                                        cellCust.InnerText += "checked|";
                                    }
                                    else
                                    {
                                        cellCust.InnerText += "unchecked|";
                                    }

                                }
                                catch
                                {
                                    cellCust.InnerText += "unchecked|";
                                }

                            }
                            cellCust.InnerText = cellCust.InnerText.Substring(0, cellCust.InnerText.Length - 1);


                        }

                        else if (typeD == "input[drop]")
                        {
                            countCheck = Convert.ToInt32(cellCust.Attributes["Count"].Value);
                            string[] lblsplit = label.Split('|');
                            cellCust.InnerText = "";
                            for (int i = 0; i < countCheck; i++)
                            {

                                formVal = formdata[id].ToString();
                                cellCust.InnerText = formVal;

                            }



                        }


                        //else if (typeD == "input[radiosingleselect]")
                        //{
                        //    countCheck = Convert.ToInt32(cellCust.Attributes["Count"].Value);
                        //    string name = cellCust.Attributes["name"].Value.ToString();
                        //    string[] idSpl = id.Split('|');
                        //    string[] lblsplit = label.Split('|');
                        //    cellCust.InnerText = "";
                        //    for (int i = 0; i < countCheck; i++)
                        //    {
                        //        try
                        //        {
                        //            formVal = formdata[idSpl[i]].ToString();
                        //            if (formVal == "checked")
                        //            {
                        //                cellCust.InnerText += "checked|";
                        //            }
                        //            else
                        //            {
                        //                cellCust.InnerText += "unchecked|";
                        //            }

                        //        }
                        //        catch
                        //        {
                        //            cellCust.InnerText += "unchecked|";
                        //        }

                        //    }
                        //    cellCust.InnerText = cellCust.InnerText.Substring(0, cellCust.InnerText.Length - 1);


                        //}

                        else if (typeD == "input[checkNolbl]")
                        {
                            countCheck = Convert.ToInt32(cellCust.Attributes["Count"].Value);
                            string[] idSpl = id.Split('|');
                            string[] lblsplit = label.Split('|');
                            cellCust.InnerText = "";
                            for (int i = 0; i < countCheck; i++)
                            {
                                try
                                {
                                    formVal = formdata[idSpl[i]].ToString();
                                    if (formVal == "checked")
                                    {
                                        cellCust.InnerText += "checked|";
                                    }
                                    else
                                    {
                                        cellCust.InnerText += "unchecked|";
                                    }

                                }
                                catch
                                {
                                    cellCust.InnerText += "unchecked|";
                                }

                            }
                            cellCust.InnerText = cellCust.InnerText.Substring(0, cellCust.InnerText.Length - 1);

                            //html += "<td colspan=" + colspan + " style='width:20%;'>";
                            //for (int i = 0; i < countCheck; i++)
                            //{
                            //    html += "<input id='" + idSpl[i] + "' type='checkbox'>" + lblsplit[i] + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</input>";
                            //}
                            //html += "</td>";

                        }
                        else if (typeD == "input[radio]")
                        {
                            countCheck = Convert.ToInt32(cellCust.Attributes["Count"].Value);
                            string name = cellCust.Attributes["name"].Value.ToString();
                            string[] idSpl = id.Split('|');
                            string[] lblsplit = label.Split('|');
                            cellCust.InnerText = "";
                            for (int i = 0; i < countCheck; i++)
                            {
                                try
                                {
                                    formVal = formdata[name].ToString();
                                    if (formVal == "checked")
                                    {
                                        cellCust.InnerText += "checked|";



                                    }
                                    else
                                    {
                                        cellCust.InnerText += "unchecked|";

                                    }

                                }
                                catch
                                {
                                    cellCust.InnerText += "unchecked|";

                                }


                            }

                            cellCust.InnerText = cellCust.InnerText.Substring(0, cellCust.InnerText.Length - 1);


                        }



                    }
                }
                html += "</tr>";

            }

            xdocCustom.PreserveWhitespace = true;
            string virtualPath = Server.MapPath("../XML/Data" + stId + ".xml");
            xdocCustom.Save(virtualPath);

            retXmlByte = objClsRef.SaveAsBlob(virtualPath);

            if (retXmlByte != null)
            {
                System.IO.File.Delete(virtualPath);
            }


            return retXmlByte;

        }


        //View Uploaded files
        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult UploadedFilesView(DocumentDownloadViewModel docDownload = null, string Name = null, string Add_doc = null)
        {
            sess = (clsSession)Session["UserSession"];
            docDownload = new DocumentDownloadViewModel();
            if (sess != null)
            {
                try
                {
                    if (Name == null)
                    {
                        MelmarkDBEntities objData = new MelmarkDBEntities();
                        docDownload.DocumentList = (from objDoc in objData.binaryFiles
                                                    join y in objData.Documents on objDoc.DocId equals y.DocumentId
                                                    join z in objData.LookUps on y.DocumentType equals z.LookupId
                                                    where (objDoc.type == "Referal" && objDoc.SchoolId == sess.SchoolId && objDoc.StudentId == sess.ReferralId)

                                                    select new DocumentList
                                                    {
                                                        DocName = objDoc.DocumentName,
                                                        DocId = objDoc.BinaryId,
                                                        DocPath = z.LookupName,
                                                        CreatedOn = objDoc.CreatedOn

                                                    }).ToList();

                        foreach (var docs in docDownload.DocumentList)
                        {
                            if (docs.CreatedOn != null)
                                docs.CreatedDate = ((DateTime)docs.CreatedOn).ToString("MM/dd/yyyy");
                        }
                    }
                    else
                    {
                        MelmarkDBEntities objData = new MelmarkDBEntities();
                        docDownload.DocumentList = (from objDoc in objData.binaryFiles
                                                    join y in objData.Documents on objDoc.DocId equals y.DocumentId
                                                    join z in objData.LookUps on y.DocumentType equals z.LookupId
                                                    where (objDoc.type == "Referal" && objDoc.SchoolId == sess.SchoolId && objDoc.StudentId == sess.ReferralId)

                                                    select new DocumentList
                                                    {
                                                        DocName = objDoc.DocumentName,
                                                        DocId = objDoc.BinaryId,
                                                        DocPath = z.LookupName,
                                                        CreatedOn = objDoc.CreatedOn

                                                    }).ToList();

                        foreach (var docs in docDownload.DocumentList)
                        {
                            if (docs.CreatedOn != null)
                                docs.CreatedDate = ((DateTime)docs.CreatedOn).ToString("MM/dd/yyyy");
                        }
                        if (!String.IsNullOrEmpty(Name))
                            docDownload.DocumentList = docDownload.DocumentList.Where(p => p.DocName != null
                                && (p.DocName.ToLower().Contains(Name.ToLower())
                                || p.DocPath.ToLower().Contains(Name.ToLower()))).ToList();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            docDownload.DocVisible = Add_doc;
            return View(docDownload);

        }

        public JsonResult otherDocumentTypes(string term)
        {
            MelmarkDBEntities RPCobj = new MelmarkDBEntities();
            sess = (clsSession)Session["UserSession"];
            GridDocument listModel = new GridDocument();
            IList<GridDocument> retunmodel = new List<GridDocument>();


            retunmodel = (from objDocuments in RPCobj.Documents
                          where objDocuments.Other != null
                          select new GridDocument
                          {
                              OtherDocumentType = objDocuments.Other,
                          }).Distinct().ToList();



            var result = (from r in retunmodel
                          where r.OtherDocumentType.ToLower().Contains(term.ToLower())
                          select new { r.OtherDocumentType }).Distinct();



            return Json(result, JsonRequestBehavior.AllowGet);


        }



    }



}

