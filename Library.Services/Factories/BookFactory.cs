using Library.Database;
using Library.Models.Models;
using Library.Services.Factories.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.Services.Factories
{
    public class BookFactory : IBookFactory
    {
        private readonly LibraryContext _context;
        public BookFactory(LibraryContext context)
        {
            _context = context;
        }
        public async Task CreateBookAsync(string title, string isbn, int year, int rack, Author author, Publisher publisher, List<int> genresIds, int copies)
        {
            for (int i = 0; i < copies; i++)
            {
                var newBook = new Book();
                newBook.Title = title;
                newBook.ISBN = isbn;
                newBook.Year = year;
                newBook.Rack = rack;
                newBook.Author = author;
                newBook.Publisher = publisher;

                _context.Books.Add(newBook);

                foreach (var genre in genresIds)
                {
                    _context.BookGenre.Add(new BookGenre
                    {
                        BookId = newBook.Id,
                        GenreId = genre
                    });
                }
            }
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
