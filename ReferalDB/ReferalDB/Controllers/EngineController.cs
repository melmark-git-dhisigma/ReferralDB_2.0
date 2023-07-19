using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using ReferalDB.Models;
using BuisinessLayer;
using System.Data.Objects.SqlClient;
using ReferalDB.Controllers;
using ReferalDB.CommonClass;

namespace ReferalDB.Controllers
{
    public class EngineController : Controller
    {
        //
        // GET: /Engine/

        MelmarkDBEntities objData = null;
        LetterEngine LEobj = new LetterEngine();
        LetterEngineItem LEIobj = new LetterEngineItem();
        ref_ChecklistOther ChkObj = new ref_ChecklistOther();
        ref_ChecklistItemOther ChkitmObj = new ref_ChecklistItemOther();
        ref_Checklist Objchklist = new ref_Checklist();

        clsSession sess = null;
        clsGeneral Objgen = null;
        EngineViewModels ObjEng = null;

        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult LetterEngine()
        {
            sess = (clsSession)Session["UserSession"];
            EngineViewModels returnModel = new EngineViewModels();
            ClsCommon getCommon = new ClsCommon();
            if (sess != null)
            {
                ViewBag.permission = getCommon.setPermission();
                returnModel = EngineViewModels.BindLetterEngine(sess.SchoolId);
                returnModel.QueueList = FillDropQueueType();
            }
            return View("../Engine/LetterEngineView", returnModel);
        }

        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult ChecklistEngine()
        {
            sess = (clsSession)Session["UserSession"];
            EngineViewModels returnModel = new EngineViewModels();
            ClsCommon getCommon = new ClsCommon();
            if (sess != null)
            {
                ViewBag.permission = getCommon.setPermission();
                
                returnModel.QueueList = FillChecklistDropQueueType();
            }
            return View("../Engine/ChecklistEngine", returnModel);
        }

        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        [HttpPost, ValidateInput(false)]
        public string SaveLetterEngine(EngineViewModels objEng)
        {

            string result = "";
            try
            {

                objData = new MelmarkDBEntities();
                sess = (clsSession)Session["UserSession"];

                if (sess != null)
                {
                    if (objEng.ApproveStatusVal == null)
                    {
                        objEng.ApproveStatusVal = objEng.ApproveSt;
                    }

                    if (objEng.ApproveStatusVal == 1)
                        objEng.ApproveStatus = true;
                    else if(objEng.ApproveStatusVal == 0)
                        objEng.ApproveStatus = false;
                    if (objEng.LetterEngineId == 0)
                    {
                        ////restrict multiple insertion of Letter Template of same Queue/////
                        objEng.listItem = (EngineViewModels.BindLetterEngine(sess.SchoolId)).listItem;
                        foreach (var item in objEng.listItem)
                        {
                            if (item.QueueId == objEng.QueueTypeId && item.ApproveStatus ==objEng.ApproveStatus)
                                return result = "<div class='error_box'>Letter for this Template Type already available...</div>";
                        }

                        //////Save Letter Engine//////
                        LEobj.LetterEngineName = objEng.LetterEngineName;
                        //LEobj.QueueId = Convert.ToInt32(collection["DdlType"]);
                        //if (objEng.QueueTypeId == 7)
                        //{
                            LEobj.ApproveStatus = objEng.ApproveStatus;
                        //}
                        //else if (objEng.QueueTypeId == 1)
                        //{
                        //    LEobj.ApproveStatus = true;
                        //}
                        //else
                        //{
                        //    LEobj.ApproveStatus = false;
                        //}
                        LEobj.QueueId = objEng.QueueTypeId;
                        LEobj.CreatedBy = sess.LoginId;
                        LEobj.CreatedOn = DateTime.Now;
                        LEobj.ModifiedBy = sess.LoginId;
                        LEobj.ModifiedOn = DateTime.Now;
                        LEobj.SchoolId = sess.SchoolId;
                        LEobj.LetterType = "Letter";
                        objData.LetterEngines.Add(LEobj);
                        objData.SaveChanges();

                        //////Save Letter Engine Items//////
                        LEIobj.LetterEngineId = LEobj.LetterEngineId;
                        LEIobj.CreatedBy = sess.LoginId;
                        LEIobj.CreatedOn = DateTime.Now;
                        LEIobj.ItemContent = objEng.ItemContent;
                        LEIobj.SchoolId = sess.SchoolId;
                        LEIobj.ModifiedBy = sess.LoginId;
                        LEIobj.ModifiedOn = DateTime.Now;
                        objData.LetterEngineItems.Add(LEIobj);
                        objData.SaveChanges();

                        result = "<div class='valid_box'>Letter Template Saved Successfully...</div>";

                    }
                    else
                    {
                        int LetterId = objEng.LetterEngineId;
                        LetterEngine le = (from x in objData.LetterEngines where x.LetterEngineId == LetterId select x).First();
                        le.LetterEngineName = objEng.LetterEngineName;
                        //le.QueueId = objEng.QueueTypeId;
                        le.ModifiedBy = sess.LoginId;
                        le.ModifiedOn = DateTime.Now;
                        objData.SaveChanges();

                        LetterEngineItem leitm = (from x in objData.LetterEngineItems where x.LetterEngineId == LetterId select x).First();
                        leitm.ItemContent = objEng.ItemContent;
                        leitm.ModifiedBy = sess.LoginId;
                        leitm.ModifiedOn = DateTime.Now;
                        objData.SaveChanges();

                        result = "<div class='valid_box'>Letter Template Updated Successfully...</div>";

                    }
                }
            }
            catch (Exception Ex)
            {
                result = "<div class='error_box'>Failed..." + Ex.Message + "</div>";

            }


            return result;
        }

        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public void Addnewtemptype(string LetterTempType)
        {
            String Name = LetterTempType;
            sess = (clsSession)Session["UserSession"];
            MelmarkDBEntities objData = new MelmarkDBEntities();
            objData = new MelmarkDBEntities();
            ref_Queue objrefq = new ref_Queue();
            objrefq.SchoolId = sess.SchoolId;
            objrefq.QueueName = Name;
            //objrefq.QueueType = Name; //Commented for setting quetype [23-Oct-2020]
            objrefq.QueueType = "OTH";
            objrefq.MasterId = 10;
            objrefq.PrevQueueId = 0;
            objrefq.SortOrder = 0;
            objrefq.CreatedBy = sess.LoginId;
            objrefq.CreatedOn = DateTime.Now;
            objrefq.ModifiedBy = sess.LoginId;
            objrefq.ModifiedOn = DateTime.Now;
            objData.ref_Queue.Add(objrefq);
            objData.SaveChanges();
        }
        
        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult AdminView()
        {
            sess = (clsSession)Session["UserSession"];
            EngineViewModels returnModel = new EngineViewModels();
            ClsCommon getCommon = new ClsCommon();
            if (sess != null)
            {
                ViewBag.permission = getCommon.setPermission();
                returnModel = EngineViewModels.BindLetterEngine(sess.SchoolId);

            }
            return View("AdminView", returnModel);

        }

        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult DeleteLetter(int Id)
        {
            TempData["Letter"] = "";
            sess = (clsSession)Session["UserSession"];
            EngineViewModels returnModel = new EngineViewModels();
            try
            {
                if (sess != null)
                {


                    if (Id != 0)
                    {
                        objData = new MelmarkDBEntities();
                        var count = objData.ref_LetterTrayValues.Where(x => x.LetterId == Id).Count();
                        if (count > 0)
                        {
                            returnModel.LetterAssigneStatus= "Assigned";
                            TempData["Letter"] = "Deletion Not Possible";
                        }
                        else
                        {
                            var deleteletter = objData.LetterEngineItems.Where(x => x.LetterEngineId == Id).First();
                            objData.LetterEngineItems.Remove(deleteletter);
                            objData.SaveChanges();

                            var deleteTemplate = objData.LetterEngines.Where(x => x.LetterEngineId == Id).First();
                            objData.LetterEngines.Remove(deleteTemplate);
                            objData.SaveChanges();
                        }
                    }
                }
                returnModel = EngineViewModels.BindLetterEngine(sess.SchoolId);

            }
            catch (Exception ex)
            {
            }
            return View("../AdminView/AdminView", returnModel);
        }

        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public string DeleteChecklist(string Id)
        {
            sess = (clsSession)Session["UserSession"];
            EngineViewModels returnModel = new EngineViewModels();
            string[] Ids = Id.Split(',');
            string Result = "";
            try
            {
                if (sess != null)
                {

                    if (Convert.ToInt32(Ids[0]) != 0)
                    {
                        int checklistitm=Convert.ToInt32(Ids[0]);
                        objData = new MelmarkDBEntities();
                        var deletechecklist = objData.ref_Checklist.Where(x => x.ChecklistId == checklistitm).First();
                        objData.ref_Checklist.Remove(deletechecklist);
                        objData.SaveChanges();
                        Result = clsGeneral.sucessMsg("Checklist item deleted successfully");
                    }
                }
                
            }
            catch (Exception ex)
            {
            }
            return Result;
        }




        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public string DeleteChecklistHeader(string Id)
        {
            sess = (clsSession)Session["UserSession"];
            EngineViewModels returnModel = new EngineViewModels();
            string[] IDs = Id.Split(',');
            string Result = "";
            try
            {
                if (sess != null)
                {
                    if (Convert.ToInt32(IDs[0]) != 0)
                    {
                        int checkid=Convert.ToInt32(IDs[0]);
                        objData = new MelmarkDBEntities();

                        var checksubitem = objData.ref_Checklist.Where(x => x.ChecklistHeaderId == checkid).ToList();
                        foreach (var Item in checksubitem)
                        {
                            var deletechecksub = objData.ref_Checklist.Where(x => x.ChecklistId == Item.ChecklistId).First();
                            objData.ref_Checklist.Remove(deletechecksub);
                            objData.SaveChanges();
                        }
                        var deletecheckheader = objData.ref_Checklist.Where(x => x.ChecklistId == checkid).First();
                        objData.ref_Checklist.Remove(deletecheckheader);
                        objData.SaveChanges();
                        Result = clsGeneral.sucessMsg("Checklist header deleted successfully");
                    }
                }
               
            }
            catch (Exception ex)
            {
            }

            return Result;
        }

        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult fillLetter(int Id)
        {
             ClsCommon getCommon = new ClsCommon();
             
                 ViewBag.permission = getCommon.setPermission();
             
            objData = new MelmarkDBEntities();
            ObjEng = new EngineViewModels();
            var templatecontent = objData.LetterEngineItems.Where(x => x.LetterEngineId == Id).ToList();
            var templatename = objData.LetterEngines.Where(x => x.LetterEngineId == Id).ToList();
            ObjEng.LetterEngineName = templatename[0].LetterEngineName;
            ObjEng.ItemContent = templatecontent[0].ItemContent;
            ObjEng.LetterEngineId = Id;
            ObjEng.QueueTypeId = templatename[0].QueueId;
            ObjEng.ApproveStatus = (bool)templatename[0].ApproveStatus;
            if (ObjEng.ApproveStatus == true)
                ObjEng.ApproveStatusVal = 1;
            else if (ObjEng.ApproveStatus == false)
                ObjEng.ApproveStatusVal = 0;
            return View("../Engine/LetterEngineView", ObjEng);

        }
        //[HttpPost, ValidateInput(false)]

        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public string SaveChecklist(string Id)
        {
            string Result = "";
            try
            {
                ObjEng = new EngineViewModels();
                objData = new MelmarkDBEntities();
                sess = (clsSession)Session["UserSession"];
                

                if (sess != null)
                {
                    string[] Items = Id.Split(',');
                    int QueueId = Convert.ToInt32(Items[1]);
                    string checkname = Items[2];
                    var Checklist = objData.ref_Checklist.Where(ObjChecklist => ObjChecklist.ChecklistName == checkname).Count();
                        if (Checklist == 0)
                        {


                            //////Save Checklist Engine//////
                            Objchklist.ChecklistName = Items[2];
                            Objchklist.ChecklistHeaderId = 0;
                            Objchklist.CreatedBy = sess.LoginId;
                            Objchklist.CreatedOn = DateTime.Now;
                            Objchklist.ModifiedBy = sess.LoginId;
                            Objchklist.ModifiedOn=DateTime.Now;
                            Objchklist.QueueId = QueueId;
                            Objchklist.SchoolId = sess.SchoolId;
                            objData.ref_Checklist.Add(Objchklist);
                            objData.SaveChanges();

                            int checkid = Objchklist.ChecklistId;
                            Objchklist = new ref_Checklist();

                            //////Save Checklist Engine Items//////
                            Objchklist.ChecklistName = Items[0];
                            Objchklist.ChecklistHeaderId = checkid;
                            Objchklist.CreatedBy = sess.LoginId;
                            Objchklist.CreatedOn = DateTime.Now;
                            Objchklist.ModifiedBy = sess.LoginId;
                            Objchklist.ModifiedOn = DateTime.Now;
                            Objchklist.QueueId = QueueId;
                            Objchklist.SchoolId = sess.SchoolId;
                            objData.ref_Checklist.Add(Objchklist);
                            objData.SaveChanges();
                            ModelState.Clear();
                            Result = clsGeneral.sucessMsg("Checklist Saved Successfully...");
                        }
                        else
                        {
                            Result = clsGeneral.warningMsg("Checklist header already exist...");
                        }
                   
                }

            }
            catch (Exception Ex)
            {
                Result = clsGeneral.failedMsg("Failed..." + Ex.Message);
            }
            
            return Result;
        }

        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult FillChecklistItem()
        {
            EngineViewModels returnModel = new EngineViewModels();
            sess = (clsSession)Session["UserSession"];
            ClsCommon getCommon = new ClsCommon();
            if (sess != null)
            {
                ViewBag.permission = getCommon.setPermission();
                returnModel = EngineViewModels.LoadChecklistEngineItem(sess.SchoolId);
            }
            return View("../Engine/ChecklistItem", returnModel);
        }


        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public string SaveChecklistitem(string Id)
        {
            string Result = "";
            EngineViewModels ObjEng = new EngineViewModels();
            try
            {
                string[] dataitem = Id.Split(',');
                objData = new MelmarkDBEntities();
                int chkheaderid = Convert.ToInt32(dataitem[0]);
                sess = (clsSession)Session["UserSession"];
                if (sess != null)
                {

                    var queueid = objData.ref_Checklist.Where(objchk => objchk.ChecklistId == chkheaderid).First();

                    //////Save checklist Engine Items//////
                    Objchklist.ChecklistName = dataitem[1];
                    Objchklist.ChecklistHeaderId = chkheaderid;
                    Objchklist.CreatedBy = sess.LoginId;
                    Objchklist.CreatedOn = DateTime.Now;
                    Objchklist.ModifiedBy = sess.LoginId;
                    Objchklist.ModifiedOn = DateTime.Now;
                    Objchklist.QueueId = queueid.QueueId;
                    Objchklist.SchoolId = sess.SchoolId;
                    objData.ref_Checklist.Add(Objchklist);
                    objData.SaveChanges();
                    ModelState.Clear();
                    Result = "<div class='valid_box'>Checklist item Saved Successfully...</div>";                    
                }

            }
            catch (Exception Ex)
            {
                Result = "<div class='error_box'>Failed..." + Ex.Message + "</div>";
            }

            return Result;

        }


        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public string SaveChecklistOtheritem(string Id)
        {
            string Result = "";
            EngineViewModels ObjEng = new EngineViewModels();
            try
            {
                string[] dataitem = Id.Split(',');
                objData = new MelmarkDBEntities();
                sess = (clsSession)Session["UserSession"];
                if (sess != null)
                {

                    //////Save other checklist Items//////
                    ChkitmObj.ChecklistOtherId = Convert.ToInt32(dataitem[0]);
                    ChkitmObj.CreatedBy = sess.LoginId;
                    ChkitmObj.CreatedOn = DateTime.Now;
                    ChkitmObj.ItemContent = dataitem[1];
                    ChkitmObj.SchoolId = sess.SchoolId;
                    ChkitmObj.ModifiedBy = sess.LoginId;
                    ChkitmObj.ModifiedOn = DateTime.Now;
                    objData.ref_ChecklistItemOther.Add(ChkitmObj);
                    objData.SaveChanges();

                    ModelState.Clear();
                    Result = "<div class='valid_box'>Checklist item Saved Successfully...</div>";
                }

            }
            catch (Exception Ex)
            {
                Result = "<div class='error_box'>Failed..." + Ex.Message + "</div>";
            }

            return Result;

        }

        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public List<QueueStatus> FillDropQueueType()
        {
            objData = new MelmarkDBEntities();
            List<QueueStatus> QueueList = new List<QueueStatus>();

            QueueList = (from objqueue in objData.ref_Queue
                         where objqueue.MasterId != 0 && (objqueue.QueueType != "RT" && objqueue.QueueType != "MO" &&
                         objqueue.QueueType != "SM" && objqueue.QueueType != "IP" && objqueue.QueueType != "PM" && objqueue.QueueType != "CM" && objqueue.QueueType != "CT" && objqueue.QueueType != "SA" && objqueue.QueueType != "PI" && objqueue.QueueType != "RS" && objqueue.QueueType != "DC" && objqueue.QueueType != "PT")                     //&& objqueue.QueueType != "NA"
                         select new QueueStatus
                         {
                             QueueName = objqueue.QueueName,
                             QueueId = objqueue.QueueId

                         }).ToList();
            //QueueList.Add(new QueueStatus
            //{
            //    QueueId=0,
            //    QueueName="Others"

            //});
            return QueueList;
        }

        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public List<QueueStatus> FillChecklistDropQueueType()
        {
            objData = new MelmarkDBEntities();
            List<QueueStatus> QueueList = new List<QueueStatus>();

            QueueList = (from objqueue in objData.ref_Queue
                         where objqueue.MasterId != 0  && (objqueue.QueueType != "NA" && objqueue.QueueType != "RT" && objqueue.QueueType != "MO" && objqueue.QueueType != "AT" &&
                         objqueue.QueueType != "SM" && objqueue.QueueType != "IP" && objqueue.QueueType != "PM" && objqueue.QueueType != "CM" && objqueue.QueueType != "CT" && objqueue.QueueType != "OTH")// && objqueue.QueueType != "CR" && objqueue.QueueType != "AR" && objqueue.QueueType != "CA")                   //&& objqueue.QueueType != "NA"
                         select new QueueStatus
                         {
                             QueueName = objqueue.QueueName,
                             QueueId = objqueue.QueueId

                         }).ToList();
            //QueueList.Add(new QueueStatus
            //{
            //    QueueId=0,
            //    QueueName="Others"

            //});
            return QueueList;
        }


        //[ActiveSession]
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

        
    }
}
