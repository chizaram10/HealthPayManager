using HealthPayManager.App.Data.Entities;

namespace HealthPayManager.App.Repositories.Interface
{
    public interface IPaymentRepo
    {
        Task<IEnumerable<Payment>> ReadPaymentsAsync();
        Task<IEnumerable<Payment>> ReadPaymentsByCustomerIdAsync(long customerId);
        Task<Payment> ReadPaymentByIdAsync(long id);
        Task CreatePaymentsAsync(Payment payment);
        Task UpdatePaymentsAsync(Payment payment);
        Task DeletePaymentsAsync(long id);
    }
}
