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
                var context = new DatContext();
                var servers = context.Servers.ToList();

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

        [HttpPost]
        public IActionResult CreateServerParameter(ServerParameterDTO server_parameter)
        {
            try
            {
                var context = new DatContext();

                var server_parameter_to_add = new ServerParameter
                {
                    ServerId = server_parameter.ServerId,
                    ParameterKey = server_parameter.ParameterKey,
                    ParameterValue = server_parameter.ParameterValue
                };

                context.ServerParameters.Add(server_parameter_to_add);
                context.SaveChanges();

                var server_parameters = context.ServerParameters
                    .Where(serverParameter => serverParameter.ServerId == server_parameter.ServerId)
                    .Select(server_parameter => new ServerParameterDTO
                    {
                        ServerId = server_parameter.ServerId,
                        ParameterKey = server_parameter.ParameterKey,
                        ParameterValue = server_parameter.ParameterValue
                    })
                    .ToList();

                return Ok(server_parameters);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut]
        public IActionResult UpdateServerParameter(ServerParameterDTO server_parameter)
        {
            try
            {
                var context = new DatContext();

                var server_parameter_to_update = context.ServerParameters
                                                .Where(serverParameter => serverParameter.ServerId == server_parameter.ServerId 
                                                                          && serverParameter.ParameterKey == server_parameter.ParameterKey)
                                                .FirstOrDefault();

                if (server_parameter_to_update == null)
                {
                    return NotFound("Server parameter not found.");
                }

                server_parameter_to_update.ParameterValue = server_parameter.ParameterValue;
 
                context.SaveChanges();

                var server_parameters = context.ServerParameters
                    .Where(serverParameter => serverParameter.ServerId == server_parameter.ServerId)
                    .Select(server_parameter => new ServerParameterDTO
                    {
                        ServerId = server_parameter.ServerId,
                        ParameterKey = server_parameter.ParameterKey,
                        ParameterValue = server_parameter.ParameterValue
                    })
                    .ToList();

                return Ok(server_parameters);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete("{server_id}/{parameter_key}")]
        public IActionResult DeleteServerParameter(int server_id, string parameter_key)
        {
            try
            {
                var context = new DatContext();

                var server_parameter_to_delete = context.ServerParameters
                    .Where(serverParameter => serverParameter.ServerId == server_id && serverParameter.ParameterKey == parameter_key)
                    .FirstOrDefault();

                if (server_parameter_to_delete == null)
                {
                    return NotFound("Server parameter not found.");
                }

                context.ServerParameters.Remove(server_parameter_to_delete);
                context.SaveChanges();

                var server_parameters = context.ServerParameters
                    .Where(serverParameter => serverParameter.ServerId == server_id)
                    .Select(server_parameter => new ServerParameterDTO
                    {
                        ServerId = server_parameter.ServerId,
                        ParameterKey = server_parameter.ParameterKey,
                        ParameterValue = server_parameter.ParameterValue
                    })
                    .ToList();

                return Ok(server_parameters);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }

}