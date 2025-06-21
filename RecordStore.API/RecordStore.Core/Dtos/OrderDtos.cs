using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordStore.Core.Dtos
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public DateTime PlacementDate { get; set; }
        public List<OrderRecordDto> OrderRecords { get; set; } = new List<OrderRecordDto>();
        public decimal TotalAmount { get; set; }
    }

    public class CreateOrderDto
    {
        [Required]
        public int UserId { get; set; }
        public List<CreateOrderRecordDto> OrderRecords { get; set; } = new List<CreateOrderRecordDto>();
    }

    public class OrderRecordDto
    {
        public int RecordId { get; set; }
        public string RecordName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubTotal { get; set; }
    }

    public class CreateOrderRecordDto
    {
        [Required]
        public int RecordId { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }

    public class UpdateOrderRecordDto
    {
        [Required]
        public int RecordId { get; set; }
        [Range(1, int.MaxValue)]
        public int? Quantity { get; set; }
    }
}
