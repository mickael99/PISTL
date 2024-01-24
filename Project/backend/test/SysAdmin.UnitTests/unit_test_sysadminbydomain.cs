
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NUnit.Framework; // Add missing using statement
using Project.Controllers;
using Project.Models;
using Project.Models.DTO;
using System;
using System.Collections.Generic;

namespace backend.Tests;

public class TestSysAdmin
{
    public class SysAdminResponse
    {
        public List<DomainDTO> domains { get; set; } // Remove 'required' keyword
        public List<LoginDomainUserDTO> users { get; set; } // Remove 'required' keyword
        public List<LoginDTO> logins { get; set; } // Remove 'required' keyword
    }
    
    [Test]
    public void TestGetSysAdmin()
    {
        var controller = new SysAdminByDomainController();
        var response = controller.GetSysAdmin() as OkObjectResult;
        Assert.AreNotEqual(response, null); // Add missing reference to Assert class

        var json = JsonConvert.SerializeObject(response.Value);
        var values = JsonConvert.DeserializeObject<SysAdminResponse>(json);
        Assert.AreNotEqual(values, null); // Add missing reference to Assert class

        Assert.Multiple(() =>
        {
            Assert.AreNotEqual(values.domains, null);
            Assert.IsInstanceOf<List<DomainDTO>>(values.domains, "Wrong type");
            Assert.AreNotEqual(values.users, null);
            Assert.IsInstanceOf<List<LoginDomainUserDTO>>(values.users, "Wrong type");
            Assert.AreNotEqual(values.logins, null);
            Assert.IsInstanceOf<List<LoginDTO>>(values.logins, "Wrong type");
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
            Assert.AreNotEqual(response, null);
            Assert.That(response.StatusCode, Is.EqualTo(200));

            var json = JsonConvert.SerializeObject(response.Value);
            Assert.AreNotEqual(json, null);
            
            Assert.IsInstanceOf<LoginDomainUserDTO>(response.Value, "Wrong type");
            var values = JsonConvert.DeserializeObject<LoginDomainUser>(json);

            // Assert that the object is not null
            Assert.AreNotEqual(values, null);
            
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
            Assert.AreNotEqual(response, null);
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
            Assert.AreNotEqual(response, null);
            Assert.That(response.StatusCode, Is.EqualTo(200));
            Assert.That(response.Value, Is.EqualTo("Unable to find user "+user.UserId+ ": "+user.LoginId+" | "+user.DomainId+" | "+user.Environment));
        });
    }
}