using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using WarehouseManagerApp.Models;
using WarehouseManagerApp.Services;

namespace WarehouseManagerApp.ViewModels
{
    public partial class DashboardViewModel : ObservableObject
    {
        private readonly IWarehousesService _warehouseService;

        [ObservableProperty]
        private bool _isLoading;

        [ObservableProperty]
        private int _totalWarehouses;

        [ObservableProperty]
        private int _totalProducts;

        [ObservableProperty]
        private int _lowStockItems;

        [ObservableProperty]
        private int _criticalCapacityWarehouses;

        [ObservableProperty]
        private double _averageUtilization;

        [ObservableProperty]
        private ObservableCollection<WarehouseCapacity> _warehouseCapacities = new();

        [ObservableProperty]
        private ObservableCollection<LowStockProduct> _lowStockProducts = new();

        [ObservableProperty]
        private ISeries[] _productDistributionSeries = Array.Empty<ISeries>();

        public DashboardViewModel(IWarehousesService warehouseService)
        {
            _warehouseService = warehouseService;
            _ = LoadStatisticsAsync();
        }

        [RelayCommand]
        public async Task LoadStatisticsAsync()
        {
            IsLoading = true;

            try
            {
                var stats = await _warehouseService.GetDashboardStatisticsAsync();

                TotalWarehouses = stats.TotalWarehouses;
                TotalProducts = stats.TotalProducts;
                LowStockItems = stats.LowStockItems;
                CriticalCapacityWarehouses = stats.CriticalCapacityWarehouses;
                AverageUtilization = stats.AverageWarehouseUtilization;

                WarehouseCapacities = new ObservableCollection<WarehouseCapacity>(stats.WarehouseCapacities);
                LowStockProducts = new ObservableCollection<LowStockProduct>(stats.LowStockProducts.Take(5));

                // Create product distribution pie chart
                ProductDistributionSeries = stats.ProductsByWarehouse.Select(p => new PieSeries<int>
                {
                    Name = p.WarehouseName,
                    Values = new int[] { p.ProductCount },
                    DataLabelsPaint = new SolidColorPaint(SKColors.White),
                    DataLabelsSize = 14,
                    DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle,
                    DataLabelsFormatter = (point) => $"{point.Coordinate.PrimaryValue}"
                }).ToArray<ISeries>();
            }
            catch (Exception)
            {
                // Handle error silently for now
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task Refresh()
        {
            await LoadStatisticsAsync();
        }
    }
}
