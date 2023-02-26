using Api.DataContracts.Dto;

namespace Api.Tests
{
    using Xunit;

    public class PaymentRequestTests
    {
        [Fact]
        public void IsValidCardNumber_InvalidLength_ReturnsFalse()
        {
            // Arrange
            var cardNumber = "123456789012345"; // 15 digits instead of 16

            // Act
            var result = PaymentRequest.IsValidCardNumber(cardNumber);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValidCardNumber_ValidCardNumber_ReturnsTrue()
        {
            // Arrange
            var cardNumber = "1234567890123456"; // Valid 16 digit card number

            // Act
            var result = PaymentRequest.IsValidCardNumber(cardNumber);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(13)]
        [InlineData(2021)] // Expired year
        public void IsValidExpiryMonth_InvalidExpiryMonth_ReturnsFalse(int expiryMonth)
        {
            // Act
            var result = PaymentRequest.IsValidExpiryMonth(expiryMonth);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(6)]
        [InlineData(12)]
        public void IsValidExpiryMonth_ValidExpiryMonth_ReturnsTrue(int expiryMonth)
        {
            // Act
            var result = PaymentRequest.IsValidExpiryMonth(expiryMonth);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(2021)] // Expired year
        [InlineData(2022)]
        public void IsValidExpiryYear_InvalidExpiryYear_ReturnsFalse(int expiryYear)
        {
            // Act
            var result = PaymentRequest.IsValidExpiryYear(expiryYear);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData(2023)]
        [InlineData(2024)]
        [InlineData(2025)]
        public void IsValidExpiryYear_ValidExpiryYear_ReturnsTrue(int expiryYear)
        {
            // Act
            var result = PaymentRequest.IsValidExpiryYear(expiryYear);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void IsValidAmount_InvalidAmount_ReturnsFalse(decimal amount)
        {
            // Act
            var result = PaymentRequest.IsValidAmount(amount);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void IsValidAmount_ValidAmount_ReturnsTrue(decimal amount)
        {
            // Act
            var result = PaymentRequest.IsValidAmount(amount);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("USD")]
        [InlineData("EUR")]
        [InlineData("JPY")]
        public void IsValidCurrency_ValidCurrency_ReturnsTrue(string currency)
        {
            // Act
            var result = PaymentRequest.IsValidCurrency(currency);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("AAA")]
        [InlineData("USD1")]
        [InlineData("US")]
        public void IsValidCurrency_InvalidCurrency_ReturnsFalse(string currency)
        {
            // Act
            var result = PaymentRequest.IsValidCurrency(currency);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData("12")]
        [InlineData("1234")]
        public void IsValidCVV_InvalidCVV_ReturnsFalse(string cvv)
        {
            // Act
            var result = PaymentRequest.IsValidCVV(cvv);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData("123")]
        [InlineData("000")]
        public void IsValidCVV_ValidCVV_ReturnsTrue(string cvv)
        {
            // Act
            var result = PaymentRequest.IsValidCVV(cvv);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValid_AllPropertiesValid_ReturnsTrue()
        {
            // Arrange
            var paymentRequest = new PaymentRequest
            {
                Card = new CardDetails()
                {
                    CardNumber = "1234567890123456",
                    ExpiryMonth = 12,
                    ExpiryYear = 2023,
                    CVV = "123"
                },
                Amount = 100,
                Currency = "USD"
            };

            // Act
            var result = paymentRequest.IsValid();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValid_InvalidCardNumber_ReturnsFalse()
        {
            // Arrange
            var paymentRequest = new PaymentRequest
            {
                Card = new CardDetails()
                {

                    CardNumber = "123456789012345", // Invalid card number
                    ExpiryMonth = 12,
                    ExpiryYear = 2023,
                    CVV = "123"
                },
                Amount = 100,
                Currency = "USD"
            };

            // Act
            var result = paymentRequest.IsValid();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValid_InvalidExpiryMonth_ReturnsFalse()
        {
            // Arrange
            var paymentRequest = new PaymentRequest
            {
                Card = new CardDetails()
                {
                    CardNumber = "1234567890123456",
                    ExpiryMonth = 1, // Expired month
                    ExpiryYear = 2023,
                    CVV = "123"
                },
                Amount = 100,
                Currency = "USD"
            };

            // Act
            var result = paymentRequest.IsValid();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValid_InvalidExpiryYear_ReturnsFalse()
        {
            // Arrange
            var paymentRequest = new PaymentRequest
            {
                Card = new CardDetails()
                {
                    CardNumber = "1234567890123456",
                    ExpiryMonth = 12,
                    ExpiryYear = 2021, // Expired year
                    CVV = "123"
                },
                Amount = 100,
                Currency = "USD"
            };

            // Act
            var result = paymentRequest.IsValid();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValid_InvalidAmount_ReturnsFalse()
        {
            // Arrange
            var paymentRequest = new PaymentRequest
            {
                Card = new CardDetails()
                {
                    CardNumber = "1234567890123456",
                    ExpiryMonth = 12,
                    ExpiryYear = 2023,
                    CVV = "123"
                },
                Amount = 0, // Invalid amount
                Currency = "USD"
            };

            // Act
            var result = paymentRequest.IsValid();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValid_InvalidCurrency_ReturnsFalse()
        {
            // Arrange
            var paymentRequest = new PaymentRequest
            {
                Card = new CardDetails()
                {
                    CardNumber = "1234567890123456",
                    ExpiryMonth = 12,
                    ExpiryYear = 2023,
                    CVV = "123"
                },
                Amount = 100,
                Currency = "AAA" // Invalid currency
            };

            // Act
            var result = paymentRequest.IsValid();

            // Assert
            Assert.False(result);
        }
    }
}
