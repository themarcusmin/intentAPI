using IntentAPI.Models;

namespace IntentAPI.DTO
{
    public class CreateEventDTO
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? Location { get; set; }
        public DateTime EventStartTime { get; set; }
        public DateTime EventEndTime { get; set; }
        public string RepeatMode { get; set; }
        public DateTime? RepeatEndDate { get; set; }
    }
}
