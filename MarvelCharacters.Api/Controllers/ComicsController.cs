using MarvelCharacters.Api.Infrastructure;
using MarvelCharacters.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MarvelCharacters.Api.Controllers
{
    [Route("api/[controller]")]
    public class ComicsController : Controller
    {
        private readonly ILogger<ComicsController> _logger;

        private readonly IMarvelHttpService _marvelService;

        public ComicsController(ILogger<ComicsController> logger, IMarvelHttpService marvelService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _marvelService = marvelService ?? throw new ArgumentNullException(nameof(marvelService));
        }

        [HttpGet]
        public async Task<IActionResult> Index(string searchString, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation(LoggingEvents.API_COMICS, "Getting Marvel Comics with searchString {SearchString}", searchString);

            var data = await _marvelService.GetComics(searchString, cancellationToken: cancellationToken);
            return Ok(data);
        }
    }
}
