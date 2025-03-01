using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sp311_mvc_project.Data;
using sp311_mvc_project.Models;
using sp311_mvc_project.Repositories.Products;
using sp311_mvc_project.Services;
using sp311_mvc_project.Settings;
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

        public IActionResult Index(string? category, int page)
        {
            var categories = _context.Categories;

            var products = string.IsNullOrEmpty(category)
                ? _productRepository.GetAll().Include(p => p.Category)
                : _productRepository.GetByCategory(category);

            // pagination -->
            int pageSize = 8;
            int totalCount = products.Count();
            int pagesCount = (int)Math.Ceiling((double)totalCount / pageSize);
            page = page < 1 || page > pagesCount ? 1 : page;
            products = products.Skip((page - 1) * pageSize).Take(pageSize);
            // <-- pagination

            // cart -->
            var cartItems = HttpContext.Session.Get<IEnumerable<CartItemVM>>(SessionSettings.SessionCartKey);
            if(cartItems != null)
            {
                foreach (var product in products)
                {
                    product.InCart = cartItems.Select(i => i.ProductId).Contains(product.Id);
                }
            }
            // <-- cart

            var viewModel = new HomeProductListVM
            {
                Products = products,
                Categories = categories,
                Category = category ?? "",
                PagesCount = pagesCount,
                Page = page
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
