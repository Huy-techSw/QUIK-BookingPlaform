using System.Text.Json.Serialization;

namespace Quik_BookingApp.DAO.Models
{
    public class ImageWS
    {
        public string ImageId { get; set; }
        public string SpaceId { get; set; }
        public string WorkingSpaceName { get; set; }
        public string ImageUrl { get; set; }
        public string WSCode { get; set; }
        public byte[]? WSImages { get; set; }

        [JsonIgnore]
        public WorkingSpace WorkingSpace { get; set; }
    }
}
