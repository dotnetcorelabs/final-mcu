using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MarvelCharacters.Api.Models;
using MarvelCharacters.Api.Services.Http.Marvel.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace MarvelCharacters.Api.Services.Http.Marvel
{
    public class HttpMarvelApi : IMarvelService
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

        /// <inheritdoc />
        public async Task<IReadOnlyList<Character>> GetCharacters(string searchString = null, int page = 0, CancellationToken cancellationToken = default)
        {
            string authorizationQuery = GetAuthorizationString(_marvelApiOptions, 1);

            string uri = $"/v1/public/characters?nameStartsWith={searchString}&limit=10&{authorizationQuery}";

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, uri))
            {
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var responseStream = await _client.SendAsync(request, cancellationToken);

                if (responseStream.IsSuccessStatusCode)
                {
                    var responseText = await responseStream.Content.ReadAsStringAsync();
                    var response = JsonConvert.DeserializeObject<ServiceResult<Character>>(responseText);

                    return response.Data.Results;
                }
                throw new Exception("Error when trying to get marvel characters");
            }
        }
    }
}
