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
    public IActionResult PostSysAdmin([FromBody] LoginDomainUserDTO loginDomainUserDTO)
    {
        try
        {
            Console.WriteLine("DTO: "+loginDomainUserDTO.LoginId+" "+loginDomainUserDTO.Environment+" "+loginDomainUserDTO.DomainId+" "+loginDomainUserDTO.UserId+"  |"+ loginDomainUserDTO.SysAdmin);
            // Remove previous LoginDomainUser object and add the new one
            var context = new MasterContext();
            var allLoginDomainUsers = context.LoginDomainUsers.ToList();
            Console.WriteLine("Before");
            foreach (var user in allLoginDomainUsers)
            {
                Console.WriteLine(user.LoginId +" "+ user.SysAdmin);
            }

            Boolean found = false;
            foreach (var user in allLoginDomainUsers)
            {
                if(user.LoginId == loginDomainUserDTO.LoginId && user.DomainId == loginDomainUserDTO.DomainId
                     && user.Environment == loginDomainUserDTO.Environment && user.UserId == loginDomainUserDTO.UserId)
                {
                    Console.WriteLine("Found, now modifying...");
                    modifyUser(user, loginDomainUserDTO);
                    context.LoginDomainUsers.Remove(user);
                    try
                    {
                        context.SaveChanges();
                        Console.WriteLine("Removed previous");
                        context.LoginDomainUsers.Add(user);
                        context.SaveChanges();
                        Console.WriteLine("Added new one");
                        found = true;
                        break;
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex.Message);
                    }
                } 
                
            }
            if(!found)
            {
                context.LoginDomainUsers.Add(createUser(loginDomainUserDTO, context));
                try
                {
                    context.SaveChanges();
                    Console.WriteLine("Added new one");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            Console.WriteLine("After");
            allLoginDomainUsers = context.LoginDomainUsers.ToList();
            foreach (var user in allLoginDomainUsers)
            {
                Console.WriteLine(user.LoginId +" "+ user.SysAdmin);
            }
            return Ok(loginDomainUserDTO);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    private void modifyUser(LoginDomainUser user, LoginDomainUserDTO newUser)
    {
        user.LoginId = newUser.LoginId;
        user.DomainId = newUser.DomainId;
        user.UserId = newUser.UserId;
        user.Environment = newUser.Environment;
        user.SysAdmin = newUser.SysAdmin;
        user.SysAdminStartDate = newUser.SysAdminStartDate;
        user.SysAdminEndDate = newUser.SysAdminEndDate;
        user.Comment = newUser.Comment;
        user.ModifiedBy = newUser.ModifiedBy;
    }

    private LoginDomainUser createUser(LoginDomainUserDTO userDTO, MasterContext context)
    {
        var user = new LoginDomainUser
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
        };
        return user;
    }
}