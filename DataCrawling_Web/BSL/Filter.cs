using DataCrawling_Web.BSL.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DataCrawling_Web.BSL
{
    public class LoginCheckFilter : ActionFilterAttribute

    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (string.IsNullOrEmpty(AuthUser.M_ID))
            {
                filterContext.Result = new RedirectResult("/Auth/Login/Nid_Login");
            }
            base.OnActionExecuting(filterContext);
        }
    }
}