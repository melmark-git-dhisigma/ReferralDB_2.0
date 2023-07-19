using DataLayer;
using System;
using System.Collections.Generic;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReferalDB.Models
{
    
    public class AddMatchOpeningViewModel
    {
        public virtual string placemntLogText { get; set; }
        public virtual int Id { get; set; }
        public virtual IEnumerable<SelectListItem> PlacementTypeList { get; set; }
        public virtual int? PlacementType { get; set; }
        public virtual IEnumerable<SelectListItem> DepartmentList { get; set; }
        public virtual int? Department { get; set; }
        public virtual IEnumerable<SelectListItem> PrimaryNurseList { get; set; }
        public virtual int? PrimaryNurse { get; set; }
        public virtual IEnumerable<SelectListItem> BehaviorAnalystList { get; set; }
        public virtual int? BehaviorAnalyst { get; set; }
        public virtual IEnumerable<SelectListItem> FundingSourceList { get; set; } //--- 22Sep2020 - List 3 - Task #2 ---//        
        public virtual int? FundingSourceId { get; set; } //--- 22Sep2020 - List 3 - Task #2 ---//
        public virtual IEnumerable<SelectListItem> UnitClerkList { get; set; }
        public virtual int? UnitClerk { get; set; }
        public virtual string EndDateDate { get; set; }
        public virtual string StartDate { get; set; }
        public virtual int? PlacementReason { get; set; }
        public virtual bool iSSubmitted { get; set; }

        public virtual string Reason { get; set; }
        public virtual string AssociatedPersonnel { get; set; }
        public virtual int? LocationId { get; set; }
        public virtual string LocationDisplay { get; set; }
        public virtual int? PlacementDepartmentId { get; set; }

        public virtual bool IsMonday { get; set; }
        public virtual bool IsTuesday { get; set; }
        public virtual bool IsWednesday { get; set; }
        public virtual bool IsThursday { get; set; }
        public virtual bool IsFriday { get; set; }
        public virtual bool IsSaturday { get; set; }
        public virtual bool IsSunday { get; set; }

        public virtual string MondayNote { get; set; }
        public virtual string TuesdayNote { get; set; }
        public virtual string WednesdayNote { get; set; }
        public virtual string ThursdayNote { get; set; }
        public virtual string FridayNote { get; set; }
        public virtual string SaturdayNote { get; set; }
        public virtual string SundayNote { get; set; }

        public List<SelectListItem> PlacementReasonList { get; set; }
        public List<SelectListItem> LocationList { get; set; }
        public List<SelectListItem> PlacementDepartmentList { get; set; }
        public List<GridListPlacement> listPlacement { get; set; }
        public virtual PagingModel pageModel { get; set; }

        public List<SelectListItem> GetLocationList()
        {
            List<SelectListItem> result = new List<SelectListItem>();
            result.Add(new SelectListItem
            {
                Text = "---Select---",
                Value = ""
            });
            MelmarkDBEntities dbobj = new MelmarkDBEntities();
            var data = (from clas in dbobj.Classes
                        where clas.ActiveInd == "A"
                        select new SelectListItem
                        {
                            Text = clas.ClassName,
                            Value = SqlFunctions.StringConvert((decimal)clas.ClassId).Trim(),
                        }).ToList();
            foreach (var item in data)
            {
                result.Add(item);
            }
            return result;
        }

        public List<SelectListItem> GetPlacementDepartment(int SchoolId)
        {
            List<SelectListItem> result = new List<SelectListItem>();
            result.Add(new SelectListItem
            {
                Text = "---Select---",
                Value = ""
            });
            MelmarkDBEntities dbobj = new MelmarkDBEntities();
            var data = (from look in dbobj.LookUps
                        where look.LookupType == "PlacementDepartment" && look.SchoolId == SchoolId && look.ActiveInd == "A"
                        select new SelectListItem
                        {
                            Text = look.LookupName,
                            Value = SqlFunctions.StringConvert((decimal)look.LookupId).Trim(),
                        }).ToList();
            foreach (var item in data)
            {
                result.Add(item);
            }
            return result;
        }

        public List<SelectListItem> GetPlacementReason(int SchoolId)
        {
            List<SelectListItem> result = new List<SelectListItem>();
            result.Add(new SelectListItem
            {
                Text = "---Select---",
                Value = ""
            });
            MelmarkDBEntities dbobj = new MelmarkDBEntities();
            var data = (from look in dbobj.LookUps
                        where look.LookupType == "Placement Reason" && look.SchoolId == SchoolId
                        select new SelectListItem
                        {
                            Text = look.LookupName,
                            Value = SqlFunctions.StringConvert((decimal)look.LookupId).Trim(),
                        }).ToList();
            foreach (var item in data)
            {
                result.Add(item);
            }
            return result;
        }

        public List<SelectListItem> GetFundingList(int SchoolId)  //--- 22Sep2020 - List 3 - Task #2 - (Start) ---//
        {
            List<SelectListItem> result = new List<SelectListItem>();
            result.Add(new SelectListItem
            {
                Text = "---Select---",
                Value = ""
            });
            MelmarkDBEntities dbobj = new MelmarkDBEntities();
            var data = (from look in dbobj.LookUps
                        where look.LookupType == "fundingsource" && look.SchoolId == SchoolId
                        select new SelectListItem
                        {
                            Text = look.LookupName,
                            Value = SqlFunctions.StringConvert((decimal)look.LookupId).Trim(),
                        }).ToList();
            foreach (var item in data)
            {
                result.Add(item);
            }
            return result;
        } //--- 22Sep2020 - List 3 - Task #2 - (End) ---//

        public AddMatchOpeningViewModel()
        {
            LocationList = new List<SelectListItem>();
            PlacementDepartmentList = new List<SelectListItem>();
            PlacementReasonList = new List<SelectListItem>();
            PlacementTypeList = new List<SelectListItem>();
            FundingSourceList = new List<SelectListItem>();  //--- 22Sep2020 - List 3 - Task #2 ---//
            DepartmentList = new List<SelectListItem>();
            PrimaryNurseList = new List<SelectListItem>();
            BehaviorAnalystList = new List<SelectListItem>();
            UnitClerkList = new List<SelectListItem>();
        }

        public List<GridListPlacement> fillPlacement(int page, int pageSize)
        {
            clsSession sess = null;
            sess = (clsSession)HttpContext.Current.Session["UserSession"];
            MelmarkDBEntities RPCobj = new MelmarkDBEntities();
            GridListPlacement grdPlacement = new GridListPlacement();
            AddMatchOpeningViewModel listModel = new AddMatchOpeningViewModel();
            //pageModel.PageSize = pageSize;
            //pageModel.CurrentPageIndex = page;
            //listModel.pageModel.CurrentPageIndex = page;
            //listModel.pageModel.PageSize = pageSize;
            List<GridListPlacement> retunmodel = new List<GridListPlacement>();
            if (sess != null)
            {
                //var Deptmdl = (from objPlacement in RPCobj.Placements
                //               join objLkUp in RPCobj.LookUps on objPlacement.Department equals objLkUp.LookupId
                //               where (objPlacement.StudentPersonalId == sess.StudentId && objPlacement.Status == 1)
                //               select new
                //               {
                //                   placementId = objPlacement.PlacementId,
                //                   DeptName = objLkUp.LookupName
                //               }).ToList();
                retunmodel = (from objPlacement in RPCobj.Placements
                              join objLookUp in RPCobj.LookUps on objPlacement.PlacementType equals objLookUp.LookupId
                              join objLkUp in RPCobj.LookUps on objPlacement.Department equals objLkUp.LookupId
                              join objdept in RPCobj.LookUps on objPlacement.PlacementDepartment equals objdept.LookupId
                              join objloc in RPCobj.Classes on objPlacement.Location equals objloc.ClassId
                              where (objPlacement.StudentPersonalId == sess.ReferralId && objPlacement.Status == 1)
                              select new GridListPlacement
                              {
                                  PlacementId = objPlacement.PlacementId,
                                  PlacementName = objdept.LookupName + "-" + objLookUp.LookupName,
                                  Program = objLkUp.LookupName,                                  
                                  StartDate = objPlacement.StartDate,
                                  EndDate = objPlacement.EndDate,
                                  LocationId = objloc.ClassName,
                                  IsMonday = objPlacement.IsMonday,
                                  IsTuesday = objPlacement.IsTuesday,
                                  IsWednesday = objPlacement.IsWednesday,
                                  IsThursday = objPlacement.IsThursday,
                                  IsFriday = objPlacement.IsFriday,
                                  IsSaturday = objPlacement.IsSaturday,
                                  IsSunday = objPlacement.IsSunday

                              }).ToList();

                //listModel.pageModel.TotalRecordCount = retunmodel.Count;
                //retunmodel = retunmodel.OrderByDescending(objEvents => objEvents.PlacementId).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                listModel.listPlacement = retunmodel.OrderBy(objEvents => objEvents.PlacementId).ToList();
                //if (listModel.pageModel.PageSize > listModel.pageModel.TotalRecordCount) { listModel.pageModel.PageSize = listModel.pageModel.TotalRecordCount; }
                //if (listModel.pageModel.TotalRecordCount == 0) { listModel.pageModel.CurrentPageIndex = 0; }
            }
            return retunmodel;
        }

        public class GridListPlacement
        {
            public virtual int PlacementId { get; set; }
            public virtual string PlacementName { get; set; }
            public virtual string Program { get; set; }
            public virtual string PlacementnStatus { get; set; }
            public virtual string FundSourceId { get; set; } //--- 22Sep2020 - List 3 - Task #2 ---//
            public DateTime? EndDate;
            public DateTime? StartDate;
            public virtual string LocationId { get; set; }
            public virtual bool? IsMonday { get; set; }
            public virtual bool? IsTuesday { get; set; }
            public virtual bool? IsWednesday { get; set; }
            public virtual bool? IsThursday { get; set; }
            public virtual bool? IsFriday { get; set; }
            public virtual bool? IsSaturday { get; set; }
            public virtual bool? IsSunday { get; set; }
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
            public virtual string startdatetime
            {
                get
                {
                    if (StartDate != null)
                    {
                        return ((DateTime)StartDate).ToString("MM/dd/yyyy");
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