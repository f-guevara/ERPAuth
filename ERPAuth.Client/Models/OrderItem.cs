using ERPAuth.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPAuth.Client.Models
{
    public class OrderItem
    {
        public int Id { get; set; } // Primary key
        public int OrderId { get; set; } // Foreign key to Order
        public Order Order { get; set; } // Navigation property
        public string CompanyCode { get; set; } // Links to inventory by company code
        public int Quantity { get; set; } // Quantity required
        public decimal Price { get; set; } // Price per unit
        public DateTime DeliveryDate { get; set; } = DateTime.UtcNow.AddDays(7); // Nullable delivery date
        public int ArticleId { get; set; } // Links to the article
        public Article Article { get; set; } // Navigation property
        public int? InventoryId { get; set; } // Nullable, links to a specific inventory lot
        public Inventory Inventory { get; set; } // Navigation property

        // Fields for packing and shipping
        public int QuantityShipped { get; set; } = 0; // Total quantity shipped
        public int QuantityRemaining => Quantity - QuantityShipped; // Computed field
    }
}
