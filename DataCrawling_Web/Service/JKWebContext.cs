using DataCrawling_Web.Service.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services.Description;

namespace DataCrawling_Web.Service
{
    /// <summary>
    /// 웹 전용입니다.
    /// </summary>
    public class MKWebContext
    {
        #region Service Area

        private CommonService _cmSvc;
        public CommonService CMSvc
        {
            get
            {
                if (_cmSvc == null) _cmSvc = new CommonService();
                return _cmSvc;
            }
        }

        #endregion

        #region Repository Service Area 


        #endregion

        #region Util Area

        private JKHttpSession _jkHttpSession;
        public JKHttpSession Session
        {
            get
            {
                if (_jkHttpSession == null) _jkHttpSession = new JKHttpSession();
                return _jkHttpSession;
            }
        }

        private MKHttpCookie _mkHttpCookie;
        public MKHttpCookie Cookie
        {
            get
            {
                if (_mkHttpCookie == null) _mkHttpCookie = new MKHttpCookie();
                return _mkHttpCookie;
            }
        }

        #endregion
    }

}