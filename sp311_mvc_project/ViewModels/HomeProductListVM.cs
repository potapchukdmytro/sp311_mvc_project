using sp311_mvc_project.Models;

namespace sp311_mvc_project.ViewModels
{
    public class HomeProductListVM
    {
        public IEnumerable<Product> Products { get; set; } = [];
        public IEnumerable<Category> Categories { get; set; } = [];
        public string Category { get; set; } = string.Empty;
        public int PagesCount { get; set; } = 1;
        public int Page { get; set; } = 1;
    }
}
