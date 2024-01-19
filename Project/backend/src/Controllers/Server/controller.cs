using System;
using Project.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Project.Interface;
using Microsoft.AspNetCore.Http.HttpResults;
using Project.Repository;
using System.Net;
using Microsoft.Extensions.Logging;


[Route("api/server")]
[ApiController]

public class ServerController : Controller
{

    private readonly IServerRepository _serverRepository;
    private readonly DatContext context;
    private readonly ILogger<ServerController> _logger;  // Add this line


    public ServerController(IServerRepository serverRepository, DatContext context, ILogger<ServerController> logger)
    {
        _serverRepository = serverRepository;
        this.context = context;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = (typeof(IEnumerable<Server>)))]
    [ProducesResponseType(400)]
    public IActionResult GetServers()
    {
        var servers = _serverRepository.GetServers();

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return Ok(servers);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(200, Type = (typeof(Server)))]
    [ProducesResponseType(400)]
    public IActionResult GetServer(int id)
    {
        _logger.LogInformation("----------------->This is a log message");

        if (!_serverRepository.ServerExists(id))
            return NotFound();

        var server = _serverRepository.GetServer(id);

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return Ok(server);
    }

    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public IActionResult CreateServer([FromBody] Server serverCreate)
    {
        _logger.LogInformation("-------->Create Server");

        if (!ModelState.IsValid)
        {
            _logger.LogInformation("------------------------->Model validation failed. ModelState: {0}", ModelState);
            return BadRequest(ModelState);
        }

        _logger.LogInformation("-------->start creating");

        try
        {
            context.Servers.Add(new Server
            {
                ServerId = _serverRepository.GetUnusedMinServerId(),
                Name = serverCreate.Name,
                Address = serverCreate.Address,
                Context = serverCreate.Context,
                Type = serverCreate.Type,
                ModifiedBy = serverCreate.ModifiedBy,
                CreatedBy = serverCreate.CreatedBy,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
            });

            _logger.LogInformation("---------->data enter finish");

            context.SaveChanges();

            _logger.LogInformation("---------->saved");

            var servers = _serverRepository.GetServers();

            return Ok(new { servers });
        }
        catch (Exception ex)
        {
            _logger.LogInformation("--------------------->Exception: " + ex.Message);
            _logger.LogInformation("--------------------->StackTrace: " + ex.StackTrace);

            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult DeleteServer(int id)
    {
        Console.WriteLine("-------->Delete Server");

        if (!_serverRepository.ServerExists(id))
            return NotFound();

        Console.WriteLine("Server Found");

        var serverToDelete = _serverRepository.GetServer(id);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!_serverRepository.DeleteServer(serverToDelete))
        {
            ModelState.AddModelError("", "Something went wrong deleting database");
        }

        Console.WriteLine("Server Deleted");

        // Send message to front-end
        var message = "Server deleted successfully";

        var servers = _serverRepository.GetServers();

        return Ok(new { servers });
    }


    [HttpPut("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult UpdateServer(int id, [FromBody] Server serverUpdate)
    {
        Console.WriteLine("-------->Update Server");
        Console.WriteLine("Server : " + serverUpdate.ServerId);
        Console.WriteLine("Server : " + serverUpdate.Address);

        if (!_serverRepository.ServerExists(id))
            return NotFound();

        var serverToUpdate = _serverRepository.GetServer(id);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Update the server properties
        serverToUpdate.Name = serverUpdate.Name;
        serverToUpdate.Address = serverUpdate.Address;
        serverToUpdate.ModifiedBy = serverUpdate.ModifiedBy;
        serverToUpdate.ModifiedDate = DateTime.Now;
        serverToUpdate.Context = serverUpdate.Context;
        serverToUpdate.Type = serverUpdate.Type;
        // Update other properties as needed

        if (!_serverRepository.UpdateServer(serverToUpdate))
        {
            ModelState.AddModelError("", "Something went wrong updating the server");
        }

        // Send message to front-end
        var message = "Server updated successfully";

        Console.WriteLine("----------->Server Updated");

        var servers = _serverRepository.GetServers();

        return Ok(new { servers });
    }


}