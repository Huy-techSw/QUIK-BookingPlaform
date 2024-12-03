namespace Quik_BookingApp.BOs.Response
{
    public class ReviewResponseModel
    {
        public Guid ReviewId { get; set; }
        public string Username { get; set; }
        public string SpaceId { get; set; }
        public float Rating { get; set; }
        public string Comment { get; set; }
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; }



    }
}
