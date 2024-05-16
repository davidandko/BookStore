using BookStore_ph1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookStore_ph1.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Genre> Genre { get; set; }
        public DbSet<BookGenre> BookGenres { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<UserBooks> UserBooks { get; set; }

        public DbSet<User> User { get; set; }

      

    }
}
