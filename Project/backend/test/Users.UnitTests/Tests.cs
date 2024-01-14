
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using Project.Controllers.Login;
using System.IdentityModel.Tokens.Jwt;
using System;
using Newtonsoft.Json;
using Project.Models;


/****************************************************************************************/
/****************************************************************************************/
/****************************************************************************************/
[TestFixture]
public class UsersTests
{
  // ClassName_MethodName_ExpectedResult

  /****************************************************************************************/
  /// <summary>
  /// Tests the behavior of the UsersController's Add_New_DAT_User method when the DTO is correct.
  /// </summary>
  [Test]
  public static void UsersController_Add_New_DAT_User_ReturnsOkResult()
  {
    // Arrange 
    var controller = new UsersPageController();
    var authorizationHeader = UsersController.create_token("test@test.com");

    var user = new UsersPageController.FormDataCreateModel
    {
      Name = "Daniel",
      Email = "daniel@upclear.com",
      Phone = "0123456789",
      ModifiedBy = "daniel@upclear.com",
      DATEnabled = true,
      Locked = false,
    };


    // Act
    var result = controller.Add_New_DAT_User(authorizationHeader, user) as OkObjectResult;

    // Assert
    Assert.IsNotNull(result, "Add result is null.");
    Assert.AreEqual(200, result.StatusCode, "Status code is not 200.");
    dynamic data = result.Value;
    Assert.IsNotNull(data, "Data is null.");

    var loginProperty = data?.GetType().GetProperty("users");
    var loginValue = loginProperty.GetValue(data) as Microsoft.EntityFrameworkCore.Internal.InternalDbSet<Project.Models.Login>;

    if (loginValue != null)
    {
      foreach (var login in loginValue)
      {
        if (login.Email == user.Email)
        {
          Assert.AreEqual(user.Name, login.Name, "User name does not match.");
          Assert.AreEqual(user.Email, login.Email, "User email does not match.");
          Assert.AreEqual(user.Phone, login.Phone, "User phone does not match.");
          Assert.AreEqual(user.ModifiedBy, login.ModifiedBy, "User modifiedBy does not match.");
          Assert.AreEqual(user.DATEnabled, login.Datenabled, "User Datenabled does not match.");
          Assert.AreEqual(user.Locked, login.TermsAccepted, "User locked (TermsAccepted) does not match.");
        }
      }
    }

    Utils.remove_login(user.Email);
    Assert.Pass("New User added verified.");
  }

  /****************************************************************************************/
  /// <summary>
  /// Tests the behavior of the UsersController's Add_New_DAT_User method when JWT is missing.
  /// </summary>
  [Test]
  public void UsersController_Add_New_DAT_User_WithMissingToken_ReturnsBadRequest()
  {
    // Arrange
    var controller = new UsersPageController();
    var authorizationHeader = "";
    var model = new UsersPageController.FormDataCreateModel
    {
      Email = "test@example.com",
      Name = "Test User",
      Locked = true,
      DATEnabled = true,
      Phone = "1234567890",
      ModifiedBy = "admin"
    };

    // Act
    var result = controller.Add_New_DAT_User(authorizationHeader, model) as BadRequestObjectResult;

    // Assert
    Assert.IsNotNull(result);
    Assert.AreEqual(400, result.StatusCode);
    Assert.AreEqual("Token JWT missing in the Header.", result.Value);

    Assert.Pass("New User not added with missing JWT verified.");
  }

  /****************************************************************************************/
  /// <summary>
  /// Tests the behavior of the UsersController's Add_New_DAT_User method when JWT is invalid.
  /// </summary>
  [Test]
  public void UsersController_Add_New_DAT_User_WithInvalidToken_ReturnsBadRequest()
  {
    // Arrange 
    var controller = new UsersPageController();
    var authorizationHeader = "Bearer invalidToken";
    var model = new UsersPageController.FormDataCreateModel
    {
      Email = "test@example.com",
      Name = "Test User",
      Locked = false,
      DATEnabled = true,
      Phone = "1234567890",
      ModifiedBy = "admin"
    };

    // Act
    var result = controller.Add_New_DAT_User(authorizationHeader, model) as BadRequestObjectResult;

    // Assert
    Assert.IsNotNull(result);
    Assert.AreEqual(400, result.StatusCode);

    Assert.Pass("New User not added with invalid JWT verified.");
  }

  /****************************************************************************************/
  /// <summary>
  /// Tests the behavior of the UsersController's Add_New_DAT_User method when the Email  
  /// already exists.
  /// </summary>
  [Test]
  public void UsersController_Add_New_DAT_User_WithExistingEmail_ReturnsBadRequest()
  {
    // Arrange
    var controller = new UsersPageController();
    var authorizationHeader = UsersController.create_token("test@test.com");
    var model = new UsersPageController.FormDataCreateModel
    {
      Email = "local@upclear.com", // this email is already in the database
      Name = "Test User",
      Locked = true,
      DATEnabled = true,
      Phone = "1234567890",
      ModifiedBy = "admin"
    };

    // Act
    var result = controller.Add_New_DAT_User(authorizationHeader, model) as BadRequestObjectResult;

    // Assert
    Assert.IsNotNull(result);
    Assert.AreEqual(400, result.StatusCode);
    dynamic data = result.Value;
    Assert.IsNotNull(data, "Data is null.");
    var messageProperty = data?.GetType().GetProperty("message");
    var messageValue = messageProperty.GetValue(data) as string;
    Assert.AreEqual("User email already exists.", messageValue, "Error messages does not match.");

    Assert.Pass("New User not added with existing Email verified.");
  }

  /****************************************************************************************/
  /// <summary>
  /// Tests the behavior of the UsersController's Add_New_DAT_User method when the Phone 
  /// already exists.
  /// </summary>
  [Test]
  public void UsersController_Add_New_DAT_User_WithExistingPhone_ReturnsBadRequest()
  {
    // Arrange
    var controller = new UsersPageController();
    var authorizationHeader = UsersController.create_token("test@test.com");
    var model = new UsersPageController.FormDataCreateModel
    {
      Email = "test@example.com",
      Name = "Test User",
      Locked = true,
      DATEnabled = true,
      Phone = "123456789", // this phone is already in the database for the user 'simadaniel@hotmail.com'
      ModifiedBy = "admin"
    };

    // Act
    var result = controller.Add_New_DAT_User(authorizationHeader, model) as BadRequestObjectResult;

    // Assert
    Assert.IsNotNull(result);
    Assert.AreEqual(400, result.StatusCode);
    dynamic data = result.Value;
    Assert.IsNotNull(data, "Data is null.");
    var messageProperty = data?.GetType().GetProperty("message");
    var messageValue = messageProperty.GetValue(data) as string;
    Assert.AreEqual("User phone already exists.", messageValue, "Error messages does not match.");

    Assert.Pass("New User not added with existing Phone verified.");
  }

  /****************************************************************************************/
  /// <summary>
  /// Tests the behavior of the UsersController's Edit_New_DAT_User method when the DTO is correct.
  /// </summary>
  [Test]
  public static void UsersController_Edit_New_DAT_User_ReturnsOkResult()
  {
    // Arrange 
    var controller = new UsersPageController();
    var authorizationHeader = UsersController.create_token("test@test.com");

    var user = new UsersPageController.UserSelectedEdit
    {
      Email = "local@upclear.com",
      Phone = "987654321",    // currently NULL
      DATEnabled = true,      // currently false
      TermsAccepted = false,  // currently true
    };


    // Act
    var result = controller.Edit_New_DAT_User(authorizationHeader, user) as OkObjectResult;

    // Assert
    Assert.IsNotNull(result, "Edit result is null.");
    Assert.AreEqual(200, result.StatusCode, "Status code is not 200.");
    dynamic data = result.Value;
    Assert.IsNotNull(data, "Data is null.");

    var loginProperty = data?.GetType().GetProperty("users");
    var loginValue = loginProperty.GetValue(data) as Microsoft.EntityFrameworkCore.Internal.InternalDbSet<Project.Models.Login>;

    if (loginValue != null)
    {
      foreach (var login in loginValue)
      {
        if (login.Email == user.Email)
        {
          Assert.AreEqual(user.Email, login.Email, "User email does not match.");
          Assert.AreEqual(user.Phone, login.Phone, "User phone does not match.");
          Assert.AreEqual(user.DATEnabled, login.Datenabled, "User Datenabled does not match.");
          Assert.AreEqual(user.TermsAccepted, login.TermsAccepted, "User locked (TermsAccepted) does not match.");
        }
      }
    }

    // Reset the user to its original state
    user = new UsersPageController.UserSelectedEdit
    {
      Email = "local@upclear.com",
      Phone = null,
      DATEnabled = false,
      TermsAccepted = true,
    };

    controller.Edit_New_DAT_User(authorizationHeader, user);
    Assert.Pass("Edit User verified.");
  }
  /****************************************************************************************/
  /// <summary>
  /// Tests the behavior of the UsersController's Edit_New_DAT_User method when JWT is missing.
  /// </summary>
  [Test]
  public void UsersController_Edit_New_DAT_User_WithMissingToken_ReturnsBadRequest()
  {
    // Arrange 
    var controller = new UsersPageController();
    var authorizationHeader = "";

    var user = new UsersPageController.UserSelectedEdit
    {
      Email = "local@upclear.com",
      Phone = "987654321",    // currently NULL
      DATEnabled = true,      // currently false
      TermsAccepted = false,  // currently true
    };

    // Act
    var result = controller.Edit_New_DAT_User(authorizationHeader, user) as BadRequestObjectResult;

    // Assert
    Assert.IsNotNull(result);
    Assert.AreEqual(400, result.StatusCode);
    Assert.AreEqual("Token JWT missing in the Header.", result.Value);

    Assert.Pass("User not edited with missing JWT verified.");
  }

  /****************************************************************************************/
  /// <summary>
  /// Tests the behavior of the UsersController's Edit_New_DAT_User method when JWT is invalid.
  /// </summary>
  [Test]
  public void UsersController_Edit_New_DAT_User_WithInvalidToken_ReturnsBadRequest()
  {
    // Arrange 
    var controller = new UsersPageController();
    var authorizationHeader = "Bearer invalidToken";
    var user = new UsersPageController.UserSelectedEdit
    {
      Email = "local@upclear.com",
      Phone = "987654321",    // currently NULL
      DATEnabled = true,      // currently false
      TermsAccepted = false,  // currently true
    };

    // Act
    var result = controller.Edit_New_DAT_User(authorizationHeader, user) as BadRequestObjectResult;

    // Assert
    Assert.IsNotNull(result);
    Assert.AreEqual(400, result.StatusCode);

    Assert.Pass("User not edited with invalid JWT verified.");
  }

  /****************************************************************************************/
  /// <summary>
  /// Tests the behavior of the UsersController's Edit_New_DAT_User method when the user does not exist. 
  /// </summary>
  [Test]
  public void UsersController_Edit_New_DAT_User_WithNotExistingEmail_ReturnsBadRequest()
  {
    // Arrange
    var controller = new UsersPageController();
    var authorizationHeader = UsersController.create_token("test@test.com");
    var user = new UsersPageController.UserSelectedEdit
    {
      Email = "inexistentUser@upclear.com",
      Phone = "987654321",
      DATEnabled = true,
      TermsAccepted = false,
    };

    // Act
    var result = controller.Edit_New_DAT_User(authorizationHeader, user) as BadRequestObjectResult;

    // Assert
    Assert.IsNotNull(result);
    Assert.AreEqual(400, result.StatusCode);
    dynamic data = result.Value;
    Assert.IsNotNull(data, "Data is null.");
    var messageProperty = data?.GetType().GetProperty("message");
    var messageValue = messageProperty.GetValue(data) as string;
    Assert.AreEqual("User not found.", messageValue, "Error messages does not match.");

    Assert.Pass("User not edited with not existing Email in the database.");
  }
}
/****************************************************************************************/
/****************************************************************************************/
/****************************************************************************************/