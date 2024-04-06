using DataCrawling_Web.BSL.Authentication;
using DataCrawling_Web.DSL.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataCrawling_Web.Service
{
    public class Ctx
    {
        public static CommonService CMSvc;
        public static GGRpSvc RpSvc;

        public Ctx()
        {
            CMSvc = new CommonService();
            RpSvc = new GGRpSvc();
        }
    }
}