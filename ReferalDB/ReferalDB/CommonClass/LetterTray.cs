using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BuisinessLayer;
using DataLayer;
using ReferalDB.Models;


namespace ReferalDB.CommonClass
{
    public class LetterTray
    {
        public clsSession session = null;
        //public IList<CommmonCheckListViewModel> insertUsersList(IList<CommmonCheckListViewModel> UpdateVals, string queueType, int studentId)
        //{
        //    MelmarkDBEntities objData = new MelmarkDBEntities();
        //    session = (clsSession)HttpContext.Current.Session["UserSession"];
        //    ClsCommon getcheck = new ClsCommon();
        //    ref_LetterTrayValues LetterTrayValues = new ref_LetterTrayValues();
        //    ref_LetterTrayAssign LetterTrayAssign = new ref_LetterTrayAssign();
        //    LetterTrayDetailsViewModel studentDetails = new LetterTrayDetailsViewModel();
        //    string multiUserIds="";
        //    //string LetterName = "";
        //    int letterTrayId = 0;
        //    int qId = getcheck.getQueueId(queueType);
        //    var LetterTrayValue = objData.ref_LetterTrayValues.Where(x => x.QueueId == qId && x.StudentPersonalId==studentId).ToList();
        //    //var letterTrayAssigned=objData.ref_LetterTrayAssign.Where()
        //    studentDetails = StudentDetails(studentId);
        //    foreach (var Chkupdate in UpdateVals)
        //    {
        //        if (Chkupdate.AssignMultiId != "0")
        //        {
        //            multiUserIds+=Chkupdate.AssignMultiId+",";
        //        }
        //    }
        //             multiUserIds = multiUserIds.TrimEnd(',');
        //                var LetterIds = objData.LetterEngines.Where(x => x.LetterType == "Letter" && x.QueueId==qId).ToList();
        //                if (LetterIds.Count > 0)
        //                {
        //                    foreach(var LetterId in LetterIds)
        //                    {
        //                        var LetterItemContent = objData.LetterEngineItems.Where(x => x.LetterEngineId == LetterId.LetterEngineId).ToList();
        //                        if (LetterItemContent.Count > 0)
        //                        {
        //                            foreach (var LetterItemContents in LetterItemContent)
        //                            {
        //                            //LetterName += LetterId.LetterEngineName + ",";
        //                            var studentdetail = studentDetails.Split(';');
        //                            string content = LetterItemContents.ItemContent;
        //                            content = content.Replace("[Student Name]", "Student Name :" + studentdetail[0] + "<br />");
        //                            content = content.Replace("[Last Name]", "Last Name :" + studentdetail[1] + "<br />");
        //                            content = content.Replace("[Date]", "Date :" + studentdetail[2] + "<br />");
        //                            content = content.Replace("[Date of birth]", "Date of birth :" + studentdetail[3] + "<br />");
        //                            content = content.Replace("[Address]", "Address :" + studentdetail[4]);
        //                            if (LetterTrayValue.Count == 0)
        //                            {
        //                                LetterTrayValues.QueueId = qId;
        //                                LetterTrayValues.StudentPersonalId = studentId;
        //                                LetterTrayValues.TrayValue = content;
        //                                LetterTrayValues.LetterId = LetterItemContents.LetterEngineId;
        //                                LetterTrayValues.CreatedBy = session.LoginId;
        //                                LetterTrayValues.CreatedOn = System.DateTime.Now;
        //                                objData.ref_LetterTrayValues.Add(LetterTrayValues);
        //                                objData.SaveChanges();
        //                                letterTrayId = LetterTrayValues.LetterTrayId;
        //                            }
        //                            else
        //                            {
        //                                LetterTrayValue[0].QueueId = qId;
        //                                LetterTrayValue[0].StudentPersonalId = studentId;
        //                                LetterTrayValue[0].TrayValue = content;
        //                                LetterTrayValue[0].LetterId = LetterItemContents.LetterEngineId;
        //                                LetterTrayValue[0].ModifiedBy = session.LoginId;
        //                                LetterTrayValue[0].ModifiedOn = System.DateTime.Now;
        //                                objData.SaveChanges();
        //                                letterTrayId = LetterTrayValue[0].LetterTrayId;
        //                            }
        //                            var letterTrayAssigned = objData.ref_LetterTrayAssign.Where(x => x.LetterTrayId == letterTrayId).ToList();
        //                            if (letterTrayAssigned.Count > 0)
        //                            {
        //                                for (int i = 0; i < letterTrayAssigned.Count; i++)
        //                                {
        //                                    objData.ref_LetterTrayAssign.Remove(letterTrayAssigned[i]);
        //                                    objData.SaveChanges();
        //                                }
        //                            }
        //                            var Allid = multiUserIds.Split(',');
        //                            for (int i = 0; i < Allid.Length; i++)
        //                            {
        //                                int id = Convert.ToInt32(Allid[i]);

        //                                LetterTrayAssign.CreatedBy = session.LoginId;
        //                                LetterTrayAssign.CreatedOn = System.DateTime.Now;
        //                                LetterTrayAssign.LetterTrayId = letterTrayId;
        //                                LetterTrayAssign.LetterUserId = id;
        //                                objData.ref_LetterTrayAssign.Add(LetterTrayAssign);
        //                                objData.SaveChanges();
        //                            }
        //                          }
        //                        }

        //                    }
        //                }
        //                //LetterName = LetterName.TrimEnd(',');
        //                return null;
        //}

        public LetterTrayDetailsViewModel StudentDetails(int studentId)
        {
            string date = null;
            string birthdate = null;
            string recievedate = null;
            string LookupNameFather = "";
            string LookupNameMother = "";
            session = (clsSession)HttpContext.Current.Session["UserSession"];
            int schoolId = Convert.ToInt32(session.SchoolId);
            MelmarkDBEntities objData = new MelmarkDBEntities();
            var studentPersonalDetails = objData.StudentPersonals.Single(x => x.StudentPersonalId == session.ReferralId);
            var studentAddressId = objData.StudentAddresRels.Single(x => x.ContactSequence == 0 && x.StudentPersonalId == session.ReferralId);
            var studentAddressDetails = objData.AddressLists.Single(x => x.AddressId == studentAddressId.AddressId);
            if (studentPersonalDetails.AdmissionDate != null)
                date = ((DateTime)studentPersonalDetails.AdmissionDate).ToString("MM'-'dd'-'yyyy");
            if (studentPersonalDetails.BirthDate != null)
                birthdate = ((DateTime)studentPersonalDetails.BirthDate).ToString("MM'-'dd'-'yyyy");
            if (studentPersonalDetails.CreatedOn != null)
                recievedate = ((DateTime)studentPersonalDetails.CreatedOn).ToString("MM'-'dd'-'yyyy");
            string day = System.DateTime.Now.DayOfWeek + "," + (System.DateTime.Now.Date).ToString("MM'-'dd'-'yyyy");
            if (schoolId == 1)
            {
                LookupNameFather = "Father";
                LookupNameMother = "Mother";
            }

            else if (schoolId == 2)
            {
                LookupNameFather = "Legal Guardian 1";
                LookupNameMother = "Legal Guardian 2";
            }

            var lukUpFathrId = ((from LookUp lukup in objData.LookUps
                                 where (lukup.LookupName == LookupNameFather)
                                 select new
                                 {
                                     lukUpFathrId = lukup.LookupId

                                 }).SingleOrDefault()).lukUpFathrId;

            var lukUpMthrId = ((from LookUp lukup in objData.LookUps
                                where (lukup.LookupName == LookupNameMother)
                                select new
                                {
                                    lukUpMthrId = lukup.LookupId

                                }).SingleOrDefault()).lukUpMthrId;

            var lukupLegalGurdianId = ((from LookUp lukup in objData.LookUps
                                        where (lukup.LookupName == "Legal Guardian")
                                        select new
                                        {
                                            lukupCloseId = lukup.LookupId


                                        }).SingleOrDefault()).lukupCloseId;

            var objRelationMthr = (from objStudntPersnl in objData.StudentPersonals
                                   join StdAddrRel in objData.StudentAddresRels on objStudntPersnl.StudentPersonalId equals StdAddrRel.StudentPersonalId
                                   join ConPersonal in objData.ContactPersonals on StdAddrRel.ContactPersonalId equals ConPersonal.ContactPersonalId
                                   join Addr in objData.AddressLists on StdAddrRel.AddressId equals Addr.AddressId
                                   join StudContactRel in objData.StudentContactRelationships on ConPersonal.ContactPersonalId equals StudContactRel.ContactPersonalId
                                   where (StudContactRel.RelationshipId == lukUpMthrId & StdAddrRel.ContactSequence == 1 & objStudntPersnl.StudentPersonalId == session.ReferralId)
                                   select new RelationCLs
                                   {
                                       txtFirstName = ConPersonal.FirstName,
                                       txtLstName = ConPersonal.LastName,
                                       txtMiddleName = ConPersonal.MiddleName,
                                       txtCountry = Addr.CountryId,
                                       txtState = Addr.StateProvince,
                                       txtStreetAdress = Addr.StreetName,
                                       txtCity = Addr.City,
                                       txtAprtmentUnit = Addr.ApartmentType,
                                       txtZipCode = Addr.PostalCode,
                                       txtHomePhone = Addr.Phone,
                                       txtWorkPhone = Addr.OtherPhone,
                                       txtEmail = Addr.PrimaryEmail

                                   }).FirstOrDefault();


            var objRelationFthr = (from objStudntPersnl in objData.StudentPersonals
                                   join StdAddrRel in objData.StudentAddresRels on objStudntPersnl.StudentPersonalId equals StdAddrRel.StudentPersonalId
                                   join ConPersonal in objData.ContactPersonals on StdAddrRel.ContactPersonalId equals ConPersonal.ContactPersonalId
                                   join Addr in objData.AddressLists on StdAddrRel.AddressId equals Addr.AddressId
                                   join StudContactRel in objData.StudentContactRelationships on ConPersonal.ContactPersonalId equals StudContactRel.ContactPersonalId
                                   where (StudContactRel.RelationshipId == lukUpFathrId & StdAddrRel.ContactSequence == 1 & objStudntPersnl.StudentPersonalId == session.ReferralId)
                                   select new RelationCLs
                                   {
                                       txtFirstName = ConPersonal.FirstName,
                                       txtLstName = ConPersonal.LastName,
                                       txtMiddleName = ConPersonal.MiddleName,
                                       txtCountry = Addr.CountryId,
                                       txtState = Addr.StateProvince,
                                       txtStreetAdress = Addr.StreetName,
                                       txtCity = Addr.City,
                                       txtAprtmentUnit = Addr.ApartmentType,
                                       txtZipCode = Addr.PostalCode,
                                       txtHomePhone = Addr.Phone,
                                       txtWorkPhone = Addr.OtherPhone,
                                       txtEmail = Addr.PrimaryEmail
                                   }).FirstOrDefault();

            var objRelationLegalGuardian = (from objStudntPersnl in objData.StudentPersonals
                                            join StdAddrRel in objData.StudentAddresRels on objStudntPersnl.StudentPersonalId equals StdAddrRel.StudentPersonalId
                                            join ConPersonal in objData.ContactPersonals on StdAddrRel.ContactPersonalId equals ConPersonal.ContactPersonalId
                                            join Addr in objData.AddressLists on StdAddrRel.AddressId equals Addr.AddressId
                                            join StudContactRel in objData.StudentContactRelationships on ConPersonal.ContactPersonalId equals StudContactRel.ContactPersonalId
                                            where (StudContactRel.RelationshipId == lukupLegalGurdianId & StdAddrRel.ContactSequence == 1 & objStudntPersnl.StudentPersonalId == session.ReferralId)
                                            select new RelationCLs
                                            {
                                                txtFirstName = ConPersonal.FirstName,
                                                txtLstName = ConPersonal.LastName,
                                                txtMiddleName = ConPersonal.MiddleName,
                                                txtCountry = Addr.CountryId,
                                                txtState = Addr.StateProvince,
                                                txtStreetAdress = Addr.StreetName,
                                                txtCity = Addr.City,
                                                txtAprtmentUnit = Addr.ApartmentType,
                                                txtZipCode = Addr.PostalCode,
                                                txtHomePhone = Addr.Phone,
                                                txtWorkPhone = Addr.OtherPhone,
                                                txtEmail = Addr.PrimaryEmail

                                            }).FirstOrDefault();
            var schoolDetails = objData.Schools.Where(x => x.SchoolId == session.SchoolId).SingleOrDefault();

            //string studentDetails = 
            //    studentPersonalDetails.LastName + ";" + studentPersonalDetails.FirstName + ";" 
            //    + objRelationFthr.txtLstName + ";" + objRelationFthr.txtFirstName + ";" 
            //    + objRelationMthr.txtFirstName + ";" + objRelationMthr.txtFirstName + ";"
            //     + birthdate + ";" + date + ";" 
            //     + studentAddressDetails.AddressLine1 + "," + studentAddressDetails.AddressLine2 + "," + studentAddressDetails.AddressLine3 + ","
            //     + objRelationLegalGuardian.txtLstName + "," + objRelationLegalGuardian.txtFirstName
            //     + day + ";" + schoolDetails.DistrictName+";"
            //     + objRelationFthr.txtHomePhone + "," + objRelationFthr.txtEmail + "," + objRelationFthr.txtZipCode + "," + objRelationFthr.txtCity
            //     + objRelationFthr.txtState + "," + objRelationFthr.txtFax ;

            var stateName = objData.LookUps.Where(x => x.LookupId == studentAddressDetails.StateProvince && x.LookupType == "State").ToList();

            LetterTrayDetailsViewModel studentDetails = new LetterTrayDetailsViewModel();
            studentDetails.ReferralFirstName = studentPersonalDetails.FirstName;
            studentDetails.ReferralLastName = studentPersonalDetails.LastName;
            if (objRelationMthr != null)
            {
                studentDetails.MotherFirstName = objRelationMthr.txtFirstName;
                studentDetails.MotherLastName = objRelationMthr.txtLstName;
            }
            if (objRelationFthr != null)
            {
                studentDetails.FatherFirstName = objRelationFthr.txtFirstName;
                studentDetails.FatherLastName = objRelationFthr.txtLstName;
            }
            studentDetails.BirthDate = birthdate;
            studentDetails.RecieveLetterDate = recievedate;
            studentDetails.ApplicationDate = date;
            studentDetails.AddressLine1 = studentAddressDetails.AddressLine1;
            studentDetails.AddressLine2 = studentAddressDetails.AddressLine2;
            studentDetails.AddressLine3 = studentAddressDetails.AddressLine3;
            if (objRelationLegalGuardian != null)
            {
                studentDetails.GuardianFirstName = objRelationLegalGuardian.txtFirstName;
                studentDetails.GuardianLastName = objRelationLegalGuardian.txtLstName;
            }
            studentDetails.DayDate = day;
            studentDetails.DistrictName = schoolDetails.DistrictName;
            studentDetails.Phone = studentAddressDetails.Phone;
            studentDetails.Email = studentAddressDetails.PrimaryEmail;
            studentDetails.Zip = studentAddressDetails.PostalCode;
            studentDetails.City = studentAddressDetails.City;
            if (stateName.Count > 0)
                studentDetails.State = stateName[0].LookupName;
            studentDetails.Fax = studentAddressDetails.Fax;
            return studentDetails;
        }

        public IList<LetterList> getLetterAll()
        {

            session = (clsSession)HttpContext.Current.Session["UserSession"];
            MelmarkDBEntities objData = new MelmarkDBEntities();
            var LetterList = new List<Models.LetterList>();
            LetterList = (from x in objData.ref_LetterTrayValues
                          join y in objData.LetterEngines on x.LetterId equals y.LetterEngineId
                          join z in objData.ref_Recipients on x.RecipientId equals z.RecipientId
                          join referral in objData.StudentPersonals on x.StudentPersonalId equals referral.StudentPersonalId
                          select new LetterList
                          {
                              LetterName = y.LetterEngineName,
                              ReferralName = referral.LastName + "," + referral.FirstName,
                              ReferralFName = referral.FirstName,
                              ReferralLName = referral.LastName,
                              RecipientName = z.RecipientName,
                              CreatedOn = x.CreatedOn,
                              //checkListval=x.Status.ToString(),
                              status = (bool)x.Status,
                              SentOn = x.ModifiedOn,
                              LetterTrayId = x.LetterTrayId
                          }).ToList();

            return LetterList;
        }

        public IList<LetterList> getLetterList(string Name)
        {
            string[] ar = new string[2];
            string nme = "";
            string search = "";
            if (Name == null) Name = "";

            if ((Name != "")&&(Name!=null))
            {
                if (Name.Contains('_'))
                {

                    ar = Name.Split('_');
                    if (ar.Length > 0)
                    {
                        if (ar.Length > 1)
                        {
                            nme = ar[0];
                            search = ar[1];
                        }
                        else { search = ar[0]; }
                    }
                }
                else if (Name.Contains(' '))
                {
                    ar = Name.Split(' ');
                    if (ar.Length > 0)
                    {
                        if (ar.Length > 1)
                        {
                            nme = ar[0];
                            search = ar[1];
                        }
                        else { search = ar[0]; }
                    }

                }
                else if (Name.Contains(','))
                {
                    ar = Name.Split(',');
                    if (ar.Length > 0)
                    {
                        if (ar.Length > 1)
                        {
                            nme = ar[0];
                            search = ar[1];
                        }
                        else { search = ar[0]; }
                    }
                }
                else
                {
                    nme = Name;
                    search = Name;
                }
            }

            session = (clsSession)HttpContext.Current.Session["UserSession"];
            MelmarkDBEntities objData = new MelmarkDBEntities();
            var LetterList = new List<Models.LetterList>();

            if (search == "")
            {
                if (session.ReferralId != 0)
                {
                    LetterList = (from x in objData.ref_LetterTrayValues
                                  join y in objData.LetterEngines on x.LetterId equals y.LetterEngineId
                                  join z in objData.ref_Recipients on x.RecipientId equals z.RecipientId
                                  join referral in objData.StudentPersonals on x.StudentPersonalId equals referral.StudentPersonalId
                                  where (x.StudentPersonalId == session.ReferralId)
                                  select new LetterList
                                  {
                                      LetterName = y.LetterEngineName,
                                      ReferralName = referral.LastName + "," + referral.FirstName,
                                      ReferralFName = referral.FirstName,
                                      ReferralLName = referral.LastName,
                                      RecipientName = z.RecipientName,
                                      CreatedOn = x.CreatedOn,
                                      //checkListval=x.Status.ToString(),
                                      status = (bool)x.Status,
                                      SentOn = x.ModifiedOn,
                                      LetterTrayId = x.LetterTrayId,
                                      LetterQueueId = x.QueueId //--- List 3 - Task #30 [20-Oct-2020] ---//
                                  }).ToList();
                }
                else
                {
                    LetterList = (from x in objData.ref_LetterTrayValues
                                  join y in objData.LetterEngines on x.LetterId equals y.LetterEngineId
                                  join z in objData.ref_Recipients on x.RecipientId equals z.RecipientId
                                  join referral in objData.StudentPersonals on x.StudentPersonalId equals referral.StudentPersonalId
                                  select new LetterList
                                  {
                                      LetterName = y.LetterEngineName,
                                      ReferralName = referral.LastName + "," + referral.FirstName,
                                      ReferralFName = referral.FirstName,
                                      ReferralLName = referral.LastName,
                                      RecipientName = z.RecipientName,
                                      CreatedOn = x.CreatedOn,
                                      //checkListval=x.Status.ToString(),
                                      status = (bool)x.Status,
                                      SentOn = x.ModifiedOn,
                                      LetterTrayId = x.LetterTrayId,
                                      LetterQueueId = x.QueueId //--- List 3 - Task #30 [20-Oct-2020] ---//
                                  }).ToList();

                    if (!String.IsNullOrEmpty(nme))
                        LetterList = LetterList.Where(p => p.ReferralFName.ToLower().Contains(nme.ToLower()) || p.ReferralLName.ToLower().Contains(nme.ToLower())).ToList();
                    else
                        LetterList = null;

                }
            }
            else
            {
                
                //LetterList = (from x in objData.ref_LetterTrayValues
                //              join y in objData.LetterEngines on x.LetterId equals y.LetterEngineId
                //              join z in objData.ref_Recipients on x.RecipientId equals z.RecipientId
                //              join referral in objData.StudentPersonals on x.StudentPersonalId equals referral.StudentPersonalId
                //              select new LetterList
                //              {
                //                  LetterName = y.LetterEngineName,
                //                  ReferralName = referral.LastName + "," + referral.FirstName,
                //                  ReferralFName = referral.FirstName,
                //                  ReferralLName = referral.LastName,
                //                  RecipientName = z.RecipientName,
                //                  CreatedOn = x.CreatedOn,
                //                  //checkListval=x.Status.ToString(),
                //                  status = (bool)x.Status,
                //                  SentOn = x.ModifiedOn,
                //                  LetterTrayId = x.LetterTrayId
                //              }).ToList();

                if (session.ReferralId != 0)
                {
                    LetterList = (from x in objData.ref_LetterTrayValues
                                  join y in objData.LetterEngines on x.LetterId equals y.LetterEngineId
                                  join z in objData.ref_Recipients on x.RecipientId equals z.RecipientId
                                  join referral in objData.StudentPersonals on x.StudentPersonalId equals referral.StudentPersonalId
                                  where (x.StudentPersonalId == session.ReferralId)
                                  select new LetterList
                                  {
                                      LetterName = y.LetterEngineName,
                                      ReferralName = referral.LastName + "," + referral.FirstName,
                                      ReferralFName = referral.FirstName,
                                      ReferralLName = referral.LastName,
                                      RecipientName = z.RecipientName,
                                      CreatedOn = x.CreatedOn,
                                      //checkListval=x.Status.ToString(),
                                      status = (bool)x.Status,
                                      SentOn = x.ModifiedOn,
                                      LetterTrayId = x.LetterTrayId,
                                      LetterQueueId = x.QueueId //--- List 3 - Task #30 [20-Oct-2020] ---//
                                  }).ToList();
                }
                else
                {
                    LetterList = (from x in objData.ref_LetterTrayValues
                                  join y in objData.LetterEngines on x.LetterId equals y.LetterEngineId
                                  join z in objData.ref_Recipients on x.RecipientId equals z.RecipientId
                                  join referral in objData.StudentPersonals on x.StudentPersonalId equals referral.StudentPersonalId
                                  select new LetterList
                                  {
                                      LetterName = y.LetterEngineName,
                                      ReferralName = referral.LastName + "," + referral.FirstName,
                                      ReferralFName = referral.FirstName,
                                      ReferralLName = referral.LastName,
                                      RecipientName = z.RecipientName,
                                      CreatedOn = x.CreatedOn,
                                      //checkListval=x.Status.ToString(),
                                      status = (bool)x.Status,
                                      SentOn = x.ModifiedOn,
                                      LetterTrayId = x.LetterTrayId,
                                      LetterQueueId = x.QueueId //--- List 3 - Task #30 [20-Oct-2020] ---//
                                  }).ToList();

                    if (!String.IsNullOrEmpty(nme))
                        LetterList = LetterList.Where(p => p.ReferralFName.ToLower().Contains(nme.ToLower()) || p.ReferralLName.ToLower().Contains(nme.ToLower())).ToList();
                    if (LetterList.Count == 0)
                    {
                        LetterList = null;
                        return LetterList;
                    }

                }
                //if (!String.IsNullOrEmpty(nme))
                //    LetterList = LetterList.Where(p => p.ReferralFName.ToLower().Contains(nme.ToLower()) || p.ReferralLName.ToLower().Contains(nme.ToLower())).ToList();
                //if (LetterList.Count == 0)
                //{
                //    LetterList = null;
                //    return LetterList;
                //}


            }
            foreach (var Letter in LetterList)
            {
                if (Letter.CreatedOn != null)
                    Letter.CreatedDate = ((DateTime)Letter.CreatedOn).ToString("MM'/'dd'/'yyyy");
                if (Letter.SentOn != null)
                    Letter.SentDate = ((DateTime)Letter.SentOn).ToString("MM'/'dd'/'yyyy");
            }


            return LetterList;
        }

        //Get LetterIEngineId from LetterEngine table
        //public int getLetterId(string LetterName)
        //{
        //    int LetterId = 0;
        //    MelmarkDBEntities objData = new MelmarkDBEntities();
        //    var LetterDetails = objData.LetterEngines.Where(x => x.LetterEngineName == LetterName).ToList();
        //    if (LetterDetails.Count > 0)
        //        LetterId = LetterDetails[0].LetterEngineId;
        //     return LetterId;
        //}


        //Get RecipientId from ref_Recipient table
        public int getRecipientId(string recipient)
        {
            int RecipientId = 0;
            MelmarkDBEntities objData = new MelmarkDBEntities();
            var RecipientDetails = objData.ref_Recipients.Where(x => x.RecipientName == recipient).ToList();
            if (RecipientDetails.Count > 0)
                RecipientId = RecipientDetails[0].RecipientId;
            return RecipientId;
        }


        public void insertLetter(string QType, int studentPersonalId, int LetterId, string recipient)
        {
            //if (QType == "NA" || QType == "AR" || QType == "CR" || QType == "SI" || QType == "PS" || QType == "FV" || QType == "CA" || QType == "PI" || QType == "IE" || QType == "AT" ) //Commented for adding other letter 'OTH' to tray
            if (QType == "NA" || QType == "AR" || QType == "CR" || QType == "SI" || QType == "PS" || QType == "FV" || QType == "CA" || QType == "PI" || QType == "IE" || QType == "AT" || QType == "OTH")
            {
                session = (clsSession)HttpContext.Current.Session["UserSession"];
                MelmarkDBEntities objData = new MelmarkDBEntities();
                ref_LetterTrayValues LetterTrayValues = new ref_LetterTrayValues();
                LetterTrayDetailsViewModel studentDetails = new LetterTrayDetailsViewModel();
                ClsCommon getcheck = new ClsCommon();
                string LetterContent = "";
                int Qid = getcheck.getQueueId(QType);
                //int LetterId = getLetterId(LetterName);
                int RecipientId = getRecipientId(recipient);
                studentDetails = StudentDetails(studentPersonalId);
                //var studentdetail = studentDetails.Split(';');

                var LetterItemContent = objData.LetterEngineItems.Where(x => x.LetterEngineId == LetterId).ToList();
                if (LetterItemContent.Count > 0)
                {
                    LetterContent = LetterItemContent[0].ItemContent;
                    if (LetterContent != null)
                    {
                        LetterContent = LetterContent.Replace("#ffff00", " #ffffff");
                        if (studentDetails.ReferralLastName != null && studentDetails.ReferralFirstName != null)
                            LetterContent = LetterContent.Replace("[Referral Name]", studentDetails.ReferralLastName + "," + studentDetails.ReferralFirstName);
                        else
                            LetterContent = LetterContent.Replace("[Referral Name]", " ");

                        if (studentDetails.FatherLastName != null && studentDetails.FatherFirstName != null)
                            LetterContent = LetterContent.Replace("[Father Name]", studentDetails.FatherLastName + "," + studentDetails.FatherFirstName);
                        else
                            LetterContent = LetterContent.Replace("[Father Name]", " ");

                        if (studentDetails.MotherLastName != null && studentDetails.MotherFirstName != null)
                            LetterContent = LetterContent.Replace("[Mother Name]", studentDetails.MotherLastName + "," + studentDetails.MotherFirstName);
                        else
                            LetterContent = LetterContent.Replace("[Mother Name]", " ");


                        if (studentDetails.BirthDate != null)
                            LetterContent = LetterContent.Replace("[Date of birth]", studentDetails.BirthDate);
                        else
                            LetterContent = LetterContent.Replace("[Date of birth]", " ");

                        if (studentDetails.ApplicationDate != null)
                            LetterContent = LetterContent.Replace("[Application Date]", studentDetails.ApplicationDate);
                        else
                            LetterContent = LetterContent.Replace("[Application Date]", " ");

                        if (studentDetails.AddressLine1 != null && studentDetails.AddressLine1 != null && studentDetails.AddressLine3 != null)
                            LetterContent = LetterContent.Replace("[Address]", studentDetails.AddressLine1 + "," + studentDetails.AddressLine2 + "," + studentDetails.AddressLine3);
                        else
                            LetterContent = LetterContent.Replace("[Address]", " ");

                        LetterContent = LetterContent.Replace("[Mr/Ms]", "Mr/Ms");

                        if (studentDetails.GuardianLastName != null && studentDetails.GuardianFirstName != null)
                            LetterContent = LetterContent.Replace("[Recipient]", studentDetails.GuardianLastName + "," + studentDetails.GuardianFirstName);
                        else
                            LetterContent = LetterContent.Replace("[Recipient]", " ");

                        if (studentDetails.DayDate != null)
                            LetterContent = LetterContent.Replace("[Day, Date]", studentDetails.DayDate);
                        else
                            LetterContent = LetterContent.Replace("[Day, Date]", " ");

                        if (studentDetails.DayDate != null)
                            LetterContent = LetterContent.Replace("[Recieve Letter Date]", studentDetails.RecieveLetterDate);
                        else
                            LetterContent = LetterContent.Replace("[Recieve Letter Date]", " ");

                        if (studentDetails.DistrictName != null)
                            LetterContent = LetterContent.Replace("[School District]", studentDetails.DistrictName);
                        else
                            LetterContent = LetterContent.Replace("[School District]", " ");

                        if (studentDetails.Phone != null)
                            LetterContent = LetterContent.Replace("[Phone (Home/work)]", studentDetails.Phone);
                        else
                            LetterContent = LetterContent.Replace("[Phone (Home/work)]", " ");

                        if (studentDetails.Email != null)
                            LetterContent = LetterContent.Replace("[Email]", studentDetails.Email);
                        else
                            LetterContent = LetterContent.Replace("[Email]", " ");

                        if (studentDetails.Zip != null)
                            LetterContent = LetterContent.Replace("[Zip]", studentDetails.Zip);
                        else
                            LetterContent = LetterContent.Replace("[Zip]", " ");

                        if (studentDetails.City != null)
                            LetterContent = LetterContent.Replace("[City]", studentDetails.City);
                        else
                            LetterContent = LetterContent.Replace("[City]", " ");

                        if (studentDetails.State != null)
                            LetterContent = LetterContent.Replace("[State]", studentDetails.State);
                        else
                            LetterContent = LetterContent.Replace("[State]", " ");

                        if (studentDetails.Fax != null)
                            LetterContent = LetterContent.Replace("[Fax Number]", studentDetails.Fax);
                        else
                            LetterContent = LetterContent.Replace("[Fax Number]", " ");
                    }
                    else
                    {
                        LetterContent = "";
                    }
                    //insert values to ref_LetterTrayValues
                    LetterTrayValues.QueueId = Qid;
                    LetterTrayValues.StudentPersonalId = studentPersonalId;
                    LetterTrayValues.TrayValue = LetterContent;
                    LetterTrayValues.LetterId = LetterId;
                    LetterTrayValues.RecipientId = RecipientId;
                    LetterTrayValues.Status = false;
                    LetterTrayValues.CreatedBy = session.LoginId;

                    LetterTrayValues.CreatedOn = DateTime.ParseExact(studentDetails.RecieveLetterDate, "MM'-'dd'-'yyyy", System.Globalization.CultureInfo.CurrentCulture); //Convert.ToDateTime(studentDetails.RecieveLetterDate); // System.DateTime.Now;
                    objData.ref_LetterTrayValues.Add(LetterTrayValues);
                    objData.SaveChanges();

                }
            }

        }
    }
}