using System.ComponentModel.DataAnnotations;

namespace IntentAPI.Models
{
    public enum RecurringMode
    {
        NEVER,
        DAILY,
        WEEKLY,
        MONTHLY,
        YEARLY
    }
    public class Recurring
    {
        public int RecurringId { get; set; }

        [Required]
        public RecurringMode RecurringMode { get; set; }
        [Required]
        public DateTime EndDate { get; set; }

        public int EventId { get; set; }
        public Event Event { get; set; } = null!;
    }
}
