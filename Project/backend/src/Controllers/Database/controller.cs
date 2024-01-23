using System;
using Project.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Project.Interface;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Hosting.Server;

[Route("api/database")]
[ApiController]
public class DatabaseController : Controller
{

    private readonly IDatabaseRepository _databaseRepository;
    private readonly DatContext context;
    private readonly IServerRepository _serverRepository;

    public DatabaseController(IDatabaseRepository databaseRepository, DatContext context, IServerRepository serverRepository)
    {
        _databaseRepository = databaseRepository;
        _serverRepository = serverRepository;
        this.context = context;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = (typeof(IEnumerable<Database>)))]
    [ProducesResponseType(400)]
    public IActionResult GetDatabases()
    {
        Console.WriteLine("-------->Get databases");
        var databases = _databaseRepository.GetDataBases();

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return Ok(databases);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(200, Type = (typeof(Database)))]
    [ProducesResponseType(400)]
    public IActionResult GetDatabase(int id)
    {
        Console.WriteLine("This is a log message");

        if (!_databaseRepository.DatabaseExists(id))
            return NotFound();

        var database = _databaseRepository.GetDatabase(id);

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return Ok(database);
    }

    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public IActionResult CreateDatabase([FromBody] Database databaseCreate)
    {
        Console.WriteLine("-------->Create Database");

        Console.WriteLine("-------->databaseCreateServer: " + databaseCreate.ServerId);

        var newId = _databaseRepository.GetUnusedMinDatabaseId();

        Console.WriteLine("-------->newId: " + newId);

        if (databaseCreate == null)
            return BadRequest(ModelState);

        Console.WriteLine("-------->Get Server");

        var server = _serverRepository.GetServer(databaseCreate.ServerId);

        if (server == null)
        {
            Console.WriteLine("-------->Server do not exists");
            return BadRequest("Server do not exists");
        }

        Console.WriteLine("start creating");
        try
        {
            if (context.Databases.FirstOrDefault(s => s.Name == databaseCreate.Name) == null)
            {
                context.Databases.Add(new Database
                {
                    DatabaseId = newId,
                    Name = databaseCreate.Name,
                    UserName = databaseCreate.UserName,
                    Password = databaseCreate.Password,
                    ServerId = databaseCreate.ServerId,
                    Server = server,
                    ModifiedBy = databaseCreate.ModifiedBy,
                    CreatedBy = databaseCreate.CreatedBy,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                });
            }
            else
            {
                return BadRequest(new { message = "Database already exists" });
            }

            Console.WriteLine("data enter finish");

            context.SaveChanges();


            if (!_serverRepository.AddDatabaseToServer(newId, databaseCreate.ServerId))
            {
                Console.WriteLine("-------->AddDatabaseToServer failed");
                return BadRequest("AddDatabaseToServer failed");
            }

            Console.WriteLine("-------->AddDatabaseToServer success");

            context.SaveChanges();

            Console.WriteLine("----------->saved");


            var databases = _databaseRepository.GetDataBases();

            return Ok(new { databases });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult UpdateDatabase([FromBody] Database dbUpdated)
    {

        Console.WriteLine("-------->Update Database");
        Console.WriteLine("-------->dbUpdated: " + dbUpdated.DatabaseId);
        Console.WriteLine("-------->dbUpdatedServer: " + dbUpdated.ServerId);

        if (dbUpdated == null)
        {
            return BadRequest(ModelState);
        }


        if (!_databaseRepository.DatabaseExists(dbUpdated.DatabaseId))
        {
            Console.WriteLine("-------->Database not found");
            return NotFound();
        }

        Console.WriteLine("-------->start editing");

        if (dbUpdated == null)
        {
            return BadRequest("---------->Provided database is null");
        }

        if (dbUpdated.Server == null)
        {
            return BadRequest("--------->Provided server is null");
        }

        Console.WriteLine("-------->Not Null ");

        var dbToUpdate = _databaseRepository.GetDatabase(dbUpdated.DatabaseId);

        var server = _serverRepository.GetServer(dbUpdated.ServerId);

        if (server == null)
        {
            return BadRequest("Server not found");
        }

        if (!_serverRepository.UpdateServer(server))
        {
            ModelState.AddModelError("", $"Something went wrong updating the server {server.Name}");
            return StatusCode(500, ModelState);
        }


        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Update the server properties
        dbToUpdate.DatabaseId = dbUpdated.DatabaseId;
        dbToUpdate.Name = dbUpdated.Name;
        dbToUpdate.UserName = dbUpdated.UserName;
        dbToUpdate.Password = dbUpdated.Password;
        dbToUpdate.ServerId = dbUpdated.ServerId;
        dbToUpdate.Server = server;
        dbToUpdate.ModifiedBy = dbUpdated.ModifiedBy;
        dbToUpdate.ModifiedDate = DateTime.Now;
        dbToUpdate.Context = dbUpdated.Context;

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        Console.WriteLine("-------->end editing");


        if (!_databaseRepository.UpdateDatabase(dbToUpdate))
        {
            ModelState.AddModelError("", $"Something went wrong updating the record {dbToUpdate.Name}");
            return StatusCode(500, ModelState);
        }

        Console.WriteLine("-------->end updating");

        var databases = _databaseRepository.GetDataBases();

        Console.WriteLine("-------->success");


        return Ok(new { databases });
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult DeleteDatabase(int id)
    {
        if (!_databaseRepository.DatabaseExists(id))
            return NotFound();

        var dbToDelete = _databaseRepository.GetDatabase(id);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!_databaseRepository.DeleteDatabase(dbToDelete))
        {
            ModelState.AddModelError("", "Something went wrong deleting database");
        }

        var databases = _databaseRepository.GetDataBases();


        return Ok(new { databases });
    }
}
