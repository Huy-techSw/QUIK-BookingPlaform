using Quik_BookingApp.BOs.Request;
using Quik_BookingApp.BOs.Response;
using Quik_BookingApp.Helper;


namespace Quik_BookingApp.Repos.Interface
{
    public interface IBookingService
    {
        Task<List<BookingResponseModel>> GetAllBookings();
        Task<APIResponseData> BookSpace(BookingRequestModel bookingRequest);
        Task<BusinessResponseModel> GetBookingById(string bookingId);
        Task<BusinessResponseModel> UpdateBookingStatusToPaid(string bookingId);
        Task<BusinessResponseModel> UpdateBookingStatusToUnPaid(string bookingId);

        Task<BusinessResponseModel> UpdateBooking(string bookingId, BookingResponseModel bookingRequest);
        Task<APIResponse> DeleteBooking(string bookingId);
        Task<List<BookingResponseModel>> GetBookingOfChuaThanhToan();
        Task<List<BookingResponseModel>> GetBookingOfDaThanhToan();
        Task<List<BookingResponseModel>> GetAllBookingsByUsername(string username);


    }
}
