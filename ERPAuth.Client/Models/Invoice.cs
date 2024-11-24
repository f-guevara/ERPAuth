using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPAuth.Client.Models
{
    public class Invoice
    {
        public int Id { get; set; } // Primary key

        // Links to an order or packing list (nullable for standalone invoices)
        public int? OrderId { get; set; } // Nullable: Links to Order if applicable
        public Order Order { get; set; } // Navigation property
        public int? PackingListId { get; set; } // Nullable: Links to PackingList if applicable
        public PackingList PackingList { get; set; } // Navigation property

        // Client reference (always required)
        public int CustomerId { get; set; } // Foreign key to Customer
        public Customer Customer { get; set; } // Navigation property

        // Invoice Details
        public decimal TotalAmount { get; set; } // Total amount for the invoice
        public string Status { get; set; } = "Pending"; // Paid, Partially Paid, Pending
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Client-specific details (carried from order or packing list if linked)
        public string? ClientOrderNumber { get; set; }
        public DateTime? ClientOrderDate { get; set; }
        public string? OrderPlacedBy { get; set; }
        public string? OrderMethod { get; set; }

        // Invoice Items
        public ICollection<InvoiceItem> Items { get; set; } = new List<InvoiceItem>(); // Associated invoice items
    }

}