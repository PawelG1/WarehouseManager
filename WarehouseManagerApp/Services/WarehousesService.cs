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
    public class WarehousesService : IWarehousesService
    {
        private readonly WarehouseContext _context;

        //cache
        private List<Product>? _productsCache;
        private DateTime? _productsCacheTime;
        private List<Warehouse>? _warehousesCache;
        private DateTime? _warehousesCacheTime;
        private readonly TimeSpan _cacheExpirationTime = TimeSpan.FromMinutes(5);

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
            await _context.SaveChangesAsync();

            _productsCache = null;
            _productsCacheTime = null;
        }

        public async Task DeleteProductAsync(int id)
        {
            var productToBeDeleted = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id);

            if (productToBeDeleted != null)
            {
                _context.Products.Remove(productToBeDeleted);
                await _context.SaveChangesAsync();

                _productsCache = null;
                _productsCacheTime = null;
            }
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            //always load from database with AsNoTracking for editing
            //cache may contain tracked entities which cause conflicts during update
            return await _context.Products
                .Include(p => p.Warehouse)
                .AsNoTracking() //don't track this entity
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            if(_productsCache != null &&
                _productsCacheTime.HasValue &&
                DateTime.Now - _productsCacheTime < _cacheExpirationTime)
            {
                return _productsCache;
            }

            _productsCache = await _context.Products
                .Include(p => p.Warehouse)
                .OrderBy(p => p.Id)
                .ToListAsync();

            _productsCacheTime = DateTime.Now;

            return _productsCache;
        }

        public async Task<List<Product>> GetProductsAsync(int warehouseId)
        {
            var allProducts = await GetProductsAsync();

            return allProducts
                .Where(p => p.WarehouseId == warehouseId)
                .OrderBy(p => p.Id)
                .ToList();
        }

        public async Task<int> GetProductsCountAsync()
        {
            var products = await GetProductsAsync();
            return products.Count;
        }

        public async Task<int> GetProductsCountAsync(int warehouseId)
        {
            var products = await GetProductsAsync(warehouseId);
            return products.Count;
        }

        public async Task<List<Warehouse>> GetWarehousesAsync()
        {
            if (_warehousesCache != null &&
                _warehousesCacheTime.HasValue &&
                DateTime.Now - _warehousesCacheTime.Value < _cacheExpirationTime)
            {
                return _warehousesCache;
            }

            _warehousesCache = await _context.Warehouses
                .ToListAsync();

            _warehousesCacheTime = DateTime.Now;

            return _warehousesCache;
        }

        public async Task<int> GetWarehousesCountAsync()
        {
            var warehouses = await GetWarehousesAsync();
            return warehouses.Count;
        }


        public async Task UpdateProductAsync(Product product)
        {
            //detach all tracked entities to avoid conflicts
            _context.ChangeTracker.Clear();
            
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            
            _productsCache = null;
            _productsCacheTime = null;
        }
    }
}
