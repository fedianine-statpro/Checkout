using Api.DataContracts;
using Api.Interfaces;

namespace Api.DB.Payments.Queries
{
    public class PaymentWriteRepository : IPaymentWriteRepository
    {
        private readonly PaymentDbContext _context;

        public PaymentWriteRepository(PaymentDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Payment payment)
        {
            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();
        }
    }
}
