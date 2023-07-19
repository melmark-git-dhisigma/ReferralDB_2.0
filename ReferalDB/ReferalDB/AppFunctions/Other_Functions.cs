using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer;

using System.Web.Mvc;
//using ReferalDB.DbModel;
using ReferalDB.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Reflection;
using System.Transactions;
using System.Data.Entity.Validation;
using System.Diagnostics;
using ReferalDB.AppFunctions;

namespace ReferalDB.AppFunctions
{
    public class Other_Functions
    {

        MelmarkDBEntities dbobj = null;
        clsSession sess = null;
        GlobalData MetaData = null;
        public static string SetLookUpCode = "USA";

        /// <summary>
        /// Function to Get Current Page Name
        /// </summary>
        /// <returns></returns>
        public static string getPageName()
        {
            string PageUrl = System.Web.HttpContext.Current.Request.Url.AbsoluteUri;
            string[] words = PageUrl.Split('/');
            int len = words.Length;
            return words[len - 1].ToString();
        }

        /// <summary>
        /// Function To list States respective to the Country
        /// </summary>
        /// <param name="id"></param>
        /// <param name="flagOverid"></param>
        /// <returns></returns>
        public IEnumerable<SelectListItem> getStateList(int? id = 0, int flagOverid = 0)
        {
            dbobj = new MelmarkDBEntities();
            IList<LookUp> Statedata = new List<LookUp>();
            SelectListItem onesele = new SelectListItem { Text = "-- Select --", Value = "" };
            IList<SelectListItem> stateSelecteditem = new List<SelectListItem>();
            stateSelecteditem.Add(onesele);
            try
            {
                Statedata = dbobj.LookUps.Where(x => x.LookupType == "State" && x.ParentLookupId == id).ToList();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }
            var stateSelecteditemsub = (from State in Statedata select new SelectListItem { Text = State.LookupName, Value = State.LookupId.ToString() }).ToList();
            foreach (SelectListItem sele in stateSelecteditemsub)
            {
                stateSelecteditem.Add(sele);
            }
            return stateSelecteditem;
        }
        public IEnumerable<SelectListItem> getCountries()
        {
            MelmarkDBEntities RPCobj = new MelmarkDBEntities();
            IList<SelectListItem> countriesitems = new List<SelectListItem>();
            SelectListItem onesele = new SelectListItem { Text = "-- Select --", Value = "" };
            countriesitems.Add(onesele);
            var countries = RPCobj.LookUps.Where(x => x.LookupType == "Country").ToList();
            countriesitems = (from country in countries select new SelectListItem { Text = country.LookupName, Value = country.LookupId.ToString() }).ToList();
            return countriesitems;
        }
        private IEnumerable<SelectListItem> getStates(int id)
        {
            IList<SelectListItem> stateItems = new List<SelectListItem>();
            MelmarkDBEntities RPCobj = new MelmarkDBEntities();
            var states = RPCobj.LookUps.Where(x => x.LookupType == "State" && x.ParentLookupId == id).ToList();
            stateItems = (from state in states select new SelectListItem { Text = state.LookupName, Value = state.LookupId.ToString() }).ToList();
            return stateItems;
        }
        public int getCountriesId()
        {
            MelmarkDBEntities RPCobj = new MelmarkDBEntities();
            var countries = RPCobj.LookUps.Where(x => x.LookupType == "Country" && x.LookupCode == SetLookUpCode).SingleOrDefault();

            return countries.LookupId;
        }
        public IEnumerable<SelectListItem> getRaceList()
        {
            dbobj = new MelmarkDBEntities();
            IList<LookUp> Racedata = new List<LookUp>();
            SelectListItem onesele = new SelectListItem { Text = "-- Select --", Value = "" };
            IList<SelectListItem> raceSelecteditem = new List<SelectListItem>();
            raceSelecteditem.Add(onesele);
            Racedata = dbobj.LookUps.Where(x => x.LookupType == "Race").ToList();
            var raceSelecteditemsub = (from race in Racedata select new SelectListItem { Text = race.LookupName, Value = race.LookupId.ToString() }).ToList();
            foreach (SelectListItem sele in raceSelecteditemsub)
            {
                raceSelecteditem.Add(sele);
            }
            return raceSelecteditem;
        }
        public IEnumerable<SelectListItem> getCountryList()
        {
            dbobj = new MelmarkDBEntities();
            IList<LookUp> Countrydata = new List<LookUp>();
            SelectListItem onesele = new SelectListItem { Text = "-- Select --", Value = "" };
            IList<SelectListItem> countrySelecteditem = new List<SelectListItem>();
            countrySelecteditem.Add(onesele);
            Countrydata = dbobj.LookUps.Where(x => x.LookupType == "Country").ToList();
            var countrySelecteditemsub = (from country in Countrydata select new SelectListItem { Text = country.LookupName, Value = country.LookupId.ToString() }).ToList();
            foreach (SelectListItem sele in countrySelecteditemsub)
            {
                countrySelecteditem.Add(sele);
            }
            return countrySelecteditem;
        }
        public IEnumerable<SelectListItem> getRelationshipList()
        {
            dbobj = new MelmarkDBEntities();
            IList<LookUp> RelationData = new List<LookUp>();
            SelectListItem onesele = new SelectListItem { Text = "-- Select --", Value = "" };
            IList<SelectListItem> relationSelecteditem = new List<SelectListItem>();
            relationSelecteditem.Add(onesele);
            RelationData = dbobj.LookUps.Where(x => x.LookupType == "Relationship" && x.ActiveInd=="A").OrderBy(x => x.LookupName).ToList();
            var relationSelecteditemsub = (from relation in RelationData select new SelectListItem { Text = relation.LookupName, Value = relation.LookupId.ToString() }).ToList();
            foreach (SelectListItem sele in relationSelecteditemsub)
            {
                relationSelecteditem.Add(sele);
            }
            return relationSelecteditem;
        }

        public IEnumerable<SelectListItem> getPhysicianList()
        {
            dbobj = new MelmarkDBEntities();
            IList<LookUp> PhysicianData = new List<LookUp>();
            SelectListItem onesele = new SelectListItem { Text = "-- Select --", Value = "" };
            IList<SelectListItem> physicianSelecteditem = new List<SelectListItem>();
            physicianSelecteditem.Add(onesele);
            PhysicianData = dbobj.LookUps.Where(x => x.LookupType == "Physician").OrderBy(x => x.LookupName).ToList();
            var physicianSelecteditemsub = (from physician in PhysicianData select new SelectListItem { Text = physician.LookupName, Value = physician.LookupId.ToString() }).ToList();
            foreach (SelectListItem sele in physicianSelecteditemsub)
            {
                physicianSelecteditem.Add(sele);
            }
            return physicianSelecteditem;
        }
        public IEnumerable<SelectListItem> getAddressTypes()
        {
            dbobj = new MelmarkDBEntities();
            IList<LookUp> AddressData = new List<LookUp>();
            SelectListItem onesele = new SelectListItem { Text = "-- Select --", Value = "" };
            IList<SelectListItem> addressSelecteditem = new List<SelectListItem>();
            addressSelecteditem.Add(onesele);
            AddressData = dbobj.LookUps.Where(x => x.LookupType == "Address Type").ToList();
            var addressSelecteditemsub = (from address in AddressData select new SelectListItem { Text = address.LookupName, Value = address.LookupId.ToString() }).ToList();
            foreach (SelectListItem sele in addressSelecteditemsub)
            {
                addressSelecteditem.Add(sele);
            }
            return addressSelecteditem;
        }
        public IEnumerable<SelectListItem> getEventStatus()
        {
            dbobj = new MelmarkDBEntities();
            IList<LookUp> EventStatusData = new List<LookUp>();
            SelectListItem onesele = new SelectListItem { Text = "-- Select --", Value = "" };
            IList<SelectListItem> eventStatusSelecteditem = new List<SelectListItem>();
            eventStatusSelecteditem.Add(onesele);
            EventStatusData = dbobj.LookUps.Where(objLookUp => objLookUp.LookupType == "Visitation Status").ToList();
            var eventStatusSelecteditemsub = (from eventType in EventStatusData select new SelectListItem { Text = eventType.LookupName, Value = eventType.LookupId.ToString() }).ToList();
            foreach (SelectListItem sele in eventStatusSelecteditemsub)
            {
                eventStatusSelecteditem.Add(sele);
            }
            return eventStatusSelecteditem;
        }
        public IEnumerable<SelectListItem> getEventType()
        {
            dbobj = new MelmarkDBEntities();
            IList<LookUp> EventTypeData = new List<LookUp>();
            SelectListItem onesele = new SelectListItem { Text = "-- Select --", Value = "" };
            IList<SelectListItem> eventTypeSelecteditem = new List<SelectListItem>();
            eventTypeSelecteditem.Add(onesele);
            EventTypeData = dbobj.LookUps.Where(objLookUp => objLookUp.LookupType == "Visitation Type").ToList();
            var eventSelecteditemsub = (from eventType in EventTypeData select new SelectListItem { Text = eventType.LookupName, Value = eventType.LookupId.ToString() }).ToList();
            foreach (SelectListItem sele in eventSelecteditemsub)
            {
                eventTypeSelecteditem.Add(sele);
            }
            return eventTypeSelecteditem;
        }

        public IEnumerable<SelectListItem> getDocumentType()
        {
            dbobj = new MelmarkDBEntities();
            IList<LookUp> DocumentTypeData = new List<LookUp>();
            SelectListItem onesele = new SelectListItem { Text = "-- Select --", Value = "" };
            IList<SelectListItem> documentTypeSelecteditem = new List<SelectListItem>();
            documentTypeSelecteditem.Add(onesele);
            DocumentTypeData = dbobj.LookUps.Where(objLookUp => objLookUp.LookupType == "Document Type").ToList();
            var documentSelecteditemsub = (from documentType in DocumentTypeData select new SelectListItem { Text = documentType.LookupName, Value = documentType.LookupId.ToString() }).ToList();
            foreach (SelectListItem sele in documentSelecteditemsub)
            {
                documentTypeSelecteditem.Add(sele);
            }
            return documentTypeSelecteditem;
        }

        public IEnumerable<SelectListItem> getType(string type)
        {
            dbobj = new MelmarkDBEntities();
            IList<LookUp> PlacementTypeData = new List<LookUp>();
            SelectListItem onesele = new SelectListItem { Text = "-- Select --", Value = "" };
            IList<SelectListItem> placementTypeSelecteditem = new List<SelectListItem>();
            placementTypeSelecteditem.Add(onesele);
            PlacementTypeData = dbobj.LookUps.Where(objLookUp => objLookUp.LookupType == type).ToList();
            var placementSelecteditemsub = (from placementType in PlacementTypeData select new SelectListItem { Text = placementType.LookupName, Value = placementType.LookupId.ToString() }).ToList();
            foreach (SelectListItem sele in placementSelecteditemsub)
            {
                placementTypeSelecteditem.Add(sele);
            }
            return placementTypeSelecteditem;
        }


        public string getCountry(int countreyID)
        {
            dbobj = new MelmarkDBEntities();
            string country = dbobj.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupId == countreyID).Select(objLookup => objLookup.LookupName).Single();
            return country;
        }
        public string getState(int stateID)
        {
            string state = "";
            try
            {
                dbobj = new MelmarkDBEntities();
                state = dbobj.LookUps.Where(objLookup => objLookup.LookupType == "State" && objLookup.LookupId == stateID).Select(objLookup => objLookup.LookupName).Single();
            }
            catch (Exception eX)
            {

            }
            return state;
        }

        /// <summary>
        /// Function To Get Dates to bind medical details calender
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        //public string CalanderDatas(int id)
        //{
        //    dbobj = new MelmarkDBEntities();
        //    MedicalAndInsurance tblObjmedical = new MedicalAndInsurance();
        //    MedicalModel Medicalmodel = new MedicalModel();
        //    string retunData = "";
        //    try
        //    {
        //        var Medicals = dbobj.MedicalAndInsurances.Where(objMedical => objMedical.StudentPersonalId == id).ToList();
        //        foreach (var item in Medicals)
        //        {
        //            Medicalmodel.CalenderDatas = item.MedicalInsuranceId + "|" + ConvertDate(item.DateOfLastPhysicalExam) + "^";
        //            if (Medicalmodel.CalenderDatas != null)
        //            {
        //                retunData += Medicalmodel.CalenderDatas.ToString();
        //            }
        //        }
        //    }
        //    catch (DbEntityValidationException dbEx)
        //    {
        //        foreach (var validationErrors in dbEx.EntityValidationErrors)
        //        {
        //            foreach (var validationError in validationErrors.ValidationErrors)
        //            {
        //                Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
        //            }
        //        }
        //    }


        //    return retunData;

        //}




        /// <summary>
        /// Function to Save / Update Client Information.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="sourceFile"></param>
        /// <returns></returns>

        public string SaveData(RegistrationModel model, HttpPostedFileBase sourceFile)
        {
            sess = (clsSession)HttpContext.Current.Session["UserSession"];

            string Result = "";
            dbobj = new MelmarkDBEntities();
            StudentPersonal sp = new StudentPersonal();
            AddressList addr = new AddressList();
            StudentAddresRel adrRel = new StudentAddresRel();
            Insurance objInsurance = new Insurance();
            EmergencyContactSchool objEmergency = new EmergencyContactSchool();
            SchoolsAttended objSchool = new SchoolsAttended();
            MetaData = new GlobalData();
            //  string dirpath = (AppDomain.CurrentDomain.BaseDirectory + "Images/StudentImages/".ToString()).Replace('\\', '/');
            string dirpath = System.Web.Configuration.WebConfigurationManager.AppSettings["ImagessLocation"].ToString();
            //ImagessLocation
            //  int ClientID = 1;

            try
            {
                using (TransactionScope trans = new TransactionScope(TransactionScopeOption.Required))
                {
                    if (sess.ReferralId == 0 && sess.AddressId == 0)
                    {

                        sp.SchoolId = sess.SchoolId;
                        sp.FirstName = model.FirstName;
                        sp.LastName = model.LastName;
                        sp.MiddleName = model.MiddleName;
                        sp.Suffix = model.LastNameSuffix;
                        sp.NickName = model.NickName;
                        sp.BirthDate = DateTime.ParseExact(model.DateOfBirth, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        sp.PlaceOfBirth = model.PlaceOfBirth;
                        sp.CountryOfBirth = model.CountryofBirth;
                        sp.StateOfBirth = model.StateOfBirth;
                        sp.CitizenshipStatus = model.Citizenship;
                        sp.RaceId = model.Race;
                        sp.Gender = model.Gender;
                        sp.HairColor = model.HairColor;
                        sp.EyeColor = model.EyeColor;
                        try
                        {
                            sp.AdmissionDate = DateTime.ParseExact(model.AdmissinDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            sp.AdmissionDate = null;
                        }
                        sp.Height = Convert.ToDecimal(model.Height);
                        sp.Weight = Convert.ToDecimal(model.Weight);
                        sp.PrimaryLanguage = model.PrimaryLanguage;

                        sp.GuardianShip = model.GuardianshipStatus;
                        sp.DistingushingMarks = model.DistigushingMarks;
                        sp.LegalCompetencyStatus = model.LegalCompetencyStatus;
                        sp.OtherStateAgenciesInvolvedWithStudent = model.OtherStateAgenciesInvolvedWithStudent;
                        sp.MaritalStatusofBothParents = model.MaritalStatusofBothParents;
                        sp.CaseManagerEducational = model.CaseManagerEducational;
                        sp.CaseManagerResidential = model.CaseManagerResidential;
                        if (sourceFile != null)
                        {
                            sp.ImageUrl = sourceFile.FileName;
                        }
                        else
                        {

                            dirpath = System.Web.Configuration.WebConfigurationManager.AppSettings["ImagessLocation"].ToString();
                            dirpath = (AppDomain.CurrentDomain.BaseDirectory + dirpath);
                            FileInfo fileInfo = new FileInfo(dirpath);
                            byte[] data = new byte[fileInfo.Length];


                            using (FileStream fs = fileInfo.OpenRead())
                            {
                                fs.Read(data, 0, data.Length);
                                //  sp.ImageUrl = fs.ToString();
                            }
                            sp.ImageUrl = Convert.ToBase64String(data);
                            //int id = objRef.StudentUpldPhoto(StudentPersnlId, data);


                        }
                        sp.ImagePermission = model.PhotoReleasePermission;
                        try
                        {
                            sp.DateInitiallyEligibleforSpecialEducation = DateTime.ParseExact(model.DateInitiallyEligibleforSpecialEducation, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            sp.DateInitiallyEligibleforSpecialEducation = null;
                        }
                        try
                        {
                            sp.DateofMostRecentSpecialEducationEvaluations = DateTime.ParseExact(model.DateofMostRecentSpecialEducationEvaluations, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            sp.DateofMostRecentSpecialEducationEvaluations = null;
                        }
                        try
                        {
                            sp.DateofNextScheduled3YearEvaluation = DateTime.ParseExact(model.DateofNextScheduled3YearEvaluation, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            sp.DateofNextScheduled3YearEvaluation = null;
                        }
                        try
                        {
                            sp.CurrentIEPStartDate = DateTime.ParseExact(model.CurrentIEPStartDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            sp.CurrentIEPStartDate = null;
                        }
                        try
                        {
                            sp.CurrentIEPExpirationDate = DateTime.ParseExact(model.CurrentIEPExpirationDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            sp.CurrentIEPExpirationDate = null;
                        }
                        try
                        {
                            sp.DischargeDate = DateTime.ParseExact(model.DischargeDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            sp.DischargeDate = null;
                        }
                        sp.LocationAfterDischarge = model.LocationAfterDischarge;
                        sp.StudentType = "Client";
                        sp.MelmarkNewEnglandsFollowUpResponsibilities = model.MelmarkNewEnglandsFollowUpResponsibilities;
                        sp.CreatedBy = sess.LoginId;
                        sp.CreatedOn = DateTime.Now;
                        int maxID = 0;
                        try
                        {
                            maxID = dbobj.StudentPersonals.Max(p => p.StudentPersonalId);
                            if (maxID == null || maxID < 0)
                                maxID = 0;
                            maxID++;
                        }
                        catch
                        {
                            maxID = 1;
                        }
                        sp.LocalId = "STD" + maxID;
                        sp.IEPReferralFullName = model.ReferralIEPFullName;
                        sp.IEPReferralPhone = model.ReferralIEPPhone;
                        sp.IEPReferralReferrinAgency = model.ReferralIEPReferringAgency;
                        sp.IEPReferralSourceofTuition = model.ReferralIEPSourceofTuition;
                        sp.IEPReferralTitle = model.ReferralIEPTitle;
                        dbobj.StudentPersonals.Add(sp);
                        dbobj.SaveChanges();
                        sess.ReferralId = sp.StudentPersonalId;
                        if (sourceFile != null)
                        {

                            byte[] fileBytes = new byte[sourceFile.ContentLength];
                            int byteCount = sourceFile.InputStream.Read(fileBytes, 0, (int)sourceFile.ContentLength);

                            sp.ImageUrl = Convert.ToBase64String(fileBytes);
                            // model.ImageUrl = dirpath + "/" + sp.StudentPersonalId + "-" + sourceFile.FileName;



                        }
                        else
                        {

                            FileInfo fileInfo = new FileInfo(dirpath);
                            byte[] data = new byte[fileInfo.Length];
                            //      sp.ImageUrl = Convert.ToBase64String(data);
                            using (FileStream fs = fileInfo.OpenRead())
                            {
                                fs.Read(data, 0, data.Length);
                                //sp.ImageUrl = fs.ToString();
                            }
                            sp.ImageUrl = Convert.ToBase64String(data);


                        }
                        addr.ApartmentType = model.AddressLine1;
                        addr.StreetName = model.AddressLine2;
                        addr.AddressType = MetaData._StudentAddressType;
                        addr.AddressLine3 = model.AddressLine3;
                        addr.City = model.City;
                        addr.CountryId = model.Country;
                        addr.StateProvince = model.State;
                        addr.PostalCode = model.ZipCode;
                        addr.CreatedBy = sess.LoginId;
                        addr.CreatedOn = DateTime.Now;
                        dbobj.AddressLists.Add(addr);
                        dbobj.SaveChanges();

                        adrRel.AddressId = addr.AddressId;
                        adrRel.StudentPersonalId = sp.StudentPersonalId;
                        adrRel.ContactSequence = 0;
                        adrRel.CreatedBy = sess.LoginId;
                        adrRel.CreatedOn = DateTime.Now;
                        dbobj.StudentAddresRels.Add(adrRel);
                        dbobj.SaveChanges();

                        objEmergency.StudentPersonalId = sp.StudentPersonalId;
                        objEmergency.SchoolId = sess.SchoolId;
                        objEmergency.FirstName = model.EmergencyContactFirstName1;
                        objEmergency.LastName = model.EmergencyContactLastName1;
                        objEmergency.Title = model.EmergencyContactTitle1;
                        objEmergency.Phone = model.EmergencyContactPhone1;
                        objEmergency.CreatedBy = sess.LoginId;
                        objEmergency.CreatedOn = DateTime.Now;
                        objEmergency.SequenceId = 1;
                        dbobj.EmergencyContactSchools.Add(objEmergency);
                        dbobj.SaveChanges();

                        objEmergency = new EmergencyContactSchool();
                        objEmergency.StudentPersonalId = sp.StudentPersonalId;
                        objEmergency.SchoolId = sess.SchoolId;
                        objEmergency.FirstName = model.EmergencyContactFirstName2;
                        objEmergency.LastName = model.EmergencyContactLastName2;
                        objEmergency.Title = model.EmergencyContactTitle2;
                        objEmergency.Phone = model.EmergencyContactPhone2;
                        objEmergency.CreatedBy = sess.LoginId;
                        objEmergency.CreatedOn = DateTime.Now;
                        objEmergency.SequenceId = 2;
                        dbobj.EmergencyContactSchools.Add(objEmergency);
                        dbobj.SaveChanges();

                        objEmergency = new EmergencyContactSchool();
                        objEmergency.StudentPersonalId = sp.StudentPersonalId;
                        objEmergency.SchoolId = sess.SchoolId;
                        objEmergency.FirstName = model.EmergencyContactFirstName3;
                        objEmergency.LastName = model.EmergencyContactLastName3;
                        objEmergency.Title = model.EmergencyContactTitle3;
                        objEmergency.Phone = model.EmergencyContactPhone3;
                        objEmergency.CreatedBy = sess.LoginId;
                        objEmergency.CreatedOn = DateTime.Now;
                        objEmergency.SequenceId = 3;
                        dbobj.EmergencyContactSchools.Add(objEmergency);
                        dbobj.SaveChanges();

                        objEmergency = new EmergencyContactSchool();
                        objEmergency.StudentPersonalId = sp.StudentPersonalId;
                        objEmergency.SchoolId = sess.SchoolId;
                        objEmergency.FirstName = model.EmergencyContactFirstName4;
                        objEmergency.LastName = model.EmergencyContactLastName4;
                        objEmergency.Title = model.EmergencyContactTitle4;
                        objEmergency.Phone = model.EmergencyContactPhone4;
                        objEmergency.CreatedBy = sess.LoginId;
                        objEmergency.CreatedOn = DateTime.Now;
                        objEmergency.SequenceId = 4;
                        dbobj.EmergencyContactSchools.Add(objEmergency);
                        dbobj.SaveChanges();

                        objEmergency = new EmergencyContactSchool();
                        objEmergency.StudentPersonalId = sp.StudentPersonalId;
                        objEmergency.SchoolId = sess.SchoolId;
                        objEmergency.FirstName = model.EmergencyContactFirstName5;
                        objEmergency.LastName = model.EmergencyContactLastName5;
                        objEmergency.Title = model.EmergencyContactTitle5;
                        objEmergency.Phone = model.EmergencyContactPhone5;
                        objEmergency.CreatedBy = sess.LoginId;
                        objEmergency.CreatedOn = DateTime.Now;
                        objEmergency.SequenceId = 5;
                        dbobj.EmergencyContactSchools.Add(objEmergency);
                        dbobj.SaveChanges();

                        objSchool.StudentPersonalId = sp.StudentPersonalId;
                        objSchool.SchoolId = sess.SchoolId;
                        objSchool.SchoolName = model.SchoolName1;
                        try
                        {
                            objSchool.DateFrom = DateTime.ParseExact(model.DateFrom1, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            objSchool.DateFrom = null;
                        }
                        try
                        {
                            objSchool.DateTo = DateTime.ParseExact(model.DateTo1, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            objSchool.DateTo = null;
                        }
                        objSchool.Address1 = model.SchoolAttendedAddress11;
                        objSchool.Address2 = model.SchoolAttendedAddress21;
                        objSchool.City = model.SchoolAttendedCity1;
                        objSchool.State = model.SchoolAttendedState1;
                        objSchool.CreatedBy = sess.LoginId;
                        objSchool.CreatedOn = DateTime.Now;
                        objSchool.SequenceId = 1;
                        dbobj.SchoolsAttendeds.Add(objSchool);
                        dbobj.SaveChanges();

                        objSchool = new SchoolsAttended();
                        objSchool.StudentPersonalId = sp.StudentPersonalId;
                        objSchool.SchoolId = sess.SchoolId;
                        objSchool.SchoolName = model.SchoolName2;
                        try
                        {
                            objSchool.DateFrom = DateTime.ParseExact(model.DateFrom1, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            objSchool.DateFrom = null;
                        }
                        try
                        {
                            objSchool.DateTo = DateTime.ParseExact(model.DateTo1, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            objSchool.DateTo = null;
                        }
                        objSchool.Address1 = model.SchoolAttendedAddress12;
                        objSchool.Address2 = model.SchoolAttendedAddress22;
                        objSchool.City = model.SchoolAttendedCity2;
                        objSchool.State = model.SchoolAttendedState2;
                        objSchool.CreatedBy = sess.LoginId;
                        objSchool.CreatedOn = DateTime.Now;
                        objSchool.SequenceId = 2;
                        dbobj.SchoolsAttendeds.Add(objSchool);
                        dbobj.SaveChanges();

                        objSchool = new SchoolsAttended();
                        objSchool.StudentPersonalId = sp.StudentPersonalId;
                        objSchool.SchoolId = sess.SchoolId;
                        objSchool.SchoolName = model.SchoolName3;
                        try
                        {
                            objSchool.DateFrom = DateTime.ParseExact(model.DateFrom1, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            objSchool.DateFrom = null;
                        }
                        try
                        {
                            objSchool.DateTo = DateTime.ParseExact(model.DateTo1, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            objSchool.DateTo = null;
                        }
                        objSchool.Address1 = model.SchoolAttendedAddress13;
                        objSchool.Address2 = model.SchoolAttendedAddress23;
                        objSchool.City = model.SchoolAttendedCity3;
                        objSchool.State = model.SchoolAttendedState3;
                        objSchool.CreatedBy = sess.LoginId;
                        objSchool.CreatedOn = DateTime.Now;
                        objSchool.SequenceId = 3;
                        dbobj.SchoolsAttendeds.Add(objSchool);
                        dbobj.SaveChanges();

                        objInsurance.StudentPersonalId = sp.StudentPersonalId;
                        objInsurance.InsuranceType = model.InsuranceType;
                        objInsurance.PolicyHolder = model.PolicyHolder;
                        objInsurance.PolicyNumber = model.PolicyNumber;
                        objInsurance.PreferType = "Primary";
                        objInsurance.CreatedBy = sess.LoginId;
                        objInsurance.CreatedOn = DateTime.Now;
                        dbobj.Insurances.Add(objInsurance);
                        dbobj.SaveChanges();
                        Result = "Sucess";
                    }
                    else
                    {
                        sp = dbobj.StudentPersonals.Where(x => x.StudentPersonalId == sess.ReferralId).SingleOrDefault();
                        adrRel = dbobj.StudentAddresRels.Where(objAddressRel => objAddressRel.StudentPersonalId == sess.ReferralId && objAddressRel.ContactSequence == 0).SingleOrDefault();
                        sp.LocalId = "STD1002";
                        sp.FirstName = model.FirstName;
                        sp.LastName = model.LastName;
                        sp.MiddleName = model.MiddleName;
                        sp.Suffix = model.LastNameSuffix;
                        sp.NickName = model.NickName;
                        sp.BirthDate = DateTime.ParseExact(model.DateOfBirth, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        sp.PlaceOfBirth = model.PlaceOfBirth;
                        sp.CountryOfBirth = model.CountryofBirth;
                        sp.StateOfBirth = model.StateOfBirth;
                        sp.CitizenshipStatus = model.Citizenship;
                        sp.RaceId = model.Race;
                        sp.Gender = model.Gender;
                        sp.HairColor = model.HairColor;
                        sp.EyeColor = model.EyeColor;
                        try
                        {
                            sp.AdmissionDate = DateTime.ParseExact(model.AdmissinDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            sp.AdmissionDate = null;
                        }
                        sp.Height = Convert.ToDecimal(model.Height);
                        sp.Weight = Convert.ToDecimal(model.Weight);
                        sp.PrimaryLanguage = model.PrimaryLanguage;
                        sp.DistingushingMarks = model.DistigushingMarks;
                        sp.LegalCompetencyStatus = model.LegalCompetencyStatus;
                        sp.OtherStateAgenciesInvolvedWithStudent = model.OtherStateAgenciesInvolvedWithStudent;
                        sp.MaritalStatusofBothParents = model.MaritalStatusofBothParents;
                        sp.CaseManagerEducational = model.CaseManagerEducational;
                        sp.CaseManagerResidential = model.CaseManagerResidential;
                        if (sourceFile != null)
                        {
                            byte[] fileBytes = new byte[sourceFile.ContentLength];
                            int byteCount = sourceFile.InputStream.Read(fileBytes, 0, (int)sourceFile.ContentLength);

                            sp.ImageUrl = Convert.ToBase64String(fileBytes);
                            //sp.ImageUrl = sourceFile.FileName;
                        }

                        sp.ImagePermission = model.PhotoReleasePermission;
                        sp.GuardianShip = model.GuardianshipStatus;
                        //sp.DateInitiallyEligibleforSpecialEducation = DateTime.ParseExact(model.DateInitiallyEligibleforSpecialEducation, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        //sp.DateofMostRecentSpecialEducationEvaluations = DateTime.ParseExact(model.DateofMostRecentSpecialEducationEvaluations, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        //sp.DateofNextScheduled3YearEvaluation = DateTime.ParseExact(model.DateofNextScheduled3YearEvaluation, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        //sp.CurrentIEPStartDate = DateTime.ParseExact(model.CurrentIEPStartDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        //sp.CurrentIEPExpirationDate = DateTime.ParseExact(model.CurrentIEPExpirationDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        //sp.DischargeDate = DateTime.ParseExact(model.DischargeDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        //sp.LocationAfterDischarge = model.LocationAfterDischarge;
                        try
                        {
                            sp.DateInitiallyEligibleforSpecialEducation = DateTime.ParseExact(model.DateInitiallyEligibleforSpecialEducation, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            sp.DateInitiallyEligibleforSpecialEducation = null;
                        }
                        try
                        {
                            sp.DateofMostRecentSpecialEducationEvaluations = DateTime.ParseExact(model.DateofMostRecentSpecialEducationEvaluations, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            sp.DateofMostRecentSpecialEducationEvaluations = null;
                        }
                        try
                        {
                            sp.DateofNextScheduled3YearEvaluation = DateTime.ParseExact(model.DateofNextScheduled3YearEvaluation, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            sp.DateofNextScheduled3YearEvaluation = null;
                        }
                        try
                        {
                            sp.CurrentIEPStartDate = DateTime.ParseExact(model.CurrentIEPStartDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            sp.CurrentIEPStartDate = null;
                        }
                        try
                        {
                            sp.CurrentIEPExpirationDate = DateTime.ParseExact(model.CurrentIEPExpirationDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            sp.CurrentIEPExpirationDate = null;
                        }
                        try
                        {
                            sp.DischargeDate = DateTime.ParseExact(model.DischargeDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            sp.DischargeDate = null;
                        }
                        sp.LocationAfterDischarge = model.LocationAfterDischarge;
                        sp.MelmarkNewEnglandsFollowUpResponsibilities = model.MelmarkNewEnglandsFollowUpResponsibilities;
                        sp.IEPReferralFullName = model.ReferralIEPFullName;
                        sp.IEPReferralPhone = model.ReferralIEPPhone;
                        sp.IEPReferralReferrinAgency = model.ReferralIEPReferringAgency;
                        sp.IEPReferralSourceofTuition = model.ReferralIEPSourceofTuition;
                        sp.IEPReferralTitle = model.ReferralIEPTitle;
                        sp.ModifiedBy = sess.LoginId;
                        sp.ModifiedOn = DateTime.Now;
                        dbobj.SaveChanges();
                        if (sourceFile != null)
                        {
                            model.ImageUrl = sp.ImageUrl;

                        }

                        addr = dbobj.AddressLists.Where(x => x.AddressId == adrRel.AddressId).SingleOrDefault();
                        addr.ApartmentType = model.AddressLine1;
                        addr.StreetName = model.AddressLine2;
                        addr.AddressLine3 = model.AddressLine3;
                        addr.AddressType = MetaData._StudentAddressType;
                        addr.City = model.City;
                        addr.CountryId = model.Country;
                        addr.StateProvince = model.State;
                        addr.PostalCode = model.ZipCode;
                        addr.ModifiedBy = sess.LoginId;
                        addr.ModifiedOn = DateTime.Now;
                        dbobj.SaveChanges();
                        var EmergencyContacts = dbobj.EmergencyContactSchools.Where(objEmergencyContact => objEmergencyContact.StudentPersonalId == sess.ReferralId).ToList();
                        if (EmergencyContacts.Count > 0)
                        {
                            foreach (var item in EmergencyContacts)
                            {
                                if (item.SequenceId == 1)
                                {
                                    objEmergency = dbobj.EmergencyContactSchools.Where(objEmergencys => objEmergencys.StudentPersonalId == sess.ReferralId &&
                                        objEmergencys.SequenceId == 1).SingleOrDefault();
                                    objEmergency.FirstName = model.EmergencyContactFirstName1;
                                    objEmergency.LastName = model.EmergencyContactLastName1;
                                    objEmergency.Title = model.EmergencyContactTitle1;
                                    objEmergency.Phone = model.EmergencyContactPhone1;
                                    objEmergency.ModifiedBy = sess.LoginId;
                                    objEmergency.ModifiedOn = DateTime.Now;
                                    dbobj.SaveChanges();
                                }
                                if (item.SequenceId == 2)
                                {

                                    objEmergency = dbobj.EmergencyContactSchools.Where(objEmergencys => objEmergencys.StudentPersonalId == sess.ReferralId &&
                                        objEmergencys.SequenceId == 2).SingleOrDefault();
                                    objEmergency.FirstName = model.EmergencyContactFirstName2;
                                    objEmergency.LastName = model.EmergencyContactLastName2;
                                    objEmergency.Title = model.EmergencyContactTitle2;
                                    objEmergency.Phone = model.EmergencyContactPhone2;
                                    objEmergency.ModifiedBy = sess.LoginId;
                                    objEmergency.ModifiedOn = DateTime.Now;
                                    dbobj.SaveChanges();
                                }
                                if (item.SequenceId == 3)
                                {

                                    objEmergency = dbobj.EmergencyContactSchools.Where(objEmergencys => objEmergencys.StudentPersonalId == sess.ReferralId &&
                                        objEmergencys.SequenceId == 3).SingleOrDefault();
                                    objEmergency.FirstName = model.EmergencyContactFirstName3;
                                    objEmergency.LastName = model.EmergencyContactLastName3;
                                    objEmergency.Title = model.EmergencyContactTitle3;
                                    objEmergency.Phone = model.EmergencyContactPhone3;
                                    objEmergency.ModifiedBy = sess.LoginId;
                                    objEmergency.ModifiedOn = DateTime.Now;
                                    dbobj.SaveChanges();
                                }
                                if (item.SequenceId == 4)
                                {

                                    objEmergency = dbobj.EmergencyContactSchools.Where(objEmergencys => objEmergencys.StudentPersonalId == sess.ReferralId &&
                                        objEmergencys.SequenceId == 4).SingleOrDefault();
                                    objEmergency.FirstName = model.EmergencyContactFirstName4;
                                    objEmergency.LastName = model.EmergencyContactLastName4;
                                    objEmergency.Title = model.EmergencyContactTitle4;
                                    objEmergency.Phone = model.EmergencyContactPhone4;
                                    objEmergency.ModifiedBy = sess.LoginId;
                                    objEmergency.ModifiedOn = DateTime.Now;
                                    dbobj.SaveChanges();
                                }
                                if (item.SequenceId == 5)
                                {

                                    objEmergency = dbobj.EmergencyContactSchools.Where(objEmergencys => objEmergencys.StudentPersonalId == sess.ReferralId &&
                                        objEmergencys.SequenceId == 5).SingleOrDefault();
                                    objEmergency.FirstName = model.EmergencyContactFirstName5;
                                    objEmergency.LastName = model.EmergencyContactLastName5;
                                    objEmergency.Title = model.EmergencyContactTitle5;
                                    objEmergency.Phone = model.EmergencyContactPhone5;
                                    objEmergency.ModifiedBy = sess.LoginId;
                                    objEmergency.ModifiedOn = DateTime.Now;
                                    dbobj.SaveChanges();
                                }
                            }
                        }
                        else
                        {
                            objEmergency.StudentPersonalId = sess.ReferralId;
                            objEmergency.SchoolId = sess.SchoolId;
                            objEmergency.FirstName = model.EmergencyContactFirstName1;
                            objEmergency.LastName = model.EmergencyContactLastName1;
                            objEmergency.Title = model.EmergencyContactTitle1;
                            objEmergency.Phone = model.EmergencyContactPhone1;
                            objEmergency.CreatedBy = sess.LoginId;
                            objEmergency.CreatedOn = DateTime.Now;
                            objEmergency.SequenceId = 1;
                            dbobj.EmergencyContactSchools.Add(objEmergency);
                            dbobj.SaveChanges();

                            objEmergency = new EmergencyContactSchool();
                            objEmergency.StudentPersonalId = sess.ReferralId;
                            objEmergency.SchoolId = sess.SchoolId;
                            objEmergency.FirstName = model.EmergencyContactFirstName2;
                            objEmergency.LastName = model.EmergencyContactLastName2;
                            objEmergency.Title = model.EmergencyContactTitle2;
                            objEmergency.Phone = model.EmergencyContactPhone2;
                            objEmergency.CreatedBy = sess.LoginId;
                            objEmergency.CreatedOn = DateTime.Now;
                            objEmergency.SequenceId = 2;
                            dbobj.EmergencyContactSchools.Add(objEmergency);
                            dbobj.SaveChanges();

                            objEmergency = new EmergencyContactSchool();
                            objEmergency.StudentPersonalId = sess.ReferralId;
                            objEmergency.SchoolId = sess.SchoolId;
                            objEmergency.FirstName = model.EmergencyContactFirstName3;
                            objEmergency.LastName = model.EmergencyContactLastName3;
                            objEmergency.Title = model.EmergencyContactTitle3;
                            objEmergency.Phone = model.EmergencyContactPhone3;
                            objEmergency.CreatedBy = sess.LoginId;
                            objEmergency.CreatedOn = DateTime.Now;
                            objEmergency.SequenceId = 3;
                            dbobj.EmergencyContactSchools.Add(objEmergency);
                            dbobj.SaveChanges();

                            objEmergency = new EmergencyContactSchool();
                            objEmergency.StudentPersonalId = sess.ReferralId;
                            objEmergency.SchoolId = sess.SchoolId;
                            objEmergency.FirstName = model.EmergencyContactFirstName4;
                            objEmergency.LastName = model.EmergencyContactLastName4;
                            objEmergency.Title = model.EmergencyContactTitle4;
                            objEmergency.Phone = model.EmergencyContactPhone4;
                            objEmergency.CreatedBy = sess.LoginId;
                            objEmergency.CreatedOn = DateTime.Now;
                            objEmergency.SequenceId = 4;
                            dbobj.EmergencyContactSchools.Add(objEmergency);
                            dbobj.SaveChanges();

                            objEmergency = new EmergencyContactSchool();
                            objEmergency.StudentPersonalId = sess.ReferralId;
                            objEmergency.SchoolId = sess.SchoolId;
                            objEmergency.FirstName = model.EmergencyContactFirstName5;
                            objEmergency.LastName = model.EmergencyContactLastName5;
                            objEmergency.Title = model.EmergencyContactTitle5;
                            objEmergency.Phone = model.EmergencyContactPhone5;
                            objEmergency.CreatedBy = sess.LoginId;
                            objEmergency.CreatedOn = DateTime.Now;
                            objEmergency.SequenceId = 5;
                            dbobj.EmergencyContactSchools.Add(objEmergency);
                            dbobj.SaveChanges();
                        }

                        var SchoolsAttended = dbobj.SchoolsAttendeds.Where(objschools => objschools.StudentPersonalId == sess.ReferralId).ToList();
                        if (SchoolsAttended.Count > 0)
                        {
                            foreach (var item in SchoolsAttended)
                            {
                                if (item.SequenceId == 1)
                                {
                                    objSchool = dbobj.SchoolsAttendeds.Where(objSchul => objSchul.StudentPersonalId == sess.ReferralId && objSchul.SequenceId == 1).SingleOrDefault();
                                    objSchool.SchoolName = model.SchoolName1;
                                    try
                                    {
                                        objSchool.DateFrom = DateTime.ParseExact(model.DateFrom1, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                    }
                                    catch
                                    {
                                        objSchool.DateFrom = null;
                                    }
                                    try
                                    {
                                        objSchool.DateTo = DateTime.ParseExact(model.DateTo1, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                    }
                                    catch
                                    {
                                        objSchool.DateTo = null;
                                    }
                                    objSchool.Address1 = model.SchoolAttendedAddress11;
                                    objSchool.Address2 = model.SchoolAttendedAddress21;
                                    objSchool.City = model.SchoolAttendedCity1;
                                    objSchool.State = model.SchoolAttendedState1;
                                    objSchool.ModifiedBy = sess.LoginId;
                                    objSchool.ModifiedOn = DateTime.Now;
                                    dbobj.SaveChanges();
                                }
                                if (item.SequenceId == 2)
                                {
                                    objSchool = dbobj.SchoolsAttendeds.Where(objSchul => objSchul.StudentPersonalId == sess.ReferralId && objSchul.SequenceId == 2).SingleOrDefault();
                                    objSchool.SchoolName = model.SchoolName2;
                                    try
                                    {
                                        objSchool.DateFrom = DateTime.ParseExact(model.DateFrom2, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                    }
                                    catch
                                    {
                                        objSchool.DateFrom = null;
                                    }
                                    try
                                    {
                                        objSchool.DateTo = DateTime.ParseExact(model.DateTo2, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                    }
                                    catch
                                    {
                                        objSchool.DateTo = null;
                                    }
                                    objSchool.Address1 = model.SchoolAttendedAddress12;
                                    objSchool.Address2 = model.SchoolAttendedAddress22;
                                    objSchool.City = model.SchoolAttendedCity2;
                                    objSchool.State = model.SchoolAttendedState2;
                                    objSchool.ModifiedBy = sess.LoginId;
                                    objSchool.ModifiedOn = DateTime.Now;
                                    dbobj.SaveChanges();
                                }
                                if (item.SequenceId == 3)
                                {
                                    objSchool = dbobj.SchoolsAttendeds.Where(objSchul => objSchul.StudentPersonalId == sess.ReferralId && objSchul.SequenceId == 3).SingleOrDefault();
                                    objSchool.SchoolName = model.SchoolName3;
                                    try
                                    {
                                        objSchool.DateFrom = DateTime.ParseExact(model.DateFrom3, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                    }
                                    catch
                                    {
                                        objSchool.DateFrom = null;
                                    }
                                    try
                                    {
                                        objSchool.DateTo = DateTime.ParseExact(model.DateTo3, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                    }
                                    catch
                                    {
                                        objSchool.DateTo = null;
                                    }
                                    objSchool.Address1 = model.SchoolAttendedAddress13;
                                    objSchool.Address2 = model.SchoolAttendedAddress23;
                                    objSchool.City = model.SchoolAttendedCity3;
                                    objSchool.State = model.SchoolAttendedState3;
                                    objSchool.ModifiedBy = sess.LoginId;
                                    objSchool.ModifiedOn = DateTime.Now;
                                    dbobj.SaveChanges();
                                }
                            }
                        }
                        else
                        {
                            objSchool.StudentPersonalId = sess.ReferralId;
                            objSchool.SchoolId = sess.SchoolId;
                            objSchool.SchoolName = model.SchoolName1;
                            try
                            {
                                objSchool.DateFrom = DateTime.ParseExact(model.DateFrom1, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            }
                            catch
                            {
                                objSchool.DateFrom = null;
                            }
                            try
                            {
                                objSchool.DateTo = DateTime.ParseExact(model.DateTo1, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            }
                            catch
                            {
                                objSchool.DateTo = null;
                            }
                            objSchool.Address1 = model.SchoolAttendedAddress11;
                            objSchool.Address2 = model.SchoolAttendedAddress21;
                            objSchool.City = model.SchoolAttendedCity1;
                            objSchool.State = model.SchoolAttendedState1;
                            objSchool.CreatedBy = sess.LoginId;
                            objSchool.CreatedOn = DateTime.Now;
                            objSchool.SequenceId = 1;
                            dbobj.SchoolsAttendeds.Add(objSchool);
                            dbobj.SaveChanges();

                            objSchool = new SchoolsAttended();
                            objSchool.StudentPersonalId = sess.ReferralId;
                            objSchool.SchoolId = sess.SchoolId;
                            objSchool.SchoolName = model.SchoolName2;
                            try
                            {
                                objSchool.DateFrom = DateTime.ParseExact(model.DateFrom1, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            }
                            catch
                            {
                                objSchool.DateFrom = null;
                            }
                            try
                            {
                                objSchool.DateTo = DateTime.ParseExact(model.DateTo1, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            }
                            catch
                            {
                                objSchool.DateTo = null;
                            }
                            objSchool.Address1 = model.SchoolAttendedAddress12;
                            objSchool.Address2 = model.SchoolAttendedAddress22;
                            objSchool.City = model.SchoolAttendedCity2;
                            objSchool.State = model.SchoolAttendedState2;
                            objSchool.CreatedBy = sess.LoginId;
                            objSchool.CreatedOn = DateTime.Now;
                            objSchool.SequenceId = 2;
                            dbobj.SchoolsAttendeds.Add(objSchool);
                            dbobj.SaveChanges();

                            objSchool = new SchoolsAttended();
                            objSchool.StudentPersonalId = sess.ReferralId;
                            objSchool.SchoolId = sess.SchoolId;
                            objSchool.SchoolName = model.SchoolName3;
                            try
                            {
                                objSchool.DateFrom = DateTime.ParseExact(model.DateFrom1, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            }
                            catch
                            {
                                objSchool.DateFrom = null;
                            }
                            try
                            {
                                objSchool.DateTo = DateTime.ParseExact(model.DateTo1, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            }
                            catch
                            {
                                objSchool.DateTo = null;
                            }
                            objSchool.Address1 = model.SchoolAttendedAddress13;
                            objSchool.Address2 = model.SchoolAttendedAddress23;
                            objSchool.City = model.SchoolAttendedCity3;
                            objSchool.State = model.SchoolAttendedState3;
                            objSchool.CreatedBy = sess.LoginId;
                            objSchool.CreatedOn = DateTime.Now;
                            objSchool.SequenceId = 3;
                            dbobj.SchoolsAttendeds.Add(objSchool);
                            dbobj.SaveChanges();

                        }
                        try
                        {
                            objInsurance = dbobj.Insurances.Where(objInsu => objInsu.StudentPersonalId == sess.ReferralId && objInsu.PreferType == "Primary").SingleOrDefault();
                            if (objInsurance != null)
                            {
                                objInsurance.InsuranceType = model.InsuranceType;
                                objInsurance.PolicyHolder = model.PolicyHolder;
                                objInsurance.PolicyNumber = model.PolicyNumber;
                                objInsurance.ModifiedBy = sess.LoginId;
                                objInsurance.ModifiedOn = DateTime.Now;
                                dbobj.SaveChanges();
                            }
                            else
                            {
                                objInsurance.StudentPersonalId = sess.ReferralId;
                                objInsurance.InsuranceType = model.InsuranceType;
                                objInsurance.PolicyHolder = model.PolicyHolder;
                                objInsurance.PolicyNumber = model.PolicyNumber;
                                objInsurance.CreatedBy = sess.LoginId;
                                objInsurance.CreatedOn = DateTime.Now;
                                dbobj.Insurances.Add(objInsurance);
                                dbobj.SaveChanges();
                            }
                        }
                        catch { }
                        //dbobj.SaveChanges();

                        Result = "Sucess";
                    }
                    Result = "Sucess";
                    trans.Complete();
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
                Result = "Failed";
            }
            return Result;
        }


        /// <summary>
        /// Function to Save / Update PA Client Information.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="sourceFile"></param>
        /// <returns></returns>

        public string SaveData(ClientRegistrationPAModel model, HttpPostedFileBase sourceFile)
        {
            sess = (clsSession)HttpContext.Current.Session["UserSession"];

            string Result = "";
            dbobj = new MelmarkDBEntities();
            StudentPersonal sp = new StudentPersonal();
            AddressList addr = new AddressList();
            StudentAddresRel adrRel = new StudentAddresRel();
            StudentPersonalPA studentPA = new StudentPersonalPA();
            DiaganosesPA diagnose = new DiaganosesPA();
            BasicBehavioralInformation basicBehavior = new BasicBehavioralInformation();
            AdaptiveEquipment adaptive = new DataLayer.AdaptiveEquipment();
            BehavioursPA behavior = new BehavioursPA();
            MetaData = new GlobalData();
            string dirpath = (AppDomain.CurrentDomain.BaseDirectory + "Images/StudentImages/".ToString()).Replace('\\', '/');

            //  int StudentId = 1;

            try
            {
                using (TransactionScope trans = new TransactionScope(TransactionScopeOption.Required))
                {
                    if (sess.ReferralId == 0 && sess.AddressId == 0)
                    {
                        sp.LocalId = "STD1002";
                        sp.SchoolId = sess.SchoolId;
                        sp.FirstName = model.FirstName;
                        sp.LastName = model.LastName;
                        sp.MiddleName = model.MiddleName;
                        sp.Suffix = model.LastNameSuffix;
                        sp.NickName = model.NickName;
                        sp.BirthDate = DateTime.ParseExact(model.DateOfBirth, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        sp.PlaceOfBirth = model.PlaceOfBirth;
                        sp.CountryOfBirth = model.CountryofBirth;
                        sp.StateOfBirth = model.StateOfBirth;
                        sp.CitizenshipStatus = model.Citizenship;
                        sp.RaceId = model.Race;
                        sp.Gender = model.Gender;
                        sp.HairColor = model.HairColor;
                        sp.EyeColor = model.EyeColor;
                        sp.MostRecentGradeLevel = model.MostRecentGradeLevel;
                        try
                        {
                            sp.AdmissionDate = DateTime.ParseExact(model.DateUpdated, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            sp.AdmissionDate = null;
                        }
                        sp.Height = Convert.ToDecimal(model.Height);
                        sp.Weight = Convert.ToDecimal(model.Weight);
                        sp.PrimaryLanguage = model.PrimaryLanguage;

                        sp.GuardianShip = model.GuardianshipStatus;
                        sp.DistingushingMarks = model.DistigushingMarks;
                        sp.LegalCompetencyStatus = model.LegalCompetencyStatus;
                        sp.OtherStateAgenciesInvolvedWithStudent = model.OtherStateAgenciesInvolvedWithStudent;
                        sp.MaritalStatusofBothParents = model.MaritalStatusofBothParents;
                        sp.CaseManagerEducational = model.CaseManagerEducational;
                        sp.CaseManagerResidential = model.CaseManagerResidential;
                        //if (sourceFile != null)
                        //{
                        //    sp.ImageUrl = sourceFile.FileName;
                        //}

                        if (sourceFile != null)
                        {

                            byte[] fileBytes = new byte[sourceFile.ContentLength];
                            int byteCount = sourceFile.InputStream.Read(fileBytes, 0, (int)sourceFile.ContentLength);

                            sp.ImageUrl = Convert.ToBase64String(fileBytes);
                            // model.ImageUrl = dirpath + "/" + sp.StudentPersonalId + "-" + sourceFile.FileName;


                        }
                        else
                        {
                            dirpath = System.Web.Configuration.WebConfigurationManager.AppSettings["ImagessLocation"].ToString();
                            //dirpath = (AppDomain.CurrentDomain.BaseDirectory + dirpath);
                            FileInfo fileInfo = new FileInfo(dirpath);
                            byte[] data = new byte[fileInfo.Length];


                            using (FileStream fs = fileInfo.OpenRead())
                            {
                                fs.Read(data, 0, data.Length);
                                //  sp.ImageUrl = fs.ToString();
                            }
                            sp.ImageUrl = Convert.ToBase64String(data);
                            //int id = objRef.StudentUpldPhoto(StudentPersnlId, data);

                        }

                        sp.ImagePermission = model.PhotoReleasePermission;

                        sp.StudentType = "Client";

                        sp.CreatedBy = 1;
                        sp.CreatedOn = DateTime.Now;
                        dbobj.StudentPersonals.Add(sp);
                        dbobj.SaveChanges();

                        sess.ReferralId = sp.StudentPersonalId;

                        addr.AddressLine1 = model.AddressLine1;
                        addr.AddressLine2 = model.AddressLine2;
                        addr.AddressType = MetaData._StudentAddressType;
                        addr.AddressLine3 = model.AddressLine3;
                        addr.Phone = model.PhoneNumber;
                        addr.City = model.City;
                        addr.CountryId = model.Country;
                        addr.StateProvince = model.State;
                        addr.PostalCode = model.ZipCode;
                        addr.CreatedBy = sess.LoginId;
                        addr.CreatedOn = DateTime.Now;
                        dbobj.AddressLists.Add(addr);
                        dbobj.SaveChanges();

                        adrRel.AddressId = addr.AddressId;
                        adrRel.StudentPersonalId = sp.StudentPersonalId;
                        adrRel.ContactSequence = 0;
                        adrRel.CreatedBy = sess.LoginId;
                        adrRel.CreatedOn = DateTime.Now;
                        dbobj.StudentAddresRels.Add(adrRel);
                        dbobj.SaveChanges();

                        studentPA.StudentPersonalId = sp.StudentPersonalId;
                        studentPA.SchoolId = sess.SchoolId;
                        studentPA.Allergies = model.Allergie;
                        studentPA.Bathroom = model.Bathroom;
                        studentPA.Diet = model.Diet;
                        studentPA.dy_TaskOrBreak = model.TaskORBreak;
                        studentPA.dy_TransitionInside = model.TransitionInside;
                        studentPA.dy_TransitionUnevenGround = model.TransitionUnevenGround;
                        studentPA.Consciousness = model.Consciousness;
                        studentPA.CommonAreas = model.CommonAreas;
                        studentPA.RiskOfResistance = model.RiskOfResistance;
                        studentPA.Mobility = model.Mobility;
                        studentPA.NeedForExtraHelp = model.NeedForExtraHelp;
                        studentPA.ResponseToInstruction = model.ResponseToInstruction;
                        studentPA.WalkingResponses = model.WalkingResponse;
                        studentPA.ho_BedroomAsleep = model.BedroomAsleep;
                        studentPA.ho_BedroomAwake = model.BedroomAwake;
                        studentPA.ho_CommonAres = model.CommonAreas;
                        studentPA.OnCampus = model.OnCampus;
                        studentPA.WhenTranspoting = model.WhenTranspoting;
                        studentPA.OffCampus = model.OffCampus;
                        studentPA.PoolOrSwimming = model.PoolOrSwimming;
                        studentPA.Van = model.van;
                        studentPA.Seizures = model.Seizures;
                        studentPA.Other = model.Other;
                        studentPA.FundingSource = model.Funding;
                        studentPA.CreatedBy = sess.LoginId;
                        studentPA.CreatedOn = DateTime.Now;
                        dbobj.StudentPersonalPAs.Add(studentPA);
                        dbobj.SaveChanges();
                        for (int i = 0; i < model.Diagnosis.Count; i++)
                        {
                            diagnose.StudentPersonalId = sp.StudentPersonalId;
                            diagnose.SchoolId = sess.SchoolId;
                            diagnose.Diaganoses = model.Diagnosis[i].Name;
                            diagnose.CreatedBy = sess.LoginId;
                            diagnose.CreatedOn = DateTime.Now;
                            dbobj.DiaganosesPAs.Add(diagnose);
                            dbobj.SaveChanges();
                        }
                        for (int i = 0; i < model.Adapt.Count; i++)
                        {
                            adaptive.StudentPersonalId = sp.StudentPersonalId;
                            adaptive.SchoolId = sess.SchoolId;
                            adaptive.Item = model.Adapt[i].item;
                            adaptive.ScheduleForUse = model.Adapt[i].ScheduledForUss;
                            adaptive.StorageLocation = model.Adapt[i].StorageLocation;
                            adaptive.CleaningInstruction = model.Adapt[i].CleaningInstruction;
                            adaptive.CreatedBy = sess.LoginId;
                            adaptive.CreatedOn = DateTime.Now;
                            dbobj.AdaptiveEquipments.Add(adaptive);
                            dbobj.SaveChanges();
                        }
                        for (int i = 0; i < model.BasicBehave.Count; i++)
                        {
                            basicBehavior.StudentPersonalId = sp.StudentPersonalId;
                            basicBehavior.SchoolId = sess.SchoolId;
                            basicBehavior.TargetBehavior = model.BasicBehave[i].TargetBehavior;
                            basicBehavior.Definition = model.BasicBehave[i].Definition;
                            basicBehavior.Antecedent = model.BasicBehave[i].Antecedent;
                            basicBehavior.FCT = model.BasicBehave[i].FCT;
                            basicBehavior.Consequence = model.BasicBehave[i].Consequances;
                            basicBehavior.CreatedBy = sess.LoginId;
                            basicBehavior.CreatedOn = DateTime.Now;
                            dbobj.BasicBehavioralInformations.Add(basicBehavior);
                            dbobj.SaveChanges();
                        }



                        saveBehaviors(behavior, sess, model);

                        #region behave save
                        // model.Dressing1;

                        // diagnose.

                        //int parentId = behavId("LIFTING / TRANSFERS");
                        //behavior.StudentPersonalId = sp.StudentPersonalId;
                        //behavior.SchoolId = sess.SchoolId;
                        //behavior.ParentId = parentId;
                        //behavior.BehaviourName = model.LiftingOrTransfers1;
                        //behavior.CreatedBy = sess.LoginId;
                        //behavior.CreatedOn = DateTime.Now;
                        //dbobj.BehavioursPAs.Add(behavior);
                        //dbobj.SaveChanges();
                        //behavior.StudentPersonalId = sp.StudentPersonalId;
                        //behavior.ParentId = parentId;
                        //behavior.SchoolId = sess.SchoolId;
                        //behavior.BehaviourName = model.LiftingOrTransfers2;
                        //behavior.CreatedBy = sess.LoginId;
                        //behavior.CreatedOn = DateTime.Now;
                        //dbobj.BehavioursPAs.Add(behavior);
                        //dbobj.SaveChanges();

                        //parentId = behavId("AMBULATION");
                        //behavior.StudentPersonalId = sp.StudentPersonalId;
                        //behavior.ParentId = parentId;
                        //behavior.SchoolId = sess.SchoolId;
                        //behavior.BehaviourName = model.Ambulation1;
                        //behavior.CreatedBy = sess.LoginId;
                        //behavior.CreatedOn = DateTime.Now;
                        //dbobj.BehavioursPAs.Add(behavior);
                        //dbobj.SaveChanges();
                        //behavior.StudentPersonalId = sp.StudentPersonalId;
                        //behavior.ParentId = parentId;
                        //behavior.SchoolId = sess.SchoolId;
                        //behavior.BehaviourName = model.Ambulation2;
                        //behavior.CreatedBy = sess.LoginId;
                        //behavior.CreatedOn = DateTime.Now;
                        //dbobj.BehavioursPAs.Add(behavior);
                        //dbobj.SaveChanges();

                        //parentId = behavId("TOILETING");
                        //behavior.StudentPersonalId = sp.StudentPersonalId;
                        //behavior.ParentId = parentId;
                        //behavior.SchoolId = sess.SchoolId;
                        //behavior.BehaviourName = model.Toileting1;
                        //behavior.CreatedBy = sess.LoginId;
                        //behavior.CreatedOn = DateTime.Now;
                        //dbobj.BehavioursPAs.Add(behavior);
                        //dbobj.SaveChanges();
                        //behavior.StudentPersonalId = sp.StudentPersonalId;
                        //behavior.ParentId = parentId;
                        //behavior.SchoolId = sess.SchoolId;
                        //behavior.BehaviourName = model.Toileting2;
                        //behavior.CreatedBy = sess.LoginId;
                        //behavior.CreatedOn = DateTime.Now;
                        //dbobj.BehavioursPAs.Add(behavior);
                        //dbobj.SaveChanges();

                        //parentId = behavId("EATING");
                        //behavior.StudentPersonalId = sp.StudentPersonalId;
                        //behavior.ParentId = parentId;
                        //behavior.SchoolId = sess.SchoolId;
                        //behavior.BehaviourName = model.Eating1;
                        //behavior.CreatedBy = sess.LoginId;
                        //behavior.CreatedOn = DateTime.Now;
                        //dbobj.BehavioursPAs.Add(behavior);
                        //dbobj.SaveChanges();
                        //behavior.StudentPersonalId = sp.StudentPersonalId;
                        //behavior.ParentId = parentId;
                        //behavior.SchoolId = sess.SchoolId;
                        //behavior.BehaviourName = model.Eating2;
                        //behavior.CreatedBy = sess.LoginId;
                        //behavior.CreatedOn = DateTime.Now;
                        //dbobj.BehavioursPAs.Add(behavior);
                        //dbobj.SaveChanges();

                        //parentId = behavId("SHOWERING");
                        //behavior.StudentPersonalId = sp.StudentPersonalId;
                        //behavior.ParentId = parentId;
                        //behavior.SchoolId = sess.SchoolId;
                        //behavior.BehaviourName = model.Showering1;
                        //behavior.CreatedBy = sess.LoginId;
                        //behavior.CreatedOn = DateTime.Now;
                        //dbobj.BehavioursPAs.Add(behavior);
                        //dbobj.SaveChanges();
                        //behavior.StudentPersonalId = sp.StudentPersonalId;
                        //behavior.ParentId = parentId;
                        //behavior.SchoolId = sess.SchoolId;
                        //behavior.BehaviourName = model.Showering2;
                        //behavior.CreatedBy = sess.LoginId;
                        //behavior.CreatedOn = DateTime.Now;
                        //dbobj.BehavioursPAs.Add(behavior);
                        //dbobj.SaveChanges();

                        //parentId = behavId("TOOTHBRUSHING");
                        //behavior.StudentPersonalId = sp.StudentPersonalId;
                        //behavior.ParentId = parentId;
                        //behavior.SchoolId = sess.SchoolId;
                        //behavior.BehaviourName = model.ToothBrushing1;
                        //behavior.CreatedBy = sess.LoginId;
                        //behavior.CreatedOn = DateTime.Now;
                        //dbobj.BehavioursPAs.Add(behavior);
                        //dbobj.SaveChanges();
                        //behavior.StudentPersonalId = sp.StudentPersonalId;
                        //behavior.ParentId = parentId;
                        //behavior.SchoolId = sess.SchoolId;
                        //behavior.BehaviourName = model.ToothBrushing2;
                        //behavior.CreatedBy = sess.LoginId;
                        //behavior.CreatedOn = DateTime.Now;
                        //dbobj.BehavioursPAs.Add(behavior);
                        //dbobj.SaveChanges();

                        //parentId = behavId("DRESSING");
                        //behavior.StudentPersonalId = sp.StudentPersonalId;
                        //behavior.ParentId = parentId;
                        //behavior.SchoolId = sess.SchoolId;
                        //behavior.BehaviourName = model.Dressing1;
                        //behavior.CreatedBy = sess.LoginId;
                        //behavior.CreatedOn = DateTime.Now;
                        //dbobj.BehavioursPAs.Add(behavior);
                        //dbobj.SaveChanges();
                        //behavior.StudentPersonalId = sp.StudentPersonalId;
                        //behavior.ParentId = parentId;
                        //behavior.SchoolId = sess.SchoolId;
                        //behavior.BehaviourName = model.Dressing2;
                        //behavior.CreatedBy = sess.LoginId;
                        //behavior.CreatedOn = DateTime.Now;
                        //dbobj.BehavioursPAs.Add(behavior);
                        //dbobj.SaveChanges();

                        //parentId = behavId("SKIN CARE/SKIN INTEGRITY");
                        //behavior.StudentPersonalId = sp.StudentPersonalId;
                        //behavior.ParentId = parentId;
                        //behavior.SchoolId = sess.SchoolId;
                        //behavior.BehaviourName = model.SkinCare1;
                        //behavior.CreatedBy = sess.LoginId;
                        //behavior.CreatedOn = DateTime.Now;
                        //dbobj.BehavioursPAs.Add(behavior);
                        //dbobj.SaveChanges();
                        //behavior.StudentPersonalId = sp.StudentPersonalId;
                        //behavior.ParentId = parentId;
                        //behavior.SchoolId = sess.SchoolId;
                        //behavior.BehaviourName = model.SkinCare2;
                        //behavior.CreatedBy = sess.LoginId;
                        //behavior.CreatedOn = DateTime.Now;
                        //dbobj.BehavioursPAs.Add(behavior);
                        //dbobj.SaveChanges();

                        //parentId = behavId("COMMUNICATION");
                        //behavior.StudentPersonalId = sp.StudentPersonalId;
                        //behavior.ParentId = parentId;
                        //behavior.SchoolId = sess.SchoolId;
                        //behavior.BehaviourName = model.Communication1;
                        //behavior.CreatedBy = sess.LoginId;
                        //behavior.CreatedOn = DateTime.Now;
                        //dbobj.BehavioursPAs.Add(behavior);
                        //dbobj.SaveChanges();
                        //behavior.StudentPersonalId = sp.StudentPersonalId;
                        //behavior.ParentId = parentId;
                        //behavior.SchoolId = sess.SchoolId;
                        //behavior.BehaviourName = model.Communication2;
                        //behavior.CreatedBy = sess.LoginId;
                        //behavior.CreatedOn = DateTime.Now;
                        //dbobj.BehavioursPAs.Add(behavior);
                        //dbobj.SaveChanges();


                        //parentId = behavId("PREFERRED ACTIVITIES");
                        //behavior.StudentPersonalId = sp.StudentPersonalId;
                        //behavior.ParentId = parentId;
                        //behavior.SchoolId = sess.SchoolId;
                        //behavior.BehaviourName = model.preferedActivities1;
                        //behavior.CreatedBy = sess.LoginId;
                        //behavior.CreatedOn = DateTime.Now;
                        //dbobj.BehavioursPAs.Add(behavior);
                        //dbobj.SaveChanges();
                        //behavior.StudentPersonalId = sp.StudentPersonalId;
                        //behavior.ParentId = parentId;
                        //behavior.SchoolId = sess.SchoolId;
                        //behavior.BehaviourName = model.preferedActivities2;
                        //behavior.CreatedBy = sess.LoginId;
                        //behavior.CreatedOn = DateTime.Now;
                        //dbobj.BehavioursPAs.Add(behavior);
                        //dbobj.SaveChanges();

                        //parentId = behavId("GENERAL INFORMATION");
                        //behavior.StudentPersonalId = sp.StudentPersonalId;
                        //behavior.ParentId = parentId;
                        //behavior.SchoolId = sess.SchoolId;
                        //behavior.BehaviourName = model.GeneralInformation1;
                        //behavior.CreatedBy = sess.LoginId;
                        //behavior.CreatedOn = DateTime.Now;
                        //dbobj.BehavioursPAs.Add(behavior);
                        //dbobj.SaveChanges();
                        //behavior.StudentPersonalId = sp.StudentPersonalId;
                        //behavior.ParentId = parentId;
                        //behavior.SchoolId = sess.SchoolId;
                        //behavior.BehaviourName = model.GeneralInformation2;
                        //behavior.CreatedBy = sess.LoginId;
                        //behavior.CreatedOn = DateTime.Now;
                        //dbobj.BehavioursPAs.Add(behavior);
                        //dbobj.SaveChanges();

                        //parentId = behavId("SUGGESTED PROACTIVE ENVIRONMENTAL PROCEDURES");
                        //behavior.StudentPersonalId = sp.StudentPersonalId;
                        //behavior.ParentId = parentId;
                        //behavior.SchoolId = sess.SchoolId;
                        //behavior.BehaviourName = model.SuggestedProactiveEnvironmentalProcedures1;
                        //behavior.CreatedBy = sess.LoginId;
                        //behavior.CreatedOn = DateTime.Now;
                        //dbobj.BehavioursPAs.Add(behavior);
                        //dbobj.SaveChanges();
                        //behavior.StudentPersonalId = sp.StudentPersonalId;
                        //behavior.ParentId = parentId;
                        //behavior.SchoolId = sess.SchoolId;
                        //behavior.BehaviourName = model.SuggestedProactiveEnvironmentalProcedures2;
                        //behavior.CreatedBy = sess.LoginId;
                        //behavior.CreatedOn = DateTime.Now;
                        //dbobj.BehavioursPAs.Add(behavior);
                        //dbobj.SaveChanges();
                        #endregion




                        Result = "Sucess";
                    }
                    else
                    {
                        sp = dbobj.StudentPersonals.Where(x => x.StudentPersonalId == sess.ReferralId && x.SchoolId == sess.SchoolId).SingleOrDefault();
                        adrRel = dbobj.StudentAddresRels.Where(objAddressRel => objAddressRel.StudentPersonalId == sess.ReferralId && objAddressRel.ContactSequence == 0).SingleOrDefault();
                        //sp.LocalId = "STD1002";
                        sp.FirstName = model.FirstName;
                        sp.LastName = model.LastName;
                        sp.MiddleName = model.MiddleName;
                        sp.Suffix = model.LastNameSuffix;
                        sp.BirthDate = DateTime.ParseExact(model.DateOfBirth, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        sp.PlaceOfBirth = model.PlaceOfBirth;
                        sp.CountryOfBirth = model.CountryofBirth;
                        sp.StateOfBirth = model.StateOfBirth;
                        sp.NickName = model.NickName;
                        sp.CitizenshipStatus = model.Citizenship;
                        sp.RaceId = model.Race;
                        sp.Gender = model.Gender;
                        sp.HairColor = model.HairColor;
                        sp.EyeColor = model.EyeColor;
                        sp.MostRecentGradeLevel = model.MostRecentGradeLevel;
                        try
                        {
                            sp.AdmissionDate = DateTime.ParseExact(model.DateUpdated, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            sp.AdmissionDate = null;
                        }
                        sp.Height = Convert.ToDecimal(model.Height);
                        sp.Weight = Convert.ToDecimal(model.Weight);
                        sp.PrimaryLanguage = model.PrimaryLanguage;
                        sp.DistingushingMarks = model.DistigushingMarks;
                        sp.LegalCompetencyStatus = model.LegalCompetencyStatus;
                        sp.OtherStateAgenciesInvolvedWithStudent = model.OtherStateAgenciesInvolvedWithStudent;
                        sp.MaritalStatusofBothParents = model.MaritalStatusofBothParents;
                        sp.CaseManagerEducational = model.CaseManagerEducational;
                        sp.CaseManagerResidential = model.CaseManagerResidential;
                        if (sourceFile != null)
                        {

                            byte[] fileBytes = new byte[sourceFile.ContentLength];
                            int byteCount = sourceFile.InputStream.Read(fileBytes, 0, (int)sourceFile.ContentLength);

                            sp.ImageUrl = Convert.ToBase64String(fileBytes);
                            model.ImageUrl = dirpath + "/" + sp.StudentPersonalId + "-" + sourceFile.FileName;


                        }

                        sp.ImagePermission = model.PhotoReleasePermission;
                        sp.GuardianShip = model.GuardianshipStatus;


                        sp.ModifiedBy = sess.LoginId;
                        sp.ModifiedOn = DateTime.Now;
                        dbobj.SaveChanges();

                        addr = dbobj.AddressLists.Where(x => x.AddressId == adrRel.AddressId).SingleOrDefault();
                        addr.AddressLine1 = model.AddressLine1;
                        addr.AddressLine2 = model.AddressLine2;
                        addr.AddressLine3 = model.AddressLine3;
                        addr.AddressType = MetaData._StudentAddressType;
                        addr.City = model.City;
                        addr.CountryId = model.Country;
                        addr.StateProvince = model.State;
                        addr.PostalCode = model.ZipCode;
                        addr.ModifiedBy = sess.LoginId;
                        addr.ModifiedOn = DateTime.Now;
                        dbobj.SaveChanges();

                        studentPA = dbobj.StudentPersonalPAs.Where(objStudentPersonalPA => objStudentPersonalPA.StudentPersonalId == sess.ReferralId
                            && objStudentPersonalPA.SchoolId == sess.SchoolId).SingleOrDefault();

                        studentPA.Allergies = model.Allergie;
                        studentPA.Bathroom = model.Bathroom;
                        studentPA.Diet = model.Diet;
                        studentPA.dy_TaskOrBreak = model.TaskORBreak;
                        studentPA.dy_TransitionInside = model.TransitionInside;
                        studentPA.dy_TransitionUnevenGround = model.TransitionUnevenGround;
                        studentPA.Consciousness = model.Consciousness;
                        studentPA.CommonAreas = model.CommonAreas;
                        studentPA.RiskOfResistance = model.RiskOfResistance;
                        studentPA.Mobility = model.Mobility;
                        studentPA.NeedForExtraHelp = model.NeedForExtraHelp;
                        studentPA.ResponseToInstruction = model.ResponseToInstruction;
                        studentPA.WalkingResponses = model.WalkingResponse;
                        studentPA.ho_BedroomAsleep = model.BedroomAsleep;
                        studentPA.ho_BedroomAwake = model.BedroomAwake;
                        studentPA.ho_CommonAres = model.CommonAreas;
                        studentPA.OnCampus = model.OnCampus;
                        studentPA.WhenTranspoting = model.WhenTranspoting;
                        studentPA.OffCampus = model.OffCampus;
                        studentPA.PoolOrSwimming = model.PoolOrSwimming;
                        studentPA.Van = model.van;
                        studentPA.Seizures = model.Seizures;
                        studentPA.Other = model.Other;
                        studentPA.FundingSource = model.Funding;
                        studentPA.ModifiedBy = sess.LoginId;
                        studentPA.ModifiedOn = DateTime.Now;

                        dbobj.SaveChanges();
                        var diagnoses = dbobj.DiaganosesPAs.Where(objDiagnoses => objDiagnoses.StudentPersonalId == sess.ReferralId && objDiagnoses.SchoolId == sess.SchoolId).ToList();
                        int i = 0;
                        if (diagnoses.Count > 0)
                        {
                            foreach (var item in diagnoses)
                            {

                                diagnose = dbobj.DiaganosesPAs.Where(objDiagno => objDiagno.DiaganosePAId == item.DiaganosePAId && objDiagno.SchoolId == sess.SchoolId).SingleOrDefault();
                                diagnose.Diaganoses = model.Diagnosis[i].Name;
                                diagnose.ModifiedBy = sess.LoginId;
                                diagnose.ModifiedOn = DateTime.Now;
                                dbobj.SaveChanges();
                                i++;
                            }
                        }
                        else
                        {
                            foreach (var item in model.Diagnosis)
                            {
                                diagnose.StudentPersonalId = sess.ReferralId;
                                diagnose.SchoolId = sess.SchoolId;
                                diagnose.Diaganoses = model.Diagnosis[i].Name;
                                diagnose.CreatedBy = sess.LoginId;
                                diagnose.CreatedOn = DateTime.Now;
                                dbobj.DiaganosesPAs.Add(diagnose);
                                dbobj.SaveChanges();
                                i++;
                            }

                        }
                        if (model.Adapt.Count > 0)
                        {
                            Int16 Id = 0;

                            for (int l = 0; l < model.Adapt.Count; l++)
                            {
                                if (model.Adapt[l].AdaptiveEquimentId != 0)
                                {
                                    Id = Convert.ToInt16(model.Adapt[l].AdaptiveEquimentId);
                                    var ADP = dbobj.AdaptiveEquipments.Where(objAdapt => objAdapt.AdaptiveEquipmentId == Id && objAdapt.SchoolId == sess.SchoolId).SingleOrDefault();
                                    ADP.Item = model.Adapt[l].item;
                                    ADP.ScheduleForUse = model.Adapt[l].ScheduledForUss;
                                    ADP.StorageLocation = model.Adapt[l].StorageLocation;
                                    ADP.CleaningInstruction = model.Adapt[l].CleaningInstruction;
                                    ADP.ModifiedBy = sess.LoginId;
                                    ADP.ModifiedOn = DateTime.Now;
                                    dbobj.SaveChanges();
                                }

                                else
                                {
                                    adaptive = new AdaptiveEquipment();
                                    adaptive.StudentPersonalId = sp.StudentPersonalId;
                                    adaptive.SchoolId = sess.SchoolId;
                                    adaptive.Item = model.Adapt[l].item;
                                    adaptive.ScheduleForUse = model.Adapt[l].ScheduledForUss;
                                    adaptive.StorageLocation = model.Adapt[l].StorageLocation;
                                    adaptive.CleaningInstruction = model.Adapt[l].CleaningInstruction;
                                    adaptive.CreatedBy = sess.LoginId;
                                    adaptive.CreatedOn = DateTime.Now;
                                    dbobj.AdaptiveEquipments.Add(adaptive);
                                    dbobj.SaveChanges();
                                }
                            }
                        }
                        //var Adaptive = dbobj.AdaptiveEquipments.Where(objAdaptive => objAdaptive.StudentPersonalId == sess.ReferralId && objAdaptive.SchoolId == sess.SchoolId).ToList();
                        //i = 0;
                        //if (Adaptive.Count > 0)
                        //{
                        //    foreach (var item in Adaptive)
                        //    {

                        //        adaptive = dbobj.AdaptiveEquipments.Where(objAdapt => objAdapt.AdaptiveEquipmentId == item.AdaptiveEquipmentId && objAdapt.SchoolId == sess.SchoolId).SingleOrDefault();
                        //        adaptive.Item = model.Adapt[i].item;
                        //        adaptive.ScheduleForUse = model.Adapt[i].ScheduledForUss;
                        //        adaptive.StorageLocation = model.Adapt[i].StorageLocation;
                        //        adaptive.CleaningInstruction = model.Adapt[i].CleaningInstruction;
                        //        adaptive.ModifiedBy = sess.LoginId;
                        //        adaptive.ModifiedOn = DateTime.Now;
                        //        dbobj.SaveChanges();
                        //        i++;
                        //    }
                        //}
                        //else
                        //{

                        //    adaptive.StudentPersonalId = sp.StudentPersonalId;
                        //    adaptive.SchoolId = sess.SchoolId;
                        //    adaptive.Item = model.Adapt[i].item;
                        //    adaptive.ScheduleForUse = model.Adapt[i].ScheduledForUss;
                        //    adaptive.StorageLocation = model.Adapt[i].StorageLocation;
                        //    adaptive.CleaningInstruction = model.Adapt[i].CleaningInstruction;
                        //    adaptive.CreatedBy = sess.LoginId;
                        //    adaptive.CreatedOn = DateTime.Now;
                        //    dbobj.AdaptiveEquipments.Add(adaptive);
                        //    dbobj.SaveChanges();
                        //}

                        if (model.BasicBehave.Count > 0)
                        {
                            Int16 Id = 0;

                            for (int l = 0; l < model.BasicBehave.Count; l++)
                            {
                                if (model.BasicBehave[l].BasicBehavioralInformationId != 0)
                                {
                                    Id = Convert.ToInt16(model.BasicBehave[l].BasicBehavioralInformationId);
                                    var BEH = dbobj.BasicBehavioralInformations.Where(objBasicBehav => objBasicBehav.BasicBehavioralInformationId == Id && objBasicBehav.SchoolId == sess.SchoolId).SingleOrDefault();

                                    BEH.TargetBehavior = model.BasicBehave[l].TargetBehavior;
                                    BEH.Definition = model.BasicBehave[l].Definition;
                                    BEH.Antecedent = model.BasicBehave[l].Antecedent;
                                    BEH.FCT = model.BasicBehave[l].FCT;
                                    BEH.Consequence = model.BasicBehave[l].Consequances;
                                    BEH.ModifiedBy = sess.LoginId;
                                    BEH.ModifiedOn = DateTime.Now;
                                    dbobj.SaveChanges();
                                }

                                else
                                {
                                    basicBehavior = new BasicBehavioralInformation();
                                    basicBehavior.StudentPersonalId = sp.StudentPersonalId;
                                    basicBehavior.SchoolId = sess.SchoolId;

                                    basicBehavior.TargetBehavior = model.BasicBehave[l].TargetBehavior;
                                    basicBehavior.Definition = model.BasicBehave[l].Definition;
                                    basicBehavior.Antecedent = model.BasicBehave[l].Antecedent;
                                    basicBehavior.FCT = model.BasicBehave[l].FCT;
                                    basicBehavior.Consequence = model.BasicBehave[l].Consequances;
                                    basicBehavior.ModifiedBy = sess.LoginId;
                                    basicBehavior.ModifiedOn = DateTime.Now;
                                    dbobj.BasicBehavioralInformations.Add(basicBehavior);
                                    dbobj.SaveChanges();
                                }
                            }
                        }

                        //var BasicBehave = dbobj.BasicBehavioralInformations.Where(objBasicBehave => objBasicBehave.StudentPersonalId == sess.ReferralId
                        //    && objBasicBehave.SchoolId == sess.SchoolId).ToList();
                        //i = 0;

                        //if (BasicBehave.Count > 0)
                        //{
                        //    foreach (var item in BasicBehave)
                        //    {

                        //        basicBehavior = dbobj.BasicBehavioralInformations.Where(objBasicBehav => objBasicBehav.BasicBehavioralInformationId == item.BasicBehavioralInformationId
                        //            && objBasicBehav.SchoolId == sess.SchoolId).SingleOrDefault();
                        //        basicBehavior.TargetBehavior = model.BasicBehave[i].TargetBehavior;
                        //        basicBehavior.Definition = model.BasicBehave[i].Definition;
                        //        basicBehavior.Antecedent = model.BasicBehave[i].Antecedent;
                        //        basicBehavior.FCT = model.BasicBehave[i].FCT;
                        //        basicBehavior.Consequence = model.BasicBehave[i].Consequances;
                        //        basicBehavior.ModifiedBy = sess.LoginId;
                        //        basicBehavior.ModifiedOn = DateTime.Now;
                        //        dbobj.SaveChanges();
                        //        i++;
                        //    }
                        //}
                        //else
                        //{
                        //    basicBehavior.StudentPersonalId = sp.StudentPersonalId;
                        //    basicBehavior.SchoolId = sess.SchoolId;
                        //    basicBehavior.TargetBehavior = model.BasicBehave[i].TargetBehavior;
                        //    basicBehavior.Definition = model.BasicBehave[i].Definition;
                        //    basicBehavior.Antecedent = model.BasicBehave[i].Antecedent;
                        //    basicBehavior.FCT = model.BasicBehave[i].FCT;
                        //    basicBehavior.Consequence = model.BasicBehave[i].Consequances;
                        //    basicBehavior.CreatedBy = sess.LoginId;
                        //    basicBehavior.CreatedOn = DateTime.Now;
                        //    dbobj.BasicBehavioralInformations.Add(basicBehavior);
                        //    dbobj.SaveChanges();

                        //}

                        var behave = dbobj.BehavioursPAs.Where(objBehav => objBehav.StudentPersonalId == sess.ReferralId && objBehav.SchoolId == sess.SchoolId && objBehav.ParentId > 0).ToList();
                        if (behave.Count == 0)
                        {
                            saveBehaviors(behavior, sess, model);
                        }
                        else
                        {
                            int parentId = behavId("LIFTING / TRANSFERS");
                            bool flag = false;
                            foreach (var item in behave)
                            {
                                if (item.ParentId == parentId)
                                {
                                    if (!flag)
                                    {
                                        behavior = dbobj.BehavioursPAs.Where(objBehavs => objBehavs.BehavioursPAId == item.BehavioursPAId).SingleOrDefault();
                                        behavior.BehaviourName = model.LiftingOrTransfers1;
                                        behavior.ModifiedBy = sess.LoginId;
                                        behavior.ModifiedOn = DateTime.Now;
                                        dbobj.SaveChanges();
                                        flag = true;
                                    }
                                    else
                                    {
                                        behavior = dbobj.BehavioursPAs.Where(objBehavs => objBehavs.BehavioursPAId == item.BehavioursPAId).SingleOrDefault();
                                        behavior.BehaviourName = model.LiftingOrTransfers2;
                                        behavior.ModifiedBy = sess.LoginId;
                                        behavior.ModifiedOn = DateTime.Now;
                                        dbobj.SaveChanges();
                                    }
                                }
                            }

                            parentId = behavId("AMBULATION");
                            flag = false;
                            foreach (var item in behave)
                            {
                                if (item.ParentId == parentId)
                                {
                                    if (!flag)
                                    {
                                        behavior = dbobj.BehavioursPAs.Where(objBehavs => objBehavs.BehavioursPAId == item.BehavioursPAId).SingleOrDefault();
                                        behavior.BehaviourName = model.Ambulation1;
                                        behavior.ModifiedBy = sess.LoginId;
                                        behavior.ModifiedOn = DateTime.Now;
                                        dbobj.SaveChanges();
                                        flag = true;
                                    }
                                    else
                                    {
                                        behavior = dbobj.BehavioursPAs.Where(objBehavs => objBehavs.BehavioursPAId == item.BehavioursPAId).SingleOrDefault();
                                        behavior.BehaviourName = model.Ambulation2;
                                        behavior.ModifiedBy = sess.LoginId;
                                        behavior.ModifiedOn = DateTime.Now;
                                        dbobj.SaveChanges();
                                    }
                                }
                            }

                            parentId = behavId("TOILETING");
                            flag = false;
                            foreach (var item in behave)
                            {
                                if (item.ParentId == parentId)
                                {
                                    if (!flag)
                                    {
                                        behavior = dbobj.BehavioursPAs.Where(objBehavs => objBehavs.BehavioursPAId == item.BehavioursPAId).SingleOrDefault();
                                        behavior.BehaviourName = model.Toileting1;
                                        behavior.ModifiedBy = sess.LoginId;
                                        behavior.ModifiedOn = DateTime.Now;
                                        dbobj.SaveChanges();
                                        flag = true;
                                    }
                                    else
                                    {
                                        behavior = dbobj.BehavioursPAs.Where(objBehavs => objBehavs.BehavioursPAId == item.BehavioursPAId).SingleOrDefault();
                                        behavior.BehaviourName = model.Toileting2;
                                        behavior.ModifiedBy = sess.LoginId;
                                        behavior.ModifiedOn = DateTime.Now;
                                        dbobj.SaveChanges();
                                    }
                                }
                            }

                            parentId = behavId("EATING");
                            flag = false;
                            foreach (var item in behave)
                            {
                                if (item.ParentId == parentId)
                                {
                                    if (!flag)
                                    {
                                        behavior = dbobj.BehavioursPAs.Where(objBehavs => objBehavs.BehavioursPAId == item.BehavioursPAId).SingleOrDefault();
                                        behavior.BehaviourName = model.Eating1;
                                        behavior.ModifiedBy = sess.LoginId;
                                        behavior.ModifiedOn = DateTime.Now;
                                        dbobj.SaveChanges();
                                        flag = true;
                                    }
                                    else
                                    {
                                        behavior = dbobj.BehavioursPAs.Where(objBehavs => objBehavs.BehavioursPAId == item.BehavioursPAId).SingleOrDefault();
                                        behavior.BehaviourName = model.Eating2;
                                        behavior.ModifiedBy = sess.LoginId;
                                        behavior.ModifiedOn = DateTime.Now;
                                        dbobj.SaveChanges();
                                    }
                                }
                            }

                            parentId = behavId("SHOWERING");
                            flag = false;
                            foreach (var item in behave)
                            {
                                if (item.ParentId == parentId)
                                {
                                    if (!flag)
                                    {
                                        behavior = dbobj.BehavioursPAs.Where(objBehavs => objBehavs.BehavioursPAId == item.BehavioursPAId).SingleOrDefault();
                                        behavior.BehaviourName = model.Showering1;
                                        behavior.ModifiedBy = sess.LoginId;
                                        behavior.ModifiedOn = DateTime.Now;
                                        dbobj.SaveChanges();
                                        flag = true;
                                    }
                                    else
                                    {
                                        behavior = dbobj.BehavioursPAs.Where(objBehavs => objBehavs.BehavioursPAId == item.BehavioursPAId).SingleOrDefault();
                                        behavior.BehaviourName = model.Showering2;
                                        behavior.ModifiedBy = sess.LoginId;
                                        behavior.ModifiedOn = DateTime.Now;
                                        dbobj.SaveChanges();
                                    }
                                }
                            }

                            parentId = behavId("TOOTHBRUSHING");
                            flag = false;
                            foreach (var item in behave)
                            {
                                if (item.ParentId == parentId)
                                {
                                    if (!flag)
                                    {
                                        behavior = dbobj.BehavioursPAs.Where(objBehavs => objBehavs.BehavioursPAId == item.BehavioursPAId).SingleOrDefault();
                                        behavior.BehaviourName = model.ToothBrushing1;
                                        behavior.ModifiedBy = sess.LoginId;
                                        behavior.ModifiedOn = DateTime.Now;
                                        dbobj.SaveChanges();
                                        flag = true;
                                    }
                                    else
                                    {
                                        behavior = dbobj.BehavioursPAs.Where(objBehavs => objBehavs.BehavioursPAId == item.BehavioursPAId).SingleOrDefault();
                                        behavior.BehaviourName = model.ToothBrushing2;
                                        behavior.ModifiedBy = sess.LoginId;
                                        behavior.ModifiedOn = DateTime.Now;
                                        dbobj.SaveChanges();
                                    }
                                }
                            }

                            parentId = behavId("DRESSING");
                            flag = false;
                            foreach (var item in behave)
                            {
                                if (item.ParentId == parentId)
                                {
                                    if (!flag)
                                    {
                                        behavior = dbobj.BehavioursPAs.Where(objBehavs => objBehavs.BehavioursPAId == item.BehavioursPAId).SingleOrDefault();
                                        behavior.BehaviourName = model.Dressing1;
                                        behavior.ModifiedBy = sess.LoginId;
                                        behavior.ModifiedOn = DateTime.Now;
                                        dbobj.SaveChanges();
                                        flag = true;
                                    }
                                    else
                                    {
                                        behavior = dbobj.BehavioursPAs.Where(objBehavs => objBehavs.BehavioursPAId == item.BehavioursPAId).SingleOrDefault();
                                        behavior.BehaviourName = model.Dressing2;
                                        behavior.ModifiedBy = sess.LoginId;
                                        behavior.ModifiedOn = DateTime.Now;
                                        dbobj.SaveChanges();
                                    }
                                }
                            }

                            parentId = behavId("SKIN CARE/SKIN INTEGRITY");
                            flag = false;
                            foreach (var item in behave)
                            {
                                if (item.ParentId == parentId)
                                {
                                    if (!flag)
                                    {
                                        behavior = dbobj.BehavioursPAs.Where(objBehavs => objBehavs.BehavioursPAId == item.BehavioursPAId).SingleOrDefault();
                                        behavior.BehaviourName = model.SkinCare1;
                                        behavior.ModifiedBy = sess.LoginId;
                                        behavior.ModifiedOn = DateTime.Now;
                                        dbobj.SaveChanges();
                                        flag = true;
                                    }
                                    else
                                    {
                                        behavior = dbobj.BehavioursPAs.Where(objBehavs => objBehavs.BehavioursPAId == item.BehavioursPAId).SingleOrDefault();
                                        behavior.BehaviourName = model.SkinCare2;
                                        behavior.ModifiedBy = sess.LoginId;
                                        behavior.ModifiedOn = DateTime.Now;
                                        dbobj.SaveChanges();
                                    }
                                }
                            }

                            parentId = behavId("COMMUNICATION");
                            flag = false;
                            foreach (var item in behave)
                            {
                                if (item.ParentId == parentId)
                                {
                                    if (!flag)
                                    {
                                        behavior = dbobj.BehavioursPAs.Where(objBehavs => objBehavs.BehavioursPAId == item.BehavioursPAId).SingleOrDefault();
                                        behavior.BehaviourName = model.Communication1;
                                        behavior.ModifiedBy = sess.LoginId;
                                        behavior.ModifiedOn = DateTime.Now;
                                        dbobj.SaveChanges();
                                        flag = true;
                                    }
                                    else
                                    {
                                        behavior = dbobj.BehavioursPAs.Where(objBehavs => objBehavs.BehavioursPAId == item.BehavioursPAId).SingleOrDefault();
                                        behavior.BehaviourName = model.Communication2;
                                        behavior.ModifiedBy = sess.LoginId;
                                        behavior.ModifiedOn = DateTime.Now;
                                        dbobj.SaveChanges();
                                    }
                                }
                            }


                            parentId = behavId("PREFERRED ACTIVITIES");
                            flag = false;
                            foreach (var item in behave)
                            {
                                if (item.ParentId == parentId)
                                {
                                    if (!flag)
                                    {
                                        behavior = dbobj.BehavioursPAs.Where(objBehavs => objBehavs.BehavioursPAId == item.BehavioursPAId).SingleOrDefault();
                                        behavior.BehaviourName = model.preferedActivities1;
                                        behavior.ModifiedBy = sess.LoginId;
                                        behavior.ModifiedOn = DateTime.Now;
                                        dbobj.SaveChanges();
                                        flag = true;
                                    }
                                    else
                                    {
                                        behavior = dbobj.BehavioursPAs.Where(objBehavs => objBehavs.BehavioursPAId == item.BehavioursPAId).SingleOrDefault();
                                        behavior.BehaviourName = model.preferedActivities2;
                                        behavior.ModifiedBy = sess.LoginId;
                                        behavior.ModifiedOn = DateTime.Now;
                                        dbobj.SaveChanges();
                                    }
                                }
                            }

                            parentId = behavId("GENERAL INFORMATION");
                            flag = false;
                            foreach (var item in behave)
                            {
                                if (item.ParentId == parentId)
                                {
                                    if (!flag)
                                    {
                                        behavior = dbobj.BehavioursPAs.Where(objBehavs => objBehavs.BehavioursPAId == item.BehavioursPAId).SingleOrDefault();
                                        behavior.BehaviourName = model.GeneralInformation1;
                                        behavior.ModifiedBy = sess.LoginId;
                                        behavior.ModifiedOn = DateTime.Now;
                                        dbobj.SaveChanges();
                                        flag = true;
                                    }
                                    else
                                    {
                                        behavior = dbobj.BehavioursPAs.Where(objBehavs => objBehavs.BehavioursPAId == item.BehavioursPAId).SingleOrDefault();
                                        behavior.BehaviourName = model.GeneralInformation2;
                                        behavior.ModifiedBy = sess.LoginId;
                                        behavior.ModifiedOn = DateTime.Now;
                                        dbobj.SaveChanges();
                                    }
                                }
                            }

                            parentId = behavId("SUGGESTED PROACTIVE ENVIRONMENTAL PROCEDURES");
                            flag = false;
                            foreach (var item in behave)
                            {
                                if (item.ParentId == parentId)
                                {
                                    if (!flag)
                                    {
                                        behavior = dbobj.BehavioursPAs.Where(objBehavs => objBehavs.BehavioursPAId == item.BehavioursPAId).SingleOrDefault();
                                        behavior.BehaviourName = model.SuggestedProactiveEnvironmentalProcedures1;
                                        behavior.ModifiedBy = sess.LoginId;
                                        behavior.ModifiedOn = DateTime.Now;
                                        dbobj.SaveChanges();
                                        flag = true;
                                    }
                                    else
                                    {
                                        behavior = dbobj.BehavioursPAs.Where(objBehavs => objBehavs.BehavioursPAId == item.BehavioursPAId).SingleOrDefault();
                                        behavior.BehaviourName = model.SuggestedProactiveEnvironmentalProcedures2;
                                        behavior.ModifiedBy = sess.LoginId;
                                        behavior.ModifiedOn = DateTime.Now;
                                        dbobj.SaveChanges();
                                    }
                                }
                            }
                        }
                        Result = "Sucess";
                    }
                    Result = "Sucess";
                    trans.Complete();
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
                Result = "Failed";
            }
            return Result;
        }

        private void saveBehaviors(BehavioursPA behavior, clsSession sess, ClientRegistrationPAModel model)
        {

            int parentId = behavId("LIFTING / TRANSFERS");
            behavior.StudentPersonalId = sess.ReferralId;
            behavior.SchoolId = sess.SchoolId;
            behavior.ParentId = parentId;
            behavior.BehaviourName = model.LiftingOrTransfers1;
            behavior.CreatedBy = sess.LoginId;
            behavior.CreatedOn = DateTime.Now;
            dbobj.BehavioursPAs.Add(behavior);
            dbobj.SaveChanges();
            behavior.StudentPersonalId = sess.ReferralId;
            behavior.ParentId = parentId;
            behavior.SchoolId = sess.SchoolId;
            behavior.BehaviourName = model.LiftingOrTransfers2;
            behavior.CreatedBy = sess.LoginId;
            behavior.CreatedOn = DateTime.Now;
            dbobj.BehavioursPAs.Add(behavior);
            dbobj.SaveChanges();

            parentId = behavId("AMBULATION");
            behavior.StudentPersonalId = sess.ReferralId;
            behavior.ParentId = parentId;
            behavior.SchoolId = sess.SchoolId;
            behavior.BehaviourName = model.Ambulation1;
            behavior.CreatedBy = sess.LoginId;
            behavior.CreatedOn = DateTime.Now;
            dbobj.BehavioursPAs.Add(behavior);
            dbobj.SaveChanges();
            behavior.StudentPersonalId = sess.ReferralId;
            behavior.ParentId = parentId;
            behavior.SchoolId = sess.SchoolId;
            behavior.BehaviourName = model.Ambulation2;
            behavior.CreatedBy = sess.LoginId;
            behavior.CreatedOn = DateTime.Now;
            dbobj.BehavioursPAs.Add(behavior);
            dbobj.SaveChanges();

            parentId = behavId("TOILETING");
            behavior.StudentPersonalId = sess.ReferralId;
            behavior.ParentId = parentId;
            behavior.SchoolId = sess.SchoolId;
            behavior.BehaviourName = model.Toileting1;
            behavior.CreatedBy = sess.LoginId;
            behavior.CreatedOn = DateTime.Now;
            dbobj.BehavioursPAs.Add(behavior);
            dbobj.SaveChanges();
            behavior.StudentPersonalId = sess.ReferralId;
            behavior.ParentId = parentId;
            behavior.SchoolId = sess.SchoolId;
            behavior.BehaviourName = model.Toileting2;
            behavior.CreatedBy = sess.LoginId;
            behavior.CreatedOn = DateTime.Now;
            dbobj.BehavioursPAs.Add(behavior);
            dbobj.SaveChanges();

            parentId = behavId("EATING");
            behavior.StudentPersonalId = sess.ReferralId;
            behavior.ParentId = parentId;
            behavior.SchoolId = sess.SchoolId;
            behavior.BehaviourName = model.Eating1;
            behavior.CreatedBy = sess.LoginId;
            behavior.CreatedOn = DateTime.Now;
            dbobj.BehavioursPAs.Add(behavior);
            dbobj.SaveChanges();
            behavior.StudentPersonalId = sess.ReferralId;
            behavior.ParentId = parentId;
            behavior.SchoolId = sess.SchoolId;
            behavior.BehaviourName = model.Eating2;
            behavior.CreatedBy = sess.LoginId;
            behavior.CreatedOn = DateTime.Now;
            dbobj.BehavioursPAs.Add(behavior);
            dbobj.SaveChanges();

            parentId = behavId("SHOWERING");
            behavior.StudentPersonalId = sess.ReferralId;
            behavior.ParentId = parentId;
            behavior.SchoolId = sess.SchoolId;
            behavior.BehaviourName = model.Showering1;
            behavior.CreatedBy = sess.LoginId;
            behavior.CreatedOn = DateTime.Now;
            dbobj.BehavioursPAs.Add(behavior);
            dbobj.SaveChanges();
            behavior.StudentPersonalId = sess.ReferralId;
            behavior.ParentId = parentId;
            behavior.SchoolId = sess.SchoolId;
            behavior.BehaviourName = model.Showering2;
            behavior.CreatedBy = sess.LoginId;
            behavior.CreatedOn = DateTime.Now;
            dbobj.BehavioursPAs.Add(behavior);
            dbobj.SaveChanges();

            parentId = behavId("TOOTHBRUSHING");
            behavior.StudentPersonalId = sess.ReferralId;
            behavior.ParentId = parentId;
            behavior.SchoolId = sess.SchoolId;
            behavior.BehaviourName = model.ToothBrushing1;
            behavior.CreatedBy = sess.LoginId;
            behavior.CreatedOn = DateTime.Now;
            dbobj.BehavioursPAs.Add(behavior);
            dbobj.SaveChanges();
            behavior.StudentPersonalId = sess.ReferralId;
            behavior.ParentId = parentId;
            behavior.SchoolId = sess.SchoolId;
            behavior.BehaviourName = model.ToothBrushing2;
            behavior.CreatedBy = sess.LoginId;
            behavior.CreatedOn = DateTime.Now;
            dbobj.BehavioursPAs.Add(behavior);
            dbobj.SaveChanges();

            parentId = behavId("DRESSING");
            behavior.StudentPersonalId = sess.ReferralId;
            behavior.ParentId = parentId;
            behavior.SchoolId = sess.SchoolId;
            behavior.BehaviourName = model.Dressing1;
            behavior.CreatedBy = sess.LoginId;
            behavior.CreatedOn = DateTime.Now;
            dbobj.BehavioursPAs.Add(behavior);
            dbobj.SaveChanges();
            behavior.StudentPersonalId = sess.ReferralId;
            behavior.ParentId = parentId;
            behavior.SchoolId = sess.SchoolId;
            behavior.BehaviourName = model.Dressing2;
            behavior.CreatedBy = sess.LoginId;
            behavior.CreatedOn = DateTime.Now;
            dbobj.BehavioursPAs.Add(behavior);
            dbobj.SaveChanges();

            parentId = behavId("SKIN CARE/SKIN INTEGRITY");
            behavior.StudentPersonalId = sess.ReferralId;
            behavior.ParentId = parentId;
            behavior.SchoolId = sess.SchoolId;
            behavior.BehaviourName = model.SkinCare1;
            behavior.CreatedBy = sess.LoginId;
            behavior.CreatedOn = DateTime.Now;
            dbobj.BehavioursPAs.Add(behavior);
            dbobj.SaveChanges();
            behavior.StudentPersonalId = sess.ReferralId;
            behavior.ParentId = parentId;
            behavior.SchoolId = sess.SchoolId;
            behavior.BehaviourName = model.SkinCare2;
            behavior.CreatedBy = sess.LoginId;
            behavior.CreatedOn = DateTime.Now;
            dbobj.BehavioursPAs.Add(behavior);
            dbobj.SaveChanges();

            parentId = behavId("COMMUNICATION");
            behavior.StudentPersonalId = sess.ReferralId;
            behavior.ParentId = parentId;
            behavior.SchoolId = sess.SchoolId;
            behavior.BehaviourName = model.Communication1;
            behavior.CreatedBy = sess.LoginId;
            behavior.CreatedOn = DateTime.Now;
            dbobj.BehavioursPAs.Add(behavior);
            dbobj.SaveChanges();
            behavior.StudentPersonalId = sess.ReferralId;
            behavior.ParentId = parentId;
            behavior.SchoolId = sess.SchoolId;
            behavior.BehaviourName = model.Communication2;
            behavior.CreatedBy = sess.LoginId;
            behavior.CreatedOn = DateTime.Now;
            dbobj.BehavioursPAs.Add(behavior);
            dbobj.SaveChanges();


            parentId = behavId("PREFERRED ACTIVITIES");
            behavior.StudentPersonalId = sess.ReferralId;
            behavior.ParentId = parentId;
            behavior.SchoolId = sess.SchoolId;
            behavior.BehaviourName = model.preferedActivities1;
            behavior.CreatedBy = sess.LoginId;
            behavior.CreatedOn = DateTime.Now;
            dbobj.BehavioursPAs.Add(behavior);
            dbobj.SaveChanges();
            behavior.StudentPersonalId = sess.ReferralId;
            behavior.ParentId = parentId;
            behavior.SchoolId = sess.SchoolId;
            behavior.BehaviourName = model.preferedActivities2;
            behavior.CreatedBy = sess.LoginId;
            behavior.CreatedOn = DateTime.Now;
            dbobj.BehavioursPAs.Add(behavior);
            dbobj.SaveChanges();

            parentId = behavId("GENERAL INFORMATION");
            behavior.StudentPersonalId = sess.ReferralId;
            behavior.ParentId = parentId;
            behavior.SchoolId = sess.SchoolId;
            behavior.BehaviourName = model.GeneralInformation1;
            behavior.CreatedBy = sess.LoginId;
            behavior.CreatedOn = DateTime.Now;
            dbobj.BehavioursPAs.Add(behavior);
            dbobj.SaveChanges();
            behavior.StudentPersonalId = sess.ReferralId;
            behavior.ParentId = parentId;
            behavior.SchoolId = sess.SchoolId;
            behavior.BehaviourName = model.GeneralInformation2;
            behavior.CreatedBy = sess.LoginId;
            behavior.CreatedOn = DateTime.Now;
            dbobj.BehavioursPAs.Add(behavior);
            dbobj.SaveChanges();

            parentId = behavId("SUGGESTED PROACTIVE ENVIRONMENTAL PROCEDURES");
            behavior.StudentPersonalId = sess.ReferralId;
            behavior.ParentId = parentId;
            behavior.SchoolId = sess.SchoolId;
            behavior.BehaviourName = model.SuggestedProactiveEnvironmentalProcedures1;
            behavior.CreatedBy = sess.LoginId;
            behavior.CreatedOn = DateTime.Now;
            dbobj.BehavioursPAs.Add(behavior);
            dbobj.SaveChanges();
            behavior.StudentPersonalId = sess.ReferralId;
            behavior.ParentId = parentId;
            behavior.SchoolId = sess.SchoolId;
            behavior.BehaviourName = model.SuggestedProactiveEnvironmentalProcedures2;
            behavior.CreatedBy = sess.LoginId;
            behavior.CreatedOn = DateTime.Now;
            dbobj.BehavioursPAs.Add(behavior);
            dbobj.SaveChanges();

        }


        public int behavId(string parentname)
        {
            int parentId = 0;
            dbobj = new MelmarkDBEntities();
            BehaveLookup behav = new BehaveLookup();
            behav = dbobj.BehaveLookups.Where(objBehave => objBehave.BehaviouralName == parentname).SingleOrDefault();
            if (behav != null) parentId = behav.BehaviouralId;
            else parentId = 0;
            return parentId;

        }





        /// <summary>
        /// Function for upload and Save Client Image.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="model"></param>
        /// <returns></returns>

        public string SaveImage(HttpPostedFileBase file, ImageUploader model)
        {
            string result = "";

            string dirpath = AppDomain.CurrentDomain.BaseDirectory + "Images/StudentImages/";
            try
            {

                if (file != null)
                {
                    if (file.ContentLength < 512000)
                    {
                        string[] fileIesplit = file.FileName.Split('\\');
                        if (Directory.Exists(dirpath))
                        {


                            string[] temp = fileIesplit[fileIesplit.Length - 1].Split('.');

                            //    file.SaveAs(dirpath + "\\" + userSessionObj.id.ToString() + "-" + formData["filetype"] + "-1." + temp[temp.Length - 1]);


                        }
                        else
                        {
                            Directory.CreateDirectory(dirpath);

                            string[] temp = fileIesplit[fileIesplit.Length - 1].Split('.');


                            //    file.SaveAs(dirpath + "\\" + userSessionObj.id.ToString() + "-" + formData["filetype"] + "-1." + temp[temp.Length - 1]);

                        }

                    }
                    else
                    {
                        result = "fileError";
                    }


                }
                dbobj.SaveChanges();
                result = "Sucess";
            }
            catch
            {
                result = "failed";
            }

            return result;
        }



        /// <summary>
        /// Function To load the Client Data.
        /// </summary>
        /// <param name="StudentId"></param>
        /// <returns></returns>

        public RegistrationModel bindCliendData(int StudentId)
        {
            // sess = (clsSession)HttpContext.Current.Session["UserSession"];

            RegistrationModel regModel = new RegistrationModel();
            dbobj = new MelmarkDBEntities();
            StudentPersonal student = new StudentPersonal();
            AddressList address = new AddressList();
            StudentAddresRel stdAddrRel = new StudentAddresRel();
            EmergencyContactSchool EmergencyContact = new EmergencyContactSchool();
            SchoolsAttended SchoolAttended = new SchoolsAttended();
            LookUp objLookUp = new LookUp();
            Insurance objInsurance = new Insurance();

            try
            {
                student = dbobj.StudentPersonals.Where(x => x.StudentPersonalId == StudentId).SingleOrDefault();
                stdAddrRel = dbobj.StudentAddresRels.Where(x => x.StudentPersonalId == StudentId && x.ContactSequence == 0).SingleOrDefault();
                address = dbobj.AddressLists.Where(x => x.AddressId == stdAddrRel.AddressId && x.AddressType == 0).SingleOrDefault();

                var EmergencyContacts = dbobj.EmergencyContactSchools.Where(objEmergencyContact => objEmergencyContact.StudentPersonalId == StudentId).ToList();

                var SchoolsAttended = dbobj.SchoolsAttendeds.Where(objSchoolAttended => objSchoolAttended.StudentPersonalId == StudentId).ToList();



                try
                {
                    regModel.InsuranceList = dbobj.Insurances.Where(objInsur => objInsur.StudentPersonalId == StudentId && objInsur.PreferType == "Primary").ToList();


                    //regModel.InsuranceList = (from objI in objIns                                             
                    //                          select new Insurance
                    //                          {
                    //                              InsuranceType = objInsurance.InsuranceType,
                    //                              PolicyNumber = objInsurance.PolicyNumber,
                    //                              PolicyHolder = objInsurance.PolicyHolder
                    //                          }).ToList();


                    if (regModel.InsuranceList != null)
                    {
                        foreach (var item in regModel.InsuranceList)
                        {
                            regModel.InsuranceType = item.InsuranceType;
                            regModel.PolicyHolder = item.PolicyHolder;
                            regModel.PolicyNumber = item.PolicyNumber;
                        }

                    }
                }
                catch
                {

                }
                regModel.Id = student.StudentPersonalId;
                regModel.FirstName = student.FirstName;
                regModel.LastName = student.LastName;
                regModel.MiddleName = student.MiddleName;
                regModel.NickName = student.NickName;
                regModel.LastNameSuffix = student.Suffix;
                regModel.DateOfBirth = ConvertDate(student.BirthDate);
                regModel.PlaceOfBirth = student.PlaceOfBirth;
                regModel.CountryofBirth = student.CountryOfBirth;
                regModel.StateOfBirth = student.StateOfBirth;
                try
                {
                    objLookUp = dbobj.LookUps.Where(objlukUp => objlukUp.LookupId == student.CountryOfBirth && objlukUp.LookupType == "Country").SingleOrDefault();
                    regModel.CountryBirth = objLookUp.LookupName;

                }
                catch
                {
                    regModel.CountryBirth = "";
                }
                try
                {
                    objLookUp = dbobj.LookUps.Where(objlukUp => objlukUp.LookupId == student.StateOfBirth && objlukUp.LookupType == "State").SingleOrDefault();
                    regModel.StateBirth = objLookUp.LookupName;
                }
                catch
                {
                    regModel.StateBirth = "";
                }
                regModel.Citizenship = Convert.ToInt32(student.CitizenshipStatus);
                regModel.Race = Convert.ToInt32(student.RaceId);
                regModel.Gender = student.Gender;
                regModel.HairColor = student.HairColor;
                regModel.EyeColor = student.EyeColor;
                regModel.AdmissinDate = ConvertDate(student.AdmissionDate);
                regModel.Height = student.Height.ToString();
                if (student.Height == null)
                {
                    regModel.Height = "";
                }
                regModel.Weight = student.Weight.ToString();
                if (student.Weight == null)
                {
                    regModel.Height = "";
                }

                regModel.PrimaryLanguage = student.PrimaryLanguage;
                regModel.GuardianshipStatus = student.GuardianShip;
                regModel.DistigushingMarks = student.DistingushingMarks;
                regModel.LegalCompetencyStatus = student.LegalCompetencyStatus;
                regModel.OtherStateAgenciesInvolvedWithStudent = student.OtherStateAgenciesInvolvedWithStudent;
                regModel.MaritalStatusofBothParents = student.MaritalStatusofBothParents;
                regModel.CaseManagerEducational = student.CaseManagerEducational;
                regModel.CaseManagerResidential = student.CaseManagerResidential;
                regModel.ImageUrl = student.ImageUrl;
                regModel.PhotoReleasePermission = student.ImagePermission;
                regModel.DateInitiallyEligibleforSpecialEducation = ConvertDate(student.DateInitiallyEligibleforSpecialEducation);
                regModel.DateofMostRecentSpecialEducationEvaluations = ConvertDate(student.DateofMostRecentSpecialEducationEvaluations);
                regModel.DateofNextScheduled3YearEvaluation = ConvertDate(student.DateofNextScheduled3YearEvaluation);
                regModel.CurrentIEPStartDate = ConvertDate(student.CurrentIEPStartDate);
                regModel.CurrentIEPExpirationDate = ConvertDate(student.CurrentIEPExpirationDate);
                regModel.DischargeDate = ConvertDate(student.DischargeDate);
                regModel.LocationAfterDischarge = student.LocationAfterDischarge;
                regModel.MelmarkNewEnglandsFollowUpResponsibilities = student.MelmarkNewEnglandsFollowUpResponsibilities;
                regModel.ReferralIEPFullName = student.IEPReferralFullName;
                regModel.ReferralIEPPhone = student.IEPReferralPhone;
                regModel.ReferralIEPReferringAgency = student.IEPReferralReferrinAgency;
                regModel.ReferralIEPSourceofTuition = student.IEPReferralSourceofTuition;
                regModel.ReferralIEPTitle = student.IEPReferralTitle;
                if (address != null)
                {
                    regModel.AddressLine1 = address.ApartmentType;
                    regModel.AddressLine2 = address.StreetName;
                    regModel.AddressLine3 = address.AddressLine3;
                    regModel.ZipCode = address.PostalCode;
                    regModel.City = address.City;
                    regModel.Country = address.CountryId;
                    regModel.State = address.StateProvince;
                    try
                    {
                        objLookUp = dbobj.LookUps.Where(objlukUp => objlukUp.LookupId == address.CountryId && objlukUp.LookupType == "Country").SingleOrDefault();
                        regModel.StrCountry = objLookUp.LookupName;
                    }
                    catch
                    {
                        regModel.StrCountry = "";
                    }
                    try
                    {
                        objLookUp = dbobj.LookUps.Where(objlukUp => objlukUp.LookupId == address.StateProvince && objlukUp.LookupType == "State").SingleOrDefault();
                        regModel.StrState = objLookUp.LookupName;
                    }
                    catch
                    {
                        regModel.StrState = "";
                    }
                }

                if (EmergencyContacts != null)
                {
                    foreach (var item in EmergencyContacts)
                    {
                        if (item.SequenceId == 1)
                        {
                            regModel.EmergencyContactFirstName1 = item.FirstName;
                            regModel.EmergencyContactLastName1 = item.LastName;
                            regModel.EmergencyContactTitle1 = item.Title;
                            regModel.EmergencyContactPhone1 = item.Phone;
                        }
                        if (item.SequenceId == 2)
                        {
                            regModel.EmergencyContactFirstName2 = item.FirstName;
                            regModel.EmergencyContactLastName2 = item.LastName;
                            regModel.EmergencyContactTitle2 = item.Title;
                            regModel.EmergencyContactPhone2 = item.Phone;
                        }
                        if (item.SequenceId == 3)
                        {
                            regModel.EmergencyContactFirstName3 = item.FirstName;
                            regModel.EmergencyContactLastName3 = item.LastName;
                            regModel.EmergencyContactTitle3 = item.Title;
                            regModel.EmergencyContactPhone3 = item.Phone;
                        }
                        if (item.SequenceId == 4)
                        {
                            regModel.EmergencyContactFirstName4 = item.FirstName;
                            regModel.EmergencyContactLastName4 = item.LastName;
                            regModel.EmergencyContactTitle4 = item.Title;
                            regModel.EmergencyContactPhone4 = item.Phone;
                        }
                        if (item.SequenceId == 5)
                        {
                            regModel.EmergencyContactFirstName5 = item.FirstName;
                            regModel.EmergencyContactLastName5 = item.LastName;
                            regModel.EmergencyContactTitle5 = item.Title;
                            regModel.EmergencyContactPhone5 = item.Phone;
                        }
                    }
                }
                if (SchoolsAttended != null)
                {
                    foreach (var item in SchoolsAttended)
                    {
                        if (item.SequenceId == 1)
                        {
                            regModel.SchoolName1 = item.SchoolName;
                            regModel.DateFrom1 = ConvertDate(item.DateFrom);
                            regModel.DateTo1 = ConvertDate(item.DateTo);
                            regModel.SchoolAttendedAddress11 = item.Address1;
                            regModel.SchoolAttendedAddress21 = item.Address2;
                            regModel.SchoolAttendedCity1 = item.City;
                            regModel.SchoolAttendedState1 = item.State;
                        }
                        if (item.SequenceId == 2)
                        {
                            regModel.SchoolName2 = item.SchoolName;
                            regModel.DateFrom2 = ConvertDate(item.DateFrom);
                            regModel.DateTo2 = ConvertDate(item.DateTo);
                            regModel.SchoolAttendedAddress12 = item.Address1;
                            regModel.SchoolAttendedAddress22 = item.Address2;
                            regModel.SchoolAttendedCity2 = item.City;
                            regModel.SchoolAttendedState2 = item.State;
                        }
                        if (item.SequenceId == 3)
                        {
                            regModel.SchoolName3 = item.SchoolName;
                            regModel.DateFrom3 = ConvertDate(item.DateFrom);
                            regModel.DateTo3 = ConvertDate(item.DateTo);
                            regModel.SchoolAttendedAddress13 = item.Address1;
                            regModel.SchoolAttendedAddress23 = item.Address2;
                            regModel.SchoolAttendedCity3 = item.City;
                            regModel.SchoolAttendedState3 = item.State;
                        }
                    }
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }

            return regModel;
        }



        /// <summary>
        /// Function To load the PA Client Data.
        /// </summary>
        /// <param name="StudentId"></param>
        /// <returns></returns>
        public ClientRegistrationPAModel bindCliendDataPA(int ClientId)
        {
            // sess = (clsSession)HttpContext.Current.Session["UserSession"];

            ClientRegistrationPAModel regModel = new ClientRegistrationPAModel();
            dbobj = new MelmarkDBEntities();
            StudentPersonal student = new StudentPersonal();
            StudentPersonalPA studentPA = new StudentPersonalPA();
            AddressList address = new AddressList();
            StudentAddresRel stdAddrRel = new StudentAddresRel();
            EmergencyContactSchool EmergencyContact = new EmergencyContactSchool();
            SchoolsAttended SchoolAttended = new SchoolsAttended();
            LookUp objLookUp = new LookUp();
            sess = (clsSession)HttpContext.Current.Session["UserSession"];
            Insurance objInsurance = new Insurance();
            try
            {
                student = dbobj.StudentPersonals.Where(x => x.StudentPersonalId == ClientId && x.SchoolId == sess.SchoolId).SingleOrDefault();
                studentPA = dbobj.StudentPersonalPAs.Where(objStudentPA => objStudentPA.StudentPersonalId == ClientId && objStudentPA.SchoolId == sess.SchoolId).SingleOrDefault();
                stdAddrRel = dbobj.StudentAddresRels.Where(x => x.StudentPersonalId == ClientId && x.ContactSequence == 0).SingleOrDefault();
                //AddressType is 0 for MelmarkPA (check for NE)
                address = dbobj.AddressLists.Where(x => x.AddressId == stdAddrRel.AddressId).SingleOrDefault();
                // var behave = dbobj.BehavioursPAs.Where(objBehave => objBehave.StudentPersonalId == sess.ClientId && objBehave.SchoolId == sess.SchoolId).ToList();
                var diagonis = dbobj.DiaganosesPAs.Where(objDiagno => objDiagno.StudentPersonalId == ClientId && objDiagno.SchoolId == sess.SchoolId).ToList();
                var adaptive = dbobj.AdaptiveEquipments.Where(objAdaptive => objAdaptive.StudentPersonalId == ClientId && objAdaptive.SchoolId == sess.SchoolId).ToList();
                var basicbehav = dbobj.BasicBehavioralInformations.Where(objBasic => objBasic.StudentPersonalId == ClientId && objBasic.SchoolId == sess.SchoolId).ToList();

                regModel.Id = student.StudentPersonalId;
                regModel.FirstName = student.FirstName;
                regModel.LastName = student.LastName;
                regModel.MiddleName = student.MiddleName;
                regModel.NickName = student.NickName;
                regModel.LastNameSuffix = student.Suffix;
                regModel.DateOfBirth = ConvertDate(student.BirthDate);
                regModel.PlaceOfBirth = student.PlaceOfBirth;
                regModel.CountryofBirth = student.CountryOfBirth;
                regModel.StateOfBirth = student.StateOfBirth;
                regModel.MostRecentGradeLevel = student.MostRecentGradeLevel;
                try
                {
                    objLookUp = dbobj.LookUps.Where(objlukUp => objlukUp.LookupId == student.CountryOfBirth && objlukUp.LookupType == "Country").SingleOrDefault();
                    regModel.CountryBirth = objLookUp.LookupName;

                }
                catch
                {
                    regModel.CountryBirth = "";
                }
                try
                {
                    objLookUp = dbobj.LookUps.Where(objlukUp => objlukUp.LookupId == student.StateOfBirth && objlukUp.LookupType == "State").SingleOrDefault();
                    regModel.StateBirth = objLookUp.LookupName;
                }
                catch
                {
                    regModel.StateBirth = "";
                }
                regModel.Citizenship = Convert.ToInt32(student.CitizenshipStatus);
                regModel.Race = Convert.ToInt32(student.RaceId);
                regModel.Gender = student.Gender;
                regModel.HairColor = student.HairColor;
                regModel.EyeColor = student.EyeColor;
                regModel.DateUpdated = ConvertDate(student.AdmissionDate);
                regModel.Height = student.Height.ToString();
                if (student.Height == null)
                {
                    regModel.Height = "";
                }
                regModel.Weight = student.Weight.ToString();
                if (student.Weight == null)
                {
                    regModel.Height = "";
                }
                regModel.PrimaryLanguage = student.PrimaryLanguage;
                regModel.GuardianshipStatus = student.GuardianShip;
                regModel.DistigushingMarks = student.DistingushingMarks;
                regModel.LegalCompetencyStatus = student.LegalCompetencyStatus;
                regModel.OtherStateAgenciesInvolvedWithStudent = student.OtherStateAgenciesInvolvedWithStudent;
                regModel.MaritalStatusofBothParents = student.MaritalStatusofBothParents;
                regModel.CaseManagerEducational = student.CaseManagerEducational;
                regModel.CaseManagerResidential = student.CaseManagerResidential;
                regModel.ImageUrl = student.ImageUrl;
                regModel.PhotoReleasePermission = student.ImagePermission;
                if (address != null)
                {
                    regModel.AddressLine1 = address.AddressLine1;
                    regModel.AddressLine2 = address.AddressLine2;
                    regModel.AddressLine3 = address.AddressLine3;
                    regModel.ZipCode = address.PostalCode;
                    regModel.City = address.City;
                    regModel.Country = address.CountryId;
                    regModel.State = address.StateProvince;
                }
                if (studentPA != null)
                {
                    regModel.Allergie = studentPA.Allergies;
                    regModel.Bathroom = studentPA.Bathroom;
                    regModel.BedroomAsleep = studentPA.ho_BedroomAsleep;
                    regModel.BedroomAwake = studentPA.ho_BedroomAwake;
                    regModel.Consciousness = studentPA.Consciousness;
                    regModel.CommonAreas = studentPA.CommonAreas;
                    regModel.Diet = studentPA.Diet;
                    regModel.Mobility = studentPA.Mobility;
                    regModel.NeedForExtraHelp = studentPA.NeedForExtraHelp;
                    regModel.OffCampus = studentPA.OffCampus;
                    regModel.OnCampus = studentPA.OnCampus;
                    regModel.Other = studentPA.Other;
                    regModel.PoolOrSwimming = studentPA.PoolOrSwimming;
                    regModel.ResponseToInstruction = studentPA.ResponseToInstruction;
                    regModel.RiskOfResistance = studentPA.RiskOfResistance;
                    regModel.Seizures = studentPA.Seizures;
                    regModel.TaskORBreak = studentPA.dy_TaskOrBreak;
                    regModel.TransitionInside = studentPA.dy_TransitionInside;
                    regModel.TransitionUnevenGround = studentPA.dy_TransitionUnevenGround;
                    regModel.van = studentPA.Van;
                    regModel.WalkingResponse = studentPA.WalkingResponses;
                    regModel.WhenTranspoting = studentPA.WhenTranspoting;
                    regModel.Funding = studentPA.FundingSource;
                }
                if (basicbehav.Count > 0)
                {
                    foreach (var item in basicbehav)
                    {
                        regModel.BasicBehave.Add(new BasicBehavior
                        {
                            Antecedent = item.Antecedent,
                            Consequances = item.Consequence,
                            Definition = item.Definition,
                            FCT = item.FCT,
                            TargetBehavior = item.TargetBehavior,
                            BasicBehavioralInformationId = item.BasicBehavioralInformationId
                        });

                    }
                }
                if (diagonis.Count > 0)
                {
                    foreach (var item in diagonis)
                    {
                        regModel.Diagnosis.Add(new Diagnosis
                        {
                            Name = item.Diaganoses
                        });

                    }
                }
                if (adaptive.Count > 0)
                {
                    foreach (var item in adaptive)
                    {
                        regModel.Adapt.Add(new AdaptiveEquipmentz
                        {
                            item = item.Item,
                            ScheduledForUss = item.ScheduleForUse,
                            StorageLocation = item.StorageLocation,
                            CleaningInstruction = item.CleaningInstruction,
                            AdaptiveEquimentId = item.AdaptiveEquipmentId
                        });

                    }
                }
                //foreach (var item in adaptive)
                //{
                //    regModel.Adapt.Add(new AdaptiveEquipmentz
                //    {
                //        item = item.Item,
                //        ScheduledForUss = item.ScheduleForUse,
                //        StorageLocation = item.StorageLocation,
                //        CleaningInstruction = item.CleaningInstruction
                //    });

                //}
                try
                {

                    objLookUp = dbobj.LookUps.Where(objlukUp => objlukUp.LookupId == address.CountryId && objlukUp.LookupType == "Country").SingleOrDefault();
                    regModel.StrCountry = objLookUp.LookupName;
                }
                catch
                {
                    regModel.StrCountry = "";
                }
                try
                {
                    objLookUp = dbobj.LookUps.Where(objlukUp => objlukUp.LookupId == address.CountryId && objlukUp.LookupType == "State").SingleOrDefault();
                    regModel.StrState = objLookUp.LookupName;
                }
                catch
                {
                    regModel.StrState = "";
                }


                var behave = dbobj.BehavioursPAs.Where(objBehav => objBehav.StudentPersonalId == ClientId && objBehav.SchoolId == sess.SchoolId && objBehav.ParentId > 0).ToList();
                if (behave.Count > 0)
                {
                    int parentId = behavId("LIFTING / TRANSFERS");
                    bool flag = false;
                    foreach (var item in behave)
                    {
                        if (item.ParentId == parentId)
                        {

                            if (!flag)
                            {
                                regModel.LiftingOrTransfers1 = item.BehaviourName;
                                flag = true;
                            }
                            else
                            {
                                regModel.LiftingOrTransfers2 = item.BehaviourName;
                            }

                        }
                    }

                    parentId = behavId("AMBULATION");
                    flag = false;
                    foreach (var item in behave)
                    {
                        if (item.ParentId == parentId)
                        {

                            if (!flag)
                            {
                                regModel.Ambulation1 = item.BehaviourName;
                                flag = true;
                            }
                            else
                            {
                                regModel.Ambulation2 = item.BehaviourName;
                            }

                        }
                    }

                    parentId = behavId("TOILETING");
                    flag = false;
                    foreach (var item in behave)
                    {
                        if (item.ParentId == parentId)
                        {

                            if (!flag)
                            {
                                regModel.Toileting1 = item.BehaviourName;
                                flag = true;
                            }
                            else
                            {
                                regModel.Toileting2 = item.BehaviourName;
                            }

                        }
                    }

                    parentId = behavId("EATING");
                    flag = false;
                    foreach (var item in behave)
                    {
                        if (item.ParentId == parentId)
                        {

                            if (!flag)
                            {
                                regModel.Eating1 = item.BehaviourName;
                                flag = true;
                            }
                            else
                            {
                                regModel.Eating2 = item.BehaviourName;
                            }

                        }
                    }

                    parentId = behavId("SHOWERING");
                    flag = false;
                    foreach (var item in behave)
                    {
                        if (item.ParentId == parentId)
                        {

                            if (!flag)
                            {
                                regModel.Showering1 = item.BehaviourName;
                                flag = true;
                            }
                            else
                            {
                                regModel.Showering2 = item.BehaviourName;
                            }

                        }
                    }

                    parentId = behavId("TOOTHBRUSHING");
                    flag = false;
                    foreach (var item in behave)
                    {
                        if (item.ParentId == parentId)
                        {

                            if (!flag)
                            {
                                regModel.ToothBrushing1 = item.BehaviourName;
                                flag = true;
                            }
                            else
                            {
                                regModel.ToothBrushing2 = item.BehaviourName;
                            }

                        }
                    }

                    parentId = behavId("DRESSING");
                    flag = false;
                    foreach (var item in behave)
                    {
                        if (item.ParentId == parentId)
                        {

                            if (!flag)
                            {
                                regModel.Dressing1 = item.BehaviourName;
                                flag = true;
                            }
                            else
                            {
                                regModel.Dressing2 = item.BehaviourName;
                            }

                        }
                    }

                    parentId = behavId("SKIN CARE/SKIN INTEGRITY");
                    flag = false;
                    foreach (var item in behave)
                    {
                        if (item.ParentId == parentId)
                        {

                            if (!flag)
                            {
                                regModel.SkinCare1 = item.BehaviourName;
                                flag = true;
                            }
                            else
                            {
                                regModel.SkinCare2 = item.BehaviourName;
                            }
                            ;
                        }
                    }

                    parentId = behavId("COMMUNICATION");
                    flag = false;
                    foreach (var item in behave)
                    {
                        if (item.ParentId == parentId)
                        {

                            if (!flag)
                            {
                                regModel.Communication1 = item.BehaviourName;
                                flag = true;
                            }
                            else
                            {
                                regModel.Communication2 = item.BehaviourName;
                            }

                        }
                    }


                    parentId = behavId("PREFERRED ACTIVITIES");
                    flag = false;
                    foreach (var item in behave)
                    {
                        if (item.ParentId == parentId)
                        {

                            if (!flag)
                            {
                                regModel.preferedActivities1 = item.BehaviourName;
                                flag = true;
                            }
                            else
                            {
                                regModel.preferedActivities2 = item.BehaviourName;
                            }

                        }
                    }

                    parentId = behavId("GENERAL INFORMATION");
                    flag = false;
                    foreach (var item in behave)
                    {
                        if (item.ParentId == parentId)
                        {

                            if (!flag)
                            {
                                regModel.GeneralInformation1 = item.BehaviourName;
                                flag = true;
                            }
                            else
                            {
                                regModel.GeneralInformation2 = item.BehaviourName;
                            }

                        }
                    }

                    parentId = behavId("SUGGESTED PROACTIVE ENVIRONMENTAL PROCEDURES");
                    flag = false;
                    foreach (var item in behave)
                    {
                        if (item.ParentId == parentId)
                        {

                            if (!flag)
                            {
                                regModel.SuggestedProactiveEnvironmentalProcedures1 = item.BehaviourName;
                                flag = true;
                            }
                            else
                            {
                                regModel.SuggestedProactiveEnvironmentalProcedures2 = item.BehaviourName;
                            }

                        }
                    }
                }



            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }

            return regModel;
        }



        /// <summary>
        /// Function is used to Fill the selected Contact Data..
        /// </summary>
        /// <param name="ClientId"></param>
        /// <param name="itemId"></param>
        /// <returns></returns>

        public ContactModel bindContactData(int ClientId, int itemId)
        {
            //sess = (clsSession)HttpContext.Current.Session["UserSession"];
            dbobj = new MelmarkDBEntities();
            ContactModel contactModel = new ContactModel();


            ContactPersonal contactPersonal = new ContactPersonal();
            StudentContactRelationship contactRelation = new StudentContactRelationship();
            StudentAddresRel addressRelation = new StudentAddresRel();
            AddressList adrList = new AddressList();
            contactPersonal = dbobj.ContactPersonals.Where(objContactPersonal => objContactPersonal.StudentPersonalId == ClientId &&
                objContactPersonal.ContactPersonalId == itemId).SingleOrDefault();

            if (contactPersonal != null)
            {
                contactModel.Id = contactPersonal.ContactPersonalId;
                contactModel.FirstName = contactPersonal.FirstName;
                contactModel.LastName = contactPersonal.LastName;
                contactModel.FirstNamePrefix = contactPersonal.Prefix;
                contactModel.LastNameSuffix = contactPersonal.Suffix;
                contactModel.Spouse = contactPersonal.Spouse;
                contactModel.MiddleName = contactPersonal.MiddleName;
                contactModel.ContactFlag = contactPersonal.ContactFlag;
                contactModel.PrimaryLanguage = contactPersonal.PrimaryLanguage;
                contactRelation = dbobj.StudentContactRelationships.Where(objContactrelation => objContactrelation.
                    ContactPersonalId == contactPersonal.ContactPersonalId).SingleOrDefault();
                contactModel.Relation = contactRelation.RelationshipId;
                LookUp lk = dbobj.LookUps.Where(objlk => objlk.LookupId == contactModel.Relation).SingleOrDefault();
                if (lk.LookupName == "Parent")
                {
                    Parent prnt = dbobj.Parents.Where(objparent => objparent.ContactPersonalId == contactPersonal.ContactPersonalId).SingleOrDefault();
                    contactModel.UserID = prnt.Username;
                }
                else
                    contactModel.UserID = "";

                var addresses = dbobj.StudentAddresRels.Join(dbobj.ContactPersonals, objAddresRel => objAddresRel.ContactPersonalId,
                    objContactPersonal => objContactPersonal.ContactPersonalId,
                (objAddresRel, objContactPersonal) => new
                {
                    ClientId = objAddresRel.StudentPersonalId,
                    ContactSequance = objAddresRel.ContactSequence,
                    AddressId = objAddresRel.AddressId,
                    ContactID = objAddresRel.ContactPersonalId
                }).
                Where(x => x.ClientId == ClientId && x.ContactSequance > 0 && x.ContactID == itemId).ToList().OrderBy(x => x.ContactSequance);



                // var contactList = dbobj.AddressLists.Where(x => x.AddressId == contactPersonal.ContactPersonalId).ToList();
                foreach (var item in addresses)
                {
                    if (item.ContactSequance == 1)
                    {
                        adrList = dbobj.AddressLists.Where(x => x.AddressId == item.AddressId).SingleOrDefault();
                        contactModel.HomeAddressTypeId = adrList.AddressType;
                        contactModel.HomeAddressLine1 = adrList.ApartmentType;
                        contactModel.HomeAddressLine2 = adrList.StreetName;
                        contactModel.HomeCity = adrList.City;
                        contactModel.HomeState = Convert.ToInt32(adrList.StateProvince);
                        contactModel.HomeCountry = Convert.ToInt32(adrList.CountryId);
                        contactModel.HomeCounty = adrList.County;
                        contactModel.HomePhone = adrList.Phone;
                        contactModel.HomeMobilePhone = adrList.Mobile;
                        contactModel.HomeWorkPhone = adrList.OtherPhone;
                        contactModel.HomeExtension = adrList.Extension;
                        contactModel.HomeFax = adrList.Fax;
                        contactModel.HomeEmail = adrList.PrimaryEmail;
                        contactModel.HomeWorkEmail = adrList.SecondryEmail;
                        contactModel.HomeZip = adrList.PostalCode;


                    }
                    if (item.ContactSequance == 2)
                    {
                        adrList = dbobj.AddressLists.Where(x => x.AddressId == item.AddressId).SingleOrDefault();
                        contactModel.WorkAddressTypeId = adrList.AddressType;
                        contactModel.WorkAddressLine1 = adrList.ApartmentType;
                        contactModel.WorkAddressLine2 = adrList.StreetName;
                        contactModel.WorkCity = adrList.City;
                        contactModel.WorkState = Convert.ToInt32(adrList.StateProvince);
                        contactModel.WorkCountry = Convert.ToInt32(adrList.CountryId);
                        contactModel.WorkCounty = adrList.County;
                        contactModel.WorkHomePhone = adrList.Phone;
                        contactModel.WorkMobilePhone = adrList.Mobile;
                        contactModel.WorkExtension = adrList.Extension;
                        contactModel.OtherExtension2 = adrList.Extension2;
                        contactModel.WorkPhone = adrList.OtherPhone;
                        contactModel.WorkFax = adrList.Fax;
                        contactModel.WorkHomeEmail = adrList.PrimaryEmail;
                        contactModel.WorkEmail = adrList.SecondryEmail;
                        contactModel.WorkZip = adrList.PostalCode;


                    }
                    if (item.ContactSequance == 3)
                    {
                        adrList = dbobj.AddressLists.Where(x => x.AddressId == item.AddressId).SingleOrDefault();
                        contactModel.OtherAddressTypeId = adrList.AddressType;
                        contactModel.OtherAddressLine1 = adrList.ApartmentType;
                        contactModel.OtherAddressLine2 = adrList.StreetName;
                        contactModel.OtherCity = adrList.City;
                        contactModel.OtherState = Convert.ToInt32(adrList.StateProvince);
                        contactModel.OtherCountry = Convert.ToInt32(adrList.CountryId);
                        contactModel.OtherCounty = adrList.County;
                        contactModel.OtherHomePhone = adrList.Phone;
                        contactModel.OtherMobilePhone = adrList.Mobile;
                        contactModel.OtherWorkPhone = adrList.OtherPhone;
                        contactModel.OtherExtension = adrList.Extension;
                        contactModel.OtherExtension3 = adrList.Extension2;
                        contactModel.OtherFax = adrList.Fax;
                        contactModel.OtherHomeEmail = adrList.PrimaryEmail;
                        contactModel.OtherWorkEmail = adrList.SecondryEmail;
                        contactModel.OtherZip = adrList.PostalCode;
                        contactModel.HomeStreet = adrList.StreetName;
                        contactModel.HomeNumber = adrList.ApartmentNumber;

                    }
                }

            }
            return contactModel;
        }


        /// <summary>
        /// Function To Delete Contact Data.
        /// </summary>
        /// <param name="ClientId"></param>
        /// <param name="itemId"></param>

        public void deleteContact(int ClientId, int itemId)
        {
            MetaData = new GlobalData();
            dbobj = new MelmarkDBEntities();
            ContactModel contactModel = new ContactModel();
            ContactPersonal contactPersonal = new ContactPersonal();
            contactPersonal = dbobj.ContactPersonals.Where(objContactPersonal => objContactPersonal.StudentPersonalId == ClientId &&
                objContactPersonal.ContactPersonalId == itemId).SingleOrDefault();
            contactPersonal.Status = MetaData._StatusFalse;
            dbobj.SaveChanges();

            var ParentData = dbobj.Parents.Where(x => x.ContactPersonalId == itemId).ToList();
            if (ParentData.Count > 0)
            {
                dbobj.Parents.Remove(ParentData[0]);
                dbobj.SaveChanges();
            }

        }


        /// <summary>
        /// The below two functions are used to convert date time to sting.
        /// </summary>
        /// <param name="nullable"></param>
        /// <returns></returns>

        private string ConvertDate(DateTime? nullable)
        {
            string result = "";
            DateTime temp;
            try
            {
                temp = (DateTime)nullable;
                result = temp.ToString("MM/dd/yyyy").Replace('-', '/');
            }
            catch
            {
                result = null;
            }

            return result;
        }

        public string ConvertDate(DateTime dateString)
        {
            string result = "";
            DateTime temp = (DateTime)dateString;
            result = temp.ToString("MM/dd/yyyy").Replace('-', '/');
            return result;
        }



        /// <summary>
        /// Function to Bind Student Image to the right side Panel.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>

        public ImageModel bindImage(int clientId)
        {

            ImageModel imgmodel = new ImageModel();
            dbobj = new MelmarkDBEntities();
            StudentPersonal student = new StudentPersonal();


            string dirpath = System.Web.Configuration.WebConfigurationManager.AppSettings["ImagessLocation"].ToString();
            try
            {
                student = dbobj.StudentPersonals.Where(x => x.StudentPersonalId == clientId).SingleOrDefault();
                if (student != null)
                {
                    imgmodel.FirstName = student.FirstName;
                    imgmodel.LastName = student.LastName;

                    if (student.ImageUrl == null)
                    {

                    }
                    imgmodel.ImageUrl = student.ImageUrl;
                    //imgmodel.ImageUrl = "../../../"+ imgmodel.ImageUrl.Replace('\\', '/');
                    imgmodel.StudentId = student.LocalId;
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }

            return imgmodel;
        }

        /// <summary>
        /// Funtion To load Saved Events Details For Edit / View
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        /// 
        public AddEventModel bindEvents(int itemId)
        {
            sess = (clsSession)HttpContext.Current.Session["UserSession"];
            dbobj = new MelmarkDBEntities();
            AddEventModel returnModel = new AddEventModel();
            DateTime now = DateTime.Now;
            Event events = new Event();
            if (itemId > 0)
            {
                try
                {
                    events = dbobj.Events.Where(objEvents => objEvents.StudentPersonalId == sess.ReferralId && objEvents.EventId == itemId).SingleOrDefault();
                    returnModel.Id = events.EventId;
                    returnModel.EventName = events.EventsName;
                    if (events.ExpiredOn <= now)
                    {
                        returnModel.EventStatus = 179;
                    }
                    else
                    {
                        returnModel.EventStatus = events.EventStatus;
                    }

                    returnModel.EventTypes =  events.EventType.ToString();
                    returnModel.ExpiredOnDate = ConvertDate(events.ExpiredOn);
                    returnModel.EventDate = ConvertDate(events.EventDate);
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                }
            }
            return returnModel;
        }


        /// <summary>
        /// Function to Save / Update Events Data
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        public string SaveEventData(AddEventModel model)
        {
            sess = (clsSession)HttpContext.Current.Session["UserSession"];
            dbobj = new MelmarkDBEntities();
            int ClientID = sess.ReferralId, SchoolId = sess.SchoolId;
            Event events = new Event();
            if (model.Id > 0)
            {
                try
                {
                    events = dbobj.Events.Where(objEvents => objEvents.EventId == model.Id && objEvents.StudentPersonalId == ClientID).SingleOrDefault();
                    events.EventsName = model.EventName;
                    events.EventType = Convert.ToInt32(model.EventTypes);
                    events.EventStatus = model.EventStatus;
                    events.Status = 1;
                    events.EventDate = DateTime.Now;
                    events.ExpiredOn = DateTime.ParseExact(model.ExpiredOnDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    events.ModifiedBy = 1;
                    events.ModifiedOn = DateTime.Now;
                    dbobj.SaveChanges();
                    return "Sucess";
                }
                catch
                {
                    return "Failed";
                }
            }
            else
            {
                if (ClientID == 0)
                {
                    return "No Client Selected";
                }
                else
                {
                    try
                    {
                        events.SchoolId = SchoolId;
                        events.EventsName = model.EventName;
                        events.EventType = Convert.ToInt32(model.EventTypes);
                        events.EventStatus = model.EventStatus;
                        events.Status = 1;
                        events.StudentPersonalId = ClientID;
                        events.EventDate = DateTime.Now;
                        events.ExpiredOn = DateTime.ParseExact(model.ExpiredOnDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        events.CreatedBy = 1;
                        events.CreatedOn = DateTime.Now;
                        dbobj.Events.Add(events);
                        dbobj.SaveChanges();
                        return "Sucess";
                    }
                    catch
                    {
                        return "Failed";
                    }
                }
            }

        }



        /// <summary>
        /// Function to Save / Update Documents Data
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        public string SaveForms(AddDocumentModel model, HttpPostedFileBase profilePicture)
        {
            sess = (clsSession)HttpContext.Current.Session["UserSession"];
            //dbobj = new BiWeeklyRCPNewEntities();
            //string dirpath = (AppDomain.CurrentDomain.BaseDirectory + "Forms/".ToString()).Replace('\\', '/');
            //dirpath = System.Web.Configuration.WebConfigurationManager.AppSettings["filesLocation"].ToString();
            // string realPath =  Request.Url.GetLeftPart(UriPartial.Authority) + Request.ApplicationPath;
            int ClientID = sess.ReferralId, SchoolId = sess.SchoolId;
            //Document Documents = new Document();

            if (ClientID == 0)
            {
                return "No Client Selected";
            }
            else
            {
                try
                {
                    if (model.DocumentName != "" && model.DocumentName != null)
                    {
                        string contentType = "";
                        int rtrnval = -1;
                        if (profilePicture.ContentType != null) contentType = profilePicture.ContentType;



                        byte[] bytes = null;
                        using (Stream fs = profilePicture.InputStream)
                        {
                            using (BinaryReader br = new BinaryReader(fs))
                            {
                                bytes = br.ReadBytes((Int32)fs.Length);
                            }
                        }


                        dbobj = new MelmarkDBEntities();
                        LookUp doctype = new LookUp();
                        doctype = dbobj.LookUps.Where(objlk => objlk.LookupId == model.DocumentType).SingleOrDefault();

                        Document tblDoc = new Document();
                        binaryFile binfile = new binaryFile();
                        StdtIEP std = new StdtIEP();

                        if (doctype.LookupName != "Other")
                        {
                            tblDoc.Other = doctype.LookupName;
                            tblDoc.DocumentName = model.DocumentName;
                            tblDoc.DocumentType = model.DocumentType;
                            tblDoc.SchoolId = SchoolId;
                            tblDoc.StudentPersonalId = sess.ReferralId;
                            tblDoc.Status = true;
                            tblDoc.UserType = "Staff";
                            tblDoc.CreatedBy = sess.LoginId;
                            tblDoc.CreatedOn = System.DateTime.Now;
                            dbobj.Documents.Add(tblDoc);
                            dbobj.SaveChanges();
                            rtrnval = tblDoc.DocumentId;




                            binfile.SchoolId = SchoolId;
                            binfile.StudentId = ClientID;
                            binfile.DocumentName = model.DocumentName;
                            binfile.DocId = rtrnval;
                            binfile.ContentType = contentType;
                            binfile.Data = bytes;
                            binfile.CreatedBy = sess.LoginId;
                            binfile.CreatedOn = DateTime.Now;
                            binfile.type = "Client";
                            binfile.Varified = true;
                            binfile.ModuleName = doctype.LookupName;
                            dbobj.binaryFiles.Add(binfile);
                            dbobj.SaveChanges();
                            rtrnval = binfile.BinaryId;
                        }
                        else
                        {

                            tblDoc.Other = model.Other;
                            tblDoc.DocumentName = model.DocumentName;
                            tblDoc.DocumentType = model.DocumentType;
                            tblDoc.SchoolId = SchoolId;
                            tblDoc.StudentPersonalId = sess.ReferralId;
                            tblDoc.Status = true;
                            tblDoc.UserType = "Staff";
                            tblDoc.CreatedBy = sess.LoginId;
                            tblDoc.CreatedOn = System.DateTime.Now;
                            dbobj.Documents.Add(tblDoc);
                            dbobj.SaveChanges();
                            rtrnval = tblDoc.DocumentId;


                            //binaryFile binfile = new binaryFile();

                            binfile.SchoolId = SchoolId;
                            binfile.StudentId = ClientID;
                            binfile.DocumentName = model.DocumentName;
                            binfile.DocId = rtrnval;
                            binfile.ContentType = contentType;
                            binfile.Data = bytes;
                            binfile.CreatedBy = sess.LoginId;
                            binfile.CreatedOn = DateTime.Now;
                            binfile.type = "Client";
                            binfile.Varified = true;
                            binfile.ModuleName = doctype.LookupName;
                            dbobj.binaryFiles.Add(binfile);
                            dbobj.SaveChanges();
                            rtrnval = binfile.BinaryId;

                        }



                        //Documents.SchoolId = SchoolId;
                        //Documents.DocumentName = model.DocumentName;
                        //Documents.DocumentType = model.DocumentType;
                        //Documents.Status = true;
                        //Documents.StudentPersonalId = ClientID;
                        //Documents.UserType = "Melmark User";
                        //Documents.CreatedBy = sess.LoginId;
                        //if (profilePicture != null)
                        //{
                        //    Documents.DocumentPath = profilePicture.FileName;
                        //}
                        //Documents.CreatedOn = DateTime.Now;
                        //dbobj.Documents.Add(Documents);
                        //dbobj.SaveChanges();
                        //if (profilePicture != null)
                        //{
                        //    profilePicture.SaveAs(dirpath + Documents.DocumentId + "-" + profilePicture.FileName);
                        //}
                    }
                    return "Success";
                }
                catch (Exception EX)
                {
                    throw EX;
                    //return "Failed";
                }
            }
        }




        /// <summary>
        /// Function To delete Document.
        /// </summary>
        /// <param name="ClientId"></param>
        /// <param name="itemId"></param>

        public void deleteDocument(int ClientId, int itemId)
        {
            dbobj = new MelmarkDBEntities();
            binaryFile Documentz = new binaryFile();
            Documentz = dbobj.binaryFiles.Where(objDocuments => objDocuments.StudentId == ClientId && objDocuments.BinaryId == itemId).SingleOrDefault();
            if (Documentz != null)
            { Documentz.Varified = false; }

            dbobj.SaveChanges();
        }


        //public FileContentResult ViewDocument(int documentId)
        //{
        //dbobj = new BiWeeklyRCPNewEntities();
        //binaryFile ObjDoc = new binaryFile();
        //////string dirpath = (AppDomain.CurrentDomain.BaseDirectory + "Forms".ToString()).Replace('\\', '/');
        //string dirpath = System.Web.Configuration.WebConfigurationManager.AppSettings["filesLocation"].ToString();
        //sess = (clsSession)HttpContext.Current.Session["UserSession"];
        ////var attachmenttable = dbobj.Appattachments.Single(x => x.Id == id && x.RefObjectid == userSessionObj.id);
        ////var userdoc = dbobj.AppAttachedFiles.Single(x => x.AttachmentId == attachmenttable.Id);
        //ObjDoc = dbobj.binaryFiles.Where(objDocument => objDocument.BinaryId == documentId && objDocument.StudentId == sess.ReferralId).SingleOrDefault();

        //if (ObjDoc != null)
        //{
        //    var documentPath = dirpath + "\\" + ObjDoc.BinaryId + "-" + ObjDoc.DocumentPath;
        //    return documentPath.Replace('\\', '/');

        //}
        //return "Failed";


        //}



        /// <summary>
        /// Function To delete Events Data.
        /// </summary>
        /// <param name="StudentId"></param>
        /// <param name="itemId"></param>

        public void deleteEvents(int ClientId, int itemId)
        {
            dbobj = new MelmarkDBEntities();
            Event events = new Event();
            events = dbobj.Events.Where(objEvents => objEvents.StudentPersonalId == ClientId && objEvents.EventId == itemId).SingleOrDefault();
            events.Status = 0;
            dbobj.SaveChanges();
        }



        /// <summary>
        /// Function to Save / Update Contact Detais. Model Is Contact
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        public string SaveContactData(ContactModel model)
        {
            string Result = "";
            dbobj = new MelmarkDBEntities();
            ContactPersonal stdtContactPersonal = new ContactPersonal();
            AddressList stdtContact = new AddressList();
            StudentAddresRel adrRel = new StudentAddresRel();
            StudentContactRelationship contactRelation = new StudentContactRelationship();
            // HttpContext.Current.Session["val"] = 1;
            sess = (clsSession)HttpContext.Current.Session["UserSession"];
            StudentContactRelationship stdtContactRel = new StudentContactRelationship();
            Parent parent = new Parent();
            StudentParentRel studentParentRel = new StudentParentRel();
            MetaData = new GlobalData();
            var RelationData = dbobj.LookUps.Where(x => x.LookupType == "Relationship" && x.LookupName == "Parent").SingleOrDefault();
            int ClientID = sess.ReferralId;
            if (ClientID == 0)
            {
                Result = "No Client Selected";
            }
            else
            {
                try
                {
                    using (TransactionScope trans = new TransactionScope(TransactionScopeOption.Required))
                    {
                        if (model.Id > 0)
                        {
                            stdtContactPersonal = dbobj.ContactPersonals.Where(objContactPersonal => objContactPersonal.StudentPersonalId == ClientID &&
                                objContactPersonal.ContactPersonalId == model.Id).SingleOrDefault();
                            if (stdtContactPersonal != null)
                            {
                                //stdtContactPersonal.StudentPersonalId = ClientID;
                                stdtContactPersonal.Prefix = model.FirstNamePrefix;
                                stdtContactPersonal.FirstName = model.FirstName;
                                stdtContactPersonal.LastName = model.LastName;
                                stdtContactPersonal.Suffix = model.LastNameSuffix;
                                stdtContactPersonal.MiddleName = model.MiddleName;
                                stdtContactPersonal.Spouse = model.Spouse;
                                stdtContactPersonal.PrimaryLanguage = model.PrimaryLanguage;
                                stdtContactPersonal.ModifiedBy = sess.LoginId;
                                stdtContactPersonal.ModifiedOn = System.DateTime.Now;
                                // dbobj.ContactPersonals.Add(stdtContactPersonal);
                                dbobj.SaveChanges();
                                //if (stdtContactPersonal.ContactFlag == "Referral")
                                //{
                                //    
                                //}
                                //else
                                //{
                                    contactRelation = dbobj.StudentContactRelationships.Where(objStudentContactRel => objStudentContactRel.ContactPersonalId == model.Id).SingleOrDefault();
                                    contactRelation.RelationshipId = Convert.ToInt32(model.Relation);
                                    contactRelation.ModifiedBy = sess.LoginId;
                                    contactRelation.ModifiedOn = System.DateTime.Now;
                                    dbobj.SaveChanges();
                                //}

                                var addresses = dbobj.StudentAddresRels.Where(x => x.ContactPersonalId == model.Id && x.ContactSequence == 1).ToList().OrderBy(x => x.ContactSequence).SingleOrDefault();

                                if (addresses != null)
                                {
                                    stdtContact = dbobj.AddressLists.Where(objAddressList => objAddressList.AddressId == addresses.AddressId).SingleOrDefault();
                                    stdtContact.AddressType = model.HomeAddressTypeId;
                                    stdtContact.ApartmentType = model.HomeAddressLine1;
                                    stdtContact.StreetName = model.HomeAddressLine2;
                                    stdtContact.AddressLine3 = model.HomeAddressLine3;
                                    stdtContact.City = model.HomeCity;
                                    stdtContact.StateProvince = model.HomeState;
                                    stdtContact.CountryId = model.HomeCountry;
                                    stdtContact.County = model.HomeCounty;
                                    stdtContact.Phone = model.HomePhone;
                                    stdtContact.Mobile = model.HomeMobilePhone;
                                    stdtContact.OtherPhone = model.HomeWorkPhone;
                                    stdtContact.Extension = model.HomeExtension;
                                    stdtContact.Fax = model.HomeFax;
                                    stdtContact.PrimaryEmail = model.HomeEmail;
                                    stdtContact.SecondryEmail = model.HomeWorkEmail;
                                    stdtContact.PostalCode = model.HomeZip;
                                    stdtContact.ModifiedBy = sess.LoginId;
                                    stdtContact.ModifiedOn = System.DateTime.Now;
                                    dbobj.SaveChanges();


                                    adrRel = dbobj.StudentAddresRels.Where(objRel => objRel.AddressId == stdtContact.AddressId).SingleOrDefault();
                                    if (adrRel != null)
                                    {
                                        adrRel.AddressId = stdtContact.AddressId;
                                        adrRel.StudentPersonalId = ClientID;
                                        adrRel.ContactPersonalId = stdtContactPersonal.ContactPersonalId;
                                        adrRel.ContactSequence = 1;
                                        adrRel.CreatedBy = sess.LoginId;
                                        adrRel.CreatedOn = DateTime.Now;
                                        dbobj.SaveChanges();
                                    }
                                    else
                                    {
                                        adrRel.AddressId = stdtContact.AddressId;
                                        adrRel.StudentPersonalId = ClientID;
                                        adrRel.ContactPersonalId = stdtContactPersonal.ContactPersonalId;
                                        adrRel.ContactSequence = 1;
                                        adrRel.CreatedBy = sess.LoginId;
                                        adrRel.CreatedOn = DateTime.Now;
                                        dbobj.StudentAddresRels.Add(adrRel);
                                        dbobj.SaveChanges();

                                    }
                                }
                                else
                                {

                                    stdtContact.AddressType = model.HomeAddressTypeId;
                                    stdtContact.ApartmentType = model.HomeAddressLine1;
                                    stdtContact.StreetName = model.HomeAddressLine2;
                                    stdtContact.AddressLine3 = model.HomeAddressLine3;
                                    stdtContact.City = model.HomeCity;
                                    stdtContact.StateProvince = model.HomeState;
                                    stdtContact.CountryId = model.HomeCountry;
                                    stdtContact.County = model.HomeCounty;
                                    stdtContact.Phone = model.HomePhone;
                                    stdtContact.Mobile = model.HomeMobilePhone;
                                    stdtContact.OtherPhone = model.HomeWorkPhone;
                                    stdtContact.Extension = model.HomeExtension;
                                    stdtContact.Fax = model.HomeFax;
                                    stdtContact.PrimaryEmail = model.HomeEmail;
                                    stdtContact.SecondryEmail = model.HomeWorkEmail;
                                    stdtContact.PostalCode = model.HomeZip;
                                    stdtContact.ModifiedBy = sess.LoginId;
                                    stdtContact.ModifiedOn = System.DateTime.Now;
                                    dbobj.AddressLists.Add(stdtContact);
                                    dbobj.SaveChanges();


                                    adrRel = dbobj.StudentAddresRels.Where(objRel => objRel.AddressId == stdtContact.AddressId).SingleOrDefault();
                                    if (adrRel != null)
                                    {
                                        adrRel.AddressId = stdtContact.AddressId;
                                        adrRel.StudentPersonalId = ClientID;
                                        adrRel.ContactPersonalId = stdtContactPersonal.ContactPersonalId;
                                        adrRel.ContactSequence = 1;
                                        adrRel.CreatedBy = sess.LoginId;
                                        adrRel.CreatedOn = DateTime.Now;
                                        dbobj.SaveChanges();
                                    }
                                    else
                                    {
                                        adrRel.AddressId = stdtContact.AddressId;
                                        adrRel.StudentPersonalId = ClientID;
                                        adrRel.ContactPersonalId = stdtContactPersonal.ContactPersonalId;
                                        adrRel.ContactSequence = 1;
                                        adrRel.CreatedBy = sess.LoginId;
                                        adrRel.CreatedOn = DateTime.Now;
                                        dbobj.StudentAddresRels.Add(adrRel);
                                        dbobj.SaveChanges();

                                    }
                                }

                                addresses = dbobj.StudentAddresRels.Where(x => x.ContactPersonalId == model.Id && x.ContactSequence == 2).ToList().OrderBy(x => x.ContactSequence).SingleOrDefault();

                                if (addresses != null)
                                {
                                    stdtContact = dbobj.AddressLists.Where(objAddressList => objAddressList.AddressId == addresses.AddressId).SingleOrDefault();
                                    stdtContact.AddressType = model.WorkAddressTypeId;
                                    stdtContact.ApartmentType = model.WorkAddressLine1;
                                    stdtContact.StreetName = model.WorkAddressLine2;
                                    stdtContact.AddressLine3 = model.WorkAddressLine3;
                                    // stdtContact.StreetName = model.HomeStreet;
                                    stdtContact.ApartmentNumber = model.HomeNumber;
                                    stdtContact.City = model.WorkCity;
                                    stdtContact.StateProvince = model.WorkState;
                                    stdtContact.CountryId = model.WorkCountry;
                                    stdtContact.County = model.WorkCounty;
                                    stdtContact.Phone = model.WorkHomePhone;
                                    stdtContact.Mobile = model.WorkMobilePhone;
                                    stdtContact.OtherPhone = model.WorkPhone;
                                    stdtContact.Extension = model.WorkExtension;
                                    stdtContact.Extension2 = model.OtherExtension2;
                                    stdtContact.Fax = model.WorkFax;
                                    stdtContact.PrimaryEmail = model.WorkHomeEmail;
                                    stdtContact.SecondryEmail = model.WorkEmail;
                                    stdtContact.PostalCode = model.WorkZip;
                                    stdtContact.ModifiedBy = sess.LoginId;
                                    stdtContact.ModifiedOn = System.DateTime.Now;
                                    dbobj.SaveChanges();


                                    adrRel = dbobj.StudentAddresRels.Where(objRel => objRel.AddressId == stdtContact.AddressId).SingleOrDefault();
                                    if (adrRel != null)
                                    {
                                        adrRel.AddressId = stdtContact.AddressId;
                                        adrRel.StudentPersonalId = ClientID;
                                        adrRel.ContactPersonalId = stdtContactPersonal.ContactPersonalId;
                                        adrRel.ContactSequence = 2;
                                        adrRel.CreatedBy = sess.LoginId;
                                        adrRel.CreatedOn = DateTime.Now;
                                        dbobj.SaveChanges();
                                    }
                                    else
                                    {
                                        adrRel = new StudentAddresRel();
                                        adrRel.AddressId = stdtContact.AddressId;
                                        adrRel.StudentPersonalId = ClientID;
                                        adrRel.ContactPersonalId = stdtContactPersonal.ContactPersonalId;
                                        adrRel.ContactSequence = 2;
                                        adrRel.CreatedBy = sess.LoginId;
                                        adrRel.CreatedOn = DateTime.Now;
                                        dbobj.StudentAddresRels.Add(adrRel);
                                        dbobj.SaveChanges();

                                    }
                                }
                                else
                                {
                                    stdtContact.AddressType = model.WorkAddressTypeId;
                                    stdtContact.ApartmentType = model.WorkAddressLine1;
                                    stdtContact.StreetName = model.WorkAddressLine2;
                                    stdtContact.AddressLine3 = model.WorkAddressLine3;
                                    // stdtContact.StreetName = model.HomeStreet;
                                    stdtContact.ApartmentNumber = model.HomeNumber;
                                    stdtContact.City = model.WorkCity;
                                    stdtContact.StateProvince = model.WorkState;
                                    stdtContact.CountryId = model.WorkCountry;
                                    stdtContact.County = model.WorkCounty;
                                    stdtContact.Phone = model.WorkHomePhone;
                                    stdtContact.Mobile = model.WorkMobilePhone;
                                    stdtContact.OtherPhone = model.WorkPhone;
                                    stdtContact.Extension = model.WorkExtension;
                                    stdtContact.Extension2 = model.OtherExtension2;
                                    stdtContact.Fax = model.WorkFax;
                                    stdtContact.PrimaryEmail = model.WorkHomeEmail;
                                    stdtContact.SecondryEmail = model.WorkEmail;
                                    stdtContact.PostalCode = model.WorkZip;
                                    stdtContact.ModifiedBy = sess.LoginId;
                                    stdtContact.ModifiedOn = System.DateTime.Now;
                                    dbobj.AddressLists.Add(stdtContact);
                                    dbobj.SaveChanges();


                                    adrRel = dbobj.StudentAddresRels.Where(objRel => objRel.AddressId == stdtContact.AddressId).SingleOrDefault();
                                    if (adrRel != null)
                                    {
                                        adrRel.AddressId = stdtContact.AddressId;
                                        adrRel.StudentPersonalId = ClientID;
                                        adrRel.ContactPersonalId = stdtContactPersonal.ContactPersonalId;
                                        adrRel.ContactSequence = 2;
                                        adrRel.CreatedBy = sess.LoginId;
                                        adrRel.CreatedOn = DateTime.Now;
                                        dbobj.SaveChanges();
                                    }
                                    else
                                    {
                                        adrRel = new StudentAddresRel();
                                        adrRel.AddressId = stdtContact.AddressId;
                                        adrRel.StudentPersonalId = ClientID;
                                        adrRel.ContactPersonalId = stdtContactPersonal.ContactPersonalId;
                                        adrRel.ContactSequence = 2;
                                        adrRel.CreatedBy = sess.LoginId;
                                        adrRel.CreatedOn = DateTime.Now;
                                        dbobj.StudentAddresRels.Add(adrRel);
                                        dbobj.SaveChanges();

                                    }
                                }
                                addresses = dbobj.StudentAddresRels.Where(x => x.ContactPersonalId == model.Id && x.ContactSequence == 3).ToList().OrderBy(x => x.ContactSequence).SingleOrDefault();

                                if (addresses != null)
                                {
                                    stdtContact = dbobj.AddressLists.Where(objAddressList => objAddressList.AddressId == addresses.AddressId).SingleOrDefault();
                                    stdtContact.AddressType = model.OtherAddressTypeId;
                                    stdtContact.ApartmentType = model.OtherAddressLine1;
                                    stdtContact.StreetName = model.OtherAddressLine2;
                                    stdtContact.AddressLine3 = model.OtherAddressLine3;
                                    //stdtContact.StreetName = model.HomeStreet;
                                    stdtContact.ApartmentNumber = model.HomeNumber;
                                    stdtContact.City = model.OtherCity;
                                    stdtContact.StateProvince = model.OtherState;
                                    stdtContact.CountryId = model.OtherCountry;
                                    stdtContact.County = model.OtherCounty;
                                    stdtContact.Phone = model.OtherHomePhone;
                                    stdtContact.Mobile = model.OtherMobilePhone;
                                    stdtContact.OtherPhone = model.OtherWorkPhone;
                                    stdtContact.Extension = model.OtherExtension;
                                    stdtContact.Extension2 = model.OtherExtension3;
                                    stdtContact.Fax = model.OtherFax;
                                    stdtContact.PrimaryEmail = model.OtherHomeEmail;
                                    stdtContact.SecondryEmail = model.OtherWorkEmail;
                                    stdtContact.PostalCode = model.OtherZip;
                                    stdtContact.ModifiedBy = sess.LoginId;
                                    stdtContact.ModifiedOn = System.DateTime.Now;
                                    dbobj.SaveChanges();


                                    adrRel = dbobj.StudentAddresRels.Where(objRel => objRel.AddressId == stdtContact.AddressId).SingleOrDefault();
                                    if (adrRel != null)
                                    {
                                        adrRel.AddressId = stdtContact.AddressId;
                                        adrRel.StudentPersonalId = ClientID;
                                        adrRel.ContactPersonalId = stdtContactPersonal.ContactPersonalId;
                                        adrRel.ContactSequence = 3;
                                        adrRel.CreatedBy = sess.LoginId;
                                        adrRel.CreatedOn = DateTime.Now;
                                        dbobj.SaveChanges();
                                    }
                                    else
                                    {
                                        adrRel = new StudentAddresRel();
                                        adrRel.AddressId = stdtContact.AddressId;
                                        adrRel.StudentPersonalId = ClientID;
                                        adrRel.ContactPersonalId = stdtContactPersonal.ContactPersonalId;
                                        adrRel.ContactSequence = 3;
                                        adrRel.CreatedBy = sess.LoginId;
                                        adrRel.CreatedOn = DateTime.Now;
                                        dbobj.StudentAddresRels.Add(adrRel);
                                        dbobj.SaveChanges();

                                    }
                                }
                                else
                                {
                                    stdtContact.AddressType = model.OtherAddressTypeId;
                                    stdtContact.ApartmentType = model.OtherAddressLine1;
                                    stdtContact.StreetName = model.OtherAddressLine2;
                                    stdtContact.AddressLine3 = model.OtherAddressLine3;
                                    //stdtContact.StreetName = model.HomeStreet;
                                    stdtContact.ApartmentNumber = model.HomeNumber;
                                    stdtContact.City = model.OtherCity;
                                    stdtContact.StateProvince = model.OtherState;
                                    stdtContact.CountryId = model.OtherCountry;
                                    stdtContact.County = model.OtherCounty;
                                    stdtContact.Phone = model.OtherHomePhone;
                                    stdtContact.Mobile = model.OtherMobilePhone;
                                    stdtContact.OtherPhone = model.OtherWorkPhone;
                                    stdtContact.Extension = model.OtherExtension;
                                    stdtContact.Extension2 = model.OtherExtension3;
                                    stdtContact.Fax = model.OtherFax;
                                    stdtContact.PrimaryEmail = model.OtherHomeEmail;
                                    stdtContact.SecondryEmail = model.OtherWorkEmail;
                                    stdtContact.PostalCode = model.OtherZip;
                                    stdtContact.ModifiedBy = sess.LoginId;
                                    stdtContact.ModifiedOn = System.DateTime.Now;
                                    dbobj.AddressLists.Add(stdtContact);
                                    dbobj.SaveChanges();


                                    adrRel = dbobj.StudentAddresRels.Where(objRel => objRel.AddressId == stdtContact.AddressId).SingleOrDefault();
                                    if (adrRel != null)
                                    {
                                        adrRel.AddressId = stdtContact.AddressId;
                                        adrRel.StudentPersonalId = ClientID;
                                        adrRel.ContactPersonalId = stdtContactPersonal.ContactPersonalId;
                                        adrRel.ContactSequence = 3;
                                        adrRel.CreatedBy = sess.LoginId;
                                        adrRel.CreatedOn = DateTime.Now;
                                        dbobj.SaveChanges();
                                    }
                                    else
                                    {
                                        adrRel = new StudentAddresRel();
                                        adrRel.AddressId = stdtContact.AddressId;
                                        adrRel.StudentPersonalId = ClientID;
                                        adrRel.ContactPersonalId = stdtContactPersonal.ContactPersonalId;
                                        adrRel.ContactSequence = 3;
                                        adrRel.CreatedBy = sess.LoginId;
                                        adrRel.CreatedOn = DateTime.Now;
                                        dbobj.StudentAddresRels.Add(adrRel);
                                        dbobj.SaveChanges();

                                    }
                                }

                                if (Convert.ToInt32(model.Relation) == RelationData.LookupId)
                                {
                                    //var ParentStudentRelData = dbobj.StudentParentRels.Where(x => x.StudentPersonalId == sess.ReferralId).ToList();
                                    //if (ParentStudentRelData.Count > 0)
                                    //{
                                    var ParentData = dbobj.Parents.Where(x => x.ContactPersonalId == stdtContactPersonal.ContactPersonalId).ToList();
                                    if (ParentData.Count > 0)
                                    {
                                        ParentData[0].Fname = model.FirstName;
                                        ParentData[0].Lname = model.LastName;
                                        //ParentData[0].Username = model.UserID;
                                        //ParentData[0].Password = model.LastName;
                                        ParentData[0].ContactPersonalId = stdtContactPersonal.ContactPersonalId;
                                        ParentData[0].ModifiedBy = sess.LoginId;
                                        ParentData[0].ModifiedOn = System.DateTime.Now;
                                        dbobj.SaveChanges();
                                    }
                                    //}
                                    else
                                    {
                                        parent.Fname = model.FirstName;
                                        parent.Lname = model.LastName;
                                        parent.Username = model.UserID;
                                        parent.Password = model.LastName;
                                        parent.ContactPersonalId = stdtContactPersonal.ContactPersonalId;
                                        parent.CreatedBy = sess.LoginId;
                                        parent.CreatedOn = System.DateTime.Now;
                                        dbobj.Parents.Add(parent);
                                        dbobj.SaveChanges();
                                        int parentid = parent.ParentID;

                                        studentParentRel.StudentPersonalId = sess.ReferralId;
                                        studentParentRel.ParentID = parentid;
                                        studentParentRel.CreatedBy = sess.LoginId;
                                        studentParentRel.CreatedOn = System.DateTime.Now;
                                        dbobj.StudentParentRels.Add(studentParentRel);
                                        dbobj.SaveChanges();
                                    }
                                }
                                else
                                {
                                    var ParentData = dbobj.Parents.Where(x => x.ContactPersonalId == stdtContactPersonal.ContactPersonalId).ToList();
                                    if (ParentData.Count > 0)
                                    {
                                        dbobj.Parents.Remove(ParentData[0]);
                                        dbobj.SaveChanges();
                                    }
                                }
                            }
                        }
                        else
                        {
                            stdtContactPersonal = new ContactPersonal();
                            stdtContactPersonal.StudentPersonalId = ClientID;
                            stdtContactPersonal.Prefix = model.FirstNamePrefix;
                            stdtContactPersonal.FirstName = model.FirstName;
                            stdtContactPersonal.LastName = model.LastName;
                            stdtContactPersonal.Suffix = model.LastNameSuffix;
                            stdtContactPersonal.MiddleName = model.MiddleName;
                            stdtContactPersonal.Spouse = model.Spouse;
                            stdtContactPersonal.ContactFlag = "Client";
                            stdtContactPersonal.PrimaryLanguage = model.PrimaryLanguage;
                            stdtContactPersonal.Status = MetaData._StatusTrue;
                            stdtContactPersonal.CreatedBy = sess.LoginId;
                            stdtContactPersonal.CreatedOn = System.DateTime.Now;
                            dbobj.ContactPersonals.Add(stdtContactPersonal);
                            dbobj.SaveChanges();

                            stdtContact.AddressType = model.HomeAddressTypeId;
                            stdtContact.ApartmentType = model.HomeAddressLine1;
                            stdtContact.StreetName = model.HomeAddressLine2;
                            stdtContact.AddressLine3 = model.HomeAddressLine3;
                            //  stdtContact.StreetName = model.HomeStreet;
                            stdtContact.ApartmentNumber = model.HomeNumber;
                            stdtContact.City = model.HomeCity;
                            stdtContact.StateProvince = model.HomeState;
                            stdtContact.CountryId = model.HomeCountry;
                            stdtContact.County = model.HomeCounty;
                            stdtContact.Phone = model.HomePhone;
                            stdtContact.Mobile = model.HomeMobilePhone;
                            stdtContact.OtherPhone = model.HomeWorkPhone;
                            stdtContact.OtherPhone = model.HomeWorkPhone;
                            stdtContact.Extension = model.HomeExtension;
                            stdtContact.Fax = model.HomeFax;
                            stdtContact.PrimaryEmail = model.HomeEmail;
                            stdtContact.SecondryEmail = model.HomeWorkEmail;
                            stdtContact.PostalCode = model.HomeZip;
                            stdtContact.CreatedBy = sess.LoginId;
                            stdtContact.CreatedOn = System.DateTime.Now;
                            dbobj.AddressLists.Add(stdtContact);
                            dbobj.SaveChanges();

                            adrRel.AddressId = stdtContact.AddressId;
                            adrRel.StudentPersonalId = ClientID;
                            adrRel.ContactPersonalId = stdtContactPersonal.ContactPersonalId;
                            adrRel.ContactSequence = 1;
                            adrRel.CreatedBy = sess.LoginId;
                            adrRel.CreatedOn = DateTime.Now;
                            dbobj.StudentAddresRels.Add(adrRel);
                            dbobj.SaveChanges();

                            if (model.WorkAddressTypeId == null)
                            {
                                stdtContact.AddressType = 2;
                            }
                            stdtContact.AddressType = model.WorkAddressTypeId;
                            stdtContact.ApartmentType = model.WorkAddressLine1;
                            stdtContact.StreetName = model.WorkAddressLine2;
                            stdtContact.AddressLine3 = model.WorkAddressLine3;
                            // stdtContact.StreetName = model.HomeStreet;
                            stdtContact.ApartmentNumber = model.HomeNumber;
                            stdtContact.City = model.WorkCity;
                            stdtContact.StateProvince = model.WorkState;
                            stdtContact.CountryId = model.WorkCountry;
                            stdtContact.County = model.WorkCounty;
                            stdtContact.Phone = model.WorkHomePhone;
                            stdtContact.Mobile = model.WorkMobilePhone;
                            stdtContact.OtherPhone = model.WorkPhone;
                            stdtContact.Extension = model.WorkExtension;
                            stdtContact.Extension2 = model.OtherExtension2;
                            stdtContact.Fax = model.WorkFax;
                            stdtContact.PrimaryEmail = model.WorkHomeEmail;
                            stdtContact.SecondryEmail = model.WorkEmail;
                            stdtContact.PostalCode = model.WorkZip;
                            stdtContact.CreatedBy = sess.LoginId;
                            stdtContact.CreatedOn = System.DateTime.Now;
                            dbobj.AddressLists.Add(stdtContact);
                            dbobj.SaveChanges();

                            adrRel.AddressId = stdtContact.AddressId;
                            adrRel.StudentPersonalId = ClientID;
                            adrRel.ContactPersonalId = stdtContactPersonal.ContactPersonalId;
                            adrRel.ContactSequence = 2;
                            adrRel.CreatedBy = sess.LoginId;
                            adrRel.CreatedOn = DateTime.Now;
                            dbobj.StudentAddresRels.Add(adrRel);
                            dbobj.SaveChanges();

                            if (model.OtherAddressTypeId == null)
                            {
                                stdtContact.AddressType = 2;
                            }
                            stdtContact.AddressType = model.OtherAddressTypeId;
                            stdtContact.ApartmentType = model.OtherAddressLine1;
                            stdtContact.StreetName = model.OtherAddressLine2;
                            stdtContact.AddressLine3 = model.OtherAddressLine3;
                            // stdtContact.StreetName = model.HomeStreet;
                            stdtContact.ApartmentNumber = model.HomeNumber;
                            stdtContact.City = model.OtherCity;
                            stdtContact.StateProvince = model.OtherState;
                            stdtContact.CountryId = model.OtherCountry;
                            stdtContact.County = model.OtherCounty;
                            stdtContact.Phone = model.OtherHomePhone;
                            stdtContact.Mobile = model.OtherMobilePhone;
                            stdtContact.OtherPhone = model.OtherWorkPhone;
                            stdtContact.Extension = model.OtherExtension;
                            stdtContact.Extension2 = model.OtherExtension3;
                            stdtContact.Fax = model.OtherFax;
                            stdtContact.PrimaryEmail = model.OtherHomeEmail;
                            stdtContact.PostalCode = model.OtherZip;
                            stdtContact.SecondryEmail = model.OtherWorkEmail;
                            stdtContact.CreatedBy = sess.LoginId;
                            stdtContact.CreatedOn = System.DateTime.Now;
                            dbobj.AddressLists.Add(stdtContact);
                            dbobj.SaveChanges();

                            adrRel.AddressId = stdtContact.AddressId;
                            adrRel.StudentPersonalId = ClientID;
                            adrRel.ContactPersonalId = stdtContactPersonal.ContactPersonalId;
                            adrRel.ContactSequence = 3;
                            adrRel.CreatedBy = sess.LoginId;
                            adrRel.CreatedOn = DateTime.Now;
                            dbobj.StudentAddresRels.Add(adrRel);
                            dbobj.SaveChanges();

                            stdtContactRel.ContactPersonalId = stdtContactPersonal.ContactPersonalId;
                            stdtContactRel.RelationshipId = Convert.ToInt32(model.Relation);
                            stdtContactRel.CreatedBy = sess.LoginId;
                            stdtContactRel.CreatedOn = System.DateTime.Now;
                            dbobj.StudentContactRelationships.Add(stdtContactRel);
                            dbobj.SaveChanges();
                            if (Convert.ToInt32(model.Relation) == RelationData.LookupId)
                            {
                                //var ParentStudentRelData = dbobj.StudentParentRels.Where(x => x.StudentPersonalId == sess.ReferralId).ToList();
                                //if (ParentStudentRelData.Count > 0)
                                //{
                                var ParentData = dbobj.Parents.Where(x => x.ContactPersonalId == stdtContactPersonal.ContactPersonalId).ToList();
                                if (ParentData.Count > 0)
                                {
                                    ParentData[0].Fname = model.FirstName;
                                    ParentData[0].Lname = model.LastName;
                                    //ParentData[0].Username = model.FirstName;
                                    //ParentData[0].Password = model.LastName;
                                    ParentData[0].ContactPersonalId = stdtContactPersonal.ContactPersonalId;
                                    ParentData[0].ModifiedBy = sess.LoginId;
                                    ParentData[0].ModifiedOn = System.DateTime.Now;
                                    dbobj.SaveChanges();
                                }
                                //}
                                else
                                {
                                    parent.Fname = model.FirstName;
                                    parent.Lname = model.LastName;
                                    parent.Username = model.UserID;
                                    parent.Password = model.LastName;
                                    parent.ContactPersonalId = stdtContactPersonal.ContactPersonalId;
                                    parent.CreatedBy = sess.LoginId;
                                    parent.CreatedOn = System.DateTime.Now;
                                    dbobj.Parents.Add(parent);
                                    dbobj.SaveChanges();
                                    int parentid = parent.ParentID;

                                    studentParentRel.StudentPersonalId = sess.ReferralId;
                                    studentParentRel.ParentID = parentid;
                                    studentParentRel.CreatedBy = sess.LoginId;
                                    studentParentRel.CreatedOn = System.DateTime.Now;
                                    dbobj.StudentParentRels.Add(studentParentRel);
                                    dbobj.SaveChanges();
                                }
                            }
                            else
                            {
                                var ParentData = dbobj.Parents.Where(x => x.ContactPersonalId == stdtContactPersonal.ContactPersonalId).ToList();
                                if (ParentData.Count > 0)
                                {
                                    dbobj.Parents.Remove(ParentData[0]);
                                    dbobj.SaveChanges();
                                }
                            }
                        }

                        ////set parent details in parent table
                        //var contactPersonal = dbobj.ContactPersonals.Where(x => x.StudentPersonalId == sess.ReferralId).ToList();
                        //if (contactPersonal.Count > 0)
                        //{
                        //    foreach (var item in contactPersonal)
                        //    {

                        //        var contactRelations = dbobj.StudentContactRelationships.Where(x => x.ContactPersonalId == item.ContactPersonalId && x.RelationshipId == RelationData.LookupId).ToList();
                        //        if (contactRelations.Count > 0)
                        //        {
                        //            foreach (var parentDetails in contactRelations)
                        //            {
                        //                model.FirstName = item.FirstName;
                        //                model.LastName = item.LastName;
                        //                var ParentStudentRelData1 = dbobj.StudentParentRels.Where(x => x.StudentPersonalId == sess.ReferralId).ToList();
                        //                if (ParentStudentRelData1.Count > 0)
                        //                {
                        //                    int parentId = ParentStudentRelData1[0].ParentID;
                        //                    var ParentData = dbobj.Parents.Where(x => x.ParentID == parentId).ToList();
                        //                    if (ParentData.Count > 0)
                        //                    {
                        //                        ParentData[0].Fname = model.FirstName;
                        //                        ParentData[0].Lname = model.LastName;
                        //                        ParentData[0].Username = model.FirstName;
                        //                        ParentData[0].Password = model.LastName;
                        //                        ParentData[0].ModifiedBy = sess.LoginId;
                        //                        ParentData[0].ModifiedOn = System.DateTime.Now;
                        //                        dbobj.SaveChanges();
                        //                    }
                        //                }
                        //                else
                        //                {
                        //                    parent.Fname = model.FirstName;
                        //                    parent.Lname = model.LastName;
                        //                    parent.Username = model.FirstName;
                        //                    parent.Password = model.LastName;
                        //                    parent.CreatedBy = sess.LoginId;
                        //                    parent.CreatedOn = System.DateTime.Now;
                        //                    dbobj.Parents.Add(parent);
                        //                    dbobj.SaveChanges();
                        //                    int parentid = parent.ParentID;

                        //                    studentParentRel.StudentPersonalId = sess.ReferralId;
                        //                    studentParentRel.ParentID = parentid;
                        //                    studentParentRel.CreatedBy = sess.LoginId;
                        //                    studentParentRel.CreatedOn = System.DateTime.Now;
                        //                    dbobj.StudentParentRels.Add(studentParentRel);
                        //                    dbobj.SaveChanges();
                        //                }
                        //            }
                        //        }
                        //    }
                        //}
                        trans.Complete();
                        Result = "Sucess";
                    }
                }
                //catch
                //{
                //    Result = "Failed";
                //}
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                    Result = "Failed";
                }
            }




            return Result;
        }



        /// <summary>
        /// Funtion To load Saved Visitation Details For Edit / View
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        /// 
        public AddVisitaionModel bindVisitation(int itemId)
        {
            sess = (clsSession)HttpContext.Current.Session["UserSession"];
            dbobj = new MelmarkDBEntities();
            AddVisitaionModel returnModel = new AddVisitaionModel();
            Visitation visit = new Visitation();
            DateTime now = DateTime.Now;
            if (itemId > 0)
            {
                try
                {
                    visit = dbobj.Visitations.Where(objvisitation => objvisitation.StudentPersonalId == sess.ReferralId && objvisitation.VisitationId == itemId).SingleOrDefault();
                    returnModel.Id = visit.VisitationId;
                    returnModel.EventName = visit.VisitationName;
                    if (visit.ExpiredOn <= now)
                    {
                        returnModel.EventStatus = 179;
                    }
                    else
                    {
                        returnModel.EventStatus = visit.VisitationStatus;
                    }

                    returnModel.EventType = visit.VisittaionType;
                    returnModel.ExpiredOnDate = ConvertDate(visit.ExpiredOn);
                    returnModel.EventDate = ConvertDate(visit.VisitationDate);
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                }
            }
            return returnModel;
        }

        /// <summary>
        /// Function to Save / Update Visitation Data
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        public string SaveVisitationData(AddVisitaionModel model)
        {
            sess = (clsSession)HttpContext.Current.Session["UserSession"];
            dbobj = new MelmarkDBEntities();
            int ClientID = sess.ReferralId, SchoolId = sess.SchoolId;

            Visitation visit = new Visitation();
            if (model.Id > 0)
            {
                try
                {
                    visit = dbobj.Visitations.Where(objVisitation => objVisitation.VisitationId == model.Id && objVisitation.StudentPersonalId == ClientID).SingleOrDefault();
                    visit.VisitationName = model.EventName;
                    visit.VisittaionType = model.EventType;
                    visit.VisitationStatus = model.EventStatus;
                    visit.Status = 1;
                    visit.VisitationDate = DateTime.Now;
                    visit.ExpiredOn = DateTime.ParseExact(model.ExpiredOnDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    visit.ModifiedBy = 1;
                    visit.ModifiedOn = DateTime.Now;
                    dbobj.SaveChanges();
                    return "Sucess";
                }
                catch
                {
                    return "Failed";
                }
            }
            else
            {
                if (ClientID == 0)
                {
                    return "No Client Selected";
                }
                else
                {
                    try
                    {
                        visit.SchoolId = SchoolId;
                        visit.VisitationName = model.EventName;
                        visit.VisittaionType = model.EventType;
                        visit.VisitationStatus = model.EventStatus;
                        visit.Status = 1;
                        visit.StudentPersonalId = ClientID;
                        visit.VisitationDate = DateTime.Now;
                        visit.ExpiredOn = DateTime.ParseExact(model.ExpiredOnDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        visit.CreatedBy = 1;
                        visit.CreatedOn = DateTime.Now;
                        dbobj.Visitations.Add(visit);
                        dbobj.SaveChanges();
                        return "Sucess";
                    }
                    catch
                    {
                        return "Failed";
                    }
                }
            }

        }


        /// <summary>
        /// Function To delete Visitation Data.
        /// </summary>
        /// <param name="ClientId"></param>
        /// <param name="itemId"></param>

        public void deleteVisitation(int ClientId, int itemId)
        {

            dbobj = new MelmarkDBEntities();
            VisitationModel contactModel = new VisitationModel();
            Visitation visitataion = new Visitation();
            visitataion = dbobj.Visitations.Where(objVisitation => objVisitation.StudentPersonalId == ClientId && objVisitation.VisitationId == itemId).SingleOrDefault();
            visitataion.Status = 0;
            dbobj.SaveChanges();
        }

        /// <summary>
        /// Function To Bind visitation Details.
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>

        public AddPlacementModel bindPlacement(int itemId)
        {
            sess = (clsSession)HttpContext.Current.Session["UserSession"];
            dbobj = new MelmarkDBEntities();
            AddPlacementModel returnModel = new AddPlacementModel();
            Placement placement = new Placement();
            if (itemId > 0)
            {
                try
                {
                    placement = dbobj.Placements.Where(objPlacement => objPlacement.StudentPersonalId == sess.ReferralId && objPlacement.PlacementId == itemId).SingleOrDefault();
                    returnModel.Id = placement.PlacementId;
                    returnModel.PlacementType = placement.PlacementType;
                    returnModel.BehaviorAnalyst = placement.BehaviorAnalyst;
                    returnModel.PrimaryNurse = placement.PrimaryNurse;
                    returnModel.Department = placement.Department;
                    returnModel.UnitClerk = placement.UnitClerk;
                    returnModel.StartDate = ConvertDate(placement.StartDate);
                    returnModel.EndDateDate = ConvertDate(placement.EndDate);
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                }
            }
            return returnModel;
        }


        /// <summary>
        /// Function to Save / Update Placement Data
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        public string SavePlacementData(AddPlacementModel model)
        {
            sess = (clsSession)HttpContext.Current.Session["UserSession"];
            dbobj = new MelmarkDBEntities();
            int ClientID = sess.ReferralId, SchoolId = sess.SchoolId;

            Placement placement = new Placement();
            if (model.Id > 0)
            {
                try
                {
                    placement = dbobj.Placements.Where(objPlacement => objPlacement.PlacementId == model.Id && objPlacement.StudentPersonalId == ClientID).SingleOrDefault();
                    placement.PlacementType = model.PlacementType;
                    placement.BehaviorAnalyst = model.BehaviorAnalyst;
                    placement.UnitClerk = model.UnitClerk;
                    placement.PrimaryNurse = model.PrimaryNurse;
                    placement.Department = model.Department;
                    placement.Status = 1;
                    placement.StartDate = DateTime.ParseExact(model.StartDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    if (model.EndDateDate != null)
                        placement.EndDate = DateTime.ParseExact(model.EndDateDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    else
                        placement.EndDate = null;
                    placement.ModifiedBy = 1;
                    placement.ModifiedOn = DateTime.Now;
                    dbobj.SaveChanges();
                    return "Sucess";
                }
                catch
                {
                    return "Failed";
                }
            }
            else
            {
                if (ClientID == 0)
                {
                    return "No Client Selected";
                }
                else
                {
                    try
                    {
                        placement.SchoolId = SchoolId;
                        placement.PlacementType = model.PlacementType;
                        placement.BehaviorAnalyst = model.BehaviorAnalyst;
                        placement.UnitClerk = model.UnitClerk;
                        placement.PrimaryNurse = model.PrimaryNurse;
                        placement.Department = model.Department;
                        placement.Status = 1;
                        placement.StudentPersonalId = ClientID;
                        placement.StartDate = DateTime.ParseExact(model.StartDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        if (model.EndDateDate != null)
                            placement.EndDate = DateTime.ParseExact(model.EndDateDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        placement.CreatedBy = 1;
                        placement.CreatedOn = DateTime.Now;
                        dbobj.Placements.Add(placement);
                        dbobj.SaveChanges();
                        return "Sucess";
                    }
                    catch
                    {
                        return "Failed";
                    }
                }
            }

        }


        /// <summary>
        /// Function is used to delete Placement.
        /// </summary>
        /// <param name="ClientId"></param>
        /// <param name="itemId"></param>

        public void deletePlacement(int ClientId, int itemId)
        {
            dbobj = new MelmarkDBEntities();
            PlacementModel PlacementModel = new PlacementModel();
            Placement placement = new Placement();
            placement = dbobj.Placements.Where(objPlacement => objPlacement.StudentPersonalId == ClientId && objPlacement.PlacementId == itemId).SingleOrDefault();
            placement.Status = 0;
            dbobj.SaveChanges();
            DeleteFromClass(ClientId, placement.Location.Value, placement.PlacementId);
        }

        public int DeleteFromClass(int StudentId, int ClassId, int placementId)
        {
            dbobj = new MelmarkDBEntities();
            PlacementModel PlacementModel = new PlacementModel();
            Placement placement = new Placement();
            IList<GridListPlacement> retunmodel = new List<GridListPlacement>();
            var currLocationId = dbobj.Placements.Where(x => x.PlacementId == placementId).ToList();

            int? currLocation = currLocationId[0].Location;
            // if (currLocation != ClassId)
            //{
            try
            {
                retunmodel = (from objPlacement in dbobj.Placements
                              join objLookUp in dbobj.LookUps on objPlacement.PlacementType equals objLookUp.LookupId
                              join objLkUp in dbobj.LookUps on objPlacement.Department equals objLkUp.LookupId
                              where (objPlacement.StudentPersonalId == StudentId && objPlacement.Status == 1 && objPlacement.Location == ClassId && objPlacement.PlacementId != placementId)
                              select new GridListPlacement
                              {
                                  PlacementId = objPlacement.PlacementId,
                                  PlacementName = objLookUp.LookupName,
                                  Program = objLkUp.LookupName,
                                  StartDate = objPlacement.StartDate,
                                  EndDate = objPlacement.EndDate,


                              }).ToList();
            }
            catch
            {

            }

            if (retunmodel.Count > 0)
            {
                return 0;
            }
            else
            {
                StdtClass stdtc = new StdtClass();
                var result = dbobj.StdtClasses.Where(x => x.StdtId == StudentId && x.ClassId == currLocation && x.ActiveInd == "A").ToList();

                if (result.Count > 0)
                {
                    result[0].ActiveInd = "D";
                    dbobj.SaveChanges();
                }
                //AssignToClass(StudentId, ClassId);

                return 1;
            }
            //}
            return 0;
        }
        public class GridListPlacement
        {
            public virtual int PlacementId { get; set; }
            public virtual string PlacementName { get; set; }
            public virtual string Program { get; set; }
            public virtual string PlacementnStatus { get; set; }
            public DateTime? EndDate;
            public DateTime? StartDate;
            public virtual string datetime
            {
                get
                {
                    if (EndDate != null)
                    {
                        return ((DateTime)EndDate).ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        return "";
                    }
                }

            }
            public virtual string startdatetime
            {
                get
                {
                    if (StartDate != null)
                    {
                        return ((DateTime)StartDate).ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        return "";
                    }
                }

            }
        }


        //public string SaveMedicalDatas(MedicalModel model)
        //{
        //    sess = (clsSession)HttpContext.Current.Session["UserSession"];
        //    dbobj = new MelmarkDBEntities();
        //    int ClientID = sess.ReferralId, SchoolId = sess.SchoolId;
        //    dbobj = new MelmarkDBEntities();
        //    MedicalAndInsurance Medical = new MedicalAndInsurance();
        //    MedicalAndInsurance test = new MedicalAndInsurance();
        //    Insurance insurance = new Insurance();
        //    AddressList AddrList = new AddressList();
        //    // DateTime lastexam =DateTime.ParseExact(model.DateOfLastPhysicalExam, "yyyy/MM/dd",null);

        //    //string lastexam = model.DateOfLastPhysicalExam;
        //    //string dateformat = "yyyy-MM-dd";

        //    //DateTime dt = DateTime.Parse(lastexam.ToString(dateformat));
        //    //test = dbobj.MedicalAndInsurances.Where(objMedical => objMedical.StudentPersonalId == ClientID && objMedical.DateOfLastPhysicalExam == dt).SingleOrDefault();

        //    //var exmdate = dbobj.MedicalAndInsurances.Where(objMedical => objMedical.StudentPersonalId == ClientID && objMedical.DateOfLastPhysicalExam.Value.ToShortDateString() == model.DateOfLastPhysicalExam).SingleOrDefault();
        //    //if (exmdate != null)
        //    //{
        //    //    return "This date already available";
        //    //}

        //    if (model.ID > 0)
        //    {
        //        try
        //        {

        //            DateTime PhDate = DateTime.ParseExact(model.DateOfLastPhysicalExam, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            Medical = dbobj.MedicalAndInsurances.Where(objMedical => objMedical.StudentPersonalId == ClientID && objMedical.DateOfLastPhysicalExam == PhDate).FirstOrDefault();
        //            if (Medical != null)
        //            {
        //                return "Please go for some other Date";
        //            }
        //            Medical = dbobj.MedicalAndInsurances.Where(objMedical => objMedical.StudentPersonalId == ClientID && objMedical.MedicalInsuranceId == model.ID).SingleOrDefault();
        //            Medical.Allergies = model.Allergies;
        //            Medical.Capabilities = model.Capabilities;
        //            Medical.PhysicianId = model.PhysicianId;
        //            // Medical.City = model.City;
        //            // Medical.CountryId = model.CountryId;
        //            Medical.CurrentMedications = model.CurrentMedications;

        //            Medical.DateOfLastPhysicalExam = model.DateOfLastPhysicalExam;
        //            Medical.FirstName = model.FirstName;
        //            Medical.LastName = model.LastNames;
        //            Medical.Limitations = model.Limitations;
        //            Medical.MedicalConditionsDiagnosis = model.MedicalConditionsDiagnosis;
        //            Medical.ModifiedBy = 1;
        //            Medical.ModifiedOn = System.DateTime.Now;
        //            // Medical.OfficePhone = model.OfficePhone;
        //            Medical.Preferances = model.Preferances;
        //            Medical.SelfPreservationAbility = model.SelfPreservationAbility;
        //            Medical.SignificantBehaviorCharacteristics = model.SignificantBehaviorCharacteristics;
        //            // Medical.StateId = model.StateId;

        //            AddrList = dbobj.AddressLists.Where(objAdress => objAdress.AddressId == Medical.AddressId).SingleOrDefault();
        //            AddrList.CountryId = model.CountryId;
        //            AddrList.StateProvince = model.StateId;
        //            AddrList.City = model.City;
        //            AddrList.Phone = model.OfficePhone;
        //            // dbobj.AddressLists.Add(AddrList);

        //            dbobj.SaveChanges();
        //            return "Sucess";

        //        }
        //        catch (DbEntityValidationException dbEx)
        //        {
        //            foreach (var validationErrors in dbEx.EntityValidationErrors)
        //            {
        //                foreach (var validationError in validationErrors.ValidationErrors)
        //                {
        //                    Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
        //                }
        //            }
        //            return "Failed";
        //        }
        //    }
        //    else
        //    {
        //        if (ClientID == 0)
        //        {
        //            return "No Client Selected";
        //        }
        //        else
        //        {
        //            try
        //            {
        //                DateTime PhDate = DateTime.ParseExact(model.DateOfLastPhysicalExam, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //                Medical = dbobj.MedicalAndInsurances.Where(objMedical => objMedical.StudentPersonalId == ClientID && objMedical.DateOfLastPhysicalExam == PhDate).FirstOrDefault();
        //                if (Medical != null)
        //                {
        //                    return "Please go for some other Date";
        //                }

        //                Medical = new MedicalAndInsurance();
        //                AddrList.CountryId = model.CountryId;
        //                AddrList.StateProvince = model.StateId;
        //                AddrList.City = model.City;
        //                AddrList.Phone = model.OfficePhone;
        //                dbobj.AddressLists.Add(AddrList);
        //                dbobj.SaveChanges();
        //                Medical.Allergies = model.Allergies;
        //                Medical.Capabilities = model.Capabilities;
        //                Medical.PhysicianId = model.PhysicianId;
        //                //  Medical.City = model.City;
        //                //  Medical.CountryId = model.CountryId;
        //                Medical.CreatedBy = 1;
        //                Medical.CreatedOn = System.DateTime.Now;
        //                Medical.CurrentMedications = model.CurrentMedications;

        //                Medical.DateOfLastPhysicalExam = DateTime.ParseExact(model.DateOfLastPhysicalExam, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //                Medical.FirstName = model.FirstName;
        //                Medical.LastName = model.LastNames;
        //                Medical.Limitations = model.Limitations;
        //                Medical.MedicalConditionsDiagnosis = model.MedicalConditionsDiagnosis;
        //                // Medical.OfficePhone = model.OfficePhone;
        //                Medical.Preferances = model.Preferances;
        //                Medical.SchoolId = SchoolId;
        //                Medical.SelfPreservationAbility = model.SelfPreservationAbility;
        //                Medical.SignificantBehaviorCharacteristics = model.SignificantBehaviorCharacteristics;
        //                //  Medical.StateId = model.StateId;
        //                Medical.StudentPersonalId = ClientID;
        //                Medical.AddressId = AddrList.AddressId;
        //                dbobj.MedicalAndInsurances.Add(Medical);



        //                dbobj.SaveChanges();

        //                return "Sucess";
        //            }
        //            catch (DbEntityValidationException dbEx)
        //            {
        //                foreach (var validationErrors in dbEx.EntityValidationErrors)
        //                {
        //                    foreach (var validationError in validationErrors.ValidationErrors)
        //                    {
        //                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
        //                    }
        //                }
        //                return "Failed";
        //            }
        //        }

        //    }


        //}


        //public MedicalModel FillMedicalDatas(int id)
        //{
        //    sess = (clsSession)HttpContext.Current.Session["UserSession"];
        //    MedicalModel returnModel = new MedicalModel();
        //    MedicalAndInsurance tblObjMedicalInsurance = new MedicalAndInsurance();
        //    LookUp objLukup = new LookUp();
        //    AddressList AddrList = new AddressList();
        //    dbobj = new MelmarkDBEntities();
        //    try
        //    {
        //        tblObjMedicalInsurance = dbobj.MedicalAndInsurances.Where(objMedicalInsurance => objMedicalInsurance.MedicalInsuranceId == id &&
        //             objMedicalInsurance.StudentPersonalId == sess.ReferralId).SingleOrDefault();
        //        if (tblObjMedicalInsurance != null)
        //        {
        //            returnModel.Allergies = tblObjMedicalInsurance.Allergies;
        //            returnModel.Capabilities = tblObjMedicalInsurance.Capabilities;
        //            int addressid = (int)tblObjMedicalInsurance.AddressId;
        //            AddrList = dbobj.AddressLists.Where(objAdress => objAdress.AddressId == addressid).SingleOrDefault();
        //            returnModel.City = AddrList.City;
        //            returnModel.CountryId = AddrList.CountryId;
        //            objLukup = dbobj.LookUps.Where(objLookup => objLookup.LookupId == AddrList.CountryId && objLookup.LookupType == "Country").SingleOrDefault();
        //            if (objLukup != null)
        //            {
        //                returnModel.Country = objLukup.LookupName;
        //            }
        //            returnModel.CurrentMedications = tblObjMedicalInsurance.CurrentMedications;
        //            returnModel.DateOfLastPhysicalExam = ConvertDate(tblObjMedicalInsurance.DateOfLastPhysicalExam);
        //            returnModel.FirstName = tblObjMedicalInsurance.FirstName;
        //            returnModel.ID = tblObjMedicalInsurance.MedicalInsuranceId;
        //            returnModel.LastNames = tblObjMedicalInsurance.LastName;
        //            returnModel.Limitations = tblObjMedicalInsurance.Limitations;
        //            returnModel.MedicalConditionsDiagnosis = tblObjMedicalInsurance.MedicalConditionsDiagnosis;
        //            returnModel.OfficePhone = AddrList.Phone;
        //            returnModel.Preferances = tblObjMedicalInsurance.Preferances;
        //            returnModel.PhysicianId = tblObjMedicalInsurance.PhysicianId;
        //            objLukup = dbobj.LookUps.Where(objlookup => objlookup.LookupId == tblObjMedicalInsurance.PhysicianId && objlookup.LookupType == "Physician").SingleOrDefault();
        //            if (objLukup != null)
        //            {
        //                returnModel.Physician = objLukup.LookupName;
        //            }

        //            returnModel.SelfPreservationAbility = tblObjMedicalInsurance.SelfPreservationAbility;
        //            returnModel.SignificantBehaviorCharacteristics = tblObjMedicalInsurance.SignificantBehaviorCharacteristics;
        //            returnModel.StateId = AddrList.StateProvince;
        //            objLukup = dbobj.LookUps.Where(objLookup => objLookup.LookupId == AddrList.StateProvince && objLookup.LookupType == "State").SingleOrDefault();
        //            if (objLukup != null)
        //            {
        //                returnModel.State = objLukup.LookupName;
        //            }
        //        }



        //    }
        //    catch (DbEntityValidationException dbEx)
        //    {
        //        foreach (var validationErrors in dbEx.EntityValidationErrors)
        //        {
        //            foreach (var validationError in validationErrors.ValidationErrors)
        //            {
        //                Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
        //            }
        //        }
        //    }
        //    return returnModel;
        //}

        public void getBindData(out string[] C1, int StudentId, int SchoolId)
        {
            dbobj = new MelmarkDBEntities();
            StudentPersonal Student = new StudentPersonal();
            AddressList Address = new AddressList();
            ContactPersonal Contact = new ContactPersonal();
            StudentAddresRel AddressRel = new StudentAddresRel();
            StudentContactRelationship ContactRel = new StudentContactRelationship();
            string[] IEPC = new string[6];
            int Count = 6;
            try
            {


                Student = dbobj.StudentPersonals.Where(objStudentPersonal => objStudentPersonal.StudentPersonalId == StudentId).SingleOrDefault();

                IEPC[0] = Student.LastName + " " + Student.FirstName;
                IEPC[1] = Student.BirthDate.ToString();
                IEPC[2] = "Dummy";
                IEPC[3] = "Dummy";
                IEPC[4] = "Dummy";
                IEPC[5] = "Dummy";

            }
            catch (Exception Ex)
            {

            }

            C1 = new string[Count];
            if (IEPC != null) Array.Copy(IEPC, C1, Count);

        }


        //To set permission based on role of user
        public string setPermission()
        {
            clsSession session = null;
            MelmarkDBEntities Objdata = new MelmarkDBEntities();
            session = (clsSession)HttpContext.Current.Session["UserSession"];
            string permission = "false";
            var Role = (from Objrole in Objdata.Roles
                        join objrgp in Objdata.RoleGroups on Objrole.RoleId equals objrgp.RoleId
                        select new
                        {
                            RoleId = Objrole.RoleId,
                            Roledesc = Objrole.RoleDesc,
                            schoolid = Objrole.SchoolId,
                            RoleCode = Objrole.RoleCode
                        }).ToList();
            var Usr = (from Objrole in Role
                       from Objusr in Objdata.Users
                       where Objusr.UserId == session.LoginId
                       select new
                       {
                           Objrole.RoleId,
                           Objrole.Roledesc,
                           Objusr.SchoolId,
                           Objusr.UserId,
                           Objusr.UserFName,
                           Objusr.UserLName,
                           Objusr.Gender,
                           Objrole.RoleCode

                       }).ToList();

            var rolePerm = (from objRoleGroupPermission in Objdata.RoleGroupPerms
                            join objObject in Objdata.Objects on objRoleGroupPermission.ObjectId equals objObject.ObjectId
                            join objRoleGroup in Objdata.RoleGroups on objRoleGroupPermission.RoleGroupId equals objRoleGroup.RoleGroupId
                            join objRole in Objdata.Roles on objRoleGroup.RoleId equals objRole.RoleId
                            join objUserRoleGroup in Objdata.UserRoleGroups on objRoleGroup.RoleGroupId equals objUserRoleGroup.RoleGroupId
                            where (objObject.ObjectName == "Referral" || objObject.ObjectName == "Referal") && objUserRoleGroup.UserId == session.LoginId && objUserRoleGroup.ActiveInd == "A"
                            select new
                            {
                                ApproveInd = objRoleGroupPermission.WriteInd

                            }).ToList();

            if (Usr.Count() > 0)
            {
                if (rolePerm.Count > 0)
                {
                    permission = "false";
                    for (int i = 0; i < rolePerm.Count; i++)
                    {
                        if (rolePerm[i].ApproveInd == true)
                            permission = "true";
                    }


                }
                else
                    permission = "false";
            }

            return permission;
        }





        //public string SaveReportData(ProgressList model, bool visibleCheckBox)
        //{
        //    string result = "";
        //    sess = (clsSession)HttpContext.Current.Session["UserSession"];
        //    int ClientID = sess.ReferralId, SchoolId = sess.SchoolId;
        //    dbobj = new MelmarkDBEntities();

        //    Progress_Report rpt = new Progress_Report();
        //    // ParentServiceReference.c
        //    ParentServiceReference.ParentServiceClient obj = new ParentServiceReference.ParentServiceClient();

        //    //  List<Progress> goaldata=new List<Progress>();



        //    try
        //    {
        //        foreach (var item in model.GoalData)
        //        {
        //            foreach (var items in item.RptList)
        //            {

        //                if (items.rptid == 0)
        //                {
        //                    // DateTime date = items.rptdate;
        //                    rpt.Report_Date = items.rptdate;// DateTime.ParseExact(items.rptdate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //                    rpt.Report_Info = items.rptinfo;
        //                    rpt.AllowVisible = visibleCheckBox;
        //                    //var GoalIds = from objStdtLessonPlan in dbobj.StdtLessonPlans
        //                    //              join ObjGoalLPRel in dbobj.GoalLPRels
        //                    //                  //on objStdtLessonPlan.GoalId equals  ObjGoalLPRel.GoalId && objStdtLessonPlan.LessonPlanId  equals  ObjGoalLPRel.LessonPlanId  
        //                    //              on new { a = (int)objStdtLessonPlan.GoalId, b = objStdtLessonPlan.LessonPlanId }
        //                    //              equals new { a = ObjGoalLPRel.GoalId, b = ObjGoalLPRel.LessonPlanId }
        //                    //              where objStdtLessonPlan.StdtIEPId == sess.IEPId
        //                    //              select new
        //                    //              {
        //                    //                  ObjGoalLPRel.GoalLPRelId,

        //                    //              };

        //                    rpt.GoalId = item.GoalLPRelId;
        //                    rpt.StdtIEPId = sess.IEPId;
        //                    rpt.CreatedBy = sess.LoginId;
        //                    rpt.CreatedOn = DateTime.Now;
        //                    rpt.ModifiedBy = sess.LoginId;
        //                    rpt.ModifiedOn = DateTime.Now;
        //                    dbobj.Progress_Report.Add(rpt);
        //                    dbobj.SaveChanges();
        //                    result = "sucess";
        //                }
        //                else
        //                {
        //                    rpt = dbobj.Progress_Report.Where(objProgreeRPT => objProgreeRPT.Report_Id == items.rptid).SingleOrDefault();
        //                    rpt.Report_Date = items.rptdate;// DateTime.ParseExact(items.rptdate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //                    rpt.Report_Info = items.rptinfo;
        //                    rpt.AllowVisible = visibleCheckBox;
        //                    rpt.GoalId = item.GoalLPRelId;
        //                    rpt.StdtIEPId = sess.IEPId;
        //                    rpt.CreatedBy = sess.LoginId;
        //                    rpt.CreatedOn = DateTime.Now;
        //                    rpt.ModifiedBy = sess.LoginId;
        //                    rpt.ModifiedOn = DateTime.Now;
        //                    dbobj.SaveChanges();
        //                    result = "sucess";
        //                }

        //            }
        //        }
        //    }
        //    catch
        //    {
        //        result = "failed";
        //    }



        //    return result;
        //}
    }
}