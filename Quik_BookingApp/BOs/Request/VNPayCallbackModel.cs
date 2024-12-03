namespace Quik_BookingApp.BOs.Request
{
    public class VNPayCallbackModel
    {
        public string vnp_Amount { get; set; }
        public string vnp_TxnRef { get; set; }
        public string vnp_ResponseCode { get; set; }
        public string vnp_SecureHash { get; set; }
    }

}
