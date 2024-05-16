using System.Diagnostics;
using BookStore_ph1.Data;
using BookStore_ph1.Models;
using BookStore_ph1.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Reflection.Metadata.BlobBuilder;

namespace BookStore_ph1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            string searchCriteria = "";
            IQueryable<Book> books = _context.Books.AsQueryable();
            books = books.Include(x => x.Author)
                .Include(x => x.Reviews)
                .Include(x => x.BookGenres!).ThenInclude(x => x.Genre);

            var bookIndex = new BookGenreIndexViewModel
            {
                Books = await books.ToListAsync(),
                Genres = await _context.Genre.ToListAsync(),
                SearchCriteria = searchCriteria
            };

            return View(bookIndex);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
