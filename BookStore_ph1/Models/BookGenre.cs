using System.ComponentModel.DataAnnotations;

namespace BookStore_ph1.Models
{
    public class BookGenre
    {
        [Required]
        public int BookGenreId { get; set; }

        [Required] 
        public int BookId { get; set; }
        public Book Book { get; set; }

        [Required]
        public int GenreId { get; set; }
        public Genre Genre { get; set; }
    }
}
