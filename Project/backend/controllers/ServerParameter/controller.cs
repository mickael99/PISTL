using Project.Models;
using Microsoft.AspNetCore.Mvc;
using Project.Models.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Project.Controllers
{
    [Route("api/serverparameter")] 
    [ApiController]
    public class ServerParametersController : ControllerBase
    {
        [HttpGet("{server_id}")]
        public IActionResult GetServerParametersByServer(int server_id)
        {
            try
            {
                var context = new MasterContext();
                var servers = context.Servers
                    .Where(server => context.ServerParameters.Select(serverParameter => serverParameter.ServerId).Count() > 0)
                    .Select(server => new ServerDTO
                    {
                        ServerId = server.ServerId,
                        Address = server.Address,
                        Name = server.Name
                    })
                    .ToList();

                if (servers.Count == 0)
                {
                    return NotFound("No servers found.");
                }

                var server_parameters = context.ServerParameters
                    .Where(serverParameter => serverParameter.ServerId == server_id)
                    .Select(server_parameter => new ServerParameterDTO
                    {
                        ServerId = server_parameter.ServerId,
                        ParameterKey = server_parameter.ParameterKey,
                        ParameterValue = server_parameter.ParameterValue
                    })
                    .ToList();

                return Ok(new { servers, server_parameters });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

    }

}