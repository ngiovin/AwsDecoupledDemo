using Common.Models;
using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore.Extensions;
using StorageLayer.Models;


namespace StorageLayer.Services
{
    public class CustomerContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        private readonly DatabaseSettings _settings;
        public CustomerContext(DatabaseSettings settings) : base()
        {
            _settings = settings;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL($"server={_settings.Server};database={_settings.Database};user={_settings.User};password={_settings.Password}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });


        }
    }
}