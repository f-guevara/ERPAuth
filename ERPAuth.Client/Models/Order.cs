using ERPAuth.Client.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPAuth.Client.Models
{
    public class Order
    {
        public int Id { get; set; } // Primary key
        public int CustomerId { get; set; } // Foreign key to Client
        public Customer Customer { get; set; } // Navigation property
        public DateTime OrderDate { get; set; } = DateTime.UtcNow; // Date our order was created
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>(); // Associated order items

        // Client-specific details
        public string? ClientOrderNumber { get; set; } // Client's reference number
        public DateTime ClientOrderDate { get; set; } // Date client placed their order
        public string? OrderPlacedBy { get; set; } // Client representative
        public string? OrderMethod { get; set; } // Email, phone, etc.
        // Navigation property for PackingLists
    public ICollection<PackingList> PackingLists { get; set; } = new List<PackingList>();
    }
}

