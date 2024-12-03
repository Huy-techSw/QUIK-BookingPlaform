namespace Quik_BookingApp.BOs.Request
{
    public class LoginModel
    {
        /// <summary>
        /// Email
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        public required string Password { get; set; }
    }
}
