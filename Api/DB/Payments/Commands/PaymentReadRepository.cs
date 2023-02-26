using Api.DataContracts;
using Api.Interfaces;

namespace Api.DB.Payments.Commands
{
    public class PaymentReadRepository : IPaymentReadRepository
    {
        private readonly PaymentDbContext _context;

        public PaymentReadRepository(PaymentDbContext context)
        {
            _context = context;
        }

        public async Task<Payment?> GetAsync(Guid id)
        {
            return await _context.Payments.FindAsync(id);
        }

        public async Task<IQueryable<Payment>> GetAllAsync()
        {
            return _context.Payments.AsQueryable();
        }
    }
}
