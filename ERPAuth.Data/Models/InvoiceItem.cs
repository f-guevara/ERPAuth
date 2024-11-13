using ERPAuth.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPAuth.Data.Models
{
    public class InvoiceItem
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public Invoice Invoice { get; set; } // Navigation property
        public int PackingListItemId { get; set; }
        public PackingListItem PackingListItem { get; set; } // Navigation property
        public int QuantityInvoiced { get; set; }
        public decimal Price { get; set; }
    }
}
