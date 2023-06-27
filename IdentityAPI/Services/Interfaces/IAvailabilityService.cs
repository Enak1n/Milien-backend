namespace Milien_backend.Services.Interfaces
{
    public interface IAvailabilityService
    {
        Task<bool> CheckCode(string code, string email);
        Task<bool> CheckLogin(string login);
        Task<bool> CheckPhoneNumber(string phoneNumber);
        Task<bool> CheckEmail(string email);
    }
}
