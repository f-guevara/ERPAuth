using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPAuth.Client.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public int PackingListId { get; set; }
        public PackingList PackingList { get; set; } // Navigation property
        public DateTime InvoiceDate { get; set; } = DateTime.UtcNow;
        public decimal TotalAmount => InvoiceItems.Sum(item => item.QuantityInvoiced * item.Price);
        public List<InvoiceItem> InvoiceItems { get; set; } = new List<InvoiceItem>();
    }
}