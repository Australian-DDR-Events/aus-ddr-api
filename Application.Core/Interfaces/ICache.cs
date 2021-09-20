namespace Application.Core.Interfaces
{
    public interface ICache
    {
        public void Add(string key, object value);
        public bool Contains(string key);
        public object Fetch(string key);
        public void Delete(string key);
    }
}