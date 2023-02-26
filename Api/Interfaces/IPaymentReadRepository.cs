using Api.DataContracts;

namespace Api.Interfaces
{
    public interface IPaymentReadRepository
    {
        Task<Payment?> GetAsync(Guid id);
        Task<IQueryable<Payment>> GetAllAsync();
    }
}
