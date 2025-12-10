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
        private double volumePerUnitM3 = 1.0;

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

            //create product object for validation
            var product = new Product
            {
                Name = Name,
                SKU = Sku,
                Quantity = Quantity,
                minimumQuantity = MinimumQuantity,
                VolumePerUnitM3 = VolumePerUnitM3,
                WarehouseId = SelectedWarehouseId
            };

            //validate using data annotations
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(product);
            var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            
            if (!System.ComponentModel.DataAnnotations.Validator.TryValidateObject(product, validationContext, validationResults, true))
            {
                ValidationError = string.Join("\n", validationResults.Select(r => r.ErrorMessage));
                HasValidationError = true;
                return;
            }

            IsLoading = true;
            try
            {
                await _warehouseService.AddProductAsync(product);
                
                SuccessMessage = "Product added successfully!";
                HasSuccess = true;
                
                OnProductAdded?.Invoke();
                ClearForm();
            }
            catch (InvalidOperationException ex)
            {
                // Handle specific validation errors (like duplicate SKU)
                ErrorMessage = ex.Message;
                HasError = true;
            }
            catch (Exception ex)
            {
                // Handle other errors with full exception details
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
            VolumePerUnitM3 = 1.0;
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
