using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quik_BookingApp.BOs.Request;
using Quik_BookingApp.DAO.Models;
using Quik_BookingApp.Repos.Interface;

namespace Quik_BookingApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllReviews()
        {
            var reviews = await _reviewService.GetAllReviewsAsync();
            return Ok(reviews);
        }

        [HttpGet("GetReviewById/{id}")]
        public async Task<IActionResult> GetReviewById(Guid id)
        {
            var review = await _reviewService.GetReviewByIdAsync(id);
            if (review == null) return NotFound();
            return Ok(review);
        }

        [HttpGet("GetReviewsBySpaceId/{spaceId}")]
        public async Task<IActionResult> GetReviewsBySpaceId(string spaceId)
        {
            var reviews = await _reviewService.GetReviewsBySpaceIdAsync(spaceId);

            if (reviews == null || !reviews.Any())
            {
                return Ok(new { Message = "No reviews found for this working space." });
            }

            return Ok(new { Reviews = reviews });
        }

        [HttpPost("CreateReview")]
        public async Task<IActionResult> CreateReview([FromBody] ReviewRequestModel review)
        {
            var createdReview = await _reviewService.CreateReviewAsync(review);
            return CreatedAtAction(nameof(GetReviewById), new { id = createdReview.ReviewId }, createdReview);
        }

        [HttpPut("UpdateReview/{id}")]
        public async Task<IActionResult> UpdateReview(Guid id, [FromBody] Review review)
        {
            if (id != review.ReviewId) return BadRequest();

            var updatedReview = await _reviewService.UpdateReviewAsync(review);
            if (updatedReview == null) return NotFound();

            return Ok(updatedReview);
        }

        [HttpDelete("RemoveReview/{id}")]
        public async Task<IActionResult> DeleteReview(Guid id)
        {
            var success = await _reviewService.DeleteReviewAsync(id);
            if (!success) return NotFound();

            return NoContent();
        }
    }

}
