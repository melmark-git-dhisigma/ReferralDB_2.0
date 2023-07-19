using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using BuisinessLayer;

namespace ReferalDB
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
        void Session_Start(object sender, EventArgs e)
        {
            Session.Timeout = 300000;
        }

        void Session_End(object sender, EventArgs e)
        {
            Session.Abandon();
        }
        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exc = Server.GetLastError();           
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageURL() + "\t" + exc.Message.ToString() + "\t" + exc.InnerException);
            Server.ClearError();
        }
    }
}