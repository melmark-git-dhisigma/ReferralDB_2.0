using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReferalDB.Models
{
    public class ClientRegistrationPAModel
    {
        public virtual int Id { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string MiddleName { get; set; }
        public virtual string NickName { get; set; }
        public virtual IEnumerable<SelectListItem> LastNameSuffixList { get; set; }
        public virtual string LastNameSuffix { get; set; }
        public virtual string DateOfBirth { get; set; }
        public virtual string DateUpdated { get; set; }
        public virtual string AdmissinDate { get; set; }
        public virtual IEnumerable<SelectListItem> GenderList { get; set; }
        public virtual string Gender { get; set; }
        public virtual IEnumerable<SelectListItem> RaceList { get; set; }
        public virtual int Race { get; set; }
        public virtual string StrRace { get; set; }
        public virtual IEnumerable<SelectListItem> CountryOfBirthList { get; set; }
        public virtual int? CountryofBirth { get; set; }
        public virtual string CountryBirth { get; set; }
        public virtual string PlaceOfBirth { get; set; }
        public virtual IEnumerable<SelectListItem> StateOfBirthList { get; set; }
        public virtual int? StateOfBirth { get; set; }
        public virtual string StateBirth { get; set; }
        public virtual IEnumerable<SelectListItem> CitizenshipList { get; set; }
        public virtual int Citizenship { get; set; }
        public virtual string CitizenshipBirth { get; set; }
        public virtual string Height { get; set; }
        public virtual string Weight { get; set; }
        public virtual string HairColor { get; set; }
        public virtual string EyeColor { get; set; }
        public virtual string PrimaryLanguage { get; set; }
        public virtual string LegalCompetencyStatus { get; set; }
        public virtual string GuardianshipStatus { get; set; }
        public virtual string OtherStateAgenciesInvolvedWithStudent { get; set; }
        public virtual string DistigushingMarks { get; set; }
        public virtual string MaritalStatusofBothParents { get; set; }
        public virtual string CaseManagerResidential { get; set; }
        public virtual string CaseManagerEducational { get; set; }
        public virtual IEnumerable<SelectListItem> AddressList { get; set; }
        public virtual int AddressID { get; set; }
        public virtual string AddressLine1 { get; set; }
        public virtual string AddressLine2 { get; set; }
        public virtual string AddressLine3 { get; set; }
        public virtual IEnumerable<SelectListItem> CountryList { get; set; }
        public virtual int? Country { get; set; }
        public virtual string StrCountry { get; set; }
        public virtual IEnumerable<SelectListItem> StateList { get; set; }
        public virtual int? State { get; set; }
        public virtual string StrState { get; set; }
        public virtual string City { get; set; }
        public virtual string Funding { get; set; }
        public virtual IEnumerable<SelectListItem> ZipList { get; set; }
        public virtual int Zip { get; set; }
        public virtual string ZipCode { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual string MostRecentGradeLevel { get; set; }


        public virtual string Bathroom { get; set; }
        public virtual string OnCampus { get; set; }
        public virtual string WhenTranspoting { get; set; }
        public virtual string OffCampus { get; set; }
        public virtual string PoolOrSwimming { get; set; }
        public virtual string van { get; set; }
        public virtual string CommonAreas { get; set; }
        public virtual string BedroomAwake { get; set; }
        public virtual string BedroomAsleep { get; set; }
        public virtual string TaskORBreak { get; set; }
        public virtual string TransitionInside { get; set; }
        public virtual string TransitionUnevenGround { get; set; }
        public virtual string RiskOfResistance { get; set; }
        public virtual string Mobility { get; set; }
        public virtual string StudentId { get; set; }
        public virtual string Name { get; set; }
        public virtual string ImageUrl { get; set; }
        public virtual bool? PhotoReleasePermission { get; set; }
        public HttpFileCollectionBase profilePicture { get; set; }

        public virtual string NeedForExtraHelp { get; set; }
        public virtual string ResponseToInstruction { get; set; }
        public virtual string Consciousness { get; set; }
        public virtual string WalkingResponse { get; set; }
        public virtual string Allergie { get; set; }
        public virtual string Seizures { get; set; }
        public virtual string Diet { get; set; }
        public virtual string Other { get; set; }
        public virtual string LiftingOrTransfers1 { get; set; }
        public virtual string LiftingOrTransfers2 { get; set; }
        public virtual string Ambulation1 { get; set; }
        public virtual string Ambulation2 { get; set; }
        public virtual string Toileting1 { get; set; }
        public virtual string Toileting2 { get; set; }
        public virtual string Eating1 { get; set; }
        public virtual string Eating2 { get; set; }
        public virtual string Showering1 { get; set; }
        public virtual string Showering2 { get; set; }
        public virtual string ToothBrushing1 { get; set; }
        public virtual string ToothBrushing2 { get; set; }

        public virtual string Dressing1 { get; set; }
        public virtual string Dressing2 { get; set; }
        public virtual string SkinCare1 { get; set; }
        public virtual string SkinCare2 { get; set; }
        public virtual string Communication1 { get; set; }
        public virtual string Communication2 { get; set; }
        public virtual string preferedActivities1 { get; set; }
        public virtual string preferedActivities2 { get; set; }
        public virtual string GeneralInformation1 { get; set; }
        public virtual string GeneralInformation2 { get; set; }
        public virtual string SuggestedProactiveEnvironmentalProcedures1 { get; set; }
        public virtual string SuggestedProactiveEnvironmentalProcedures2 { get; set; }
        //public virtual string SchoolAttendedCity2 { get; set; }
        //public virtual string SchoolAttendedState2 { get; set; }
        //public virtual string SchoolName3 { get; set; }
        //public virtual string DateFrom3 { get; set; }
        //public virtual string DateTo3 { get; set; }
        //public virtual string SchoolAttendedAddress13 { get; set; }
        //public virtual string SchoolAttendedAddress23 { get; set; }
        //public virtual string SchoolAttendedCity3 { get; set; }
        //public virtual string SchoolAttendedState3 { get; set; }
        public virtual IList<AdaptiveEquipmentz> Adapt{get;set;}
        public virtual IList<BasicBehavior> BasicBehave { get; set; }
        public virtual IList<Diagnosis> Diagnosis { get; set; }
       
        public ClientRegistrationPAModel()
        {
            Adapt = new List<AdaptiveEquipmentz>();
            BasicBehave = new List<BasicBehavior>();
            Diagnosis = new List<Diagnosis>();
        }
       
    }

    

    public class  AdaptiveEquipmentz
    {
        //finance officer university of kerala tvm 15010 and 725
        //- hon director uit headqtrs university of kerasla tvm 1510 SBI/SBT/DCB
        //50012 674.67 15010 1510 725 5000 
        public virtual string item { get; set; }
        public virtual string ScheduledForUss { get; set; }
        public virtual string StorageLocation { get; set; }
        public virtual string CleaningInstruction { get; set; }
        public virtual int AdaptiveEquimentId { get; set; }

    }

    public class BasicBehavior
    {
        public virtual string TargetBehavior { get; set; }
        public virtual string Definition { get; set; }
        public virtual string Antecedent { get; set; }
        public virtual string FCT { get; set; }
        public virtual string Consequances { get; set; }
        public virtual int BasicBehavioralInformationId { get; set; }
    }

    public class Diagnosis
    {
        public virtual string Name { get; set; }
        //public virtual string Value { get; set; }
    }
}