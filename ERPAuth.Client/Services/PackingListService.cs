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

            // Filter out items without assigned lots
            var validPackingItems = packingListItems
                .Where(p => p.OrderItemId != 0 && order.Items.Any(oi => oi.Id == p.OrderItemId && oi.InventoryId != null))
                .ToList();

            if (!validPackingItems.Any())
                throw new Exception("No items with assigned lots to include in the packing list.");

            // Create the packing list
            var packingList = new PackingList
            {
                OrderId = order.Id,
                ClientOrderNumber = order.ClientOrderNumber,
                ClientOrderDate = order.ClientOrderDate,
                OrderPlacedBy = order.OrderPlacedBy,
                OrderMethod = order.OrderMethod,
                CreatedAt = DateTime.UtcNow,
                Items = validPackingItems
            };

            foreach (var packingItem in validPackingItems)
            {
                var orderItem = order.Items.FirstOrDefault(oi => oi.Id == packingItem.OrderItemId);
                if (orderItem == null)
                    throw new Exception($"OrderItem with ID {packingItem.OrderItemId} not found.");

                var inventory = orderItem.Inventory;
                if (inventory == null)
                    throw new Exception($"Inventory not found for OrderItem ID {packingItem.OrderItemId}.");

                var remainingQuantity = orderItem.Quantity - orderItem.QuantityShipped;
                if (packingItem.QuantityShipped > remainingQuantity)
                {
                    throw new Exception($"Shipped quantity exceeds remaining quantity for OrderItemId: {packingItem.OrderItemId}.");
                }

                // Update OrderItem.QuantityShipped
                orderItem.QuantityShipped += packingItem.QuantityShipped;

                // Update Inventory.TotalQuantity and Reserved
                if (inventory != null)
                {
                    inventory.TotalQuantity -= packingItem.QuantityShipped;
                    inventory.ReservedQuantity -= packingItem.QuantityShipped;

                    if (inventory.TotalQuantity < 0 || inventory.ReservedQuantity < 0)
                    {
                        throw new Exception($"Inventory for Lot {inventory.LotNumber} is invalid: TotalQuantity={inventory.TotalQuantity}, Reserved={inventory.ReservedQuantity}.");
                    }

                    _context.Entry(inventory).State = EntityState.Modified;
                }
            }

            _context.PackingLists.Add(packingList);
            await _context.SaveChangesAsync();

            return packingList;
        }

        public async Task DeletePackingListAsync(int packingListId)
        {
            // Fetch the packing list along with its items and related inventory
            var packingList = await _context.PackingLists
                .Include(pl => pl.Items)
                .ThenInclude(pli => pli.OrderItem)
                .ThenInclude(oi => oi.Inventory)
                .FirstOrDefaultAsync(pl => pl.Id == packingListId);

            if (packingList == null)
            {
                throw new Exception("Packing list not found.");
            }

            // Adjust inventory for each packing list item
            foreach (var item in packingList.Items)
            {
                if (item.OrderItem?.Inventory != null)
                {
                    // Return shipped quantities to inventory
                    item.OrderItem.Inventory.TotalQuantity += item.QuantityShipped;
                    item.OrderItem.Inventory.ReservedQuantity -= item.QuantityShipped;

                    if (item.OrderItem.Inventory.ReservedQuantity < 0)
                    {
                        throw new Exception($"ReservedQuantity for inventory ID {item.OrderItem.Inventory.Id} went below zero. Data may be corrupted.");
                    }

                    // Mark the inventory as modified
                    _context.Entry(item.OrderItem.Inventory).State = EntityState.Modified;

                    // Adjust the OrderItem quantities
                    item.OrderItem.QuantityShipped -= item.QuantityShipped;
                    _context.Entry(item.OrderItem).State = EntityState.Modified;
                }
            }

            // Remove the packing list and its items
            _context.PackingListItems.RemoveRange(packingList.Items);
            _context.PackingLists.Remove(packingList);

            // Save changes
            await _context.SaveChangesAsync();
        }









    }

}
