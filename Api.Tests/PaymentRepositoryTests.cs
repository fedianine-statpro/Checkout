using Api.DataContracts;
using Api.DataContracts.Dto;
using Api.DB.Payments.Commands;
using Api.DB.Payments.Queries;

namespace Api.Tests
{
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class PaymentRepositoryTests
    {
        [Fact]
        public async Task AddAsync_Should_Add_Payment_To_Database()
        {
            // Act
            await using var context = new PaymentDbContext();
            var paymentReadRepository = new PaymentReadRepository(context);
            var paymentWriteRepository = new PaymentWriteRepository(context);
            Payment payment = new Payment(
                new CardDetails()
                {
                    CardNumber = "4111111111111111",
                    ExpiryMonth = 12,
                    ExpiryYear = 2024,
                    CVV = "123",
                },
                100.00M,
                "USD"
                , PaymentStatus.Successful);
            await paymentWriteRepository.AddAsync(payment);

            // Assert
            var result = (await paymentReadRepository.GetAllAsync()).FirstOrDefault(p => p.Id == payment.Id);
            Assert.NotNull(result);
            Assert.Equal(payment.Amount, result.Amount);
        }

        [Fact]
        public async Task GetAsync_Should_Return_Null_For_Nonexistent_Id()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act
            await using var context = new PaymentDbContext();
            var repository = new PaymentReadRepository(context);
            var result = await repository.GetAsync(id);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAsync_Should_Return_Payment_For_Existing_Id()
        {
            // Arrange
            Payment payment = new Payment(
                new CardDetails()
                {
                    CardNumber = "4111111111111111",
                    ExpiryMonth = 12,
                    ExpiryYear = 2024,
                    CVV = "123",
                },
                100.00M,
                "USD"
                , PaymentStatus.Successful);
            await using (var context = new PaymentDbContext())
            {
                context.Payments.Add(payment);
                await context.SaveChangesAsync();
            }

            // Act
            await using (var context = new PaymentDbContext())
            {
                var repository = new PaymentReadRepository(context);
                var result = await repository.GetAsync(payment.Id);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(payment.Amount, result.Amount);
            }
        }
    }
}
