using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WarehouseManagerApp.Models;
using WarehouseManagerApp.Services;

namespace WarehouseManagerApp.ViewModels
{
    public partial class AddWarehouseViewModel : ObservableObject
    {
        private readonly IWarehousesService _warehouseService;
        public Action? OnWarehouseAdded { get; set; }

        [ObservableProperty]
        private string name = string.Empty;

        [ObservableProperty]
        private string location = string.Empty;

        [ObservableProperty]
        private int capacityM3 = 1000;

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private string? errorMessage;

        [ObservableProperty]
        private string? successMessage;

        [ObservableProperty]
        private string? validationError;

        [ObservableProperty]
        private bool hasError;

        [ObservableProperty]
        private bool hasSuccess;

        [ObservableProperty]
        private bool hasValidationError;

        public AddWarehouseViewModel(IWarehousesService warehouseService)
        {
            _warehouseService = warehouseService;
        }

        [RelayCommand]
        private async Task AddWarehouse()
        {
            ClearMessages();

            // Create warehouse object for validation
            var warehouse = new Warehouse
            {
                Name = Name.Trim(),
                Location = Location.Trim(),
                CapacityM3 = CapacityM3
            };

            // Validate using data annotations
            var validationContext = new ValidationContext(warehouse);
            var validationResults = new List<ValidationResult>();

            if (!Validator.TryValidateObject(warehouse, validationContext, validationResults, true))
            {
                ValidationError = string.Join("\n", validationResults.Select(r => r.ErrorMessage));
                HasValidationError = true;
                return;
            }

            IsLoading = true;

            try
            {
                await _warehouseService.AddWarehouseAsync(warehouse);

                SuccessMessage = "Warehouse added successfully!";
                HasSuccess = true;

                ClearFormInternal();
                
                // Notify that warehouse was added
                OnWarehouseAdded?.Invoke();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error adding warehouse: {ex.Message}";
                HasError = true;
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        public void ClearForm()
        {
            ClearFormInternal();
        }

        private void ClearFormInternal()
        {
            Name = string.Empty;
            Location = string.Empty;
            CapacityM3 = 1000;
            ClearMessages();
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
