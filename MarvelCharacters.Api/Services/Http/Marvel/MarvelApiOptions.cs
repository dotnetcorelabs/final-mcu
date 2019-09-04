using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarvelCharacters.Api.Services.Http.Marvel
{
    public class MarvelApiOptions
    {
        public string PrivateKey { get; set; }

        public string PublicKey { get; set; }

        public string Uri { get; set; }
    }
}
