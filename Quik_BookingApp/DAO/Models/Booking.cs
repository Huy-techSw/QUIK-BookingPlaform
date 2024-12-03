namespace Quik_BookingApp.DAO.Models
{
    public class Booking
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
        public string Status { get; set; }

        public User User { get; set; }
        public WorkingSpace WorkingSpace { get; set; }
        public Payment Payment { get; set; }
    }


}
