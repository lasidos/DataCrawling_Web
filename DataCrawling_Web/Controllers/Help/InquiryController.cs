﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DataCrawling_Web.Controllers.Help
{
    public class InquiryController : Controller
    {
        public ActionResult Index()
        {
            return View("~/Views/Help/Inquiry/Index.cshtml");
        }
    }
}