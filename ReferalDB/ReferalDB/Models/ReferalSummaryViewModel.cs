using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReferalDB.Models
{
    public class ReferalSummaryViewModel
    {
        public IList<CommonMulHeadViewMode> ChkAll { get; set; }
        public IList<string> checklist { get; set; }
        public IList<CommmonCheckListViewModel> enginLetterList { get; set; }
        public CommonAccRevComntsViewModel Comment { get; set; }
        public ReferalSummaryViewModel()
        {
            checklist = new List<string>();
            //ChkAll = new List<CommonMulHeadViewMode>();
            enginLetterList = new List<CommmonCheckListViewModel>();
            Comment = new CommonAccRevComntsViewModel();
        }
    }
}



