using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookStore_ph1.Data;
using BookStore_ph1.Models;
using BookStore_ph1.ViewModels;
using BookStore_ph1.Data.Migrations;
using System.Drawing.Text;

namespace BookStore_ph1.Controllers
{
    public class BooksController : Controller
    {
         
        private readonly ApplicationDbContext _context;
        
        public BooksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index(string searchString, string searchCriteria)
        {
            IQueryable<Book> books = _context.Books.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                switch (searchCriteria)
                {
                    case "Title":
                        books = books.Where(x => x.Title.Contains(searchString));
                        break;
                    case "Author":
                        books = books.Where(x => x.Author.FirstName.Contains(searchString));
                        break;
                    case "Genre":
                        books = books.Where(x => x.BookGenres.Any(bg => bg.Genre.GenreName.Contains(searchString)));
                        break;
                    default:
                        books = books.Where(x => x.Title.Contains(searchString));
                        break;
                }
            }

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

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.BookGenres)
                .ThenInclude(b => b.Genre)
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            var genres = _context.Genre.ToList();
            var viewModel = new BookGenreCreateViewModel
            {
                Book = new Book(),
                GenreList = genres.Select(g => new SelectListItem
                {
                    Value = g.GenreId.ToString(),
                    Text = g.GenreName
                })
            };

            ViewData["AuthorId"] = new SelectList(_context.Authors, "AuthorId", "FullName");
            return View(viewModel);
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookGenreCreateViewModel viewmodel,List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);
            
            
            
            foreach (var formFile in files)
            {
                if(formFile.Length == 1)
                {
                    var filePath = Path.GetTempFileName();
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await formFile.CopyToAsync(stream);
                        
                    }
                }
            }
            if (ModelState.IsValid)
            {
                _context.Add(viewmodel.Book);
                await _context.SaveChangesAsync();
                if (viewmodel.SelectedGenres != null)
                {
                    foreach (var genreId in viewmodel.SelectedGenres)
                    {
                        var bookGenre = new BookGenre { BookId = viewmodel.Book.BookId, GenreId = genreId };
                        _context.Add(bookGenre);
                    }
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "AuthorId", "FullName", viewmodel.Book.AuthorId);
            viewmodel.GenreList = _context.Genre.Select(g => new SelectListItem
            {
                Value = g.GenreId.ToString(),
                Text = g.GenreName
            });
            return View(viewmodel.Book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = _context.Books.Where(x => x.BookId == id).Include(x => x.BookGenres).First();

            if (book == null)
            {
                return NotFound();
            }

            var genres = _context.Genre.AsEnumerable();

            BookGenreEditViewModel viewmodel = new BookGenreEditViewModel
            {
                Book = book,
                GenreList = new MultiSelectList(genres, "GenreId", "GenreName"),
                SelectedGenres = book.BookGenres!.Select(x => x.GenreId)
            };

            ViewData["AuthorId"] = new SelectList(_context.Authors, "AuthorId", "FullName", book.AuthorId);
            return View(viewmodel);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BookGenreEditViewModel viewmodel)
        {
            if (id != viewmodel.Book?.BookId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(viewmodel.Book);
                    await _context.SaveChangesAsync();

                    IEnumerable<int> newGenreList = viewmodel.SelectedGenres!;
                    IEnumerable<int> prevGenreList = _context.BookGenres.Where(s => s.BookId == id).Select(s => s.GenreId);
                    IQueryable<BookGenre> toBeRemoved = _context.BookGenres.Where(s => s.BookId == id);

                    if (newGenreList != null)
                    {
                        toBeRemoved = toBeRemoved.Where(s => !newGenreList.Contains(s.GenreId));
                        foreach (int genreId in newGenreList)
                        {
                            if (!prevGenreList.Any(s => s == genreId))
                            {
                                _context.BookGenres.Add(new BookGenre { GenreId = genreId, BookId = id });
                            }
                        }
                    }
                    _context.BookGenres.RemoveRange(toBeRemoved);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(viewmodel.Book.BookId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "FullName", viewmodel.Book.AuthorId);
            return View(viewmodel);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.BookGenres)
                .ThenInclude(b => b.Genre)
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.BookId == id);
        }
    }
}
