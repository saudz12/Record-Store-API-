using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordStore.Database.Entities
{
    public class Review : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReviewId { get; set; }

        public int UserId { get; set; }
        public int RecordId { get; set; }
        [Column(TypeName = "decimal(2,1)")]
        public decimal Rating {  get; set; }
        [MaxLength(500)]
        public string Message { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("RecordId")]
        public virtual Record Record { get; set; }
    }
}
