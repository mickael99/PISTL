using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using Project.Controllers.Database;
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
    var controller = new DatabaseController();
    var authorizationHeader = DatabaseController.create_token("test@test.com");

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


  
}
/****************************************************************************************/
/****************************************************************************************/
/****************************************************************************************/