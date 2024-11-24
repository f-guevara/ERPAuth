using ERPAuth.Client.Models;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPAuth.Client.Models
{
    public class InvoiceItem
    {
        public int Id { get; set; } // Primary key

        // Links to Invoice
        public int InvoiceId { get; set; } // Foreign key to Invoice
        public Invoice Invoice { get; set; } // Navigation property

        // Optional link to OrderItem (null for standalone charges)
        public int? OrderItemId { get; set; } // Nullable
        public OrderItem OrderItem { get; set; } // Navigation property

        // Item Details
        public string Description { get; set; } // Free-form description for V100/general charges
        public int Quantity { get; set; } = 1; // Quantity (default is 1 for general charges)
        public decimal Price { get; set; } // Price per unit
        public decimal Total => Quantity * Price; // Computed field for total cost
    }

}
