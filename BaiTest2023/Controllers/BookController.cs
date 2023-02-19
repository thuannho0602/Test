//using BaiTest2023.Modols;
//using Microsoft.AspNetCore.Components;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
using BaiTest2023.DTO;
using BaiTest2023.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BaiTest2023.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : Controller
    {
        private readonly BookDbContext _bookContext;

        public BookController(BookDbContext  context)
        {
            _bookContext = context;
        }
        // Get/api/book
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetBookItems()
        {
            return await _bookContext.Books
                .Select(x => ItemBook(x))
                .ToListAsync();
        }
        // Get/api/book/{id}
        [HttpGet("{Id}")]
        public async Task<ActionResult<BookDTO>> GetBookItem(int Id)
        {
            var bookItem = await _bookContext.Books.FindAsync(Id);

            if (bookItem == null)
            {
                return NotFound();
            }

            return ItemBook(bookItem);
        }
        //Post/api/book
        [HttpPost]
        public async Task<ActionResult<BookDTO>> CreateBookItem(BookDTO bookItemDTO)
        {
            var bookItem = new Book
            {
                Tile = bookItemDTO.Tile,
                Description = bookItemDTO.Description,
                Authorld= bookItemDTO.Authorld,
                PulblishDate= bookItemDTO.PulblishDate,
                Price = bookItemDTO.Price

            };

            _bookContext.Books.Add(bookItem);
            await _bookContext.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetBookItem),
                new { id = bookItem.Id },
                ItemBook(bookItem));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutBookItem(int id, BookDTO bookDTO)
        {
            if (id != bookDTO.Id)
            {
                return BadRequest();
            }

            var bookItem = await _bookContext.Books.FindAsync(id);
            if (bookItem == null)
            {
                return NotFound();
            }

            bookItem.Tile = bookDTO.Tile;
            bookItem.Description = bookDTO.Description;
            bookItem.Authorld = bookDTO.Authorld;   
            bookItem.PulblishDate = bookDTO.PulblishDate;
            bookItem.Price = bookDTO.Price;

            try
            {
                await _bookContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!BookItemExists(id))
            {
                return NotFound();
            }

            return CreatedAtAction(
             nameof(GetBookItem),
             new { id = bookItem.Id },
             ItemBook(bookItem));
        }
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DatleteBookItem(int id)
        {
            var bookItem = await _bookContext.Books.FindAsync(id);
            if(bookItem == null)
            {
                return NotFound();
            }
            _bookContext.Books.Remove(bookItem);
            await _bookContext.SaveChangesAsync();
            return Ok();
        }
        private bool BookItemExists(int id)
        {
            return _bookContext.Books.Any(e => e.Id == id);
        }
        private static BookDTO ItemBook(Book bookItem) =>
            new BookDTO
            {
                Id = bookItem.Id,
                Tile = bookItem.Tile,
                Description = bookItem.Description,
                Authorld=bookItem.Authorld,
                PulblishDate=bookItem.PulblishDate,
                Price = bookItem.Price  
            };


    }
}
