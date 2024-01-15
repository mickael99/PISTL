using Project.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using Project.Controllers.Login;

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
        return Ok(new { users });
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
  /// This POST method is used to create a new DAT user by adding it to the database at the Login table.
  /// </summary>
  /// <returns>An HTTP response.</returns>
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
        foreach (var claim in jsonToken.Claims)
        {
          if (claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name") // TODO AR 
          {
            var context = new DatContext();
            var users = context.Logins;

            // Test if the user (email or phone) already exists
            foreach (var login in users)
            {
              if (login.Email == model.Email)
              {
                return BadRequest(new { message = "User email already exists." });
              }

              if (login.Phone == model.Phone)
              {
                return BadRequest(new { message = "User phone already exists." });
              }
            }

            var bytes = Encoding.UTF8.GetBytes("test");
            var encryptedBytes = SHA512.HashData(bytes);
            var newLogin = new Project.Models.Login
            {
              Email = model.Email,
              Name = model.Name,
              Password = encryptedBytes, // test
              PasswordModifiedDate = DateTime.Now,
              PasswordExpirationDate = DateTime.Now.AddDays(30),
              InvalidAttemptCount = 3,
              ResetPasswordEndDate = DateTime.Now.AddDays(1),
              ResetPasswordKey = null,
              ResetPasswordSentCount = 1, // TODO a voir ce que c'est
              ResetPasswordInvalidAttemptCount = 0,
              LastLoginDate = null,
              TermsAccepted = model.Locked,
              Datenabled = model.DATEnabled,
              Phone = model.Phone,
              BlockedReason = "No reason yet", // TODO mettre par rapport a locked  
              CreatedDate = DateTime.Now,
              CreatedBy = model.ModifiedBy,
              ModifiedDate = DateTime.Now,
              ModifiedBy = model.ModifiedBy
            };

            users.Add(newLogin);
            context.SaveChanges();
            return Ok(new { users, message = "User found." });
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
  /// <summary>
  /// This POST method is used to edit an existing DAT user by update information to the database at the Login table.
  /// </summary>
  /// <returns>An HTTP response.</returns>
  [HttpPost("edit")]
  public IActionResult Edit_New_DAT_User([FromHeader(Name = "Authorization")] string authorizationHeader, [FromBody] UserSelectedEdit model)
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
        foreach (var claim in jsonToken.Claims)
        {
          if (claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name") // TODO AR 
          {
            var context = new DatContext();
            var users = context.Logins;
            bool found = false;
            foreach (var login in users)
            {
              if (login.Email == model.Email)
              {
                login.Phone = model.Phone;
                login.Datenabled = model.DATEnabled;
                login.TermsAccepted = model.TermsAccepted;
                found = true;
              }
            }
            if (found)
            {
              context.SaveChanges();
              return Ok(new { users, message = "User found." });
            }
            else
            {
              return BadRequest(new { message = "User not found." });
            }
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
      return BadRequest(new
      {
        message = ex.Message
      });
    }

    return Ok(new { message = "User not found." });
  }

  /*************************************************************************************/
  /// <summary>
  /// This POST method is used to remove an existing DAT user by deleting all his information from the database.
  /// </summary>
  /// <returns>An HTTP response.</returns>
  [HttpPost("delete")]
  public IActionResult Delete_New_DAT_User([FromHeader(Name = "Authorization")] string authorizationHeader, [FromBody] UserSelectedEdit model)
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
        foreach (var claim in jsonToken.Claims)
        {
          if (claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name") // TODO AR 
          {
            var context = new DatContext();
            var users = context.Logins;
            bool found = false;
            foreach (var login in users)
            {
              if (login.Email == model.Email)
              {
                users.Remove(login);
                found = true;
              }
            }
            if (found)
            {
              context.SaveChanges();
              return Ok(new { users, message = "User deleted." });
            }
            else
            {
              return BadRequest(new { message = "User not found." });
            }
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
      return BadRequest(new
      {
        message = ex.Message
      });
    }

    return Ok(new { message = "User not found." });
  }

  /*************************************************************************************/
  /// <summary>
  /// This POST method is used to remove an existing DAT user by deleting all his information from the database.
  /// </summary>
  /// <returns>An HTTP response.</returns>
  [HttpPost("reset-password")]
  public IActionResult Reset_Password_DAT_User([FromHeader(Name = "Authorization")] string authorizationHeader, [FromBody] UserSelectedEdit model)
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
        foreach (var claim in jsonToken.Claims)
        {
          if (claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name") // TODO AR 
          {
            var context = new DatContext();
            var users = context.Logins;
            bool found = false;
            string password = "";
            foreach (var login in users)
            {
              if (login.Email == model.Email)
              {
                password = Utils.GenerateRandomString(8);
                string passwordSalt = Utils.GetSalt(24);
                byte[] passwordHash = Utils.EncryptPassword(password, passwordSalt);
                login.Password = passwordHash;
                login.PasswordSalt = passwordSalt;
                login.PasswordModifiedDate = DateTime.Now;
                login.PasswordExpirationDate = DateTime.Now.AddDays(30); // TODO a revoir 
                login.InvalidAttemptCount = 0;
                login.ModifiedDate = DateTime.Now; // TODO a revoir
                login.ModifiedBy = model.ModifiedBy;
                found = true;
              }
            }
            if (found)
            {
              context.SaveChanges();
              return Ok(new { users, message = "User password reseted.", password });
            }
            else
            {
              return BadRequest(new { message = "User not found." });
            }
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
      return BadRequest(new
      {
        message = ex.Message
      });
    }

    return Ok(new { message = "User not found." });
  }

  /*************************************************************************************/
  /// <summary>
  /// Represents the data model for creating a form data.
  /// </summary>
  public class FormDataCreateModel
  {
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }
    public required string ModifiedBy { get; set; }
    public bool DATEnabled { get; set; }
    public bool Locked { get; set; }
  }

  /*************************************************************************************/
  /// <summary>
  /// Represents the data model for editing a Sys Admin.
  /// </summary>
  public class UserSelectedEdit // TODO change name
  {
    public required string Email { get; set; }
    public required string Phone { get; set; }
    public bool DATEnabled { get; set; }
    public bool TermsAccepted { get; set; }
    public string? ModifiedBy { get; set; }
  }
}
/***************************************************************************************/
/***************************************************************************************/
/***************************************************************************************/