using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordStore.Core.Dtos
{
    public class InventoryDto
    {
        public int Id { get; set; }
        public int RecordId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

    public class CreateInventoryDto
    {
        [Required]
        public int RecordId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

    public class UpdateInventoryDto
    {
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
    }
}
