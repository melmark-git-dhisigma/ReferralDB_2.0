using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReferalDB.Models;
using BuisinessLayer;
using DataLayer;
using System.Globalization;
using ReferalDBApplicant.Classes;


namespace ReferalDB.CommonClass
{
    public class clsDashboard
    {
        public MelmarkDBEntities objData = null;
        public IList<DashboardModelClass> FillDashboard(int Schoolid, string Queuename, string SearchAlpahabet, string SearchWeek, string SearchMonth, string SearchAge, string SearchAppdata, string SearchSort, string SearchQueue, string SearchNotification)
        {
            objData = new MelmarkDBEntities();
            clsReferral objClsRef = new clsReferral();
            ClsCommon objCommon = new ClsCommon();
            IList<DashboardModelClass> retunmodel = new List<DashboardModelClass>();
            IList<DashboardModelClass> modellist = new List<DashboardModelClass>();
            int queueid = 0;

            string[] Queues = Queuename.Split(',');
            int MasterId = 0;
            string lastStepName = "";
            int studentId = 0;
            string CurrentQId = "";
            //queueid = Convert.ToInt32(Queues[0].ToString());
            if (!String.IsNullOrEmpty(SearchQueue))
            {
                queueid = SelectQueueID(SearchQueue);
            }
            else
            {
                queueid = Convert.ToInt32(Queues[0].ToString());
            }


            string SType = "Referral";
            if (SearchQueue == "CL")
            {
                //if (Queues[1].ToString() == "ClientList")
                //{
                SType = "Client";
                //}
            }
            else if (SearchQueue == "")
            {
                if (Queues[1].ToString() == "ClientList")
                {
                    SType = "Client";
                }
            }

            if (SType == "Client")
            {
                modellist = (from objref in objData.Client_Dashboard(queueid, Schoolid, SType)
                             select new DashboardModelClass
                             {
                                 ReferralId = objref.ReferralId,
                                 ReferralName = objref.ReferralName,
                                 ReferralDob = objref.BirthDate,
                                 ReferralGender = objref.Gender,
                                 ApplicationDate = objref.Appdate,
                                 CompletedStep = objref.LastCompleted,
                                 CompletedBy = objref.CompletedBy,
                                 ImageUrl = objClsRef.GetStudentImage(objref.ReferralId),
                                 ActiveProcess = Convert.ToString(objref.ActiveProcess),
                                 Perc = 100
                             }).ToList();
                if (modellist.Count > 0)
                {
                    foreach (var items in modellist)
                    {
                        retunmodel.Add(items);
                    }
                }

            }
            else
            {
                if (SType != "Referral")
                {
                    int LastQId = objCommon.getQueueId("DC");
                    var currentProcessQIds = objData.ref_QueueStatus.Where(x => x.SchoolId == Schoolid && x.CurrentStatus == true).ToList();
                    if (currentProcessQIds.Count > 0)
                    {
                        foreach (var item in currentProcessQIds)
                        {
                            if (item.QueueId <= LastQId)
                            {
                                studentId = item.StudentPersonalId;
                                CurrentQId = Convert.ToString(item.QueueId);
                                var masterId = objData.ref_Queue.Where(x => x.SchoolId == Schoolid && x.QueueId == item.QueueId).ToList();
                                if (masterId.Count > 0)
                                    MasterId = masterId[0].MasterId;
                                if (queueid < MasterId)
                                {
                                    var lastStep = objData.ref_Queue.Where(x => x.SchoolId == Schoolid && x.MasterId == queueid).ToList();
                                    if (lastStep.Count > 0)
                                        lastStepName = lastStep[lastStep.Count - 1].QueueName;
                                    modellist = (from objref in objData.StudentPersonals
                                                 join objUser in objData.Users on objref.CreatedBy equals objUser.UserId
                                                 where objref.StudentPersonalId == studentId
                                                 select new DashboardModelClass
                                                 {
                                                     ReferralId = objref.StudentPersonalId,
                                                     ReferralName = objref.LastName + "," + objref.FirstName,
                                                     ReferralDob = objref.BirthDate,
                                                     ReferralGender = objref.Gender,
                                                     ApplicationDate = objref.CreatedOn,
                                                     CompletedStep = lastStepName,
                                                     CompletedBy = objUser.UserLName + "," + objUser.UserFName,
                                                     ImageUrl = objref.ImageUrl,
                                                     ActiveProcess = CurrentQId,
                                                     Perc = 100
                                                 }).ToList();
                                    if (modellist.Count > 0)
                                    {
                                        foreach (var items in modellist)
                                        {
                                            if (items.ReferralGender == "1")
                                                items.ReferralGender = "Male";
                                            else
                                                items.ReferralGender = "Female";
                                            retunmodel.Add(items);
                                        }
                                    }

                                }
                            }
                        }
                    }
                }
                modellist = (from objref in objData.Referral_Dashboard(queueid, Schoolid, SType)
                             select new DashboardModelClass
                             {
                                 ReferralId = objref.ReferralId,
                                 ReferralName = objref.ReferralName,
                                 ReferralDob = objref.BirthDate,
                                 ReferralGender = objref.Gender,
                                 ApplicationDate = objref.Appdate,
                                 CompletedStep = objref.LastCompleted,
                                 CompletedBy = objref.CompletedBy,
                                 ImageUrl = objClsRef.GetStudentImage(objref.ReferralId),
                                 ActiveProcess = Convert.ToString(objref.ActiveProcess),
                                 Perc = objref.Percentage
                             }).ToList();
                if (modellist.Count > 0)
                {
                    foreach (var items in modellist)
                    {

                        var now = float.Parse(DateTime.Now.ToString("yyyy.MMdd"));
                        var dob = float.Parse(((DateTime)items.ReferralDob).ToString("yyyy.MMdd"));
                        var age = (int)(now - dob);
                        items.age = age;
                        retunmodel.Add(items);
                    }
                }
            }


            //retunmodel = (from objref in objData.Referral_Dashboard(queueid, Schoolid, SType)
            //              select new DashboardModelClass
            //              {
            //                  ReferralId = objref.ReferralId,
            //                  ReferralName = objref.ReferralName,
            //                  ReferralDob = objref.BirthDate,
            //                  ReferralGender = objref.Gender,
            //                  ApplicationDate = objref.Appdate,
            //                  CompletedStep = objref.LastCompleted,
            //                  CompletedBy = objref.CompletedBy,
            //                  ImageUrl = objClsRef.GetStudentImage(objref.ReferralId),
            //                  ActiveProcess = Convert.ToString(objref.ActiveProcess),
            //                  Perc = objref.Percentage
            //              }).ToList();
            //}

            if (!String.IsNullOrEmpty(SearchAlpahabet))
                retunmodel = retunmodel.Where(p => p.ReferralName.StartsWith(SearchAlpahabet.ToLower()) || p.ReferralName.StartsWith(SearchAlpahabet)).ToList();
            if (!String.IsNullOrEmpty(SearchWeek))
                retunmodel = retunmodel.Where(p => p.ApplicationDate >= DateTime.Now.AddDays(-5) && p.ApplicationDate <= DateTime.Now.Date).ToList();
            if (!String.IsNullOrEmpty(SearchMonth))
                retunmodel = retunmodel.Where(p => p.ApplicationDate >= DateTime.Now.AddDays(-30) && p.ApplicationDate <= DateTime.Now.Date).ToList();
            if (!String.IsNullOrEmpty(SearchAge))
            {
                string[] Age = SearchAge.Split(',');
                int SAge = Convert.ToInt32(Age[0]);
                int EAge = Convert.ToInt32(Age[1]);

                retunmodel = retunmodel.Where(p => (p.age >= SAge) && (p.age <= EAge)).ToList();
            }
            if (!String.IsNullOrEmpty(SearchAppdata))
            {
                string[] AppDate = SearchAppdata.Split(',');
                DateTime SDate = new DateTime();
                DateTime Edate = new DateTime();
                Edate = DateTime.ParseExact(AppDate[1].Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                SDate = DateTime.ParseExact(AppDate[0].Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                retunmodel = retunmodel.Where(p => p.ApplicationDate >= SDate && p.ApplicationDate <= Edate).ToList();
            }
            if (!string.IsNullOrEmpty(SearchSort))
            {
                if (SearchSort == "1") // Sort for age
                {
                    retunmodel = retunmodel.OrderByDescending(p => p.ReferralDob).ToList();
                }
                else if (SearchSort == "2") //sort for date of birth
                {
                    retunmodel = retunmodel.OrderBy(p => p.ReferralDob).ToList();
                }
                else if (SearchSort == "3") //sort for application date
                {
                    retunmodel = retunmodel.OrderBy(p => p.ApplicationDate).ToList();
                }
            }


            return retunmodel;
            //if (!String.IsNullOrEmpty(SearchNotification))
            //    Result = Result.Where(p => p.ReferralName.StartsWith(SearchAlpahabet));
        }

        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public int SelectQueueID(string QueueType)
        {
            objData = new MelmarkDBEntities();
            var Queue = objData.ref_Queue.Where(objque => objque.QueueType == QueueType).SingleOrDefault();
            return Queue.QueueId;
        }
    }
}