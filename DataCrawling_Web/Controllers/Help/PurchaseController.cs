using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DataCrawling_Web.Controllers.Help
{
    public class PurchaseController : Controller
    {
        public ActionResult History()
        {
            return View("~/Views/Help/Purchase/History.cshtml");
        }

        public ActionResult Product()
        {
            return View("~/Views/Help/Purchase/Product.cshtml");
        }
    }
}