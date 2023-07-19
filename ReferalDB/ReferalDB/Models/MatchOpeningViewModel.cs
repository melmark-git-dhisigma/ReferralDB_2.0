using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReferalDB.Models
{
    public class MatchOpeningViewModel
    {
        public IEnumerable<SelectListItem> Department { get; set; }
        public string DepartmentNameDisplay { get; set; }
        public virtual IEnumerable<SelectListItem> PlacementTypeList { get; set; }
        public virtual int PlacementType { get; set; }
        public string comments { get; set; }
        public string draft { get; set; }
        public int createdBy { get; set; }
        public DateTime createdOn { get; set; }
        public int modifiedBy { get; set; }
        public DateTime modifiedOn { get; set; }
        public int approvedStatus { get; set; }
        public int DepartmentId { get; set; }
        public int programId { get; set; }
        public int matchOpeningId { get; set; }
        public int count { get; set; }
        public bool iSSubmitted { get; set; }
        public MatchOpeningViewModel()
        {
            Department=new List<SelectListItem>();
        }
    }
}