namespace backend.Tests;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NUnit.Framework;
using Project.Controllers;
using Project.Models;
using Project.Models.DTO;

public class Tests
{
    public class SysAdminResponse
    {
        public List<DomainDTO> domains { get; set; }
        public List<LoginDomainUserDTO> users { get; set; }
        public List<LoginDTO> logins { get; set; }
    }
    [Test]
    public void TestGetSysAdmin()
    {
        var controller = new SysAdminByDomainController();
        var response = controller.GetSysAdmin() as OkObjectResult;
        Assert.IsNotNull(response);

        var json = JsonConvert.SerializeObject(response.Value);
        var values = JsonConvert.DeserializeObject<SysAdminResponse>(json);
        Assert.IsNotNull(values);

        Assert.IsInstanceOf<List<DomainDTO>>(values.domains, "Wrong type");
        Assert.IsInstanceOf<List<LoginDomainUserDTO>>(values.users, "Wrong type");
        Assert.IsInstanceOf<List<LoginDTO>>(values.logins, "Wrong type");
    }

    [Test]
    public void TestPostSysAdmin()
    {
        var controller = new SysAdminByDomainController();
        var userDTO = new LoginDomainUserDTO
        {
            LoginId = 1,
            DomainId = 1,
            UserId = "99999999-9999-9999-9999-999999999999",
            Environment = 1,
            SysAdmin = true,
            SysAdminStartDate = DateTime.Now,
            SysAdminEndDate = DateTime.Now,
            Comment = "Test",
            UserName = "Test",
            ModifiedBy = "Test"
        };
        var response = controller.PostSysAdmin(userDTO) as OkObjectResult;
        Assert.IsNotNull(response, "Null response");

        var json = JsonConvert.SerializeObject(response.Value);
        var values = JsonConvert.DeserializeObject<SysAdminResponse>(json);
        Assert.IsNotNull(values, "Null userDTO");

        Console.WriteLine(values);
        Assert.IsInstanceOf<LoginDomainUserDTO>(values, "Wrong type");
    }
}