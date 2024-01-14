
using System.Collections;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Project.Controllers;
using Project.Models;
using Project.Models.DTO;

namespace backend.Tests;
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

        Assert.IsNotNull(values.domains);
        Assert.IsInstanceOf<List<DomainDTO>>(values.domains, "Wrong type");
        Assert.IsNotNull(values.users);
        Assert.IsInstanceOf<List<LoginDomainUserDTO>>(values.users, "Wrong type");
        Assert.IsNotNull(values.logins);
        Assert.IsInstanceOf<List<LoginDTO>>(values.logins, "Wrong type");
    } 
        
    [Test]
    public void TestUpdateUser()
    {
        var controller = new SysAdminByDomainController();
        var response = controller.UpdateUser(
            new LoginDomainUserDTO
            {
                DomainId = 351,
                Environment = 4,
                LoginId = 26995,
                UserId = "99999999-9999-9999-9999-999999999999",
                ModifiedBy = "admin",
                SysAdmin = true,
                SysAdminStartDate = DateTime.Parse("2020-05-13T00:00:00"),
                SysAdminEndDate = DateTime.Parse("2021-05-13T00:00:00"),
                Comment = "dsvs",
                UserName = null
            }) as OkObjectResult;

        Assert.IsNotNull(response);
        Assert.That(response.StatusCode, Is.EqualTo(200));

        var json = JsonConvert.SerializeObject(response.Value);
        Assert.IsNotNull(json);
        
        Assert.IsInstanceOf<LoginDomainUserDTO>(response.Value, "Wrong type");
        var values = JsonConvert.DeserializeObject<LoginDomainUser>(json);

        // Assert that the object is not null
        Assert.IsNotNull(values);
        
        // Assert that the types are valid
        Assert.That(values.ModifiedBy, Is.TypeOf(typeof(string)));
        Assert.That(values.UserId, Is.TypeOf(typeof(string)));
        Assert.That(values.UserId, Is.EqualTo("99999999-9999-9999-9999-999999999999"));
        Assert.That(values.Environment, Is.TypeOf(typeof(int)));
        Assert.That(values.LoginId, Is.TypeOf(typeof(int)));
        Assert.That(values.DomainId, Is.TypeOf(typeof(int)));
        Assert.That(values.SysAdmin, Is.EqualTo(true));
        Assert.That(values.Comment, Is.TypeOf(typeof(string)));

        // Assert that the dates are valid
        Assert.That(values.SysAdminEndDate, Is.TypeOf(typeof(DateTime)));
        Assert.That(values.SysAdminStartDate, Is.TypeOf(typeof(DateTime)));
        Assert.That(values.SysAdminStartDate, Is.LessThan(values.SysAdminEndDate));
    }
    
    [Test]
    public void TestDeleteUser()
    {
        var controller = new SysAdminByDomainController();
        var response = controller.DeleteUser(26995, "99999999-9999-9999-9999-999999999999" , 351, 4) as OkObjectResult;

        Assert.IsNotNull(response);
        Assert.That(response.StatusCode, Is.EqualTo(200));
        Hashtable responseValues = (Hashtable)response.Value;

        // Assert that the user deleted is the right one
        Assert.That(responseValues["loginID"], Is.EqualTo(26995));
        Assert.That(responseValues["userID"], Is.EqualTo("99999999-9999-9999-9999-999999999999"));
        Assert.That(responseValues["domainID"], Is.EqualTo(351));
        Assert.That(responseValues["env"], Is.EqualTo(4));
    }
}