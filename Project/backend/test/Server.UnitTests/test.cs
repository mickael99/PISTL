using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
// using Project.Controllers.Database;
using System.IdentityModel.Tokens.Jwt;
using System;
using Newtonsoft.Json;
using Project.Models;
using Project.Interface;
using Project.Repository;

namespace backend.Tests;

/****************************************************************************************/
/****************************************************************************************/
/****************************************************************************************/
[TestFixture]
public class ServerTests
{

  /****************************************************************************************/
  /// <summary>
  /// Tests the behavior of the SreverController's CreateServer method when the DTO is correct.
  /// </summary>
  [Test]
  public static void ServerController_CreateServer_ReturnsOkResult()
  {
    // Arrange
    var context = new DatContext();
    var serverRepository = new Project.Repository.ServerRepository(context);
    var controller = new ServerController(serverRepository, context);

    var idMin = serverRepository.GetUnusedMinServerId();

    var server = new Project.Models.Server
    {
      Name = "TestSeverName",
      Address = "TestAdress",
      Context = 1,
      CreatedDate = DateTime.Now,
      CreatedBy = "TestCreatedBy",
      ModifiedDate = DateTime.Now,
      ModifiedBy = "TestModifiedBy",
      Type = "TestType"
    };

    // Act
    var result = controller.CreateServer(server) as OkObjectResult;


    // Assert
    Assert.IsNotNull(result, "Add result is null.");
    Assert.AreEqual(200, result.StatusCode, "Status code is not 200.");
    dynamic data = result.Value;


    var serversProperty = data?.GetType().GetProperty("servers");
    var serversValue = serversProperty.GetValue(data) as System.Collections.Generic.List<Project.Models.Server>;

    if (serversValue != null)
    {
      foreach (var s in serversValue)
      {
        if (s.Name == server.Name)
        {
          // Assert.Mutliple(() => {    Assert1;    Assert2;    ....});
          Assert.Multiple(() =>
          {
            Assert.AreEqual(s.Name, server.Name, "Server name does not match.");
            Assert.AreEqual(s.Context, server.Context, "Server context does not match.");
            Assert.AreEqual(s.Type, server.Type, "Server type does not match.");
            Assert.AreEqual(s.CreatedBy, server.CreatedBy, "Server createdBy does not match.");
            Assert.AreEqual(s.ModifiedBy, server.ModifiedBy, "Server modifiedBy does not match.");
            Assert.AreEqual(idMin, s.ServerId, "Server ID does not match");
          });
        }
      }
    }

    controller.DeleteServer(idMin);
    Assert.Pass("New Server added verified.");
  }


  /****************************************************************************************/
  /// <summary>
  /// Tests the behavior of the SreverController's CreateServer method when the DTO is correct.
  /// </summary>
  [Test]
  public static void ServerController_UpdateServer_ReturnsOkResult()
  {
    // Arrange
    var context = new DatContext();
    var serverRepository = new Project.Repository.ServerRepository(context);
    var controller = new ServerController(serverRepository, context);

    var idMin = serverRepository.GetUnusedMinServerId();

    var server = new Project.Models.Server
    {
      Name = "TestSeverName",
      Address = "TestAdress",
      Context = 1,
      CreatedDate = DateTime.Now,
      CreatedBy = "TestCreatedBy",
      ModifiedDate = DateTime.Now,
      ModifiedBy = "TestModifiedBy",
      Type = "TestType"
    };

    var serverUpdate = new Project.Models.Server
    {
      Name = "TestSeverNameUpdate",
      Address = "TestAdressUpdate",
      Context = 1,
      ModifiedDate = DateTime.Now,
      ModifiedBy = "TestModifiedByUpdate",
      Type = "TestTypeUpdate"
    };

    // Act
    var result = controller.CreateServer(server) as OkObjectResult;

    var result2 = controller.UpdateServer(idMin, serverUpdate) as OkObjectResult;



    // Assert
    Assert.IsNotNull(result2, "Add result is null.");
    Assert.AreEqual(200, result2.StatusCode, "Status code is not 200.");
    dynamic data = result2.Value;


    var serversProperty = data?.GetType().GetProperty("servers");
    var serversValue = serversProperty.GetValue(data) as System.Collections.Generic.List<Project.Models.Server>;

    if (serversValue != null)
    {
      foreach (var s in serversValue)
      {
        if (s.Name == serverUpdate.Name)
        {
          Assert.Multiple(() =>
          {
            Assert.AreEqual(s.Name, serverUpdate.Name, "Server name does not match.");
            Assert.AreEqual(s.Context, serverUpdate.Context, "Server context does not match.");
            Assert.AreEqual(s.Type, serverUpdate.Type, "Server type does not match.");
            Assert.AreEqual(s.ModifiedBy, serverUpdate.ModifiedBy, "Server modifiedBy does not match.");
            Assert.AreEqual(idMin, s.ServerId, "Server ID does not match");
          });
        }
      }
    }

    controller.DeleteServer(idMin);
    Assert.Pass("New Server updated verified.");

  }


  /****************************************************************************************/
  /// <summary>
  /// Tests the behavior of the SreverController's CreateServer method when the DTO is correct.
  /// </summary>
  [Test]
  public static void ServerController_DeleteServer_ReturnsOkResult()
  {
    // Arrange
    var context = new DatContext();
    var serverRepository = new Project.Repository.ServerRepository(context);
    var controller = new ServerController(serverRepository, context);

    var idMin = serverRepository.GetUnusedMinServerId();

    var server = new Project.Models.Server
    {
      Name = "TestSeverName",
      Address = "TestAdress",
      Context = 1,
      CreatedDate = DateTime.Now,
      CreatedBy = "TestCreatedBy",
      ModifiedDate = DateTime.Now,
      ModifiedBy = "TestModifiedBy",
      Type = "TestType"
    };

    // Act
    var result = controller.CreateServer(server) as OkObjectResult;

    var result2 = controller.DeleteServer(idMin) as OkObjectResult;

    // Assert
    Assert.IsNotNull(result2, "Delete result is null.");
    Assert.AreEqual(200, result2.StatusCode, "Status code is not 200.");

  }
}
/****************************************************************************************/
/****************************************************************************************/
/****************************************************************************************/
