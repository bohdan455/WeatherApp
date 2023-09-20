using Microsoft.EntityFrameworkCore;
using WeatherApp.DataAccess.Entities;

namespace WeatherApp.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<City> Cities { get; set; }

        public DbSet<WeatherInformation> WeatherInformations { get; set; }

        public DbSet<ZipCode> ZipCodes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<WeatherInformation>()
                .Property(wi => wi.ZipCodeId)
                .IsRequired(false);

            modelBuilder.Entity<WeatherInformation>()
                .HasOne(wi => wi.ZipCode)
                .WithOne(zc => zc.WeatherInformation)
                .HasForeignKey<WeatherInformation>(wi => wi.ZipCodeId)
                .IsRequired(false);

            modelBuilder.Entity<WeatherInformation>()
                .HasMany(wi => wi.Cities)
                .WithOne(c => c.WeatherInformation)
                .HasForeignKey(c => c.WeatherInformationId)
                .IsRequired();
        }
    }
}
