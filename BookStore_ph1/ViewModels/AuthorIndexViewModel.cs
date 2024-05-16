using BookStore_ph1.Models;

namespace BookStore_ph1.ViewModels
{
    public class AuthorIndexViewModel
    {
        public IList<Author> Authors { get; set; }
        public string SearchString { get; set; }
    }
}
