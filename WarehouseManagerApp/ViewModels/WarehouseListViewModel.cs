using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using WarehouseManagerApp.Models;
using WarehouseManagerApp.Services;

namespace WarehouseManagerApp.ViewModels
{
    public partial class WarehouseListViewModel : ObservableObject
    {
        private readonly IWarehousesService _warehouseService;

        public Action? AddWarehouse { get; set; }
        public Action<Warehouse>? EditWarehouse { get; set; }

        [ObservableProperty]
        private ObservableCollection<Warehouse> warehouses = new();

        [ObservableProperty]
        private Warehouse? selectedWarehouse;

        [ObservableProperty]
        private bool isLoading = false;

        public WarehouseListViewModel(IWarehousesService warehouseService)
        {
            _warehouseService = warehouseService;
            _ = LoadWarehousesAsync();
        }

        public async Task LoadWarehousesAsync()
        {
            IsLoading = true;

            try
            {
                var allWarehouses = await _warehouseService.GetWarehousesAsync();
                Warehouses = new ObservableCollection<Warehouse>(allWarehouses);
            }
            catch (Exception)
            {
                // Ignore errors for now
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private void AddWarehouseClick()
        {
            AddWarehouse?.Invoke();
        }

        [RelayCommand]
        private void EditWarehouseClick(Warehouse? warehouse)
        {
            if (warehouse == null) return;
            EditWarehouse?.Invoke(warehouse);
        }

        [RelayCommand]
        private async Task DeleteWarehouse(Warehouse? warehouse)
        {
            if (warehouse == null) return;

            // Potwierdzenie usuniêcia
            var result = MessageBox.Show(
                $"Are you sure you want to delete warehouse '{warehouse.Name}'?\n\n" +
                $"Location: {warehouse.Location}\n" +
                $"This action cannot be undone!",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                IsLoading = true;

                try
                {
                    await _warehouseService.DeleteWarehouseAsync(warehouse.Id);

                    // Odœwie¿ listê
                    await LoadWarehousesAsync();

                    MessageBox.Show(
                        $"Warehouse '{warehouse.Name}' has been deleted successfully!",
                        "Success",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Error deleting warehouse: {ex.Message}",
                        "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
                finally
                {
                    IsLoading = false;
                }
            }
        }
    }
}
