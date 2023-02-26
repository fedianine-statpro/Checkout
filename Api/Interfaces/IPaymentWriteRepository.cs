using Api.DataContracts;

namespace Api.Interfaces
{
    public interface IPaymentWriteRepository
    {
        Task AddAsync(Payment payment);
    }
}
