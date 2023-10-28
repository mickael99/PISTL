using DAT_project.API.Models.DTO;
using DAT_project.API.Repositories.Implementation;
using DAT_project.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DAT_project.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginRepository loginRepository;

        public LoginController(ILoginRepository loginRepository)
        {
            this.loginRepository = loginRepository;
        }


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
