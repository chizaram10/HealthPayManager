namespace HealthPayManager.App.Services.Interface
{
    public interface ICustomerService
    {
        Task<ResponseDTO<PaginatedList<GetCustomerDTO>>> GetCustomers(string searchText, int pageSize = 20, int pageNumber = 1);
        Task<ResponseDTO<string>> CreateCustomer(CreateCustomerDTO customerDTO);
    }
}