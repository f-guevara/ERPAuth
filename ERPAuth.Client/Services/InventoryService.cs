// ERPSystem.UI/Services/InventoryService.cs
using ERPAuth.Client.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERPAuth.Client.Data;

public class InventoryService
{
    private readonly ApplicationDbContext _context;

    public InventoryService(ApplicationDbContext context)
    {
        _context = context;
    }

    // Retrieve all inventory entries, optionally with their associated articles
    public async Task<List<Inventory>> GetAllInventoryAsync()
    {
        return await _context.Inventories.Include(i => i.Article).ToListAsync();
    }

    public async Task<List<Inventory>> GetAllAvailableInventoryAsync()
    {
        // Fetch all inventory where available quantity is greater than zero
        return await _context.Inventories
            .Where(i => i.TotalQuantity - i.ReservedQuantity > 0) // Updated to remove `Sold`
            .ToListAsync();
    }

    // Add a new inventory entry
    public async Task AddInventoryEntryAsync(Inventory newInventory)
    {
        try
        {
            // Ensure DateTime fields are UTC
            newInventory.CreatedAt = newInventory.CreatedAt.ToUniversalTime();
            newInventory.ExpirationDate = newInventory.ExpirationDate.ToUniversalTime();

            // Initialize reserved quantity
            newInventory.ReservedQuantity = 0;

            await _context.Inventories.AddAsync(newInventory);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error in AddInventoryEntryAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<Inventory>> GetAvailableInventoryForArticle(int articleId)
    {
        return _context.Inventories
            .Where(i => i.ArticleId == articleId) // Fetch relevant inventory
            .AsEnumerable() // Switch to client-side evaluation
            .Where(i => i.TotalQuantity - i.ReservedQuantity > 0) // Updated to remove `Sold`
            .Select(i => new Inventory
            {
                Id = i.Id,
                ArticleId = i.ArticleId,
                LotNumber = i.LotNumber,
                TotalQuantity = i.TotalQuantity,
                ReservedQuantity = i.ReservedQuantity,
                Location = i.Location,
                Cost = i.Cost,
                ExpirationDate = i.ExpirationDate,
                CreatedAt = i.CreatedAt
            });
    }

    public async Task ReserveInventoryAsync(int inventoryId, int quantity)
    {
        var inventory = await _context.Inventories.FindAsync(inventoryId);

        if (inventory == null)
        {
            throw new Exception("Inventory not found.");
        }

        int availableQuantity = inventory.TotalQuantity - inventory.ReservedQuantity;

        if (availableQuantity < quantity)
        {
            throw new Exception("Insufficient available inventory.");
        }

        inventory.ReservedQuantity += quantity;
        await _context.SaveChangesAsync();
    }

}
