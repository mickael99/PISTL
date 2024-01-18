using System;
using Project.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;

[Route("api/database")]
[ApiController]
public class DatabaseController : ControllerBase {
    
    [HttpGet]
    public IActionResult GetDatabases() {
        var context = new MasterContext();

        try {
            var databases = context.Databases.ToList();
            return Ok(databases);
        } catch(Exception e) {
            return BadRequest(e.Message);
        }
    }
}