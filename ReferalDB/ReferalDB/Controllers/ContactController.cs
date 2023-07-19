using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReferalDB.Models;
using ReferalDB.AppFunctions;
using DataLayer;

namespace ClientDB.Controllers
{
    public class ContactController : Controller
    {
        //
        // GET: /Contact/
        Other_Functions objFuns = new Other_Functions();
        public clsSession sess = null;
        public static string SetLookUpCode = "USA";
        public ActionResult Index()
        {
            sess = (clsSession)Session["UserSession"];

            ReferalDB.AppFunctions.Other_Functions OF = new ReferalDB.AppFunctions.Other_Functions();
            ViewBag.permission = OF.setPermission();

            //sess.StudentId = 1;
            //Session["UserSession"] = sess;
            return RedirectToAction("Contact");


        }
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult Contact(ContactModel model = null)
        {
            // ContactModel ContactMod = new ContactModel();
            sess = (clsSession)Session["UserSession"];
            IList<checkBoxViewModel> checkmode = new List<checkBoxViewModel>
            {
                new checkBoxViewModel(){name="Emergency",check=false},
                new checkBoxViewModel(){name="Incedent",check=false},
                new checkBoxViewModel(){name="Mail",check=false},
               
            };

            IList<SelectListItem> x = new List<SelectListItem>();
            x.Add(new SelectListItem { Text = "--Select--", Value = "0" });
            x.Add(new SelectListItem { Text = "Mr.", Value = "1" });
            x.Add(new SelectListItem { Text = "Mrs.", Value = "2" });
            x.Add(new SelectListItem { Text = "Ms.", Value = "3" });
            x.Add(new SelectListItem { Text = "Dr.", Value = "4" });
            model.FirstNamePrefixList = x;

            x = new List<SelectListItem>();
            x.Add(new SelectListItem { Text = "--Select--", Value = "0" });
            x.Add(new SelectListItem { Text = "Jr.", Value = "1" });
            x.Add(new SelectListItem { Text = "Sr.", Value = "2" });
            if (sess != null)
            {
                ViewBag.permission = objFuns.setPermission();

                model.LastNameSuffixList = x;

                model.RelationList = objFuns.getRelationshipList();
                model.HomeAddressTypeList = objFuns.getAddressTypes();
                model.WorkAddressTypeList = objFuns.getAddressTypes();
                model.OtherAddressTypeList = objFuns.getAddressTypes();
                int countyID = objFuns.getCountriesId();

                x = new List<SelectListItem>();
                x.Add(new SelectListItem { Text = "United States of America", Value = "1" });
                model.HomeCountryList = x;

                x = new List<SelectListItem>();
                x.Add(new SelectListItem { Text = "United States of America", Value = "1" });
                model.WorkCountryList = x;

                x = new List<SelectListItem>();
                x.Add(new SelectListItem { Text = "United States of America", Value = "1" });
                model.OtherCountryList = x;

                //model.HomeCountryList = objFuns.getCountryList();
                //model.WorkCountryList = objFuns.getCountryList();
                //model.OtherCountryList = objFuns.getCountryList();
                model.HomeStateList = objFuns.getStateList(countyID);
                model.WorkStateList = objFuns.getStateList(countyID);
                model.OtherStateList = objFuns.getStateList(countyID);
                model.checkbox = checkmode;
            }
            return View(model);
        }
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult SaveContacts(ContactModel model)
        {
            string result = "";
            result = objFuns.SaveContactData(model);
            if (result == "No Client Selected")
            {
                TempData["notice"] = "No Client Selected";
            }
            return RedirectToAction("ListContactVendor");
        }
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult fillContactDetails(int id)
        {
            //int ClientId = 1;
            ContactModel model;
            Other_Functions objFuns = new Other_Functions();
            MelmarkDBEntities dbobj = new MelmarkDBEntities();

            sess = (clsSession)Session["UserSession"];
            if (sess != null)
            {
                ViewBag.permission = objFuns.setPermission();
            }
            //sess.ClientId = ClientId;

            model = objFuns.bindContactData(sess.ReferralId, id);



            IList<checkBoxViewModel> checkmode = new List<checkBoxViewModel>
            {
                new checkBoxViewModel(){name="Emergency",check=false},
                new checkBoxViewModel(){name="Incedent",check=false},
                new checkBoxViewModel(){name="Mail",check=false},
               
            };

            IList<SelectListItem> x = new List<SelectListItem>();
            x.Add(new SelectListItem { Text = "--Select--", Value = "0" });
            x.Add(new SelectListItem { Text = "Mr.", Value = "1" });
            x.Add(new SelectListItem { Text = "Mrs.", Value = "2" });
            x.Add(new SelectListItem { Text = "Ms.", Value = "3" });
            x.Add(new SelectListItem { Text = "Dr.", Value = "4" });
            model.FirstNamePrefixList = x;

            x = new List<SelectListItem>();
            x.Add(new SelectListItem { Text = "--Select--", Value = "0" });
            x.Add(new SelectListItem { Text = "Jr.", Value = "1" });
            x.Add(new SelectListItem { Text = "Sr.", Value = "2" });

            model.LastNameSuffixList = x;

            model.RelationList = objFuns.getRelationshipList();
            model.HomeAddressTypeList = objFuns.getAddressTypes();
            model.WorkAddressTypeList = objFuns.getAddressTypes();
            model.OtherAddressTypeList = objFuns.getAddressTypes();
            model.HomeCountryList = objFuns.getCountryList();
            model.WorkCountryList = objFuns.getCountryList();
            model.OtherCountryList = objFuns.getCountryList();
           
            if (model.WorkCountry == 0 || model.OtherCountry == 0|| model.HomeCountry==0)
            {
                int countryId = dbobj.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupCode == SetLookUpCode).Select(objLookup => objLookup.LookupId).Single();
                model.WorkCountry = countryId;
                model.OtherCountry = countryId;
                model.HomeCountry = countryId;
            }
            model.HomeStateList = objFuns.getStateList(model.HomeCountry);
            model.WorkStateList = objFuns.getStateList(model.WorkCountry);
            model.OtherStateList = objFuns.getStateList(model.OtherCountry);
            model.checkbox = checkmode;

            return View("Contact", model);

        }
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult DeleteContactDetails(int id)
        {
            // int ClientId = 1;
            sess = (clsSession)Session["UserSession"];
            //sess.ClientId = ClientId;
            objFuns.deleteContact(sess.ReferralId, id);
            return Content("Success");
        }


        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult ImageUploadPanel(string edit = null)
        {
            sess = (clsSession)Session["UserSession"];
            string temp = "";
            ViewBag.imageUrl = "";
            ViewBag.StudentId = "";
            ViewBag.StudentName = "";
            ViewBag.ModelId = sess.ReferralId;
            ViewBag.SchoolId = sess.SchoolId;
            ImageModel imgModel = new ImageModel();
            if (sess != null)
            {
                imgModel = objFuns.bindImage(sess.ReferralId);
                if ((imgModel.ImageUrl != null) && (imgModel.StudentId != null))
                {
                    ViewBag.imageUrl = imgModel.ImageUrl;
                    ViewBag.StudentId = "Student Id : " + imgModel.StudentId;

                    temp = imgModel.LastName + "," + imgModel.FirstName;
                    int length = temp.Length;
                    ViewBag.StudentName = temp;
                    if (temp.Length > 18)
                    {
                        temp = temp.Remove(18, length - 18);
                        ViewBag.StudentName = temp + "...";
                    }
                }
            }

            if (edit == "0")
            {
                ViewBag.editButton = "none";
            }
            else
            {
                ViewBag.editButton = "block";
            }
            return View();
        }

        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult ListContactVendor(int page = 1, int pageSize = 5)
        {
            sess = (clsSession)Session["UserSession"];
            ContactSearch search = new ContactSearch();
            ListContactModel bindModel = new ListContactModel();

            ReferalDB.AppFunctions.Other_Functions OF = new ReferalDB.AppFunctions.Other_Functions();
            ViewBag.permission = OF.setPermission();

            //search.SearchArgument = argument;
            //search.SortStatus = bSort;
            //search.PagingArgument = Data;
            //ViewBag.curval = "";
            //ViewBag.flage = "";
            //ViewBag.SearchArg = "";
            //ViewBag.itemCount = 0;
            //if ((bSort == false) && (argument != "*"))
            //{
            //    ViewBag.SearchArg = argument;
            //}

            //ViewBag.SortArg = argument;
            Other_Functions objFuns = new Other_Functions();
            if (sess != null)
            {
                ViewBag.permission = objFuns.setPermission();
                bindModel = ListContactModel.fillContacts(page, pageSize);
            }
            return View(bindModel);
        }
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult UpdateContactDetails(ContactModel model)
        {

            string result = "";
            // result = objFuns.UpdateData(model);
            return Content(result);
        }



        public JsonResult searchParentFirstName(string term)
        {
            MelmarkDBEntities RPCobj = new MelmarkDBEntities();
            sess = (clsSession)Session["UserSession"];
            IList<contactFirstName> retunmodel = new List<contactFirstName>();


            retunmodel = (from objDocuments in RPCobj.Parents
                          where objDocuments.Username != null
                          select new contactFirstName
                          {
                              username = objDocuments.Username,
                          }).Distinct().ToList();



            var result = (from r in retunmodel
                          where r.username.ToLower() == (term.ToLower())
                          select new { r.username }).Distinct();


            var resultcount = (from obj in result
                               select new contactFirstName
                               {
                                   Usercount = result.Count(),
                               }).Distinct().ToList();


            return Json(resultcount, JsonRequestBehavior.AllowGet);


        }

    }
}
