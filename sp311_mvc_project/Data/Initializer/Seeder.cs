using Microsoft.EntityFrameworkCore;
using sp311_mvc_project.Models;

namespace sp311_mvc_project.Data.Initializer
{
    public static class Seeder
    {
        public static void Seed(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                context.Database.Migrate();

                if(!context.Categories.Any())
                {
                    var categories = new List<Category>
                    {
                        new() {Id = Guid.NewGuid().ToString(), Name = "Процесори"},
                        new() {Id = Guid.NewGuid().ToString(), Name = "Відеокарти"},
                        new() {Id = Guid.NewGuid().ToString(), Name = "Материнські плати"},
                        new() {Id = Guid.NewGuid().ToString(), Name = "Оперативна пам'ять"},
                        new() {Id = Guid.NewGuid().ToString(), Name = "Блоки живилення"},
                        new() {Id = Guid.NewGuid().ToString(), Name = "Накопичувачі"},
                    };

                    context.Categories.AddRange(categories);
                    context.SaveChanges();

                    var products = new List<Product>
                    {
                        new() 
                        { 
                            Id = Guid.NewGuid().ToString(), 
                            Name = "Gigabyte B550M AORUS ELITE AX",
                            Amount = 2,
                            Price = 4699,
                            CategoryId = categories[2].Id
                        },
                        new()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Name = "AMD Ryzen 5 5600 3.5(4.4)GHz 32MB sAM4 Tray",
                            Amount = 5,
                            Price = 4499,
                            CategoryId = categories[0].Id
                        }
                    };

                    context.Products.AddRange(products);
                    context.SaveChanges();
                }
            }
        }
    }
}
