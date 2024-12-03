namespace Quik_BookingApp.BOs.Response
{
    public class WSWBNameResponse
    {
        
            public string SpaceId { get; set; }
            public string BusinessId { get; set; }
            public string ImageId { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public decimal PricePerHour { get; set; }
            public string RoomType { get; set; }
            public int Capacity { get; set; }
            public string Location { get; set; }

            public string BusinessName { get; set; }
        
    }
}
