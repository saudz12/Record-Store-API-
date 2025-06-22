using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordStore.Core.Dtos
{
    public class ReviewDto
    {
        public int ReviewId { get; set; }
        public int RecordId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string RecordName { get; set; }
        public string Message { get; set; }
        public decimal Rating { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class CreateReviewDto
    {
        [Required]
        public int RecordId { get; set; }
        [Required]
        public int UserId { get; set; }
        public string Message { get; set; }
        [Required]
        [Range(0.0, 10.0)]
        public decimal Rating { get; set; }
    }

    public class UpdateReviewDto
    {
        public string Message { get; set; }
        [Range(0.0, 10.0)]
        public decimal? Rating { get; set; }
    }
}
