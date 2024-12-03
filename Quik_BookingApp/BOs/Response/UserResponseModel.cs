namespace Quik_BookingApp.BOs.Response
{
    public class UserResponseModel
    {
        public long Id { get; set; }

        public string? UserName { get; set; }

        public string Email { get; set; } = null!;

        public int? Role { get; set; }

        public string AccessToken { get; set; } = null!;
    }
}
