using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using WarehouseManagerApp.ViewModels;

namespace WarehouseManagerApp.Views
{
    public partial class EditProductWindow : Window
    {
        public EditProductWindow(int productId)
        {
            InitializeComponent();
            
            var app = (App)Application.Current;
            var warehouseService = app.ServiceProvider.GetRequiredService<Services.IWarehousesService>();
            
            var viewModel = new EditProductViewModel(warehouseService, productId);
            
            //close window on success
            viewModel.OnProductUpdated = () =>
            {
                DialogResult = true;
                Close();
            };
            
            //close window on cancel
            viewModel.OnCancelRequested = () =>
            {
                DialogResult = false;
                Close();
            };
            
            DataContext = viewModel;
        }
    }
}
