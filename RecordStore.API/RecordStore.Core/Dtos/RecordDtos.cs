using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordStore.Core.Dtos
{
    public class RecordDto
    {
        public int RecordId { get; set; }
        public string Name { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int Duration { get; set; }
        public string Genre { get; set; }
        public string RecordLabel { get; set; }
        public List<ArtistRecordDto> Artists { get; set; } = new List<ArtistRecordDto>();
        public InventoryDto Inventory { get; set; }
    }

    public class CreateRecordDto
    {
        [Required]
        public string Name { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int Duration { get; set; }
        public string Genre { get; set; }
        public string RecordLabel { get; set; }
    }

    public class UpdateRecordDto
    {
        public string? Name { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public int? Duration { get; set; }
        public string? Genre { get; set; }
        public string? RecordLabel { get; set; }
    }
}
