using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReferalDB.Models
{
    public class GenInfoModel
    {
        public RelationCLs objRelationFthr { get; set; }
        public RelationCLs objRelationMthr { get; set; }
        public RelationCLs objRelationClose { get; set; }
      
        public RelationCLs objPhysicianDetails { get; set; }
        public RelationCLs objInsuranceDetails { get; set; }
        public RelationCLs objInsuranceSecDetails { get; set; }
        public RelationCLs objInsuranceDentalDetails { get; set; }
        public RelationCLs objRelationLegalGuardian { get; set; }
        public RelationCLs objRelationEmergncyContact { get; set; }
        public clsUploadDownload objclsUpld { get; set; }
        public IEnumerable<clsBehaviorCategry> objclsBehavior { get; set; }

        //public SelfHelpSkills objToiletSelfskills { get; set; }


        public GenInfoModel()
        {
            objRelationFthr = new RelationCLs();
            objRelationMthr = new RelationCLs();
            objRelationClose = new RelationCLs();
          
            objPhysicianDetails = new RelationCLs();
            objInsuranceDetails = new RelationCLs();
            objInsuranceSecDetails = new RelationCLs();
            objInsuranceDentalDetails = new RelationCLs();
            objRelationLegalGuardian = new RelationCLs();
            objRelationEmergncyContact = new RelationCLs();
            objclsUpld = new clsUploadDownload();
            objclsBehavior = new List<clsBehaviorCategry>();


            //objToiletSelfskills = new SelfHelpSkills();

        }

        public int RefPersonalId { get; set; }
        public int AddressId { get; set; }
        public int ContactId { get; set; }
        public int lukUpFathrId { get; set; }
        public int lukUpMthrId { get; set; }
        public int lukupCloseId { get; set; }
        public int lukupLegalGurdianId { get; set; }
        public int lukupEmergencyId { get; set; }




        public string RefFrstName { get; set; }
        public string RefLstName { get; set; }
        public string RefMaidenName { get; set; }
        public DateTime? RefDateDatetime { get; set; }
        public string RefDate { get; set; }
        public string RefPrimaryDiagnosis { get; set; }
        public string RefSecondaryDiagnosis { get; set; }
        public DateTime? RefDOBDateTime { get; set; }
        public string RefDOB { get; set; }

        public string PrimaryDiag { get; set; }
        public string SecondaryDiag { get; set; }
        public string SocialSecurityNo { get; set; }
        public string SsiNo { get; set; }



        public string RefSocSecNo { get; set; }
        public decimal? RefPresentHght { get; set; }
        public decimal? RefPresntWeigth { get; set; }
        public string RefSSiNo { get; set; }
        public string RefHairColor { get; set; }
        public string RefEyecolor { get; set; }
        public string RefAptUnit { get; set; }
        public string RefStreetAdrs { get; set; }
        public string RefCity { get; set; }
        public string RefGender { get; set; }
        public string StudentType { get; set; }
        public string RefZipCode { get; set; }
        public int? RefCountry { get; set; }
        public int? RefState { get; set; }
        public string RefUpldPhoto { get; set; }
        public string RefAnyIdentification { get; set; }
        public int? RefRace { get; set; }
        public string RefEthinicity { get; set; }
        public string RefReligiousAffilation { get; set; }


        public string RefbirthPlace { get; set; }
        public int? RefCntryBirth { get; set; }
        public int? RefStateBirth { get; set; }

        public string RefDiagnosis { get; set; }
        public string RefAllergies { get; set; }


        public string RefSpecificProb { get; set; }

        //  public string RefPrimPhyName { get; set; }
        //public string RefInsuranceCompany { get; set; }
        //public string InsuranceCoverage { get; set; }
        //public string InsurancePolNum { get; set; }





        public string html { get; set; }
        public int TabId { get; set; }




    }


    public class RelationCLs
    {
        public string txtFirstName { get; set; }
        public string txtMiddleName { get; set; }
        public string txtLstName { get; set; }
        public int? txtCountry { get; set; }
        public string txtCounty { get; set; }
        public string txtOccupation { get; set; }
        public int? txtState { get; set; }
       
        public string txtStreetAdress { get; set; }

        public string txtAge { get; set; }
        public DateTime? txtDobdatetime { get; set; }
        public string txtDob { get; set; }
        public string txtUScitzen { get; set; }
        public string txtEmployer { get; set; }
        public string txtStreetAdrs { get; set; }
        public string txtSuite { get; set; }
        public string txtCity { get; set; }
        public string txtAprtmentUnit { get; set; }
        public string txtZipCode { get; set; }
        public string txtHomePhone { get; set; }
        public string txtWorkPhone { get; set; }
        public string txtMobileNum { get; set; }
        public string txtFax { get; set; }
        public string txtEmail { get; set; }

        public string txtEducation { get; set; }
        public string BusinessAddress { get; set; }
        public string GrossIncome { get; set; }

        public int? txtMaritalStatus { get; set; }
        public string Occupation { get; set; }


        public string RefBloodType { get; set; }
        public string RefAllergiesReaction { get; set; }
        public string RefSummaryCurrentHealth { get; set; }

        public string RefPrimPhyName { get; set; }
        public string phySpeciality { get; set; }
        public string PhylstApmntdateTime { get; set; }
        public string PhylstApmnt { get; set; }

        public string RefInsuranceCompany { get; set; }
        public string InsuranceCoverage { get; set; }
        public string InsurancePolNum { get; set; }
        public string RelationName { get; set; }
        public string LgRelationName { get; set; }
        public string EmRelationName { get; set; }
        public string causedeath { get; set; }

        public int closeRelation { get; set; }
        public int LegalGrRelation { get; set; }
        public int EmConRelation { get; set; }
      
    }


    public class clsUploadDownload
    {
        public int DocType { get; set; }
        public string DocName { get; set; }
        public string OtherName { get; set; }
    }

    public class GridDocument
    {
        public virtual string DocumentName { get; set; }
        public virtual string Other { get; set; }
        public virtual string OtherDocumentType { get; set; }

    }


    public class clsBehaviorCategry
    {
        public int? behaviorId { get; set; }
        public string behaviorName { get; set; }
        public int? scoreId { get; set; }


        //public int behavior1 { get; set; }
        //public int behavior2 { get; set; }
        //public int behavior3 { get; set; }
        //public int behavior4 { get; set; }
        //public int behavior5 { get; set; }
        //public int behavior6 { get; set; }
        //public int behavior7 { get; set; }
        //public int behavior8 { get; set; }
        //public int behavior9 { get; set; }
        //public int behavior10 { get; set; }
        //public int behavior11 { get; set; }
        //public int score1 { get; set; }
        //public int score2 { get; set; }
        //public int score3 { get; set; }
        //public int score4 { get; set; }
        //public int score5 { get; set; }
        //public int score6 { get; set; }
        //public int score7 { get; set; }
        //public int score8 { get; set; }
        //public int score9 { get; set; }
        //public int score10 { get; set; }
        //public int score11 { get; set; }
    }




}
