using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReferalDB.Models
{
    public class MedicalModel
    {
        public virtual int ID { get; set; }
        public virtual string InsuranceType { get; set; }
        public virtual string PolicyNumber { get; set; }
        public virtual string PolicyHolder { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastNames { get; set; }
        public virtual string City { get; set; }
        public virtual string OfficePhone { get; set; }
        public virtual string DateOfLastPhysicalExam { get; set; }
        public virtual string MedicalConditionsDiagnosis { get; set; }
        public virtual string Allergies { get; set; }
        public virtual string CurrentMedications { get; set; }
        public virtual string SelfPreservationAbility { get; set; }
        public virtual string SignificantBehaviorCharacteristics { get; set; }
        public virtual string Capabilities { get; set; }
        public virtual string Limitations { get; set; }
        public virtual string Preferances { get; set; }
        public virtual IEnumerable<SelectListItem> PhysicianList { get; set; }
        public virtual int? PhysicianId { get; set; }
        public virtual string Physician { get; set; }
        public virtual IEnumerable<SelectListItem> CountryList { get; set; }
        public virtual int? CountryId { get; set; }
        public virtual string Country { get; set; }
        public virtual IEnumerable<SelectListItem> StateList { get; set; }
        public virtual int? StateId { get; set; }
        public virtual string State { get; set; }
        public virtual string Addressline1 { get; set; }
        public virtual string Street { get; set; }
        public virtual string CalenderDatas { get; set; }

    }
}