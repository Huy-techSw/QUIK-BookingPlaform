namespace Quik_BookingApp.BOs.Request
{
    public class CreatePaymentRequest
    {
        public string PaymentStatus { get; set; }
        public double Amount { get; set; }
        public string BookingId { get; set; }
    }
}
