using Microsoft.AspNetCore.Mvc;
using ServiceAPI.Services.Interfaces;

namespace ServiceAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet]
        public async Task<IActionResult> CreatePayment()
        {
            var res = await _paymentService.CreatePayment();

            return Ok(res);
        }

        [HttpGet]
        public bool CheckPayment(string paymentId)
        {
            return _paymentService.CheckPayment(paymentId);
        }
    }
}
