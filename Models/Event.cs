using System.ComponentModel.DataAnnotations;

namespace IntentAPI.Models
{
    public class Event
    {
        public int EventId { get; set; }

        public string Title { get; set; }
        public string? Description { get; set; }

        public string? Location { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public bool IsRecurring { get; set; }

        public int UserId { get; set; }

        [Required]
        public string FirebaseUserId { get; set; }

        public required User User { get; set; } = null!;

        public Recurring? Recurring { get; set; }
    }
}
