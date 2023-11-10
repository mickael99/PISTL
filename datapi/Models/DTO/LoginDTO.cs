namespace DAT_project.API.Models.DTO
{
    public class LoginDTO
    {
        public string? Email { get; set; }
        public byte[] Password { get; set; } = null!;
    }
}
