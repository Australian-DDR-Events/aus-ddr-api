namespace Application.Core.Interfaces
{
    public interface IIdentity<in T>
    {
        public bool IsAdmin(T source);
    }
}