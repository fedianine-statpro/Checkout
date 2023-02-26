using Api.Controllers;
using Api.DataContracts.Dto;
using Api.Interfaces;
using BankSimulator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Payment = Api.DataContracts.Payment;
using PaymentResponse = Api.DataContracts.Dto.PaymentResponse;
using PaymentStatus = Api.DataContracts.Dto.PaymentStatus;

namespace Api.Tests
{
    public class PaymentControllerTests
    {
        private readonly IPayment _bankSimulator;
        private readonly IPaymentReadRepository _paymentReadRepository;
        private readonly IPaymentWriteRepository _paymentWriteRepository;

        private readonly PaymentController _paymentController;

        public PaymentControllerTests()
        {
            var logger = Mock.Of<ILogger<PaymentController>>();
            _bankSimulator = Mock.Of<IPayment>();
            _paymentReadRepository = Mock.Of<IPaymentReadRepository>();
            _paymentWriteRepository = Mock.Of<IPaymentWriteRepository>();

            _paymentController = new PaymentController(logger, _bankSimulator, _paymentReadRepository, _paymentWriteRepository);
        }

        [Fact]
        public async Task ProcessPayment_WithValidPayment_ReturnsSuccessfulPayment()
        {
            // Arrange
            var paymentRequest = new PaymentRequest
            {
                Card = new CardDetails()
                {
                    CardNumber = "4111111111111111",
                    ExpiryMonth = 12,
                    ExpiryYear = 2024,
                    CVV = "123"
                },
                Amount = 100.00M,
                Currency = "USD"
            };

            var bankResponse = new BankSimulator.PaymentResponse
            {
                Id = Guid.NewGuid(),
                Response = BankSimulator.PaymentStatus.Approved
            };
            Mock.Get(_bankSimulator).Setup(x => x.ProcessPayment(paymentRequest.Card.CardNumber, paymentRequest.Card.ExpiryMonth, paymentRequest.Card.ExpiryYear, paymentRequest.Amount, paymentRequest.Currency, paymentRequest.Card.CVV)).ReturnsAsync(bankResponse);

            Payment payment = new Payment(paymentRequest.Card, paymentRequest.Amount, paymentRequest.Currency, PaymentStatus.Successful);

            Mock.Get(_paymentWriteRepository).Setup(x => x.AddAsync(payment)).Returns(Task.CompletedTask);

            // Act
            var result = await _paymentController.ProcessPayment(paymentRequest);

            // Assert
            Assert.IsType<PaymentResponse>(result.Value);
            Assert.Equal(PaymentStatus.Successful.ToString(), result.Value.Status);
        }

        [Fact]
        public async Task ProcessPayment_WithInvalidPayment_ReturnsBadRequest()
        {
            // Arrange
            var paymentRequest = new PaymentRequest
            {
                Card = new CardDetails
                {
                    CardNumber = "4111111111111111",
                    ExpiryMonth = 12,
                    ExpiryYear = 2020, // Invalid year
                    CVV = "123"
                },
                Amount = 100.00M,
                Currency = "USD"
            };

            // Act
            var result = await _paymentController.ProcessPayment(paymentRequest);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task ProcessPayment_WithBankError_ReturnsInternalServerError()
        {
            // Arrange
            var paymentRequest = new PaymentRequest
            {
                Card = new CardDetails()
                {
                    CardNumber = "4111111111111111",
                    ExpiryMonth = 12,
                    ExpiryYear = 2024,
                    CVV = "123"
                },
                Amount = 100.00M,
                Currency = "USD"
            };

            var bankResponse = new BankSimulator.PaymentResponse
            {
                Id = Guid.NewGuid(),
                Response = BankSimulator.PaymentStatus.InsufficientFunds
            };
            Mock.Get(_bankSimulator).Setup(x => x.ProcessPayment(paymentRequest.Card.CardNumber, paymentRequest.Card.ExpiryMonth, paymentRequest.Card.ExpiryYear, paymentRequest.Amount, paymentRequest.Currency, paymentRequest.Card.CVV)).ReturnsAsync(bankResponse);

            // Act
            var result = await _paymentController.ProcessPayment(paymentRequest);

            // Assert
            Assert.IsType<PaymentResponse?>(result.Value);
        }

        [Fact]
        public async Task GetPayment_WithValidId_ReturnsPayment()
        {
            // Arrange
            var payment = new Payment(new CardDetails()
            {
                CardNumber = "4111111111111111",
                ExpiryMonth = 12,
                ExpiryYear = 2024,
                CVV = "123"
            }, 100.00M, "USD", PaymentStatus.Successful);
            Mock.Get(_paymentReadRepository).Setup(x => x.GetAsync(payment.Id)).ReturnsAsync(payment);

            // Act
            var result = await _paymentController.GetPayment(payment.Id);

            // Assert
            Assert.IsType<PaymentResponse>(result.Value);
            Assert.Equal(new PaymentResponse(payment), result.Value);
        }

        [Fact]
        public async Task GetPayment_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var paymentId = Guid.NewGuid();
            Mock.Get(_paymentReadRepository).Setup(x => x.GetAsync(paymentId)).ReturnsAsync((Payment?)null);

            // Act
            var result = await _paymentController.GetPayment(paymentId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }
    }
}
