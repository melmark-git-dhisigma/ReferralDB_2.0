using DataLayer;
using ReferalDB.CommonClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReferalDB.Models
{
    public class AddEventModel
    {
        public virtual int Id { get; set; }
        public virtual IEnumerable<SelectListItem> EventTypeList { get; set; }
        public virtual int? EventType { get; set; }
        public virtual IEnumerable<SelectListItem> EventStatusList { get; set; }
        public virtual int? EventStatus { get; set; }
        public virtual string EventName { get; set; }
        public virtual string ExpiredOnDate { get; set; }
        public virtual string EventDate { get; set; }
        public virtual string EventTypes { get; set; }
        public virtual string Note { get; set; }
        public virtual string UserName { get; set; }
        public virtual bool IsSystemEvent { get; set; }
        public virtual int? Contact { get; set; }

        public static bool CreateSystemEvent(string EvtName, string EvetTypes, string Note)
        {
            MelmarkDBEntities objData = new MelmarkDBEntities();
            var sess = (clsSession)HttpContext.Current.Session["UserSession"];
            AddEventModel model = new AddEventModel();
            model.EventDate = DateTime.Now.ToString("MM/dd/yyyy").Replace("-", "/");
            model.ExpiredOnDate = DateTime.Now.ToString("MM/dd/yyyy").Replace("-", "/");
            model.Note = Note;
            model.IsSystemEvent = true;
            model.EventName = EvtName;
            model.UserName = sess.UserName;
            //finding LookupVals
            var EventType = objData.LookUps.Where(x => x.LookupCode == EvetTypes && x.LookupType == "SysEventType").ToList();
            var Status = objData.LookUps.Where(x => x.LookupCode == "Expired" && x.LookupType == "Visitation Status").ToList();
            if (EventType.Count > 0)
            {
                model.EventTypes = EventType[0].LookupId.ToString();
            }
            if (Status.Count > 0)
            {
                model.EventStatus = Status[0].LookupId;
            }
            ClsCommon objFuns = new ClsCommon();
            var result = objFuns.SaveEventData(model, true);
            if (result == "Sucess")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}