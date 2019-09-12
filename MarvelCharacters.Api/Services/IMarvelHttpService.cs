using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MarvelCharacters.Api.Models;

namespace MarvelCharacters.Api.Services
{
    public interface IMarvelHttpService
    {
        Task<IReadOnlyList<Character>> GetCharacters(string searchString = null, int limit = 10, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<Comic>> GetComics(string searchString = null, int limit = 10, CancellationToken cancellationToken = default);
    }
}
