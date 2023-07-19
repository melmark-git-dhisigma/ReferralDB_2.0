using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReferalDB.Models
{
    public class ScheduleParentScreeningViewModel
    {
        public IList<CommonMulHeadViewMode> ChkAll { get; set; }
        public IList<CommmonCheckListViewModel> enginLetterList { get; set; }
        public IList<string> checklist { get; set; }
        public IList<CommonCallLogViewModel> CallLog { get; set; }
        public CommonAccRevComntsViewModel Comment { get; set; }
        public int approvedStatus;
        public ScheduleParentScreeningViewModel()
        {
            checklist = new List<string>();
            enginLetterList = new List<CommmonCheckListViewModel>();
            Comment = new CommonAccRevComntsViewModel();
            CallLog=new List<CommonCallLogViewModel>();
        }              

    }
}