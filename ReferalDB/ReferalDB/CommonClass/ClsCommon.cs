using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReferalDB.Models;
using BuisinessLayer;
using DataLayer;
using ReferalDBApplicant.Classes;
using System.Data.Objects.SqlClient;
using System.Data.Entity.Validation;
using System.Diagnostics;


namespace ReferalDB.CommonClass
{
    public class ClsCommon
    {
        public virtual PagingModel pageModel { get; set; }
        public clsSession session = null;
        public clsSession1 session1 = null;

        //Function to select Checklist Corresponding to the type

        public string getFileExtention(string ContentType)
        {
            var Ext = "";

            if (ContentType.ToLower() == "application/msword")
            {
                Ext = ".doc";
            }
            else if (ContentType.ToLower() == "text/plain")
            {
                Ext = ".txt";
            }
            else if (ContentType.ToLower() == "application/vnd.ms-word")
            {
                Ext = ".docx";
            }
            else if (ContentType.ToLower() == "application/pdf")
            {
                Ext = ".pdf";
            }
            else if (ContentType.ToLower() == "application/vnd.ms-excel")
            {
                Ext = ".xls";
            }
            else if (ContentType.ToLower() == "image/jpeg")
            {
                Ext = ".jpg";
            }
            else if (ContentType.ToLower() == "image/jpeg")
            {
                Ext = ".jpeg";
            }
            else if (ContentType.ToLower() == "image/png")
            {
                Ext = ".png";
            }
            else if (ContentType.ToLower() == "application/octet-stream")
            {
                Ext = ".docx";
            }
            else if (ContentType.ToLower() == "application/vnd.oasis.opendocument.spreadsheet")
            {
                Ext = ".ods";
            }
            else if (ContentType.ToLower() == "application/vnd.oasis.opendocument.text")
            {
                Ext = ".odt";
            }
            return Ext;
        }


        public string GetFileType(string extension)
        {
            string ContentType = "";
            switch (extension)
            {
                case "text/plain":
                    ContentType = ".txt";
                    break;
                case "application/msword":
                    ContentType = ".doc";
                    break;
                case "application/vnd.ms-word":
                    ContentType = ".docx";
                    break;
                case "application/pdf":
                    ContentType = ".pdf";
                    break;
                case "application/vnd.ms-excel":
                    ContentType = ".xls";
                    break;
                case "image/jpeg":
                    ContentType = ".jpg";
                    break;
                case "image/png":
                    ContentType = ".png";
                    break;
                case "application/octet-stream":
                    ContentType = ".docx";
                    break;
                case "application/vnd.oasis.opendocument.spreadsheet":
                    ContentType = ".ods";
                    break;
                case "application/vnd.oasis.opendocument.text":
                    ContentType = ".odt";
                    break;
                //case "application/vnd.ms-excel":
                //    ContentType = ".xlsx";
                //    break;
                //case "application/vnd.ms-excel":
                //    ContentType = ".csv";
                //    break;
            }
            return ContentType;
        }

        public string GetFileAllowed(string extension)
        {
            string ContentType = "";
            if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png" || extension.ToLower() == ".bmp")
                ContentType = "true";
            return ContentType;
        }


        public List<CommmonCheckListViewModel> getCheckList(string querytype, string letterType)
        {

            session = (clsSession)HttpContext.Current.Session["UserSession"];
            bool isSubmit = false;
            // 'querytype' is the type
            if (session != null)
            {
                QstatusDetails qDetails = getQueueStatusId(session.ReferralId, querytype);
                int QstatusId = qDetails.QueueStatusId;
                if (QstatusId == 0)
                {
                    QstatusId = getQueueStatusIdIfSubmitted(session.ReferralId, querytype);
                    if (QstatusId > 0)
                    {
                        isSubmit = true;
                    }
                }
                MelmarkDBEntities objData = new MelmarkDBEntities();
                DateTime valDefalt = DateTime.Parse("1/1/2000");
                //Selecting Values for Checklist left join From chkAssin table
                //var engineLetter = (from x in objData.LetterEngines
                //                    join y in objData.LetterEngineItems on x.LetterEngineId equals y.LetterEngineId
                //                    join z in objData.ref_CheckListAssign on y.LetterEngineId equals z.CheckListId into studentInfo
                //                    from ref_CheckListAssign in studentInfo.DefaultIfEmpty()
                //                    where (x.QueueType == querytype && x.LetterType == "Check")
                //                    select new CommmonCheckListViewModel
                //                    {
                //                        checkListId = y.LetterEngineItemId,
                //                        CheckListName = y.ItemContent,
                //                        DueDate = (ref_CheckListAssign.DueDate == null) ? valDefalt : ref_CheckListAssign.DueDate,
                //                        Complete = (ref_CheckListAssign.CompleteInd == true) ? true : false,
                //                        IsPresent = (ref_CheckListAssign.CompleteInd != null) ? true : false,
                //                        AssginId = (ref_CheckListAssign.CompleteInd != null) ? ref_CheckListAssign.AssignId :0 ,
                //                        checkListval = "false"
                //                    }).ToList();
                int qidVal = getQueueId(querytype);
                List<CommmonCheckListViewModel> engineLetter = new List<CommmonCheckListViewModel>();
                if (isSubmit == false)
                {
                    engineLetter = (from x in objData.LetterEngines
                                    join y in objData.LetterEngineItems on x.LetterEngineId equals y.LetterEngineId
                                    where (x.QueueId == qidVal && x.LetterType == letterType)
                                    select new CommmonCheckListViewModel
                                    {
                                        checkListId = y.LetterEngineItemId,
                                        CheckListName = y.ItemContent,

                                    }).ToList();

                    //engineLetter = (from x in objData.ref_CheckListAssign
                    //                where (x.QueueStatusId == QstatusId)
                    //                select new CommmonCheckListViewModel
                    //                {
                    //                    checkListId = x.CheckListId,
                    //                    CheckListName = x.CheckListName,
                    //                    DueDate = x.DueDate,
                    //                    Complete = x.CompleteInd,
                    //                    AssginId = x.AssignId,
                    //                    IsPresent = true,
                    //                }).ToList();



                    foreach (var v in engineLetter)
                    {
                        var IspresenVal = (from x in objData.ref_CheckListAssign
                                           join y in objData.ref_ReviewComments on x.AcReviewId equals y.AcReviewId
                                           where (y.QueueStatusId == QstatusId && x.CheckListId == v.checkListId)
                                           select new CommmonCheckListViewModel
                                           {
                                               DueDate = x.DueDate,
                                               Complete = x.CompleteInd,
                                               AssginId = x.AssignId,
                                           }).ToList();
                        if (IspresenVal.Count > 0)
                        {
                            v.IsPresent = true;
                            v.DueDate = IspresenVal[0].DueDate;
                            v.AssginId = IspresenVal[0].AssginId;
                            v.Complete = IspresenVal[0].Complete;
                        }
                        else
                        {
                            v.IsPresent = false;
                        }
                    }
                }
                else
                {
                    engineLetter = (from x in objData.ref_CheckListAssign
                                    where (x.QueueStatusId == QstatusId)
                                    select new CommmonCheckListViewModel
                                    {
                                        checkListId = x.CheckListId,
                                        CheckListName = x.CheckListName,
                                        DueDate = x.DueDate,
                                        Complete = x.CompleteInd,
                                        AssginId = x.AssignId,
                                        IsPresent = true,
                                    }).ToList();

                }

                foreach (var v in engineLetter)
                {
                    //checkin if Value Present in ChkAssinTable
                    if (v.IsPresent == true)
                    {
                        if (v.DueDate.HasValue == true)
                            v.DueDateToShow = v.DueDate.Value.ToString("MM'/'dd'/'yyyy");
                        else
                            v.DueDateToShow = "";
                        if (v.AssginId != 0)
                        {
                            //Assigning AssignId nd userNames to the view model
                            var useraval = (from x in objData.ref_CheckListUsers
                                            join y in objData.Users on x.UserId equals y.UserId
                                            where (y.ActiveInd == "A" && x.ChecklistUserId == v.AssginId)
                                            select new
                                            {
                                                UserId = x.UserId,
                                                FirstName = y.UserFName,
                                                LastName = y.UserLName,
                                            }).ToList();
                            v.AssignMultiId = "";
                            v.AssignMultiName = "";
                            int h = 0;
                            foreach (var vr in useraval)
                            {

                                v.AssignMultiId = v.AssignMultiId + (h == 0 ? vr.UserId.ToString() : "," + vr.UserId);
                                v.AssignMultiName = v.AssignMultiName + (h == 0 ? vr.LastName + "," + vr.LastName : ";" + vr.LastName + "," + vr.LastName);
                                h++;
                            }
                        }

                    }
                    else
                    {
                        v.DueDateToShow = "";
                        v.checkListval = "False";
                    }
                }
                foreach (var v in engineLetter)
                {
                    if (v.AssignMultiId == "" || v.AssignMultiId == null)
                    {
                        v.AssignMultiId = "0";
                    }
                    if (v.AssignMultiName == null || v.AssignMultiName == "")
                    {
                        v.AssignMultiName = " ";
                    }

                }

                return engineLetter;
            }
            else
            {
                List<CommmonCheckListViewModel> defaultView = new List<CommmonCheckListViewModel>();
                return defaultView;
            }
        }

        // function to select Comment table WRT to type
        public CommonAccRevComntsViewModel getRevCmt(string querytype)
        {
            MelmarkDBEntities objData = new MelmarkDBEntities();
            session = (clsSession)HttpContext.Current.Session["UserSession"];
            CommonAccRevComntsViewModel defaltNullVal = new CommonAccRevComntsViewModel();
            //var qStatusRow = objData.ref_QueueStatus.Where(QueueStatus => QueueStatus.CurrentStatus == true && QueueStatus.StudentPersonalId == 1 && QueueStatus.QueueStatus == "WL").ToList();
            //if (qStatusRow.Count == 0)
            //{
            if (session != null)
            {
                int QueueSub = 0;
                QstatusDetails qDetails = getQueueStatusId(session.ReferralId, querytype);
                int QstatusId = qDetails.QueueStatusId;
                bool isSubmit = false;
                if (QstatusId > 0)
                {
                    QueueSub = getQueueStatusIdIfSubmitted(session.ReferralId, querytype);
                    if (QueueSub > 0)
                    {
                        isSubmit = true;
                    }
                }
                //selecting value from table
                var RevCmt = (from x in objData.ref_ReviewComments
                              where (x.QueueStatusId == QstatusId && x.StudentId == session.ReferralId)
                              select new CommonAccRevComntsViewModel
                              {

                                  academicReviewId = x.AcReviewId,
                                  Comments = x.Comments,
                                  AproveInt = x.ApproveInd,
                                  Draft = x.Draft,
                                  SchoolId = x.SchoolId,
                                  StudentId = x.StudentId,
                                  Type = x.Type,
                                  QstatusIdGet = x.QueueStatusId
                              }).ToList();


                //Checkin weather the Value Present or not in table

                if (RevCmt.Count > 0)
                {
                    //setting IsPresent Flag     
                    RevCmt[0].IsPresent = true;
                    if (RevCmt[0].QstatusIdGet == null)
                    {
                        RevCmt[0].QstatusId = 0;
                    }
                    else
                    {
                        RevCmt[0].QstatusId = int.Parse(RevCmt[0].QstatusIdGet.ToString());
                    }
                    RevCmt[0].iSSubmitted = isSubmit;
                    return RevCmt[0];
                }
                else
                {
                    //setting IsPresent Flag
                    defaltNullVal.IsPresent = false;
                    defaltNullVal.academicReviewId = 0;
                    defaltNullVal.QstatusId = 0;
                    defaltNullVal.AproveInt = true;
                    defaltNullVal.iSSubmitted = false;
                    return defaltNullVal;
                }
                //}
                //else
                //    return defaltNullVal;

            }
            else
            {
                defaltNullVal.iSSubmitted = false;
                return defaltNullVal;
            }


        }

        // function to select CallLog table WRT to type and AcdReviewId 
        public CommonCallLogViewModel getRevCallLog(string querytype, int AcdReviewId)
        {
            MelmarkDBEntities objData = new MelmarkDBEntities();
            session = (clsSession)HttpContext.Current.Session["UserSession"];
            if (session != null)
            {
                //selecting value from table
                QstatusDetails qDetails = getQueueStatusId(session.ReferralId, querytype);
                int QstatusId = qDetails.QueueStatusId;
                if (QstatusId == 0)
                {
                    QstatusId = getQueueStatusIdIfSubmitted(session.ReferralId, querytype);
                }

                var CallLogVar = (from x in objData.ref_CallLogs
                                  where (x.QueueStatusId == QstatusId && x.StudentId == session.ReferralId && x.AcReviewId == AcdReviewId)
                                  select new CommonCallLogViewModel
                                  {
                                      academicReviewId = x.AcReviewId,
                                      AppntTime = x.AppointmentTime,
                                      CallLogId = x.CallLogId,
                                      CallTime = x.CallTime,
                                      Conversation = x.Conversation,
                                      Draft = x.Draft,
                                      NameOfContact = x.Nameofcontact,
                                      SchoolId = x.SchoolId,
                                      StaffName = x.StaffName,
                                      StudentId = x.StudentId,
                                      type = x.Type

                                  }).ToList();
                CommonCallLogViewModel defaltNullVal = new CommonCallLogViewModel();
                //Checkin weather the Value Present or not in table
                if (CallLogVar.Count > 0)
                {
                    //setting IsPresent Flag
                    CallLogVar[0].IsPresent = true;
                    CallLogVar[0].AppntTimeShow = (CallLogVar[0].AppntTime == null) ? "" : CallLogVar[0].AppntTime.Value.ToString("MM'/'dd'/'yyyy");
                    CallLogVar[0].CallDateShow = (CallLogVar[0].CallTime == null) ? "" : CallLogVar[0].CallTime.Value.ToString("MM'/'dd'/'yyyy");
                    CallLogVar[0].CallTimeShow = (CallLogVar[0].CallTime == null) ? "" : CallLogVar[0].CallTime.Value.ToString("HH':'mm");
                    return CallLogVar[0];
                }
                else
                {
                    //setting IsPresent Flag
                    defaltNullVal.IsPresent = false;
                    defaltNullVal.academicReviewId = 0;
                    return defaltNullVal;
                }


            }
            else
            {
                CommonCallLogViewModel defaultView = new CommonCallLogViewModel();
                return defaultView;
            }

        }

        //Function to get checked Checklist
        public bool getCheckedCheckList(IList<CommmonCheckListViewModel> UpdateVals)
        {
            foreach (var Chkupdate in UpdateVals)
            {
                if ((Chkupdate.checkListval == "False" || Chkupdate.checkListval == "false") || Chkupdate.AssignMultiId == "0")
                {
                    return false;
                }
            }
            return true;
        }
        public bool getCheckedCheckListMul(IList<CommonMulHeadViewMode> UpdateVals)
        {
            foreach (var Chkupdate in UpdateVals)
            {
                foreach (var chkList in Chkupdate.chkList)
                {
                    if ((chkList.checkListval == "False" || chkList.checkListval == "false"))// commented bcoz user assign not required---> || chkList.AssignMultiId == "0")
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public AddMatchOpeningViewModel bindPlacement(int itemId, AddMatchOpeningViewModel returnModel)
        {
            session = (clsSession)HttpContext.Current.Session["UserSession"];
            MelmarkDBEntities dbobj = new MelmarkDBEntities();
            clsReferral objRef = new clsReferral(); //--- 02Oct2020 - List 3 - Task #2 ---//
            //AddMatchOpeningViewModel returnModel = new AddMatchOpeningViewModel();
            Placement placement = new Placement();
            if (itemId > 0)
            {
                try
                {
                    placement = dbobj.Placements.Where(objPlacement => objPlacement.StudentPersonalId == session.ReferralId && objPlacement.PlacementId == itemId).SingleOrDefault();
                    returnModel.Id = placement.PlacementId;
                    returnModel.PlacementType = placement.PlacementType;
                    returnModel.BehaviorAnalyst = placement.BehaviorAnalyst;
                    returnModel.PrimaryNurse = placement.PrimaryNurse;
                    returnModel.Department = placement.Department;
                    returnModel.UnitClerk = placement.UnitClerk;
                    returnModel.StartDate = ConvertDate(placement.StartDate);
                    returnModel.EndDateDate = ConvertDate(placement.EndDate);
                    returnModel.PlacementReason = placement.PlacementReason;

                    returnModel.IsMonday = placement.IsMonday.GetBool();
                    returnModel.IsTuesday = placement.IsTuesday.GetBool();
                    returnModel.IsWednesday = placement.IsWednesday.GetBool();
                    returnModel.IsThursday = placement.IsThursday.GetBool();
                    returnModel.IsFriday = placement.IsFriday.GetBool();
                    returnModel.IsSaturday = placement.IsSaturday.GetBool();
                    returnModel.IsSunday = placement.IsSunday.GetBool();

                    returnModel.MondayNote = placement.MondayNote;
                    returnModel.TuesdayNote = placement.TuesdayNote;
                    returnModel.WednesdayNote = placement.WednesdayNote;
                    returnModel.ThursdayNote = placement.ThursdayNote;
                    returnModel.FridayNote = placement.FridayNote;
                    returnModel.SaturdayNote = placement.SaturdayNote;
                    returnModel.SundayNote = placement.SundayNote;

                    returnModel.Reason = placement.Reason;
                    returnModel.AssociatedPersonnel = placement.AssociatedPersonnel;
                    returnModel.LocationId = placement.Location;
                    returnModel.PlacementDepartmentId = placement.PlacementDepartment;
                    try
                    {

                        //--- 02Oct2020 - List 3 - Task #2 -(Start)--//
                        int? LookUpID = objRef.GetFundingSourceID(session.ReferralId, session.SchoolId);
                        if (LookUpID != null)
                        {
                            returnModel.FundingSourceId = LookUpID;
                        }
                        else
                        {
                            returnModel.FundingSourceId = 0;
                        }
                        //--- 02Oct2020 - List 3 - Task #2 -(Start)--//
                    }
                    catch (Exception ex)
                    {
                        ClsErrorLog erorLog = new ClsErrorLog();
                        erorLog.WriteToLog(ex.ToString());
                    }

                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                }
            }
            return returnModel;
        }

        //--- 02Oct2020 - List 3 - Task #2 -(Start)--//
        public AddMatchOpeningViewModel bindPlacementFundid(int itemId, AddMatchOpeningViewModel returnModel)
        {
            session = (clsSession)HttpContext.Current.Session["UserSession"];
            MelmarkDBEntities dbobj = new MelmarkDBEntities();
            clsReferral objRef = new clsReferral();
            try
            {
                int? LookUpID = objRef.GetFundingSourceID(session.ReferralId, session.SchoolId);
                if (LookUpID != null)
                {
                    returnModel.FundingSourceId = LookUpID;
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }
            return returnModel;
        }
        //--- 02Oct2020 - List 3 - Task #2 -(Start)--//

        private string ConvertDate(DateTime? nullable)
        {
            string result = "";
            DateTime temp;
            try
            {
                temp = (DateTime)nullable;
                result = temp.ToString("MM/dd/yyyy").Replace('-', '/');
            }
            catch
            {
                result = null;
            }

            return result;
        }

        //Function to Update Or insert Checklist 
        public IList<CommmonCheckListViewModel> UpdateOrInsertCheckList(IList<CommmonCheckListViewModel> UpdateVals, int AcReviewId, string Type, int QstatusId, string letterType)
        {


            MelmarkDBEntities objData = new MelmarkDBEntities();
            session = (clsSession)HttpContext.Current.Session["UserSession"];
            if (session != null)
            {
                QstatusDetails qStatusValues = getQueueStatusId(session.ReferralId, "AR");

                //bool presentVAl = false;
                var testPresent = objData.ref_CheckListAssign.Where(x => x.AcReviewId == AcReviewId && x.QueueStatusId == QstatusId).ToList();
                foreach (var val in testPresent)
                {
                    foreach (var ChkLst in UpdateVals)
                    {
                        if (val.CheckListId == ChkLst.checkListId)
                        {
                            ChkLst.IsPresent = true;
                            ChkLst.AssginId = val.AssignId;
                        }
                    }
                }
                //if (testPresent.Count > 0)
                //{
                //    presentVAl = true;
                //}

                foreach (var Chkupdate in UpdateVals)
                {

                    ref_CheckListAssign CheckListAssign = new ref_CheckListAssign();
                    if (Chkupdate.IsPresent == true)
                    {
                        if (Chkupdate.checkListval == "True" || Chkupdate.checkListval == "true")
                        {
                            CheckListAssign = objData.ref_CheckListAssign.Single(x => x.AssignId == Chkupdate.AssginId);
                            CheckListAssign.CompleteInd = true;
                            if (Chkupdate.DueDateToShow != null && Chkupdate.DueDateToShow != "" && Chkupdate.AssignMultiId != "NA")
                            {
                                CheckListAssign.DueDate = DateTime.ParseExact(Chkupdate.DueDateToShow, "MM'/'dd'/'yyyy", System.Globalization.CultureInfo.CurrentCulture);
                            }
                            CheckListAssign.ModifiedOn = System.DateTime.Now;
                            CheckListAssign.ModifiedBy = session.LoginId;
                            objData.SaveChanges();
                        }
                        else
                        {
                            CheckListAssign = objData.ref_CheckListAssign.Single(x => x.AssignId == Chkupdate.AssginId);
                            CheckListAssign.CompleteInd = false;
                            if (Chkupdate.DueDateToShow != null && Chkupdate.DueDateToShow != "" && Chkupdate.AssignMultiId != "NA")
                            {
                                CheckListAssign.DueDate = DateTime.ParseExact(Chkupdate.DueDateToShow, "MM'/'dd'/'yyyy", System.Globalization.CultureInfo.CurrentCulture);
                            }
                            CheckListAssign.DueDate = System.DateTime.Now;
                            CheckListAssign.ModifiedOn = System.DateTime.Now;
                            CheckListAssign.ModifiedBy = session.LoginId;
                            objData.SaveChanges();
                        }
                    }
                    else
                    {
                        //if (Chkupdate.checkListval == "True")
                        //{
                        CheckListAssign.CheckListId = Chkupdate.checkListId;
                        CheckListAssign.CheckListName = Chkupdate.CheckListName;
                        if (Chkupdate.DueDateToShow != null && Chkupdate.DueDateToShow != "" && Chkupdate.AssignMultiId != "NA")
                        {
                            CheckListAssign.DueDate = DateTime.ParseExact(Chkupdate.DueDateToShow, "MM'/'dd'/'yyyy", System.Globalization.CultureInfo.CurrentCulture);
                        }
                        CheckListAssign.Type = Type;

                        CheckListAssign.AcReviewId = AcReviewId;
                        CheckListAssign.CompleteInd = (Chkupdate.checkListval == "False" || Chkupdate.checkListval == "false") ? false : true;
                        CheckListAssign.CreatedOn = System.DateTime.Now;
                        CheckListAssign.CreatedBy = session.LoginId;
                        CheckListAssign.QueueStatusId = CheckListAssign.ModifiedBy = QstatusId;

                        objData.ref_CheckListAssign.Add(CheckListAssign);
                        objData.SaveChanges();
                        Chkupdate.AssginId = CheckListAssign.AssignId;
                        //}
                    }
                    if (Chkupdate.AssignMultiId != "NA")
                    {

                        var CheckListusers = objData.ref_CheckListUsers.Where(x => x.ChecklistUserId == Chkupdate.AssginId).ToList();
                        if (CheckListusers.Count == 0)
                        {
                            if (Chkupdate.AssignMultiId != "0")
                            {
                                var Allid = Chkupdate.AssignMultiId.Split(',');
                                for (int i = 0; i < Allid.Length; i++)
                                {
                                    int id = Convert.ToInt32(Allid[i]);
                                    ref_CheckListUsers users = new ref_CheckListUsers();
                                    users.ChecklistUserId = Chkupdate.AssginId;
                                    users.UserId = id;
                                    users.CreatedBy = session.LoginId;
                                    users.CreatedOn = System.DateTime.Now;
                                    users.ModifiedBy = session.LoginId;
                                    users.ModifiedOn = System.DateTime.Now;
                                    objData.ref_CheckListUsers.Add(users);
                                    objData.SaveChanges();
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < CheckListusers.Count; i++)
                            {
                                objData.ref_CheckListUsers.Remove(CheckListusers[i]);
                                objData.SaveChanges();
                            }
                            if (Chkupdate.AssignMultiId != "0")
                            {
                                var Allid = Chkupdate.AssignMultiId.Split(',');
                                for (int j = 0; j < Allid.Length; j++)
                                {
                                    int id = Convert.ToInt32(Allid[j]);
                                    ref_CheckListUsers users = new ref_CheckListUsers();
                                    users.ChecklistUserId = Chkupdate.AssginId;
                                    users.UserId = id;
                                    users.CreatedBy = session.LoginId;
                                    users.CreatedOn = System.DateTime.Now;
                                    users.ModifiedBy = session.LoginId;
                                    users.ModifiedOn = System.DateTime.Now;
                                    objData.ref_CheckListUsers.Add(users);
                                    objData.SaveChanges();
                                }
                            }

                        }

                    }
                }

                var engineLetter = getCheckList(Type, letterType);
                return engineLetter;
            }
            else
            {
                List<CommmonCheckListViewModel> defaultView = new List<CommmonCheckListViewModel>();
                return defaultView;
            }
        }

        // function to Update Or insert Comment table 
        public CommonAccRevComntsViewModel UpdateOrInsertRevCmt(string Type, int academicReviewId, string Comments, string Draft, bool isPresent, bool ApproveInd, string nextType)
        {
            //in case of save nextType is equal to Type and for submit nextType is the next queue type

            MelmarkDBEntities objData = new MelmarkDBEntities();
            session = (clsSession)HttpContext.Current.Session["UserSession"];
            if (session != null)
            {
                QstatusDetails qDetails = getQueueStatusId(session.ReferralId, Type);
                int QstatusId = qDetails.QueueStatusId;
                //Selecting Q id and Updateing previous Current id False            
                ref_Queue Queue = new ref_Queue();
                int QueueStatusId = insertQstatus(Type, Draft);

                int QstatusIdChk = qDetails.QueueStatusId;
                if (QstatusIdChk == 0)
                {
                    QstatusIdChk = getQueueStatusIdIfSubmitted(session.ReferralId, Type);
                }
                //selecting value from table
                var RevCmtChk = (from x in objData.ref_ReviewComments
                                 where (x.QueueStatusId == QstatusId && x.StudentId == session.ReferralId)
                                 select new CommonAccRevComntsViewModel
                                 {
                                     academicReviewId = x.AcReviewId,
                                 }).ToList();
                //Checkin weather the Value Present or not in table
                if (RevCmtChk.Count > 0)
                {
                    isPresent = true;
                    academicReviewId = RevCmtChk[0].academicReviewId;
                }

                //Updating Current queue
                ref_ReviewComments ReviewComments = new ref_ReviewComments();
                if (QstatusId > 0)
                {
                    if (ApproveInd == false && Type == "N")
                    {
                        MakeQstatusInactive(QueueStatusId);
                    }
                    if (isPresent == true)
                    {
                        ReviewComments = objData.ref_ReviewComments.Single(x => x.AcReviewId == academicReviewId);
                        ReviewComments.Comments = Comments;
                        ReviewComments.ApproveInd = ApproveInd;
                        ReviewComments.Draft = Draft;
                        ReviewComments.ModifiedOn = System.DateTime.Now;
                        ReviewComments.ModifiedBy = session.LoginId;
                        objData.SaveChanges();
                    }
                    else
                    {
                        ReviewComments.SchoolId = session.SchoolId;
                        ReviewComments.StudentId = session.ReferralId;
                        ReviewComments.Comments = Comments;
                        ReviewComments.ApproveInd = ApproveInd;
                        ReviewComments.Draft = Draft;
                        ReviewComments.CreatedOn = System.DateTime.Now;
                        ReviewComments.CreatedBy = session.LoginId;
                        ReviewComments.Type = Type;

                        ReviewComments.QueueStatusId = QstatusId;
                        objData.ref_ReviewComments.Add(ReviewComments);
                        objData.SaveChanges();
                    }
                }
                var RevCmt = getRevCmt(Type);

                //if (Draft == "N" && ApproveInd == true && Type != "FV" && Type != "DC" && Type != "IE" && Type != "AT")
                //{
                //    insertNewStatus(Type, nextType, session.ReferralId);
                //}
                //else

                ClsErrorLog err = new ClsErrorLog();
                err.WriteToLog("Cls Common Class");
                err.WriteToLog("Draft-- " + Draft);
                err.WriteToLog("ApproveInd-- " + ApproveInd);
                err.WriteToLog("Type-- " + Type);

                if (Draft == "N" && ApproveInd == false && Type != "FV" && Type != "DC" && Type != "IE" && Type != "AT")
                {
                    MakeQstatusInactive(QueueStatusId);
                    int qid = getQueueId("IL");
                    session.CurrentProcessId = qid;
                }
                else if (Draft == "N" && Type == "FV")
                {
                    updateWaitingListStatus(Type, nextType, session.ReferralId);
                    int qid = getQueueId(nextType);
                    session.CurrentProcessId = qid;
                }
                else if (Draft == "N" && ApproveInd == true && Type == "DC")
                {
                    makeReferralClient();
                }
                return RevCmt;
            }
            else
            {
                CommonAccRevComntsViewModel defaultview = new CommonAccRevComntsViewModel();
                return defaultview;
            }

        }

        //public void updateStatus(bool ApproveInd, string Draft, string Type, string nextType)
        //{
        //    int QueueStatusId = insertQstatus(Type, Draft);
        //    if (Draft == "N" && ApproveInd == true && Type != "FV" && Type != "DC")
        //    {
        //        insertNewStatus(Type, nextType, session.ReferralId);
        //    }
        //    else if (Draft == "N" && ApproveInd == false && Type != "FV")
        //    {
        //        MakeQstatusInactive(QueueStatusId);
        //        int qid = getQueueId("IL");
        //        session.CurrentProcessId = qid;
        //    }
        //    else if (Draft == "N" && Type == "FV")
        //    {
        //        updateWaitingListStatus(Type, nextType, session.ReferralId);
        //        int qid = getQueueId(nextType);
        //        session.CurrentProcessId = qid;
        //    }
        //    else if (Draft == "N" && ApproveInd == true && Type == "DC")
        //    {
        //        makeReferralClient();
        //    }
        //}


        //Funtion to make a Queue Process inactive
        public void MakeQstatusInactive(int qstatus)
        {
            MelmarkDBEntities objData = new MelmarkDBEntities();
            //var queueProcess = objData.ref_QueueStatus.Where(x => x.QueueStatusId == qstatus).Single();
            int qid = getQueueId("IL");
            //queueProcess.CurrentStatus = false;
            // objData.SaveChanges();
            int qProcess = getProcessId();
            var qStatusRow = objData.ref_QueueStatus.Where(x => x.StudentPersonalId == session.ReferralId && x.QueueProcess == qProcess).ToList();
            if (qStatusRow.Count > 0)
            {
                foreach (var item in qStatusRow)
                {
                    item.CurrentStatus = false;
                    item.Draft = "N";
                    objData.SaveChanges();
                }
            }



            ref_QueueStatus qStatus = new ref_QueueStatus();
            qStatus.CurrentStatus = true;
            qStatus.CreatedOn = System.DateTime.Now;
            qStatus.EndDate = System.DateTime.Now;
            qStatus.StartDate = System.DateTime.Now;
            qStatus.QueueId = qid;
            qStatus.SchoolId = session.SchoolId;
            qStatus.StudentPersonalId = session.ReferralId;
            qStatus.Draft = "Y";
            qStatus.QueueProcess = qProcess;// queueProcess.QueueProcess;
            qStatus.CreatedBy = session.LoginId;
            qStatus.CreatedOn = System.DateTime.Now;
            objData.ref_QueueStatus.Add(qStatus);
            objData.SaveChanges();


        }

        // function to Update Or insert CallLog table  
        public CommonCallLogViewModel UpdateOrInsertCallLog(string Type, int academicReviewId, DateTime? AppntTime, string Draft, DateTime? CallTime, int CallLogId, string Conversation, string NameOfContact, string StaffName, bool isPresent, int QstatusId)
        {
            MelmarkDBEntities objData = new MelmarkDBEntities();
            session = (clsSession)HttpContext.Current.Session["UserSession"];
            if (session != null)
            {

                var CallLogVarChk = (from x in objData.ref_CallLogs
                                     where (x.QueueStatusId == QstatusId && x.StudentId == session.ReferralId && x.AcReviewId == academicReviewId)
                                     select new CommonCallLogViewModel
                                     {
                                         CallLogId = x.CallLogId,
                                         academicReviewId = x.AcReviewId
                                     }).ToList();
                CommonCallLogViewModel defaltNullVal = new CommonCallLogViewModel();
                //Checkin weather the Value Present or not in table
                if (CallLogVarChk.Count > 0)
                {
                    isPresent = true;
                    CallLogId = CallLogVarChk[0].CallLogId;
                }

                ref_CallLogs CallLogVar = new ref_CallLogs();
                if (isPresent == true)
                {
                    CallLogVar = objData.ref_CallLogs.Single(x => x.CallLogId == CallLogId);
                    CallLogVar.AppointmentTime = AppntTime;
                    CallLogVar.CallTime = CallTime;
                    CallLogVar.Draft = Draft;
                    if (Conversation != "")
                    {
                        CallLogVar.Conversation = Conversation;
                    }
                    CallLogVar.Nameofcontact = NameOfContact;
                    CallLogVar.StaffName = StaffName;
                    CallLogVar.ModifiedOn = System.DateTime.Now;
                    CallLogVar.ModifiedBy = session.LoginId;
                    CallLogVar.QueueStatusId = QstatusId;
                    objData.SaveChanges();
                }
                else
                {
                    CallLogVar.AppointmentTime = AppntTime;
                    CallLogVar.CallTime = CallTime;
                    CallLogVar.Draft = Draft;
                    if (Conversation != "")
                    {
                        CallLogVar.Conversation = Conversation;
                    }
                    CallLogVar.Nameofcontact = NameOfContact;
                    CallLogVar.StaffName = StaffName;
                    CallLogVar.CreatedOn = System.DateTime.Now;
                    CallLogVar.CreatedBy = session.LoginId;
                    CallLogVar.SchoolId = session.SchoolId;
                    CallLogVar.StudentId = session.ReferralId;
                    CallLogVar.Type = Type;
                    CallLogVar.AcReviewId = academicReviewId;
                    CallLogVar.QueueStatusId = QstatusId;
                    objData.ref_CallLogs.Add(CallLogVar);
                    objData.SaveChanges();
                }

                var CallLog = getRevCallLog(Type, academicReviewId);

                return CallLog;
            }
            else
            {
                CommonCallLogViewModel defaultView = new CommonCallLogViewModel();
                return defaultView;
            }
        }

        //Function to get the user list in popup for cheklist
        public List<UsersListModel> getUserList(int schoolId)
        {
            MelmarkDBEntities objData = new MelmarkDBEntities();

            var userList = (from x in objData.Users
                            where (x.SchoolId == schoolId && x.ActiveInd == "A")
                            select new UsersListModel
                            {
                                userId = x.UserId,
                                userFName = x.UserFName,
                                UserLName = x.UserLName
                            }).ToList();

            return userList;

        }

        //function to get the details of Q Process of which current status=true
        public QstatusDetails getQueueStatusId(int studentId, string typeVal)
        {

            MelmarkDBEntities objData = new MelmarkDBEntities();
            int ProcessId = getProcessId();
            int QueueId = getQueueId(typeVal);

            var qStatusRowVals = (from x in objData.ref_QueueStatus
                                  join y in objData.ref_Queue on x.QueueId equals y.QueueId
                                  where (x.QueueProcess == ProcessId && x.StudentPersonalId == studentId && y.QueueType == typeVal)
                                  select new QstatusDetails
                                  {
                                      QueueStatusId = x.QueueStatusId,
                                      Type = y.QueueType,
                                      CurrentStatus = x.CurrentStatus,
                                      DraftStatus = x.Draft,
                                      qProcess = x.QueueProcess
                                  }).ToList();

            QstatusDetails defaltVal = new QstatusDetails();
            if (qStatusRowVals.Count > 0)
            {
                if (qStatusRowVals[qStatusRowVals.Count - 1].QueueStatusId != 0)
                {
                    defaltVal = qStatusRowVals[qStatusRowVals.Count - 1];
                }

            }
            else
            {
                var qStatusUpdate = objData.ref_QueueStatus.Where(x => x.StudentPersonalId == session.ReferralId && x.QueueProcess == ProcessId).ToList();
                if (qStatusUpdate.Count > 0)
                {
                    foreach (var StatusUpdate in qStatusUpdate)
                    {
                        if (StatusUpdate.CurrentStatus == true)
                        {
                            StatusUpdate.CurrentStatus = false;
                            objData.SaveChanges();
                        }
                    }
                }
                //Adding new row To QStatus Table With CurrentId True
                ref_QueueStatus qStatus = new ref_QueueStatus();
                qStatus.CurrentStatus = true;
                qStatus.CreatedOn = System.DateTime.Now;
                qStatus.EndDate = System.DateTime.Now;
                qStatus.StartDate = System.DateTime.Now;
                qStatus.QueueId = QueueId;
                qStatus.SchoolId = session.SchoolId;
                qStatus.StudentPersonalId = session.ReferralId;
                qStatus.Draft = "Y";
                qStatus.QueueProcess = ProcessId;
                qStatus.CreatedBy = session.LoginId;
                qStatus.CreatedOn = System.DateTime.Now;
                objData.ref_QueueStatus.Add(qStatus);
                objData.SaveChanges();
                var qStatusNew = objData.ref_QueueStatus.Where(x => x.StudentPersonalId == studentId && x.CurrentStatus == true && x.QueueProcess == ProcessId).ToList();
                defaltVal.QueueStatusId = qStatusNew[0].QueueStatusId;
                defaltVal.DraftStatus = qStatusNew[0].Draft;
                session.CurrentProcessId = qStatusNew[0].QueueId;
            }
            return defaltVal;

        }

        //Function to get the qStatus id Of the Process according to the type corresponding Queue Process
        public int getQueueStatusIdIfSubmitted(int studentId, string typeVal)
        {

            MelmarkDBEntities objData = new MelmarkDBEntities();
            int qProcessId = getProcessId();
            int QueueId = getQueueId(typeVal);
            var qStatusRowVals = objData.ref_QueueStatus.Where(x => x.QueueProcess == qProcessId && x.StudentPersonalId == studentId && x.QueueId == QueueId && x.Draft == "Y").ToList();
            //var qStatusRowVals = (from x in objData.ref_QueueStatus
            //                      join y in objData.ref_Queue on x.QueueId equals y.QueueId
            //                      where (x.QueueProcess == qProcessId && x.StudentPersonalId == studentId && y.QueueType == typeVal && x.Draft == "N")
            //                      select new QstatusDetails
            //                      {
            //                          QueueStatusId = x.QueueStatusId,
            //                          Type = y.QueueType,
            //                          CurrentStatus = x.CurrentStatus,
            //                          DraftStatus = x.Draft
            //                          // qProcess=y.
            //                      }).ToList();

            QstatusDetails defaltVal = new QstatusDetails();

            if (qStatusRowVals.Count > 0)
            {
                return 0;
            }
            else
            {
                return 1;
                //return qStatusRowVals[qStatusRowVals.Count - 1].QueueStatusId;
            }
        }

        //Function to get the Queue Id corresponding to the Type

        public int getClientListCount(int SchoolId, int QueueId)
        {
            MelmarkDBEntities objData = new MelmarkDBEntities();
            var qStatusRowVals = objData.ref_QueueStatus.Where(x => x.SchoolId == SchoolId && x.QueueId == QueueId && x.Draft == "N" && x.CurrentStatus == false).ToList();
            if (qStatusRowVals != null)
            {
                return qStatusRowVals.Count;
            }
            else
            {
                return 0;
            }
        }


        public int getQueueId(string typeVal)
        {
            MelmarkDBEntities objData = new MelmarkDBEntities();
            var Queue = objData.ref_Queue.Where(x => x.QueueType == typeVal).ToList();
            if (Queue.Count > 0)
            {
                return Queue[0].QueueId;
            }
            else
            {
                return 0;
            }
        }

        //---- List 3 - Task #30 [20-Oct-2020] - (Start) ---//
        public string getQueueType(int Queid)
        {
            MelmarkDBEntities objData = new MelmarkDBEntities();
            var Queue = objData.ref_Queue.Where(x => x.QueueId == Queid).ToList();
            if (Queue.Count > 0)
            {
                return Queue[0].QueueType;
            }
            else
            {
                return null;
            }
        }
        //---- List 3 - Task #30 [20-Oct-2020] - (End) ---//

        public int getQueueStatusIdCurrent(int QueueId, int ProcessId, int ReferralId)
        {
            MelmarkDBEntities objData = new MelmarkDBEntities();
            var Queue = objData.ref_QueueStatus.Where(x => x.QueueId == QueueId && x.QueueProcess == ProcessId && x.StudentPersonalId == ReferralId).OrderByDescending(x => x.QueueStatusId).FirstOrDefault();
            if (Queue != null)
            {
                return Queue.QueueStatusId;
            }
            else
            {
                return 0;
            }
        }
        public int getQueueIdCurrent(int ProcessId, int ReferralId)
        {
            MelmarkDBEntities objData = new MelmarkDBEntities();
            var Queue = objData.ref_QueueStatus.Where(x => x.QueueProcess == ProcessId && x.StudentPersonalId == ReferralId).OrderByDescending(x => x.QueueStatusId).FirstOrDefault();
            if (Queue != null)
            {
                return Queue.QueueId;
            }
            else
            {
                return 0;
            }
        }
        //Function to get the Process Id of Current Q Process
        public int getProcessId()
        {
            MelmarkDBEntities objData = new MelmarkDBEntities();
            session = (clsSession)HttpContext.Current.Session["UserSession"];
            if (session != null)
            {
                var process = objData.ref_QueueStatus.Where(x => x.StudentPersonalId == session.ReferralId).OrderByDescending(X => X.QueueStatusId).FirstOrDefault();
                if (process != null)
                {
                    return process.QueueProcess;
                }
                else
                {
                    return 1;
                }
            }
            else
                return 0;

        }
        public int getProcessIdForStudent(int ReferralId)
        {
            MelmarkDBEntities objData = new MelmarkDBEntities();
            session = (clsSession)HttpContext.Current.Session["UserSession"];
            if (session != null)
            {
                var process = objData.ref_QueueStatus.Where(x => x.StudentPersonalId == ReferralId).OrderByDescending(X => X.QueueStatusId).FirstOrDefault();
                if (process != null)
                {
                    return process.QueueProcess;
                }
                else
                {
                    return 1;
                }
            }
            else
                return 0;

        }

        //Function to check if all subQueues of Acceptance Procedure is submitted
        public bool AcceptanceSubQSubmitted()
        {
            MelmarkDBEntities objData = new MelmarkDBEntities();
            session = (clsSession)HttpContext.Current.Session["UserSession"];
            bool AllSubmitted = true;
            if (session != null)
            {
                int currentProcessId = getProcessId();
                int AcceptanceQid = getQueueId("AP");
                var subQueues = objData.ref_Queue.Where(x => x.MasterId == AcceptanceQid).ToList();
                foreach (var item in subQueues)
                {
                    if (item.QueueType != "DC")
                    {
                        var ifStatusPresent = objData.ref_QueueStatus.Where(x => x.StudentPersonalId == session.ReferralId && x.QueueId == item.QueueId && x.QueueProcess == currentProcessId && x.CurrentStatus == false && x.Draft == "N").ToList();
                        if (ifStatusPresent.Count == 0)
                        {
                            AllSubmitted = false;
                            return AllSubmitted;
                        }
                    }
                }
            }
            else
            {
                AllSubmitted = false;
                return AllSubmitted;
            }
            return AllSubmitted;
        }

        //Function to check if any subQueues of Acceptance Procedure is in draft mode
        public bool AcceptanceSubQDraftMode(int Qid)
        {
            bool AllSubmitted = true;
            string Status = "";
            Status = IsSubmit(Qid);

            if (Status == "")
            {
                AllSubmitted = true;
            }
            else
            {
                AllSubmitted = false;
            }

            //MelmarkDBEntities objData = new MelmarkDBEntities();
            //session = (clsSession)HttpContext.Current.Session["UserSession"];

            //if (session != null)
            //{
            //    int currentProcessId = getProcessId();
            //    int AcceptanceQid = getQueueId("AP");
            //    var subQueues = objData.ref_Queue.Where(x => x.MasterId == AcceptanceQid).ToList();
            //    foreach (var item in subQueues)
            //    {
            //        if (item.QueueId != Qid)
            //        {
            //            var ifStatusPresent = objData.ref_QueueStatus.Where(x => x.StudentPersonalId == session.ReferralId && x.QueueId == item.QueueId && x.QueueProcess == currentProcessId && x.CurrentStatus == true && x.Draft == "Y").ToList();
            //            if (ifStatusPresent.Count > 0)
            //            {
            //                AllSubmitted = false;
            //                return AllSubmitted;
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    AllSubmitted = false;
            //    return AllSubmitted;
            //}
            return AllSubmitted;
        }

        // funtion To insert New Qid to Next Q Process
        public void insertNewStatus(string type, string nextType, int ReferralId)
        {
            MelmarkDBEntities objData = new MelmarkDBEntities();
            int qId = getQueueId(type);
            int qIdNext = getQueueId(nextType);
            int qProcess = getProcessId();
            var qStatusRow = objData.ref_QueueStatus.Where(x => x.StudentPersonalId == ReferralId && x.QueueId == qId && x.QueueProcess == qProcess).ToList();
            if (qStatusRow.Count > 0)
            {
                foreach (var item in qStatusRow)
                {
                    item.CurrentStatus = true;
                    item.ModifiedOn = System.DateTime.Now;
                    item.ModifiedBy = session.LoginId;
                    item.Draft = "N";
                    item.EndDate = System.DateTime.Now;
                    item.StartDate = System.DateTime.Now;
                    objData.SaveChanges();
                }
            }

            qStatusRow = objData.ref_QueueStatus.Where(x => x.StudentPersonalId == ReferralId && x.QueueId != qId && x.QueueProcess == qProcess).ToList();
            if (qStatusRow.Count > 0)
            {
                foreach (var item in qStatusRow)
                {
                    item.CurrentStatus = false;
                    objData.SaveChanges();
                }
            }
            //Adding new row To QStatus Table With CurrentId True

            //qStatusRow = objData.ref_QueueStatus.Where(x => x.StudentPersonalId == ReferralId && x.QueueId == qIdNext && x.QueueProcess == qProcess).ToList();
            //if (qStatusRow.Count != 0)
            //{
            //    var qStatusRowNext = objData.ref_QueueStatus.Where(x => x.StudentPersonalId == ReferralId && x.Draft == "Y" && x.QueueId == qIdNext && x.QueueProcess == qProcess).ToList();
            //    if (qStatusRowNext.Count > 0)
            //    {
            //        qStatusRowNext[qStatusRowNext.Count - 1].CurrentStatus = true;
            //        objData.SaveChanges();
            //    }
            //}
            //else
            //{
            //    ref_QueueStatus qStatus = new ref_QueueStatus();
            //    qStatus.CurrentStatus = true;
            //    qStatus.CreatedOn = System.DateTime.Now;
            //    qStatus.EndDate = System.DateTime.Now;
            //    qStatus.StartDate = System.DateTime.Now;
            //    qStatus.QueueId = qIdNext;
            //    qStatus.SchoolId = session.SchoolId;
            //    qStatus.StudentPersonalId = session.ReferralId;
            //    qStatus.Draft = "Y";
            //    qStatus.QueueProcess = qProcess;
            //    qStatus.CreatedBy = session.LoginId;
            //    qStatus.CreatedOn = System.DateTime.Now;
            //    objData.ref_QueueStatus.Add(qStatus);
            //    objData.SaveChanges();
            //}
            var qStatusNew = objData.ref_QueueStatus.Where(x => x.StudentPersonalId == ReferralId && x.CurrentStatus == true && x.QueueProcess == qProcess).ToList();
            if (qStatusNew.Count > 0) session.CurrentProcessId = qStatusNew[0].QueueId;
        }

        //Function to insert QStatus in Acceptance
        public void insertQstatusForAcceptance(string Type, string Draft)
        {
            MelmarkDBEntities objData = new MelmarkDBEntities();
            session = (clsSession)HttpContext.Current.Session["UserSession"];
            if (session != null)
            {
                int qId = getQueueId(Type);
                int qProcess = getProcessId();

                //deleting existing current Qstatus
                var qStatusRow = objData.ref_QueueStatus.Where(x => x.StudentPersonalId == session.ReferralId && x.SchoolId == session.SchoolId && x.CurrentStatus == true && x.QueueProcess == qProcess).ToList();
                if (qStatusRow.Count > 0)
                {
                    objData.ref_QueueStatus.Remove(qStatusRow[0]);
                    objData.SaveChanges();
                }
                //Adding new row To QStatus Table With CurrentId True
                ref_QueueStatus qStatus = new ref_QueueStatus();
                qStatus.CurrentStatus = true;
                qStatus.CreatedOn = System.DateTime.Now;
                qStatus.EndDate = System.DateTime.Now;
                qStatus.StartDate = System.DateTime.Now;
                qStatus.QueueId = qId;
                qStatus.SchoolId = session.SchoolId;
                qStatus.StudentPersonalId = session.ReferralId;
                qStatus.Draft = Draft;
                qStatus.QueueProcess = qProcess;
                qStatus.CreatedBy = session.LoginId;
                qStatus.CreatedOn = System.DateTime.Now;
                qStatus.ModifiedBy = session.LoginId;
                qStatus.ModifiedOn = System.DateTime.Now;
                objData.ref_QueueStatus.Add(qStatus);
                objData.SaveChanges();

            }
        }

        public string IsSubmit(int QueueId)
        {
            MelmarkDBEntities objData = new MelmarkDBEntities();
            session = (clsSession)HttpContext.Current.Session["UserSession"];
            string PrevQueue = "";
            int QueueProcss = 0;
            if (QueueId != 0)
            {

                var SelectedQueueStatus = objData.ref_Queue.Where(x => x.QueueId == QueueId).ToList();
                if (SelectedQueueStatus.Count > 0)
                {
                    int prevQId = SelectedQueueStatus[0].PrevQueueId;
                    QueueProcss = getProcessId();

                    if (prevQId != 0)
                    {
                        var Qname = objData.ref_Queue.Where(x => x.QueueId == prevQId).ToList();
                        string QueueName = Qname[0].QueueName;
                        var prevQueueStatus = objData.ref_QueueStatus.Where(x => x.QueueId == prevQId && x.CurrentStatus == false && x.StudentPersonalId == session.ReferralId && x.QueueProcess == QueueProcss).ToList();
                        if (prevQueueStatus.Count != 0)
                        {
                            foreach (var checkYExist in prevQueueStatus)
                            {
                                if (checkYExist.Draft == "Y")
                                {
                                    PrevQueue = clsGeneral.warningMsg(QueueName + " is not submitted");
                                    return PrevQueue;
                                }
                            }

                        }
                        else if (prevQueueStatus.Count == 0)
                        {
                            PrevQueue = clsGeneral.warningMsg(QueueName + " is not submitted");
                            return PrevQueue;
                        }
                    }
                }
            }

            if (PrevQueue == "")
            {
                var prevStatus = objData.ref_QueueStatus.Where(x => x.QueueId < QueueId && x.StudentPersonalId == session.ReferralId && x.QueueProcess == QueueProcss).ToList();
                if (prevStatus.Count != 0)
                {
                    foreach (var checkYExist in prevStatus)
                    {
                        if (checkYExist.Draft == "Y")
                        {
                            var Qname = objData.ref_Queue.Where(x => x.QueueId == checkYExist.QueueId).ToList();
                            string QueueName = Qname[0].QueueName;
                            PrevQueue = clsGeneral.warningMsg(QueueName + " is not submitted");
                            return PrevQueue;
                        }
                    }

                }
            }

            return PrevQueue;
        }


        //Fumction to insert new entry to Qstatus
        public int insertQstatus(string Type, string Draft)
        {
            MelmarkDBEntities objData = new MelmarkDBEntities();
            session = (clsSession)HttpContext.Current.Session["UserSession"];
            int QueueStatusId = 0;
            if (session != null)
            {
                QstatusDetails qDetails = getQueueStatusId(session.ReferralId, Type);
                int QstatusId = qDetails.QueueStatusId;


                //Selecting Q id and Updateing previous Current id False            
                ref_Queue Queue = new ref_Queue();
                int qId = getQueueId(Type);
                int qProcess = getProcessId();
                int Count = 0;
                var qStatusRow = objData.ref_QueueStatus.Where(x => x.StudentPersonalId == session.ReferralId && x.QueueId == qId && x.CurrentStatus == true && x.QueueProcess == qProcess).ToList();
                if (qStatusRow.Count > 0)
                {
                    Count = qStatusRow.Count - 1;


                    var qStatusUpdate = objData.ref_QueueStatus.Where(x => x.StudentPersonalId == session.ReferralId && x.QueueId == qId && x.Draft == "Y" && x.QueueProcess == qProcess).ToList();
                    if (qStatusUpdate.Count > 0)
                    {
                        foreach (var StatusUpdate in qStatusUpdate)
                        {
                            if (Draft == "N")
                            {
                                StatusUpdate.CurrentStatus = false;
                            }
                            else
                            {
                                StatusUpdate.CurrentStatus = true;
                            }
                            StatusUpdate.Draft = Draft;
                            objData.SaveChanges();
                        }

                    }

                    if (Draft == "Y")
                    {
                        qStatusRow[Count].CurrentStatus = true;
                    }
                    else
                    {
                        qStatusRow[Count].CurrentStatus = false;
                    }
                    qStatusRow[Count].ModifiedOn = System.DateTime.Now;
                    qStatusRow[Count].ModifiedBy = session.LoginId;
                    //qStatusRow[0].QueueStatus = nextType;
                    qStatusRow[Count].EndDate = System.DateTime.Now;
                    qStatusRow[Count].StartDate = System.DateTime.Now;
                    qStatusRow[Count].Draft = Draft;
                    objData.SaveChanges();
                    var qStatusRow1 = objData.ref_QueueStatus.Where(x => x.StudentPersonalId == session.ReferralId && x.QueueId == qId && x.CurrentStatus == true && x.QueueProcess == qProcess).ToList();
                    QueueStatusId = qStatusRow1.Count > 0 ? qStatusRow1[Count].QueueStatusId : 0;

                    if (QueueStatusId == 0)
                    {
                        qStatusRow1 = objData.ref_QueueStatus.Where(x => x.StudentPersonalId == session.ReferralId && x.Draft == "Y" && x.QueueProcess == qProcess).ToList();
                        if (qStatusRow1 != null)
                        {
                            if (qStatusRow1.Count > 0)
                            {
                                qStatusRow1[0].CurrentStatus = true;
                                objData.SaveChanges();
                                QueueStatusId = qStatusRow1[0].QueueStatusId;
                            }
                        }

                    }


                }
                else
                {
                    var qStatusUpdate = objData.ref_QueueStatus.Where(x => x.StudentPersonalId == session.ReferralId && x.QueueProcess == qProcess).ToList();
                    if (qStatusUpdate.Count > 0)
                    {
                        foreach (var StatusUpdate in qStatusUpdate)
                        {
                            if (StatusUpdate.CurrentStatus == true)
                            {
                                StatusUpdate.CurrentStatus = false;
                                objData.SaveChanges();
                            }
                            if (StatusUpdate.QueueId == qId)
                            {
                                StatusUpdate.Draft = Draft;
                                if (Draft == "Y")
                                {
                                    StatusUpdate.CurrentStatus = true;
                                }
                                else
                                {
                                    StatusUpdate.CurrentStatus = false;
                                }
                                objData.SaveChanges();
                            }
                        }

                    }

                    var qStatusRowS = objData.ref_QueueStatus.Where(x => x.StudentPersonalId == session.ReferralId && x.Draft == "Y" && x.QueueProcess == qProcess).OrderByDescending(X => X.QueueStatusId).FirstOrDefault();
                    if (qStatusRowS == null)
                    {
                        var qStatusRow1 = objData.ref_QueueStatus.Where(x => x.StudentPersonalId == session.ReferralId && x.QueueId == qId && x.QueueProcess == qProcess).ToList();
                        //Adding new row To QStatus Table With CurrentId True

                        if (qStatusRow1.Count == 0)
                        {
                            ref_QueueStatus qStatus = new ref_QueueStatus();
                            qStatus.CurrentStatus = true;
                            qStatus.CreatedOn = System.DateTime.Now;
                            qStatus.EndDate = System.DateTime.Now;
                            qStatus.StartDate = System.DateTime.Now;
                            qStatus.QueueId = qId;
                            qStatus.SchoolId = session.SchoolId;
                            qStatus.StudentPersonalId = session.ReferralId;
                            qStatus.Draft = Draft;
                            qStatus.QueueProcess = qProcess;
                            qStatus.CreatedBy = session.LoginId;
                            qStatus.CreatedOn = System.DateTime.Now;
                            qStatus.ModifiedBy = session.LoginId;
                            qStatus.ModifiedOn = System.DateTime.Now;
                            objData.ref_QueueStatus.Add(qStatus);
                            objData.SaveChanges();
                        }

                    }
                    else if (Draft != "Y")
                    {
                        qStatusRowS.CurrentStatus = true;
                        objData.SaveChanges();
                    }

                    var LastRow = objData.ref_QueueStatus.Where(x => x.StudentPersonalId == session.ReferralId && x.CurrentStatus == true && x.QueueProcess == qProcess).ToList();
                    QueueStatusId = LastRow.Count > 0 ? LastRow[0].QueueStatusId : 0;
                }

            }
            return QueueStatusId;
        }

        public void updateWaitingListStatus(string type, string nextType, int ReferralId)
        {
            MelmarkDBEntities objData = new MelmarkDBEntities();
            int qId = getQueueId(type);
            int qIdNext = getQueueId(nextType);
            int qProcess = getProcessId();
            var qStatusRowPrev = objData.ref_QueueStatus.Where(x => x.StudentPersonalId == session.ReferralId && x.QueueId == qId && x.CurrentStatus == true && x.QueueProcess == qProcess).ToList();
            if (qStatusRowPrev.Count > 0)
            {
                qStatusRowPrev[0].CurrentStatus = false;
                qStatusRowPrev[0].ModifiedOn = System.DateTime.Now;
                qStatusRowPrev[0].ModifiedBy = session.LoginId;
                qStatusRowPrev[0].EndDate = System.DateTime.Now;
                qStatusRowPrev[0].StartDate = System.DateTime.Now;
                qStatusRowPrev[0].Draft = "N";
                objData.SaveChanges();
            }
            var qStatusRow = objData.ref_QueueStatus.Where(x => x.StudentPersonalId == ReferralId && x.QueueId == qIdNext && x.CurrentStatus == true && x.QueueProcess == qProcess).ToList();
            if (qStatusRow.Count > 0)
            {

                qStatusRow[0].CurrentStatus = true;
                qStatusRow[0].ModifiedOn = System.DateTime.Now;
                qStatusRow[0].ModifiedBy = session.LoginId;
                qStatusRow[0].QueueId = qIdNext;
                qStatusRow[0].EndDate = System.DateTime.Now;
                qStatusRow[0].StartDate = System.DateTime.Now;
                qStatusRow[0].Draft = "Y";
                objData.SaveChanges();
            }
            else
            {
                var qStatusRows = objData.ref_QueueStatus.Where(x => x.StudentPersonalId == session.ReferralId && x.QueueProcess == qProcess).ToList();
                if (qStatusRows.Count > 0)
                {
                    foreach (var item in qStatusRows)
                    {
                        item.CurrentStatus = false;
                        objData.SaveChanges();
                    }
                }
                //qStatusRows = objData.ref_QueueStatus.Where(x => x.StudentPersonalId == session.ReferralId && x.QueueId == qIdNext && x.QueueProcess == qProcess).ToList();
                //if (qStatusRows.Count > 0)
                //{
                //    qStatusRows[0].CurrentStatus = true;
                //    qStatusRows[0].Draft = "Y";
                //    objData.SaveChanges();
                //}else
                //{
                //Adding new row To QStatus Table With CurrentId True
                ref_QueueStatus qStatus = new ref_QueueStatus();
                qStatus.CurrentStatus = true;
                qStatus.CreatedOn = System.DateTime.Now;
                qStatus.EndDate = System.DateTime.Now;
                qStatus.StartDate = System.DateTime.Now;
                qStatus.QueueId = qIdNext;
                qStatus.SchoolId = session.SchoolId;
                qStatus.StudentPersonalId = session.ReferralId;
                qStatus.Draft = "Y";
                qStatus.QueueProcess = qProcess;
                qStatus.CreatedBy = session.LoginId;
                qStatus.CreatedOn = System.DateTime.Now;
                objData.ref_QueueStatus.Add(qStatus);
                objData.SaveChanges();
                //}
            }
        }

        public bool checkInactive()
        {
            MelmarkDBEntities objData = new MelmarkDBEntities();
            session = (clsSession)HttpContext.Current.Session["UserSession"];

            if (session != null)
            {
                var procesId = getProcessId();
                int qid = getQueueId("IL");
                var chkInactive = objData.ref_QueueStatus.Where(x => x.CurrentStatus == true && x.StudentPersonalId == session.ReferralId && x.QueueProcess == procesId && x.QueueId == qid).ToList();
                if (chkInactive.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        public void makeReferralClient()
        {
            MelmarkDBEntities objData = new MelmarkDBEntities();
            session = (clsSession)HttpContext.Current.Session["UserSession"];
            int qId = getQueueId("DC");
            int qProcess = getProcessId();
            HttpContext.Current.Session["Client"] = null;
            HttpContext.Current.Session["QueueList"] = null;
            var queueList = objData.ref_Queue.OrderBy(x => x.QueueId).ToList();
            string qNames = "";
            ClsErrorLog err = new ClsErrorLog();
            err.WriteToLog("Make Referral CLient");

            var student = objData.StudentPersonals.Where(x => x.StudentPersonalId == session.ReferralId).SingleOrDefault();
            if (student.FundingVerification == false)
            {
                qNames = qNames + "|Funding Verification";
            }
            if (student.InactiveList == true)
            {
                qNames = qNames + "|Inactive List";
            }
            if (student.WaitingList == true)
            {
                qNames = qNames + "|Waiting List";
            }

            foreach (var Qitem in queueList)
            {
                if (Qitem.MasterId > 1)
                {
                    var refQueueList = objData.ref_QueueStatus.Where(x => x.StudentPersonalId == session.ReferralId && x.QueueId == Qitem.QueueId && x.Draft == "N" && x.QueueProcess == qProcess).ToList();
                    if (refQueueList.Count > 0)
                    {

                    }
                    else
                    {
                        qNames = qNames + "|" + Qitem.QueueName + "$" + Qitem.QueueId;
                    }
                }

            }
            //    err.WriteToLog("Session--- :" + HttpContext.Current.Session["QueueList"].ToString());
            HttpContext.Current.Session["QueueList"] = qNames;
            if (qNames == "")
            {

                var qStatusRow = objData.ref_QueueStatus.Where(x => x.StudentPersonalId == session.ReferralId && x.QueueId == qId && x.Draft == "N" && x.QueueProcess == qProcess).ToList();
                if (qStatusRow.Count > 0)
                {
                    int processId = qStatusRow[0].QueueProcess;
                    //qStatusRow[0].CurrentStatus = false;
                    //qStatusRow[0].ModifiedOn = System.DateTime.Now;
                    //qStatusRow[0].ModifiedBy = session.LoginId;
                    //qStatusRow[0].QueueId = getQueueId("DC");
                    //qStatusRow[0].EndDate = System.DateTime.Now;
                    //qStatusRow[0].StartDate = System.DateTime.Now;
                    //qStatusRow[0].Draft = "N";
                    //objData.SaveChanges();

                    //insert clientlist queueid
                    ref_QueueStatus QStatus = new ref_QueueStatus();
                    QStatus.CurrentStatus = false;
                    QStatus.CreatedOn = System.DateTime.Now;
                    QStatus.CreatedBy = session.LoginId;
                    QStatus.ModifiedOn = System.DateTime.Now;
                    QStatus.ModifiedBy = session.LoginId;
                    QStatus.QueueId = getQueueId("CL");
                    QStatus.EndDate = System.DateTime.Now;
                    QStatus.StartDate = System.DateTime.Now;
                    QStatus.Draft = "N";
                    QStatus.QueueProcess = processId;
                    QStatus.SchoolId = session.SchoolId;
                    QStatus.StudentPersonalId = session.ReferralId;
                    objData.ref_QueueStatus.Add(QStatus);
                    objData.SaveChanges();
                }
                var studentPersonalDetails = objData.StudentPersonals.Single(x => x.StudentPersonalId == session.ReferralId);

                studentPersonalDetails.AdmissionDate = System.DateTime.Now;
                studentPersonalDetails.StudentType = "Client";
                objData.SaveChanges();
            }
        }

        //Type1 = ChecklistUser Type2= ActiveRefReferral
        public IList<ActiveReferalNdUser> GetActiveReferalNdUser(string TypeProcedure)
        {
            MelmarkDBEntities objData = new MelmarkDBEntities();
            IList<ActiveReferalNdUser> val = null;
            session = (clsSession)HttpContext.Current.Session["UserSession"];
            clsReferral objClsRef = new clsReferral();
            ClsCommon com = new ClsCommon();
            if (session != null)
            {
                if (session.ReferralId > 0)
                {
                    string referalId = session.ReferralId.ToString();
                    val = (from objref in objData.ActiveReferalNdUser(TypeProcedure, session.SchoolId, session.LoginId)
                           select new ActiveReferalNdUser
                           {
                               Gender = objref.Gender,
                               AssignDate = objref.AssignDate.ToString(),
                               ImageUrl = objClsRef.GetStudentImage(Convert.ToInt16((objref.QueueId).Split('_')[0])),
                               QueueName = objref.QueueName,
                               ReferralName = objref.RefferalName,
                               QueueId = objref.QueueId,//Convert.ToInt16((objref.QueueId).Split('_')[1]),
                               UserName = objref.UserName,
                               CheckListName = objref.CheckListName,
                               ReferralId = objref.QueueId.Split('_')[0]
                           }).ToList();
                    val = val.Where(c => c.ReferralId == referalId).ToList();
                }
                else
                {
                    val = (from objref in objData.ActiveReferalNdUser(TypeProcedure, session.SchoolId, session.LoginId)
                           select new ActiveReferalNdUser
                           {
                               Gender = objref.Gender,
                               AssignDate = objref.AssignDate.ToString(),
                               ImageUrl = objClsRef.GetStudentImage(Convert.ToInt16((objref.QueueId).Split('_')[0])),
                               QueueName = objref.QueueName,
                               ReferralName = objref.RefferalName,
                               QueueId = objref.QueueId,//Convert.ToInt16((objref.QueueId).Split('_')[1]),
                               UserName = objref.UserName,
                               CheckListName = objref.CheckListName,
                               ReferralId = objref.QueueId.Split('_')[0]
                           }).ToList();
                }
            }
            return val;
        }
        public IList<StudentSearchDetails> GetStudentSearch(string SearchName, int page = 1, int pageSize = 10)
        {
            MelmarkDBEntities objData = new MelmarkDBEntities();
            IList<StudentSearchDetails> val = null;
            StudentSearchDetails obj = new StudentSearchDetails();
            //PagingModel Pgmodel = new PagingModel();
            //obj.pageModel.CurrentPageIndex = page;
            //obj.pageModel.PageSize = pageSize;
            string studentype = "Referral";
            session = (clsSession)HttpContext.Current.Session["UserSession"];
            clsReferral objClsRef = new clsReferral();
            int n;
            bool isNumeric = int.TryParse(SearchName, out n);
            if (session != null)
            {
                int ReferralId = 0;

                if (SearchName != "undefined" && SearchName != "" && SearchName != null)
                {
                    string[] args = SearchName.Split('$');
                    if (args.Length > 1)
                    {
                        if (args[1].ToString() == "Referral")
                            studentype = "Referral";
                        else
                            studentype = "Client";
                    }
                    else
                        studentype = "Referral";
                    if (args[0] != "undefined" && args[0] != "" && args[0] != null)
                    {
                        string name = args[0].ToString();
                        val = (from objref in objData.StudentPersonals
                               where (objref.StudentType == studentype && (objref.LastName.ToLower().StartsWith(name.ToLower()) || objref.FirstName.ToLower().StartsWith(name.ToLower()) || (objref.LastName.ToLower() + "," + objref.FirstName.ToLower()).Contains(name.ToLower()) || objref.StudentPersonalId.Equals(n)))
                               select new StudentSearchDetails
                               {
                                   ReferralId = objref.StudentPersonalId,
                                   Gender = objref.Gender,
                                   BirthDate = SqlFunctions.StringConvert((double)objref.BirthDate.Value.Month).TrimStart() + "/" + SqlFunctions.DateName("day", objref.BirthDate).Trim() + "/" + SqlFunctions.DateName("year", objref.BirthDate),
                                   AdmissionDate = SqlFunctions.StringConvert((double)objref.AdmissionDate.Value.Month).TrimStart() + "/" + SqlFunctions.DateName("day", objref.AdmissionDate).Trim() + "/" + SqlFunctions.DateName("year", objref.AdmissionDate),
                                   ReferralName = (objref.LastName ?? "") + "," + (objref.FirstName ?? ""),
                                   LastQueue = "",
                                   FundingVerification = objref.FundingVerification,
                                   InactiveList = objref.InactiveList,
                                   WaitingList = objref.WaitingList,
                                   StudentType = objref.StudentType,
                               }).ToList();
                    }
                    else
                    {
                        val = (from objref in objData.StudentPersonals
                               where (objref.StudentType == studentype)
                               select new StudentSearchDetails
                               {
                                   ReferralId = objref.StudentPersonalId,
                                   Gender = objref.Gender,
                                   BirthDate = SqlFunctions.StringConvert((double)objref.BirthDate.Value.Month).TrimStart() + "/" + SqlFunctions.DateName("day", objref.BirthDate).Trim() + "/" + SqlFunctions.DateName("year", objref.BirthDate),
                                   AdmissionDate = SqlFunctions.StringConvert((double)objref.AdmissionDate.Value.Month).TrimStart() + "/" + SqlFunctions.DateName("day", objref.AdmissionDate).Trim() + "/" + SqlFunctions.DateName("year", objref.AdmissionDate),
                                   ReferralName = (objref.LastName ?? "") + "," + (objref.FirstName ?? ""),
                                   LastQueue = "",
                                   FundingVerification = objref.FundingVerification,
                                   InactiveList = objref.InactiveList,
                                   WaitingList = objref.WaitingList,
                                   StudentType = objref.StudentType,
                               }).ToList();
                    }

                }
                else
                {
                    val = (from objref in objData.StudentPersonals
                           where (objref.StudentType == studentype)
                           select new StudentSearchDetails
                           {
                               ReferralId = objref.StudentPersonalId,
                               Gender = objref.Gender,
                               BirthDate = SqlFunctions.StringConvert((double)objref.BirthDate.Value.Month).TrimStart() + "/" + SqlFunctions.DateName("day", objref.BirthDate).Trim() + "/" + SqlFunctions.DateName("year", objref.BirthDate),
                               AdmissionDate = SqlFunctions.StringConvert((double)objref.AdmissionDate.Value.Month).TrimStart() + "/" + SqlFunctions.DateName("day", objref.AdmissionDate).Trim() + "/" + SqlFunctions.DateName("year", objref.AdmissionDate),
                               ReferralName = (objref.LastName ?? "") + "," + (objref.FirstName ?? ""),
                               LastQueue = "",
                               FundingVerification = objref.FundingVerification,
                               InactiveList = objref.InactiveList,
                               WaitingList = objref.WaitingList,
                               StudentType = objref.StudentType,
                           }).ToList();

                }


                if (val != null)
                {
                    if (val.Count > 0)
                    {
                        foreach (var item in val)
                        {
                            ReferralId = item.ReferralId;
                            var QueueLevel = objData.ref_QueueStatus.Where(x => x.StudentPersonalId == ReferralId && x.CurrentStatus == true).OrderByDescending(x => x.QueueStatusId).FirstOrDefault();
                            if (QueueLevel != null)
                            {

                                item.QueueId = ReferralId.ToString() + "_" + QueueLevel.QueueId.ToString();
                                var QueueName = objData.ref_Queue.Where(x => x.QueueId == QueueLevel.QueueId).ToList();
                                if (QueueName.Count > 0)
                                {
                                    item.LastQueue = QueueName[0].QueueName;
                                }

                            }
                            //var QueueLevel = objData.ref_QueueStatus.Where(x => x.StudentPersonalId == ReferralId && x.CurrentStatus == true).OrderByDescending(x => x.QueueStatusId).FirstOrDefault();

                        }
                    }
                }


            }
            // obj.pageModel.TotalRecordCount = val.Count;
            // val = val.OrderByDescending(objcall => objcall.ReferralId).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            //// obj.= val;
            // if (obj.pageModel.PageSize > obj.pageModel.TotalRecordCount) { obj.pageModel.PageSize = obj.pageModel.TotalRecordCount; }
            // if (obj.pageModel.TotalRecordCount == 0) { obj.pageModel.CurrentPageIndex = 0; }

            // return obj;
            return val;
        }

        public IList<StudentSearchDetails> GetStudentSearch_ref(string SearchName, int page = 1, int pageSize = 10)
        {
            MelmarkDBEntities objData = new MelmarkDBEntities();
            IList<StudentSearchDetails> val = null;
            StudentSearchDetails obj = new StudentSearchDetails();
            //PagingModel Pgmodel = new PagingModel();
            //obj.pageModel.CurrentPageIndex = page;
            //obj.pageModel.PageSize = pageSize;

            session = (clsSession)HttpContext.Current.Session["UserSession"];
            clsReferral objClsRef = new clsReferral();
            string studentype = "Referral";
            if (session != null)
            {
                int ReferralId = 0;

                if (SearchName != "undefined" && SearchName != "" && SearchName != null)
                {
                    string[] args = SearchName.Split('$');
                    if (args.Length > 1)
                    {
                        if (args[1].ToString() == "referral")
                            studentype = "Referral";
                        else
                            studentype = "Client";
                    }
                    else
                        studentype = "Referral";
                    bool isNumber = System.Text.RegularExpressions.Regex.IsMatch(args[0].Trim(), @"^\d+$");

                    if (!isNumber)
                    {
                        string name = args[0].ToString();
                        val = (from objref in objData.StudentPersonals
                               where (objref.StudentType == studentype && (objref.LastName.ToLower().StartsWith(name.ToLower()) || objref.FirstName.ToLower().StartsWith(name.ToLower()) || (objref.LastName.ToLower() + "," + objref.FirstName.ToLower()).Contains(name.ToLower())))
                               select new StudentSearchDetails
                               {
                                   ReferralId = objref.StudentPersonalId,
                                   Gender = objref.Gender,
                                   BirthDate = SqlFunctions.StringConvert((double)objref.BirthDate.Value.Month).TrimStart() + "/" + SqlFunctions.DateName("day", objref.BirthDate).Trim() + "/" + SqlFunctions.DateName("year", objref.BirthDate),
                                   AdmissionDate = SqlFunctions.StringConvert((double)objref.AdmissionDate.Value.Month).TrimStart() + "/" + SqlFunctions.DateName("day", objref.AdmissionDate).Trim() + "/" + SqlFunctions.DateName("year", objref.AdmissionDate),
                                   ReferralName = SqlFunctions.StringConvert((double)objref.StudentPersonalId).Trim() + "|" + (objref.LastName ?? "") + "," + (objref.FirstName ?? ""),
                                   LastQueue = "",
                                   FundingVerification = objref.FundingVerification,
                                   InactiveList = objref.InactiveList,
                                   WaitingList = objref.WaitingList,
                                   StudentType = objref.StudentType,
                                   ReferralName_short = (objref.LastName ?? "") + "," + (objref.FirstName ?? ""),
                               }).ToList();
                    }
                    else
                    {

                        int searchNum = Convert.ToInt32(args[0].Trim());
                        string name = args[0].ToString();
                        val = (from objref in objData.StudentPersonals
                               where (objref.StudentType == studentype && (objref.LastName.ToLower().StartsWith(name.ToLower()) || objref.FirstName.ToLower().StartsWith(name.ToLower()) || (objref.LastName.ToLower() + "," + objref.FirstName.ToLower()).Contains(name.ToLower()) || objref.StudentPersonalId == searchNum))
                               select new StudentSearchDetails
                               {
                                   ReferralId = objref.StudentPersonalId,
                                   Gender = objref.Gender,
                                   BirthDate = SqlFunctions.StringConvert((double)objref.BirthDate.Value.Month).TrimStart() + "/" + SqlFunctions.DateName("day", objref.BirthDate).Trim() + "/" + SqlFunctions.DateName("year", objref.BirthDate),
                                   AdmissionDate = SqlFunctions.StringConvert((double)objref.AdmissionDate.Value.Month).TrimStart() + "/" + SqlFunctions.DateName("day", objref.AdmissionDate).Trim() + "/" + SqlFunctions.DateName("year", objref.AdmissionDate),
                                   ReferralName = SqlFunctions.StringConvert((double)objref.StudentPersonalId).Trim() + "|" + (objref.LastName ?? "") + "," + (objref.FirstName ?? ""),
                                   LastQueue = "",
                                   FundingVerification = objref.FundingVerification,
                                   InactiveList = objref.InactiveList,
                                   WaitingList = objref.WaitingList,
                                   StudentType = objref.StudentType,
                                   ReferralName_short = (objref.LastName ?? "") + "," + (objref.FirstName ?? ""),
                               }).ToList();
                    }

                }
                else
                {
                    val = (from objref in objData.StudentPersonals
                           where (objref.StudentType == studentype)
                           select new StudentSearchDetails
                           {
                               ReferralId = objref.StudentPersonalId,
                               Gender = objref.Gender,
                               BirthDate = SqlFunctions.StringConvert((double)objref.BirthDate.Value.Month).TrimStart() + "/" + SqlFunctions.DateName("day", objref.BirthDate).Trim() + "/" + SqlFunctions.DateName("year", objref.BirthDate),
                               AdmissionDate = SqlFunctions.StringConvert((double)objref.AdmissionDate.Value.Month).TrimStart() + "/" + SqlFunctions.DateName("day", objref.AdmissionDate).Trim() + "/" + SqlFunctions.DateName("year", objref.AdmissionDate),
                               ReferralName = (objref.LastName ?? "") + "," + (objref.FirstName ?? ""),
                               LastQueue = "",
                               FundingVerification = objref.FundingVerification,
                               InactiveList = objref.InactiveList,
                               WaitingList = objref.WaitingList,
                               StudentType = objref.StudentType,
                           }).ToList();

                }


                if (val != null)
                {
                    if (val.Count > 0)
                    {
                        foreach (var item in val)
                        {
                            ReferralId = item.ReferralId;
                            var QueueLevel = objData.ref_QueueStatus.Where(x => x.StudentPersonalId == ReferralId && x.CurrentStatus == true).OrderByDescending(x => x.QueueStatusId).FirstOrDefault();
                            if (QueueLevel != null)
                            {
                                item.QueueId = ReferralId.ToString() + "_" + QueueLevel.QueueId.ToString();
                                var QueueName = objData.ref_Queue.Where(x => x.QueueId == QueueLevel.QueueId).FirstOrDefault();
                                item.LastQueue = QueueName.QueueName;
                            }
                            //var QueueLevel = objData.ref_QueueStatus.Where(x => x.StudentPersonalId == ReferralId && x.CurrentStatus == true).OrderByDescending(x => x.QueueStatusId).FirstOrDefault();

                        }
                    }
                }


            }
            // obj.pageModel.TotalRecordCount = val.Count;
            // val = val.OrderByDescending(objcall => objcall.ReferralId).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            //// obj.= val;
            // if (obj.pageModel.PageSize > obj.pageModel.TotalRecordCount) { obj.pageModel.PageSize = obj.pageModel.TotalRecordCount; }
            // if (obj.pageModel.TotalRecordCount == 0) { obj.pageModel.CurrentPageIndex = 0; }

            // return obj;
            return val;
        }

        public IList<StaffSearchDetails> GetStaffList(string SearchName)
        {
            MelmarkDBEntities objData = new MelmarkDBEntities();
            IList<StaffSearchDetails> val = null;
            session = (clsSession)HttpContext.Current.Session["UserSession"];
            clsReferral objClsRef = new clsReferral();

            if (session != null)
            {

                if (SearchName != "undefined" && SearchName != "" && SearchName != null)
                {
                    val = (from objref in objData.Users
                           where (objref.ActiveInd == "A" && (objref.UserLName.ToLower().StartsWith(SearchName.ToLower()) || objref.UserFName.ToLower().StartsWith(SearchName.ToLower()) || (objref.UserLName.ToLower() + "," + objref.UserFName.ToLower()).Contains(SearchName.ToLower())))
                           select new StaffSearchDetails
                           {
                               UserId = objref.UserId,
                               UserName = (objref.UserLName ?? "") + "," + (objref.UserFName ?? ""),
                           }).ToList();


                }
                else
                {
                    val = (from objref in objData.Users
                           select new StaffSearchDetails
                           {
                               UserId = objref.UserId,
                               UserName = (objref.UserLName ?? "") + "," + (objref.UserFName ?? ""),
                           }).ToList();

                }




            }
            return val;
        }

        public IList<ContactNameSearchDetails> GetContactNameList(string SearchName)
        {
            MelmarkDBEntities objData = new MelmarkDBEntities();
            IList<ContactNameSearchDetails> val = null;
            session = (clsSession)HttpContext.Current.Session["UserSession"];
            session1 = (clsSession1)HttpContext.Current.Session["UserSession1"];

            clsReferral objClsRef = new clsReferral();

            if (session != null)
            {

                if (SearchName != "undefined" && SearchName != "" && SearchName != null)
                {
                    val = (from objref in objData.ContactPersonals
                           where (objref.StudentPersonalId == session.ReferralId)
                           select new ContactNameSearchDetails
                           {
                               ContactId = objref.ContactPersonalId,
                               ContactName = (objref.LastName ?? "") + "," + (objref.FirstName ?? "") + "," + (objref.MiddleName ?? ""),

                           }).ToList();


                }
                else
                {
                    val = (from objref in objData.ContactPersonals
                           where (objref.StudentPersonalId == session.ReferralId)
                           select new ContactNameSearchDetails
                           {
                               ContactId = objref.ContactPersonalId,
                               ContactName = (objref.LastName ?? "") + "," + (objref.FirstName ?? "") + "," + (objref.MiddleName ?? ""),
                           }).ToList();

                }


                val.Add(new ContactNameSearchDetails { ContactId = 0, ContactName = "Others" });


            }
            return val;
        }


        public StdDetailsViewModel getStdDetails()
        {
            MelmarkDBEntities objData = new MelmarkDBEntities();
            StdDetailsViewModel val = new StdDetailsViewModel();
            clsReferral objClsRef = new clsReferral();
            List<ReferralQueueStatus> RefQueueStat = new List<ReferralQueueStatus>();
            session = (clsSession)HttpContext.Current.Session["UserSession"];
            AddressList addr = new AddressList();
            StudentAddresRel adrRel = new StudentAddresRel();
            ref_Queue refQueue = new ref_Queue();
            ref_QueueStatus refQueueStatus = new ref_QueueStatus();
            ref_ReviewComments rvw = new ref_ReviewComments();
            ref_LetterTrayValues ltrEngine = new ref_LetterTrayValues();
            ref_LetterTrayValues ltrValues = new ref_LetterTrayValues();
            if (session != null)
            {
                if (session.ReferralId > 0)
                {

                    var ClientDetails = objData.StudentPersonals.Where(x => x.StudentPersonalId == session.ReferralId).SingleOrDefault();
                    adrRel = objData.StudentAddresRels.Where(x => x.StudentPersonalId == session.ReferralId && x.ContactSequence == 0).SingleOrDefault();
                    if (adrRel != null)
                    {
                    addr = objData.AddressLists.Where(x => x.AddressId == adrRel.AddressId).SingleOrDefault();
                    }
                    refQueue = objData.ref_Queue.Where(x => x.QueueType == "NA").SingleOrDefault();
                    var allQueue = objData.ref_Queue.OrderBy(x => x.QueueId).ToList();
                    int qid = 0;
                    int qstatusid = 0;
                    foreach (var qitem in allQueue)
                    {
                        if (qitem.MasterId > 1)
                        {
                            var QueueStatus = objData.ref_QueueStatus.Where(x => x.StudentPersonalId == session.ReferralId && x.QueueId == qitem.QueueId).ToList();
                            if (QueueStatus.Count > 0)
                            {
                                foreach (var item in QueueStatus)
                                {
                                    if (item.Draft.ToString() == "N")
                                    {
                                        qid = item.QueueId;
                                        qstatusid = item.QueueStatusId;
                                    }
                                    else
                                    {
                                        goto Outer;
                                    }
                                }
                            }
                            else
                            {

                                break;
                            }
                        }
                    }
                Outer:
                    string Queue_Name = "";
                    if (qstatusid > 0)
                    {
                        ref_QueueStatus qstatus = objData.ref_QueueStatus.Where(x => x.QueueStatusId == qstatusid && x.StudentPersonalId == session.ReferralId).SingleOrDefault();
                        qstatus.CurrentStatus = true;
                        objData.SaveChanges();
                    }
                    if (qid == 0)
                    {
                        Queue_Name = objData.ref_Queue.Where(x => x.QueueType == "NA").SingleOrDefault().QueueName.ToString();
                    }
                    else
                    {
                        Queue_Name = objData.ref_Queue.Where(x => x.QueueId == qid).SingleOrDefault().QueueName.ToString();
                    }
                    string Diagnoses = "";
                    var DiagnosesList = objData.DiaganosesPAs.Where(x => x.StudentPersonalId == session.ReferralId).ToList();
                    if (DiagnosesList.Count > 0)
                        Diagnoses = DiagnosesList[0].Diaganoses;
                    //  ltrEngine = objData.ref_LetterTrayValues.Where(x => x.QueueId == refQueue.QueueId).Single();
                    // ltrEngine = objData.ref_LetterTrayValues.OrderByDescending(x => x.CreatedOn).Where(x => x.QueueId == refQueue.QueueId).First();
                    try
                    {
                        ltrValues = objData.ref_LetterTrayValues.OrderByDescending(x => x.CreatedOn).Where(x => x.QueueId == refQueue.QueueId && x.StudentPersonalId == session.ReferralId).First();
                    }
                    catch
                    {
                        ltrValues = null;
                    }


                    string LetterDateString = "";
                    DateTime? LetterDate = null;
                    if (ltrValues != null && ltrValues.CreatedOn != null)
                    {
                        LetterDate = ltrValues.CreatedOn;
                        LetterDateString = ltrValues.CreatedOn.ToString("MM'/'dd'/'yyyy");

                    }

                    if (session.ReferralId > 0)
                    {

                        val = (from objref in objData.SelectStudentPersonalDetails(session.ReferralId)
                               select new StdDetailsViewModel
                               {
                                   StudentPersonalId = objref.StudentPersonalId,
                                   Gender = objref.Gender,
                                   studentPersonalName = objref.studentPersonalName,
                                   ImageUrl = objClsRef.GetStudentImage(objref.StudentPersonalId),
                                   BirthDate = objref.BirthDate.ToString(),
                                   PlaceOfBirth = objref.PlaceOfBirth,
                                   PrimaryLanguage = objref.PrimaryLanguage,
                                   Height = objref.Height.ToString(),
                                   Weight = objref.Weight.ToString(),
                                   AdmissionDate = objref.AdmissionDate.ToString(),
                                   Address = addr.ApartmentType + "," + addr.StreetName + "," + addr.City,
                                   Street = addr.StreetName,
                                   Firstname = ClientDetails.FirstName,
                                   Lastname = ClientDetails.LastName,
                                   Apartment = addr.ApartmentType,
                                   City = addr.City,
                                   ZipCode = addr.PostalCode,
                                   LetterDate = LetterDate,
                                   LetterDateString = LetterDateString,
                                   State = addr.StateProvince,
                                   GenderNum = objref.Gender,
                                   Status = Queue_Name,
                                   Diagnosis = Diagnoses,
                                   ApplicationDate = ClientDetails.CreatedOn.ToString(),
                                   fl_AT = ClientDetails.InactiveList,
                                   fl_FA = ClientDetails.FundingVerification,
                                   fl_WL = ClientDetails.WaitingList,
                                   FundingSourceId = ClientDetails.FundingSource //--- 22Sep2020 - List 3 - Task #2 ---//
                               }).Single();

                    }

                    if (val != null)
                    {
                        if (val.Gender == "0")
                        {
                            val.Gender = "";
                        }
                        else if (val.Gender == "1")
                        {
                            val.Gender = "Male";
                        }
                        else if (val.Gender == "2")
                        {
                            val.Gender = "Female";
                        }

                        if (val.ImageUrl == null || val.ImageUrl == "")
                        {
                            val.ImageUrl = "/Images/Male.png";
                        }
                        //if (ClientDetails.StudentType != "Client")
                        //{
                        var Queuedata = from objqueue in objData.ref_Queue
                                        where objqueue.MasterId != 0
                                        orderby objqueue.MasterId, objqueue.SortOrder
                                        select objqueue;
                        val.QueueList = (from objquedata in Queuedata
                                         join objqueue in objData.ref_Queue on objquedata.MasterId equals objqueue.QueueId
                                         select new QueueStatus
                                         {
                                             QueueName = objquedata.QueueName,
                                             QueueId = objquedata.QueueId,
                                             QueueType = objqueue.QueueType
                                         }).ToList();
                        //= (from objqueue in objData.ref_Queue
                        //                where  objqueue.MasterId!=0
                        //                orderby objqueue.MasterId, objqueue.SortOrder
                        //                select new QueueStatus
                        //                {
                        //                    QueueName = objqueue.QueueName,
                        //                    QueueId = objqueue.QueueId,
                        //                    QueueType=objqueue.QueueType
                        //                }).ToList();
                        //val.ReferralQueueList


                        var PCMid = (from objchk in objData.ref_Queue where objchk.QueueName == "Placement/Consent Meeting" && objchk.QueueType == "PCM" select new { Queid = objchk.QueueId }).FirstOrDefault();

                        //13-Oct-2020 List 3 #24 Start---
                        for (var ij = 0; ij < val.QueueList.Count; ij++)
                        {
                            var queue = val.QueueList[ij];

                            if (PCMid.Queid > 0 && queue.QueueId == PCMid.Queid)
                            {
                                continue;
                            }

                            //if (queue.QueueId == 6 || queue.QueueId == 8 || queue.QueueId == 20 || queue.QueueId == 21 || queue.QueueId == 22 || queue.QueueId == 23 || queue.QueueId == 24) // Commented For Not Showing Placement Meeting (22) and Consent Meeting (23)
                            if (queue.QueueId == 6 || queue.QueueId == 8 || queue.QueueId == 20 || queue.QueueId == 21 || queue.QueueId == 22 || queue.QueueId == 23 || queue.QueueId == 24)
                                continue;
                            var ContentsList = (from objchk in objData.ref_Checklist where objchk.QueueId == queue.QueueId && objchk.ChecklistHeaderId != 55 select new { CheckListId = objchk.ChecklistId }).ToList();
                            if (ContentsList.Count == 0)
                            {
                                val.QueueList.Remove(queue);
                                ij = ij != 0 ? --ij : 0;
                            }
                        }
                        //END----
                        foreach (var queue in val.QueueList)
                        {
                            string Status = "Not Started";
                            string StatusDate = "";

                            //var queueProcess = objData.ref_QueueStatus.Where(objqstatus => objqstatus.StudentPersonalId == session.ReferralId && objqstatus.CurrentStatus == true).SingleOrDefault();
                            //var QProcess = objData.ref_QueueStatus.Where(objqstatus => objqstatus.StudentPersonalId == session.ReferralId && objqstatus.QueueProcess == queueProcess.QueueProcess).ToList();
                            //foreach (var item in QProcess)
                            //{
                            //    if (queue.QueueId == item.QueueId)
                            //    {
                            //        if (item.Draft == "Y")
                            //        {
                            //            Status = "Drafted";
                            //        }
                            //        else
                            //        {
                            //            Status = "Submitted";
                            //        }
                            //        StatusDate = Convert.ToString(item.CreatedOn);
                            //    }
                            //}
                            if (ClientDetails != null && ClientDetails.StudentType != "Client")
                            {
                                var queueProcess = objData.ref_QueueStatus.Where(objqstatus => objqstatus.StudentPersonalId == session.ReferralId && objqstatus.CurrentStatus == true).OrderByDescending(obj => obj.QueueStatusId).FirstOrDefault();
                                if (queueProcess != null)
                                {
                                    var QProcess = objData.ref_QueueStatus.Where(objqstatus => objqstatus.StudentPersonalId == session.ReferralId && objqstatus.QueueProcess == queueProcess.QueueProcess).ToList();
                                    foreach (var item in QProcess)
                                    {
                                        if (queue.QueueId == item.QueueId && queue.QueueId != PCMid.Queid)
                                        {
                                            if (item.Draft == "Y")
                                            {
                                                Status = "Drafted";
                                            }
                                            else
                                            {
                                                Status = "Submitted";
                                            }
                                            StatusDate = Convert.ToString(item.CreatedOn);
                                        }
                                        else if (queue.QueueId == PCMid.Queid)
                                        {
                                            var getPMstat = (from objchk in objData.ref_QueueStatus where objchk.QueueId == 22 && objchk.StudentPersonalId == session.ReferralId select new { DraftStat = objchk.Draft }).FirstOrDefault();
                                            var getCMstat = (from objchk in objData.ref_QueueStatus where objchk.QueueId == 23 && objchk.StudentPersonalId == session.ReferralId select new { DraftStat = objchk.Draft }).FirstOrDefault();
                                            if (getPMstat != null && getCMstat != null)
                                            {
                                                if (getPMstat.DraftStat == "N" && getCMstat.DraftStat == "N")
                                                {
                                                    Status = "Submitted";
                                                }
                                                StatusDate = Convert.ToString(item.CreatedOn);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                int qId = getQueueId("CL");
                                var queueProcess = objData.ref_QueueStatus.Where(objqstatus => objqstatus.StudentPersonalId == session.ReferralId && objqstatus.QueueId == qId).ToList();
                                if (queueProcess.Count > 0)
                                {
                                    if (queueProcess != null)
                                    {
                                        int qprocess = Convert.ToInt32(queueProcess[0].QueueProcess);
                                        var QProcess = objData.ref_QueueStatus.Where(objqstatus => objqstatus.StudentPersonalId == session.ReferralId && objqstatus.QueueProcess == qprocess).ToList();
                                        foreach (var item in QProcess)
                                        {
                                            if (queue.QueueId == item.QueueId)
                                            {
                                                if (item.Draft == "Y")
                                                {
                                                    Status = "Drafted";
                                                }
                                                else
                                                {
                                                    Status = "Submitted";
                                                }
                                                StatusDate = Convert.ToString(item.CreatedOn);
                                            }
                                        }
                                    }
                                }
                            }
                            RefQueueStat.Add(new ReferralQueueStatus { QueueID = queue.QueueId, QueueStatus = Status, QueueDate = StatusDate, ReferralId = session.ReferralId });
                        }

                        var PCMid2 = (from objchk in objData.ref_Queue where objchk.QueueName == "Placement/Consent Meeting" && objchk.QueueType == "PCM" select new { Queid = objchk.QueueId }).FirstOrDefault();

                        //13-Oct-2020 List 3 #24 Start--- 
                        val.ReferralQueueList = RefQueueStat;
                        for (var ij = 0; ij < val.ReferralQueueList.Count; ij++)
                        {
                            var queue = val.ReferralQueueList[ij];

                            if (PCMid2.Queid > 0 && queue.QueueID == PCMid2.Queid)
                            {
                                continue;
                            }

                            //if (queue.QueueID == 6 || queue.QueueID == 8 || queue.QueueID == 20 || queue.QueueID == 21 || queue.QueueID == 22 || queue.QueueID == 23 || queue.QueueID == 24) // Commented For Not Showing Placement Meeting (22) and Consent Meeting (23)
                            if (queue.QueueID == 6 || queue.QueueID == 8 || queue.QueueID == 20 || queue.QueueID == 21 || queue.QueueID == 24)
                                continue;
                            var ContentsList = (from objchk in objData.ref_Checklist where objchk.QueueId == queue.QueueID && objchk.ChecklistHeaderId != 55 select new { CheckListId = objchk.ChecklistId }).ToList();
                            if (ContentsList.Count == 0)
                            {
                                val.ReferralQueueList.Remove(queue);
                                ij = ij != 0 ? --ij : 0;
                            }
                        }
                     //END----
                        try
                        {
                            int ReferralId = session.ReferralId;
                            var QueueLevel = objData.ref_QueueStatus.Where(x => x.StudentPersonalId == ReferralId && x.CurrentStatus == true).OrderByDescending(x => x.QueueStatusId).FirstOrDefault();
                            if (QueueLevel != null)
                            {

                                var QueueName = objData.ref_Queue.Where(x => x.QueueId == QueueLevel.QueueId).FirstOrDefault();
                                val.currentQueue = QueueName.QueueName;
                                refQueue = objData.ref_Queue.Where(x => x.QueueType == "FV").FirstOrDefault();
                                //  var result = table.OrderByDescending(x => x.Status).First();
                                var maxQueProcess = objData.ref_QueueStatus.Where(x => x.StudentPersonalId == ReferralId).OrderByDescending(x => x.QueueProcess).Select(x => x.QueueProcess).First();
                                var status = objData.ref_QueueStatus.Where(x => x.StudentPersonalId == ReferralId & x.QueueProcess == maxQueProcess).OrderByDescending(x => x.QueueProcess).ToList();
                                foreach (var item in status)
                                {
                                    if (item.QueueId == refQueue.QueueId)
                                    {
                                        int id = Convert.ToInt32(item.QueueStatusId);
                                        var srvw = objData.ref_ReviewComments.Where(x => x.StudentId == ReferralId && x.QueueStatusId == id & x.Draft == "N").ToList();
                                        if (srvw != null && srvw.Count > 0)
                                            val.FVqueueStatus = Convert.ToBoolean(srvw[0].ApproveInd);
                                        else
                                            val.FVqueueStatus = false;
                                    }

                                }
                            }

                        }
                        catch
                        {

                            val.FVqueueStatus = false;

                        }
                    }

                }
            }
            return val;
        }

        public StdDetailsQuickUpdateViewModel getStdDetails_quickUpdate()
        {
            MelmarkDBEntities objData = new MelmarkDBEntities();
            StdDetailsQuickUpdateViewModel val = new StdDetailsQuickUpdateViewModel();
            clsReferral objClsRef = new clsReferral();
            List<ReferralQueueStatus_QuickUpdate> RefQueueStat = new List<ReferralQueueStatus_QuickUpdate>();
            session = (clsSession)HttpContext.Current.Session["UserSession"];
            AddressList addr = new AddressList();
            StudentAddresRel adrRel = new StudentAddresRel();
            ref_Queue refQueue = new ref_Queue();
            ref_QueueStatus refQueueStatus = new ref_QueueStatus();
            ref_ReviewComments rvw = new ref_ReviewComments();
            ref_LetterTrayValues ltrEngine = new ref_LetterTrayValues();
            ref_LetterTrayValues ltrValues = new ref_LetterTrayValues();
            if (session != null)
            {

                var ClientDetails = objData.StudentPersonals.Where(x => x.StudentPersonalId == session.ReferralId).SingleOrDefault();
                adrRel = objData.StudentAddresRels.Where(x => x.StudentPersonalId == session.ReferralId && x.ContactSequence == 0).SingleOrDefault();
                addr = objData.AddressLists.Where(x => x.AddressId == adrRel.AddressId).SingleOrDefault();
                refQueue = objData.ref_Queue.Where(x => x.QueueType == "NA").SingleOrDefault();
                //  ltrEngine = objData.ref_LetterTrayValues.Where(x => x.QueueId == refQueue.QueueId).Single();
                // ltrEngine = objData.ref_LetterTrayValues.OrderByDescending(x => x.CreatedOn).Where(x => x.QueueId == refQueue.QueueId).First();
                try
                {
                    ltrValues = objData.ref_LetterTrayValues.OrderByDescending(x => x.CreatedOn).Where(x => x.QueueId == refQueue.QueueId && x.StudentPersonalId == session.ReferralId).First();
                }
                catch
                {
                    ltrValues = null;
                }


                string LetterDateString = "";
                DateTime? LetterDate = null;
                if (ltrValues != null && ltrValues.CreatedOn != null)
                {
                    LetterDate = ltrValues.CreatedOn;
                    LetterDateString = ltrValues.CreatedOn.ToString("MM'/'dd'/'yyyy");

                }

                if (session.ReferralId > 0)
                {

                    val = (from objref in objData.SelectStudentPersonalDetails(session.ReferralId)
                           select new StdDetailsQuickUpdateViewModel
                           {
                               StudentPersonalId = objref.StudentPersonalId,
                               Gender = objref.Gender,
                               studentPersonalName = objref.studentPersonalName,
                               ImageUrl = objClsRef.GetStudentImage(objref.StudentPersonalId),
                               BirthDate = objref.BirthDate,
                               PlaceOfBirth = objref.PlaceOfBirth,
                               PrimaryLanguage = objref.PrimaryLanguage,
                               Height = objref.Height.ToString(),
                               Weight = objref.Weight.ToString(),
                               ApplicationDate = objref.AdmissionDate,
                               AddressAppartment = addr.ApartmentType,
                               AddressStreet = addr.StreetName,
                               AddressCity = addr.City,
                               LetterDate = LetterDate,
                               LetterDateString = LetterDateString
                           }).Single();

                }

                if (val != null)
                {
                    if (val.Gender == "0")
                    {
                        val.Gender = "";
                    }
                    else if (val.Gender == "1")
                    {
                        val.Gender = "Male";
                    }
                    else if (val.Gender == "2")
                    {
                        val.Gender = "Female";
                    }

                    if (val.ImageUrl == null || val.ImageUrl == "")
                    {
                        val.ImageUrl = "/Images/Male.png";
                    }

                    var Queuedata = from objqueue in objData.ref_Queue
                                    where objqueue.MasterId != 0
                                    orderby objqueue.MasterId, objqueue.SortOrder
                                    select objqueue;
                    val.QueueList = (from objquedata in Queuedata
                                     join objqueue in objData.ref_Queue on objquedata.MasterId equals objqueue.QueueId
                                     select new QueueStatus
                                     {
                                         QueueName = objquedata.QueueName,
                                         QueueId = objquedata.QueueId,
                                         QueueType = objqueue.QueueType
                                     }).ToList();

                    foreach (var queue in val.QueueList)
                    {
                        string Status = "Not Started";
                        string StatusDate = "";


                        if (ClientDetails != null && ClientDetails.StudentType != "Client")
                        {
                            var queueProcess = objData.ref_QueueStatus.Where(objqstatus => objqstatus.StudentPersonalId == session.ReferralId && objqstatus.CurrentStatus == true).OrderByDescending(obj => obj.QueueStatusId).FirstOrDefault();
                            if (queueProcess != null)
                            {
                                var QProcess = objData.ref_QueueStatus.Where(objqstatus => objqstatus.StudentPersonalId == session.ReferralId && objqstatus.QueueProcess == queueProcess.QueueProcess).ToList();
                                foreach (var item in QProcess)
                                {
                                    if (queue.QueueId == item.QueueId)
                                    {
                                        if (item.Draft == "Y")
                                        {
                                            Status = "Drafted";
                                        }
                                        else
                                        {
                                            Status = "Submitted";
                                        }
                                        StatusDate = Convert.ToString(item.CreatedOn);
                                    }
                                }
                            }
                        }
                        else
                        {
                            int qId = getQueueId("CL");
                            var queueProcess = objData.ref_QueueStatus.Where(objqstatus => objqstatus.StudentPersonalId == session.ReferralId && objqstatus.QueueId == qId).SingleOrDefault();
                            if (queueProcess != null)
                            {
                                var QProcess = objData.ref_QueueStatus.Where(objqstatus => objqstatus.StudentPersonalId == session.ReferralId && objqstatus.QueueProcess == queueProcess.QueueProcess).ToList();
                                foreach (var item in QProcess)
                                {
                                    if (queue.QueueId == item.QueueId)
                                    {
                                        if (item.Draft == "Y")
                                        {
                                            Status = "Drafted";
                                        }
                                        else
                                        {
                                            Status = "Submitted";
                                        }
                                        StatusDate = Convert.ToString(item.CreatedOn);
                                    }
                                }
                            }
                        }
                        RefQueueStat.Add(new ReferralQueueStatus_QuickUpdate { QueueID = queue.QueueId, QueueStatus = Status, QueueDate = StatusDate, ReferralId = session.ReferralId });
                    }
                    val.ReferralQueueList = RefQueueStat;

                    try
                    {
                        int ReferralId = session.ReferralId;
                        var QueueLevel = objData.ref_QueueStatus.Where(x => x.StudentPersonalId == ReferralId && x.CurrentStatus == true).OrderByDescending(x => x.QueueStatusId).FirstOrDefault();
                        if (QueueLevel != null)
                        {

                            var QueueName = objData.ref_Queue.Where(x => x.QueueId == QueueLevel.QueueId).FirstOrDefault();
                            val.currentQueue = QueueName.QueueName;
                            refQueue = objData.ref_Queue.Where(x => x.QueueType == "FV").FirstOrDefault();
                            //  var result = table.OrderByDescending(x => x.Status).First();
                            var maxQueProcess = objData.ref_QueueStatus.Where(x => x.StudentPersonalId == ReferralId).OrderByDescending(x => x.QueueProcess).Select(x => x.QueueProcess).First();
                            var status = objData.ref_QueueStatus.Where(x => x.StudentPersonalId == ReferralId & x.QueueProcess == maxQueProcess).OrderByDescending(x => x.QueueProcess).ToList();
                            foreach (var item in status)
                            {
                                if (item.QueueId == refQueue.QueueId)
                                {
                                    int id = Convert.ToInt32(item.QueueStatusId);
                                    var srvw = objData.ref_ReviewComments.Where(x => x.StudentId == ReferralId && x.QueueStatusId == id & x.Draft == "N").ToList();
                                    if (srvw != null && srvw.Count > 0)
                                        val.FVqueueStatus = Convert.ToBoolean(srvw[0].ApproveInd);
                                    else
                                        val.FVqueueStatus = false;
                                }

                            }
                        }

                    }
                    catch
                    {

                        val.FVqueueStatus = false;

                    }
                }

            }
            return val;
        }

        //public StdDetailsViewModel GetQueue()
        //{
        //    MelmarkDBEntities objData = new MelmarkDBEntities();
        //    StdDetailsViewModel val = new StdDetailsViewModel();
        //    session = (clsSession)HttpContext.Current.Session["UserSession"];
        //    if (session != null)
        //    {

        //        if (session.ReferralId > 0)
        //        {
        //        }
        //    }
        //}

        public bool checkIfSubmitted(int ReferralId, string querytype)
        {
            QstatusDetails qDetails = getQueueStatusId(ReferralId, querytype);
            int QstatusId = qDetails.QueueStatusId;
            if (QstatusId == 0)
            {
                QstatusId = getQueueStatusIdIfSubmitted(ReferralId, querytype);
                if (QstatusId > 0)
                {
                    return true;
                }
                else
                    return false;

            }
            return false;

        }

        public string SavePlacementData(AddMatchOpeningViewModel model)
        {
            session = (clsSession)HttpContext.Current.Session["UserSession"];
            MelmarkDBEntities objData = new MelmarkDBEntities();
            int ClientID = session.ReferralId, SchoolId = session.SchoolId;

            Placement placement = new Placement();
            if (model.Id > 0)
            {
                try
                {

                    //--- 02Oct2020 - List 3 - Task #2 -(Start)--//
                    StudentPersonal stdprs = new StudentPersonal();
                    stdprs = objData.StudentPersonals.Where(obj => obj.StudentPersonalId == session.ReferralId).SingleOrDefault();
                    if (stdprs != null)
                    {
                        stdprs.FundingSource = model.FundingSourceId;
                        objData.SaveChanges();
                    }
                    else
                    {
                        StudentPersonal stdprs2 = new StudentPersonal();
                        stdprs2.FundingSource = model.FundingSourceId;
                        objData.StudentPersonals.Add(stdprs2);
                        objData.SaveChanges();
                    }
                    //--- 02Oct2020 - List 3 - Task #2 -(End)--//

                    placement = objData.Placements.Where(objPlacement => objPlacement.PlacementId == model.Id && objPlacement.StudentPersonalId == ClientID).SingleOrDefault();
                    placement.PlacementType = model.PlacementType;
                    placement.BehaviorAnalyst = model.BehaviorAnalyst;
                    placement.UnitClerk = model.UnitClerk;
                    placement.PrimaryNurse = model.PrimaryNurse;
                    placement.Department = model.Department;
                    placement.Status = 1;
                    placement.StartDate = DateTime.ParseExact(model.StartDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    if (model.EndDateDate != null)
                        placement.EndDate = DateTime.ParseExact(model.EndDateDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    else
                        placement.EndDate = null;
                    placement.ModifiedBy = 1;
                    placement.ModifiedOn = DateTime.Now;

                    placement.Reason = model.Reason;
                    placement.AssociatedPersonnel = model.AssociatedPersonnel;
                    placement.Location = model.LocationId;
                    placement.PlacementDepartment = model.PlacementDepartmentId;
                    placement.PlacementReason = model.PlacementReason;

                    placement.IsMonday = model.IsMonday;
                    placement.IsTuesday = model.IsTuesday;
                    placement.IsWednesday = model.IsWednesday;
                    placement.IsThursday = model.IsThursday;
                    placement.IsFriday = model.IsFriday;
                    placement.IsSaturday = model.IsSaturday;
                    placement.IsSunday = model.IsSunday;

                    placement.MondayNote = model.MondayNote;
                    placement.TuesdayNote = model.TuesdayNote;
                    placement.WednesdayNote = model.WednesdayNote;
                    placement.ThursdayNote = model.ThursdayNote;
                    placement.FridayNote = model.FridayNote;
                    placement.SaturdayNote = model.SaturdayNote;
                    placement.SundayNote = model.SundayNote;




                    RemoveFromClass(ClientID, Convert.ToInt32(model.LocationId), model.Id);

                    objData.SaveChanges();

                    string PlacementName = "";
                    if (placement.Department != null && placement.Department > 0)
                    {
                        var Plcname = objData.LookUps.Where(x => x.LookupId == placement.Department).ToList();
                        if (Plcname.Count > 0) PlacementName = Plcname[0].LookupName;
                    }
                    if (model.EndDateDate != null && model.EndDateDate != "")
                    {
                        AddEventModel.CreateSystemEvent("Placement " + PlacementName + " Discharged on :" + model.EndDateDate, "Discharged", model.placemntLogText);
                    }
                    else
                    {
                        AddEventModel.CreateSystemEvent("Placement  " + PlacementName + " Changed", "Moved", model.placemntLogText);
                    }
                    //DisplayStatus();
                    return "Sucess";
                }
                catch
                {
                    return "Failed";
                }
            }
            else
            {
                if (ClientID == 0)
                {
                    return "No Client Selected";
                }
                else
                {
                    try
                    {
                        //--- 02Oct2020 - List 3 - Task #2 -(Start)--//
                        StudentPersonal stdprs = new StudentPersonal();
                        stdprs = objData.StudentPersonals.Where(obj => obj.StudentPersonalId == session.ReferralId).SingleOrDefault();
                        if (stdprs != null)
                        {
                            stdprs.FundingSource = model.FundingSourceId;
                            objData.SaveChanges();
                        }
                        else
                        {
                            StudentPersonal stdprs2 = new StudentPersonal();
                            stdprs2.FundingSource = model.FundingSourceId;
                            objData.StudentPersonals.Add(stdprs2);
                            objData.SaveChanges();
                        }
                        //--- 02Oct2020 - List 3 - Task #2 -(End)--//

                        placement.SchoolId = SchoolId;
                        placement.PlacementType = model.PlacementType;
                        placement.BehaviorAnalyst = model.BehaviorAnalyst;
                        placement.UnitClerk = model.UnitClerk;
                        placement.PrimaryNurse = model.PrimaryNurse;
                        placement.Department = model.Department;
                        placement.Status = 1;
                        placement.StudentPersonalId = ClientID;
                        if (model.StartDate != null)
                            placement.StartDate = DateTime.ParseExact(model.StartDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        if (model.EndDateDate != null)
                            placement.EndDate = DateTime.ParseExact(model.EndDateDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        placement.CreatedBy = 1;
                        placement.CreatedOn = DateTime.Now;

                        placement.Reason = model.Reason;
                        placement.AssociatedPersonnel = model.AssociatedPersonnel;
                        placement.Location = model.LocationId;
                        placement.PlacementDepartment = model.PlacementDepartmentId;
                        placement.PlacementReason = model.PlacementReason;

                        placement.IsMonday = model.IsMonday;
                        placement.IsTuesday = model.IsTuesday;
                        placement.IsWednesday = model.IsWednesday;
                        placement.IsThursday = model.IsThursday;
                        placement.IsFriday = model.IsFriday;
                        placement.IsSaturday = model.IsSaturday;
                        placement.IsSunday = model.IsSunday;

                        placement.MondayNote = model.MondayNote;
                        placement.TuesdayNote = model.TuesdayNote;
                        placement.WednesdayNote = model.WednesdayNote;
                        placement.ThursdayNote = model.ThursdayNote;
                        placement.FridayNote = model.FridayNote;
                        placement.SaturdayNote = model.SaturdayNote;
                        placement.SundayNote = model.SundayNote;

                        objData.Placements.Add(placement);
                        objData.SaveChanges();



                        AssignToClass(ClientID, Convert.ToInt32(model.LocationId));


                        string PlacementName = "";
                        if (placement.Department != null && placement.Department > 0)
                        {
                            var Plcname = objData.LookUps.Where(x => x.LookupId == placement.Department).ToList();
                            if (Plcname.Count > 0) PlacementName = Plcname[0].LookupName;
                        }
                        //DisplayStatus();
                        AddEventModel.CreateSystemEvent("Placement " + PlacementName + " Inserted", "Admitted", "Placement " + PlacementName + " Admitted");
                        return "Sucess";
                    }
                    catch
                    {
                        return "Failed";
                    }
                }
            }



        }
        public int RemoveFromClass(int StudentId, int ClassId, int placementId)
        {
            MelmarkDBEntities objData = new MelmarkDBEntities();
            IList<ContactNameSearchDetails> val = null;
            IList<ReferalDB.Models.PlacementModel.GridListPlacement> retunmodel = new List<ReferalDB.Models.PlacementModel.GridListPlacement>();
            session = (clsSession)HttpContext.Current.Session["UserSession"];

            var currLocationId = objData.Placements.Where(x => x.PlacementId == placementId).ToList();

            int? currLocation = currLocationId[0].Location;
            if (currLocation != ClassId)
            {
                try
                {
                    retunmodel = (from objPlacement in objData.Placements
                                  join objLookUp in objData.LookUps on objPlacement.PlacementType equals objLookUp.LookupId
                                  join objLkUp in objData.LookUps on objPlacement.Department equals objLkUp.LookupId
                                  where (objPlacement.StudentPersonalId == StudentId && objPlacement.Status == 1 && objPlacement.Location == currLocation && objPlacement.PlacementId != placementId)
                                  select new ReferalDB.Models.PlacementModel.GridListPlacement
                                  {
                                      PlacementId = objPlacement.PlacementId,
                                      PlacementName = objLookUp.LookupName,
                                      Program = objLkUp.LookupName,
                                      StartDate = objPlacement.StartDate,
                                      EndDate = objPlacement.EndDate,


                                  }).ToList();
                }
                catch
                {

                }

                if (retunmodel.Count > 0)
                {
                    return 0;
                }
                else
                {
                    StdtClass stdtc = new StdtClass();
                    var result = objData.StdtClasses.Where(x => x.StdtId == StudentId && x.ClassId == currLocation && x.ActiveInd == "A").ToList();

                    if (result.Count > 0)
                    {
                        result[0].ActiveInd = "D";
                        objData.SaveChanges();
                    }
                    AssignToClass(StudentId, ClassId);

                    return 1;
                }
            }
            return 0;
        }

        public int AssignToClass(int StudentId, int ClassId)
        {
            MelmarkDBEntities objData = new MelmarkDBEntities();
            IList<ContactNameSearchDetails> val = null;
            IList<ReferalDB.Models.PlacementModel.GridListPlacement> retunmodel = new List<ReferalDB.Models.PlacementModel.GridListPlacement>();
            session = (clsSession)HttpContext.Current.Session["UserSession"];

            var result = objData.StdtClasses.Where(x => x.StdtId == StudentId && x.ClassId == ClassId && x.ActiveInd == "A").ToList();

            if (result.Count() > 0)
            {
                return 0;
            }
            else
            {
                StdtClass stdc = new StdtClass();

                try
                {
                    stdc.StdtId = StudentId;
                    stdc.ClassId = ClassId;
                    stdc.ActiveInd = "A";
                    stdc.PrimaryInd = "A";
                    stdc.SchoolId = session.SchoolId;
                    stdc.CreatedBy = session.LoginId.ToString();
                    stdc.CreatedOn = DateTime.Now;

                    objData.StdtClasses.Add(stdc);
                    objData.SaveChanges();
                }
                catch
                {

                }

                return 1;
            }
        }

        public string SaveEventData(AddEventModel model, bool IsSysEvent = false)
        {
            session = (clsSession)HttpContext.Current.Session["UserSession"];
            MelmarkDBEntities dbobj = new MelmarkDBEntities();
            int ClientID = session.ReferralId, SchoolId = session.SchoolId;
            Event events = new Event();
            if (model.Id > 0)
            {
                try
                {
                    events = dbobj.Events.Where(objEvents => objEvents.EventId == model.Id && objEvents.StudentPersonalId == ClientID).SingleOrDefault();
                    events.EventsName = model.EventName;
                    events.EventType = Convert.ToInt32(model.EventTypes);
                    events.EventStatus = model.EventStatus;
                    events.Status = 1;
                    //events.EventDate = DateTime.Now;
                    events.ExpiredOn = DateTime.ParseExact(model.ExpiredOnDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    events.ModifiedBy = 1;
                    events.ModifiedOn = DateTime.Now;

                    events.Contact = model.Contact;
                    events.EventDate = (model.EventDate != null && model.EventDate != "") ?
                        DateTime.ParseExact(model.EventDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture) : DateTime.Now;
                    events.Username = model.UserName;
                    events.Note = model.Note;
                    events.IsSystemEvent = IsSysEvent;

                    dbobj.SaveChanges();
                    return "Sucess";
                }
                catch
                {
                    return "Failed";
                }
            }
            else
            {
                if (ClientID == 0)
                {
                    return "No Client Selected";
                }
                else
                {
                    try
                    {
                        events.SchoolId = SchoolId;
                        events.EventsName = model.EventName;
                        events.EventType = Convert.ToInt32(model.EventTypes);
                        events.EventStatus = model.EventStatus;
                        events.Status = 1;
                        events.StudentPersonalId = ClientID;
                        //events.EventDate = DateTime.Now;
                        if (model.ExpiredOnDate != null)
                        {
                            events.ExpiredOn = DateTime.ParseExact(model.ExpiredOnDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            events.ExpiredOn = null;
                        }
                        events.CreatedBy = 1;
                        events.CreatedOn = DateTime.Now;

                        events.Contact = model.Contact;
                        events.EventDate = (model.EventDate != null && model.EventDate != "") ?
                            DateTime.ParseExact(model.EventDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture) : DateTime.Now;
                        events.Username = model.UserName;
                        events.Note = model.Note;
                        events.IsSystemEvent = IsSysEvent;

                        dbobj.Events.Add(events);
                        dbobj.SaveChanges();
                        return "Sucess";
                    }
                    catch
                    {
                        return "Failed";
                    }
                }
            }

        }

        public List<CommonCallLogViewModel> UpdateOrInsertMultipleCallLog(string Type, int academicReviewId, string Draft, IList<CommonCallLogViewModel> UpdateVals, int QstatusId)
        {
            MelmarkDBEntities objData = new MelmarkDBEntities();
            session = (clsSession)HttpContext.Current.Session["UserSession"];
            if (session != null)
            {
                // CommonCallLogViewModel defaltNullVal = new CommonCallLogViewModel();
                var testPresent = objData.ref_CallLogs.Where(x => x.QueueStatusId == QstatusId && x.StudentId == session.ReferralId && x.AcReviewId == academicReviewId).ToList();
                //Checkin weather the Value Present or not in table
                foreach (var val in testPresent)
                {
                    foreach (var callLog in UpdateVals)
                    {
                        if (val.CallLogId == callLog.CallLogId)
                        {
                            callLog.IsPresent = true;
                        }

                    }
                }


                foreach (var item in UpdateVals)
                {
                    ref_CallLogs CallLogVar = new ref_CallLogs();
                    //if (item.IsPresent == true)
                    //{
                    if (item.CallLogId != 0) CallLogVar = objData.ref_CallLogs.Single(x => x.CallLogId == item.CallLogId);
                    if (CallLogVar != null && CallLogVar.CallLogId != 0)
                    {
                        if (item.AppntTime != null || item.CallTime != null || item.Conversation != null || item.NameOfContact != null || item.StaffName != null)
                        {

                            CallLogVar.AppointmentTime = item.AppntTime;
                            CallLogVar.CallTime = item.CallTime;
                            CallLogVar.Draft = Draft;
                            if (item.Conversation != "")
                            {
                                CallLogVar.Conversation = item.Conversation;
                            }
                            CallLogVar.Nameofcontact = item.NameOfContact;
                            CallLogVar.StaffName = item.StaffName;
                            CallLogVar.ModifiedOn = System.DateTime.Now;
                            CallLogVar.ModifiedBy = session.LoginId;
                            CallLogVar.CallFlag = GetOutgoingId();
                            CallLogVar.QueueStatusId = QstatusId;
                            objData.SaveChanges();
                        }
                        else if (item.AppntTime == null && item.CallTime == null && item.Conversation == null && item.NameOfContact == null && item.StaffName == null)
                        {
                            objData.ref_CallLogs.Remove(CallLogVar);
                            objData.SaveChanges();
                        }
                    }
                    else
                    {
                        //if (item.AppntTime != null || item.CallTime != null || item.Conversation != null || item.NameOfContact != null || item.StaffName != null)
                        //{

                        if (item.NameOfContact != null && item.StaffName != null)
                        {
                            var vCallLogVar = objData.ref_CallLogs.Where(x => x.Nameofcontact == item.NameOfContact && x.StudentId == session.ReferralId && x.QueueStatusId == QstatusId && x.StaffName == item.StaffName && x.Conversation == item.Conversation).ToList();
                            if (vCallLogVar.Count() == 0)
                            {
                                CallLogVar.AppointmentTime = item.AppntTime;
                                CallLogVar.CallTime = item.CallTime;
                                CallLogVar.Draft = Draft;
                                if (item.Conversation != "")
                                {
                                    CallLogVar.Conversation = item.Conversation;
                                }
                                CallLogVar.Nameofcontact = item.NameOfContact;
                                CallLogVar.StaffName = item.StaffName;
                                CallLogVar.CreatedOn = System.DateTime.Now;
                                CallLogVar.CreatedBy = session.LoginId;
                                CallLogVar.SchoolId = session.SchoolId;
                                CallLogVar.StudentId = session.ReferralId;
                                CallLogVar.CallFlag = GetOutgoingId();
                                CallLogVar.Type = Type;
                                CallLogVar.AcReviewId = academicReviewId;
                                CallLogVar.QueueStatusId = QstatusId;
                                objData.ref_CallLogs.Add(CallLogVar);
                                objData.SaveChanges();
                            }
                        }
                    }
                }
                var CallLog = getRevMultipleCallLog(Type, academicReviewId);

                return CallLog;
            }


            else
            {
                List<CommonCallLogViewModel> defaultView = new List<CommonCallLogViewModel>();
                return defaultView;
            }
        }

        public List<CommonCallLogViewModel> UpdateOrInsertMultipleCallLogPre(IList<CommonCallLogViewModel> UpdateVals, int QstatusId)
        {
            MelmarkDBEntities objData = new MelmarkDBEntities();
            session = (clsSession)HttpContext.Current.Session["UserSession"];
            if (session != null)
            {
                // CommonCallLogViewModel defaltNullVal = new CommonCallLogViewModel();
                var testPresent = objData.ref_CallLogs.Where(x => x.QueueStatusId == QstatusId && x.StudentId == session.ReferralId).ToList();
                //Checkin weather the Value Present or not in table
                foreach (var val in testPresent)
                {
                    foreach (var callLog in UpdateVals)
                    {
                        if (val.CallLogId == callLog.CallLogId)
                        {
                            callLog.IsPresent = true;
                        }

                    }
                }


                foreach (var item in UpdateVals)
                {
                    ref_CallLogs CallLogVar = new ref_CallLogs();
                    //if (item.IsPresent == true)
                    //{
                    if (item.CallLogId != 0) CallLogVar = objData.ref_CallLogs.Single(x => x.CallLogId == item.CallLogId);
                    if (CallLogVar != null && CallLogVar.CallLogId != 0)
                    {
                        if (item.AppntTime != null || item.CallTime != null || item.Conversation != null || item.NameOfContact != null || item.StaffName != null)
                        {

                            CallLogVar.AppointmentTime = item.AppntTime;
                            CallLogVar.CallTime = item.CallTime;
                            CallLogVar.Draft = "Y";
                            if (item.Conversation != "")
                            {
                                CallLogVar.Conversation = item.Conversation;
                            }
                            CallLogVar.Nameofcontact = item.NameOfContact;
                            CallLogVar.StaffName = item.StaffName;
                            CallLogVar.ModifiedOn = System.DateTime.Now;
                            CallLogVar.ModifiedBy = session.LoginId;
                            CallLogVar.CallFlag = GetOutgoingId();
                            CallLogVar.QueueStatusId = QstatusId;
                            objData.SaveChanges();
                        }
                        else if (item.AppntTime == null && item.CallTime == null && item.Conversation == null && item.NameOfContact == null && item.StaffName == null)
                        {
                            objData.ref_CallLogs.Remove(CallLogVar);
                            objData.SaveChanges();
                        }
                    }
                    else
                    {
                        //if (item.AppntTime != null || item.CallTime != null || item.Conversation != null || item.NameOfContact != null || item.StaffName != null)
                        //{

                        if (item.NameOfContact != null && item.StaffName != null)
                        {
                            var vCallLogVar = objData.ref_CallLogs.Where(x => x.Nameofcontact == item.NameOfContact && x.StudentId == session.ReferralId && x.QueueStatusId == QstatusId && x.StaffName == item.StaffName && x.Conversation == item.Conversation).ToList();
                            if (vCallLogVar.Count() == 0)
                            {
                                CallLogVar.AppointmentTime = item.AppntTime;
                                CallLogVar.CallTime = item.CallTime;
                                CallLogVar.Draft = "Y";
                                if (item.Conversation != "")
                                {
                                    CallLogVar.Conversation = item.Conversation;
                                }
                                CallLogVar.Nameofcontact = item.NameOfContact;
                                CallLogVar.StaffName = item.StaffName;
                                CallLogVar.CreatedOn = System.DateTime.Now;
                                CallLogVar.CreatedBy = session.LoginId;
                                CallLogVar.SchoolId = session.SchoolId;
                                CallLogVar.StudentId = session.ReferralId;
                                CallLogVar.CallFlag = GetOutgoingId();
                                CallLogVar.Type = "SM";
                                CallLogVar.AcReviewId = 0;
                                CallLogVar.QueueStatusId = QstatusId;
                                objData.ref_CallLogs.Add(CallLogVar);
                                objData.SaveChanges();
                            }
                        }
                    }
                }
                var CallLog = getRevMultipleCallLogPre("SM");

                return CallLog;
            }


            else
            {
                List<CommonCallLogViewModel> defaultView = new List<CommonCallLogViewModel>();
                return defaultView;
            }
        }

        public int GetOutgoingId()
        {
            MelmarkDBEntities objData = new MelmarkDBEntities();
            var lookupModel = objData.LookUps.Where(x => x.LookupType == "Calllog Type" && x.LookupName == "Outgoing").SingleOrDefault();
            return lookupModel.LookupId;
        }
        public List<CommonCallLogViewModel> getRevMultipleCallLog(string querytype, int AcdReviewId)
        {
            MelmarkDBEntities objData = new MelmarkDBEntities();
            session = (clsSession)HttpContext.Current.Session["UserSession"];
            if (session != null)
            {
                //selecting value from table
                QstatusDetails qDetails = getQueueStatusId(session.ReferralId, querytype);
                int QstatusId = qDetails.QueueStatusId;
                bool SubmitStatus = false;
                if (QstatusId == 0)
                {
                    QstatusId = getQueueStatusIdIfSubmitted(session.ReferralId, querytype);
                    if (QstatusId > 0)
                    {
                        SubmitStatus = true;
                    }
                }

                var CallLogVar = (from x in objData.ref_CallLogs
                                  where (x.QueueStatusId == QstatusId && x.StudentId == session.ReferralId && x.AcReviewId == AcdReviewId)
                                  select new CommonCallLogViewModel
                                  {
                                      academicReviewId = x.AcReviewId,
                                      AppntTime = x.AppointmentTime,
                                      CallLogId = x.CallLogId,
                                      CallTime = x.CallTime,
                                      Conversation = x.Conversation,
                                      Draft = x.Draft,
                                      NameOfContact = x.Nameofcontact,
                                      SchoolId = x.SchoolId,
                                      StaffName = x.StaffName,
                                      StudentId = x.StudentId,
                                      type = x.Type,
                                      IsSubmit = SubmitStatus

                                  }).ToList();
                List<CommonCallLogViewModel> defaltNullVal = new List<CommonCallLogViewModel>();
                //Checkin weather the Value Present or not in table
                if (CallLogVar.Count > 0)
                {
                    foreach (var item in CallLogVar)
                    {
                        item.IsPresent = true;
                        //item.AppntTimeShow = (item.AppntTime == null) ? "" : item.AppntTime.Value.ToString("MM'/'dd'/'yyyy");
                        item.AppntDateShow = (item.AppntTime == null) ? "" : item.AppntTime.Value.ToString("MM'/'dd'/'yyyy");
                        item.AppntTimeShow = (item.AppntTime == null) ? "" : item.AppntTime.Value.ToString("HH':'mm");
                        item.CallDateShow = (item.CallTime == null) ? "" : item.CallTime.Value.ToString("MM'/'dd'/'yyyy");
                        item.CallTimeShow = (item.CallTime == null) ? "" : item.CallTime.Value.ToString("HH':'mm");
                    }

                    return CallLogVar;
                }
                else
                {
                    return defaltNullVal;
                }


            }
            else
            {
                List<CommonCallLogViewModel> defaultView = new List<CommonCallLogViewModel>();
                return defaultView;
            }

        }
        public List<CommonCallLogViewModel> getRevMultipleCallLogPre(string querytype = "SM")
        {
            MelmarkDBEntities objData = new MelmarkDBEntities();
            session = (clsSession)HttpContext.Current.Session["UserSession"];
            if (session != null)
            {
                //selecting value from table
                QstatusDetails qDetails = getQueueStatusId(session.ReferralId, querytype);
                int QstatusId = qDetails.QueueStatusId;
                bool SubmitStatus = false;
                if (QstatusId == 0)
                {
                    QstatusId = getQueueStatusIdIfSubmitted(session.ReferralId, querytype);
                    if (QstatusId > 0)
                    {
                        SubmitStatus = true;
                    }
                }

                var CallLogVar = (from x in objData.ref_CallLogs
                                  where (x.QueueStatusId == QstatusId && x.StudentId == session.ReferralId)
                                  select new CommonCallLogViewModel
                                  {
                                      academicReviewId = x.AcReviewId,
                                      AppntTime = x.AppointmentTime,
                                      CallLogId = x.CallLogId,
                                      CallTime = x.CallTime,
                                      Conversation = x.Conversation,
                                      Draft = x.Draft,
                                      NameOfContact = x.Nameofcontact,
                                      SchoolId = x.SchoolId,
                                      StaffName = x.StaffName,
                                      StudentId = x.StudentId,
                                      type = x.Type,
                                      IsSubmit = SubmitStatus

                                  }).ToList();
                List<CommonCallLogViewModel> defaltNullVal = new List<CommonCallLogViewModel>();
                //Checkin weather the Value Present or not in table
                if (CallLogVar.Count > 0)
                {
                    foreach (var item in CallLogVar)
                    {
                        item.IsPresent = true;
                        //item.AppntTimeShow = (item.AppntTime == null) ? "" : item.AppntTime.Value.ToString("MM'/'dd'/'yyyy");
                        item.AppntDateShow = (item.AppntTime == null) ? "" : item.AppntTime.Value.ToString("MM'/'dd'/'yyyy");
                        item.AppntTimeShow = (item.AppntTime == null) ? "" : item.AppntTime.Value.ToString("HH':'mm");
                        item.CallDateShow = (item.CallTime == null) ? "" : item.CallTime.Value.ToString("MM'/'dd'/'yyyy");
                        item.CallTimeShow = (item.CallTime == null) ? "" : item.CallTime.Value.ToString("HH':'mm");
                    }

                    return CallLogVar;
                }
                else
                {
                    return defaltNullVal;
                }


            }
            else
            {
                List<CommonCallLogViewModel> defaultView = new List<CommonCallLogViewModel>();
                return defaultView;
            }

        }


        //cheklistMultiple

        public List<CommonMulHeadViewMode> getCheckListMultiple(string querytype, string letterType)
        {

            session = (clsSession)HttpContext.Current.Session["UserSession"];
            bool isSubmit = false;
            // 'querytype' is the type
            if (session != null)
            {
                QstatusDetails qDetails = getQueueStatusId(session.ReferralId, querytype);
                int QstatusId = qDetails.QueueStatusId;
                if (QstatusId == 0)
                {
                    QstatusId = getQueueStatusIdIfSubmitted(session.ReferralId, querytype);
                    if (QstatusId > 0)
                    {
                        isSubmit = true;
                    }
                }
                MelmarkDBEntities objData = new MelmarkDBEntities();
                DateTime valDefalt = DateTime.Parse("1/1/2000");

                int qidVal = getQueueId(querytype);
                List<CommonMulHeadViewMode> ChkHead = new List<CommonMulHeadViewMode>();
                if (isSubmit == false)
                {
                    var SubmittedChecklist = objData.ref_CheckListAssign.Where(x => x.QueueStatusId == QstatusId && x.CheckListHeaderId == 0).ToList();
                    if (SubmittedChecklist.Count == 0)
                    {
                        ChkHead = (from x in objData.ref_Checklist
                                   where (x.QueueId == qidVal && x.ChecklistHeaderId == 0)
                                   select new CommonMulHeadViewMode
                                   {
                                       ChkHeadId = x.ChecklistId,
                                       ChkHeadName = x.ChecklistName
                                   }).ToList();
                    }
                    else
                    {
                        ChkHead = (from x in objData.ref_CheckListAssign
                                   //  join y in objData.ref_QueueStatus on x.QueueStatusId equals y.QueueStatusId
                                   where (x.QueueStatusId == QstatusId && x.CheckListHeaderId == 0)
                                   select new CommonMulHeadViewMode
                                   {
                                       ChkHeadId = x.CheckListId,
                                       ChkHeadName = x.CheckListName
                                   }).ToList();
                    }
                    foreach (var chkHd in ChkHead)
                    {
                        if (SubmittedChecklist.Count == 0)
                        {
                            chkHd.chkList = (from x in objData.ref_Checklist
                                             where (x.QueueId == qidVal && x.ChecklistHeaderId == chkHd.ChkHeadId)
                                             select new CommonMulCheckListViewModel
                                             {
                                                 checkListId = x.ChecklistId,
                                                 CheckListName = x.ChecklistName,
                                                 ChkHeadId = x.ChecklistHeaderId,
                                                 AssignMultiId = ""
                                             }).ToList();
                        }
                        else
                        {
                            chkHd.chkList = (from x in objData.ref_CheckListAssign
                                             where (x.QueueStatusId == QstatusId && x.CheckListHeaderId == chkHd.ChkHeadId)
                                             select new CommonMulCheckListViewModel
                                             {
                                                 checkListId = x.CheckListId,
                                                 CheckListName = x.CheckListName,
                                                 ChkHeadId = x.CheckListHeaderId,
                                                 AssignMultiId = ""
                                             }).ToList();
                        }
                        //ChkHead = (from x in objData.ref_Checklist
                        //           where (x.QueueId == qidVal && x.ChecklistHeaderId == 0)
                        //           select new CommonMulHeadViewMode
                        //           {
                        //               ChkHeadId = x.ChecklistId,
                        //               ChkHeadName = x.ChecklistName
                        //           }).ToList();
                        //foreach (var chkHd in ChkHead)
                        //{
                        //    chkHd.chkList = (from x in objData.ref_Checklist
                        //                     where (x.QueueId == qidVal && x.ChecklistHeaderId == chkHd.ChkHeadId)
                        //                     select new CommonMulCheckListViewModel
                        //                     {
                        //                         checkListId = x.ChecklistId,
                        //                         CheckListName = x.ChecklistName,
                        //                         ChkHeadId = x.ChecklistHeaderId,
                        //                         AssignMultiId = ""
                        //                     }).ToList();

                        if (QstatusId > 0)
                        {
                            foreach (var v in chkHd.chkList)
                            {
                                var IspresenVal = (from x in objData.ref_CheckListAssign
                                                   join y in objData.ref_ReviewComments on x.AcReviewId equals y.AcReviewId
                                                   where (y.QueueStatusId == QstatusId && x.CheckListId == v.checkListId && x.CheckListId != 0)
                                                   select new CommmonCheckListViewModel
                                                   {
                                                       DueDate = x.DueDate,
                                                       Complete = x.CompleteInd,
                                                       AssginId = x.AssignId,
                                                   }).ToList();
                                if (IspresenVal.Count > 0)
                                {
                                    v.IsPresent = true;
                                    v.DueDate = IspresenVal[0].DueDate;
                                    v.AssginId = IspresenVal[0].AssginId;
                                    v.Complete = IspresenVal[0].Complete;
                                }
                                else
                                {
                                    v.IsPresent = false;
                                }
                            }

                            foreach (var v in chkHd.chkList)
                            {
                                //checkin if Value Present in ChkAssinTable
                                if (v.IsPresent == true)
                                {
                                    if (v.DueDate.HasValue == true)
                                        v.DueDateToShow = v.DueDate.Value.ToString("MM'/'dd'/'yyyy");
                                    else
                                        v.DueDateToShow = "";
                                    if (v.AssginId != 0)
                                    {
                                        //Assigning AssignId nd userNames to the view model
                                        var useraval = (from x in objData.ref_CheckListUsers
                                                        join y in objData.Users on x.UserId equals y.UserId
                                                        where (y.ActiveInd == "A" && x.ChecklistUserId == v.AssginId)
                                                        select new
                                                        {
                                                            UserId = x.UserId,
                                                            FirstName = y.UserFName,
                                                            LastName = y.UserLName,
                                                        }).ToList();
                                        v.AssignMultiId = "";
                                        v.AssignMultiName = "";
                                        int h = 0;
                                        foreach (var vr in useraval)
                                        {

                                            v.AssignMultiId = v.AssignMultiId + (h == 0 ? vr.UserId.ToString() : "," + vr.UserId);
                                            v.AssignMultiName = v.AssignMultiName + (h == 0 ? vr.LastName + "," + vr.FirstName : ";" + vr.LastName + "," + vr.FirstName);
                                            h++;
                                        }
                                    }

                                }
                                else
                                {
                                    v.DueDateToShow = "";
                                    v.checkListval = "False";
                                }
                            }

                            foreach (var v in chkHd.chkList)
                            {
                                if (v.AssignMultiId == "" || v.AssignMultiId == null)
                                {
                                    v.AssignMultiId = "0";
                                }
                                if (v.AssignMultiName == null || v.AssignMultiName == "")
                                {
                                    v.AssignMultiName = " ";
                                }

                            }

                        }
                    }
                }
                else
                {
                    if (QstatusId > 0)
                    {
                        ChkHead = (from x in objData.ref_CheckListAssign
                                   //  join y in objData.ref_QueueStatus on x.QueueStatusId equals y.QueueStatusId
                                   where (x.QueueStatusId == QstatusId && x.CheckListHeaderId == 0)
                                   select new CommonMulHeadViewMode
                                   {
                                       ChkHeadId = x.CheckListId,
                                       ChkHeadName = x.CheckListName
                                   }).ToList();
                        foreach (var chkHd in ChkHead)
                        {
                            chkHd.chkList = (from x in objData.ref_CheckListAssign
                                             where (x.QueueStatusId == QstatusId && x.CheckListHeaderId == chkHd.ChkHeadId)
                                             select new CommonMulCheckListViewModel
                                             {
                                                 checkListId = x.CheckListId,
                                                 CheckListName = x.CheckListName,
                                                 ChkHeadId = x.CheckListHeaderId,
                                                 DueDate = x.DueDate,
                                                 Complete = x.CompleteInd,
                                                 AssginId = x.AssignId,
                                                 AssignMultiId = "",
                                                 IsPresent = true
                                             }).ToList();
                            foreach (var v in chkHd.chkList)
                            {
                                //checkin if Value Present in ChkAssinTable
                                if (v.IsPresent == true)
                                {
                                    if (v.DueDate.HasValue == true)
                                        v.DueDateToShow = v.DueDate.Value.ToString("MM'/'dd'/'yyyy");
                                    else
                                        v.DueDateToShow = "";
                                    if (v.AssginId != 0)
                                    {
                                        //Assigning AssignId nd userNames to the view model
                                        var useraval = (from x in objData.ref_CheckListUsers
                                                        join y in objData.Users on x.UserId equals y.UserId
                                                        where (y.ActiveInd == "A" && x.ChecklistUserId == v.AssginId)
                                                        select new
                                                        {
                                                            UserId = x.UserId,
                                                            FirstName = y.UserFName,
                                                            LastName = y.UserLName,
                                                        }).ToList();
                                        v.AssignMultiId = "";
                                        v.AssignMultiName = "";
                                        int h = 0;
                                        foreach (var vr in useraval)
                                        {

                                            v.AssignMultiId = v.AssignMultiId + (h == 0 ? vr.UserId.ToString() : "," + vr.UserId);
                                            v.AssignMultiName = v.AssignMultiName + (h == 0 ? vr.LastName + "," + vr.LastName : ";" + vr.LastName + "," + vr.LastName);
                                            h++;
                                        }
                                    }

                                }
                                else
                                {
                                    v.DueDateToShow = "";
                                    v.checkListval = "False";
                                }
                            }

                            foreach (var v in chkHd.chkList)
                            {
                                if (v.AssignMultiId == "" || v.AssignMultiId == null)
                                {
                                    v.AssignMultiId = "0";
                                }
                                if (v.AssignMultiName == null || v.AssignMultiName == "")
                                {
                                    v.AssignMultiName = " ";
                                }

                            }

                        }
                    }

                }

                return ChkHead;
            }
            else
            {
                List<CommonMulHeadViewMode> defaultView = new List<CommonMulHeadViewMode>();
                return defaultView;
            }
        }

        public IList<CommonMulHeadViewMode> UpdateOrInsertMulCheckList(IList<CommonMulHeadViewMode> UpdateVals, int AcReviewId, string Type, int QstatusId, string letterType)
        {


            MelmarkDBEntities objData = new MelmarkDBEntities();
            ref_CheckListAssign CheckListAssign = new ref_CheckListAssign();
            session = (clsSession)HttpContext.Current.Session["UserSession"];
            if (session != null)
            {
                if (UpdateVals != null)
                {
                    foreach (var chkHd in UpdateVals)
                    {
                        var testPresentHd = objData.ref_CheckListAssign.Where(x => x.QueueStatusId == QstatusId && x.CheckListHeaderId == 0 && x.CheckListId == chkHd.ChkHeadId).ToList();
                        if (testPresentHd.Count == 0)
                        {
                            CheckListAssign.CheckListId = chkHd.ChkHeadId;
                            CheckListAssign.CheckListName = chkHd.ChkHeadName;
                            CheckListAssign.Type = Type;
                            CheckListAssign.AcReviewId = AcReviewId;
                            CheckListAssign.CreatedOn = System.DateTime.Now;
                            CheckListAssign.CreatedBy = session.LoginId;
                            CheckListAssign.QueueStatusId = QstatusId;
                            CheckListAssign.CheckListHeaderId = 0;
                            objData.ref_CheckListAssign.Add(CheckListAssign);
                            objData.SaveChanges();
                        }

                        foreach (var chkLst in chkHd.chkList)
                        {
                            var testPresentCgk = objData.ref_CheckListAssign.Where(x => x.QueueStatusId == QstatusId && x.CheckListHeaderId == chkLst.ChkHeadId && x.CheckListId == chkLst.checkListId).ToList();


                            if (testPresentCgk.Count > 0) //  Data Present
                            {
                                if (testPresentCgk[0].CheckListHeaderId != 0)
                                {
                                    if (chkLst.checkListval == "True" || chkLst.checkListval == "true")
                                    {
                                        //CheckListAssign = objData.ref_CheckListAssign.Single(x => x.AssignId == testPresentCgk[0].AssignId);
                                        CheckListAssign = testPresentCgk[0];
                                        CheckListAssign.CompleteInd = true;
                                        if (chkLst.DueDateToShow != null && chkLst.DueDateToShow != "" && chkLst.AssignMultiId != "NA")
                                        {
                                            CheckListAssign.DueDate = DateTime.ParseExact(chkLst.DueDateToShow, "MM'/'dd'/'yyyy", System.Globalization.CultureInfo.CurrentCulture);
                                        }
                                        CheckListAssign.ModifiedOn = System.DateTime.Now;
                                        CheckListAssign.ModifiedBy = session.LoginId;
                                        objData.SaveChanges();
                                    }
                                    else
                                    {
                                        //CheckListAssign = objData.ref_CheckListAssign.Single(x => x.AssignId == testPresentCgk[0].AssignId);
                                        // var vr = objData.ref_CheckListAssign.Where(x => x.AssignId == testPresentCgk[0].AssignId).ToList();
                                        // if (vr.Count > 0)
                                        //  {
                                        CheckListAssign = testPresentCgk[0];
                                        CheckListAssign.CompleteInd = false;
                                        if (chkLst.DueDateToShow != null && chkLst.DueDateToShow != "" && chkLst.AssignMultiId != "NA")
                                        {
                                            CheckListAssign.DueDate = DateTime.ParseExact(chkLst.DueDateToShow, "MM'/'dd'/'yyyy", System.Globalization.CultureInfo.CurrentCulture);
                                        }
                                        else
                                            CheckListAssign.DueDate = null;
                                        //CheckListAssign.DueDate = System.DateTime.Now;
                                        CheckListAssign.ModifiedOn = System.DateTime.Now;
                                        CheckListAssign.ModifiedBy = session.LoginId;
                                        objData.SaveChanges();
                                        // }
                                    }
                                }
                            }
                            else
                            {

                                CheckListAssign.CheckListId = chkLst.checkListId;
                                CheckListAssign.CheckListName = chkLst.CheckListName;
                                if (chkLst.DueDateToShow != null && chkLst.DueDateToShow != "" && chkLst.AssignMultiId != "NA")
                                {
                                    CheckListAssign.DueDate = DateTime.ParseExact(chkLst.DueDateToShow, "MM'/'dd'/'yyyy", System.Globalization.CultureInfo.CurrentCulture);
                                }
                                else
                                    CheckListAssign.DueDate = null;

                                CheckListAssign.Type = Type;

                                CheckListAssign.AcReviewId = AcReviewId;
                                CheckListAssign.CompleteInd = (chkLst.checkListval == "False" || chkLst.checkListval == "false") ? false : true;
                                CheckListAssign.CreatedOn = System.DateTime.Now;
                                CheckListAssign.CreatedBy = session.LoginId;
                                CheckListAssign.QueueStatusId = QstatusId;
                                CheckListAssign.CheckListHeaderId = chkLst.ChkHeadId;
                                objData.ref_CheckListAssign.Add(CheckListAssign);
                                objData.SaveChanges();
                                chkLst.AssginId = CheckListAssign.AssignId;

                            }

                            ////    NA
                            //check if assignmultiid = null
                            if (chkLst.AssignMultiId != null)
                            {
                                if (chkLst.AssignMultiId != "NA")
                                {

                                    var CheckListusers = objData.ref_CheckListUsers.Where(x => x.ChecklistUserId == chkLst.AssginId).ToList();
                                    if (CheckListusers.Count == 0)
                                    {
                                        if (chkLst.AssignMultiId != "0")
                                        {
                                            var Allid = chkLst.AssignMultiId.Split(',');
                                            for (int i = 0; i < Allid.Length; i++)
                                            {
                                                int id = Convert.ToInt32(Allid[i]);
                                                ref_CheckListUsers users = new ref_CheckListUsers();
                                                users.ChecklistUserId = chkLst.AssginId;
                                                users.QueueStatusId = QstatusId;
                                                users.UserId = id;
                                                users.CreatedBy = session.LoginId;
                                                users.CreatedOn = System.DateTime.Now;
                                                users.ModifiedBy = session.LoginId;
                                                users.ModifiedOn = System.DateTime.Now;
                                                objData.ref_CheckListUsers.Add(users);
                                                objData.SaveChanges();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        for (int i = 0; i < CheckListusers.Count; i++)
                                        {
                                            objData.ref_CheckListUsers.Remove(CheckListusers[i]);
                                            objData.SaveChanges();
                                        }
                                        if (chkLst.AssignMultiId != "0" && chkLst.AssignMultiId != null && chkLst.AssignMultiId != "")
                                        {
                                            var Allid = chkLst.AssignMultiId.Split(',');
                                            for (int j = 0; j < Allid.Length; j++)
                                            {
                                                int id = Convert.ToInt32(Allid[j]);
                                                ref_CheckListUsers users = new ref_CheckListUsers();
                                                users.QueueStatusId = QstatusId;
                                                users.ChecklistUserId = chkLst.AssginId;
                                                users.UserId = id;
                                                users.CreatedBy = session.LoginId;
                                                users.CreatedOn = System.DateTime.Now;
                                                users.ModifiedBy = session.LoginId;
                                                users.ModifiedOn = System.DateTime.Now;
                                                objData.ref_CheckListUsers.Add(users);
                                                objData.SaveChanges();
                                            }
                                        }

                                    }
                                }
                            }

                            ////    NA End


                        }


                    }
                }
                var check = getCheckListMultiple(Type, letterType);
                return check;
            }
            else
            {
                List<CommonMulHeadViewMode> defaultView = new List<CommonMulHeadViewMode>();
                return defaultView;
            }
        }

        //To set permission based on role of user
        public string setPermission()
        {
            MelmarkDBEntities Objdata = new MelmarkDBEntities();
            session = (clsSession)HttpContext.Current.Session["UserSession"];
            string permission = "false";
            var Role = (from Objrole in Objdata.Roles
                        join objrgp in Objdata.RoleGroups on Objrole.RoleId equals objrgp.RoleId
                        select new
                        {
                            RoleId = Objrole.RoleId,
                            Roledesc = Objrole.RoleDesc,
                            schoolid = Objrole.SchoolId,
                            RoleCode = Objrole.RoleCode
                        }).ToList();
            var Usr = (from Objrole in Role
                       from Objusr in Objdata.Users
                       where Objusr.UserId == session.LoginId
                       select new
                       {
                           Objrole.RoleId,
                           Objrole.Roledesc,
                           Objusr.SchoolId,
                           Objusr.UserId,
                           Objusr.UserFName,
                           Objusr.UserLName,
                           Objusr.Gender,
                           Objrole.RoleCode

                       }).ToList();

            var rolePerm = (from objRoleGroupPermission in Objdata.RoleGroupPerms
                            join objObject in Objdata.Objects on objRoleGroupPermission.ObjectId equals objObject.ObjectId
                            join objRoleGroup in Objdata.RoleGroups on objRoleGroupPermission.RoleGroupId equals objRoleGroup.RoleGroupId
                            join objRole in Objdata.Roles on objRoleGroup.RoleId equals objRole.RoleId
                            join objUserRoleGroup in Objdata.UserRoleGroups on objRoleGroup.RoleGroupId equals objUserRoleGroup.RoleGroupId
                            where (objObject.ObjectName == "Referral" || objObject.ObjectName == "Referal") && objUserRoleGroup.UserId == session.LoginId && objUserRoleGroup.ActiveInd == "A"
                            select new
                            {
                                ApproveInd = objRoleGroupPermission.WriteInd

                            }).ToList();

            if (Usr.Count() > 0)
            {
                if (rolePerm.Count > 0)
                {
                    permission = "false";
                    for (int i = 0; i < rolePerm.Count; i++)
                    {
                        if (rolePerm[i].ApproveInd == true)
                            permission = "true";
                    }


                }
                else
                    permission = "false";
            }


            //ClsErrorLog errorLog = new ClsErrorLog();
            //errorLog.WriteToLog("permission: " + permission);
            return permission;
        }

        public IEnumerable<SelectListItem> getType(string type)
        {
            MelmarkDBEntities dbobj = new MelmarkDBEntities();
            IList<LookUp> PlacementTypeData = new List<LookUp>();
            SelectListItem onesele = new SelectListItem { Text = "-- Select --", Value = "" };
            IList<SelectListItem> placementTypeSelecteditem = new List<SelectListItem>();
            placementTypeSelecteditem.Add(onesele);
            PlacementTypeData = dbobj.LookUps.Where(objLookUp => objLookUp.LookupType == type && objLookUp.ActiveInd == "A").ToList();
            var placementSelecteditemsub = (from placementType in PlacementTypeData select new SelectListItem { Text = placementType.LookupName, Value = placementType.LookupId.ToString() }).ToList();
            foreach (SelectListItem sele in placementSelecteditemsub)
            {
                placementTypeSelecteditem.Add(sele);
            }
            return placementTypeSelecteditem;
        }

        public IEnumerable<SelectListItem> getUserType(string type)
        {
            MelmarkDBEntities dbobj = new MelmarkDBEntities();
            IList<User> UserTypeData = new List<User>();
            SelectListItem onesele = new SelectListItem { Text = "-- Select --", Value = "" };
            IList<SelectListItem> placementTypeSelecteditem = new List<SelectListItem>();
            placementTypeSelecteditem.Add(onesele);
            UserTypeData = (from objUser in dbobj.Users
                            join objUserRoleGroup in dbobj.UserRoleGroups on objUser.UserId equals objUserRoleGroup.UserId
                            join objRoleGroup in dbobj.RoleGroups on objUserRoleGroup.RoleGroupId equals objRoleGroup.RoleGroupId
                            join objGroup in dbobj.Groups on objRoleGroup.GroupId equals objGroup.GroupId
                            where objGroup.GroupCode == type && objUser.ActiveInd == "A" && objUserRoleGroup.ActiveInd == "A"
                            select objUser).ToList();

            var placementSelecteditemsub = (from userType in UserTypeData select new SelectListItem { Text = userType.UserLName + " " + userType.UserLName, Value = userType.UserId.ToString() }).ToList();
            foreach (SelectListItem sele in placementSelecteditemsub)
            {
                placementTypeSelecteditem.Add(sele);
            }
            return placementTypeSelecteditem;
        }


        public string Save(int StudentId, int SchoolId, int UserId, CommonCallLogViewModel model)
        {
            string result = "";
            MelmarkDBEntities objData = new MelmarkDBEntities();
            ref_CallLogs CallLogVar = new ref_CallLogs();
            DateTime dtcalldate = new DateTime();
            DateTime dtappntdate = new DateTime();
            try
            {
                if (model.CallDateShow != null)
                    dtcalldate = DateTime.ParseExact(model.CallDateShow, "MM'/'dd'/'yyyy", System.Globalization.CultureInfo.CurrentCulture);
                if (model.CallTimeShow == null && model.CallDateShow != null)
                    model.CallTimeShow = "00:00AM";
                if (model.CallDateShow != null)
                {
                    model.CallTime = dtcalldate.Add(TimeSpan.Parse(amPmTo24hourConverter(model.CallTimeShow)));
                }

                if (model.AppntDateShow != null)
                    dtappntdate = DateTime.ParseExact(model.AppntDateShow, "MM'/'dd'/'yyyy", System.Globalization.CultureInfo.CurrentCulture);
                if (model.AppntTimeShow == null && model.AppntDateShow != null)
                    model.AppntTimeShow = "00:00AM";
                if (model.AppntDateShow != null)
                {
                    model.AppntTime = dtappntdate.Add(TimeSpan.Parse(amPmTo24hourConverter(model.AppntTimeShow)));
                }

                int contactlogtype = Convert.ToInt32(model.ContactlogType);
                CallLogVar.AppointmentTime = model.AppntTime;
                CallLogVar.CallTime = model.CallTime;
                CallLogVar.Draft = "Y";
                CallLogVar.CallFlag = contactlogtype;
                if (model.Relationship == "")
                    CallLogVar.RelationshipId = 0;
                else
                    CallLogVar.RelationshipId = Convert.ToInt32(model.Relationship);
                CallLogVar.Conversation = model.Conversation;
                CallLogVar.Nameofcontact = model.NameOfContact;
                CallLogVar.StaffName = model.StaffName;
                CallLogVar.CreatedOn = System.DateTime.Now;
                CallLogVar.CreatedBy = UserId;
                CallLogVar.SchoolId = SchoolId;
                CallLogVar.StudentId = model.StudentId;
                CallLogVar.Type = "NA";
                CallLogVar.AcReviewId = 0;
                CallLogVar.QueueStatusId = 0;
                objData.ref_CallLogs.Add(CallLogVar);
                objData.SaveChanges();
                result = "Sucess";
            }
            catch (Exception ex)
            {
                result = "Failed";

            }

            return result;
        }

        public string amPmTo24hourConverter(string time)
        {
            string[] Alltime = time.Split(':');
            int h = Convert.ToInt16(Alltime[0]);
            string startAMPM = Alltime[1].Substring(2, 2);
            string m = Alltime[1].Substring(0, 2);
            if (startAMPM == "PM")
            {
                if (h != 12)
                {
                    h += 12;
                }

            }
            else if (startAMPM == "AM")
            {
                if (h == 12)
                {
                    h = 00;
                }
            }
            return (h.ToString() + ":" + m.ToString());
        }
    }


}