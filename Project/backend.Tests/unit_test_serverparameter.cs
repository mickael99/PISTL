using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NUnit.Framework.Legacy;
using Project.Controllers;
using Project.Models;
using Project.Models.DTO;

namespace backend.Tests;
public class TestServerParameter
{
    public class ServerParameterResponse
    {
        public required List<ServerDTO> servers { get; set; }
        public required List<ServerParameterDTO> server_parameters { get; set; }
    }

    [Test]
    public void TestGetServerParametersByServer()
    {
        // Arrange
        var controller = new ServerParametersController();

        // Act
        var response = controller.GetServerParametersByServer(1) as OkObjectResult;
        ClassicAssert.IsNotNull(response);

        var json = JsonConvert.SerializeObject(response.Value);
        var values = JsonConvert.DeserializeObject<ServerParameterResponse>(json);
        ClassicAssert.IsNotNull(values);

        Assert.Multiple(() =>
        {
            ClassicAssert.IsNotNull(values.servers);
            ClassicAssert.IsInstanceOf<List<ServerDTO>>(values.servers, "Wrong type");
            ClassicAssert.IsNotNull(values.server_parameters);
            ClassicAssert.IsInstanceOf<List<ServerParameterDTO>>(values.server_parameters, "Wrong type");
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
            ClassicAssert.IsNotNull(response);

            var json = JsonConvert.SerializeObject(response.Value);
            var values = JsonConvert.DeserializeObject<List<ServerParameterDTO>>(json);
            ClassicAssert.IsNotNull(values);
            ClassicAssert.IsInstanceOf<List<ServerParameterDTO>>(values, "Wrong type");
            ClassicAssert.IsNotNull(values.FirstOrDefault(p => p.ParameterKey == "test"), "Server parameter not found");
            ClassicAssert.IsNotNull(values.FirstOrDefault(p => p.ParameterValue == "test_value"), "Server parameter not found"); 
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
            ClassicAssert.IsNotNull(response);

            var json = JsonConvert.SerializeObject(response.Value);
            var values = JsonConvert.DeserializeObject<List<ServerParameterDTO>>(json);
            ClassicAssert.IsNotNull(values);

            ClassicAssert.IsInstanceOf<List<ServerParameterDTO>>(values, "Wrong type");
            ClassicAssert.IsNotNull(values.FirstOrDefault(p => p.ParameterKey == "test"), "Server parameter not deleted");
            ClassicAssert.IsNotNull(values.FirstOrDefault(p => p.ParameterValue == "test_value"), "Server parameter not deleted"); 
        });
    }
}