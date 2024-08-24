using DataCrawling_Web.BSL.Attributes;
using DataCrawling_Web.BSL.Authentication;
using DataCrawling_Web.BSL.Code;
using DataCrawling_Web.BSL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DataCrawling_Web.Controllers.Help
{
    public class FaqController : Controller
    {
        [LoginCheck(Url = "/Help/Faq")]
        public ActionResult Index()
        {
            return View("~/Views/Help/Faq/Index.cshtml");
        }
    }
}