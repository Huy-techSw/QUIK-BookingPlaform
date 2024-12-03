namespace Quik_BookingApp.BOs.Request
{
    public class ReviewRequestModel
    {
        public string Username { get; set; }
        public string SpaceId { get; set; }
        public float Rating { get; set; }
        public string Title { get; set; }
        public string Comment { get; set; }
    }
}
