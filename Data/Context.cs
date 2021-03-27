using Microsoft.EntityFrameworkCore;
using book.Models;

namespace book.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options) {}
        public DbSet<User> User {get; set;}
        public DbSet<Book> Book {get; set;}
        public DbSet<Book_description> Book_Description {get; set;}
    }
}