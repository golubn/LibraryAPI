using LibraryAPI.Domain.Models;
namespace LibraryAPI.Infrastructure.Services.BookService
{
    public interface IBookService
    {
        Task<List<BookDTO>> GetAllBooks();
        Task<BookDTO?> GetOnlyBook(int id);
        Task<BookDTO?> GetOneIsbnBook(int isbn);
        Task<List<BookDTO>> AddBook(Book book);
        Task<List<BookDTO>?> UpdateBook(int id, BookDTO request);
        Task<List<BookDTO>?> DeleteBook(int id);
        Task<bool> TakeBook(int isbn, User user);
        Task<bool> ReturnBook(int isbn, User user);
    }
}
