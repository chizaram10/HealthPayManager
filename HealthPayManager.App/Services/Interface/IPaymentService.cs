namespace HealthPayManager.App.Services.Interface
{
    public interface IPaymentService
    {
        Task<ResponseDTO<PaginatedList<GetPaymentDTO>>> GetPaymentsByCustomerId(long customerId, int pageSize = 20, int pageNumber = 1);
        Task<ResponseDTO<string>> CreatePayment(CreatePaymentDTO paymentDTO);
    }
}