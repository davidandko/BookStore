using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace BookStore_ph1.Models
{
    public class User : IdentityUser
    {
       
        [StringLength(100)]
        public string? Name { get; set; }
        
        public string? Role { get; set; }
        public IList<Book>? Books { get; set; }
    }
}
