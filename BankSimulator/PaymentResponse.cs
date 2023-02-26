namespace BankSimulator
{
    public record PaymentResponse
    {
        public Guid Id { get; init; }
        public PaymentStatus Response { get; init; }
    }
}
