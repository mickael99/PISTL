using Project.Models;

namespace Project.Repositories.Interface
{
    public interface ILoginDomainUserRepository
    {
        Task<LoginDomainUser> CreateAsync(LoginDomainUser user);
    }
}
