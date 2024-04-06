using DataCrawling_Web.BSL.Authentication;
using DataCrawling_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DataCrawling_Web.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            string id = AuthUser.M_ID;
            //Session["asdasdasd"] = "asdasd";
            //Session["asdasda"] = "asdasd";
            //string asd = MKCtx.Cookie.M_ID;
            UserInfo userInfo = new UserInfo();
            //if (AuthUser.M_ID != "")
            //{

            //    userInfo = new UserInfo()
            //    {
            //        User_ID = "lasidos",
            //        MemberType = "M",
            //        User_Name = "유지혁"
            //    };
            //}

            return View(userInfo);
        }
    }
}