namespace HealthPayManager.App.Data.Entities
{
    public class Payment : BaseEntity
    {
        public decimal Amount { get; set; }
        public long CustomerId { get; set; }
    }
}
