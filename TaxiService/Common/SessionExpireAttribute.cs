using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TaxiService.Common
{
    public class SessionExpireAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;
            // check  sessions here
            if (HttpContext.Current.Session["LoggedUserID"] == null)
            {
                filterContext.Result = new RedirectResult("~/UserHome/Login");
                return;
            }
            base.OnActionExecuting(filterContext);
        }
    }
}