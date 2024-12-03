namespace Quik_BookingApp.BOs.Request
{
    public class CreateWSRequest
    {
        public string ImageId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal PricePerHour { get; set; }
        public int Capacity { get; set; }
        public string Location { get; set; }
    }
}
