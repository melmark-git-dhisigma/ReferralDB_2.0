using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer;
using ReferalDB.Models;
using System.Web.Mvc;
using System.Data.Objects.SqlClient;
using System.Transactions;
using System.IO;
using System.Xml;
using System.Data.SqlClient;

namespace ReferalDBApplicant.Classes
{
    public class clsReferral
    {
        public clsSession session = null;
        public int SaveGeneralData(GenInfoModel model, string type, int saveId, string status, int loginId, int schoolId)
        {
            int retVal = 0;
            MelmarkDBEntities DbEntity = new MelmarkDBEntities();
            StudentPersonal StudentTbl = new StudentPersonal();
            AddressList AddrList = new AddressList();
            StudentAddresRel StdAdrRel = new StudentAddresRel();
            ContactPersonal ConPersonal = new ContactPersonal();
            LookUp LukUp = new LookUp();
            StudentContactRelationship StdCntctRel = new StudentContactRelationship();
            DiaganosesPA Diagnose = new DiaganosesPA();
            MedicalAndInsurance medicalIns = new MedicalAndInsurance();
            StudentPersonalPA StudPersPa = new StudentPersonalPA();
            Insurance Insurnce = new Insurance();
            BehavioursPA BehaviorPa = new BehavioursPA();

            try
            {

                try
                {
                    model.lukUpFathrId = ((from LookUp lukup in DbEntity.LookUps
                                           where (lukup.LookupName == "Father")
                                           select new
                                           {
                                               lukUpFathrId = lukup.LookupId

                                           }).SingleOrDefault()).lukUpFathrId;

                    model.lukUpMthrId = ((from LookUp lukup in DbEntity.LookUps
                                          where (lukup.LookupName == "Mother")
                                          select new
                                          {
                                              lukUpMthrId = lukup.LookupId


                                          }).SingleOrDefault()).lukUpMthrId;

                    model.lukupCloseId = ((from LookUp lukup in DbEntity.LookUps
                                           where (lukup.LookupName == "None")
                                           select new
                                           {
                                               lukupCloseId = lukup.LookupId


                                           }).SingleOrDefault()).lukupCloseId;

                    model.lukupLegalGurdianId = ((from LookUp lukup in DbEntity.LookUps
                                                  where (lukup.LookupName == "Legal Guardian")
                                                  select new
                                                  {
                                                      lukupCloseId = lukup.LookupId


                                                  }).SingleOrDefault()).lukupCloseId;


                    model.lukupEmergencyId = ((from LookUp lukup in DbEntity.LookUps
                                               where (lukup.LookupName == "Emergency Contact")
                                               select new
                                               {
                                                   lukupCloseId = lukup.LookupId


                                               }).SingleOrDefault()).lukupCloseId;


                }
                catch
                {
                    model.lukUpFathrId = 0;
                    model.lukUpMthrId = 0;
                }



                using (TransactionScope trans = new TransactionScope(TransactionScopeOption.Required))
                {
                    ClsErrorLog erorLg = new ClsErrorLog();

                    if ((saveId > 0) && (status == "update"))
                    {

                        if (type == "General Information")
                        {

                            int RefaddrId = (from objStudntPersnl in DbEntity.StudentPersonals
                                             join objStdtAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals objStdtAddrRel.StudentPersonalId
                                             join objAddress in DbEntity.AddressLists on objStdtAddrRel.AddressId equals objAddress.AddressId
                                             where (objStudntPersnl.StudentPersonalId == saveId & objStdtAddrRel.ContactSequence == 0)

                                             select new
                                             {
                                                 AdressId = objAddress.AddressId

                                             }).SingleOrDefault().AdressId;

                            int mothrAdressId = 0;
                            var mothrAdress = (from objStudntPersnl in DbEntity.StudentPersonals
                                               join StdAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals StdAddrRel.StudentPersonalId
                                               join ConPrsonal in DbEntity.ContactPersonals on StdAddrRel.ContactPersonalId equals ConPrsonal.ContactPersonalId
                                               join Addr in DbEntity.AddressLists on StdAddrRel.AddressId equals Addr.AddressId
                                               join StudContactRel in DbEntity.StudentContactRelationships on ConPrsonal.ContactPersonalId equals StudContactRel.ContactPersonalId
                                               where (StudContactRel.RelationshipId == model.lukUpMthrId & StdAddrRel.ContactSequence == 1 & objStudntPersnl.StudentPersonalId == saveId)
                                               select new
                                               {
                                                   AdresId = Addr.AddressId
                                               }).ToList();
                            if (mothrAdress.Count > 0)
                            {
                                foreach (var item in mothrAdress)
                                {
                                    mothrAdressId = item.AdresId;
                                }
                            }

                            int mothrConPersId = 0;

                            var mothrConPers = (from objStudntPersnl in DbEntity.StudentPersonals
                                                join StdAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals StdAddrRel.StudentPersonalId
                                                join ConPrsonal in DbEntity.ContactPersonals on StdAddrRel.ContactPersonalId equals ConPrsonal.ContactPersonalId
                                                join Addr in DbEntity.AddressLists on StdAddrRel.AddressId equals Addr.AddressId
                                                join StudContactRel in DbEntity.StudentContactRelationships on ConPrsonal.ContactPersonalId equals StudContactRel.ContactPersonalId
                                                where (StudContactRel.RelationshipId == model.lukUpMthrId & StdAddrRel.ContactSequence == 1 & objStudntPersnl.StudentPersonalId == saveId)
                                                select new
                                                {
                                                    ContactId = ConPrsonal.ContactPersonalId
                                                }).ToList();
                            if (mothrConPers.Count > 0)
                            {
                                foreach (var item in mothrConPers)
                                {
                                    mothrConPersId = item.ContactId;
                                }
                            }

                            int FthrAdressId = 0;


                            var FthrAdress = (from objStudntPersnl in DbEntity.StudentPersonals
                                              join StdAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals StdAddrRel.StudentPersonalId
                                              join ConPrsonal in DbEntity.ContactPersonals on StdAddrRel.ContactPersonalId equals ConPrsonal.ContactPersonalId
                                              join Addr in DbEntity.AddressLists on StdAddrRel.AddressId equals Addr.AddressId
                                              join StudContactRel in DbEntity.StudentContactRelationships on ConPrsonal.ContactPersonalId equals StudContactRel.ContactPersonalId
                                              where (StudContactRel.RelationshipId == model.lukUpFathrId & StdAddrRel.ContactSequence == 1 & objStudntPersnl.StudentPersonalId == saveId)
                                              select new
                                              {
                                                  AdresId = Addr.AddressId
                                              }).ToList();

                            if (FthrAdress.Count > 0)
                            {
                                foreach (var item in FthrAdress)
                                {
                                    FthrAdressId = item.AdresId;
                                }
                            }

                            int FthrConPersId = 0;

                            var FthrConPers = (from objStudntPersnl in DbEntity.StudentPersonals
                                               join StdAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals StdAddrRel.StudentPersonalId
                                               join ConPrsonal in DbEntity.ContactPersonals on StdAddrRel.ContactPersonalId equals ConPrsonal.ContactPersonalId
                                               join Addr in DbEntity.AddressLists on StdAddrRel.AddressId equals Addr.AddressId
                                               join StudContactRel in DbEntity.StudentContactRelationships on ConPrsonal.ContactPersonalId equals StudContactRel.ContactPersonalId
                                               where (StudContactRel.RelationshipId == model.lukUpFathrId & StdAddrRel.ContactSequence == 1 & objStudntPersnl.StudentPersonalId == saveId)
                                               select new
                                               {
                                                   ContactId = ConPrsonal.ContactPersonalId
                                               }).ToList();
                            if (FthrConPers.Count > 0)
                            {
                                foreach (var item in FthrConPers)
                                {
                                    FthrConPersId = item.ContactId;
                                }
                            }


                            int closeRelAdressId = 0;
                            var closeRelAdress = (from objStudntPersnl in DbEntity.StudentPersonals
                                                  join StdAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals StdAddrRel.StudentPersonalId
                                                  join ConPrsonal in DbEntity.ContactPersonals on StdAddrRel.ContactPersonalId equals ConPrsonal.ContactPersonalId
                                                  join Addr in DbEntity.AddressLists on StdAddrRel.AddressId equals Addr.AddressId
                                                  join StudContactRel in DbEntity.StudentContactRelationships on ConPrsonal.ContactPersonalId equals StudContactRel.ContactPersonalId
                                                  join Look in DbEntity.LookUps on StudContactRel.RelationshipId equals Look.LookupId
                                                  where (Look.LookupType == "Relationship" & StdAddrRel.ContactSequence == 1 & objStudntPersnl.StudentPersonalId == saveId & ConPrsonal.Relation == -1)
                                                  select new
                                                  {
                                                      AddressIds = Addr.AddressId
                                                  }).ToList();

                            if (closeRelAdress.Count > 0)
                            {
                                foreach (var item in closeRelAdress)
                                {
                                    closeRelAdressId = item.AddressIds;
                                }
                            }

                            int closeCntPersId = 0;
                            var closeCntPers = (from objStudntPersnl in DbEntity.StudentPersonals
                                                join StdAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals StdAddrRel.StudentPersonalId
                                                join ConPrsonal in DbEntity.ContactPersonals on StdAddrRel.ContactPersonalId equals ConPrsonal.ContactPersonalId
                                                join Addr in DbEntity.AddressLists on StdAddrRel.AddressId equals Addr.AddressId
                                                join StudContactRel in DbEntity.StudentContactRelationships on ConPrsonal.ContactPersonalId equals StudContactRel.ContactPersonalId
                                                join Look in DbEntity.LookUps on StudContactRel.RelationshipId equals Look.LookupId
                                                where (Look.LookupType == "Relationship" & StdAddrRel.ContactSequence == 1 & objStudntPersnl.StudentPersonalId == saveId & ConPrsonal.Relation ==-1)
                                                select new
                                                {
                                                    ContactId = ConPrsonal.ContactPersonalId
                                                }).ToList();

                            if (closeCntPers.Count > 0)
                            {
                                foreach (var item in closeCntPers)
                                {
                                    closeCntPersId = item.ContactId;
                                }
                            }

                            int legalGuardianAdressId = 0;

                            var legalGuardianAdress = (from objStudntPersnl in DbEntity.StudentPersonals
                                                       join StdAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals StdAddrRel.StudentPersonalId
                                                       join ConPrsonal in DbEntity.ContactPersonals on StdAddrRel.ContactPersonalId equals ConPrsonal.ContactPersonalId
                                                       join Addr in DbEntity.AddressLists on StdAddrRel.AddressId equals Addr.AddressId
                                                       join StudContactRel in DbEntity.StudentContactRelationships on ConPrsonal.ContactPersonalId equals StudContactRel.ContactPersonalId
                                                       where (StudContactRel.RelationshipId == model.lukupLegalGurdianId & StdAddrRel.ContactSequence == 1 & objStudntPersnl.StudentPersonalId == saveId)
                                                       select new
                                                       {
                                                           AdresId = Addr.AddressId
                                                       }).ToList();

                            if (legalGuardianAdress.Count > 0)
                            {
                                foreach (var item in legalGuardianAdress)
                                {
                                    legalGuardianAdressId = item.AdresId;
                                }
                            }

                            int LegalContactPersnlId = 0;
                            var LegalContactPersnl = (from objStudntPersnl in DbEntity.StudentPersonals
                                                      join StdAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals StdAddrRel.StudentPersonalId
                                                      join ConPrsonal in DbEntity.ContactPersonals on StdAddrRel.ContactPersonalId equals ConPrsonal.ContactPersonalId
                                                      join Addr in DbEntity.AddressLists on StdAddrRel.AddressId equals Addr.AddressId
                                                      join StudContactRel in DbEntity.StudentContactRelationships on ConPrsonal.ContactPersonalId equals StudContactRel.ContactPersonalId
                                                      where (StudContactRel.RelationshipId == model.lukupLegalGurdianId & StdAddrRel.ContactSequence == 1 & objStudntPersnl.StudentPersonalId == saveId)
                                                      select new
                                                      {
                                                          ContactId = ConPrsonal.ContactPersonalId
                                                      }).ToList();
                            if (LegalContactPersnl.Count > 0)
                            {
                                foreach (var item in LegalContactPersnl)
                                {
                                    LegalContactPersnlId = item.ContactId;
                                }
                            }
                            int emergncyAdrsId = 0;

                            var emergncyAdrs = (from objStudntPersnl in DbEntity.StudentPersonals
                                                join StdAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals StdAddrRel.StudentPersonalId
                                                join ConPrsonal in DbEntity.ContactPersonals on StdAddrRel.ContactPersonalId equals ConPrsonal.ContactPersonalId
                                                join Addr in DbEntity.AddressLists on StdAddrRel.AddressId equals Addr.AddressId
                                                join StudContactRel in DbEntity.StudentContactRelationships on ConPrsonal.ContactPersonalId equals StudContactRel.ContactPersonalId
                                                where (StudContactRel.RelationshipId == model.lukupEmergencyId & StdAddrRel.ContactSequence == 1 & objStudntPersnl.StudentPersonalId == saveId)
                                                select new
                                                {
                                                    AdresId = Addr.AddressId
                                                }).ToList();

                            if (emergncyAdrs.Count > 0)
                            {
                                foreach (var item in emergncyAdrs)
                                {
                                    emergncyAdrsId = item.AdresId;
                                }
                            }

                            int emergncyContactId = 0;
                            var emergncyContact = (from objStudntPersnl in DbEntity.StudentPersonals
                                                   join StdAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals StdAddrRel.StudentPersonalId
                                                   join ConPrsonal in DbEntity.ContactPersonals on StdAddrRel.ContactPersonalId equals ConPrsonal.ContactPersonalId
                                                   join Addr in DbEntity.AddressLists on StdAddrRel.AddressId equals Addr.AddressId
                                                   join StudContactRel in DbEntity.StudentContactRelationships on ConPrsonal.ContactPersonalId equals StudContactRel.ContactPersonalId
                                                   where (StudContactRel.RelationshipId == model.lukupEmergencyId & StdAddrRel.ContactSequence == 1 & objStudntPersnl.StudentPersonalId == saveId)
                                                   select new
                                                   {
                                                       ContactId = ConPrsonal.ContactPersonalId
                                                   }).ToList();


                            if (emergncyContact.Count > 0)
                            {
                                foreach (var item in emergncyContact)
                                {
                                    emergncyContactId = item.ContactId;
                                }
                            }

                            StudentTbl = DbEntity.StudentPersonals.Where(objStudent => objStudent.StudentPersonalId == saveId).SingleOrDefault();
                            StudentTbl.FirstName = model.RefFrstName;
                            StudentTbl.LastName = model.RefLstName;
                            StudentTbl.MiddleName = model.RefMaidenName;
                            StudentTbl.PrimaryDiag = model.PrimaryDiag;
                            StudentTbl.SecondaryDiag = model.SecondaryDiag;
                            StudentTbl.SocialSecurityNo = model.SocialSecurityNo;
                            StudentTbl.SSINo = model.SsiNo;
                            StudentTbl.Height = model.RefPresentHght;
                            StudentTbl.Weight = model.RefPresntWeigth;
                            StudentTbl.HairColor = model.RefHairColor;
                            StudentTbl.EyeColor = model.RefEyecolor;
                            StudentTbl.LocalId = "STD " + saveId;
                            StudentTbl.Gender = model.RefGender;
                            StudentTbl.AdmissionDate = (model.RefDate == null) ? (DateTime?)null : DateTime.ParseExact(model.RefDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            StudentTbl.BirthDate = (model.RefDOB == null) ? (DateTime?)null : DateTime.ParseExact(model.RefDOB, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            StudentTbl.ModifiedBy = loginId;
                            StudentTbl.CreatedOn = DateTime.Now;
                            DbEntity.SaveChanges();


                            AddrList = DbEntity.AddressLists.Where(objAdr => objAdr.AddressId == RefaddrId).SingleOrDefault();
                            AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.RefCountry;
                            AddrList.StateProvince = model.RefState;
                            AddrList.City = model.RefCity;
                            AddrList.StreetName = model.RefStreetAdrs;
                            AddrList.ApartmentType = model.RefAptUnit;
                            AddrList.PostalCode = model.RefZipCode;
                            DbEntity.SaveChanges();


                            ConPersonal = DbEntity.ContactPersonals.Where(objCont => objCont.ContactPersonalId == mothrConPersId).SingleOrDefault();
                            if (ConPersonal != null)
                            {
                                ConPersonal.FirstName = model.objRelationMthr.txtFirstName;
                                ConPersonal.MiddleName = model.objRelationMthr.txtMiddleName;
                                ConPersonal.LastName = model.objRelationMthr.txtLstName;
                                if (model.objRelationMthr.txtDob != null)
                                    model.objRelationMthr.txtDob = model.objRelationMthr.txtDob.Replace('-', '/');
                                ConPersonal.BirthDate = (model.objRelationMthr.txtDob == null) ? (DateTime?)null : DateTime.ParseExact(model.objRelationMthr.txtDob, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                ConPersonal.CountryOfCitizenship = model.objRelationMthr.txtUScitzen;
                                ConPersonal.Employer = model.objRelationMthr.txtEmployer;
                                ConPersonal.Occupation = model.objRelationMthr.txtOccupation;
                                ConPersonal.MaritalStatus = model.objRelationMthr.txtMaritalStatus;

                                DbEntity.SaveChanges();


                                AddrList = DbEntity.AddressLists.Where(objAdr => objAdr.AddressId == mothrAdressId).SingleOrDefault();
                                AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objRelationMthr.txtCountry;
                                AddrList.StateProvince = model.objRelationMthr.txtState;
                                AddrList.City = model.objRelationMthr.txtCity;
                                AddrList.StreetName = model.objRelationMthr.txtStreetAdress;
                                AddrList.ApartmentType = model.objRelationMthr.txtAprtmentUnit;
                                AddrList.PostalCode = model.objRelationMthr.txtZipCode;
                                AddrList.Phone = model.objRelationMthr.txtHomePhone;
                                AddrList.OtherPhone = model.objRelationMthr.txtWorkPhone;
                                AddrList.PrimaryEmail = model.objRelationMthr.txtEmail;

                                DbEntity.SaveChanges();

                            }
                            else
                            {
                                if ((model.objRelationMthr.txtFirstName != null && model.objRelationMthr.txtFirstName != "") || (model.objRelationMthr.txtLstName != null && model.objRelationMthr.txtLstName != ""))
                                {
                                    AddrList = new AddressList();
                                    StdAdrRel = new StudentAddresRel();
                                    ConPersonal = new ContactPersonal();
                                    StdCntctRel = new StudentContactRelationship();
                                    int addressType = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Address Type" && objLookup.LookupName == "Physical location address").Select(objlookup => objlookup.LookupId).Single();

                                    ConPersonal.StudentPersonalId = StudentTbl.StudentPersonalId;
                                    ConPersonal.FirstName = model.objRelationMthr.txtFirstName;
                                    ConPersonal.LastName = model.objRelationMthr.txtLstName;
                                    ConPersonal.MiddleName = model.objRelationMthr.txtMiddleName;
                                    ConPersonal.ContactFlag = "Referral";
                                    ConPersonal.Status = 1;
                                    ConPersonal.CreatedBy = loginId;
                                    ConPersonal.CreatedOn = DateTime.Now;
                                    
                                    ConPersonal.BirthDate = (model.objRelationMthr.txtDob == null) ? (DateTime?)null : DateTime.ParseExact(model.objRelationMthr.txtDob, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                    ConPersonal.CountryOfCitizenship = model.objRelationMthr.txtUScitzen;
                                    ConPersonal.Employer = model.objRelationMthr.txtEmployer;
                                    ConPersonal.Occupation = model.objRelationMthr.txtOccupation;
                                    ConPersonal.MaritalStatus = model.objRelationMthr.txtMaritalStatus;

                                    DbEntity.ContactPersonals.Add(ConPersonal);
                                    DbEntity.SaveChanges();
                                    model.ContactId = ConPersonal.ContactPersonalId;

                                    AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single();// model.objRelationMthr.txtCountry;
                                    AddrList.StateProvince = model.objRelationMthr.txtState;
                                    AddrList.AddressType = addressType;
                                    AddrList.City = model.objRelationMthr.txtCity;
                                    AddrList.StreetName = model.objRelationMthr.txtStreetAdress;
                                    AddrList.ApartmentType = model.objRelationMthr.txtAprtmentUnit;
                                    AddrList.PostalCode = model.objRelationMthr.txtZipCode;
                                    AddrList.Phone = model.objRelationMthr.txtHomePhone;
                                    AddrList.OtherPhone = model.objRelationMthr.txtWorkPhone;
                                    AddrList.PrimaryEmail = model.objRelationMthr.txtEmail;
                                    AddrList.CreatedBy = loginId;
                                    AddrList.CreatedOn = DateTime.Now;
                                    DbEntity.AddressLists.Add(AddrList);
                                    DbEntity.SaveChanges();
                                    model.AddressId = AddrList.AddressId;

                                    StdCntctRel.ContactPersonalId = model.ContactId;
                                    StdCntctRel.RelationshipId = model.lukUpMthrId;
                                    StdCntctRel.CreatedBy = loginId;
                                    StdCntctRel.CreatedOn = DateTime.Now;
                                    DbEntity.StudentContactRelationships.Add(StdCntctRel);
                                    DbEntity.SaveChanges();

                                    StdAdrRel.AddressId = AddrList.AddressId;
                                    StdAdrRel.StudentPersonalId = StudentTbl.StudentPersonalId;
                                    StdAdrRel.ContactPersonalId = ConPersonal.ContactPersonalId;
                                    StdAdrRel.ContactSequence = 1;
                                    StdAdrRel.CreatedBy = loginId;
                                    StdAdrRel.CreatedOn = DateTime.Now;
                                    DbEntity.StudentAddresRels.Add(StdAdrRel);
                                    DbEntity.SaveChanges();
                                }
                            }
                            ConPersonal = DbEntity.ContactPersonals.Where(objCont => objCont.ContactPersonalId == FthrConPersId).SingleOrDefault();
                            if (ConPersonal != null)
                            {
                                ConPersonal.FirstName = model.objRelationFthr.txtFirstName;
                                ConPersonal.MiddleName = model.objRelationFthr.txtMiddleName;
                                ConPersonal.LastName = model.objRelationFthr.txtLstName;
                                
                                ConPersonal.BirthDate = (model.objRelationFthr.txtDob == null) ? (DateTime?)null : DateTime.ParseExact(model.objRelationFthr.txtDob, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                ConPersonal.CountryOfCitizenship = model.objRelationFthr.txtUScitzen;
                                ConPersonal.Employer = model.objRelationFthr.txtEmployer;
                                ConPersonal.Occupation = model.objRelationFthr.txtOccupation;
                                ConPersonal.MaritalStatus = model.objRelationFthr.txtMaritalStatus;

                                DbEntity.SaveChanges();

                                AddrList = DbEntity.AddressLists.Where(objAdr => objAdr.AddressId == FthrAdressId).SingleOrDefault();
                                AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objRelationFthr.txtCountry;
                                AddrList.StateProvince = model.objRelationFthr.txtState;
                                AddrList.City = model.objRelationFthr.txtCity;
                                AddrList.StreetName = model.objRelationFthr.txtStreetAdress;
                                AddrList.ApartmentType = model.objRelationFthr.txtAprtmentUnit;
                                AddrList.PostalCode = model.objRelationFthr.txtZipCode;
                                AddrList.Phone = model.objRelationFthr.txtHomePhone;
                                AddrList.OtherPhone = model.objRelationFthr.txtWorkPhone;
                                AddrList.PrimaryEmail = model.objRelationFthr.txtEmail;


                                DbEntity.SaveChanges();

                            }
                            else
                            {

                                if ((model.objRelationFthr.txtFirstName != null && model.objRelationFthr.txtFirstName != "") || (model.objRelationFthr.txtLstName != null && model.objRelationFthr.txtLstName != ""))
                                {
                                    AddrList = new AddressList();
                                    StdAdrRel = new StudentAddresRel();
                                    ConPersonal = new ContactPersonal();
                                    StdCntctRel = new StudentContactRelationship();
                                    int addressType = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Address Type" && objLookup.LookupName == "Physical location address").Select(objlookup => objlookup.LookupId).Single();

                                    ConPersonal.StudentPersonalId = StudentTbl.StudentPersonalId;
                                    ConPersonal.FirstName = model.objRelationFthr.txtFirstName;
                                    ConPersonal.LastName = model.objRelationFthr.txtLstName;
                                    ConPersonal.MiddleName = model.objRelationFthr.txtMiddleName;
                                    ConPersonal.ContactFlag = "Referral";
                                    ConPersonal.Status = 1;
                                    ConPersonal.CreatedBy = loginId;
                                    ConPersonal.CreatedOn = DateTime.Now;
                                    
                                    ConPersonal.BirthDate = (model.objRelationFthr.txtDob == null) ? (DateTime?)null : DateTime.ParseExact(model.objRelationFthr.txtDob, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                    ConPersonal.CountryOfCitizenship = model.objRelationFthr.txtUScitzen;
                                    ConPersonal.Employer = model.objRelationFthr.txtEmployer;
                                    ConPersonal.Occupation = model.objRelationFthr.txtOccupation;
                                    ConPersonal.MaritalStatus = model.objRelationFthr.txtMaritalStatus;

                                    DbEntity.ContactPersonals.Add(ConPersonal);
                                    DbEntity.SaveChanges();
                                    model.ContactId = ConPersonal.ContactPersonalId;

                                    AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objRelationFthr.txtCountry;
                                    AddrList.StateProvince = model.objRelationFthr.txtState;
                                    AddrList.City = model.objRelationFthr.txtCity;
                                    AddrList.AddressType = addressType;
                                    AddrList.StreetName = model.objRelationFthr.txtStreetAdress;
                                    AddrList.ApartmentType = model.objRelationFthr.txtAprtmentUnit;
                                    AddrList.PostalCode = model.objRelationFthr.txtZipCode;
                                    AddrList.Phone = model.objRelationFthr.txtHomePhone;
                                    AddrList.OtherPhone = model.objRelationFthr.txtWorkPhone;
                                    AddrList.PrimaryEmail = model.objRelationFthr.txtEmail;
                                    AddrList.CreatedBy = loginId;
                                    AddrList.CreatedOn = DateTime.Now;
                                    DbEntity.AddressLists.Add(AddrList);
                                    DbEntity.SaveChanges();
                                    model.AddressId = AddrList.AddressId;

                                    StdCntctRel.ContactPersonalId = model.ContactId;
                                    StdCntctRel.RelationshipId = model.lukUpFathrId;
                                    StdCntctRel.CreatedBy = loginId;
                                    StdCntctRel.CreatedOn = DateTime.Now;
                                    DbEntity.StudentContactRelationships.Add(StdCntctRel);
                                    DbEntity.SaveChanges();

                                    StdAdrRel.AddressId = AddrList.AddressId;
                                    StdAdrRel.StudentPersonalId = StudentTbl.StudentPersonalId;
                                    StdAdrRel.ContactPersonalId = ConPersonal.ContactPersonalId;
                                    StdAdrRel.ContactSequence = 1;
                                    StdAdrRel.CreatedBy = loginId;
                                    StdAdrRel.CreatedOn = DateTime.Now;
                                    DbEntity.StudentAddresRels.Add(StdAdrRel);
                                    DbEntity.SaveChanges();
                                }
                            }

                            ConPersonal = DbEntity.ContactPersonals.Where(objCont => objCont.ContactPersonalId == closeCntPersId).SingleOrDefault();
                            if (ConPersonal != null)
                            {
                                ConPersonal.FirstName = model.objRelationClose.txtFirstName;
                                ConPersonal.MiddleName = model.objRelationClose.txtMiddleName;
                                ConPersonal.LastName = model.objRelationClose.txtLstName;
                                DbEntity.SaveChanges();
                                model.objRelationClose.RelationName = "";



                                StdCntctRel = DbEntity.StudentContactRelationships.Where(objRel => objRel.ContactPersonalId == closeCntPersId).SingleOrDefault();
                                StdCntctRel.RelationshipId = model.objRelationClose.closeRelation;
                                StdCntctRel.ModifiedBy = loginId;
                                StdCntctRel.ModifiedOn = DateTime.Now;
                                DbEntity.SaveChanges();


                                AddrList = DbEntity.AddressLists.Where(objAdr => objAdr.AddressId == closeRelAdressId).SingleOrDefault();
                                AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objRelationClose.txtCountry;
                                AddrList.StateProvince = model.objRelationClose.txtState;
                                AddrList.City = model.objRelationClose.txtCity;
                                AddrList.StreetName = model.objRelationClose.txtStreetAdress;
                                AddrList.ApartmentType = model.objRelationClose.txtAprtmentUnit;
                                AddrList.PostalCode = model.objRelationClose.txtZipCode;
                                AddrList.Phone = model.objRelationClose.txtHomePhone;
                                AddrList.OtherPhone = model.objRelationClose.txtWorkPhone;
                                AddrList.PrimaryEmail = model.objRelationClose.txtEmail;
                                DbEntity.SaveChanges();
                            }
                            else
                            {

                                if ((model.objRelationClose.txtFirstName != null && model.objRelationClose.txtFirstName != "") || (model.objRelationClose.txtLstName != null && model.objRelationClose.txtLstName != ""))
                                {
                                    AddrList = new AddressList();
                                    StdAdrRel = new StudentAddresRel();
                                    ConPersonal = new ContactPersonal();
                                    StdCntctRel = new StudentContactRelationship();
                                    int addressType = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Address Type" && objLookup.LookupName == "Physical location address").Select(objlookup => objlookup.LookupId).Single();


                                    model.objRelationClose.RelationName = (-1).ToString();
                                    ConPersonal.StudentPersonalId = StudentTbl.StudentPersonalId;
                                    ConPersonal.ContactFlag = "Referral";
                                    ConPersonal.Status = 1;
                                    ConPersonal.FirstName = model.objRelationClose.txtFirstName;
                                    ConPersonal.LastName = model.objRelationClose.txtLstName;
                                    ConPersonal.MiddleName = model.objRelationClose.txtMiddleName;
                                    ConPersonal.Relation = Convert.ToInt32(model.objRelationClose.RelationName);
                                    ConPersonal.CreatedBy = loginId;
                                    ConPersonal.CreatedOn = DateTime.Now;
                                    DbEntity.ContactPersonals.Add(ConPersonal);
                                    DbEntity.SaveChanges();
                                    model.ContactId = ConPersonal.ContactPersonalId;


                                    AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objRelationClose.txtCountry;
                                    AddrList.StateProvince = model.objRelationClose.txtState;
                                    AddrList.City = model.objRelationClose.txtCity;
                                    AddrList.AddressType = addressType;
                                    AddrList.StreetName = model.objRelationClose.txtStreetAdress;
                                    AddrList.ApartmentType = model.objRelationClose.txtAprtmentUnit;
                                    AddrList.PostalCode = model.objRelationClose.txtZipCode;
                                    AddrList.Phone = model.objRelationClose.txtHomePhone;
                                    AddrList.OtherPhone = model.objRelationClose.txtWorkPhone;
                                    AddrList.PrimaryEmail = model.objRelationClose.txtEmail;
                                    AddrList.CreatedBy = loginId;
                                    AddrList.CreatedOn = DateTime.Now;
                                    DbEntity.AddressLists.Add(AddrList);
                                    DbEntity.SaveChanges();
                                    model.AddressId = AddrList.AddressId;

                                    StdCntctRel.ContactPersonalId = model.ContactId;
                                    if (model.objRelationClose.closeRelation == 0)
                                    {
                                        model.objRelationClose.closeRelation = model.lukupCloseId;
                                    }
                                    StdCntctRel.RelationshipId = model.objRelationClose.closeRelation;
                                    StdCntctRel.CreatedBy = loginId;
                                    StdCntctRel.CreatedOn = DateTime.Now;
                                    DbEntity.StudentContactRelationships.Add(StdCntctRel);
                                    DbEntity.SaveChanges();


                                    StdAdrRel.AddressId = AddrList.AddressId;
                                    StdAdrRel.StudentPersonalId = StudentTbl.StudentPersonalId;
                                    StdAdrRel.ContactPersonalId = ConPersonal.ContactPersonalId;
                                    StdAdrRel.ContactSequence = 1;
                                    StdAdrRel.CreatedBy = loginId;
                                    StdAdrRel.CreatedOn = DateTime.Now;
                                    DbEntity.StudentAddresRels.Add(StdAdrRel);
                                    DbEntity.SaveChanges();
                                }

                            }

                            ConPersonal = DbEntity.ContactPersonals.Where(objCont => objCont.ContactPersonalId == LegalContactPersnlId).SingleOrDefault();
                            if (ConPersonal != null)
                            {
                                ConPersonal.FirstName = model.objRelationLegalGuardian.txtFirstName;
                                ConPersonal.MiddleName = model.objRelationLegalGuardian.txtMiddleName;
                                ConPersonal.LastName = model.objRelationLegalGuardian.txtLstName;
                                DbEntity.SaveChanges();

                                AddrList = DbEntity.AddressLists.Where(objAdr => objAdr.AddressId == legalGuardianAdressId).SingleOrDefault();
                                AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objRelationLegalGuardian.txtCountry;
                                AddrList.StateProvince = model.objRelationLegalGuardian.txtState;
                                AddrList.City = model.objRelationLegalGuardian.txtCity;
                                AddrList.StreetName = model.objRelationLegalGuardian.txtStreetAdress;
                                AddrList.ApartmentType = model.objRelationLegalGuardian.txtAprtmentUnit;
                                AddrList.PostalCode = model.objRelationLegalGuardian.txtZipCode;
                                AddrList.Phone = model.objRelationLegalGuardian.txtHomePhone;
                                AddrList.OtherPhone = model.objRelationLegalGuardian.txtWorkPhone;
                                AddrList.PrimaryEmail = model.objRelationLegalGuardian.txtEmail;
                                DbEntity.SaveChanges();
                            }
                            else
                            {
                                if ((model.objRelationLegalGuardian.txtFirstName != null && model.objRelationLegalGuardian.txtFirstName != "") || (model.objRelationLegalGuardian.txtLstName != null && model.objRelationLegalGuardian.txtLstName != ""))
                                {
                                    AddrList = new AddressList();
                                    StdAdrRel = new StudentAddresRel();
                                    ConPersonal = new ContactPersonal();
                                    StdCntctRel = new StudentContactRelationship();
                                    int addressType = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Address Type" && objLookup.LookupName == "Physical location address").Select(objlookup => objlookup.LookupId).Single();

                                    ConPersonal.StudentPersonalId = StudentTbl.StudentPersonalId;
                                    ConPersonal.FirstName = model.objRelationLegalGuardian.txtFirstName;
                                    ConPersonal.LastName = model.objRelationLegalGuardian.txtLstName;
                                    ConPersonal.MiddleName = model.objRelationLegalGuardian.txtMiddleName;
                                    ConPersonal.ContactFlag = "Referral";
                                    ConPersonal.Status = 1;
                                    ConPersonal.CreatedBy = loginId;
                                    ConPersonal.CreatedOn = DateTime.Now;
                                    ConPersonal.Relation = model.objRelationLegalGuardian.LgRelationName;
                                    DbEntity.ContactPersonals.Add(ConPersonal);
                                    DbEntity.SaveChanges();
                                    model.ContactId = ConPersonal.ContactPersonalId;

                                    AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objRelationLegalGuardian.txtCountry;
                                    AddrList.StateProvince = model.objRelationLegalGuardian.txtState;
                                    AddrList.City = model.objRelationLegalGuardian.txtCity;
                                    AddrList.AddressType = addressType;
                                    AddrList.StreetName = model.objRelationLegalGuardian.txtStreetAdress;
                                    AddrList.ApartmentType = model.objRelationLegalGuardian.txtAprtmentUnit;
                                    AddrList.PostalCode = model.objRelationLegalGuardian.txtZipCode;
                                    AddrList.Phone = model.objRelationLegalGuardian.txtHomePhone;
                                    AddrList.OtherPhone = model.objRelationLegalGuardian.txtWorkPhone;
                                    AddrList.PrimaryEmail = model.objRelationLegalGuardian.txtEmail;
                                    AddrList.CreatedBy = loginId;
                                    AddrList.CreatedOn = DateTime.Now;
                                    DbEntity.AddressLists.Add(AddrList);
                                    DbEntity.SaveChanges();
                                    model.AddressId = AddrList.AddressId;

                                    StdCntctRel.ContactPersonalId = model.ContactId;
                                    StdCntctRel.RelationshipId = model.lukupLegalGurdianId;
                                    StdCntctRel.CreatedBy = loginId;
                                    StdCntctRel.CreatedOn = DateTime.Now;
                                    DbEntity.StudentContactRelationships.Add(StdCntctRel);
                                    DbEntity.SaveChanges();

                                    StdAdrRel.AddressId = AddrList.AddressId;
                                    StdAdrRel.StudentPersonalId = StudentTbl.StudentPersonalId;
                                    StdAdrRel.ContactPersonalId = ConPersonal.ContactPersonalId;
                                    StdAdrRel.ContactSequence = 1;
                                    StdAdrRel.CreatedBy = loginId;
                                    StdAdrRel.CreatedOn = DateTime.Now;
                                    DbEntity.StudentAddresRels.Add(StdAdrRel);
                                    DbEntity.SaveChanges();
                                }
                            }
                            ConPersonal = DbEntity.ContactPersonals.Where(objCont => objCont.ContactPersonalId == emergncyContactId).SingleOrDefault();
                            if (ConPersonal != null)
                            {

                                int addressType = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Address Type" && objLookup.LookupName == "Physical location address").Select(objlookup => objlookup.LookupId).Single();

                                ConPersonal.FirstName = model.objRelationEmergncyContact.txtFirstName;
                                ConPersonal.MiddleName = model.objRelationEmergncyContact.txtMiddleName;
                                ConPersonal.LastName = model.objRelationEmergncyContact.txtLstName;
                                ConPersonal.Relation = model.objRelationEmergncyContact.EmRelationName;
                                DbEntity.SaveChanges();

                                AddrList = DbEntity.AddressLists.Where(objAdr => objAdr.AddressId == emergncyAdrsId).SingleOrDefault();
                                AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objRelationEmergncyContact.txtCountry;
                                AddrList.StateProvince = model.objRelationEmergncyContact.txtState;
                                AddrList.City = model.objRelationEmergncyContact.txtCity;
                                AddrList.StreetName = model.objRelationEmergncyContact.txtStreetAdress;
                                AddrList.ApartmentType = model.objRelationEmergncyContact.txtAprtmentUnit;
                                AddrList.PostalCode = model.objRelationEmergncyContact.txtZipCode;
                                AddrList.Phone = model.objRelationEmergncyContact.txtHomePhone;
                                AddrList.OtherPhone = model.objRelationEmergncyContact.txtWorkPhone;
                                AddrList.PrimaryEmail = model.objRelationEmergncyContact.txtEmail;
                                DbEntity.SaveChanges();
                            }

                            else
                            {
                                if ((model.objRelationEmergncyContact.txtFirstName != null && model.objRelationEmergncyContact.txtFirstName != "") || (model.objRelationEmergncyContact.txtLstName != null && model.objRelationEmergncyContact.txtLstName != ""))
                                {
                                    AddrList = new AddressList();
                                    StdAdrRel = new StudentAddresRel();
                                    ConPersonal = new ContactPersonal();
                                    StdCntctRel = new StudentContactRelationship();
                                    int addressType = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Address Type" && objLookup.LookupName == "Physical location address").Select(objlookup => objlookup.LookupId).Single();

                                    ConPersonal.StudentPersonalId = StudentTbl.StudentPersonalId;
                                    ConPersonal.ContactFlag = "Referral";
                                    ConPersonal.Status = 1;
                                    ConPersonal.FirstName = model.objRelationEmergncyContact.txtFirstName;
                                    ConPersonal.LastName = model.objRelationEmergncyContact.txtLstName;
                                    ConPersonal.MiddleName = model.objRelationEmergncyContact.txtMiddleName;
                                    ConPersonal.Relation = model.objRelationEmergncyContact.EmRelationName;
                                    ConPersonal.CreatedBy = loginId;
                                    ConPersonal.CreatedOn = DateTime.Now;
                                    DbEntity.ContactPersonals.Add(ConPersonal);
                                    DbEntity.SaveChanges();
                                    model.ContactId = ConPersonal.ContactPersonalId;

                                    AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objRelationEmergncyContact.txtCountry;
                                    AddrList.StateProvince = model.objRelationEmergncyContact.txtState;
                                    AddrList.City = model.objRelationEmergncyContact.txtCity;
                                    AddrList.AddressType = addressType;
                                    AddrList.StreetName = model.objRelationEmergncyContact.txtStreetAdress;
                                    AddrList.ApartmentType = model.objRelationEmergncyContact.txtAprtmentUnit;
                                    AddrList.PostalCode = model.objRelationEmergncyContact.txtZipCode;
                                    AddrList.Phone = model.objRelationEmergncyContact.txtHomePhone;
                                    AddrList.OtherPhone = model.objRelationEmergncyContact.txtWorkPhone;
                                    AddrList.PrimaryEmail = model.objRelationEmergncyContact.txtEmail;
                                    AddrList.CreatedBy = loginId;
                                    AddrList.CreatedOn = DateTime.Now;
                                    DbEntity.AddressLists.Add(AddrList);
                                    DbEntity.SaveChanges();
                                    model.AddressId = AddrList.AddressId;

                                    StdCntctRel.ContactPersonalId = model.ContactId;
                                    StdCntctRel.RelationshipId = model.lukupEmergencyId;
                                    StdCntctRel.CreatedBy = loginId;
                                    StdCntctRel.CreatedOn = DateTime.Now;
                                    DbEntity.StudentContactRelationships.Add(StdCntctRel);
                                    DbEntity.SaveChanges();

                                    StdAdrRel.AddressId = AddrList.AddressId;
                                    StdAdrRel.StudentPersonalId = StudentTbl.StudentPersonalId;
                                    StdAdrRel.ContactPersonalId = ConPersonal.ContactPersonalId;
                                    StdAdrRel.ContactSequence = 1;
                                    StdAdrRel.CreatedBy = loginId;
                                    StdAdrRel.CreatedOn = DateTime.Now;
                                    DbEntity.StudentAddresRels.Add(StdAdrRel);
                                    DbEntity.SaveChanges();
                                }


                            }
                        }


                        else if (type == "General Family Background Information")
                        {

                            try
                            {


                                int mothrConPersId = (from objStudntPersnl in DbEntity.StudentPersonals
                                                      join StdAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals StdAddrRel.StudentPersonalId
                                                      join ConPrsonal in DbEntity.ContactPersonals on StdAddrRel.ContactPersonalId equals ConPrsonal.ContactPersonalId
                                                      join Addr in DbEntity.AddressLists on StdAddrRel.AddressId equals Addr.AddressId
                                                      join StudContactRel in DbEntity.StudentContactRelationships on ConPrsonal.ContactPersonalId equals StudContactRel.ContactPersonalId
                                                      where (StudContactRel.RelationshipId == model.lukUpMthrId & StdAddrRel.ContactSequence == 1 & objStudntPersnl.StudentPersonalId == saveId
                                                      && ConPrsonal.ContactPersonalId == StdAddrRel.ContactPersonalId)
                                                      select new
                                                      {
                                                          ContactId = ConPrsonal.ContactPersonalId
                                                      }).SingleOrDefault().ContactId;



                                int FthrConPersId = (from objStudntPersnl in DbEntity.StudentPersonals
                                                     join StdAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals StdAddrRel.StudentPersonalId
                                                     join ConPrsonal in DbEntity.ContactPersonals on StdAddrRel.ContactPersonalId equals ConPrsonal.ContactPersonalId
                                                     join Addr in DbEntity.AddressLists on StdAddrRel.AddressId equals Addr.AddressId
                                                     join StudContactRel in DbEntity.StudentContactRelationships on ConPrsonal.ContactPersonalId equals StudContactRel.ContactPersonalId
                                                     where (StudContactRel.RelationshipId == model.lukUpFathrId & StdAddrRel.ContactSequence == 1 & objStudntPersnl.StudentPersonalId == saveId
                                                     && ConPrsonal.ContactPersonalId == StdAddrRel.ContactPersonalId)
                                                     select new
                                                     {
                                                         ContactId = ConPrsonal.ContactPersonalId
                                                     }).SingleOrDefault().ContactId;
                                //ConPersonal = DbEntity.ContactPersonals.Where(objCont => objCont.ContactPersonalId == mothrConPersId).SingleOrDefault();
                                //if (ConPersonal != null)
                                //{


                                    
                                //    DbEntity.SaveChanges();
                                //}
                                //else
                                //{
                                //    if ((model.objRelationMthr.txtFirstName != null && model.objRelationMthr.txtFirstName != "") || (model.objRelationMthr.txtLstName != null && model.objRelationMthr.txtLstName != ""))
                                //    {
                                //        ConPersonal = DbEntity.ContactPersonals.Where(objContPers => objContPers.ContactPersonalId == mothrConPersId).SingleOrDefault();
                                //        ConPersonal.BirthDate = (model.objRelationMthr.txtDob == null) ? (DateTime?)null : DateTime.ParseExact(model.objRelationMthr.txtDob, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                //        ConPersonal.CountryOfCitizenship = model.objRelationMthr.txtUScitzen;
                                //        ConPersonal.Employer = model.objRelationMthr.txtEmployer;
                                //        ConPersonal.Occupation = model.objRelationMthr.txtOccupation;
                                //        ConPersonal.MaritalStatus = model.objRelationMthr.txtMaritalStatus;
                                //        DbEntity.ContactPersonals.Add(ConPersonal);
                                //        DbEntity.SaveChanges();
                                //    }
                                //}
                                //ConPersonal = DbEntity.ContactPersonals.Where(objCont => objCont.ContactPersonalId == FthrConPersId).SingleOrDefault();
                                //if (ConPersonal != null)
                                //{

                                    
                                //    DbEntity.SaveChanges();
                                //}
                                //else
                                //{

                                //    if ((model.objRelationFthr.txtFirstName != null && model.objRelationFthr.txtFirstName != "") || (model.objRelationFthr.txtLstName != null && model.objRelationFthr.txtLstName != ""))
                                //    {
                                //        ConPersonal = DbEntity.ContactPersonals.Where(objContPers => objContPers.ContactPersonalId == FthrConPersId).SingleOrDefault();
                                //        ConPersonal.BirthDate = (model.objRelationFthr.txtDob == null) ? (DateTime?)null : DateTime.ParseExact(model.objRelationFthr.txtDob, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                //        ConPersonal.CountryOfCitizenship = model.objRelationFthr.txtUScitzen;
                                //        ConPersonal.Employer = model.objRelationFthr.txtEmployer;
                                //        ConPersonal.Occupation = model.objRelationFthr.txtOccupation;
                                //        ConPersonal.MaritalStatus = model.objRelationFthr.txtMaritalStatus;
                                //        DbEntity.ContactPersonals.Add(ConPersonal);
                                //        DbEntity.SaveChanges();
                                //    }

                                //}


                            }

                            catch (Exception ex)
                            {
                                ClsErrorLog erorLog = new ClsErrorLog();
                                erorLog.WriteToLog(ex.ToString());
                            }

                        }

                        else if (type == "Birth And Development History")
                        {
                            try
                            {
                                int phyAdressID = 0;
                                int insAdressId = 0;
                                int insSecAdressId = 0;
                                int insDentalAdressId = 0;

                                var phyAdressIDs = (from mediclInsur in DbEntity.MedicalAndInsurances
                                                    join Addr in DbEntity.AddressLists on mediclInsur.AddressId equals Addr.AddressId
                                                    where (mediclInsur.StudentPersonalId == saveId)
                                                    select new
                                                    {
                                                        AdressId = Addr.AddressId

                                                    }).SingleOrDefault();
                                if (phyAdressIDs != null)
                                {
                                    phyAdressID = phyAdressIDs.AdressId;
                                }

                                var insAdressIds = (from Insrnce in DbEntity.Insurances
                                                    join Addr in DbEntity.AddressLists on Insrnce.AddressId equals Addr.AddressId
                                                    where (Insrnce.StudentPersonalId == saveId & Insrnce.PreferType == "Primary")
                                                    select new
                                                    {
                                                        AdresId = Addr.AddressId

                                                    }).SingleOrDefault();

                                if (insAdressIds != null)
                                {
                                    insAdressId = insAdressIds.AdresId;
                                }


                                var insSecAdressIds = (from Insrnce in DbEntity.Insurances
                                                       join Addr in DbEntity.AddressLists on Insrnce.AddressId equals Addr.AddressId
                                                       where (Insrnce.StudentPersonalId == saveId & Insrnce.PreferType == "Secondary")
                                                       select new
                                                       {
                                                           AdresId = Addr.AddressId

                                                       }).SingleOrDefault();
                                if (insSecAdressIds != null)
                                {
                                    insSecAdressId = insSecAdressIds.AdresId;
                                }

                                var insDentalAdressIds = (from Insrnce in DbEntity.Insurances
                                                          join Addr in DbEntity.AddressLists on Insrnce.AddressId equals Addr.AddressId
                                                          where (Insrnce.StudentPersonalId == saveId & Insrnce.PreferType == "Dental")
                                                          select new
                                                          {
                                                              AdresId = Addr.AddressId

                                                          }).SingleOrDefault();
                                if (insDentalAdressIds != null)
                                {
                                    insDentalAdressId = insDentalAdressIds.AdresId;
                                }

                                StudentTbl = DbEntity.StudentPersonals.Where(objStudent => objStudent.StudentPersonalId == saveId).SingleOrDefault();
                                if (StudentTbl != null)
                                {
                                    StudentTbl.PlaceOfBirth = model.RefbirthPlace;
                                    StudentTbl.StateOfBirth = model.RefStateBirth;
                                    StudentTbl.CountryOfBirth = model.RefCntryBirth;
                                    DbEntity.SaveChanges();
                                }

                                Diagnose = DbEntity.DiaganosesPAs.Where(objDiag => objDiag.StudentPersonalId == saveId).SingleOrDefault();
                                if (Diagnose != null)
                                {
                                    Diagnose.Diaganoses = model.RefDiagnosis;
                                    DbEntity.SaveChanges();
                                }

                                StudPersPa = DbEntity.StudentPersonalPAs.Where(objStPers => objStPers.StudentPersonalId == saveId).SingleOrDefault();
                                if (StudPersPa != null)
                                {
                                    StudPersPa.Allergies = model.RefAllergies;
                                    DbEntity.SaveChanges();
                                }

                                medicalIns = DbEntity.MedicalAndInsurances.Where(objMedIns => objMedIns.StudentPersonalId == saveId).SingleOrDefault();
                                if (medicalIns != null)
                                {
                                    medicalIns.FirstName = model.objPhysicianDetails.RefPrimPhyName;
                                    /// medicalIns.SignificantBehaviorCharacteristics=model.objPhysicianDetails.re
                                    medicalIns.DateOfLastPhysicalExam = model.objPhysicianDetails.PhylstApmnt;
                                    DbEntity.SaveChanges();
                                }

                                AddrList = DbEntity.AddressLists.Where(objAdress => objAdress.AddressId == phyAdressID).SingleOrDefault();
                                if (AddrList != null)
                                {
                                    //if (model.objPhysicianDetails.txtCountry != null)
                                    //{
                                    //    try
                                    //    {
                                    //        int coun = Convert.ToInt32(model.objPhysicianDetails.txtCountry);
                                    //        AddrList.CountryId = coun;
                                    //    }
                                    //    catch (InvalidCastException Ex)
                                    //    {
                                    //        AddrList.CountryId = 0;
                                    //        throw Ex;
                                    //    }
                                    //}



                                    //if (model.objPhysicianDetails.txtState != null)
                                    //{
                                    //    try
                                    //    {
                                    //        int State = Convert.ToInt32(model.objPhysicianDetails.txtState);
                                    //        AddrList.StateProvince = State;
                                    //    }
                                    //    catch (InvalidCastException Ex)
                                    //    {
                                    //        AddrList.StateProvince = 0;
                                    //        throw Ex;
                                    //    }
                                    //}

                                    AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objPhysicianDetails.txtCountry;
                                    AddrList.StateProvince = model.objPhysicianDetails.txtState;
                                    AddrList.City = model.objPhysicianDetails.txtCity;
                                    AddrList.StreetName = model.objPhysicianDetails.txtStreetAdress;
                                    AddrList.PostalCode = model.objPhysicianDetails.txtZipCode;
                                    AddrList.Phone = model.objPhysicianDetails.txtHomePhone;
                                    DbEntity.SaveChanges();
                                }

                                Insurnce = DbEntity.Insurances.Where(objInsurnce => objInsurnce.StudentPersonalId == saveId & objInsurnce.PreferType == "Primary").SingleOrDefault();
                                if (Insurnce != null)
                                {
                                    Insurnce.InsuranceType = model.objInsuranceDetails.InsuranceCoverage;
                                    Insurnce.PolicyNumber = model.objInsuranceDetails.InsurancePolNum;
                                    Insurnce.CompanyName = model.objInsuranceDetails.RefInsuranceCompany;
                                    Insurnce.SchoolId = schoolId;
                                    DbEntity.SaveChanges();
                                }


                                AddrList = DbEntity.AddressLists.Where(objAdress => objAdress.AddressId == insAdressId).SingleOrDefault();
                                if (AddrList != null)
                                {
                                    AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objInsuranceDetails.txtCountry;
                                    AddrList.StateProvince = model.objInsuranceDetails.txtState;
                                    AddrList.City = model.objInsuranceDetails.txtCity;
                                    AddrList.StreetName = model.objInsuranceDetails.txtStreetAdress;
                                    AddrList.PostalCode = model.objInsuranceDetails.txtZipCode;
                                    DbEntity.SaveChanges();
                                }

                                Insurnce = DbEntity.Insurances.Where(objInsurnce => objInsurnce.StudentPersonalId == saveId & objInsurnce.PreferType == "Secondary").SingleOrDefault();
                                if (Insurnce != null)
                                {
                                    Insurnce.InsuranceType = model.objInsuranceSecDetails.InsuranceCoverage;
                                    Insurnce.PolicyNumber = model.objInsuranceSecDetails.InsurancePolNum;
                                    Insurnce.CompanyName = model.objInsuranceSecDetails.RefInsuranceCompany;
                                    DbEntity.SaveChanges();
                                }


                                AddrList = DbEntity.AddressLists.Where(objAdress => objAdress.AddressId == insSecAdressId).SingleOrDefault();
                                if (AddrList != null)
                                {
                                    AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objInsuranceSecDetails.txtCountry;
                                    AddrList.StateProvince = model.objInsuranceSecDetails.txtState;
                                    AddrList.City = model.objInsuranceSecDetails.txtCity;
                                    AddrList.StreetName = model.objInsuranceSecDetails.txtStreetAdress;
                                    AddrList.PostalCode = model.objInsuranceSecDetails.txtZipCode;
                                    DbEntity.SaveChanges();
                                }

                                Insurnce = DbEntity.Insurances.Where(objInsurnce => objInsurnce.StudentPersonalId == saveId & objInsurnce.PreferType == "Dental").SingleOrDefault();
                                if (Insurnce != null)
                                {
                                    Insurnce.InsuranceType = model.objInsuranceDentalDetails.InsuranceCoverage;
                                    Insurnce.PolicyNumber = model.objInsuranceDentalDetails.InsurancePolNum;
                                    Insurnce.CompanyName = model.objInsuranceDentalDetails.RefInsuranceCompany;
                                    DbEntity.SaveChanges();
                                }


                                AddrList = DbEntity.AddressLists.Where(objAdress => objAdress.AddressId == insDentalAdressId).SingleOrDefault();
                                if (AddrList != null)
                                {
                                    AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objInsuranceDentalDetails.txtCountry;
                                    AddrList.StateProvince = model.objInsuranceDentalDetails.txtState;
                                    AddrList.City = model.objInsuranceDentalDetails.txtCity;
                                    AddrList.StreetName = model.objInsuranceDentalDetails.txtStreetAdress;
                                    AddrList.PostalCode = model.objInsuranceDentalDetails.txtZipCode;
                                    DbEntity.SaveChanges();
                                }
                            }
                            catch (InvalidCastException Ex)
                            {
                                throw Ex;
                            }
                        }

                        else if (type == "Personal History")
                        {
                            medicalIns = DbEntity.MedicalAndInsurances.Where(objMedIns => objMedIns.StudentPersonalId == saveId).SingleOrDefault();
                            if (medicalIns != null)
                            {
                                medicalIns.SignificantBehaviorCharacteristics = model.RefSpecificProb;
                                DbEntity.SaveChanges();
                            }

                        }



                        retVal = saveId;
                    }
                    else
                    {
                        if (type == "General Information")
                        {
                            StudentTbl.FirstName = model.RefFrstName;
                            StudentTbl.LastName = model.RefLstName;
                            StudentTbl.MiddleName = model.RefMaidenName;
                            StudentTbl.SchoolId = schoolId;
                            StudentTbl.Height = Convert.ToDecimal(model.RefPresentHght);
                            StudentTbl.Weight = Convert.ToDecimal(model.RefPresntWeigth);
                            StudentTbl.HairColor = model.RefHairColor;
                            StudentTbl.EyeColor = model.RefEyecolor;
                            StudentTbl.MiddleName = model.RefMaidenName;
                            StudentTbl.PrimaryDiag = model.PrimaryDiag;
                            StudentTbl.SecondaryDiag = model.SecondaryDiag;
                            StudentTbl.SocialSecurityNo = model.SocialSecurityNo;
                            StudentTbl.SSINo = model.SsiNo;
                            StudentTbl.LocalId = "STD " + saveId;
                            StudentTbl.Gender = model.RefGender;
                            StudentTbl.AdmissionDate = (model.RefDate == null) ? (DateTime?)null : DateTime.ParseExact(model.RefDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            StudentTbl.BirthDate = (model.RefDOB == null) ? (DateTime?)null : DateTime.ParseExact(model.RefDOB, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            StudentTbl.StudentType = "Referral";
                            StudentTbl.CreatedBy = loginId;
                            StudentTbl.CreatedOn = DateTime.Now;
                            DbEntity.StudentPersonals.Add(StudentTbl);
                            DbEntity.SaveChanges();
                            model.RefPersonalId = StudentTbl.StudentPersonalId;

                            int addressType = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Address Type" && objLookup.LookupName == "Physical location address").Select(objlookup => objlookup.LookupId).Single();


                            //AddrList.CountryId = model.RefCountry;//DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Address Type" && objLookup.LookupName == "Physical location address").Select(objlookup => objlookup.LookupId).Single();
                            AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single();
                            AddrList.StateProvince = model.RefState;
                            AddrList.City = model.RefCity;
                            AddrList.StreetName = model.RefStreetAdrs;
                            AddrList.ApartmentType = model.RefAptUnit;
                            AddrList.PostalCode = model.RefZipCode;
                            AddrList.CreatedBy = loginId;
                            AddrList.CreatedOn = DateTime.Now;
                            DbEntity.AddressLists.Add(AddrList);
                            DbEntity.SaveChanges();
                            model.AddressId = AddrList.AddressId;

                            StdAdrRel.StudentPersonalId = model.RefPersonalId;
                            StdAdrRel.AddressId = model.AddressId;
                            StdAdrRel.ContactSequence = 0;
                            StdAdrRel.ContactPersonalId = 0;
                            StdAdrRel.CreatedBy = loginId;
                            StdAdrRel.CreatedOn = DateTime.Now;
                            DbEntity.StudentAddresRels.Add(StdAdrRel);
                            DbEntity.SaveChanges();
                            if ((model.objRelationMthr.txtFirstName != null && model.objRelationMthr.txtFirstName != "") && (model.objRelationMthr.txtLstName != null && model.objRelationMthr.txtLstName != ""))
                            {
                                ConPersonal.StudentPersonalId = model.RefPersonalId;
                                ConPersonal.FirstName = model.objRelationMthr.txtFirstName;
                                ConPersonal.LastName = model.objRelationMthr.txtLstName;
                                ConPersonal.MiddleName = model.objRelationMthr.txtMiddleName;
                                ConPersonal.ContactFlag = "Referral";
                                ConPersonal.Status = 1;
                                ConPersonal.CreatedBy = loginId;
                                ConPersonal.CreatedOn = DateTime.Now;
                                
                                ConPersonal.BirthDate = (model.objRelationMthr.txtDob == null) ? (DateTime?)null : DateTime.ParseExact(model.objRelationMthr.txtDob, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                ConPersonal.CountryOfCitizenship = model.objRelationMthr.txtUScitzen;
                                ConPersonal.Employer = model.objRelationMthr.txtEmployer;
                                ConPersonal.Occupation = model.objRelationMthr.txtOccupation;
                                ConPersonal.MaritalStatus = model.objRelationMthr.txtMaritalStatus;

                                DbEntity.ContactPersonals.Add(ConPersonal);
                                DbEntity.SaveChanges();
                                model.ContactId = ConPersonal.ContactPersonalId;

                                AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single();// model.objRelationMthr.txtCountry;
                                AddrList.StateProvince = model.objRelationMthr.txtState;
                                AddrList.AddressType = addressType;
                                AddrList.City = model.objRelationMthr.txtCity;
                                AddrList.StreetName = model.objRelationMthr.txtStreetAdress;
                                AddrList.ApartmentType = model.objRelationMthr.txtAprtmentUnit;
                                AddrList.PostalCode = model.objRelationMthr.txtZipCode;
                                AddrList.Phone = model.objRelationMthr.txtHomePhone;
                                AddrList.OtherPhone = model.objRelationMthr.txtWorkPhone;
                                AddrList.PrimaryEmail = model.objRelationMthr.txtEmail;
                                AddrList.CreatedBy = loginId;
                                AddrList.CreatedOn = DateTime.Now;
                                DbEntity.AddressLists.Add(AddrList);
                                DbEntity.SaveChanges();
                                model.AddressId = AddrList.AddressId;

                                StdCntctRel.ContactPersonalId = model.ContactId;
                                StdCntctRel.RelationshipId = model.lukUpMthrId;
                                StdCntctRel.CreatedBy = loginId;
                                StdCntctRel.CreatedOn = DateTime.Now;
                                DbEntity.StudentContactRelationships.Add(StdCntctRel);
                                DbEntity.SaveChanges();

                                StdAdrRel.AddressId = AddrList.AddressId;
                                StdAdrRel.StudentPersonalId = StudentTbl.StudentPersonalId;
                                StdAdrRel.ContactPersonalId = ConPersonal.ContactPersonalId;
                                StdAdrRel.ContactSequence = 1;
                                StdAdrRel.CreatedBy = loginId;
                                StdAdrRel.CreatedOn = DateTime.Now;
                                DbEntity.StudentAddresRels.Add(StdAdrRel);
                                DbEntity.SaveChanges();
                            }
                            if ((model.objRelationFthr.txtFirstName != null && model.objRelationFthr.txtFirstName != "") && (model.objRelationFthr.txtLstName != null && model.objRelationFthr.txtLstName != ""))
                            {
                                ConPersonal.StudentPersonalId = model.RefPersonalId;
                                ConPersonal.FirstName = model.objRelationFthr.txtFirstName;
                                ConPersonal.LastName = model.objRelationFthr.txtLstName;
                                ConPersonal.MiddleName = model.objRelationFthr.txtMiddleName;
                                ConPersonal.ContactFlag = "Referral";
                                ConPersonal.Status = 1;
                                ConPersonal.CreatedBy = loginId;
                                ConPersonal.CreatedOn = DateTime.Now;
                               
                                ConPersonal.BirthDate = (model.objRelationFthr.txtDob == null) ? (DateTime?)null : DateTime.ParseExact(model.objRelationFthr.txtDob, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                ConPersonal.CountryOfCitizenship = model.objRelationFthr.txtUScitzen;
                                ConPersonal.Employer = model.objRelationFthr.txtEmployer;
                                ConPersonal.Occupation = model.objRelationFthr.txtOccupation;
                                ConPersonal.MaritalStatus = model.objRelationFthr.txtMaritalStatus;

                                DbEntity.ContactPersonals.Add(ConPersonal);
                                DbEntity.SaveChanges();
                                model.ContactId = ConPersonal.ContactPersonalId;

                                AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objRelationFthr.txtCountry;
                                AddrList.StateProvince = model.objRelationFthr.txtState;
                                AddrList.City = model.objRelationFthr.txtCity;
                                AddrList.AddressType = addressType;
                                AddrList.StreetName = model.objRelationFthr.txtStreetAdress;
                                AddrList.ApartmentType = model.objRelationFthr.txtAprtmentUnit;
                                AddrList.PostalCode = model.objRelationFthr.txtZipCode;
                                AddrList.Phone = model.objRelationFthr.txtHomePhone;
                                AddrList.OtherPhone = model.objRelationFthr.txtWorkPhone;
                                AddrList.PrimaryEmail = model.objRelationFthr.txtEmail;
                                AddrList.CreatedBy = loginId;
                                AddrList.CreatedOn = DateTime.Now;
                                DbEntity.AddressLists.Add(AddrList);
                                DbEntity.SaveChanges();
                                model.AddressId = AddrList.AddressId;

                                StdCntctRel.ContactPersonalId = model.ContactId;
                                StdCntctRel.RelationshipId = model.lukUpFathrId;
                                StdCntctRel.CreatedBy = loginId;
                                StdCntctRel.CreatedOn = DateTime.Now;
                                DbEntity.StudentContactRelationships.Add(StdCntctRel);
                                DbEntity.SaveChanges();

                                StdAdrRel.AddressId = AddrList.AddressId;
                                StdAdrRel.StudentPersonalId = StudentTbl.StudentPersonalId;
                                StdAdrRel.ContactPersonalId = ConPersonal.ContactPersonalId;
                                StdAdrRel.ContactSequence = 1;
                                StdAdrRel.CreatedBy = loginId;
                                StdAdrRel.CreatedOn = DateTime.Now;
                                DbEntity.StudentAddresRels.Add(StdAdrRel);
                                DbEntity.SaveChanges();
                            }
                            if ((model.objRelationClose.txtFirstName != null && model.objRelationClose.txtFirstName != "") && (model.objRelationClose.txtLstName != null && model.objRelationClose.txtLstName != ""))
                            {
                                model.objRelationClose.RelationName = (-1).ToString();
                                ConPersonal.StudentPersonalId = model.RefPersonalId;
                                ConPersonal.ContactFlag = "Referral";
                                ConPersonal.Status = 1;
                                ConPersonal.FirstName = model.objRelationClose.txtFirstName;
                                ConPersonal.LastName = model.objRelationClose.txtLstName;
                                ConPersonal.MiddleName = model.objRelationClose.txtMiddleName;
                                ConPersonal.Relation = Convert.ToInt32(model.objRelationClose.RelationName);
                                ConPersonal.CreatedBy = loginId;
                                ConPersonal.CreatedOn = DateTime.Now;
                                DbEntity.ContactPersonals.Add(ConPersonal);
                                DbEntity.SaveChanges();
                                model.ContactId = ConPersonal.ContactPersonalId;


                                AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objRelationClose.txtCountry;
                                AddrList.StateProvince = model.objRelationClose.txtState;
                                AddrList.City = model.objRelationClose.txtCity;
                                AddrList.AddressType = addressType;
                                AddrList.StreetName = model.objRelationClose.txtStreetAdress;
                                AddrList.ApartmentType = model.objRelationClose.txtAprtmentUnit;
                                AddrList.PostalCode = model.objRelationClose.txtZipCode;
                                AddrList.Phone = model.objRelationClose.txtHomePhone;
                                AddrList.OtherPhone = model.objRelationClose.txtWorkPhone;
                                AddrList.PrimaryEmail = model.objRelationClose.txtEmail;
                                AddrList.CreatedBy = loginId;
                                AddrList.CreatedOn = DateTime.Now;
                                DbEntity.AddressLists.Add(AddrList);
                                DbEntity.SaveChanges();
                                model.AddressId = AddrList.AddressId;

                                StdCntctRel.ContactPersonalId = model.ContactId;
                                StdCntctRel.RelationshipId = model.objRelationClose.closeRelation;
                                StdCntctRel.CreatedBy = loginId;
                                StdCntctRel.CreatedOn = DateTime.Now;
                                DbEntity.StudentContactRelationships.Add(StdCntctRel);
                                DbEntity.SaveChanges();


                                StdAdrRel.AddressId = AddrList.AddressId;
                                StdAdrRel.StudentPersonalId = StudentTbl.StudentPersonalId;
                                StdAdrRel.ContactPersonalId = ConPersonal.ContactPersonalId;
                                StdAdrRel.ContactSequence = 1;
                                StdAdrRel.CreatedBy = loginId;
                                StdAdrRel.CreatedOn = DateTime.Now;
                                DbEntity.StudentAddresRels.Add(StdAdrRel);
                                DbEntity.SaveChanges();
                            }

                            ConPersonal.Relation = -1;
                            if ((model.objRelationLegalGuardian.txtFirstName != null && model.objRelationLegalGuardian.txtFirstName != "") && (model.objRelationLegalGuardian.txtLstName != null && model.objRelationLegalGuardian.txtLstName != ""))
                            {
                                ConPersonal.StudentPersonalId = model.RefPersonalId;
                                ConPersonal.FirstName = model.objRelationLegalGuardian.txtFirstName;
                                ConPersonal.LastName = model.objRelationLegalGuardian.txtLstName;
                                ConPersonal.MiddleName = model.objRelationLegalGuardian.txtMiddleName;
                                ConPersonal.ContactFlag = "Referral";
                                ConPersonal.Status = 1;
                                ConPersonal.CreatedBy = loginId;
                                ConPersonal.CreatedOn = DateTime.Now;
                                DbEntity.ContactPersonals.Add(ConPersonal);
                                DbEntity.SaveChanges();
                                model.ContactId = ConPersonal.ContactPersonalId;

                                AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objRelationLegalGuardian.txtCountry;
                                AddrList.StateProvince = model.objRelationLegalGuardian.txtState;
                                AddrList.City = model.objRelationLegalGuardian.txtCity;
                                AddrList.AddressType = addressType;
                                AddrList.StreetName = model.objRelationLegalGuardian.txtStreetAdress;
                                AddrList.ApartmentType = model.objRelationLegalGuardian.txtAprtmentUnit;
                                AddrList.PostalCode = model.objRelationLegalGuardian.txtZipCode;
                                AddrList.Phone = model.objRelationLegalGuardian.txtHomePhone;
                                AddrList.OtherPhone = model.objRelationLegalGuardian.txtWorkPhone;
                                AddrList.PrimaryEmail = model.objRelationLegalGuardian.txtEmail;
                                AddrList.CreatedBy = loginId;
                                AddrList.CreatedOn = DateTime.Now;
                                DbEntity.AddressLists.Add(AddrList);
                                DbEntity.SaveChanges();
                                model.AddressId = AddrList.AddressId;

                                StdCntctRel.ContactPersonalId = model.ContactId;
                                StdCntctRel.RelationshipId = model.lukupLegalGurdianId;
                                StdCntctRel.CreatedBy = loginId;
                                StdCntctRel.CreatedOn = DateTime.Now;
                                DbEntity.StudentContactRelationships.Add(StdCntctRel);
                                DbEntity.SaveChanges();

                                StdAdrRel.AddressId = AddrList.AddressId;
                                StdAdrRel.StudentPersonalId = StudentTbl.StudentPersonalId;
                                StdAdrRel.ContactPersonalId = ConPersonal.ContactPersonalId;
                                StdAdrRel.ContactSequence = 1;
                                StdAdrRel.CreatedBy = loginId;
                                StdAdrRel.CreatedOn = DateTime.Now;
                                DbEntity.StudentAddresRels.Add(StdAdrRel);
                                DbEntity.SaveChanges();
                            }
                            if ((model.objRelationEmergncyContact.txtFirstName != null && model.objRelationEmergncyContact.txtFirstName != "") && (model.objRelationEmergncyContact.txtLstName != null && model.objRelationEmergncyContact.txtLstName != ""))
                            {
                                ConPersonal.StudentPersonalId = model.RefPersonalId;
                                ConPersonal.ContactFlag = "Referral";
                                ConPersonal.Status = 1;
                                ConPersonal.FirstName = model.objRelationEmergncyContact.txtFirstName;
                                ConPersonal.LastName = model.objRelationEmergncyContact.txtLstName;
                                ConPersonal.MiddleName = model.objRelationEmergncyContact.txtMiddleName;
                                ConPersonal.Relation = model.objRelationEmergncyContact.EmRelationName;
                                ConPersonal.CreatedBy = loginId;
                                ConPersonal.CreatedOn = DateTime.Now;
                                DbEntity.ContactPersonals.Add(ConPersonal);
                                DbEntity.SaveChanges();
                                model.ContactId = ConPersonal.ContactPersonalId;

                                AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objRelationEmergncyContact.txtCountry;
                                AddrList.StateProvince = model.objRelationEmergncyContact.txtState;
                                AddrList.City = model.objRelationEmergncyContact.txtCity;
                                AddrList.AddressType = addressType;
                                AddrList.StreetName = model.objRelationEmergncyContact.txtStreetAdress;
                                AddrList.ApartmentType = model.objRelationEmergncyContact.txtAprtmentUnit;
                                AddrList.PostalCode = model.objRelationEmergncyContact.txtZipCode;
                                AddrList.Phone = model.objRelationEmergncyContact.txtHomePhone;
                                AddrList.OtherPhone = model.objRelationEmergncyContact.txtWorkPhone;
                                AddrList.PrimaryEmail = model.objRelationEmergncyContact.txtEmail;
                                AddrList.CreatedBy = loginId;
                                AddrList.CreatedOn = DateTime.Now;
                                DbEntity.AddressLists.Add(AddrList);
                                DbEntity.SaveChanges();
                                model.AddressId = AddrList.AddressId;

                                StdCntctRel.ContactPersonalId = model.ContactId;
                                StdCntctRel.RelationshipId = model.lukupEmergencyId;
                                StdCntctRel.CreatedBy = loginId;
                                StdCntctRel.CreatedOn = DateTime.Now;
                                DbEntity.StudentContactRelationships.Add(StdCntctRel);
                                DbEntity.SaveChanges();

                                StdAdrRel.AddressId = AddrList.AddressId;
                                StdAdrRel.StudentPersonalId = StudentTbl.StudentPersonalId;
                                StdAdrRel.ContactPersonalId = ConPersonal.ContactPersonalId;
                                StdAdrRel.ContactSequence = 1;
                                StdAdrRel.CreatedBy = loginId;
                                StdAdrRel.CreatedOn = DateTime.Now;
                                DbEntity.StudentAddresRels.Add(StdAdrRel);
                                DbEntity.SaveChanges();
                            }
                            retVal = model.RefPersonalId;


                        }
                        else if (type == "General Family Background Information")
                        {
                            ClsErrorLog erorLog = new ClsErrorLog();
                            try
                            {
                                int mothrConPersId = (from objStudntPersnl in DbEntity.StudentPersonals
                                                      join StdAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals StdAddrRel.StudentPersonalId
                                                      join ConPrsonal in DbEntity.ContactPersonals on StdAddrRel.ContactPersonalId equals ConPrsonal.ContactPersonalId
                                                      join Addr in DbEntity.AddressLists on StdAddrRel.AddressId equals Addr.AddressId
                                                      join StudContactRel in DbEntity.StudentContactRelationships on ConPrsonal.ContactPersonalId equals StudContactRel.ContactPersonalId
                                                      where (StudContactRel.RelationshipId == model.lukUpMthrId & StdAddrRel.ContactSequence == 1 & objStudntPersnl.StudentPersonalId == saveId
                                                      && ConPrsonal.ContactPersonalId == StdAddrRel.ContactPersonalId)
                                                      select new
                                                      {
                                                          ContactId = ConPrsonal.ContactPersonalId
                                                      }).SingleOrDefault().ContactId;
                                erorLog.WriteToLog("mothrConPersId : " + mothrConPersId);

                                int FthrConPersId = (from objStudntPersnl in DbEntity.StudentPersonals
                                                     join StdAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals StdAddrRel.StudentPersonalId
                                                     join ConPrsonal in DbEntity.ContactPersonals on StdAddrRel.ContactPersonalId equals ConPrsonal.ContactPersonalId
                                                     join Addr in DbEntity.AddressLists on StdAddrRel.AddressId equals Addr.AddressId
                                                     join StudContactRel in DbEntity.StudentContactRelationships on ConPrsonal.ContactPersonalId equals StudContactRel.ContactPersonalId
                                                     where (StudContactRel.RelationshipId == model.lukUpFathrId & StdAddrRel.ContactSequence == 1 & objStudntPersnl.StudentPersonalId == saveId
                                                     && ConPrsonal.ContactPersonalId == StdAddrRel.ContactPersonalId)
                                                     select new
                                                     {
                                                         ContactId = ConPrsonal.ContactPersonalId
                                                     }).SingleOrDefault().ContactId;

                                erorLog.WriteToLog("FthrConPersId : " + FthrConPersId);

                                
                               
                            }
                            catch (Exception ex)
                            {

                                erorLog.WriteToLog("SaveId : " + saveId);
                                erorLog.WriteToLog(ex.ToString());
                            }
                            retVal = saveId;


                        }


                        else if (type == "Birth And Development History")
                        {
                            if (saveId > 0)
                            {
                                StudentTbl = DbEntity.StudentPersonals.Where(objStudent => objStudent.StudentPersonalId == saveId).SingleOrDefault();
                                StudentTbl.PlaceOfBirth = model.RefbirthPlace;
                                StudentTbl.StateOfBirth = model.RefStateBirth;
                                StudentTbl.CountryOfBirth = model.RefCntryBirth;
                                DbEntity.SaveChanges();
                                int var = DbEntity.DiaganosesPAs.Where(objStudent => objStudent.StudentPersonalId == saveId).Count();
                                Diagnose = DbEntity.DiaganosesPAs.Where(objDiag => objDiag.StudentPersonalId == saveId).SingleOrDefault();
                                if (var > 0)
                                {

                                    if (Diagnose != null)
                                    {
                                        Diagnose.Diaganoses = model.RefDiagnosis;
                                        DbEntity.SaveChanges();
                                    }
                                }
                                else
                                {
                                    Diagnose.StudentPersonalId = saveId;
                                    Diagnose.Diaganoses = model.RefDiagnosis;
                                    Diagnose.CreatedBy = loginId;
                                    Diagnose.CreatedOn = DateTime.Now;
                                    DbEntity.DiaganosesPAs.Add(Diagnose);
                                    DbEntity.SaveChanges();
                                }

                                int var1 = DbEntity.StudentPersonalPAs.Where(objStudent => objStudent.StudentPersonalId == saveId).Count();
                                StudPersPa = DbEntity.StudentPersonalPAs.Where(objDiag => objDiag.StudentPersonalId == saveId).SingleOrDefault();
                                if (var > 0)
                                {

                                    if (StudPersPa != null)
                                    {
                                        StudPersPa.Allergies = model.RefAllergies;
                                        DbEntity.SaveChanges();
                                    }
                                }
                                else
                                {
                                    StudPersPa.StudentPersonalId = saveId;
                                    StudPersPa.Allergies = model.RefAllergies;
                                    StudPersPa.CreatedBy = loginId;
                                    StudPersPa.CreatedOn = DateTime.Now;
                                    DbEntity.StudentPersonalPAs.Add(StudPersPa);
                                    DbEntity.SaveChanges();
                                }



                                AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objPhysicianDetails.txtCountry;
                                AddrList.StateProvince = model.objPhysicianDetails.txtState;
                                AddrList.City = model.objPhysicianDetails.txtCity;
                                AddrList.StreetName = model.objPhysicianDetails.txtStreetAdress;
                                AddrList.PostalCode = model.objPhysicianDetails.txtZipCode;
                                AddrList.Phone = model.objPhysicianDetails.txtHomePhone;
                                DbEntity.AddressLists.Add(AddrList);
                                DbEntity.SaveChanges();
                                model.AddressId = AddrList.AddressId;

                                medicalIns.StudentPersonalId = saveId;
                                medicalIns.AddressId = model.AddressId;
                                medicalIns.FirstName = model.objPhysicianDetails.RefPrimPhyName;
                                medicalIns.DateOfLastPhysicalExam = model.objPhysicianDetails.PhylstApmnt;
                                medicalIns.CreatedBy = loginId;
                                medicalIns.CreatedOn = DateTime.Now;
                                DbEntity.MedicalAndInsurances.Add(medicalIns);
                                DbEntity.SaveChanges();

                                AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objInsuranceDetails.txtCountry;
                                AddrList.StateProvince = model.objInsuranceDetails.txtState;
                                AddrList.City = model.objInsuranceDetails.txtCity;
                                AddrList.StreetName = model.objInsuranceDetails.txtStreetAdress;
                                AddrList.PostalCode = model.objInsuranceDetails.txtZipCode;
                                DbEntity.AddressLists.Add(AddrList);
                                DbEntity.SaveChanges();
                                model.AddressId = AddrList.AddressId;

                                Insurnce.StudentPersonalId = saveId;
                                Insurnce.AddressId = model.AddressId;
                                Insurnce.PreferType = "Primary";
                                Insurnce.InsuranceType = model.objInsuranceDetails.InsuranceCoverage;
                                Insurnce.PolicyNumber = model.objInsuranceDetails.InsurancePolNum;
                                Insurnce.CompanyName = model.objInsuranceDetails.RefInsuranceCompany;
                                Insurnce.SchoolId = schoolId;
                                DbEntity.Insurances.Add(Insurnce);
                                DbEntity.SaveChanges();

                                AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objInsuranceSecDetails.txtCountry;
                                AddrList.StateProvince = model.objInsuranceSecDetails.txtState;
                                AddrList.City = model.objInsuranceSecDetails.txtCity;
                                AddrList.StreetName = model.objInsuranceSecDetails.txtStreetAdress;
                                AddrList.PostalCode = model.objInsuranceSecDetails.txtZipCode;
                                DbEntity.AddressLists.Add(AddrList);
                                DbEntity.SaveChanges();
                                model.AddressId = AddrList.AddressId;

                                Insurnce.StudentPersonalId = saveId;
                                Insurnce.AddressId = model.AddressId;
                                Insurnce.PreferType = "Secondary";
                                Insurnce.InsuranceType = model.objInsuranceSecDetails.InsuranceCoverage;
                                Insurnce.PolicyNumber = model.objInsuranceSecDetails.InsurancePolNum;
                                Insurnce.CompanyName = model.objInsuranceSecDetails.RefInsuranceCompany;
                                Insurnce.SchoolId = schoolId;
                                DbEntity.Insurances.Add(Insurnce);
                                DbEntity.SaveChanges();

                                AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objInsuranceDentalDetails.txtCountry;
                                AddrList.StateProvince = model.objInsuranceDentalDetails.txtState;
                                AddrList.City = model.objInsuranceDentalDetails.txtCity;
                                AddrList.StreetName = model.objInsuranceDentalDetails.txtStreetAdress;
                                AddrList.PostalCode = model.objInsuranceDentalDetails.txtZipCode;
                                DbEntity.AddressLists.Add(AddrList);
                                DbEntity.SaveChanges();
                                model.AddressId = AddrList.AddressId;

                                Insurnce.StudentPersonalId = saveId;
                                Insurnce.AddressId = model.AddressId;
                                Insurnce.PreferType = "Dental";
                                Insurnce.InsuranceType = model.objInsuranceDentalDetails.InsuranceCoverage;
                                Insurnce.PolicyNumber = model.objInsuranceDentalDetails.InsurancePolNum;
                                Insurnce.CompanyName = model.objInsuranceDentalDetails.RefInsuranceCompany;
                                Insurnce.SchoolId = schoolId;
                                DbEntity.Insurances.Add(Insurnce);
                                DbEntity.SaveChanges();


                                retVal = saveId;

                            }
                        }
                        else if (type == "Personal History")
                        {
                            MedicalAndInsurance med = new MedicalAndInsurance();
                            try
                            {
                                medicalIns = DbEntity.MedicalAndInsurances.Where(objMedIns => objMedIns.StudentPersonalId == saveId).SingleOrDefault();
                            }
                            catch
                            {

                            }
                            if (medicalIns != null)
                            {
                                medicalIns.SignificantBehaviorCharacteristics = model.RefSpecificProb;
                                DbEntity.SaveChanges();
                            }
                            else
                            {
                                med.StudentPersonalId = saveId;
                                med.SignificantBehaviorCharacteristics = model.RefSpecificProb;
                                med.CreatedBy = loginId;
                                med.CreatedOn = DateTime.Now;
                                DbEntity.MedicalAndInsurances.Add(med);
                                DbEntity.SaveChanges();
                            }
                            retVal = saveId;

                        }
                        else if (type == "Recreational Activities")
                        {

                            retVal = saveId;

                        }
                        else if (type == "Present Self-Help Skills, Social Skills and Mobility")
                        {

                            retVal = saveId;

                        }
                        else if (type == "Funding Information")
                        {

                            retVal = saveId;

                        }

                    }
                    trans.Complete();
                }
            }
            catch
            {

            }
            return retVal;
        }


        public GenInfoModel EditData(int clientId, string type)
        {

            MelmarkDBEntities DbEntity = new MelmarkDBEntities();
            GenInfoModel model = new GenInfoModel();

            if (type == "General Information")
            {
                if (model != null)
                {

                    model = (from objStudntPersnl in DbEntity.StudentPersonals
                             join objStdtAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals objStdtAddrRel.StudentPersonalId
                             join objAddress in DbEntity.AddressLists on objStdtAddrRel.AddressId equals objAddress.AddressId
                             where (objStudntPersnl.StudentPersonalId == clientId & objStdtAddrRel.ContactSequence == 0)

                             select new GenInfoModel
                             {
                                 RefFrstName = objStudntPersnl.FirstName,
                                 RefLstName = objStudntPersnl.LastName,
                                 RefGender = objStudntPersnl.Gender,
                                 PrimaryDiag = objStudntPersnl.PrimaryDiag,
                                 SecondaryDiag = objStudntPersnl.SecondaryDiag,
                                 SocialSecurityNo = objStudntPersnl.SocialSecurityNo,
                                 SsiNo = objStudntPersnl.SSINo,
                                 RefMaidenName = objStudntPersnl.MiddleName,
                                 RefPresentHght = objStudntPersnl.Height,
                                 RefPresntWeigth = objStudntPersnl.Weight,
                                 RefEyecolor = objStudntPersnl.EyeColor,
                                 RefHairColor = objStudntPersnl.HairColor,
                                 RefDateDatetime = objStudntPersnl.AdmissionDate,
                                 RefDOBDateTime = objStudntPersnl.BirthDate,
                                 RefCountry = objAddress.CountryId,
                                 RefState = objAddress.StateProvince,
                                 RefCity = objAddress.City,
                                 RefStreetAdrs = objAddress.StreetName,
                                 RefAptUnit = objAddress.ApartmentType,
                                 RefZipCode = objAddress.PostalCode

                             }).SingleOrDefault();
                    model.RefDate = model.RefDateDatetime == null ? "" : ((DateTime)model.RefDateDatetime).ToString("MM/dd/yyyy").Replace("-", "/");
                    model.RefDOB = model.RefDOBDateTime == null ? "" : ((DateTime)model.RefDOBDateTime).ToString("MM/dd/yyyy").Replace("-", "/");


                    try
                    {
                        model.lukUpFathrId = ((from LookUp lukup in DbEntity.LookUps
                                               where (lukup.LookupName == "Father")
                                               select new
                                               {
                                                   lukUpFathrId = lukup.LookupId

                                               }).SingleOrDefault()).lukUpFathrId;

                        model.lukUpMthrId = ((from LookUp lukup in DbEntity.LookUps
                                              where (lukup.LookupName == "Mother")
                                              select new
                                              {
                                                  lukUpMthrId = lukup.LookupId

                                              }).SingleOrDefault()).lukUpMthrId;


                        model.lukupCloseId = ((from LookUp lukup in DbEntity.LookUps
                                               where (lukup.LookupName == "None")
                                               select new
                                               {
                                                   lukupCloseId = lukup.LookupId


                                               }).SingleOrDefault()).lukupCloseId;

                        model.lukupLegalGurdianId = ((from LookUp lukup in DbEntity.LookUps
                                                      where (lukup.LookupName == "Legal Guardian")
                                                      select new
                                                      {
                                                          lukupCloseId = lukup.LookupId


                                                      }).SingleOrDefault()).lukupCloseId;

                        model.lukupEmergencyId = ((from LookUp lukup in DbEntity.LookUps
                                                   where (lukup.LookupName == "Emergency Contact")
                                                   select new
                                                   {
                                                       lukupCloseId = lukup.LookupId


                                                   }).SingleOrDefault()).lukupCloseId;
                    }
                    catch
                    {
                        model.lukUpFathrId = 0;
                        model.lukUpMthrId = 0;
                        model.lukupCloseId = 0;
                        model.lukupLegalGurdianId = 0;
                        model.lukupEmergencyId = 0;
                    }



                    model.objRelationMthr = (from objStudntPersnl in DbEntity.StudentPersonals
                                             join StdAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals StdAddrRel.StudentPersonalId
                                             join ConPersonal in DbEntity.ContactPersonals on StdAddrRel.ContactPersonalId equals ConPersonal.ContactPersonalId
                                             join Addr in DbEntity.AddressLists on StdAddrRel.AddressId equals Addr.AddressId
                                             join StudContactRel in DbEntity.StudentContactRelationships on ConPersonal.ContactPersonalId equals StudContactRel.ContactPersonalId
                                             where (StudContactRel.RelationshipId == model.lukUpMthrId & StdAddrRel.ContactSequence == 1 & objStudntPersnl.StudentPersonalId == clientId)
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


                    model.objRelationFthr = (from objStudntPersnl in DbEntity.StudentPersonals
                                             join StdAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals StdAddrRel.StudentPersonalId
                                             join ConPersonal in DbEntity.ContactPersonals on StdAddrRel.ContactPersonalId equals ConPersonal.ContactPersonalId
                                             join Addr in DbEntity.AddressLists on StdAddrRel.AddressId equals Addr.AddressId
                                             join StudContactRel in DbEntity.StudentContactRelationships on ConPersonal.ContactPersonalId equals StudContactRel.ContactPersonalId
                                             where (StudContactRel.RelationshipId == model.lukUpFathrId & StdAddrRel.ContactSequence == 1 & objStudntPersnl.StudentPersonalId == clientId)
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




                    model.objRelationClose = (from objStudntPersnl in DbEntity.StudentPersonals
                                              join StdAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals StdAddrRel.StudentPersonalId
                                              join ConPersonal in DbEntity.ContactPersonals on StdAddrRel.ContactPersonalId equals ConPersonal.ContactPersonalId
                                              join Addr in DbEntity.AddressLists on StdAddrRel.AddressId equals Addr.AddressId
                                              join StudContactRel in DbEntity.StudentContactRelationships on ConPersonal.ContactPersonalId equals StudContactRel.ContactPersonalId
                                              where (StudContactRel.RelationshipId == model.lukupCloseId & StdAddrRel.ContactSequence == 1 & objStudntPersnl.StudentPersonalId == clientId)
                                              select new RelationCLs
                                              {
                                                  txtFirstName = ConPersonal.FirstName,
                                                  txtLstName = ConPersonal.LastName,
                                                  txtMiddleName = ConPersonal.MiddleName,
                                                  RelationName = ConPersonal.Relation.ToString(),
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


                    //model.objRelationClose = (from objStudntPersnl in DbEntity.StudentPersonals
                    //                          join StdAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals StdAddrRel.StudentPersonalId
                    //                          join ConPersonal in DbEntity.ContactPersonals on StdAddrRel.ContactPersonalId equals ConPersonal.ContactPersonalId
                    //                          join Addr in DbEntity.AddressLists on StdAddrRel.AddressId equals Addr.AddressId
                    //                          join StudContactRel in DbEntity.StudentContactRelationships on ConPersonal.ContactPersonalId equals StudContactRel.ContactPersonalId
                    //                          where (StudContactRel.RelationshipId == model.lukupCloseId & StdAddrRel.ContactSequence == 1 & objStudntPersnl.StudentPersonalId == clientId)
                    //                          select new RelationCLs
                    //                          {
                    //                              txtFirstName = ConPersonal.FirstName,
                    //                              txtLstName = ConPersonal.LastName,
                    //                              txtMiddleName = ConPersonal.MiddleName,
                    //                              txtCountry = Addr.CountryId,
                    //                              txtState = Addr.StateProvince,
                    //                              txtStreetAdress = Addr.StreetName,
                    //                              txtCity = Addr.City,
                    //                              txtAprtmentUnit = Addr.ApartmentType,
                    //                              txtZipCode = Addr.PostalCode,
                    //                              txtHomePhone = Addr.Phone,
                    //                              txtWorkPhone = Addr.OtherPhone,
                    //                              txtEmail = Addr.PrimaryEmail

                    //                          }).FirstOrDefault();

                    model.objRelationLegalGuardian = (from objStudntPersnl in DbEntity.StudentPersonals
                                                      join StdAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals StdAddrRel.StudentPersonalId
                                                      join ConPersonal in DbEntity.ContactPersonals on StdAddrRel.ContactPersonalId equals ConPersonal.ContactPersonalId
                                                      join Addr in DbEntity.AddressLists on StdAddrRel.AddressId equals Addr.AddressId
                                                      join StudContactRel in DbEntity.StudentContactRelationships on ConPersonal.ContactPersonalId equals StudContactRel.ContactPersonalId
                                                      where (StudContactRel.RelationshipId == model.lukupLegalGurdianId & StdAddrRel.ContactSequence == 1 & objStudntPersnl.StudentPersonalId == clientId)
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

                    model.objRelationEmergncyContact = (from objStudntPersnl in DbEntity.StudentPersonals
                                                        join StdAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals StdAddrRel.StudentPersonalId
                                                        join ConPersonal in DbEntity.ContactPersonals on StdAddrRel.ContactPersonalId equals ConPersonal.ContactPersonalId
                                                        join Addr in DbEntity.AddressLists on StdAddrRel.AddressId equals Addr.AddressId
                                                        join StudContactRel in DbEntity.StudentContactRelationships on ConPersonal.ContactPersonalId equals StudContactRel.ContactPersonalId
                                                        where (StudContactRel.RelationshipId == model.lukupEmergencyId & StdAddrRel.ContactSequence == 1 & objStudntPersnl.StudentPersonalId == clientId)
                                                        select new RelationCLs
                                                        {
                                                            txtFirstName = ConPersonal.FirstName,
                                                            txtLstName = ConPersonal.LastName,
                                                            txtMiddleName = ConPersonal.MiddleName,
                                                            EmRelationName =Convert.ToInt32(ConPersonal.Relation),
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



                }
            }
            else if (type == "General Family Background Information")
            {

                if (model != null)
                {
                    model.lukUpFathrId = ((from LookUp lukup in DbEntity.LookUps
                                           where (lukup.LookupName == "Father")
                                           select new
                                           {
                                               lukUpFathrId = lukup.LookupId

                                           }).SingleOrDefault()).lukUpFathrId;

                    model.lukUpMthrId = ((from LookUp lukup in DbEntity.LookUps
                                          where (lukup.LookupName == "Mother")
                                          select new
                                          {
                                              lukUpMthrId = lukup.LookupId

                                          }).SingleOrDefault()).lukUpMthrId;


                    model.lukupCloseId = ((from LookUp lukup in DbEntity.LookUps
                                           where (lukup.LookupName == "None")
                                           select new
                                           {
                                               lukupCloseId = lukup.LookupId


                                           }).SingleOrDefault()).lukupCloseId;


                    model.objRelationMthr = (from objStudntPersnl in DbEntity.StudentPersonals
                                             join StdAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals StdAddrRel.StudentPersonalId
                                             join ConPersonal in DbEntity.ContactPersonals on StdAddrRel.ContactPersonalId equals ConPersonal.ContactPersonalId
                                             join Addr in DbEntity.AddressLists on StdAddrRel.AddressId equals Addr.AddressId
                                             join StudContactRel in DbEntity.StudentContactRelationships on ConPersonal.ContactPersonalId equals StudContactRel.ContactPersonalId
                                             where (StudContactRel.RelationshipId == model.lukUpMthrId & StdAddrRel.ContactSequence == 1 & objStudntPersnl.StudentPersonalId == clientId)
                                             select new RelationCLs
                                             {
                                                 txtFirstName = ConPersonal.FirstName,
                                                 txtLstName = ConPersonal.LastName,
                                                 txtDobdatetime = ConPersonal.BirthDate,
                                                 txtUScitzen = ConPersonal.CountryOfCitizenship,
                                                 txtEmployer = ConPersonal.Employer,
                                                 txtMaritalStatus = ConPersonal.MaritalStatus,
                                                 Occupation = ConPersonal.Occupation

                                             }).FirstOrDefault();

                    model.objRelationMthr.txtDob = model.objRelationMthr.txtDobdatetime == null ? "" : ((DateTime)model.objRelationMthr.txtDobdatetime).ToString("MM/dd/yyyy").Replace("-", "/");



                    model.objRelationFthr = (from objStudntPersnl in DbEntity.StudentPersonals
                                             join StdAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals StdAddrRel.StudentPersonalId
                                             join ConPersonal in DbEntity.ContactPersonals on StdAddrRel.ContactPersonalId equals ConPersonal.ContactPersonalId
                                             join Addr in DbEntity.AddressLists on StdAddrRel.AddressId equals Addr.AddressId
                                             join StudContactRel in DbEntity.StudentContactRelationships on ConPersonal.ContactPersonalId equals StudContactRel.ContactPersonalId
                                             where (StudContactRel.RelationshipId == model.lukUpFathrId & StdAddrRel.ContactSequence == 1 & objStudntPersnl.StudentPersonalId == clientId)
                                             select new RelationCLs
                                             {
                                                 txtFirstName = ConPersonal.FirstName,
                                                 txtLstName = ConPersonal.LastName,
                                                 txtDobdatetime = ConPersonal.BirthDate,
                                                 txtUScitzen = ConPersonal.CountryOfCitizenship,
                                                 txtEmployer = ConPersonal.Employer,
                                                 Occupation = ConPersonal.Occupation

                                             }).FirstOrDefault();

                    model.objRelationFthr.txtDob = model.objRelationFthr.txtDobdatetime == null ? "" : ((DateTime)model.objRelationFthr.txtDobdatetime).ToString("MM/dd/yyyy").Replace("-", "/");

                }

            }

            else if (type == "Birth And Development History")
            {
                if (model != null)
                {

                    model = (from objStudPersnl in DbEntity.StudentPersonals
                             where (objStudPersnl.StudentPersonalId == clientId)
                             select new GenInfoModel
                             {
                                 RefbirthPlace = objStudPersnl.PlaceOfBirth,
                                 RefCntryBirth = objStudPersnl.CountryOfBirth,
                                 RefStateBirth = objStudPersnl.StateOfBirth

                             }).SingleOrDefault();

                    model.RefDiagnosis = (from DiagPa in DbEntity.DiaganosesPAs
                                          where (DiagPa.StudentPersonalId == clientId)
                                          select new GenInfoModel
                                          {
                                              RefDiagnosis = DiagPa.Diaganoses

                                          }).SingleOrDefault().RefDiagnosis;

                    model.RefAllergies = (from StdPersPa in DbEntity.StudentPersonalPAs
                                          where (StdPersPa.StudentPersonalId == clientId)
                                          select new GenInfoModel
                                          {
                                              RefAllergies = StdPersPa.Allergies

                                          }).SingleOrDefault().RefAllergies;

                    model.objPhysicianDetails = (from mediclInsur in DbEntity.MedicalAndInsurances
                                                 join Addr in DbEntity.AddressLists on mediclInsur.AddressId equals Addr.AddressId
                                                 where (mediclInsur.StudentPersonalId == clientId)
                                                 select new RelationCLs
                                                 {
                                                     RefPrimPhyName = mediclInsur.FirstName,
                                                     txtCountry = Addr.CountryId,
                                                     txtState = Addr.StateProvince,
                                                     txtCity = Addr.City,
                                                     txtStreetAdress = Addr.StreetName,
                                                     txtZipCode = Addr.PostalCode,
                                                     txtHomePhone = Addr.Phone,
                                                     PhylstApmntdateTime = mediclInsur.DateOfLastPhysicalExam

                                                 }).FirstOrDefault();

                    model.objPhysicianDetails.PhylstApmnt = model.objPhysicianDetails.PhylstApmntdateTime;

                    model.objInsuranceDetails = (from Insurnce in DbEntity.Insurances
                                                 join Addr in DbEntity.AddressLists on Insurnce.AddressId equals Addr.AddressId
                                                 where (Insurnce.StudentPersonalId == clientId & Insurnce.PreferType == "Primary")
                                                 select new RelationCLs
                                                 {
                                                     RefInsuranceCompany = Insurnce.CompanyName,
                                                     txtCountry = Addr.CountryId,
                                                     txtState = Addr.StateProvince,
                                                     txtCity = Addr.City,
                                                     txtStreetAdress = Addr.StreetName,
                                                     txtZipCode = Addr.PostalCode,
                                                     InsuranceCoverage = Insurnce.InsuranceType,
                                                     InsurancePolNum = Insurnce.PolicyNumber

                                                 }).FirstOrDefault();

                    model.objInsuranceSecDetails = (from Insurnce in DbEntity.Insurances
                                                    join Addr in DbEntity.AddressLists on Insurnce.AddressId equals Addr.AddressId
                                                    where (Insurnce.StudentPersonalId == clientId & Insurnce.PreferType == "Secondary")
                                                    select new RelationCLs
                                                    {
                                                        RefInsuranceCompany = Insurnce.CompanyName,
                                                        txtCountry = Addr.CountryId,
                                                        txtState = Addr.StateProvince,
                                                        txtCity = Addr.City,
                                                        txtStreetAdress = Addr.StreetName,
                                                        txtZipCode = Addr.PostalCode,
                                                        InsuranceCoverage = Insurnce.InsuranceType,
                                                        InsurancePolNum = Insurnce.PolicyNumber

                                                    }).FirstOrDefault();

                    model.objInsuranceDentalDetails = (from Insurnce in DbEntity.Insurances
                                                       join Addr in DbEntity.AddressLists on Insurnce.AddressId equals Addr.AddressId
                                                       where (Insurnce.StudentPersonalId == clientId & Insurnce.PreferType == "Dental")
                                                       select new RelationCLs
                                                       {
                                                           RefInsuranceCompany = Insurnce.CompanyName,
                                                           txtCountry = Addr.CountryId,
                                                           txtState = Addr.StateProvince,
                                                           txtCity = Addr.City,
                                                           txtStreetAdress = Addr.StreetName,
                                                           txtZipCode = Addr.PostalCode,
                                                           InsuranceCoverage = Insurnce.InsuranceType,
                                                           InsurancePolNum = Insurnce.PolicyNumber

                                                       }).FirstOrDefault();




                }

            }

            else if (type == "Personal History")
            {
                if (model != null)
                {
                    model = (from MediclIns in DbEntity.MedicalAndInsurances
                             where (MediclIns.StudentPersonalId == clientId)
                             select new GenInfoModel
                             {
                                 RefSpecificProb = MediclIns.SignificantBehaviorCharacteristics

                             }).FirstOrDefault();

                }
            }

            else if (type == "Upload Doccuments")
            {
                model.objclsUpld.DocName = "";
                model.objclsUpld.DocType = 0;


            }


            return model;
        }


        public GenInfoModel LoadStudentData(int studentId, int Index)
        {
            MelmarkDBEntities DbEntity = new MelmarkDBEntities();
            GenInfoModel model = new GenInfoModel();

            if (Index == 1)
            {
                if (model != null)
                {

                    model = (from objStudntPersnl in DbEntity.StudentPersonals
                             join objStdtAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals objStdtAddrRel.StudentPersonalId
                             join objAddress in DbEntity.AddressLists on objStdtAddrRel.AddressId equals objAddress.AddressId
                             where (objStudntPersnl.StudentPersonalId == studentId & objStdtAddrRel.ContactSequence == 0)

                             select new GenInfoModel
                             {
                                 RefFrstName = objStudntPersnl.FirstName,
                                 RefLstName = objStudntPersnl.LastName,
                                 RefGender = objStudntPersnl.Gender,
                                 PrimaryDiag = objStudntPersnl.PrimaryDiag,
                                 SecondaryDiag = objStudntPersnl.SecondaryDiag,
                                 SocialSecurityNo = objStudntPersnl.SocialSecurityNo,
                                 SsiNo = objStudntPersnl.SSINo,
                                 RefMaidenName = objStudntPersnl.MiddleName,
                                 RefPresentHght = objStudntPersnl.Height,
                                 RefPresntWeigth = objStudntPersnl.Weight,
                                 RefEyecolor = objStudntPersnl.EyeColor,
                                 RefHairColor = objStudntPersnl.HairColor,
                                 RefDateDatetime = objStudntPersnl.AdmissionDate,
                                 RefDOBDateTime = objStudntPersnl.BirthDate,
                                 RefCountry = objAddress.CountryId,
                                 RefState = objAddress.StateProvince,
                                 RefCity = objAddress.City,
                                 RefStreetAdrs = objAddress.StreetName,
                                 RefAptUnit = objAddress.ApartmentType,
                                 RefZipCode = objAddress.PostalCode

                             }).SingleOrDefault();

                    model.RefDate = model.RefDateDatetime == null ? "" : ((DateTime)model.RefDateDatetime).ToString("MM/dd/yyyy").Replace("-", "/");
                    model.RefDOB = model.RefDOBDateTime == null ? "" : ((DateTime)model.RefDOBDateTime).ToString("MM/dd/yyyy").Replace("-", "/");


                    try
                    {
                        model.lukUpFathrId = ((from LookUp lukup in DbEntity.LookUps
                                               where (lukup.LookupName == "Father")
                                               select new
                                               {
                                                   lukUpFathrId = lukup.LookupId

                                               }).SingleOrDefault()).lukUpFathrId;

                        model.lukUpMthrId = ((from LookUp lukup in DbEntity.LookUps
                                              where (lukup.LookupName == "Mother")
                                              select new
                                              {
                                                  lukUpMthrId = lukup.LookupId

                                              }).SingleOrDefault()).lukUpMthrId;


                        model.lukupCloseId = ((from LookUp lukup in DbEntity.LookUps
                                               where (lukup.LookupName == "None")
                                               select new
                                               {
                                                   lukupCloseId = lukup.LookupId


                                               }).SingleOrDefault()).lukupCloseId;

                        model.lukupLegalGurdianId = ((from LookUp lukup in DbEntity.LookUps
                                                      where (lukup.LookupName == "Legal Guardian")
                                                      select new
                                                      {
                                                          lukupCloseId = lukup.LookupId


                                                      }).SingleOrDefault()).lukupCloseId;

                        model.lukupEmergencyId = ((from LookUp lukup in DbEntity.LookUps
                                                   where (lukup.LookupName == "Emergency Contact")
                                                   select new
                                                   {
                                                       lukupCloseId = lukup.LookupId


                                                   }).SingleOrDefault()).lukupCloseId;
                    }
                    catch
                    {
                        model.lukUpFathrId = 0;
                        model.lukUpMthrId = 0;
                        model.lukupCloseId = 0;
                        model.lukupLegalGurdianId = 0;
                        model.lukupEmergencyId = 0;
                    }



                    model.objRelationMthr = (from objStudntPersnl in DbEntity.StudentPersonals
                                             join StdAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals StdAddrRel.StudentPersonalId
                                             join ConPersonal in DbEntity.ContactPersonals on StdAddrRel.ContactPersonalId equals ConPersonal.ContactPersonalId
                                             join Addr in DbEntity.AddressLists on StdAddrRel.AddressId equals Addr.AddressId
                                             join StudContactRel in DbEntity.StudentContactRelationships on ConPersonal.ContactPersonalId equals StudContactRel.ContactPersonalId
                                             where (StudContactRel.RelationshipId == model.lukUpMthrId & StdAddrRel.ContactSequence == 1 & objStudntPersnl.StudentPersonalId == studentId)
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
                                                 txtEmail = Addr.PrimaryEmail,
                                                 txtDobdatetime = ConPersonal.BirthDate,
                                                 txtUScitzen = ConPersonal.CountryOfCitizenship,
                                                 txtEmployer = ConPersonal.Employer,
                                                 txtOccupation = ConPersonal.Occupation,
                                                 txtMaritalStatus = ConPersonal.MaritalStatus
                                                 
                                                 

                                             }).FirstOrDefault();


                    if (model.objRelationMthr != null)
                    {

                        model.objRelationMthr.txtDob = model.objRelationMthr.txtDobdatetime == null ? "" : ((DateTime)model.objRelationMthr.txtDobdatetime).ToString("MM/dd/yyyy".Replace("-", "/"));
                    }
                    else
                    {
                        int country = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.RefCountry;
                        model.objRelationMthr = new RelationCLs
                        {
                            txtFirstName = "",
                            txtLstName = "",
                            txtMiddleName = "",
                            txtCountry = country

                        };
                    }


                    model.objRelationFthr = (from objStudntPersnl in DbEntity.StudentPersonals
                                             join StdAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals StdAddrRel.StudentPersonalId
                                             join ConPersonal in DbEntity.ContactPersonals on StdAddrRel.ContactPersonalId equals ConPersonal.ContactPersonalId
                                             join Addr in DbEntity.AddressLists on StdAddrRel.AddressId equals Addr.AddressId
                                             join StudContactRel in DbEntity.StudentContactRelationships on ConPersonal.ContactPersonalId equals StudContactRel.ContactPersonalId
                                             where (StudContactRel.RelationshipId == model.lukUpFathrId & StdAddrRel.ContactSequence == 1 & objStudntPersnl.StudentPersonalId == studentId)
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
                                                 txtEmail = Addr.PrimaryEmail,
                                                 txtDobdatetime = ConPersonal.BirthDate,
                                                 txtUScitzen = ConPersonal.CountryOfCitizenship,
                                                 txtEmployer = ConPersonal.Employer,
                                                 txtOccupation = ConPersonal.Occupation,
                                                 txtMaritalStatus = ConPersonal.MaritalStatus
                                                 
                                             }).FirstOrDefault();


                    if (model.objRelationFthr != null)
                    {

                        model.objRelationFthr.txtDob = model.objRelationMthr.txtDobdatetime == null ? "" : ((DateTime)model.objRelationMthr.txtDobdatetime).ToString("MM/dd/yyyy".Replace("-", "/"));
                    }
                    else
                    {
                        int country = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.RefCountry;
                        model.objRelationFthr = new RelationCLs
                        {
                            txtFirstName = "",
                            txtLstName = "",
                            txtMiddleName = "",
                            txtCountry = country

                        };
                    }


                    model.objRelationClose = (from objStudntPersnl in DbEntity.StudentPersonals
                                              join StdAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals StdAddrRel.StudentPersonalId
                                              join ConPersonal in DbEntity.ContactPersonals on StdAddrRel.ContactPersonalId equals ConPersonal.ContactPersonalId
                                              join Addr in DbEntity.AddressLists on StdAddrRel.AddressId equals Addr.AddressId
                                              join StudContactRel in DbEntity.StudentContactRelationships on ConPersonal.ContactPersonalId equals StudContactRel.ContactPersonalId

                                              join Look in DbEntity.LookUps on StudContactRel.RelationshipId equals Look.LookupId
                                              where (Look.LookupType == "Relationship" & StdAddrRel.ContactSequence == 1 & objStudntPersnl.StudentPersonalId == studentId & ConPersonal.Relation == -1)

                                              select new RelationCLs
                                              {
                                                  txtFirstName = ConPersonal.FirstName,
                                                  txtLstName = ConPersonal.LastName,
                                                  txtMiddleName = ConPersonal.MiddleName,
                                                  //RelationName = ConPersonal.Relation,
                                                  closeRelation = (int)ConPersonal.Relation,
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

                    if (model.objRelationClose != null)
                    {

                        model.objRelationFthr.txtDob = model.objRelationMthr.txtDobdatetime == null ? "" : ((DateTime)model.objRelationMthr.txtDobdatetime).ToString("MM/dd/yyyy".Replace("-", "/"));
                    }
                    else
                    {
                        int country = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.RefCountry;
                        model.objRelationClose = new RelationCLs
                        {
                            txtFirstName = "",
                            txtLstName = "",
                            txtMiddleName = "",
                            txtCountry = country

                        };
                    }


                    //model.objRelationClose = (from objStudntPersnl in DbEntity.StudentPersonals
                    //                          join StdAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals StdAddrRel.StudentPersonalId
                    //                          join ConPersonal in DbEntity.ContactPersonals on StdAddrRel.ContactPersonalId equals ConPersonal.ContactPersonalId
                    //                          join Addr in DbEntity.AddressLists on StdAddrRel.AddressId equals Addr.AddressId
                    //                          join StudContactRel in DbEntity.StudentContactRelationships on ConPersonal.ContactPersonalId equals StudContactRel.ContactPersonalId
                    //                          where (StudContactRel.RelationshipId == model.lukupCloseId & StdAddrRel.ContactSequence == 1 & objStudntPersnl.StudentPersonalId == clientId)
                    //                          select new RelationCLs
                    //                          {
                    //                              txtFirstName = ConPersonal.FirstName,
                    //                              txtLstName = ConPersonal.LastName,
                    //                              txtMiddleName = ConPersonal.MiddleName,
                    //                              txtCountry = Addr.CountryId,
                    //                              txtState = Addr.StateProvince,
                    //                              txtStreetAdress = Addr.StreetName,
                    //                              txtCity = Addr.City,
                    //                              txtAprtmentUnit = Addr.ApartmentType,
                    //                              txtZipCode = Addr.PostalCode,
                    //                              txtHomePhone = Addr.Phone,
                    //                              txtWorkPhone = Addr.OtherPhone,
                    //                              txtEmail = Addr.PrimaryEmail

                    //                          }).FirstOrDefault();

                    model.objRelationLegalGuardian = (from objStudntPersnl in DbEntity.StudentPersonals
                                                      join StdAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals StdAddrRel.StudentPersonalId
                                                      join ConPersonal in DbEntity.ContactPersonals on StdAddrRel.ContactPersonalId equals ConPersonal.ContactPersonalId
                                                      join Addr in DbEntity.AddressLists on StdAddrRel.AddressId equals Addr.AddressId
                                                      join StudContactRel in DbEntity.StudentContactRelationships on ConPersonal.ContactPersonalId equals StudContactRel.ContactPersonalId
                                                      join Look in DbEntity.LookUps on StudContactRel.RelationshipId equals Look.LookupId
                                                      where (Look.LookupType == "Relationship" & StudContactRel.RelationshipId == model.lukupLegalGurdianId & StdAddrRel.ContactSequence == 1 & objStudntPersnl.StudentPersonalId == studentId)
                                                      select new RelationCLs
                                                      {
                                                          txtFirstName = ConPersonal.FirstName,
                                                          txtLstName = ConPersonal.LastName,
                                                          txtMiddleName = ConPersonal.MiddleName,
                                                          LgRelationName = Look.LookupId,
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
                    if (model.objRelationLegalGuardian != null)
                    {

                        model.objRelationFthr.txtDob = model.objRelationMthr.txtDobdatetime == null ? "" : ((DateTime)model.objRelationMthr.txtDobdatetime).ToString("MM/dd/yyyy".Replace("-", "/"));
                    }
                    else
                    {
                        int country = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.RefCountry;
                        model.objRelationLegalGuardian = new RelationCLs
                        {
                            txtFirstName = "",
                            txtLstName = "",
                            txtMiddleName = "",
                            txtCountry = country

                        };
                    }


                    model.objRelationEmergncyContact = (from objStudntPersnl in DbEntity.StudentPersonals
                                                        join StdAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals StdAddrRel.StudentPersonalId
                                                        join ConPersonal in DbEntity.ContactPersonals on StdAddrRel.ContactPersonalId equals ConPersonal.ContactPersonalId
                                                        join Addr in DbEntity.AddressLists on StdAddrRel.AddressId equals Addr.AddressId
                                                        join StudContactRel in DbEntity.StudentContactRelationships on ConPersonal.ContactPersonalId equals StudContactRel.ContactPersonalId
                                                        join Look in DbEntity.LookUps on StudContactRel.RelationshipId equals Look.LookupId
                                                        where (Look.LookupType == "Relationship" & StudContactRel.RelationshipId == model.lukupEmergencyId & StdAddrRel.ContactSequence == 1 & objStudntPersnl.StudentPersonalId == studentId)
                                                        select new RelationCLs
                                                        {
                                                            txtFirstName = ConPersonal.FirstName,
                                                            txtLstName = ConPersonal.LastName,
                                                            txtMiddleName = ConPersonal.MiddleName,
                                                           // RelationName = ConPersonal.Relation,
                                                            EmRelationName=Look.LookupId,
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

                    if (model.objRelationEmergncyContact != null)
                    {

                        model.objRelationFthr.txtDob = model.objRelationMthr.txtDobdatetime == null ? "" : ((DateTime)model.objRelationMthr.txtDobdatetime).ToString("MM/dd/yyyy".Replace("-", "/"));
                    }
                    else
                    {
                        int country = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.RefCountry;
                        model.objRelationEmergncyContact = new RelationCLs
                        {
                            txtFirstName = "",
                            txtLstName = "",
                            txtMiddleName = "",
                            txtCountry = country

                        };
                    }



                }
            }
            else if (Index == 2)
            {

                if (model != null)
                {
                    model.lukUpFathrId = ((from LookUp lukup in DbEntity.LookUps
                                           where (lukup.LookupName == "Father")
                                           select new
                                           {
                                               lukUpFathrId = lukup.LookupId

                                           }).SingleOrDefault()).lukUpFathrId;

                    model.lukUpMthrId = ((from LookUp lukup in DbEntity.LookUps
                                          where (lukup.LookupName == "Mother")
                                          select new
                                          {
                                              lukUpMthrId = lukup.LookupId

                                          }).SingleOrDefault()).lukUpMthrId;


                    model.lukupCloseId = ((from LookUp lukup in DbEntity.LookUps
                                           where (lukup.LookupName == "None")
                                           select new
                                           {
                                               lukupCloseId = lukup.LookupId


                                           }).SingleOrDefault()).lukupCloseId;


                    model.objRelationMthr = (from objStudntPersnl in DbEntity.StudentPersonals
                                             join StdAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals StdAddrRel.StudentPersonalId
                                             join ConPersonal in DbEntity.ContactPersonals on StdAddrRel.ContactPersonalId equals ConPersonal.ContactPersonalId
                                             join Addr in DbEntity.AddressLists on StdAddrRel.AddressId equals Addr.AddressId
                                             join StudContactRel in DbEntity.StudentContactRelationships on ConPersonal.ContactPersonalId equals StudContactRel.ContactPersonalId
                                             where (StudContactRel.RelationshipId == model.lukUpMthrId & StdAddrRel.ContactSequence == 1 & objStudntPersnl.StudentPersonalId == studentId)
                                             select new RelationCLs
                                             {
                                                 

                                             }).FirstOrDefault();

                    if (model.objRelationMthr != null)
                    {
                        model.objRelationMthr.txtDob = model.objRelationMthr.txtDobdatetime == null ? "" : ((DateTime)model.objRelationMthr.txtDobdatetime).ToString("MM/dd/yyyy").Replace("-", "/");
                    }
                    else
                    {
                        int country = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.RefCountry;
                        model.objRelationMthr = new RelationCLs
                        {
                            txtFirstName = "",
                            txtLstName = "",
                            txtMiddleName = "",
                            txtCountry = country

                        };
                    }



                    model.objRelationFthr = (from objStudntPersnl in DbEntity.StudentPersonals
                                             join StdAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals StdAddrRel.StudentPersonalId
                                             join ConPersonal in DbEntity.ContactPersonals on StdAddrRel.ContactPersonalId equals ConPersonal.ContactPersonalId
                                             join Addr in DbEntity.AddressLists on StdAddrRel.AddressId equals Addr.AddressId
                                             join StudContactRel in DbEntity.StudentContactRelationships on ConPersonal.ContactPersonalId equals StudContactRel.ContactPersonalId
                                             where (StudContactRel.RelationshipId == model.lukUpFathrId & StdAddrRel.ContactSequence == 1 & objStudntPersnl.StudentPersonalId == studentId)
                                             select new RelationCLs
                                             {
                                                 

                                             }).FirstOrDefault();


                    if (model.objRelationFthr != null)
                    {


                        model.objRelationFthr.txtDob = model.objRelationFthr.txtDobdatetime == null ? "" : ((DateTime)model.objRelationFthr.txtDobdatetime).ToString("MM/dd/yyyy").Replace("-", "/");

                    }
                    else
                    {
                        int country = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.RefCountry;
                        model.objRelationFthr = new RelationCLs
                        {
                            txtFirstName = "",
                            txtLstName = "",
                            txtMiddleName = "",
                            txtCountry = country

                        };
                    }




                }

            }

            else if (Index == 3)
            {
                bool valid = false;

                valid = IsValidCheckTab3(studentId);
                ClsErrorLog errlog = new ClsErrorLog();
                errlog.WriteToLog(" tab 3 valid: " + valid);
                if (valid == true)
                {

                    if (model != null)
                    {

                        model = (from objStudPersnl in DbEntity.StudentPersonals
                                 where (objStudPersnl.StudentPersonalId == studentId)
                                 select new GenInfoModel
                                 {
                                     RefbirthPlace = objStudPersnl.PlaceOfBirth,
                                     RefCntryBirth = objStudPersnl.CountryOfBirth,
                                     RefStateBirth = objStudPersnl.StateOfBirth

                                 }).SingleOrDefault();

                        var RefDiagnosiss = ((from DiagPa in DbEntity.DiaganosesPAs
                                              where (DiagPa.StudentPersonalId == studentId)
                                              select new GenInfoModel
                                              {
                                                  RefDiagnosis = DiagPa.Diaganoses

                                              }).Count() > 0) ?
                            (from DiagPa in DbEntity.DiaganosesPAs
                             where (DiagPa.StudentPersonalId == studentId)
                             select new GenInfoModel
                             {
                                 RefDiagnosis = DiagPa.Diaganoses

                             }).FirstOrDefault() : null;
                        if (RefDiagnosiss != null)
                        {
                            model.RefDiagnosis = RefDiagnosiss.RefDiagnosis;
                        }

                        var RefAllergiess = ((from StdPersPa in DbEntity.StudentPersonalPAs
                                              where (StdPersPa.StudentPersonalId == studentId)
                                              select new GenInfoModel
                                              {
                                                  RefAllergies = StdPersPa.Allergies

                                              }).Count() > 0) ?
                                             (from StdPersPa in DbEntity.StudentPersonalPAs
                                              where (StdPersPa.StudentPersonalId == studentId)
                                              select new GenInfoModel
                                              {
                                                  RefAllergies = StdPersPa.Allergies

                                              }).FirstOrDefault() : null;
                        if (RefAllergiess != null)
                        {
                            model.RefAllergies = RefAllergiess.RefAllergies;
                        }

                        model.objPhysicianDetails = (from mediclInsur in DbEntity.MedicalAndInsurances
                                                     join Addr in DbEntity.AddressLists on mediclInsur.AddressId equals Addr.AddressId
                                                     where (mediclInsur.StudentPersonalId == studentId)
                                                     select new RelationCLs
                                                     {
                                                         RefPrimPhyName = mediclInsur.FirstName,
                                                         txtCountry = Addr.CountryId,
                                                         txtState = Addr.StateProvince,
                                                         txtCity = Addr.City,
                                                         txtStreetAdress = Addr.StreetName,
                                                         txtZipCode = Addr.PostalCode,
                                                         txtHomePhone = Addr.Phone,
                                                         PhylstApmntdateTime = mediclInsur.DateOfLastPhysicalExam

                                                     }).FirstOrDefault();
                        if (model.objPhysicianDetails != null)
                        {
                            model.objPhysicianDetails.PhylstApmnt = model.objPhysicianDetails.PhylstApmntdateTime;
                        }
                        model.objInsuranceDetails = (from Insurnce in DbEntity.Insurances
                                                     join Addr in DbEntity.AddressLists on Insurnce.AddressId equals Addr.AddressId
                                                     where (Insurnce.StudentPersonalId == studentId & Insurnce.PreferType == "Primary")
                                                     select new RelationCLs
                                                     {
                                                         RefInsuranceCompany = Insurnce.CompanyName,
                                                         txtCountry = Addr.CountryId,
                                                         txtState = Addr.StateProvince,
                                                         txtCity = Addr.City,
                                                         txtStreetAdress = Addr.StreetName,
                                                         txtZipCode = Addr.PostalCode,
                                                         InsuranceCoverage = Insurnce.InsuranceType,
                                                         InsurancePolNum = Insurnce.PolicyNumber

                                                     }).FirstOrDefault();

                        model.objInsuranceSecDetails = (from Insurnce in DbEntity.Insurances
                                                        join Addr in DbEntity.AddressLists on Insurnce.AddressId equals Addr.AddressId
                                                        where (Insurnce.StudentPersonalId == studentId & Insurnce.PreferType == "Secondary")
                                                        select new RelationCLs
                                                        {
                                                            RefInsuranceCompany = Insurnce.CompanyName,
                                                            txtCountry = Addr.CountryId,
                                                            txtState = Addr.StateProvince,
                                                            txtCity = Addr.City,
                                                            txtStreetAdress = Addr.StreetName,
                                                            txtZipCode = Addr.PostalCode,
                                                            InsuranceCoverage = Insurnce.InsuranceType,
                                                            InsurancePolNum = Insurnce.PolicyNumber

                                                        }).FirstOrDefault();

                        model.objInsuranceDentalDetails = (from Insurnce in DbEntity.Insurances
                                                           join Addr in DbEntity.AddressLists on Insurnce.AddressId equals Addr.AddressId
                                                           where (Insurnce.StudentPersonalId == studentId & Insurnce.PreferType == "Dental")
                                                           select new RelationCLs
                                                           {
                                                               RefInsuranceCompany = Insurnce.CompanyName,
                                                               txtCountry = Addr.CountryId,
                                                               txtState = Addr.StateProvince,
                                                               txtCity = Addr.City,
                                                               txtStreetAdress = Addr.StreetName,
                                                               txtZipCode = Addr.PostalCode,
                                                               InsuranceCoverage = Insurnce.InsuranceType,
                                                               InsurancePolNum = Insurnce.PolicyNumber

                                                           }).FirstOrDefault();

                    }
                }

            }

            else if (Index == 4)
            {
                bool check = false;
                check = IsValidCheckTab3(studentId);

                if (check == true)
                {

                    if (model != null)
                    {
                        model = ((from MediclIns in DbEntity.MedicalAndInsurances
                                  where (MediclIns.StudentPersonalId == studentId)
                                  select new GenInfoModel
                                  {
                                      RefSpecificProb = MediclIns.SignificantBehaviorCharacteristics

                                  }).Count() > 0) ?
                                  (from MediclIns in DbEntity.MedicalAndInsurances
                                   where (MediclIns.StudentPersonalId == studentId)
                                   select new GenInfoModel
                                   {
                                       RefSpecificProb = MediclIns.SignificantBehaviorCharacteristics

                                   }).FirstOrDefault() : null;

                    }
                }
            }

            else if (Index == 8)
            {
                model.objclsUpld.DocName = "";
                model.objclsUpld.DocType = 0;

            }

            return model;
        }



        public bool IsValidCheckTab3(int studentId)
        {
            MelmarkDBEntities objData = new MelmarkDBEntities();
            bool valid = false;


            var result = (from MedicalInsrnce in objData.MedicalAndInsurances
                          where (MedicalInsrnce.StudentPersonalId == studentId)
                          select new
                          {
                              phyId = MedicalInsrnce.MedicalInsuranceId

                          }).ToList();


            if (result.Count > 0)
            {
                valid = true;
            }
            else valid = false;
            return valid;
        }


        public bool CheckLegalParent(int studentId)
        {
            bool valid = false;
            try
            {
                MelmarkDBEntities DbEntity = new MelmarkDBEntities();


                int legalGuardianId = ((from LookUp lukup in DbEntity.LookUps
                                        where (lukup.LookupName == "Legal Guardian")
                                        select new
                                        {
                                            lukupCloseId = lukup.LookupId


                                        }).SingleOrDefault()).lukupCloseId;


                var result = (from objStudntPersnl in DbEntity.StudentPersonals
                              join StdAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals StdAddrRel.StudentPersonalId
                              join ConPersonal in DbEntity.ContactPersonals on StdAddrRel.ContactPersonalId equals ConPersonal.ContactPersonalId
                              join Addr in DbEntity.AddressLists on StdAddrRel.AddressId equals Addr.AddressId
                              join StudContactRel in DbEntity.StudentContactRelationships on ConPersonal.ContactPersonalId equals StudContactRel.ContactPersonalId
                              where (StudContactRel.RelationshipId == legalGuardianId & StdAddrRel.ContactSequence == 1 & objStudntPersnl.StudentPersonalId == studentId)
                              select new
                              {
                                  contactPersonalId = ConPersonal.ContactPersonalId

                              }).ToList();

                if (result != null)
                {
                    valid = true;
                }
                else valid = false;
            }
            catch
            {
            }
            return valid;
        }




        public int SaveGeneralDataPE(GenInfoModel model, string type, int saveId, string status, int loginId, int schoolId, FormCollection formdata)
        {
            int retVal = 0;

            try
            {
                MelmarkDBEntities DbEntity = new MelmarkDBEntities();
                StudentPersonal StudentTbl = new StudentPersonal();
                AddressList AddrList = new AddressList();
                StudentAddresRel StdAdrRel = new StudentAddresRel();
                ContactPersonal ConPersonal = new ContactPersonal();
                LookUp LukUp = new LookUp();
                StudentContactRelationship StdCntctRel = new StudentContactRelationship();
                DiaganosesPA Diagnose = new DiaganosesPA();
                MedicalAndInsurance medicalIns = new MedicalAndInsurance();
                StudentPersonalPA StudPersPa = new StudentPersonalPA();
                Insurance Insurnce = new Insurance();
                BehavioursPA BehaviorPa = new BehavioursPA();

                try
                {

                    try
                    {
                        model.lukUpFathrId = ((from LookUp lukup in DbEntity.LookUps
                                               where (lukup.LookupName == "Legal Guardian 2")
                                               select new
                                               {
                                                   lukUpFathrId = lukup.LookupId

                                               }).SingleOrDefault()).lukUpFathrId;

                        model.lukUpMthrId = ((from LookUp lukup in DbEntity.LookUps
                                              where (lukup.LookupName == "Legal Guardian 1")
                                              select new
                                              {
                                                  lukUpMthrId = lukup.LookupId


                                              }).SingleOrDefault()).lukUpMthrId;

                        model.lukupCloseId = ((from LookUp lukup in DbEntity.LookUps
                                               where (lukup.LookupName == "None")
                                               select new
                                               {
                                                   lukupCloseId = lukup.LookupId


                                               }).SingleOrDefault()).lukupCloseId;

                        model.lukupLegalGurdianId = ((from LookUp lukup in DbEntity.LookUps
                                                      where (lukup.LookupName == "Legal Guardian")
                                                      select new
                                                      {
                                                          lukupCloseId = lukup.LookupId


                                                      }).SingleOrDefault()).lukupCloseId;


                        model.lukupEmergencyId = ((from LookUp lukup in DbEntity.LookUps
                                                   where (lukup.LookupName == "Emergency Contact")
                                                   select new
                                                   {
                                                       lukupCloseId = lukup.LookupId


                                                   }).SingleOrDefault()).lukupCloseId;


                    }
                    catch
                    {
                        model.lukUpFathrId = 0;
                        model.lukUpMthrId = 0;
                    }



                    using (TransactionScope trans = new TransactionScope(TransactionScopeOption.Required))
                    {

                        if ((saveId > 0) && (status == "update"))
                        {

                            if (type == "General Information")
                            {

                                int RefaddrId = (from objStudntPersnl in DbEntity.StudentPersonals
                                                 join objStdtAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals objStdtAddrRel.StudentPersonalId
                                                 join objAddress in DbEntity.AddressLists on objStdtAddrRel.AddressId equals objAddress.AddressId
                                                 where (objStudntPersnl.StudentPersonalId == saveId & objStdtAddrRel.ContactSequence == 0)

                                                 select new
                                                 {
                                                     AdressId = objAddress.AddressId

                                                 }).SingleOrDefault().AdressId;

                                int mothrAdressId = 0;
                                var mothrAdress = (from objStudntPersnl in DbEntity.StudentPersonals
                                                   join StdAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals StdAddrRel.StudentPersonalId
                                                   join ConPrsonal in DbEntity.ContactPersonals on StdAddrRel.ContactPersonalId equals ConPrsonal.ContactPersonalId
                                                   join Addr in DbEntity.AddressLists on StdAddrRel.AddressId equals Addr.AddressId
                                                   join StudContactRel in DbEntity.StudentContactRelationships on ConPrsonal.ContactPersonalId equals StudContactRel.ContactPersonalId
                                                   where (StudContactRel.RelationshipId == model.lukUpMthrId & StdAddrRel.ContactSequence == 1 & objStudntPersnl.StudentPersonalId == saveId)
                                                   select new
                                                   {
                                                       AdresId = Addr.AddressId
                                                   }).ToList();
                                if (mothrAdress.Count > 0)
                                {
                                    foreach (var item in mothrAdress)
                                    {
                                        mothrAdressId = item.AdresId;
                                    }
                                }


                                int mothrConPersId = 0;
                                var motherConPers = (from objStudntPersnl in DbEntity.StudentPersonals
                                                     join StdAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals StdAddrRel.StudentPersonalId
                                                     join ConPrsonal in DbEntity.ContactPersonals on StdAddrRel.ContactPersonalId equals ConPrsonal.ContactPersonalId
                                                     join Addr in DbEntity.AddressLists on StdAddrRel.AddressId equals Addr.AddressId
                                                     join StudContactRel in DbEntity.StudentContactRelationships on ConPrsonal.ContactPersonalId equals StudContactRel.ContactPersonalId
                                                     where (StudContactRel.RelationshipId == model.lukUpMthrId & StdAddrRel.ContactSequence == 1 & objStudntPersnl.StudentPersonalId == saveId)
                                                     select new
                                                     {
                                                         ContactId = ConPrsonal.ContactPersonalId
                                                     }).ToList();
                                if (motherConPers.Count > 0)
                                {
                                    foreach (var item in motherConPers)
                                    {
                                        mothrConPersId = item.ContactId;
                                    }
                                }

                                int FthrAdressId = 0;
                                var FthrAdress = (from objStudntPersnl in DbEntity.StudentPersonals
                                                  join StdAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals StdAddrRel.StudentPersonalId
                                                  join ConPrsonal in DbEntity.ContactPersonals on StdAddrRel.ContactPersonalId equals ConPrsonal.ContactPersonalId
                                                  join Addr in DbEntity.AddressLists on StdAddrRel.AddressId equals Addr.AddressId
                                                  join StudContactRel in DbEntity.StudentContactRelationships on ConPrsonal.ContactPersonalId equals StudContactRel.ContactPersonalId
                                                  where (StudContactRel.RelationshipId == model.lukUpFathrId & StdAddrRel.ContactSequence == 1 & objStudntPersnl.StudentPersonalId == saveId)
                                                  select new
                                                  {
                                                      AdresId = Addr.AddressId
                                                  }).ToList();

                                if (FthrAdress.Count > 0)
                                {
                                    foreach (var item in FthrAdress)
                                    {
                                        FthrAdressId = item.AdresId;
                                    }
                                }

                                int FthrConPersId = 0;
                                var FthrConPers = (from objStudntPersnl in DbEntity.StudentPersonals
                                                   join StdAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals StdAddrRel.StudentPersonalId
                                                   join ConPrsonal in DbEntity.ContactPersonals on StdAddrRel.ContactPersonalId equals ConPrsonal.ContactPersonalId
                                                   join Addr in DbEntity.AddressLists on StdAddrRel.AddressId equals Addr.AddressId
                                                   join StudContactRel in DbEntity.StudentContactRelationships on ConPrsonal.ContactPersonalId equals StudContactRel.ContactPersonalId
                                                   where (StudContactRel.RelationshipId == model.lukUpFathrId & StdAddrRel.ContactSequence == 1 & objStudntPersnl.StudentPersonalId == saveId)
                                                   select new
                                                   {
                                                       ContactId = ConPrsonal.ContactPersonalId
                                                   }).ToList();

                                if (FthrConPers.Count > 0)
                                {
                                    foreach (var item in FthrConPers)
                                    {
                                        FthrConPersId = item.ContactId;
                                    }
                                }


                                StudentTbl = DbEntity.StudentPersonals.Where(objStudent => objStudent.StudentPersonalId == saveId).SingleOrDefault();
                                StudentTbl.FirstName = model.RefFrstName;
                                StudentTbl.LastName = model.RefLstName;
                                StudentTbl.MiddleName = model.RefMaidenName;
                                StudentTbl.Height = model.RefPresentHght;
                                StudentTbl.Weight = model.RefPresntWeigth;
                                StudentTbl.HairColor = model.RefHairColor;
                                StudentTbl.EyeColor = model.RefEyecolor;
                                StudentTbl.LocalId = "STD " + saveId;
                                StudentTbl.Gender = model.RefGender;
                                StudentTbl.PrimaryDiag = model.PrimaryDiag;
                                StudentTbl.SecondaryDiag = model.SecondaryDiag;
                                StudentTbl.AdmissionDate = (model.RefDate == null) ? (DateTime?)null : DateTime.ParseExact(model.RefDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                StudentTbl.BirthDate = (model.RefDOB == null) ? (DateTime?)null : DateTime.ParseExact(model.RefDOB, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                StudentTbl.DistingushingMarks = model.RefAnyIdentification;
                                StudentTbl.RaceId = model.RefRace;
                                StudentTbl.Ethinicity = model.RefEthinicity;
                                StudentTbl.ReligiousAffiliation = model.RefReligiousAffilation;
                                StudentTbl.ModifiedBy = loginId;
                                StudentTbl.CreatedOn = DateTime.Now;
                                DbEntity.SaveChanges();


                                AddrList = DbEntity.AddressLists.Where(objAdr => objAdr.AddressId == RefaddrId).SingleOrDefault();
                                AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.RefCountry;
                                AddrList.StateProvince = model.RefState;
                                AddrList.City = model.RefCity;
                                AddrList.StreetName = model.RefStreetAdrs;
                                AddrList.ApartmentType = model.RefAptUnit;
                                AddrList.PostalCode = model.RefZipCode;
                                DbEntity.SaveChanges();

                                ConPersonal = DbEntity.ContactPersonals.Where(objCont => objCont.ContactPersonalId == mothrConPersId).SingleOrDefault();
                                if (ConPersonal != null)
                                {
                                    ConPersonal.StudentPersonalId = saveId;
                                    ConPersonal.FirstName = model.objRelationMthr.txtFirstName;
                                    ConPersonal.MiddleName = model.objRelationMthr.txtMiddleName;
                                    ConPersonal.LastName = model.objRelationMthr.txtLstName;
                                   // ConPersonal.Relation = model.objRelationMthr.RelationName;

                                    if (model.objRelationMthr.txtDob != null)
                                    {
                                        model.objRelationMthr.txtDob = model.objRelationMthr.txtDob.Replace("-", "/");
                                    }

                                    ConPersonal.BirthDate = (model.objRelationMthr.txtDob == null) ? (DateTime?)null : DateTime.ParseExact(model.objRelationMthr.txtDob, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                    ConPersonal.CountryOfCitizenship = model.objRelationMthr.txtUScitzen;
                                    ConPersonal.Employer = model.objRelationMthr.txtEmployer;
                                    ConPersonal.Occupation = model.objRelationMthr.Occupation;
                                    ConPersonal.Education = model.objRelationMthr.txtEducation;
                                    ConPersonal.MaritalStatus = model.objRelationMthr.txtMaritalStatus;
                                    ConPersonal.GrossIncome = model.objRelationMthr.GrossIncome;
                                    ConPersonal.CauseDeath = model.objRelationMthr.causedeath;
                                    ConPersonal.ContactFlag = "Referral";
                                    ConPersonal.Status = 1;
                                    DbEntity.SaveChanges();


                                    AddrList = DbEntity.AddressLists.Where(objAdr => objAdr.AddressId == mothrAdressId).SingleOrDefault();
                                    AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objRelationMthr.txtCountry;
                                    AddrList.StateProvince = model.objRelationMthr.txtState;
                                    AddrList.City = model.objRelationMthr.txtCity;
                                    AddrList.StreetName = model.objRelationMthr.txtStreetAdress;
                                    AddrList.ApartmentType = model.objRelationMthr.txtAprtmentUnit;
                                    AddrList.PostalCode = model.objRelationMthr.txtZipCode;
                                    AddrList.Phone = model.objRelationMthr.txtHomePhone;
                                    AddrList.Mobile = model.objRelationMthr.txtMobileNum;
                                    AddrList.Fax = model.objRelationMthr.txtFax;
                                    AddrList.County = model.objRelationMthr.txtCounty;
                                    AddrList.OtherPhone = model.objRelationMthr.txtWorkPhone;
                                    AddrList.PrimaryEmail = model.objRelationMthr.txtEmail;
                                    AddrList.BusinessAddress = model.objRelationMthr.BusinessAddress;
                                    DbEntity.SaveChanges();
                                }
                                else
                                {
                                    if ((model.objRelationMthr.txtFirstName != null && model.objRelationMthr.txtFirstName != "") || (model.objRelationMthr.txtLstName != null && model.objRelationMthr.txtLstName != ""))
                                    {

                                        int addressType = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Address Type" && objLookup.LookupName == "Physical location address").Select(objlookup => objlookup.LookupId).Single();

                                        ConPersonal = new ContactPersonal();
                                        ConPersonal.StudentPersonalId = saveId;
                                        ConPersonal.FirstName = model.objRelationMthr.txtFirstName;
                                        ConPersonal.MiddleName = model.objRelationMthr.txtMiddleName;
                                        ConPersonal.LastName = model.objRelationMthr.txtLstName;
                                       // ConPersonal.Relation = model.objRelationMthr.RelationName;

                                        if (model.objRelationMthr.txtDob != null)
                                        {
                                            model.objRelationMthr.txtDob = model.objRelationMthr.txtDob.Replace("-", "/");
                                        }

                                        ConPersonal.BirthDate = (model.objRelationMthr.txtDob == null) ? (DateTime?)null : DateTime.ParseExact(model.objRelationMthr.txtDob, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                        ConPersonal.CountryOfCitizenship = model.objRelationMthr.txtUScitzen;
                                        ConPersonal.Employer = model.objRelationMthr.txtEmployer;
                                        ConPersonal.Occupation = model.objRelationMthr.Occupation;
                                        ConPersonal.Education = model.objRelationMthr.txtEducation;
                                        ConPersonal.MaritalStatus = model.objRelationMthr.txtMaritalStatus;
                                        ConPersonal.GrossIncome = model.objRelationMthr.GrossIncome;
                                        ConPersonal.CauseDeath = model.objRelationMthr.causedeath;
                                        ConPersonal.ContactFlag = "Referral";
                                        ConPersonal.Status = 1;
                                        ConPersonal.CreatedBy = loginId;
                                        ConPersonal.CreatedOn = DateTime.Now;
                                        DbEntity.ContactPersonals.Add(ConPersonal);
                                        DbEntity.SaveChanges();


                                        AddrList = new AddressList();
                                        AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objRelationMthr.txtCountry;
                                        AddrList.StateProvince = model.objRelationMthr.txtState;
                                        AddrList.City = model.objRelationMthr.txtCity;
                                        AddrList.StreetName = model.objRelationMthr.txtStreetAdress;
                                        AddrList.ApartmentType = model.objRelationMthr.txtAprtmentUnit;
                                        AddrList.PostalCode = model.objRelationMthr.txtZipCode;
                                        AddrList.Phone = model.objRelationMthr.txtHomePhone;
                                        AddrList.Mobile = model.objRelationMthr.txtMobileNum;
                                        AddrList.Fax = model.objRelationMthr.txtFax;
                                        AddrList.County = model.objRelationMthr.txtCounty;
                                        AddrList.OtherPhone = model.objRelationMthr.txtWorkPhone;
                                        AddrList.AddressType = addressType;
                                        AddrList.PrimaryEmail = model.objRelationMthr.txtEmail;
                                        AddrList.BusinessAddress = model.objRelationMthr.BusinessAddress;
                                        AddrList.CreatedBy = loginId;
                                        AddrList.CreatedOn = DateTime.Now;
                                        DbEntity.AddressLists.Add(AddrList);
                                        DbEntity.SaveChanges();

                                        StdCntctRel = new StudentContactRelationship();
                                        StdCntctRel.ContactPersonalId = ConPersonal.ContactPersonalId;
                                        StdCntctRel.RelationshipId = model.lukUpMthrId;
                                        StdCntctRel.CreatedBy = loginId;
                                        StdCntctRel.CreatedOn = DateTime.Now;
                                        DbEntity.StudentContactRelationships.Add(StdCntctRel);
                                        DbEntity.SaveChanges();

                                        StdAdrRel = new StudentAddresRel();
                                        StdAdrRel.StudentPersonalId = saveId;
                                        StdAdrRel.AddressId = AddrList.AddressId;
                                        StdAdrRel.ContactSequence = 1;
                                        StdAdrRel.ContactPersonalId = ConPersonal.ContactPersonalId;
                                        StdAdrRel.CreatedBy = loginId;
                                        StdAdrRel.CreatedOn = DateTime.Now;
                                        DbEntity.StudentAddresRels.Add(StdAdrRel);
                                        DbEntity.SaveChanges();
                                    }

                                }

                                ConPersonal = DbEntity.ContactPersonals.Where(objCont => objCont.ContactPersonalId == FthrConPersId).SingleOrDefault();
                                if (ConPersonal != null)
                                {
                                    ConPersonal.StudentPersonalId = saveId;
                                    ConPersonal.FirstName = model.objRelationFthr.txtFirstName;
                                    ConPersonal.MiddleName = model.objRelationFthr.txtMiddleName;
                                    ConPersonal.LastName = model.objRelationFthr.txtLstName;
                                    //ConPersonal.Relation = model.objRelationFthr.RelationName;
                                    if (model.objRelationFthr.txtDob != null)
                                    {
                                        model.objRelationFthr.txtDob = model.objRelationFthr.txtDob.Replace("-", "/");
                                    }
                                    ConPersonal.BirthDate = (model.objRelationFthr.txtDob == null) ? (DateTime?)null : DateTime.ParseExact(model.objRelationFthr.txtDob, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                    ConPersonal.CountryOfCitizenship = model.objRelationFthr.txtUScitzen;
                                    ConPersonal.Employer = model.objRelationFthr.txtEmployer;
                                    ConPersonal.Occupation = model.objRelationFthr.Occupation;
                                    ConPersonal.Education = model.objRelationFthr.txtEducation;
                                    ConPersonal.MaritalStatus = model.objRelationFthr.txtMaritalStatus;
                                    ConPersonal.GrossIncome = model.objRelationFthr.GrossIncome;
                                    ConPersonal.CauseDeath = model.objRelationFthr.causedeath;
                                    ConPersonal.ContactFlag = "Referral";
                                    ConPersonal.Status = 1;
                                    DbEntity.SaveChanges();

                                    AddrList = DbEntity.AddressLists.Where(objAdr => objAdr.AddressId == FthrAdressId).SingleOrDefault();
                                    AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objRelationFthr.txtCountry;
                                    AddrList.StateProvince = model.objRelationFthr.txtState;
                                    AddrList.City = model.objRelationFthr.txtCity;
                                    AddrList.StreetName = model.objRelationFthr.txtStreetAdress;
                                    AddrList.ApartmentType = model.objRelationFthr.txtAprtmentUnit;
                                    AddrList.PostalCode = model.objRelationFthr.txtZipCode;
                                    AddrList.Phone = model.objRelationFthr.txtHomePhone;
                                    AddrList.OtherPhone = model.objRelationFthr.txtWorkPhone;
                                    AddrList.Mobile = model.objRelationFthr.txtMobileNum;
                                    AddrList.Fax = model.objRelationFthr.txtFax;
                                    AddrList.County = model.objRelationFthr.txtCounty;
                                    AddrList.PrimaryEmail = model.objRelationFthr.txtEmail;
                                    AddrList.BusinessAddress = model.objRelationFthr.BusinessAddress;
                                    DbEntity.SaveChanges();

                                }
                                else
                                {
                                    if ((model.objRelationFthr.txtFirstName != null && model.objRelationFthr.txtFirstName != "") || (model.objRelationFthr.txtLstName != null && model.objRelationFthr.txtLstName != ""))
                                    {

                                        int addressType = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Address Type" && objLookup.LookupName == "Physical location address").Select(objlookup => objlookup.LookupId).Single();

                                        ConPersonal = new ContactPersonal();
                                        ConPersonal.StudentPersonalId = saveId;
                                        ConPersonal.FirstName = model.objRelationFthr.txtFirstName;
                                        ConPersonal.MiddleName = model.objRelationFthr.txtMiddleName;
                                        ConPersonal.LastName = model.objRelationFthr.txtLstName;
                                       // ConPersonal.Relation = model.objRelationFthr.RelationName;
                                        if (model.objRelationFthr.txtDob != null)
                                        {
                                            model.objRelationFthr.txtDob = model.objRelationFthr.txtDob.Replace("-", "/");
                                        }
                                        ConPersonal.BirthDate = (model.objRelationFthr.txtDob == null) ? (DateTime?)null : DateTime.ParseExact(model.objRelationFthr.txtDob, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                        ConPersonal.CountryOfCitizenship = model.objRelationFthr.txtUScitzen;
                                        ConPersonal.Employer = model.objRelationFthr.txtEmployer;
                                        ConPersonal.Occupation = model.objRelationFthr.Occupation;
                                        ConPersonal.Education = model.objRelationFthr.txtEducation;
                                        ConPersonal.MaritalStatus = model.objRelationFthr.txtMaritalStatus;
                                        ConPersonal.GrossIncome = model.objRelationFthr.GrossIncome;
                                        ConPersonal.CauseDeath = model.objRelationFthr.causedeath;
                                        ConPersonal.ContactFlag = "Referral";
                                        ConPersonal.CreatedBy = loginId;
                                        ConPersonal.CreatedOn = DateTime.Now;
                                        ConPersonal.Status = 1;
                                        DbEntity.ContactPersonals.Add(ConPersonal);
                                        DbEntity.SaveChanges();

                                        AddrList = new AddressList();
                                        AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objRelationFthr.txtCountry;
                                        AddrList.StateProvince = model.objRelationFthr.txtState;
                                        AddrList.City = model.objRelationFthr.txtCity;
                                        AddrList.StreetName = model.objRelationFthr.txtStreetAdress;
                                        AddrList.ApartmentType = model.objRelationFthr.txtAprtmentUnit;
                                        AddrList.PostalCode = model.objRelationFthr.txtZipCode;
                                        AddrList.Phone = model.objRelationFthr.txtHomePhone;
                                        AddrList.OtherPhone = model.objRelationFthr.txtWorkPhone;
                                        AddrList.Mobile = model.objRelationFthr.txtMobileNum;
                                        AddrList.Fax = model.objRelationFthr.txtFax;
                                        AddrList.County = model.objRelationFthr.txtCounty;
                                        AddrList.PrimaryEmail = model.objRelationFthr.txtEmail;
                                        AddrList.BusinessAddress = model.objRelationFthr.BusinessAddress;
                                        AddrList.CreatedBy = loginId;
                                        AddrList.AddressType = addressType;
                                        AddrList.CreatedOn = DateTime.Now;
                                        DbEntity.AddressLists.Add(AddrList);
                                        DbEntity.SaveChanges();

                                        StdCntctRel = new StudentContactRelationship();
                                        StdCntctRel.ContactPersonalId = ConPersonal.ContactPersonalId;
                                        StdCntctRel.RelationshipId = model.lukUpFathrId;
                                        StdCntctRel.CreatedBy = loginId;
                                        StdCntctRel.CreatedOn = DateTime.Now;
                                        DbEntity.StudentContactRelationships.Add(StdCntctRel);
                                        DbEntity.SaveChanges();

                                        StdAdrRel = new StudentAddresRel();
                                        StdAdrRel.StudentPersonalId = saveId;
                                        StdAdrRel.AddressId = AddrList.AddressId;
                                        StdAdrRel.ContactSequence = 1;
                                        StdAdrRel.ContactPersonalId = ConPersonal.ContactPersonalId;
                                        StdAdrRel.CreatedBy = loginId;
                                        StdAdrRel.CreatedOn = DateTime.Now;
                                        DbEntity.StudentAddresRels.Add(StdAdrRel);
                                        DbEntity.SaveChanges();
                                    }
                                }


                            }


                            else if (type == "Educational And Vocational Information")
                            {


                            }

                            else if (type == "Birth And Development History")
                            {



                            }

                            else if (type == "Applicant Present Self-Help Skills")
                            {
                                if (saveId > 0)
                                {

                                    var result = (from objBehaviorPa in DbEntity.BehavioursPAs
                                                  join objBehaveCategry in DbEntity.BehaviorCategories on objBehaviorPa.BehaviorId equals objBehaveCategry.BehaviorCategryId
                                                  where (objBehaviorPa.StudentPersonalId == saveId)
                                                  select new
                                                  {
                                                      behaviorId = objBehaviorPa.BehaviorId


                                                  }).ToList();

                                    foreach (var item in result)
                                    {
                                        var dataFromClinet = formdata["behaveNameid_" + item.behaviorId.ToString()];
                                        int behaviorId = Convert.ToInt32(item.behaviorId);
                                        BehaviorPa = DbEntity.BehavioursPAs.Where(objBehaviorPa => objBehaviorPa.BehaviorId == behaviorId & objBehaviorPa.StudentPersonalId == saveId).SingleOrDefault();
                                        BehaviorPa.Score = Convert.ToInt32(dataFromClinet);
                                        DbEntity.SaveChanges();
                                    }
                                }

                                retVal = saveId;
                            }

                            else if (type == "Medical History")
                            {
                                int phyAdressID = 0;
                                try
                                {
                                    phyAdressID = (from mediclInsur in DbEntity.MedicalAndInsurances
                                                   join Addr in DbEntity.AddressLists on mediclInsur.AddressId equals Addr.AddressId
                                                   where (mediclInsur.StudentPersonalId == saveId)
                                                   select new
                                                   {
                                                       AdressId = Addr.AddressId

                                                   }).FirstOrDefault().AdressId;
                                }
                                catch
                                {

                                }


                                Diagnose = DbEntity.DiaganosesPAs.Where(objDiag => objDiag.StudentPersonalId == saveId).FirstOrDefault();
                                Diagnose.BloodGroup = model.objPhysicianDetails.RefBloodType;
                                Diagnose.AllergiesAndReactions = model.objPhysicianDetails.RefAllergiesReaction;
                                Diagnose.SummaryHealthProblem = model.objPhysicianDetails.RefSummaryCurrentHealth;
                                DbEntity.SaveChanges();


                                //try
                                //{
                                medicalIns = DbEntity.MedicalAndInsurances.Where(objMedIns => objMedIns.StudentPersonalId == saveId).FirstOrDefault();
                                medicalIns.FirstName = model.objPhysicianDetails.RefPrimPhyName;
                                medicalIns.Speciality = model.objPhysicianDetails.phySpeciality;
                                if (model.objPhysicianDetails.PhylstApmnt != null)
                                {
                                    model.objPhysicianDetails.PhylstApmnt.Replace("-", "/");
                                }
                                medicalIns.DateOfLastPhysicalExam = model.objPhysicianDetails.PhylstApmnt;
                                DbEntity.SaveChanges();
                                //}
                                //catch { }
                                if (phyAdressID != 0)
                                {
                                    //try
                                    //{

                                    AddrList = DbEntity.AddressLists.Where(objAdress => objAdress.AddressId == phyAdressID).SingleOrDefault();
                                    AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objPhysicianDetails.txtCountry;
                                    AddrList.StateProvince = model.objPhysicianDetails.txtState;
                                    AddrList.City = model.objPhysicianDetails.txtCity;
                                    AddrList.StreetName = model.objPhysicianDetails.txtStreetAdress;
                                    AddrList.PostalCode = model.objPhysicianDetails.txtZipCode;
                                    AddrList.Phone = model.objPhysicianDetails.txtHomePhone;
                                    DbEntity.SaveChanges();
                                    //}
                                    //catch { }
                                }
                                try
                                {
                                    int insAdressId = 0;
                                    var insAdress = (from Insrnce in DbEntity.Insurances
                                                     join Addr in DbEntity.AddressLists on Insrnce.AddressId equals Addr.AddressId
                                                     where (Insrnce.StudentPersonalId == saveId & Insrnce.PreferType == "Primary")
                                                     select new
                                                     {
                                                         AdresId = Addr.AddressId

                                                     }).ToList();
                                    if (insAdress.Count > 0)
                                    {
                                        foreach (var item in insAdress)
                                        {
                                            insAdressId = item.AdresId;
                                        }
                                    }

                                    int insSecAdressId = 0;
                                    var insSecAdress = (from Insrnce in DbEntity.Insurances
                                                        join Addr in DbEntity.AddressLists on Insrnce.AddressId equals Addr.AddressId
                                                        where (Insrnce.StudentPersonalId == saveId & Insrnce.PreferType == "Secondary")
                                                        select new
                                                        {
                                                            AdresId = Addr.AddressId

                                                        }).ToList();

                                    if (insSecAdress.Count > 0)
                                    {
                                        foreach (var item in insSecAdress)
                                        {
                                            insSecAdressId = item.AdresId;
                                        }
                                    }



                                    Insurnce = DbEntity.Insurances.Where(objInsurnce => objInsurnce.StudentPersonalId == saveId & objInsurnce.PreferType == "Primary").SingleOrDefault();
                                    if (Insurnce != null)
                                    {
                                        Insurnce.InsuranceType = model.objInsuranceDetails.InsuranceCoverage;
                                        Insurnce.PolicyNumber = model.objInsuranceDetails.InsurancePolNum;
                                        Insurnce.CompanyName = model.objInsuranceDetails.RefInsuranceCompany;
                                        DbEntity.SaveChanges();


                                        AddrList = DbEntity.AddressLists.Where(objAdress => objAdress.AddressId == insAdressId).SingleOrDefault();
                                        AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objInsuranceDetails.txtCountry;
                                        AddrList.StateProvince = model.objInsuranceDetails.txtState;
                                        AddrList.City = model.objInsuranceDetails.txtCity;
                                        AddrList.StreetName = model.objInsuranceDetails.txtStreetAdress;
                                        AddrList.PostalCode = model.objInsuranceDetails.txtZipCode;
                                        DbEntity.SaveChanges();
                                    }
                                    else
                                    {

                                        Insurnce.StudentPersonalId = model.RefPersonalId;
                                        Insurnce.AddressId = model.AddressId;
                                        Insurnce.PreferType = "Primary";
                                        Insurnce.InsuranceType = model.objInsuranceDetails.InsuranceCoverage;
                                        Insurnce.PolicyNumber = model.objInsuranceDetails.InsurancePolNum;
                                        Insurnce.CompanyName = model.objInsuranceDetails.RefInsuranceCompany;
                                        DbEntity.Insurances.Add(Insurnce);
                                        DbEntity.SaveChanges();

                                        AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objInsuranceSecDetails.txtCountry;
                                        AddrList.StateProvince = model.objInsuranceSecDetails.txtState;
                                        AddrList.City = model.objInsuranceSecDetails.txtCity;
                                        AddrList.StreetName = model.objInsuranceSecDetails.txtStreetAdress;
                                        AddrList.PostalCode = model.objInsuranceSecDetails.txtZipCode;
                                        DbEntity.AddressLists.Add(AddrList);
                                        DbEntity.SaveChanges();
                                        model.AddressId = AddrList.AddressId;


                                    }
                                    Insurnce = DbEntity.Insurances.Where(objInsurnce => objInsurnce.StudentPersonalId == saveId & objInsurnce.PreferType == "Secondary").SingleOrDefault();
                                    if (Insurnce != null)
                                    {
                                        Insurnce.InsuranceType = model.objInsuranceSecDetails.InsuranceCoverage;
                                        Insurnce.PolicyNumber = model.objInsuranceSecDetails.InsurancePolNum;
                                        Insurnce.CompanyName = model.objInsuranceSecDetails.RefInsuranceCompany;
                                        DbEntity.SaveChanges();


                                        AddrList = DbEntity.AddressLists.Where(objAdress => objAdress.AddressId == insSecAdressId).SingleOrDefault();
                                        AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objInsuranceSecDetails.txtCountry;
                                        AddrList.StateProvince = model.objInsuranceSecDetails.txtState;
                                        AddrList.City = model.objInsuranceSecDetails.txtCity;
                                        AddrList.StreetName = model.objInsuranceSecDetails.txtStreetAdress;
                                        AddrList.PostalCode = model.objInsuranceSecDetails.txtZipCode;
                                        DbEntity.SaveChanges();
                                    }
                                    else
                                    {
                                        AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objInsuranceSecDetails.txtCountry;
                                        AddrList.StateProvince = model.objInsuranceSecDetails.txtState;
                                        AddrList.City = model.objInsuranceSecDetails.txtCity;
                                        AddrList.StreetName = model.objInsuranceSecDetails.txtStreetAdress;
                                        AddrList.PostalCode = model.objInsuranceSecDetails.txtZipCode;
                                        DbEntity.AddressLists.Add(AddrList);
                                        DbEntity.SaveChanges();
                                        model.AddressId = AddrList.AddressId;

                                        Insurnce.StudentPersonalId = model.RefPersonalId;
                                        Insurnce.AddressId = model.AddressId;
                                        Insurnce.PreferType = "Secondary";
                                        Insurnce.InsuranceType = model.objInsuranceSecDetails.InsuranceCoverage;
                                        Insurnce.PolicyNumber = model.objInsuranceSecDetails.InsurancePolNum;
                                        Insurnce.CompanyName = model.objInsuranceSecDetails.RefInsuranceCompany;
                                        DbEntity.Insurances.Add(Insurnce);
                                        DbEntity.SaveChanges();

                                    }
                                }
                                catch { }

                            }

                            else if (type == "Legal Material")
                            {
                                int legalGuardianAdressId = 0;
                                int LegalContactPersnlId = 0;
                                //try
                                //{
                                var legalGuardianAdress = (from objStudntPersnl in DbEntity.StudentPersonals
                                                           join StdAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals StdAddrRel.StudentPersonalId
                                                           join ConPrsonal in DbEntity.ContactPersonals on StdAddrRel.ContactPersonalId equals ConPrsonal.ContactPersonalId
                                                           join Addr in DbEntity.AddressLists on StdAddrRel.AddressId equals Addr.AddressId
                                                           join StudContactRel in DbEntity.StudentContactRelationships on ConPrsonal.ContactPersonalId equals StudContactRel.ContactPersonalId
                                                           where (StudContactRel.RelationshipId == model.lukupLegalGurdianId & StdAddrRel.ContactSequence == 1 & objStudntPersnl.StudentPersonalId == saveId)
                                                           select new
                                                           {
                                                               AdresId = Addr.AddressId
                                                           }).OrderByDescending(x => x.AdresId).ToList();
                                //}  
                                if (legalGuardianAdress.Count > 0)
                                {
                                    foreach (var item in legalGuardianAdress)
                                    {
                                        legalGuardianAdressId = item.AdresId;
                                    }
                                }

                                //catch { }
                                //try
                                //{
                                var LegalContactPersnl = (from objStudntPersnl in DbEntity.StudentPersonals
                                                          join StdAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals StdAddrRel.StudentPersonalId
                                                          join ConPrsonal in DbEntity.ContactPersonals on StdAddrRel.ContactPersonalId equals ConPrsonal.ContactPersonalId
                                                          join Addr in DbEntity.AddressLists on StdAddrRel.AddressId equals Addr.AddressId
                                                          join StudContactRel in DbEntity.StudentContactRelationships on ConPrsonal.ContactPersonalId equals StudContactRel.ContactPersonalId
                                                          where (StudContactRel.RelationshipId == model.lukupLegalGurdianId & StdAddrRel.ContactSequence == 1 & objStudntPersnl.StudentPersonalId == saveId)
                                                          select new
                                                          {
                                                              ContactId = ConPrsonal.ContactPersonalId
                                                          }).OrderByDescending(x => x.ContactId).ToList();

                                if (LegalContactPersnl.Count > 0)
                                {
                                    foreach (var item in LegalContactPersnl)
                                    {
                                        LegalContactPersnlId = item.ContactId;
                                    }
                                }
                                //}
                                //catch { }

                                if (LegalContactPersnlId != 0)
                                {
                                    ConPersonal = DbEntity.ContactPersonals.Where(objCont => objCont.ContactPersonalId == LegalContactPersnlId).SingleOrDefault();
                                    ConPersonal.FirstName = model.objRelationLegalGuardian.txtFirstName;
                                    ConPersonal.MiddleName = model.objRelationLegalGuardian.txtMiddleName;
                                    ConPersonal.LastName = model.objRelationLegalGuardian.txtLstName;
                                    DbEntity.SaveChanges();
                                }
                                if (legalGuardianAdressId != 0)
                                {
                                    AddrList = DbEntity.AddressLists.Where(objAdr => objAdr.AddressId == legalGuardianAdressId).SingleOrDefault();
                                    AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objRelationLegalGuardian.txtCountry;
                                    AddrList.StateProvince = model.objRelationLegalGuardian.txtState;
                                    AddrList.City = model.objRelationLegalGuardian.txtCity;
                                    AddrList.StreetName = model.objRelationLegalGuardian.txtStreetAdress;
                                    AddrList.ApartmentType = model.objRelationLegalGuardian.txtAprtmentUnit;
                                    AddrList.PostalCode = model.objRelationLegalGuardian.txtZipCode;
                                    AddrList.Phone = model.objRelationLegalGuardian.txtHomePhone;
                                    AddrList.OtherPhone = model.objRelationLegalGuardian.txtWorkPhone;
                                    AddrList.PrimaryEmail = model.objRelationLegalGuardian.txtEmail;
                                    DbEntity.SaveChanges();
                                }
                                else
                                {

                                    int addressType = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Address Type" && objLookup.LookupName == "Physical location address").Select(objlookup => objlookup.LookupId).Single();

                                    if ((model.objRelationLegalGuardian.txtFirstName != null && model.objRelationLegalGuardian.txtFirstName != "") || (model.objRelationLegalGuardian.txtLstName != null && model.objRelationLegalGuardian.txtLstName != ""))
                                    {
                                        ConPersonal.StudentPersonalId = saveId;
                                        ConPersonal.FirstName = model.objRelationLegalGuardian.txtFirstName;
                                        ConPersonal.LastName = model.objRelationLegalGuardian.txtLstName;
                                        ConPersonal.MiddleName = model.objRelationLegalGuardian.txtMiddleName;
                                        ConPersonal.CreatedBy = loginId;
                                        ConPersonal.CreatedOn = DateTime.Now;
                                        ConPersonal.ContactFlag = "Referral";
                                        ConPersonal.Status = 1;
                                        DbEntity.ContactPersonals.Add(ConPersonal);
                                        DbEntity.SaveChanges();
                                        model.ContactId = ConPersonal.ContactPersonalId;


                                        AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objRelationLegalGuardian.txtCountry;
                                        AddrList.StateProvince = model.objRelationLegalGuardian.txtState;
                                        AddrList.City = model.objRelationLegalGuardian.txtCity;
                                        AddrList.StreetName = model.objRelationLegalGuardian.txtStreetAdress;
                                        AddrList.ApartmentType = model.objRelationLegalGuardian.txtAprtmentUnit;
                                        AddrList.PostalCode = model.objRelationLegalGuardian.txtZipCode;
                                        AddrList.Phone = model.objRelationLegalGuardian.txtHomePhone;
                                        AddrList.OtherPhone = model.objRelationLegalGuardian.txtWorkPhone;
                                        AddrList.PrimaryEmail = model.objRelationLegalGuardian.txtEmail;
                                        AddrList.AddressType = addressType;
                                        AddrList.CreatedBy = loginId;
                                        AddrList.CreatedOn = DateTime.Now;
                                        DbEntity.AddressLists.Add(AddrList);
                                        DbEntity.SaveChanges();
                                        model.AddressId = AddrList.AddressId;


                                        StdAdrRel.StudentPersonalId = saveId;
                                        StdAdrRel.AddressId = model.AddressId;
                                        StdAdrRel.ContactSequence = 1;
                                        StdAdrRel.ContactPersonalId = model.ContactId;
                                        StdAdrRel.CreatedBy = loginId;
                                        StdAdrRel.CreatedOn = DateTime.Now;
                                        DbEntity.StudentAddresRels.Add(StdAdrRel);
                                        DbEntity.SaveChanges();

                                        StdCntctRel = new StudentContactRelationship();
                                        StdCntctRel.ContactPersonalId = ConPersonal.ContactPersonalId;
                                        StdCntctRel.RelationshipId = model.lukupLegalGurdianId;
                                        StdCntctRel.CreatedBy = loginId;
                                        StdCntctRel.CreatedOn = DateTime.Now;
                                        DbEntity.StudentContactRelationships.Add(StdCntctRel);
                                        DbEntity.SaveChanges();

                                    }
                                }

                            }



                            retVal = saveId;
                        }
                        else
                        {
                            if (type == "General Information")
                            {
                                StudentTbl.FirstName = model.RefFrstName;
                                StudentTbl.LastName = model.RefLstName;
                                StudentTbl.MiddleName = model.RefMaidenName;
                                StudentTbl.SchoolId = schoolId;
                                StudentTbl.Height = model.RefPresentHght;
                                StudentTbl.Weight = model.RefPresntWeigth;
                                StudentTbl.HairColor = model.RefHairColor;
                                StudentTbl.EyeColor = model.RefEyecolor;
                                StudentTbl.LocalId = "STD " + saveId;
                                StudentTbl.Gender = model.RefGender;
                                StudentTbl.PrimaryDiag = model.PrimaryDiag;
                                StudentTbl.SecondaryDiag = model.SecondaryDiag;
                                StudentTbl.AdmissionDate = (model.RefDate == null) ? (DateTime?)null : DateTime.ParseExact(model.RefDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                StudentTbl.BirthDate = (model.RefDOB == null) ? (DateTime?)null : DateTime.ParseExact(model.RefDOB, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                StudentTbl.DistingushingMarks = model.RefAnyIdentification;
                                StudentTbl.RaceId = model.RefRace;
                                StudentTbl.Ethinicity = model.RefEthinicity;
                                StudentTbl.ReligiousAffiliation = model.RefReligiousAffilation;
                                StudentTbl.StudentType = "Referral";
                                StudentTbl.CreatedBy = loginId;
                                StudentTbl.CreatedOn = DateTime.Now;
                                DbEntity.StudentPersonals.Add(StudentTbl);
                                DbEntity.SaveChanges();
                                model.RefPersonalId = StudentTbl.StudentPersonalId;

                                //insert into studentpersonalPA
                                StudPersPa.StudentPersonalId = model.RefPersonalId;
                                StudPersPa.CreatedBy = loginId;
                                StudPersPa.SchoolId = schoolId;
                                StudPersPa.CreatedOn = DateTime.Now;
                                DbEntity.StudentPersonalPAs.Add(StudPersPa);
                                DbEntity.SaveChanges();


                                AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.RefCountry;
                                AddrList.StateProvince = model.RefState;
                                AddrList.City = model.RefCity;
                                AddrList.StreetName = model.RefStreetAdrs;
                                AddrList.ApartmentType = model.RefAptUnit;
                                AddrList.PostalCode = model.RefZipCode;
                                AddrList.CreatedBy = loginId;
                                AddrList.CreatedOn = DateTime.Now;
                                DbEntity.AddressLists.Add(AddrList);
                                DbEntity.SaveChanges();
                                model.AddressId = AddrList.AddressId;

                                StdAdrRel.StudentPersonalId = model.RefPersonalId;
                                StdAdrRel.AddressId = model.AddressId;
                                StdAdrRel.ContactSequence = 0;
                                StdAdrRel.ContactPersonalId = 0;
                                StdAdrRel.CreatedBy = loginId;
                                StdAdrRel.CreatedOn = DateTime.Now;
                                DbEntity.StudentAddresRels.Add(StdAdrRel);
                                DbEntity.SaveChanges();
                                int addressType = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Address Type" && objLookup.LookupName == "Physical location address").Select(objlookup => objlookup.LookupId).Single();
                                if ((model.objRelationMthr.txtFirstName != null && model.objRelationMthr.txtFirstName != "") && (model.objRelationMthr.txtLstName != null && model.objRelationMthr.txtLstName != ""))
                                {




                                    ConPersonal.StudentPersonalId = saveId;
                                    ConPersonal.FirstName = model.objRelationMthr.txtFirstName;
                                    ConPersonal.LastName = model.objRelationMthr.txtLstName;
                                    ConPersonal.MiddleName = model.objRelationMthr.txtMiddleName;
                                   // ConPersonal.Relation = model.objRelationMthr.RelationName;
                                    try
                                    {
                                        if (model.objRelationMthr.txtDob != null)
                                        {
                                            model.objRelationMthr.txtDob = model.objRelationMthr.txtDob.Replace("-", "/");
                                        }
                                        ConPersonal.BirthDate = (model.objRelationMthr.txtDob == null) ? (DateTime?)null : DateTime.ParseExact(model.objRelationMthr.txtDob, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                    }
                                    catch
                                    {

                                    }
                                    ConPersonal.CountryOfCitizenship = model.objRelationMthr.txtUScitzen;
                                    ConPersonal.Employer = model.objRelationMthr.txtEmployer;
                                    ConPersonal.Occupation = model.objRelationMthr.Occupation;
                                    ConPersonal.Education = model.objRelationMthr.txtEducation;
                                    ConPersonal.MaritalStatus = model.objRelationMthr.txtMaritalStatus;
                                    ConPersonal.ContactFlag = "Referral";
                                    ConPersonal.Status = 1;
                                    ConPersonal.GrossIncome = model.objRelationMthr.GrossIncome;
                                    ConPersonal.CauseDeath = model.objRelationMthr.causedeath;
                                    ConPersonal.CreatedBy = loginId;
                                    ConPersonal.CreatedOn = DateTime.Now;
                                    DbEntity.ContactPersonals.Add(ConPersonal);
                                    DbEntity.SaveChanges();
                                    model.ContactId = ConPersonal.ContactPersonalId;


                                    AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objRelationMthr.txtCountry;
                                    AddrList.StateProvince = model.objRelationMthr.txtState;
                                    AddrList.City = model.objRelationMthr.txtCity;
                                    AddrList.StreetName = model.objRelationMthr.txtStreetAdress;
                                    AddrList.ApartmentType = model.objRelationMthr.txtAprtmentUnit;
                                    AddrList.PostalCode = model.objRelationMthr.txtZipCode;
                                    AddrList.Phone = model.objRelationMthr.txtHomePhone;
                                    AddrList.OtherPhone = model.objRelationMthr.txtWorkPhone;
                                    AddrList.Fax = model.objRelationMthr.txtFax;
                                    AddrList.County = model.objRelationMthr.txtCounty;
                                    AddrList.Mobile = model.objRelationMthr.txtMobileNum;
                                    AddrList.PrimaryEmail = model.objRelationMthr.txtEmail;
                                    AddrList.BusinessAddress = model.objRelationMthr.BusinessAddress;
                                    AddrList.AddressType = addressType;
                                    AddrList.CreatedBy = loginId;
                                    AddrList.CreatedOn = DateTime.Now;
                                    DbEntity.AddressLists.Add(AddrList);
                                    DbEntity.SaveChanges();
                                    model.AddressId = AddrList.AddressId;


                                    StdCntctRel.ContactPersonalId = model.ContactId;
                                    StdCntctRel.RelationshipId = model.lukUpMthrId;
                                    StdCntctRel.CreatedBy = loginId;
                                    StdCntctRel.CreatedOn = DateTime.Now;
                                    DbEntity.StudentContactRelationships.Add(StdCntctRel);
                                    DbEntity.SaveChanges();


                                    StdAdrRel.StudentPersonalId = model.RefPersonalId;
                                    StdAdrRel.AddressId = model.AddressId;
                                    StdAdrRel.ContactSequence = 1;
                                    StdAdrRel.ContactPersonalId = model.ContactId;
                                    StdAdrRel.CreatedBy = loginId;
                                    StdAdrRel.CreatedOn = DateTime.Now;
                                    DbEntity.StudentAddresRels.Add(StdAdrRel);
                                    DbEntity.SaveChanges();

                                }

                                if ((model.objRelationFthr.txtFirstName != null && model.objRelationFthr.txtFirstName != "") && (model.objRelationFthr.txtLstName != null && model.objRelationFthr.txtLstName != ""))
                                {
                                    ConPersonal.StudentPersonalId = saveId;
                                    ConPersonal.FirstName = model.objRelationFthr.txtFirstName;
                                    ConPersonal.LastName = model.objRelationFthr.txtLstName;
                                    ConPersonal.MiddleName = model.objRelationFthr.txtMiddleName;
                                   // ConPersonal.Relation = model.objRelationFthr.RelationName;
                                    if (model.objRelationFthr.txtDob != null)
                                    {
                                        model.objRelationFthr.txtDob = model.objRelationFthr.txtDob.Replace("-", "/");
                                    }
                                    ConPersonal.BirthDate = (model.objRelationFthr.txtDob == null) ? (DateTime?)null : DateTime.ParseExact(model.objRelationFthr.txtDob, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                    ConPersonal.CountryOfCitizenship = model.objRelationFthr.txtUScitzen;
                                    ConPersonal.Employer = model.objRelationFthr.txtEmployer;
                                    ConPersonal.Occupation = model.objRelationFthr.Occupation;
                                    ConPersonal.Education = model.objRelationFthr.txtEducation;
                                    ConPersonal.ContactFlag = "Referral";
                                    ConPersonal.Status = 1;
                                    ConPersonal.MaritalStatus = model.objRelationFthr.txtMaritalStatus;
                                    ConPersonal.GrossIncome = model.objRelationFthr.GrossIncome;
                                    ConPersonal.CauseDeath = model.objRelationFthr.causedeath;
                                    ConPersonal.CreatedBy = loginId;
                                    ConPersonal.CreatedOn = DateTime.Now;
                                    DbEntity.ContactPersonals.Add(ConPersonal);
                                    DbEntity.SaveChanges();
                                    model.ContactId = ConPersonal.ContactPersonalId;


                                    AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objRelationFthr.txtCountry;
                                    AddrList.StateProvince = model.objRelationFthr.txtState;
                                    AddrList.City = model.objRelationFthr.txtCity;
                                    AddrList.StreetName = model.objRelationFthr.txtStreetAdress;
                                    AddrList.ApartmentType = model.objRelationFthr.txtAprtmentUnit;
                                    AddrList.PostalCode = model.objRelationFthr.txtZipCode;
                                    AddrList.Phone = model.objRelationFthr.txtHomePhone;
                                    AddrList.OtherPhone = model.objRelationFthr.txtWorkPhone;
                                    AddrList.PrimaryEmail = model.objRelationFthr.txtEmail;
                                    AddrList.County = model.objRelationFthr.txtCounty;
                                    AddrList.BusinessAddress = model.objRelationFthr.BusinessAddress;
                                    AddrList.CreatedBy = loginId;
                                    AddrList.AddressType = addressType;
                                    AddrList.CreatedOn = DateTime.Now;
                                    DbEntity.AddressLists.Add(AddrList);
                                    DbEntity.SaveChanges();
                                    model.AddressId = AddrList.AddressId;


                                    StdCntctRel.ContactPersonalId = model.ContactId;
                                    StdCntctRel.RelationshipId = model.lukUpFathrId;
                                    StdCntctRel.CreatedBy = loginId;
                                    StdCntctRel.CreatedOn = DateTime.Now;
                                    DbEntity.StudentContactRelationships.Add(StdCntctRel);
                                    DbEntity.SaveChanges();


                                    StdAdrRel.StudentPersonalId = model.RefPersonalId;
                                    StdAdrRel.AddressId = model.AddressId;
                                    StdAdrRel.ContactSequence = 1;
                                    StdAdrRel.ContactPersonalId = model.ContactId;
                                    StdAdrRel.CreatedBy = loginId;
                                    StdAdrRel.CreatedOn = DateTime.Now;
                                    DbEntity.StudentAddresRels.Add(StdAdrRel);
                                    DbEntity.SaveChanges();

                                }
                                //


                                retVal = model.RefPersonalId;


                            }
                            else if (type == "General Family Background Information")
                            {
                                retVal = saveId;
                            }

                            else if (type == "Birth And Development History")
                            {
                                retVal = saveId;
                            }
                            else if (type == "Applicant Present Self-Help Skills")
                            {
                                if (saveId > 0)
                                {

                                    var data = DbEntity.BehaviorCategories.ToList();
                                    foreach (var item in data)
                                    {
                                        var dataFromClinet = formdata["behaveNameid_" + item.BehaviorCategryId.ToString()];
                                        int behaviorId = Convert.ToInt32(item.BehaviorCategryId);
                                        BehaviorPa.StudentPersonalId = saveId;
                                        BehaviorPa.SchoolId = schoolId;
                                        BehaviorPa.BehaviorId = behaviorId;
                                        BehaviorPa.Score = Convert.ToInt32(dataFromClinet);
                                        BehaviorPa.CreatedBy = loginId;
                                        BehaviorPa.CreatedOn = DateTime.Now;
                                        DbEntity.BehavioursPAs.Add(BehaviorPa);
                                        DbEntity.SaveChanges();
                                    }
                                }

                                retVal = saveId;

                            }

                            else if (type == "Medical History")
                            {
                                if (saveId > 0)
                                {
                                    int addressType = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Address Type" && objLookup.LookupName == "Physical location address").Select(objlookup => objlookup.LookupId).Single();


                                    Diagnose.StudentPersonalId = saveId;
                                    Diagnose.BloodGroup = model.objPhysicianDetails.RefBloodType;
                                    Diagnose.AllergiesAndReactions = model.objPhysicianDetails.RefAllergiesReaction;
                                    Diagnose.SummaryHealthProblem = model.objPhysicianDetails.RefSummaryCurrentHealth;
                                    Diagnose.CreatedBy = loginId;
                                    Diagnose.CreatedOn = DateTime.Now;
                                    DbEntity.DiaganosesPAs.Add(Diagnose);
                                    DbEntity.SaveChanges();

                                    AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objPhysicianDetails.txtCountry;
                                    AddrList.StateProvince = model.objPhysicianDetails.txtState;
                                    AddrList.City = model.objPhysicianDetails.txtCity;
                                    AddrList.StreetName = model.objPhysicianDetails.txtStreetAdress;
                                    AddrList.PostalCode = model.objPhysicianDetails.txtZipCode;
                                    AddrList.Phone = model.objPhysicianDetails.txtHomePhone;
                                    AddrList.AddressType = addressType;
                                    DbEntity.AddressLists.Add(AddrList);
                                    DbEntity.SaveChanges();
                                    model.AddressId = AddrList.AddressId;

                                    medicalIns.StudentPersonalId = saveId;
                                    medicalIns.AddressId = model.AddressId;
                                    medicalIns.FirstName = model.objPhysicianDetails.RefPrimPhyName;
                                    medicalIns.Speciality = model.objPhysicianDetails.phySpeciality;
                                    if (model.objPhysicianDetails.PhylstApmnt != null)
                                    {
                                        model.objPhysicianDetails.PhylstApmnt = model.objPhysicianDetails.PhylstApmnt.Replace("-", "/");
                                    }
                                    medicalIns.DateOfLastPhysicalExam = model.objPhysicianDetails.PhylstApmnt;
                                    medicalIns.CreatedBy = loginId;
                                    medicalIns.CreatedOn = DateTime.Now;
                                    DbEntity.MedicalAndInsurances.Add(medicalIns);
                                    DbEntity.SaveChanges();

                                    AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single();// model.objInsuranceDetails.txtCountry;
                                    AddrList.StateProvince = model.objInsuranceDetails.txtState;
                                    AddrList.City = model.objInsuranceDetails.txtCity;
                                    AddrList.StreetName = model.objInsuranceDetails.txtStreetAdress;
                                    AddrList.PostalCode = model.objInsuranceDetails.txtZipCode;
                                    DbEntity.AddressLists.Add(AddrList);
                                    DbEntity.SaveChanges();
                                    model.AddressId = AddrList.AddressId;

                                    Insurnce.StudentPersonalId = saveId;
                                    Insurnce.AddressId = model.AddressId;
                                    Insurnce.PreferType = "Primary";
                                    Insurnce.InsuranceType = model.objInsuranceDetails.InsuranceCoverage;
                                    Insurnce.PolicyNumber = model.objInsuranceDetails.InsurancePolNum;
                                    Insurnce.CompanyName = model.objInsuranceDetails.RefInsuranceCompany;
                                    DbEntity.Insurances.Add(Insurnce);
                                    DbEntity.SaveChanges();

                                    AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objInsuranceSecDetails.txtCountry;
                                    AddrList.StateProvince = model.objInsuranceSecDetails.txtState;
                                    AddrList.City = model.objInsuranceSecDetails.txtCity;
                                    AddrList.StreetName = model.objInsuranceSecDetails.txtStreetAdress;
                                    AddrList.PostalCode = model.objInsuranceSecDetails.txtZipCode;
                                    DbEntity.AddressLists.Add(AddrList);
                                    DbEntity.SaveChanges();
                                    model.AddressId = AddrList.AddressId;

                                    Insurnce.StudentPersonalId = saveId;
                                    Insurnce.AddressId = model.AddressId;
                                    Insurnce.PreferType = "Secondary";
                                    Insurnce.InsuranceType = model.objInsuranceSecDetails.InsuranceCoverage;
                                    Insurnce.PolicyNumber = model.objInsuranceSecDetails.InsurancePolNum;
                                    Insurnce.CompanyName = model.objInsuranceSecDetails.RefInsuranceCompany;
                                    DbEntity.Insurances.Add(Insurnce);
                                    DbEntity.SaveChanges();


                                    retVal = saveId;



                                }
                            }

                            else if (type == "Legal Material")
                            {
                                int addressType = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Address Type" && objLookup.LookupName == "Physical location address").Select(objlookup => objlookup.LookupId).Single();

                                if ((model.objRelationLegalGuardian.txtFirstName != null && model.objRelationLegalGuardian.txtFirstName != "") || (model.objRelationLegalGuardian.txtLstName != null && model.objRelationLegalGuardian.txtLstName != ""))
                                {
                                    ConPersonal.StudentPersonalId = saveId;
                                    ConPersonal.FirstName = model.objRelationLegalGuardian.txtFirstName;
                                    ConPersonal.LastName = model.objRelationLegalGuardian.txtLstName;
                                    ConPersonal.MiddleName = model.objRelationLegalGuardian.txtMiddleName;
                                    ConPersonal.ContactFlag = "Referral";
                                    ConPersonal.Status = 1;
                                    ConPersonal.CreatedBy = loginId;
                                    ConPersonal.CreatedOn = DateTime.Now;
                                    DbEntity.ContactPersonals.Add(ConPersonal);
                                    DbEntity.SaveChanges();
                                    model.ContactId = ConPersonal.ContactPersonalId;


                                    AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objRelationLegalGuardian.txtCountry;
                                    AddrList.StateProvince = model.objRelationLegalGuardian.txtState;
                                    AddrList.City = model.objRelationLegalGuardian.txtCity;
                                    AddrList.StreetName = model.objRelationLegalGuardian.txtStreetAdress;
                                    AddrList.ApartmentType = model.objRelationLegalGuardian.txtAprtmentUnit;
                                    AddrList.PostalCode = model.objRelationLegalGuardian.txtZipCode;
                                    AddrList.Phone = model.objRelationLegalGuardian.txtHomePhone;
                                    AddrList.OtherPhone = model.objRelationLegalGuardian.txtWorkPhone;
                                    AddrList.PrimaryEmail = model.objRelationLegalGuardian.txtEmail;
                                    AddrList.AddressType = addressType;
                                    AddrList.CreatedBy = loginId;
                                    AddrList.CreatedOn = DateTime.Now;
                                    DbEntity.AddressLists.Add(AddrList);
                                    DbEntity.SaveChanges();
                                    model.AddressId = AddrList.AddressId;


                                    StdAdrRel.StudentPersonalId = saveId;
                                    StdAdrRel.AddressId = model.AddressId;
                                    StdAdrRel.ContactSequence = 1;
                                    StdAdrRel.ContactPersonalId = model.ContactId;
                                    StdAdrRel.CreatedBy = loginId;
                                    StdAdrRel.CreatedOn = DateTime.Now;
                                    DbEntity.StudentAddresRels.Add(StdAdrRel);
                                    DbEntity.SaveChanges();

                                    StdCntctRel = new StudentContactRelationship();
                                    StdCntctRel.ContactPersonalId = ConPersonal.ContactPersonalId;
                                    StdCntctRel.RelationshipId = model.lukupLegalGurdianId;
                                    StdCntctRel.CreatedBy = loginId;
                                    StdCntctRel.CreatedOn = DateTime.Now;
                                    DbEntity.StudentContactRelationships.Add(StdCntctRel);
                                    DbEntity.SaveChanges();

                                }
                                retVal = saveId;
                            }


                        }
                        trans.Complete();
                    }
                }
                catch
                {

                }
            }
            catch
            {

            }
            return retVal;
        }



        public GenInfoModel EditDataPE(int clientId, string type)
        {

            MelmarkDBEntities DbEntity = new MelmarkDBEntities();
            GenInfoModel model = new GenInfoModel();

            if (type == "General Information")
            {
                if (model != null)
                {
                    model = (from objStudntPersnl in DbEntity.StudentPersonals
                             join objStdtAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals objStdtAddrRel.StudentPersonalId
                             join objAddress in DbEntity.AddressLists on objStdtAddrRel.AddressId equals objAddress.AddressId
                             where (objStudntPersnl.StudentPersonalId == clientId & objStdtAddrRel.ContactSequence == 0)

                             select new GenInfoModel
                             {
                                 RefFrstName = objStudntPersnl.FirstName,
                                 RefLstName = objStudntPersnl.LastName,
                                 RefGender = objStudntPersnl.Gender,
                                 RefMaidenName = objStudntPersnl.MiddleName,
                                 RefPresentHght = objStudntPersnl.Height,
                                 RefPresntWeigth = objStudntPersnl.Weight,
                                 RefEyecolor = objStudntPersnl.EyeColor,
                                 RefHairColor = objStudntPersnl.HairColor,
                                 RefAnyIdentification = objStudntPersnl.DistingushingMarks,
                                 RefRace = objStudntPersnl.RaceId,
                                 RefEthinicity = objStudntPersnl.Ethinicity,
                                 RefReligiousAffilation = objStudntPersnl.ReligiousAffiliation,
                                 RefDateDatetime = objStudntPersnl.AdmissionDate,
                                 RefDOBDateTime = objStudntPersnl.BirthDate,
                                 RefCountry = objAddress.CountryId,
                                 RefState = objAddress.StateProvince,
                                 RefCity = objAddress.City,
                                 RefStreetAdrs = objAddress.StreetName,
                                 RefAptUnit = objAddress.ApartmentType,
                                 RefZipCode = objAddress.PostalCode


                             }).SingleOrDefault();
                    model.RefDate = model.RefDateDatetime == null ? "" : ((DateTime)model.RefDateDatetime).ToString("MM/dd/yyyy").Replace("-", "/");
                    model.RefDOB = model.RefDOBDateTime == null ? "" : ((DateTime)model.RefDOBDateTime).ToString("MM/dd/yyyy").Replace("-", "/");


                    try
                    {
                        model.lukUpFathrId = ((from LookUp lukup in DbEntity.LookUps
                                               where (lukup.LookupName == "Legal Guardian 1")
                                               select new
                                               {
                                                   lukUpFathrId = lukup.LookupId

                                               }).SingleOrDefault()).lukUpFathrId;

                        model.lukUpMthrId = ((from LookUp lukup in DbEntity.LookUps
                                              where (lukup.LookupName == "Legal Guardian 2")
                                              select new
                                              {
                                                  lukUpMthrId = lukup.LookupId

                                              }).SingleOrDefault()).lukUpMthrId;



                    }
                    catch
                    {
                        model.lukUpFathrId = 0;
                        model.lukUpMthrId = 0;
                        model.lukupCloseId = 0;
                        model.lukupLegalGurdianId = 0;
                        model.lukupEmergencyId = 0;
                    }



                    model.objRelationMthr = (from objStudntPersnl in DbEntity.StudentPersonals
                                             join StdAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals StdAddrRel.StudentPersonalId
                                             join ConPersonal in DbEntity.ContactPersonals on StdAddrRel.ContactPersonalId equals ConPersonal.ContactPersonalId
                                             join Addr in DbEntity.AddressLists on StdAddrRel.AddressId equals Addr.AddressId
                                             join StudContactRel in DbEntity.StudentContactRelationships on ConPersonal.ContactPersonalId equals StudContactRel.ContactPersonalId
                                             where (StudContactRel.RelationshipId == model.lukUpMthrId & StdAddrRel.ContactSequence == 1 & objStudntPersnl.StudentPersonalId == clientId)
                                             select new RelationCLs
                                             {
                                                 txtFirstName = ConPersonal.FirstName,
                                                 txtLstName = ConPersonal.LastName,
                                                 txtMiddleName = ConPersonal.MiddleName,
                                               //  RelationName = ConPersonal.Relation,
                                                 txtDobdatetime = ConPersonal.BirthDate,
                                                 txtUScitzen = ConPersonal.CountryOfCitizenship,
                                                 txtEmployer = ConPersonal.Employer,
                                                 txtMaritalStatus = ConPersonal.MaritalStatus,
                                                 causedeath = ConPersonal.CauseDeath,
                                                 Occupation = ConPersonal.Occupation,
                                                 txtCountry = Addr.CountryId,
                                                 txtState = Addr.StateProvince,
                                                 txtStreetAdress = Addr.StreetName,
                                                 txtCity = Addr.City,
                                                 txtAprtmentUnit = Addr.ApartmentType,
                                                 txtZipCode = Addr.PostalCode,
                                                 txtHomePhone = Addr.Phone,
                                                 txtWorkPhone = Addr.OtherPhone,
                                                 txtMobileNum = Addr.Mobile,
                                                 txtFax = Addr.Fax,
                                                 txtEmail = Addr.PrimaryEmail

                                             }).FirstOrDefault();

                    model.objRelationMthr.txtDob = model.objRelationMthr.txtDobdatetime == null ? "" : ((DateTime)model.objRelationMthr.txtDobdatetime).ToString("MM/dd/yyyy").Replace("-", "/");


                    model.objRelationFthr = (from objStudntPersnl in DbEntity.StudentPersonals
                                             join StdAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals StdAddrRel.StudentPersonalId
                                             join ConPersonal in DbEntity.ContactPersonals on StdAddrRel.ContactPersonalId equals ConPersonal.ContactPersonalId
                                             join Addr in DbEntity.AddressLists on StdAddrRel.AddressId equals Addr.AddressId
                                             join StudContactRel in DbEntity.StudentContactRelationships on ConPersonal.ContactPersonalId equals StudContactRel.ContactPersonalId
                                             where (StudContactRel.RelationshipId == model.lukUpFathrId & StdAddrRel.ContactSequence == 1 & objStudntPersnl.StudentPersonalId == clientId)
                                             select new RelationCLs
                                             {
                                                 txtFirstName = ConPersonal.FirstName,
                                                 txtLstName = ConPersonal.LastName,
                                                 txtMiddleName = ConPersonal.MiddleName,
                                                 //RelationName = ConPersonal.Relation,
                                                 txtDobdatetime = ConPersonal.BirthDate,
                                                 txtUScitzen = ConPersonal.CountryOfCitizenship,
                                                 txtEmployer = ConPersonal.Employer,
                                                 txtMaritalStatus = ConPersonal.MaritalStatus,
                                                 causedeath = ConPersonal.CauseDeath,
                                                 Occupation = ConPersonal.Occupation,
                                                 txtCountry = Addr.CountryId,
                                                 txtState = Addr.StateProvince,
                                                 txtStreetAdress = Addr.StreetName,
                                                 txtCity = Addr.City,
                                                 txtAprtmentUnit = Addr.ApartmentType,
                                                 txtZipCode = Addr.PostalCode,
                                                 txtHomePhone = Addr.Phone,
                                                 txtWorkPhone = Addr.OtherPhone,
                                                 txtMobileNum = Addr.Mobile,
                                                 txtFax = Addr.Fax,
                                                 txtEmail = Addr.PrimaryEmail
                                             }).FirstOrDefault();



                    model.objRelationFthr.txtDob = model.objRelationFthr.txtDobdatetime == null ? "" : ((DateTime)model.objRelationFthr.txtDobdatetime).ToString("MM/dd/yyyy").Replace("-", "/");



                }
            }
            else if (type == "General Family Background Information")
            {

                //if (model != null)
                //{
                //    model.lukUpFathrId = ((from LookUp lukup in DbEntity.LookUps
                //                           where (lukup.LookupName == "Father - step")
                //                           select new
                //                           {
                //                               lukUpFathrId = lukup.LookupId

                //                           }).SingleOrDefault()).lukUpFathrId;

                //    model.lukUpMthrId = ((from LookUp lukup in DbEntity.LookUps
                //                          where (lukup.LookupName == "Mother - foster")
                //                          select new
                //                          {
                //                              lukUpMthrId = lukup.LookupId

                //                          }).SingleOrDefault()).lukUpMthrId;


                //    model.lukupCloseId = ((from LookUp lukup in DbEntity.LookUps
                //                           where (lukup.LookupName == "Cousin")
                //                           select new
                //                           {
                //                               lukupCloseId = lukup.LookupId


                //                           }).SingleOrDefault()).lukupCloseId;


                //    model.objRelationMthr = (from objStudntPersnl in DbEntity.StudentPersonals
                //                             join StdAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals StdAddrRel.StudentPersonalId
                //                             join ConPersonal in DbEntity.ContactPersonals on StdAddrRel.ContactPersonalId equals ConPersonal.ContactPersonalId
                //                             join Addr in DbEntity.AddressLists on StdAddrRel.AddressId equals Addr.AddressId
                //                             join StudContactRel in DbEntity.StudentContactRelationships on ConPersonal.ContactPersonalId equals StudContactRel.ContactPersonalId
                //                             where (StudContactRel.RelationshipId == model.lukUpMthrId & StdAddrRel.ContactSequence == 1 & objStudntPersnl.StudentPersonalId == clientId)
                //                             select new RelationCLs
                //                             {
                //                                 txtFirstName = ConPersonal.FirstName,
                //                                 txtLstName = ConPersonal.LastName,
                //                                 txtDobdatetime = ConPersonal.BirthDate,
                //                                 txtUScitzen = ConPersonal.CountryOfCitizenship,
                //                                 txtEmployer = ConPersonal.Employer,
                //                                 txtMaritalStatus = ConPersonal.MaritalStatus,
                //                                 Occupation = ConPersonal.Occupation

                //                             }).FirstOrDefault();

                //    model.objRelationMthr.txtDob = model.objRelationMthr.txtDobdatetime == null ? "" : ((DateTime)model.objRelationMthr.txtDobdatetime).ToString("MM/dd/yyyy");



                //    model.objRelationFthr = (from objStudntPersnl in DbEntity.StudentPersonals
                //                             join StdAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals StdAddrRel.StudentPersonalId
                //                             join ConPersonal in DbEntity.ContactPersonals on StdAddrRel.ContactPersonalId equals ConPersonal.ContactPersonalId
                //                             join Addr in DbEntity.AddressLists on StdAddrRel.AddressId equals Addr.AddressId
                //                             join StudContactRel in DbEntity.StudentContactRelationships on ConPersonal.ContactPersonalId equals StudContactRel.ContactPersonalId
                //                             where (StudContactRel.RelationshipId == model.lukUpFathrId & StdAddrRel.ContactSequence == 1 & objStudntPersnl.StudentPersonalId == clientId)
                //                             select new RelationCLs
                //                             {
                //                                 txtFirstName = ConPersonal.FirstName,
                //                                 txtLstName = ConPersonal.LastName,
                //                                 txtDobdatetime = ConPersonal.BirthDate,
                //                                 txtUScitzen = ConPersonal.CountryOfCitizenship,
                //                                 txtEmployer = ConPersonal.Employer,
                //                                 Occupation = ConPersonal.Occupation

                //                             }).FirstOrDefault();

                //    model.objRelationFthr.txtDob = model.objRelationFthr.txtDobdatetime == null ? "" : ((DateTime)model.objRelationFthr.txtDobdatetime).ToString("MM/dd/yyyy");

                //}

            }

            else if (type == "Birth And Development History")
            {
                //if (model != null)
                //{

                //    model = (from objStudPersnl in DbEntity.StudentPersonals
                //             where (objStudPersnl.StudentPersonalId == clientId)
                //             select new GenInfoModel
                //             {
                //                 RefbirthPlace = objStudPersnl.PlaceOfBirth,
                //                 RefCntryBirth = objStudPersnl.CountryOfBirth,
                //                 RefStateBirth = objStudPersnl.StateOfBirth

                //             }).SingleOrDefault();

                //    model.RefDiagnosis = (from DiagPa in DbEntity.DiaganosesPAs
                //                          where (DiagPa.StudentPersonalId == clientId)
                //                          select new GenInfoModel
                //                          {
                //                              RefDiagnosis = DiagPa.Diaganoses

                //                          }).SingleOrDefault().RefDiagnosis;

                //    model.RefAllergies = (from StdPersPa in DbEntity.StudentPersonalPAs
                //                          where (StdPersPa.StudentPersonalId == clientId)
                //                          select new GenInfoModel
                //                          {
                //                              RefAllergies = StdPersPa.Allergies

                //                          }).SingleOrDefault().RefAllergies;

                //    model.objPhysicianDetails = (from mediclInsur in DbEntity.MedicalAndInsurances
                //                                 join Addr in DbEntity.AddressLists on mediclInsur.AddressId equals Addr.AddressId
                //                                 where (mediclInsur.StudentPersonalId == clientId)
                //                                 select new RelationCLs
                //                                 {
                //                                     RefPrimPhyName = mediclInsur.FirstName,
                //                                     txtCountry = Addr.CountryId,
                //                                     txtState = Addr.StateProvince,
                //                                     txtCity = Addr.City,
                //                                     txtStreetAdress = Addr.StreetName,
                //                                     txtZipCode = Addr.PostalCode,
                //                                     txtHomePhone = Addr.Phone,
                //                                     PhylstApmntdateTime = mediclInsur.DateOfLastPhysicalExam

                //                                 }).FirstOrDefault();

                //    model.objPhysicianDetails.PhylstApmnt = model.objPhysicianDetails.PhylstApmntdateTime == null ? "" : ((DateTime)model.objPhysicianDetails.PhylstApmntdateTime).ToString("MM/dd/yyyy");

                //    model.objInsuranceDetails = (from Insurnce in DbEntity.Insurances
                //                                 join Addr in DbEntity.AddressLists on Insurnce.AddressId equals Addr.AddressId
                //                                 where (Insurnce.StudentPersonalId == clientId & Insurnce.PreferType == "Primary")
                //                                 select new RelationCLs
                //                                 {
                //                                     RefInsuranceCompany = Insurnce.CompanyName,
                //                                     txtCountry = Addr.CountryId,
                //                                     txtState = Addr.StateProvince,
                //                                     txtCity = Addr.City,
                //                                     txtStreetAdress = Addr.StreetName,
                //                                     txtZipCode = Addr.PostalCode,
                //                                     InsuranceCoverage = Insurnce.InsuranceType,
                //                                     InsurancePolNum = Insurnce.PolicyNumber

                //                                 }).FirstOrDefault();

                //    model.objInsuranceSecDetails = (from Insurnce in DbEntity.Insurances
                //                                    join Addr in DbEntity.AddressLists on Insurnce.AddressId equals Addr.AddressId
                //                                    where (Insurnce.StudentPersonalId == clientId & Insurnce.PreferType == "Secondary")
                //                                    select new RelationCLs
                //                                    {
                //                                        RefInsuranceCompany = Insurnce.CompanyName,
                //                                        txtCountry = Addr.CountryId,
                //                                        txtState = Addr.StateProvince,
                //                                        txtCity = Addr.City,
                //                                        txtStreetAdress = Addr.StreetName,
                //                                        txtZipCode = Addr.PostalCode,
                //                                        InsuranceCoverage = Insurnce.InsuranceType,
                //                                        InsurancePolNum = Insurnce.PolicyNumber

                //                                    }).FirstOrDefault();

                //    model.objInsuranceDentalDetails = (from Insurnce in DbEntity.Insurances
                //                                       join Addr in DbEntity.AddressLists on Insurnce.AddressId equals Addr.AddressId
                //                                       where (Insurnce.StudentPersonalId == clientId & Insurnce.PreferType == "Dental")
                //                                       select new RelationCLs
                //                                       {
                //                                           RefInsuranceCompany = Insurnce.CompanyName,
                //                                           txtCountry = Addr.CountryId,
                //                                           txtState = Addr.StateProvince,
                //                                           txtCity = Addr.City,
                //                                           txtStreetAdress = Addr.StreetName,
                //                                           txtZipCode = Addr.PostalCode,
                //                                           InsuranceCoverage = Insurnce.InsuranceType,
                //                                           InsurancePolNum = Insurnce.PolicyNumber

                //                                       }).FirstOrDefault();




                //}

            }

            else if (type == "Applicant Present Self-Help Skills")
            {
                //if (model != null)
                //{
                //    model = (from MediclIns in DbEntity.MedicalAndInsurances
                //             where (MediclIns.StudentPersonalId == clientId)
                //             select new GenInfoModel
                //             {
                //                 RefSpecificProb = MediclIns.SignificantBehaviorCharacteristics

                //             }).FirstOrDefault();

                //}
            }

            else if (type == "Medical History")
            {
                if (model != null)
                {
                    model.objPhysicianDetails = (from mediclInsur in DbEntity.MedicalAndInsurances
                                                 join Diagn in DbEntity.DiaganosesPAs on mediclInsur.StudentPersonalId equals Diagn.StudentPersonalId
                                                 join Addr in DbEntity.AddressLists on mediclInsur.AddressId equals Addr.AddressId
                                                 where (mediclInsur.StudentPersonalId == clientId)
                                                 select new RelationCLs
                                                 {
                                                     RefBloodType = Diagn.BloodGroup,
                                                     RefAllergiesReaction = Diagn.AllergiesAndReactions,
                                                     RefSummaryCurrentHealth = Diagn.SummaryHealthProblem,
                                                     RefPrimPhyName = mediclInsur.FirstName,
                                                     phySpeciality = mediclInsur.Speciality,
                                                     txtCountry = Addr.CountryId,
                                                     txtState = Addr.StateProvince,
                                                     txtCity = Addr.City,
                                                     txtStreetAdress = Addr.StreetName,
                                                     txtZipCode = Addr.PostalCode,
                                                     txtHomePhone = Addr.Phone,
                                                     PhylstApmntdateTime = mediclInsur.DateOfLastPhysicalExam

                                                 }).FirstOrDefault();


                    model.objPhysicianDetails.PhylstApmnt = model.objPhysicianDetails.PhylstApmntdateTime;


                }

                model.objInsuranceDetails = (from Insurnce in DbEntity.Insurances
                                             join Addr in DbEntity.AddressLists on Insurnce.AddressId equals Addr.AddressId
                                             where (Insurnce.StudentPersonalId == clientId & Insurnce.PreferType == "Primary")
                                             select new RelationCLs
                                             {
                                                 RefInsuranceCompany = Insurnce.CompanyName,
                                                 txtCountry = Addr.CountryId,
                                                 txtState = Addr.StateProvince,
                                                 txtCity = Addr.City,
                                                 txtStreetAdress = Addr.StreetName,
                                                 txtZipCode = Addr.PostalCode,
                                                 InsuranceCoverage = Insurnce.InsuranceType,
                                                 InsurancePolNum = Insurnce.PolicyNumber

                                             }).FirstOrDefault();

                model.objInsuranceSecDetails = (from Insurnce in DbEntity.Insurances
                                                join Addr in DbEntity.AddressLists on Insurnce.AddressId equals Addr.AddressId
                                                where (Insurnce.StudentPersonalId == clientId & Insurnce.PreferType == "Secondary")
                                                select new RelationCLs
                                                {
                                                    RefInsuranceCompany = Insurnce.CompanyName,
                                                    txtCountry = Addr.CountryId,
                                                    txtState = Addr.StateProvince,
                                                    txtCity = Addr.City,
                                                    txtStreetAdress = Addr.StreetName,
                                                    txtZipCode = Addr.PostalCode,
                                                    InsuranceCoverage = Insurnce.InsuranceType,
                                                    InsurancePolNum = Insurnce.PolicyNumber

                                                }).FirstOrDefault();

            }

            else if (type == "Legal Material")
            {
                if (model != null)
                {
                    model.lukupLegalGurdianId = ((from LookUp lukup in DbEntity.LookUps
                                                  where (lukup.LookupName == "Legal Guardian")
                                                  select new
                                                  {
                                                      lukupCloseId = lukup.LookupId


                                                  }).SingleOrDefault()).lukupCloseId;



                    model.objRelationLegalGuardian = (from objStudntPersnl in DbEntity.StudentPersonals
                                                      join StdAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals StdAddrRel.StudentPersonalId
                                                      join ConPersonal in DbEntity.ContactPersonals on StdAddrRel.ContactPersonalId equals ConPersonal.ContactPersonalId
                                                      join Addr in DbEntity.AddressLists on StdAddrRel.AddressId equals Addr.AddressId
                                                      join StudContactRel in DbEntity.StudentContactRelationships on ConPersonal.ContactPersonalId equals StudContactRel.ContactPersonalId
                                                      where (StudContactRel.RelationshipId == model.lukupLegalGurdianId & StdAddrRel.ContactSequence == 1 & objStudntPersnl.StudentPersonalId == clientId)
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

                }
            }

            else if (type == "Upload Doccuments")
            {
                model.objclsUpld.DocName = "";
                model.objclsUpld.DocType = 0;


            }


            return model;
        }



        public int SaveGeneralDataPEOld(GenInfoModel model, string type, int saveId, string status, int loginId, int schoolId, FormCollection formdata)
        {
            int retVal = 0;

            try
            {
                MelmarkDBEntities DbEntity = new MelmarkDBEntities();
                StudentPersonal StudentTbl = new StudentPersonal();
                AddressList AddrList = new AddressList();
                StudentAddresRel StdAdrRel = new StudentAddresRel();
                ContactPersonal ConPersonal = new ContactPersonal();
                LookUp LukUp = new LookUp();
                StudentContactRelationship StdCntctRel = new StudentContactRelationship();
                DiaganosesPA Diagnose = new DiaganosesPA();
                MedicalAndInsurance medicalIns = new MedicalAndInsurance();
                StudentPersonalPA StudPersPa = new StudentPersonalPA();
                Insurance Insurnce = new Insurance();
                BehavioursPA BehaviorPa = new BehavioursPA();

                try
                {

                    try
                    {
                        model.lukUpFathrId = ((from LookUp lukup in DbEntity.LookUps
                                               where (lukup.LookupName == "Legal Guardian 2")
                                               select new
                                               {
                                                   lukUpFathrId = lukup.LookupId

                                               }).SingleOrDefault()).lukUpFathrId;

                        model.lukUpMthrId = ((from LookUp lukup in DbEntity.LookUps
                                              where (lukup.LookupName == "Legal Guardian 1")
                                              select new
                                              {
                                                  lukUpMthrId = lukup.LookupId


                                              }).SingleOrDefault()).lukUpMthrId;

                        model.lukupCloseId = ((from LookUp lukup in DbEntity.LookUps
                                               where (lukup.LookupName == "None")
                                               select new
                                               {
                                                   lukupCloseId = lukup.LookupId


                                               }).SingleOrDefault()).lukupCloseId;

                        model.lukupLegalGurdianId = ((from LookUp lukup in DbEntity.LookUps
                                                      where (lukup.LookupName == "Legal Guardian")
                                                      select new
                                                      {
                                                          lukupCloseId = lukup.LookupId


                                                      }).SingleOrDefault()).lukupCloseId;


                        model.lukupEmergencyId = ((from LookUp lukup in DbEntity.LookUps
                                                   where (lukup.LookupName == "Emergency Contact")
                                                   select new
                                                   {
                                                       lukupCloseId = lukup.LookupId


                                                   }).SingleOrDefault()).lukupCloseId;


                    }
                    catch
                    {
                        model.lukUpFathrId = 0;
                        model.lukUpMthrId = 0;
                    }



                    using (TransactionScope trans = new TransactionScope(TransactionScopeOption.Required))
                    {

                        if ((saveId > 0) && (status == "update"))
                        {

                            if (type == "General Information")
                            {

                                int RefaddrId = (from objStudntPersnl in DbEntity.StudentPersonals
                                                 join objStdtAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals objStdtAddrRel.StudentPersonalId
                                                 join objAddress in DbEntity.AddressLists on objStdtAddrRel.AddressId equals objAddress.AddressId
                                                 where (objStudntPersnl.StudentPersonalId == saveId & objStdtAddrRel.ContactSequence == 0)

                                                 select new
                                                 {
                                                     AdressId = objAddress.AddressId

                                                 }).SingleOrDefault().AdressId;

                                int mothrAdressId = 0;
                                var mothrAdress = (from objStudntPersnl in DbEntity.StudentPersonals
                                                   join StdAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals StdAddrRel.StudentPersonalId
                                                   join ConPrsonal in DbEntity.ContactPersonals on StdAddrRel.ContactPersonalId equals ConPrsonal.ContactPersonalId
                                                   join Addr in DbEntity.AddressLists on StdAddrRel.AddressId equals Addr.AddressId
                                                   join StudContactRel in DbEntity.StudentContactRelationships on ConPrsonal.ContactPersonalId equals StudContactRel.ContactPersonalId
                                                   where (StudContactRel.RelationshipId == model.lukUpMthrId & StdAddrRel.ContactSequence == 1 & objStudntPersnl.StudentPersonalId == saveId)
                                                   select new
                                                   {
                                                       AdresId = Addr.AddressId
                                                   }).ToList();
                                if (mothrAdress.Count > 0)
                                {
                                    foreach (var item in mothrAdress)
                                    {
                                        mothrAdressId = item.AdresId;
                                    }
                                }


                                int mothrConPersId = 0;
                                var motherConPers = (from objStudntPersnl in DbEntity.StudentPersonals
                                                     join StdAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals StdAddrRel.StudentPersonalId
                                                     join ConPrsonal in DbEntity.ContactPersonals on StdAddrRel.ContactPersonalId equals ConPrsonal.ContactPersonalId
                                                     join Addr in DbEntity.AddressLists on StdAddrRel.AddressId equals Addr.AddressId
                                                     join StudContactRel in DbEntity.StudentContactRelationships on ConPrsonal.ContactPersonalId equals StudContactRel.ContactPersonalId
                                                     where (StudContactRel.RelationshipId == model.lukUpMthrId & StdAddrRel.ContactSequence == 1 & objStudntPersnl.StudentPersonalId == saveId)
                                                     select new
                                                     {
                                                         ContactId = ConPrsonal.ContactPersonalId
                                                     }).ToList();
                                if (motherConPers.Count > 0)
                                {
                                    foreach (var item in motherConPers)
                                    {
                                        mothrConPersId = item.ContactId;
                                    }
                                }

                                int FthrAdressId = 0;
                                var FthrAdress = (from objStudntPersnl in DbEntity.StudentPersonals
                                                  join StdAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals StdAddrRel.StudentPersonalId
                                                  join ConPrsonal in DbEntity.ContactPersonals on StdAddrRel.ContactPersonalId equals ConPrsonal.ContactPersonalId
                                                  join Addr in DbEntity.AddressLists on StdAddrRel.AddressId equals Addr.AddressId
                                                  join StudContactRel in DbEntity.StudentContactRelationships on ConPrsonal.ContactPersonalId equals StudContactRel.ContactPersonalId
                                                  where (StudContactRel.RelationshipId == model.lukUpFathrId & StdAddrRel.ContactSequence == 1 & objStudntPersnl.StudentPersonalId == saveId)
                                                  select new
                                                  {
                                                      AdresId = Addr.AddressId
                                                  }).ToList();

                                if (FthrAdress.Count > 0)
                                {
                                    foreach (var item in FthrAdress)
                                    {
                                        FthrAdressId = item.AdresId;
                                    }
                                }

                                int FthrConPersId = 0;
                                var FthrConPers = (from objStudntPersnl in DbEntity.StudentPersonals
                                                   join StdAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals StdAddrRel.StudentPersonalId
                                                   join ConPrsonal in DbEntity.ContactPersonals on StdAddrRel.ContactPersonalId equals ConPrsonal.ContactPersonalId
                                                   join Addr in DbEntity.AddressLists on StdAddrRel.AddressId equals Addr.AddressId
                                                   join StudContactRel in DbEntity.StudentContactRelationships on ConPrsonal.ContactPersonalId equals StudContactRel.ContactPersonalId
                                                   where (StudContactRel.RelationshipId == model.lukUpFathrId & StdAddrRel.ContactSequence == 1 & objStudntPersnl.StudentPersonalId == saveId)
                                                   select new
                                                   {
                                                       ContactId = ConPrsonal.ContactPersonalId
                                                   }).ToList();

                                if (FthrConPers.Count > 0)
                                {
                                    foreach (var item in FthrConPers)
                                    {
                                        FthrConPersId = item.ContactId;
                                    }
                                }
                                int insAdressId = (from Insrnce in DbEntity.Insurances
                                                   join Addr in DbEntity.AddressLists on Insrnce.AddressId equals Addr.AddressId
                                                   where (Insrnce.StudentPersonalId == saveId & Insrnce.PreferType == "Primary")
                                                   select new
                                                   {
                                                       AdresId = Addr.AddressId

                                                   }).SingleOrDefault().AdresId;

                                int insSecAdressId = (from Insrnce in DbEntity.Insurances
                                                      join Addr in DbEntity.AddressLists on Insrnce.AddressId equals Addr.AddressId
                                                      where (Insrnce.StudentPersonalId == saveId & Insrnce.PreferType == "Secondary")
                                                      select new
                                                      {
                                                          AdresId = Addr.AddressId

                                                      }).SingleOrDefault().AdresId;




                                StudentTbl = DbEntity.StudentPersonals.Where(objStudent => objStudent.StudentPersonalId == saveId).SingleOrDefault();
                                StudentTbl.FirstName = model.RefFrstName;
                                StudentTbl.LastName = model.RefLstName;
                                StudentTbl.MiddleName = model.RefMaidenName;
                                StudentTbl.Height = model.RefPresentHght;
                                StudentTbl.Weight = model.RefPresntWeigth;
                                StudentTbl.HairColor = model.RefHairColor;
                                StudentTbl.EyeColor = model.RefEyecolor;
                                StudentTbl.LocalId = "STD " + saveId;
                                StudentTbl.Gender = model.RefGender;
                                StudentTbl.AdmissionDate = (model.RefDate == null) ? (DateTime?)null : DateTime.ParseExact(model.RefDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                StudentTbl.BirthDate = (model.RefDOB == null) ? (DateTime?)null : DateTime.ParseExact(model.RefDOB, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                StudentTbl.DistingushingMarks = model.RefAnyIdentification;
                                StudentTbl.RaceId = model.RefRace;
                                StudentTbl.Ethinicity = model.RefEthinicity;
                                StudentTbl.ReligiousAffiliation = model.RefReligiousAffilation;
                                StudentTbl.ModifiedBy = loginId;
                                StudentTbl.CreatedOn = DateTime.Now;
                                DbEntity.SaveChanges();


                                AddrList = DbEntity.AddressLists.Where(objAdr => objAdr.AddressId == RefaddrId).SingleOrDefault();
                                AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.RefCountry;
                                AddrList.StateProvince = model.RefState;
                                AddrList.City = model.RefCity;
                                AddrList.StreetName = model.RefStreetAdrs;
                                AddrList.ApartmentType = model.RefAptUnit;
                                AddrList.PostalCode = model.RefZipCode;
                                DbEntity.SaveChanges();

                                ConPersonal = DbEntity.ContactPersonals.Where(objCont => objCont.ContactPersonalId == mothrConPersId).SingleOrDefault();
                                if (ConPersonal != null)
                                {
                                    ConPersonal.FirstName = model.objRelationMthr.txtFirstName;
                                    ConPersonal.MiddleName = model.objRelationMthr.txtMiddleName;
                                    ConPersonal.LastName = model.objRelationMthr.txtLstName;
                                   // ConPersonal.Relation = model.objRelationMthr.RelationName;

                                    if (model.objRelationMthr.txtDob != null)
                                    {
                                        model.objRelationMthr.txtDob = model.objRelationMthr.txtDob.Replace("-", "/");
                                    }

                                    ConPersonal.BirthDate = (model.objRelationMthr.txtDob == null) ? (DateTime?)null : DateTime.ParseExact(model.objRelationMthr.txtDob, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                    ConPersonal.CountryOfCitizenship = model.objRelationMthr.txtUScitzen;
                                    ConPersonal.Employer = model.objRelationMthr.txtEmployer;
                                    ConPersonal.Occupation = model.objRelationMthr.Occupation;
                                    ConPersonal.Education = model.objRelationMthr.txtEducation;
                                    ConPersonal.MaritalStatus = model.objRelationMthr.txtMaritalStatus;
                                    ConPersonal.GrossIncome = model.objRelationMthr.GrossIncome;
                                    ConPersonal.CauseDeath = model.objRelationMthr.causedeath;
                                    ConPersonal.ContactFlag = "Referral";
                                    ConPersonal.Status = 1;
                                    DbEntity.SaveChanges();


                                    AddrList = DbEntity.AddressLists.Where(objAdr => objAdr.AddressId == mothrAdressId).SingleOrDefault();
                                    AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objRelationMthr.txtCountry;
                                    AddrList.StateProvince = model.objRelationMthr.txtState;
                                    AddrList.City = model.objRelationMthr.txtCity;
                                    AddrList.StreetName = model.objRelationMthr.txtStreetAdress;
                                    AddrList.ApartmentType = model.objRelationMthr.txtAprtmentUnit;
                                    AddrList.PostalCode = model.objRelationMthr.txtZipCode;
                                    AddrList.Phone = model.objRelationMthr.txtHomePhone;
                                    AddrList.Mobile = model.objRelationMthr.txtMobileNum;
                                    AddrList.Fax = model.objRelationMthr.txtFax;
                                    AddrList.OtherPhone = model.objRelationMthr.txtWorkPhone;
                                    AddrList.PrimaryEmail = model.objRelationMthr.txtEmail;
                                    AddrList.BusinessAddress = model.objRelationMthr.BusinessAddress;
                                    DbEntity.SaveChanges();
                                }
                                else
                                {
                                    if ((model.objRelationMthr.txtFirstName != null && model.objRelationMthr.txtFirstName != "") || (model.objRelationMthr.txtLstName != null && model.objRelationMthr.txtLstName != ""))
                                    {

                                        int addressType = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Address Type" && objLookup.LookupName == "Physical location address").Select(objlookup => objlookup.LookupId).Single();

                                        ConPersonal = new ContactPersonal();
                                        ConPersonal.FirstName = model.objRelationMthr.txtFirstName;
                                        ConPersonal.MiddleName = model.objRelationMthr.txtMiddleName;
                                        ConPersonal.LastName = model.objRelationMthr.txtLstName;
                                       // ConPersonal.Relation = model.objRelationMthr.RelationName;

                                        if (model.objRelationMthr.txtDob != null)
                                        {
                                            model.objRelationMthr.txtDob = model.objRelationMthr.txtDob.Replace("-", "/");
                                        }

                                        ConPersonal.BirthDate = (model.objRelationMthr.txtDob == null) ? (DateTime?)null : DateTime.ParseExact(model.objRelationMthr.txtDob, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                        ConPersonal.CountryOfCitizenship = model.objRelationMthr.txtUScitzen;
                                        ConPersonal.Employer = model.objRelationMthr.txtEmployer;
                                        ConPersonal.Occupation = model.objRelationMthr.Occupation;
                                        ConPersonal.Education = model.objRelationMthr.txtEducation;
                                        ConPersonal.MaritalStatus = model.objRelationMthr.txtMaritalStatus;
                                        ConPersonal.GrossIncome = model.objRelationMthr.GrossIncome;
                                        ConPersonal.CauseDeath = model.objRelationMthr.causedeath;
                                        ConPersonal.ContactFlag = "Referral";
                                        ConPersonal.Status = 1;
                                        ConPersonal.CreatedBy = loginId;
                                        ConPersonal.CreatedOn = DateTime.Now;
                                        DbEntity.ContactPersonals.Add(ConPersonal);
                                        DbEntity.SaveChanges();


                                        AddrList = new AddressList();
                                        AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objRelationMthr.txtCountry;
                                        AddrList.StateProvince = model.objRelationMthr.txtState;
                                        AddrList.City = model.objRelationMthr.txtCity;
                                        AddrList.StreetName = model.objRelationMthr.txtStreetAdress;
                                        AddrList.ApartmentType = model.objRelationMthr.txtAprtmentUnit;
                                        AddrList.PostalCode = model.objRelationMthr.txtZipCode;
                                        AddrList.Phone = model.objRelationMthr.txtHomePhone;
                                        AddrList.Mobile = model.objRelationMthr.txtMobileNum;
                                        AddrList.Fax = model.objRelationMthr.txtFax;
                                        AddrList.OtherPhone = model.objRelationMthr.txtWorkPhone;
                                        AddrList.AddressType = addressType;
                                        AddrList.PrimaryEmail = model.objRelationMthr.txtEmail;
                                        AddrList.BusinessAddress = model.objRelationMthr.BusinessAddress;
                                        AddrList.CreatedBy = loginId;
                                        AddrList.CreatedOn = DateTime.Now;
                                        DbEntity.AddressLists.Add(AddrList);
                                        DbEntity.SaveChanges();

                                        StdCntctRel = new StudentContactRelationship();
                                        StdCntctRel.ContactPersonalId = ConPersonal.ContactPersonalId;
                                        StdCntctRel.RelationshipId = model.lukUpMthrId;
                                        StdCntctRel.CreatedBy = loginId;
                                        StdCntctRel.CreatedOn = DateTime.Now;
                                        DbEntity.StudentContactRelationships.Add(StdCntctRel);
                                        DbEntity.SaveChanges();

                                        StdAdrRel = new StudentAddresRel();
                                        StdAdrRel.StudentPersonalId = saveId;
                                        StdAdrRel.AddressId = AddrList.AddressId;
                                        StdAdrRel.ContactSequence = 1;
                                        StdAdrRel.ContactPersonalId = ConPersonal.ContactPersonalId;
                                        StdAdrRel.CreatedBy = loginId;
                                        StdAdrRel.CreatedOn = DateTime.Now;
                                        DbEntity.StudentAddresRels.Add(StdAdrRel);
                                        DbEntity.SaveChanges();
                                    }

                                }

                                ConPersonal = DbEntity.ContactPersonals.Where(objCont => objCont.ContactPersonalId == FthrConPersId).SingleOrDefault();
                                if (ConPersonal != null)
                                {

                                    ConPersonal.FirstName = model.objRelationFthr.txtFirstName;
                                    ConPersonal.MiddleName = model.objRelationFthr.txtMiddleName;
                                    ConPersonal.LastName = model.objRelationFthr.txtLstName;
                                    //ConPersonal.Relation = model.objRelationFthr.RelationName;
                                    if (model.objRelationFthr.txtDob != null)
                                    {
                                        model.objRelationFthr.txtDob = model.objRelationFthr.txtDob.Replace("-", "/");
                                    }
                                    ConPersonal.BirthDate = (model.objRelationFthr.txtDob == null) ? (DateTime?)null : DateTime.ParseExact(model.objRelationFthr.txtDob, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                    ConPersonal.CountryOfCitizenship = model.objRelationFthr.txtUScitzen;
                                    ConPersonal.Employer = model.objRelationFthr.txtEmployer;
                                    ConPersonal.Occupation = model.objRelationFthr.Occupation;
                                    ConPersonal.Education = model.objRelationFthr.txtEducation;
                                    ConPersonal.MaritalStatus = model.objRelationFthr.txtMaritalStatus;
                                    ConPersonal.GrossIncome = model.objRelationFthr.GrossIncome;
                                    ConPersonal.CauseDeath = model.objRelationFthr.causedeath;
                                    ConPersonal.ContactFlag = "Referral";
                                    ConPersonal.Status = 1;
                                    DbEntity.SaveChanges();

                                    AddrList = DbEntity.AddressLists.Where(objAdr => objAdr.AddressId == FthrAdressId).SingleOrDefault();
                                    AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objRelationFthr.txtCountry;
                                    AddrList.StateProvince = model.objRelationFthr.txtState;
                                    AddrList.City = model.objRelationFthr.txtCity;
                                    AddrList.StreetName = model.objRelationFthr.txtStreetAdress;
                                    AddrList.ApartmentType = model.objRelationFthr.txtAprtmentUnit;
                                    AddrList.PostalCode = model.objRelationFthr.txtZipCode;
                                    AddrList.Phone = model.objRelationFthr.txtHomePhone;
                                    AddrList.OtherPhone = model.objRelationFthr.txtWorkPhone;
                                    AddrList.Mobile = model.objRelationFthr.txtMobileNum;
                                    AddrList.Fax = model.objRelationFthr.txtFax;
                                    AddrList.PrimaryEmail = model.objRelationFthr.txtEmail;
                                    AddrList.BusinessAddress = model.objRelationFthr.BusinessAddress;
                                    DbEntity.SaveChanges();

                                }
                                else
                                {
                                    if ((model.objRelationFthr.txtFirstName != null && model.objRelationFthr.txtFirstName != "") || (model.objRelationFthr.txtLstName != null && model.objRelationFthr.txtLstName != ""))
                                    {

                                        int addressType = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Address Type" && objLookup.LookupName == "Physical location address").Select(objlookup => objlookup.LookupId).Single();

                                        ConPersonal = new ContactPersonal();
                                        ConPersonal.FirstName = model.objRelationFthr.txtFirstName;
                                        ConPersonal.MiddleName = model.objRelationFthr.txtMiddleName;
                                        ConPersonal.LastName = model.objRelationFthr.txtLstName;
                                       // ConPersonal.Relation = model.objRelationFthr.RelationName;
                                        if (model.objRelationFthr.txtDob != null)
                                        {
                                            model.objRelationFthr.txtDob = model.objRelationFthr.txtDob.Replace("-", "/");
                                        }
                                        ConPersonal.BirthDate = (model.objRelationFthr.txtDob == null) ? (DateTime?)null : DateTime.ParseExact(model.objRelationFthr.txtDob, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                        ConPersonal.CountryOfCitizenship = model.objRelationFthr.txtUScitzen;
                                        ConPersonal.Employer = model.objRelationFthr.txtEmployer;
                                        ConPersonal.Occupation = model.objRelationFthr.Occupation;
                                        ConPersonal.Education = model.objRelationFthr.txtEducation;
                                        ConPersonal.MaritalStatus = model.objRelationFthr.txtMaritalStatus;
                                        ConPersonal.GrossIncome = model.objRelationFthr.GrossIncome;
                                        ConPersonal.CauseDeath = model.objRelationFthr.causedeath;
                                        ConPersonal.ContactFlag = "Referral";
                                        ConPersonal.CreatedBy = loginId;
                                        ConPersonal.CreatedOn = DateTime.Now;
                                        ConPersonal.Status = 1;
                                        DbEntity.ContactPersonals.Add(ConPersonal);
                                        DbEntity.SaveChanges();

                                        AddrList = new AddressList();
                                        AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objRelationFthr.txtCountry;
                                        AddrList.StateProvince = model.objRelationFthr.txtState;
                                        AddrList.City = model.objRelationFthr.txtCity;
                                        AddrList.StreetName = model.objRelationFthr.txtStreetAdress;
                                        AddrList.ApartmentType = model.objRelationFthr.txtAprtmentUnit;
                                        AddrList.PostalCode = model.objRelationFthr.txtZipCode;
                                        AddrList.Phone = model.objRelationFthr.txtHomePhone;
                                        AddrList.OtherPhone = model.objRelationFthr.txtWorkPhone;
                                        AddrList.Mobile = model.objRelationFthr.txtMobileNum;
                                        AddrList.Fax = model.objRelationFthr.txtFax;
                                        AddrList.PrimaryEmail = model.objRelationFthr.txtEmail;
                                        AddrList.BusinessAddress = model.objRelationFthr.BusinessAddress;
                                        AddrList.CreatedBy = loginId;
                                        AddrList.AddressType = addressType;
                                        AddrList.CreatedOn = DateTime.Now;
                                        DbEntity.AddressLists.Add(AddrList);
                                        DbEntity.SaveChanges();

                                        StdCntctRel = new StudentContactRelationship();
                                        StdCntctRel.ContactPersonalId = ConPersonal.ContactPersonalId;
                                        StdCntctRel.RelationshipId = model.lukUpFathrId;
                                        StdCntctRel.CreatedBy = loginId;
                                        StdCntctRel.CreatedOn = DateTime.Now;
                                        DbEntity.StudentContactRelationships.Add(StdCntctRel);
                                        DbEntity.SaveChanges();

                                        StdAdrRel = new StudentAddresRel();
                                        StdAdrRel.StudentPersonalId = saveId;
                                        StdAdrRel.AddressId = AddrList.AddressId;
                                        StdAdrRel.ContactSequence = 1;
                                        StdAdrRel.ContactPersonalId = ConPersonal.ContactPersonalId;
                                        StdAdrRel.CreatedBy = loginId;
                                        StdAdrRel.CreatedOn = DateTime.Now;
                                        DbEntity.StudentAddresRels.Add(StdAdrRel);
                                        DbEntity.SaveChanges();
                                    }
                                }
                                Insurnce = DbEntity.Insurances.Where(objInsurnce => objInsurnce.StudentPersonalId == saveId & objInsurnce.PreferType == "Primary").SingleOrDefault();
                                Insurnce.InsuranceType = model.objInsuranceDetails.InsuranceCoverage;
                                Insurnce.PolicyNumber = model.objInsuranceDetails.InsurancePolNum;
                                Insurnce.CompanyName = model.objInsuranceDetails.RefInsuranceCompany;
                                DbEntity.SaveChanges();


                                AddrList = DbEntity.AddressLists.Where(objAdress => objAdress.AddressId == insAdressId).SingleOrDefault();
                                AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objInsuranceDetails.txtCountry;
                                AddrList.StateProvince = model.objInsuranceDetails.txtState;
                                AddrList.City = model.objInsuranceDetails.txtCity;
                                AddrList.StreetName = model.objInsuranceDetails.txtStreetAdress;
                                AddrList.PostalCode = model.objInsuranceDetails.txtZipCode;
                                DbEntity.SaveChanges();

                                Insurnce = DbEntity.Insurances.Where(objInsurnce => objInsurnce.StudentPersonalId == saveId & objInsurnce.PreferType == "Secondary").SingleOrDefault();
                                Insurnce.InsuranceType = model.objInsuranceSecDetails.InsuranceCoverage;
                                Insurnce.PolicyNumber = model.objInsuranceSecDetails.InsurancePolNum;
                                Insurnce.CompanyName = model.objInsuranceSecDetails.RefInsuranceCompany;
                                DbEntity.SaveChanges();


                                AddrList = DbEntity.AddressLists.Where(objAdress => objAdress.AddressId == insSecAdressId).SingleOrDefault();
                                AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objInsuranceSecDetails.txtCountry;
                                AddrList.StateProvince = model.objInsuranceSecDetails.txtState;
                                AddrList.City = model.objInsuranceSecDetails.txtCity;
                                AddrList.StreetName = model.objInsuranceSecDetails.txtStreetAdress;
                                AddrList.PostalCode = model.objInsuranceSecDetails.txtZipCode;
                                DbEntity.SaveChanges();
                            }


                            else if (type == "Educational And Vocational Information")
                            {


                            }

                            else if (type == "Birth And Development History")
                            {



                            }

                            else if (type == "Applicant Present Self-Help Skills")
                            {
                                if (saveId > 0)
                                {

                                    var result = (from objBehaviorPa in DbEntity.BehavioursPAs
                                                  join objBehaveCategry in DbEntity.BehaviorCategories on objBehaviorPa.BehaviorId equals objBehaveCategry.BehaviorCategryId
                                                  where (objBehaviorPa.StudentPersonalId == saveId)
                                                  select new
                                                  {
                                                      behaviorId = objBehaviorPa.BehaviorId


                                                  }).ToList();

                                    foreach (var item in result)
                                    {
                                        var dataFromClinet = formdata["behaveNameid_" + item.behaviorId.ToString()];
                                        int behaviorId = Convert.ToInt32(item.behaviorId);
                                        BehaviorPa = DbEntity.BehavioursPAs.Where(objBehaviorPa => objBehaviorPa.BehaviorId == behaviorId & objBehaviorPa.StudentPersonalId == saveId).SingleOrDefault();
                                        BehaviorPa.Score = Convert.ToInt32(dataFromClinet);
                                        DbEntity.SaveChanges();
                                    }
                                }

                                retVal = saveId;
                            }

                            else if (type == "Medical History")
                            {
                                int phyAdressID = 0;
                                try
                                {
                                    phyAdressID = (from mediclInsur in DbEntity.MedicalAndInsurances
                                                   join Addr in DbEntity.AddressLists on mediclInsur.AddressId equals Addr.AddressId
                                                   where (mediclInsur.StudentPersonalId == saveId)
                                                   select new
                                                   {
                                                       AdressId = Addr.AddressId

                                                   }).FirstOrDefault().AdressId;
                                }
                                catch
                                {

                                }


                                Diagnose = DbEntity.DiaganosesPAs.Where(objDiag => objDiag.StudentPersonalId == saveId).FirstOrDefault();
                                Diagnose.BloodGroup = model.objPhysicianDetails.RefBloodType;
                                Diagnose.AllergiesAndReactions = model.objPhysicianDetails.RefAllergiesReaction;
                                Diagnose.SummaryHealthProblem = model.objPhysicianDetails.RefSummaryCurrentHealth;
                                DbEntity.SaveChanges();


                                //try
                                //{
                                medicalIns = DbEntity.MedicalAndInsurances.Where(objMedIns => objMedIns.StudentPersonalId == saveId).FirstOrDefault();
                                medicalIns.FirstName = model.objPhysicianDetails.RefPrimPhyName;
                                medicalIns.Speciality = model.objPhysicianDetails.phySpeciality;
                                if (model.objPhysicianDetails.PhylstApmnt != null)
                                {
                                    model.objPhysicianDetails.PhylstApmnt.Replace("-", "/");
                                }
                                medicalIns.DateOfLastPhysicalExam = model.objPhysicianDetails.PhylstApmnt;
                                DbEntity.SaveChanges();
                                //}
                                //catch { }
                                if (phyAdressID != 0)
                                {
                                    //try
                                    //{

                                    AddrList = DbEntity.AddressLists.Where(objAdress => objAdress.AddressId == phyAdressID).SingleOrDefault();
                                    AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objPhysicianDetails.txtCountry;
                                    AddrList.StateProvince = model.objPhysicianDetails.txtState;
                                    AddrList.City = model.objPhysicianDetails.txtCity;
                                    AddrList.StreetName = model.objPhysicianDetails.txtStreetAdress;
                                    AddrList.PostalCode = model.objPhysicianDetails.txtZipCode;
                                    AddrList.Phone = model.objPhysicianDetails.txtHomePhone;
                                    DbEntity.SaveChanges();
                                    //}
                                    //catch { }
                                }

                            }

                            else if (type == "Legal Material")
                            {
                                int legalGuardianAdressId = 0;
                                int LegalContactPersnlId = 0;
                                //try
                                //{
                                legalGuardianAdressId = (from objStudntPersnl in DbEntity.StudentPersonals
                                                         join StdAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals StdAddrRel.StudentPersonalId
                                                         join ConPrsonal in DbEntity.ContactPersonals on StdAddrRel.ContactPersonalId equals ConPrsonal.ContactPersonalId
                                                         join Addr in DbEntity.AddressLists on StdAddrRel.AddressId equals Addr.AddressId
                                                         join StudContactRel in DbEntity.StudentContactRelationships on ConPrsonal.ContactPersonalId equals StudContactRel.ContactPersonalId
                                                         where (StudContactRel.RelationshipId == model.lukupLegalGurdianId & StdAddrRel.ContactSequence == 1 & objStudntPersnl.StudentPersonalId == saveId)
                                                         select new
                                                         {
                                                             AdresId = Addr.AddressId
                                                         }).OrderByDescending(x => x.AdresId).FirstOrDefault().AdresId;
                                //}
                                //catch { }
                                //try
                                //{
                                LegalContactPersnlId = (from objStudntPersnl in DbEntity.StudentPersonals
                                                        join StdAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals StdAddrRel.StudentPersonalId
                                                        join ConPrsonal in DbEntity.ContactPersonals on StdAddrRel.ContactPersonalId equals ConPrsonal.ContactPersonalId
                                                        join Addr in DbEntity.AddressLists on StdAddrRel.AddressId equals Addr.AddressId
                                                        join StudContactRel in DbEntity.StudentContactRelationships on ConPrsonal.ContactPersonalId equals StudContactRel.ContactPersonalId
                                                        where (StudContactRel.RelationshipId == model.lukupLegalGurdianId & StdAddrRel.ContactSequence == 1 & objStudntPersnl.StudentPersonalId == saveId)
                                                        select new
                                                        {
                                                            ContactId = ConPrsonal.ContactPersonalId
                                                        }).OrderByDescending(x => x.ContactId).FirstOrDefault().ContactId;
                                //}
                                //catch { }

                                if (LegalContactPersnlId != 0)
                                {
                                    ConPersonal = DbEntity.ContactPersonals.Where(objCont => objCont.ContactPersonalId == LegalContactPersnlId).SingleOrDefault();
                                    ConPersonal.FirstName = model.objRelationLegalGuardian.txtFirstName;
                                    ConPersonal.MiddleName = model.objRelationLegalGuardian.txtMiddleName;
                                    ConPersonal.LastName = model.objRelationLegalGuardian.txtLstName;
                                    DbEntity.SaveChanges();
                                }
                                if (legalGuardianAdressId != 0)
                                {
                                    AddrList = DbEntity.AddressLists.Where(objAdr => objAdr.AddressId == legalGuardianAdressId).SingleOrDefault();
                                    AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objRelationLegalGuardian.txtCountry;
                                    AddrList.StateProvince = model.objRelationLegalGuardian.txtState;
                                    AddrList.City = model.objRelationLegalGuardian.txtCity;
                                    AddrList.StreetName = model.objRelationLegalGuardian.txtStreetAdress;
                                    AddrList.ApartmentType = model.objRelationLegalGuardian.txtAprtmentUnit;
                                    AddrList.PostalCode = model.objRelationLegalGuardian.txtZipCode;
                                    AddrList.Phone = model.objRelationLegalGuardian.txtHomePhone;
                                    AddrList.OtherPhone = model.objRelationLegalGuardian.txtWorkPhone;
                                    AddrList.PrimaryEmail = model.objRelationLegalGuardian.txtEmail;
                                    DbEntity.SaveChanges();
                                }

                            }



                            retVal = saveId;
                        }
                        else
                        {
                            if (type == "General Information")
                            {
                                StudentTbl.FirstName = model.RefFrstName;
                                StudentTbl.LastName = model.RefLstName;
                                StudentTbl.MiddleName = model.RefMaidenName;
                                StudentTbl.SchoolId = schoolId;
                                StudentTbl.Height = model.RefPresentHght;
                                StudentTbl.Weight = model.RefPresntWeigth;
                                StudentTbl.HairColor = model.RefHairColor;
                                StudentTbl.EyeColor = model.RefEyecolor;
                                StudentTbl.LocalId = "STD " + saveId;
                                StudentTbl.Gender = model.RefGender;
                                StudentTbl.AdmissionDate = (model.RefDate == null) ? (DateTime?)null : DateTime.ParseExact(model.RefDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                StudentTbl.BirthDate = (model.RefDOB == null) ? (DateTime?)null : DateTime.ParseExact(model.RefDOB, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                StudentTbl.DistingushingMarks = model.RefAnyIdentification;
                                StudentTbl.RaceId = model.RefRace;
                                StudentTbl.Ethinicity = model.RefEthinicity;
                                StudentTbl.ReligiousAffiliation = model.RefReligiousAffilation;
                                StudentTbl.StudentType = "Referral";
                                StudentTbl.CreatedBy = loginId;
                                StudentTbl.CreatedOn = DateTime.Now;
                                DbEntity.StudentPersonals.Add(StudentTbl);
                                DbEntity.SaveChanges();
                                model.RefPersonalId = StudentTbl.StudentPersonalId;

                                //insert into studentpersonalPA
                                StudPersPa.StudentPersonalId = model.RefPersonalId;
                                StudPersPa.CreatedBy = loginId;
                                StudPersPa.SchoolId = schoolId;
                                StudPersPa.CreatedOn = DateTime.Now;
                                DbEntity.StudentPersonalPAs.Add(StudPersPa);
                                DbEntity.SaveChanges();


                                AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.RefCountry;
                                AddrList.StateProvince = model.RefState;
                                AddrList.City = model.RefCity;
                                AddrList.StreetName = model.RefStreetAdrs;
                                AddrList.ApartmentType = model.RefAptUnit;
                                AddrList.PostalCode = model.RefZipCode;
                                AddrList.CreatedBy = loginId;
                                AddrList.CreatedOn = DateTime.Now;
                                DbEntity.AddressLists.Add(AddrList);
                                DbEntity.SaveChanges();
                                model.AddressId = AddrList.AddressId;

                                StdAdrRel.StudentPersonalId = model.RefPersonalId;
                                StdAdrRel.AddressId = model.AddressId;
                                StdAdrRel.ContactSequence = 0;
                                StdAdrRel.ContactPersonalId = 0;
                                StdAdrRel.CreatedBy = loginId;
                                StdAdrRel.CreatedOn = DateTime.Now;
                                DbEntity.StudentAddresRels.Add(StdAdrRel);
                                DbEntity.SaveChanges();
                                int addressType = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Address Type" && objLookup.LookupName == "Physical location address").Select(objlookup => objlookup.LookupId).Single();
                                if ((model.objRelationMthr.txtFirstName != null && model.objRelationMthr.txtFirstName != "") && (model.objRelationMthr.txtLstName != null && model.objRelationMthr.txtLstName != ""))
                                {




                                    ConPersonal.StudentPersonalId = model.RefPersonalId;
                                    ConPersonal.FirstName = model.objRelationMthr.txtFirstName;
                                    ConPersonal.LastName = model.objRelationMthr.txtLstName;
                                    ConPersonal.MiddleName = model.objRelationMthr.txtMiddleName;
                                   // ConPersonal.Relation = model.objRelationMthr.RelationName;
                                    try
                                    {
                                        if (model.objRelationMthr.txtDob != null)
                                        {
                                            model.objRelationMthr.txtDob = model.objRelationMthr.txtDob.Replace("-", "/");
                                        }
                                        ConPersonal.BirthDate = (model.objRelationMthr.txtDob == null) ? (DateTime?)null : DateTime.ParseExact(model.objRelationMthr.txtDob, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                    }
                                    catch
                                    {

                                    }
                                    ConPersonal.CountryOfCitizenship = model.objRelationMthr.txtUScitzen;
                                    ConPersonal.Employer = model.objRelationMthr.txtEmployer;
                                    ConPersonal.Occupation = model.objRelationMthr.Occupation;
                                    ConPersonal.Education = model.objRelationMthr.txtEducation;
                                    ConPersonal.MaritalStatus = model.objRelationMthr.txtMaritalStatus;
                                    ConPersonal.ContactFlag = "Referral";
                                    ConPersonal.Status = 1;
                                    ConPersonal.GrossIncome = model.objRelationMthr.GrossIncome;
                                    ConPersonal.CauseDeath = model.objRelationMthr.causedeath;
                                    ConPersonal.CreatedBy = loginId;
                                    ConPersonal.CreatedOn = DateTime.Now;
                                    DbEntity.ContactPersonals.Add(ConPersonal);
                                    DbEntity.SaveChanges();
                                    model.ContactId = ConPersonal.ContactPersonalId;


                                    AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objRelationMthr.txtCountry;
                                    AddrList.StateProvince = model.objRelationMthr.txtState;
                                    AddrList.City = model.objRelationMthr.txtCity;
                                    AddrList.StreetName = model.objRelationMthr.txtStreetAdress;
                                    AddrList.ApartmentType = model.objRelationMthr.txtAprtmentUnit;
                                    AddrList.PostalCode = model.objRelationMthr.txtZipCode;
                                    AddrList.Phone = model.objRelationMthr.txtHomePhone;
                                    AddrList.OtherPhone = model.objRelationMthr.txtWorkPhone;
                                    AddrList.Fax = model.objRelationMthr.txtFax;
                                    AddrList.Mobile = model.objRelationMthr.txtMobileNum;
                                    AddrList.PrimaryEmail = model.objRelationMthr.txtEmail;
                                    AddrList.BusinessAddress = model.objRelationMthr.BusinessAddress;
                                    AddrList.AddressType = addressType;
                                    AddrList.CreatedBy = loginId;
                                    AddrList.CreatedOn = DateTime.Now;
                                    DbEntity.AddressLists.Add(AddrList);
                                    DbEntity.SaveChanges();
                                    model.AddressId = AddrList.AddressId;


                                    StdCntctRel.ContactPersonalId = model.ContactId;
                                    StdCntctRel.RelationshipId = model.lukUpMthrId;
                                    StdCntctRel.CreatedBy = loginId;
                                    StdCntctRel.CreatedOn = DateTime.Now;
                                    DbEntity.StudentContactRelationships.Add(StdCntctRel);
                                    DbEntity.SaveChanges();


                                    StdAdrRel.StudentPersonalId = model.RefPersonalId;
                                    StdAdrRel.AddressId = model.AddressId;
                                    StdAdrRel.ContactSequence = 1;
                                    StdAdrRel.ContactPersonalId = model.ContactId;
                                    StdAdrRel.CreatedBy = loginId;
                                    StdAdrRel.CreatedOn = DateTime.Now;
                                    DbEntity.StudentAddresRels.Add(StdAdrRel);
                                    DbEntity.SaveChanges();

                                }

                                if ((model.objRelationFthr.txtFirstName != null && model.objRelationFthr.txtFirstName != "") && (model.objRelationFthr.txtLstName != null && model.objRelationFthr.txtLstName != ""))
                                {
                                    ConPersonal.StudentPersonalId = model.RefPersonalId;
                                    ConPersonal.FirstName = model.objRelationFthr.txtFirstName;
                                    ConPersonal.LastName = model.objRelationFthr.txtLstName;
                                    ConPersonal.MiddleName = model.objRelationFthr.txtMiddleName;
                                   // ConPersonal.Relation = model.objRelationFthr.RelationName;
                                    if (model.objRelationFthr.txtDob != null)
                                    {
                                        model.objRelationFthr.txtDob = model.objRelationFthr.txtDob.Replace("-", "/");
                                    }
                                    ConPersonal.BirthDate = (model.objRelationFthr.txtDob == null) ? (DateTime?)null : DateTime.ParseExact(model.objRelationFthr.txtDob, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                    ConPersonal.CountryOfCitizenship = model.objRelationFthr.txtUScitzen;
                                    ConPersonal.Employer = model.objRelationFthr.txtEmployer;
                                    ConPersonal.Occupation = model.objRelationFthr.Occupation;
                                    ConPersonal.Education = model.objRelationFthr.txtEducation;
                                    ConPersonal.ContactFlag = "Referral";
                                    ConPersonal.Status = 1;
                                    ConPersonal.MaritalStatus = model.objRelationFthr.txtMaritalStatus;
                                    ConPersonal.GrossIncome = model.objRelationFthr.GrossIncome;
                                    ConPersonal.CauseDeath = model.objRelationFthr.causedeath;
                                    ConPersonal.CreatedBy = loginId;
                                    ConPersonal.CreatedOn = DateTime.Now;
                                    DbEntity.ContactPersonals.Add(ConPersonal);
                                    DbEntity.SaveChanges();
                                    model.ContactId = ConPersonal.ContactPersonalId;


                                    AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objRelationFthr.txtCountry;
                                    AddrList.StateProvince = model.objRelationFthr.txtState;
                                    AddrList.City = model.objRelationFthr.txtCity;
                                    AddrList.StreetName = model.objRelationFthr.txtStreetAdress;
                                    AddrList.ApartmentType = model.objRelationFthr.txtAprtmentUnit;
                                    AddrList.PostalCode = model.objRelationFthr.txtZipCode;
                                    AddrList.Phone = model.objRelationFthr.txtHomePhone;
                                    AddrList.OtherPhone = model.objRelationFthr.txtWorkPhone;
                                    AddrList.PrimaryEmail = model.objRelationFthr.txtEmail;
                                    AddrList.BusinessAddress = model.objRelationFthr.BusinessAddress;
                                    AddrList.CreatedBy = loginId;
                                    AddrList.AddressType = addressType;
                                    AddrList.CreatedOn = DateTime.Now;
                                    DbEntity.AddressLists.Add(AddrList);
                                    DbEntity.SaveChanges();
                                    model.AddressId = AddrList.AddressId;


                                    StdCntctRel.ContactPersonalId = model.ContactId;
                                    StdCntctRel.RelationshipId = model.lukUpFathrId;
                                    StdCntctRel.CreatedBy = loginId;
                                    StdCntctRel.CreatedOn = DateTime.Now;
                                    DbEntity.StudentContactRelationships.Add(StdCntctRel);
                                    DbEntity.SaveChanges();


                                    StdAdrRel.StudentPersonalId = model.RefPersonalId;
                                    StdAdrRel.AddressId = model.AddressId;
                                    StdAdrRel.ContactSequence = 1;
                                    StdAdrRel.ContactPersonalId = model.ContactId;
                                    StdAdrRel.CreatedBy = loginId;
                                    StdAdrRel.CreatedOn = DateTime.Now;
                                    DbEntity.StudentAddresRels.Add(StdAdrRel);
                                    DbEntity.SaveChanges();

                                }
                                //
                                AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single();// model.objInsuranceDetails.txtCountry;
                                AddrList.StateProvince = model.objInsuranceDetails.txtState;
                                AddrList.City = model.objInsuranceDetails.txtCity;
                                AddrList.StreetName = model.objInsuranceDetails.txtStreetAdress;
                                AddrList.PostalCode = model.objInsuranceDetails.txtZipCode;
                                DbEntity.AddressLists.Add(AddrList);
                                DbEntity.SaveChanges();
                                model.AddressId = AddrList.AddressId;

                                Insurnce.StudentPersonalId = model.RefPersonalId;
                                Insurnce.AddressId = model.AddressId;
                                Insurnce.PreferType = "Primary";
                                Insurnce.InsuranceType = model.objInsuranceDetails.InsuranceCoverage;
                                Insurnce.PolicyNumber = model.objInsuranceDetails.InsurancePolNum;
                                Insurnce.CompanyName = model.objInsuranceDetails.RefInsuranceCompany;
                                DbEntity.Insurances.Add(Insurnce);
                                DbEntity.SaveChanges();

                                AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objInsuranceSecDetails.txtCountry;
                                AddrList.StateProvince = model.objInsuranceSecDetails.txtState;
                                AddrList.City = model.objInsuranceSecDetails.txtCity;
                                AddrList.StreetName = model.objInsuranceSecDetails.txtStreetAdress;
                                AddrList.PostalCode = model.objInsuranceSecDetails.txtZipCode;
                                DbEntity.AddressLists.Add(AddrList);
                                DbEntity.SaveChanges();
                                model.AddressId = AddrList.AddressId;

                                Insurnce.StudentPersonalId = model.RefPersonalId;
                                Insurnce.AddressId = model.AddressId;
                                Insurnce.PreferType = "Secondary";
                                Insurnce.InsuranceType = model.objInsuranceSecDetails.InsuranceCoverage;
                                Insurnce.PolicyNumber = model.objInsuranceSecDetails.InsurancePolNum;
                                Insurnce.CompanyName = model.objInsuranceSecDetails.RefInsuranceCompany;
                                DbEntity.Insurances.Add(Insurnce);
                                DbEntity.SaveChanges();


                                retVal = model.RefPersonalId;


                            }
                            else if (type == "General Family Background Information")
                            {
                                retVal = saveId;
                            }

                            else if (type == "Birth And Development History")
                            {
                                retVal = saveId;
                            }
                            else if (type == "Applicant Present Self-Help Skills")
                            {
                                if (saveId > 0)
                                {

                                    var data = DbEntity.BehaviorCategories.ToList();
                                    foreach (var item in data)
                                    {
                                        var dataFromClinet = formdata["behaveNameid_" + item.BehaviorCategryId.ToString()];
                                        int behaviorId = Convert.ToInt32(item.BehaviorCategryId);
                                        BehaviorPa.StudentPersonalId = saveId;
                                        BehaviorPa.SchoolId = schoolId;
                                        BehaviorPa.BehaviorId = behaviorId;
                                        BehaviorPa.Score = Convert.ToInt32(dataFromClinet);
                                        BehaviorPa.CreatedBy = loginId;
                                        BehaviorPa.CreatedOn = DateTime.Now;
                                        DbEntity.BehavioursPAs.Add(BehaviorPa);
                                        DbEntity.SaveChanges();
                                    }
                                }

                                retVal = saveId;

                            }

                            else if (type == "Medical History")
                            {
                                if (saveId > 0)
                                {
                                    int addressType = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Address Type" && objLookup.LookupName == "Physical location address").Select(objlookup => objlookup.LookupId).Single();


                                    Diagnose.StudentPersonalId = saveId;
                                    Diagnose.BloodGroup = model.objPhysicianDetails.RefBloodType;
                                    Diagnose.AllergiesAndReactions = model.objPhysicianDetails.RefAllergiesReaction;
                                    Diagnose.SummaryHealthProblem = model.objPhysicianDetails.RefSummaryCurrentHealth;
                                    Diagnose.CreatedBy = loginId;
                                    Diagnose.CreatedOn = DateTime.Now;
                                    DbEntity.DiaganosesPAs.Add(Diagnose);
                                    DbEntity.SaveChanges();

                                    AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objPhysicianDetails.txtCountry;
                                    AddrList.StateProvince = model.objPhysicianDetails.txtState;
                                    AddrList.City = model.objPhysicianDetails.txtCity;
                                    AddrList.StreetName = model.objPhysicianDetails.txtStreetAdress;
                                    AddrList.PostalCode = model.objPhysicianDetails.txtZipCode;
                                    AddrList.Phone = model.objPhysicianDetails.txtHomePhone;
                                    AddrList.AddressType = addressType;
                                    DbEntity.AddressLists.Add(AddrList);
                                    DbEntity.SaveChanges();
                                    model.AddressId = AddrList.AddressId;

                                    medicalIns.StudentPersonalId = saveId;
                                    medicalIns.AddressId = model.AddressId;
                                    medicalIns.FirstName = model.objPhysicianDetails.RefPrimPhyName;
                                    medicalIns.Speciality = model.objPhysicianDetails.phySpeciality;
                                    if (model.objPhysicianDetails.PhylstApmnt != null)
                                    {
                                        model.objPhysicianDetails.PhylstApmnt = model.objPhysicianDetails.PhylstApmnt.Replace("-", "/");
                                    }
                                    medicalIns.DateOfLastPhysicalExam = model.objPhysicianDetails.PhylstApmnt;
                                    medicalIns.CreatedBy = loginId;
                                    medicalIns.CreatedOn = DateTime.Now;
                                    DbEntity.MedicalAndInsurances.Add(medicalIns);
                                    DbEntity.SaveChanges();


                                    retVal = saveId;



                                }
                            }

                            else if (type == "Legal Material")
                            {
                                int addressType = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Address Type" && objLookup.LookupName == "Physical location address").Select(objlookup => objlookup.LookupId).Single();

                                if ((model.objRelationLegalGuardian.txtFirstName != null && model.objRelationLegalGuardian.txtFirstName != "") && (model.objRelationLegalGuardian.txtLstName != null && model.objRelationLegalGuardian.txtLstName != ""))
                                {
                                    ConPersonal.StudentPersonalId = saveId;
                                    ConPersonal.FirstName = model.objRelationLegalGuardian.txtFirstName;
                                    ConPersonal.LastName = model.objRelationLegalGuardian.txtLstName;
                                    ConPersonal.MiddleName = model.objRelationLegalGuardian.txtMiddleName;
                                    ConPersonal.CreatedBy = loginId;
                                    ConPersonal.CreatedOn = DateTime.Now;
                                    DbEntity.ContactPersonals.Add(ConPersonal);
                                    DbEntity.SaveChanges();
                                    model.ContactId = ConPersonal.ContactPersonalId;


                                    AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objRelationLegalGuardian.txtCountry;
                                    AddrList.StateProvince = model.objRelationLegalGuardian.txtState;
                                    AddrList.City = model.objRelationLegalGuardian.txtCity;
                                    AddrList.StreetName = model.objRelationLegalGuardian.txtStreetAdress;
                                    AddrList.ApartmentType = model.objRelationLegalGuardian.txtAprtmentUnit;
                                    AddrList.PostalCode = model.objRelationLegalGuardian.txtZipCode;
                                    AddrList.Phone = model.objRelationLegalGuardian.txtHomePhone;
                                    AddrList.OtherPhone = model.objRelationLegalGuardian.txtWorkPhone;
                                    AddrList.PrimaryEmail = model.objRelationLegalGuardian.txtEmail;
                                    AddrList.AddressType = addressType;
                                    AddrList.CreatedBy = loginId;
                                    AddrList.CreatedOn = DateTime.Now;
                                    DbEntity.AddressLists.Add(AddrList);
                                    DbEntity.SaveChanges();
                                    model.AddressId = AddrList.AddressId;


                                    StdAdrRel.StudentPersonalId = saveId;
                                    StdAdrRel.AddressId = model.AddressId;
                                    StdAdrRel.ContactSequence = 1;
                                    StdAdrRel.ContactPersonalId = model.ContactId;
                                    StdAdrRel.CreatedBy = loginId;
                                    StdAdrRel.CreatedOn = DateTime.Now;
                                    DbEntity.StudentAddresRels.Add(StdAdrRel);
                                    DbEntity.SaveChanges();

                                    StdCntctRel = new StudentContactRelationship();
                                    StdCntctRel.ContactPersonalId = ConPersonal.ContactPersonalId;
                                    StdCntctRel.RelationshipId = model.lukupLegalGurdianId;
                                    StdCntctRel.CreatedBy = loginId;
                                    StdCntctRel.CreatedOn = DateTime.Now;
                                    DbEntity.StudentContactRelationships.Add(StdCntctRel);
                                    DbEntity.SaveChanges();

                                }
                                retVal = saveId;
                            }


                        }
                        trans.Complete();
                    }
                }
                catch
                {

                }
            }
            catch
            {

            }
            return retVal;
        }





        public GenInfoModel LoadStudentDataPE(int clientId, int Index)
        {

            GenInfoModel model = new GenInfoModel();
            try
            {
                MelmarkDBEntities DbEntity = new MelmarkDBEntities();
                if (Index == 1)
                {
                    if (model != null)
                    {

                        model = (from objStudntPersnl in DbEntity.StudentPersonals
                                 join objStdtAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals objStdtAddrRel.StudentPersonalId
                                 join objAddress in DbEntity.AddressLists on objStdtAddrRel.AddressId equals objAddress.AddressId
                                 where (objStudntPersnl.StudentPersonalId == clientId & objStdtAddrRel.ContactSequence == 0)

                                 select new GenInfoModel
                                 {
                                     RefFrstName = objStudntPersnl.FirstName,
                                     RefLstName = objStudntPersnl.LastName,
                                     RefGender = objStudntPersnl.Gender,
                                     RefMaidenName = objStudntPersnl.MiddleName,
                                     RefPresentHght = objStudntPersnl.Height,
                                     RefPresntWeigth = objStudntPersnl.Weight,
                                     RefEyecolor = objStudntPersnl.EyeColor,
                                     RefHairColor = objStudntPersnl.HairColor,
                                     RefAnyIdentification = objStudntPersnl.DistingushingMarks,
                                     RefRace = objStudntPersnl.RaceId,
                                     RefEthinicity = objStudntPersnl.Ethinicity,
                                     RefReligiousAffilation = objStudntPersnl.ReligiousAffiliation,
                                     RefDateDatetime = objStudntPersnl.AdmissionDate,
                                     RefDOBDateTime = objStudntPersnl.BirthDate,
                                     RefCountry = objAddress.CountryId,
                                     RefState = objAddress.StateProvince,
                                     PrimaryDiag = objStudntPersnl.PrimaryDiag,
                                     SecondaryDiag = objStudntPersnl.SecondaryDiag,
                                     RefCity = objAddress.City,
                                     RefStreetAdrs = objAddress.StreetName,
                                     RefAptUnit = objAddress.ApartmentType,
                                     RefZipCode = objAddress.PostalCode


                                 }).FirstOrDefault();
                        model.RefDate = model.RefDateDatetime == null ? "" : ((DateTime)model.RefDateDatetime).ToString("MM/dd/yyyy").Replace("-", "/");
                        model.RefDOB = model.RefDOBDateTime == null ? "" : ((DateTime)model.RefDOBDateTime).ToString("MM/dd/yyyy").Replace("-", "/");


                        try
                        {
                            model.lukUpFathrId = ((from LookUp lukup in DbEntity.LookUps
                                                   where (lukup.LookupName == "Legal Guardian 2")
                                                   select new
                                                   {
                                                       lukUpFathrId = lukup.LookupId

                                                   }).SingleOrDefault()).lukUpFathrId;

                            model.lukUpMthrId = ((from LookUp lukup in DbEntity.LookUps
                                                  where (lukup.LookupName == "Legal Guardian 1")
                                                  select new
                                                  {
                                                      lukUpMthrId = lukup.LookupId

                                                  }).SingleOrDefault()).lukUpMthrId;



                        }
                        catch
                        {
                            model.lukUpFathrId = 0;
                            model.lukUpMthrId = 0;
                            model.lukupCloseId = 0;
                            model.lukupLegalGurdianId = 0;
                            model.lukupEmergencyId = 0;
                        }



                        model.objRelationMthr = (from objStudntPersnl in DbEntity.StudentPersonals
                                                 join StdAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals StdAddrRel.StudentPersonalId
                                                 join ConPersonal in DbEntity.ContactPersonals on StdAddrRel.ContactPersonalId equals ConPersonal.ContactPersonalId
                                                 join Addr in DbEntity.AddressLists on StdAddrRel.AddressId equals Addr.AddressId
                                                 join StudContactRel in DbEntity.StudentContactRelationships on ConPersonal.ContactPersonalId equals StudContactRel.ContactPersonalId
                                                 where (StudContactRel.RelationshipId == model.lukUpMthrId & StdAddrRel.ContactSequence == 1 & objStudntPersnl.StudentPersonalId == clientId)
                                                 select new RelationCLs
                                                 {
                                                     txtFirstName = ConPersonal.FirstName,
                                                     txtLstName = ConPersonal.LastName,
                                                     txtMiddleName = ConPersonal.MiddleName,
                                                    // RelationName = ConPersonal.Relation,
                                                     txtDobdatetime = ConPersonal.BirthDate,
                                                     txtUScitzen = ConPersonal.CountryOfCitizenship,
                                                     txtEmployer = ConPersonal.Employer,
                                                     txtMaritalStatus = ConPersonal.MaritalStatus,
                                                     GrossIncome = ConPersonal.GrossIncome,
                                                     causedeath = ConPersonal.CauseDeath,
                                                     Occupation = ConPersonal.Occupation,
                                                     txtEducation = ConPersonal.Education,
                                                     BusinessAddress = Addr.BusinessAddress,
                                                     txtCountry = Addr.CountryId,
                                                     txtState = Addr.StateProvince,
                                                     txtStreetAdress = Addr.StreetName,
                                                     txtCity = Addr.City,
                                                     txtAprtmentUnit = Addr.ApartmentType,
                                                     txtZipCode = Addr.PostalCode,
                                                     txtHomePhone = Addr.Phone,
                                                     txtWorkPhone = Addr.OtherPhone,
                                                     txtMobileNum = Addr.Mobile,
                                                     txtFax = Addr.Fax,
                                                     txtCounty = Addr.County,
                                                     txtEmail = Addr.PrimaryEmail

                                                 }).FirstOrDefault();

                        if (model.objRelationMthr != null)
                        {

                            model.objRelationMthr.txtDob = model.objRelationMthr.txtDobdatetime == null ? "" : ((DateTime)model.objRelationMthr.txtDobdatetime).ToString("MM/dd/yyyy".Replace("-", "/"));
                        }
                        else
                        {
                            int country = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.RefCountry;
                            model.objRelationMthr = new RelationCLs
                            {
                                txtFirstName = "",
                                txtLstName = "",
                                txtMiddleName = "",
                                txtCountry = country

                            };
                        }


                        model.objRelationFthr = (from objStudntPersnl in DbEntity.StudentPersonals
                                                 join StdAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals StdAddrRel.StudentPersonalId
                                                 join ConPersonal in DbEntity.ContactPersonals on StdAddrRel.ContactPersonalId equals ConPersonal.ContactPersonalId
                                                 join Addr in DbEntity.AddressLists on StdAddrRel.AddressId equals Addr.AddressId
                                                 join StudContactRel in DbEntity.StudentContactRelationships on ConPersonal.ContactPersonalId equals StudContactRel.ContactPersonalId
                                                 where (StudContactRel.RelationshipId == model.lukUpFathrId & StdAddrRel.ContactSequence == 1 & objStudntPersnl.StudentPersonalId == clientId)
                                                 select new RelationCLs
                                                 {
                                                     txtFirstName = ConPersonal.FirstName,
                                                     txtLstName = ConPersonal.LastName,
                                                     txtMiddleName = ConPersonal.MiddleName,
                                                    // RelationName = ConPersonal.Relation,
                                                     txtDobdatetime = ConPersonal.BirthDate,
                                                     txtUScitzen = ConPersonal.CountryOfCitizenship,
                                                     txtEmployer = ConPersonal.Employer,
                                                     txtMaritalStatus = ConPersonal.MaritalStatus,
                                                     GrossIncome = ConPersonal.GrossIncome,
                                                     causedeath = ConPersonal.CauseDeath,
                                                     Occupation = ConPersonal.Occupation,
                                                     txtEducation = ConPersonal.Education,
                                                     BusinessAddress = Addr.BusinessAddress,
                                                     txtCountry = Addr.CountryId,
                                                     txtState = Addr.StateProvince,
                                                     txtStreetAdress = Addr.StreetName,
                                                     txtCity = Addr.City,
                                                     txtAprtmentUnit = Addr.ApartmentType,
                                                     txtZipCode = Addr.PostalCode,
                                                     txtHomePhone = Addr.Phone,
                                                     txtWorkPhone = Addr.OtherPhone,
                                                     txtMobileNum = Addr.Mobile,
                                                     txtFax = Addr.Fax,
                                                     txtCounty = Addr.County,
                                                     txtEmail = Addr.PrimaryEmail
                                                 }).FirstOrDefault();


                        if (model.objRelationFthr != null)
                        {

                            model.objRelationFthr.txtDob = model.objRelationFthr.txtDobdatetime == null ? "" : ((DateTime)model.objRelationFthr.txtDobdatetime).ToString("MM/dd/yyyy".Replace("-", "/"));
                        }
                        else
                        {
                            int country = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.RefCountry;
                            model.objRelationFthr = new RelationCLs
                            {
                                txtFirstName = "",
                                txtLstName = "",
                                txtMiddleName = "",
                                txtCountry = country

                            };
                        }

                        clsReferral cls = new clsReferral();
                        bool flag = cls.findNewAppFlag(clientId);
                        if (flag == false)
                        {

                            model.objInsuranceDetails = (from Insurnce in DbEntity.Insurances
                                                         join Addr in DbEntity.AddressLists on Insurnce.AddressId equals Addr.AddressId
                                                         where (Insurnce.StudentPersonalId == clientId & Insurnce.PreferType == "Primary")
                                                         select new RelationCLs
                                                         {
                                                             RefInsuranceCompany = Insurnce.CompanyName,
                                                             txtCountry = Addr.CountryId,
                                                             txtState = Addr.StateProvince,
                                                             txtCity = Addr.City,
                                                             txtStreetAdress = Addr.StreetName,
                                                             txtZipCode = Addr.PostalCode,
                                                             InsuranceCoverage = Insurnce.InsuranceType,
                                                             InsurancePolNum = Insurnce.PolicyNumber

                                                         }).FirstOrDefault();

                            model.objInsuranceSecDetails = (from Insurnce in DbEntity.Insurances
                                                            join Addr in DbEntity.AddressLists on Insurnce.AddressId equals Addr.AddressId
                                                            where (Insurnce.StudentPersonalId == clientId & Insurnce.PreferType == "Secondary")
                                                            select new RelationCLs
                                                            {
                                                                RefInsuranceCompany = Insurnce.CompanyName,
                                                                txtCountry = Addr.CountryId,
                                                                txtState = Addr.StateProvince,
                                                                txtCity = Addr.City,
                                                                txtStreetAdress = Addr.StreetName,
                                                                txtZipCode = Addr.PostalCode,
                                                                InsuranceCoverage = Insurnce.InsuranceType,
                                                                InsurancePolNum = Insurnce.PolicyNumber

                                                            }).FirstOrDefault();

                        }


                    }
                }
                else if (Index == 2)
                {


                }

                else if (Index == 3)
                {


                }

                else if (Index == 4)
                {

                }

                else if (Index == 5)
                {
                    bool valid = false;

                    valid = IsValidCheckTab3(clientId);

                    if (valid == true)
                    {

                        if (model != null)
                        {
                            model.objPhysicianDetails = (from mediclInsur in DbEntity.MedicalAndInsurances
                                                         join Diagn in DbEntity.DiaganosesPAs on mediclInsur.StudentPersonalId equals Diagn.StudentPersonalId
                                                         join Addr in DbEntity.AddressLists on mediclInsur.AddressId equals Addr.AddressId
                                                         where (mediclInsur.StudentPersonalId == clientId)
                                                         select new RelationCLs
                                                         {
                                                             RefBloodType = Diagn.BloodGroup,
                                                             RefAllergiesReaction = Diagn.AllergiesAndReactions,
                                                             RefSummaryCurrentHealth = Diagn.SummaryHealthProblem,
                                                             RefPrimPhyName = mediclInsur.FirstName,
                                                             phySpeciality = mediclInsur.Speciality,
                                                             txtCountry = Addr.CountryId,
                                                             txtState = Addr.StateProvince,
                                                             txtCity = Addr.City,
                                                             txtStreetAdress = Addr.StreetName,
                                                             txtZipCode = Addr.PostalCode,
                                                             txtHomePhone = Addr.Phone,
                                                             PhylstApmntdateTime = mediclInsur.DateOfLastPhysicalExam

                                                         }).FirstOrDefault();

                            if (model.objPhysicianDetails != null)
                            {
                                model.objPhysicianDetails.PhylstApmnt = model.objPhysicianDetails.PhylstApmntdateTime;
                            }
                            else
                            {
                                int country = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.RefCountry;
                                model.objRelationMthr = new RelationCLs
                                {
                                    txtFirstName = "",
                                    txtLstName = "",
                                    txtMiddleName = "",
                                    txtCountry = country

                                };
                            }
                        }
                        clsReferral cls = new clsReferral();
                        bool flag = cls.findNewAppFlag(clientId);
                        if (flag == true)
                        {
                            model.objInsuranceDetails = (from Insurnce in DbEntity.Insurances
                                                         join Addr in DbEntity.AddressLists on Insurnce.AddressId equals Addr.AddressId
                                                         where (Insurnce.StudentPersonalId == clientId & Insurnce.PreferType == "Primary")
                                                         select new RelationCLs
                                                         {
                                                             RefInsuranceCompany = Insurnce.CompanyName,
                                                             txtCountry = Addr.CountryId,
                                                             txtState = Addr.StateProvince,
                                                             txtCity = Addr.City,
                                                             txtStreetAdress = Addr.StreetName,
                                                             txtZipCode = Addr.PostalCode,
                                                             InsuranceCoverage = Insurnce.InsuranceType,
                                                             InsurancePolNum = Insurnce.PolicyNumber

                                                         }).FirstOrDefault();

                            model.objInsuranceSecDetails = (from Insurnce in DbEntity.Insurances
                                                            join Addr in DbEntity.AddressLists on Insurnce.AddressId equals Addr.AddressId
                                                            where (Insurnce.StudentPersonalId == clientId & Insurnce.PreferType == "Secondary")
                                                            select new RelationCLs
                                                            {
                                                                RefInsuranceCompany = Insurnce.CompanyName,
                                                                txtCountry = Addr.CountryId,
                                                                txtState = Addr.StateProvince,
                                                                txtCity = Addr.City,
                                                                txtStreetAdress = Addr.StreetName,
                                                                txtZipCode = Addr.PostalCode,
                                                                InsuranceCoverage = Insurnce.InsuranceType,
                                                                InsurancePolNum = Insurnce.PolicyNumber

                                                            }).FirstOrDefault();
                        }


                    }
                }

                else if (Index == 6)
                {
                    bool valid = false;
                    valid = CheckLegalParent(clientId);

                    if (valid == true)
                    {

                        if (model != null)
                        {
                            model.lukupLegalGurdianId = ((from LookUp lukup in DbEntity.LookUps
                                                          where (lukup.LookupName == "Legal Guardian")
                                                          select new
                                                          {
                                                              lukupCloseId = lukup.LookupId


                                                          }).SingleOrDefault()).lukupCloseId;

                            try
                            {

                                model.objRelationLegalGuardian = (from objStudntPersnl in DbEntity.StudentPersonals
                                                                  join StdAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals StdAddrRel.StudentPersonalId
                                                                  join ConPersonal in DbEntity.ContactPersonals on StdAddrRel.ContactPersonalId equals ConPersonal.ContactPersonalId
                                                                  join Addr in DbEntity.AddressLists on StdAddrRel.AddressId equals Addr.AddressId
                                                                  join StudContactRel in DbEntity.StudentContactRelationships on ConPersonal.ContactPersonalId equals StudContactRel.ContactPersonalId
                                                                  where (StudContactRel.RelationshipId == model.lukupLegalGurdianId & StdAddrRel.ContactSequence == 1 & objStudntPersnl.StudentPersonalId == clientId)
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

                                if (model.objRelationLegalGuardian != null)
                                {
                                    // model.objPhysicianDetails.PhylstApmnt = model.objPhysicianDetails.PhylstApmntdateTime == null ? "" : ((DateTime)model.objPhysicianDetails.PhylstApmntdateTime).ToString("MM/dd/yyyy").Replace("-", "/");
                                }
                                else
                                {
                                    int country = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.RefCountry;
                                    model.objRelationLegalGuardian = new RelationCLs
                                    {
                                        txtFirstName = "",
                                        txtLstName = "",
                                        txtMiddleName = "",
                                        txtCountry = country

                                    };
                                }
                            }
                            catch
                            {
                            }

                        }
                    }
                }

                else if (Index == 8)
                {
                    model.objclsUpld.DocName = "";
                    model.objclsUpld.DocType = 0;


                }


            }
            catch (Exception eX)
            {
                throw eX;
            }
            return model;
        }

        public int SaveGeneralDataOld(GenInfoModel model, string type, int saveId, string status, int loginId, int schoolId)
        {
            int retVal = 0;
            MelmarkDBEntities DbEntity = new MelmarkDBEntities();
            StudentPersonal StudentTbl = new StudentPersonal();
            AddressList AddrList = new AddressList();
            StudentAddresRel StdAdrRel = new StudentAddresRel();
            ContactPersonal ConPersonal = new ContactPersonal();
            LookUp LukUp = new LookUp();
            StudentContactRelationship StdCntctRel = new StudentContactRelationship();
            DiaganosesPA Diagnose = new DiaganosesPA();
            MedicalAndInsurance medicalIns = new MedicalAndInsurance();
            StudentPersonalPA StudPersPa = new StudentPersonalPA();
            Insurance Insurnce = new Insurance();
            BehavioursPA BehaviorPa = new BehavioursPA();

            try
            {

                try
                {
                    model.lukUpFathrId = ((from LookUp lukup in DbEntity.LookUps
                                           where (lukup.LookupName == "Father")
                                           select new
                                           {
                                               lukUpFathrId = lukup.LookupId

                                           }).SingleOrDefault()).lukUpFathrId;

                    model.lukUpMthrId = ((from LookUp lukup in DbEntity.LookUps
                                          where (lukup.LookupName == "Mother")
                                          select new
                                          {
                                              lukUpMthrId = lukup.LookupId


                                          }).SingleOrDefault()).lukUpMthrId;

                    model.lukupCloseId = ((from LookUp lukup in DbEntity.LookUps
                                           where (lukup.LookupName == "None")
                                           select new
                                           {
                                               lukupCloseId = lukup.LookupId


                                           }).SingleOrDefault()).lukupCloseId;

                    model.lukupLegalGurdianId = ((from LookUp lukup in DbEntity.LookUps
                                                  where (lukup.LookupName == "Legal Guardian")
                                                  select new
                                                  {
                                                      lukupCloseId = lukup.LookupId


                                                  }).SingleOrDefault()).lukupCloseId;


                    model.lukupEmergencyId = ((from LookUp lukup in DbEntity.LookUps
                                               where (lukup.LookupName == "Emergency Contact")
                                               select new
                                               {
                                                   lukupCloseId = lukup.LookupId


                                               }).SingleOrDefault()).lukupCloseId;


                }
                catch
                {
                    model.lukUpFathrId = 0;
                    model.lukUpMthrId = 0;
                }



                using (TransactionScope trans = new TransactionScope(TransactionScopeOption.Required))
                {
                    ClsErrorLog erorLg = new ClsErrorLog();

                    if ((saveId > 0) && (status == "update"))
                    {

                        if (type == "General Information")
                        {

                            int RefaddrId = (from objStudntPersnl in DbEntity.StudentPersonals
                                             join objStdtAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals objStdtAddrRel.StudentPersonalId
                                             join objAddress in DbEntity.AddressLists on objStdtAddrRel.AddressId equals objAddress.AddressId
                                             where (objStudntPersnl.StudentPersonalId == saveId & objStdtAddrRel.ContactSequence == 0)

                                             select new
                                             {
                                                 AdressId = objAddress.AddressId

                                             }).SingleOrDefault().AdressId;

                            int mothrAdressId = 0;
                            var mothrAdress = (from objStudntPersnl in DbEntity.StudentPersonals
                                               join StdAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals StdAddrRel.StudentPersonalId
                                               join ConPrsonal in DbEntity.ContactPersonals on StdAddrRel.ContactPersonalId equals ConPrsonal.ContactPersonalId
                                               join Addr in DbEntity.AddressLists on StdAddrRel.AddressId equals Addr.AddressId
                                               join StudContactRel in DbEntity.StudentContactRelationships on ConPrsonal.ContactPersonalId equals StudContactRel.ContactPersonalId
                                               where (StudContactRel.RelationshipId == model.lukUpMthrId & StdAddrRel.ContactSequence == 1 & objStudntPersnl.StudentPersonalId == saveId)
                                               select new
                                               {
                                                   AdresId = Addr.AddressId
                                               }).ToList();
                            if (mothrAdress.Count > 0)
                            {
                                foreach (var item in mothrAdress)
                                {
                                    mothrAdressId = item.AdresId;
                                }
                            }

                            int mothrConPersId = 0;

                            var mothrConPers = (from objStudntPersnl in DbEntity.StudentPersonals
                                                join StdAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals StdAddrRel.StudentPersonalId
                                                join ConPrsonal in DbEntity.ContactPersonals on StdAddrRel.ContactPersonalId equals ConPrsonal.ContactPersonalId
                                                join Addr in DbEntity.AddressLists on StdAddrRel.AddressId equals Addr.AddressId
                                                join StudContactRel in DbEntity.StudentContactRelationships on ConPrsonal.ContactPersonalId equals StudContactRel.ContactPersonalId
                                                where (StudContactRel.RelationshipId == model.lukUpMthrId & StdAddrRel.ContactSequence == 1 & objStudntPersnl.StudentPersonalId == saveId)
                                                select new
                                                {
                                                    ContactId = ConPrsonal.ContactPersonalId
                                                }).ToList();
                            if (mothrConPers.Count > 0)
                            {
                                foreach (var item in mothrConPers)
                                {
                                    mothrConPersId = item.ContactId;
                                }
                            }

                            int FthrAdressId = 0;


                            var FthrAdress = (from objStudntPersnl in DbEntity.StudentPersonals
                                              join StdAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals StdAddrRel.StudentPersonalId
                                              join ConPrsonal in DbEntity.ContactPersonals on StdAddrRel.ContactPersonalId equals ConPrsonal.ContactPersonalId
                                              join Addr in DbEntity.AddressLists on StdAddrRel.AddressId equals Addr.AddressId
                                              join StudContactRel in DbEntity.StudentContactRelationships on ConPrsonal.ContactPersonalId equals StudContactRel.ContactPersonalId
                                              where (StudContactRel.RelationshipId == model.lukUpFathrId & StdAddrRel.ContactSequence == 1 & objStudntPersnl.StudentPersonalId == saveId)
                                              select new
                                              {
                                                  AdresId = Addr.AddressId
                                              }).ToList();

                            if (FthrAdress.Count > 0)
                            {
                                foreach (var item in FthrAdress)
                                {
                                    FthrAdressId = item.AdresId;
                                }
                            }

                            int FthrConPersId = 0;

                            var FthrConPers = (from objStudntPersnl in DbEntity.StudentPersonals
                                               join StdAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals StdAddrRel.StudentPersonalId
                                               join ConPrsonal in DbEntity.ContactPersonals on StdAddrRel.ContactPersonalId equals ConPrsonal.ContactPersonalId
                                               join Addr in DbEntity.AddressLists on StdAddrRel.AddressId equals Addr.AddressId
                                               join StudContactRel in DbEntity.StudentContactRelationships on ConPrsonal.ContactPersonalId equals StudContactRel.ContactPersonalId
                                               where (StudContactRel.RelationshipId == model.lukUpFathrId & StdAddrRel.ContactSequence == 1 & objStudntPersnl.StudentPersonalId == saveId)
                                               select new
                                               {
                                                   ContactId = ConPrsonal.ContactPersonalId
                                               }).ToList();
                            if (FthrConPers.Count > 0)
                            {
                                foreach (var item in FthrConPers)
                                {
                                    FthrConPersId = item.ContactId;
                                }
                            }


                            int closeRelAdressId = 0;
                            var closeRelAdress = (from objStudntPersnl in DbEntity.StudentPersonals
                                                  join StdAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals StdAddrRel.StudentPersonalId
                                                  join ConPrsonal in DbEntity.ContactPersonals on StdAddrRel.ContactPersonalId equals ConPrsonal.ContactPersonalId
                                                  join Addr in DbEntity.AddressLists on StdAddrRel.AddressId equals Addr.AddressId
                                                  join StudContactRel in DbEntity.StudentContactRelationships on ConPrsonal.ContactPersonalId equals StudContactRel.ContactPersonalId
                                                  join Look in DbEntity.LookUps on StudContactRel.RelationshipId equals Look.LookupId
                                                  where (Look.LookupType == "Relationship" & StdAddrRel.ContactSequence == 1 & objStudntPersnl.StudentPersonalId == saveId & ConPrsonal.Relation == -1)
                                                  select new
                                                  {
                                                      AddressIds = Addr.AddressId
                                                  }).ToList();

                            if (closeRelAdress.Count > 0)
                            {
                                foreach (var item in closeRelAdress)
                                {
                                    closeRelAdressId = item.AddressIds;
                                }
                            }

                            int closeCntPersId = 0;
                            var closeCntPers = (from objStudntPersnl in DbEntity.StudentPersonals
                                                join StdAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals StdAddrRel.StudentPersonalId
                                                join ConPrsonal in DbEntity.ContactPersonals on StdAddrRel.ContactPersonalId equals ConPrsonal.ContactPersonalId
                                                join Addr in DbEntity.AddressLists on StdAddrRel.AddressId equals Addr.AddressId
                                                join StudContactRel in DbEntity.StudentContactRelationships on ConPrsonal.ContactPersonalId equals StudContactRel.ContactPersonalId
                                                join Look in DbEntity.LookUps on StudContactRel.RelationshipId equals Look.LookupId
                                                where (Look.LookupType == "Relationship" & StdAddrRel.ContactSequence == 1 & objStudntPersnl.StudentPersonalId == saveId & ConPrsonal.Relation == -1)
                                                select new
                                                {
                                                    ContactId = ConPrsonal.ContactPersonalId
                                                }).ToList();

                            if (closeCntPers.Count > 0)
                            {
                                foreach (var item in closeCntPers)
                                {
                                    closeCntPersId = item.ContactId;
                                }
                            }

                            int legalGuardianAdressId = 0;

                            var legalGuardianAdress = (from objStudntPersnl in DbEntity.StudentPersonals
                                                       join StdAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals StdAddrRel.StudentPersonalId
                                                       join ConPrsonal in DbEntity.ContactPersonals on StdAddrRel.ContactPersonalId equals ConPrsonal.ContactPersonalId
                                                       join Addr in DbEntity.AddressLists on StdAddrRel.AddressId equals Addr.AddressId
                                                       join StudContactRel in DbEntity.StudentContactRelationships on ConPrsonal.ContactPersonalId equals StudContactRel.ContactPersonalId
                                                       where (StudContactRel.RelationshipId == model.lukupLegalGurdianId & StdAddrRel.ContactSequence == 1 & objStudntPersnl.StudentPersonalId == saveId)
                                                       select new
                                                       {
                                                           AdresId = Addr.AddressId
                                                       }).ToList();

                            if (legalGuardianAdress.Count > 0)
                            {
                                foreach (var item in legalGuardianAdress)
                                {
                                    legalGuardianAdressId = item.AdresId;
                                }
                            }

                            int LegalContactPersnlId = 0;
                            var LegalContactPersnl = (from objStudntPersnl in DbEntity.StudentPersonals
                                                      join StdAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals StdAddrRel.StudentPersonalId
                                                      join ConPrsonal in DbEntity.ContactPersonals on StdAddrRel.ContactPersonalId equals ConPrsonal.ContactPersonalId
                                                      join Addr in DbEntity.AddressLists on StdAddrRel.AddressId equals Addr.AddressId
                                                      join StudContactRel in DbEntity.StudentContactRelationships on ConPrsonal.ContactPersonalId equals StudContactRel.ContactPersonalId
                                                      where (StudContactRel.RelationshipId == model.lukupLegalGurdianId & StdAddrRel.ContactSequence == 1 & objStudntPersnl.StudentPersonalId == saveId)
                                                      select new
                                                      {
                                                          ContactId = ConPrsonal.ContactPersonalId
                                                      }).ToList();
                            if (LegalContactPersnl.Count > 0)
                            {
                                foreach (var item in LegalContactPersnl)
                                {
                                    LegalContactPersnlId = item.ContactId;
                                }
                            }
                            int emergncyAdrsId = 0;

                            var emergncyAdrs = (from objStudntPersnl in DbEntity.StudentPersonals
                                                join StdAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals StdAddrRel.StudentPersonalId
                                                join ConPrsonal in DbEntity.ContactPersonals on StdAddrRel.ContactPersonalId equals ConPrsonal.ContactPersonalId
                                                join Addr in DbEntity.AddressLists on StdAddrRel.AddressId equals Addr.AddressId
                                                join StudContactRel in DbEntity.StudentContactRelationships on ConPrsonal.ContactPersonalId equals StudContactRel.ContactPersonalId
                                                where (StudContactRel.RelationshipId == model.lukupEmergencyId & StdAddrRel.ContactSequence == 1 & objStudntPersnl.StudentPersonalId == saveId)
                                                select new
                                                {
                                                    AdresId = Addr.AddressId
                                                }).ToList();

                            if (emergncyAdrs.Count > 0)
                            {
                                foreach (var item in emergncyAdrs)
                                {
                                    emergncyAdrsId = item.AdresId;
                                }
                            }

                            int emergncyContactId = 0;
                            var emergncyContact = (from objStudntPersnl in DbEntity.StudentPersonals
                                                   join StdAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals StdAddrRel.StudentPersonalId
                                                   join ConPrsonal in DbEntity.ContactPersonals on StdAddrRel.ContactPersonalId equals ConPrsonal.ContactPersonalId
                                                   join Addr in DbEntity.AddressLists on StdAddrRel.AddressId equals Addr.AddressId
                                                   join StudContactRel in DbEntity.StudentContactRelationships on ConPrsonal.ContactPersonalId equals StudContactRel.ContactPersonalId
                                                   where (StudContactRel.RelationshipId == model.lukupEmergencyId & StdAddrRel.ContactSequence == 1 & objStudntPersnl.StudentPersonalId == saveId)
                                                   select new
                                                   {
                                                       ContactId = ConPrsonal.ContactPersonalId
                                                   }).ToList();


                            if (emergncyContact.Count > 0)
                            {
                                foreach (var item in emergncyContact)
                                {
                                    emergncyContactId = item.ContactId;
                                }
                            }

                            StudentTbl = DbEntity.StudentPersonals.Where(objStudent => objStudent.StudentPersonalId == saveId).SingleOrDefault();
                            StudentTbl.FirstName = model.RefFrstName;
                            StudentTbl.LastName = model.RefLstName;
                            StudentTbl.MiddleName = model.RefMaidenName;
                            StudentTbl.PrimaryDiag = model.PrimaryDiag;
                            StudentTbl.SecondaryDiag = model.SecondaryDiag;
                            StudentTbl.SocialSecurityNo = model.SocialSecurityNo;
                            StudentTbl.SSINo = model.SsiNo;
                            StudentTbl.Height = model.RefPresentHght;
                            StudentTbl.Weight = model.RefPresntWeigth;
                            StudentTbl.HairColor = model.RefHairColor;
                            StudentTbl.EyeColor = model.RefEyecolor;
                            StudentTbl.LocalId = "STD " + saveId;
                            StudentTbl.Gender = model.RefGender;
                            StudentTbl.AdmissionDate = (model.RefDate == null) ? (DateTime?)null : DateTime.ParseExact(model.RefDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            StudentTbl.BirthDate = (model.RefDOB == null) ? (DateTime?)null : DateTime.ParseExact(model.RefDOB, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            StudentTbl.ModifiedBy = loginId;
                            StudentTbl.CreatedOn = DateTime.Now;
                            DbEntity.SaveChanges();


                            AddrList = DbEntity.AddressLists.Where(objAdr => objAdr.AddressId == RefaddrId).SingleOrDefault();
                            AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.RefCountry;
                            AddrList.StateProvince = model.RefState;
                            AddrList.City = model.RefCity;
                            AddrList.StreetName = model.RefStreetAdrs;
                            AddrList.ApartmentType = model.RefAptUnit;
                            AddrList.PostalCode = model.RefZipCode;
                            DbEntity.SaveChanges();


                            ConPersonal = DbEntity.ContactPersonals.Where(objCont => objCont.ContactPersonalId == mothrConPersId).SingleOrDefault();
                            if (ConPersonal != null)
                            {
                                ConPersonal.FirstName = model.objRelationMthr.txtFirstName;
                                ConPersonal.MiddleName = model.objRelationMthr.txtMiddleName;
                                ConPersonal.LastName = model.objRelationMthr.txtLstName;
                                DbEntity.SaveChanges();


                                AddrList = DbEntity.AddressLists.Where(objAdr => objAdr.AddressId == mothrAdressId).SingleOrDefault();
                                AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objRelationMthr.txtCountry;
                                AddrList.StateProvince = model.objRelationMthr.txtState;
                                AddrList.City = model.objRelationMthr.txtCity;
                                AddrList.StreetName = model.objRelationMthr.txtStreetAdress;
                                AddrList.ApartmentType = model.objRelationMthr.txtAprtmentUnit;
                                AddrList.PostalCode = model.objRelationMthr.txtZipCode;
                                AddrList.Phone = model.objRelationMthr.txtHomePhone;
                                AddrList.OtherPhone = model.objRelationMthr.txtWorkPhone;
                                AddrList.PrimaryEmail = model.objRelationMthr.txtEmail;
                                DbEntity.SaveChanges();

                            }
                            else
                            {
                                if ((model.objRelationMthr.txtFirstName != null && model.objRelationMthr.txtFirstName != "") || (model.objRelationMthr.txtLstName != null && model.objRelationMthr.txtLstName != ""))
                                {
                                    AddrList = new AddressList();
                                    StdAdrRel = new StudentAddresRel();
                                    ConPersonal = new ContactPersonal();
                                    StdCntctRel = new StudentContactRelationship();
                                    int addressType = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Address Type" && objLookup.LookupName == "Physical location address").Select(objlookup => objlookup.LookupId).Single();

                                    ConPersonal.StudentPersonalId = StudentTbl.StudentPersonalId;
                                    ConPersonal.FirstName = model.objRelationMthr.txtFirstName;
                                    ConPersonal.LastName = model.objRelationMthr.txtLstName;
                                    ConPersonal.MiddleName = model.objRelationMthr.txtMiddleName;
                                    ConPersonal.ContactFlag = "Referral";
                                    ConPersonal.Status = 1;
                                    ConPersonal.CreatedBy = loginId;
                                    ConPersonal.CreatedOn = DateTime.Now;
                                    DbEntity.ContactPersonals.Add(ConPersonal);
                                    DbEntity.SaveChanges();
                                    model.ContactId = ConPersonal.ContactPersonalId;

                                    AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single();// model.objRelationMthr.txtCountry;
                                    AddrList.StateProvince = model.objRelationMthr.txtState;
                                    AddrList.AddressType = addressType;
                                    AddrList.City = model.objRelationMthr.txtCity;
                                    AddrList.StreetName = model.objRelationMthr.txtStreetAdress;
                                    AddrList.ApartmentType = model.objRelationMthr.txtAprtmentUnit;
                                    AddrList.PostalCode = model.objRelationMthr.txtZipCode;
                                    AddrList.Phone = model.objRelationMthr.txtHomePhone;
                                    AddrList.OtherPhone = model.objRelationMthr.txtWorkPhone;
                                    AddrList.PrimaryEmail = model.objRelationMthr.txtEmail;
                                    AddrList.CreatedBy = loginId;
                                    AddrList.CreatedOn = DateTime.Now;
                                    DbEntity.AddressLists.Add(AddrList);
                                    DbEntity.SaveChanges();
                                    model.AddressId = AddrList.AddressId;

                                    StdCntctRel.ContactPersonalId = model.ContactId;
                                    StdCntctRel.RelationshipId = model.lukUpMthrId;
                                    StdCntctRel.CreatedBy = loginId;
                                    StdCntctRel.CreatedOn = DateTime.Now;
                                    DbEntity.StudentContactRelationships.Add(StdCntctRel);
                                    DbEntity.SaveChanges();

                                    StdAdrRel.AddressId = AddrList.AddressId;
                                    StdAdrRel.StudentPersonalId = StudentTbl.StudentPersonalId;
                                    StdAdrRel.ContactPersonalId = ConPersonal.ContactPersonalId;
                                    StdAdrRel.ContactSequence = 1;
                                    StdAdrRel.CreatedBy = loginId;
                                    StdAdrRel.CreatedOn = DateTime.Now;
                                    DbEntity.StudentAddresRels.Add(StdAdrRel);
                                    DbEntity.SaveChanges();
                                }
                            }
                            ConPersonal = DbEntity.ContactPersonals.Where(objCont => objCont.ContactPersonalId == FthrConPersId).SingleOrDefault();
                            if (ConPersonal != null)
                            {
                                ConPersonal.FirstName = model.objRelationFthr.txtFirstName;
                                ConPersonal.MiddleName = model.objRelationFthr.txtMiddleName;
                                ConPersonal.LastName = model.objRelationFthr.txtLstName;
                                DbEntity.SaveChanges();

                                AddrList = DbEntity.AddressLists.Where(objAdr => objAdr.AddressId == FthrAdressId).SingleOrDefault();
                                AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objRelationFthr.txtCountry;
                                AddrList.StateProvince = model.objRelationFthr.txtState;
                                AddrList.City = model.objRelationFthr.txtCity;
                                AddrList.StreetName = model.objRelationFthr.txtStreetAdress;
                                AddrList.ApartmentType = model.objRelationFthr.txtAprtmentUnit;
                                AddrList.PostalCode = model.objRelationFthr.txtZipCode;
                                AddrList.Phone = model.objRelationFthr.txtHomePhone;
                                AddrList.OtherPhone = model.objRelationFthr.txtWorkPhone;
                                AddrList.PrimaryEmail = model.objRelationFthr.txtEmail;
                                DbEntity.SaveChanges();

                            }
                            else
                            {

                                if ((model.objRelationFthr.txtFirstName != null && model.objRelationFthr.txtFirstName != "") || (model.objRelationFthr.txtLstName != null && model.objRelationFthr.txtLstName != ""))
                                {
                                    AddrList = new AddressList();
                                    StdAdrRel = new StudentAddresRel();
                                    ConPersonal = new ContactPersonal();
                                    StdCntctRel = new StudentContactRelationship();
                                    int addressType = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Address Type" && objLookup.LookupName == "Physical location address").Select(objlookup => objlookup.LookupId).Single();

                                    ConPersonal.StudentPersonalId = StudentTbl.StudentPersonalId;
                                    ConPersonal.FirstName = model.objRelationFthr.txtFirstName;
                                    ConPersonal.LastName = model.objRelationFthr.txtLstName;
                                    ConPersonal.MiddleName = model.objRelationFthr.txtMiddleName;
                                    ConPersonal.ContactFlag = "Referral";
                                    ConPersonal.Status = 1;
                                    ConPersonal.CreatedBy = loginId;
                                    ConPersonal.CreatedOn = DateTime.Now;
                                    DbEntity.ContactPersonals.Add(ConPersonal);
                                    DbEntity.SaveChanges();
                                    model.ContactId = ConPersonal.ContactPersonalId;

                                    AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objRelationFthr.txtCountry;
                                    AddrList.StateProvince = model.objRelationFthr.txtState;
                                    AddrList.City = model.objRelationFthr.txtCity;
                                    AddrList.AddressType = addressType;
                                    AddrList.StreetName = model.objRelationFthr.txtStreetAdress;
                                    AddrList.ApartmentType = model.objRelationFthr.txtAprtmentUnit;
                                    AddrList.PostalCode = model.objRelationFthr.txtZipCode;
                                    AddrList.Phone = model.objRelationFthr.txtHomePhone;
                                    AddrList.OtherPhone = model.objRelationFthr.txtWorkPhone;
                                    AddrList.PrimaryEmail = model.objRelationFthr.txtEmail;
                                    AddrList.CreatedBy = loginId;
                                    AddrList.CreatedOn = DateTime.Now;
                                    DbEntity.AddressLists.Add(AddrList);
                                    DbEntity.SaveChanges();
                                    model.AddressId = AddrList.AddressId;

                                    StdCntctRel.ContactPersonalId = model.ContactId;
                                    StdCntctRel.RelationshipId = model.lukUpFathrId;
                                    StdCntctRel.CreatedBy = loginId;
                                    StdCntctRel.CreatedOn = DateTime.Now;
                                    DbEntity.StudentContactRelationships.Add(StdCntctRel);
                                    DbEntity.SaveChanges();

                                    StdAdrRel.AddressId = AddrList.AddressId;
                                    StdAdrRel.StudentPersonalId = StudentTbl.StudentPersonalId;
                                    StdAdrRel.ContactPersonalId = ConPersonal.ContactPersonalId;
                                    StdAdrRel.ContactSequence = 1;
                                    StdAdrRel.CreatedBy = loginId;
                                    StdAdrRel.CreatedOn = DateTime.Now;
                                    DbEntity.StudentAddresRels.Add(StdAdrRel);
                                    DbEntity.SaveChanges();
                                }
                            }

                            ConPersonal = DbEntity.ContactPersonals.Where(objCont => objCont.ContactPersonalId == closeCntPersId).SingleOrDefault();
                            if (ConPersonal != null)
                            {
                                ConPersonal.FirstName = model.objRelationClose.txtFirstName;
                                ConPersonal.MiddleName = model.objRelationClose.txtMiddleName;
                                ConPersonal.LastName = model.objRelationClose.txtLstName;
                                DbEntity.SaveChanges();
                                model.objRelationClose.RelationName = "";



                                StdCntctRel = DbEntity.StudentContactRelationships.Where(objRel => objRel.ContactPersonalId == closeCntPersId).SingleOrDefault();
                                StdCntctRel.RelationshipId = model.objRelationClose.closeRelation;
                                StdCntctRel.ModifiedBy = loginId;
                                StdCntctRel.ModifiedOn = DateTime.Now;
                                DbEntity.SaveChanges();


                                AddrList = DbEntity.AddressLists.Where(objAdr => objAdr.AddressId == closeRelAdressId).SingleOrDefault();
                                AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objRelationClose.txtCountry;
                                AddrList.StateProvince = model.objRelationClose.txtState;
                                AddrList.City = model.objRelationClose.txtCity;
                                AddrList.StreetName = model.objRelationClose.txtStreetAdress;
                                AddrList.ApartmentType = model.objRelationClose.txtAprtmentUnit;
                                AddrList.PostalCode = model.objRelationClose.txtZipCode;
                                AddrList.Phone = model.objRelationClose.txtHomePhone;
                                AddrList.OtherPhone = model.objRelationClose.txtWorkPhone;
                                AddrList.PrimaryEmail = model.objRelationClose.txtEmail;
                                DbEntity.SaveChanges();
                            }
                            else
                            {

                                if ((model.objRelationClose.txtFirstName != null && model.objRelationClose.txtFirstName != "") || (model.objRelationClose.txtLstName != null && model.objRelationClose.txtLstName != ""))
                                {
                                    AddrList = new AddressList();
                                    StdAdrRel = new StudentAddresRel();
                                    ConPersonal = new ContactPersonal();
                                    StdCntctRel = new StudentContactRelationship();
                                    int addressType = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Address Type" && objLookup.LookupName == "Physical location address").Select(objlookup => objlookup.LookupId).Single();


                                    model.objRelationClose.RelationName = (-1).ToString();
                                    ConPersonal.StudentPersonalId = StudentTbl.StudentPersonalId;
                                    ConPersonal.ContactFlag = "Referral";
                                    ConPersonal.Status = 1;
                                    ConPersonal.FirstName = model.objRelationClose.txtFirstName;
                                    ConPersonal.LastName = model.objRelationClose.txtLstName;
                                    ConPersonal.MiddleName = model.objRelationClose.txtMiddleName;
                                    ConPersonal.Relation = Convert.ToInt32(model.objRelationClose.RelationName);
                                    ConPersonal.CreatedBy = loginId;
                                    ConPersonal.CreatedOn = DateTime.Now;
                                    DbEntity.ContactPersonals.Add(ConPersonal);
                                    DbEntity.SaveChanges();
                                    model.ContactId = ConPersonal.ContactPersonalId;


                                    AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objRelationClose.txtCountry;
                                    AddrList.StateProvince = model.objRelationClose.txtState;
                                    AddrList.City = model.objRelationClose.txtCity;
                                    AddrList.AddressType = addressType;
                                    AddrList.StreetName = model.objRelationClose.txtStreetAdress;
                                    AddrList.ApartmentType = model.objRelationClose.txtAprtmentUnit;
                                    AddrList.PostalCode = model.objRelationClose.txtZipCode;
                                    AddrList.Phone = model.objRelationClose.txtHomePhone;
                                    AddrList.OtherPhone = model.objRelationClose.txtWorkPhone;
                                    AddrList.PrimaryEmail = model.objRelationClose.txtEmail;
                                    AddrList.CreatedBy = loginId;
                                    AddrList.CreatedOn = DateTime.Now;
                                    DbEntity.AddressLists.Add(AddrList);
                                    DbEntity.SaveChanges();
                                    model.AddressId = AddrList.AddressId;

                                    StdCntctRel.ContactPersonalId = model.ContactId;
                                    if (model.objRelationClose.closeRelation == 0)
                                    {
                                        model.objRelationClose.closeRelation = model.lukupCloseId;
                                    }
                                    StdCntctRel.RelationshipId = model.objRelationClose.closeRelation;
                                    StdCntctRel.CreatedBy = loginId;
                                    StdCntctRel.CreatedOn = DateTime.Now;
                                    DbEntity.StudentContactRelationships.Add(StdCntctRel);
                                    DbEntity.SaveChanges();


                                    StdAdrRel.AddressId = AddrList.AddressId;
                                    StdAdrRel.StudentPersonalId = StudentTbl.StudentPersonalId;
                                    StdAdrRel.ContactPersonalId = ConPersonal.ContactPersonalId;
                                    StdAdrRel.ContactSequence = 1;
                                    StdAdrRel.CreatedBy = loginId;
                                    StdAdrRel.CreatedOn = DateTime.Now;
                                    DbEntity.StudentAddresRels.Add(StdAdrRel);
                                    DbEntity.SaveChanges();
                                }

                            }

                            ConPersonal = DbEntity.ContactPersonals.Where(objCont => objCont.ContactPersonalId == LegalContactPersnlId).SingleOrDefault();
                            if (ConPersonal != null)
                            {
                                ConPersonal.FirstName = model.objRelationLegalGuardian.txtFirstName;
                                ConPersonal.MiddleName = model.objRelationLegalGuardian.txtMiddleName;
                                ConPersonal.LastName = model.objRelationLegalGuardian.txtLstName;
                                DbEntity.SaveChanges();

                                AddrList = DbEntity.AddressLists.Where(objAdr => objAdr.AddressId == legalGuardianAdressId).SingleOrDefault();
                                AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objRelationLegalGuardian.txtCountry;
                                AddrList.StateProvince = model.objRelationLegalGuardian.txtState;
                                AddrList.City = model.objRelationLegalGuardian.txtCity;
                                AddrList.StreetName = model.objRelationLegalGuardian.txtStreetAdress;
                                AddrList.ApartmentType = model.objRelationLegalGuardian.txtAprtmentUnit;
                                AddrList.PostalCode = model.objRelationLegalGuardian.txtZipCode;
                                AddrList.Phone = model.objRelationLegalGuardian.txtHomePhone;
                                AddrList.OtherPhone = model.objRelationLegalGuardian.txtWorkPhone;
                                AddrList.PrimaryEmail = model.objRelationLegalGuardian.txtEmail;
                                DbEntity.SaveChanges();
                            }
                            else
                            {
                                if ((model.objRelationLegalGuardian.txtFirstName != null && model.objRelationLegalGuardian.txtFirstName != "") || (model.objRelationLegalGuardian.txtLstName != null && model.objRelationLegalGuardian.txtLstName != ""))
                                {
                                    AddrList = new AddressList();
                                    StdAdrRel = new StudentAddresRel();
                                    ConPersonal = new ContactPersonal();
                                    StdCntctRel = new StudentContactRelationship();
                                    int addressType = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Address Type" && objLookup.LookupName == "Physical location address").Select(objlookup => objlookup.LookupId).Single();

                                    ConPersonal.StudentPersonalId = StudentTbl.StudentPersonalId;
                                    ConPersonal.FirstName = model.objRelationLegalGuardian.txtFirstName;
                                    ConPersonal.LastName = model.objRelationLegalGuardian.txtLstName;
                                    ConPersonal.MiddleName = model.objRelationLegalGuardian.txtMiddleName;
                                    ConPersonal.ContactFlag = "Referral";
                                    ConPersonal.Status = 1;
                                    ConPersonal.CreatedBy = loginId;
                                    ConPersonal.CreatedOn = DateTime.Now;
                                    DbEntity.ContactPersonals.Add(ConPersonal);
                                    DbEntity.SaveChanges();
                                    model.ContactId = ConPersonal.ContactPersonalId;

                                    AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objRelationLegalGuardian.txtCountry;
                                    AddrList.StateProvince = model.objRelationLegalGuardian.txtState;
                                    AddrList.City = model.objRelationLegalGuardian.txtCity;
                                    AddrList.AddressType = addressType;
                                    AddrList.StreetName = model.objRelationLegalGuardian.txtStreetAdress;
                                    AddrList.ApartmentType = model.objRelationLegalGuardian.txtAprtmentUnit;
                                    AddrList.PostalCode = model.objRelationLegalGuardian.txtZipCode;
                                    AddrList.Phone = model.objRelationLegalGuardian.txtHomePhone;
                                    AddrList.OtherPhone = model.objRelationLegalGuardian.txtWorkPhone;
                                    AddrList.PrimaryEmail = model.objRelationLegalGuardian.txtEmail;
                                    AddrList.CreatedBy = loginId;
                                    AddrList.CreatedOn = DateTime.Now;
                                    DbEntity.AddressLists.Add(AddrList);
                                    DbEntity.SaveChanges();
                                    model.AddressId = AddrList.AddressId;

                                    StdCntctRel.ContactPersonalId = model.ContactId;
                                    StdCntctRel.RelationshipId = model.lukupLegalGurdianId;
                                    StdCntctRel.CreatedBy = loginId;
                                    StdCntctRel.CreatedOn = DateTime.Now;
                                    DbEntity.StudentContactRelationships.Add(StdCntctRel);
                                    DbEntity.SaveChanges();

                                    StdAdrRel.AddressId = AddrList.AddressId;
                                    StdAdrRel.StudentPersonalId = StudentTbl.StudentPersonalId;
                                    StdAdrRel.ContactPersonalId = ConPersonal.ContactPersonalId;
                                    StdAdrRel.ContactSequence = 1;
                                    StdAdrRel.CreatedBy = loginId;
                                    StdAdrRel.CreatedOn = DateTime.Now;
                                    DbEntity.StudentAddresRels.Add(StdAdrRel);
                                    DbEntity.SaveChanges();
                                }
                            }
                            ConPersonal = DbEntity.ContactPersonals.Where(objCont => objCont.ContactPersonalId == emergncyContactId).SingleOrDefault();
                            if (ConPersonal != null)
                            {

                                int addressType = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Address Type" && objLookup.LookupName == "Physical location address").Select(objlookup => objlookup.LookupId).Single();

                                ConPersonal.FirstName = model.objRelationEmergncyContact.txtFirstName;
                                ConPersonal.MiddleName = model.objRelationEmergncyContact.txtMiddleName;
                                ConPersonal.LastName = model.objRelationEmergncyContact.txtLstName;
                                ConPersonal.Relation = model.objRelationEmergncyContact.EmRelationName;
                                DbEntity.SaveChanges();

                                AddrList = DbEntity.AddressLists.Where(objAdr => objAdr.AddressId == emergncyAdrsId).SingleOrDefault();
                                AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objRelationEmergncyContact.txtCountry;
                                AddrList.StateProvince = model.objRelationEmergncyContact.txtState;
                                AddrList.City = model.objRelationEmergncyContact.txtCity;
                                AddrList.StreetName = model.objRelationEmergncyContact.txtStreetAdress;
                                AddrList.ApartmentType = model.objRelationEmergncyContact.txtAprtmentUnit;
                                AddrList.PostalCode = model.objRelationEmergncyContact.txtZipCode;
                                AddrList.Phone = model.objRelationEmergncyContact.txtHomePhone;
                                AddrList.OtherPhone = model.objRelationEmergncyContact.txtWorkPhone;
                                AddrList.PrimaryEmail = model.objRelationEmergncyContact.txtEmail;
                                DbEntity.SaveChanges();
                            }

                            else
                            {
                                if ((model.objRelationEmergncyContact.txtFirstName != null && model.objRelationEmergncyContact.txtFirstName != "") || (model.objRelationEmergncyContact.txtLstName != null && model.objRelationEmergncyContact.txtLstName != ""))
                                {
                                    AddrList = new AddressList();
                                    StdAdrRel = new StudentAddresRel();
                                    ConPersonal = new ContactPersonal();
                                    StdCntctRel = new StudentContactRelationship();
                                    int addressType = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Address Type" && objLookup.LookupName == "Physical location address").Select(objlookup => objlookup.LookupId).Single();

                                    ConPersonal.StudentPersonalId = StudentTbl.StudentPersonalId;
                                    ConPersonal.ContactFlag = "Referral";
                                    ConPersonal.Status = 1;
                                    ConPersonal.FirstName = model.objRelationEmergncyContact.txtFirstName;
                                    ConPersonal.LastName = model.objRelationEmergncyContact.txtLstName;
                                    ConPersonal.MiddleName = model.objRelationEmergncyContact.txtMiddleName;
                                    ConPersonal.Relation = model.objRelationEmergncyContact.EmRelationName;
                                    ConPersonal.CreatedBy = loginId;
                                    ConPersonal.CreatedOn = DateTime.Now;
                                    DbEntity.ContactPersonals.Add(ConPersonal);
                                    DbEntity.SaveChanges();
                                    model.ContactId = ConPersonal.ContactPersonalId;

                                    AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objRelationEmergncyContact.txtCountry;
                                    AddrList.StateProvince = model.objRelationEmergncyContact.txtState;
                                    AddrList.City = model.objRelationEmergncyContact.txtCity;
                                    AddrList.AddressType = addressType;
                                    AddrList.StreetName = model.objRelationEmergncyContact.txtStreetAdress;
                                    AddrList.ApartmentType = model.objRelationEmergncyContact.txtAprtmentUnit;
                                    AddrList.PostalCode = model.objRelationEmergncyContact.txtZipCode;
                                    AddrList.Phone = model.objRelationEmergncyContact.txtHomePhone;
                                    AddrList.OtherPhone = model.objRelationEmergncyContact.txtWorkPhone;
                                    AddrList.PrimaryEmail = model.objRelationEmergncyContact.txtEmail;
                                    AddrList.CreatedBy = loginId;
                                    AddrList.CreatedOn = DateTime.Now;
                                    DbEntity.AddressLists.Add(AddrList);
                                    DbEntity.SaveChanges();
                                    model.AddressId = AddrList.AddressId;

                                    StdCntctRel.ContactPersonalId = model.ContactId;
                                    StdCntctRel.RelationshipId = model.lukupEmergencyId;
                                    StdCntctRel.CreatedBy = loginId;
                                    StdCntctRel.CreatedOn = DateTime.Now;
                                    DbEntity.StudentContactRelationships.Add(StdCntctRel);
                                    DbEntity.SaveChanges();

                                    StdAdrRel.AddressId = AddrList.AddressId;
                                    StdAdrRel.StudentPersonalId = StudentTbl.StudentPersonalId;
                                    StdAdrRel.ContactPersonalId = ConPersonal.ContactPersonalId;
                                    StdAdrRel.ContactSequence = 1;
                                    StdAdrRel.CreatedBy = loginId;
                                    StdAdrRel.CreatedOn = DateTime.Now;
                                    DbEntity.StudentAddresRels.Add(StdAdrRel);
                                    DbEntity.SaveChanges();
                                }


                            }
                        }


                        else if (type == "General Family Background Information")
                        {

                            try
                            {


                                int mothrConPersId = (from objStudntPersnl in DbEntity.StudentPersonals
                                                      join StdAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals StdAddrRel.StudentPersonalId
                                                      join ConPrsonal in DbEntity.ContactPersonals on StdAddrRel.ContactPersonalId equals ConPrsonal.ContactPersonalId
                                                      join Addr in DbEntity.AddressLists on StdAddrRel.AddressId equals Addr.AddressId
                                                      join StudContactRel in DbEntity.StudentContactRelationships on ConPrsonal.ContactPersonalId equals StudContactRel.ContactPersonalId
                                                      where (StudContactRel.RelationshipId == model.lukUpMthrId & StdAddrRel.ContactSequence == 1 & objStudntPersnl.StudentPersonalId == saveId
                                                      && ConPrsonal.ContactPersonalId == StdAddrRel.ContactPersonalId)
                                                      select new
                                                      {
                                                          ContactId = ConPrsonal.ContactPersonalId
                                                      }).SingleOrDefault().ContactId;


                                int FthrConPersId = (from objStudntPersnl in DbEntity.StudentPersonals
                                                     join StdAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals StdAddrRel.StudentPersonalId
                                                     join ConPrsonal in DbEntity.ContactPersonals on StdAddrRel.ContactPersonalId equals ConPrsonal.ContactPersonalId
                                                     join Addr in DbEntity.AddressLists on StdAddrRel.AddressId equals Addr.AddressId
                                                     join StudContactRel in DbEntity.StudentContactRelationships on ConPrsonal.ContactPersonalId equals StudContactRel.ContactPersonalId
                                                     where (StudContactRel.RelationshipId == model.lukUpFathrId & StdAddrRel.ContactSequence == 1 & objStudntPersnl.StudentPersonalId == saveId
                                                     && ConPrsonal.ContactPersonalId == StdAddrRel.ContactPersonalId)
                                                     select new
                                                     {
                                                         ContactId = ConPrsonal.ContactPersonalId
                                                     }).SingleOrDefault().ContactId;



                                ConPersonal = DbEntity.ContactPersonals.Where(objContPers => objContPers.ContactPersonalId == mothrConPersId).SingleOrDefault();
                                ConPersonal.BirthDate = (model.objRelationMthr.txtDob == null) ? (DateTime?)null : DateTime.ParseExact(model.objRelationMthr.txtDob, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                ConPersonal.CountryOfCitizenship = model.objRelationMthr.txtUScitzen;
                                ConPersonal.Employer = model.objRelationMthr.txtEmployer;
                                ConPersonal.Occupation = model.objRelationMthr.txtOccupation;
                                ConPersonal.MaritalStatus = model.objRelationMthr.txtMaritalStatus;
                                DbEntity.SaveChanges();

                                ConPersonal = DbEntity.ContactPersonals.Where(objContPers => objContPers.ContactPersonalId == FthrConPersId).SingleOrDefault();
                                ConPersonal.BirthDate = (model.objRelationFthr.txtDob == null) ? (DateTime?)null : DateTime.ParseExact(model.objRelationFthr.txtDob, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                ConPersonal.CountryOfCitizenship = model.objRelationFthr.txtUScitzen;
                                ConPersonal.Employer = model.objRelationFthr.txtEmployer;
                                ConPersonal.Occupation = model.objRelationFthr.txtOccupation;
                                DbEntity.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                                ClsErrorLog erorLog = new ClsErrorLog();
                                erorLog.WriteToLog(ex.ToString());
                            }

                        }

                        else if (type == "Birth And Development History")
                        {
                            try
                            {
                                int phyAdressID = 0;
                                int insAdressId = 0;
                                int insSecAdressId = 0;
                                int insDentalAdressId = 0;

                                var phyAdressIDs = (from mediclInsur in DbEntity.MedicalAndInsurances
                                                    join Addr in DbEntity.AddressLists on mediclInsur.AddressId equals Addr.AddressId
                                                    where (mediclInsur.StudentPersonalId == saveId)
                                                    select new
                                                    {
                                                        AdressId = Addr.AddressId

                                                    }).SingleOrDefault();
                                if (phyAdressIDs != null)
                                {
                                    phyAdressID = phyAdressIDs.AdressId;
                                }

                                var insAdressIds = (from Insrnce in DbEntity.Insurances
                                                    join Addr in DbEntity.AddressLists on Insrnce.AddressId equals Addr.AddressId
                                                    where (Insrnce.StudentPersonalId == saveId & Insrnce.PreferType == "Primary")
                                                    select new
                                                    {
                                                        AdresId = Addr.AddressId

                                                    }).SingleOrDefault();

                                if (insAdressIds != null)
                                {
                                    insAdressId = insAdressIds.AdresId;
                                }


                                var insSecAdressIds = (from Insrnce in DbEntity.Insurances
                                                       join Addr in DbEntity.AddressLists on Insrnce.AddressId equals Addr.AddressId
                                                       where (Insrnce.StudentPersonalId == saveId & Insrnce.PreferType == "Secondary")
                                                       select new
                                                       {
                                                           AdresId = Addr.AddressId

                                                       }).SingleOrDefault();
                                if (insSecAdressIds != null)
                                {
                                    insSecAdressId = insSecAdressIds.AdresId;
                                }

                                var insDentalAdressIds = (from Insrnce in DbEntity.Insurances
                                                          join Addr in DbEntity.AddressLists on Insrnce.AddressId equals Addr.AddressId
                                                          where (Insrnce.StudentPersonalId == saveId & Insrnce.PreferType == "Dental")
                                                          select new
                                                          {
                                                              AdresId = Addr.AddressId

                                                          }).SingleOrDefault();
                                if (insDentalAdressIds != null)
                                {
                                    insDentalAdressId = insDentalAdressIds.AdresId;
                                }

                                StudentTbl = DbEntity.StudentPersonals.Where(objStudent => objStudent.StudentPersonalId == saveId).SingleOrDefault();
                                if (StudentTbl != null)
                                {
                                    StudentTbl.PlaceOfBirth = model.RefbirthPlace;
                                    StudentTbl.StateOfBirth = model.RefStateBirth;
                                    StudentTbl.CountryOfBirth = model.RefCntryBirth;
                                    DbEntity.SaveChanges();
                                }

                                Diagnose = DbEntity.DiaganosesPAs.Where(objDiag => objDiag.StudentPersonalId == saveId).SingleOrDefault();
                                if (Diagnose != null)
                                {
                                    Diagnose.Diaganoses = model.RefDiagnosis;
                                    DbEntity.SaveChanges();
                                }

                                StudPersPa = DbEntity.StudentPersonalPAs.Where(objStPers => objStPers.StudentPersonalId == saveId).SingleOrDefault();
                                if (StudPersPa != null)
                                {
                                    StudPersPa.Allergies = model.RefAllergies;
                                    DbEntity.SaveChanges();
                                }

                                medicalIns = DbEntity.MedicalAndInsurances.Where(objMedIns => objMedIns.StudentPersonalId == saveId).SingleOrDefault();
                                if (medicalIns != null)
                                {
                                    medicalIns.FirstName = model.objPhysicianDetails.RefPrimPhyName;
                                    /// medicalIns.SignificantBehaviorCharacteristics=model.objPhysicianDetails.re
                                    medicalIns.DateOfLastPhysicalExam = model.objPhysicianDetails.PhylstApmnt;
                                    DbEntity.SaveChanges();
                                }

                                AddrList = DbEntity.AddressLists.Where(objAdress => objAdress.AddressId == phyAdressID).SingleOrDefault();
                                if (AddrList != null)
                                {
                                    //if (model.objPhysicianDetails.txtCountry != null)
                                    //{
                                    //    try
                                    //    {
                                    //        int coun = Convert.ToInt32(model.objPhysicianDetails.txtCountry);
                                    //        AddrList.CountryId = coun;
                                    //    }
                                    //    catch (InvalidCastException Ex)
                                    //    {
                                    //        AddrList.CountryId = 0;
                                    //        throw Ex;
                                    //    }
                                    //}



                                    //if (model.objPhysicianDetails.txtState != null)
                                    //{
                                    //    try
                                    //    {
                                    //        int State = Convert.ToInt32(model.objPhysicianDetails.txtState);
                                    //        AddrList.StateProvince = State;
                                    //    }
                                    //    catch (InvalidCastException Ex)
                                    //    {
                                    //        AddrList.StateProvince = 0;
                                    //        throw Ex;
                                    //    }
                                    //}

                                    AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objPhysicianDetails.txtCountry;
                                    AddrList.StateProvince = model.objPhysicianDetails.txtState;
                                    AddrList.City = model.objPhysicianDetails.txtCity;
                                    AddrList.StreetName = model.objPhysicianDetails.txtStreetAdress;
                                    AddrList.PostalCode = model.objPhysicianDetails.txtZipCode;
                                    AddrList.Phone = model.objPhysicianDetails.txtHomePhone;
                                    DbEntity.SaveChanges();
                                }

                                Insurnce = DbEntity.Insurances.Where(objInsurnce => objInsurnce.StudentPersonalId == saveId & objInsurnce.PreferType == "Primary").SingleOrDefault();
                                if (Insurnce != null)
                                {
                                    Insurnce.InsuranceType = model.objInsuranceDetails.InsuranceCoverage;
                                    Insurnce.PolicyNumber = model.objInsuranceDetails.InsurancePolNum;
                                    Insurnce.CompanyName = model.objInsuranceDetails.RefInsuranceCompany;
                                    Insurnce.SchoolId = schoolId;
                                    DbEntity.SaveChanges();
                                }


                                AddrList = DbEntity.AddressLists.Where(objAdress => objAdress.AddressId == insAdressId).SingleOrDefault();
                                if (AddrList != null)
                                {
                                    AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objInsuranceDetails.txtCountry;
                                    AddrList.StateProvince = model.objInsuranceDetails.txtState;
                                    AddrList.City = model.objInsuranceDetails.txtCity;
                                    AddrList.StreetName = model.objInsuranceDetails.txtStreetAdress;
                                    AddrList.PostalCode = model.objInsuranceDetails.txtZipCode;
                                    DbEntity.SaveChanges();
                                }

                                Insurnce = DbEntity.Insurances.Where(objInsurnce => objInsurnce.StudentPersonalId == saveId & objInsurnce.PreferType == "Secondary").SingleOrDefault();
                                if (Insurnce != null)
                                {
                                    Insurnce.InsuranceType = model.objInsuranceSecDetails.InsuranceCoverage;
                                    Insurnce.PolicyNumber = model.objInsuranceSecDetails.InsurancePolNum;
                                    Insurnce.CompanyName = model.objInsuranceSecDetails.RefInsuranceCompany;
                                    DbEntity.SaveChanges();
                                }


                                AddrList = DbEntity.AddressLists.Where(objAdress => objAdress.AddressId == insSecAdressId).SingleOrDefault();
                                if (AddrList != null)
                                {
                                    AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objInsuranceSecDetails.txtCountry;
                                    AddrList.StateProvince = model.objInsuranceSecDetails.txtState;
                                    AddrList.City = model.objInsuranceSecDetails.txtCity;
                                    AddrList.StreetName = model.objInsuranceSecDetails.txtStreetAdress;
                                    AddrList.PostalCode = model.objInsuranceSecDetails.txtZipCode;
                                    DbEntity.SaveChanges();
                                }

                                Insurnce = DbEntity.Insurances.Where(objInsurnce => objInsurnce.StudentPersonalId == saveId & objInsurnce.PreferType == "Dental").SingleOrDefault();
                                if (Insurnce != null)
                                {
                                    Insurnce.InsuranceType = model.objInsuranceDentalDetails.InsuranceCoverage;
                                    Insurnce.PolicyNumber = model.objInsuranceDentalDetails.InsurancePolNum;
                                    Insurnce.CompanyName = model.objInsuranceDentalDetails.RefInsuranceCompany;
                                    DbEntity.SaveChanges();
                                }


                                AddrList = DbEntity.AddressLists.Where(objAdress => objAdress.AddressId == insDentalAdressId).SingleOrDefault();
                                if (AddrList != null)
                                {
                                    AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objInsuranceDentalDetails.txtCountry;
                                    AddrList.StateProvince = model.objInsuranceDentalDetails.txtState;
                                    AddrList.City = model.objInsuranceDentalDetails.txtCity;
                                    AddrList.StreetName = model.objInsuranceDentalDetails.txtStreetAdress;
                                    AddrList.PostalCode = model.objInsuranceDentalDetails.txtZipCode;
                                    DbEntity.SaveChanges();
                                }
                            }
                            catch (InvalidCastException Ex)
                            {
                                throw Ex;
                            }
                        }

                        else if (type == "Personal History")
                        {
                            medicalIns = DbEntity.MedicalAndInsurances.Where(objMedIns => objMedIns.StudentPersonalId == saveId).SingleOrDefault();
                            if (medicalIns != null)
                            {
                                medicalIns.SignificantBehaviorCharacteristics = model.RefSpecificProb;
                                DbEntity.SaveChanges();
                            }

                        }



                        retVal = saveId;
                    }
                    else
                    {
                        if (type == "General Information")
                        {
                            StudentTbl.FirstName = model.RefFrstName;
                            StudentTbl.LastName = model.RefLstName;
                            StudentTbl.MiddleName = model.RefMaidenName;
                            StudentTbl.SchoolId = schoolId;
                            StudentTbl.Height = Convert.ToDecimal(model.RefPresentHght);
                            StudentTbl.Weight = Convert.ToDecimal(model.RefPresntWeigth);
                            StudentTbl.HairColor = model.RefHairColor;
                            StudentTbl.EyeColor = model.RefEyecolor;
                            StudentTbl.MiddleName = model.RefMaidenName;
                            StudentTbl.PrimaryDiag = model.PrimaryDiag;
                            StudentTbl.SecondaryDiag = model.SecondaryDiag;
                            StudentTbl.SocialSecurityNo = model.SocialSecurityNo;
                            StudentTbl.SSINo = model.SsiNo;
                            StudentTbl.LocalId = "STD " + saveId;
                            StudentTbl.Gender = model.RefGender;
                            StudentTbl.AdmissionDate = (model.RefDate == null) ? (DateTime?)null : DateTime.ParseExact(model.RefDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            StudentTbl.BirthDate = (model.RefDOB == null) ? (DateTime?)null : DateTime.ParseExact(model.RefDOB, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            StudentTbl.StudentType = "Referral";
                            StudentTbl.CreatedBy = loginId;
                            StudentTbl.CreatedOn = DateTime.Now;
                            DbEntity.StudentPersonals.Add(StudentTbl);
                            DbEntity.SaveChanges();
                            model.RefPersonalId = StudentTbl.StudentPersonalId;

                            int addressType = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Address Type" && objLookup.LookupName == "Physical location address").Select(objlookup => objlookup.LookupId).Single();


                            //AddrList.CountryId = model.RefCountry;//DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Address Type" && objLookup.LookupName == "Physical location address").Select(objlookup => objlookup.LookupId).Single();
                            AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single();
                            AddrList.StateProvince = model.RefState;
                            AddrList.City = model.RefCity;
                            AddrList.StreetName = model.RefStreetAdrs;
                            AddrList.ApartmentType = model.RefAptUnit;
                            AddrList.PostalCode = model.RefZipCode;
                            AddrList.CreatedBy = loginId;
                            AddrList.CreatedOn = DateTime.Now;
                            DbEntity.AddressLists.Add(AddrList);
                            DbEntity.SaveChanges();
                            model.AddressId = AddrList.AddressId;

                            StdAdrRel.StudentPersonalId = model.RefPersonalId;
                            StdAdrRel.AddressId = model.AddressId;
                            StdAdrRel.ContactSequence = 0;
                            StdAdrRel.ContactPersonalId = 0;
                            StdAdrRel.CreatedBy = loginId;
                            StdAdrRel.CreatedOn = DateTime.Now;
                            DbEntity.StudentAddresRels.Add(StdAdrRel);
                            DbEntity.SaveChanges();
                            if ((model.objRelationMthr.txtFirstName != null && model.objRelationMthr.txtFirstName != "") && (model.objRelationMthr.txtLstName != null && model.objRelationMthr.txtLstName != ""))
                            {
                                ConPersonal.StudentPersonalId = model.RefPersonalId;
                                ConPersonal.FirstName = model.objRelationMthr.txtFirstName;
                                ConPersonal.LastName = model.objRelationMthr.txtLstName;
                                ConPersonal.MiddleName = model.objRelationMthr.txtMiddleName;
                                ConPersonal.ContactFlag = "Referral";
                                ConPersonal.Status = 1;
                                ConPersonal.CreatedBy = loginId;
                                ConPersonal.CreatedOn = DateTime.Now;
                                DbEntity.ContactPersonals.Add(ConPersonal);
                                DbEntity.SaveChanges();
                                model.ContactId = ConPersonal.ContactPersonalId;

                                AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single();// model.objRelationMthr.txtCountry;
                                AddrList.StateProvince = model.objRelationMthr.txtState;
                                AddrList.AddressType = addressType;
                                AddrList.City = model.objRelationMthr.txtCity;
                                AddrList.StreetName = model.objRelationMthr.txtStreetAdress;
                                AddrList.ApartmentType = model.objRelationMthr.txtAprtmentUnit;
                                AddrList.PostalCode = model.objRelationMthr.txtZipCode;
                                AddrList.Phone = model.objRelationMthr.txtHomePhone;
                                AddrList.OtherPhone = model.objRelationMthr.txtWorkPhone;
                                AddrList.PrimaryEmail = model.objRelationMthr.txtEmail;
                                AddrList.CreatedBy = loginId;
                                AddrList.CreatedOn = DateTime.Now;
                                DbEntity.AddressLists.Add(AddrList);
                                DbEntity.SaveChanges();
                                model.AddressId = AddrList.AddressId;

                                StdCntctRel.ContactPersonalId = model.ContactId;
                                StdCntctRel.RelationshipId = model.lukUpMthrId;
                                StdCntctRel.CreatedBy = loginId;
                                StdCntctRel.CreatedOn = DateTime.Now;
                                DbEntity.StudentContactRelationships.Add(StdCntctRel);
                                DbEntity.SaveChanges();

                                StdAdrRel.AddressId = AddrList.AddressId;
                                StdAdrRel.StudentPersonalId = StudentTbl.StudentPersonalId;
                                StdAdrRel.ContactPersonalId = ConPersonal.ContactPersonalId;
                                StdAdrRel.ContactSequence = 1;
                                StdAdrRel.CreatedBy = loginId;
                                StdAdrRel.CreatedOn = DateTime.Now;
                                DbEntity.StudentAddresRels.Add(StdAdrRel);
                                DbEntity.SaveChanges();
                            }
                            if ((model.objRelationFthr.txtFirstName != null && model.objRelationFthr.txtFirstName != "") && (model.objRelationFthr.txtLstName != null && model.objRelationFthr.txtLstName != ""))
                            {
                                ConPersonal.StudentPersonalId = model.RefPersonalId;
                                ConPersonal.FirstName = model.objRelationFthr.txtFirstName;
                                ConPersonal.LastName = model.objRelationFthr.txtLstName;
                                ConPersonal.MiddleName = model.objRelationFthr.txtMiddleName;
                                ConPersonal.ContactFlag = "Referral";
                                ConPersonal.Status = 1;
                                ConPersonal.CreatedBy = loginId;
                                ConPersonal.CreatedOn = DateTime.Now;
                                DbEntity.ContactPersonals.Add(ConPersonal);
                                DbEntity.SaveChanges();
                                model.ContactId = ConPersonal.ContactPersonalId;

                                AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objRelationFthr.txtCountry;
                                AddrList.StateProvince = model.objRelationFthr.txtState;
                                AddrList.City = model.objRelationFthr.txtCity;
                                AddrList.AddressType = addressType;
                                AddrList.StreetName = model.objRelationFthr.txtStreetAdress;
                                AddrList.ApartmentType = model.objRelationFthr.txtAprtmentUnit;
                                AddrList.PostalCode = model.objRelationFthr.txtZipCode;
                                AddrList.Phone = model.objRelationFthr.txtHomePhone;
                                AddrList.OtherPhone = model.objRelationFthr.txtWorkPhone;
                                AddrList.PrimaryEmail = model.objRelationFthr.txtEmail;
                                AddrList.CreatedBy = loginId;
                                AddrList.CreatedOn = DateTime.Now;
                                DbEntity.AddressLists.Add(AddrList);
                                DbEntity.SaveChanges();
                                model.AddressId = AddrList.AddressId;

                                StdCntctRel.ContactPersonalId = model.ContactId;
                                StdCntctRel.RelationshipId = model.lukUpFathrId;
                                StdCntctRel.CreatedBy = loginId;
                                StdCntctRel.CreatedOn = DateTime.Now;
                                DbEntity.StudentContactRelationships.Add(StdCntctRel);
                                DbEntity.SaveChanges();

                                StdAdrRel.AddressId = AddrList.AddressId;
                                StdAdrRel.StudentPersonalId = StudentTbl.StudentPersonalId;
                                StdAdrRel.ContactPersonalId = ConPersonal.ContactPersonalId;
                                StdAdrRel.ContactSequence = 1;
                                StdAdrRel.CreatedBy = loginId;
                                StdAdrRel.CreatedOn = DateTime.Now;
                                DbEntity.StudentAddresRels.Add(StdAdrRel);
                                DbEntity.SaveChanges();
                            }
                            if ((model.objRelationClose.txtFirstName != null && model.objRelationClose.txtFirstName != "") && (model.objRelationClose.txtLstName != null && model.objRelationClose.txtLstName != ""))
                            {
                                model.objRelationClose.RelationName = (-1).ToString();
                                ConPersonal.StudentPersonalId = model.RefPersonalId;
                                ConPersonal.ContactFlag = "Referral";
                                ConPersonal.Status = 1;
                                ConPersonal.FirstName = model.objRelationClose.txtFirstName;
                                ConPersonal.LastName = model.objRelationClose.txtLstName;
                                ConPersonal.MiddleName = model.objRelationClose.txtMiddleName;
                                ConPersonal.Relation = Convert.ToInt32(model.objRelationClose.RelationName);
                                ConPersonal.CreatedBy = loginId;
                                ConPersonal.CreatedOn = DateTime.Now;
                                DbEntity.ContactPersonals.Add(ConPersonal);
                                DbEntity.SaveChanges();
                                model.ContactId = ConPersonal.ContactPersonalId;


                                AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objRelationClose.txtCountry;
                                AddrList.StateProvince = model.objRelationClose.txtState;
                                AddrList.City = model.objRelationClose.txtCity;
                                AddrList.AddressType = addressType;
                                AddrList.StreetName = model.objRelationClose.txtStreetAdress;
                                AddrList.ApartmentType = model.objRelationClose.txtAprtmentUnit;
                                AddrList.PostalCode = model.objRelationClose.txtZipCode;
                                AddrList.Phone = model.objRelationClose.txtHomePhone;
                                AddrList.OtherPhone = model.objRelationClose.txtWorkPhone;
                                AddrList.PrimaryEmail = model.objRelationClose.txtEmail;
                                AddrList.CreatedBy = loginId;
                                AddrList.CreatedOn = DateTime.Now;
                                DbEntity.AddressLists.Add(AddrList);
                                DbEntity.SaveChanges();
                                model.AddressId = AddrList.AddressId;

                                StdCntctRel.ContactPersonalId = model.ContactId;
                                StdCntctRel.RelationshipId = model.objRelationClose.closeRelation;
                                StdCntctRel.CreatedBy = loginId;
                                StdCntctRel.CreatedOn = DateTime.Now;
                                DbEntity.StudentContactRelationships.Add(StdCntctRel);
                                DbEntity.SaveChanges();


                                StdAdrRel.AddressId = AddrList.AddressId;
                                StdAdrRel.StudentPersonalId = StudentTbl.StudentPersonalId;
                                StdAdrRel.ContactPersonalId = ConPersonal.ContactPersonalId;
                                StdAdrRel.ContactSequence = 1;
                                StdAdrRel.CreatedBy = loginId;
                                StdAdrRel.CreatedOn = DateTime.Now;
                                DbEntity.StudentAddresRels.Add(StdAdrRel);
                                DbEntity.SaveChanges();
                            }

                            ConPersonal.Relation = null;
                            if ((model.objRelationLegalGuardian.txtFirstName != null && model.objRelationLegalGuardian.txtFirstName != "") && (model.objRelationLegalGuardian.txtLstName != null && model.objRelationLegalGuardian.txtLstName != ""))
                            {
                                ConPersonal.StudentPersonalId = model.RefPersonalId;
                                ConPersonal.FirstName = model.objRelationLegalGuardian.txtFirstName;
                                ConPersonal.LastName = model.objRelationLegalGuardian.txtLstName;
                                ConPersonal.MiddleName = model.objRelationLegalGuardian.txtMiddleName;
                                ConPersonal.ContactFlag = "Referral";
                                ConPersonal.Status = 1;
                                ConPersonal.CreatedBy = loginId;
                                ConPersonal.CreatedOn = DateTime.Now;
                                DbEntity.ContactPersonals.Add(ConPersonal);
                                DbEntity.SaveChanges();
                                model.ContactId = ConPersonal.ContactPersonalId;

                                AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objRelationLegalGuardian.txtCountry;
                                AddrList.StateProvince = model.objRelationLegalGuardian.txtState;
                                AddrList.City = model.objRelationLegalGuardian.txtCity;
                                AddrList.AddressType = addressType;
                                AddrList.StreetName = model.objRelationLegalGuardian.txtStreetAdress;
                                AddrList.ApartmentType = model.objRelationLegalGuardian.txtAprtmentUnit;
                                AddrList.PostalCode = model.objRelationLegalGuardian.txtZipCode;
                                AddrList.Phone = model.objRelationLegalGuardian.txtHomePhone;
                                AddrList.OtherPhone = model.objRelationLegalGuardian.txtWorkPhone;
                                AddrList.PrimaryEmail = model.objRelationLegalGuardian.txtEmail;
                                AddrList.CreatedBy = loginId;
                                AddrList.CreatedOn = DateTime.Now;
                                DbEntity.AddressLists.Add(AddrList);
                                DbEntity.SaveChanges();
                                model.AddressId = AddrList.AddressId;

                                StdCntctRel.ContactPersonalId = model.ContactId;
                                StdCntctRel.RelationshipId = model.lukupLegalGurdianId;
                                StdCntctRel.CreatedBy = loginId;
                                StdCntctRel.CreatedOn = DateTime.Now;
                                DbEntity.StudentContactRelationships.Add(StdCntctRel);
                                DbEntity.SaveChanges();

                                StdAdrRel.AddressId = AddrList.AddressId;
                                StdAdrRel.StudentPersonalId = StudentTbl.StudentPersonalId;
                                StdAdrRel.ContactPersonalId = ConPersonal.ContactPersonalId;
                                StdAdrRel.ContactSequence = 1;
                                StdAdrRel.CreatedBy = loginId;
                                StdAdrRel.CreatedOn = DateTime.Now;
                                DbEntity.StudentAddresRels.Add(StdAdrRel);
                                DbEntity.SaveChanges();
                            }
                            if ((model.objRelationEmergncyContact.txtFirstName != null && model.objRelationEmergncyContact.txtFirstName != "") && (model.objRelationEmergncyContact.txtLstName != null && model.objRelationEmergncyContact.txtLstName != ""))
                            {
                                ConPersonal.StudentPersonalId = model.RefPersonalId;
                                ConPersonal.ContactFlag = "Referral";
                                ConPersonal.Status = 1;
                                ConPersonal.FirstName = model.objRelationEmergncyContact.txtFirstName;
                                ConPersonal.LastName = model.objRelationEmergncyContact.txtLstName;
                                ConPersonal.MiddleName = model.objRelationEmergncyContact.txtMiddleName;
                                ConPersonal.Relation = model.objRelationEmergncyContact.EmRelationName;
                                ConPersonal.CreatedBy = loginId;
                                ConPersonal.CreatedOn = DateTime.Now;
                                DbEntity.ContactPersonals.Add(ConPersonal);
                                DbEntity.SaveChanges();
                                model.ContactId = ConPersonal.ContactPersonalId;

                                AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objRelationEmergncyContact.txtCountry;
                                AddrList.StateProvince = model.objRelationEmergncyContact.txtState;
                                AddrList.City = model.objRelationEmergncyContact.txtCity;
                                AddrList.AddressType = addressType;
                                AddrList.StreetName = model.objRelationEmergncyContact.txtStreetAdress;
                                AddrList.ApartmentType = model.objRelationEmergncyContact.txtAprtmentUnit;
                                AddrList.PostalCode = model.objRelationEmergncyContact.txtZipCode;
                                AddrList.Phone = model.objRelationEmergncyContact.txtHomePhone;
                                AddrList.OtherPhone = model.objRelationEmergncyContact.txtWorkPhone;
                                AddrList.PrimaryEmail = model.objRelationEmergncyContact.txtEmail;
                                AddrList.CreatedBy = loginId;
                                AddrList.CreatedOn = DateTime.Now;
                                DbEntity.AddressLists.Add(AddrList);
                                DbEntity.SaveChanges();
                                model.AddressId = AddrList.AddressId;

                                StdCntctRel.ContactPersonalId = model.ContactId;
                                StdCntctRel.RelationshipId = model.lukupEmergencyId;
                                StdCntctRel.CreatedBy = loginId;
                                StdCntctRel.CreatedOn = DateTime.Now;
                                DbEntity.StudentContactRelationships.Add(StdCntctRel);
                                DbEntity.SaveChanges();

                                StdAdrRel.AddressId = AddrList.AddressId;
                                StdAdrRel.StudentPersonalId = StudentTbl.StudentPersonalId;
                                StdAdrRel.ContactPersonalId = ConPersonal.ContactPersonalId;
                                StdAdrRel.ContactSequence = 1;
                                StdAdrRel.CreatedBy = loginId;
                                StdAdrRel.CreatedOn = DateTime.Now;
                                DbEntity.StudentAddresRels.Add(StdAdrRel);
                                DbEntity.SaveChanges();
                            }
                            retVal = model.RefPersonalId;


                        }
                        else if (type == "General Family Background Information")
                        {
                            ClsErrorLog erorLog = new ClsErrorLog();
                            try
                            {
                                int mothrConPersId = (from objStudntPersnl in DbEntity.StudentPersonals
                                                      join StdAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals StdAddrRel.StudentPersonalId
                                                      join ConPrsonal in DbEntity.ContactPersonals on StdAddrRel.ContactPersonalId equals ConPrsonal.ContactPersonalId
                                                      join Addr in DbEntity.AddressLists on StdAddrRel.AddressId equals Addr.AddressId
                                                      join StudContactRel in DbEntity.StudentContactRelationships on ConPrsonal.ContactPersonalId equals StudContactRel.ContactPersonalId
                                                      where (StudContactRel.RelationshipId == model.lukUpMthrId & StdAddrRel.ContactSequence == 1 & objStudntPersnl.StudentPersonalId == saveId
                                                      && ConPrsonal.ContactPersonalId == StdAddrRel.ContactPersonalId)
                                                      select new
                                                      {
                                                          ContactId = ConPrsonal.ContactPersonalId
                                                      }).SingleOrDefault().ContactId;
                                erorLog.WriteToLog("mothrConPersId : " + mothrConPersId);

                                int FthrConPersId = (from objStudntPersnl in DbEntity.StudentPersonals
                                                     join StdAddrRel in DbEntity.StudentAddresRels on objStudntPersnl.StudentPersonalId equals StdAddrRel.StudentPersonalId
                                                     join ConPrsonal in DbEntity.ContactPersonals on StdAddrRel.ContactPersonalId equals ConPrsonal.ContactPersonalId
                                                     join Addr in DbEntity.AddressLists on StdAddrRel.AddressId equals Addr.AddressId
                                                     join StudContactRel in DbEntity.StudentContactRelationships on ConPrsonal.ContactPersonalId equals StudContactRel.ContactPersonalId
                                                     where (StudContactRel.RelationshipId == model.lukUpFathrId & StdAddrRel.ContactSequence == 1 & objStudntPersnl.StudentPersonalId == saveId
                                                     && ConPrsonal.ContactPersonalId == StdAddrRel.ContactPersonalId)
                                                     select new
                                                     {
                                                         ContactId = ConPrsonal.ContactPersonalId
                                                     }).SingleOrDefault().ContactId;

                                erorLog.WriteToLog("FthrConPersId : " + FthrConPersId);

                                ConPersonal = DbEntity.ContactPersonals.Where(objContPers => objContPers.ContactPersonalId == mothrConPersId).SingleOrDefault();
                                ConPersonal.BirthDate = (model.objRelationMthr.txtDob == null) ? (DateTime?)null : DateTime.ParseExact(model.objRelationMthr.txtDob, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                ConPersonal.CountryOfCitizenship = model.objRelationMthr.txtUScitzen;
                                ConPersonal.Employer = model.objRelationMthr.txtEmployer;
                                ConPersonal.Occupation = model.objRelationMthr.txtOccupation;
                                ConPersonal.MaritalStatus = model.objRelationMthr.txtMaritalStatus;
                                DbEntity.SaveChanges();

                                ConPersonal = DbEntity.ContactPersonals.Where(objContPers => objContPers.ContactPersonalId == FthrConPersId).SingleOrDefault();
                                ConPersonal.BirthDate = (model.objRelationFthr.txtDob == null) ? (DateTime?)null : DateTime.ParseExact(model.objRelationFthr.txtDob, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                ConPersonal.CountryOfCitizenship = model.objRelationFthr.txtUScitzen;
                                ConPersonal.Employer = model.objRelationFthr.txtEmployer;
                                ConPersonal.Occupation = model.objRelationFthr.txtOccupation;
                                DbEntity.SaveChanges();
                            }
                            catch (Exception ex)
                            {

                                erorLog.WriteToLog("SaveId : " + saveId);
                                erorLog.WriteToLog(ex.ToString());
                            }
                            retVal = saveId;

                        }

                        else if (type == "Birth And Development History")
                        {
                            if (saveId > 0)
                            {
                                StudentTbl = DbEntity.StudentPersonals.Where(objStudent => objStudent.StudentPersonalId == saveId).SingleOrDefault();
                                StudentTbl.PlaceOfBirth = model.RefbirthPlace;
                                StudentTbl.StateOfBirth = model.RefStateBirth;
                                StudentTbl.CountryOfBirth = model.RefCntryBirth;
                                DbEntity.SaveChanges();

                                Diagnose.StudentPersonalId = saveId;
                                Diagnose.Diaganoses = model.RefDiagnosis;
                                Diagnose.CreatedBy = loginId;
                                Diagnose.CreatedOn = DateTime.Now;
                                DbEntity.DiaganosesPAs.Add(Diagnose);
                                DbEntity.SaveChanges();

                                StudPersPa.StudentPersonalId = saveId;
                                StudPersPa.Allergies = model.RefAllergies;
                                StudPersPa.CreatedBy = loginId;
                                StudPersPa.CreatedOn = DateTime.Now;
                                DbEntity.StudentPersonalPAs.Add(StudPersPa);
                                DbEntity.SaveChanges();

                                AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objPhysicianDetails.txtCountry;
                                AddrList.StateProvince = model.objPhysicianDetails.txtState;
                                AddrList.City = model.objPhysicianDetails.txtCity;
                                AddrList.StreetName = model.objPhysicianDetails.txtStreetAdress;
                                AddrList.PostalCode = model.objPhysicianDetails.txtZipCode;
                                AddrList.Phone = model.objPhysicianDetails.txtHomePhone;
                                DbEntity.AddressLists.Add(AddrList);
                                DbEntity.SaveChanges();
                                model.AddressId = AddrList.AddressId;

                                medicalIns.StudentPersonalId = saveId;
                                medicalIns.AddressId = model.AddressId;
                                medicalIns.FirstName = model.objPhysicianDetails.RefPrimPhyName;
                                medicalIns.DateOfLastPhysicalExam = model.objPhysicianDetails.PhylstApmnt;
                                medicalIns.CreatedBy = loginId;
                                medicalIns.CreatedOn = DateTime.Now;
                                DbEntity.MedicalAndInsurances.Add(medicalIns);
                                DbEntity.SaveChanges();

                                AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objInsuranceDetails.txtCountry;
                                AddrList.StateProvince = model.objInsuranceDetails.txtState;
                                AddrList.City = model.objInsuranceDetails.txtCity;
                                AddrList.StreetName = model.objInsuranceDetails.txtStreetAdress;
                                AddrList.PostalCode = model.objInsuranceDetails.txtZipCode;
                                DbEntity.AddressLists.Add(AddrList);
                                DbEntity.SaveChanges();
                                model.AddressId = AddrList.AddressId;

                                Insurnce.StudentPersonalId = saveId;
                                Insurnce.AddressId = model.AddressId;
                                Insurnce.PreferType = "Primary";
                                Insurnce.InsuranceType = model.objInsuranceDetails.InsuranceCoverage;
                                Insurnce.PolicyNumber = model.objInsuranceDetails.InsurancePolNum;
                                Insurnce.CompanyName = model.objInsuranceDetails.RefInsuranceCompany;
                                Insurnce.SchoolId = schoolId;
                                DbEntity.Insurances.Add(Insurnce);
                                DbEntity.SaveChanges();

                                AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objInsuranceSecDetails.txtCountry;
                                AddrList.StateProvince = model.objInsuranceSecDetails.txtState;
                                AddrList.City = model.objInsuranceSecDetails.txtCity;
                                AddrList.StreetName = model.objInsuranceSecDetails.txtStreetAdress;
                                AddrList.PostalCode = model.objInsuranceSecDetails.txtZipCode;
                                DbEntity.AddressLists.Add(AddrList);
                                DbEntity.SaveChanges();
                                model.AddressId = AddrList.AddressId;

                                Insurnce.StudentPersonalId = saveId;
                                Insurnce.AddressId = model.AddressId;
                                Insurnce.PreferType = "Secondary";
                                Insurnce.InsuranceType = model.objInsuranceSecDetails.InsuranceCoverage;
                                Insurnce.PolicyNumber = model.objInsuranceSecDetails.InsurancePolNum;
                                Insurnce.CompanyName = model.objInsuranceSecDetails.RefInsuranceCompany;
                                Insurnce.SchoolId = schoolId;
                                DbEntity.Insurances.Add(Insurnce);
                                DbEntity.SaveChanges();

                                AddrList.CountryId = DbEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupName == "United States of America").Select(objlookup => objlookup.LookupId).Single(); //model.objInsuranceDentalDetails.txtCountry;
                                AddrList.StateProvince = model.objInsuranceDentalDetails.txtState;
                                AddrList.City = model.objInsuranceDentalDetails.txtCity;
                                AddrList.StreetName = model.objInsuranceDentalDetails.txtStreetAdress;
                                AddrList.PostalCode = model.objInsuranceDentalDetails.txtZipCode;
                                DbEntity.AddressLists.Add(AddrList);
                                DbEntity.SaveChanges();
                                model.AddressId = AddrList.AddressId;

                                Insurnce.StudentPersonalId = saveId;
                                Insurnce.AddressId = model.AddressId;
                                Insurnce.PreferType = "Dental";
                                Insurnce.InsuranceType = model.objInsuranceDentalDetails.InsuranceCoverage;
                                Insurnce.PolicyNumber = model.objInsuranceDentalDetails.InsurancePolNum;
                                Insurnce.CompanyName = model.objInsuranceDentalDetails.RefInsuranceCompany;
                                Insurnce.SchoolId = schoolId;
                                DbEntity.Insurances.Add(Insurnce);
                                DbEntity.SaveChanges();


                                retVal = saveId;

                            }
                        }
                        else if (type == "Personal History")
                        {
                            MedicalAndInsurance med = new MedicalAndInsurance();
                            try
                            {
                                medicalIns = DbEntity.MedicalAndInsurances.Where(objMedIns => objMedIns.StudentPersonalId == saveId).SingleOrDefault();
                            }
                            catch
                            {

                            }
                            if (medicalIns != null)
                            {
                                medicalIns.SignificantBehaviorCharacteristics = model.RefSpecificProb;
                                DbEntity.SaveChanges();
                            }
                            else
                            {
                                med.StudentPersonalId = saveId;
                                med.SignificantBehaviorCharacteristics = model.RefSpecificProb;
                                med.CreatedBy = loginId;
                                med.CreatedOn = DateTime.Now;
                                DbEntity.MedicalAndInsurances.Add(med);
                                DbEntity.SaveChanges();
                            }
                            retVal = saveId;

                        }
                        else if (type == "Recreational Activities")
                        {

                            retVal = saveId;

                        }
                        else if (type == "Present Self-Help Skills, Social Skills and Mobility")
                        {

                            retVal = saveId;

                        }
                        else if (type == "Funding Information")
                        {

                            retVal = saveId;

                        }

                    }
                    trans.Complete();
                }
            }
            catch
            {

            }
            return retVal;
        }



        public string GetStudentImage(int studentId)
        {
            MelmarkDBEntities objData = new MelmarkDBEntities();
            string retValue = "";
            var ImageUrl = (from StudPersonal in objData.StudentPersonals
                            where StudPersonal.StudentPersonalId == studentId
                            select new
                            {
                                image = StudPersonal.ImageUrl
                            }).SingleOrDefault();

            if (ImageUrl.image != null)
            {
                retValue = ImageUrl.image.ToString();
            }

            return retValue;

        }


        public IEnumerable<SelectListItem> FillDropList(string status)
        {
            MelmarkDBEntities DbEntity = new MelmarkDBEntities();
            GenInfoModel model = new GenInfoModel();

            IEnumerable<SelectListItem> List = (from objLookup in DbEntity.LookUps
                                                where (objLookup.LookupType == status)
                                                select new SelectListItem
                                                {
                                                    Text = objLookup.LookupName,
                                                    Value = SqlFunctions.StringConvert((double)objLookup.LookupId).Trim()

                                                });
            return List;
        }

        public IEnumerable<SelectListItem> FillState(int id)
        {
            MelmarkDBEntities dbEntity = new MelmarkDBEntities();
            IEnumerable<SelectListItem> List = (from objLookup in dbEntity.LookUps
                                                where (objLookup.ParentLookupId == id)
                                                select new SelectListItem
                                                {
                                                    Text = objLookup.LookupName,
                                                    Value = SqlFunctions.StringConvert((double)objLookup.LookupId).Trim()
                                                });
            return List;
        }


        public IEnumerable<SelectListItem> FillGender()
        {
            List<SelectListItem> obj = new List<SelectListItem>();
            obj.Add(new SelectListItem { Text = "Male", Value = "1" });
            obj.Add(new SelectListItem { Text = "Female", Value = "2" });
            return obj;
        }

        public IEnumerable<SelectListItem> FillMaritalStatus()
        {
            MelmarkDBEntities dbEntity = new MelmarkDBEntities();
            IEnumerable<SelectListItem> List = (from objlukup in dbEntity.LookUps
                                                where (objlukup.LookupType == "MaritalStatus")
                                                select new SelectListItem
                                                {
                                                    Text = objlukup.LookupName,
                                                    Value = SqlFunctions.StringConvert((double)objlukup.LookupId).Trim()
                                                });
            return List;
        }



        public IEnumerable<SelectListItem> FillDoccumentType()
        {
            MelmarkDBEntities dbEntity = new MelmarkDBEntities();
            IEnumerable<SelectListItem> List = (from objlukup in dbEntity.LookUps
                                                where (objlukup.LookupType == "Document Type")
                                                select new SelectListItem
                                                {
                                                    Text = objlukup.LookupName,
                                                    Value = SqlFunctions.StringConvert((double)objlukup.LookupId).Trim()
                                                });
            return List;
        }


        public IEnumerable<SelectListItem> FillRaceType()
        {
            MelmarkDBEntities dbEntity = new MelmarkDBEntities();
            IEnumerable<SelectListItem> List = (from objlukup in dbEntity.LookUps
                                                where (objlukup.LookupType == "Race")
                                                select new SelectListItem
                                                {
                                                    Text = objlukup.LookupName,
                                                    Value = SqlFunctions.StringConvert((double)objlukup.LookupId).Trim()
                                                });
            return List;
        }

        public IEnumerable<SelectListItem> FillBehaviorScore()
        {
            MelmarkDBEntities dbEntity = new MelmarkDBEntities();
            IEnumerable<SelectListItem> List = (from objlukup in dbEntity.LookUps
                                                where (objlukup.LookupType == "BehaviorScore")
                                                select new SelectListItem
                                                {
                                                    Text = objlukup.LookupName,
                                                    Value = SqlFunctions.StringConvert((double)objlukup.LookupId).Trim()
                                                });
            return List;
        }


        public IEnumerable<clsBehaviorCategry> FillBehaviorCategory(string parentName, GenInfoModel model)
        {
            MelmarkDBEntities dbEntity = new MelmarkDBEntities();

            model.objclsBehavior = (from objBehaveCat in dbEntity.BehaviorCategories
                                    where (objBehaveCat.ParentCategryName == parentName)
                                    select new clsBehaviorCategry
                                    {
                                        behaviorName = objBehaveCat.CategoryName,
                                        behaviorId = objBehaveCat.BehaviorCategryId
                                    }).ToList();

            return model.objclsBehavior;
        }



        public IEnumerable<clsBehaviorCategry> FillBehaviorCategoryOnStudentId(int RefferalId, GenInfoModel model, string parentName)
        {
            MelmarkDBEntities dbEntity = new MelmarkDBEntities();

            model.objclsBehavior = (from objBehaviorPa in dbEntity.BehavioursPAs
                                    join objBehaviorCat in dbEntity.BehaviorCategories
                                        on objBehaviorPa.BehaviorId equals objBehaviorCat.BehaviorCategryId
                                    where (objBehaviorPa.StudentPersonalId == RefferalId & objBehaviorCat.ParentCategryName == parentName)
                                    select new clsBehaviorCategry
                                    {
                                        behaviorId = objBehaviorPa.BehaviorId,
                                        behaviorName = objBehaviorCat.CategoryName,
                                        scoreId = objBehaviorPa.Score

                                    }).ToList();

            return model.objclsBehavior;
        }


        //public IList<string> FillBehaviorCategory(string parentName)
        //{
        //    MelmarkDBEntities dbEntity = new MelmarkDBEntities();
        //    var listName = (from objBehaveCat in dbEntity.BehaviorCategories
        //                             where (objBehaveCat.ParentCategryName == parentName)
        //                             select new 
        //                             {

        //                                 text = objBehaveCat.CategoryName
        //                             }).ToList();
        //    IList<string> strList =new List<string>();
        //    foreach (var item in listName)
        //    {
        //        strList.Add(item.text);
        //    }
        //    return strList;


        //}


        public int FileUpload(int StudentId, int SchoolId, int LoginId, string DocName, string OtherName, string DocPath, int DocType)
        {
            MelmarkDBEntities objData = new MelmarkDBEntities();
            LookUp doctype = new LookUp();
            doctype = objData.LookUps.Where(objlk => objlk.LookupId == DocType).SingleOrDefault();
            int rtrnval = -1;

            if (doctype.LookupName == "Other")
            {
                Document tblDoc = new Document();
                tblDoc.DocumentName = DocName;
                tblDoc.Other = OtherName;
                tblDoc.QueueStatusId = 0;
                tblDoc.DocumentType = DocType;
                tblDoc.DocumentPath = DocPath;
                tblDoc.SchoolId = SchoolId;
                tblDoc.StudentPersonalId = StudentId;
                tblDoc.Status = true;
                tblDoc.UserType = "Staff";
                tblDoc.CreatedBy = LoginId;
                tblDoc.CreatedOn = DateTime.Now;
                objData.Documents.Add(tblDoc);
                objData.SaveChanges();
                rtrnval = tblDoc.DocumentId;

                return rtrnval;

            }
            else
            {
                Document tblDoc = new Document();
                tblDoc.DocumentName = DocName;
                tblDoc.QueueStatusId = 0;
                tblDoc.DocumentType = DocType;
                tblDoc.DocumentPath = DocPath;
                tblDoc.SchoolId = SchoolId;
                tblDoc.StudentPersonalId = StudentId;
                tblDoc.Status = true;
                tblDoc.UserType = "Staff";
                tblDoc.CreatedBy = LoginId;
                tblDoc.CreatedOn = DateTime.Now;
                objData.Documents.Add(tblDoc);
                objData.SaveChanges();
                rtrnval = tblDoc.DocumentId;


                return rtrnval;

            }


        }


        public int SaveCurrentTab(int studentId, string TabName)
        {
            //session = (clsSession)HttpContext.Current.Session["UserSession"];
            MelmarkDBEntities objData = new MelmarkDBEntities();
            int returnVal = -1;
            //studentId = Convert.ToInt32(session.ReferralId);
            TabDefinition tabDef = new TabDefinition();
            tabDef.StudentId = studentId;
            tabDef.TabName = TabName;
            tabDef.CreatedBy = 1;
            tabDef.CreatedOn = DateTime.Now;
            objData.TabDefinitions.Add(tabDef);
            objData.SaveChanges();

            returnVal = tabDef.TabId;
            return returnVal;
        }

        //public int StudentUpldPhoto(int studentId, string Path)
        //{
        //    MelmarkDBEntities objData = new MelmarkDBEntities();
        //    int returnVal = -1;
        //    StudentPersonal Sp = new StudentPersonal();
        //    Sp = objData.StudentPersonals.Where(objStudent => objStudent.StudentPersonalId == studentId).SingleOrDefault();
        //    Sp.ImageUrl = Path;
        //    objData.SaveChanges();
        //    returnVal = Sp.StudentPersonalId;

        //    return returnVal;
        //}


        public int StudentUpldPhoto(int studentId, byte[] Path)
        {
            MelmarkDBEntities objData = new MelmarkDBEntities();
            int returnVal = -1;
            StudentPersonal Sp = new StudentPersonal();
            Sp = objData.StudentPersonals.Where(objStudent => objStudent.StudentPersonalId == studentId).SingleOrDefault();
            Sp.ImageUrl = Convert.ToBase64String(Path);
            objData.SaveChanges();
            returnVal = Sp.StudentPersonalId;

            return returnVal;
        }





        public void SaveXmlToDB(byte[] xmlData, int id)
        {
            MelmarkDBEntities DbEntity = new MelmarkDBEntities();
            StudentPersonal Sp = new StudentPersonal();
            Sp = DbEntity.StudentPersonals.Where(objStudPersonal => objStudPersonal.StudentPersonalId == id).SingleOrDefault();
            Sp.StudentXMLData = xmlData;
            DbEntity.SaveChanges();

        }



        public byte[] SaveAsBlob(string studXml)
        {
            byte[] byteArray = null;

            using (FileStream fs = new FileStream
                (studXml, FileMode.Open, FileAccess.Read, FileShare.Read))
            {

                byteArray = new byte[fs.Length];

                int iBytesRead = fs.Read(byteArray, 0, (int)fs.Length);
            }
            return byteArray;
        }
        /// <summary>
        /// Load the xmldocument from the byte array
        /// </summary>
        /// <param name="XMLName"></param>
        /// <returns></returns>
        public XmlDocument LoadXmlfromBlob(int studId)
        {

            XmlDocument oDoc = new XmlDocument();
            byte[] buffer = (byte[])ReturnXmlData(studId);
            if (buffer != null)
            {

                using (System.IO.MemoryStream oByteStream = new System.IO.MemoryStream(buffer)) //To Load the xml taken from the database into a XmlDocument object
                {
                    using (System.Xml.XmlTextReader oRD = new System.Xml.XmlTextReader(oByteStream))
                    {
                        oDoc.Load(oRD);
                    }
                }
            }
            return oDoc;
        }
        public bool findNewAppFlag(int id)
        {

            MelmarkDBEntities DbEntity = new MelmarkDBEntities();
            StudentPersonal Sp = new StudentPersonal();
            Sp = DbEntity.StudentPersonals.Where(objStudPersonal => objStudPersonal.StudentPersonalId == id).SingleOrDefault();
            // string Flag = "";
            bool Flag = (bool)Sp.NewApplication;
            return Flag;
        }
        public byte[] ReturnXmlData(int studId)
        {
            MelmarkDBEntities DbEntity = new MelmarkDBEntities();
            byte[] xml = null;
            try
            {
                xml = (from objStudPersnl in DbEntity.StudentPersonals
                       where (objStudPersnl.StudentPersonalId == studId)
                       select new
                       {
                           xmlData = objStudPersnl.StudentXMLData
                       }).SingleOrDefault().xmlData;
            }
            catch
            {
            }
            return xml;

        }

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


        public bool IsModel(XmlNode cell)
        {
            if (cell.Attributes["modelName"] != null)
                return true;
            else
                return false;


        }
        public bool IsCheckName(XmlNode cell)
        {
            if (cell.Attributes["name"] != null)
                return true;
            else
                return false;


        }

        public int countryID()
        {
            MelmarkDBEntities DbEntity = new MelmarkDBEntities();
            LookUp Sp = new LookUp();
            Sp = DbEntity.LookUps.Where(objLookUp => objLookUp.LookupName == "United States of America").SingleOrDefault();
            return Sp.LookupId;
        }

    }

}