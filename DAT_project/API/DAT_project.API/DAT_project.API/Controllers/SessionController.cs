using DAT_project.API.Models.DTO;
using DAT_project.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DAT_project.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        private readonly ISessionRepository sessionRepository;

        public SessionController(ISessionRepository sessionRepository)
        {
            this.sessionRepository = sessionRepository;
        }

        //ajouter authent de token (JWT)
        [HttpPost]
        public async Task<IActionResult> CreateSession(CreateSessionRequestDTO request)
        {
            // Map DTO to domain model
            var session = new Session
            {
                SessionId = request.SessionId,
                Date = request.Date,
                UserHostName = request.UserHostName
            };

            await sessionRepository.CreateAsync(session);

            //Domain model to DTO
            var response = new SessionDTO
            {
                SessionId = session.SessionId,
                Date = session.Date,
                UserHostName = session.UserHostName
            };

            return Ok(response);
        }
    }
}
