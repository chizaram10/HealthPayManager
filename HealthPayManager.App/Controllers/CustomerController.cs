using HealthPayManager.App.Services;
using HealthPayManager.App.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace HealthPayManager.App.Controllers
{
	public class CustomerController : Controller
	{
		private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<IActionResult> Index(string searchText = "")
		{
			var model = await _customerService.GetCustomers(searchText);
			return View(model);
		}

		[HttpGet]
		public IActionResult Add()
		{
			return View("AddCustomer");
		}

		[HttpPost]
		public async Task<IActionResult> Add(CreateCustomerDTO customerDTO)
		{
            var response = await _customerService.CreateCustomer(customerDTO);
			return View("ConfirmCustomerCreation", response);
		}
	}
}
