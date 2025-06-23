using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecordStore.Core.Dtos;
using RecordStore.Core.Services.Interfaces;

namespace RecordStore.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetReviews()
        {
            var reviews = await _reviewService.GetAllReviewsAsync();
            return Ok(reviews);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReviewDto>> GetReview(int id)
        {
            var review = await _reviewService.GetReviewByIdAsync(id);
            if (review == null)
                return NotFound();

            return Ok(review);
        }

        [HttpGet("record/{recordId}")]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetReviewsByRecord(int recordId)
        {
            var reviews = await _reviewService.GetReviewsByRecordIdAsync(recordId);
            return Ok(reviews);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetReviewsByUser(int userId)
        {
            var reviews = await _reviewService.GetReviewsByUserIdAsync(userId);
            return Ok(reviews);
        }

        [HttpGet("user/{userId}/record/{recordId}")]
        public async Task<ActionResult<ReviewDto>> GetReviewByUserAndRecord(int userId, int recordId)
        {
            var review = await _reviewService.GetReviewByUserAndRecordAsync(userId, recordId);
            if (review == null)
                return NotFound();

            return Ok(review);
        }

        [HttpGet("record/{recordId}/average-rating")]
        public async Task<ActionResult<decimal>> GetAverageRating(int recordId)
        {
            var averageRating = await _reviewService.GetAverageRatingForRecordAsync(recordId);
            return Ok(averageRating);
        }

        [HttpPost]
        public async Task<ActionResult<ReviewDto>> CreateReview(CreateReviewDto createReviewDto)
        {
            try
            {
                var review = await _reviewService.CreateReviewAsync(createReviewDto);
                return CreatedAtAction(nameof(GetReview), new { id = review.ReviewId }, review);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<ReviewDto>> UpdateReview(int id, UpdateReviewDto updateReviewDto)
        {
            var review = await _reviewService.UpdateReviewAsync(id, updateReviewDto);
            if (review == null)
                return NotFound();

            return Ok(review);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteReview(int id)
        {
            var success = await _reviewService.DeleteReviewAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
