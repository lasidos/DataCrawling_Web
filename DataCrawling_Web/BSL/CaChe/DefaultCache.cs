using System;

namespace DataCrawling_Web.BSL.CaChe
{
    public class DefaultCache
    {
        private ICacheStorage _storage;

        private string _refresh;

        public object this[string key]
        {
            get
            {
                return Get(key);
            }
            set
            {
                Add(key, value);
            }
        }

        public string Refresh
        {
            get
            {
                return _refresh;
            }
            set
            {
                _refresh = value;
            }
        }

        public DefaultCache()
        {
            _refresh = "* * * * *";
            _storage = ActivateCacheStorage();
        }

        public void Add(string key, object item)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key", " no key ");
            }

            if (item == null)
            {
                throw new ArgumentNullException("item", " no item ");
            }

            _storage.Add(key, new CacheValue(item, DateTime.Now));
        }

        public void Add(string key, object item, string refresh)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key", " no key ");
            }

            if (item == null)
            {
                throw new ArgumentNullException("item", " no item ");
            }

            _storage.Add(key, new CacheValue(item, DateTime.Now, refresh));
        }

        private CacheValue Get(string key)
        {
            CacheValue cacheValue = null;
            cacheValue = _storage.Get(key);
            if (cacheValue == null)
            {
                return null;
            }

            string empty = string.Empty;
            empty = (string.IsNullOrEmpty(cacheValue.Refresh) ? _refresh : cacheValue.Refresh);
            if (cacheValue == null || ExtendedFormatHelper.IsExtendedExpired(empty, cacheValue.ItemAge, DateTime.Now))
            {
                return null;
            }

            return cacheValue;
        }

        public bool ContainsKey(string key)
        {
            return _storage.ContainsKey(key);
        }

        public void Remove(string key)
        {
            if (_storage != null)
            {
                _storage.Remove(key);
            }
        }

        public void Clear()
        {
            if (_storage != null)
            {
                _storage.Clear();
            }
        }

        protected virtual ICacheStorage ActivateCacheStorage()
        {
            return new MemoryCacheStorage();
        }
    }
}