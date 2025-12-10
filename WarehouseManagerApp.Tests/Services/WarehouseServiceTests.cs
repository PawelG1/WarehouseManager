using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using WarehouseManagerApp.Data;
using WarehouseManagerApp.Models;
using WarehouseManagerApp.Services;
using Xunit;

namespace WarehouseManagerApp.Tests.Services
{
    public class WarehousesServiceTests : IDisposable
    {
        private readonly WarehouseContext _context;
        private readonly WarehousesService _service;

        public WarehousesServiceTests()
        {
            //in-memory db options with unique name for each test
            var options = new DbContextOptionsBuilder<WarehouseContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new WarehouseContext(options);
            
            _service = new WarehousesService(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task InitializeDbAsync_ShouldCreateExampleData_WhenDatabaseIsEmpty()
        {
            // Act
            await _service.InitializeDbAsync();

            // Assert
            var warehouses = await _context.Warehouses.ToListAsync();
            var products = await _context.Products.ToListAsync();

            warehouses.Should().HaveCount(2);
            products.Should().HaveCount(3);
            warehouses[0].Name.Should().Be("Main Warehouse");
        }

        [Fact]
        public async Task InitializeDbAsync_ShouldNotDuplicateData_WhenCalledMultipleTimes()
        {
            // Act
            await _service.InitializeDbAsync();
            await _service.InitializeDbAsync();

            // Assert
            var warehouses = await _context.Warehouses.ToListAsync();
            warehouses.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetProductsAsync_ShouldReturnAllProducts_WithWarehouseIncluded()
        {
            // Arrange
            await SeedTestData();

            // Act
            var products = await _service.GetProductsAsync();

            // Assert
            products.Should().HaveCount(2);
            products.Should().AllSatisfy(p => p.Warehouse.Should().NotBeNull());
            products[0].Id.Should().BeLessThan(products[1].Id); // check ascending sorting by ID
        }

        [Fact]
        public async Task GetProductByIdAsync_ShouldReturnProduct_WhenExists()
        {
            // Arrange
            await SeedTestData();
            var existingProduct = await _context.Products.FirstAsync();

            // Act
            var result = await _service.GetProductByIdAsync(existingProduct.Id);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(existingProduct.Id);
            result.Name.Should().Be(existingProduct.Name);
            result.Warehouse.Should().NotBeNull();
        }

        [Fact]
        public async Task GetProductByIdAsync_ShouldReturnNull_WhenNotExists()
        {
            // Act
            var result = await _service.GetProductByIdAsync(999);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task AddProductAsync_ShouldAddProduct_ToContext()
        {
            // Arrange
            var warehouse = new Warehouse
            {
                Name = "Test Warehouse",
                Location = "Test Location",
                CapacityM3 = 1000
            };
            await _context.Warehouses.AddAsync(warehouse);
            await _context.SaveChangesAsync();

            var newProduct = new Product
            {
                Name = "Test Product",
                SKU = "TEST-001",
                Quantity = 10,
                minimumQuantity = 5,
                WarehouseId = warehouse.Id
            };

            // Act
            await _service.AddProductAsync(newProduct);
            await _context.SaveChangesAsync();

            // Assert
            var products = await _context.Products.ToListAsync();
            products.Should().HaveCount(1);
            products[0].Name.Should().Be("Test Product");
        }

        [Fact]
        public async Task DeleteProductAsync_ShouldRemoveProduct_WhenExists()
        {
            // Arrange
            await SeedTestData();
            var productToDelete = await _context.Products.FirstAsync();
            var productId = productToDelete.Id;

            // Act
            await _service.DeleteProductAsync(productId);

            // Assert
            var product = await _context.Products.FindAsync(productId);
            product.Should().BeNull();
        }

        [Fact]
        public async Task GetProductsCountAsync_ShouldReturnCorrectCount()
        {
            // Arrange
            await SeedTestData();

            // Act
            var count = await _service.GetProductsCountAsync();

            // Assert
            count.Should().Be(2);
        }

        [Fact]
        public async Task GetWarehousesAsync_ShouldReturnAllWarehouses()
        {
            // Arrange
            await SeedTestData();

            // Act
            var warehouses = await _service.GetWarehousesAsync();

            // Assert
            warehouses.Should().HaveCount(1);
        }

        [Fact]
        public async Task GetWarehousesCountAsync_ShouldReturnCorrectCount()
        {
            // Arrange
            await SeedTestData();

            // Act
            var count = await _service.GetWarehousesCountAsync();

            // Assert
            count.Should().Be(1);
        }

        // Helper method do tworzenia danych testowych
        private async Task SeedTestData()
        {
            var warehouse = new Warehouse
            {
                Name = "Test Warehouse",
                Location = "Test Location",
                CapacityM3 = 5000
            };

            await _context.Warehouses.AddAsync(warehouse);
            await _context.SaveChangesAsync();

            var products = new[]
            {
                new Product
                {
                    Name = "Product 1",
                    SKU = "SKU-001",
                    Quantity = 50,
                    minimumQuantity = 10,
                    WarehouseId = warehouse.Id
                },
                new Product
                {
                    Name = "Product 2",
                    SKU = "SKU-002",
                    Quantity = 30,
                    minimumQuantity = 5,
                    WarehouseId = warehouse.Id
                }
            };

            await _context.Products.AddRangeAsync(products);
            await _context.SaveChangesAsync();
        }
    }
}