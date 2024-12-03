
using System.Text.Json.Serialization;

namespace Quik_BookingApp.DAO.Models
{
    public class WorkingSpace
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
        public float Rating {  get; set; }
        

        
        public Business Business { get; set; }
        public ICollection<Booking> Bookings { get; set; }
        public ICollection<Amenity> Amenities { get; set; }
      
        public ICollection<ImageWS> Images { get; set; } = new List<ImageWS>();
        public ICollection<Review> Reviews { get; set; }
        

    }

}
