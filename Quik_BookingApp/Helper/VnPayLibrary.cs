using System.Net.Sockets;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Globalization;
using Quik_BookingApp.BOs.Response;

namespace Quik_BookingApp.Helper
{
    //public class VnPayLibrary
    //{
    //private SortedList<string, string> _requestData = new SortedList<string, string>();
    //private SortedList<string, string> _responseData = new SortedList<string, string>();

    //public void AddRequestData(string key, string value)
    //{
    //    if (!string.IsNullOrEmpty(value))
    //    {
    //        _requestData.Add(key, value);
    //    }
    //}

    //public void AddResponseData(string key, string value)
    //{
    //    if (!string.IsNullOrEmpty(value))
    //    {
    //        _responseData.Add(key, value);
    //    }
    //}

    //public string CreateRequestUrl(string baseUrl, string hashSecret)
    //{
    //    var data = string.Join("&", _requestData.Select(kv => $"{kv.Key}={HttpUtility.UrlEncode(kv.Value)}"));
    //    var rawData = string.Join("&", _requestData.Select(kv => $"{kv.Key}={kv.Value}"));
    //    var secureHash = HmacSHA512(hashSecret, rawData);
    //    return $"{baseUrl}?{data}&vnp_SecureHash={secureHash}";
    //}

    //public bool ValidateSignature(string receivedHash, string hashSecret)
    //{
    //    var rawData = string.Join("&", _responseData.Where(kv => kv.Key != "vnp_SecureHash").Select(kv => $"{kv.Key}={kv.Value}"));
    //    var expectedHash = HmacSHA512(hashSecret, rawData);
    //    return receivedHash.Equals(expectedHash, StringComparison.InvariantCultureIgnoreCase);
    //}

    //private string HmacSHA512(string key, string inputData)
    //{
    //    var hash = new HMACSHA512(Encoding.UTF8.GetBytes(key));
    //    var hashBytes = hash.ComputeHash(Encoding.UTF8.GetBytes(inputData));
    //    return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
    //}
    //}

    public class VnPayLibrary
    {
        private readonly SortedList<string, string> _requestData = new SortedList<string, string>(new VnPayCompare());
        private readonly SortedList<string, string> _responseData = new SortedList<string, string>(new VnPayCompare());
        public VNPayPaymentResponseModel GetFullResponseData(IQueryCollection collection, string hashSecret)
        {


            var vnPay = new VnPayLibrary();

            foreach (var (key, value) in collection)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    vnPay.AddResponseData(key, value);
                }
            }

            var orderId = Convert.ToInt64(vnPay.GetResponseData("vnp_TxnRef"));
            var vnPayTranId = Convert.ToInt64(vnPay.GetResponseData("vnp_TransactionNo"));
            var vnpResponseCode = vnPay.GetResponseData("vnp_ResponseCode");
            var vnpSecureHash =
                collection.FirstOrDefault(k => k.Key == "vnp_SecureHash").Value;
            var amountRental = vnPay.GetResponseData("vnp_Amount");

            var checkSignature =
                vnPay.ValidateSignature(vnpSecureHash, hashSecret); //check Signature

            if (!checkSignature)
                return new VNPayPaymentResponseModel()
                {
                    Success = false,
                    OrderDescription = this.GetResponseData(),


                };
            if (vnpResponseCode != "00")
            {
                return new VNPayPaymentResponseModel()
                {
                    Success = false,
                    OrderDescription = this.GetResponseData(),

                };
            }
            else
            {
                return new VNPayPaymentResponseModel()
                {
                    Success = true,
                    PaymentMethod = "VnPay",
                    //AmountOfRental = decimal.Parse(amountRental),
                    OrderDescription = this.GetResponseData(),
                    OrderId = orderId.ToString(),
                    PaymentId = vnPayTranId.ToString(),
                    TransactionId = vnPayTranId.ToString(),
                    Token = vnpSecureHash,
                    VnPayResponseCode = vnpResponseCode,

                };
            }
        }
        public string GetIpAddress(HttpContext context)
        {
            var ipAddress = string.Empty;
            try
            {
                var remoteIpAddress = context.Connection.RemoteIpAddress;

                if (remoteIpAddress != null)
                {
                    if (remoteIpAddress.AddressFamily == AddressFamily.InterNetworkV6)
                    {
                        remoteIpAddress = Dns.GetHostEntry(remoteIpAddress).AddressList
                            .FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork);
                    }

                    if (remoteIpAddress != null) ipAddress = remoteIpAddress.ToString();

                    return ipAddress;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return "127.0.0.1";
        }
        public void AddRequestData(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                _requestData.Add(key, value);
            }
        }

        public void AddResponseData(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                _responseData.Add(key, value);
            }
        }

        public string GetResponseData(string key)
        {
            return _responseData.TryGetValue(key, out var retValue) ? retValue : string.Empty;
        }

        public string CreateRequestUrl(string baseUrl, string vnpHashSecret)
        {
            var data = new StringBuilder();

            foreach (var (key, value) in _requestData.Where(kv => !string.IsNullOrEmpty(kv.Value)))
            {
                data.Append(WebUtility.UrlEncode(key) + "=" + WebUtility.UrlEncode(value) + "&");
            }

            var querystring = data.ToString();

            baseUrl += "?" + querystring;
            var signData = querystring;
            if (signData.Length > 0)
            {
                signData = signData.Remove(data.Length - 1, 1);
            }

            var vnpSecureHash = HmacSha512(vnpHashSecret, signData);
            baseUrl += "vnp_SecureHash=" + vnpSecureHash;

            return baseUrl;
        }

        public bool ValidateSignature(string inputHash, string secretKey)
        {
            var rspRaw = GetResponseData();
            var myChecksum = HmacSha512(secretKey, rspRaw);
            return myChecksum.Equals(inputHash, StringComparison.InvariantCultureIgnoreCase);
        }

        private string HmacSha512(string key, string inputData)
        {
            var hash = new StringBuilder();
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var inputBytes = Encoding.UTF8.GetBytes(inputData);
            using (var hmac = new HMACSHA512(keyBytes))
            {
                var hashValue = hmac.ComputeHash(inputBytes);
                foreach (var theByte in hashValue)
                {
                    hash.Append(theByte.ToString("x2"));
                }
            }

            return hash.ToString();
        }

        private string GetResponseData()
    {
        var data = new StringBuilder();
        if (_responseData.ContainsKey("vnp_SecureHashType"))
        {
            _responseData.Remove("vnp_SecureHashType");
        }

        if (_responseData.ContainsKey("vnp_SecureHash"))
        {
            _responseData.Remove("vnp_SecureHash");
        }

        foreach (var (key, value) in _responseData.Where(kv => !string.IsNullOrEmpty(kv.Value)))
        {
            data.Append(WebUtility.UrlEncode(key) + "=" + WebUtility.UrlEncode(value) + "&");
        }

        //remove last '&'
        if (data.Length > 0)
        {
            data.Remove(data.Length - 1, 1);
        }

        return data.ToString();
    }
}

public class VnPayCompare : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if (x == y) return 0;
            if (x == null) return -1;
            if (y == null) return 1;
            var vnpCompare = CompareInfo.GetCompareInfo("en-US");
            return vnpCompare.Compare(x, y, CompareOptions.Ordinal);
        }
    }
}
