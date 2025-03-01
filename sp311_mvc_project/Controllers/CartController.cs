using Microsoft.AspNetCore.Mvc;
using sp311_mvc_project.Services;
using sp311_mvc_project.Settings;
using sp311_mvc_project.ViewModels;
using System.Collections.Generic;

namespace sp311_mvc_project.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddToCart([FromBody] CartItemVM viewModel)
        {
            if (string.IsNullOrEmpty(viewModel.ProductId))
                return BadRequest();

            var items = HttpContext.Session.Get<IEnumerable<CartItemVM>>(SessionSettings.SessionCartKey);
            items ??= new List<CartItemVM>();

            if(items.Select(i => i.ProductId).Contains(viewModel.ProductId))
                return BadRequest();

            var newItems = items.ToList();
            newItems.Add(new CartItemVM { ProductId = viewModel.ProductId });
            HttpContext.Session.Set<IEnumerable<CartItemVM>>(SessionSettings.SessionCartKey, newItems);

            return Ok();
        }

        [HttpPost]
        public IActionResult RemoveFromCart([FromBody] CartItemVM viewModel)
        {
            if (string.IsNullOrEmpty(viewModel.ProductId))
                return BadRequest();

            var items = HttpContext.Session.Get<IEnumerable<CartItemVM>>(SessionSettings.SessionCartKey);

            if (items == null)
                return BadRequest();

            if (!items.Select(i => i.ProductId).Contains(viewModel.ProductId))
                return BadRequest();

            items = items.Where(i => i.ProductId != viewModel.ProductId);
            HttpContext.Session.Set(SessionSettings.SessionCartKey, items);

            return Ok();
        }
    }
}
