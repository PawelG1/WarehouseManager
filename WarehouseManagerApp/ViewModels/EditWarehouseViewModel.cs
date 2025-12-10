using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Threading.Tasks;
using WarehouseManagerApp.Models;
using WarehouseManagerApp.Services;

namespace WarehouseManagerApp.ViewModels
{
    public partial class EditWarehouseViewModel : ObservableObject
    {
        private readonly IWarehousesService _warehouseService;
        public Action? OnWarehouseUpdated { get; set; }
        public Action? OnCancel { get; set; }

        [ObservableProperty]
        private int warehouseId;

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

        public EditWarehouseViewModel(IWarehousesService warehouseService)
        {
            _warehouseService = warehouseService;
        }

        public async Task LoadWarehouse(int id)
        {
            IsLoading = true;
            ClearMessages();

            try
            {
                var warehouse = await _warehouseService.GetWarehouseByIdAsync(id);

                if (warehouse != null)
                {
                    WarehouseId = warehouse.Id;
                    Name = warehouse.Name;
                    Location = warehouse.Location;
                    CapacityM3 = warehouse.CapacityM3;
                }
                else
                {
                    ErrorMessage = "Warehouse not found";
                    HasError = true;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error loading warehouse: {ex.Message}";
                HasError = true;
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task UpdateWarehouse()
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
                    Id = WarehouseId,
                    Name = Name.Trim(),
                    Location = Location.Trim(),
                    CapacityM3 = CapacityM3
                };

                await _warehouseService.UpdateWarehouseAsync(warehouse);

                SuccessMessage = "Warehouse updated successfully!";
                HasSuccess = true;

                // Notify that warehouse was updated
                OnWarehouseUpdated?.Invoke();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error updating warehouse: {ex.Message}";
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
            OnCancel?.Invoke();
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
