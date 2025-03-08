using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sp311_mvc_project.Models
{
    public class Product
    {
        [Key]
        public string? Id { get; set; }
        [Required, MaxLength(150)]
        public string? Name { get; set; }
        [MaxLength]
        public string? Description { get; set; }
        [Range(0, double.MaxValue)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
        [Range(0, int.MaxValue)]
        public int Amount { get; set; }
        [MaxLength(255)]
        public string? Image { get; set; }

        [ForeignKey("Category")]
        public string? CategoryId { get; set; }
        public Category? Category { get; set; }

        [NotMapped]
        public bool InCart { get; set; } = false;
        [NotMapped]
        public int QuantityInCart { get; set; } = 1;
    }
}
