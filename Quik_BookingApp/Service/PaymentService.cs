using Microsoft.EntityFrameworkCore;
using Quik_BookingApp.DAO;
using Quik_BookingApp.DAO.Models;
using Quik_BookingApp.Repos.Interface;

namespace Quik_BookingApp.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly QuikDbContext _context;

        public PaymentService(QuikDbContext context)
        {
            _context = context;
        }

        public async Task SavePaymentAsync(Payment payment)
        {
            if (payment == null)
            {
                throw new ArgumentNullException(nameof(payment));
            }

            // Lưu thông tin thanh toán vào cơ sở dữ liệu
            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();
        }

        public async Task<Payment> GetPaymentByBookingIdAsync(Guid bookingId)
        {
            return await _context.Payments.FirstOrDefaultAsync(p => p.BookingId.Equals(bookingId));
        }

        public async Task UpdatePaymentStatusAsync(Guid paymentId, string status)
        {
            var payment = await _context.Payments.FindAsync(paymentId);
            if (payment == null)
            {
                throw new KeyNotFoundException("Payment not found");
            }

            payment.PaymentStatus = status;
            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();
        }
    }



}
