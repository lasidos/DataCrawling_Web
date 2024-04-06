using DataCrawling_Web.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace DataCrawling_Web.Controllers
{
    public class BaseController : Controller
    {
        #region [ MKCtx ]

        private MKWebContext _mkCtx;
        protected MKWebContext MKCtx
        {
            get
            {
                if (_mkCtx == null) _mkCtx = new MKWebContext();
                return _mkCtx;
            }
        }

        #endregion
    }
}