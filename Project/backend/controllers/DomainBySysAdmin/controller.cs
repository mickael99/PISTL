using System;
using Project.Models;
using Microsoft.AspNetCore.Mvc;
using Project.Models.DTO;

namespace Project.Controllers
{
    [Route("api/domainbysysadmin")]
    [ApiController]
    public class DomainBySysAdmin : ControllerBase
    {
        [HttpGet]
        public IActionResult GetSysAdmin()
        {
            try
            {
                
                // Retrieve the list of all domains from your data source
                var context = new MasterContext();
                var domains = context.Domains.Select(d => new DomainDTO
                {
                    // Map properties from Domain to DomainDTO
                    DomainId = d.DomainId,
                    Name = d.Name
                }).ToList();
                var users = context.LoginDomainUsers.Select(u => new LoginDomainUserDTO
                {
                    // Map properties from LoginDomainUser to LoginDomainUserDTO
                    LoginId = u.LoginId,
                    DomainId = u.DomainId,
                    UserId = u.UserId,
                    Environment = u.Environment,
                    SysAdmin = u.SysAdmin,
                    Comment = u.Comment,
                    ModifiedBy = u.ModifiedBy
                }).ToList();
                var logins = context.Logins.Select(l => new LoginDTO
                {
                    // Map properties from Login to LoginDTO
                    LoginId = l.LoginId,
                    Email = l.Email
                }).ToList();
                var envs = context.DomainEnvironments.Select(e => new DomainEnvironmentDTO
                {
                    // Map properties from Environment to EnvironmentDTO
                    DomainEnvironmentId = e.DomainEnvironmentId,
                    DomainId = e.DomainId,
                    Environment = e.Environment
                }).ToList();

                return Ok(new{domains, users, logins, envs});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult PostSysAdmin([FromBody] LoginDomainUserDTO userDTO)
        {
            try
            {
                Console.WriteLine("Posting new user...");
                Console.WriteLine("DTO: "+userDTO.LoginId+" "+userDTO.Environment+" "+userDTO.DomainId+" "+userDTO.UserId+" | "+ userDTO.SysAdmin);
                
                var context = new MasterContext();
                Console.WriteLine("Before");
                foreach (var user in context.LoginDomainUsers.ToList())
                {
                    Console.WriteLine(user.LoginId +" "+ user.Environment+" "+ user.SysAdmin);
                }
                try
                {
                    context.LoginDomainUsers.Add(new LoginDomainUser
                    {
                        LoginId = userDTO.LoginId,
                        DomainId = userDTO.DomainId,
                        UserId = userDTO.UserId,
                        Environment = userDTO.Environment,
                        SysAdmin = userDTO.SysAdmin,
                        SysAdminStartDate = userDTO.SysAdminStartDate,
                        SysAdminEndDate = userDTO.SysAdminEndDate,
                        Comment = userDTO.Comment,
                        UserName = userDTO.UserName,
                        ModifiedBy = userDTO.ModifiedBy,
                        UserActive = false,
                        LoginEnabled = true,
                        LoginTypeId = null,
                        AnalyticsEnabled = null,
                        IsLight = null,
                        DomainLastLoginDate = null,
                        CreatedBy = "admin", // TODO: Change this to the current user
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        Domain = context.Domains.Find(userDTO.DomainId),
                        Login = context.Logins.Find(userDTO.LoginId)
                    });
                    context.SaveChanges();
                    Console.WriteLine("Added new one");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
                Console.WriteLine("After");
                foreach (var user in context.LoginDomainUsers.ToList())
                {
                    Console.WriteLine(user.LoginId +" "+ user.Environment+" "+ user.SysAdmin);
                }
                return Ok(userDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}