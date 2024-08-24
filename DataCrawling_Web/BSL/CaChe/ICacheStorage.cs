namespace DataCrawling_Web.BSL.CaChe
{
    public interface ICacheStorage
    {
        void Add(string key, CacheValue item);

        CacheValue Get(string key);

        bool ContainsKey(string key);

        void Clear();

        void Remove(string key);
    }
}