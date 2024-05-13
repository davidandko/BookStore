using BookStore_ph1.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookStore_ph1.ViewModels
{
    public class BookGenreEditViewModel
    {
        public Book Book { get; set; }
        public IEnumerable<int>? SelectedGenres { get; set; }
        public IEnumerable<SelectListItem>? GenreList { get; set; }
    }
}
