using BankSimulator;
using Xunit;

namespace Api.Tests
{
    public class BankSimulatorTests
    {
        private readonly IPayment _paymentProcessor;

        public BankSimulatorTests()
        {
            _paymentProcessor = new Payment();
        }

        [Fact]
        public async Task ProcessPayment_Approved()
        {
            // Arrange
            var cardNumber = "1234567890123456";
            var expiryMonth = 12;
            var expiryYear = 2023;
            var amount = 100;
            var currency = "USD";
            var cvv = "123";

            // Act
            var response = await _paymentProcessor.ProcessPayment(cardNumber, expiryMonth, expiryYear, amount, currency, cvv);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(PaymentStatus.Approved, response.Response);
            Assert.NotEqual(Guid.Empty, response.Id);
        }

        [Fact]
        public async Task ProcessPayment_InsufficientFunds()
        {
            // Arrange
            var cardNumber = "1234567890123456";
            var expiryMonth = 12;
            var expiryYear = 2023;
            var amount = 10000000000000;
            var currency = "USD";
            var cvv = "123";

            // Act
            var response = await _paymentProcessor.ProcessPayment(cardNumber, expiryMonth, expiryYear, amount, currency, cvv);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(PaymentStatus.InsufficientFunds, response.Response);
            Assert.NotEqual(Guid.Empty, response.Id);
        }
    }
}
