using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReferalDB.Models
{
    public class MedicalReviewViewModel
    {
        public IList<CommmonCheckListViewModel> enginLetterList { get; set; }
        public IList<CommonMulHeadViewMode> ChkAll { get; set; }
        
        public CommonAccRevComntsViewModel Comment { get; set; }
        public int approvedStatus;
        public MedicalReviewViewModel()
        {           
            enginLetterList = new List<CommmonCheckListViewModel>();
            Comment = new CommonAccRevComntsViewModel();
           
        }
    }
}