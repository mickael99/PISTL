using DAT_project.API.Models.DTO;
using DAT_project.API.Repositories.Implementation;
using DAT_project.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DAT_project.API.Controllers
{
    //https://localhost:xxxx/api/login
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginRepository loginRepository;

        public LoginController(ILoginRepository loginRepository)
        {
            this.loginRepository = loginRepository;
        }

        //ajouter authent de token (JWT)
        [HttpPost]
        public async Task<IActionResult> CreateLogin(CreateLoginRequestDTO request)
        {
            // Map DTO to domain model
            var login = new Login
            {
                Email = request.Email,
                Password = request.Password
            };

            await loginRepository.CreateAsync(login);

            //Domain model to DTO
            var response = new LoginDTO
            {
                Email = login.Email,
                Password = login.Password
            };

            return Ok(response);
        }
    }
}
