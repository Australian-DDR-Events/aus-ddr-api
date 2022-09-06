using System;

namespace Application.Core.Interfaces
{
    public interface ICache
    {
        public void Add(string key, object value);
        public void Add(string key, object value, DateTime expiration);
        public bool Contains(string key);
        public object? Fetch(string key);
        public T? Fetch<T>(string key) where T : class;
        public void Delete(string key);
    }
}