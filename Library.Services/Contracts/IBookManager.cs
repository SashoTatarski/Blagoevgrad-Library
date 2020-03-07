using Library.Models.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.Services.Contracts
{
    public interface IBookManager
    {
        Task CreateBookAsync(string title, string isbn, int year, int rack, string authorId, string publisherId, List<int> genresIds, int copies);
        Task<List<Book>> GetAllBooksAsync();
        Task<List<Book>> GetDistinctBooksByIsbnAsync();
        Task<Book> GetBookByIdAsync(string id);
        Task<Book> GetBookByIsbnAsync(string isbn);
        Task<List<Book>> GetTopRatedBooks(int number);
        Task AddBookCopies(string id, int copies);
        Task<int> BookCopiesCountAsync(string isbn);
        Task EditBookAsync(string bookId, string title, string isbn, int year, int rack, string authorId, string publisherId, List<int> genresIds);
        Task DeleteBookAsync(string id);

        Task<Author> CreateAuthorAsync(string authorName);
        Task<List<Author>> GetAllAuthorsAsync();
        Task<Author> GetAuthorAsync(string id);

        Task<Publisher> CreatePublisherAsync(string publisherName);
        Task<List<Publisher>> GetAllPublishersAsync();

        Task<Genre> CreateGenreAsync(string genre);
        Task<List<Genre>> GetAllGenresAsync();

        Task<List<Book>> SearchAsync(string searchCriteria, bool byTitle, bool byAuthor, bool byPublisher, bool byGenre);
        Task<List<Book>> GetBooksByAuthorAsync(string authorId);
        Task<List<Book>> GetBooksByIsbnAsync(string isbn);
        bool isIsbnUnique(string isbn);
    }
}