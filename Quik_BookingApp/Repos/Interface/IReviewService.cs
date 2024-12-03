using Quik_BookingApp.BOs.Request;
using Quik_BookingApp.BOs.Response;
using Quik_BookingApp.DAO.Models;

namespace Quik_BookingApp.Repos.Interface
{
    public interface IReviewService
    {
        Task<List<ReviewResponseModel>> GetAllReviewsAsync();
        Task<Review> GetReviewByIdAsync(Guid reviewId);
        Task<List<ReviewResponseModel>> GetReviewsBySpaceIdAsync(string spaceId);
        Task<ReviewResponseModel> CreateReviewAsync(ReviewRequestModel reviewRequest);
        Task<Review> UpdateReviewAsync(Review review);
        Task<bool> DeleteReviewAsync(Guid reviewId);
    }

}
