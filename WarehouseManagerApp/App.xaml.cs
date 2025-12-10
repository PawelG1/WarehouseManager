using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using WarehouseManagerApp.Data;
using WarehouseManagerApp.Services;
using WarehouseManagerApp.ViewModels;

namespace WarehouseManagerApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            
            ServiceProvider = serviceCollection.BuildServiceProvider();

            //init db
            var warehouseService = ServiceProvider.GetRequiredService<IWarehousesService>();
            warehouseService.InitializeDbAsync().Wait();

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            //register DbContext 
            services.AddDbContext<WarehouseContext>();
            //register services
            services.AddSingleton<IWarehousesService, WarehousesService>();
            //register ViewModels
            services.AddTransient<ProductListViewModel>();
            services.AddTransient<AddProductViewModel>();
            services.AddTransient<WarehouseListViewModel>();
            services.AddTransient<MainWindowViewModel>();
            //register windows
            services.AddSingleton<MainWindow>();
        }
    }

}
