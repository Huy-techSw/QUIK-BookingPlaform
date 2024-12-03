namespace Quik_BookingApp.DAO.Models
{
    public class Review
    {
        public Guid ReviewId { get; set; }
        public string Username { get; set; }
        public string SpaceId { get; set; }
        public float Rating { get; set; }
        public string Comment { get; set; }
        public string Title {  get; set; }
        public DateTime CreatedAt { get; set; }
        

        public User User { get; set; }
        public WorkingSpace WorkingSpace { get; set; }
    }
}
