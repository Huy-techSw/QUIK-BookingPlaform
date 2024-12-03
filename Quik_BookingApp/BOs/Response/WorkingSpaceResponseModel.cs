namespace Quik_BookingApp.BOs.Response
{

    public class WorkingSpaceResponse
    {
        public string SpaceId { get; set; }
        public string BusinessId { get; set; }
        public string ImageId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal PricePerHour { get; set; }
        public string RoomType {  get; set; }
        public int Capacity { get; set; }
        public string Location { get; set; }

        // Optionally include Business information if needed
        public BusinessResponse Business { get; set; }
        public ICollection<BookingResponse> Bookings { get; set; }
    }

    public class BusinessResponse
    {
        public string BusinessId { get; set; }
        public string BusinessName { get; set; }
        // Add other relevant fields as necessary
    }

    public class BookingResponse
    {
        public string BookingId { get; set; }
        public string UserId { get; set; }
        public string SpaceId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        // Add other relevant fields as necessary
    }
}
