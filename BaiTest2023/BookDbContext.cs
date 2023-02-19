using BaiTest2023.Models;
using Microsoft.EntityFrameworkCore;

namespace BaiTest2023
{
    public class BookDbContext: DbContext
    {
        public BookDbContext(DbContextOptions<BookDbContext>options):base(options)
        {

        }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Author> Authors { get; set; }
        
    }
}
