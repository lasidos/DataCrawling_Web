using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace DataCrawling_Web.BSL.Core
{
    public class SimpleCookie
    {
        private HttpCookieCollection Cookies
        {
            get { return HttpContext.Current.Response.Cookies; }
        }
        public Exception Exception { get; private set; }

        public SimpleCookie()
        {
        }

        public bool ContainsKey(string key)
        {
            for (int index = 0; index < Cookies.Keys.Count; index++)
            {
                string sessionKey = Cookies.Keys[index];

                if (sessionKey.Equals(key, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }

        public bool IsNull(string key)
        {
            if (ContainsKey(key))
                return Cookies[key] == null;

            return true;
        }

        public void Set(string key, string value, bool HttpOnly = true)
        {
            HttpCookie cookie = new HttpCookie(key.Replace("%5", "_"), value);
            cookie.HttpOnly = HttpOnly;
            Cookies.Add(cookie);
        }

        public object Get(string key)
        {
            return Cookies[key];
        }

        public void Remove(string key)
        {
            if (ContainsKey(key))
                Cookies.Remove(key);
        }
    }
}
