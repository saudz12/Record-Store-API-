using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordStore.Core.Dtos
{
    public class ArtistDto
    {
        public int ArtistId { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
    }

    public class CreateArtistDto
    {
        [Required]
        public string Name { get; set; }
        public string Label { get; set; }
    }

    public class UpdateArtistDto
    {
        public string? Name { get; set; }
        public string? Label { get; set; }
    }
}
