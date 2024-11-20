using ERPAuth.Client.Models;
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
        public int CustomerId { get; set; } // Foreign key to Customer
        public Customer Customer { get; set; } // Navigation property
        public DateTime OrderDate { get; set; } = DateTime.UtcNow; // Default to current date
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>(); // Associated items
    }
}

