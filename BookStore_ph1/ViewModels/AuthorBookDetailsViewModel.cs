using BookStore_ph1.Models;

namespace BookStore_ph1.ViewModels
{
    public class AuthorBookDetailsViewModel
    {
        public IList<Book> Books { get; set; }
        public Author Author { get; set; }
    }
}
