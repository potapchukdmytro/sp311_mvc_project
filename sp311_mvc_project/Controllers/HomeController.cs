using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
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
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, IProductRepository productRepository, AppDbContext context, RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _logger = logger;
            _productRepository = productRepository;
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult Index(string? category, int page)
        {
            var categories = _context.Categories;

            var products = string.IsNullOrEmpty(category)
                ? _productRepository.GetAll().Include(p => p.Category)
                : _productRepository.GetByCategory(category);

            // sorting -->
            products = products.OrderByDescending(p => p.Amount);
            // <-- sorting

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
                Products = products.ToList(),
                Categories = categories.ToList(),
                Category = category ?? "",
                PagesCount = pagesCount,
                Page = page
            };

            return View(viewModel);
        }

        [ActionName("Details")]
        public async Task<IActionResult> ProductDetails(string? id)
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToAction("Index");

            var model = await _productRepository.FindByIdAsync(id);

            if (model == null)
                return RedirectToAction("Index");

            var cartItems = HttpContext.Session.Get<IEnumerable<CartItemVM>>(SessionSettings.SessionCartKey);
            if (cartItems != null)
            {
                model.InCart = cartItems.Select(c => c.ProductId).Contains(model.Id);
            }

            return View("ProductDetails", model);
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

        public async Task<IActionResult> SetAdmin()
        {
            if(User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Redirect("/Identity/Account/Login");
            }

            var userId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

            var user = await _userManager.FindByIdAsync(userId);

            if(user == null)
            {
                return RedirectToAction("Index");
            }
            
            if(!await _roleManager.RoleExistsAsync("admin"))
            {
                var adminRole = new IdentityRole
                {
                    Name = "admin"
                };
                await _roleManager.CreateAsync(adminRole);
            }

            if(!await _userManager.IsInRoleAsync(user, "admin"))
            {
                await _userManager.AddToRoleAsync(user, "admin");
            }


            return RedirectToAction("Index");
        }
    }
}
