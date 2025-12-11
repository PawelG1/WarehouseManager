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
            EditWarehouseViewModel editWarehouseViewModel,
            DashboardViewModel dashboardViewModel)
        {
            ProductListViewModel = productListViewModel;
            AddProductViewModel = addProductViewModel;
            WarehouseListViewModel = warehouseListViewModel;
            AddWarehouseViewModel = addWarehouseViewModel;
            EditWarehouseViewModel = editWarehouseViewModel;
            DashboardViewModel = dashboardViewModel;

            //set initial page
            CurrentView = "Dashboard";
            CurrentPageTitle = "Dashboard";

            //hook up callback for products - when add product is clicked
            productListViewModel.AddProduct = () =>
            {
                ShowAddProductForm = true;
            };

            //hook up callback for products - when product is added successfully
            addProductViewModel.OnProductAdded = async () =>
            {
                ShowAddProductForm = false;
                await productListViewModel.LoadProductsAsync();
            };

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
                // REFRESH warehouses list in AddProduct form as well
                await addProductViewModel.RefreshWarehousesAsync();
            };

            //hook up callback - when warehouse is updated successfully
            editWarehouseViewModel.OnWarehouseUpdated = async () =>
            {
                ShowEditWarehouseForm = false;
                await warehouseListViewModel.LoadWarehousesAsync();
                // REFRESH warehouses list in AddProduct form as well
                await addProductViewModel.RefreshWarehousesAsync();
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
        public DashboardViewModel DashboardViewModel { get; }

        //current view name
        [ObservableProperty]
        private string currentView = "Inventory";

        //current page title for header
        [ObservableProperty]
        private string currentPageTitle = "Inventory Management";

        //show add product form
        [ObservableProperty]
        private bool showAddProductForm = false;

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

        [RelayCommand]
        private void CloseAddProductForm()
        {
            ShowAddProductForm = false;
        }
    }
}
