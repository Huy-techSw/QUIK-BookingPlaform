using Quik_BookingApp.DAO.Models;

namespace Quik_BookingApp.Repos.Interface
{
    public interface IPaymentService
    {
        Task SavePaymentAsync(Payment payment);
        Task<Payment> GetPaymentByBookingIdAsync(Guid bookingId);
        Task UpdatePaymentStatusAsync(Guid paymentId, string status);
    }

}
