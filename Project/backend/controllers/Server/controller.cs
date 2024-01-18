using System;
using Project.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;

[Route("api/server")]
[ApiController]
public class ServerController : ControllerBase {
    
    [HttpGet]
    public IActionResult GetDatabases() {
        var context = new MasterContext();

        try {
            var servers = context.Servers.ToList();
            return Ok(servers);
        } catch(Exception e) {
            return BadRequest(e.Message);
        }
    }
}