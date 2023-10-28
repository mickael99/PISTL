using DAT_project.API.Repositories.Interface;

namespace DAT_project.API.Repositories.Implementation
{
    public class SessionRepository : ISessionRepository
    {
        private readonly DatdbContext dbContext;

        public SessionRepository(DatdbContext dbContext) 
        {
            this.dbContext = dbContext;
        }
        public async Task<Session> CreateAsync(Session session)
        {
            await dbContext.Sessions.AddAsync(session);
            await dbContext.SaveChangesAsync();

            return session;
        }
    }
}
