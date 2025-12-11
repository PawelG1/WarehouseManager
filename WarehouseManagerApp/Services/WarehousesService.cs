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
            // Create database if doesn't exist (includes all properties from current model)
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
                        VolumePerUnitM3 = 0.5,
                        WarehouseId = warehouses[0].Id
                    },
                    new Product
                    {
                        Name = "Defender mouse",
                        SKU = "MM-275",
                        Quantity = 24,
                        minimumQuantity = 10,
                        VolumePerUnitM3 = 0.1,
                        WarehouseId = warehouses[0].Id
                    },
                    new Product
                    {
                        Name = "Dell Mechanical Keyboard",
                        SKU = "KB212-B",
                        Quantity = 12,
                        minimumQuantity = 20,
                        VolumePerUnitM3 = 0.2,
                        WarehouseId = warehouses[1].Id
                    },
                };

                await _context.Products.AddRangeAsync(products);
                await _context.SaveChangesAsync();
            }
        }
        public async Task AddProductAsync(Product product)
        {
            // Check if SKU already exists
            var existingProduct = await _context.Products
                .FirstOrDefaultAsync(p => p.SKU == product.SKU);
            
            if (existingProduct != null)
            {
                throw new InvalidOperationException($"A product with SKU '{product.SKU}' already exists. SKU must be unique.");
            }

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
                .OrderBy(p => p.Id) // oldest products first (ascending by ID)
                .ToListAsync();

            _productsCacheTime = DateTime.Now;

            return _productsCache;
        }

        public async Task<List<Product>> GetProductsAsync(int warehouseId)
        {
            var allProducts = await GetProductsAsync();

            return allProducts
                .Where(p => p.WarehouseId == warehouseId)
                .ToList(); // already sorted by GetProductsAsync
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
                .Include(w => w.Products) // Include products for space calculations
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
            // Check if another product with the same SKU exists (excluding current product)
            var existingProduct = await _context.Products
                .FirstOrDefaultAsync(p => p.SKU == product.SKU && p.Id != product.Id);
            
            if (existingProduct != null)
            {
                throw new InvalidOperationException($"Another product with SKU '{product.SKU}' already exists. SKU must be unique.");
            }

            //detach all tracked entities to avoid conflicts
            _context.ChangeTracker.Clear();
            
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            
            _productsCache = null;
            _productsCacheTime = null;
        }

        public async Task<Warehouse?> GetWarehouseByIdAsync(int id)
        {
            return await _context.Warehouses
                .AsNoTracking()
                .FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task AddWarehouseAsync(Warehouse warehouse)
        {
            await _context.Warehouses.AddAsync(warehouse);
            await _context.SaveChangesAsync();

            _warehousesCache = null;
            _warehousesCacheTime = null;
        }

        public async Task UpdateWarehouseAsync(Warehouse warehouse)
        {
            _context.ChangeTracker.Clear();

            _context.Warehouses.Update(warehouse);
            await _context.SaveChangesAsync();

            _warehousesCache = null;
            _warehousesCacheTime = null;
        }

        public async Task DeleteWarehouseAsync(int id)
        {
            var warehouse = await _context.Warehouses
                .Include(w => w.Products)
                .FirstOrDefaultAsync(w => w.Id == id);

            if (warehouse != null)
            {
                // Remove all products first
                _context.Products.RemoveRange(warehouse.Products);
                
                _context.Warehouses.Remove(warehouse);
                await _context.SaveChangesAsync();

                _warehousesCache = null;
                _warehousesCacheTime = null;
                _productsCache = null;
                _productsCacheTime = null;
            }
        }

        public async Task<DashboardStatistics> GetDashboardStatisticsAsync()
        {
            var warehouses = await GetWarehousesAsync();
            var products = await GetProductsAsync();

            var statistics = new DashboardStatistics
            {
                TotalWarehouses = warehouses.Count,
                TotalProducts = products.Count,
                LowStockItems = products.Count(p => p.Quantity < p.minimumQuantity),
            };

            // Calculate warehouse capacities and utilization
            foreach (var warehouse in warehouses)
            {
                var warehouseProducts = products.Where(p => p.WarehouseId == warehouse.Id).ToList();
                var productCount = warehouseProducts.Count;
                
                // Calculate total volume occupied by all products in this warehouse
                var totalVolumeOccupied = warehouseProducts.Sum(p => p.TotalVolumeM3);
                
                // Utilization = (total volume occupied / warehouse capacity) * 100
                var utilization = warehouse.CapacityM3 > 0 
                    ? (totalVolumeOccupied / warehouse.CapacityM3) * 100 
                    : 0;

                var capacity = new WarehouseCapacity
                {
                    WarehouseName = warehouse.Name,
                    CurrentProducts = productCount,
                    Capacity = warehouse.CapacityM3,
                    UtilizationPercentage = utilization,
                    IsCritical = utilization > 80, // Critical only when overfilled (>80%)
                    VolumeOccupiedM3 = totalVolumeOccupied
                };

                statistics.WarehouseCapacities.Add(capacity);

                if (capacity.IsCritical)
                {
                    statistics.CriticalCapacityWarehouses++;
                }

                // Product distribution
                statistics.ProductsByWarehouse.Add(new ProductDistribution
                {
                    WarehouseName = warehouse.Name,
                    ProductCount = productCount
                });
            }

            // Calculate average utilization
            statistics.AverageWarehouseUtilization = statistics.WarehouseCapacities.Any()
                ? statistics.WarehouseCapacities.Average(w => w.UtilizationPercentage)
                : 0;

            // Get low stock products
            statistics.LowStockProducts = products
                .Where(p => p.Quantity < p.minimumQuantity)
                .Select(p => new LowStockProduct
                {
                    ProductName = p.Name,
                    SKU = p.SKU,
                    CurrentQuantity = p.Quantity,
                    MinimumQuantity = p.minimumQuantity,
                    WarehouseName = p.Warehouse?.Name ?? "Unknown"
                })
                .ToList();

            return statistics;
        }
    }
}
