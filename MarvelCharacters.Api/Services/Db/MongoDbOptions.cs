using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarvelCharacters.Api.Services.Db
{
    public class MongoDbOptions
    {
        public string ConnectionString { get; set; }

        public string Database { get; set; }
    }
}
