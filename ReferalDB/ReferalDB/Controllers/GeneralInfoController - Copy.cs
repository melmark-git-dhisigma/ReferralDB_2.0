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
            bool tabSaved = false;
            int studentId = 0;
            clsReferral clsRef = new clsReferral();
            sess = (clsSession)Session["UserSession"];
            // clsSessionTab sesClsObj = (clsSessionTab)Session["sesData"];
            bool valid = false;

            if (sess != null)
            {
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

                ViewBag.html = XmlResult(model, id);

            }
            else
            {
                GenInfoModel model = new GenInfoModel();
                ViewBag.html = XmlResult(model, id);
                //if (isAlert == true)
                //{
                //    ViewBag.html += " <div id='popupDiv'>Data Saved Successfully</div>";
                //}
            }
            return View();

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
            string xmlPath = Server.MapPath("../XML/RefInfoCustom.xml");
            TempData["type"] = type;

            if (sess != null)
            {

                if (model.TabId == 1)
                {
                    int tempId = 0;
                    tempId = Convert.ToInt32(Session["RefId"]);
                    if (sess.SessTab1 > 0 || tempId > 0)
                    {
                        status = "update";
                        sveId = Convert.ToInt32(sess.SessTab1);
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
                        if (sess.SessTab2 > 0)
                        {
                            status = "update";
                            sveId = Convert.ToInt32(sess.SessTab2);
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
                    Response.Write("<script langauge='javascript'>alert('hi1')</script>");
                    if (Session["RefId"] != null)
                    {
                        int tempId = 0;
                        tempId = Convert.ToInt32(Session["RefId"]);
                        ClsErrorLog errlog = new ClsErrorLog();
                        errlog.WriteToLog("tempId: " + tempId);
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
                        if (sess.SessTab4 > 0)
                        {
                            status = "update";
                            sveId = Convert.ToInt32(sess.SessTab4);
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
                        if (sess.SessTab5 > 0)
                        {
                            status = "update";
                            sveId = Convert.ToInt32(sess.SessTab5);
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
                        if (sess.SessTab6 > 0)
                        {
                            status = "update";
                            sveId = Convert.ToInt32(sess.SessTab6);
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
                        if (sess.SessTab7 > 0)
                        {
                            status = "update";
                            sveId = Convert.ToInt32(sess.SessTab7);
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
                                sveId = Convert.ToInt32(Session["RefId"]);
                                TempData["EditId"] = id = sveId;

                                UploadDoccuments(id, model, fileUpldDocName, sess.SchoolId, sess.LoginId);

                                //   xmlData = SaveCustomXml(formdata, id, model.TabId);
                                //   clsRf.SaveXmlToDB(xmlData, id);
                                sess.SessTab8 = id;
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
            if (upldFile != null && upldFile.ContentLength > 0)
            {

                string fileName = Path.GetFileName(upldFile.FileName);

                //string path = @"D:\SavedDocs\";
                //if (!Directory.Exists(path))
                //{
                //    Directory.CreateDirectory(path);
                //}


                int Docid = objRef.FileUpload(studentPersnlId, schoolId, loginId, model.objclsUpld.DocName, fileName, model.objclsUpld.DocType);
                if (Docid > 0)

                    objBinary.SaveBinaryFiles(sess.SchoolId, sess.ReferralId, model.objclsUpld.DocName, sess.LoginId, upldFile, "Referal", "GenInfo", Docid);


                //  upldFile.SaveAs(path + id + "-" + fileName);


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
            // int formid = 0;
            XmlNode xSectn = xSections[Index - 1];
            //foreach (XmlNode xSectn in xSections)
            //{
            // formid++;
            string TableIds = "tblData" + Index;
            html += " <form method='post' id='frm" + Index + "' enctype='multipart/form-data' action='../GeneralInfo/SaveGeneralData?type=" + xSectn.Attributes["name"].Value + "'><div><span class='headPanel'  onclick='getColapse(\"" + TableIds + "\")'>" + xSectn.Attributes["name"].Value + "</span><div class='divSubs'><table id='" + TableIds + "' class='clsTabDiv' style='float:left;'>";
            XmlNodeList rows = xSectn.ChildNodes[0].ChildNodes;
            foreach (XmlNode row in rows)
            {
                html += "<tr>";
                foreach (XmlNode cell in row.ChildNodes)
                {
                    string id = cell.Attributes["id"].Value;
                    string type = cell.Attributes["type"].Value;
                    string label = cell.Attributes["label"].Value;
                    string colspan = cell.Attributes["colspan"].Value;
                    if (type == "label")
                        html += "<td class='tdtext' colspan=" + colspan + " style='width: 20%;'>" + label + "</td>";

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
                                    value = genModel.objPhysicianDetails.GetType().GetProperty(split[1]).GetValue(genModel.objPhysicianDetails, null);
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

                        html += "<td colspan=" + colspan + " style='width: 20%;'><span class='fontwhite'>*<span><input id='" + id + "' name='" + textValue.ToString() + "' value='" + value + "' maxlength='" + maxLength + "' type='text' /></td>";
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
                                    value = genModel.objPhysicianDetails.GetType().GetProperty(split[1]).GetValue(genModel.objPhysicianDetails, null);
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
                            html += "<td colspan=" + colspan + " style='width: 20%;'><span class='validate'>*<span><input id='" + id + "' name='" + textValue.ToString() + "' value='" + value + "' class='" + className + "' maxlength='" + maxLength + "' type='text' /></td>";

                        else
                            html += "<td colspan=" + colspan + " style='width: 20%;'><span class='fontwhite'>*<span><input id='" + id + "' name='" + textValue.ToString() + "' value='" + value + "' class='" + className + "' maxlength='" + maxLength + "' type='text' /></td>";
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
                                    value = genModel.objPhysicianDetails.GetType().GetProperty(split[1]).GetValue(genModel.objPhysicianDetails, null);
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
                            html += "<td colspan=" + colspan + " style='width: 20%;'><span class='validate'>*<span><input id='" + id + "' name='" + textValue.ToString() + "' value='" + value + "' maxlength='" + maxLength + "' class='validate[required]' onkeypress='" + funName + "' type='text' /></td>";
                        }
                        else html += "<td colspan=" + colspan + " style='width: 20%;'><span class='fontwhite'>*<span><input id='" + id + "' name='" + textValue.ToString() + "' value='" + value + "' maxlength='" + maxLength + "' onkeypress='" + funName + "' type='text' /></td>";


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
                                    value = genModel.objPhysicianDetails.GetType().GetProperty(split[1]).GetValue(genModel.objPhysicianDetails, null);
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

                        html += "<td colspan=" + colspan + " style='width: 20%;'><span class='fontwhite'>*<span><input id='" + id + "' name='" + textValue.ToString() + "' value='" + value + "' style='width: " + width + ";' maxlength='" + maxLength + "' onkeypress='" + funName + "' type='text' /><span style='color:#595959;'>" + keyName + "</span></td>";

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
                                    value = genModel.objPhysicianDetails.GetType().GetProperty(split[1]).GetValue(genModel.objPhysicianDetails, null);
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

                        html += "<td colspan=" + colspan + " style='width: 20%;'><span class='fontwhite'>*<span><textarea id='" + id + "' name='" + textValue.ToString() + "' value='" + value + "' type='text' style='width:" + width + ";' rows='3'>" + value + "</textarea></td>";
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
                                    value = genModel.objPhysicianDetails.GetType().GetProperty(split[1]).GetValue(genModel.objPhysicianDetails, null);
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
                            html += "<td colspan=" + colspan + " style='width: 20%;'><span class='validate'>*<span><input id='" + id + "' name='" + textValue.ToString() + "' class='validate[required] datepicker' onkeypress='return false' value='" + value + "' type='text' /></td>";
                        }
                        else
                            html += "<td colspan=" + colspan + " style='width: 20%;'><span class='fontwhite'>*<span><input id='" + id + "' name='" + textValue.ToString() + "' class='datepicker' onkeypress='return false' value='" + value + "' type='text' /></td>";
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
                        html += "<td colspan=" + colspan + " style='width: 20%;'><span class='fontwhite'>*<span><input id='" + id + "' name='" + nameFile + "' type='file' onchange ='selectFileImage();' /></td>";
                    }
                    else if (type == "input[check]")
                    {
                        countCheck = Convert.ToInt32(cell.Attributes["Count"].Value);
                        string[] idSplit = id.Split('|');
                        string[] lblSplit = label.Split('|');
                        html += "<td colspan=" + colspan + " style='width: 20%;'>";
                        for (int i = 0; i < countCheck; i++)
                        {
                            html += "<input id='" + idSplit[i] + "' type='checkbox'>" + lblSplit[i] + "</input><br>";
                        }
                        html += "</td>";

                    }
                    else if (type == "lblBold")
                    {
                        html += "<td class='lblBold' colspan=" + colspan + " style='width: 20%;'>" + label + "</td>";
                    }
                    else if (type == "input[checkVt]")
                    {
                        countCheck = Convert.ToInt32(cell.Attributes["Count"].Value);
                        string[] idSpl = id.Split('|');
                        string[] lblsplit = label.Split('|');
                        html += "<td colspan=" + colspan + " style='width:20%;'>";
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
                                html += "<td colspan=" + colspan + " style='width: 20%;'>";
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
                            else
                            {
                                html += "<td colspan=" + colspan + " style='width: 20%;'>";

                                foreach (SelectListItem item in listMar)
                                {
                                    html += "<input id='" + item.Value + "' type='radio' value='" + item.Value + "' name='" + textValue + "'><label>" + item.Text + "</label></input>";
                                }
                                html += "</td>";
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
                            html += "<td colspan=" + colspan + " style='width: 20%;'><span class='fontwhite'>*<span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
                            html += "<option value=0>Select " + label + "</option>";

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


                    else if (type == "input[drop]")
                    {
                        IEnumerable<SelectListItem> listData = new List<SelectListItem>();
                        IEnumerable<SelectListItem> listDataState = new List<SelectListItem>();
                        if (label == "Country")
                        {
                            listData = objClsRef.FillDropList("Country");
                        }
                        //else if (label == "State")
                        //{
                        //    listData = objClsRef.FillDropList("State");
                        //}


                        if (label == "Country")
                        {
                            string textValue = cell.Attributes["modelName"].Value;
                            string stateVal = "0";

                            if (textValue == "RefCountry")
                            {
                                html += "<td colspan=" + colspan + " style='width: 20%;'><span class='validate'>*<span><select id='" + id + "' name='" + textValue.ToString() + "' class='validate[required]' onchange='GetState(this.id," + stateVal + ")'>";
                                html += "<option value=''>Select " + label + "</option>";
                            }
                            else
                            {
                                html += "<td colspan=" + colspan + " style='width: 20%;'><span class='fontwhite'>*<span><select id='" + id + "' name='" + textValue.ToString() + "' onchange='GetState(this.id," + stateVal + ")'>";
                                html += "<option value=0>Select " + label + "</option>";
                            }



                            foreach (SelectListItem list in listData)
                            {
                                if (textValue == "objRelationMthr.txtCountry")
                                {
                                    if (genModel.objRelationMthr.txtCountry != null)
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
                                    if (genModel.objPhysicianDetails.txtCountry != null)
                                    {
                                        if (genModel.objPhysicianDetails.txtCountry.ToString() == list.Value)
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
                                else if (textValue == "objInsuranceDetails.txtCountry")
                                {
                                    if (genModel.objInsuranceDetails.txtCountry != null)
                                    {
                                        if (genModel.objInsuranceDetails.txtCountry.ToString() == list.Value)
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
                                else if (textValue == "objInsuranceSecDetails.txtCountry")
                                {
                                    if (genModel.objInsuranceSecDetails.txtCountry != null)
                                    {
                                        if (genModel.objInsuranceSecDetails.txtCountry.ToString() == list.Value)
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
                                else if (textValue == "objInsuranceDentalDetails.txtCountry")
                                {
                                    if (genModel.objInsuranceDentalDetails.txtCountry != null)
                                    {
                                        if (genModel.objInsuranceDentalDetails.txtCountry.ToString() == list.Value)
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
                                html += "<td colspan=" + colspan + " style='width: 20%;'><span class='validate'>*<span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
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
                                html += "<td colspan=" + colspan + " style='width: 20%;'><span class='fontwhite'>*<span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
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
                                html += "<td colspan=" + colspan + " style='width: 20%;'><span class='fontwhite'>*<span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
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
                                    html += "<td colspan=" + colspan + " style='width: 20%;'><span class='fontwhite'>*<span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
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
                                html += "<td colspan=" + colspan + " style='width: 20%;'><span class='fontwhite'>*<span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
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
                                html += "<td colspan=" + colspan + " style='width: 20%;'><span class='fontwhite'>*<span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
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
                                html += "<td colspan=" + colspan + " style='width: 20%;'><span class='fontwhite'>*<span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
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
                                int countryId = Convert.ToInt32(genModel.objPhysicianDetails.txtCountry);
                                listDataState = objClsRef.FillState(countryId);
                                html += "<td colspan=" + colspan + " style='width: 20%;'><span class='fontwhite'>*<span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
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
                                int countryId = Convert.ToInt32(genModel.objInsuranceDetails.txtCountry);
                                listDataState = objClsRef.FillState(countryId);
                                html += "<td colspan=" + colspan + " style='width: 20%;'><span class='fontwhite'>*<span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
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
                                int countryId = Convert.ToInt32(genModel.objInsuranceSecDetails.txtCountry);
                                listDataState = objClsRef.FillState(countryId);
                                html += "<td colspan=" + colspan + " style='width: 20%;'><span class='fontwhite'>*<span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
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
                                int countryId = Convert.ToInt32(genModel.objInsuranceDentalDetails.txtCountry);
                                listDataState = objClsRef.FillState(countryId);
                                html += "<td colspan=" + colspan + " style='width: 20%;'><span class='fontwhite'>*<span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
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
                                html += "<td colspan=" + colspan + " style='width: 20%;'><span class='fontwhite'>*<span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
                                html += "<option value=0>Select " + label + "</option>";
                                html += "</select></td>";
                            }



                        }
                        else if (label == "Gender")
                        {
                            string textValue = cell.Attributes["modelName"].Value;

                            html += "<td colspan=" + colspan + " style='width: 20%;'><span class='validate'>*<span><select id='" + id + "' name='" + textValue.ToString() + "' class='validate[required]'>";
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

                            html += "<td colspan=" + colspan + " style='width: 20%;'><span class='fontwhite'>*<span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
                            html += "<option value=0>Select " + label + "</option>";
                            html += "</select></td>";
                        }
                    }

                    else if (type == "td")
                    {
                        html += "<td colspan=" + colspan + " style='width:20%;'></td>";
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

                        if (type == "label")
                            html += "<td class='tdtext' colspan=" + colspan + " style='width: 20%;'>" + label + "</td>";
                        else if (type == "input[text]")
                        {
                            html += "<td colspan=" + colspan + " style='width: 20%;'><span class='fontwhite'>*<span><input id='" + id + "' name='" + id + "' type='text' /></td>";
                        }
                        else if (type == "input[textfunction]")
                        {
                            string maxLength = cellCust.Attributes["Maxlength"].Value;
                            string funName = cellCust.Attributes["onkeypress"].Value;
                            html += "<td colspan=" + colspan + " style='width: 20%;'><span class='fontwhite'>*<span><input id='" + id + "' name='" + id + "' maxlength='" + maxLength + "' onkeypress='" + funName + "' type='text' /></td>";
                        }

                        else if (type == "input[textvalidate]")
                        {
                            string className = cellCust.Attributes["className"].Value;
                            html += "<td colspan=" + colspan + " style='width: 20%;'><span class='fontwhite'>*<span><input id='" + id + "' name='" + id + "' class='" + className + "' type='text' /></td>";
                        }

                        else if (type == "input[textdate]")
                        {
                            html += "<td colspan=" + colspan + " style='width: 20%;'><span class='fontwhite'>*<span><input id='" + id + "' onkeypress='return false' name='" + id + "' type='text' class='datepicker' /></td>";
                        }

                        else if (type == "input[multitext]")
                        {
                            string width = cellCust.Attributes["Width"].Value;
                            html += "<td colspan=" + colspan + " style='width: 20%;'><span class='fontwhite'>*<span><textarea id='" + id + "' name='" + id + "' type='text' style='width:" + width + ";' rows='3'></textarea></td>";
                        }
                        else if (type == "input[checkVt]")
                        {

                            countCheck = Convert.ToInt32(cellCust.Attributes["Count"].Value);
                            string[] idSpl = id.Split('|');
                            string[] lblsplit = label.Split('|');
                            html += "<td colspan=" + colspan + " style='width:20%;'>";
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
                            html += "<td colspan=" + colspan + " style='width:20%;'><span class='fontwhite'>*<span><select id='" + id + "' name='" + id + "' class='drpClass'><option value=0>-----Select.....</option>";
                            for (int i = 0; i < countCheck; i++)
                            {
                                html += "<option value='" + lblsplit[i] + "'>" + lblsplit[i] + "</option>";

                                //html += "<input id='" + id + "' type='checkbox' value='unchecked' name='" + idSpl[i] + "' onclick='GetSelected(this);' !checked><span>" + lblsplit[i] + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>";

                            }
                            html += "</select></td>";

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
                            html += "<td colspan=" + colspan + " style='width:20%;'>";
                            for (int i = 0; i < countCheck; i++)
                            {
                                html += "<input id='" + idSpl[i] + "' type='checkbox' value='unchecked' name='" + idSpl[i] + "' onclick='GetSelected(this);' !checked>";
                            }
                            html += "</td>";

                        }
                        else if (type == "input[file]")
                        {
                            string nameFile = cellCust.Attributes["name"].Value.ToString();
                            html += "<td colspan=" + colspan + " style='width: 20%;'><span class='fontwhite'>*<span><input id='" + id + "' name='" + nameFile + "' type='file'/></td>";
                        }
                        else if (type == "input[check]")
                        {
                            countCheck = Convert.ToInt32(cellCust.Attributes["Count"].Value);
                            string[] idSplit = id.Split('|');
                            string[] lblSplit = label.Split('|');
                            html += "<td colspan=" + colspan + " style='width: 20%;'>";
                            for (int i = 0; i < countCheck; i++)
                            {
                                html += "<input id='" + idSplit[i] + "' type='checkbox'>" + lblSplit[i] + "</input><br>";
                            }
                            html += "</td>";

                        }
                        else if (type == "lblBold")
                        {
                            html += "<td class='lblBold' colspan=" + colspan + " style='width: 20%;'>" + label + "</td>";
                        }
                        else if (type == "td")
                        {
                            html += "<td colspan=" + colspan + " style='width:20%;'></td>";
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
                                html += "<td class='tdtext' colspan=" + colspan + " style='width: 20%;'>" + label + "</td>";
                            else if (type == "input[text]")
                            {
                                html += "<td colspan=" + colspan + " style='width: 20%;'><span class='fontwhite'>*<span><input id='" + id + "' name='" + id + "' type='text' value='" + cellCustEdit.InnerText + "' /></td>";
                            }

                            else if (type == "input[textfunction]")
                            {
                                string funName = cellCustEdit.Attributes["onkeypress"].Value;
                                html += "<td colspan=" + colspan + " style='width: 20%;'><span class='fontwhite'>*<span><input id='" + id + "' name='" + id + "' value='" + cellCustEdit.InnerText + "' onkeypress='" + funName + "' type='text' /></td>";
                            }

                            else if (type == "input[textvalidate]")
                            {
                                string className = cellCustEdit.Attributes["className"].Value;
                                html += "<td colspan=" + colspan + " style='width: 20%;'><span class='fontwhite'>*<span><input id='" + id + "' name='" + id + "' value='" + cellCustEdit.InnerText + "' class='" + className + "' type='text' /></td>";
                            }
                            else if (type == "input[multitext]")
                            {
                                string width = cellCustEdit.Attributes["Width"].Value;
                                html += "<td colspan=" + colspan + " style='width: 20%;'><span class='fontwhite'>*<span><textarea id='" + id + "' name='" + id + "' type='text' style='width:" + width + ";' rows='3' value='" + cellCustEdit.InnerText + "'>" + cellCustEdit.InnerText + "</textarea></td>";
                            }
                            else if (type == "input[textdate]")
                            {
                                html += "<td colspan=" + colspan + " style='width: 20%;'><span class='fontwhite'>*<span><input id='" + id + "' value='" + cellCustEdit.InnerText + "' onkeypress='return false'  name='" + id + "' type='text' class='datepicker' /></td>";
                            }

                            else if (type == "input[drop]")
                            {
                                string result = cellCustEdit.InnerText.ToString();
                                countCheck = Convert.ToInt32(cellCustEdit.Attributes["Count"].Value);
                                string[] lblsplit = label.Split('|');
                                html += "<td colspan=" + colspan + " style='width:20%;'><span class='fontwhite'>*<span><select id='" + id + "' name='" + id + "' class='drpClass'><option value=0>-----Select.....</option>";
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

                            else if (type == "input[file]")
                            {
                                string nameFile = cellCustEdit.Attributes["name"].Value.ToString();
                                html += "<td colspan=" + colspan + " style='width: 20%;'><span class='fontwhite'>*<span><input id='" + id + "' name='" + nameFile + "' type='file' /></td>";
                            }
                            else if (type == "input[check]")
                            {
                                countCheck = Convert.ToInt32(cellCustEdit.Attributes["Count"].Value);
                                string[] idSplit = id.Split('|');
                                string[] lblSplit = label.Split('|');
                                html += "<td colspan=" + colspan + " style='width: 20%;'>";
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
                                html += "<td colspan=" + colspan + " style='width:20%;'>";
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
                                html += "<td colspan=" + colspan + " style='width:20%;'>";
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
                                html += "<td class='lblBold' colspan=" + colspan + " style='width: 20%;'>" + label + "</td>";
                            }
                            else if (type == "label")
                            {
                                html += "<td class='tdtext' colspan=" + colspan + " style='width: 20%;'>" + label + "</td>";
                            }
                            else if (type == "td")
                            {
                                html += "<td colspan=" + colspan + " style='width:20%;'></td>";
                            }

                        }
                        html += "</tr>";
                    }
                }
            }

            if (Index == 1)
            {
                MelmarkDBEntities objData = new MelmarkDBEntities();
                ClsCommon getcheck = new ClsCommon();
                int QId = getcheck.getQueueId("CL");
                var IsClient = objData.ref_QueueStatus.Where(x => x.SchoolId == sess.SchoolId && x.StudentPersonalId == sess.ReferralId && x.QueueId == QId).ToList();
                if (IsClient.Count == 0)
                {
                    if (isUpated == 1)
                    {
                        html += "<td colspan= 3 style='width: 20%; text-align:right;'><input id='btnGenSave' value='Update' type='submit' class='NFButton' /></td>";
                    }
                    else
                    {
                        html += "<td colspan=3 style='width: 20%; text-align:right;'><input id='btnGenSave' value='Save' type='submit' class='NFButton' /></td>";
                    }
                }
                else
                {
                    if (isUpated == 1)
                    {
                        html += "<td colspan= 3 style='width: 20%; text-align:right;'><input id='btnGenSave' value='Update' type='submit' class='NFButton' style='display:none;'/></td>";
                    }
                    else
                    {
                        html += "<td colspan=3 style='width: 20%; text-align:right;'><input id='btnGenSave' value='Save' type='submit' class='NFButton' style='display:none;'/></td>";
                    }
                }

                //html += "<td colspan=" + colspan + " style='width: 20%; text-align:right;'><input id='" + id + "' value='" + (isUpated == 1 ? "update" : "save") + "' type='submit' class='NFButton' /></td>";
            }
            else if (Index == 2)
            {
                if (isUpated == 1)
                {
                    html += "<td colspan= 3 style='width: 20%; text-align:right;'><input id='btnFamDataSave' value='Update' type='submit' class='NFButton' /></td>";
                }
                else
                {
                    html += "<td colspan=3 style='width: 20%; text-align:right;'><input id='btnFamDataSave' value='Save' type='submit' class='NFButton' /></td>";
                }
            }

            else if (Index == 3)
            {
                if (isUpated == 1)
                {
                    html += "<td colspan= 3 style='width: 20%; text-align:right;'><input id='btnBirthDetSave' value='Update' type='submit' class='NFButton' /></td>";   //no rows on top
                }
                else
                {
                    html += "<td colspan=3 style='width: 20%; text-align:right;'><input id='btnBirthDetSave' value='Save' type='submit' class='NFButton' /></td>";
                }
            }
            else if (Index == 4)
            {
                if (isUpated == 1)
                {
                    html += "<td colspan= 3 style='width: 20%; text-align:right;'><input id='btnPersnlHistrySave' value='Update' type='submit' class='NFButton' /></td>";
                }
                else
                {
                    html += "<td colspan=3 style='width: 20%; text-align:right;'><input id='btnPersnlHistrySave' value='Save' type='submit'  class='NFButton' /></td>";
                }
            }
            else if (Index == 5)
            {
                if (isUpated == 1)
                {
                    html += "<td colspan= 3 style='width: 20%; text-align:right;'><input id='btnRecretionSave' value='Update' type='submit' class='NFButton' /></td>";
                }
                else
                {
                    html += "<td colspan=3 style='width: 20%; text-align:right;'><input id='btnRecretionSave' value='Save' type='submit' class='NFButton' /></td>";
                }
            }
            else if (Index == 6)
            {
                if (isUpated == 1)
                {
                    html += "<td colspan= 3 style='width: 20%; text-align:right;'><input id='btnPresentSkillsSave' value='Update' type='submit' class='NFButton' /></td>";
                }
                else
                {
                    html += "<td colspan=3 style='width: 20%; text-align:right;'><input id='btnPresentSkillsSave' value='Save' type='submit' class='NFButton' /></td>";
                }
            }
            else if (Index == 7)
            {
                if (isUpated == 1)
                {
                    html += "<td colspan= 3 style='width: 20%; text-align:right;'><input id='btnFundDataSave' value='Update' type='submit' class='NFButton' /></td>";
                }
                else
                {
                    html += "<td colspan=3 style='width: 20%; text-align:right;'><input id='btnFundDataSave' value='Save' type='submit' class='NFButton' /></td>";
                }
            }
            else if (Index == 8)
            {
                if (isUpated == 1)
                {
                    html += "<td style='width: 20%; text-align:right;'><a id='lnk' href='#'  onclick='ShowDocuments()'  >View Items</a></td> <td style='width: 20%; text-align:right;'><input id='btnSaveDoccuments' value='Save' type='submit' class='NFButton' /></td>";
                }
                else
                {
                    html += "<td style='width: 20%; text-align:right;'><a id='lnk' href='#' onclick='ShowDocuments()' >View Items</a></td> <td style='width: 20%; text-align:right;'><input id='btnSaveDoccuments' value='Save' type='submit' class='NFButton' /></td>";
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
        public ActionResult UploadedFilesView(DocumentDownloadViewModel docDownload)
        {
            sess = (clsSession)Session["UserSession"];
            docDownload = new DocumentDownloadViewModel();
            //docDownload = new List<DocumentDownloadViewModel>();
            if (sess != null)
            {
                try
                {

                    MelmarkDBEntities objData = new MelmarkDBEntities();
                    docDownload.DocumentList = (from objDoc in objData.binaryFiles
                                                where (objDoc.type == "Referal" && objDoc.ModuleName == "GenInfo" && objDoc.SchoolId == sess.SchoolId && objDoc.StudentId == sess.ReferralId)
                                                select new DocumentList
                                                {
                                                    DocName = objDoc.DocumentName,
                                                    DocId = objDoc.BinaryId,
                                                    DocPath = "",
                                                    CreatedOn = objDoc.CreatedOn

                                                }).ToList();
                    foreach (var docs in docDownload.DocumentList)
                    {
                        if (docs.CreatedOn != null)
                            docs.CreatedDate = ((DateTime)docs.CreatedOn).ToString("MM'/'dd'/'yyyy");
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return View(docDownload);
            //}
            //else
            //    return View(LetterGeneration);

        }
    }
}
