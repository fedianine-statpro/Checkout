namespace BankSimulator
{
    public interface IPayment
    {
        public Task<PaymentResponse> ProcessPayment(string cardNumber, int expiryMonth, int expiryYear, decimal amount,
            string currency, string cvv);
    }
}
