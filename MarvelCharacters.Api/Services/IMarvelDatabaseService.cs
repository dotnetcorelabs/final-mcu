using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarvelCharacters.Api.Models;

namespace MarvelCharacters.Api.Services
{
    public interface IMarvelDatabaseService
    {
        Task<Character> AddLike(Character data);

        Task RemoveLike(int id);

        Task<List<Character>> GetLikes();
    }
}
