using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseManagerApp.Models;
using WarehouseManagerApp.Services;

namespace WarehouseManagerApp.ViewModels
{
    public partial class ProductListViewModel : ObservableObject
    {
        private readonly IWarehousesService _warehouseService;

        public Action? AddProduct { get; set; }

        public ProductListViewModel(IWarehousesService warehouseService)
        {
            _warehouseService = warehouseService;

            //load available warehouses for warehouse selection
            LoadWarehousesAsync();

            //load products from db file
            LoadProductsAsync();
        }

        [ObservableProperty]
        private List<Product>? products;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteProductCommand))]
        [NotifyCanExecuteChangedFor(nameof(EditProductCommand))]
        private Product? selectedProduct; //selected product in listview

        [ObservableProperty]
        private string searchText = string.Empty; //search filter text

        partial void OnSearchTextChanged(string value)
        {
            FilterProducts();
        }

        [ObservableProperty]
        private List<Warehouse>? warehouses;

        [ObservableProperty]
        private int? selectedWarehouseId;

        [ObservableProperty]
        private int productsCount;

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private string? errorMessage;

        [ObservableProperty]
        private bool hasError;

        private List<Product>? _allProducts; //cache of all products for filtering

        partial void OnSelectedWarehouseIdChanged(int? value)
        {
            FilterProducts();
        }

        [RelayCommand]
        private async Task LoadWarehousesAsync()
        {
            IsLoading = true;
            ErrorMessage = null;
            HasError = false;
            try
            {
                var warehousesFromDb = await _warehouseService.GetWarehousesAsync();
                Warehouses = new List<Warehouse> //first warehouse represents "all warehouses" option
                {
                    new Warehouse{
                    Id= -1,
                    Name= "All Warehouses"
                    }
                };
                Warehouses.AddRange(warehousesFromDb);
                SelectedWarehouseId = -1; //all warehouses as default selection
                
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error loading warehouses: {ex.Message}";
                HasError = true;
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        public async Task LoadProductsAsync()
        {
            IsLoading = true;
            ErrorMessage = null;
            HasError = false;
            try
            {
                //load all products to cache
                _allProducts = await _warehouseService.GetProductsAsync();
                
                //apply filters
                FilterProducts();
            }
            catch(Exception e)
            {
                ErrorMessage = $"An error occurred while loading products: {e.Message}";
                HasError = true;
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void FilterProducts()
        {
            if (_allProducts == null)
            {
                Products = null;
                ProductsCount = 0;
                return;
            }

            var filtered = _allProducts.AsEnumerable();

            //filter by warehouse
            if (SelectedWarehouseId != null && SelectedWarehouseId != -1)
            {
                filtered = filtered.Where(p => p.WarehouseId == SelectedWarehouseId.Value);
            }

            //filter by search text
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var search = SearchText.ToLower();
                filtered = filtered.Where(p =>
                    p.Name.ToLower().Contains(search) ||
                    p.SKU.ToLower().Contains(search) ||
                    p.Warehouse?.Name.ToLower().Contains(search) == true
                );
            }

            Products = filtered.ToList();
            ProductsCount = Products.Count;
        }

        //delete product
        [RelayCommand]
        private async Task DeleteProduct(Product? product)
        {
            if (product == null) return;

            // Confirmation dialog
            var result = System.Windows.MessageBox.Show(
                $"Are you sure you want to delete product '{product.Name}'?\n\n" +
                $"SKU: {product.SKU}\n" +
                $"Warehouse: {product.Warehouse?.Name}\n\n" +
                $"This action cannot be undone!",
                "Confirm Delete",
                System.Windows.MessageBoxButton.YesNo,
                System.Windows.MessageBoxImage.Warning);

            if (result == System.Windows.MessageBoxResult.Yes)
            {
                IsLoading = true;

                try
                {
                    await _warehouseService.DeleteProductAsync(product.Id);
                    await LoadProductsAsync(); //refresh product list

                    System.Windows.MessageBox.Show(
                        $"Product '{product.Name}' has been deleted successfully!",
                        "Success",
                        System.Windows.MessageBoxButton.OK,
                        System.Windows.MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    ErrorMessage = $"Error deleting product: {ex.Message}";
                    HasError = true;
                    
                    System.Windows.MessageBox.Show(
                        $"Error deleting product: {ex.Message}",
                        "Error",
                        System.Windows.MessageBoxButton.OK,
                        System.Windows.MessageBoxImage.Error);
                }
                finally
                {
                    IsLoading = false;
                }
            }
        }

        //edit product
        [RelayCommand]
        private void EditProduct(Product? product)
        {
            if (product == null) return;
            
            var editWindow = new Views.EditProductWindow(product.Id);
            editWindow.Owner = System.Windows.Application.Current.MainWindow;
            
            if (editWindow.ShowDialog() == true)
            {
                //refresh list after successful edit
                _ = LoadProductsAsync();
            }
        }

        //add product
        [RelayCommand]
        private void AddProductClick()
        {
            AddProduct?.Invoke();
        }

        [RelayCommand]
        private void ClearSearch()
        {
            SearchText = string.Empty;
        }
    }
}
