using System.ComponentModel.DataAnnotations;

namespace BookStore_ph1.Models
{
    public class Review
    {
        [Required]
        public int ReviewId { get; set; }
        [Required]
        public int BookId { get; set; }
        public Book? Book { get; set; }

        [Required,StringLength(450)]
        public string AppUser { get; set; }
        [Required,StringLength(500)]
        public string Comment { get; set; }

        [Range(minimum:1,maximum:10)]
        public int? Rating { get; set; }
    }
}
