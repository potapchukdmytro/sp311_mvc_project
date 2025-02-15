using Microsoft.AspNetCore.Mvc.Rendering;
using sp311_mvc_project.Models;

namespace sp311_mvc_project.ViewModels
{
    public class CreateProductVM
    {
        public Product Product { get; set; } = new();
        public IEnumerable<SelectListItem> Categories { get; set; } = [];
        public IFormFile? File { get; set; }
    }
}
