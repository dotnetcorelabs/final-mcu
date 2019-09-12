using MarvelCharacters.Api.Infrastructure;
using MarvelCharacters.Api.Models;
using MarvelCharacters.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MarvelCharacters.Api.Controllers
{
    [Route("api/[controller]")]
    public class CharactersController : Controller
    {
        private readonly IMarvelHttpService _marvelService;

        private readonly IMarvelDatabaseService _marvelDatabase;

        private readonly ILogger<CharactersController> _logger;

        /// <inheritdoc />
        public CharactersController(IMarvelHttpService marvelService, IMarvelDatabaseService marvelDatabase,
            ILogger<CharactersController> logger)
        {
            _marvelService = marvelService ?? throw new ArgumentNullException(nameof(marvelService));
            _marvelDatabase = marvelDatabase ?? throw new ArgumentNullException(nameof(marvelDatabase));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery]string searchString, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation(LoggingEvents.API_CHARACTERS, "Getting marvel characters with searchString {SearchString}", searchString);

            var charactersColl = await _marvelService.GetCharacters(searchString, cancellationToken: cancellationToken);

            var likesCollection = await _marvelDatabase.GetLikes();

            for (int i = 0; i < charactersColl.Count; i++)
            {
                var current = charactersColl[i];

                var characterLike = likesCollection.FirstOrDefault(x => x.Id == current.Id);

                current.Liked = characterLike != null;
            }

            return Ok(charactersColl);
        }

        [HttpPost("{id}/likes")]
        public async Task<IActionResult> CreateLike([FromRoute]int? id, [FromBody]Character character, CancellationToken cancellationToken = default)
        {
            if (id.HasValue == false || character == null)
                return BadRequest();

            _logger.LogInformation(LoggingEvents.API_CHARACTERS, "Adding like to marvel character id {ID}", id);

            var data = await _marvelDatabase.AddLike(character);

            return Ok(data);
        }

        [HttpDelete("{id}/likes")]
        public async Task<IActionResult> DeleteLike([FromRoute]int? id, CancellationToken cancellationToken = default)
        {
            if (id.HasValue == false)
                return BadRequest();

            _logger.LogInformation(LoggingEvents.API_CHARACTERS, "Removing like to marvel character id {ID}", id);

            await _marvelDatabase.RemoveLike(id.Value);

            return Ok();
        }
    }
}
