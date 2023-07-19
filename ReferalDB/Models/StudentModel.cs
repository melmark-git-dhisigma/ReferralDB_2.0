using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer;
namespace ReferalDB.Models
{
    public class StudentModel
    {
        public int StudentId { get; set; }
        public int AddressId { get; set; }
        public int ClassId { get; set; }
        public int SchoolId { get; set; }
        public int DistrictId { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime JoinDt { get; set; }
        public DateTime DOB { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Boolean ResidenceInd { get; set; }
        public string StudentNbr { get; set; }
        public string SASID { get; set; }
        public string StudentFname { get; set; }
        public string StudentLname { get; set; }
        public string Gender { get; set; }
        public string GradeLevel { get; set; }
        public string DistFunction { get; set; }
        public string DistContactPerson { get; set; }
        public string Citizenship { get; set; }
        public string GuardenshipStatus { get; set; }
        public string PrimaryLang { get; set; }
        public string SignificantBehavior { get; set; }
        public string Capabilities { get; set; }
        public string Limitations { get; set; }
        public string Performances { get; set; }
        public string DistrictCode { get; set; }
        public string DistrictType { get; set; }
        public string MaritulStatus { get; set; }
        public string OtherStageAgency { get; set; }
        public string MedicalConditions { get; set; }
        public string Allergies { get; set; }
        public string CurrentMedications { get; set; }
        public string SelfPresAbility { get; set; }
        public string HairColor { get; set; }
        public string DistinguishMark { get; set; }
        public string CaseManagerResidential { get; set; }
        public string CaseManagerEducational { get; set; }
        public string LastPhysicalDate { get; set; }
        public string LegalComp { get; set; }
        public string ActiveInd { get; set; }
        public string ImageURL { get; set; }
        public string Height { get; set; }
        public string Weight { get; set; }
        public string Race { get; set; }
        public string EyeColor { get; set; } 

    }
}