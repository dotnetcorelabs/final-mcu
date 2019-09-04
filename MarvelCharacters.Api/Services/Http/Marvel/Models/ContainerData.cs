using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarvelCharacters.Api.Services.Http.Marvel.Models
{
    public class ContainerData<T>
        where T : class, new()
    {
        public int Offset { get; set; }

        public int Limit { get; set; }

        public int Total { get; set; }

        public int Count { get; set; }

        public List<T> Results { get; set; }
    }
}
