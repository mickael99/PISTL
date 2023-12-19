
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using Project.Controllers.Login;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using System;


/****************************************************************************************/
/****************************************************************************************/
/****************************************************************************************/
[TestFixture]
public class ConnectionTests
{
  // ClassName_MethodName_ExpectedResult
  /****************************************************************************************/

  [Test]
  public static void LoginController_Connect_ReturnsToken()
  {
    // Arrange 
    var controller = new UsersController();
    var user = new UsersController.User
    {
      Email = "simadaniel@hotmail.com",
      Password = "STL"
    };

    Utils.AddLogin(user.Email, user.Password);

    // Act
    // var result = controller.Connect(user);
    var result = controller.Connect(user) as OkObjectResult;

    // Assert
    Assert.IsNotNull(result);
    dynamic data = result.Value;
    Assert.IsNotNull(data);

    string tokenString = result.Value.ToString();
    Assert.IsFalse(string.IsNullOrWhiteSpace(tokenString), "JWT should not be empty or null.");

    // var handler = new JwtSecurityTokenHandler();

    // JwtSecurityToken jsonToken = null;
    // try
    // {
    //   jsonToken = handler.ReadToken(tokenString) as JwtSecurityToken;
    // }
    // catch (ArgumentException)
    // {
    //   Assert.Fail("JWT was not well-formed.");
    // }

    // if (jsonToken != null)
    // {
    //   foreach (var claim in jsonToken.Claims)
    //   {
    //     if (claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name") // TODO AR 
    //     {
    //       string userEmail = claim.Value;
    //       Assert.AreEqual(user.Email, userEmail, "User email claim does not match.");
    //       Assert.Pass("User email claim verified.");
    //     }
    //   }
    // }

    Utils.remove_login(user.Email);
  }

  [Test]
  public void OnPost_ValidTokenAndModel_ReturnsOk()
  {
    // Arrange
    // var registerController = new RegisterController
    // {
    //   QrCodeUrl = "your_qr_code_url",
    //   ManualEntryCode = "123456"
    // };

    // var authorizationHeader = "Bearer validToken";

    // // Act
    // var result = controller.OnPost(authorizationHeader, registerController);

    // // Assert
    // Assert.IsType<OkObjectResult>(result);
    // var okResult = (OkObjectResult)result;
    // Assert.Equal("2FA is now enabled for this user.", okResult.Value.message);
  }
}
/****************************************************************************************/
/****************************************************************************************/
/****************************************************************************************/
