using Quik_BookingApp.BOs.Request;

namespace Quik_BookingApp.Helper
{
    public class VNPayHelper
    {
        private readonly IConfiguration _configuration;

        public VNPayHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreatePaymentUrl(VNPayPaymentRequestModel model, HttpContext httpContext)
        {
            var vnpayUrl = _configuration["VNPaySettings:VNPayUrl"];
            var returnUrl = _configuration["VNPaySettings:ReturnUrl"];
            var tmnCode = _configuration["VNPaySettings:TmnCode"];
            var hashSecret = _configuration["VNPaySettings:HashSecret"];

            var vnpay = new VnPayLibrary();
            vnpay.AddRequestData("vnp_Version", "2.1.0");
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", tmnCode);
            vnpay.AddRequestData("vnp_Amount", (model.Amount * 100).ToString()); // VNPay calculates in VND x100
            vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_OrderInfo", $"{model.Name} - Booking {model.BookingId}"); // Includes Name and BookingId in the description
            vnpay.AddRequestData("vnp_ReturnUrl", returnUrl);

            var paymentUrl = vnpay.CreateRequestUrl(vnpayUrl, hashSecret); // Generate the payment URL
            return paymentUrl; // Return the payment URL
        }
    }
}
