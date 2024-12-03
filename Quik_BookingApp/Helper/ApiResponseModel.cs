using System.Net;

namespace Quik_BookingApp.Helper
{
    public class ApiResponseModel<T>
    {
        public HttpStatusCode StatusCode { get; set; }

        public string Message { get; set; } = null!;

        public T? Data { get; set; }
    }
}
