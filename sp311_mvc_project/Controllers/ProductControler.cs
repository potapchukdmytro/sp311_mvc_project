using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using sp311_mvc_project.Data;
using sp311_mvc_project.ViewModels;

namespace sp311_mvc_project.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var products = _context.Products
                .Include(p => p.Category)
                .AsEnumerable();

            return View(products);
        }

        public IActionResult Create()
        {
            var categories = _context.Categories.AsEnumerable();

            var viewModel = new CreateProductVM
            {
                Categories = new SelectList(categories, "Id", "Name")
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateProductVM viewModel)
        {
            string? fileName = null;

            if(viewModel.File != null)
            {
                fileName = SaveImage(viewModel.File);
            }

            viewModel.Product.Image = fileName;
            viewModel.Product.Id = Guid.NewGuid().ToString();

            _context.Products.Add(viewModel.Product);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        private string? SaveImage(IFormFile file)
        {
            try
            {
                var types = file.ContentType.Split("/");

                if (types[0] != "image")
                {
                    return null;
                }

                var imageName = $"{Guid.NewGuid()}.{types[1]}";
                var imagesPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "products");
                var imagePath = Path.Combine(imagesPath, imageName);

                using (var fileStream = System.IO.File.Create(imagePath))
                {
                    using (var formStream = file.OpenReadStream())
                    {
                        formStream.CopyTo(fileStream);
                    }
                }

                return imageName;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
