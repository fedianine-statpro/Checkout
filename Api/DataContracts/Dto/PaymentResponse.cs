namespace Api.DataContracts.Dto
{
    public sealed record PaymentResponse : PaymentBase
    {
        public CardDetails Card { get; set; }

        public Guid Id { get; set; }

        public string Status { get; set; }

        public PaymentResponse(Payment payment)
        {
            Card = new CardDetails
            {
                CardNumber = payment.CardNumber,
                ExpiryMonth = payment.ExpiryMonth,
                ExpiryYear = payment.ExpiryYear,
                CVV = payment.CVV
            };
            Amount = payment.Amount;
            Currency = payment.Currency;
            Status = payment.Status;
            Id = payment.Id;
        }
    }

    public enum PaymentStatus
    {
        Successful,
        Unsuccessful,
        Unknown
    }
}
