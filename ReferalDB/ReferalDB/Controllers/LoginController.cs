using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReferalDB.Models;
using ReferalDB;
using ReferalDB.CommonClass;
using BuisinessLayer;
using DataLayer;

namespace ReferalDB.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/
        DbFunctions oDb = null;
        clsSession objSession = null;
        clsSession1 objSession1 = null;
        MelmarkDBEntities Objdata = null;
        public ActionResult Index( LoginModel model=null)
        {
            return View("../Home/Login");
        }

        public ActionResult Login(LoginModel model)
        {
            if (model.UserName != null && model.Password != null)
            {
                oDb = new DbFunctions();
                int UserId = oDb.Login(model);
                if (UserId == 0) 
                { 
                    model.IsValid = false; 
                    model.Message = "Invalid Username or Password";
                }
                else { 
                    Session["UserID"] = UserId; 
                    SetUserSession(UserId);                     
                    return RedirectToAction("Dashboard", "Dashboard");
                    //return RedirectToAction("Dashboard_refMode", "Dashboard"); 
                }
            }
            
            return View("../Home/Login",model);


            //if (model.UserName != null && model.Password != null)
            //{
            //    oDb = new DbFunctions();
            //    int UserId = oDb.Login(model);
            //    int schoolId = ;
            //    int classId = 1;
            //    string userName = "admin";

            //    //string Values = objSession.SchoolId + "#" + objSession.LoginId + "#" + objSession.Classid + "#" + objSession.UserName;
            //    //Session["Values"] = Values;

            //    string Values = schoolId + "#" + UserId + "#" + classId + "#" + userName;
            //    Session["Values"] = Values;

            //    return RedirectToAction("Dashboard", "Dashboard");

            //    //if (UserId == 0) { model.IsValid = false; model.Message = "Invalid Username or Password"; }
            //    //else { Session["UserID"] = UserId; SetUserSession(UserId); return RedirectToAction("Dashboard", "Dashboard"); }
            //}
            //else
            //    return View("../Home/Login", model);


        }

        private void SetUserSession(int UserID)
        {
            Objdata = new MelmarkDBEntities();
            if (UserID != 0)
            {
                var Role = (from Objrole in Objdata.Roles
                            join objrgp in Objdata.RoleGroups on Objrole.RoleId equals objrgp.RoleId
                            select new
                                {
                                    RoleId = Objrole.RoleId,
                                    Roledesc = Objrole.RoleDesc,
                                    schoolid = Objrole.SchoolId,
                                    RoleCode =Objrole.RoleCode
                                }).ToList();
                var Usr = (from Objrole in Role
                           from Objusr in Objdata.Users
                           where Objusr.UserId == UserID
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

                var rolePerm=(from objRoleGroupPermission in Objdata.RoleGroupPerms
                             join objObject in Objdata.Objects on objRoleGroupPermission.ObjectId equals objObject.ObjectId
                             join objRoleGroup in Objdata.RoleGroups on objRoleGroupPermission.RoleGroupId equals objRoleGroup.RoleGroupId
                             join objRole in Objdata.Roles on objRoleGroup.RoleId equals objRole.RoleId
                             join objUserRoleGroup in Objdata.UserRoleGroups on objRoleGroup.RoleGroupId equals objUserRoleGroup.RoleGroupId
                             where objObject.ObjectName == "Referal" && objUserRoleGroup.UserId == UserID && objUserRoleGroup.ActiveInd=="A"
                             select new
                                {
                                    ApproveInd = objRoleGroupPermission.ApproveInd

                                }).ToList();

                if (Usr == null) return;
                if (Usr.Count() > 0)
                {

                    objSession = new clsSession();
                    objSession.IsLogin = true;
                    objSession.LoginTime = (DateTime.Now.ToShortTimeString()).ToString();
                    objSession.SchoolId = Convert.ToInt32(Usr[0].SchoolId);
                    objSession.LoginId = Convert.ToInt32(Usr[0].UserId);
                    objSession.UserName = Convert.ToString(Usr[0].UserLName + "," + Usr[0].UserFName);
                    objSession.RoleId = Convert.ToInt32(Usr[0].RoleId);
                    objSession.Gender = Convert.ToString(Usr[0].Gender);
                    objSession.RoleName = Convert.ToString(Usr[0].Roledesc);
                    objSession.SessionID = Session.SessionID.ToString();
                    objSession.ReferralId = 0;
                    objSession.RoleCode = Convert.ToString(Usr[0].RoleCode);
                    if (rolePerm.Count > 0)
                    {
                        if (rolePerm[0].ApproveInd == true)
                            objSession.IsApproved = "true";
                        else
                            objSession.IsApproved = "false";
                    }
                    else
                        objSession.IsApproved = "false";


                    // ObjSession1 filling

                    objSession1 = new clsSession1();
                    objSession1.AddressId = 0;
                    objSession1.StudentId = objSession.ReferralId;
                    objSession1.LoginId = objSession.LoginId;
                    objSession1.SchoolId = objSession.SchoolId;

                    Session["UserSession"] = objSession;
                    Session["UserSession1"] = objSession1;
                }
            }
            else
            {
                Session["UserSession"] = null;
            }
        }

        public ActionResult Logout()
        {
            Session["UserSession"] = null;
            Session.RemoveAll();
            Session.Abandon(); 
            return View("../Dashboard/LogoutView");
        }

        //public bool SessionExist()
        //{
        //    objSession = (clsSession)Session["UserSession"];
        //    bool Result = true;
        //    if (objSession == null)
        //    {
        //        Result = false;
        //    }
        //    return Result;
        //}

    }
}
