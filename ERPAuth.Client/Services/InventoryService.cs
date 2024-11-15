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
    public async Task<Inventory> AddInventoryEntryAsync(Inventory inventory)
    {
        _context.Inventories.Add(inventory);
        await _context.SaveChangesAsync();
        return inventory;
    }
}
