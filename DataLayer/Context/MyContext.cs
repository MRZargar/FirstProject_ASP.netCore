
using Microsoft.EntityFrameworkCore;

namespace DataLayer
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> option) : base(option)
        {

        }

        public DbSet<Colleague> Colleagues { get; set; }
        public DbSet<Sponsor> Sponsors { get; set; }
        public DbSet<BankData> BankDatas { get; set; }
        public DbSet<SponsorTransaction> SponsorTransactions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Colleague>()
                .HasIndex(u => u.PhoneNumber)
                .IsUnique();

            builder.Entity<Sponsor>()
                .HasIndex(u => u.PhoneNumber)
                .IsUnique();

            builder.Entity<BankData>()
                .HasIndex(u => u.TransactionDate);

            builder.Entity<SponsorTransaction>()
                .HasIndex(u => u.TransactionDate);
        }
    }
}