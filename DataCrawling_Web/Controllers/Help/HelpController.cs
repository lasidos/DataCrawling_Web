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
    public class HelpController : Controller
    {
        [Route("Help/Notice")]
        [LoginCheck(Url = "/Help/Notice")]
        public ActionResult Notice()
        {
            return View();
        }

        [Route("Help/Event")]
        [LoginCheck(Url = "/Help/Event")]
        public ActionResult Event()
        {
            return View();
        }

        [HttpPost]
        [Route("Help/GetFaqData")]
        [LoginCheck(Url = "/Help/GetFaqData")]
        public ActionResult GetFaqData(string tab)
        {
            int n;
            if(!int.TryParse(tab, out n)) tab = "1";
            string view = string.Format("~/Views/Help/_PartialView/Help/_tab{0}.cshtml", tab);
            return PartialView(view);
        }
    }
}