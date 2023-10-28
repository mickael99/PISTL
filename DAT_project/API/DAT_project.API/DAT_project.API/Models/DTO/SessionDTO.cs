namespace DAT_project.API.Models.DTO
{
    public class SessionDTO
    {
        public Guid SessionId { get; set; }
        public DateTime Date { get; set; }
        public string? UserHostName { get; set; }
    }
}
