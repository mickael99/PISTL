using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Project.Controllers;
using Project.Models;
using Project.Models.DTO;
using NUnit.Framework; // Add missing using statement

namespace backend.Tests;

public class TestServerParameter
{
    public class ServerParameterResponse
    {
        public List<ServerDTO> servers { get; set; } // Remove unnecessary "required" keyword
        public List<ServerParameterDTO> server_parameters { get; set; } // Remove unnecessary "required" keyword
    }

    [Test]
    public void TestGetServerParametersByServer()
    {
        // Arrange
        var controller = new ServerParametersController();

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

    [Test]
    public void TestCreateServerParameter()
    {
        // Arrange
        var controller = new ServerParametersController();

        // Act
        var response = controller.CreateServerParameter(
            new ServerParameterDTO
            {
                ServerId = 1,
                ParameterKey = "test",
                ParameterValue = "test_value",
            }) as OkObjectResult;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.IsNotNull(response);

            var json = JsonConvert.SerializeObject(response.Value);
            var values = JsonConvert.DeserializeObject<List<ServerParameterDTO>>(json);
            Assert.IsNotNull(values);
            Assert.IsInstanceOf<List<ServerParameterDTO>>(values, "Wrong type");
            Assert.IsNotNull(values.FirstOrDefault(p => p.ParameterKey == "test"), "Server parameter not found");
            Assert.IsNotNull(values.FirstOrDefault(p => p.ParameterValue == "test_value"), "Server parameter not found"); 
        });
    }

    [Test]
    public void TestUpdateServerParameter()
    {
        // Arrange
        var controller = new ServerParametersController();

        // Act
        var response = controller.UpdateServerParameter(
            new ServerParameterDTO
            {
                ServerId = 1,
                ParameterKey = "test",
                ParameterValue = "test_value",
            }) as OkObjectResult;

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
    }
}