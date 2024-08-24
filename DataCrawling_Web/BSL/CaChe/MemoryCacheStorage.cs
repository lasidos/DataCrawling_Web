using System.Collections;

namespace DataCrawling_Web.BSL.CaChe
{
    public class MemoryCacheStorage : ICacheStorage
    {
        private Hashtable items = new Hashtable();

        public void Add(string key, CacheValue item)
        {
            lock (items.SyncRoot)
            {
                items[key] = item;
            }
        }

        public CacheValue Get(string key)
        {
            return (CacheValue)items[key];
        }

        public bool ContainsKey(string key)
        {
            return items.ContainsKey(key);
        }

        public void Clear()
        {
            lock (items.SyncRoot)
            {
                items.Clear();
            }
        }

        public void Remove(string key)
        {
            if (items.ContainsKey(key))
            {
                lock (items.SyncRoot)
                {
                    items.Remove(key);
                }
            }
        }
    }
}