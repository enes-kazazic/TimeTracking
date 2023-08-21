namespace WorkhoursTracker.Models
{
    public class Items
    {
        public int Id { get; set; }
        public DateTime logDateTime { get; set; }
        public string purposeName { get; set; } = string.Empty;
        public string logSource { get; set; } = string.Empty;
    }
}