namespace Continuous_Learning_Booking.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public string LecturerName { get; set; }
        public string Subject { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; } // Pending, Approved, Rejected
    }

}
