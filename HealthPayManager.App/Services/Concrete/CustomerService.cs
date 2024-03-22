using HealthPayManager.App.Data.Entities;
using HealthPayManager.App.Repositories.Interface;
using HealthPayManager.App.Services.Interface;
using System.Drawing;

namespace HealthPayManager.App.Services.Concrete
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepo _customerRepo;

        public CustomerService(ICustomerRepo customerRepo)
        {
            _customerRepo = customerRepo;
        }

        public async Task<ResponseDTO<PaginatedList<GetCustomerDTO>>> GetCustomers(string searchText, int pageSize = 20, int pageNumber = 1)
        {
            try
            {
                IEnumerable<GetCustomerDTO> customers = (await _customerRepo.ReadCustomersAsync(searchText)).OrderBy(x => x.Name).Select(x => new GetCustomerDTO(x.Id, x.Name, x.PatientId, x.TimeCreated.ToString("dd MMMM yyyy"))).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                var response = new PaginatedList<GetCustomerDTO>(customers.Count(), pageSize, pageNumber, (int)Math.Ceiling((double)customers.Count() / pageSize), customers);

                return new ResponseDTO<PaginatedList<GetCustomerDTO>>(true, "Success", response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ResponseDTO<PaginatedList<GetCustomerDTO>>(false, "An error occured while trying to load customers", null!);
            }
        }

        public async Task<ResponseDTO<string>> CreateCustomer(CreateCustomerDTO customerDTO)
        {
            try
            {
                Customer customer = new Customer
                {
                    Name = $"{customerDTO.FirstName} {customerDTO.LastName}",
                    PatientId = GeneratePatientId(customerDTO.FirstName, customerDTO.LastName),
                    TimeCreated = DateTime.Now,
                    TimeUpdated = DateTime.Now,
                };

                await _customerRepo.CreateCustomersAsync(customer);

                return new ResponseDTO<string>(true, "Success", $"New customer was successfully added. Customer Patient ID is {customer.PatientId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ResponseDTO<string>(false, "An error occured while trying to create and save a customer", null!);
            }
        }

        private static string GeneratePatientId(string firstName, string lastName)
        {
            string initials = $"{firstName[0]}{lastName[0]}";
            string uniqueId = GenerateRandomAlphanumericId(5);
            string patientId = $"HTP-{initials.ToUpper()}{uniqueId}";

            return patientId;
        }

        static string GenerateRandomAlphanumericId(int length)
        {
            Random random = new Random();
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
