using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using ReferalDB.Models;
using BuisinessLayer;
using ReferalDB.CommonClass;

namespace ReferalDB.Controllers
{

    public class AdmissionReviewController : Controller
    {
        MelmarkDBEntities objData = null;
        clsSession sess = null;
        ReviewTeam rtobj = null;
        TeamMember tmobj = null;
        //
        // GET: /AdmissionReview/

        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult LoadUsers()
        {
            sess = (clsSession)Session["UserSession"];
            UserModel usrModel = new UserModel();
            ClsCommon getCommon = new ClsCommon();
            if (sess != null)
            {
                ViewBag.permission = getCommon.setPermission();
                usrModel = UserModel.BindReviewTeam(sess.SchoolId);
            }
            return View("../Engine/AdmissionReview", usrModel);
        }

        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult LoadTeam()
        {
            sess = (clsSession)Session["UserSession"];
            UserModel usrModel = new UserModel();
            if (sess != null)
            {
                usrModel = UserModel.BindReviewTeam(sess.SchoolId);
                usrModel.SelectdPersonals = "1,2,3";
                objData = new MelmarkDBEntities();
                usrModel.StdList = (from x in objData.StudentPersonals
                                    where x.StudentType == "Referral"
                                    select new studentDetails
                          {
                              studentPersonalId = x.StudentPersonalId,
                              studentPersonal = x.LastName + "'" + x.FirstName
                          }).ToList();

            }
            return View("../Engine/TeamUsers", usrModel);
        }

        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public string GetAssigndStd(string TeamId)// Refid is a combination of Referralid_CurrentQueueId
        {
            objData = new MelmarkDBEntities();
            int tId = int.Parse(TeamId);
            var valStdteam = objData.ref_TeamReferrals.Where(x => x.TeamId == tId && x.ActiveInd == "A").ToList();
            string retVal = "";
            foreach (var vstd in valStdteam)
            {
                retVal += vstd.StudentPersonalId.ToString() + ",";
            }
            return retVal;
        }

        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public string AssigndStdSubmit(string StdIdz, int teamId)// Refid is a combination of Referralid_CurrentQueueId
        {
            sess = (clsSession)Session["UserSession"];

            if (sess != null)
            {
                try
                {
                    string[] StdtIdz = StdIdz.Split(',');
                    objData = new MelmarkDBEntities();
                    var val = objData.ref_TeamReferrals.Where(x => x.TeamId == teamId).ToList();
                    if (val.Count > 0)
                    {
                        foreach (var vr in val)
                        {
                            objData.ref_TeamReferrals.Remove(vr);
                            objData.SaveChanges();
                        }
                    }
                    ref_TeamReferrals tr = new ref_TeamReferrals();
                    for (int i = 0; i < StdtIdz.Length - 1; i++)
                    {
                        tr.TeamId = teamId;
                        tr.SchoolId = sess.SchoolId;
                        tr.StudentPersonalId = int.Parse(StdtIdz[i]);
                        tr.ActiveInd = "A";
                        tr.CreatedBy = sess.LoginId;
                        tr.CreatedOn = System.DateTime.Now;
                        objData.ref_TeamReferrals.Add(tr);
                        objData.SaveChanges();
                    }
                    return clsGeneral.sucessMsg("Team updated successfully ");
                }
                catch (Exception e)
                {
                    return clsGeneral.failedMsg(e.Message);
                }

            }
            return "";
        }

        //[HttpPost, ValidateInput(false)]
        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult SaveAdmissionReviewTeam(string id)
        {
            UserModel objuser = new UserModel();
            sess = (clsSession)Session["UserSession"];
            ClsCommon getCommon = null;
            try
            {
                string[] AdmissionList = id.Split(',');
                if (AdmissionList.Count() < 4)
                {
                    ViewData["Message"] = "<div class='error_box'>At least one member required..</div>";
                    objuser = UserModel.BindReviewTeam(sess.SchoolId);
                    getCommon = new ClsCommon();
                    ViewBag.permission = getCommon.setPermission();
                    return View("../Engine/AdmissionReview", objuser);
                }
                objData = new MelmarkDBEntities();

                if (sess != null)
                {
                    rtobj = new ReviewTeam();
                    tmobj = new TeamMember();
                    if (AdmissionList[0] == "0")
                    {
                        //////Save Review Team//////
                        rtobj.TeamName = AdmissionList[1];
                        rtobj.CreatedBy = sess.LoginId;
                        rtobj.CreatedOn = DateTime.Now;
                        rtobj.ModifiedBy = sess.LoginId;
                        rtobj.ModifiedOn = DateTime.Now;
                        rtobj.SchoolId = sess.SchoolId;
                        rtobj.ActiveInd = "A";
                        objData.ReviewTeams.Add(rtobj);
                        objData.SaveChanges();
                        for (int i = 2; i < AdmissionList.Count(); i++)
                        {
                            if (AdmissionList[i] != "")
                            {
                                int UserId = Convert.ToInt32(AdmissionList[i]);

                                tmobj.TeamId = rtobj.TeamId;
                                tmobj.CreatedBy = sess.LoginId;
                                tmobj.CreatedOn = DateTime.Now;
                                tmobj.UserId = UserId;
                                tmobj.SchoolId = sess.SchoolId;
                                tmobj.ModifiedBy = sess.LoginId;
                                tmobj.ModifiedOn = DateTime.Now;
                                tmobj.ActiveInd = "A";
                                objData.TeamMembers.Add(tmobj);
                                objData.SaveChanges();

                            }
                        }

                        ViewData["Message"] = "<div class='valid_box'>Admission Review Team added successfully...</div>";
                    }
                    else
                    {
                        int ReviewTmId = Convert.ToInt32(AdmissionList[0]);
                        ReviewTeam RT = (from objrwteam in objData.ReviewTeams where objrwteam.TeamId == ReviewTmId select objrwteam).First();
                        RT.TeamName = AdmissionList[1];
                        RT.ActiveInd = "A";
                        RT.ModifiedBy = sess.LoginId;
                        RT.ModifiedOn = DateTime.Now;
                        objData.SaveChanges();

                        var Team = (from objtmMember in objData.TeamMembers where objtmMember.TeamId == ReviewTmId select objtmMember).ToList();
                        foreach (var item in Team)
                        {
                            var deleteTeam = objData.TeamMembers.Where(x => x.MemberId == item.MemberId).First();
                            objData.TeamMembers.Remove(deleteTeam);
                            objData.SaveChanges();
                        }
                        for (int i = 2; i < AdmissionList.Count(); i++)
                        {
                            if (AdmissionList[i] != "")
                            {
                                int UserId = Convert.ToInt32(AdmissionList[i]);

                                tmobj.TeamId = Convert.ToInt32(AdmissionList[0]);
                                tmobj.CreatedBy = sess.LoginId;
                                tmobj.CreatedOn = DateTime.Now;
                                tmobj.UserId = UserId;
                                tmobj.SchoolId = sess.SchoolId;
                                tmobj.ModifiedBy = sess.LoginId;
                                tmobj.ModifiedOn = DateTime.Now;
                                tmobj.ActiveInd = "A";
                                objData.TeamMembers.Add(tmobj);
                                objData.SaveChanges();

                            }
                        }

                        ViewData["Message"] = "<div class='valid_box'>Admission Review Team updated successfully...</div>";
                    }
                    objuser = UserModel.BindReviewTeam(sess.SchoolId);
                }

                //set Permission After save By--Neethu 20/8/14
                getCommon = new ClsCommon();
                ViewBag.permission = getCommon.setPermission();


            }
            catch (Exception Ex)
            {
                ViewData["Message"] = "<div class='error_box'>Failed..." + Ex.Message + "</div>";
            }

            return View("../Engine/AdmissionReview", objuser);
        }

        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public string FillTeam(int Id)
        {
            sess = (clsSession)Session["UserSession"];
            string Result = "";
            try
            {
                if (sess != null)
                {
                    Result = UserModel.EditReviewTeam(Id, sess.SchoolId);

                }

            }
            catch (Exception Ex)
            {
            }

            return Result;

        }

        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult DeleteReviewTeam(int Id)
        {
            sess = (clsSession)Session["UserSession"];
            UserModel objuser = new UserModel();
            try
            {
                if (sess != null)
                {
                    if (Id != 0)
                    {
                        objData = new MelmarkDBEntities();
                        var deleteTeam = objData.ReviewTeams.Where(x => x.TeamId == Id).First();
                        objData.ReviewTeams.Remove(deleteTeam);
                        objData.SaveChanges();
                    }
                }
                objuser = UserModel.BindReviewTeam(sess.SchoolId);

                //set Permission After Delete By--Neethu 20/8/14
                ClsCommon getCommon = new ClsCommon();
                ViewBag.permission = getCommon.setPermission();

            }
            catch (Exception ex)
            {
            }
            return View("../Engine/AdmissionReview", objuser);
        }
    }
}
