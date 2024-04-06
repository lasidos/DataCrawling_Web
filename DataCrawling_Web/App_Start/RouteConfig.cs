using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DataCrawling_Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            // 고객센터 > 유료이용내역
            routes.IgnoreRoute("Help/Purchase/History");
            routes.MapRoute(
                name: null,
                url: "Help/Purchase/{action}",
                defaults: new { controller = "Purchase", action = "History", ns = "Help" },
                namespaces: new[] { "DataCrawling_Web.Controllers.Help" }
            );

            // 고객센터 > FAQ
            routes.MapRoute(
                name: null,
                url: "Help/faq/{action}",
                defaults: new { controller = "Faq", ns = "Help" },
                namespaces: new[] { "DataCrawling_Web.Controllers.Help" }
            );

            // 고객센터 > 1:1 문의하기
            routes.IgnoreRoute("Help/inquiry/index");
            routes.MapRoute(
                name: null,
                url: "Help/inquiry/{action}",
                defaults: new { controller = "Inquiry", action = "Index", ns = "Help" },
                namespaces: new[] { "DataCrawling_Web.Controllers.Help" }
            );

            // 고객센터 > 공지사항
            routes.IgnoreRoute("Help/notice/index");
            routes.MapRoute(
                name: null,
                url: "Help/notice/{action}",
                defaults: new { controller = "Notice", action = "Index", ns = "Help" },
                namespaces: new[] { "DataCrawling_Web.Controllers.Help" }
            );

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
