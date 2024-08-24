using System;

namespace DataCrawling_Web.BSL.CaChe
{
    public class CacheValue
    {
        private string _refresh = string.Empty;

        private object _value;

        private DateTime _itemAge;

        public object Value => _value;

        public DateTime ItemAge => _itemAge;

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

        public CacheValue(object value, DateTime itemAge)
        {
            _value = value;
            _itemAge = itemAge;
        }

        public CacheValue(object value, DateTime itemAge, string refresh)
        {
            _value = value;
            _itemAge = itemAge;
            _refresh = refresh;
        }
    }
}