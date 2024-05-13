using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookStore_ph1.Models
{
    public class Author
    {
        [Required] 
        public int AuthorId { get; set; }

        [Required,StringLength(50)]
        [Display(Name ="First Name")]
        public string FirstName { get; set; } 

        [Required, StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } 

        [Display(Name = "Date of birth")]
        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        [StringLength(50)]
        public string? Nationality { get; set; }

        [StringLength (50)]
        
        public string? Gender { get; set; }

        [Display(Name="Full Name")]
        public string FullName => $"{FirstName} {LastName}";
        public ICollection<Book>? Books { get; set;} = new List<Book>();
    }
}
