using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Bulky.Models
{
    public class AppUser : IdentityUser
    {
        [Required]
        public string Name { get; set; }

        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }
    }
}
