using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordStore.Core.Dtos
{
    public class OrderRecordDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int RecordId { get; set; }
        public string RecordName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubTotal { get; set; }
    }

    public class CreateOrderRecordDto
    {
        [Required]
        public int OrderId { get; set; }
        [Required]
        public int RecordId { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }

    public class UpdateOrderRecordDto
    {
        [Range(1, int.MaxValue)]
        public int? Quantity { get; set; }
    }
}
