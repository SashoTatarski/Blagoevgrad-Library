using Library.Database;
using Library.Models.Models;
using Library.Services.Factories.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Library.Services.Factories
{
    public class PublisherFactory : IPublisherFactory
    {
        private readonly LibraryContext _context;
        public PublisherFactory(LibraryContext context)
        {
            _context = context;
        }
        public async Task<Publisher> CreatePublisherAsync(string name)
        {
            var existingPublisher = await _context.Publishers.FirstOrDefaultAsync(p => string.Equals(p.Name, name, System.StringComparison.OrdinalIgnoreCase)).ConfigureAwait(false);

            if (existingPublisher is null)
            {
                var newPublisher = new Publisher { Name = name };
                await _context.Publishers.AddAsync(newPublisher).ConfigureAwait(false);
                await _context.SaveChangesAsync().ConfigureAwait(false);

                return newPublisher;
            }
            else
                return existingPublisher;
        }
    }
}
