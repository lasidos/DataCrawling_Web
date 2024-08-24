using DataCrawling_Web.BSL.Admin;
using DataCrawling_Web.DSL.Account;
using DataCrawling_Web.DSL.Admin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace DataCrawling_Web.Controllers.Admin
{
    enum ContentCode
    {
        Main = 1,
        Main_Help = 2
    }

    public class MainController : Controller
    {
        public ActionResult Index()
        {
            return Redirect("/Admin/Main/item");
        }

        public ActionResult item()
        {
            var vm = new Contents().GetContents().OrderByDescending(s => s.E_DATE);
            Session["ContentInfoModel"] = JsonConvert.SerializeObject(vm);

            return View("~/Views/Admin/Main/item.cshtml", vm);
        }

        public ActionResult service()
        {
            return View("~/Views/Admin/Main/service.cshtml");
        }
    }
}