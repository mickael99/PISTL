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
[Route("api/users")]
[ApiController]
public class UsersPageController : ControllerBase // TODO change name
{
  /*************************************************************************************/
  /// <summary>
  ///  This method is used to GET all users from the DataBase.
  /// </summary>
  /// <param name="authorizationHeader"></param>
  /// <returns></returns>
  [HttpGet]
  public IActionResult Get_all_Users([FromHeader(Name = "Authorization")] string authorizationHeader)
  {
    try
    {
      var token = authorizationHeader?.Replace("Bearer ", "");

      if (string.IsNullOrEmpty(token))
      {
        return BadRequest(new { message = "Token JWT missing in the Header." });
      }

      var handler = new JwtSecurityTokenHandler();
      var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
      if (jsonToken != null)
      {
        var context = new DatContext();
        var users = context.Logins;
        return Ok(users);
      }
      else
      {
        return BadRequest(new { message = "Invalid JWT token." });
      }
    }
    catch (Exception ex)
    {
      return BadRequest(new { message = $"Error while decoding the JWT : {ex.Message}" });
    }

  }
}
/***************************************************************************************/
/***************************************************************************************/
/***************************************************************************************/