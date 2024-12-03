using ERPAuth.Client.Data;
using ERPAuth.Client.Models;
using Microsoft.EntityFrameworkCore;

public class OrderService
{
    private readonly ApplicationDbContext _context;

    public OrderService(ApplicationDbContext context)
    {
        _context = context;
    }

    // Method to retrieve all orders, including their items and customer details
    public async Task<List<Order>> GetAllOrdersAsync()
    {
        return await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.Items)
            .ToListAsync();
    }

    // Method to retrieve a specific order by ID, including its items and customer
    public async Task<Order?> GetOrderByIdAsync(int orderId)
    {
        return await _context.Orders
            .Include(o => o.Customer) // Keep Customer if used in the UI
            .Include(o => o.Items)
                .ThenInclude(oi => oi.Article) // Include Article to prevent null reference
            .FirstOrDefaultAsync(o => o.Id == orderId);
    }


    // Method to add a new order
    public async Task<Order> AddOrderAsync(Order order)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        return order;
    }

    // Method to update an existing order
    public async Task<Order?> UpdateOrderAsync(int orderId, Order updatedOrder)
    {
        var existingOrder = await _context.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (existingOrder != null)
        {
            // Update order properties
            existingOrder.CustomerId = updatedOrder.CustomerId;
            existingOrder.OrderDate = updatedOrder.OrderDate;

            // Update order items (optional)
            _context.OrderItems.RemoveRange(existingOrder.Items);
            existingOrder.Items = updatedOrder.Items;

            await _context.SaveChangesAsync();
        }

        return existingOrder;
    }

    public async Task UpdateOrderItemAsync(OrderItem item)
    {
        var existingItem = await _context.OrderItems.FindAsync(item.Id);

        if (existingItem != null)
        {
            // Update fields
            existingItem.DeliveryDate = item.DeliveryDate;
            existingItem.InventoryId = item.InventoryId;
            existingItem.Quantity = item.Quantity;
            existingItem.Price = item.Price;

            await _context.SaveChangesAsync();
        }
        else
        {
            throw new Exception("OrderItem not found.");
        }
    }

    public async Task DeleteOrderAsync(int orderId)
    {
        var order = await _context.Orders
            .Include(o => o.Items) // Ensure related items are included
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order != null)
        {
            _context.OrderItems.RemoveRange(order.Items); // Delete related items
            _context.Orders.Remove(order); // Delete the order itself
            await _context.SaveChangesAsync(); // Persist changes
        }
        else
        {
            Console.WriteLine($"Order with Id {orderId} not found.");
        }
    }




}
