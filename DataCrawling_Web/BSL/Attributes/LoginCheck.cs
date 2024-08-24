using DataCrawling_Web.BSL.Authentication;
using DataCrawling_Web.BSL.Code;
using DataCrawling_Web.BSL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DataCrawling_Web.BSL.Attributes
{
    public class LoginCheck : ActionFilterAttribute
    {
        public string Url { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (W_Menu.GetMenuIdx(Url).Login_Stat == 1 && string.IsNullOrEmpty(AuthUser.M_ID))
            {
                var script = JSBuilder.ConfirmMoveCancel(@"로그인이 필요한 서비스입니다..\n로그인페이지로 이동하시겠습니까?",
                        "/Auth/Login/Nid_Login", "/");
                filterContext.Result = new ContentResult
                {
                    Content = script,
                    ContentType = "text/html"
                };
            }
            base.OnActionExecuting(filterContext);
        }
    }
}