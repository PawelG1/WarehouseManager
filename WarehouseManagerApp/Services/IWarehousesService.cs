using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseManagerApp.Models;

namespace WarehouseManagerApp.Services
{
    public interface IWarehousesService
    {
        Task InitializeDbAsync();
        Task<List<Product>> GetProductsAsync();
        Task<List<Product>> GetProductsAsync(int warehouseId);
        Task<List<Warehouse>> GetWarehousesAsync();
        Task<Product?> GetProductByIdAsync(int id);
        Task<Warehouse?> GetWarehouseByIdAsync(int id);
        Task AddProductAsync(Product product);
        Task AddWarehouseAsync(Warehouse warehouse);
        Task UpdateProductAsync(Product product);
        Task UpdateWarehouseAsync(Warehouse warehouse);
        Task DeleteProductAsync(int id);
        Task DeleteWarehouseAsync(int id);
        Task<int> GetProductsCountAsync();
        Task<int> GetProductsCountAsync(int warehouseId);
        Task<int> GetWarehousesCountAsync();
    }
}
