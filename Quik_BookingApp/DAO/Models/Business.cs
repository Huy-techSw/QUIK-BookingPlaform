using System.Text.Json.Serialization;

namespace Quik_BookingApp.DAO.Models

{
    public class Business
    {
        public string BusinessId { get; set; }
        public string BusinessName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; } 
        public string Location { get; set; }
        public string Description { get; set; }
        public float Rating { get; set; }

        [JsonIgnore]
        public ICollection<WorkingSpace> WorkingSpaces { get; set; }
        
    }
}
