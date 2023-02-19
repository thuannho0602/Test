using BaiTest2023.DTO;
using BaiTest2023.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace BaiTest2023.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController:Controller
    {
        private readonly BookDbContext _authorContext;

        public AuthorController(BookDbContext context)
        {
            _authorContext= context;
        }
        //Get/api/Author 
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorCountBookDTO>>> GetAuthorItems()
        {
            var data =  await _authorContext.Authors
                .Join(_authorContext.Books,
                    c=> c.Id,
                    x=> x.Authorld,
                    (c,x)=> new  { c.Id, c.Firstname, c.Lastname, BookId = x.Id})
                .GroupBy(c=> new { c.Id, c.Firstname, c.Lastname})
                .Select(x => new AuthorCountBookDTO
                {
                    Id = x.Key.Id,
                    Firstname = x.Key.Firstname,
                    Lastname = x.Key.Lastname,
                    TotalBook = x.Select(c=>c.BookId).Count()
                }).ToListAsync();
            return data;
        }
        [HttpPost]
        public async Task<ActionResult<AuthorDTO>> CreateAuthorItem(AuthorDTO authorItemDTO)
        {
            var authorItem = new Author
            {
                Id=authorItemDTO.Id,
                Firstname= authorItemDTO.Firstname,
                Lastname= authorItemDTO.Lastname,
                Email= authorItemDTO.Email,
                Age= authorItemDTO.Age,
                DOB= authorItemDTO.DOB,
                Address= authorItemDTO.Address,
            };

            _authorContext.Authors.Add(authorItem);
            await _authorContext.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetAuthorItems),
                new { id = authorItem.Id },
                ItemAuthor(authorItem));
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuthorItem(int id, AuthorDTO authorDTO)
        {
            if (id != authorDTO.Id)
            {
                return BadRequest();
            }

            var authorItem = await _authorContext.Authors.FindAsync(id);
            if (authorItem == null)
            {
                return NotFound();
            }

            authorItem.Firstname = authorDTO.Firstname;
            authorItem.Lastname = authorDTO.Lastname;
            authorItem.Email = authorDTO.Email;
            authorItem.Age = authorDTO.Age;
            authorItem.DOB = authorDTO.DOB;
            authorItem.Address = authorDTO.Address;


            try
            {
                await _authorContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!AuthorItemExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        private bool AuthorItemExists(int id)
        {
            return _authorContext.Authors.Any(e => e.Id == id);
        }
        private static AuthorDTO ItemAuthor(Author authorItem) =>
            new AuthorDTO
            {
                Id = authorItem.Id,
                Firstname = authorItem.Firstname,
                Lastname = authorItem.Lastname,
                Email = authorItem.Email,
                Age = authorItem.Age,
                DOB = authorItem.DOB,
                Address=authorItem.Address,
            };
        
    }
}
