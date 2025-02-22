using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using sp311_mvc_project.Data;
using sp311_mvc_project.Models;
using sp311_mvc_project.Repositories.Products;
using sp311_mvc_project.Services.Image;
using sp311_mvc_project.Settings;
using sp311_mvc_project.ViewModels;

namespace sp311_mvc_project.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IProductRepository _productRepository;
        private readonly IImageService _imageService;

        public ProductController(AppDbContext context, IProductRepository productRepository, IImageService imageService)
        {
            _context = context;
            _productRepository = productRepository;
            _imageService = imageService;
        }

        public IActionResult Index()
        {
            var products = _productRepository
                .GetAll()
                .Include(p => p.Category);

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
        public async Task<IActionResult> CreateAsync(CreateProductVM viewModel)
        {
            string? fileName = null;

            if(viewModel.File != null)
            {
                fileName = await _imageService.SaveImageAsync(viewModel.File, PathSettings.ProductImagesPath);
            }

            viewModel.Product.Image = fileName;
            viewModel.Product.Id = Guid.NewGuid().ToString();

            await _productRepository.CreateAsync(viewModel.Product);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteAsync(string? id)
        {
            if(id == null)
                return NotFound();

            var model = await _productRepository.FindByIdAsync(id);

            if (model == null)
                return NotFound();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAsync(Product model)
        {
            if (model.Id == null)
                return NotFound();

            if(model.Image != null)
            {
                _imageService.DeleteImage(Path.Combine(PathSettings.ProductImagesPath, model.Image));
            }

            await _productRepository.DeleteAsync(model.Id);
            return RedirectToAction("Index");
        }
    }
}