namespace WorkhoursTracker.Models
{
    public class LoginResponse
    {
        public string jwt { get; set; } = string.Empty;
        public int employeeId { get; set; }
    }
}
