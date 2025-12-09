using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseManagerApp.Models;
using WarehouseManagerApp.Services;

namespace WarehouseManagerApp.ViewModels
{
    public partial class ProductListViewModel : ObservableObject
    {
        private readonly IWarehousesService _warehouseService;

        public ProductListViewModel(IWarehousesService warehouseService)
        {
            _warehouseService = warehouseService;

            //load available warehouses for warehouse selection
            LoadWarehouses();

            //load products from db file
            LoadProductsAsync();
        }

        [ObservableProperty]
        private List<Product>? products;

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

        [ObservableProperty]
        private List<Warehouse> warehouses; //used for warehouse selection in combobox

        partial void OnSelectedWarehouseIdChanged(int? value)
        {
            LoadProductsAsync();
        }

        [RelayCommand]
        public async Task LoadWarehouses()
        {
            IsLoading = true;
            ErrorMessage = null;
            HasError = false;
            try
            {
                var warehousesFromDb = await _warehouseService.GetWarehousesAsync();
                Warehouses = new List<Warehouse> //first "Warehouse" represents all warehouses option
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
                ErrorMessage = ex.Message;
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
                if (SelectedWarehouseId != null 
                    && SelectedWarehouseId != -1) // id == -1 means that products from all warehouses should be shown
                {   
                    //TODO: collect products from selected warehouse
                    Products = await _warehouseService.GetProductsAsync(SelectedWarehouseId??0);
                }
                else
                {
                    Products = await _warehouseService.GetProductsAsync();
                }
                ProductsCount = await _warehouseService.GetProductsCountAsync();
            }
            catch(Exception e)
            {
                ErrorMessage = $"An error occurred while loading products from db: {e.Message}";
                HasError = true;
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
