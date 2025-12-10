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
        public MainWindow(MainWindowViewModel viewModel)
        {
            InitializeComponent();

            //set main window datacontext
            DataContext = viewModel;

            //set datacontext for controls
            AddProductControl.DataContext = viewModel.AddProductViewModel;
            ProductsListControl.DataContext = viewModel.ProductListViewModel;
            
           
        }
    }
}