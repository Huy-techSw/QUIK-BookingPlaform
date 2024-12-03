using NuGet.Versioning;

namespace Quik_BookingApp.BOs.Request
{
    public class WorkingSpaceRequestRatingMode
    {
        public string SpaceId { get; set; }
        public string BusinessId { get; set; } 
        public string BusinessName {  get; set; }
        public string ImageId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal PricePerHour { get; set; }
        public string RoomType { get; set; }
        public int Capacity { get; set; }
        public string Location { get; set; }
        public IFormFile Image { get; set; }
        public float Rating {  get; set; }
        public List<string> ImageUrls { get; set; }


    }
}
