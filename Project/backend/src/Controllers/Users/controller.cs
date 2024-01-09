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

  /*************************************************************************************/
  [HttpPost("create")]
  public IActionResult Add_New_DAT_User([FromHeader(Name = "Authorization")] string authorizationHeader, [FromBody] FormDataCreateModel model)
  {
    try
    {
      var token = authorizationHeader?.Replace("Bearer ", "");

      if (string.IsNullOrEmpty(token))
      {
        return BadRequest("Token JWT missing in the Header.");
      }

      var handler = new JwtSecurityTokenHandler();
      var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
      if (jsonToken != null)
      {
        bool found = false;
        foreach (var claim in jsonToken.Claims)
        {
          if (claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name") // TODO AR 
          {
            var bytes = Encoding.UTF8.GetBytes("test");
            var encryptedBytes = SHA512.HashData(bytes);
            var newLogin = new Project.Models.Login
            {
              Email = model.email,
              Name = model.name,
              Password = encryptedBytes, // test
              PasswordModifiedDate = DateTime.Now,
              PasswordExpirationDate = DateTime.Now.AddDays(30),
              InvalidAttemptCount = 3,
              ResetPasswordEndDate = DateTime.Now.AddDays(1),
              ResetPasswordKey = null,
              ResetPasswordSentCount = 1,
              ResetPasswordInvalidAttemptCount = 1,
              LastLoginDate = DateTime.Now,
              TermsAccepted = model.locked, // TODO AR
              Datenabled = model.DATEnabled,
              Phone = model.phone,
              BlockedReason = "No reason yet",
              CreatedDate = DateTime.Now,
              CreatedBy = model.modifiedBy,
              ModifiedDate = DateTime.Now,
              ModifiedBy = model.modifiedBy
            };

            using (var context = new DatContext())
            {
              context.Logins.Add(newLogin);
              context.SaveChanges();
            }

            return Ok(new { message = "User found." });

          }
        }
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

    return Ok(new { message = "User not found." });
  }

  /*************************************************************************************/
  public class FormDataCreateModel
  {
    public string name { get; set; }
    public string email { get; set; }
    public string phone { get; set; }
    public int invalidAttemptsCount { get; set; }
    public string modifiedBy { get; set; }
    public DateTime createdDate { get; set; }
    public bool DATEnabled { get; set; }
    public bool locked { get; set; }
  }

}
/***************************************************************************************/
/***************************************************************************************/
/***************************************************************************************/