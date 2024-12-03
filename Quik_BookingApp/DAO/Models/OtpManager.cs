namespace Quik_BookingApp.DAO.Models
{
    public class OtpManager
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string OtpText { get; set; }
        public string OtpType { get; set; }
        public DateTime Expiration { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
