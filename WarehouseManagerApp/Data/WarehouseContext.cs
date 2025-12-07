using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseManagerApp.Models;

namespace WarehouseManagerApp.Data
{
    class WarehouseContext : DbContext
    {
        //tables representations for db
        public DbSet<Product> Products { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }

        //db connection config
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //path to local db file
            optionsBuilder.UseSqlite("Data Source=localDb.db");
        }

        //model relations and boundries config
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //relations
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Warehouse)
                .WithMany(w => w.Products)
                .HasForeignKey(p => p.WarehouseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .Property(p => p.minimumQuantity)
                .HasDefaultValue(1);
                
        }
    }
}
