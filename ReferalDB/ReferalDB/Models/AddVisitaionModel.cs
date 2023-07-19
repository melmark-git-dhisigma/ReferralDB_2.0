using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReferalDB.Models
{
    public class AddVisitaionModel
    {
        public virtual int Id { get; set; }
        public virtual IEnumerable<SelectListItem> EventTypeList { get; set; }
        public virtual int? EventType { get; set; }
        public virtual IEnumerable<SelectListItem> EventStatusList { get; set; }
        public virtual int? EventStatus { get; set; }
        public virtual string EventName { get; set; }
        public virtual string ExpiredOnDate { get; set; }
        public virtual string EventDate { get; set; }
    }
}