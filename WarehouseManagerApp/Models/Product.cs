using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagerApp.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Product name must be between 3 and 200 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "SKU is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "SKU must be between 2 and 100 characters")]
        [RegularExpression(@"^[A-Z0-9\-]+$", ErrorMessage = "SKU must contain only uppercase letters, numbers and hyphens")]
        public string SKU { get; set; } = string.Empty;

        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be 0 or greater")]
        public int Quantity { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Minimum quantity must be 0 or greater")]
        public int minimumQuantity { get; set; }

        [Required(ErrorMessage = "Warehouse is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a warehouse")]
        public int WarehouseId { get; set; }

        public Warehouse Warehouse { get; set; } = null!;
    }
}
