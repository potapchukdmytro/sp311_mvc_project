using sp311_mvc_project.Models;

namespace sp311_mvc_project.Repositories.Products
{
    public interface IProductRepository
    {
        Task CreateAsync(Product model);
        Task UpdateAsync(Product model);
        Task DeleteAsync(string id);
        Task<Product?> FindByIdAsync(string id);
        IQueryable<Product> GetAll();
        IQueryable<Product> GetByCategory(string category);
    }
}
