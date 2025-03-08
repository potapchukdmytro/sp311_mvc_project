using sp311_mvc_project.Models;

namespace sp311_mvc_project.ViewModels
{
    public class CartVM
    {
        public IEnumerable<Product> Products { get; set; } = [];
        public decimal ProductsPrice 
        {
            get
            {
                var prices = Products.Select(p => p.Price * p.QuantityInCart);
                decimal sum = prices.Aggregate(0M, (res, i) => res + i);
                return sum;
            }
        }
        public decimal TotalPrice { get => ProductsPrice + Shipping; }
        public decimal Shipping { get; set; } = 100;
    }
}
