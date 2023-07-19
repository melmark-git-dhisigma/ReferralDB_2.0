using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer;
using ReferalDB.Models;
using ReferalDB.AppFunctions;


namespace ReferalDB.Models
{
    public class VisitationModel
    {
        GlobalData MetaData = new GlobalData();
        public static clsSession1 sess = null;
        public virtual PagingModel pageModel { get; set; }
        public virtual string Searchtext { get; set; }
        public virtual IList<GridListVisitation> listVisitation { get; set; }
        public static MelmarkDBEntities RPCobj = new MelmarkDBEntities();
        public static VisitationModel fillVisitations(int page, int pageSize)
        {

            sess = (clsSession1)HttpContext.Current.Session["UserSession"];
            GridListVisitation grdVisitation = new GridListVisitation();
            DateTime now = DateTime.Now;
            VisitationModel listModel = new VisitationModel();
            listModel.pageModel.CurrentPageIndex = page;
            listModel.pageModel.PageSize = pageSize;
            IList<GridListVisitation> retunmodel = new List<GridListVisitation>();
            retunmodel = (from objVisitation in RPCobj.Visitations
                          join objLookUp in RPCobj.LookUps on objVisitation.VisittaionType equals objLookUp.LookupId
                          join objLookUps in RPCobj.LookUps on objVisitation.VisitationStatus equals objLookUps.LookupId
                          where (objVisitation.StudentPersonalId == sess.StudentId && objVisitation.Status == 1)
                          select new GridListVisitation
                          {
                              VisitationId = objVisitation.VisitationId,
                              VisitationName = objVisitation.VisitationName,
                              VisitaionStatus = objLookUps.LookupName,
                              VisitationDate = objVisitation.VisitationDate,
                              VisitationType = objLookUp.LookupName,
                              ExpiredOn = objVisitation.ExpiredOn,


                          }).ToList();

            if (retunmodel != null)
            {
                foreach (var item in retunmodel)
                {
                    if (item.ExpiredOn <= now)
                    {
                        var data = (from objVisitation in RPCobj.Visitations
                                    join objLookUp in RPCobj.LookUps on objVisitation.VisittaionType equals objLookUp.LookupId
                                    join objLookUps in RPCobj.LookUps on objVisitation.VisitationStatus equals objLookUps.LookupId
                                    where (objVisitation.StudentPersonalId == sess.StudentId && objVisitation.Status == 1 && objVisitation.VisitationId == item.VisitationId)
                                    select new GridListVisitation
                                    {
                                        VisitaionStatus = "Expired",
                                    }).SingleOrDefault();
                        item.VisitaionStatus = data.VisitaionStatus;

                    }

                }
            }

          
            listModel.pageModel.TotalRecordCount = retunmodel.Count;
            retunmodel = retunmodel.OrderByDescending(objEvents => objEvents.VisitationId).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            listModel.listVisitation = retunmodel;

            if (listModel.pageModel.PageSize > listModel.pageModel.TotalRecordCount) { listModel.pageModel.PageSize = listModel.pageModel.TotalRecordCount; }
            if (listModel.pageModel.TotalRecordCount == 0) { listModel.pageModel.CurrentPageIndex = 0; }

            return listModel;
        }

        public VisitationModel()
        {
            listVisitation = new List<GridListVisitation>();
            pageModel = new PagingModel();
        }
    }
    public class GridListVisitation
    {
        public virtual int VisitationId { get; set; }
        public virtual string VisitationName { get; set; }
        public virtual DateTime VisitationDate { get; set; }
        public virtual DateTime ExpiredOn { get; set; }
        public virtual string VisitaionStatus { get; set; }
        public virtual string VisitationType { get; set; }
        public virtual string Visitaiondate { get; set; }
        public virtual string expiredOn { get; set; }


    }
}