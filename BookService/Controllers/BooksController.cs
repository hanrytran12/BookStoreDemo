using AutoMapper;
using BookService.Data;
using BookService.DTO;
using Grpc.Net.Client;
using InventoryService.Grpc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly BookDbContext _context;
        private readonly IMapper _mapper;

        public BooksController(BookDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            var books = await _context.Books.ToListAsync();
            var booksDTO = _mapper.Map<List<GetBooksDTO>>(books);
            return Ok(booksDTO);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            var bookDTO = _mapper.Map<GetDetailBookDTO>(book);
            return Ok(bookDTO);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] AddBookDTO addBookDTO)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new InventoryGrpc.InventoryGrpcClient(channel);
            var book = _mapper.Map<Models.Book>(addBookDTO);
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            var reply = await client.CreateInventoryAsync(new CreateRequest
            {
                BookId = book.Id,
            });

            return Ok(reply.Success);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return Ok("Deleted Successfully!");
        }
    }
}
