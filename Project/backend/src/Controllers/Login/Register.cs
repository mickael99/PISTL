using Google.Authenticator;
using Microsoft.AspNetCore.Mvc;
using Project.Controllers.Login;
using Project.Models;
using System.IdentityModel.Tokens.Jwt;
using System.IO;


/***************************************************************************************/
/***************************************************************************************/
/***************************************************************************************/
[Route("api/account")]
[ApiController]
public class RegisterController : ControllerBase
{
  public required string QrCodeUrl { get; set; }
  public required string ManualEntryCode { get; set; }

  /***************************************************************************************/
  /// <summary>
  /// Represents an action result that performs a HTTP GET request to retrieve a QR code.
  /// </summary>
  /// <param name="authorizationHeader">The authorization header containing the JWT token.</param>
  /// <returns>An IActionResult object representing the HTTP response.</returns>
  [HttpGet]
  public IActionResult Get_QR_Code([FromHeader(Name = "Authorization")] string authorizationHeader)
  {
    string email = "";
    try
    {
      var token = authorizationHeader?.Replace("Bearer ", "");

      if (string.IsNullOrEmpty(token))
      {
        return BadRequest(new { message = "Token JWT missing in the Header." });
      }

      // string secretKey = "super secret key"; // TODO AV

      var handler = new JwtSecurityTokenHandler();
      var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
      if (jsonToken != null)
      {
        foreach (var claim in jsonToken.Claims)
        {
          if (claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name") // TODO AR 
          {
            string userEmail = claim.Value;

            var context = new DatContext();
            var logins = context.Logins;
            foreach (var login in logins)
            {
              if (login.Email == userEmail)
              {
                Console.WriteLine($"Email : {userEmail}");
                email = userEmail;
                if (login.ResetPasswordKey != null)
                {
                  return Ok(new { message = "2FA is enabled for this user." });
                }
              }
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
      return BadRequest(new { message = $"Error while decoding the JWT : {ex.Message}" });
    }

    string key = Utils.GenerateRandomString(20);

    TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
    var setupInfo = tfa.GenerateSetupCode("DAT_2FA", email, key, true); // AR cmmt

    var data = new
    {
      QrCodeUrl = setupInfo.QrCodeSetupImageUrl,
      ManualEntryCode = setupInfo.ManualEntryKey
    };
    return Ok(data);
  }

  // /***************************************************************************************/
  public IActionResult OnPost([FromHeader(Name = "Authorization")] string authorizationHeader, [FromBody] ManualEntryKeyModel model)
  {
    try
    {
      var token = authorizationHeader?.Replace("Bearer ", "");

      if (string.IsNullOrEmpty(token))
      {
        return BadRequest("Token JWT missing in the Header.");
      }

      // string secretKey = "super secret key";  // TODO AV

      var handler = new JwtSecurityTokenHandler();
      var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
      if (jsonToken != null)
      {
        bool found = false;
        foreach (var claim in jsonToken.Claims)
        {
          if (claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name") // TODO AR 
          {
            string userEmail = claim.Value;
            Console.WriteLine($"====> Email : {userEmail}");
            var context = new DatContext();
            var logins = context.Logins;
            foreach (var login in logins)
            {
              if (login.Email == userEmail)
              {
                login.ResetPasswordKey = model.ManualEntryKey; // TODO Need to be changed !! 
                found = true;
              }
            }
            if (found)
            {
              context.SaveChanges();
              return Ok(new { message = "2FA is now enabled for this user." });
            }
            else
            {
              return Ok(new { message = "User not found." });
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
      return BadRequest(new { message = $"Error while decoding the JWT : {ex.Message}" });
    }

    return Ok(new { message = "User not found." });
  }

  /***************************************************************************************/
  [HttpPost("2fa")]
  public IActionResult SetupTwoFactorAuth([FromBody] Code2FA model)
  {
    bool result = false;
    try
    {
      var context = new DatContext();
      var logins = context.Logins;
      foreach (var login in logins)
      {
        if (login.Email == model.email)
        {
          TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
          result = tfa.ValidateTwoFactorPIN(login.ResetPasswordKey, model.code, true); // Preciser qu'il faut ajouter une nouvelle colonne dans la DB 
          string[] pinsWithOneHourTolerance = tfa.GetCurrentPINs(login.ResetPasswordKey);
        }
      }
    }
    catch (Exception ex)
    {
      return BadRequest(new { message = $"Error while decoding the JWT : {ex.Message}" });
    }

    if (result)
    {
      string token = UsersController.create_token(model.email);
      return Ok(new { token, message = "success" });

    }
    else
    {
      return Ok(new { message = "failed" });
    }
  }

  /***************************************************************************************/
  public class ManualEntryKeyModel
  {
    public required string ManualEntryKey { get; set; }
  }

  /***************************************************************************************/
  public class Code2FA
  {
    public required string code { get; set; }
    public required string email { get; set; }
  }
}
/***************************************************************************************/
/***************************************************************************************/
/***************************************************************************************/