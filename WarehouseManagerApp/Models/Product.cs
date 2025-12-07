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
        //Foreign Key for Warehouses table
        public int WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; } =null!;
        public int Quantity { get; set; }
        public int minimumQuantity { get; set; }
        public int OccupiedSpaceM3 { get; set; }


        //public enum category

    }
}
