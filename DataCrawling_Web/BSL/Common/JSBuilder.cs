using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace DataCrawling_Web.BSL.Common
{
    public static class JSBuilder
    {
        public static String StaticStr(String script, bool noRobots)
        {

            StringBuilder _sb = new StringBuilder();
            if (noRobots)
            {
                _sb.Append("<html><head><meta name=\"Robots\" content=\"noindex,nofollow\" /><head><body>");
            }
            else
            {
                _sb.Append("<html><body>");
            }
            _sb.AppendLine("<meta charset=\"UTF-8\">");
            _sb.Append("<script type=\"text/javascript\">");
            _sb.Append(script);
            _sb.Append("</script>");
            _sb.Append("</body></html>");

            return _sb.ToString();
        }

        /// <summary>
        /// 스크립트 영역
        /// </summary>
        /// <param name="script"></param>
        /// <returns></returns>
        public static string StaticScriptsStr(string script)
        {

            StringBuilder _sb = new StringBuilder();
            _sb.AppendLine("<script type=\"text/javascript\" >");
            _sb.AppendLine("<!--");
            _sb.AppendLine(script);
            _sb.AppendLine("//-->");
            _sb.AppendLine("</script>");
            return _sb.ToString();
        }

        /// <summary>
        /// html 및 스크립트 영역
        /// </summary>
        /// <param name="script"></param>
        /// <returns></returns>
        public static string StaticStr(string script)
        {

            StringBuilder _sb = new StringBuilder();
            _sb.AppendLine("<html><body>");
            _sb.AppendLine("<meta charset=\"UTF-8\">");
            _sb.AppendLine("<script type=\"text/javascript\">");
            _sb.AppendLine("<!--");
            _sb.AppendLine(script);
            _sb.AppendLine("//-->");
            _sb.AppendLine("</script>");
            _sb.AppendLine("</body></html>");

            return _sb.ToString();
        }

        /// <summary>
        /// history.back
        /// </summary>
        /// <param name="alert">빈값이면 얼럿 안띄움</param>
        /// <returns></returns>
        public static string HistoryBack(string alert = "")
        {
            return string.IsNullOrWhiteSpace(alert) ? StaticStr(string.Format("history.back();", alert)) : StaticStr(string.Format("alert('{0}');history.back();", alert));
        }

        /// <summary>
        /// top.location.href
        /// alert 있으면 띄움 
        /// </summary>
        /// <param name="href"></param>
        /// <param name="alert"></param>
        /// <returns></returns>
        public static string TopLocation(string href, string alert = "")
        {
            return string.IsNullOrWhiteSpace(alert) ? StaticStr(string.Format("top.location.href='{0}';", href)) : StaticStr(string.Format("alert('{0}');top.location.href='{1}';", alert, href));
        }

        /// <summary>
        /// 얼럿만 띄움
        /// </summary>
        /// <param name="alert"></param>
        /// <returns></returns>
        public static string MsgAlert(string alert)
        {
            return StaticStr(string.Format("alert('{0}');", alert));
        }

        /// <summary>
        /// 얼럿띄우고 location.href
        /// </summary>
        /// <param name="alert"></param>
        /// <param name="href"></param>
        /// <returns></returns>
        public static string MsgUrlAlert(string alert, string href, bool noRobots = false)
        {
            return StaticStr(string.Format("alert('{0}');location.href='{1}';", alert, href), noRobots);
        }

        /// <summary>
        /// opener.location.href 이동하고 this.close()
        /// </summary>
        /// <param name="href"></param>
        /// <returns></returns>
        public static string OpenerLocation(string href)
        {
            return StaticStr(string.Format("opener.location.href='{0}';this.close();", href));
        }

        /// <summary>
        /// 얼럿 띄우고 window.close()
        /// </summary>
        /// <param name="alert"></param>
        /// <returns></returns>
        public static string PupTopClose(string alert)
        {
            return StaticStr(string.Format("alert('{0}');window.close();", alert));
        }

        /// <summary>
        /// location.href
        /// </summary>
        /// <param name="href"></param>
        /// <returns></returns>
        public static string GoLocation(string href)
        {
            return StaticStr(string.Format("location.href='{0}';", href));
        }

        /// <summary>
        /// location.href
        /// </summary>
        /// <param name="href"></param>
        /// <returns></returns>
        public static string GoReplace(string href)
        {
            return StaticStr(string.Format("window.location.replace('{0}');", href));
        }

        /// <summary>
        /// 얼럿 띄우고 parent.location.reload()
        /// </summary>
        /// <param name="alert"></param>
        /// <returns></returns>
        public static string MsgParentReload(string alert)
        {
            return StaticStr(string.Format("alert('{0}');parent.location.reload();", alert));
        }

        /// <summary>
        /// 얼럿 있으면 띄우고 parent.location.href
        /// </summary>
        /// <param name="href"></param>
        /// <param name="alert"></param>
        /// <returns></returns>
        public static string MsgParentLocation(string href, string alert = "")
        {
            return string.IsNullOrWhiteSpace(alert) ? StaticStr(string.Format("parent.location.href='{0}';", href)) : StaticStr(string.Format("alert('{0}');parent.location.href='{1}';", alert, href));
        }

        /// <summary>
        /// 얼럿 띄우고 opener.location.reload(); 하고 window.close()
        /// </summary>
        /// <param name="alert"></param>
        /// <returns></returns>
        public static string MsgOpenerReload(string alert)
        {
            return StaticStr(string.Format("alert('{0}');opener.location.reload();window.close();", alert));
        }

        /// <summary>
        /// 얼럿 띄우고 opener.location 이동하고 window.close();
        /// </summary>
        /// <param name="alert"></param>
        /// <param name="href"></param>
        /// <returns></returns>
        public static string MsgOpenerLocation(string alert, string href)
        {
            return StaticStr(string.Format("alert('{0}');opener.location ='{1}';window.close();", alert, href));
        }

        /// <summary>
        /// opener.location.reload(); window.close();
        /// </summary>
        /// <returns></returns>
        public static string OpenerReload()
        {
            return StaticStr("opener.location.reload();window.close();");
        }

        /// <summary>
        /// window.open('{0}'); setTimeout(window.close, 10);
        /// </summary>
        /// <param name="href"></param>
        /// <returns></returns>
        public static string PupCloseOpen(string href)
        {
            return StaticStr(string.Format("window.open('{0}'); setTimeout(window.close, 10);", href));
        }

        /// <summary>
        /// if(confirm('{0}')) history.back(); else top.location.href='{1}';
        /// </summary>
        /// <param name="alert"></param>
        /// <param name="href"></param>
        /// <returns></returns>
        public static string MsgUrlConfirm(string alert, string href)
        {
            return StaticStr(string.Format("if(confirm('{0}')) history.back(); else top.location.href='{1}';", alert, href));
        }

        public static string ConfirmMove(string alert = "", string moveUrl = "")
        {
            return StaticStr("if(confirm('" + alert + "')) { location.href='" + moveUrl + "'; } else { history.back(); }");
        }

        public static string ConfirmMoveCancel(string alert = "", string moveUrl = "", string cancelUrl = "")
        {
            return StaticStr("if(confirm('" + alert + "')) { location.href='" + moveUrl + "'; } else { location.href='" + cancelUrl + "'; }");
        }

        /// <summary>
        /// alert('{0}');self.close();
        /// </summary>
        /// <param name="alert"></param>
        /// <returns></returns>
        public static string MsgAlertClose(string alert)
        {
            return StaticStr(string.Format("alert('{0}');self.close();", alert));
        }

        /// <summary>
        /// alert('{0}');toapp://closeSub?closeLevel=self;
        /// </summary>
        /// <param name="alert"></param>
        /// <returns></returns>
        public static string MsgAlertCloseApp(string alert)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("alert('{0}');", alert));
            sb.Append("var iframe = document.createElement('iframe');");
            sb.Append("document.documentElement.appendChild(iframe);");
            sb.Append("iframe.style.display = 'none';");
            sb.Append("iframe.setAttribute('src', 'toapp://closeSub?closeLevel=self');");
            sb.Append("setTimeout(function () {");
            sb.Append("   iframe.parentNode.removeChild(iframe);");
            sb.Append("   iframe = null;");
            sb.Append("}, 100);");
            return StaticStr(sb.ToString());
        }
        /// <summary>
        /// alert('{0}');self.close();
        /// </summary>
        /// <param name="alert"></param>
        /// <returns></returns>
        public static string PopClose()
        {
            return StaticStr(string.Format("self.close();"));
        }

        public static string OvertureScript
        {
            get
            {
                StringBuilder _sb = new StringBuilder();
                _sb.AppendLine("window.ysm_customData = new Object();");
                _sb.AppendLine("window.ysm_customData.conversion = \"transId=,currency=,amount=\";");
                _sb.AppendLine("var ysm_accountid  = \"12QNPBF6IQGJEV16AFELFBNRQ2G\";");
                _sb.AppendLine("document.write(\"<SCR\" + \"IPT language='JavaScript' type='text/javascript' \"");
                _sb.AppendLine("+ \"SRC=//\" + \"srv1.wa.marketingsolutions.yahoo.com\" + \"/script/ScriptServlet\" + \"?aid=\" + ysm_accountid + \"></SCR\" + \"IPT>\");");

                return StaticScriptsStr(_sb.ToString());
            }
        }


        /// <summary>
        /// alert('{0}');toapp://closeSub?closeLevel=self;
        /// </summary>
        /// <param name="alert"></param>
        /// <returns></returns>
        public static string GoMainHome(string alert, string link)
        {
            string HomeUrl = "http://" + HttpContext.Current.Request.Url.Host.ToLower();

            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("{0};", alert));
            sb.Append("var iframe = document.createElement('iframe');");
            sb.Append("document.documentElement.appendChild(iframe);");
            sb.Append("iframe.style.display = 'none';");
            sb.Append("iframe.setAttribute('src', 'toapp://goMain?url=" + HomeUrl + "');");
            sb.Append("setTimeout(function () {");
            sb.Append("   iframe.parentNode.removeChild(iframe);");
            sb.Append("   iframe = null;");
            sb.Append("}, 100);");

            if (!string.IsNullOrEmpty(link))
            {
                sb.Append("var iframe2 = document.createElement('iframe');");
                sb.Append("document.documentElement.appendChild(iframe2);");
                sb.Append("iframe2.style.display = 'none';");
                sb.Append("iframe2.setAttribute('src', '" + link + "');");
                sb.Append("setTimeout(function () {");
                sb.Append("   iframe2.parentNode.removeChild(iframe2);");
                sb.Append("   iframe2 = null;");
                sb.Append("}, 100);");
            }

            return StaticStr(sb.ToString());
        }

        public static string GoUrlClose(string url)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("var iframe = document.createElement('iframe');");
            sb.Append("document.documentElement.appendChild(iframe);");
            sb.Append("iframe.style.display = 'none';");
            sb.Append("iframe.setAttribute('src', 'toapp://" + url + "');");
            sb.Append("setTimeout(function () {");
            sb.Append("   iframe.parentNode.removeChild(iframe);");
            sb.Append("   iframe = null;");
            sb.Append("}, 100);");

            sb.Append("var iframe2 = document.createElement('iframe');");
            sb.Append("document.documentElement.appendChild(iframe2);");
            sb.Append("iframe2.style.display = 'none';");
            sb.Append("iframe2.setAttribute('src', 'toapp://closeSub?closeLevel=self');");
            sb.Append("setTimeout(function () {");
            sb.Append("   iframe2.parentNode.removeChild(iframe2);");
            sb.Append("   iframe2 = null;");
            sb.Append("}, 100);");

            return StaticStr(sb.ToString());
        }

        public static string CloseGoUrl(string url)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("var iframe2 = document.createElement('iframe');");
            sb.Append("document.documentElement.appendChild(iframe2);");
            sb.Append("iframe2.style.display = 'none';");
            sb.Append("iframe2.setAttribute('src', 'toapp://closeSub?closeLevel=self');");
            sb.Append("setTimeout(function () {");
            sb.Append("   iframe2.parentNode.removeChild(iframe2);");
            sb.Append("   iframe2 = null;");
            sb.Append("}, 100);");

            sb.Append("var iframe = document.createElement('iframe');");
            sb.Append("document.documentElement.appendChild(iframe);");
            sb.Append("iframe.style.display = 'none';");
            sb.Append("iframe.setAttribute('src', 'toapp://" + url + "');");
            sb.Append("setTimeout(function () {");
            sb.Append("   iframe.parentNode.removeChild(iframe);");
            sb.Append("   iframe = null;");
            sb.Append("}, 100);");

            return StaticStr(sb.ToString());
        }

        public static string ToAppWithAlert(string url, string alert)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(string.Format("alert('{0}');", alert));
            sb.Append("var iframe = document.createElement('iframe');");
            sb.Append("document.documentElement.appendChild(iframe);");
            sb.Append("iframe.style.display = 'none';");
            sb.Append("iframe.setAttribute('src', 'toapp://" + url + "');");
            sb.Append("setTimeout(function () {");
            sb.Append("   iframe.parentNode.removeChild(iframe);");
            sb.Append("   iframe = null;");
            sb.Append("}, 100);");

            return StaticStr(sb.ToString());
        }

        public static string CloseGoMain()
        {
            string HomeUrl = "http://" + HttpContext.Current.Request.Url.Host.ToLower();

            StringBuilder sb = new StringBuilder();

            sb.Append("var iframe = document.createElement('iframe');");
            sb.Append("document.documentElement.appendChild(iframe);");
            sb.Append("iframe.style.display = 'none';");
            sb.Append("iframe.setAttribute('src', 'toapp://closeSub?closeLevel=all');");
            sb.Append("setTimeout(function () {");
            sb.Append("   iframe.parentNode.removeChild(iframe);");
            sb.Append("   iframe = null;");
            sb.Append("}, 100);");

            sb.Append("var iframe2 = document.createElement('iframe');");
            sb.Append("document.documentElement.appendChild(iframe2);");
            sb.Append("iframe2.style.display = 'none';");
            sb.Append("iframe.setAttribute('src', 'toapp://goMain?url=" + HomeUrl + "');");
            sb.Append("setTimeout(function () {");
            sb.Append("   iframe2.parentNode.removeChild(iframe2);");
            sb.Append("   iframe2 = null;");
            sb.Append("}, 100);");


            return StaticStr(sb.ToString());
        }
    }
}