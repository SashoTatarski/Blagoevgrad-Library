using Library.Database;
using Library.Models.Models;
using Library.Services.Contracts;
using Library.Services.Factories.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Services
{
    public class BookManager : IBookManager
    {
        private readonly IBookFactory _bookFac;
        private readonly IAuthorFactory _authorFac;
        private readonly IGenreFactory _genreFac;
        private readonly IPublisherFactory _publisherFac;
        private readonly LibraryContext _context;

        public BookManager(LibraryContext context, IBookFactory bookFac, IAuthorFactory authorFac, IGenreFactory genreFac, IPublisherFactory publisherFac)
        {
            _context = context;
            _bookFac = bookFac;
            _authorFac = authorFac;
            _genreFac = genreFac;
            _publisherFac = publisherFac;
        }

        public async Task CreateBookAsync(string title, string isbn, int year, int rack, string authorId, string publisherId, List<int> genresIds, int copies)
        {
            var author = await _context.Authors.FirstOrDefaultAsync(a => a.Id.ToString() == authorId);

            var publisher = await _context.Publishers.FirstOrDefaultAsync(p => p.Id.ToString() == publisherId);

            await _bookFac.CreateBookAsync(title, isbn, year, rack, author, publisher, genresIds, copies);
        }
       
        public async Task<Book> GetBookByIdAsync(string id)
           => await _context.Books
               .Include(b => b.Author)
               .Include(b => b.Publisher)
               .Include(b => b.BookGenres)
                   .ThenInclude(bg => bg.Genre)
               .Include(b => b.CheckedoutBook)
                    .ThenInclude(chb => chb.User)
                .Include(b => b.ReservedBooks)
                    .ThenInclude(rb => rb.User)
               .Include(b => b.Ratings)
               .FirstOrDefaultAsync(book => book.Id.ToString() == id)
            .ConfigureAwait(false);
        public async Task<Book> GetBookByIsbnAsync(string isbn)
            => await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .Include(b => b.BookGenres)
                    .ThenInclude(bg => bg.Genre)
                .Include(b => b.CheckedoutBook)
                    .ThenInclude(chb => chb.User)
                .Include(b => b.ReservedBooks)
                    .ThenInclude(rb => rb.User)
                .Include(b => b.Ratings)
                .FirstOrDefaultAsync(book => book.ISBN == isbn)
            .ConfigureAwait(false);
        public async Task<List<Book>> GetTopRatedBooks(int number)
        {
            var distinctBooks = await this.GetDistinctBooksByIsbnAsync();
            return distinctBooks.OrderByDescending(b => b.Rating).Take(number).ToList();
        }
        public async Task<List<Book>> GetAllBooksAsync() =>
            await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .Include(b => b.BookGenres)
                    .ThenInclude(bg => bg.Genre)
                .Include(b => b.CheckedoutBook)
                    .ThenInclude(chb => chb.User)
                .Include(b => b.ReservedBooks)
                    .ThenInclude(rb => rb.User)
                .Include(b => b.Ratings)
           .ToListAsync()
           .ConfigureAwait(false);
        public async Task<List<Book>> GetDistinctBooksByIsbnAsync()
        {
            var books = await this.GetAllBooksAsync();
            return books.GroupBy(b => b.ISBN).Select(g => g.First()).ToList();
        }
        public async Task<List<Book>> GetBooksByIsbnAsync(string isbn)
        {
            var books = await this.GetAllBooksAsync();
            return books.Where(b => b.ISBN == isbn).ToList();
        }
        public async Task AddBookCopies(string id, int copies)
        {
            var book = await GetBookByIdAsync(id);

            List<int> genreIds = new List<int>();
            book.BookGenres.ForEach(bg => genreIds.Add(bg.GenreId));

            await this.CreateBookAsync(book.Title, book.ISBN, book.Year, book.Rack, book.AuthorId.ToString(), book.PublisherId.ToString(), genreIds, copies).ConfigureAwait(false);
        }
        public async Task<int> BookCopiesCountAsync(string isbn) => await _context.Books.Where(b => b.ISBN == isbn).CountAsync().ConfigureAwait(false);
        public async Task EditBookAsync(string bookId, string title, string isbn, int year, int rack, string authorId, string publisherId, List<int> genresIds)
        {
            var tempBook = await GetBookByIdAsync(bookId);

            var allBooks = await GetAllBooksAsync();
            var booksToEditIsbn = allBooks
                .Where(b => b.ISBN == tempBook.ISBN).ToList();

            foreach (var book in booksToEditIsbn)
            {
                if (book.Title != title)
                    book.Title = title;

                if (book.ISBN != isbn)
                    book.ISBN = isbn;

                if (book.Year != year)
                    book.Year = year;

                if (book.Rack != rack)
                    book.Rack = rack;

                var newAuthor = await _context.Authors.FirstOrDefaultAsync(a => a.Id.ToString() == authorId).ConfigureAwait(false);
                book.AuthorId = newAuthor.Id;

                var newPublisher = await _context.Publishers.FirstOrDefaultAsync(p => p.Id.ToString() == publisherId).ConfigureAwait(false);
                book.PublisherId = newPublisher.Id;

                var genres = await _context.Genres.ToListAsync().ConfigureAwait(false);

                // Clean all genres for the book
                foreach (var bookGenre in _context.BookGenre)
                {
                    if (bookGenre.BookId == book.Id)
                        _context.BookGenre.Remove(bookGenre);
                }
                await _context.SaveChangesAsync().ConfigureAwait(false);

                // Add the genres to the clean table
                foreach (var genre in genres)
                {
                    foreach (var genreParam in genresIds)
                    {
                        if (genre.Id == genreParam)
                        {
                            _context.BookGenre.Add(new BookGenre
                            {
                                BookId = book.Id,
                                GenreId = genreParam
                            });
                        }
                    }
                }
            }
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }
        public async Task DeleteBookAsync(string isbn)
        {
            var booksToDelete = await _context.Books.Where(book => book.ISBN == isbn).ToListAsync().ConfigureAwait(false);

            foreach (var book in booksToDelete)
            {
                _context.Books.Remove(book);
            }
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<Author> CreateAuthorAsync(string authorName) => await _authorFac.CreateAuthorAsync(authorName).ConfigureAwait(false);
        public async Task<Author> GetAuthorAsync(string id)
            => await _context.Authors
                .Include(a => a.Books)
                .FirstOrDefaultAsync(a => a.Id.ToString() == id)
                .ConfigureAwait(false);
        public async Task<List<Author>> GetAllAuthorsAsync() => await _context.Authors.ToListAsync().ConfigureAwait(false);

        public async Task<Publisher> CreatePublisherAsync(string publisherName) => await _publisherFac.CreatePublisherAsync(publisherName);
        public async Task<List<Publisher>> GetAllPublishersAsync() => await _context.Publishers.ToListAsync().ConfigureAwait(false);

        public async Task<Genre> CreateGenreAsync(string genre) => await _genreFac.CreateGenreAsync(genre);
        public async Task<List<Genre>> GetAllGenresAsync() => await _context.Genres.ToListAsync().ConfigureAwait(false);

        public async Task<List<Book>> SearchAsync(string searchCriteria, bool byTitle, bool byAuthor, bool byPublisher, bool byGenre)
        {
            var searchResult = new List<Book>();
            var allBooks = await this.GetDistinctBooksByIsbnAsync();

            if (byTitle)
            {
                searchResult.AddRange(allBooks.Where(b => b.Title.ToLower().Contains(searchCriteria)));
            }
            if (byAuthor)
            {
                searchResult.AddRange(allBooks.Where(b => b.Author.Name.ToLower().Contains(searchCriteria)));
            }
            if (byPublisher)
            {
                searchResult.AddRange(allBooks.Where(b => b.Publisher.Name.ToLower().Contains(searchCriteria)));
            }
            if (byGenre)
            {
                foreach (var book in allBooks)
                {
                    foreach (var bookGenre in book.BookGenres)
                    {
                        if (bookGenre.Genre.Name.ToLower().Contains(searchCriteria))
                        {
                            searchResult.Add(book);
                        }
                    }
                }
            }
            if (!byTitle && !byAuthor && !byPublisher && !byGenre)
            {

                return allBooks
                    .Where(b => b.Title.ToLower().Contains(searchCriteria) || b.Author.Name.ToLower().Contains(searchCriteria) || b.Publisher.Name.ToLower().Contains(searchCriteria) || b.ISBN.ToLower().Contains(searchCriteria))
                .ToList();
            }
            else
            {
                return searchResult.GroupBy(b => b.ISBN).Select(g => g.First()).ToList();
            }            
        }

        public async Task<List<Book>> GetBooksByAuthorAsync(string authorId)
        {
            var distinctBooks = await this.GetDistinctBooksByIsbnAsync();

            return distinctBooks.Where(b => b.AuthorId.ToString() == authorId).ToList();
        }
        public bool isIsbnUnique(string isbn) => _context.Books.Any(x => x.ISBN == isbn);
    }
}

