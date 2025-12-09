using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseManagerApp.Models;

namespace WarehouseManagerApp.Services
{
    interface IWarehouseService
    {
        Task InitializeDbAsync();
        Task<List<Product>> GetProductsAsync();
        Task<List<Warehouse>> GetWarehousesAsync();
        Task<Product?> GetProductByIdAsync(int id);
        Task AddProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(int id);
        Task<int> GetProductsCountAsync();
        Task<int> GetWarehousesCountAsync();
    }
}
