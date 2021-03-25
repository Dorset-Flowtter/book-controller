using Microsoft.EntityFrameworkCore;
using TestApplication.Models;

namespace TestApplication.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options) {}
        public DbSet<Values> Values {get; set;}
        public DbSet<Book> Book {get; set;}
        public DbSet<Book_description> Book_Description {get; set;}
    }
}