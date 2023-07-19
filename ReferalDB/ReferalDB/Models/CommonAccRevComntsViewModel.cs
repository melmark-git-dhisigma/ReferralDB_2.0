using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReferalDB.Models
{
    public class CommonAccRevComntsViewModel
    {
        public int academicReviewId { get; set; }
        public int StudentId { get; set; }
        public int QstatusId { get; set; }
        public int? QstatusIdGet { get; set; }
        
        public int SchoolId { get; set; }
        public string Comments { get; set; }
        public bool AproveInt { get; set; }
        public string Type { get; set; }
        public string Draft { get; set; }        
        public bool IsPresent { get; set; }
        public bool iSSubmitted { get; set; }
    }
}