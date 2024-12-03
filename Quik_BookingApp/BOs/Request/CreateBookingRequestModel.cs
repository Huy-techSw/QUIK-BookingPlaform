namespace Quik_BookingApp.BOs.Request
{
    public class CreateBookingRequestModel
    {
        public string Username { get; set; }
        public string SpaceId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int NumberOfPeople { get; set; }
    }
}
