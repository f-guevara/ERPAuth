using ERPAuth.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPAuth.Client.Models
{
    public class PackingListItem
    {
        public int Id { get; set; }
        public int PackingListId { get; set; }
        public PackingList PackingList { get; set; } // Navigation property
        public int OrderItemId { get; set; }
        public OrderItem OrderItem { get; set; } // Navigation property
        public int QuantityPacked { get; set; }
        public decimal Price { get; set; }
    }
}
