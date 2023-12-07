using System;
using Project.Models;
using Microsoft.AspNetCore.Mvc;
using Project.Models.DTO;

[Route("api/sysadminbydomain")]
[ApiController]
public class SysAdminByDomainController : ControllerBase
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

    [HttpPut]
    public IActionResult UpdateUser([FromBody] LoginDomainUserDTO userDTO)
    {
        try
        {
            var context = new MasterContext();

            Console.WriteLine("Updating user "+userDTO.UserId+"...");
            Console.WriteLine("Before");
            foreach (var u in context.LoginDomainUsers.ToList())
            {
                Console.WriteLine(u.LoginId +" "+ u.SysAdmin);
            }
            LoginDomainUser user = context.LoginDomainUsers.Where(u => u.LoginId == userDTO.LoginId && u.DomainId == userDTO.DomainId
                     && u.Environment == userDTO.Environment && u.UserId == userDTO.UserId).Single<LoginDomainUser>();
            if(user == null)
            {
                Console.WriteLine("Not found");
                return NotFound();
            }
            else
            { 
                Console.WriteLine("Found user "+user.UserId+ ": "+user.DomainId+" | "+user.Environment+" | "+user.SysAdmin);
                user.LoginId = userDTO.LoginId;
                user.DomainId = userDTO.DomainId;
                user.UserId = userDTO.UserId;
                user.Environment = userDTO.Environment;
                user.SysAdmin = userDTO.SysAdmin;
                user.SysAdminStartDate = userDTO.SysAdminStartDate;
                user.SysAdminEndDate = userDTO.SysAdminEndDate;
                user.Comment = userDTO.Comment;
                user.ModifiedBy = userDTO.ModifiedBy;
                
                context.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                Console.WriteLine("Modified user "+user.UserId);
                context.SaveChanges();

                Console.WriteLine("After");
                foreach (var u in context.LoginDomainUsers.ToList())
                {
                    Console.WriteLine(u.LoginId +" "+ u.SysAdmin);
                }
                
                return Ok(user);
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}