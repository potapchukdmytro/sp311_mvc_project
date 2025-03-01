using System.ComponentModel.DataAnnotations;

namespace sp311_mvc_project.Models
{
    public class Category
    {
        [Key]
        public string? Id { get; set; }
        [Required(ErrorMessage = "Обов'язкове поле")]
        [MinLength(2, ErrorMessage = "Мінімальна довжина 2 символи")]
        [MaxLength(100, ErrorMessage = "Максимальна довжина 100 символів")]
        public string? Name { get; set; }

        public List<Product> Products { get; set; } = [];
    }
}
