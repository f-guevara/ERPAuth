using ERPAuth.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPAuth.Client.Models
{
    public class PackingList
    {
        public int Id { get; set; } // Primary key
        public int OrderId { get; set; } // Foreign key to Order
        public Order Order { get; set; } // Navigation property

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<PackingListItem> Items { get; set; } = new List<PackingListItem>();

        // Client-specific details carried from the Order
        public string? ClientOrderNumber { get; set; }
        public DateTime? ClientOrderDate { get; set; }
        public string? OrderPlacedBy { get; set; }
        public string? OrderMethod { get; set; }
    }

}

