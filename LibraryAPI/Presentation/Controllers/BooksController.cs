using AutoMapper;
using LibraryAPI.Infrastructure.Services.BookService;
using LibraryAPI.Infrastructure.Services.GuardService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly IGuardService _guardService;
        private readonly IMapper _mapper;

        public BooksController(IBookService book, IMapper mapper, IGuardService guardService)
        {
            _bookService = book;
            _mapper = mapper;
            _guardService = guardService;
        }

        [HttpGet, Authorize]
        public async Task<ActionResult<List<BookDTO>>> GetAllBooks()
        {
            Request.Headers.TryGetValue("Authorization", out var defaultValue);
            await _guardService.ValidateSession(defaultValue);
            return await _bookService.GetAllBooks();
        }

        [HttpGet("isbn"), Authorize]
        public async Task<ActionResult<BookDTO>> GetOneIsbnBook(int isbn)
        {
            Request.Headers.TryGetValue("Authorization", out var defaultValue);
            await _guardService.ValidateSession(defaultValue);
            var result = await _bookService.GetOneIsbnBook(isbn);
            if (result is null)
                return NotFound("Not Found");
            return Ok(result);
        }

        [HttpGet("{id}"), Authorize]
        public async Task<ActionResult<BookDTO>> GetOnlyBook(int id)
        {
            Request.Headers.TryGetValue("Authorization", out var defaultValue);
            await _guardService.ValidateRole(defaultValue, Role.Admin);
            var result = await _bookService.GetOnlyBook(id);
            if (result is null)
                return NotFound("Not Found");
            return Ok(result);
        }

        [HttpPost, Authorize]
        public async Task<ActionResult<List<BookDTO>>> AddBook(BookDTO book)
        {
            Request.Headers.TryGetValue("Authorization", out var defaultValue);
            await _guardService.ValidateRole(defaultValue, Role.Admin);
            var bookMap = _mapper.Map<Book>(book);
            var result = await _bookService.AddBook(bookMap);
            return Ok(result);
        }

        [HttpPut("{id}"), Authorize]
        public async Task<ActionResult<List<BookDTO>>> UpdateBook(int id, BookDTO request)
        {
            Request.Headers.TryGetValue("Authorization", out var defaultValue);
            await _guardService.ValidateRole(defaultValue, Role.Admin);
            var result = await _bookService.UpdateBook(id, request);
            if (result is null)
                return NotFound("Not Found");
            return Ok(result);
        }


        [HttpDelete("{id}"), Authorize]
        public async Task<ActionResult<List<BookDTO>>> DeleteBook(int id)
        {
            Request.Headers.TryGetValue("Authorization", out var defaultValue);
            await _guardService.ValidateRole(defaultValue, Role.Admin);
            var result = await _bookService.DeleteBook(id);
            if (result is null)
                return NotFound("Not Found");
            return Ok(result);
        }


        [HttpPut("takeBook/isbn"), Authorize]
        public async Task<ActionResult<BookDTO>> TakeBook(int isbn)
        {
            Request.Headers.TryGetValue("Authorization", out var defaultValue);
            Session session = await _guardService.ValidateSession(defaultValue);
            var result = await _bookService.TakeBook(isbn, session.User);
            if (result is false)
                return NotFound("Not Found");
            return Ok(result);
        }


        [HttpPut("returnBook/isbn"), Authorize]
        public async Task<ActionResult<BookDTO>> ReturnBook(int isbn)
        {
            Request.Headers.TryGetValue("Authorization", out var defaultValue);
            Session session = await _guardService.ValidateSession(defaultValue);
            var result = await _bookService.ReturnBook(isbn, session.User);
            if (result is false)
                return NotFound("Not Found");
            return Ok(result);
        }

    }
}

