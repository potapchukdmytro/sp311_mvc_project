using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace sp311_mvc_project.Models
{
    public class AppUser : IdentityUser
    {
        [MaxLength(255)]
        public string? FirstName { get; set; }
        [MaxLength(255)]
        public string? LastName { get; set; }
        [Range(0, 500)]
        public int Age { get; set; }
    }
}
