using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReferalDB.Models
{
    public class SchedulePrntInterviewViewModel
    {
        public IList<CommonCallLogViewModel> CallLog { get; set; }
        public CommonAccRevComntsViewModel Comment { get; set; }
        public IList<CommonMulHeadViewMode> ChkAll { get; set; }
        
        public SchedulePrntInterviewViewModel()
        {
            CallLog = new List<CommonCallLogViewModel>();
            Comment = new CommonAccRevComntsViewModel();
            ChkAll = new List<CommonMulHeadViewMode>();
        }
    }
}