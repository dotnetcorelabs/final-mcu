using MarvelCharacters.Api.Infrastructure;
using MarvelCharacters.Api.Models;
using MarvelCharacters.Api.Services.Http.Marvel.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MarvelCharacters.Api.Services.Http.Marvel
{
    public class HttpMarvelApi : IMarvelHttpService
    {
        private readonly MarvelApiOptions _marvelApiOptions;

        private readonly HttpClient _client;

        private readonly ILogger<HttpMarvelApi> _logger;

        public HttpMarvelApi(HttpClient client, IOptions<MarvelApiOptions> options, ILogger<HttpMarvelApi> logger)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));

            _marvelApiOptions = options?.Value ?? throw new ArgumentNullException(nameof(options));

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _client.BaseAddress = new Uri(_marvelApiOptions.Uri);
        }

        #region Authorization
        string GetAuthorizationString(MarvelApiOptions marvelApiOptions, int ts = 0)
        {
            var hash = ComputeHash($"{ts}{marvelApiOptions.PrivateKey}{marvelApiOptions.PublicKey}");
            return $"ts={ts}&apikey={marvelApiOptions.PublicKey}&hash={hash}";
        }

        static string ComputeHash(string input)
        {
            using (var md5Hash = MD5.Create())
            {
                // Convert the input string to a byte array and compute the hash.
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Create a new StringBuilder to collect the bytes
                // and create a string.
                StringBuilder sBuilder = new StringBuilder();

                // Loop through each byte of the hashed data 
                // and format each one as a hexadecimal string.
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                // Return the hexadecimal string.
                return sBuilder.ToString();
            }
        }
        #endregion

        public async Task<IReadOnlyList<Character>> GetCharacters(string searchString = "", int limit = 10, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation(LoggingEvents.MV_API_CHARACTERS, "Getting characters with SearchString {SEARCH_STRING} and limit {LIMIT}", searchString, limit);

            string authorizationQuery = GetAuthorizationString(_marvelApiOptions, 1);

            _logger.LogInformation(LoggingEvents.MV_API_CHARACTERS, "Authorization token generated");

            string uri = $"/v1/public/characters?limit={limit}&{authorizationQuery}";

            if (!string.IsNullOrEmpty(searchString))
                uri += $"&nameStartsWith={searchString}";

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, uri))
            {
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    var responseStream = await _client.SendAsync(request, cancellationToken);

                    if (responseStream.IsSuccessStatusCode)
                    {
                        var responseText = await responseStream.Content.ReadAsStringAsync();
                        var response = JsonConvert.DeserializeObject<ServiceResult<Character>>(responseText);

                        return response.Data.Results;
                    }
                    _logger.LogWarning(LoggingEvents.MV_API_CHARACTERS, "Status code response different from success. status code {STATUS_CODE}", responseStream.StatusCode);
                    return Array.Empty<Character>();
                }
                catch (Exception ex)
                {
                    _logger.LogError(LoggingEvents.MV_API_CHARACTERS, ex, "Error when trying to get Marvel Characters from api with SearchString {SEARCH_STRING} and limit {LIMIT}", searchString, limit);
                    throw;
                }
            }
        }

        public async Task<IReadOnlyList<Comic>> GetComics(string searchString = null, int limit = 10, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation(LoggingEvents.MV_API_COMICS, "Getting Comics with SearchString {SEARCH_STRING} and limit {LIMIT}", searchString, limit);

            string authorizationQuery = GetAuthorizationString(_marvelApiOptions, 1);

            _logger.LogInformation(LoggingEvents.MV_API_COMICS, "Authorization token generated");

            string uri = $"/v1/public/comics?limit={limit}&{authorizationQuery}";

            if (!string.IsNullOrEmpty(searchString))
                uri += $"&titleStartsWith={searchString}";

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, uri))
            {
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    var responseStream = await _client.SendAsync(request, cancellationToken);

                    if (responseStream.IsSuccessStatusCode)
                    {
                        var responseText = await responseStream.Content.ReadAsStringAsync();
                        var response = JsonConvert.DeserializeObject<ServiceResult<Comic>>(responseText);

                        return response.Data.Results;
                    }
                    _logger.LogWarning(LoggingEvents.MV_API_COMICS, "Status code response different from success. status code {STATUS_CODE}", responseStream.StatusCode);
                    return Array.Empty<Comic>();
                }
                catch (Exception ex)
                {
                    _logger.LogError(LoggingEvents.MV_API_COMICS, ex, "Error when trying to getting marvel Comics from API with SearchString {SEARCH_STRING} and limit {LIMIT}", searchString, limit);
                    throw;
                }
            }
        }
    }
}
