using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReferalDB.Models
{
    public class SearchModel
    {
        public virtual string SearchArgument { get; set; }
        public virtual bool SortStatus { get; set; }
        public virtual string PagingArgument { get; set; }
        public virtual int itemCount { get; set; }
        public virtual string flag { get; set; }
        public virtual int perPage { get; set; }
    }
    public class SearchDashboardModel
    {
        public string SearchAlpahabet { get; set; }
        public string SearchWeek { get; set; }
        public string SearchMonth { get; set; }
        public string SearchAge { get; set; }
        public string SearchAppdata { get; set; }
        public string SearchSort { get; set; }
        public string SearchNotification { get; set; }
    }
}