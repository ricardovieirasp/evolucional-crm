using System;
using System.Runtime.Caching;
using System.Security.AccessControl;

namespace Evolucional.Matriculas.Api.Infrastructure.Cache
{
    public class MemoryCacheService : ICacheService
    {
        private readonly ObjectCache _cache;

        public MemoryCacheService()
        {
            _cache = MemoryCache.Default;
        }

        public T Get<T>(string key)
        {
            var value = _cache.Get(key);

            if (value == null)
                return default(T);

            return (T)value;
        }

        public void Set<T>(
            string key,
            T value,
            TimeSpan expiration)
        {
            var policy = new CacheItemPolicy
            {
                AbsoluteExpiration =
                    DateTimeOffset.UtcNow.Add(expiration)
            };

            _cache.Set(key, value, policy);
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }
    }
}
