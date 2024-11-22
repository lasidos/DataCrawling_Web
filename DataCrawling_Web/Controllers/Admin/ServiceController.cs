using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DataCrawling_Web.Controllers.Admin
{
    public class ServiceController : Controller
    {
        // GET: Service
        public ActionResult API()
        {
            return View("~/Views/Admin/Service/API.cshtml");
        }

        public ActionResult Automation()
        {
            return View("~/Views/Admin/Service/Automation.cshtml");
        }
    }
}