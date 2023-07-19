using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReferalDB.Models
{
    public class ParentInterviewViewModel
    {
        
        
        public CommonAccRevComntsViewModel Comment { get; set; }
        public IList<CommonMulHeadViewMode> ChkAll { get; set; }
        public int approvedStatus;
        public ParentInterviewViewModel()
        {
          
            Comment = new CommonAccRevComntsViewModel();          
        }
      
    }
}