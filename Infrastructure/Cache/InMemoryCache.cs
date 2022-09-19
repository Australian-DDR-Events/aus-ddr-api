using System;
using Application.Core.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Cache
{
    public class InMemoryCache : ICache
    {
        private readonly MemoryCache _cache;
        
        public InMemoryCache()
        {
            _cache = new MemoryCache(new MemoryCacheOptions());
        }
        
        public void Add(string key, object value)
        {
            _cache.Set(key, value);
        }

        public void Add(string key, object value, DateTime expiration)
        {
            _cache.Set(key, value, expiration);
        }

        public bool Contains(string key)
        {
            return _cache.TryGetValue(key, out _);
        }

        public object Fetch(string key)
        {
            _cache.TryGetValue(key, out var value);
            return value;
        }

        public T Fetch<T>(string key) where T : class
        {
            _cache.TryGetValue(key, out var value);
            return value as T;
        }

        public void Delete(string key)
        {
            _cache.Remove(key);
        }
    }
}