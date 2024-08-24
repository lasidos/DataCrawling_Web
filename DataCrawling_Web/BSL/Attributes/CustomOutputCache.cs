using System;
using System.Web.Caching;
using System.Web.Mvc;

namespace DataCrawling_Web.BSL.Attributes
{
    public class CustomOutputCache : ActionFilterAttribute
    {
        private string _cachedKey;

        public int Duration { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.Url != null)
            {
                _cachedKey = string.Format("/{0}/{1}", filterContext.RouteData.Values["controller"], filterContext.RouteData.Values["action"]).ToLower();
            }

            if (_cachedKey == "/part/removecache")
            {
                filterContext.HttpContext.Cache.Remove("/part/main");
            }

            if (filterContext.HttpContext.Cache[_cachedKey] != null)
            {
                filterContext.Result = (ActionResult)filterContext.HttpContext.Cache[_cachedKey];
            }
            else
            {
                base.OnActionExecuting(filterContext);
            }
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            filterContext.HttpContext.Cache.Add(_cachedKey, filterContext.Result, null,
                DateTime.Now.AddSeconds(Duration), Cache.NoSlidingExpiration,
                CacheItemPriority.Default, null);
            base.OnActionExecuted(filterContext);
        }
    }

}