using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReferalDB.Models
{
    public class PreAdmissionMeetingViewModel
    {
        public IList<CommonCallLogViewModel> CallLog { get; set; }
        public IList<CommonMulHeadViewMode> ChkAll { get; set; }
        public bool IsSubmit { get; set; }
        public PreAdmissionMeetingViewModel()
        {            
             CallLog = new List<CommonCallLogViewModel>();
             ChkAll = new List<CommonMulHeadViewMode>();
        }
    }
}