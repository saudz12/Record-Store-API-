using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordStore.Database.Entities
{
    public class OrderRecord : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public int OrderId { get; set; }
        public int RecordId { get; set; }
        public int Quantity { get; set; }

        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }

        [ForeignKey("RecordId")]
        public virtual Record Record { get; set; }
    }
}
