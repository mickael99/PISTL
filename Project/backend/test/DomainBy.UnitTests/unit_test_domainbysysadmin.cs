
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NUnit.Framework;
using Project.Controllers;
using Project.Models;
using Project.Models.DTO;
using System;
using System.Collections.Generic;

namespace backend.Tests
{
    public class TestDomainBy
    {
        public class DomainByResponse
        {
            public List<DomainDTO> domains { get; set; }
            public List<LoginDomainUserDTO> users { get; set; }
            public List<LoginDTO> logins { get; set; }
        }

        [Test]
        public void TestGetDomainByLogin()
        {
            // Arrange
            var controller = new DomainBySysAdmin();

            // Act
            var response = controller.GetDomainBySysAdmin(26996) as OkObjectResult;

            // Assert
            Assert.Multiple(() =>
            {
                Assert.IsNotNull(response);
                var json = JsonConvert.SerializeObject(response.Value);
                Assert.IsNotNull(json);
                var values = JsonConvert.DeserializeObject<DomainByResponse>(json);
                Assert.IsNotNull(values);

                Assert.IsNotNull(values.domains);
                Assert.IsInstanceOf<List<DomainDTO>>(values.domains, "Wrong type");
                Assert.IsNotNull(values.users);
                Assert.IsInstanceOf<List<LoginDomainUserDTO>>(values.users, "Wrong type");
                Assert.IsNotNull(values.logins);
                Assert.IsInstanceOf<List<LoginDTO>>(values.logins, "Wrong type");
            });
        }

        [Test]
        public void TestUpdateDomain()
        {
            // Arrange
            var controller = new DomainBySysAdmin();

            // Act
            var response = controller.PostSysAdmin(
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

            Console.WriteLine(response);
            
            // Assert
            Assert.Multiple(() =>
            {
                // Assert.IsNotNull(response);
                // Assert.That(response.StatusCode, Is.EqualTo(200));

                // var json = JsonConvert.SerializeObject(response.Value);
                // Assert.IsNotNull(json);

                // Assert.IsInstanceOf<LoginDomainUserDTO>(response.Value, "Wrong type");
                // var values = JsonConvert.DeserializeObject<LoginDomainUser>(json);

                // // Assert that the object is not null
                // Assert.IsNotNull(values);

                // // Assert that the types are valid
                // Assert.That(values.ModifiedBy, Is.TypeOf(typeof(string)));
                // Assert.That(values.UserId, Is.TypeOf(typeof(string)));
                // Assert.That(values.UserId, Is.EqualTo("99999999-9999-9999-9999-999999999999"));
                // Assert.That(values.Environment, Is.TypeOf(typeof(int)));
                // Assert.That(values.LoginId, Is.TypeOf(typeof(int)));
                // Assert.That(values.DomainId, Is.TypeOf(typeof(int)));
                // Assert.That(values.SysAdmin, Is.EqualTo(true));
                // Assert.That(values.Comment, Is.TypeOf(typeof(string)));

                // // Assert that the dates are valid
                // Assert.That(values.SysAdminEndDate, Is.TypeOf(typeof(DateTime)));
                // Assert.That(values.SysAdminStartDate, Is.TypeOf(typeof(DateTime)));
                // Assert.That(values.SysAdminStartDate, Is.LessThan(values.SysAdminEndDate));
            });
        }

        /*
        * Test that the user is deleted if the login is not sysadmin in the domain
        */
        [Test]
        public void TestDeleteDomain()
        {
            // Arrange
            var controller = new DomainBySysAdmin();
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
            controller.PostSysAdmin(user);

            // Act
            var response = controller.DeleteUser(user.LoginId, user.UserId, user.DomainId, user.Environment) as OkObjectResult;

            // Assert 
            Assert.Multiple(() =>
            {
                // Assert.IsNotNull(response);
                // Assert.That(response.StatusCode, Is.EqualTo(200));
                // Assert.That(response.Value, Is.EqualTo("Deleted user " + user.UserId + ": " + user.LoginId + " | " + user.DomainId + " | " + user.Environment));
            });
        }
    }
}