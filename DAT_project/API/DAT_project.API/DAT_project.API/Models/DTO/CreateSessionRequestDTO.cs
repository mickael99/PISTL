namespace DAT_project.API.Models.DTO
{
    public class CreateSessionRequestDTO
    {
        public int Id { get; set; }

        public Guid SessionId { get; set; }

        public DateTime Date { get; set; }

        public int LoginId { get; set; }

        public string Action { get; set; } = null!;

        public string? UserHostAddress { get; set; }

        public string? UserHostName { get; set; }

        public string? UserAgent { get; set; }

        public string? Data { get; set; }

        public virtual Login Login { get; set; } = null!;
    }
}
