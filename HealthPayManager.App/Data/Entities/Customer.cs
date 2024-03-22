namespace HealthPayManager.App.Data.Entities
{
    public class Customer : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string PatientId { get; set; } = string.Empty;
    }
}
