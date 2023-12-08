using System;
using Project.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

/***************************************************************************************/
/***************************************************************************************/
/***************************************************************************************/
[Route("api/auth")]
[ApiController]
public class UsersController : ControllerBase
{
    /***************************************************************************************/
    /// <summary>
    /// HTTP POST operation that checks the user connections and returns an HTTP status code and the jwt token.
    /// </summary>
    /// <param name="user">The user object containing the email and password.</param>
    /// <returns>An IActionResult representing the result of the Connect operation.</returns>
    [HttpPost]
    public IActionResult Connect([FromBody] User user)
    {
        try
        {
            var context = new DatContext();
            var logins = context.Logins;
            foreach (var login in logins)
            {
                if (login.Email == user.Email)
                {

                    using (var hmac = new System.Security.Cryptography.HMACSHA512(login.PasswordSalt))
                    {
                        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(user.Password));
                        Console.WriteLine("Computed hash: " + BitConverter.ToString(computedHash));
                        Console.WriteLine("Login hash: " + BitConverter.ToString(login.Password));

                        if (computedHash.SequenceEqual(login.Password))
                        {
                            string token = _create_token(user);
                            return Ok(new { token });
                        }
                    }
                }
            }
            return BadRequest("Incorrect email or password");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /***************************************************************************************/
    /// <summary>
    /// Creates a JWT token for the specified user.
    /// </summary>
    /// <param name="user">The user for whom the token is being created.</param>
    /// <returns>The generated JWT token.</returns>
    private string _create_token(User user)
    {
        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Email)
        };

        // key used to signe the token
        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("super secret key"));
        // credits signature 
        var creditentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        // create the token
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creditentials
        );
        // create the jwt token
        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return jwt;
    }

    /***************************************************************************************/
    /// <summary>
    /// Represents a user data structure with email and password.
    /// </summary>
    public class User
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
/***************************************************************************************/
/***************************************************************************************/
/***************************************************************************************/