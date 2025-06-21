using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordStore.Database.Entities
{
    public class Order : BaseEntity
    {
        [Key]
        public int OrderId { get; set; }

        public int UserId { get; set; }
        public DateTime PlacementDate { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public virtual ICollection<OrderRecord> OrderRecords { get; set; } = new List<OrderRecord>();
    }
}
