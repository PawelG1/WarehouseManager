using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagerApp.Models
{
    class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SKU { get; set; }
        public int Quantity { get; set; }
        public int minimumQuantity { get; set; }
        //Foreign Key for Warehouses table
        public int WarehouseId { get; set; }

        public Warehouse Warehouse { get; set; } =null!;

        //public enum category

    }
}
