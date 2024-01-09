using System;
using Project.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Project.Interface;
using Microsoft.AspNetCore.Http.HttpResults;

[Route("api/database")]
[ApiController]
public class DatabaseController : Controller
{
    
    private readonly IDatabaseRepository _databaseRepository;
    private readonly DatContext context;

    public DatabaseController(IDatabaseRepository databaseRepository, DatContext context){
        _databaseRepository = databaseRepository;
        this.context = context;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = (typeof(IEnumerable<Database>)))]
    [ProducesResponseType(400)]
    public IActionResult GetDatabases(){
        var databases = _databaseRepository.GetDataBases();

        if(!ModelState.IsValid){
            return BadRequest(ModelState);
        }

        return Ok(databases);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(200, Type = (typeof(Database)))]
    [ProducesResponseType(400)]
    public IActionResult GetDatabase(int id){
        Console.WriteLine("This is a log message");

        if(!_databaseRepository.DatabaseExists(id))
            return NotFound();
        
        var database = _databaseRepository.GetDatabase(id);

        if(!ModelState.IsValid){
            return BadRequest(ModelState);
        }

        return Ok(database);
    }

    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public IActionResult CreateDatabase([FromBody] Database databaseCreate){
        Console.WriteLine("-------->Create Database");

        if (databaseCreate == null)
            return BadRequest(ModelState);

        Console.WriteLine("start creating");
        try
        {

            context.Databases.Add(new Database
            {
                DatabaseId = databaseCreate.DatabaseId,
                Name = databaseCreate.Name,
                UserName = databaseCreate.UserName,
                Password = databaseCreate.Password,
                ServerId = databaseCreate.ServerId,
                Server = databaseCreate.Server,
                ModifiedBy = databaseCreate.ModifiedBy,
                CreatedBy = databaseCreate.CreatedBy, 
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
            });

            Console.WriteLine("data enter finish");

            context.SaveChanges();
            
            Console.WriteLine("saved");

            return Ok(context.Databases);
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
    public IActionResult UpdateDatabase(int dataBaseId, [FromBody]Database dbUpdated){

        Console.WriteLine("-------->Update Database");

        if(dbUpdated == null){
            return BadRequest(ModelState);
        }

        if(dataBaseId != dbUpdated.DatabaseId)
            return BadRequest(ModelState);

        if(!_databaseRepository.DatabaseExists(dataBaseId)){
            Console.WriteLine("-------->Database not found");
            return NotFound();
        }

        Console.WriteLine("-------->start editing");


        var dbToUpdate = _databaseRepository.GetDatabase(dataBaseId);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Update the server properties
        dbToUpdate.Name = dbUpdated.Name;
        dbToUpdate.UserName = dbUpdated.UserName;
        dbToUpdate.Password = dbUpdated.Password;
        dbToUpdate.ServerId = dbUpdated.ServerId;
        dbToUpdate.Server = dbUpdated.Server;
        dbToUpdate.ModifiedBy = dbUpdated.ModifiedBy;
        dbToUpdate.ModifiedDate = DateTime.Now;
        dbToUpdate.Context = dbUpdated.Context;

        if(!ModelState.IsValid)
            return BadRequest(ModelState);

        Console.WriteLine("-------->end editing");


        if(!_databaseRepository.UpdateDatabase(dbUpdated)){
            ModelState.AddModelError("","Something went wrong updating database");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult DeleteDatabase(int id){
        if(!_databaseRepository.DatabaseExists(id))
            return NotFound();
        
        var dbToDelete = _databaseRepository.GetDatabase(id);

        if(!ModelState.IsValid)
            return BadRequest(ModelState);
        
        if(!_databaseRepository.DeleteDatabase(dbToDelete)){
            ModelState.AddModelError("", "Something went wrong deleting database");
        }

        return NoContent();
    }
}
