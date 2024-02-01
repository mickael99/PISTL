using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Project.Controllers;
using Project.Models;
using Project.Models.DTO;
using NUnit.Framework;
using Project.Repository; // Add missing using statement

namespace backend.Tests;

/****************************************************************************************/
/****************************************************************************************/
/****************************************************************************************/
public class TestServerParameter
{
    /****************************************************************************************/
    /// <summary>
    /// Represents the response containing a list of servers and server parameters.
    /// </summary>
    public class ServerParameterResponse
    {
        public List<ServerDTO> servers { get; set; } // Remove unnecessary "required" keyword
        public List<ServerParameterDTO> server_parameters { get; set; } // Remove unnecessary "required" keyword
    }

    /****************************************************************************************/
    /// <summary>
    /// Tests the GetServerParametersByServer method of the ServerParametersController class.
    /// </summary>
    /// <remarks>
    /// This test verifies that the GetServerParametersByServer method returns the correct server parameters
    /// for a given server ID. It checks that the response is not null, and that the returned values are of
    /// the expected types (List<ServerDTO> and List<ServerParameterDTO>).
    /// </remarks>
    [Test]
    public void TestGetServerParametersByServer()
    {
        // Arrange
        var context = new DatContext();
        var serverRepository = new ServerRepository(context);
        var controller = new ServerParametersController();
        var controller2 = new ServerController(serverRepository, context);
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
        controller2.CreateServer(server);
        Console.WriteLine(server.ServerId);

        // Act
        var response = controller.GetServerParametersByServer(1) as OkObjectResult;
        Assert.IsNotNull(response);

        var json = JsonConvert.SerializeObject(response.Value);
        var values = JsonConvert.DeserializeObject<ServerParameterResponse>(json);
        Assert.IsNotNull(values);

        Assert.Multiple(() =>
        {
            Assert.IsNotNull(values.servers);
            Assert.IsInstanceOf<List<ServerDTO>>(values.servers, "Wrong type");
            Assert.IsNotNull(values.server_parameters);
            Assert.IsInstanceOf<List<ServerParameterDTO>>(values.server_parameters, "Wrong type");
        });
    }

    /****************************************************************************************/
    /// <summary>
    /// Tests the creation of a server parameter.
    /// </summary>
    /// <remarks>
    /// This test method verifies that the CreateServerParameter method of the ServerParametersController
    /// correctly creates a server parameter with the specified values. 
    /// </remarks>
    [Test]
    public void TestCreateServerParameter()
    {
        // Arrange
        var controller = new ServerParametersController();
        var server_parameter = new ServerParameterDTO
        {
            ServerId = 1,
            ParameterKey = "test",
            ParameterValue = "test_value",
        };

        // Act
        var response = controller.CreateServerParameter(server_parameter) as OkObjectResult;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.IsNotNull(response, "Response is null");

            var json = JsonConvert.SerializeObject(response.Value);
            Assert.IsNotNull(json);
            var values = JsonConvert.DeserializeObject<List<ServerParameterDTO>>(json);
            Assert.IsNotNull(values);
            Assert.IsInstanceOf<List<ServerParameterDTO>>(values, "Wrong type");
            Assert.IsNotNull(values.FirstOrDefault(p => p.ParameterKey == "test"), "Server parameter not found");
            Assert.IsNotNull(values.FirstOrDefault(p => p.ParameterValue == "test_value"), "Server parameter not found");
        });

        controller.DeleteServerParameter(server_parameter.ServerId, server_parameter.ParameterKey);
    }

    /****************************************************************************************/
    /// <summary>
    /// Test method for updating a server parameter.
    /// </summary>
    [Test]
    public void TestUpdateServerParameter()
    {
        // Arrange
        var controller = new ServerParametersController();
        var server_parameter = new ServerParameterDTO
        {
            ServerId = 1,
            ParameterKey = "test",
            ParameterValue = "test_value",
        };

        // Act
        controller.CreateServerParameter(server_parameter);
        var response = controller.UpdateServerParameter(server_parameter) as OkObjectResult;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.IsNotNull(response);

            var json = JsonConvert.SerializeObject(response.Value);
            var values = JsonConvert.DeserializeObject<List<ServerParameterDTO>>(json);
            Assert.IsNotNull(values);

            Assert.IsInstanceOf<List<ServerParameterDTO>>(values, "Wrong type");
            Assert.IsNotNull(values.FirstOrDefault(p => p.ParameterKey == "test"), "Server parameter not deleted");
            Assert.IsNotNull(values.FirstOrDefault(p => p.ParameterValue == "test_value"), "Server parameter not deleted");
        });
        controller.DeleteServerParameter(server_parameter.ServerId, server_parameter.ParameterKey);
    }
}
/****************************************************************************************/
/****************************************************************************************/
/****************************************************************************************/
