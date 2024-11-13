using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPAuth.Data.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; } // Navigation property
        public DateTime OrderDate { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        // Computed property for total pieces
        public int TotalPieces => OrderItems.Sum(item => item.Quantity);

        public decimal TotalAmount => OrderItems.Sum(item => item.Quantity * item.Price);


    }
}
