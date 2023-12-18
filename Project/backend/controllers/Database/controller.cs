using System;
using Project.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Project.Interface;
using Project.Data;
using Microsoft.AspNetCore.Http.HttpResults;

[Route("api/database")]
[ApiController]
public class DatabaseController : Controller
{
    
    private readonly IDatabaseRepository _databaseRepository;
    private readonly DatabaseContext context;

    public DatabaseController(IDatabaseRepository databaseRepository, DatabaseContext context){
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
        if (databaseCreate == null)
            return BadRequest(ModelState);
        
        var db = _databaseRepository.GetDataBases()
                    .Where(db => db.DatabaseId == databaseCreate.DatabaseId)
                    .FirstOrDefault();

        if(db != null){
            ModelState.AddModelError("", "DataBase already exists!");
            return StatusCode(422, ModelState);
        }

        if(!ModelState.IsValid){
            return BadRequest(ModelState);
        }

        if(!_databaseRepository.CreateDatabase(databaseCreate)){
            ModelState.AddModelError("", "Something went Wrong while saving");
            return StatusCode(500, ModelState);
        }

        return Ok("Successfully created");
    }

    [HttpPut("{id}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult UpdateDatabase(int dataBaseId, [FromBody]Database dbUpdated){
        if(dbUpdated == null){
            return BadRequest(ModelState);
        }

        if(dataBaseId != dbUpdated.DatabaseId)
            return BadRequest(ModelState);

        if(!_databaseRepository.DatabaseExists(dataBaseId)){
            return NotFound();
        }

        if(!ModelState.IsValid)
            return BadRequest(ModelState);

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
