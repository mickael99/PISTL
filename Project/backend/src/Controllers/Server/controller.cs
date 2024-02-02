using System;
using Project.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Project.Interface;
using Microsoft.AspNetCore.Http.HttpResults;
using Project.Repository;
using System.Net;
using Microsoft.Extensions.Logging;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;


[Route("api/server")]
[ApiController]

public class ServerController : Controller
{

    private readonly IServerRepository _serverRepository;
    private readonly DatContext context;


    public ServerController(IServerRepository serverRepository, DatContext context)
    {
        _serverRepository = serverRepository;
        this.context = context;
    }

    /// <summary>
    /// Retrieves a list of servers.
    /// </summary>
    /// <returns>An IActionResult representing the HTTP response.</returns>
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

    /// <summary>
    /// Retrieves a server by its ID.
    /// </summary>
    /// <param name="id">The ID of the server to retrieve.</param>
    /// <returns>The server with the specified ID.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(200, Type = (typeof(Server)))]
    [ProducesResponseType(400)]
    public IActionResult GetServer(int id)
    {

        if (!_serverRepository.ServerExists(id))
            return NotFound();

        var server = _serverRepository.GetServer(id);

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return Ok(server);
    }

    /// <summary>
    /// Represents an action result that performs a HTTP POST operation and returns a response with no content.
    /// </summary>
    /// <returns>An <see cref="IActionResult"/> that performs a HTTP POST operation and returns a response with no content.</returns>
    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public IActionResult CreateServer([FromBody] Server serverCreate)
    {

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        Console.WriteLine("Server Name: " + serverCreate.Name);

        try
        {
            if (context.Servers.FirstOrDefault(s => s.Name == serverCreate.Name || s.Address == serverCreate.Address) == null)
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
            }
            else
            {
                return BadRequest(new { message = "Name or Address of server already exists" });
            }

            context.SaveChanges();


            var servers = _serverRepository.GetServers();

            return Ok(new { servers });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Deletes a server with the specified ID.
    /// </summary>
    /// <param name="id">The ID of the server to delete.</param>
    /// <returns>
    /// Returns an <see cref="IActionResult"/> representing the result of the delete operation.
    /// If the server is successfully deleted, returns an HTTP 200 OK status code with the updated list of servers.
    /// If the server does not exist, returns an HTTP 404 Not Found status code.
    /// If the request is invalid, returns an HTTP 400 Bad Request status code with the validation errors.
    /// </returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult DeleteServer(int id)
    {
        if (!_serverRepository.ServerExists(id))
            return NotFound();


        var serverToDelete = _serverRepository.GetServer(id);


        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!_serverRepository.DeleteServer(serverToDelete))
        {
            ModelState.AddModelError("", "Something went wrong deleting database");
        }

        var servers = _serverRepository.GetServers();

        return Ok(new { servers });
    }


    /// <summary>
    /// Updates a server with the specified ID.
    /// </summary>
    /// <param name="id">The ID of the server to update.</param>
    /// <param name="serverUpdate">The updated server object.</param>
    /// <returns>An IActionResult representing the result of the update operation.</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult UpdateServer(int id, [FromBody] Server serverUpdate)
    {
        if (!_serverRepository.ServerExists(id))
            return NotFound();

        var serverToUpdate = _serverRepository.GetServer(id);

        if (_serverRepository.ServerExists(serverUpdate.Name))
        {
            var server = _serverRepository.GetServer(serverUpdate.Name);
            if (server.ServerId != id)
            {
                return BadRequest(new { message = "This name of server already exists" });
            }
        }

        if(_serverRepository.ServerExistsWithAddress(serverUpdate.Address)){
            var server = _serverRepository.GetServerWithAddress(serverUpdate.Address);
            if(server.ServerId != id){
                return BadRequest(new { message = "This address of server already exists" });
            }
        }

        Console.WriteLine("Server to update: " + serverToUpdate.Name);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        
        // Update the server properties
        serverToUpdate.Name = serverUpdate.Name;
        serverToUpdate.Address = serverUpdate.Address;
        serverToUpdate.ModifiedBy = serverUpdate.ModifiedBy;
        serverToUpdate.ModifiedDate = DateTime.Now;
        serverToUpdate.Context = serverUpdate.Context;
        serverToUpdate.Type = serverUpdate.Type;
        

        if (!_serverRepository.UpdateServer(serverToUpdate))
        {
            ModelState.AddModelError("", "Something went wrong updating the server");
        }

        var servers = _serverRepository.GetServers();

        return Ok(new { servers });
    }


}