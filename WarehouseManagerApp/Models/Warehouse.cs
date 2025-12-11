using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagerApp.Models
{
    public class Warehouse
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Warehouse name is required")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Warehouse name must be between 3 and 200 characters")]
        [RegularExpression(@"^[^!@#$%^&*(){}\[\]<>?/\\]+$", ErrorMessage = "Warehouse name contains some forbbiden characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Location is required")]
        [StringLength(500, MinimumLength = 5, ErrorMessage = "Location must be between 5 and 500 characters")]

        [RegularExpression(@"^[^!@#$%^&*(){}\<>?/\\]+$", ErrorMessage = "Warehouse location contains some forbbiden characters")]
        public string Location { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Capacity must be greater than 0")]
        public int CapacityM3 { get; set; }

        public List<Product> Products { get; set; } = new();
    }
}
