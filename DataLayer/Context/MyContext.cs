
using DataLayer.Models;
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
        public DbSet<SponsorTransaction> SponsorTransactions { get; set; }
        public DbSet<BankTransaction> BankTransactions { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<SponsorTransactionError> Errors { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Colleague>()
                .HasIndex(u => u.PhoneNumber)
                .IsUnique();

            builder.Entity<Sponsor>()
                .HasIndex(u => u.PhoneNumber)
                .IsUnique();
        }
    }
}