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
    [ProducesResponseType(200, Type = (typeof(Database)))]
    public IActionResult GetDatabase(){
        var database = _databaseRepository.GetDatabase();

        if(!ModelState.IsValid){
            return BadRequest(ModelState);
        }

        return Ok(database);
    }

    public class DatabaseModel
    {
        public required string Name { get; set; }
        public int DatabaseId { get; set; }
        public required string UserName { get; set; }
        public required string ServerID { get; set; }
        public int? Context { get; set; }
    }
}
