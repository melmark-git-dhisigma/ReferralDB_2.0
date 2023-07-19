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

namespace ReferalDB.CommonClass
{
    
    using System;
    using System.Web.Routing;
    [AttributeUsage(AttributeTargets.All)]
    public class clsSessionActive : System.Attribute
    {

        clsSession objSession = null;
        private bool result=true;
        public clsSessionActive()  
        {
            objSession = (clsSession)HttpContext.Current.Session["UserSession"];            
            if (objSession == null)
            {
                //result = false;
                //var context = new RequestContext(new HttpContextWrapper(System.Web.HttpContext.Current),new RouteData());
                //var urlHelper = new UrlHelper(context);
                //var url = urlHelper.Action("Index", new { OtherParm = "other value" });
                System.Web.HttpContext.Current.Response.Redirect("../../Login.aspx");
                
            }
            
        }

        public bool Result
        {
            get
            {
                return result;
            }
            set
            {
                result = value;
            }
        }

        
    }



}