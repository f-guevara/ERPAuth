using ERPAuth.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPAuth.Client.Models
{
    public class Inventory
    {
        public int Id { get; set; }
        public int ArticleId { get; set; }
        public Article Article { get; set; } // Navigation property
        public int ProviderId { get; set; }
        public Provider Provider { get; set; } // Navigation property
        public string? ProviderCode { get; set; } // Provider-specific code
        public string LotNumber { get; set; }
        public int InitialQuantity { get; set; } // Historical record of original quantity
        public int TotalQuantity { get; set; } // Current total stock
        public int ReservedQuantity { get; set; } // Reserved but not shipped
        public int AvailableQuantity => TotalQuantity - ReservedQuantity; // Computed property
        public string Location { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ExpirationDate { get; set; }
        public decimal? Cost { get; set; }
    }


}
