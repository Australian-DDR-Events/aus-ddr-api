namespace AusDdrApi.Services.Authorization
{
    public interface IAuthorization
    {
        public string? GetUserId();
        public void EnforceAdmin();
    }
}