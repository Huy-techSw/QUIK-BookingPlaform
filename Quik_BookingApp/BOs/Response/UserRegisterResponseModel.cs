namespace Quik_BookingApp.BOs.Response
{
    public class UserRegisterResponseModel
    {
        public string Username { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } 
        public string ImageId { get; set; }
        public string PhoneNumber { get; set; }
        public bool OTPVerified { get; set; }
        public bool IsActive { get; set; }
        public bool IsLocked { get; set; }
        public string Status { get; set; }
        public string OTPText { get; set; }
        public string Message {  get; set; }
    }
}
