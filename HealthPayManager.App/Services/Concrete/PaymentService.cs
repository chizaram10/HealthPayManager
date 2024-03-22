using HealthPayManager.App.Data.Entities;
using HealthPayManager.App.Repositories.Concrete;
using HealthPayManager.App.Repositories.Interface;
using HealthPayManager.App.Services.Interface;

namespace HealthPayManager.App.Services.Concrete
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepo _paymentRepo;
        private readonly ICustomerRepo _customerRepo;

        public PaymentService(IPaymentRepo paymentRepo, ICustomerRepo customerRepo)
        {
            _paymentRepo = paymentRepo;
            _customerRepo = customerRepo;
        }

        public async Task<ResponseDTO<PaginatedList<GetPaymentDTO>>> GetPaymentsByCustomerId(long customerId, int pageSize = 20, int pageNumber = 1)
        {
            try
            {
                string patientId = (await _customerRepo.ReadCustomerByIdAsync(customerId)).PatientId;
                IEnumerable<GetPaymentDTO> payments = (await _paymentRepo.ReadPaymentsByCustomerIdAsync(customerId)).OrderBy(x => x.TimeCreated).Select(x => new GetPaymentDTO(x.Id, x.Amount.ToString("C"), x.CustomerId, patientId, x.TimeCreated.ToString("dd MMMM yyyy"))).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                var response = new PaginatedList<GetPaymentDTO>(payments.Count(), pageSize, pageNumber, (int)Math.Ceiling((double)payments.Count() / pageSize), payments);

                return new ResponseDTO<PaginatedList<GetPaymentDTO>>(true, "Success", response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ResponseDTO<PaginatedList<GetPaymentDTO>>(false, "An error occured trying to get a customer's payments", null!);
            }
        }

        public async Task<ResponseDTO<string>> CreatePayment(CreatePaymentDTO paymentDTO)
        {
            try
            {
                Payment payment = new Payment
                {
                    Amount = paymentDTO.Amount,
                    CustomerId = paymentDTO.CustomerId,
                    TimeCreated = DateTime.Now,
                    TimeUpdated = DateTime.Now,
                };

                await _paymentRepo.CreatePaymentsAsync(payment);

                return new ResponseDTO<string>(true, "Success", $"New payment was successful");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ResponseDTO<string>(false, "An error occured while trying to make payment", null!);
            }
        }
    }
}
