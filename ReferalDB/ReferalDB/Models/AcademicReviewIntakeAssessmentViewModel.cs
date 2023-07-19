using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReferalDB.Models
{
    public class AcademicReviewIntakeAssessmentViewModel
    {
        public IList<CommmonCheckListViewModel> enginLetterList { get; set; }
        public IList<CommonMulHeadViewMode> ChkAll { get; set; }
        public IList<string> checklist { get; set; }
        
        public CommonAccRevComntsViewModel Comment { get; set; }
        public int approvedStatus;
        public AcademicReviewIntakeAssessmentViewModel()
        {
            checklist = new List<string>();
            ChkAll = new List<CommonMulHeadViewMode>(); 
            enginLetterList = new List<CommmonCheckListViewModel>();
            Comment = new CommonAccRevComntsViewModel();            
        }

    }
}