namespace Quik_BookingApp.DAO.Models
{
    public class Payment
    {
        public Guid PaymentId { get; set; }
        public string BookingId { get; set; } // Foreign key for Booking

        public double Amount { get; set; }
        public string PaymentMethod { get; set; } // E.g., VNPay, Credit Card, PayPal
        public DateTime PaymentDate { get; set; }
        public string PaymentStatus { get; set; } // E.g., Success, Failed, Pending

        public string VNPayTransactionId { get; set; } // VNPay's transaction reference
        public string VNPayResponseCode { get; set; } // VNPay's response code
        public string PaymentUrl { get; set; } // Optional: Store generated VNPay URL

        public Booking Booking { get; set; }
    }
}
