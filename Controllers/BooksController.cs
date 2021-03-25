using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestApplication.Data;
using TestApplication.DTO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TestApplication.Models;
using DTO;

namespace TestApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly Context _context;

        public BooksController(Context context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetBooks()
        {
            var book = from books in _context.Book
            join book_descriptions in _context.Book_Description on books.id equals book_descriptions.book_id
            select new BookDTO
            {
                Book_id = books.id,
                Book_price = books.price,
                ISBN = books.isbn,
                Book_name = book_descriptions.book_name,
                Book_description = book_descriptions.book_description
            };

            return await book.ToListAsync();
        }

    }
}