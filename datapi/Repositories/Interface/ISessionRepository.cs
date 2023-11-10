using DAT_project.API.Models;

namespace DAT_project.API.Repositories.Interface
{
    public interface ISessionRepository
    {
        Task<Session> CreateAsync(Session session);
    }
}
