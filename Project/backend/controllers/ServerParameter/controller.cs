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

public class ServerParameterController : Controller
{

    private readonly IServerRepository _serverRepository;
    private readonly DatContext context;
    private readonly ILogger<ServerController> _logger;  // Add this line


    
}