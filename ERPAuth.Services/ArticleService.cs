using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPAuth.Client.Data;
using ERPAuth.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ERPAuth.Services
{
    public class ArticleService
    {
        private readonly ApplicationDbContext _context;

        public ArticleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Article>> GetAllArticlesAsync()
        {
            return await _context.Articles.Include(a => a.Inventories).ToListAsync();
        }

        public async Task<Article> AddArticleAsync(Article article)
        {
            _context.Articles.Add(article);
            await _context.SaveChangesAsync();
            return article;
        }

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
}
