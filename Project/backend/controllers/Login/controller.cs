using System;
using Project.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;

[Route("api/auth")]
[ApiController]
public class UsersController : ControllerBase
{
    [HttpPost]
    public IActionResult GetUsers([FromBody] User user)
    {
        try
        {
            // get all logins
            var context = new MasterContext();
            var logins = context.Logins;
            foreach (var login in logins)
            {
                var password = Encoding.UTF8.GetString(login.Password); 
                if (login.Email == user.Email && password == user.Password)
                {
                    return Ok(new { message = "Successful connection" });
                }
            }
            return Ok(new { message = "Incorrect email or password" });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    public class User
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
