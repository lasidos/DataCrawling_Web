using System;
using System.Web;
using System.Web.SessionState;

namespace DataCrawling_Web.BSL.Core
{
    public class SimpleSession
    {
        private HttpSessionState Session
        {
            get { return HttpContext.Current.Session; }
        }
        public Exception Exception { get; private set; }

        public SimpleSession()
        {
        }

        public bool ContainsKey(string key)
        {
            for (int index = 0; index < Session.Keys.Count; index++)
            {
                string sessionKey = Session.Keys[index];

                if (sessionKey.Equals(key, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }

        public bool IsNull(string key)
        {
            if (ContainsKey(key))
                return Session[key] == null;

            return true;
        }

        public void Set(string key, object value)
        {
            Session[key] = value;
        }

        public object Get(string key)
        {
            return Session[key];
        }

        public T Get<T>(string key)
        {
            if (ContainsKey(key))
                return (T)Session[key];

            return default(T);
        }

        public bool TryGet<T>(string key, out T value)
        {
            value = default(T);
            Exception = null;

            try
            {
                value = Get<T>(key);
            }
            catch (Exception ex)
            {
                Exception = ex;
                return false;
            }

            return true;
        }

        public void Remove(string key)
        {
            if (ContainsKey(key))
                Session.Remove(key);
        }
    }
}
