using AddressValidation.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AddressValidation.Data.Persistences
{
    public class AddressValidationDb : DbContext
    {
        public AddressValidationDb() { }
        public AddressValidationDb(DbContextOptions<AddressValidationDb> options) : base(options) { }

        public virtual DbSet<UsageStatistic> UsageStatistics { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UsageStatistic>()
                .Property<int>("Id")
                .ValueGeneratedOnAdd()
                .IsRequired();
            builder.Entity<UsageStatistic>().HasKey("Id");
            builder.Entity<UsageStatistic>().OwnsOne(stat => stat.Address);
        }
    }
}
