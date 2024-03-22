using System.ComponentModel.DataAnnotations;

namespace HealthPayManager.App.Services
{
	public record ResponseDTO<T>(bool Status, string Message, T Data);
    public record PaginatedList<T>(int TotalRecords, int PageSize, int CurrentPage, int TotalPages, IEnumerable<T>? Result);
    public record GetCustomerDTO(long Id, string Name, string PatientId, string TimeCreated);
    public record CreateCustomerDTO([Required]string FirstName, [Required]string LastName);
    public record GetPaymentDTO(long Id, string Amount, long CustomerId, string PatientId, string TimeCreated);
    public record CreatePaymentDTO([Required]decimal Amount, [Required]long CustomerId);
}
