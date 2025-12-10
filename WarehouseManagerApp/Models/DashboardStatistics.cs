namespace WarehouseManagerApp.Models
{
    public class DashboardStatistics
    {
        public int TotalWarehouses { get; set; }
        public int TotalProducts { get; set; }
        public int LowStockItems { get; set; }
        public int CriticalCapacityWarehouses { get; set; }
        public double AverageWarehouseUtilization { get; set; }
        public List<WarehouseCapacity> WarehouseCapacities { get; set; } = new();
        public List<ProductDistribution> ProductsByWarehouse { get; set; } = new();
        public List<LowStockProduct> LowStockProducts { get; set; } = new();
    }

    public class WarehouseCapacity
    {
        public string WarehouseName { get; set; } = string.Empty;
        public int CurrentProducts { get; set; }
        public int Capacity { get; set; }
        public double UtilizationPercentage { get; set; }
        public bool IsCritical { get; set; } // Only when > 80% (overfilled)
        public double VolumeOccupiedM3 { get; set; } // Actual volume occupied in m?
    }

    public class ProductDistribution
    {
        public string WarehouseName { get; set; } = string.Empty;
        public int ProductCount { get; set; }
    }

    public class LowStockProduct
    {
        public string ProductName { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public int CurrentQuantity { get; set; }
        public int MinimumQuantity { get; set; }
        public string WarehouseName { get; set; } = string.Empty;
    }
}
