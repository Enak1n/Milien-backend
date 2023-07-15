namespace ServiceAPI.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<object> CreatePayment();
        bool CheckPayment(string paymentId);
    }
}
