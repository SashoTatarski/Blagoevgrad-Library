using Library.Database;
using Library.Models.Models;
using Library.Services.Factories.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Library.Services.Factories
{
    public class GenreFactory : IGenreFactory
    {
        private readonly LibraryContext _context;
        public GenreFactory(LibraryContext context)
        {
            _context = context;
        }

        public async Task<Genre> CreateGenreAsync(string genreName)
        {
            var existingGenre = await _context.Genres.FirstOrDefaultAsync(g => g.Name.ToLower() == genreName.ToLower()).ConfigureAwait(false);

            if (existingGenre is null)
            {
                var newGenre = new Genre { Name = genreName };

                await _context.Genres.AddAsync(newGenre).ConfigureAwait(false);
                await _context.SaveChangesAsync().ConfigureAwait(false);
                return newGenre;
            }
            else
                return existingGenre;
        }
    }
}