using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quik_BookingApp.BOs.Request;
using Quik_BookingApp.DAO;
using Quik_BookingApp.DAO.Models;
using Quik_BookingApp.Helper;
using Quik_BookingApp.Repos.Interface;
using Quik_BookingApp.Service;
using Swashbuckle.AspNetCore.Annotations;

namespace Quik_BookingApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IVnPayService _vnpayService; // Giả sử có một service để xử lý VNPay

        public PaymentController(IPaymentService paymentService, IVnPayService vnpayService)
        {
            _paymentService = paymentService;
            _vnpayService = vnpayService;
        }

        // POST: api/VNPay/CreatePaymentUrl
        [HttpPost("CreatePaymentUrl")]
        public IActionResult CreatePaymentUrl([FromBody] PaymentInformationModel model)
        {
            if (model == null || model.Amount <= 0)
            {
                return BadRequest(new { message = "Invalid payment information" });
            }

            var paymentUrl = _vnpayService.CreatePaymentUrl(model, HttpContext);

            if (!string.IsNullOrEmpty(paymentUrl))
            {
                return Ok(new { PaymentUrl = paymentUrl });
            }

            return StatusCode(500, new { message = "Failed to create payment URL" });
        }

        // GET: api/VNPay/PaymentCallback
        [HttpGet("PaymentCallback")]
        public IActionResult PaymentCallback()
        {
            var queryParams = Request.Query;

            if (queryParams.Count == 0)
            {
                return BadRequest(new { message = "No query parameters found" });
            }

            var paymentResponse = _vnpayService.PaymentExecute(queryParams);

            if (paymentResponse != null && paymentResponse.VnPayResponseCode == "200")
            {
                // Xử lý thành công thanh toán
                return Ok(new { message = "Payment successful", paymentResponse });
            }

            // Xử lý thanh toán thất bại
            return BadRequest(new { message = "Payment failed", paymentResponse });
        }

        // Tạo URL thanh toán cho VNPay
        //[HttpPost("create")]
        //public async Task<IActionResult> CreatePaymentUrl([FromBody] VNPayPaymentRequestModel model)
        //{
        //    var vnpayUrl = _vnpayService.CreatePaymentUrl(model.Amount, model.BookingId, model.Name);

        //    var payment = new Payment
        //    {
        //        PaymentId = Guid.NewGuid(),
        //        BookingId = model.BookingId,
        //        Amount = model.Amount,
        //        PaymentUrl = vnpayUrl,
        //        PaymentStatus = "Pending",
        //        PaymentMethod = "VNPay",
        //        VNPayResponseCode = "200",
        //        VNPayTransactionId = "Ok"

        //    };

        //    await _paymentService.SavePaymentAsync(payment);

        //    return Ok(new { paymentUrl = vnpayUrl });
        //}

        //// Xử lý callback từ VNPay
        //[HttpGet("callback")]
        //public async Task<IActionResult> PaymentCallback([FromQuery] VNPayCallbackModel model)
        //{
        //    var isValid = _vnpayService.ValidateSignature(model);
        //    if (!isValid)
        //    {
        //        return BadRequest("Invalid signature");
        //    }

        //    if (model.vnp_ResponseCode == "00")
        //    {
        //        await _paymentService.UpdatePaymentStatusAsync(Guid.Parse(model.vnp_TxnRef), "Success");
        //        return Ok("Payment successful");
        //    }
        //    else
        //    {
        //        await _paymentService.UpdatePaymentStatusAsync(Guid.Parse(model.vnp_TxnRef), "Failed");
        //        return BadRequest("Payment failed");
        //    }
        //}
    }



    //[HttpPost]
    //public async Task<IActionResult> CreatePaymentUrl([FromBody] VNPayPaymentRequestModel model)
    //{
    //    // Retrieve the booking and payment records based on BookingId
    //    var payment = await _context.Payments.FirstOrDefaultAsync(p => p.BookingId == model.BookingId);

    //    if (payment == null)
    //    {
    //        return BadRequest("Payment record not found.");
    //    }

    //    // Generate the payment URL using VNPayService
    //    var paymentUrl = _vnPayService.CreatePaymentUrl(model, HttpContext);

    //    // Save the payment URL into the Payment record
    //    payment.PaymentUrl = paymentUrl;

    //    // Save the updated payment record to the database
    //    await _context.SaveChangesAsync();

    //    // Return the generated payment URL as the response
    //    return Ok(paymentUrl);
    //}


    //[HttpGet("payment-callback")]
    //public async Task<IActionResult> PaymentCallback()
    //{
    //    var response = _vnPayService.PaymentExecute(Request.Query);
    //    var paymentResponseModel = response;

    //    // Parse order description to retrieve the bookingId from the response
    //    var parts = paymentResponseModel.OrderDescription?.Split(' ') ?? new string[0];
    //    Guid bookingId = Guid.Empty;

    //    if (parts.Length > 1)
    //    {
    //        Guid.TryParse(parts[1], out bookingId);
    //    }

    //    // Find the booking and payment records based on the bookingId
    //    var payment = _context.Payments.FirstOrDefault(p => p.BookingId == bookingId.ToString());
    //    var booking = _context.Bookings.FirstOrDefault(b => b.BookingId == bookingId.ToString());

    //    if (payment == null || booking == null)
    //    {
    //        // If no payment or booking is found, return an error
    //        return BadRequest("Payment or booking not found.");
    //    }

    //    // If the payment is successful
    //    if (response.Success)
    //    {
    //        // Update the payment status to Success
    //        payment.PaymentStatus = "Success";
    //        payment.VNPayTransactionId = response.TransactionId;
    //        payment.VNPayResponseCode = response.VnPayResponseCode;
    //        payment.PaymentDate = DateTime.Now;

    //        // Update the booking status to confirmed
    //        booking.Status = "Confirmed";

    //        await _context.SaveChangesAsync();

    //        // Redirect user to the success page on the frontend
    //        return Redirect($"http://localhost:1024/payment-status?status=success&bookingId={bookingId}");
    //    }
    //    else
    //    {
    //        // If the payment failed, update the status accordingly
    //        payment.PaymentStatus = "Failed";
    //        payment.VNPayResponseCode = response.VnPayResponseCode;

    //        await _context.SaveChangesAsync();

    //        // Redirect user to the failure page on the frontend
    //        return Redirect($"http://localhost:1024/payment-status?status=failed&bookingId={bookingId}");
    //    }
    //}

}
