using Microsoft.EntityFrameworkCore;

namespace BookService.Data
{
    public class BookDbContext : DbContext
    {
        public BookDbContext(DbContextOptions<BookDbContext> options) : base(options)
        {
        }
        public DbSet<BookService.Models.Book> Books { get; set; }
    }
}
