namespace IntentAPI.DTO
{
    public class DeleteEventDTO
    {
        public string Id { get; set; }
        public int? IdAsInt => int.TryParse(Id, out int val) ? val : null;
    }
}
