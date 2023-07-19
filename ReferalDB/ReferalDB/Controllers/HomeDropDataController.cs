using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using ReferalDB.Models;
using BuisinessLayer;
using ReferalDB.CommonClass;
using ReferalDBApplicant.Classes;
namespace ReferalDB.Controllers
{
    public class DetailsController : Controller
    {
        //
        // GET: /HomeDropData/
        public clsSession sess = null;
        public static string SetLookUpCode = "USA";
        public ActionResult Index()
        {
            return View();
        }

        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult CheckListData(HomeDropDataViewModel Model = null)
        {
            ClsCommon clsComn = new ClsCommon();
            string paraVal = "ChecklistUser";
            Model.CheckDetails = clsComn.GetActiveReferalNdUser(paraVal);
            return View("ChecklistDetails", Model);
        }

        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult ReferralData(HomeDropDataViewModel Model)
        {
            ClsCommon clsComn = new ClsCommon();
            MelmarkDBEntities objData = new MelmarkDBEntities();
            IList<ActivityModelClass> ActModel = new List<ActivityModelClass>();
            ReferralDashboardModel ObjRef = new ReferralDashboardModel();

            sess = (clsSession)Session["UserSession"];
            if (sess != null)
            {
                ActModel = (from x in objData.StudentPersonals
                            join y in objData.ref_QueueStatus
                            on x.StudentPersonalId equals y.StudentPersonalId
                            join z in objData.ref_Queue
                            on y.QueueId equals z.QueueId
                            join a in objData.Users
                            on y.CreatedBy equals a.UserId
                            where x.StudentType == "Referral" && x.SchoolId == sess.SchoolId && x.StudentPersonalId == sess.ReferralId
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
                                ModifiedOn = y.ModifiedOn,
                                SType = x.StudentType
                            }).ToList();
            }

            Model.RefDetailsMore = ActModel;

            return View("ReferalsDetails", Model);
        }

        //[ActiveSession]
        //[OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        //public ActionResult ReferralData(HomeDropDataViewModel Model)
        //{
        //    ClsCommon clsComn = new ClsCommon();
        //    string paraVal = "ActiveRefReferral";
        //    Model.RefDetails = clsComn.GetActiveReferalNdUser(paraVal);
        //    return View("ReferalsDetails", Model);
        //}

        //[ActiveSession]

        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public int GetStudentID()
        {
            sess = (clsSession)Session["UserSession"];
            int ID = 0;
            if (sess != null)
            {
                ID = sess.ReferralId;
            }
            return ID;
        }
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult StudentDetails(StdDetailsViewModel Model)
        {
            ClsCommon clsComn = new ClsCommon();
            Model = clsComn.getStdDetails();
            Model.Search = "NO";
            return View("RefDetails", Model);
        }
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult StudentDetailsSelect(StdDetailsViewModel Model, int ReferralId = 0)
        {
            ClsCommon clsComn = new ClsCommon();
            sess = (clsSession)Session["UserSession"];
            if (ReferralId != 0&&sess!=null)
                sess.ReferralId = ReferralId;
            Model = clsComn.getStdDetails();
            //Model.Search = "YES";
            return View("RefDetails", Model);
        }

        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult StudentDetailsSelect_quickUpdate(StdDetailsViewModel Model, int ReferralId = 0)
        {
            ClsCommon clsComn = new ClsCommon();
            sess = (clsSession)Session["UserSession"];
            //if (ReferralId != 0)
            //    sess.ReferralId = ReferralId;
            //else
            sess.ReferralId = ReferralId;

            ReferalDB.AppFunctions.Other_Functions OF = new ReferalDB.AppFunctions.Other_Functions();
            ViewBag.permission = OF.setPermission();

            Model = clsComn.getStdDetails();
            if (ReferralId > 0)
                Model.CallLists = Model.getCallLog();
            return View("RefDetails_quickUpdate", Model);
        }

        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult StudentDetailsSelect_refMode(StdDetailsViewModel Model, int ReferralId = 0)
        {
            ClsCommon clsComn = new ClsCommon();
            sess = (clsSession)Session["UserSession"];
            if (ReferralId != 0)
                sess.ReferralId = ReferralId;
            Model = clsComn.getStdDetails();
            //Model.Search = "YES";
            return View("RefDetails_refMode", Model);
        }

        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult StudentSearchDetails(StdDetailsViewModel Model, string SearchTag = "", int page = 1, int pageSize = 10)
        {
            string type = "";
            ClsCommon clsComn = new ClsCommon();
            if (SearchTag != "undefined")
            {
                string[] types = SearchTag.Split('$');
                if (types.Length > 1)
                {
                    if (types[1] == "Referral")
                    {
                        ViewBag.type = "1";
                    }
                    else
                        ViewBag.type = "2";
                }
            }
            IList<StudentSearchDetails> val = clsComn.GetStudentSearch(SearchTag, page, pageSize);
            ViewBag.SearchDetails = val;

            Model.Search = "YES";


            return View("RefDetails", Model);
        }
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public string StudentStatus()
        {
            sess = (clsSession)Session["UserSession"];
            ClsCommon clsComn = new ClsCommon();
            string Status = "";
            int ProcessId = clsComn.getProcessId();
            int Id = clsComn.getQueueIdCurrent(ProcessId, sess.ReferralId);
            if (Id > 25) Status = "Expired";
            return Status;
        }

        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult QuickUpdateSave(StdDetailsViewModel Model)
        {
            sess = (clsSession)Session["UserSession"];
            AddressList addr = new AddressList();
            StudentAddresRel adrRel = new StudentAddresRel();
            MelmarkDBEntities objData = new MelmarkDBEntities();
            StudentPersonal SPObj = new StudentPersonal();
            StudentPersonalPA SPPAObj = new StudentPersonalPA();
            DiaganosesPA DiaObj = new DiaganosesPA();
            ClsCommon updateCommon = new ClsCommon();
            LetterTray clsletter = new LetterTray();
            clsReferral clsRf = new clsReferral();
            byte[] xmlData = null;
            DateTime dtbirthdate = new DateTime();
            DateTime dtAdmission = new DateTime();
            DateTime dtLetter = new DateTime();

            if (Model.BirthDate != null)
            {
                dtbirthdate = DateTime.ParseExact(Model.BirthDate, "MM'/'dd'/'yyyy", System.Globalization.CultureInfo.CurrentCulture);
            }
            if (Model.AdmissionDate != null)
            {
                dtAdmission = DateTime.ParseExact(Model.AdmissionDate, "MM'/'dd'/'yyyy", System.Globalization.CultureInfo.CurrentCulture);
            }
            if (Model.ApplicationDate != null)
            {
                //string getLtrDat = Model.ApplicationDate.ToString("MM'/'dd'/'yyyy");
                dtLetter = DateTime.ParseExact(Model.ApplicationDate, "MM'/'dd'/'yyyy", System.Globalization.CultureInfo.CurrentCulture);
            }
            else 
            {
                string getLtrDat = DateTime.Now.ToString("MM'/'dd'/'yyyy");
                dtLetter = DateTime.ParseExact(getLtrDat, "MM'/'dd'/'yyyy", System.Globalization.CultureInfo.CurrentCulture);
            }
            if (sess.ReferralId == 0)
            {
                string Localid = "";
                var Cnt = objData.StudentPersonals.Count();
                if (Cnt > 0)
                {
                    var maxid = objData.StudentPersonals.Max(x => x.StudentPersonalId);
                    Localid = Convert.ToString(maxid + 1);
                }
                else
                {
                    Localid = "1";
                }
                SPObj.FirstName = Model.Firstname;
                SPObj.LastName = Model.Lastname;
                SPObj.BirthDate = dtbirthdate;
                SPObj.Gender = Model.GenderNum;
                SPObj.CreatedBy = sess.LoginId;
                SPObj.CreatedOn = dtLetter; //DateTime.Now;
                SPObj.SchoolId = sess.SchoolId;
                SPObj.AdmissionDate = dtAdmission;
                SPObj.StudentType = "Referral";
                SPObj.PrimaryDiag = Model.Diagnosis;
                SPObj.LocalId = "STD" + Localid;
                SPObj.NewApplication = true;
                SPObj.FundingVerification = Model.fl_FA;
                SPObj.FundingSource = Model.FundingSourceId; //--- 22Sep2020 - List 3 - Task #2 ---//
                objData.StudentPersonals.Add(SPObj);
                objData.SaveChanges();

                SPPAObj.StudentPersonalId = SPObj.StudentPersonalId;
                SPPAObj.SchoolId = sess.SchoolId;
                SPPAObj.CreatedBy = sess.LoginId;
                SPPAObj.CreatedOn = DateTime.Now;
                objData.StudentPersonalPAs.Add(SPPAObj);
                objData.SaveChanges();

                sess.ReferralId = SPObj.StudentPersonalId;
                Model.StudentPersonalId = SPObj.StudentPersonalId;

                updateCommon.insertQstatus("NA", "N");
                updateCommon.insertQstatus("AR", "Y");                

                int addressType = objData.LookUps.Where(objLookup => objLookup.LookupType == "Address Type" && objLookup.LookupName == "Physical location address").Select(objlookup => objlookup.LookupId).Single();

                //AddrList.CountryId = model.RefCountry;//DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Address Type" && objLookup.LookupName == "Physical location address").Select(objlookup => objlookup.LookupId).Single();
                //addr.CountryId = objData.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single();
                addr.CountryId = objData.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupCode == SetLookUpCode).Select(objlookup => objlookup.LookupId).Single();
                addr.StreetName = Model.Street;
                addr.ApartmentType = Model.Apartment;
                addr.City = Model.City;
                addr.StateProvince = Model.State;
                addr.PostalCode = Model.ZipCode;
                addr.CreatedBy = sess.LoginId;
                addr.CreatedOn = DateTime.Now;
                objData.AddressLists.Add(addr);
                objData.SaveChanges();

                int addressId = addr.AddressId;

                adrRel.StudentPersonalId = Model.StudentPersonalId;
                adrRel.AddressId = addressId;
                adrRel.ContactSequence = 0;
                adrRel.ContactPersonalId = 0;
                adrRel.CreatedBy = sess.LoginId;
                adrRel.CreatedOn = DateTime.Now;
                objData.StudentAddresRels.Add(adrRel);
                objData.SaveChanges();

                DiaObj.StudentPersonalId = sess.ReferralId;
                DiaObj.Diaganoses = Model.Diagnosis;
                DiaObj.CreatedBy = sess.LoginId;
                DiaObj.CreatedOn = DateTime.Now;
                objData.DiaganosesPAs.Add(DiaObj);
                objData.SaveChanges();
                string schoolType = System.Web.Configuration.WebConfigurationManager.AppSettings["Server"].ToString();
                 string xmlPath="";
                    if (schoolType == "NE")
                           xmlPath = Server.MapPath("../XML/NewRefInfoNE.xml");
                   else
                      xmlPath = Server.MapPath("../XML/NewRefInfoPA.xml");
                

                xmlData = clsRf.SaveAsBlob(xmlPath);   //Initial full xml doccument saving to database
                clsRf.SaveXmlToDB(xmlData, sess.ReferralId);

                int Qid = updateCommon.getQueueId("NA");
                var LetterName = objData.LetterEngines.Where(x => x.QueueId == Qid && x.ApproveStatus == true && x.SchoolId == sess.SchoolId && x.LetterType == "Letter").ToList();
                if (LetterName.Count > 0)
                {
                    clsletter.insertLetter("NA", sess.ReferralId, LetterName[0].LetterEngineId, "Parent");
                }

            }
            else
            { 
                SPObj = objData.StudentPersonals.Where(x => x.StudentPersonalId == sess.ReferralId).SingleOrDefault();
                adrRel = objData.StudentAddresRels.Where(x => x.StudentPersonalId == sess.ReferralId && x.ContactSequence == 0).SingleOrDefault();
                if (adrRel != null)
                {
                addr = objData.AddressLists.Where(x => x.AddressId == adrRel.AddressId).SingleOrDefault();
                }
                DiaObj = objData.DiaganosesPAs.Where(x => x.StudentPersonalId == sess.ReferralId).First();
                SPObj.FirstName = Model.Firstname;
                SPObj.LastName = Model.Lastname;
                SPObj.BirthDate = dtbirthdate;
                SPObj.Gender = Model.GenderNum;
                SPObj.AdmissionDate = dtAdmission;
                SPObj.PrimaryDiag = Model.Diagnosis;
                SPObj.FundingVerification = Model.fl_FA;
                SPObj.FundingSource = Model.FundingSourceId; //--- 22Sep2020 - List 3 - Task #2 ---//
                SPObj.CreatedOn = dtLetter; //DateTime.Now;
                objData.SaveChanges();


                addr.StreetName = Model.Street;
                addr.ApartmentType = Model.Apartment;
                addr.City = Model.City;
                addr.StateProvince = Model.State;
                addr.PostalCode = Model.ZipCode;
                objData.SaveChanges();

                if (DiaObj == null)
                {
                    DiaganosesPA DiaObj1 = new DiaganosesPA();
                    DiaObj1.StudentPersonalId = sess.ReferralId;
                    DiaObj1.Diaganoses = Model.Diagnosis;
                    DiaObj1.CreatedBy = sess.LoginId;
                    DiaObj1.CreatedOn = DateTime.Now;
                    objData.DiaganosesPAs.Add(DiaObj1);
                    objData.SaveChanges();
                }
                else
                {
                    DiaObj.Diaganoses = Model.Diagnosis;
                    objData.SaveChanges();
                }

                
            }
            return Content("Success*" + sess.ReferralId); ;
        }

        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult UpdateAdmission(StdDetailsViewModel Model)
        {
            string test = "ab";
            sess = (clsSession)Session["UserSession"];
            clsReferral clsRf = new clsReferral();
            byte[] xmlData = null;
            string resStat = "";

            try
            {
                string xmlPath = "";
                string schoolType = System.Web.Configuration.WebConfigurationManager.AppSettings["Server"].ToString();
                if (schoolType == "NE")
                    xmlPath = Server.MapPath("../XML/NewRefInfoNE.xml");
                else
                    xmlPath = Server.MapPath("../XML/NewRefInfoPA.xml");
                xmlData = clsRf.SaveAsBlob(xmlPath);   //Initial full xml doccument saving to database
                clsRf.SaveXmlToDB(xmlData, sess.ReferralId);
                resStat = "Success";
            }
            catch (Exception exp)
            {
                resStat = "Failed" + exp.ToString();
            }

            return Content(resStat+"*" + sess.ReferralId);
        }
    }
}
