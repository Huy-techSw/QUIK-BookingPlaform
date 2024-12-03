namespace Quik_BookingApp.Modal
{
    public class UserModal
    {
        public string Username { get; set; }
        public string Name { get; set; }
        public string Email { get; set; } 
        public string Password { get; set; } 
        public string Role { get; set; } 
        public string PhoneNumber { get; set; } 
        public bool OTPVerified { get; set; }
        public Boolean IsActive { get; set; } 
        public Boolean IsLocked { get; set; }
        public string Status { get; set; }
        
        
    }
}
