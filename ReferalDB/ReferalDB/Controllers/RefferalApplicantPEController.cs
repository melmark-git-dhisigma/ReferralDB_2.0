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
    public class RefferalApplicantPEController : Controller
    {
        //
        // GET: /RefferalApplicantPE/
        public clsSession sess = null;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RefferalApplicantPE()
        {
            //  clsSessionTab sesClsObj = new clsSessionTab();
            // Session["sesData"] = sesClsObj;
            ClsCommon getCommon = new ClsCommon();
            ViewBag.permission = getCommon.setPermission();
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
                //  Session["RefId"] = null;

            }

        }

        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult TabLoad(int id, bool isAlert = false)
        {
            bool tabSaved = false;
            int studentId = 0;

            // int studentId = 1046;
            clsReferral clsRef = new clsReferral();
            ClsCommon getCommon = new ClsCommon();
            ViewBag.permission = getCommon.setPermission();

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
                model = clsRef.LoadStudentDataPE(studentId, id);
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
                    tabSaved = IsTabSaved(studentId, "Tab8");

                    if (tabSaved == true)
                    {
                        sess.SessTab8 = studentId;
                    }
                    else
                    {
                        sess.SessTab8 = 0;
                    }
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
                //if (isAlert == true)
                //{
                //    ViewBag.html += " <div id='popupDiv'>Data Saved Successfully</div>";
                //}
            }
            return View();
        }


        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public byte[] SaveCustomXmlOld(FormCollection formdata, int stId, int Index)
        {
            // stId = 0;     
            int countCheck = 0;
            byte[] retXmlByte = null;
            string formVal = "";
            string html = "";
            clsReferral objClsRef = new clsReferral();
            XmlDocument xdocCustom = new XmlDocument();
            xdocCustom = objClsRef.LoadXmlfromBlob(stId);
            //  xdocCustom.Load(Server.MapPath("~/XML/RefInfoCustom.xml"));
            XmlNodeList xSectionsCustom = xdocCustom.SelectNodes("/RefferalTemplate/Sections/Section[@name]");
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
                    //html += "<td class='tdtext' colspan=" + colspan + " style='width: "+widthCell+"%;'>" + label + "</td>";
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

                        cellCust.InnerText = formVal.Trim();
                    }
                    else if (typeD == "table")
                    {
                        foreach (XmlNode tblRows in cellCust.ChildNodes)
                        {
                            foreach (XmlNode tblRow in tblRows.ChildNodes)
                            {
                                foreach (XmlNode tblCell in tblRow.ChildNodes)
                                {
                                    string tblCellId = tblCell.Attributes["id"].Value;
                                    string tblCellType = tblCell.Attributes["type"].Value;
                                    string tblCellLabel = tblCell.Attributes["label"].Value;
                                    string tblCellColspan = tblCell.Attributes["colspan"].Value;

                                    if (tblCellType == "input[text]")
                                    {
                                        formVal = formdata[tblCellId].ToString();
                                        tblCell.InnerText = formVal;
                                    }
                                    else if (tblCellType == "input[multitext]")
                                    {
                                        formVal = formdata[tblCellId].ToString().Trim();
                                        tblCell.InnerText = formVal;
                                    }

                                    else if (tblCellType == "input[textdate]")
                                    {
                                        formVal = formdata[tblCellId].ToString();
                                        tblCell.InnerText = formVal;
                                    }

                                    else if (tblCellType == "input[drop]")
                                    {
                                        countCheck = Convert.ToInt32(tblCell.Attributes["Count"].Value);
                                        string[] lblsplit = tblCellLabel.Split('|');
                                        tblCell.InnerText = "";
                                        for (int i = 0; i < countCheck; i++)
                                        {

                                            formVal = formdata[tblCellId].ToString();
                                            tblCell.InnerText = formVal;

                                        }


                                    }


                                    else if (tblCellType == "input[checkVt]")
                                    {
                                        countCheck = Convert.ToInt32(tblCell.Attributes["Count"].Value);
                                        string[] idSpl = tblCellId.Split('|');
                                        string[] lblsplit = tblCellLabel.Split('|');
                                        tblCell.InnerText = "";
                                        for (int i = 0; i < countCheck; i++)
                                        {
                                            try
                                            {
                                                formVal = formdata[idSpl[i]].ToString();
                                                if (formVal == "checked")
                                                {
                                                    tblCell.InnerText += "checked|";
                                                }
                                                else
                                                {
                                                    tblCell.InnerText += "unchecked|";
                                                }

                                            }
                                            catch
                                            {
                                                tblCell.InnerText += "unchecked|";
                                            }

                                        }
                                        tblCell.InnerText = tblCell.InnerText.Substring(0, tblCell.InnerText.Length - 1);


                                    }

                                }
                            }
                        }
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

                        //html += "<td colspan=" + colspan + " style='width:"+widthCell+"%;'>";
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


        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult SaveGeneralDataOld(GenInfoModel model, string type, FormCollection formdata, HttpPostedFileBase fileUpldDocName, HttpPostedFileBase fileUploadPhotoName)
        {
            ActionResult returnResult = null;
            try
            {
                int id = 0;
                int sveId = 0;
                string status = "";
                byte[] xmlData = null;
                clsReferral clsRf = new clsReferral();

                ClsCommon clsCommon = new ClsCommon();
                sess = (clsSession)Session["UserSession"];

                ApplicantUploadDownload clsUpld = new ApplicantUploadDownload();
                //    clsSessionTab sesClsObj = (clsSessionTab)Session["sesData"];
                string xmlPath = Server.MapPath("../XML/RefInfoCustomPE.xml");
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
                                TempData["EditId"] = id = clsRf.SaveGeneralDataPEOld(model, type, sveId, status, sess.LoginId, sess.SchoolId, formdata);
                            }
                            else
                                TempData["EditId"] = id = clsRf.SaveGeneralDataPEOld(model, type, tempId, status, sess.LoginId, sess.SchoolId, formdata);

                        }

                        //if (sess.SessTab1 > 0)
                        //{
                        //    status = "update";
                        //    sveId = Convert.ToInt32(sess.SessTab1);
                        //    TempData["EditId"] = id = clsRf.SaveGeneralDataPE(model, type, sveId, status, sess.LoginId, sess.SchoolId, formdata);

                        //}
                        else
                        {
                            status = "save";
                            TempData["EditId"] = id = clsRf.SaveGeneralDataPEOld(model, type, sveId, status, sess.LoginId, sess.SchoolId, formdata);
                            clsRf.SaveCurrentTab(id, "Tab1");

                            xmlData = clsRf.SaveAsBlob(xmlPath);   //Initial full xml doccument saving to database
                            clsRf.SaveXmlToDB(xmlData, id);

                            sess.ReferralId = id;                    // Insert the new referal Id in Q Status
                            clsCommon.insertQstatus("NA", "N");
                            clsCommon.insertQstatus("AR", "Y");
                            LetterTray clsletter = new LetterTray();
                            MelmarkDBEntities objData = new MelmarkDBEntities();
                            int Qid = clsCommon.getQueueId("NA");
                            var LetterName = objData.LetterEngines.Where(x => x.QueueId == Qid && x.ApproveStatus == true && x.SchoolId == sess.SchoolId && x.LetterType == "Letter").ToList();
                            if (LetterName.Count > 0)
                            {
                                clsletter.insertLetter("NA", sess.ReferralId, LetterName[0].LetterEngineId, "Parent");
                            }



                            sess.SessTab1 = id;
                            Session["RefId"] = id;
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

                            if (sess.SessTab2 > 0)
                            {
                                sveId = Convert.ToInt32(sess.SessTab2);
                                if (Session["RefId"] != null)
                                {
                                    TempData["EditId"] = id = sveId;
                                    xmlData = SaveCustomXmlOld(formdata, id, model.TabId);
                                    clsRf.SaveXmlToDB(xmlData, id);
                                }
                            }
                            else
                            {
                                if (Session["RefId"] != null)
                                {
                                    sveId = Convert.ToInt32(Session["RefId"]);
                                    TempData["EditId"] = id = sveId;
                                    xmlData = SaveCustomXmlOld(formdata, id, model.TabId);
                                    clsRf.SaveXmlToDB(xmlData, id);
                                    clsRf.SaveCurrentTab(id, "Tab2");
                                    sess.SessTab2 = id;
                                }


                            }
                            returnResult = RedirectToAction("SectionRedirect", new { tabID = model.TabId });
                        }
                        else returnResult = View();


                        //if (Session["RefId"] != null)
                        //{
                        //    if (sesClsObj.SessTab2 > 0)
                        //    {
                        //        status = "update";
                        //        sveId = Convert.ToInt32(sesClsObj.SessTab2);
                        //        TempData["EditId"] = id = clsRf.SaveGeneralDataPE(model, type, sveId, status, sess.LoginId, sess.SchoolId);
                        //    }
                        //    else
                        //    {
                        //        if (Session["RefId"] != null)
                        //        {
                        //            sveId = Convert.ToInt32(Session["RefId"]);
                        //        }
                        //        status = "update";
                        //        TempData["EditId"] = id = clsRf.SaveGeneralDataPE(model, type, sveId, status, sess.LoginId, sess.SchoolId);
                        //        sesClsObj.SessTab2 = id;
                        //    }

                        //    xmlData = SaveCustomXml(formdata, id, model.TabId);
                        //    clsRf.SaveXmlToDB(xmlData, id);

                        //    returnResult = RedirectToAction("SectionRedirect", new { tabID = model.TabId });
                        //}
                        //else
                        //    returnResult = View();


                    }
                    else if (model.TabId == 3)
                    {

                        if (Session["RefId"] != null)
                        {

                            if (sess.SessTab3 > 0)
                            {
                                sveId = Convert.ToInt32(sess.SessTab3);
                                if (Session["RefId"] != null)
                                {
                                    TempData["EditId"] = id = sveId;
                                    xmlData = SaveCustomXmlOld(formdata, id, model.TabId);
                                    clsRf.SaveXmlToDB(xmlData, id);
                                }
                            }
                            else
                            {
                                if (Session["RefId"] != null)
                                {
                                    sveId = Convert.ToInt32(Session["RefId"]);
                                    TempData["EditId"] = id = sveId;
                                    xmlData = SaveCustomXmlOld(formdata, id, model.TabId);
                                    clsRf.SaveXmlToDB(xmlData, id);
                                    sess.SessTab3 = id;
                                    clsRf.SaveCurrentTab(id, "Tab3");
                                }


                            }
                            returnResult = RedirectToAction("SectionRedirect", new { tabID = model.TabId });
                        }
                        else returnResult = View();





                        //if (Session["RefId"] != null)
                        //{

                        //    if (sess.SessTab3 > 0)
                        //    {
                        //        sveId = Convert.ToInt32(sess.SessTab3);
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
                        //            sess.SessTab3 = id;
                        //        }


                        //    }
                        //    returnResult = RedirectToAction("SectionRedirect", new { tabID = model.TabId });
                        //}
                        //else returnResult = View();

                        //if (Session["RefId"] != null)
                        //{
                        //    if (sesClsObj.SessTab3 > 0)
                        //    {
                        //        status = "update";
                        //        sveId = Convert.ToInt32(sesClsObj.SessTab3);
                        //        TempData["EditId"] = id = clsRf.SaveGeneralDataPE(model, type, sveId, status, sess.LoginId, sess.SchoolId);
                        //    }
                        //    else
                        //    {
                        //        if (Session["RefId"] != null)
                        //        {
                        //            sveId = Convert.ToInt32(Session["RefId"]);
                        //        }
                        //        status = "save";
                        //        TempData["EditId"] = id = clsRf.SaveGeneralDataPE(model, type, sveId, status, sess.LoginId, sess.SchoolId);
                        //        sesClsObj.SessTab3 = id;
                        //    }

                        //    xmlData = SaveCustomXml(formdata, id, model.TabId);
                        //    clsRf.SaveXmlToDB(xmlData, id);

                        //    returnResult = RedirectToAction("SectionRedirect", new { tabID = model.TabId });
                        //}
                        //else returnResult = View();
                    }
                    else if (model.TabId == 4)
                    {
                        ClsErrorLog errorLog = new ClsErrorLog();
                        bool itabSaved = IsTabSaved(sess.ReferralId, "Tab4");
                        errorLog.WriteToLog("itabSaved: " + itabSaved);
                        if (itabSaved)
                        {
                            sess.SessTab4 = sess.ReferralId;
                        }
                        if (Session["RefId"] != null)
                        {
                            errorLog.WriteToLog("sess.SessTab4 : " + sess.SessTab4);
                            if (sess.SessTab4 > 0)
                            {
                                status = "update";
                                errorLog.WriteToLog("update : " + status);
                                sveId = Convert.ToInt32(sess.SessTab4);
                                TempData["EditId"] = id = clsRf.SaveGeneralDataPEOld(model, type, sveId, status, sess.LoginId, sess.SchoolId, formdata);
                            }
                            else
                            {
                                if (Session["RefId"] != null)
                                {
                                    sveId = Convert.ToInt32(Session["RefId"]);
                                }
                                status = "save";
                                TempData["EditId"] = id = clsRf.SaveGeneralDataPEOld(model, type, sveId, status, sess.LoginId, sess.SchoolId, formdata);
                                sess.SessTab4 = id;
                                clsRf.SaveCurrentTab(id, "Tab4");
                            }

                            xmlData = SaveCustomXmlOld(formdata, id, model.TabId);
                            clsRf.SaveXmlToDB(xmlData, id);

                            returnResult = RedirectToAction("SectionRedirect", new { tabID = model.TabId });
                        }
                        else returnResult = View();
                    }
                    else if (model.TabId == 5)
                    {
                        sess.ReferralId = Convert.ToInt32(Session["RefId"]);
                        bool itabSaved = IsTabSaved(sess.ReferralId, "Tab5");
                        sess.SessTab5 = sess.ReferralId;
                        if (Session["RefId"] != null)
                        {
                            if (itabSaved == true)
                            {
                                status = "update";
                                sveId = Convert.ToInt32(sess.SessTab5);
                                TempData["EditId"] = id = clsRf.SaveGeneralDataPEOld(model, type, sveId, status, sess.LoginId, sess.SchoolId, formdata);
                            }
                            else
                            {

                                if (Session["RefId"] != null)
                                {
                                    sveId = Convert.ToInt32(Session["RefId"]);
                                }
                                status = "save";
                                TempData["EditId"] = id = clsRf.SaveGeneralDataPEOld(model, type, sveId, status, sess.LoginId, sess.SchoolId, formdata);
                                sess.SessTab5 = id;
                                clsRf.SaveCurrentTab(id, "Tab5");
                            }

                            xmlData = SaveCustomXmlOld(formdata, id, model.TabId);
                            clsRf.SaveXmlToDB(xmlData, id);

                            returnResult = RedirectToAction("SectionRedirect", new { tabID = model.TabId });
                        }
                        else returnResult = View();



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
                        //            sess.SessTab5 = id;
                        //        }


                        //    }
                        //    returnResult = RedirectToAction("SectionRedirect", new { tabID = model.TabId });
                        //}
                        //else returnResult = View();
                    }
                    else if (model.TabId == 6)
                    {
                        sess.ReferralId = Convert.ToInt32(Session["RefId"]);
                        bool itabSaved = IsTabSaved(sess.ReferralId, "Tab6");
                        sess.SessTab6 = sess.ReferralId;

                        if (Session["RefId"] != null)
                        {
                            if (itabSaved == true)
                            {
                                status = "update";
                                sveId = Convert.ToInt32(sess.SessTab6);
                                TempData["EditId"] = id = clsRf.SaveGeneralDataPEOld(model, type, sveId, status, sess.LoginId, sess.SchoolId, formdata);
                            }
                            else
                            {
                                if (Session["RefId"] != null)
                                {
                                    sveId = Convert.ToInt32(Session["RefId"]);
                                }
                                status = "save";
                                TempData["EditId"] = id = clsRf.SaveGeneralDataPEOld(model, type, sveId, status, sess.LoginId, sess.SchoolId, formdata);
                                sess.SessTab6 = id;
                                clsRf.SaveCurrentTab(id, "Tab6");
                            }

                            xmlData = SaveCustomXmlOld(formdata, id, model.TabId);
                            clsRf.SaveXmlToDB(xmlData, id);

                            returnResult = RedirectToAction("SectionRedirect", new { tabID = model.TabId });
                        }
                        else returnResult = View();

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
                        bool itabSaved = IsTabSaved(sess.ReferralId, "Tab7");
                        if (Session["RefId"] != null)
                        {

                            if (sess.SessTab7 > 0)
                            {
                                sveId = Convert.ToInt32(sess.SessTab7);
                                if (Session["RefId"] != null)
                                {
                                    TempData["EditId"] = id = sveId;
                                    //        fillUploadedDocuments(id,);
                                    if (fileUpldDocName != null)
                                    {
                                        string Result = clsCommon.GetFileType(fileUpldDocName.ContentType);
                                        if (Result != "")
                                        {
                                            UploadDoccuments(id, model, fileUpldDocName, sess.SchoolId, sess.LoginId);
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

                                    xmlData = SaveCustomXmlOld(formdata, id, model.TabId);
                                    clsRf.SaveXmlToDB(xmlData, id);
                                }
                            }
                            else
                            {
                                if (Session["RefId"] != null)
                                {
                                    sveId = Convert.ToInt32(Session["RefId"]);
                                    TempData["EditId"] = id = sveId;
                                    if (fileUpldDocName != null)
                                    {
                                        string Result = clsCommon.GetFileType(fileUpldDocName.ContentType);
                                        if (Result != "")
                                        {
                                            UploadDoccuments(id, model, fileUpldDocName, sess.SchoolId, sess.LoginId);
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

                                    xmlData = SaveCustomXmlOld(formdata, id, model.TabId);
                                    clsRf.SaveXmlToDB(xmlData, id);
                                    sess.SessTab7 = id;
                                    clsRf.SaveCurrentTab(id, "Tab7");
                                }

                                // TempData["EditId"] = id = clsRf.SaveGeneralData(model, type, sveId, status);
                                //  sesClsObj.SessTab5 = id;
                            }

                            returnResult = RedirectToAction("SectionRedirect", new { tabID = model.TabId });
                        }
                        else returnResult = View();

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
                                    if (fileUpldDocName != null)
                                    {
                                        string Result = clsCommon.GetFileType(fileUpldDocName.ContentType);
                                        if (Result != "")
                                        {
                                            UploadDoccuments(id, model, fileUpldDocName, sess.SchoolId, sess.LoginId);
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

                                    //   xmlData = SaveCustomXml(formdata, id, model.TabId);
                                    //   clsRf.SaveXmlToDB(xmlData, id);
                                    sess.SessTab8 = id;
                                    clsRf.SaveCurrentTab(id, "Tab2");
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
            }
            catch (Exception Ex)
            {
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
                //   sesClsObj = (clsSessionTab)Session["sesData"];
            }
            else if (TempData["LoadStudentData"] != null)
            {
                genModel = (GenInfoModel)TempData["LoadStudentData"];
                //   sesClsObj = (clsSessionTab)Session["sesData"];
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
            xdocCustom.Load(Server.MapPath("~/XML/RefInfoCustomPE.xml"));
            XmlNodeList xSectionsCustom = xdocCustom.SelectNodes("/RefferalTemplate/Sections/Section[@name]");
            XmlNode xSectnCustom = xSectionsCustom[Index - 1];
            XmlNodeList rowsCust = xSectnCustom.ChildNodes[0].ChildNodes;



            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(Server.MapPath("~/XML/RefInfoDBPE.xml"));
            XmlNodeList xSections = xdoc.SelectNodes("/RefferalTemplate/Sections/Section[@name]");

            int widthCell = 25;
            int tdwidth = 8;
            string html = "";
            int countCheck = 0;
            // int formid = 0;
            XmlNode xSectn = xSections[Index - 1];
            //foreach (XmlNode xSectn in xSections)
            //{
            // formid++;
            string TableIds = "tblData" + Index;
            html += " <form method='post' id='frm" + Index + "' enctype='multipart/form-data' action='../RefferalApplicantPE/SaveGeneralDataOld?type=" + xSectn.Attributes["name"].Value + "'><div><span class='headPanel'  onclick='getColapse(\"" + TableIds + "\")'>" + xSectn.Attributes["name"].Value + "</span><div class='divSubs'><table id='" + TableIds + "' class='clsTabDiv' style='float:left;width:100%'>";
            XmlNodeList rows = xSectn.ChildNodes[0].ChildNodes;
            foreach (XmlNode row in rows)
            {
                html += "<tr style='width:100%'>";
                foreach (XmlNode cell in row.ChildNodes)
                {
                    string id = cell.Attributes["id"].Value;
                    string type = cell.Attributes["type"].Value;
                    string label = cell.Attributes["label"].Value;
                    string colspan = cell.Attributes["colspan"].Value;
                    if (type == "label")
                        html += "<td class='tdtext' colspan=" + colspan + ">" + label + "</td>";
                    else if (type == "passage")
                        html += "<td class='tdPassage' colspan=" + colspan + ">" + label + "</td>";
                    else if (type == "header")
                        html += "<td class='subhead' colspan=" + colspan + ">" + label + "</td>";


                    else if (type == "StudentPhoto")
                    {
                        string imagePath = "";

                        if (sess.ReferralId != 0)
                        {
                            imagePath = objClsRef.GetStudentImage(sess.ReferralId);
                            if (imagePath != null && imagePath != "")
                            {
                                html += "<td colspan=" + colspan + "><img id='imgStudPhoto' height='150px' width='130px' src=data:image/gif;base64," + imagePath + "></td>";
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
                            value = genModel.GetType().GetProperty(textValue.ToString()).GetValue(genModel, null);
                        }
                        catch
                        {
                            string[] split = textValue.Split('.');
                            if (split[0] == "objRelationMthr")

                                value = genModel.objRelationMthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationMthr, null);
                            if (split[0] == "objRelationFthr")

                                value = genModel.objRelationFthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationFthr, null);
                            if (split[0] == "objRelationClose")

                                value = genModel.objRelationClose.GetType().GetProperty(split[1]).GetValue(genModel.objRelationClose, null);
                            if (split[0] == "objPhysicianDetails")

                                if (genModel.objPhysicianDetails != null) value = genModel.objPhysicianDetails.GetType().GetProperty(split[1]).GetValue(genModel.objPhysicianDetails, null);
                            if (split[0] == "objInsuranceDetails")

                                value = genModel.objInsuranceDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDetails, null);
                            if (split[0] == "objInsuranceSecDetails")

                                value = genModel.objInsuranceSecDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceSecDetails, null);
                            if (split[0] == "objInsuranceDentalDetails")

                                value = genModel.objInsuranceDentalDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDentalDetails, null);
                            if (split[0] == "objRelationLegalGuardian")

                                if (genModel.objRelationLegalGuardian != null) value = genModel.objRelationLegalGuardian.GetType().GetProperty(split[1]).GetValue(genModel.objRelationLegalGuardian, null);
                            if (split[0] == "objRelationEmergncyContact")

                                value = genModel.objRelationEmergncyContact.GetType().GetProperty(split[1]).GetValue(genModel.objRelationEmergncyContact, null);
                            if (split[0] == "objclsUpld")

                                value = genModel.objclsUpld.GetType().GetProperty(split[1]).GetValue(genModel.objclsUpld, null);
                        }


                        // html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + textValue.ToString() + "' value='" + value + "' maxlength='" + maxLength + "' type='text' /></td>";
                        html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + textValue.ToString() + "' style='" + widthXml + "' value='" + Server.HtmlEncode(Convert.ToString(value)) + "' maxlength='" + maxLength + "' type='text' /></td>";

                    }


                    else if (type == "input[textvalidate]")
                    {
                        string textValue = cell.Attributes["modelName"].Value;
                        string className = cell.Attributes["className"].Value;
                        string maxLength = cell.Attributes["MaxLength"].Value;
                        object value = null;
                        try
                        {
                            value = genModel.GetType().GetProperty(textValue.ToString()).GetValue(genModel, null);
                        }
                        catch
                        {
                            string[] split = textValue.Split('.');
                            if (split[0] == "objRelationMthr")
                                value = genModel.objRelationMthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationMthr, null);
                            if (split[0] == "objRelationFthr")
                                value = genModel.objRelationFthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationFthr, null);
                            if (split[0] == "objRelationClose")
                                value = genModel.objRelationClose.GetType().GetProperty(split[1]).GetValue(genModel.objRelationClose, null);
                            if (split[0] == "objPhysicianDetails")
                                if (genModel.objPhysicianDetails != null) value = genModel.objPhysicianDetails.GetType().GetProperty(split[1]).GetValue(genModel.objPhysicianDetails, null);
                            if (split[0] == "objInsuranceDetails")
                                value = genModel.objInsuranceDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDetails, null);
                            if (split[0] == "objInsuranceSecDetails")
                                value = genModel.objInsuranceSecDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceSecDetails, null);
                            if (split[0] == "objInsuranceDentalDetails")
                                value = genModel.objInsuranceDentalDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDentalDetails, null);
                            if (split[0] == "objRelationLegalGuardian")
                                if (genModel.objRelationLegalGuardian != null) value = genModel.objRelationLegalGuardian.GetType().GetProperty(split[1]).GetValue(genModel.objRelationLegalGuardian, null);
                            if (split[0] == "objRelationEmergncyContact")
                                value = genModel.objRelationEmergncyContact.GetType().GetProperty(split[1]).GetValue(genModel.objRelationEmergncyContact, null);
                            if (split[0] == "objclsUpld")
                                value = genModel.objclsUpld.GetType().GetProperty(split[1]).GetValue(genModel.objclsUpld, null);
                        }

                        if (className == "validate[required]")
                            html += "<td colspan=" + colspan + "><span class='validate'>*</span><input id='" + id + "' name='" + textValue.ToString() + "' value='" + Server.HtmlEncode(Convert.ToString(value)) + "' class='" + className + "' maxlength='" + maxLength + "' type='text' /></td>";
                        else if (className == "validate[required] namefield")
                            html += "<td colspan=" + colspan + "><span class='validate'>*</span><input id='" + id + "' name='" + textValue.ToString() + "' value='" + Server.HtmlEncode(Convert.ToString(value)) + "' class='" + className + "' maxlength='" + maxLength + "' type='text' onpaste='PreventDef(event)' /></td>";
                        else if (className == "namefield")
                            html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + textValue.ToString() + "' value='" + Server.HtmlEncode(Convert.ToString(value)) + "' class='" + className + "' maxlength='" + maxLength + "' type='text' onpaste='PreventDef(event)' /></td>";
                        else if (className == "grossIncome")
                            html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + textValue.ToString() + "' value='" + Server.HtmlEncode(Convert.ToString(value)) + "' class='" + className + "' maxlength='" + maxLength + "' type='text' data-thousands='.' data-decimal=',' data-prefix='R$' onpaste='PreventDef(event)' /></td>";
                        else
                            html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + textValue.ToString() + "' value='" + Server.HtmlEncode(Convert.ToString(value)) + "' class='" + className + "' maxlength='" + maxLength + "' type='text' /></td>";
                    }

                    else if (type == "input[textfunction]")
                    {
                        string maxLength = cell.Attributes["MaxLength"].Value;
                        string textValue = cell.Attributes["modelName"].Value;
                        string funName = cell.Attributes["onkeypress"].Value;
                        object value = null;
                        try
                        {
                            value = genModel.GetType().GetProperty(textValue.ToString()).GetValue(genModel, null);
                        }
                        catch
                        {
                            string[] split = textValue.Split('.');
                            if (split[0] == "objRelationMthr")
                                value = genModel.objRelationMthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationMthr, null);
                            if (split[0] == "objRelationFthr")
                                value = genModel.objRelationFthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationFthr, null);
                            if (split[0] == "objRelationClose")
                                value = genModel.objRelationClose.GetType().GetProperty(split[1]).GetValue(genModel.objRelationClose, null);
                            if (split[0] == "objPhysicianDetails")
                                if (genModel.objPhysicianDetails != null) value = genModel.objPhysicianDetails.GetType().GetProperty(split[1]).GetValue(genModel.objPhysicianDetails, null);
                            if (split[0] == "objInsuranceDetails")
                                value = genModel.objInsuranceDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDetails, null);
                            if (split[0] == "objInsuranceSecDetails")
                                value = genModel.objInsuranceSecDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceSecDetails, null);
                            if (split[0] == "objInsuranceDentalDetails")
                                value = genModel.objInsuranceDentalDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDentalDetails, null);
                            if (split[0] == "objRelationLegalGuardian")
                                if (genModel.objRelationLegalGuardian != null) value = genModel.objRelationLegalGuardian.GetType().GetProperty(split[1]).GetValue(genModel.objRelationLegalGuardian, null);
                            if (split[0] == "objRelationEmergncyContact")
                                value = genModel.objRelationEmergncyContact.GetType().GetProperty(split[1]).GetValue(genModel.objRelationEmergncyContact, null);
                            if (split[0] == "objclsUpld")
                                value = genModel.objclsUpld.GetType().GetProperty(split[1]).GetValue(genModel.objclsUpld, null);
                        }


                        if (textValue == "RefZipCode")
                        {
                            html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + textValue.ToString() + "' value='" + Server.HtmlEncode(Convert.ToString(value)) + "' maxlength='" + maxLength + "'  onkeypress='" + funName + "' type='text' /></td>";
                        }
                        else html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + textValue.ToString() + "' value='" + Server.HtmlEncode(Convert.ToString(value)) + "' maxlength='" + maxLength + "' onkeypress='" + funName + "' type='text' /></td>";
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
                            value = genModel.GetType().GetProperty(textValue.ToString()).GetValue(genModel, null);
                        }
                        catch
                        {
                            string[] split = textValue.Split('.');
                            if (split[0] == "objRelationMthr")
                                value = genModel.objRelationMthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationMthr, null);
                            if (split[0] == "objRelationFthr")
                                value = genModel.objRelationFthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationFthr, null);
                            if (split[0] == "objRelationClose")
                                value = genModel.objRelationClose.GetType().GetProperty(split[1]).GetValue(genModel.objRelationClose, null);
                            if (split[0] == "objPhysicianDetails")
                                if (genModel.objPhysicianDetails != null) value = genModel.objPhysicianDetails.GetType().GetProperty(split[1]).GetValue(genModel.objPhysicianDetails, null);
                            if (split[0] == "objInsuranceDetails")
                                value = genModel.objInsuranceDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDetails, null);
                            if (split[0] == "objInsuranceSecDetails")
                                value = genModel.objInsuranceSecDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceSecDetails, null);
                            if (split[0] == "objInsuranceDentalDetails")
                                value = genModel.objInsuranceDentalDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDentalDetails, null);
                            if (split[0] == "objRelationLegalGuardian")
                                if (genModel.objRelationLegalGuardian != null) value = genModel.objRelationLegalGuardian.GetType().GetProperty(split[1]).GetValue(genModel.objRelationLegalGuardian, null);
                            if (split[0] == "objRelationEmergncyContact")
                                value = genModel.objRelationEmergncyContact.GetType().GetProperty(split[1]).GetValue(genModel.objRelationEmergncyContact, null);
                            if (split[0] == "objclsUpld")
                                value = genModel.objclsUpld.GetType().GetProperty(split[1]).GetValue(genModel.objclsUpld, null);
                        }

                        html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + textValue.ToString() + "' value='" + Server.HtmlEncode(Convert.ToString(value)) + "' style='width: " + width + ";' maxlength='" + maxLength + "' onkeypress='" + funName + "' type='text' /><span style='color:#595959;'>" + keyName + "</span></td>";


                    }

                    else if (type == "input[multitext]")
                    {
                        string textValue = cell.Attributes["modelName"].Value;
                        string width = cell.Attributes["Width"].Value;
                        object value = null;
                        try
                        {
                            value = genModel.GetType().GetProperty(textValue.ToString()).GetValue(genModel, null);
                        }
                        catch
                        {
                            string[] split = textValue.Split('.');
                            if (split[0] == "objRelationMthr")
                                value = genModel.objRelationMthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationMthr, null);
                            if (split[0] == "objRelationFthr")
                                value = genModel.objRelationFthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationFthr, null);
                            if (split[0] == "objRelationClose")
                                value = genModel.objRelationClose.GetType().GetProperty(split[1]).GetValue(genModel.objRelationClose, null);
                            if (split[0] == "objPhysicianDetails")
                                if (genModel.objPhysicianDetails != null) value = genModel.objPhysicianDetails.GetType().GetProperty(split[1]).GetValue(genModel.objPhysicianDetails, null);
                            if (split[0] == "objInsuranceDetails")
                                value = genModel.objInsuranceDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDetails, null);
                            if (split[0] == "objInsuranceSecDetails")
                                value = genModel.objInsuranceSecDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceSecDetails, null);
                            if (split[0] == "objInsuranceDentalDetails")
                                value = genModel.objInsuranceDentalDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDentalDetails, null);
                            if (split[0] == "objRelationLegalGuardian")
                                if (genModel.objRelationLegalGuardian != null) value = genModel.objRelationLegalGuardian.GetType().GetProperty(split[1]).GetValue(genModel.objRelationLegalGuardian, null);

                            if (split[0] == "objRelationEmergncyContact")
                                value = genModel.objRelationEmergncyContact.GetType().GetProperty(split[1]).GetValue(genModel.objRelationEmergncyContact, null);

                            if (split[0] == "objclsUpld")
                                value = genModel.objclsUpld.GetType().GetProperty(split[1]).GetValue(genModel.objclsUpld, null);
                        }

                        html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><textarea id='" + id + "' name='" + textValue.ToString() + "' value='" + Server.HtmlEncode(Convert.ToString(value)) + "' type='text' style='width:" + width + ";' rows='3'>" + Server.HtmlEncode(Convert.ToString(value)) + "</textarea></td>";
                    }
                    else if (type == "input[textDate]")
                    {
                        object value = null;
                        string textValue = cell.Attributes["modelName"].Value;
                        string datePicker = "datepicker";

                        if (textValue == "RefDate" || textValue == "RefDOB")
                        {
                            datePicker = "validate[required] datepicker";
                        }
                        try
                        {
                            value = genModel.GetType().GetProperty(textValue.ToString()).GetValue(genModel, null);
                        }
                        catch
                        {
                            string[] split = textValue.Split('.');
                            if (split[0] == "objRelationMthr")
                                value = genModel.objRelationMthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationMthr, null);
                            if (split[0] == "objRelationFthr")
                                value = genModel.objRelationFthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationFthr, null);
                            if (split[0] == "objRelationClose")
                                value = genModel.objRelationClose.GetType().GetProperty(split[1]).GetValue(genModel.objRelationClose, null);
                            if (split[0] == "objPhysicianDetails")
                                if (genModel.objPhysicianDetails != null) value = genModel.objPhysicianDetails.GetType().GetProperty(split[1]).GetValue(genModel.objPhysicianDetails, null);
                            if (split[0] == "objInsuranceDetails")
                                value = genModel.objInsuranceDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDetails, null);
                            if (split[0] == "objInsuranceSecDetails")
                                value = genModel.objInsuranceSecDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceSecDetails, null);
                            if (split[0] == "objInsuranceDentalDetails")
                                value = genModel.objInsuranceDentalDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDentalDetails, null);
                            if (split[0] == "objRelationLegalGuardian")
                                if (genModel.objRelationLegalGuardian != null) value = genModel.objRelationLegalGuardian.GetType().GetProperty(split[1]).GetValue(genModel.objRelationLegalGuardian, null);
                            if (split[0] == "objRelationEmergncyContact")
                                value = genModel.objRelationEmergncyContact.GetType().GetProperty(split[1]).GetValue(genModel.objRelationEmergncyContact, null);

                            if (split[0] == "objclsUpld")
                                value = genModel.objclsUpld.GetType().GetProperty(split[1]).GetValue(genModel.objclsUpld, null);

                        }

                        if (textValue == "RefDate" || textValue == "RefDOB")
                        {
                            html += "<td colspan=" + colspan + "><span class='validate'>*</span><input id='" + id + "' name='" + textValue.ToString() + "' class='validate[required] datepicker' onpaste='PreventDef(event)' onkeypress='return false' value='" + Server.HtmlEncode(Convert.ToString(value)) + "' type='text' /></td>";
                        }
                        else
                            html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + textValue.ToString() + "' class='datepicker' onpaste='PreventDef(event)' onkeypress='return false' value='" + Server.HtmlEncode(Convert.ToString(value)) + "' type='text' /></td>";
                    }

                    //else if (type == "input[button]")
                    //{
                    //    if (isUpated == 1)
                    //    {
                    //        html += "<td colspan=" + colspan + " style='width: "+widthCell+"%; text-align:right;'><input id='" + id + "' value='Update' type='submit' class='NFButton' /></td>";
                    //    }
                    //    else
                    //    {
                    //        html += "<td colspan=" + colspan + " style='width: "+widthCell+"%; text-align:right;'><input id='" + id + "' value='" + label + "' type='submit' class='NFButton' /></td>";
                    //    }

                    //    //html += "<td colspan=" + colspan + " style='width: "+widthCell+"%; text-align:right;'><input id='" + id + "' value='" + (isUpated == 1 ? "update" : "save") + "' type='submit' class='NFButton' /></td>";
                    //}
                    else if (type == "input[file]")
                    {
                        string nameFile = cell.Attributes["name"].Value.ToString();
                        html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + nameFile + "' type='file' /></td>";
                    }

                    else if (type == "input[check]")
                    {
                        countCheck = Convert.ToInt32(cell.Attributes["Count"].Value);
                        string[] idSplit = id.Split('|');
                        string[] lblSplit = label.Split('|');
                        html += "<td colspan=" + colspan + ">";
                        for (int i = 0; i < countCheck; i++)
                        {
                            html += "<input id='" + idSplit[i] + "' type='checkbox'>" + lblSplit[i] + "</input><br>";
                        }
                        html += "</td>";

                    }
                    else if (type == "lblBold")
                    {
                        html += "<td class='lblBold' colspan=" + colspan + ">" + label + "</td>";
                    }
                    else if (type == "input[checkVt]")
                    {
                        countCheck = Convert.ToInt32(cell.Attributes["Count"].Value);
                        string[] idSpl = id.Split('|');
                        string[] lblsplit = label.Split('|');
                        html += "<td colspan=" + colspan + ">";
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
                            if (textValue == "objRelationMthr.txtMaritalStatus")
                            {
                                html += "<td colspan=" + colspan + ">";
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
                                html += "<td colspan=" + colspan + ">";
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
                                html += "<td colspan=" + colspan + ">";

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
                            html += "<td colspan=" + colspan + "><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass1'>";
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

                    else if (type == "BehaviorCategory")
                    {
                        bool isValid = false;

                        isValid = IsRefSavedBehavior(sess.ReferralId);

                        if (isValid == false)
                        {
                            IEnumerable<SelectListItem> ListScore = new List<SelectListItem>();
                            IEnumerable<clsBehaviorCategry> ListBehaviorList = new List<clsBehaviorCategry>();
                            html += "<td colspan=" + colspan + " style='width:100%;'><table style='width:100%; border:0px;'>";
                            foreach (XmlNode subcell in cell.ChildNodes)
                            {
                                string parentName = subcell.Attributes["label"].Value;
                                ListBehaviorList = objClsRef.FillBehaviorCategory(parentName, genModel);
                                ListScore = objClsRef.FillBehaviorScore();

                                html += "<tr><td colspan='2'><span class='subhead'>" + parentName + "</span><td></tr>";

                                foreach (var item in ListBehaviorList)
                                {
                                    html += "<tr>";
                                    html += "<td>" + item.behaviorName + "</td>";

                                    html += "<td ><select id='" + item.behaviorId + "' name='behaveNameid_" + item.behaviorId.ToString() + "' class='drpClass1'>";
                                    html += "<option value=0>---Select Score----</option>";
                                    foreach (SelectListItem dropItem in ListScore)
                                    {
                                        html += "<option value='" + dropItem.Value + "'>" + dropItem.Text + "</option>";
                                    }

                                    html += "</td></tr>";
                                }

                            }
                            html += "</table></td>";
                        }
                        else
                        {
                            IEnumerable<SelectListItem> ListScore = new List<SelectListItem>();
                            IEnumerable<clsBehaviorCategry> ListBehaviorList = new List<clsBehaviorCategry>();
                            html += "<td colspan=" + colspan + " style='width:100%;'><table style='width:100%; border:0px;'>";
                            foreach (XmlNode subcell in cell.ChildNodes)
                            {
                                string parentName = subcell.Attributes["label"].Value;
                                ListBehaviorList = objClsRef.FillBehaviorCategoryOnStudentId(sess.ReferralId, genModel, parentName);
                                ListScore = objClsRef.FillBehaviorScore();

                                html += "<tr><td colspan='2'><span class='subhead'>" + parentName + "</span><td></tr>";

                                foreach (var item in ListBehaviorList)
                                {
                                    html += "<tr>";
                                    html += "<td>" + item.behaviorName + "</td>";

                                    html += "<td ><select id='" + item.behaviorId + "' name='behaveNameid_" + item.behaviorId.ToString() + "' class='drpClass1'>";
                                    html += "<option value=0>---Select Score----</option>";
                                    foreach (SelectListItem dropItem in ListScore)
                                    {
                                        if (dropItem.Value == item.scoreId.ToString())
                                        {
                                            html += "<option value='" + dropItem.Value + "' selected ='true'>" + dropItem.Text + "</option>";
                                        }
                                        else
                                            html += "<option value='" + dropItem.Value + "'>" + dropItem.Text + "</option>";
                                    }

                                    html += "</td></tr>";
                                }

                            }
                            html += "</table></td>";


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
                                html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='ddlRefCountry'>";
                                //html += "<option value=''>Select " + label + "</option>";
                            }
                            else
                            {
                                html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "'class='ddlOtherCountry'>";
                                //html += "<option value=0>Select " + label + "</option>";
                            }
                            foreach (SelectListItem list in listData)
                            {
                                if (textValue == "objRelationMthr.txtCountry")
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
                                else if (textValue == "objRelationFthr.txtCountry")
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
                                else if (textValue == "objRelationClose.txtCountry")
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
                                else if (textValue == "objPhysicianDetails.txtCountry")
                                {
                                    try
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
                                    catch
                                    {
                                    }
                                }
                                else if (textValue == "objInsuranceDetails.txtCountry")
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
                                else if (textValue == "objInsuranceSecDetails.txtCountry")
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
                                else if (textValue == "objInsuranceDentalDetails.txtCountry")
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

                                else if (textValue == "objRelationLegalGuardian.txtCountry")
                                {
                                    try
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
                                    catch
                                    {
                                    }
                                }
                                else if (textValue == "objRelationEmergncyContact.txtCountry")
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
                                else if (textValue == "RefCntryBirth")
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
                                html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
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
                                html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
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
                                html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
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
                                int countryId = Convert.ToInt32(genModel.objRelationClose.txtCountry);
                                listDataState = objClsRef.FillState(countryId);
                                html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
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
                                html += "</select></td>";
                            }

                            else if (textValue == "objRelationLegalGuardian.txtState")
                            {
                                int countryId = 0;
                                try { countryId = Convert.ToInt32(genModel.objRelationLegalGuardian.txtCountry); }
                                catch { }
                                listDataState = objClsRef.FillState(countryId);
                                html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
                                html += "<option value=0>Select " + label + "</option>";
                                foreach (SelectListItem listSt in listDataState)
                                {
                                    if (textValue == "objRelationLegalGuardian.txtState")
                                    {
                                        try
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
                                        catch
                                        {
                                        }
                                    }
                                }
                                html += "</select></td>";
                            }

                            else if (textValue == "objRelationEmergncyContact.txtState")
                            {
                                int countryId = Convert.ToInt32(genModel.objRelationEmergncyContact.txtCountry);
                                listDataState = objClsRef.FillState(countryId);
                                html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
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
                                if (genModel.objPhysicianDetails == null)
                                {
                                    countryId = 0;
                                }
                                else { countryId = Convert.ToInt32(genModel.objPhysicianDetails.txtCountry); }
                                listDataState = objClsRef.FillState(countryId);
                                html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
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
                                html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
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
                                html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
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
                                html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
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
                                html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
                                html += "<option value=0>Select " + label + "</option>";
                                html += "</select></td>";
                            }

                        }
                        else if (label == "Race")
                        {
                            listData = objClsRef.FillRaceType();
                            string textValue = cell.Attributes["modelName"].Value;
                            html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass1'>";
                            html += "<option value=0>Select " + label + "</option>";
                            foreach (SelectListItem Item in listData)
                            {
                                if (textValue == "RefRace")
                                {
                                    if (genModel.RefRace.ToString() == Item.Value)
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
                        else if (label == "Gender")
                        {
                            string textValue = cell.Attributes["modelName"].Value;

                            html += "<td colspan=" + colspan + "><span class='validate'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='validate[required]'>";
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

                            html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
                            html += "<option value=0>Select " + label + "</option>";
                            html += "</select></td>";
                        }
                    }

                    else if (type == "td")
                    {
                        html += "<td colspan=" + colspan + "></td>";
                    }


                }
                html += "</tr>";
            }

            if (TempData["EditId"] == null)
            {

                foreach (XmlNode rowcust in rowsCust)
                {
                    html += "<tr style='width:100%'>";
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
                            html += "<td class='tdtext' colspan=" + colspan + ">" + label + "</td>";
                        else if (type == "header")
                        {
                            html += "<td class='subhead' colspan=" + colspan + ">" + label + "</td>";
                        }
                        else if (type == "passage")
                            html += "<td class='tdPassage' colspan=" + colspan + ">" + label + "</td>";
                        else if (type == "input[text]")
                        {


                            html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + id + "'  style='" + widthXml + "' type='text' /></td>";


                            //html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + id + "' type='text' /></td>";
                        }

                        else if (type == "table")
                        {
                            html += "<td class='tdtext' colspan=" + colspan + ">";
                            html += "<table class='tblPattern' style='width:100%; border: 0px none;'>";

                            foreach (XmlNode tblRows in cellCust.ChildNodes)
                            {
                                foreach (XmlNode tblRow in tblRows.ChildNodes)
                                {

                                    html += "<tr>";
                                    foreach (XmlNode tblCell in tblRow.ChildNodes)
                                    {
                                        string tblCellId = tblCell.Attributes["id"].Value;
                                        string tblCellType = tblCell.Attributes["type"].Value;
                                        string tblCellLabel = tblCell.Attributes["label"].Value;
                                        string tblCellColspan = tblCell.Attributes["colspan"].Value;

                                        if (tblCellType == "label")
                                        {
                                            html += "<td class='tdtext' colspan=" + tblCellColspan + ">" + tblCellLabel + "</td>";
                                        }
                                        else if (tblCellType == "header")
                                        {
                                            html += "<td class='tblHeader' colspan=" + tblCellColspan + ">" + tblCellLabel + "</td>";
                                        }
                                        else if (tblCellType == "input[text]")
                                        {
                                            string width = tblCell.Attributes["width"].Value;


                                            html += "<td colspan=" + tblCellColspan + " class='tblCell'><input id='" + tblCellId + "' name='" + tblCellId + "' style='width:" + width + "' type='text' /></td>";

                                        }
                                        else if (tblCellType == "input[textvalidate]")
                                        {
                                            string width = tblCell.Attributes["width"].Value;
                                            string ClassName = tblCell.Attributes["className"].Value;
                                            if (ClassName == "namefield")
                                                html += "<td colspan=" + tblCellColspan + " class='tblCell'><input id='" + tblCellId + "' name='" + tblCellId + "' style='width:" + width + "' type='text'  class='" + ClassName + "' onpaste='PreventDef(event)' /></td>";
                                            else
                                                html += "<td colspan=" + tblCellColspan + " class='tblCell'><input id='" + tblCellId + "' name='" + tblCellId + "' style='width:" + width + "' type='text'  class='" + ClassName + "' /></td>";

                                        }

                                        else if (tblCellType == "input[textdate]")
                                        {
                                            string width = tblCell.Attributes["width"].Value;
                                            html += "<td colspan=" + tblCellColspan + " class='tblCell'><input id='" + tblCellId + "' name='" + tblCellId + "' style='width:" + width + "' type='text' class='datepicker' onpaste='PreventDef(event)'  onkeypress='return false' /></td>";
                                        }
                                        else if (tblCellType == "input[multitext]")
                                        {
                                            string width = tblCell.Attributes["width"].Value;
                                            html += "<td colspan=" + tblCellColspan + " class='tblCell'><textarea id='" + tblCellId + "' name='" + tblCellId + "' type='text' style='width:" + width + ";' rows='2'></textarea></td>";
                                        }

                                        else if (tblCellType == "input[drop]")
                                        {
                                            countCheck = Convert.ToInt32(tblCell.Attributes["Count"].Value);
                                            string width = tblCell.Attributes["width"].Value;
                                            string[] lblsplit = tblCellLabel.Split('|');
                                            html += "<td colspan=" + tblCellColspan + " class='tblCell'><select id='" + tblCellId + "' name='" + tblCellId + "' class='drpClass1' style='width:" + width + ";'><option value=0>-----Select.....</option>";
                                            for (int i = 0; i < countCheck; i++)
                                            {
                                                html += "<option value='" + lblsplit[i] + "'>" + lblsplit[i] + "</option>";

                                                //html += "<input id='" + id + "' type='checkbox' value='unchecked' name='" + idSpl[i] + "' onclick='GetSelected(this);' !checked><span>" + lblsplit[i] + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>";

                                            }
                                            html += "</select></td>";

                                        }

                                        else if (tblCellType == "input[checkVt]")
                                        {
                                            countCheck = Convert.ToInt32(tblCell.Attributes["Count"].Value);
                                            string[] idSpl = tblCellId.Split('|');
                                            string[] lblsplit = tblCellLabel.Split('|');
                                            html += "<td colspan=" + tblCellColspan + " class='tblCell'>";
                                            for (int i = 0; i < countCheck; i++)
                                            {
                                                html += "<input id='" + idSpl[i] + "' type='checkbox' value='unchecked' name='" + idSpl[i] + "' onclick='GetSelected(this);' !checked><span>" + lblsplit[i] + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>";
                                            }
                                            html += "</td>";

                                        }

                                    }

                                    html += "</tr>";
                                }
                            }
                            html += "</table></td>";

                        }
                        else if (type == "input[textfunction]")
                        {
                            string funName = cellCust.Attributes["onkeypress"].Value;
                            html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + id + "' onkeypress='" + funName + "' type='text' /></td>";
                        }

                        else if (type == "input[textvalidate]")
                        {
                            string className = cellCust.Attributes["className"].Value;
                            if (className == "namefield")
                                html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + id + "' class='" + className + "' type='text' onpaste='PreventDef(event)' /></td>";
                            else
                                html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + id + "' class='" + className + "' type='text' /></td>";
                        }

                        else if (type == "input[textdate]")
                        {
                            html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + id + "' type='text' class='datepicker' onpaste='PreventDef(event)'  onkeypress='return false' /></td>";
                        }

                        else if (type == "input[multitext]")
                        {
                            string width = cellCust.Attributes["Width"].Value;
                            html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><textarea id='" + id + "' name='" + id + "' type='text' style='width:" + width + ";' rows='3'></textarea></td>";
                        }
                        else if (type == "input[checkVt]")
                        {

                            countCheck = Convert.ToInt32(cellCust.Attributes["Count"].Value);
                            string[] idSpl = id.Split('|');
                            string[] lblsplit = label.Split('|');
                            html += "<td colspan=" + colspan + ">";
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
                            html += "<td colspan=" + colspan + "><select id='" + id + "' name='" + id + "' class='drpClass1'><option value=0>-----Select.....</option>";
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
                        //    html += "<td colspan=" + colspan + " style='width:"+widthCell+"%;'>";
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
                            html += "<td colspan=" + colspan + ">";
                            for (int i = 0; i < countCheck; i++)
                            {
                                html += "<input id='" + idSpl[i] + "' type='checkbox' value='unchecked' name='" + idSpl[i] + "' onclick='GetSelected(this);' !checked>";
                            }
                            html += "</td>";

                        }
                        else if (type == "input[file]")
                        {
                            string nameFile = cellCust.Attributes["name"].Value.ToString();
                            html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + nameFile + "' type='file' /></td>";
                        }
                        else if (type == "input[check]")
                        {
                            countCheck = Convert.ToInt32(cellCust.Attributes["Count"].Value);
                            string[] idSplit = id.Split('|');
                            string[] lblSplit = label.Split('|');
                            html += "<td colspan=" + colspan + ">";
                            for (int i = 0; i < countCheck; i++)
                            {
                                html += "<input id='" + idSplit[i] + "' type='checkbox'>" + lblSplit[i] + "</input><br>";
                            }
                            html += "</td>";

                        }
                        else if (type == "lblBold")
                        {
                            html += "<td class='lblBold' colspan=" + colspan + " style='width:100%;'>" + label + "</td>";
                        }
                        else if (type == "td")
                        {
                            html += "<td colspan=" + colspan + "></td>";
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
                XmlNodeList xSectionsCustomEdit = xdocCustomEdit.SelectNodes("/RefferalTemplate/Sections/Section[@name]");
                XmlNode xSectnCustomEdit = xSectionsCustomEdit[Index - 1];
                XmlNodeList rowsCustEdit = xSectnCustomEdit.ChildNodes[0].ChildNodes;



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
                            html += "<td class='tdtext' colspan=" + colspan + ">" + label + "</td>";
                        else if (type == "header")
                        {
                            html += "<td class='subhead' colspan=" + colspan + ">" + label + "</td>";
                        }
                        else if (type == "input[text]")
                        {
                            html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + id + "' type='text' value='" + cellCustEdit.InnerText + "' /></td>";
                        }

                        else if (type == "table")
                        {
                            html += "<td class='tdtext' colspan=" + colspan + ">";
                            html += "<table class='tblPattern' style='width:100%; border: 0px none;'>";

                            foreach (XmlNode tblRowsEdit in cellCustEdit.ChildNodes)
                            {
                                foreach (XmlNode tblRowEdit in tblRowsEdit.ChildNodes)
                                {

                                    html += "<tr style='width:100%'>";
                                    foreach (XmlNode tblCellEdit in tblRowEdit.ChildNodes)
                                    {
                                        string tblCellId = tblCellEdit.Attributes["id"].Value;
                                        string tblCellType = tblCellEdit.Attributes["type"].Value;
                                        string tblCellLabel = tblCellEdit.Attributes["label"].Value;
                                        string tblCellColspan = tblCellEdit.Attributes["colspan"].Value;

                                        if (tblCellType == "label")
                                        {
                                            html += "<td class='tdtext' colspan=" + tblCellColspan + ">" + tblCellLabel + "</td>";
                                        }
                                        else if (tblCellType == "header")
                                        {
                                            html += "<td class='tblHeader' colspan=" + tblCellColspan + ">" + tblCellLabel + "</td>";
                                        }
                                        else if (tblCellType == "input[text]")
                                        {
                                            string width = tblCellEdit.Attributes["width"].Value;
                                            html += "<td colspan=" + tblCellColspan + " class='tblCell'><input id='" + tblCellId + "' name='" + tblCellId + "' style='width:" + width + ";' value='" + Server.HtmlEncode(Convert.ToString(tblCellEdit.InnerText)) + "' type='text' /></td>";
                                        }

                                        else if (tblCellType == "input[textdate]")
                                        {
                                            string width = tblCellEdit.Attributes["width"].Value;
                                            html += "<td colspan=" + tblCellColspan + " class='tblCell'><input id='" + tblCellId + "' name='" + tblCellId + "' style='width:" + width + ";' value='" + Server.HtmlEncode(Convert.ToString(tblCellEdit.InnerText)) + "' type='text' class='datepicker' onpaste='PreventDef(event)'  onkeypress='return false' /></td>";
                                        }
                                        else if (tblCellType == "input[multitext]")
                                        {
                                            string width = tblCellEdit.Attributes["width"].Value;
                                            html += "<td colspan=" + tblCellColspan + " class='tblCell'><textarea id='" + tblCellId + "' name='" + tblCellId + "' value='" + Server.HtmlEncode(Convert.ToString(tblCellEdit.InnerText)) + "' type='text' style='width:" + width + ";' rows='2'>" + tblCellEdit.InnerText + "</textarea></td>";
                                        }

                                        else if (tblCellType == "input[drop]")
                                        {
                                            string width = tblCellEdit.Attributes["width"].Value;
                                            string result = tblCellEdit.InnerText.ToString();
                                            countCheck = Convert.ToInt32(tblCellEdit.Attributes["Count"].Value);
                                            string[] lblsplit = tblCellLabel.Split('|');
                                            html += "<td colspan=" + colspan + " class='tblCell'><select id='" + tblCellId + "' name='" + tblCellId + "' class='drpClass1' style='width:" + width + ";'><option value=0>-----Select.....</option>";
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

                                                //html += "<input id='" + id + "' type='checkbox' value='unchecked' name='" + idSpl[i] + "' onclick='GetSelected(this);' !checked><span>" + lblsplit[i] + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>";

                                            }
                                            html += "</select></td>";

                                        }

                                        else if (tblCellType == "input[checkVt]")
                                        {
                                            string[] splitInnerTxt = tblCellEdit.InnerText.Split('|');
                                            countCheck = Convert.ToInt32(tblCellEdit.Attributes["Count"].Value);
                                            string[] idSpl = tblCellId.Split('|');
                                            string[] lblsplit = tblCellLabel.Split('|');
                                            html += "<td colspan=" + tblCellColspan + " class='tblCell'>";
                                            for (int i = 0; i < countCheck; i++)
                                            {
                                                if (splitInnerTxt[i] == "checked")
                                                {
                                                    html += "<input id='" + idSpl[i] + "' type='checkbox' value='checked' name='" + idSpl[i] + "' checked><span>" + lblsplit[i] + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>";
                                                }
                                                else
                                                {
                                                    html += "<input id='" + idSpl[i] + "' type='checkbox' value='unchecked' name='" + idSpl[i] + "'><span>" + lblsplit[i] + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>";
                                                }
                                            }
                                            html += "</td>";

                                        }

                                    }

                                    html += "</tr>";
                                }
                            }
                            html += "</table></td>";

                        }

                        else if (type == "input[textfunction]")
                        {
                            string funName = cellCustEdit.Attributes["onkeypress"].Value;
                            html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + id + "' value='" + Server.HtmlEncode(Convert.ToString(cellCustEdit.InnerText)) + "' onkeypress='" + funName + "' type='text' /></td>";
                        }

                        else if (type == "input[textvalidate]")
                        {
                            string className = cellCustEdit.Attributes["className"].Value;
                            if (className == "namefield")
                                html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + id + "' value='" + Server.HtmlEncode(Convert.ToString(cellCustEdit.InnerText)) + "' class='" + className + "' type='text' onpaste='PreventDef(event)' /></td>";
                            else
                                html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + id + "' value='" + Server.HtmlEncode(Convert.ToString(cellCustEdit.InnerText)) + "' class='" + className + "' type='text' /></td>";

                        }
                        else if (type == "input[multitext]")
                        {
                            string width = cellCustEdit.Attributes["Width"].Value;
                            html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><textarea id='" + id + "' name='" + id + "' type='text' style='width:" + width + ";' rows='3' value='" + Server.HtmlEncode(Convert.ToString(cellCustEdit.InnerText)) + "'>" + cellCustEdit.InnerText + "</textarea></td>";
                        }
                        else if (type == "input[textdate]")
                        {
                            html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' value='" + Server.HtmlEncode(Convert.ToString(cellCustEdit.InnerText)) + "'  name='" + id + "' type='text' class='datepicker' onpaste='PreventDef(event)' onkeypress='return false' /></td>";
                        }

                        else if (type == "input[drop]")
                        {
                            string result = cellCustEdit.InnerText.ToString();
                            countCheck = Convert.ToInt32(cellCustEdit.Attributes["Count"].Value);
                            string[] lblsplit = label.Split('|');
                            html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><select id='" + id + "' name='" + id + "' class='drpClass1'><option value=0>-----Select.....</option>";
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

                                //html += "<input id='" + id + "' type='checkbox' value='unchecked' name='" + idSpl[i] + "' onclick='GetSelected(this);' !checked><span>" + lblsplit[i] + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>";

                            }
                            html += "</select></td>";

                        }

                        else if (type == "input[file]")
                        {
                            string nameFile = cellCustEdit.Attributes["name"].Value.ToString();
                            html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + nameFile + "' type='file' /></td>";
                        }
                        else if (type == "input[check]")
                        {
                            countCheck = Convert.ToInt32(cellCustEdit.Attributes["Count"].Value);
                            string[] idSplit = id.Split('|');
                            string[] lblSplit = label.Split('|');
                            html += "<td colspan=" + colspan + ">";
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
                            html += "<td colspan=" + colspan + ">";
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

                        else if (type == "input[checkNolbl]")
                        {
                            string[] splitInnerTxt = cellCustEdit.InnerText.Split('|');
                            countCheck = Convert.ToInt32(cellCustEdit.Attributes["Count"].Value);
                            string[] idSpl = id.Split('|');
                            string[] lblsplit = label.Split('|');
                            html += "<td colspan=" + colspan + ">";
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
                            html += "<td class='lblBold' colspan=" + colspan + ">" + label + "</td>";
                        }
                        else if (type == "label")
                        {
                            html += "<td class='tdtext' colspan=" + colspan + ">" + label + "</td>";
                        }
                        else if (type == "td")
                        {
                            html += "<td colspan=" + colspan + "></td>";
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
                            html += "<td colspan= 3 style='text-align:right;'><input id='btnGenSave' value='Update' type='submit' class='NFButton' /></td>";
                        }
                        else
                        {
                            html += "<td colspan=3 style='text-align:right;'><input id='btnGenSave' value='Save' type='submit' class='NFButton' /></td>";
                        }
                    }
                    else
                    {
                        if (isUpated == 1)
                        {
                            html += "<td colspan= 3 style='text-align:right;'><input id='btnGenSave' value='Update' type='submit' class='NFButton' style='display:none;'/></td>";
                        }
                        else
                        {
                            html += "<td colspan=3 style='text-align:right;'><input id='btnGenSave' value='Save' type='submit' class='NFButton' style='display:none;'/></td>";
                        }
                    }
                }
                else
                {
                    if (isUpated == 1)
                    {
                        html += "<td colspan= 3 style='text-align:right;'><input id='btnGenSave' value='Update' type='submit' class='NFButton' style='display:none;'/></td>";
                    }
                    else
                    {
                        html += "<td colspan=3 style='text-align:right;'><input id='btnGenSave' value='Save' type='submit' class='NFButton' style='display:none;'/></td>";
                    }
                }

                //html += "<td colspan=" + colspan + " style='width: "+widthCell+"%; text-align:right;'><input id='" + id + "' value='" + (isUpated == 1 ? "update" : "save") + "' type='submit' class='NFButton' /></td>";
            }
            else if (Index == 2)
            {
                if (permission == "true")
                {
                    if (IsClient.Count == 0)
                    {
                        if (isUpated == 1)
                        {
                            html += "<td colspan= 3 style='text-align:right;'><input id='btnFamDataSave' value='Update' type='submit' class='NFButton' /></td>";
                        }
                        else
                        {
                            html += "<td colspan=3 style='text-align:right;'><input id='btnFamDataSave' value='Save' type='submit' class='NFButton' /></td>";
                        }
                    }
                    else
                    {
                        if (isUpated == 1)
                        {
                            html += "<td colspan= 3 style='text-align:right;'><input id='btnFamDataSave' value='Update'  style='display:none;' type='submit' class='NFButton' /></td>";
                        }
                        else
                        {
                            html += "<td colspan=3 style='text-align:right;'><input id='btnFamDataSave' value='Save'  style='display:none;' type='submit' class='NFButton' /></td>";
                        }

                    }
                }
                else
                {
                    if (isUpated == 1)
                    {
                        html += "<td colspan= 3 style='text-align:right;'><input id='btnFamDataSave' value='Update' type='submit' class='NFButton' /></td>";
                    }
                    else
                    {
                        html += "<td colspan=3 style='text-align:right;'><input id='btnFamDataSave' value='Save' type='submit' class='NFButton' /></td>";
                    }

                }
            }

            else if (Index == 3)
            {
                if (IsClient.Count == 0)
                {
                    if (isUpated == 1)
                    {
                        html += "<td colspan= 3 style='text-align:right;'><input id='btnBirthDetSave' value='Update' type='submit' class='NFButton' /></td>";   //no rows on top
                    }
                    else
                    {
                        html += "<td colspan=3 style='text-align:right;'><input id='btnBirthDetSave' value='Save' type='submit' class='NFButton' /></td>";
                    }
                }
                else
                {
                    if (isUpated == 1)
                    {
                        html += "<td colspan= 3 style='text-align:right;'><input id='btnBirthDetSave' value='Update'  style='display:none;' type='submit' class='NFButton' /></td>";   //no rows on top
                    }
                    else
                    {
                        html += "<td colspan=3 style='text-align:right;'><input id='btnBirthDetSave' value='Save'  style='display:none;' type='submit' class='NFButton' /></td>";
                    }
                }
            }
            else if (Index == 4)
            {
                if (IsClient.Count == 0)
                {
                    if (isUpated == 1)
                    {
                        html += "<td colspan= 3 style='text-align:right;'><input id='btnPersnlHistrySave' value='Update' type='submit' class='NFButton' /></td>";
                    }
                    else
                    {
                        html += "<td colspan=3 style='text-align:right;'><input id='btnPersnlHistrySave' value='Save' type='submit'  class='NFButton' /></td>";
                    }

                }
                else
                {
                    if (isUpated == 1)
                    {
                        html += "<td colspan= 3 style='text-align:right;'><input id='btnPersnlHistrySave' value='Update'   style='display:none;' type='submit' class='NFButton' /></td>";
                    }
                    else
                    {
                        html += "<td colspan=3 style='text-align:right;'><input id='btnPersnlHistrySave' value='Save'   style='display:none;' type='submit'  class='NFButton' /></td>";
                    }
                }
            }
            else if (Index == 5)
            {
                if (IsClient.Count == 0)
                {
                    if (isUpated == 1)
                    {
                        html += "<td colspan= 3 style='text-align:right;'><input id='btnRecretionSave' value='Update' type='submit' class='NFButton' /></td>";
                    }
                    else
                    {
                        html += "<td colspan=3 style='text-align:right;'><input id='btnRecretionSave' value='Save' type='submit' class='NFButton' /></td>";
                    }
                }
                else
                {
                    if (isUpated == 1)
                    {
                        html += "<td colspan= 3 style='text-align:right;'><input id='btnRecretionSave'  style='display:none;' value='Update' type='submit' class='NFButton' /></td>";
                    }
                    else
                    {
                        html += "<td colspan=3 style='text-align:right;'><input id='btnRecretionSave'  style='display:none;' value='Save' type='submit' class='NFButton' /></td>";
                    }

                }
            }
            else if (Index == 6)
            {
                if (IsClient.Count == 0)
                {
                    if (isUpated == 1)
                    {
                        html += "<td colspan= 3 style='text-align:right;'><input id='btnPresentSkillsSave' value='Update' type='submit' class='NFButton' /></td>";
                    }
                    else
                    {
                        html += "<td colspan=3 style='text-align:right;'><input id='btnPresentSkillsSave' value='Save' type='submit' class='NFButton' /></td>";
                    }
                }
                else
                {
                    if (isUpated == 1)
                    {
                        html += "<td colspan= 3 style='text-align:right;'><input id='btnPresentSkillsSave' style='display:none;'  value='Update' type='submit' class='NFButton' /></td>";
                    }
                    else
                    {
                        html += "<td colspan=3 style='text-align:right;'><input id='btnPresentSkillsSave' style='display:none;'  value='Save' type='submit' class='NFButton' /></td>";
                    }
                }
            }
            else if (Index == 7)
            {
                if (IsClient.Count == 0)
                {
                    if (isUpated == 1)
                    {
                        //html += "<td colspan= 3 style='width: "+widthCell+"%; text-align:right;'><input id='btnFundDataSave' value='Update' type='submit' class='NFButton' /></td>";
                        html += "<td style='text-align:right;'><a id='lnk' href='#' style='display:none;'  onclick='ShowDocuments()'  >View Items</a></td> <td style='text-align:right;'>" +
                            "<input id='btnFundDataSave' value='Save' type='submit' class='NFButton' /></td>";
                    }
                    else
                    {
                        //html += "<td colspan=3 style='width: "+widthCell+"%; text-align:right;'><input id='btnFundDataSave' value='Save' type='submit' class='NFButton' /></td>";
                        html += "<td style='text-align:right;'><a id='lnk' href='#' style='display:none;' onclick='ShowDocuments()' >View Items</a></td> <td style='text-align:right;'>" +
                        "<input id='btnFundDataSave' value='Save' type='submit' class='NFButton' /></td>";
                    }
                }
                else
                {
                    if (isUpated == 1)
                    {
                        //html += "<td colspan= 3 style='width: "+widthCell+"%; text-align:right;'><input id='btnFundDataSave' value='Update' type='submit' class='NFButton' /></td>";
                        html += "<td style='text-align:right;'><a id='lnk' href='#' style='display:none;'  onclick='ShowDocuments()'  >View Items</a></td> <td style='text-align:right;'>" +
                            "<input id='btnFundDataSave' value='Save' type='submit' style='display:none;'  class='NFButton' /></td>";
                    }
                    else
                    {
                        //html += "<td colspan=3 style='width: "+widthCell+"%; text-align:right;'><input id='btnFundDataSave' value='Save' type='submit' class='NFButton' /></td>";
                        html += "<td style='text-align:right;'><a id='lnk' href='#' style='display:none;' onclick='ShowDocuments()' >View Items</a></td> <td style='text-align:right;'>" +
                        "<input id='btnFundDataSave' value='Save' type='submit' class='NFButton' style='display:none;'  /></td>";
                    }
                }
            }
            else if (Index == 8)
            {
                if (IsClient.Count == 0)
                {
                    if (isUpated == 1)
                    {
                        html += "<td style='text-align:right;'><a id='lnk' href='#' style='display:none;'  onclick='ShowDocuments()'  >View Items</a></td> <td style='text-align:right;'><input id='btnSaveDoccuments' value='Save' type='submit' class='NFButton' /></td>";
                        //html += "<td colspan= 3 style='width: "+widthCell+"%; text-align:right;'><input id='btnSaveDoccuments' value='Save' type='submit' class='NFButton' /></td>";

                    }
                    else
                    {
                        html += "<td style='text-align:right;'><a id='lnk' href='#' style='display:none;' onclick='ShowDocuments()' >View Items</a></td> <td style='text-align:right;'><input id='btnSaveDoccuments' value='Save' type='submit' class='NFButton' /></td>";
                        //html += "<td colspan=3 style='width: "+widthCell+"%; text-align:right;'><input id='btnSaveDoccuments' value='Save' type='submit' class='NFButton' /></td>";

                    }
                }
                else
                {
                    if (isUpated == 1)
                    {
                        html += "<td style='text-align:right;'><a id='lnk' href='#' style='display:none;'  onclick='ShowDocuments()'  >View Items</a></td> <td style='text-align:right;'><input id='btnSaveDoccuments' value='Save'  style='display:none;' type='submit' class='NFButton' /></td>";
                        //html += "<td colspan= 3 style='width: "+widthCell+"%; text-align:right;'><input id='btnSaveDoccuments' value='Save' type='submit' class='NFButton' /></td>";

                    }
                    else
                    {
                        html += "<td style='text-align:right;'><a id='lnk' href='#' style='display:none;' onclick='ShowDocuments()' >View Items</a></td> <td style='text-align:right;'><input id='btnSaveDoccuments' value='Save'  style='display:none;' type='submit' class='NFButton' /></td>";
                        //html += "<td colspan=3 style='width: "+widthCell+"%; text-align:right;'><input id='btnSaveDoccuments' value='Save' type='submit' class='NFButton' /></td>";

                    }
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
        public ActionResult GetStates(int countryId)
        {
            clsReferral clsRf = new clsReferral();
            var dataRes = clsRf.FillState(countryId);
            return Json(dataRes, JsonRequestBehavior.AllowGet);

        }

        public void UploadDoccuments(int studentPersnlId, GenInfoModel model, HttpPostedFileBase upldFile, int schoolId, int loginId)
        {
            clsReferral objRef = new clsReferral();
            clsDocumentasBinary objBinary = new clsDocumentasBinary();
            if (upldFile != null && upldFile.ContentLength > 0)
            {
                string Name = "";

                string fileName = Path.GetFileName(upldFile.FileName);
                try
                {
                    if (Convert.ToInt16(model.objclsUpld.DocType) != 0) Name = objBinary.LookName(model.objclsUpld.DocType);
                }
                catch { }
                if (Name != "")
                {
                    int Docid = objRef.FileUpload(studentPersnlId, schoolId, loginId, model.objclsUpld.DocName, model.objclsUpld.OtherName, fileName, model.objclsUpld.DocType);
                    if (Docid > 0)
                        objBinary.SaveBinaryFiles(sess.SchoolId, sess.ReferralId, model.objclsUpld.DocName, sess.LoginId, upldFile, "Referal", Name, Docid);
                    //upldFile.SaveAs(path + id + "-" + fileName);
                }

            }
        }


        public void UploadStudentPhoto(int StudentPersnlId, GenInfoModel model, HttpPostedFileBase upldStdPhoto)
        {

            //clsReferral objRef = new clsReferral();
            //if (upldStdPhoto != null && upldStdPhoto.ContentLength > 0)
            //{
            //    byte[] fileBytes = new byte[upldStdPhoto.ContentLength];
            //    int byteCount = upldStdPhoto.InputStream.Read(fileBytes, 0, (int)upldStdPhoto.ContentLength);

            //    int id = objRef.StudentUpldPhoto(StudentPersnlId, fileBytes);
            //}

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


        }



        //[OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        //public ActionResult SectionRedirect(GenInfoModel model)
        //{
        //    string type = "";
        //    if (TempData["type"] != null)
        //    {
        //        type = TempData["type"].ToString();
        //    }
        //    //    clsReferral clsRf = new clsReferral();
        //    //int editId = Convert.ToInt32(TempData["EditId"]);
        //    //TempData["datatomethod"] = clsRf.EditDataPE(editId, type);
        //    return RedirectToAction("TabLoad", new { id = model.TabId, isAlert = true });

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
            //    clsReferral clsRf = new clsReferral();
            //int editId = Convert.ToInt32(TempData["EditId"]);
            //TempData["datatomethod"] = clsRf.EditDataPE(editId, type);
            return RedirectToAction("TabLoad", new { id = tabID, isAlert = true });

        }


        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public byte[] SaveCustomXml(FormCollection formdata, int stId, int Index)
        {
            // stId = 0;     
            int countCheck = 0;
            byte[] retXmlByte = null;
            string formVal = "";
            string html = "";
            string Globalname = "";
            bool rbtchecked = false;
            clsReferral objClsRef = new clsReferral();
            XmlDocument xdocCustom = new XmlDocument();
            xdocCustom = objClsRef.LoadXmlfromBlob(stId);
            //  xdocCustom.Load(Server.MapPath("~/XML/RefInfoCustom.xml"));
            XmlNodeList xSectionsCustom = xdocCustom.SelectNodes("/RefferalTemplate/Sections/Section[@name]");
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
                        //html += "<td class='tdtext' colspan=" + colspan + " style='width: "+widthCell+"%;'>" + label + "</td>";
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
                        else if (typeD == "table")
                        {
                            foreach (XmlNode tblRows in cellCust.ChildNodes)
                            {
                                foreach (XmlNode tblRow in tblRows.ChildNodes)
                                {
                                    foreach (XmlNode tblCell in tblRow.ChildNodes)
                                    {
                                        string tblCellId = tblCell.Attributes["id"].Value;
                                        string tblCellType = tblCell.Attributes["type"].Value;
                                        string tblCellLabel = tblCell.Attributes["label"].Value;
                                        string tblCellColspan = tblCell.Attributes["colspan"].Value;

                                        if (tblCellType == "input[text]")
                                        {
                                            formVal = formdata[tblCellId].ToString();
                                            tblCell.InnerText = formVal;
                                        }
                                        if (tblCellType == "input[textvalidate]")
                                        {
                                            formVal = formdata[tblCellId].ToString();
                                            tblCell.InnerText = formVal;
                                        }
                                        else if (tblCellType == "input[multitext]")
                                        {
                                            formVal = formdata[tblCellId].ToString();
                                            tblCell.InnerText = formVal;
                                        }

                                        else if (tblCellType == "input[textdate]")
                                        {
                                            formVal = formdata[tblCellId].ToString();
                                            tblCell.InnerText = formVal;
                                        }

                                        else if (tblCellType == "input[drop]")
                                        {
                                            countCheck = Convert.ToInt32(tblCell.Attributes["Count"].Value);
                                            string[] lblsplit = tblCellLabel.Split('|');
                                            tblCell.InnerText = "";
                                            for (int i = 0; i < countCheck; i++)
                                            {

                                                formVal = formdata[tblCellId].ToString();
                                                tblCell.InnerText = formVal;

                                            }


                                        }


                                        else if (tblCellType == "input[checkVt]")
                                        {
                                            countCheck = Convert.ToInt32(tblCell.Attributes["Count"].Value);
                                            string[] idSpl = tblCellId.Split('|');
                                            string[] lblsplit = tblCellLabel.Split('|');
                                            tblCell.InnerText = "";
                                            for (int i = 0; i < countCheck; i++)
                                            {
                                                try
                                                {
                                                    formVal = formdata[idSpl[i]].ToString();
                                                    if (formVal == "checked")
                                                    {
                                                        tblCell.InnerText += "checked|";
                                                    }
                                                    else
                                                    {
                                                        tblCell.InnerText += "unchecked|";
                                                    }

                                                }
                                                catch
                                                {
                                                    tblCell.InnerText += "unchecked|";
                                                }

                                            }
                                            tblCell.InnerText = tblCell.InnerText.Substring(0, tblCell.InnerText.Length - 1);


                                        }

                                    }
                                }
                            }
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

                            //html += "<td colspan=" + colspan + " style='width:"+widthCell+"%;'>";
                            //for (int i = 0; i < countCheck; i++)
                            //{
                            //    html += "<input id='" + idSpl[i] + "' type='checkbox'>" + lblsplit[i] + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</input>";
                            //}
                            //html += "</td>";

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

        public bool IsRefSavedBehavior(int studentId)
        {
            MelmarkDBEntities DbEntity = new MelmarkDBEntities();
            bool valid = false;

            var result = (from objBehaviorPa in DbEntity.BehavioursPAs
                          where (objBehaviorPa.StudentPersonalId == studentId)
                          select new
                          {
                              behaviorId = objBehaviorPa.BehavioursPAId

                          }).FirstOrDefault();

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
            ActionResult returnResult = null;
            try
            {
                int id = 0;
                int sveId = 0;
                string status = "";
                byte[] xmlData = null;
                clsReferral clsRf = new clsReferral();

                ClsCommon clsCommon = new ClsCommon();
                sess = (clsSession)Session["UserSession"];

                ApplicantUploadDownload clsUpld = new ApplicantUploadDownload();
                //    clsSessionTab sesClsObj = (clsSessionTab)Session["sesData"];
                string xmlPath = Server.MapPath("../XML/NewRefInfoPA.xml");
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
                                TempData["EditId"] = id = clsRf.SaveGeneralDataPE(model, type, sveId, status, sess.LoginId, sess.SchoolId, formdata);
                            }
                            else
                                TempData["EditId"] = id = clsRf.SaveGeneralDataPE(model, type, tempId, status, sess.LoginId, sess.SchoolId, formdata);

                        }

                        //if (sess.SessTab1 > 0)
                        //{
                        //    status = "update";
                        //    sveId = Convert.ToInt32(sess.SessTab1);
                        //    TempData["EditId"] = id = clsRf.SaveGeneralDataPE(model, type, sveId, status, sess.LoginId, sess.SchoolId, formdata);

                        //}
                        else
                        {
                            status = "save";
                            TempData["EditId"] = id = clsRf.SaveGeneralDataPE(model, type, sveId, status, sess.LoginId, sess.SchoolId, formdata);
                            clsRf.SaveCurrentTab(id, "Tab1");

                            xmlData = clsRf.SaveAsBlob(xmlPath);   //Initial full xml doccument saving to database
                            clsRf.SaveXmlToDB(xmlData, id);

                            sess.ReferralId = id;                    // Insert the new referal Id in Q Status
                            clsCommon.insertQstatus("NA", "N");
                            clsCommon.insertQstatus("AR", "Y");
                            LetterTray clsletter = new LetterTray();
                            MelmarkDBEntities objData = new MelmarkDBEntities();
                            int Qid = clsCommon.getQueueId("NA");
                            var LetterName = objData.LetterEngines.Where(x => x.QueueId == Qid && x.ApproveStatus == true && x.SchoolId == sess.SchoolId && x.LetterType == "Letter").ToList();
                            if (LetterName.Count > 0)
                            {
                                clsletter.insertLetter("NA", sess.ReferralId, LetterName[0].LetterEngineId, "Parent");
                            }



                            sess.SessTab1 = id;
                            Session["RefId"] = id;
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

                            if (sess.SessTab2 > 0)
                            {
                                sveId = Convert.ToInt32(sess.SessTab2);
                                if (Session["RefId"] != null)
                                {
                                    TempData["EditId"] = id = sveId;
                                    xmlData = SaveCustomXml(formdata, id, model.TabId);
                                    clsRf.SaveXmlToDB(xmlData, id);
                                }
                            }
                            else
                            {
                                if (Session["RefId"] != null)
                                {
                                    sveId = Convert.ToInt32(Session["RefId"]);
                                    TempData["EditId"] = id = sveId;
                                    xmlData = SaveCustomXml(formdata, id, model.TabId);
                                    clsRf.SaveXmlToDB(xmlData, id);
                                    clsRf.SaveCurrentTab(id, "Tab2");
                                    sess.SessTab2 = id;
                                }


                            }
                            returnResult = RedirectToAction("SectionRedirect", new { tabID = model.TabId });
                        }
                        else returnResult = View();


                        //if (Session["RefId"] != null)
                        //{
                        //    if (sesClsObj.SessTab2 > 0)
                        //    {
                        //        status = "update";
                        //        sveId = Convert.ToInt32(sesClsObj.SessTab2);
                        //        TempData["EditId"] = id = clsRf.SaveGeneralDataPE(model, type, sveId, status, sess.LoginId, sess.SchoolId);
                        //    }
                        //    else
                        //    {
                        //        if (Session["RefId"] != null)
                        //        {
                        //            sveId = Convert.ToInt32(Session["RefId"]);
                        //        }
                        //        status = "update";
                        //        TempData["EditId"] = id = clsRf.SaveGeneralDataPE(model, type, sveId, status, sess.LoginId, sess.SchoolId);
                        //        sesClsObj.SessTab2 = id;
                        //    }

                        //    xmlData = SaveCustomXml(formdata, id, model.TabId);
                        //    clsRf.SaveXmlToDB(xmlData, id);

                        //    returnResult = RedirectToAction("SectionRedirect", new { tabID = model.TabId });
                        //}
                        //else
                        //    returnResult = View();


                    }
                    else if (model.TabId == 3)
                    {

                        if (Session["RefId"] != null)
                        {

                            if (sess.SessTab3 > 0)
                            {
                                sveId = Convert.ToInt32(sess.SessTab3);
                                if (Session["RefId"] != null)
                                {
                                    TempData["EditId"] = id = sveId;
                                    xmlData = SaveCustomXml(formdata, id, model.TabId);
                                    clsRf.SaveXmlToDB(xmlData, id);
                                }
                            }
                            else
                            {
                                if (Session["RefId"] != null)
                                {
                                    sveId = Convert.ToInt32(Session["RefId"]);
                                    TempData["EditId"] = id = sveId;
                                    xmlData = SaveCustomXml(formdata, id, model.TabId);
                                    clsRf.SaveXmlToDB(xmlData, id);
                                    sess.SessTab3 = id;
                                    clsRf.SaveCurrentTab(id, "Tab3");
                                }


                            }
                            returnResult = RedirectToAction("SectionRedirect", new { tabID = model.TabId });
                        }
                        else returnResult = View();





                        //if (Session["RefId"] != null)
                        //{

                        //    if (sess.SessTab3 > 0)
                        //    {
                        //        sveId = Convert.ToInt32(sess.SessTab3);
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
                        //            sess.SessTab3 = id;
                        //        }


                        //    }
                        //    returnResult = RedirectToAction("SectionRedirect", new { tabID = model.TabId });
                        //}
                        //else returnResult = View();

                        //if (Session["RefId"] != null)
                        //{
                        //    if (sesClsObj.SessTab3 > 0)
                        //    {
                        //        status = "update";
                        //        sveId = Convert.ToInt32(sesClsObj.SessTab3);
                        //        TempData["EditId"] = id = clsRf.SaveGeneralDataPE(model, type, sveId, status, sess.LoginId, sess.SchoolId);
                        //    }
                        //    else
                        //    {
                        //        if (Session["RefId"] != null)
                        //        {
                        //            sveId = Convert.ToInt32(Session["RefId"]);
                        //        }
                        //        status = "save";
                        //        TempData["EditId"] = id = clsRf.SaveGeneralDataPE(model, type, sveId, status, sess.LoginId, sess.SchoolId);
                        //        sesClsObj.SessTab3 = id;
                        //    }

                        //    xmlData = SaveCustomXml(formdata, id, model.TabId);
                        //    clsRf.SaveXmlToDB(xmlData, id);

                        //    returnResult = RedirectToAction("SectionRedirect", new { tabID = model.TabId });
                        //}
                        //else returnResult = View();
                    }
                    else if (model.TabId == 4)
                    {
                        ClsErrorLog errorLog = new ClsErrorLog();
                        bool itabSaved = IsTabSaved(sess.ReferralId, "Tab4");
                        errorLog.WriteToLog("itabSaved: " + itabSaved);
                        if (itabSaved)
                        {
                            sess.SessTab4 = sess.ReferralId;
                        }
                        if (Session["RefId"] != null)
                        {
                            errorLog.WriteToLog("sess.SessTab4 : " + sess.SessTab4);
                            if (sess.SessTab4 > 0)
                            {
                                status = "update";
                                errorLog.WriteToLog("update : " + status);
                                sveId = Convert.ToInt32(sess.SessTab4);
                                TempData["EditId"] = id = clsRf.SaveGeneralDataPE(model, type, sveId, status, sess.LoginId, sess.SchoolId, formdata);
                            }
                            else
                            {
                                if (Session["RefId"] != null)
                                {
                                    sveId = Convert.ToInt32(Session["RefId"]);
                                }
                                status = "save";
                                TempData["EditId"] = id = clsRf.SaveGeneralDataPE(model, type, sveId, status, sess.LoginId, sess.SchoolId, formdata);
                                sess.SessTab4 = id;
                                clsRf.SaveCurrentTab(id, "Tab4");
                            }

                            xmlData = SaveCustomXml(formdata, id, model.TabId);
                            clsRf.SaveXmlToDB(xmlData, id);

                            returnResult = RedirectToAction("SectionRedirect", new { tabID = model.TabId });
                        }
                        else returnResult = View();
                    }
                    else if (model.TabId == 5)
                    {
                        sess.ReferralId = Convert.ToInt32(Session["RefId"]);
                        bool itabSaved = IsTabSaved(sess.ReferralId, "Tab5");
                        sess.SessTab5 = sess.ReferralId;
                        if (Session["RefId"] != null)
                        {
                            if (itabSaved == true)
                            {
                                status = "update";
                                sveId = Convert.ToInt32(sess.SessTab5);
                                TempData["EditId"] = id = clsRf.SaveGeneralDataPE(model, type, sveId, status, sess.LoginId, sess.SchoolId, formdata);
                            }
                            else
                            {

                                if (Session["RefId"] != null)
                                {
                                    sveId = Convert.ToInt32(Session["RefId"]);
                                }
                                status = "save";
                                TempData["EditId"] = id = clsRf.SaveGeneralDataPE(model, type, sveId, status, sess.LoginId, sess.SchoolId, formdata);
                                sess.SessTab5 = id;
                                clsRf.SaveCurrentTab(id, "Tab5");
                            }

                            xmlData = SaveCustomXml(formdata, id, model.TabId);
                            clsRf.SaveXmlToDB(xmlData, id);

                            returnResult = RedirectToAction("SectionRedirect", new { tabID = model.TabId });
                        }
                        else returnResult = View();



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
                        //            sess.SessTab5 = id;
                        //        }


                        //    }
                        //    returnResult = RedirectToAction("SectionRedirect", new { tabID = model.TabId });
                        //}
                        //else returnResult = View();
                    }
                    else if (model.TabId == 6)
                    {
                        sess.ReferralId = Convert.ToInt32(Session["RefId"]);
                        bool itabSaved = IsTabSaved(sess.ReferralId, "Tab6");
                        sess.SessTab6 = sess.ReferralId;

                        if (Session["RefId"] != null)
                        {
                            if (itabSaved == true)
                            {
                                status = "update";
                                sveId = Convert.ToInt32(sess.SessTab6);
                                TempData["EditId"] = id = clsRf.SaveGeneralDataPE(model, type, sveId, status, sess.LoginId, sess.SchoolId, formdata);
                            }
                            else
                            {
                                if (Session["RefId"] != null)
                                {
                                    sveId = Convert.ToInt32(Session["RefId"]);
                                }
                                status = "save";
                                TempData["EditId"] = id = clsRf.SaveGeneralDataPE(model, type, sveId, status, sess.LoginId, sess.SchoolId, formdata);
                                sess.SessTab6 = id;
                                clsRf.SaveCurrentTab(id, "Tab6");
                            }

                            xmlData = SaveCustomXml(formdata, id, model.TabId);
                            clsRf.SaveXmlToDB(xmlData, id);

                            returnResult = RedirectToAction("SectionRedirect", new { tabID = model.TabId });
                        }
                        else returnResult = View();

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
                        bool itabSaved = IsTabSaved(sess.ReferralId, "Tab7");
                        if (Session["RefId"] != null)
                        {

                            if (sess.SessTab7 > 0)
                            {
                                sveId = Convert.ToInt32(sess.SessTab7);
                                if (Session["RefId"] != null)
                                {
                                    TempData["EditId"] = id = sveId;
                                    //        fillUploadedDocuments(id,);
                                    if (fileUpldDocName != null)
                                    {
                                        string Result = clsCommon.GetFileType(fileUpldDocName.ContentType);
                                        if (Result != "")
                                        {
                                            UploadDoccuments(id, model, fileUpldDocName, sess.SchoolId, sess.LoginId);
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

                                    xmlData = SaveCustomXml(formdata, id, model.TabId);
                                    clsRf.SaveXmlToDB(xmlData, id);
                                }
                            }
                            else
                            {
                                if (Session["RefId"] != null)
                                {
                                    sveId = Convert.ToInt32(Session["RefId"]);
                                    TempData["EditId"] = id = sveId;
                                    if (fileUpldDocName != null)
                                    {
                                        string Result = clsCommon.GetFileType(fileUpldDocName.ContentType);
                                        if (Result != "")
                                        {
                                            UploadDoccuments(id, model, fileUpldDocName, sess.SchoolId, sess.LoginId);
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

                                    xmlData = SaveCustomXml(formdata, id, model.TabId);
                                    clsRf.SaveXmlToDB(xmlData, id);
                                    sess.SessTab7 = id;
                                    clsRf.SaveCurrentTab(id, "Tab7");
                                }

                                // TempData["EditId"] = id = clsRf.SaveGeneralData(model, type, sveId, status);
                                //  sesClsObj.SessTab5 = id;
                            }

                            returnResult = RedirectToAction("SectionRedirect", new { tabID = model.TabId });
                        }
                        else returnResult = View();

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
                                    if (fileUpldDocName != null)
                                    {
                                        string Result = clsCommon.GetFileType(fileUpldDocName.ContentType);
                                        if (Result != "")
                                        {
                                            UploadDoccuments(id, model, fileUpldDocName, sess.SchoolId, sess.LoginId);
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

                                    //   xmlData = SaveCustomXml(formdata, id, model.TabId);
                                    //   clsRf.SaveXmlToDB(xmlData, id);
                                    sess.SessTab8 = id;
                                    clsRf.SaveCurrentTab(id, "Tab2");
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
            }
            catch (Exception Ex)
            {
            }
            // return RedirectToAction("SectionRedirect", model);           

            return returnResult;
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
                //   sesClsObj = (clsSessionTab)Session["sesData"];
            }
            else if (TempData["LoadStudentData"] != null)
            {
                genModel = (GenInfoModel)TempData["LoadStudentData"];
                //   sesClsObj = (clsSessionTab)Session["sesData"];
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
            xdoc.Load(Server.MapPath("~/XML/NewRefInfoPA.xml"));
            XmlNodeList xSections = xdoc.SelectNodes("/RefferalTemplate/Sections/Section[@name]");

            int widthCell = 25;
            int tdwidth = 8;
            string html = "";
            int countCheck = 0;
            if (TempData["EditId"] == null)
            {
                // int formid = 0;
                XmlNode xSectn = xSections[Index - 1];
                //foreach (XmlNode xSectn in xSections)
                //{
                // formid++;
                string TableIds = "tblData" + Index;
                int count = 0;
                html += " <form method='post' id='frm" + Index + "' enctype='multipart/form-data' action='../RefferalApplicantPE/SaveGeneralData?type=" + xSectn.Attributes["name"].Value + "'><div><span class='headPanel'  onclick='getColapse(\"" + TableIds + "\")'>" + xSectn.Attributes["name"].Value + "</span><div class='divSubs'><table id='" + TableIds + "' class='clsTabDiv' style='float:left;width:100%'><tr style='width:100%'><td style='width:100%'><table style='width:100%;border:none;'>";
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
                                    html += "<td class='tdtext' colspan=" + colspan + ">&nbsp&nbsp&nbsp&nbsp&nbsp" + label + "</td>";
                            }
                            else
                                html += "<td class='tdtext' colspan=" + colspan + ">" + label + "</td>";
                        }
                        else if (type == "passage")
                            html += "<td class='tdPassage' colspan=" + colspan + ">" + label + "</td>";
                        else if (type == "header")
                            html += "<td class='subhead' colspan=" + colspan + ">" + label + "</td>";
                        else if (type == "skills_header")
                            html += "<td class='subhead-selfskills' colspan=" + colspan + ">" + label + "</td>";
                        else if (type == "subheader")
                            html += "<td class='sub-subhead' colspan=" + colspan + ">" + label + "</td>";

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
                                    html += "<td colspan=" + colspan + "><img id='imgStudPhoto' height='150px' width='130px' src=data:image/gif;base64," + imagePath + "></td>";
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
                                widthXml += "px !important";
                            }
                            if (val)
                            {
                                string maxLength = cell.Attributes["MaxLength"].Value;
                                string textValue = cell.Attributes["modelName"].Value;
                                object value = null;
                                try
                                {
                                    value = genModel.GetType().GetProperty(textValue.ToString()).GetValue(genModel, null);
                                }
                                catch
                                {
                                    string[] split = textValue.Split('.');
                                    if (split[0] == "objRelationMthr")

                                        value = genModel.objRelationMthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationMthr, null);
                                    if (split[0] == "objRelationFthr")

                                        value = genModel.objRelationFthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationFthr, null);
                                    if (split[0] == "objRelationClose")

                                        value = genModel.objRelationClose.GetType().GetProperty(split[1]).GetValue(genModel.objRelationClose, null);
                                    if (split[0] == "objPhysicianDetails")

                                        if (genModel.objPhysicianDetails != null) value = genModel.objPhysicianDetails.GetType().GetProperty(split[1]).GetValue(genModel.objPhysicianDetails, null);
                                    if (split[0] == "objInsuranceDetails")

                                        value = genModel.objInsuranceDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDetails, null);
                                    if (split[0] == "objInsuranceSecDetails")

                                        value = genModel.objInsuranceSecDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceSecDetails, null);
                                    if (split[0] == "objInsuranceDentalDetails")

                                        value = genModel.objInsuranceDentalDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDentalDetails, null);
                                    if (split[0] == "objRelationLegalGuardian")

                                        if (genModel.objRelationLegalGuardian != null) value = genModel.objRelationLegalGuardian.GetType().GetProperty(split[1]).GetValue(genModel.objRelationLegalGuardian, null);
                                    if (split[0] == "objRelationEmergncyContact")

                                        value = genModel.objRelationEmergncyContact.GetType().GetProperty(split[1]).GetValue(genModel.objRelationEmergncyContact, null);
                                    if (split[0] == "objclsUpld")

                                        value = genModel.objclsUpld.GetType().GetProperty(split[1]).GetValue(genModel.objclsUpld, null);
                                }


                                // html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + textValue.ToString() + "' value='" + value + "' maxlength='" + maxLength + "' type='text' /></td>";
                                html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + textValue.ToString() + "' style='" + widthXml + "' value='" + Server.HtmlEncode(Convert.ToString(value)) + "' maxlength='" + maxLength + "' type='text' /></td>";

                            }
                            else
                            {
                                if (TempData["EditId"] == null)
                                {
                                    if (label == "Birth HeightIN")
                                    {
                                        string funName = cell.Attributes["onkeypress"].Value;
                                        html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span></span><input id='" + id + "' name='" + id + "'  style='" + widthXml + " ' onkeypress='" + funName + "'' type='text' value='" + cell.InnerText + "' />OZ</td>";
                                    }
                                    else if (label == "Birth WeightLBS")
                                    {
                                        string funName = cell.Attributes["onkeypress"].Value;
                                        html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + id + "'  style='" + widthXml + " ' onkeypress='" + funName + "';margin-right:1px !important;' type='text' value='" + cell.InnerText + "' />LBS</td>";
                                    }

                                    else if (label == "Birth Weight OZ")
                                    {
                                        string funName = cell.Attributes["onkeypress"].Value;
                                        html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + id + "'  style='" + widthXml + " ' onkeypress='" + funName + "';margin-right:1px !important;' type='text' value='" + cell.InnerText + "' />OZ</td>";
                                    }
                                    else if (label == "Duration of Labor")
                                    {
                                        // string funName = cell.Attributes["onkeypress"].Value;
                                        html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + id + "'  style='width:80px !important; margin-right:1px !important;' type='text' value='" + cell.InnerText + "' />Hours</td>";
                                    }

                                    else if (label == "AgeHousehold")
                                    {
                                        string funName = cell.Attributes["onkeypress"].Value;
                                        html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + id + "'  style='" + widthXml + "' onkeypress='" + funName + "' type='text' value='" + cell.InnerText + "' /></td>";
                                    }
                                    else if (label == "Age first initiated toilet training" || label == "Age of Onset")
                                    {
                                        string funName = cell.Attributes["onkeypress"].Value;
                                        html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + id + "'  style='" + widthXml + "' onkeypress='" + funName + "';margin-right:1px !important;' type='text' value='" + cell.InnerText + "' />Yrs</td>";
                                    }
                                    else
                                        html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + id + "'  style='" + widthXml + "' type='text' value='" + cell.InnerText + "' /></td>";
                                }
                                else
                                {
                                    if (label == "Birth HeightIN")
                                    {
                                        string funName = cell.Attributes["onkeypress"].Value;
                                        html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + id + "'  style='" + widthXml + " ' onkeypress='" + funName + "'' type='text' value='" + cell.InnerText + "' />OZ</td>";
                                    }
                                    else if (label == "Birth WeightLBS")
                                    {
                                        string funName = cell.Attributes["onkeypress"].Value;
                                        html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + id + "'  style='" + widthXml + " ' onkeypress='" + funName + "';margin-right:1px !important;' type='text' value='" + cell.InnerText + "' />LBS</td>";
                                    }
                                    else if (label == "Birth Weight OZ")
                                    {
                                        string funName = cell.Attributes["onkeypress"].Value;
                                        html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + id + "'  style='" + widthXml + " ' onkeypress='" + funName + "';margin-right:1px !important;' type='text' value='" + cell.InnerText + "' />OZ</td>";
                                    }
                                    else if (label == "Duration of Labor")
                                    {
                                        // string funName = cell.Attributes["onkeypress"].Value;
                                        html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + id + "'  style='width:80px !important; margin-right:1px !important;' type='text' value='" + cell.InnerText + "' />Hours</td>";
                                    }
                                    else if (label == "AgeHousehold")
                                    {
                                        string funName = cell.Attributes["onkeypress"].Value;
                                        html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + id + "'  style='" + widthXml + "' onkeypress='" + funName + "' type='text' value='" + cell.InnerText + "'/></td>";
                                    }
                                    else if (label == "Age first initiated toilet training" || label == "Age of Onset")
                                    {
                                        string funName = cell.Attributes["onkeypress"].Value;
                                        html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + id + "'  style='" + widthXml + " ' onkeypress='" + funName + "';margin-right:1px !important;' type='text' value='" + cell.InnerText + "' />Yrs</td>";
                                    }
                                    else
                                        html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + id + "'  style='" + widthXml + "' type='text' value='" + cell.InnerText + "'/></td>";
                                }
                            }
                        }
                        else if (type == "table")
                        {
                            html += "<td class='tdtext' colspan=" + colspan + ">";
                            html += "<table class='tblPattern' style='width:100%; border: 0px none;'>";

                            foreach (XmlNode tblRowsEdit in cell.ChildNodes)
                            {
                                foreach (XmlNode tblRowEdit in tblRowsEdit.ChildNodes)
                                {

                                    html += "<tr style='width:100%'>";
                                    foreach (XmlNode tblCellEdit in tblRowEdit.ChildNodes)
                                    {
                                        string tblCellId = tblCellEdit.Attributes["id"].Value;
                                        string tblCellType = tblCellEdit.Attributes["type"].Value;
                                        string tblCellLabel = tblCellEdit.Attributes["label"].Value;
                                        string tblCellColspan = tblCellEdit.Attributes["colspan"].Value;

                                        if (tblCellType == "label")
                                        {
                                            html += "<td class='tdtext' colspan=" + tblCellColspan + ">" + tblCellLabel + "</td>";
                                        }
                                        else if (tblCellType == "header")
                                        {
                                            html += "<td class='tblHeader' colspan=" + tblCellColspan + ">" + tblCellLabel + "</td>";
                                        }
                                        else if (tblCellType == "input[text]")
                                        {
                                            string width = tblCellEdit.Attributes["width"].Value;
                                            string height = tblCellEdit.Attributes["height"].Value;
                                            html += "<td colspan=" + tblCellColspan + " class='tblCell'><input id='" + tblCellId + "' name='" + tblCellId + "' style='width:" + width + ";height:" + height + ";' value='" + Server.HtmlEncode(Convert.ToString(tblCellEdit.InnerText)) + "' type='text' /></td>";
                                        }
                                        else if (tblCellType == "input[textvalidate]")
                                        {
                                            string className = tblCellEdit.Attributes["className"].Value;
                                            string width = tblCellEdit.Attributes["width"].Value;
                                            html += "<td colspan=" + tblCellColspan + " class='tblCell'><input id='" + id + "' name='" + id + "' style='width:" + width + ";' type='text' value='" + Server.HtmlEncode(Convert.ToString(tblCellEdit.InnerText)) + "'  class='" + className + "' onpaste='PreventDef(event)' /></td>";
                                        }

                                        else if (tblCellType == "input[textdate]")
                                        {
                                            string width = tblCellEdit.Attributes["width"].Value;
                                            html += "<td colspan=" + tblCellColspan + " class='tblCell'><input id='" + tblCellId + "' name='" + tblCellId + "' style='width:" + width + ";' value='" + Server.HtmlEncode(Convert.ToString(tblCellEdit.InnerText)) + "' type='text' class='datepicker' onkeypress='return false' /></td>";
                                        }
                                        else if (tblCellType == "input[multitext]")
                                        {
                                            string width = tblCellEdit.Attributes["width"].Value;
                                            html += "<td colspan=" + tblCellColspan + " class='tblCell'><textarea id='" + tblCellId + "' name='" + tblCellId + "' value='" + Server.HtmlEncode(Convert.ToString(tblCellEdit.InnerText)) + "' type='text' style='width:" + width + ";' rows='3'>" + tblCellEdit.InnerText.Trim() + "</textarea></td>";
                                        }

                                        else if (tblCellType == "input[drop]")
                                        {
                                            string width = tblCellEdit.Attributes["width"].Value;
                                            string result = tblCellEdit.InnerText.ToString();
                                            countCheck = Convert.ToInt32(tblCellEdit.Attributes["Count"].Value);
                                            string[] lblsplit = tblCellLabel.Split('|');
                                            html += "<td colspan=" + colspan + " class='tblCell'><select id='" + tblCellId + "' name='" + tblCellId + "' class='drpClass1' style='width:" + width + ";'><option value=0>-----Select.....</option>";
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

                                                //html += "<input id='" + id + "' type='checkbox' value='unchecked' name='" + idSpl[i] + "' onclick='GetSelected(this);' !checked><span>" + lblsplit[i] + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>";

                                            }
                                            html += "</select></td>";

                                        }

                                        else if (tblCellType == "input[checkVt]")
                                        {
                                            countCheck = Convert.ToInt32(tblCellEdit.Attributes["Count"].Value);
                                            string[] idSpl = tblCellId.Split('|');
                                            string[] lblsplit = tblCellLabel.Split('|');
                                            html += "<td colspan=" + tblCellColspan + " class='tblCell'>";
                                            for (int i = 0; i < countCheck; i++)
                                            {
                                                html += "<input id='" + idSpl[i] + "' type='checkbox' value='unchecked' name='" + idSpl[i] + "' onclick='GetSelected(this);' !checked><span>" + lblsplit[i] + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>";
                                            }
                                            html += "</td>";


                                        }

                                    }

                                    html += "</tr>";
                                }
                            }
                            html += "</table></td>";

                        }


                        else if (type == "input[textvalidate]")
                        {
                            bool val = objClsRef.IsModel(cell);
                            string funName = "";
                            if (val)
                            {

                                string className = cell.Attributes["className"].Value;
                                if (className == "grossIncome")
                                {
                                    funName = cell.Attributes["onkeypress"].Value;
                                }
                                object value = null;

                                string maxLength = cell.Attributes["MaxLength"].Value;
                                string textValue = cell.Attributes["modelName"].Value;
                                try
                                {
                                    value = genModel.GetType().GetProperty(textValue.ToString()).GetValue(genModel, null);
                                }
                                catch
                                {
                                    string[] split = textValue.Split('.');
                                    if (split[0] == "objRelationMthr")
                                        value = genModel.objRelationMthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationMthr, null);
                                    if (split[0] == "objRelationFthr")
                                        value = genModel.objRelationFthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationFthr, null);
                                    if (split[0] == "objRelationClose")
                                        value = genModel.objRelationClose.GetType().GetProperty(split[1]).GetValue(genModel.objRelationClose, null);
                                    if (split[0] == "objPhysicianDetails")
                                        if (genModel.objPhysicianDetails != null) value = genModel.objPhysicianDetails.GetType().GetProperty(split[1]).GetValue(genModel.objPhysicianDetails, null);
                                    if (split[0] == "objInsuranceDetails")
                                        value = genModel.objInsuranceDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDetails, null);
                                    if (split[0] == "objInsuranceSecDetails")
                                        value = genModel.objInsuranceSecDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceSecDetails, null);
                                    if (split[0] == "objInsuranceDentalDetails")
                                        value = genModel.objInsuranceDentalDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDentalDetails, null);
                                    if (split[0] == "objRelationLegalGuardian")
                                        if (genModel.objRelationLegalGuardian != null) value = genModel.objRelationLegalGuardian.GetType().GetProperty(split[1]).GetValue(genModel.objRelationLegalGuardian, null);
                                    if (split[0] == "objRelationEmergncyContact")
                                        value = genModel.objRelationEmergncyContact.GetType().GetProperty(split[1]).GetValue(genModel.objRelationEmergncyContact, null);
                                    if (split[0] == "objclsUpld")
                                        value = genModel.objclsUpld.GetType().GetProperty(split[1]).GetValue(genModel.objclsUpld, null);
                                }

                                if (className == "validate[required]" || className == "validate[required] namefield")
                                    html += "<td colspan=" + colspan + "><span class='validate'>*</span><input id='" + textValue.ToString() + "' name='" + textValue.ToString() + "' value='" + Server.HtmlEncode(Convert.ToString(value)) + "' class='" + className + "' maxlength='" + maxLength + "'  onpaste='PreventDef(event)' type='text' /></td>";
                                else if (className == "namefield")
                                    html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + textValue.ToString() + "' name='" + textValue.ToString() + "' value='" + Server.HtmlEncode(Convert.ToString(value)) + "' class='" + className + "' maxlength='" + maxLength + "'  onpaste='PreventDef(event)' type='text' /></td>";
                                else if (className == "grossIncome")
                                    html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + textValue.ToString() + "' name='" + textValue.ToString() + "' value='" + Server.HtmlEncode(Convert.ToString(value)) + "' class='" + className + "' maxlength='" + maxLength + "'  onpaste='PreventDef(event)' onkeypress='" + funName + "' type='text' /></td>";
                                else
                                    html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + textValue.ToString() + "' name='" + textValue.ToString() + "' value='" + Server.HtmlEncode(Convert.ToString(value)) + "' class='" + className + "' maxlength='" + maxLength + "' type='text' /></td>";
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
                            object value = null;
                            if (val)
                            {
                                string textValue = cell.Attributes["modelName"].Value;
                                try
                                {
                                    value = genModel.GetType().GetProperty(textValue.ToString()).GetValue(genModel, null);
                                }
                                catch
                                {
                                    string[] split = textValue.Split('.');
                                    if (split[0] == "objRelationMthr")
                                        value = genModel.objRelationMthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationMthr, null);
                                    if (split[0] == "objRelationFthr")
                                        value = genModel.objRelationFthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationFthr, null);
                                    if (split[0] == "objRelationClose")
                                        value = genModel.objRelationClose.GetType().GetProperty(split[1]).GetValue(genModel.objRelationClose, null);
                                    if (split[0] == "objPhysicianDetails")
                                        if (genModel.objPhysicianDetails != null) value = genModel.objPhysicianDetails.GetType().GetProperty(split[1]).GetValue(genModel.objPhysicianDetails, null);
                                    if (split[0] == "objInsuranceDetails")
                                        value = genModel.objInsuranceDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDetails, null);
                                    if (split[0] == "objInsuranceSecDetails")
                                        value = genModel.objInsuranceSecDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceSecDetails, null);
                                    if (split[0] == "objInsuranceDentalDetails")
                                        value = genModel.objInsuranceDentalDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDentalDetails, null);
                                    if (split[0] == "objRelationLegalGuardian")
                                        if (genModel.objRelationLegalGuardian != null) value = genModel.objRelationLegalGuardian.GetType().GetProperty(split[1]).GetValue(genModel.objRelationLegalGuardian, null);
                                    if (split[0] == "objRelationEmergncyContact")
                                        value = genModel.objRelationEmergncyContact.GetType().GetProperty(split[1]).GetValue(genModel.objRelationEmergncyContact, null);
                                    if (split[0] == "objclsUpld")
                                        value = genModel.objclsUpld.GetType().GetProperty(split[1]).GetValue(genModel.objclsUpld, null);

                                }


                                if (textValue == "RefZipCode")
                                {
                                    html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + textValue.ToString() + "' value='" + Server.HtmlEncode(Convert.ToString(value)) + "' maxlength='" + maxLength + "'  onkeypress='" + funName + " ' onpaste='PreventDef(event)' type='text' /></td>";
                                }

                                else html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + textValue.ToString() + "' value='" + Server.HtmlEncode(Convert.ToString(value)) + "' maxlength='" + maxLength + "' onkeypress='" + funName + " ' onpaste='PreventDef(event)' type='text' /></td>";
                            }
                            else
                            {
                                if (label == "NameZip")
                                {
                                    maxLength = cell.Attributes["MaxLength"].Value;
                                    funName = cell.Attributes["onkeypress"].Value;
                                    html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + id + "' value='" + cell.InnerText + "' maxlength='" + maxLength + "'  onkeypress='" + funName + " ' onpaste='PreventDef(event)' type='text' /></td>";
                                }
                                else
                                    html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + id + "' type='text' value='" + cell.InnerText + "' /></td>";
                            }
                        }


                        else if (type == "input[textKeyType]")
                        {
                            bool val = objClsRef.IsModel(cell);


                            string funName = cell.Attributes["onkeypress"].Value;
                            string keyName = cell.Attributes["key"].Value;
                            string width = cell.Attributes["width"].Value;
                            object value = null;
                            if (val)
                            {
                                string maxLength = cell.Attributes["MaxLength"].Value;
                                string textValue = cell.Attributes["modelName"].Value;
                                try
                                {
                                    value = genModel.GetType().GetProperty(textValue.ToString()).GetValue(genModel, null);
                                }
                                catch
                                {
                                    string[] split = textValue.Split('.');
                                    if (split[0] == "objRelationMthr")
                                        value = genModel.objRelationMthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationMthr, null);
                                    if (split[0] == "objRelationFthr")
                                        value = genModel.objRelationFthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationFthr, null);
                                    if (split[0] == "objRelationClose")
                                        value = genModel.objRelationClose.GetType().GetProperty(split[1]).GetValue(genModel.objRelationClose, null);
                                    if (split[0] == "objPhysicianDetails")
                                        if (genModel.objPhysicianDetails != null) value = genModel.objPhysicianDetails.GetType().GetProperty(split[1]).GetValue(genModel.objPhysicianDetails, null);
                                    if (split[0] == "objInsuranceDetails")
                                        value = genModel.objInsuranceDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDetails, null);
                                    if (split[0] == "objInsuranceSecDetails")
                                        value = genModel.objInsuranceSecDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceSecDetails, null);
                                    if (split[0] == "objInsuranceDentalDetails")
                                        value = genModel.objInsuranceDentalDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDentalDetails, null);
                                    if (split[0] == "objRelationLegalGuardian")
                                        if (genModel.objRelationLegalGuardian != null) value = genModel.objRelationLegalGuardian.GetType().GetProperty(split[1]).GetValue(genModel.objRelationLegalGuardian, null);
                                    if (split[0] == "objRelationEmergncyContact")
                                        value = genModel.objRelationEmergncyContact.GetType().GetProperty(split[1]).GetValue(genModel.objRelationEmergncyContact, null);
                                    if (split[0] == "objclsUpld")
                                        value = genModel.objclsUpld.GetType().GetProperty(split[1]).GetValue(genModel.objclsUpld, null);
                                }

                                html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + textValue.ToString() + "' value='" + Server.HtmlEncode(Convert.ToString(value)) + "' style='width: " + width + ";' maxlength='" + maxLength + "' onkeypress='" + funName + "' type='text' /><span style='color:#595959;'>" + keyName + "</span></td>";

                            }
                            else
                                html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + id + "' type='text' value='" + cell.InnerText + "' style='width: " + width + ";' onkeypress='" + funName + "' <span style='color:#595959;'>" + keyName + "</span> </td>";
                        }

                        else if (type == "input[textKeyTypeHW]")
                        {
                            bool val = objClsRef.IsModel(cell);


                            string className = cell.Attributes["className"].Value;
                            string keyName = cell.Attributes["key"].Value;
                            string width = cell.Attributes["width"].Value;
                            object value = null;
                            if (val)
                            {
                                string maxLength = cell.Attributes["MaxLength"].Value;
                                string textValue = cell.Attributes["modelName"].Value;
                                try
                                {
                                    value = genModel.GetType().GetProperty(textValue.ToString()).GetValue(genModel, null);
                                }
                                catch
                                {
                                    string[] split = textValue.Split('.');
                                    if (split[0] == "objRelationMthr")
                                        value = genModel.objRelationMthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationMthr, null);
                                    if (split[0] == "objRelationFthr")
                                        value = genModel.objRelationFthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationFthr, null);
                                    if (split[0] == "objRelationClose")
                                        value = genModel.objRelationClose.GetType().GetProperty(split[1]).GetValue(genModel.objRelationClose, null);
                                    if (split[0] == "objPhysicianDetails")
                                        if (genModel.objPhysicianDetails != null) value = genModel.objPhysicianDetails.GetType().GetProperty(split[1]).GetValue(genModel.objPhysicianDetails, null);
                                    if (split[0] == "objInsuranceDetails")
                                        value = genModel.objInsuranceDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDetails, null);
                                    if (split[0] == "objInsuranceSecDetails")
                                        value = genModel.objInsuranceSecDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceSecDetails, null);
                                    if (split[0] == "objInsuranceDentalDetails")
                                        value = genModel.objInsuranceDentalDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDentalDetails, null);
                                    if (split[0] == "objRelationLegalGuardian")
                                        if (genModel.objRelationLegalGuardian != null) value = genModel.objRelationLegalGuardian.GetType().GetProperty(split[1]).GetValue(genModel.objRelationLegalGuardian, null);
                                    if (split[0] == "objRelationEmergncyContact")
                                        value = genModel.objRelationEmergncyContact.GetType().GetProperty(split[1]).GetValue(genModel.objRelationEmergncyContact, null);
                                    if (split[0] == "objclsUpld")
                                        value = genModel.objclsUpld.GetType().GetProperty(split[1]).GetValue(genModel.objclsUpld, null);
                                }

                                html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + textValue.ToString() + "' class='" + className + "' value='" + Server.HtmlEncode(Convert.ToString(value)) + "' style='width: " + width + ";' maxlength='" + maxLength + "' type='text' /><span style='color:#595959;'>" + keyName + "</span></td>";

                            }
                            else
                                html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + id + "' type='text' class='" + className + "' value='" + cell.InnerText + "' style='width: " + width + ";' <span style='color:#595959;'>" + keyName + "</span> </td>";
                        }

                        else if (type == "input[multitext]")
                        {
                            bool val = objClsRef.IsModel(cell);
                            string width = cell.Attributes["Width"].Value;

                            object value = null;
                            if (val)
                            {

                                string textValue = cell.Attributes["modelName"].Value;
                                try
                                {
                                    value = genModel.GetType().GetProperty(textValue.ToString()).GetValue(genModel, null);
                                }
                                catch
                                {
                                    string[] split = textValue.Split('.');
                                    if (split[0] == "objRelationMthr")
                                        value = genModel.objRelationMthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationMthr, null);
                                    if (split[0] == "objRelationFthr")
                                        value = genModel.objRelationFthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationFthr, null);
                                    if (split[0] == "objRelationClose")
                                        value = genModel.objRelationClose.GetType().GetProperty(split[1]).GetValue(genModel.objRelationClose, null);
                                    if (split[0] == "objPhysicianDetails")
                                        if (genModel.objPhysicianDetails != null) value = genModel.objPhysicianDetails.GetType().GetProperty(split[1]).GetValue(genModel.objPhysicianDetails, null);
                                    if (split[0] == "objInsuranceDetails")
                                        value = genModel.objInsuranceDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDetails, null);
                                    if (split[0] == "objInsuranceSecDetails")
                                        value = genModel.objInsuranceSecDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceSecDetails, null);
                                    if (split[0] == "objInsuranceDentalDetails")
                                        value = genModel.objInsuranceDentalDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDentalDetails, null);
                                    if (split[0] == "objRelationLegalGuardian")
                                        if (genModel.objRelationLegalGuardian != null) value = genModel.objRelationLegalGuardian.GetType().GetProperty(split[1]).GetValue(genModel.objRelationLegalGuardian, null);

                                    if (split[0] == "objRelationEmergncyContact")
                                        value = genModel.objRelationEmergncyContact.GetType().GetProperty(split[1]).GetValue(genModel.objRelationEmergncyContact, null);

                                    if (split[0] == "objclsUpld")
                                        value = genModel.objclsUpld.GetType().GetProperty(split[1]).GetValue(genModel.objclsUpld, null);
                                }

                                html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><textarea id='" + id + "' name='" + textValue.ToString() + "' value='" + Server.HtmlEncode(Convert.ToString(value)) + "' type='text' style='width:" + width + ";' rows='3'>" + Server.HtmlEncode(Convert.ToString(value)) + "</textarea></td>";
                            }
                            else
                            {
                                if (label == "Supports Coordinator")
                                {
                                    string className = cell.Attributes["className"].Value;
                                    html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><textarea id='" + id + "' name='" + id + "' onpaste='PreventDef(event)' type='text' value='" + cell.InnerText.Trim() + "' class='" + className + "'   style='width:" + width + ";' rows='3'>" + cell.InnerText.Trim() + "</textarea></td>";
                                }
                                else
                                    html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><textarea id='" + id + "' name='" + id + "' type='text' value='" + cell.InnerText.Trim() + "'   style='width:" + width + ";' rows='3'>" + cell.InnerText + "</textarea></td>";
                            }
                        }


                        else if (type == "input[textDate]")
                        {
                            bool val = objClsRef.IsModel(cell);
                            object value = null;
                            if (val)
                            {
                                string textValue = cell.Attributes["modelName"].Value;
                                string datePicker = "datepicker";

                                if (textValue == "RefDate" || textValue == "RefDOB")
                                {
                                    datePicker = "validate[required] datepicker";
                                }



                                try
                                {
                                    value = genModel.GetType().GetProperty(textValue.ToString()).GetValue(genModel, null);
                                }
                                catch
                                {
                                    string[] split = textValue.Split('.');
                                    if (split[0] == "objRelationMthr")
                                        value = genModel.objRelationMthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationMthr, null);
                                    if (split[0] == "objRelationFthr")
                                        value = genModel.objRelationFthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationFthr, null);
                                    if (split[0] == "objRelationClose")
                                        value = genModel.objRelationClose.GetType().GetProperty(split[1]).GetValue(genModel.objRelationClose, null);
                                    if (split[0] == "objPhysicianDetails")
                                        if (genModel.objPhysicianDetails != null) value = genModel.objPhysicianDetails.GetType().GetProperty(split[1]).GetValue(genModel.objPhysicianDetails, null);
                                    if (split[0] == "objInsuranceDetails")
                                        value = genModel.objInsuranceDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDetails, null);
                                    if (split[0] == "objInsuranceSecDetails")
                                        value = genModel.objInsuranceSecDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceSecDetails, null);
                                    if (split[0] == "objInsuranceDentalDetails")
                                        value = genModel.objInsuranceDentalDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDentalDetails, null);
                                    if (split[0] == "objRelationLegalGuardian")
                                        if (genModel.objRelationLegalGuardian != null) value = genModel.objRelationLegalGuardian.GetType().GetProperty(split[1]).GetValue(genModel.objRelationLegalGuardian, null);
                                    if (split[0] == "objRelationEmergncyContact")
                                        value = genModel.objRelationEmergncyContact.GetType().GetProperty(split[1]).GetValue(genModel.objRelationEmergncyContact, null);

                                    if (split[0] == "objclsUpld")
                                        value = genModel.objclsUpld.GetType().GetProperty(split[1]).GetValue(genModel.objclsUpld, null);

                                }

                                if (textValue == "RefDate" || textValue == "RefDOB")
                                {
                                    html += "<td colspan=" + colspan + "><span class='validate'>*</span><input id='" + id + "' name='" + textValue.ToString() + "' class='validate[required] datepicker' onkeypress='return false' value='" + Server.HtmlEncode(Convert.ToString(value)) + "' type='text' /></td>";
                                }
                                else
                                    html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + textValue.ToString() + "' class='datepicker' onkeypress='return false' value='" + Server.HtmlEncode(Convert.ToString(value)) + "' type='text' /></td>";
                            }
                            else
                                html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + id + "' type='text' class='datepicker' onkeypress='return false' /></td>";
                        }

                        //else if (type == "input[button]")
                        //{
                        //    if (isUpated == 1)
                        //    {
                        //        html += "<td colspan=" + colspan + " style='width: "+widthCell+"%; text-align:right;'><input id='" + id + "' value='Update' type='submit' class='NFButton' /></td>";
                        //    }
                        //    else
                        //    {
                        //        html += "<td colspan=" + colspan + " style='width: "+widthCell+"%; text-align:right;'><input id='" + id + "' value='" + label + "' type='submit' class='NFButton' /></td>";
                        //    }

                        //    //html += "<td colspan=" + colspan + " style='width: "+widthCell+"%; text-align:right;'><input id='" + id + "' value='" + (isUpated == 1 ? "update" : "save") + "' type='submit' class='NFButton' /></td>";
                        //}
                        else if (type == "input[file]")
                        {
                            string nameFile = cell.Attributes["name"].Value.ToString();
                            html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + nameFile + "' type='file' style='width:210px !important' /></td>";
                        }
                        else if (type == "input[textdate]")
                        {
                            html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' value='" + Server.HtmlEncode(Convert.ToString(cell.InnerText)) + "'  name='" + id + "' type='text' class='datepicker' onkeypress='return false' /></td>";
                        }

                        else if (type == "input[check]")
                        {
                            bool val = objClsRef.IsModel(cell);
                            countCheck = Convert.ToInt32(cell.Attributes["Count"].Value);
                            string[] idSplit = id.Split('|');
                            string[] lblSplit = label.Split('|');
                            html += "<td colspan=" + colspan + ">";
                            for (int i = 0; i < countCheck; i++)
                            {
                                html += "<input id='" + idSplit[i] + "' type='checkbox'>" + lblSplit[i] + "</input><br>";
                            }
                            html += "</td>";

                        }
                        else if (type == "lblBold")
                        {
                            html += "<td class='lblBold' colspan=" + colspan + ">" + label + "</td>";
                        }
                        //else if (type == "input[checkVt]")
                        //{
                        //    string[] splitInnerTxt = cell.InnerText.Split('|');
                        //    countCheck = Convert.ToInt32(cell.Attributes["Count"].Value);
                        //    string[] idSpl = id.Split('|');
                        //    string[] lblsplit = label.Split('|');
                        //    html += "<td colspan=" + colspan + ">";
                        //    for (int i = 0; i < countCheck; i++)
                        //    {
                        //        if (splitInnerTxt[i] == "checked")
                        //        {
                        //            html += "<input id='" + idSpl[i] + "' type='checkbox' value='checked' name='" + idSpl[i] + "' onclick='GetSelected(this);' checked><span>" + lblsplit[i] + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>";
                        //        }
                        //        else
                        //        {
                        //            html += "<input id='" + idSpl[i] + "' type='checkbox' value='unchecked' name='" + idSpl[i] + "' onclick='GetSelected(this);'><span>" + lblsplit[i] + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>";
                        //        }
                        //    }
                        //    html += "</td>";

                        //}

                        else if (type == "input[checkVt]")
                        {
                            bool val = objClsRef.IsModel(cell);
                            if (val)
                            {
                                countCheck = Convert.ToInt32(cell.Attributes["Count"].Value);
                                string[] idSpl = id.Split('|');
                                string[] lblsplit = label.Split('|');
                                html += "<td colspan=" + colspan + ">";
                                for (int i = 0; i < countCheck; i++)
                                {
                                    html += "<input id='" + idSpl[i] + "' type='checkbox'>" + lblsplit[i] + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</input>";
                                }
                                html += "</td>";
                            }
                            else
                            {

                                countCheck = Convert.ToInt32(cell.Attributes["Count"].Value);
                                string[] idSpl = id.Split('|');
                                string[] lblsplit = label.Split('|');
                                html += "<td colspan=" + colspan + ">";
                                for (int i = 0; i < countCheck; i++)
                                {
                                    html += "<input id='" + idSpl[i] + "' type='checkbox' value='unchecked' name='" + idSpl[i] + "' onclick='GetSelected(this);' !checked><span>" + lblsplit[i] + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>";
                                }
                                html += "</td>";
                            }


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
                                        html += "<td colspan=" + colspan + ">";
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
                                        html += "<td colspan=" + colspan + ">";
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
                                        html += "<td colspan=" + colspan + ">";

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
                                string[] splitInnerTxt = cell.InnerText.Split('|');
                                string name = cell.Attributes["name"].Value.ToString();
                                countCheck = Convert.ToInt32(cell.Attributes["Count"].Value);
                                string[] idSpl = id.Split('|');
                                string[] lblsplit = label.Split('|');
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

                        else if (type == "input[dropDocType]")
                        {
                            bool val = objClsRef.IsModel(cell);
                            IEnumerable<SelectListItem> ListData = new List<SelectListItem>();
                            if (val)
                            {

                                if (label == "DocumentType")
                                {
                                    ListData = objClsRef.FillDoccumentType();
                                    string textValue = cell.Attributes["modelName"].Value;
                                    html += "<td colspan=" + colspan + "><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass1'>";
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
                            else
                                html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + id + "' type='text' value='" + cell.InnerText + "' /></td>";
                        }

                        else if (type == "BehaviorCategory")
                        {
                            bool isValid = false;

                            isValid = IsRefSavedBehavior(sess.ReferralId);

                            if (isValid == false)
                            {
                                IEnumerable<SelectListItem> ListScore = new List<SelectListItem>();
                                IEnumerable<clsBehaviorCategry> ListBehaviorList = new List<clsBehaviorCategry>();
                                html += "<td colspan=" + colspan + " style='width:100%;'><table style='width:100%; border:0px;'>";
                                foreach (XmlNode subcell in cell.ChildNodes)
                                {
                                    string parentName = subcell.Attributes["label"].Value;
                                    ListBehaviorList = objClsRef.FillBehaviorCategory(parentName, genModel);
                                    ListScore = objClsRef.FillBehaviorScore();

                                    html += "<tr><td colspan='2'><span class='subhead'>" + parentName + "</span><td></tr>";

                                    foreach (var item in ListBehaviorList)
                                    {
                                        html += "<tr>";
                                        html += "<td>" + item.behaviorName + "</td>";

                                        html += "<td ><select id='" + item.behaviorId + "' name='behaveNameid_" + item.behaviorId.ToString() + "' class='drpClass1'>";
                                        html += "<option value=0>---Select Score----</option>";
                                        foreach (SelectListItem dropItem in ListScore)
                                        {
                                            html += "<option value='" + dropItem.Value + "'>" + dropItem.Text + "</option>";
                                        }

                                        html += "</td></tr>";
                                    }

                                }
                                html += "</table></td>";
                            }
                            else
                            {
                                IEnumerable<SelectListItem> ListScore = new List<SelectListItem>();
                                IEnumerable<clsBehaviorCategry> ListBehaviorList = new List<clsBehaviorCategry>();
                                html += "<td colspan=" + colspan + " style='width:100%;'><table style='width:100%; border:0px;'>";
                                foreach (XmlNode subcell in cell.ChildNodes)
                                {
                                    string parentName = subcell.Attributes["label"].Value;
                                    ListBehaviorList = objClsRef.FillBehaviorCategoryOnStudentId(sess.ReferralId, genModel, parentName);
                                    ListScore = objClsRef.FillBehaviorScore();

                                    html += "<tr><td colspan='2'><span class='subhead'>" + parentName + "</span><td></tr>";

                                    foreach (var item in ListBehaviorList)
                                    {
                                        html += "<tr>";
                                        html += "<td>" + item.behaviorName + "</td>";

                                        html += "<td ><select id='" + item.behaviorId + "' name='behaveNameid_" + item.behaviorId.ToString() + "' class='drpClass1'>";
                                        html += "<option value=0>---Select Score----</option>";
                                        foreach (SelectListItem dropItem in ListScore)
                                        {
                                            if (dropItem.Value == item.scoreId.ToString())
                                            {
                                                html += "<option value='" + dropItem.Value + "' selected ='true'>" + dropItem.Text + "</option>";
                                            }
                                            else
                                                html += "<option value='" + dropItem.Value + "'>" + dropItem.Text + "</option>";
                                        }

                                        html += "</td></tr>";
                                    }

                                }
                                html += "</table></td>";


                            }


                        }


                        else if (type == "input[drop]")
                        {
                            bool val = objClsRef.IsModel(cell);
                            IEnumerable<SelectListItem> listData = new List<SelectListItem>();
                            IEnumerable<SelectListItem> listDataState = new List<SelectListItem>();
                            if (label == "Country")
                            {
                                listData = objClsRef.FillDropList("Country");
                            }

                            if (val)
                            {

                                if (label == "Country")
                                {
                                    string textValue = cell.Attributes["modelName"].Value;
                                    string stateVal = "0";

                                    if (textValue == "RefCountry")
                                    {
                                        html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='ddlRefCountry'>";
                                        //html += "<option value=''>Select " + label + "</option>";
                                    }
                                    else
                                    {
                                        html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "'class='ddlOtherCountry'>";
                                        //html += "<option value=0>Select " + label + "</option>";
                                    }
                                    foreach (SelectListItem list in listData)
                                    {
                                        if (textValue == "objRelationMthr.txtCountry")
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
                                        else if (textValue == "objRelationFthr.txtCountry")
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
                                        else if (textValue == "objRelationClose.txtCountry")
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
                                        else if (textValue == "objPhysicianDetails.txtCountry")
                                        {
                                            try
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
                                            catch
                                            {
                                            }
                                        }
                                        else if (textValue == "objInsuranceDetails.txtCountry")
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
                                        else if (textValue == "objInsuranceSecDetails.txtCountry")
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
                                        else if (textValue == "objInsuranceDentalDetails.txtCountry")
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

                                        else if (textValue == "objRelationLegalGuardian.txtCountry")
                                        {
                                            try
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
                                            catch
                                            {
                                            }
                                        }
                                        else if (textValue == "objRelationEmergncyContact.txtCountry")
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
                                        else if (textValue == "RefCntryBirth")
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
                                        html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
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
                                        html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
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
                                        html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
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
                                        int countryId = Convert.ToInt32(genModel.objRelationClose.txtCountry);
                                        listDataState = objClsRef.FillState(countryId);
                                        html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
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
                                        html += "</select></td>";
                                    }

                                    else if (textValue == "objRelationLegalGuardian.txtState")
                                    {
                                        int countryId = 0;
                                        try { countryId = Convert.ToInt32(genModel.objRelationLegalGuardian.txtCountry); }
                                        catch { }
                                        listDataState = objClsRef.FillState(countryId);
                                        html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
                                        html += "<option value=0>Select " + label + "</option>";
                                        foreach (SelectListItem listSt in listDataState)
                                        {
                                            if (textValue == "objRelationLegalGuardian.txtState")
                                            {
                                                try
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
                                                catch
                                                {
                                                }
                                            }
                                        }
                                        html += "</select></td>";
                                    }

                                    else if (textValue == "objRelationEmergncyContact.txtState")
                                    {
                                        int countryId = Convert.ToInt32(genModel.objRelationEmergncyContact.txtCountry);
                                        listDataState = objClsRef.FillState(countryId);
                                        html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
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
                                        if (genModel.objPhysicianDetails == null)
                                        {
                                            countryId = 0;
                                        }
                                        else { countryId = Convert.ToInt32(genModel.objPhysicianDetails.txtCountry); }
                                        listDataState = objClsRef.FillState(countryId);
                                        html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
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
                                        html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
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
                                        html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
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
                                        html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
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
                                        html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
                                        html += "<option value=0>Select " + label + "</option>";
                                        html += "</select></td>";
                                    }

                                }
                                else if (label == "Race")
                                {
                                    listData = objClsRef.FillRaceType();
                                    string textValue = cell.Attributes["modelName"].Value;
                                    html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass1'>";
                                    html += "<option value=0>Select " + label + "</option>";
                                    foreach (SelectListItem Item in listData)
                                    {
                                        if (textValue == "RefRace")
                                        {
                                            if (genModel.RefRace.ToString() == Item.Value)
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
                                else if (label == "Gender")
                                {
                                    string textValue = cell.Attributes["modelName"].Value;

                                    html += "<td colspan=" + colspan + "><span class='validate'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='validate[required]'>";
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

                                    html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
                                    html += "<option value=0>Select " + label + "</option>";
                                    html += "</select></td>";
                                }
                            }

                            else if (type == "td")
                            {
                                html += "<td colspan=" + colspan + "></td>";
                            }
                            else
                            {
                                countCheck = Convert.ToInt32(cell.Attributes["Count"].Value);
                                string[] lblsplit = label.Split('|');
                                html += "<td colspan=" + colspan + "><select id='" + id + "' name='" + id + "' class='drpClass1'><option value=0>-----Select.....</option>";
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
                XmlNodeList xSectionsCustomEdit = xdocCustomEdit.SelectNodes("/RefferalTemplate/Sections/Section[@name]");
                XmlNode xSectnCustomEdit = xSectionsCustomEdit[Index - 1];


                //
                if (xSectnCustomEdit == null)
                {
                    if (Index == 1)
                    {
                        return "<div style='padding: 5px; border: 1px solid #ff0000; background-color: #fed6d6; color: #ff0000; text-align: center;'>No data to display</div>";
                    }
                    else {
                        return "";
                    }
                }

                XmlNodeList rowsCustEdit = xSectnCustomEdit.ChildNodes[0].ChildNodes;
                // int formid = 0;

                //foreach (XmlNode xSectn in xSections)
                //{
                // formid++;
                string TableIds = "tblData" + Index;
                int count = 0;
                string tabType = "";
                html += " <form method='post' id='frm" + Index + "' enctype='multipart/form-data' action='../RefferalApplicantPE/SaveGeneralData?type=" + xSectnCustomEdit.Attributes["name"].Value + "'><div><span class='headPanel'  onclick='getColapse(\"" + TableIds + "\")'>" + xSectnCustomEdit.Attributes["name"].Value + "</span><div class='divSubs'><table id='" + TableIds + "' class='clsTabDiv' style='float:left;width:100%'><tr style='width:100%'><td style='width:100%'><table style='width:100%;border:none;table-layout:fixed;'>";
                foreach (XmlNode row in rowsCustEdit)
                {
                    count = 0;
                    html += "<tr style='width:100%'>";
                    foreach (XmlNode cell in row.ChildNodes)
                    {
                        count++;
                        tabType = "";
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
                                html += "<td  class='tdtext' colspan=" + colspan + ">" + label + "</td>";
                        }
                        else if (type == "passage")
                            html += "<td class='tdPassage' colspan=" + colspan + ">" + label + "</td>";
                        else if (type == "header")
                            html += "<td class='subhead' colspan=" + colspan + ">" + label + "</td>";
                        else if (type == "skills_header")
                            html += "<td class='subhead-selfskills' colspan=" + colspan + ">" + label + "</td>";
                        else if (type == "subheader")
                            html += "<td class='sub-subhead' colspan=" + colspan + ">" + label + "</td>";
                        else if (type == "TableBreak")
                        {
                            tabType = type;
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
                                    html += "<td colspan=" + colspan + "><img id='imgStudPhoto' height='150px' width='130px' src=data:image/gif;base64," + imagePath + "></td>";
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
                                widthXml += "px !important";
                            }
                            if (val)
                            {
                                string maxLength = cell.Attributes["MaxLength"].Value;
                                string textValue = cell.Attributes["modelName"].Value;
                                object value = null;
                                try
                                {
                                    value = genModel.GetType().GetProperty(textValue.ToString()).GetValue(genModel, null);
                                }
                                catch
                                {
                                    string[] split = textValue.Split('.');
                                    if (split[0] == "objRelationMthr")

                                        value = genModel.objRelationMthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationMthr, null);
                                    if (split[0] == "objRelationFthr")

                                        value = genModel.objRelationFthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationFthr, null);
                                    if (split[0] == "objRelationClose")

                                        value = genModel.objRelationClose.GetType().GetProperty(split[1]).GetValue(genModel.objRelationClose, null);
                                    if (split[0] == "objPhysicianDetails")

                                        if (genModel.objPhysicianDetails != null) value = genModel.objPhysicianDetails.GetType().GetProperty(split[1]).GetValue(genModel.objPhysicianDetails, null);
                                    if (split[0] == "objInsuranceDetails")

                                        value = genModel.objInsuranceDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDetails, null);
                                    if (split[0] == "objInsuranceSecDetails")

                                        value = genModel.objInsuranceSecDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceSecDetails, null);
                                    if (split[0] == "objInsuranceDentalDetails")

                                        value = genModel.objInsuranceDentalDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDentalDetails, null);
                                    if (split[0] == "objRelationLegalGuardian")

                                        if (genModel.objRelationLegalGuardian != null) value = genModel.objRelationLegalGuardian.GetType().GetProperty(split[1]).GetValue(genModel.objRelationLegalGuardian, null);
                                    if (split[0] == "objRelationEmergncyContact")

                                        value = genModel.objRelationEmergncyContact.GetType().GetProperty(split[1]).GetValue(genModel.objRelationEmergncyContact, null);
                                    if (split[0] == "objclsUpld")

                                        value = genModel.objclsUpld.GetType().GetProperty(split[1]).GetValue(genModel.objclsUpld, null);
                                }

                                // html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + textValue.ToString() + "' value='" + value + "' maxlength='" + maxLength + "' type='text' /></td>";
                                html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + textValue.ToString() + "' style='" + widthXml + "' value='" + Server.HtmlEncode(Convert.ToString(value)) + "' maxlength='" + maxLength + "' type='text' /></td>";

                            }
                            else
                            {

                                if (TempData["EditId"] == null)
                                {

                                    if (label == "Birth HeightIN")
                                    {
                                        string funName = cell.Attributes["onkeypress"].Value;
                                        html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + id + "' ' onkeypress='" + funName + "'  style='width:80px !important;' type='text' value='" + cell.InnerText + "'  onpaste='PreventDef(event)' />IN</td>";
                                    }
                                    else if (label == "Birth WeightLBS")
                                    {
                                        string funName = cell.Attributes["onkeypress"].Value;
                                        html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + id + "' ' onkeypress='" + funName + "'  style='width:80px !important;margin-right:1px !important;' type='text' value='" + cell.InnerText + "' ' onpaste='PreventDef(event)' />LBS</td>";
                                    }
                                    else if (label == "Birth Weight OZ")
                                    {
                                        string funName = cell.Attributes["onkeypress"].Value;
                                        html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + id + "' ' onkeypress='" + funName + "'  style='width:80px !important;margin-right:1px !important;' type='text' value='" + cell.InnerText + "' ' onpaste='PreventDef(event)' />OZ</td>";
                                    }
                                    else if (label == "Duration of Labor")
                                    {
                                        // string funName = cell.Attributes["onkeypress"].Value;
                                        html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + id + "'  style='width:80px !important; margin-right:1px !important;' type='text' value='" + cell.InnerText + "' />Hours</td>";
                                    }
                                    else if (label == "AgeHousehold")
                                    {
                                        string funName = cell.Attributes["onkeypress"].Value;
                                        html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + id + "'  style='width:80px !important' onkeypress='" + funName + "' type='text' value='" + cell.InnerText + "' />YRS</td>";
                                    }
                                    else if (label == "Age first initiated toilet training" || label == "Age of Onset")
                                    {
                                        string funName = cell.Attributes["onkeypress"].Value;
                                        html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + id + "' ' onkeypress='" + funName + "'  style='width:80px !important;margin-right:1px !important;' type='text' value='" + cell.InnerText + "' />YRS</td>";
                                    }
                                    else
                                        html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + id + "'  style='" + widthXml + "' type='text' value='" + cell.InnerText + "' /></td>";
                                }
                                else
                                {

                                    if (label == "Birth HeightIN")
                                    {
                                        string funName = cell.Attributes["onkeypress"].Value;
                                        html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + id + "' ' onkeypress='" + funName + "'  style='width:80px !important;' type='text' value='" + cell.InnerText + "' />IN</td>";
                                    }
                                    else if (label == "Birth WeightLBS")
                                    {
                                        string funName = cell.Attributes["onkeypress"].Value;
                                        html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + id + "' ' onkeypress='" + funName + "'  style='width:80px !important;margin-right:1px !important;' type='text' value='" + cell.InnerText + "' />LBS</td>";
                                    }
                                    else if (label == "Birth Weight OZ")
                                    {
                                        string funName = cell.Attributes["onkeypress"].Value;
                                        html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + id + "' ' onkeypress='" + funName + "'  style='width:80px !important;margin-right:1px !important;' type='text' value='" + cell.InnerText + "' />OZ</td>";
                                    }
                                    else if (label == "Duration of Labor")
                                    {
                                        // string funName = cell.Attributes["onkeypress"].Value;

                                        html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + id + "'   style='width:80px !important; margin-right:1px !important;' type='text' value='" + cell.InnerText + "'  />Hours</td>";
                                    }

                                    else if (label == "AgeHousehold")
                                    {
                                        string funName = cell.Attributes["onkeypress"].Value;
                                        html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + id + "'  style='width:80px !important' onkeypress='" + funName + "' type='text' value='" + cell.InnerText + "' /></td>";
                                    }
                                    else if (label == "Age first initiated toilet training" || label == "Age of Onset")
                                    {
                                        string funName = cell.Attributes["onkeypress"].Value;
                                        html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + id + "' ' onkeypress='" + funName + "'  style='width:80px !important;margin-right:1px !important;' type='text' value='" + cell.InnerText + "' />YRS</td>";
                                    }
                                    else
                                        html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + id + "' style='" + widthXml + "' type='text' value='" + cell.InnerText + "' /></td>";
                                }
                            }
                        }
                        else if (type == "table")
                        {
                            html += "<td class='tdtext' colspan=" + colspan + ">";
                            html += "<table class='tblPattern' style='width:100%; border: 0px none;'>";

                            foreach (XmlNode tblRowsEdit in cell.ChildNodes)
                            {
                                foreach (XmlNode tblRowEdit in tblRowsEdit.ChildNodes)
                                {

                                    html += "<tr style='width:100%'>";
                                    foreach (XmlNode tblCellEdit in tblRowEdit.ChildNodes)
                                    {
                                        string tblCellId = tblCellEdit.Attributes["id"].Value;
                                        string tblCellType = tblCellEdit.Attributes["type"].Value;
                                        string tblCellLabel = tblCellEdit.Attributes["label"].Value;
                                        string tblCellColspan = tblCellEdit.Attributes["colspan"].Value;

                                        if (tblCellType == "label")
                                        {
                                            html += "<td class='tdtext' colspan=" + tblCellColspan + ">" + tblCellLabel + "</td>";
                                        }
                                        else if (tblCellType == "header")
                                        {
                                            html += "<td class='tblHeader' colspan=" + tblCellColspan + ">" + tblCellLabel + "</td>";
                                        }
                                        else if (tblCellType == "input[text]")
                                        {
                                            string height = "";
                                            string width = tblCellEdit.Attributes["width"].Value;
                                            try { height = tblCellEdit.Attributes["height"].Value; }
                                            catch { height = "47px !important;"; }
                                            // height = tblCellEdit.Attributes["height"].Value;
                                            html += "<td colspan=" + tblCellColspan + " class='tblCell'><input id='" + tblCellId + "' name='" + tblCellId + "' style='width:" + width + ";height:" + height + ";' value='" + Server.HtmlEncode(Convert.ToString(tblCellEdit.InnerText)) + "' type='text' /></td>";
                                        }

                                        else if (tblCellType == "input[textdate]")
                                        {
                                            string width = tblCellEdit.Attributes["width"].Value;
                                            html += "<td colspan=" + tblCellColspan + " class='tblCell'><input id='" + tblCellId + "' name='" + tblCellId + "' style='width:" + width + ";' value='" + Server.HtmlEncode(Convert.ToString(tblCellEdit.InnerText)) + "' type='text' class='datepicker' onkeypress='return false' /></td>";
                                        }
                                        else if (tblCellType == "input[multitext]")
                                        {
                                            string width = tblCellEdit.Attributes["width"].Value;
                                            if (width == "")
                                            {
                                                width = "100% !important";
                                            }
                                            html += "<td colspan=" + tblCellColspan + " class='tblCell'><textarea id='" + tblCellId + "' name='" + tblCellId + "' value='" + Server.HtmlEncode(Convert.ToString(tblCellEdit.InnerText.Trim())) + "' type='text' style='width:" + width + ";' rows='3'>" + tblCellEdit.InnerText + "</textarea></td>";
                                        }
                                        else if (tblCellType == "input[textvalidate]")
                                        {
                                            string className = tblCellEdit.Attributes["className"].Value;
                                            string width = tblCellEdit.Attributes["width"].Value;
                                            html += "<td colspan=" + tblCellColspan + " class='tblCell'><input id='" + tblCellId + "' name='" + tblCellId + "' type='text' value='" + Server.HtmlEncode(Convert.ToString(tblCellEdit.InnerText)) + "' style='width:" + width + ";'  class='" + className + "' onpaste='PreventDef(event)' /></td>";
                                        }

                                        else if (tblCellType == "input[drop]")
                                        {
                                            string result = tblCellEdit.InnerText.ToString();
                                            countCheck = Convert.ToInt32(tblCellEdit.Attributes["Count"].Value);
                                            string width = tblCellEdit.Attributes["width"].Value;
                                            string[] lblsplit = tblCellLabel.Split('|');
                                            html += "<td colspan=" + tblCellColspan + " class='tblCell'><select id='" + tblCellId + "' name='" + tblCellId + "' class='drpClass1' style='width:" + width + ";'><option value=0>-----Select.....</option>";
                                            for (int i = 0; i < countCheck; i++)
                                            {
                                                if (lblsplit[i] == result)
                                                {
                                                    html += "<option value='" + lblsplit[i] + "' selected='true'>" + lblsplit[i] + "</option>";
                                                }
                                                else
                                                    html += "<option value='" + lblsplit[i] + "'>" + lblsplit[i] + "</option>";
                                            }

                                            html += "</select></td>";

                                        }

                                        else if (tblCellType == "input[checkVt]")
                                        {
                                            string[] splitInnerTxt = tblCellEdit.InnerText.Split('|');
                                            countCheck = Convert.ToInt32(tblCellEdit.Attributes["Count"].Value);
                                            string[] idSpl = tblCellId.Split('|');
                                            string[] lblsplit = tblCellLabel.Split('|');
                                            html += "<td colspan=" + tblCellColspan + " class='tblCell'>";
                                            for (int i = 0; i < countCheck; i++)
                                            {
                                                if (splitInnerTxt[i] == "checked")
                                                {
                                                    html += "<input id='" + idSpl[i] + "' type='checkbox' value='checked' name='" + idSpl[i] + "' checked><span>" + lblsplit[i] + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>";
                                                }
                                                else
                                                {
                                                    html += "<input id='" + idSpl[i] + "' type='checkbox' value='unchecked' name='" + idSpl[i] + "'><span>" + lblsplit[i] + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>";
                                                }
                                            }
                                            html += "</td>";

                                        }

                                    }

                                    html += "</tr>";
                                }
                            }
                            html += "</table></td>";

                        }


                        else if (type == "input[textvalidate]")
                        {
                            bool val = objClsRef.IsModel(cell);
                            string funName = "";
                            if (val)
                            {
                                string className = cell.Attributes["className"].Value;
                                if (className == "grossIncome")
                                {
                                    funName = cell.Attributes["onkeypress"].Value;
                                }
                                string maxLength = cell.Attributes["MaxLength"].Value;
                                object value = null;

                                string textValue = cell.Attributes["modelName"].Value;
                                try
                                {
                                    value = genModel.GetType().GetProperty(textValue.ToString()).GetValue(genModel, null);
                                }
                                catch
                                {
                                    string[] split = textValue.Split('.');
                                    if (split[0] == "objRelationMthr")
                                        value = genModel.objRelationMthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationMthr, null);
                                    if (split[0] == "objRelationFthr")
                                        value = genModel.objRelationFthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationFthr, null);
                                    if (split[0] == "objRelationClose")
                                        value = genModel.objRelationClose.GetType().GetProperty(split[1]).GetValue(genModel.objRelationClose, null);
                                    if (split[0] == "objPhysicianDetails")
                                        if (genModel.objPhysicianDetails != null) value = genModel.objPhysicianDetails.GetType().GetProperty(split[1]).GetValue(genModel.objPhysicianDetails, null);
                                    if (split[0] == "objInsuranceDetails")
                                        value = genModel.objInsuranceDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDetails, null);
                                    if (split[0] == "objInsuranceSecDetails")
                                        value = genModel.objInsuranceSecDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceSecDetails, null);
                                    if (split[0] == "objInsuranceDentalDetails")
                                        value = genModel.objInsuranceDentalDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDentalDetails, null);
                                    if (split[0] == "objRelationLegalGuardian")
                                        if (genModel.objRelationLegalGuardian != null) value = genModel.objRelationLegalGuardian.GetType().GetProperty(split[1]).GetValue(genModel.objRelationLegalGuardian, null);
                                    if (split[0] == "objRelationEmergncyContact")
                                        value = genModel.objRelationEmergncyContact.GetType().GetProperty(split[1]).GetValue(genModel.objRelationEmergncyContact, null);
                                    if (split[0] == "objclsUpld")
                                        value = genModel.objclsUpld.GetType().GetProperty(split[1]).GetValue(genModel.objclsUpld, null);
                                }

                                if (className == "validate[required]" || className == "validate[required] namefield")
                                    html += "<td colspan=" + colspan + "><span class='validate'>*</span><input id='" + id + "' name='" + textValue.ToString() + "' value='" + Server.HtmlEncode(Convert.ToString(value)) + "' class='" + className + "' maxlength='" + maxLength + "' onpaste='PreventDef(event)' type='text' /></td>";
                                else if (className == "namefield")
                                    html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + textValue.ToString() + "' value='" + Server.HtmlEncode(Convert.ToString(value)) + "' class='" + className + "' maxlength='" + maxLength + "' onpaste='PreventDef(event)' type='text' /></td>";
                                else if (className == "grossIncome")
                                    html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input data-thousands=',' data-decimal='.' data-prefix='$ ' id='" + id + "' name='" + textValue.ToString() + "' value='" + Server.HtmlEncode(Convert.ToString(value)) + "' class='" + className + "' maxlength='" + maxLength + "' onpaste='PreventDef(event)' onkeypress='" + funName + "' type='text' /></td>";

                                else
                                    html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + textValue.ToString() + "' value='" + Server.HtmlEncode(Convert.ToString(value)) + "' class='" + className + "' maxlength='" + maxLength + "' type='text' /></td>";
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


                            string funName = cell.Attributes["onkeypress"].Value;
                            object value = null;
                            if (val)
                            {
                                string maxLength = cell.Attributes["MaxLength"].Value;
                                string textValue = cell.Attributes["modelName"].Value;
                                try
                                {
                                    value = genModel.GetType().GetProperty(textValue.ToString()).GetValue(genModel, null);
                                }
                                catch
                                {
                                    string[] split = textValue.Split('.');
                                    if (split[0] == "objRelationMthr")
                                        value = genModel.objRelationMthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationMthr, null);
                                    if (split[0] == "objRelationFthr")
                                        value = genModel.objRelationFthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationFthr, null);
                                    if (split[0] == "objRelationClose")
                                        value = genModel.objRelationClose.GetType().GetProperty(split[1]).GetValue(genModel.objRelationClose, null);
                                    if (split[0] == "objPhysicianDetails")
                                        if (genModel.objPhysicianDetails != null) value = genModel.objPhysicianDetails.GetType().GetProperty(split[1]).GetValue(genModel.objPhysicianDetails, null);
                                    if (split[0] == "objInsuranceDetails")
                                        value = genModel.objInsuranceDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDetails, null);
                                    if (split[0] == "objInsuranceSecDetails")
                                        value = genModel.objInsuranceSecDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceSecDetails, null);
                                    if (split[0] == "objInsuranceDentalDetails")
                                        value = genModel.objInsuranceDentalDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDentalDetails, null);
                                    if (split[0] == "objRelationLegalGuardian")
                                        if (genModel.objRelationLegalGuardian != null) value = genModel.objRelationLegalGuardian.GetType().GetProperty(split[1]).GetValue(genModel.objRelationLegalGuardian, null);
                                    if (split[0] == "objRelationEmergncyContact")
                                        value = genModel.objRelationEmergncyContact.GetType().GetProperty(split[1]).GetValue(genModel.objRelationEmergncyContact, null);
                                    if (split[0] == "objclsUpld")
                                        value = genModel.objclsUpld.GetType().GetProperty(split[1]).GetValue(genModel.objclsUpld, null);

                                }


                                if (textValue == "RefZipCode")
                                {
                                    html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + textValue.ToString() + "' value='" + Server.HtmlEncode(Convert.ToString(value)) + "' maxlength='" + maxLength + "' onkeypress='" + funName + " ' onpaste='PreventDef(event)'' type='text' /></td>";
                                }

                                else html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + textValue.ToString() + "' value='" + Server.HtmlEncode(Convert.ToString(value)) + "' maxlength='" + maxLength + "' onkeypress='" + funName + " ' onpaste='PreventDef(event)'' type='text' /></td>";
                            }

                            else
                            {
                                if (label == "NameZip")
                                {
                                    string maxLength = cell.Attributes["MaxLength"].Value;
                                    funName = cell.Attributes["onkeypress"].Value;
                                    html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + id + "' value='" + cell.InnerText + "' maxlength='" + maxLength + "'  onkeypress='" + funName + " ' onpaste='PreventDef(event)'' type='text' /></td>";
                                }
                                else
                                    html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + id + "' type='text' value='" + cell.InnerText + " /></td>";
                            }

                        }


                        else if (type == "input[textKeyType]")
                        {
                            bool val = objClsRef.IsModel(cell);


                            string funName = cell.Attributes["onkeypress"].Value;
                            string keyName = cell.Attributes["key"].Value;
                            string width = cell.Attributes["width"].Value;
                            object value = null;
                            if (val)
                            {
                                string maxLength = cell.Attributes["MaxLength"].Value;
                                string textValue = cell.Attributes["modelName"].Value;
                                try
                                {
                                    value = genModel.GetType().GetProperty(textValue.ToString()).GetValue(genModel, null);
                                }
                                catch
                                {
                                    string[] split = textValue.Split('.');
                                    if (split[0] == "objRelationMthr")
                                        value = genModel.objRelationMthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationMthr, null);
                                    if (split[0] == "objRelationFthr")
                                        value = genModel.objRelationFthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationFthr, null);
                                    if (split[0] == "objRelationClose")
                                        value = genModel.objRelationClose.GetType().GetProperty(split[1]).GetValue(genModel.objRelationClose, null);
                                    if (split[0] == "objPhysicianDetails")
                                        if (genModel.objPhysicianDetails != null) value = genModel.objPhysicianDetails.GetType().GetProperty(split[1]).GetValue(genModel.objPhysicianDetails, null);
                                    if (split[0] == "objInsuranceDetails")
                                        value = genModel.objInsuranceDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDetails, null);
                                    if (split[0] == "objInsuranceSecDetails")
                                        value = genModel.objInsuranceSecDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceSecDetails, null);
                                    if (split[0] == "objInsuranceDentalDetails")
                                        value = genModel.objInsuranceDentalDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDentalDetails, null);
                                    if (split[0] == "objRelationLegalGuardian")
                                        if (genModel.objRelationLegalGuardian != null) value = genModel.objRelationLegalGuardian.GetType().GetProperty(split[1]).GetValue(genModel.objRelationLegalGuardian, null);
                                    if (split[0] == "objRelationEmergncyContact")
                                        value = genModel.objRelationEmergncyContact.GetType().GetProperty(split[1]).GetValue(genModel.objRelationEmergncyContact, null);
                                    if (split[0] == "objclsUpld")
                                        value = genModel.objclsUpld.GetType().GetProperty(split[1]).GetValue(genModel.objclsUpld, null);
                                }

                                html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + textValue.ToString() + "' value='" + Server.HtmlEncode(Convert.ToString(value)) + "' style='width: " + width + ";' maxlength='" + maxLength + "' onkeypress='" + funName + "' type='text' /><span style='color:#595959;'>" + keyName + "</span></td>";

                            }
                            else
                                html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + id + "' type='text' value='" + cell.InnerText + "' style='width: " + width + ";' onkeypress='" + funName + "' <span style='color:#595959;'>" + keyName + "</span> </td>";
                        }

                        else if (type == "input[textKeyTypeHW]")
                        {
                            bool val = objClsRef.IsModel(cell);


                            string className = cell.Attributes["className"].Value;
                            string keyName = cell.Attributes["key"].Value;
                            string width = cell.Attributes["width"].Value;
                            object value = null;
                            if (val)
                            {
                                string maxLength = cell.Attributes["MaxLength"].Value;
                                string textValue = cell.Attributes["modelName"].Value;
                                try
                                {
                                    value = genModel.GetType().GetProperty(textValue.ToString()).GetValue(genModel, null);
                                }
                                catch
                                {
                                    string[] split = textValue.Split('.');
                                    if (split[0] == "objRelationMthr")
                                        value = genModel.objRelationMthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationMthr, null);
                                    if (split[0] == "objRelationFthr")
                                        value = genModel.objRelationFthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationFthr, null);
                                    if (split[0] == "objRelationClose")
                                        value = genModel.objRelationClose.GetType().GetProperty(split[1]).GetValue(genModel.objRelationClose, null);
                                    if (split[0] == "objPhysicianDetails")
                                        if (genModel.objPhysicianDetails != null) value = genModel.objPhysicianDetails.GetType().GetProperty(split[1]).GetValue(genModel.objPhysicianDetails, null);
                                    if (split[0] == "objInsuranceDetails")
                                        value = genModel.objInsuranceDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDetails, null);
                                    if (split[0] == "objInsuranceSecDetails")
                                        value = genModel.objInsuranceSecDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceSecDetails, null);
                                    if (split[0] == "objInsuranceDentalDetails")
                                        value = genModel.objInsuranceDentalDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDentalDetails, null);
                                    if (split[0] == "objRelationLegalGuardian")
                                        if (genModel.objRelationLegalGuardian != null) value = genModel.objRelationLegalGuardian.GetType().GetProperty(split[1]).GetValue(genModel.objRelationLegalGuardian, null);
                                    if (split[0] == "objRelationEmergncyContact")
                                        value = genModel.objRelationEmergncyContact.GetType().GetProperty(split[1]).GetValue(genModel.objRelationEmergncyContact, null);
                                    if (split[0] == "objclsUpld")
                                        value = genModel.objclsUpld.GetType().GetProperty(split[1]).GetValue(genModel.objclsUpld, null);
                                }

                                html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + textValue.ToString() + "' class='" + className + "' value='" + Server.HtmlEncode(Convert.ToString(value)) + "' style='width: " + width + ";' maxlength='" + maxLength + "' type='text' /><span style='color:#595959;'>" + keyName + "</span></td>";

                            }
                            else
                                html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + id + "' type='text' class='" + className + "' value='" + cell.InnerText + "' style='width: " + width + ";' <span style='color:#595959;'>" + keyName + "</span> </td>";
                        }


                        else if (type == "input[multitext]")
                        {
                            bool val = objClsRef.IsModel(cell);

                            string width = cell.Attributes["Width"].Value;
                            object value = null;
                            if (val)
                            {

                                string textValue = cell.Attributes["modelName"].Value;
                                try
                                {
                                    value = genModel.GetType().GetProperty(textValue.ToString()).GetValue(genModel, null);
                                }
                                catch
                                {
                                    string[] split = textValue.Split('.');
                                    if (split[0] == "objRelationMthr")
                                        value = genModel.objRelationMthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationMthr, null);
                                    if (split[0] == "objRelationFthr")
                                        value = genModel.objRelationFthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationFthr, null);
                                    if (split[0] == "objRelationClose")
                                        value = genModel.objRelationClose.GetType().GetProperty(split[1]).GetValue(genModel.objRelationClose, null);
                                    if (split[0] == "objPhysicianDetails")
                                        if (genModel.objPhysicianDetails != null) value = genModel.objPhysicianDetails.GetType().GetProperty(split[1]).GetValue(genModel.objPhysicianDetails, null);
                                    if (split[0] == "objInsuranceDetails")
                                        value = genModel.objInsuranceDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDetails, null);
                                    if (split[0] == "objInsuranceSecDetails")
                                        value = genModel.objInsuranceSecDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceSecDetails, null);
                                    if (split[0] == "objInsuranceDentalDetails")
                                        value = genModel.objInsuranceDentalDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDentalDetails, null);
                                    if (split[0] == "objRelationLegalGuardian")
                                        if (genModel.objRelationLegalGuardian != null) value = genModel.objRelationLegalGuardian.GetType().GetProperty(split[1]).GetValue(genModel.objRelationLegalGuardian, null);

                                    if (split[0] == "objRelationEmergncyContact")
                                        value = genModel.objRelationEmergncyContact.GetType().GetProperty(split[1]).GetValue(genModel.objRelationEmergncyContact, null);

                                    if (split[0] == "objclsUpld")
                                        value = genModel.objclsUpld.GetType().GetProperty(split[1]).GetValue(genModel.objclsUpld, null);
                                }

                                html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><textarea id='" + id + "' name='" + textValue.ToString() + "' value='" + Server.HtmlEncode(Convert.ToString(value)) + "' type='text' style='width:" + width + ";' rows='3'>" + Server.HtmlEncode(Convert.ToString(value)) + "</textarea></td>";
                            }
                            else
                            {
                                string className = "";
                                if (label == "Supports Coordinator")
                                {
                                    if (null == cell.Attributes.GetNamedItem("className"))
                                    {
                                        html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><textarea id='" + id + "' name='" + id + "' type='text' value='" + cell.InnerText.Trim() + "'   style='width:" + width + ";' rows='3'>" + cell.InnerText + "</textarea></td>";
                                    }
                                    else
                                    {
                                        className = cell.Attributes["className"].Value;
                                        html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><textarea id='" + id + "' name='" + id + "' onpaste='PreventDef(event)' type='text' value='" + cell.InnerText.Trim() + "' class='" + className + "'   style='width:" + width + ";' rows='3'>" + cell.InnerText.Trim() + "</textarea></td>";
                                    }
                                }
                                else
                                    html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><textarea id='" + id + "' name='" + id + "' type='text' value='" + cell.InnerText.Trim() + "'   style='width:" + width + ";' rows='3'>" + cell.InnerText + "</textarea></td>";
                            }
                        }

                        else if (type == "input[textDate]")
                        {
                            bool val = objClsRef.IsModel(cell);
                            object value = null;
                            if (val)
                            {
                                string textValue = cell.Attributes["modelName"].Value;
                                string datePicker = "datepicker";

                                if (textValue == "RefDate" || textValue == "RefDOB")
                                {
                                    datePicker = "validate[required] datepicker";
                                }



                                try
                                {
                                    value = genModel.GetType().GetProperty(textValue.ToString()).GetValue(genModel, null);
                                }
                                catch
                                {
                                    string[] split = textValue.Split('.');
                                    if (split[0] == "objRelationMthr")
                                        value = genModel.objRelationMthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationMthr, null);
                                    if (split[0] == "objRelationFthr")
                                        value = genModel.objRelationFthr.GetType().GetProperty(split[1]).GetValue(genModel.objRelationFthr, null);
                                    if (split[0] == "objRelationClose")
                                        value = genModel.objRelationClose.GetType().GetProperty(split[1]).GetValue(genModel.objRelationClose, null);
                                    if (split[0] == "objPhysicianDetails")
                                        if (genModel.objPhysicianDetails != null) value = genModel.objPhysicianDetails.GetType().GetProperty(split[1]).GetValue(genModel.objPhysicianDetails, null);
                                    if (split[0] == "objInsuranceDetails")
                                        value = genModel.objInsuranceDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDetails, null);
                                    if (split[0] == "objInsuranceSecDetails")
                                        value = genModel.objInsuranceSecDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceSecDetails, null);
                                    if (split[0] == "objInsuranceDentalDetails")
                                        value = genModel.objInsuranceDentalDetails.GetType().GetProperty(split[1]).GetValue(genModel.objInsuranceDentalDetails, null);
                                    if (split[0] == "objRelationLegalGuardian")
                                        if (genModel.objRelationLegalGuardian != null) value = genModel.objRelationLegalGuardian.GetType().GetProperty(split[1]).GetValue(genModel.objRelationLegalGuardian, null);
                                    if (split[0] == "objRelationEmergncyContact")
                                        value = genModel.objRelationEmergncyContact.GetType().GetProperty(split[1]).GetValue(genModel.objRelationEmergncyContact, null);

                                    if (split[0] == "objclsUpld")
                                        value = genModel.objclsUpld.GetType().GetProperty(split[1]).GetValue(genModel.objclsUpld, null);

                                }

                                if (textValue == "RefDate" || textValue == "RefDOB")
                                {
                                    html += "<td colspan=" + colspan + "><span class='validate'>*</span><input id='" + id + "' name='" + textValue.ToString() + "' class='validate[required] datepicker' onkeypress='return false' value='" + Server.HtmlEncode(Convert.ToString(value)) + "' type='text' /></td>";
                                }
                                else
                                    html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + textValue.ToString() + "' class='datepicker' onkeypress='return false' value='" + Server.HtmlEncode(Convert.ToString(value)) + "' type='text' /></td>";
                            }
                            else
                                html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + id + "' type='text' class='datepicker' onkeypress='return false' /></td>";
                        }

                        //else if (type == "input[button]")
                        //{
                        //    if (isUpated == 1)
                        //    {
                        //        html += "<td colspan=" + colspan + " style='width: "+widthCell+"%; text-align:right;'><input id='" + id + "' value='Update' type='submit' class='NFButton' /></td>";
                        //    }
                        //    else
                        //    {
                        //        html += "<td colspan=" + colspan + " style='width: "+widthCell+"%; text-align:right;'><input id='" + id + "' value='" + label + "' type='submit' class='NFButton' /></td>";
                        //    }

                        //    //html += "<td colspan=" + colspan + " style='width: "+widthCell+"%; text-align:right;'><input id='" + id + "' value='" + (isUpated == 1 ? "update" : "save") + "' type='submit' class='NFButton' /></td>";
                        //}
                        else if (type == "input[file]")
                        {
                            string nameFile = cell.Attributes["name"].Value.ToString();
                            html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + nameFile + "' type='file' style='width:210px !important' /></td>";
                        }
                        else if (type == "input[textdate]")
                        {
                            html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' value='" + Server.HtmlEncode(Convert.ToString(cell.InnerText)) + "'  name='" + id + "' type='text' class='datepicker' onkeypress='return false' /></td>";
                        }

                        else if (type == "input[check]")
                        {
                            string[] splitInnerTxt = cell.InnerText.Split('|');
                            countCheck = Convert.ToInt32(cell.Attributes["Count"].Value);
                            string[] idSpl = id.Split('|');
                            string[] lblsplit = label.Split('|');
                            html += "<td colspan=" + colspan + ">";
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
                        else if (type == "lblBold")
                        {
                            html += "<td class='lblBold' colspan=" + colspan + ">" + label + "</td>";
                        }
                        else if (type == "input[checkVt]")
                        {
                            string[] splitInnerTxt = cell.InnerText.Split('|');
                            countCheck = Convert.ToInt32(cell.Attributes["Count"].Value);
                            string[] idSpl = id.Split('|');
                            string[] lblsplit = label.Split('|');
                            html += "<td colspan=" + colspan + ">";
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
                                        html += "<td colspan=" + colspan + ">";
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
                                        html += "<td colspan=" + colspan + ">";
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
                                        html += "<td colspan=" + colspan + ">";

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

                        else if (type == "input[dropDocType]")
                        {
                            bool val = objClsRef.IsModel(cell);
                            IEnumerable<SelectListItem> ListData = new List<SelectListItem>();
                            if (val)
                            {

                                if (label == "DocumentType")
                                {
                                    ListData = objClsRef.FillDoccumentType();
                                    string textValue = cell.Attributes["modelName"].Value;
                                    html += "<td colspan=" + colspan + "><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass1'>";
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
                            else
                                html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><input id='" + id + "' name='" + id + "' type='text' value='" + cell.InnerText + "' /></td>";
                        }

                        else if (type == "BehaviorCategory")
                        {
                            bool isValid = false;

                            isValid = IsRefSavedBehavior(sess.ReferralId);

                            if (isValid == false)
                            {
                                IEnumerable<SelectListItem> ListScore = new List<SelectListItem>();
                                IEnumerable<clsBehaviorCategry> ListBehaviorList = new List<clsBehaviorCategry>();
                                html += "<td colspan=" + colspan + " style='width:100%;'><table style='width:100%; border:0px;'>";
                                foreach (XmlNode subcell in cell.ChildNodes)
                                {
                                    string parentName = subcell.Attributes["label"].Value;
                                    ListBehaviorList = objClsRef.FillBehaviorCategory(parentName, genModel);
                                    ListScore = objClsRef.FillBehaviorScore();

                                    html += "<tr><td colspan='2'><span class='subhead'>" + parentName + "</span><td></tr>";

                                    foreach (var item in ListBehaviorList)
                                    {
                                        html += "<tr>";
                                        html += "<td>" + item.behaviorName + "</td>";

                                        html += "<td ><select id='" + item.behaviorId + "' name='behaveNameid_" + item.behaviorId.ToString() + "' class='drpClass1'>";
                                        html += "<option value=0>---Select Score----</option>";
                                        foreach (SelectListItem dropItem in ListScore)
                                        {
                                            html += "<option value='" + dropItem.Value + "'>" + dropItem.Text + "</option>";
                                        }

                                        html += "</td></tr>";
                                    }

                                }
                                html += "</table></td>";
                            }
                            else
                            {
                                IEnumerable<SelectListItem> ListScore = new List<SelectListItem>();
                                IEnumerable<clsBehaviorCategry> ListBehaviorList = new List<clsBehaviorCategry>();
                                html += "<td colspan=" + colspan + " style='width:100%;'><table style='width:100%; border:0px;'>";
                                foreach (XmlNode subcell in cell.ChildNodes)
                                {
                                    string parentName = subcell.Attributes["label"].Value;
                                    ListBehaviorList = objClsRef.FillBehaviorCategoryOnStudentId(sess.ReferralId, genModel, parentName);
                                    ListScore = objClsRef.FillBehaviorScore();

                                    html += "<tr><td colspan='2'><span class='subhead'>" + parentName + "</span><td></tr>";

                                    foreach (var item in ListBehaviorList)
                                    {
                                        html += "<tr>";
                                        html += "<td>" + item.behaviorName + "</td>";

                                        html += "<td ><select id='" + item.behaviorId + "' name='behaveNameid_" + item.behaviorId.ToString() + "' class='drpClass1'>";
                                        html += "<option value=0>---Select Score----</option>";
                                        foreach (SelectListItem dropItem in ListScore)
                                        {
                                            if (dropItem.Value == item.scoreId.ToString())
                                            {
                                                html += "<option value='" + dropItem.Value + "' selected ='true'>" + dropItem.Text + "</option>";
                                            }
                                            else
                                                html += "<option value='" + dropItem.Value + "'>" + dropItem.Text + "</option>";
                                        }

                                        html += "</td></tr>";
                                    }

                                }
                                html += "</table></td>";


                            }


                        }


                        else if (type == "input[drop]")
                        {

                            bool val = objClsRef.IsModel(cell);

                            IEnumerable<SelectListItem> listData = new List<SelectListItem>();
                            IEnumerable<SelectListItem> listDataState = new List<SelectListItem>();
                            if (label == "Country")
                            {

                                listData = objClsRef.FillDropList("Country");
                            }

                            if (val)
                            {
                                if (label == "Country")
                                {

                                    string textValue = cell.Attributes["modelName"].Value;
                                    string stateVal = "0";

                                    if (textValue == "RefCountry")
                                    {
                                        html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='ddlRefCountry'>";
                                        //html += "<option value=''>Select " + label + "</option>";
                                    }
                                    else
                                    {
                                        html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "'class='ddlOtherCountry'>";
                                        //html += "<option value=0>Select " + label + "</option>";
                                    }
                                    foreach (SelectListItem list in listData)
                                    {
                                        if (textValue == "objRelationMthr.txtCountry")
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
                                        else if (textValue == "objRelationFthr.txtCountry")
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
                                        else if (textValue == "objRelationClose.txtCountry")
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
                                        else if (textValue == "objPhysicianDetails.txtCountry")
                                        {
                                            try
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
                                            catch
                                            {
                                            }
                                        }
                                        else if (textValue == "objInsuranceDetails.txtCountry")
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
                                        else if (textValue == "objInsuranceSecDetails.txtCountry")
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
                                        else if (textValue == "objInsuranceDentalDetails.txtCountry")
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

                                        else if (textValue == "objRelationLegalGuardian.txtCountry")
                                        {
                                            try
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
                                            catch
                                            {
                                            }
                                        }
                                        else if (textValue == "objRelationEmergncyContact.txtCountry")
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
                                        else if (textValue == "RefCntryBirth")
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
                                        html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
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
                                        html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
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
                                        html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
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
                                        int countryId = Convert.ToInt32(genModel.objRelationClose.txtCountry);
                                        listDataState = objClsRef.FillState(countryId);
                                        html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
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
                                        html += "</select></td>";
                                    }

                                    else if (textValue == "objRelationLegalGuardian.txtState")
                                    {
                                        int countryId = 0;
                                        try { countryId = Convert.ToInt32(genModel.objRelationLegalGuardian.txtCountry); }
                                        catch { }
                                        listDataState = objClsRef.FillState(countryId);
                                        html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
                                        html += "<option value=0>Select " + label + "</option>";
                                        foreach (SelectListItem listSt in listDataState)
                                        {
                                            if (textValue == "objRelationLegalGuardian.txtState")
                                            {
                                                try
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
                                                catch
                                                {
                                                }
                                            }
                                        }
                                        html += "</select></td>";
                                    }

                                    else if (textValue == "objRelationEmergncyContact.txtState")
                                    {
                                        int countryId = Convert.ToInt32(genModel.objRelationEmergncyContact.txtCountry);
                                        listDataState = objClsRef.FillState(countryId);
                                        html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
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
                                        if (genModel.objPhysicianDetails == null)
                                        {
                                            countryId = 0;
                                        }
                                        else { countryId = Convert.ToInt32(genModel.objPhysicianDetails.txtCountry); }
                                        listDataState = objClsRef.FillState(countryId);
                                        html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
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
                                        html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
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
                                        html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
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
                                        html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
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
                                        html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
                                        html += "<option value=0>Select " + label + "</option>";
                                        html += "</select></td>";
                                    }

                                }
                                else if (label == "Race")
                                {
                                    listData = objClsRef.FillRaceType();
                                    string textValue = cell.Attributes["modelName"].Value;
                                    html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass1'>";
                                    html += "<option value=0>Select " + label + "</option>";
                                    foreach (SelectListItem Item in listData)
                                    {
                                        if (textValue == "RefRace")
                                        {
                                            if (genModel.RefRace.ToString() == Item.Value)
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
                                else if (label == "Gender")
                                {
                                    string textValue = cell.Attributes["modelName"].Value;

                                    html += "<td colspan=" + colspan + "><span class='validate'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='validate[required]'>";
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

                                    html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><select id='" + id + "' name='" + textValue.ToString() + "' class='drpClass'>";
                                    html += "<option value=0>Select " + label + "</option>";
                                    html += "</select></td>";
                                }
                            }
                            else
                            {
                                if (label == "United States Of America")
                                {

                                    string result = cell.InnerText.ToString();
                                    if (result == "")
                                        result = "United States Of America";
                                    countCheck = Convert.ToInt32(cell.Attributes["Count"].Value);
                                    string[] lblsplit = label.Split('|');
                                    html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><select id='" + id + "' name='" + id + "' class='drpClass1'><option value=0>-----Select.....</option>";
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

                                        //html += "<input id='" + id + "' type='checkbox' value='unchecked' name='" + idSpl[i] + "' onclick='GetSelected(this);' !checked><span>" + lblsplit[i] + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>";

                                    }
                                    html += "</select></td>";
                                }
                                else
                                {
                                    string result = cell.InnerText.ToString();
                                    countCheck = Convert.ToInt32(cell.Attributes["Count"].Value);
                                    string[] lblsplit = label.Split('|');
                                    html += "<td colspan=" + colspan + "><span class='fontwhite'>*</span><select id='" + id + "' name='" + id + "' class='drpClass1'><option value=0>-----Select.....</option>";
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

                                        //html += "<input id='" + id + "' type='checkbox' value='unchecked' name='" + idSpl[i] + "' onclick='GetSelected(this);' !checked><span>" + lblsplit[i] + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>";

                                    }
                                    html += "</select></td>";
                                }
                            }


                        }

                        else if (type == "td")
                        {
                            html += "<td colspan=" + colspan + "></td>";
                        }

                    }


                    if (tabType == "")
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
                            html += "<td colspan= 3 style='text-align:right;'><input id='btnGenSave' value='Update' type='submit' class='NFButton' /></td>";
                        }
                        else
                        {
                            html += "<td colspan=3 style='text-align:right;'><input id='btnGenSave' value='Save' type='submit' class='NFButton' /></td>";
                        }
                    }
                    else
                    {
                        if (isUpated == 1)
                        {
                            html += "<td colspan= 3 style='text-align:right;'><input id='btnGenSave' value='Update' type='submit' class='NFButton' style='display:none;'/></td>";
                        }
                        else
                        {
                            html += "<td colspan=3 style='text-align:right;'><input id='btnGenSave' value='Save' type='submit' class='NFButton' style='display:none;'/></td>";
                        }
                    }
                }
                else
                {
                    if (isUpated == 1)
                    {
                        html += "<td colspan= 3 style='text-align:right;'><input id='btnGenSave' value='Update' type='submit' class='NFButton' style='display:none;'/></td>";
                    }
                    else
                    {
                        html += "<td colspan=3 style='text-align:right;'><input id='btnGenSave' value='Save' type='submit' class='NFButton' style='display:none;'/></td>";
                    }
                }

                //html += "<td colspan=" + colspan + " style='width: "+widthCell+"%; text-align:right;'><input id='" + id + "' value='" + (isUpated == 1 ? "update" : "save") + "' type='submit' class='NFButton' /></td>";
            }
            else if (Index == 2)
            {
                if (permission == "true")
                {
                    if (IsClient.Count == 0)
                    {
                        if (isUpated == 1)
                        {
                            html += "<td colspan= 3 style='text-align:right;'><input id='btnFamDataSave' value='Update' type='submit' class='NFButton' /></td>";
                        }
                        else
                        {
                            html += "<td colspan=3 style='text-align:right;'><input id='btnFamDataSave' value='Save' type='submit' class='NFButton' /></td>";
                        }
                    }
                    else
                    {
                        if (isUpated == 1)
                        {
                            html += "<td colspan= 3 style='text-align:right;'><input id='btnFamDataSave' value='Update'  style='display:none;' type='submit' class='NFButton' /></td>";
                        }
                        else
                        {
                            html += "<td colspan=3 style='text-align:right;'><input id='btnFamDataSave' value='Save'  style='display:none;' type='submit' class='NFButton' /></td>";
                        }

                    }
                }
                else
                {
                    if (isUpated == 1)
                    {
                        html += "<td colspan= 3 style='text-align:right;'><input id='btnFamDataSave' value='Update' type='submit' class='NFButton' /></td>";
                    }
                    else
                    {
                        html += "<td colspan=3 style='text-align:right;'><input id='btnFamDataSave' value='Save' type='submit' class='NFButton' /></td>";
                    }

                }
            }

            else if (Index == 3)
            {
                if (IsClient.Count == 0)
                {
                    if (isUpated == 1)
                    {
                        html += "<td colspan= 3 style='text-align:right;'><input id='btnBirthDetSave' value='Update' type='submit' class='NFButton' /></td>";   //no rows on top
                    }
                    else
                    {
                        html += "<td colspan=3 style='text-align:right;'><input id='btnBirthDetSave' value='Save' type='submit' class='NFButton' /></td>";
                    }
                }
                else
                {
                    if (isUpated == 1)
                    {
                        html += "<td colspan= 3 style='text-align:right;'><input id='btnBirthDetSave' value='Update'  style='display:none;' type='submit' class='NFButton' /></td>";   //no rows on top
                    }
                    else
                    {
                        html += "<td colspan=3 style='text-align:right;'><input id='btnBirthDetSave' value='Save'  style='display:none;' type='submit' class='NFButton' /></td>";
                    }
                }
            }
            else if (Index == 4)
            {
                if (IsClient.Count == 0)
                {
                    if (isUpated == 1)
                    {
                        html += "<td colspan= 3 style='text-align:right;'><input id='btnPersnlHistrySave' value='Update' type='submit' class='NFButton' /></td>";
                    }
                    else
                    {
                        html += "<td colspan=3 style='text-align:right;'><input id='btnPersnlHistrySave' value='Save' type='submit'  class='NFButton' /></td>";
                    }

                }
                else
                {
                    if (isUpated == 1)
                    {
                        html += "<td colspan= 3 style='text-align:right;'><input id='btnPersnlHistrySave' value='Update'   style='display:none;' type='submit' class='NFButton' /></td>";
                    }
                    else
                    {
                        html += "<td colspan=3 style='text-align:right;'><input id='btnPersnlHistrySave' value='Save'   style='display:none;' type='submit'  class='NFButton' /></td>";
                    }
                }
            }
            else if (Index == 5)
            {
                if (IsClient.Count == 0)
                {
                    if (isUpated == 1)
                    {
                        html += "<td colspan= 3 style='text-align:right;'><input id='btnRecretionSave' value='Update' type='submit' class='NFButton' /></td>";
                    }
                    else
                    {
                        html += "<td colspan=3 style='text-align:right;'><input id='btnRecretionSave' value='Save' type='submit' class='NFButton' /></td>";
                    }
                }
                else
                {
                    if (isUpated == 1)
                    {
                        html += "<td colspan= 3 style='text-align:right;'><input id='btnRecretionSave'  style='display:none;' value='Update' type='submit' class='NFButton' /></td>";
                    }
                    else
                    {
                        html += "<td colspan=3 style='text-align:right;'><input id='btnRecretionSave'  style='display:none;' value='Save' type='submit' class='NFButton' /></td>";
                    }

                }
            }
            else if (Index == 6)
            {
                if (IsClient.Count == 0)
                {
                    if (isUpated == 1)
                    {
                        html += "<td colspan= 3 style='text-align:right;'><input id='btnPresentSkillsSave' value='Update' type='submit' class='NFButton' /></td>";
                    }
                    else
                    {
                        html += "<td colspan=3 style='text-align:right;'><input id='btnPresentSkillsSave' value='Save' type='submit' class='NFButton' /></td>";
                    }
                }
                else
                {
                    if (isUpated == 1)
                    {
                        html += "<td colspan= 3 style='text-align:right;'><input id='btnPresentSkillsSave' style='display:none;'  value='Update' type='submit' class='NFButton' /></td>";
                    }
                    else
                    {
                        html += "<td colspan=3 style='text-align:right;'><input id='btnPresentSkillsSave' style='display:none;'  value='Save' type='submit' class='NFButton' /></td>";
                    }
                }
            }
            else if (Index == 7)
            {
                if (IsClient.Count == 0)
                {
                    if (isUpated == 1)
                    {
                        //html += "<td colspan= 3 style='width: "+widthCell+"%; text-align:right;'><input id='btnFundDataSave' value='Update' type='submit' class='NFButton' /></td>";
                        html += "<td style='text-align:right;'><a id='lnk' href='#' style='display:none;'  onclick='ShowDocuments()'  >View Items</a></td> <td style='text-align:right;'>" +
                            "<input id='btnFundDataSave' value='Save' type='submit' class='NFButton' /></td>";
                    }
                    else
                    {
                        //html += "<td colspan=3 style='width: "+widthCell+"%; text-align:right;'><input id='btnFundDataSave' value='Save' type='submit' class='NFButton' /></td>";
                        html += "<td style='text-align:right;'><a id='lnk' href='#' style='display:none;' onclick='ShowDocuments()' >View Items</a></td> <td style='text-align:right;'>" +
                        "<input id='btnFundDataSave' value='Save' type='submit' class='NFButton' /></td>";
                    }
                }
                else
                {
                    if (isUpated == 1)
                    {
                        //html += "<td colspan= 3 style='width: "+widthCell+"%; text-align:right;'><input id='btnFundDataSave' value='Update' type='submit' class='NFButton' /></td>";
                        html += "<td style='text-align:right;'><a id='lnk' href='#' style='display:none;'  onclick='ShowDocuments()'  >View Items</a></td> <td style='text-align:right;'>" +
                            "<input id='btnFundDataSave' value='Save' type='submit' style='display:none;'  class='NFButton' /></td>";
                    }
                    else
                    {
                        //html += "<td colspan=3 style='width: "+widthCell+"%; text-align:right;'><input id='btnFundDataSave' value='Save' type='submit' class='NFButton' /></td>";
                        html += "<td style='text-align:right;'><a id='lnk' href='#' style='display:none;' onclick='ShowDocuments()' >View Items</a></td> <td style='text-align:right;'>" +
                        "<input id='btnFundDataSave' value='Save' type='submit' class='NFButton' style='display:none;'  /></td>";
                    }
                }
            }
            else if (Index == 8)
            {
                if (IsClient.Count == 0)
                {
                    if (isUpated == 1)
                    {
                        html += "<td style='text-align:right;'><a id='lnk' href='#' style='display:none;'  onclick='ShowDocuments()'  >View Items</a></td> <td style='text-align:right;'><input id='btnSaveDoccuments' value='Save' type='submit' class='NFButton' /></td>";
                        //html += "<td colspan= 3 style='width: "+widthCell+"%; text-align:right;'><input id='btnSaveDoccuments' value='Save' type='submit' class='NFButton' /></td>";

                    }
                    else
                    {
                        html += "<td style='text-align:right;'><a id='lnk' href='#' style='display:none;' onclick='ShowDocuments()' >View Items</a></td> <td style='text-align:right;'><input id='btnSaveDoccuments' value='Save' type='submit' class='NFButton' /></td>";
                        //html += "<td colspan=3 style='width: "+widthCell+"%; text-align:right;'><input id='btnSaveDoccuments' value='Save' type='submit' class='NFButton' /></td>";

                    }
                }
                else
                {
                    if (isUpated == 1)
                    {
                        html += "<td style='text-align:right;'><a id='lnk' href='#' style='display:none;'  onclick='ShowDocuments()'  >View Items</a></td> <td style='text-align:right;'><input id='btnSaveDoccuments' value='Save'  style='display:none;' type='submit' class='NFButton' /></td>";
                        //html += "<td colspan= 3 style='width: "+widthCell+"%; text-align:right;'><input id='btnSaveDoccuments' value='Save' type='submit' class='NFButton' /></td>";

                    }
                    else
                    {
                        html += "<td style='text-align:right;'><a id='lnk' href='#' style='display:none;' onclick='ShowDocuments()' >View Items</a></td> <td style='text-align:right;'><input id='btnSaveDoccuments' value='Save'  style='display:none;' type='submit' class='NFButton' /></td>";
                        //html += "<td colspan=3 style='width: "+widthCell+"%; text-align:right;'><input id='btnSaveDoccuments' value='Save' type='submit' class='NFButton' /></td>";

                    }
                }
            }


            html += "<tr><td><input id='hidSec" + Index + "' class='tabcls' value='" + Index + "' name='TabId' type='hidden'/></td></tr>";
            html += "</table></div></div></form>";
            genModel.html = html;

            //}
            return html;

            //return View("GeneralInfoData", genModel);

        }



    }
}
