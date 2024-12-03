using Quik_BookingApp.DAO.Models;
using Quik_BookingApp.DAO;
using Microsoft.EntityFrameworkCore;
using Quik_BookingApp.Repos.Interface;
using Quik_BookingApp.BOs.Request;
using Quik_BookingApp.BOs.Response;
using AutoMapper;

namespace Quik_BookingApp.Service
{
    public class ReviewService : IReviewService
    {
        private readonly QuikDbContext _context;
        private readonly IMapper _mapper;

        public ReviewService(QuikDbContext context, IMapper _mapper)
        {
            this._context = context;
            this._mapper = _mapper;
        }

        public async Task<List<ReviewResponseModel>> GetAllReviewsAsync()
        {
            List<ReviewResponseModel> _response = new List<ReviewResponseModel>();
            var data = await _context.Reviews.ToListAsync();
            if(data != null)
            {
                _response = _mapper.Map<List<Review>, List<ReviewResponseModel>>(data);
            }
            return _response;
        }

        public async Task<Review> GetReviewByIdAsync(Guid reviewId)
        {
            return await _context.Reviews
                                 .Include(r => r.User)
                                 .Include(r => r.WorkingSpace)
                                 .FirstOrDefaultAsync(r => r.ReviewId == reviewId);
        }


        public async Task<List<ReviewResponseModel>> GetReviewsBySpaceIdAsync(string spaceId)
        {
            // Log giá trị spaceId
            Console.WriteLine($"SpaceId received: {spaceId}");

            // Kiểm tra xem spaceId có phải là Guid hay không
            bool isGuid = Guid.TryParse(spaceId, out Guid spaceGuid);

            var reviewsQuery = _context.Reviews.AsNoTracking();

            // Nếu spaceId là chuỗi kiểu Guid, truy vấn theo Guid
            if (isGuid)
            {
                reviewsQuery = reviewsQuery.Where(r => r.SpaceId == spaceGuid.ToString());
            }
            else
            {
                // Nếu không, truy vấn theo chuỗi thông thường
                reviewsQuery = reviewsQuery.Where(r => r.SpaceId == spaceId);
            }

            // Log truy vấn trước khi thực hiện
            Console.WriteLine($"Query: {reviewsQuery.ToQueryString()}");

            var reviews = await reviewsQuery
                .Select(r => new ReviewResponseModel
                {
                    ReviewId = r.ReviewId,
                    Username = r.Username,
                    SpaceId = r.SpaceId,
                    Rating = r.Rating,
                    Comment = r.Comment,
                    CreatedAt = r.CreatedAt,
                    Title = r.Title
                })
                .ToListAsync();

            // Không cần kiểm tra null hoặc danh sách trống, trả về luôn
            return reviews;
        }






        public async Task<ReviewResponseModel> CreateReviewAsync(ReviewRequestModel reviewRequest)
            {
                var review = new Review
                {
                    ReviewId = Guid.NewGuid(),
                    Username = reviewRequest.Username,
                    SpaceId = reviewRequest.SpaceId,
                    Rating = reviewRequest.Rating,
                    Comment = reviewRequest.Comment,
                    Title = reviewRequest.Title,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Reviews.Add(review);
                await _context.SaveChangesAsync();

                return new ReviewResponseModel
                {
                    ReviewId = review.ReviewId,
                    Username = review.Username,
                    SpaceId = review.SpaceId,
                    Rating = review.Rating,
                    Comment = review.Comment,
                    Title = review.Title,
                    CreatedAt = review.CreatedAt
                };
            }


        public async Task<Review> UpdateReviewAsync(Review review)
            {
                var existingReview = await _context.Reviews.FindAsync(review.ReviewId);
                if (existingReview == null) return null;

                existingReview.Rating = review.Rating;
                existingReview.Comment = review.Comment;
                existingReview.Title = review.Title;
                // Add other properties to update if needed

                await _context.SaveChangesAsync();
                return existingReview;
            }

        public async Task<bool> DeleteReviewAsync(Guid reviewId)
        {
            var review = await _context.Reviews.FindAsync(reviewId);
            if (review == null) return false;

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
