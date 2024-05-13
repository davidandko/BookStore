using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore_ph1.Models
{
    public class Book
    {
        [Required]
        public int BookId {  get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Display(Name ="Year Published")]
        public int? YearPublished { get; set; }

        [Display(Name = "Number of Pages")]
        public int? NumPages { get; set; }
        [StringLength(1024)]
        public string? Description { get; set; }

        [StringLength (50)]
        public string? Publisher { get; set; }

        [StringLength(1024)]
        [Display(Name = "Front Page")]
        public string? FrontPage { get; set; }

        [StringLength(1024)]
        [Display(Name ="Download URL")]
        public string? DownloadUrl { get; set; }

        [Required]
        public int AuthorId { get; set; }
        public Author? Author { get; set; }

        public ICollection<UserBooks>? UserBooks { get; set; } = new List<UserBooks>(); 
        public ICollection<Review>? Reviews { get; set; }= new List<Review>();

        [NotMapped]
        public double? Rating
        {
            get
            {
                double? avg = Reviews.Count>0 ? Reviews.Average(r => r.Rating) : 0;
                return avg;
            }
        }
        
        [Display(Name ="Genres")]
        public ICollection<BookGenre>? BookGenres { get; set; }= new List<BookGenre>();
    }
}
