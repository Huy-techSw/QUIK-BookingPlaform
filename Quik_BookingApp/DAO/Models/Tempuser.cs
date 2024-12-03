namespace Quik_BookingApp.DAO.Models
{
    public class Tempuser
    {
        public string Id { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
    }
}
