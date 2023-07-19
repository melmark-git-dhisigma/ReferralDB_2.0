using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReferalDB.Models
{
    public class CommonMulHeadViewMode
    {   
        public IList<CommonMulCheckListViewModel> chkList { get; set; }
        public int ChkHeadId { get; set; }
        public string ChkHeadName { get; set; }
        public CommonMulHeadViewMode()
        {
            chkList = new List<CommonMulCheckListViewModel>();
            ChkHeadId = 0;
            ChkHeadName = "";
        }
    }
}