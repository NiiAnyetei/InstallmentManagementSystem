using DataLayer.Models.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Context
{
    
    public class IMSDbContext : DbContext
    {
        // DbSettings field to store the connection string
        private readonly DbSettings _dbsettings;

        // Constructor to inject the DbSettings model
        public IMSDbContext(IOptions<DbSettings> dbSettings)
        {
            _dbsettings = dbSettings.Value;
        }

        // DbSet properties to represent the tables
        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Installment> Installments { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<UserRefreshToken> UserRefreshTokens { get; set; }

        // Configuring the database provider and connection string
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_dbsettings.ConnectionString);
        }

        // Configuring the model for the Todo entity
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Email).IsUnique();
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
            
            modelBuilder.Entity<Installment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Customer)
                    .WithMany(e => e.Installment)
                    .HasForeignKey(e => e.CustomerId);
                entity.HasMany(e => e.Payments)
                    .WithOne(e => e.Installment)
                    .HasForeignKey(e => e.InstallmentId);
                entity.HasMany(e => e.Bills)
                    .WithOne(e => e.Installment)
                    .HasForeignKey(e => e.InstallmentId);
            });
            
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
            
            modelBuilder.Entity<Bill>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
        }
    }
}
