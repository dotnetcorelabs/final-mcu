using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarvelCharacters.Api.Models
{
    public class Comic
    {
        public int Id { get; set; }

        public int DigitalId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public Thumbnail Thumbnail { get; set; }
    }
}
