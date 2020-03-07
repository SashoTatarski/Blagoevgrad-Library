using Library.Database;
using Library.Models.Models;
using Library.Services.Factories.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Library.Services.Factories
{
    public class AuthorFactory : IAuthorFactory
    {
        private readonly LibraryContext _context;
        public AuthorFactory(LibraryContext context)
        {
            _context = context;
        }
        public async Task<Author> CreateAuthorAsync(string authorName)
        {
            var existingAuthor = await _context.Authors.FirstOrDefaultAsync(a => string.Equals(a.Name, authorName, System.StringComparison.CurrentCultureIgnoreCase)).ConfigureAwait(false);

            if (existingAuthor is null)
            {
                var newAuthor = new Author { Name = authorName };
                await _context.Authors.AddAsync(newAuthor).ConfigureAwait(false);
                await _context.SaveChangesAsync().ConfigureAwait(false);
                return newAuthor;
            }
            else
                return existingAuthor;
        }
    }
}
