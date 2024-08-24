using DataCrawling_Web.BSL.CaChe;
using System;

namespace DataCrawling_Web.BSL.Code
{
    public static class ExtensionMethod
    {
        public static void AddEx(this DefaultCache defaultCache, string key, object item)
        {
            if (defaultCache.ContainsKey(key))
            {
                defaultCache[key] = item;
            }
            else
            {
                defaultCache.Add(key, item);
            }
        }

        public static void AddEx(this DefaultCache defaultCache, string key, object item, string refrash)
        {
            if (defaultCache.ContainsKey(key))
            {
                defaultCache[key] = item;
            }
            else
            {
                defaultCache.Add(key, item, refrash);
            }
        }

        [Obsolete("해당 메서드는 에러 발생 여지가 있으니, TryGetValue를 활용 바랍니다.")]
        public static T Get<T>(this DefaultCache defaultCache, string key)
        {
            if (!defaultCache.ContainsKey(key))
            {
                throw new ArgumentException();
            }

            return (T)(((CacheValue)defaultCache[key]).Value);
        }

        public static bool ContainsKeyEx(this DefaultCache defaultCache, string key)
        {
            return defaultCache.ContainsKey(key) && defaultCache[key] != null;
        }

        public static bool TryGetValue<T>(this DefaultCache defaultCache, string key, out T value)
        {
            bool isSucess = false;
            value = default(T);

            try
            {
                T cacheValue = (T)((CacheValue)defaultCache[key]).Value;
                if (cacheValue != null)
                {
                    value = cacheValue;
                }

                isSucess = true;
            }
            catch (System.Exception)
            {
                isSucess = false;
            }

            return isSucess;
        }
    }
}