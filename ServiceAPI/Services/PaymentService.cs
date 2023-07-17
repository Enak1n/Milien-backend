using Microsoft.Extensions.Configuration;
using ServiceAPI.Services.Interfaces;
using Yandex.Checkout.V3;

namespace ServiceAPI.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;

        public PaymentService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool CheckPayment(string paymentId)
        {
            Client client = new Client(_configuration.GetSection("YooKassa:ShopId").Value,
            _configuration.GetSection("YooKassa:SecretKey").Value);

            try
            {
                var paymentInfo = client.GetPayment(paymentId);

                if (paymentInfo.Status == PaymentStatus.Succeeded)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<object> CreatePayment()
        {
            Client client = new Client(_configuration.GetSection("YooKassa:ShopId").Value,
            _configuration.GetSection("YooKassa:SecretKey").Value);

            var newPayment = new NewPayment
            {
                Amount = new Amount { Value = 50.00m, Currency = "RUB" },
                Confirmation = new Confirmation
                {
                    Type = ConfirmationType.Redirect,
                    ReturnUrl = "https://xn--h1agbg8e4a.xn--p1ai/payment-success"
                },
                Capture = true
            };

            Payment payment = client.CreatePayment(newPayment);
            string paymentUrl = payment.Confirmation.ConfirmationUrl;
            return new { PaymentUrl = paymentUrl, PaymentId = payment.Id };
        }
    }
}
