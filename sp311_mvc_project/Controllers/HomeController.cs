using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sp311_mvc_project.Data;
using sp311_mvc_project.Models;
using sp311_mvc_project.Repositories.Products;
using sp311_mvc_project.ViewModels;

namespace sp311_mvc_project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductRepository _productRepository;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, IProductRepository productRepository, AppDbContext context)
        {
            _logger = logger;
            _productRepository = productRepository;
            _context = context;
        }

        public IActionResult Index(string? category)
        {
            var categories = _context.Categories;

            var products = string.IsNullOrEmpty(category)
                ? _productRepository.GetAll().Include(p => p.Category)
                : _productRepository.GetByCategory(category);

            var viewModel = new HomeProductListVM
            {
                Products = products,
                Categories = categories
            };

            return View(viewModel);
        }

        [ActionName("Details")]
        public IActionResult ProductDetails(string? id)
        {
            return View("ProductDetails");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Info()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
