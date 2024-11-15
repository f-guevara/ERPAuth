using ERPAuth.Client.Models;
using Microsoft.EntityFrameworkCore;
using System;
using ERPAuth.Client.Data;

public class CustomerService
{
    private readonly ApplicationDbContext _dbContext;

    public CustomerService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // Add a new customer
    public async Task AddCustomerAsync(Customer customer)
    {
        // Ensure CreatedAt is in UTC
        customer.CreatedAt = customer.CreatedAt.ToUniversalTime();

        await _dbContext.Customers.AddAsync(customer);
        await _dbContext.SaveChangesAsync();
    }

    // Get all customers
    public async Task<List<Customer>> GetAllCustomersAsync()
    {
        return await _dbContext.Customers.OrderBy(c => c.LastName).ToListAsync();
    }

    // Get a customer by ID
    public async Task<Customer?> GetCustomerByIdAsync(int id)
    {
        return await _dbContext.Customers.FirstOrDefaultAsync(c => c.Id == id);
    }

    // Update an existing customer
    public async Task UpdateCustomerAsync(Customer customer)
    {
        _dbContext.Customers.Update(customer);
        await _dbContext.SaveChangesAsync();
    }

    // Delete a customer
    public async Task DeleteCustomerAsync(int id)
    {
        var customer = await GetCustomerByIdAsync(id);
        if (customer != null)
        {
            _dbContext.Customers.Remove(customer);
            await _dbContext.SaveChangesAsync();
        }
    }
}
