namespace ERPAuth.Client.Models
{
    public class PackingListItemViewModel
    {
        public int OrderItemId { get; set; }
        public string ArticleName { get; set; }
        public int Quantity { get; set; }
        public int QuantityShipped { get; set; }
        public int QuantityToShip { get; set; } = 0;
        public string? LotNumber { get; set; }
        public int? InventoryId { get; set; }
    }


}

