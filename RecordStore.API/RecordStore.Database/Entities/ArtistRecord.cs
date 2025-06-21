using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordStore.Database.Entities
{
    public class ArtistRecord : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int RecordId { get; set; }
        public int ArtistId { get; set; }

        [MaxLength(50)]
        public string Role { get; set; }

        [ForeignKey("RecordId")]
        public virtual Record Record { get; set; }

        [ForeignKey("ArtistId")]
        public virtual Artist Artist { get; set; }
    }
}
