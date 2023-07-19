using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer;
using ReferalDB.Models;
using ReferalDB.AppFunctions;


namespace ReferalDB.Models
{
    public class PlacementModel
    {
        GlobalData MetaData = new GlobalData();
        public static clsSession1 sess = null;
        public virtual PagingModel pageModel { get; set; }
        public virtual string Searchtext { get; set; }
        public virtual IList<GridListPlacement> listPlacement { get; set; }
        public static MelmarkDBEntities RPCobj = new MelmarkDBEntities();
        public static PlacementModel fillPlacement(int page, int pageSize)
        {

            sess = (clsSession1)HttpContext.Current.Session["UserSession"];
            GridListPlacement grdPlacement = new GridListPlacement();
            PlacementModel listModel = new PlacementModel();
            Other_Functions clsFunctions=new Other_Functions();
            listModel.pageModel.CurrentPageIndex = page;
            listModel.pageModel.PageSize = pageSize;
            IList<GridListPlacement> retunmodel = new List<GridListPlacement>();
            if (sess != null)
            {
                retunmodel = (from objPlacement in RPCobj.Placements
                              join objLookUp in RPCobj.LookUps on objPlacement.PlacementType equals objLookUp.LookupId
                              where (objPlacement.StudentPersonalId == sess.StudentId && objPlacement.Status == 1)
                              select new GridListPlacement
                              {
                                  PlacementId = objPlacement.PlacementId,
                                  PlacementName = objLookUp.LookupName,
                                  StartDate = (DateTime)objPlacement.StartDate,
                                  EndDate = objPlacement.EndDate,


                              }).ToList();

                listModel.pageModel.TotalRecordCount = retunmodel.Count;
                retunmodel = retunmodel.OrderByDescending(objEvents => objEvents.PlacementId).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                listModel.listPlacement = retunmodel;
                if (listModel.pageModel.PageSize > listModel.pageModel.TotalRecordCount) { listModel.pageModel.PageSize = listModel.pageModel.TotalRecordCount; }
                if (listModel.pageModel.TotalRecordCount == 0) { listModel.pageModel.CurrentPageIndex = 0; }
            }
            return listModel;
        }
        public PlacementModel()
        {
            listPlacement = new List<GridListPlacement>();
            pageModel = new PagingModel();
        }

        public class GridListPlacement
        {
            public virtual int PlacementId { get; set; }
            public virtual string PlacementName { get; set; }
            public virtual string PlacementnStatus { get; set; }
            public virtual string Program { get; set; }
            public DateTime? EndDate;
            public virtual DateTime? StartDate { get; set; }
            public virtual string datetime
            {
                get
                {
                    if (EndDate != null)
                    {
                        return ((DateTime)EndDate).ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        return "";
                    }
                }
                
            }
        }
    }
}