using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseManagerApp.Data;
using WarehouseManagerApp.Models;

namespace WarehouseManagerApp.Services
{
    public class WarehousesService : IWarehouseService
    {
        private readonly WarehouseContext _context;

        public WarehousesService(WarehouseContext context)
        {
            _context = context;
        }
        public async Task InitializeDbAsync()
        {
            await _context.Database.EnsureCreatedAsync();

            if(!await _context.Warehouses.AnyAsync())//if there is no records then create some example data
            {
                var warehouses = new[]
                {
                    new Warehouse
                    {
                        Name = "Main Warehouse",
                        Location = "Niepolomice, ul.Przemyslowa 3",
                        CapacityM3 = 10000,
                    },
                    new Warehouse {
                        Name = "Secondary Warehouse",
                        Location = "Jasionka, ul.Lotnicza 6",
                        CapacityM3 = 6000,
                    },
                };

                await _context.Warehouses.AddRangeAsync(warehouses);
                await _context.SaveChangesAsync();

                var products = new[]
                {
                    new Product
                    {
                        Name = "Dell Laptop",
                        SKU = "DELL-SPX15",
                        Quantity = 36,
                        minimumQuantity = 5,
                        WarehouseId = warehouses[0].Id
                    },
                    new Product
                    {
                        Name = "Defender mouse",
                        SKU = "MM-275",
                        Quantity = 24,
                        minimumQuantity = 10,
                        WarehouseId = warehouses[0].Id
                    },
                    new Product
                    {
                        Name = "Dell Mechanical Keyboard",
                        SKU = "KB212-B",
                        Quantity = 12,
                        minimumQuantity = 20,
                        WarehouseId = warehouses[1].Id
                    },
                };

                await _context.Products.AddRangeAsync(products);
                await _context.SaveChangesAsync();
            }
        }
        public async Task AddProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);
        }

        public async Task DeleteProductAsync(int id)
        {
            var productToBeDeleted = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id);

            if (productToBeDeleted != null)
            {
                _context.Products.Remove(productToBeDeleted);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Warehouse)
                .FirstOrDefaultAsync(p => p.Id == id);
            
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            return await _context.Products
                .Include(p => p.Warehouse)
                .OrderByDescending(p => p.Id)
                .ToListAsync();
        }

        public async Task<int> GetProductsCountAsync()
        {
            return await _context.Products
                .Include(p => p.Warehouse)
                .CountAsync();
        }

        public async Task<List<Warehouse>> GetWarehousesAsync()
        {
            return await _context.Warehouses
                .ToListAsync();
        }

        public async Task<int> GetWarehousesCountAsync()
        {
            return await _context.Warehouses
                .CountAsync();
        }


        public Task UpdateProductAsync(Product product)
        {
            throw new NotImplementedException();
        }
    }
}
