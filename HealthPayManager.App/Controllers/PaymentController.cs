using HealthPayManager.App.Repositories.Interface;
using HealthPayManager.App.Services;
using HealthPayManager.App.Services.Concrete;
using HealthPayManager.App.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace HealthPayManager.App.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public async Task<IActionResult> Index(long customerId)
        {
            ViewData["customerId"] = customerId;
            var model = await _paymentService.GetPaymentsByCustomerId(customerId);
            return View(model);
        }

        [HttpGet]
        public IActionResult Add(long customerId)
        {
            ViewData["customerId"] = customerId;
            return View("AddPayment");
        }

        [HttpPost]
        public async Task<IActionResult> Add(CreatePaymentDTO paymentDTO)
        {
            ViewData["customerId"] = paymentDTO.CustomerId;
			var response = await _paymentService.CreatePayment(paymentDTO);
            return View("ConfirmPayment", response);
        }
    }
}
