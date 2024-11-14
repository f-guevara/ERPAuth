using ERPAuth.Client.Models;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPAuth.Client.Models
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
