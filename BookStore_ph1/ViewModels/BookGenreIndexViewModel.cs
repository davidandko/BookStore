using System.Security.Policy;
using BookStore_ph1.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookStore_ph1.ViewModels
{
    public class BookGenreIndexViewModel
    {
        public IList<Book> Books { get; set; }
        public IList<Genre> Genres { get; set; }
        public string BookGenre { get; set; }
        public string SearchString { get; set; }
        public string SearchCriteria { get; set; }
    }
}
