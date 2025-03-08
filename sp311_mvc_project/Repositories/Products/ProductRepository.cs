using Microsoft.EntityFrameworkCore;
using sp311_mvc_project.Data;
using sp311_mvc_project.Models;

namespace sp311_mvc_project.Repositories.Products
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Product model)
        {
            await _context.Products.AddAsync(model);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var model = await FindByIdAsync(id);
            if (model != null)
            {
                _context.Products.Remove(model);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Product?> FindByIdAsync(string id)
        {
            return await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public IQueryable<Product> GetAll()
        {
            return _context.Products;
        }

        public IQueryable<Product> GetByCategory(string category)
        {
            return GetAll()
                .Include(p => p.Category)
                .Where(p =>
                p.Category == null ? false
                : p.Category.Name == null ? false
                : p.Category.Name.ToLower() == category.ToLower());
        }

        public async Task UpdateAsync(Product model)
        {
            _context.Products.Update(model);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRangeAsync(IEnumerable<Product> models)
        {
            foreach (var model in models)
            {
                _context.Products.Update(model);
            }

            await _context.SaveChangesAsync();
        }
    }
}
