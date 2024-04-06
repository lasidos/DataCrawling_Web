using DataCrawling_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DataCrawling_Web.Controllers
{
    public class DataController : Controller
    {
        // GET: API
        public ActionResult ApiList(string PubType)
        {
            UserInfo userInfo = new UserInfo()
            {
                User_ID = "lasidos",
                MemberType = "M",
                User_Name = "유지혁"
            };
            return View();
        }

        public ActionResult Info(string A_No)
        {
            ViewBag.A_No = A_No;
            return View();
        }
    }
}