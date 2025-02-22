using sp311_mvc_project.Models;

namespace sp311_mvc_project.ViewModels
{
    public class HomeProductListVM
    {
        public IEnumerable<Product> Products { get; set; } = [];
        public IEnumerable<Category> Categories { get; set; } = [];
    }
}
