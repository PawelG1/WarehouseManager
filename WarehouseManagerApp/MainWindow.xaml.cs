using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Extensions.DependencyInjection;
using WarehouseManagerApp.ViewModels;
using WarehouseManagerApp.Views;

namespace WarehouseManagerApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(AddProductViewModel addProductViewModel, ProductListViewModel productListViewModel)
        {
            InitializeComponent();
            
            // Set DataContext for both controls
            AddProductControl.DataContext = addProductViewModel;
            ProductsListControl.DataContext = productListViewModel;
            
            // Hook up the callback
            addProductViewModel.OnProductAdded = async () => await productListViewModel.LoadProductsAsync();
        }
    }
}