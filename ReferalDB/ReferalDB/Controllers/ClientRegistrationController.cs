using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReferalDB.AppFunctions;
using ReferalDB.Models;
using DataLayer;

namespace ReferalDB.Controllers
{
    public class ClientRegistrationController : Controller
    {
        //
        // GET: /ClientRegistration/
        Other_Functions objFuns = new Other_Functions();
        public clsSession1 sess = null;
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult Index(int Param = 0)
        {
            //clsSessionCon obj = new clsSessionCon();
            //sess = (ClsSession)obj.getSessionObject();
            //if (sess == null)
            //{
            //    Response.Redirect("Error.aspx?Error=Your session has expired. Please log-in again");
            //}
            sess = (clsSession1)Session["UserSession1"];
            if (sess == null)
            {


            }
            else
            {
                ////(HttpContext.Current.Session["StudentId"]
                //Session["StudentId"] = Param;
                //Session["SchoolId"] = 1;
                sess = (clsSession1)Session["UserSession1"];
                sess.StudentId = Param;
            }
            ViewBag.Param = Param;
            ViewBag.Usename = sess.UserName;
            return View();
        }
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult ClientRegistration(RegistrationModel model = null, string data = null)
        {
            //errorLog errlog = new errorLog();
            string templastname = "", temprace = "", tempcitizenship = "";
            //  errlog.WriteToLog("clientregistation");
            sess = (clsSession1)Session["UserSession1"];
            // sess.ClientId = Param;
            ViewBag.Usename = sess.UserName;
            if (data == null)
                data = "0|*";

            string[] Param = data.Split('|');
            model = new RegistrationModel();
            Other_Functions objFuns = new Other_Functions();
            if (sess != null)
            {
                ViewBag.permission = objFuns.setPermission();
                if (Convert.ToInt32(Param[0]) > 0 || sess.StudentId > 0)
                {
                    model = objFuns.bindCliendData(sess.StudentId);
                }
            }
            IList<SelectListItem> x = new List<SelectListItem>();
            x.Add(new SelectListItem { Text = "--Select--", Value = "0" });
            x.Add(new SelectListItem { Text = "Jr.", Value = "1" });
            x.Add(new SelectListItem { Text = "Sr.", Value = "2" });
            model.LastNameSuffixList = x;

            x = new List<SelectListItem>();
            x.Add(new SelectListItem { Text = "--Select--", Value = "0" });
            x.Add(new SelectListItem { Text = "Male", Value = "1" });
            x.Add(new SelectListItem { Text = "Female", Value = "2" });
            model.GenderList = x;

            model.CountryList = objFuns.getCountryList();
            model.CountryOfBirthList = objFuns.getCountryList();

            model.StateOfBirthList = objFuns.getStateList(model.CountryofBirth);

            model.StateList = objFuns.getStateList(model.Country);




            //x = new List<SelectListItem>();
            //x.Add(new SelectListItem { Text = "--Select--", Value = "0" });
            //x.Add(new SelectListItem { Text = "American Indian or Alaska Native", Value = "998" });
            //x.Add(new SelectListItem { Text = "Asian", Value = "999" });
            //x.Add(new SelectListItem { Text = "Black or African American", Value = "1000" });
            //x.Add(new SelectListItem { Text = "Native Hawaiian or Other Pacific Islander", Value = "1001" });
            //x.Add(new SelectListItem { Text = "White", Value = "1002" });
            model.RaceList = objFuns.getRaceList();


            x = new List<SelectListItem>();
            x.Add(new SelectListItem { Text = "--Select--", Value = "0" });
            x.Add(new SelectListItem { Text = "Dual national", Value = "1014" });
            x.Add(new SelectListItem { Text = "Non-resident alien", Value = "1015" });
            x.Add(new SelectListItem { Text = "Resident alien", Value = "1016" });
            x.Add(new SelectListItem { Text = "United States Citizen", Value = "1017" });
            x.Add(new SelectListItem { Text = "Other", Value = "9999" });
            model.CitizenshipList = x;


            if (Convert.ToInt32(Param[0]) > 0 && Param[1] == "Fill")
            {
                if (model.LastNameSuffix != null)
                {
                    templastname = model.LastNameSuffix.ToString();
                    if (templastname != "0")
                        model.LastNameSuffix = model.LastNameSuffixList.Where(objTempList => objTempList.Value == templastname).Select(objTempList => objTempList.Text).First();
                    else
                        model.LastNameSuffix = "";
                }
                temprace = model.Race.ToString();
                if (temprace != "0")
                    try
                    {
                        model.StrRace = model.RaceList.Where(objTempList => objTempList.Value == temprace).Select(objTempList => objTempList.Text).First();
                    }
                    catch { }
                else
                    model.StrRace = "";
                tempcitizenship = model.Citizenship.ToString();
                if (tempcitizenship != "0")
                    try
                    {
                        model.CitizenshipBirth = model.CitizenshipList.Where(objTempList => objTempList.Value == tempcitizenship).Select(objTempList => objTempList.Text).First();
                    }
                    catch { }
                else
                    model.CitizenshipBirth = "";
                return View("ClientRegistrationView", model);

            }
            else
            {

                return View(model);
            }
            // getCountries();

        }

        [HttpPost]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public string SaveClients(RegistrationModel model, HttpPostedFileBase profilePicture)
        {
            sess = (clsSession1)Session["UserSession1"];
            string result = "";
            result = objFuns.SaveData(model, profilePicture);
            if (result == "Failed")
            {
                TempData["notice"] = "Failed To Insert Data";
                ViewBag.id = sess.StudentId;
                //return RedirectToAction("ClientRegistration");
                return result + "|" + sess.StudentId;
            }
            else
            {
                ViewBag.id = sess.StudentId;
                return result + "|" + sess.StudentId;
                //string data = sess.ClientId + "|Fill";
                //return RedirectToAction("ClientRegistration",data);
            }


        }

        public ActionResult getCountries()
        {

            objFuns = new Other_Functions();
            var Areas = objFuns.getCountryList();
            return Json(Areas, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getStates(int? countryid)
        {

            objFuns = new Other_Functions();
            var Areas = objFuns.getStateList(countryid);
            return Json(Areas, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ImageUploadPanel(ImageUploader model = null)
        {

            return View(model);
        }

        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        [HttpPost]
        public ActionResult saveUploadDocument(HttpPostedFileBase file, ImageUploader model)
        {
            string result = "";
            result = objFuns.SaveImage(file, model);
            return Content(result);
        }

        public ActionResult fillCLientDetails(RegistrationModel model = null)
        {
            // int ClientId = 1;

            sess = (clsSession1)Session["UserSession1"];
            // sess.ClientId = ClientId;
            StudentPersonal student = new StudentPersonal();
            Other_Functions objFuns = new Other_Functions();
            if (sess != null)
            {
                ViewBag.permission = objFuns.setPermission();
                model = objFuns.bindCliendData(sess.StudentId);
            }
            IList<SelectListItem> x = new List<SelectListItem>();
            x.Add(new SelectListItem { Text = "--Select--", Value = "0" });
            x.Add(new SelectListItem { Text = "Jr.", Value = "1" });
            x.Add(new SelectListItem { Text = "Sr.", Value = "2" });
            model.LastNameSuffixList = x;

            x = new List<SelectListItem>();
            x.Add(new SelectListItem { Text = "--Select--", Value = "0" });
            x.Add(new SelectListItem { Text = "Male", Value = "1" });
            x.Add(new SelectListItem { Text = "Female", Value = "2" });
            model.GenderList = x;

            model.CountryList = objFuns.getCountryList();
            model.CountryOfBirthList = objFuns.getCountryList();

            model.StateOfBirthList = objFuns.getStateList(model.CountryofBirth);

            model.StateList = objFuns.getStateList(model.Country);




            //x = new List<SelectListItem>();
            //x.Add(new SelectListItem { Text = "--Select--", Value = "0" });
            //x.Add(new SelectListItem { Text = "American Indian or Alaska Native", Value = "0998" });
            //x.Add(new SelectListItem { Text = "Asian", Value = "0999" });
            //x.Add(new SelectListItem { Text = "Black or African American", Value = "1000" });
            //x.Add(new SelectListItem { Text = "Native Hawaiian or Other Pacific Islander", Value = "1001" });
            //x.Add(new SelectListItem { Text = "White", Value = "1002" });

            model.RaceList = objFuns.getRaceList();

            x = new List<SelectListItem>();
            x.Add(new SelectListItem { Text = "--Select--", Value = "0" });
            x.Add(new SelectListItem { Text = "Dual national", Value = "1014" });
            x.Add(new SelectListItem { Text = "Non-resident alien", Value = "1015" });
            x.Add(new SelectListItem { Text = "Resident alien", Value = "1016" });
            x.Add(new SelectListItem { Text = "United States Citizen", Value = "1017" });
            x.Add(new SelectListItem { Text = "Other", Value = "9999" });
            model.CitizenshipList = x;


            return View("ClientRegistration", model);
        }

        #region Referral Quick updates

        public ActionResult referralQuickUpdates() {
            return View();
        }

        #endregion


    }
}
