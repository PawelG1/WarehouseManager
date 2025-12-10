using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
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

            // Validation
            if (string.IsNullOrWhiteSpace(Name) || Name.Length < 3)
            {
                ValidationError = "Warehouse name must be at least 3 characters";
                HasValidationError = true;
                return;
            }

            if (string.IsNullOrWhiteSpace(Location) || Location.Length < 5)
            {
                ValidationError = "Location must be at least 5 characters";
                HasValidationError = true;
                return;
            }

            if (CapacityM3 <= 0)
            {
                ValidationError = "Capacity must be greater than 0";
                HasValidationError = true;
                return;
            }

            IsLoading = true;

            try
            {
                var warehouse = new Warehouse
                {
                    Name = Name.Trim(),
                    Location = Location.Trim(),
                    CapacityM3 = CapacityM3
                };

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
