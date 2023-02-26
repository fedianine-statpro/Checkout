
namespace Api.DataContracts
{
    public record PaymentBase
    {
        public decimal Amount { get; set; }
        public string? Currency { get; set; }

        
    }
}
