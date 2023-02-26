using Api.DataContracts;

namespace Api
{
    using Microsoft.EntityFrameworkCore;

    public class PaymentDbContext : DbContext
    {
        public DbSet<Payment>? Payments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("PaymentDatabase");
        }
    }
}
