namespace Quik_BookingApp.Helper
{
    public class APIResponse
    {
        public int ResponseCode { get; set; }
        public string Result { get; set; }
        public string Message { get; set; }
    }

    public class APIResponseData
    {
        public int ResponseCode { get; set; }
        public string Result { get; set; }
        public string Message { get; set; }
        public object Data {  get; set; }
    }

    public interface IServiceResult
    {
        int Status { get; set; }
        string? Message { get; set; }
        object? Data { get; set; }
    }

    public class ServiceResult : IServiceResult
    {
        public int Status { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }

        public ServiceResult()
        {
            Status = -1;
            Message = "Action fail";
        }

        public ServiceResult(int status, string message)
        {
            Status = status;
            Message = message;
        }

        public ServiceResult(int status, string message, object data)
        {
            Status = status;
            Message = message;
            Data = data;
        }
    }
}
