using ERPAuth.Client.Models;
using Microsoft.EntityFrameworkCore;
using System;
using ERPAuth.Client.Data;

public class ProviderService
{
    private readonly ApplicationDbContext _dbContext;

    public ProviderService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // Method to get all providers
    public async Task<List<Provider>> GetAllProvidersAsync()
    {
        return await _dbContext.Providers.OrderBy(p => p.Name).ToListAsync();
    }

    // Additional method to get a provider by ID if needed
    public async Task<Provider?> GetProviderByIdAsync(int id)
    {
        return await _dbContext.Providers.FirstOrDefaultAsync(p => p.Id == id);
    }

    // Optional: Add a new provider to the database
    public async Task AddProviderAsync(Provider provider)
    {
        await _dbContext.Providers.AddAsync(provider);
        await _dbContext.SaveChangesAsync();
    }
}
