
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Project.Controllers;
using Project.Models;
using Project.Models.DTO;
using NUnit.Framework.Legacy;

namespace backend.Tests;
public class TestSysAdmin
{
    public class SysAdminResponse
    {
        public required List<DomainDTO> domains { get; set; }
        public required List<LoginDomainUserDTO> users { get; set; }
        public required List<LoginDTO> logins { get; set; }
    }
    [Test]
    public void TestGetSysAdmin()
    {
        var controller = new SysAdminByDomainController();
        var response = controller.GetSysAdmin() as OkObjectResult;
        ClassicAssert.IsNotNull(response);

        var json = JsonConvert.SerializeObject(response.Value);
        var values = JsonConvert.DeserializeObject<SysAdminResponse>(json);
        ClassicAssert.IsNotNull(values);

        Assert.Multiple(() =>
        {
            ClassicAssert.IsNotNull(values.domains);
            ClassicAssert.IsInstanceOf<List<DomainDTO>>(values.domains, "Wrong type");
            ClassicAssert.IsNotNull(values.users);
            ClassicAssert.IsInstanceOf<List<LoginDomainUserDTO>>(values.users, "Wrong type");
            ClassicAssert.IsNotNull(values.logins);
            ClassicAssert.IsInstanceOf<List<LoginDTO>>(values.logins, "Wrong type");
        });
    } 
        
    [Test]
    public void TestUpdateUser()
    {
        // Arrange
        var controller = new SysAdminByDomainController();

        // Act
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

        // Assert
        Assert.Multiple(() =>
        {
            ClassicAssert.IsNotNull(response);
            Assert.That(response.StatusCode, Is.EqualTo(200));

            var json = JsonConvert.SerializeObject(response.Value);
            ClassicAssert.IsNotNull(json);
            
            ClassicAssert.IsInstanceOf<LoginDomainUserDTO>(response.Value, "Wrong type");
            var values = JsonConvert.DeserializeObject<LoginDomainUser>(json);

            // Assert that the object is not null
            ClassicAssert.IsNotNull(values);
            
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
        });
    }
    
    /*
    * Test that the user is deleted if the user is a sysadmin
    */
    [Test]
    public void TestDeleteUser()
    {
        // Arrange
        var controller = new SysAdminByDomainController();
        var user = new LoginDomainUserDTO
        {
            LoginId = 26995,
            UserId = "99999999-9999-9999-9999-999999999999",
            DomainId = 351,
            Environment = 4,
            SysAdmin = true,
            SysAdminStartDate = DateTime.Parse("2024-01-16T00:00:00"),
            SysAdminEndDate = DateTime.Parse("2024-02-16T00:00:00"),
            Comment = "added rights",
            ModifiedBy = "admin",
            UserName = null
        };
        controller.UpdateUser(user);

        // Act
        var response = controller.DeleteUser(user.LoginId, user.UserId, user.DomainId, user.Environment) as OkObjectResult;
        
        // Assert 
        Assert.Multiple(() =>
        {
            ClassicAssert.IsNotNull(response);
            Assert.That(response.StatusCode, Is.EqualTo(200));
            Assert.That(response.Value, Is.EqualTo("Deleted user "+user.UserId+ ": "+user.LoginId+" | "+user.DomainId+" | "+user.Environment));
        });
    }

    /*
     * Test that the user is not deleted if the user is not a sysadmin
     */
    [Test]
    public void TestDeleteUserNotSysAdmin()
    {
        // Arrange
        var controller = new SysAdminByDomainController();
        var user = new LoginDomainUserDTO
        {
            LoginId = 26995,
            UserId = "99999999-9999-9999-9999-999999999999",
            DomainId = 351,
            Environment = 4,
            SysAdmin = false,
            SysAdminStartDate = DateTime.Parse("2024-01-16T00:00:00"),
            SysAdminEndDate = DateTime.Parse("2024-02-16T00:00:00"),
            Comment = "added rights",
            ModifiedBy = "admin",
            UserName = null
        };
        controller.UpdateUser(user);

        // Act
        var response = controller.DeleteUser(user.LoginId, user.UserId, user.DomainId, user.Environment) as OkObjectResult;
        
        // Assert 
        Assert.Multiple(() =>
        {
            ClassicAssert.IsNotNull(response);
            Assert.That(response.StatusCode, Is.EqualTo(200));
            Assert.That(response.Value, Is.EqualTo("Unable to find user "+user.UserId+ ": "+user.LoginId+" | "+user.DomainId+" | "+user.Environment));
        });
    }
}