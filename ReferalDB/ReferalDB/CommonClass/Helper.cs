using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


    public static class Helper
    {
        public static List<SelectListItem> SelectValue(this List<SelectListItem> Obj, string Value)
        {
            var selectVal = Obj.Where(x => x.Value == Value).ToList();
            if (selectVal.Count > 0)
            {
                selectVal[0].Selected = true;
            }
            return Obj;
        }

        public static string ToString(this DateTime? date,string format)
        {
            return date == null ? "" : ((DateTime)date).ToString(format);
        }
    }