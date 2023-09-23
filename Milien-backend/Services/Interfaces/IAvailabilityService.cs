namespace Milien_backend.Services.Interfaces
{
    public interface IAvailabilityService
    {
        Task<bool> CheckLogin(string login);
        Task<bool> CheckPhoneNumber(string phoneNumber);
        Task<bool> CheckAcceptEmail(string login);
    }
}
