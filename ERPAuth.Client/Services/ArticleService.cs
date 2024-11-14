// ERPSystem.UI/Services/ArticleService.cs
using ERPAuth.Client.Data;
using ERPAuth.Client.Models;
using Microsoft.EntityFrameworkCore;

public class ArticleService
{
    private readonly ApplicationDbContext _context;

    public ArticleService(ApplicationDbContext context)
    {
        _context = context;
    }

    // Method to retrieve all articles, including their inventory entries
    public async Task<List<Article>> GetAllArticlesAsync()
    {
        return await _context.Articles.Include(a => a.Inventories).ToListAsync();
    }

    // Method to add a new article
    public async Task<Article> AddArticleAsync(Article article)
    {
        _context.Articles.Add(article);
        await _context.SaveChangesAsync();
        return article;
    }

    // Method to add an inventory entry for a specific article
    public async Task<Inventory> AddInventoryEntryAsync(int articleId, Inventory inventory)
    {
        var article = await _context.Articles.FindAsync(articleId);
        if (article != null)
        {
            article.Inventories.Add(inventory);
            await _context.SaveChangesAsync();
        }
        return inventory;
    }
}