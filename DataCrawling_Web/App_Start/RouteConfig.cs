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
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
                name: "Auth",
                url: "Auth/{controller}/{action}/{id}",
                defaults: new { controller = "Join", action = "Regist", id = UrlParameter.Optional },
                namespaces: new[] { "DataCrawling_Web.Controllers.Auth" }
            );

            routes.MapRoute(
                name: "Help",
                url: "Help/{controller}/{action}/{id}",
                defaults: new { controller = "Help", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "DataCrawling_Web.Controllers.Help" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );            
        }
    }
}
