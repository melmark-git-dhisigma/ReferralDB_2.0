using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ReferalDB.Models
{
    public class AddPlacementModel
    {
        public virtual int Id { get; set; }
        public virtual IEnumerable<SelectListItem> PlacementTypeList { get; set; }
        public virtual int? PlacementType { get; set; }
        public virtual IEnumerable<SelectListItem> DepartmentList { get; set; }
        public virtual int? Department { get; set; }
        public virtual IEnumerable<SelectListItem> PrimaryNurseList { get; set; }
        public virtual int? PrimaryNurse { get; set; }
        public virtual IEnumerable<SelectListItem> BehaviorAnalystList { get; set; }
        public virtual int? BehaviorAnalyst { get; set; }
        public virtual IEnumerable<SelectListItem> UnitClerkList { get; set; }
        public virtual int? UnitClerk { get; set; }
        public virtual string EndDateDate { get; set; }
        public virtual string StartDate { get; set; }
    }

    
}