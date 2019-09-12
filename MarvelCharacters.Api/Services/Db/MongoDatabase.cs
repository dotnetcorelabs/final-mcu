using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarvelCharacters.Api.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MarvelCharacters.Api.Services.Db
{
    public class MongoDatabase : IMarvelDatabaseService
    {
        private readonly MongoDbOptions _mongoDbOptions;
        private readonly ILogger<MongoDatabase> _logger;

        private const string CHARACTER_COLLECTION_NAME = "character";

        /// <inheritdoc />
        public MongoDatabase(IOptions<MongoDbOptions> mongoDbOptions, ILogger<MongoDatabase> logger)
        {
            _mongoDbOptions = mongoDbOptions?.Value ?? throw new ArgumentNullException(nameof(mongoDbOptions));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        static IMongoDatabase GetDatabase(MongoDbOptions dbOptions)
        {
            var client = new MongoClient(dbOptions.ConnectionString);
            return client.GetDatabase(dbOptions.Database);
        }

        /// <inheritdoc />
        public async Task<Character> AddLike(Character data)
        {
            var collection = GetDatabase(_mongoDbOptions).GetCollection<Character>(CHARACTER_COLLECTION_NAME);

            await collection.InsertOneAsync(data, new InsertOneOptions());

            return data;
        }

        /// <inheritdoc />
        public Task RemoveLike(int id)
        {
            var collection = GetDatabase(_mongoDbOptions).GetCollection<Character>(CHARACTER_COLLECTION_NAME);

            return collection.DeleteOneAsync(x => x.Id == id);
        }

        /// <inheritdoc />
        public async Task<List<Character>> GetLikes()
        {
            var collection = GetDatabase(_mongoDbOptions).GetCollection<Character>(CHARACTER_COLLECTION_NAME);

            return await collection.AsQueryable().ToListAsync();
        }
    }
}
