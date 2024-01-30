using System;
using Project.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Project.Interface;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Hosting.Server;


// namespace Project.Controllers.Database
[Route("api/database")]
[ApiController]
public class DatabaseController : ControllerBase
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

    /// <summary>
    /// Retrieves a list of databases.
    /// </summary>
    /// <returns>An <see cref="IActionResult"/> representing the response of the request.</returns>
    [HttpGet]
    [ProducesResponseType(200, Type = (typeof(IEnumerable<Database>)))]
    [ProducesResponseType(400)]
    public IActionResult GetDatabases()
    {
        // Retrieves a list of databases
        var databases = _databaseRepository.GetDataBases();

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return Ok(databases);
    }

    /// <summary>
    /// Retrieves a specific database by its ID.
    /// </summary>
    /// <param name="id">The ID of the database to retrieve.</param>
    /// <returns>An <see cref="IActionResult"/> representing the response of the request.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(200, Type = (typeof(Database)))]
    [ProducesResponseType(400)]
    public IActionResult GetDatabase(int id)
    {
        // Retrieves a specific database by its ID

        if (!_databaseRepository.DatabaseExists(id))
            return NotFound();

        var database = _databaseRepository.GetDatabase(id);

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return Ok(database);
    }

    /// <summary>
    /// Creates a new database.
    /// </summary>
    /// <param name="databaseCreate">The database object containing the information for the new database.</param>
    /// <returns>An <see cref="IActionResult"/> representing the response of the request.</returns>
    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public IActionResult CreateDatabase([FromBody] Database databaseCreate)
    {
        // Creates a new database
        string passwordHash = _databaseRepository.EncryptPassword(databaseCreate.Password);


        var newId = _databaseRepository.GetUnusedMinDatabaseId();


        if (databaseCreate == null)
            return BadRequest(ModelState);


        var server = _serverRepository.GetServer(databaseCreate.ServerId);

        if (server == null)
        {
            return BadRequest("Server do not exists");
        }

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
                    Context = databaseCreate.Context,
                });
            }
            else
            {
                return BadRequest(new { message = "This name of database already exists" });
            }

            _databaseRepository.Save();

            var databases = _databaseRepository.GetDataBases();

            return Ok(new { databases });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Updates an existing database.
    /// </summary>
    /// <param name="dbUpdated">The updated database object.</param>
    /// <returns>An <see cref="IActionResult"/> representing the response of the request.</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult UpdateDatabase([FromBody] Database dbUpdated)
    {
        // Updates an existing database
        if (dbUpdated == null)
        {
            return BadRequest(ModelState);
        }
        string passwordHash = _databaseRepository.EncryptPassword(dbUpdated.Password);


        if (!_databaseRepository.DatabaseExists(dbUpdated.DatabaseId))
        {
            return NotFound();
        }

        if (dbUpdated == null)
        {
            return BadRequest("---------->Provided database is null");
        }

        if(_databaseRepository.DatabaseExists(dbUpdated.Name)){
            var db = _databaseRepository.GetDatabase(dbUpdated.Name);
            if(db.DatabaseId != dbUpdated.DatabaseId){
                return BadRequest("This name of database already exists");
            }
        }

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
        dbToUpdate.Password = passwordHash;
        dbToUpdate.ServerId = dbUpdated.ServerId;
        dbToUpdate.Server = server;
        dbToUpdate.ModifiedBy = dbUpdated.ModifiedBy;
        dbToUpdate.ModifiedDate = DateTime.Now;
        dbToUpdate.Context = dbUpdated.Context;
        


        if (!ModelState.IsValid)
            return BadRequest(ModelState);


        if (!_databaseRepository.UpdateDatabase(dbToUpdate))
        {
            ModelState.AddModelError("", $"Something went wrong updating the record {dbToUpdate.Name}");
            return StatusCode(500, ModelState);
        }

        var databases = _databaseRepository.GetDataBases();

        return Ok(new { databases });
    }

    /// <summary>
    /// Deletes a database by its ID.
    /// </summary>
    /// <param name="id">The ID of the database to delete.</param>
    /// <returns>An <see cref="IActionResult"/> representing the response of the request.</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult DeleteDatabase(int id)
    {
        // Deletes a database by its ID
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
