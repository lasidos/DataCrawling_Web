using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace DataCrawling_Web.BSL.Common
{
    /// <summary>
    /// WebViewPage 기능을 확장합니다.
    /// <para>사용방법: Views/Web.config 설정의 'pageBaseType="System.Web.Mvc.WebViewPage"' -> pageBaseType="JobKorea.WebUI.Commons.Mvc.DefaultWebViewPage"로 변경</para>
    /// </summary>
    public abstract class DefaultWebViewPage<T> : System.Web.Mvc.WebViewPage<T>
    {
        public string ControllerName
        {
            get
            {
                return Request.RequestContext.RouteData.Values["controller"].ToString();
            }
        }

        public string ActionName
        {
            get
            {
                return Request.RequestContext.RouteData.Values["action"].ToString();
            }
        }

        private string _path;

        /// <summary>
        /// 현재 웹 경로를 반환합니다. 경로 문자열은 소문자입니다.
        /// </summary>
        public string WebPath
        {
            get
            {
                if (_path == null)
                    _path = _GetWebPath();

                return _path;
            }
        }

        private string _GetWebPath()
        {
            if (this.Request == null)
                return string.Empty;

            // 현재 페이지 경로 확인. 테스트(104)/운영 서버일 경우 앞에 '/net' 경로가 추가된다.
            string absolutePath = this.Request.Url.AbsolutePath.ToLower();
            string path = Regex.Replace(absolutePath, "^/net", "");

            // Url 끝 문자가 '/'인 경우 제거한다.
            if (path.Length > 0 && path.Last() == '/')
                path = path.Substring(0, path.Length - 1);

            return path;
        }

        protected override void InitializePage()
        {
            base.InitializePage();
        }

        public override void InitHelpers()
        {
            base.InitHelpers();
        }

        //private UiHelper _ui;
        //public UiHelper UI
        //{
        //    get
        //    {
        //        if (_ui == null) _ui = new UiHelper();
        //        return _ui;
        //    }
        //}
    }

    /// <summary>
    /// Web view page
    /// </summary>
    //public abstract class DefaultWebViewPage : DefaultWebViewPage<dynamic>
    //{
    //    private UiHelper _ui;
    //    public UiHelper UI
    //    {
    //        get
    //        {
    //            if (_ui == null) _ui = new UiHelper();
    //            return _ui;
    //        }
    //    }
    //}
}