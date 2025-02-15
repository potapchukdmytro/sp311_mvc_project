using Microsoft.EntityFrameworkCore;
using sp311_mvc_project.Models;

namespace sp311_mvc_project.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options)
            : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
