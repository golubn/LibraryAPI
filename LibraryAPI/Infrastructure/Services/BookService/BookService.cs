
using AutoMapper;
using LibraryAPI.Domain.Models;
using LibraryAPI.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Infrastructure.Services.BookService
{
    public class BookService : IBookService
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        public BookService(DataContext datacontext, IMapper mapper)
        {
            _dataContext = datacontext;
            _mapper = mapper;
        }

        public async Task<List<BookDTO>> AddBook(Book book)
        {
            _dataContext.Books.Add(book);
            await _dataContext.SaveChangesAsync();
            return await _dataContext.Books.Select(book => _mapper.Map<BookDTO>(book)).ToListAsync();
        }

        public async Task<List<BookDTO>?> DeleteBook(int id)
        {
            var book = await _dataContext.Books.FindAsync(id);
            if (book is null)
            {
                return null;
            }

            _dataContext.Books.Remove(book);
            await _dataContext.SaveChangesAsync();
            return await _dataContext.Books.Select(book => _mapper.Map<BookDTO>(book)).ToListAsync();
        }

        public async Task<List<BookDTO>> GetAllBooks()
        {
            var getbooks = await _dataContext.Books.Select(book => _mapper.Map<BookDTO>(book)).ToListAsync();
            return getbooks;
        }

        public async Task<BookDTO?> GetOnlyBook(int id)
        {
            var book = await _dataContext.Books.FindAsync(id);
            if (book is null)
            {
                return null;
            }
            var mapBook = _mapper.Map<BookDTO>(book);
            return mapBook;
        }

        public async Task<BookDTO?> GetOneIsbnBook(int isbn)
        {
            var book = await _dataContext.Books.Where(x => x.ISBN == isbn).FirstOrDefaultAsync();
            if (book is null)
            {
                return null;
            }
            var mapBook = _mapper.Map<BookDTO>(book);
            return mapBook;
        }

        public async Task<List<BookDTO>?> UpdateBook(int id, BookDTO request)
        {
            var book = await _dataContext.Books.FindAsync(id);
            if (book is null)
            {
                return null;
            }

            book.Name = request.Name;
            book.Author = request.Author;
            book.Genre = request.Genre;
            book.ISBN = request.ISBN;
            book.ReturnBook = request.ReturnBook;
            book.GetBook = request.GetBook;

            await _dataContext.SaveChangesAsync();

            return await _dataContext.Books.Select(book => _mapper.Map<BookDTO>(book)).ToListAsync();
        }

        public async Task<bool> TakeBook(int isbn, User user)
        {
            var book = await _dataContext.Books.Where(x => x.ISBN == isbn && x.User == null).FirstOrDefaultAsync();
            if (book is null)
            {
                return false;
            }
            book.User = user;
            await _dataContext.SaveChangesAsync();
            return true;
        }


        public async Task<bool> ReturnBook(int isbn, User user)
        {
            var book = await _dataContext.Books.Where(x => x.ISBN == isbn && x.User.UserId == user.UserId).FirstOrDefaultAsync();
            if (book is null)
            {
                return false;
            }
            book.User = null;
            await _dataContext.SaveChangesAsync();
            return true;
        }
    }
}
