using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagerApp.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        public MainWindowViewModel(
            ProductListViewModel productListViewModel,
            AddProductViewModel addProductViewModel,
            WarehouseListViewModel warehouseListViewModel,
            AddWarehouseViewModel addWarehouseViewModel,
            EditWarehouseViewModel editWarehouseViewModel)
        {
            ProductListViewModel = productListViewModel;
            AddProductViewModel = addProductViewModel;
            WarehouseListViewModel = warehouseListViewModel;
            AddWarehouseViewModel = addWarehouseViewModel;
            EditWarehouseViewModel = editWarehouseViewModel;

            //set initial page
            CurrentView = "Inventory";
            CurrentPageTitle = "Inventory Management";

            //hook up callback for products
            addProductViewModel.OnProductAdded = async () =>
                await productListViewModel.LoadProductsAsync();

            //hook up callback for warehouses - when add warehouse is clicked
            warehouseListViewModel.AddWarehouse = () =>
            {
                ShowAddWarehouseForm = true;
                ShowEditWarehouseForm = false;
            };

            //hook up callback for warehouses - when edit warehouse is clicked
            warehouseListViewModel.EditWarehouse = async (warehouse) =>
            {
                await editWarehouseViewModel.LoadWarehouse(warehouse.Id);
                ShowEditWarehouseForm = true;
                ShowAddWarehouseForm = false;
            };

            //hook up callback - when warehouse is added successfully
            addWarehouseViewModel.OnWarehouseAdded = async () =>
            {
                ShowAddWarehouseForm = false;
                await warehouseListViewModel.LoadWarehousesAsync();
            };

            //hook up callback - when warehouse is updated successfully
            editWarehouseViewModel.OnWarehouseUpdated = async () =>
            {
                ShowEditWarehouseForm = false;
                await warehouseListViewModel.LoadWarehousesAsync();
            };

            //hook up callback - when edit is cancelled
            editWarehouseViewModel.OnCancel = () =>
            {
                ShowEditWarehouseForm = false;
            };
        }

        //viewmodels for controls
        public ProductListViewModel ProductListViewModel { get; }
        public AddProductViewModel AddProductViewModel { get; }
        public WarehouseListViewModel WarehouseListViewModel { get; }
        public AddWarehouseViewModel AddWarehouseViewModel { get; }
        public EditWarehouseViewModel EditWarehouseViewModel { get; }

        //current view name
        [ObservableProperty]
        private string currentView = "Inventory";

        //current page title for header
        [ObservableProperty]
        private string currentPageTitle = "Inventory Management";

        //show add warehouse form
        [ObservableProperty]
        private bool showAddWarehouseForm = false;

        //show edit warehouse form
        [ObservableProperty]
        private bool showEditWarehouseForm = false;

        //navigation commands
        [RelayCommand]
        private void NavigateToDashboard()
        {
            CurrentView = "Dashboard";
            CurrentPageTitle = "Dashboard";
        }

        [RelayCommand]
        private void NavigateToWarehouses()
        {
            CurrentView = "Warehouses";
            CurrentPageTitle = "Warehouses";
        }

        [RelayCommand]
        private void NavigateToInventory()
        {
            CurrentView = "Inventory";
            CurrentPageTitle = "Inventory Management";
        }

        [RelayCommand]
        private void NavigateToAbout()
        {
            CurrentView = "About";
            CurrentPageTitle = "About Application";
        }

        [RelayCommand]
        private void CloseAddWarehouseForm()
        {
            ShowAddWarehouseForm = false;
            AddWarehouseViewModel.ClearForm();
        }
    }
}
