using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MarvelCharacters.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MarvelCharacters.Api.Controllers
{
    [Route("api/[controller]")]
    public class CharactersController : Controller
    {
        private readonly IMarvelService _marvelService;

        private readonly ILogger<CharactersController> _logger;

        public CharactersController(IMarvelService marvelService, ILogger<CharactersController> logger)
        {
            _marvelService = marvelService ?? throw new ArgumentNullException(nameof(marvelService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery]string searchString, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Getting marvel characters with searchString {SearchString}", searchString);

            var data = await _marvelService.GetCharacters(searchString, cancellationToken: cancellationToken);
            return Ok(data);
        }
    }
}
