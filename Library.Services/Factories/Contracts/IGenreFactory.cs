using Library.Models.Models;
using System.Threading.Tasks;

namespace Library.Services.Factories.Contracts
{
    public interface IGenreFactory
    {
        Task<Genre> CreateGenreAsync(string genreName);
    }
}
