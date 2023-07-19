using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReferalDB.Models;
using BuisinessLayer;
using DataLayer;

namespace ReferalDB.Controllers
{
    public class HomeController : Controller
    {
        clsGeneral objGeneral = new clsGeneral();
        MelmarkDBEntities objData = new MelmarkDBEntities();
        StudentOld objStudent = new StudentOld();

        public ActionResult Index(StudentModel mdlStudent)
        {
            //objStudent.StudentId=mdlStudent.StudentId;

            //objData.Students.Add(objStudent);
            //objData.SaveChanges();
            return View();
        }

    }
}
