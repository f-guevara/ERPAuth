using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPAuth.Client.Models
{
    public class Article
    {
        public int Id { get; set; }
        public string CompanyCode { get; set; } // Single source of truth for your company's codes
        public string Name { get; set; }
        public ArticleType Type { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property to link Article to its Inventory entries
        public List<Inventory> Inventories { get; set; } = new List<Inventory>();

    }



    public enum ArticleType
    {
        Screw,
        Plate,
        Instrument,
        Rack,
        Tray,
        StainlessSteelCase,
        Container,
        Filter,
        Label,
        PlasticSecuritySeal,
        Mesh,
        InstrumentHolders,
        SpiralDrill,
        Template,
        Prostheses
    }
}
