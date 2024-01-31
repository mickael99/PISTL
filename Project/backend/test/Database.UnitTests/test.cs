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
public class DatabaseTests
{
  // ClassName_MethodName_ExpectedResult

  /****************************************************************************************/
  /// <summary>
  /// Tests the behavior of the DatabaseController's CreateDatabase method when the DTO is correct.
  /// </summary>
  [Test]
  public static void DatabaseController_CreateDatabase_ReturnsOkResult()
  {
    // Arrange
    var context = new DatContext();
    var databaseRepository = new Project.Repository.DatabaseRepository(context);
    var serverRepository = new Project.Repository.ServerRepository(context);
    var controller = new DatabaseController(databaseRepository, context, serverRepository);

    var idMin = databaseRepository.GetUnusedMinDatabaseId();

    var database = new Project.Models.Database
    {
      Name = "TestDatabase",
      UserName = "TestUserName",
      Password = "TestPassword",
      Context = 1,
      ServerId = 1,
      CreatedDate = DateTime.Now,
      CreatedBy = "TestCreatedBy",
      ModifiedDate = DateTime.Now,
      ModifiedBy = "TestModifiedBy"
    };

    // Act
    var result = controller.CreateDatabase(database) as OkObjectResult;


    // Assert
    Assert.IsNotNull(result, "Add result is null.");
    Assert.AreEqual(200, result.StatusCode, "Status code is not 200.");
    dynamic data = result.Value;


    var databaseProperty = data?.GetType().GetProperty("databases");
    var databaseValue = databaseProperty.GetValue(data) as System.Collections.Generic.List<Project.Models.Database>;

    if (databaseValue != null)
    {
      foreach (var db in databaseValue)
      {
        if (db.Name == database.Name)
        {
          // Assert.Mutliple(() => {    Assert1;    Assert2;    ....});
          Assert.Multiple(() =>
          {
            Assert.AreEqual(db.Name, database.Name, "Databases name does not match.");
            Assert.AreEqual(db.UserName, database.UserName, "Databases username does not match.");
            Assert.AreEqual(db.Context, database.Context, "Databases context does not match.");
            Assert.AreEqual(db.ServerId, database.ServerId, "Databases serverId does not match.");
            Assert.AreEqual(db.CreatedBy, database.CreatedBy, "Databases createdBy does not match.");
            Assert.AreEqual(db.ModifiedBy, database.ModifiedBy, "Databases modifiedBy does not match.");
            Assert.AreEqual(idMin, db.DatabaseId, "Databases ID does not match");
          });
        }
      }
    }

    controller.DeleteDatabase(idMin);
    Assert.Pass("New Database added verified.");
  }

  /****************************************************************************************/
  /// <summary>
  /// Tests the behavior of the DatabaseController's CreateDatabase method when the database name already exists.
  /// </summary>
  [Test]
  public static void DatabaseController_CreateDatabase_WithExistingName_ReturnsBadRequest()
  {
    // Arrange
    var context = new DatContext();
    var databaseRepository = new Project.Repository.DatabaseRepository(context);
    var serverRepository = new Project.Repository.ServerRepository(context);
    var controller = new DatabaseController(databaseRepository, context, serverRepository);

    var idMin = databaseRepository.GetUnusedMinDatabaseId();

    var database = new Project.Models.Database
    {
      Name = "TestDatabase",
      UserName = "TestUserName",
      Password = "TestPassword",
      Context = 1,
      ServerId = 1,
      CreatedDate = DateTime.Now,
      CreatedBy = "TestCreatedBy",
      ModifiedDate = DateTime.Now,
      ModifiedBy = "TestModifiedBy"
    };

    var database2 = new Project.Models.Database
    {
      Name = "TestDatabase",
      UserName = "TestUserName",
      Password = "TestPassword",
      Context = 1,
      ServerId = 1,
      CreatedDate = DateTime.Now,
      CreatedBy = "TestCreatedBy",
      ModifiedDate = DateTime.Now,
      ModifiedBy = "TestModifiedBy"
    };

    // Act
    var result = controller.CreateDatabase(database) as OkObjectResult;
    var result2 = controller.CreateDatabase(database2) as BadRequestObjectResult;

    // Assert
    Assert.IsNotNull(result2, "Add result is null.");
    Assert.AreEqual(400, result2.StatusCode, "Status code is not 400.");
    dynamic data = result2.Value;


    var databaseProperty = data?.GetType().GetProperty("message");
    var databaseValue = databaseProperty.GetValue(data) as string;
    Assert.AreEqual("This name of database already exists", databaseValue);


    controller.DeleteDatabase(idMin);
    Assert.Pass("New Database added verified.");
  }



  /****************************************************************************************/
  /// <summary>
  /// Tests the behavior of the DatabaseController's UpdateDatabase method when the DTO is correct.
  /// </summary>
  [Test]
  public static void DatabaseController_UpdateDatabase_ReturnsOkResult()
  {
    // Arrange
    var context = new DatContext();
    var databaseRepository = new Project.Repository.DatabaseRepository(context);
    var serverRepository = new Project.Repository.ServerRepository(context);
    var controller = new DatabaseController(databaseRepository, context, serverRepository);

    var idMin = databaseRepository.GetUnusedMinDatabaseId();

    var database = new Project.Models.Database
    {
      Name = "TestDatabase",
      UserName = "TestUserName",
      Password = "TestPassword",
      Context = 1,
      ServerId = 1,
      CreatedDate = DateTime.Now,
      CreatedBy = "TestCreatedBy",
      ModifiedDate = DateTime.Now,
      ModifiedBy = "TestModifiedBy"
    };

    var databaseUpdate = new Project.Models.Database
    {
      DatabaseId = idMin,
      Name = "TestDatabaseUpdate",
      UserName = "TestUserName",
      Password = "TestPassword",
      Server = serverRepository.GetServer(1),
      Context = 2,
      ServerId = 1,
      ModifiedDate = DateTime.Now,
      ModifiedBy = "TestModifiedBy"
    };

    // Act
    var result = controller.CreateDatabase(database) as OkObjectResult;

    var result2 = controller.UpdateDatabase(databaseUpdate) as OkObjectResult;

    // Assert
    Assert.IsNotNull(result2, "Update result is null.");
    Assert.AreEqual(200, result2.StatusCode, "Status code is not 200.");
    dynamic data = result2.Value;


    var databaseProperty = data?.GetType().GetProperty("databases");
    var databaseValue = databaseProperty.GetValue(data) as System.Collections.Generic.List<Project.Models.Database>;

    if (databaseValue != null)
    {
      foreach (var db in databaseValue)
      {
        if (db.Name == databaseUpdate.Name)
        {
          // Assert.Mutliple(() => {    Assert1;    Assert2;    ....});
          Assert.Multiple(() =>
          {
            Assert.AreEqual(db.Name, databaseUpdate.Name, "Databases name does not match.");
            Assert.AreEqual(db.UserName, databaseUpdate.UserName, "Databases username does not match.");
            Assert.AreEqual(db.Context, databaseUpdate.Context, "Databases context does not match.");
            Assert.AreEqual(db.ServerId, databaseUpdate.ServerId, "Databases serverId does not match.");
            Assert.AreEqual(db.ModifiedBy, databaseUpdate.ModifiedBy, "Databases modifiedBy does not match.");
            Assert.AreEqual(idMin, db.DatabaseId, "Databases ID does not match");
          });
        }
      }
    }

    controller.DeleteDatabase(idMin);
    Assert.Pass("New Database added verified.");


  }

  public static void DatabaseController_UpdateDatabase_WithExistingName_ReturnsBadRequest()
  {
    // Arrange
    var context = new DatContext();
    var databaseRepository = new Project.Repository.DatabaseRepository(context);
    var serverRepository = new Project.Repository.ServerRepository(context);
    var controller = new DatabaseController(databaseRepository, context, serverRepository);

    var idMin = databaseRepository.GetUnusedMinDatabaseId();

    var database = new Project.Models.Database
    {
      Name = "TestDatabase",
      UserName = "TestUserName",
      Password = "TestPassword",
      Context = 1,
      ServerId = 1,
      CreatedDate = DateTime.Now,
      CreatedBy = "TestCreatedBy",
      ModifiedDate = DateTime.Now,
      ModifiedBy = "TestModifiedBy"
    };

    var database2 = new Project.Models.Database
    {
      Name = "TestDatabase1",
      UserName = "TestUserName",
      Password = "TestPassword",
      Context = 1,
      ServerId = 1,
      CreatedDate = DateTime.Now,
      CreatedBy = "TestCreatedBy",
      ModifiedDate = DateTime.Now,
      ModifiedBy = "TestModifiedBy"
    };

    var databaseUpdate = new Project.Models.Database
    {
      DatabaseId = idMin,
      Name = "TestDatabase1",
      UserName = "TestUserName",
      Password = "TestPassword",
      Server = serverRepository.GetServer(1),
      Context = 2,
      ServerId = 1,
      ModifiedDate = DateTime.Now,
      ModifiedBy = "TestModifiedBy"
    };

    // Act
    var result = controller.CreateDatabase(database) as OkObjectResult;

    var result3 = controller.CreateDatabase(database2) as OkObjectResult;

    var result2 = controller.UpdateDatabase(databaseUpdate) as BadRequestObjectResult;

    // Assert
    Assert.IsNotNull(result2, "Add result is null.");
    Assert.AreEqual(400, result2.StatusCode, "Status code is not 400.");
    dynamic data = result2.Value;


    var databaseProperty = data?.GetType().GetProperty("message");
    var databaseValue = databaseProperty.GetValue(data) as string;
    Assert.AreEqual("This name of database already exists", databaseValue);


    controller.DeleteDatabase(idMin);
    Assert.Pass("New Database added verified.");

  }


  /****************************************************************************************/
  /// <summary>
  /// Tests the behavior of the DatabaseController's DeleteDatabase method when the DTO is correct.
  /// </summary>
  [Test]
  public static void DatabaseController_DeleteDatabase_ReturnsOkResult()
  {
    // Arrange
    var context = new DatContext();
    var databaseRepository = new Project.Repository.DatabaseRepository(context);
    var serverRepository = new Project.Repository.ServerRepository(context);
    var controller = new DatabaseController(databaseRepository, context, serverRepository);

    var idMin = databaseRepository.GetUnusedMinDatabaseId();

    var database = new Project.Models.Database
    {
      Name = "TestDatabase",
      UserName = "TestUserName",
      Password = "TestPassword",
      Context = 1,
      ServerId = 1,
      CreatedDate = DateTime.Now,
      CreatedBy = "TestCreatedBy",
      ModifiedDate = DateTime.Now,
      ModifiedBy = "TestModifiedBy"
    };

    // Act
    var result = controller.CreateDatabase(database) as OkObjectResult;

    var result2 = controller.DeleteDatabase(idMin) as OkObjectResult;

    // Assert
    Assert.IsNotNull(result2, "Delete result is null.");
    Assert.AreEqual(200, result2.StatusCode, "Status code is not 200.");
  }
}
/****************************************************************************************/
/****************************************************************************************/
/****************************************************************************************/
