using System.ComponentModel.DataAnnotations;

namespace BookStore_ph1.Models
{
    public class UserBooks
    {
        [Required]
        public int UserBooksId { get; set; }
        [Required, StringLength(450)]
        public string AppUser {  get; set; }
        [Required]
        public int BookId { get; set; }
        public Book Book { get; set; }
    }
}
