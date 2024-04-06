using DataCrawling_Web.BSL.Extentions;
using System;
using System.Linq;
using System.Web.Mvc;

namespace DataCrawling_Web.Service.Util
{
    /// <summary>
    /// 잘못된 ReturnURL일경우 메인으로 이동 이외의경우는 기존 프로세스 유지
    /// 1순위 라우팅 규칙, QueryString 1순위, 2순위 Form 2가지 Reqest만 검사한다
    /// Request["KEY"] OR Request.Param 로 검색했을경우 Querystring, Form, ServerVariables, Cookies 순서로 검색하여 의도치 않는값에에서 매치 될수 있어 제외하였다.
    /// </summary>    
    public class ReturnUrlValid : ActionFilterAttribute
    {
        private string ParamName = "";

        private string[] whiteListDomain = { "jobkorea.co.kr", "albamon.com" };

        public ReturnUrlValid(string paramName)
        {
            ParamName = paramName;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var request = filterContext.RequestContext.HttpContext.Request;
            UrlHelper urlHelper = new UrlHelper(filterContext.RequestContext);


            string url = null;
            //URL?Rtn   : false
            //URL?Rtn=  : true

            //라우팅규칙에 있을경우
            /*
            vsar route = filterContext.RequestContext.RouteData.Route.GetRouteData(filterContext.RequestContext.HttpContext).Values;

            if(route.ContainsKey(ParamName))
            {
                url = route[ParamName].ToString();
            }
            else
            {
                var hasQueryStringKey = request.QueryString.AllKeys.IsAny(d => d == ParamName);
                if (hasQueryStringKey)
                {
                    url = request.QueryString[ParamName];
                }
                else
                {
                    url = request.Form[ParamName];
                }
            }
            */
            //Querystring, Form으로 변경
            var hasQueryStringKey = request.QueryString.AllKeys.IsAny(d => d == ParamName);
            if (hasQueryStringKey)
            {
                url = request.QueryString[ParamName];
            }
            else
            {
                url = request.Form[ParamName];
            }

            //검증 최소조건
            if (ParamName.IsNotEmpty() && !string.IsNullOrWhiteSpace(url))
            {
                //http, https로 시작하는지 확인
                if (url.IsValidHttpUrl())
                {
                    var isContain = whiteListDomain.Any(d => url.Contains(d, StringComparison.CurrentCultureIgnoreCase));
                    //화이트 리스트 도메인이 포함 안되어 있으면
                    if (!isContain)
                    {
                        filterContext.Result = new RedirectResult("https://jobkorea.co.kr");
                    }
                }
                else
                {
                    // 'returnURL=%2Ftest%2Ftest 와 같이 절대 경로 가 아닌 이상한 경로일경우
                    if (!urlHelper.IsLocalUrl(url))
                    {
                        filterContext.Result = new RedirectResult("https://jobkorea.co.kr");
                    }
                }
            }
            base.OnActionExecuting(filterContext);
        }

    }

}
