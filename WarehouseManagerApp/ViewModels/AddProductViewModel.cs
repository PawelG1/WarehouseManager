using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WarehouseManagerApp.Models;
using WarehouseManagerApp.Services;

namespace WarehouseManagerApp.ViewModels
{
    public partial class AddProductViewModel : ObservableObject
    {
        private readonly IWarehousesService _warehouseService;
        public Action? OnProductAdded { get; set; }

        public AddProductViewModel(IWarehousesService warehouseService)
        {
            _warehouseService = warehouseService;
            LoadWarehousesAsync();
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
        private string? successMessage;

        [ObservableProperty]
        private string? validationError;

        //bool properties for visibility binding
        [ObservableProperty]
        private bool hasError;

        [ObservableProperty]
        private bool hasSuccess;

        [ObservableProperty]
        private bool hasValidationError;

        private async Task LoadWarehousesAsync()
        {
            IsLoading = true;
            ClearMessages();
            try
            {
                Warehouses = await _warehouseService.GetWarehousesAsync();
                if (Warehouses?.Count > 0)
                {
                    SelectedWarehouseId = Warehouses[0].Id;
                }
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
        private async Task AddProductAsync()
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
                    Name = Name,
                    SKU = Sku,
                    Quantity = Quantity,
                    minimumQuantity = MinimumQuantity,
                    WarehouseId = SelectedWarehouseId
                };

                await _warehouseService.AddProductAsync(product);
                
                SuccessMessage = "Product added successfully!";
                HasSuccess = true;
                
                OnProductAdded?.Invoke();
                ClearForm();
            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;
                var innerEx = ex.InnerException;
                while (innerEx != null)
                {
                    errorMessage += $"\n\nInner Exception: {innerEx.Message}";
                    innerEx = innerEx.InnerException;
                }
                
                ErrorMessage = $"Error adding product:\n\n{errorMessage}";
                HasError = true;
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private void ClearForm()
        {
            Name = string.Empty;
            Sku = string.Empty;
            Quantity = 0;
            MinimumQuantity = 0;
            ClearMessages();
            if (Warehouses?.Count > 0)
            {
                SelectedWarehouseId = Warehouses[0].Id;
            }
        }

        private void ClearMessages()
        {
            ErrorMessage = null;
            SuccessMessage = null;
            ValidationError = null;
            HasError = false;
            HasSuccess = false;
            HasValidationError = false;
        }
    }
}
