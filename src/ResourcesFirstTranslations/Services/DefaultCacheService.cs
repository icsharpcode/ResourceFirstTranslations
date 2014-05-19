using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace ResourcesFirstTranslations.Services
{
    public class DefaultCacheService : ICacheService
    {
        private const int DefaultTimeoutInMinutes = 60;

        public DefaultCacheService()
        {
            IsCacheEnabled = true;
            InitializeCache();
        }

        // Needed because the translation service (and others) will use it with ConfigureAwait(false)
        private void InitializeCache()
        {
            _cache = HttpContext.Current.Cache;
        }
        private Cache _cache;

        protected Cache GetDefaultCache()
        {
            return _cache;
        }

        public bool IsCacheEnabled { get; set; }

        public T Get<T>(string cacheKey, Func<T> loaderFunction)
        {
            if (!IsCacheEnabled)
            {
                return loaderFunction();
            }

            var cache = GetDefaultCache();
            object item = cache.Get(cacheKey);

            if (null == item)
            {
                T itemToCache = loaderFunction();

                cache.Add(cacheKey, itemToCache, null, Cache.NoAbsoluteExpiration, 
                    TimeSpan.FromMinutes(DefaultTimeoutInMinutes), CacheItemPriority.Default, null);

                return itemToCache;
            }

            return (T)item;
        }

        public async Task<T> GetAsync<T>(string cacheKey, Func<Task<T>> loaderFunction)
        {
            if (!IsCacheEnabled)
            {
                return await loaderFunction().ConfigureAwait(false);
            }

            var cache = GetDefaultCache();
            object item = cache.Get(cacheKey);

            if (null == item)
            {
                T itemToCache = await loaderFunction().ConfigureAwait(false);

                cache.Add(cacheKey, itemToCache, null, Cache.NoAbsoluteExpiration,
                    TimeSpan.FromMinutes(DefaultTimeoutInMinutes), CacheItemPriority.Default, null);

                return itemToCache;
            }

            return (T)item;
        }

        public void Invalidate(string cacheKey)
        {
            if (!IsCacheEnabled) return;

            var cache = GetDefaultCache();
            cache.Remove(cacheKey);
        }
    }
}