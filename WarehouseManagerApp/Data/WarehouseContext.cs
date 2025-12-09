using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseManagerApp.Models;

namespace WarehouseManagerApp.Data
{
    public class WarehouseContext : DbContext
    {
        //tables representations for db
        public DbSet<Product> Products { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }

        public WarehouseContext() : base()
        {
        }
        //contructor for tests DI
        public WarehouseContext(DbContextOptions<WarehouseContext> options) : base(options) { 
        }

        //db connection config
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //config only if options are unset
            if (!optionsBuilder.IsConfigured)
            {
                //path to local db file
                optionsBuilder.UseSqlite("Data Source=localDb.db");
            }
        }

        //model relations and boundries config
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Product configuration
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.Id);
                
                entity.Property(p => p.Name)
                    .IsRequired()
                    .HasMaxLength(200);
                
                entity.Property(p => p.SKU)
                    .IsRequired()
                    .HasMaxLength(100);
                
                entity.Property(p => p.minimumQuantity)
                    .HasDefaultValue(1);
                
                entity.HasOne(p => p.Warehouse)
                    .WithMany(w => w.Products)
                    .HasForeignKey(p => p.WarehouseId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Warehouse configuration
            modelBuilder.Entity<Warehouse>(entity =>
            {
                entity.HasKey(w => w.Id);
                
                entity.Property(w => w.Name)
                    .IsRequired()
                    .HasMaxLength(200);
                
                entity.Property(w => w.Location)
                    .IsRequired()
                    .HasMaxLength(500);
            });
        }
    }
}
