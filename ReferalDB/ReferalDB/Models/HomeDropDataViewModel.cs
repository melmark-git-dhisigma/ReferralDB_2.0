using ReferalDB.CommonClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReferalDB.Models
{
    public class HomeDropDataViewModel
    {
        public virtual IList<ActiveReferalNdUser> CheckDetails { get; set; }
        public virtual IList<ActiveReferalNdUser> RefDetails { get; set; }
        public virtual IList<ActivityModelClass> RefDetailsMore { get; set; }

        public HomeDropDataViewModel()
        {
            RefDetails = new List<ActiveReferalNdUser>();
            CheckDetails = new List<ActiveReferalNdUser>();
            RefDetailsMore = new List<ActivityModelClass>();
        }
    }
}