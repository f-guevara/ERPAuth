using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPAuth.Data.Models
{
    public class PackingList
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; } // Navigation property
        public DateTime PackingListDate { get; set; } = DateTime.UtcNow;
        public List<PackingListItem> PackingListItems { get; set; } = new List<PackingListItem>();
    }
}
