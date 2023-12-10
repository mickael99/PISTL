using Project.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;

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
                Console.WriteLine("==> Connect");
                if (login.Email == user.Email)
                {

                    var bytes = Encoding.UTF8.GetBytes(user.Password + login.PasswordSalt);
                    var computedHash = SHA512.HashData(bytes);
                    Console.WriteLine("Computed hash: " + BitConverter.ToString(computedHash));
                    Console.WriteLine("Login hash: " + BitConverter.ToString(login.Password));

                    if (computedHash.SequenceEqual(login.Password))
                    {
                        if (login.ResetPasswordKey != null)
                        {
                            Console.WriteLine("==> reset password key exists ");
                            return Ok(new { exist = true });
                        }
                        else
                        {
                            string token = create_token(user.Email);
                            Console.WriteLine("==> reset password NOT key exists ");
                            return Ok(new { token, exist = false });
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return BadRequest(new { message = "User not found." });
    }


    /***************************************************************************************/
    /// <summary>
    /// Creates a JWT token for the specified user.
    /// </summary>
    /// <param name="user">The user for whom the token is being created.</param>
    /// <returns>The generated JWT token.</returns>
    public static string create_token(string email)
    {
        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, email)
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