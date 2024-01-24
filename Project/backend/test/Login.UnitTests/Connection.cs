using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using Project.Controllers.Login;
using System.IdentityModel.Tokens.Jwt;
using System;
using Newtonsoft.Json;

namespace backend.Tests;

/****************************************************************************************/
/****************************************************************************************/
/****************************************************************************************/

public class ConnectionTests
{
  // ClassName_MethodName_ExpectedResult
  /****************************************************************************************/
  /// <summary>
  /// Tests the behavior of the LoginController's Connect method when the DTO is correct.
  /// </summary>
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
    var result = controller.Connect(user) as OkObjectResult;

    // Assert
    Assert.IsNotNull(result, "Connect result is null.");
    Assert.AreEqual(200, result.StatusCode, "Status code is not 200.");
    dynamic data = result.Value;
    Assert.IsNotNull(data, "Data is null.");

    var stringProperty = data?.GetType().GetProperty("token");
    var stringTokenValue = stringProperty.GetValue(data) as string;

    var handler = new JwtSecurityTokenHandler();

    JwtSecurityToken jsonToken = null;
    try
    {
      jsonToken = handler.ReadToken(stringTokenValue) as JwtSecurityToken;
    }
    catch (ArgumentException)
    {
      Assert.Fail("JWT was not well-formed.");
    }

    if (jsonToken != null)
    {
      foreach (var claim in jsonToken.Claims)
      {
        if (claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")
        {
          string userEmail = claim.Value;
          Assert.AreEqual(user.Email, userEmail, "User email claim does not match.");
        }
      }
    }

    Utils.remove_login(user.Email);
    Assert.Pass("User email claim verified.");
  }

  /****************************************************************************************/
  /// <summary>
  /// Tests the behavior of the LoginController's Connect method when the Password is wrong.
  /// </summary>
  [Test]
  public static void LoginController_Connect_ReturnsBadRequestPassword()
  {
    // Arrange 
    var controller = new UsersController();
    var user = new UsersController.User
    {
      Email = "simadaniel@hotmail.com",
      Password = "wrongpassword"
    };

    Utils.AddLogin(user.Email, "goodpassword");

    // Act
    var result = controller.Connect(user) as BadRequestObjectResult;

    // Assert
    Assert.NotNull(result, "Connect result is null.");
    Assert.AreEqual(400, result.StatusCode, "Status code is not 400.");

    var stringProperty = result.Value.GetType().GetProperty("message");
    var stringMessageValue = stringProperty.GetValue(result.Value) as string;
    Assert.NotNull(result.Value, "result.Value is null.");
    Assert.AreEqual("User not found.", stringMessageValue);

    Utils.remove_login(user.Email);
    Assert.Pass("User wrong password verified.");
  }

  /****************************************************************************************/
  /// <summary>
  /// Tests the behavior of the LoginController's Connect method when the Email is wrong.
  /// </summary>
  [Test]
  public static void LoginController_Connect_ReturnsBadRequestEmail()
  {
    // Arrange 
    var controller = new UsersController();
    var user = new UsersController.User
    {
      Email = "good@email.com",
      Password = "goodpassword"
    };

    Utils.AddLogin("wrong@email.com", user.Password);

    // Act
    var result = controller.Connect(user) as BadRequestObjectResult;

    // Assert
    Assert.NotNull(result, "Connect result is null.");
    Assert.AreEqual(400, result.StatusCode, "Status code is not 400.");

    var stringProperty = result.Value.GetType().GetProperty("message");
    var stringMessageValue = stringProperty.GetValue(result.Value) as string;
    Assert.NotNull(result.Value, "result.Value is null.");
    Assert.AreEqual("User not found.", stringMessageValue);

    Utils.remove_login("wrong@email.com");
    Assert.Pass("User wrong email verified.");
  }
}
/****************************************************************************************/
/****************************************************************************************/
/****************************************************************************************/
