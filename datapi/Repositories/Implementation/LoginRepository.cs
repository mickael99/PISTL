using DAT_project.API.Models;

namespace DAT_project.API.Repositories.Implementation
{
    public class LoginRepository : ILoginRepository
    {
        private readonly DatdbContext dbContext;

        public LoginRepository(DatdbContext dbContext) 
        {
            this.dbContext = dbContext;
        }
        public async Task<Login> CreateAsync(Login login)
        {
            await dbContext.Logins.AddAsync(login);
            await dbContext.SaveChangesAsync();

            return login;
        }
    }
}
