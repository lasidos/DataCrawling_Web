using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DataCrawling_Web.Controllers.Help
{
    public class FaqController : Controller
    {
        public ActionResult Corporation()
        {
            return View("~/Views/Help/Faq/Corporation.cshtml");
        }
    }
}