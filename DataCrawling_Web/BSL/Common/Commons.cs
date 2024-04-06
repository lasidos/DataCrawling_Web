using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataCrawling_Web.BSL.Common
{
    public class Commons
    {
        public static string AlertMessage(string msg, string url = "")
        {
            if (string.IsNullOrEmpty(url)) url = "/Home/Index";
            return string.Format("<script language='javascript' type='text/javascript'>alert('{0}');window.location.href='{1}';</script>", msg, url);
        }
    }
}