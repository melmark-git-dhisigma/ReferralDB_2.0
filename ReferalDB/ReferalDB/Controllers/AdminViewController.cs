using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReferalDB.Models;
using BuisinessLayer;

namespace ReferalDB.Controllers
{
    public class AdminViewController : Controller
    {
        //
        // GET: /AdminView/

        public clsSession sess = null;

        //[ActiveSession]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult AdminView()
        {
            sess = (clsSession)Session["UserSession"];
            EngineViewModels returnModel = new EngineViewModels();
            if (sess != null)
            {               
                returnModel = EngineViewModels.BindLetterEngine(sess.SchoolId);
            }
            return View("AdminView", returnModel);            
        }

    }
}
