using Project.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using Project.Controllers.Login;

/***************************************************************************************/
/***************************************************************************************/
/***************************************************************************************/
[Route("api/reports")]
[ApiController]
public class ReportsPageController : ControllerBase
{
  /*************************************************************************************/
  /// <summary>
  ///  This method is used to GET all data from the Login_Log table.
  /// </summary>
  /// <param name="authorizationHeader"></param> 
  /// <returns></returns>
  [HttpGet("userActivity")]

  public IActionResult Get_User_Activity_Report([FromHeader(Name = "Authorization")] string authorizationHeader)
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
        var loginLog = context.LoginLogs;
        return Ok(new { loginLog });
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

  /*************************************************************************************/
  /// <summary>
  ///  This method is used to GET all data from the LoginDomainUser_Log table.
  /// </summary>
  /// <param name="authorizationHeader"></param> 
  /// <returns></returns>
  [HttpGet("sysAdmin")]

  public IActionResult Get_SysAdmin_By_Domain_History_Report([FromHeader(Name = "Authorization")] string authorizationHeader)
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
        var loginDomainUserLog = context.LoginDomainUserLogs;
        return Ok(new { loginDomainUserLog });
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

  /*************************************************************************************/
  /// <summary>
  ///  This method is used to GET all data from the Domain_Log table.
  /// </summary>
  /// <param name="authorizationHeader"></param> 
  /// <returns></returns>
  [HttpGet("domainLog")]

  public IActionResult Get_Domain_Administration_History([FromHeader(Name = "Authorization")] string authorizationHeader)
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
        var domainLog = context.DomainLogs;
        return Ok(new { domainLog });
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
