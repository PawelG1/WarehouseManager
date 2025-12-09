using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WarehouseManagerApp.Models;
using WarehouseManagerApp.Services;

namespace WarehouseManagerApp.ViewModels
{
    public partial class EditProductViewModel : ObservableObject
    {
        private readonly IWarehousesService _warehouseService;
        private readonly int _productId;
        public Action? OnProductUpdated { get; set; }
        public Action? OnCancelRequested { get; set; }

        public EditProductViewModel(IWarehousesService warehouseService, int productId)
        {
            _warehouseService = warehouseService;
            _productId = productId;
            _ = LoadDataAsync();
        }

        [ObservableProperty]
        private string name = string.Empty;

        [ObservableProperty]
        private string sku = string.Empty;

        [ObservableProperty]
        private int quantity;

        [ObservableProperty]
        private int minimumQuantity;

        [ObservableProperty]
        private int selectedWarehouseId;

        [ObservableProperty]
        private List<Warehouse>? warehouses;

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private string? errorMessage;

        [ObservableProperty]
        private string? validationError;

        //bool properties for visibility binding
        [ObservableProperty]
        private bool hasError;

        [ObservableProperty]
        private bool hasValidationError;

        private async Task LoadDataAsync()
        {
            IsLoading = true;
            ClearMessages();
            try
            {
                //load warehouses
                Warehouses = await _warehouseService.GetWarehousesAsync();
                
                //load product data
                var product = await _warehouseService.GetProductByIdAsync(_productId);
                if (product != null)
                {
                    Name = product.Name;
                    Sku = product.SKU;
                    Quantity = product.Quantity;
                    MinimumQuantity = product.minimumQuantity;
                    SelectedWarehouseId = product.WarehouseId;
                }
                else
                {
                    ErrorMessage = "Product not found.";
                    HasError = true;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error loading product: {ex.Message}";
                HasError = true;
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task UpdateProductAsync()
        {
            ClearMessages();

            //validation
            if (string.IsNullOrWhiteSpace(Name))
            {
                ValidationError = "Product name is required.";
                HasValidationError = true;
                return;
            }

            if (string.IsNullOrWhiteSpace(Sku))
            {
                ValidationError = "SKU is required.";
                HasValidationError = true;
                return;
            }

            if (SelectedWarehouseId == 0)
            {
                ValidationError = "Please select a warehouse.";
                HasValidationError = true;
                return;
            }

            IsLoading = true;
            try
            {
                var product = new Product
                {
                    Id = _productId,
                    Name = Name,
                    SKU = Sku,
                    Quantity = Quantity,
                    minimumQuantity = MinimumQuantity,
                    WarehouseId = SelectedWarehouseId
                };

                await _warehouseService.UpdateProductAsync(product);
                
                OnProductUpdated?.Invoke();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error updating product: {ex.Message}";
                HasError = true;
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private void Cancel()
        {
            OnCancelRequested?.Invoke();
        }

        private void ClearMessages()
        {
            ErrorMessage = null;
            ValidationError = null;
            HasError = false;
            HasValidationError = false;
        }
    }
}
