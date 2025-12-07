using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagerApp.Models
{
    class Warehouse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int CapacityM3 { get; set; }

        public List<Product> Products { get; set; } = new List<Product>();
        //TODO: change list of products to query to get them, and then only cache them

    }
}
