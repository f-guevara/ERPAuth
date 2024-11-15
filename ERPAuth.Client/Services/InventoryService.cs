// ERPSystem.UI/Services/InventoryService.cs
using ERPAuth.Client.Models; // Adjust based on the actual namespace for your models
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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

    // Add a new inventory entry
    public async Task AddInventoryEntryAsync(Inventory newInventory)
    {
        try
        {
            // Ensure DateTime fields are UTC
            newInventory.CreatedAt = newInventory.CreatedAt.ToUniversalTime();
            newInventory.ExpirationDate = newInventory.ExpirationDate.ToUniversalTime();

            // Initialize other fields
            newInventory.Reserved = 0;
            newInventory.Sold = 0;

            await _context.Inventories.AddAsync(newInventory);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error in AddInventoryEntryAsync: {ex.Message}");
            throw;
        }
    }

}
