namespace Project.Repositories.Imprementation
{
    public class CreateLoginDomainUserRequestDTO
    {
        public int LoginId { get; set; }
        public int DomainId { get; set; }
        public string? Email { get; set; }
    }
}
