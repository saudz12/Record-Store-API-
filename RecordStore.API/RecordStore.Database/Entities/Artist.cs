using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecordStore.Database.Entities
{
    public class Artist : BaseEntity
    {
        [Key]
        public int ArtistId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string Label { get; set; }

        public virtual ICollection<ArtistRecord> ArtistRecords { get; set; } = new List<ArtistRecord>();
    }
}
