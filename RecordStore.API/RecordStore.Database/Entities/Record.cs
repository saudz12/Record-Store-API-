using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordStore.Database.Entities
{
    public class Record : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RecordId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        public DateTime ReleaseDate { get; set; }

        public int Duration { get; set; } 

        [MaxLength(100)]
        public string Genre { get; set; }

        [MaxLength(100)]
        public string RecordLabel { get; set; }

        public virtual ICollection<ArtistRecord> ArtistRecords { get; set; } = new List<ArtistRecord>();
        public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();
        public virtual ICollection<OrderRecord> OrderRecords { get; set; } = new List<OrderRecord>();
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    }
}
