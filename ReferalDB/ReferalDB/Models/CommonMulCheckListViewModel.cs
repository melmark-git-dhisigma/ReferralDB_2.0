using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReferalDB.Models
{
    public class CommonMulCheckListViewModel
    {
        public int? ChkHeadId { get; set; }
        public string ChkHeadName { get; set; } 
        public int academicReviewId { get; set; }
        public int  checkListId { get; set; }
        public string checkListval { get; set; }
        public int AssginId { get; set; }
        public string CheckListName { get; set; }        
        public DateTime? DueDate { get; set; }
        public string DueDateToShow { get; set; }
        public bool Complete { get; set; }
        public string AssignMultiName { get; set; }
        public string AssignMultiId { get; set; }
        public bool IsPresent { get; set; }

        public CommonMulCheckListViewModel()
        {
           
        }
    }
}