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
            AddProductViewModel addProductViewModel)
        {
            ProductListViewModel = productListViewModel;
            AddProductViewModel = addProductViewModel;

            //set initial page
            CurrentView = "Inventory";
            CurrentPageTitle = "Inventory Management";

            //hook up callback
            addProductViewModel.OnProductAdded = async () =>
                await productListViewModel.LoadProductsAsync();
        }

        //viewmodels for controls
        public ProductListViewModel ProductListViewModel { get; }
        public AddProductViewModel AddProductViewModel { get; }

        //current view name
        [ObservableProperty]
        private string currentView = "Inventory";

        //current page title for header
        [ObservableProperty]
        private string currentPageTitle = "Inventory Management";

        //navigation commands
        [RelayCommand]
        private void NavigateToDashboard()
        {
            CurrentView = "Dashboard";
            CurrentPageTitle = "Dashboard";
        }

        [RelayCommand]
        private void NavigateToConfiguration()
        {
            CurrentView = "Configuration";
            CurrentPageTitle = "Configuration";
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
    }
}
