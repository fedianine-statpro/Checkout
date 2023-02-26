namespace BankSimulator
{
    public class Payment : IPayment
    {
        public async Task<PaymentResponse> ProcessPayment(string cardNumber, int expiryMonth, int expiryYear, decimal amount, string currency, string cvv)
        {
            // TODO: Validate Parameters


            // Simulate payment processing
            // Use CKO bank simulator API to process payment
            // Return payment response (approved or declined)
            return new PaymentResponse
            {
                Id = Guid.NewGuid(),
                Response = amount > 100000000 ? PaymentStatus.InsufficientFunds : PaymentStatus.Approved
            };
        }
    }
}