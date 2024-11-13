using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPAuth.Data.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; } // Navigation property
        public int ArticleId { get; set; }
        public Article Article { get; set; } // Navigation property
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
