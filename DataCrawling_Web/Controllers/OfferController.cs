using DataCrawling_Web.BSL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;

namespace DataCrawling_Web.Controllers
{
    public class OfferController : Controller
    {
        // GET: RequestLetter
        public ActionResult List(string menu)
        {
            string subject = "";
            switch (menu)
            {
                case "scrap":
                    subject = "스크랩핑";
                    break;
                case "prs":
                    subject = "웹·업무 자동화";
                    break;
            }
            if (subject == "") return Content(Commons.AlertMessage("잘못된 접근입니다."));

            ViewBag.Subject = subject;
            return View();
        }

        public ActionResult Terms()
        {
            return View();
        }

        public new ActionResult Request()
        {
            if (System.Web.HttpContext.Current.Session["Term"] == null)
            {
                return Redirect("/Offer/List");
            }
            return View();
        }

        [HttpPost]
        public string functionname(string Term)
        {
            System.Web.HttpContext.Current.Session["Term"] = Term;
            return "ok";
        }
    }
}