using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReferalDB.Controllers
{
    public class ActiveSession : ActionFilterAttribute
    {
        clsSession objSession = null;
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            objSession = (clsSession)HttpContext.Current.Session["UserSession"];
            if (objSession == null)
            {
                filterContext.HttpContext.Response.Redirect("/Home/index", true);
            }
        }
    }
}