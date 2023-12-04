using System;
using Project.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;

[Route("api/database")]
[ApiController]
public class DatabaseController : ControllerBase
{
    
    public class DatabaseModel
    {
        public required string Name { get; set; }
        public int DatabaseId { get; set; }
        public required string UserName { get; set; }
        public required string ServerID { get; set; }
        public int? Context { get; set; }
    }
}
