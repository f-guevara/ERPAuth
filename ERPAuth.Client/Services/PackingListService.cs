using ERPAuth.Client.Data;
using ERPAuth.Client.Models;
using Microsoft.EntityFrameworkCore;


namespace ERPAuth.Client.Services
{
    public class PackingListService
    {
        private readonly ApplicationDbContext _context;

        public PackingListService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Create a packing list for an order
        public async Task<PackingList> CreatePackingListAsync(int orderId, List<PackingListItem> packingListItems)
        {
            var order = await _context.Orders
                .Include(o => o.Items)
                .ThenInclude(oi => oi.Inventory) // Include Inventory for updates
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
                throw new Exception("Order not found.");

            var packingList = new PackingList
            {
                OrderId = order.Id,
                ClientOrderNumber = order.ClientOrderNumber,
                ClientOrderDate = order.ClientOrderDate,
                OrderPlacedBy = order.OrderPlacedBy,
                OrderMethod = order.OrderMethod,
                CreatedAt = DateTime.UtcNow,
                Items = packingListItems
            };

            foreach (var packingItem in packingListItems)
            {
                var orderItem = order.Items.FirstOrDefault(oi => oi.Id == packingItem.OrderItemId);
                if (orderItem == null)
                    throw new Exception($"OrderItem with ID {packingItem.OrderItemId} not found.");

                var remainingQuantity = orderItem.Quantity - orderItem.QuantityShipped;
                if (packingItem.QuantityShipped > remainingQuantity)
                {
                    throw new Exception($"Shipped quantity exceeds remaining quantity for OrderItemId: {packingItem.OrderItemId}.");
                }

                // Update OrderItem.QuantityShipped
                orderItem.QuantityShipped += packingItem.QuantityShipped;

                // Update Inventory quantities
                if (orderItem.Inventory != null)
                {
                    var inventory = orderItem.Inventory;

                    // Ensure enough stock is available
                    var availableQuantity = inventory.TotalQuantity - inventory.ReservedQuantity;
                    if (packingItem.QuantityShipped > availableQuantity)
                    {
                        throw new Exception($"Insufficient inventory for lot {inventory.LotNumber}. Available: {availableQuantity}.");
                    }

                    // Reduce TotalQuantity and ReservedQuantity
                    inventory.TotalQuantity -= packingItem.QuantityShipped;
                    inventory.ReservedQuantity -= packingItem.QuantityShipped;

                    // Mark Inventory as Modified
                    _context.Entry(inventory).State = EntityState.Modified;
                }
            }

            // Save Packing List and update OrderItems and Inventories
            _context.PackingLists.Add(packingList);
            await _context.SaveChangesAsync();

            return packingList;
        }






    }

}
