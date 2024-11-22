using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DataCrawling_Web.Controllers.Admin
{
    public class APIController : Controller
    {
        // GET: API
        public ActionResult API()
        {
            return View("~/Views/Admin/API/API.cshtml");
        }

        public ActionResult Automation()
        {
            return View();
        }
    }
}