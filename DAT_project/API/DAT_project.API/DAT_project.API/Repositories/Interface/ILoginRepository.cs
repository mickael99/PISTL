namespace DAT_project.API.Repositories.Implementation
{
    public interface ILoginRepository
    {
        Task<Login> CreateAsync(Login login);
    }
}