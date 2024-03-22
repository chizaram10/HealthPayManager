using HealthPayManager.App.Data.Entities;

namespace HealthPayManager.App.Repositories.Interface
{
    public interface ICustomerRepo
    {
        Task<IEnumerable<Customer>> ReadCustomersAsync(string searchText);
        Task<Customer> ReadCustomerByIdAsync(long id);
        Task<IEnumerable<string>> ReadCustomersPatientIdsAsync();
        Task CreateCustomersAsync(Customer customer);
        Task UpdateCustomersAsync(Customer customer);
        Task DeleteCustomersAsync(long id);
    }
}
