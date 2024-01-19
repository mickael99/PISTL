using System;
using Project.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Project.Interface;
using Microsoft.AspNetCore.Http.HttpResults;
using Project.Repository;
using System.Net;
using Microsoft.Extensions.Logging;


[Route("api/serverParameter")]
[ApiController]

public class ServerParameterController : Controller
{

    private readonly IServerRepository _serverRepository;
    private readonly IServerParameterRepository _serverParameterRepository;
    private readonly DatContext context;

        public ServerParameterController(IServerRepository serverRepository, DatContext context, IServerParameterRepository serverParameterRepository)
    {
        _serverRepository = serverRepository;
        _serverParameterRepository = serverParameterRepository;
        this.context = context;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = (typeof(IEnumerable<ServerParameter>)))]
    [ProducesResponseType(400)]
    public IActionResult GetServerParameters()
    {
        var serverParameters = _serverParameterRepository.GetServerParameters();

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return Ok(serverParameters);
    }

    


    
}