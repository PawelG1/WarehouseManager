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
        private void AddWarehouse()
        {
            MessageBox.Show("Add Warehouse - Coming in next step!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        [RelayCommand]
        private void EditWarehouse(Warehouse? warehouse)
        {
            if (warehouse == null) return;
            MessageBox.Show($"Edit Warehouse: {warehouse.Name} - Coming in next step!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        [RelayCommand]
        private void DeleteWarehouse(Warehouse? warehouse)
        {
            if (warehouse == null) return;
            MessageBox.Show($"Delete Warehouse: {warehouse.Name} - Coming in next step!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
