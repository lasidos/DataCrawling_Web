using DataCrawling_Web.BSL.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DataCrawling_Web.Controllers.Auth
{
    public class PolicyController : Controller
    {
        public ActionResult Terms(string Type = "1")
        {
            string title = null, content = null;
            switch (Type)
            {
                case "1":
                    title = "마이플랫폼 약관";
                    content = string.Join("\n", Utility.ReadAllText(Server.MapPath("~/Resource/Text/termsofservice.txt")));
                    break;
                case "2":
                    title = "개인정보 수집 및 이용 동의";
                    content = string.Join("\n", Utility.ReadAllText(Server.MapPath("~/Resource/Text/privacy.txt")));
                    break;
            }

            ViewBag.Title = title;
            ViewBag.Content = content.Replace("\n", "<br />"); ;
            return View("~/Views/Auth/Policy/Terms.cshtml");
        }
    }
}