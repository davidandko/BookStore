using System.ComponentModel.DataAnnotations;

namespace BookStore_ph1.Models
{
    public class Genre
    {
        [Required]
        public int GenreId { get; set; }
        [Required,StringLength(50)]
        [Display(Name ="Genre")]
        public string GenreName {  get; set; }
        public ICollection<BookGenre> BookGenres { get; set; } = new List<BookGenre>();

    }
}
