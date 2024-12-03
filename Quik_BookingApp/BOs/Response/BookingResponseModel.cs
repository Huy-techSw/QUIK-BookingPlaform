namespace Quik_BookingApp.BOs.Response
{
    public class BookingResponseModel
    {
        public string BookingId { get; set; }
        public string Username { get; set; }
        public string SpaceId { get; set; }
        public Guid PaymentId { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int NumberOfPeople { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DepositAmount { get; set; }
        public decimal RemainingAmount { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }

        public string BusinessName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal PricePerHour { get; set; }
        public string RoomType { get; set; }
        public int Capacity { get; set; }
        public string ImageUrl { get; set; }
    }
}
