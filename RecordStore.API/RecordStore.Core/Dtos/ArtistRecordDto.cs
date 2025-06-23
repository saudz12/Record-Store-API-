using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordStore.Core.Dtos
{
    public class ArtistRecordDto
    {
        public int Id { get; set; }
        public int RecordId { get; set; }
        public int ArtistId { get; set; }
        public string RecordName { get; set; }
        public string ArtistName { get; set; }
        public string Role { get; set; }
    }

    public class CreateArtistRecordDto
    {
        [Required]
        public int RecordId { get; set; }
        [Required]
        public int ArtistId { get; set; }
        [Required]
        public string Role { get; set; }
    }

    public class UpdateArtistRecordDto
    {
        public string Role { get; set; }
    }
}
