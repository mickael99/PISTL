using Project.Models;
using Microsoft.AspNetCore.Mvc;
using Project.Models.DTO;
using System.Collections;

namespace Project.Controllers
{
    [Route("api/sysadminbydomain")] 
    [ApiController]
    public class SysAdminByDomainController : ControllerBase
    {
        [HttpGet("{domainId}")]
        public IActionResult GetSysAdminByDomain(int domainId)
        {
            try
            {
                // Retrieve the domain selected from your data source
                var context = new MasterContext();
                var domains = context.Domains.Select(d => new DomainDTO
                {
                    // Map properties from Domain to DomainDTO
                    DomainId = d.DomainId,
                    Name = d.Name,
                    Environments = new bool[6] {false, false, false, false, false, false}
                }).ToList();
                
                // Retrieve the list of all users with sysadmin rights and the domain selected from your data source
                var users = context.LoginDomainUsers.Where(u => u.DomainId == domainId && u.UserId == "99999999-9999-9999-9999-999999999999")
                                                    .Select(u => new LoginDomainUserDTO
                {
                    // Map properties from LoginDomainUser to LoginDomainUserDTO
                    LoginId = u.LoginId,
                    DomainId = u.DomainId,
                    UserId = u.UserId,
                    Environment = u.Environment,
                    SysAdmin = u.SysAdmin,
                    SysAdminStartDate = u.SysAdminStartDate,
                    SysAdminEndDate = u.SysAdminEndDate,
                    Comment = u.Comment,
                    UserName = u.UserName,
                    ModifiedBy = u.ModifiedBy,
                }).ToList();

                // Retrieve the list of all logins from your data source
                var logins = context.Logins.Select(l => new LoginDTO
                {
                    // Map properties from Login to LoginDTO
                    LoginId = l.LoginId,
                    Email = l.Email
                }).ToList();

                // Retrieve the list of all users from your data source
                var all_users = context.LoginDomainUsers.Where(u => u.UserId == "99999999-9999-9999-9999-999999999999")
                                                    .Select(u => new LoginDomainUserDTO
                {
                    // Map properties from LoginDomainUser to LoginDomainUserDTO
                    LoginId = u.LoginId,
                    DomainId = u.DomainId,
                    UserId = u.UserId,
                    Environment = u.Environment,
                    SysAdmin = u.SysAdmin,
                    SysAdminStartDate = u.SysAdminStartDate,
                    SysAdminEndDate = u.SysAdminEndDate,
                    Comment = u.Comment,
                    UserName = u.UserName,
                    ModifiedBy = u.ModifiedBy,
                }).ToList();
                foreach (var domain in domains)
                {
                    foreach (var user in all_users)
                    {
                        if (domain.DomainId == user.DomainId && user.SysAdmin == true){
                            {
                                if(user.Environment == 1)
                                {
                                    domain.Environments[0] = true;
                                }
                                else if(user.Environment == 2)
                                {
                                    domain.Environments[1] = true;
                                }
                                else if(user.Environment == 3)
                                {
                                    domain.Environments[2] = true;
                                }
                                else if(user.Environment == 4)
                                {
                                    domain.Environments[3] = true;
                                }
                                else if(user.Environment == 5)
                                {
                                    domain.Environments[4] = true;
                                }
                                else if(user.Environment == 6)
                                {
                                    domain.Environments[5] = true;
                                }
                            }
                        }
                    }
                }
                // free all_users
                all_users = null;

                return Ok(new { domains, users, logins });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


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

                // Retrieve the list of all users from your data source
                var users = context.LoginDomainUsers.Where(u => u.UserId == "99999999-9999-9999-9999-999999999999")
                                                    .Select(u => new LoginDomainUserDTO
                {
                    // Map properties from LoginDomainUser to LoginDomainUserDTO
                    LoginId = u.LoginId,
                    DomainId = u.DomainId,
                    UserId = u.UserId,
                    Environment = u.Environment,
                    SysAdmin = u.SysAdmin,
                    SysAdminStartDate = u.SysAdminStartDate,
                    SysAdminEndDate = u.SysAdminEndDate,
                    Comment = u.Comment,
                    UserName = u.UserName,
                    ModifiedBy = u.ModifiedBy,
                }).ToList();

                // Retrieve the list of all logins from your data source
                var logins = context.Logins.Select(l => new LoginDTO
                {
                    // Map properties from Login to LoginDTO
                    LoginId = l.LoginId,
                    Email = l.Email
                }).ToList();

                return Ok(new { domains, users, logins });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public IActionResult UpdateUser([FromBody] LoginDomainUserDTO userDTO) {
            try
            {
                var context = new MasterContext();
                var user = context.LoginDomainUsers.Where(u => u.LoginId == userDTO.LoginId && u.DomainId == userDTO.DomainId
                        && u.Environment == userDTO.Environment && u.UserId == userDTO.UserId).SingleOrDefault();
                if(user == null)
                {
                    Console.WriteLine("Posted user "+userDTO.UserId+ ": "+userDTO.LoginId+" | "+userDTO.DomainId+" | "+userDTO.Environment+" | "+userDTO.SysAdmin);
                    user = new LoginDomainUser
                    {
                        LoginId = userDTO.LoginId,
                        DomainId = userDTO.DomainId,
                        UserId = userDTO.UserId,
                        Environment = userDTO.Environment,
                        SysAdmin = userDTO.SysAdmin,
                        SysAdminStartDate = userDTO.SysAdminStartDate,
                        SysAdminEndDate = userDTO.SysAdminEndDate,
                        Comment = userDTO.Comment,
                        ModifiedBy = userDTO.ModifiedBy,
                        ModifiedDate = DateTime.Now,
                        CreatedBy = userDTO.ModifiedBy, // TODO get the actual session user
                        CreatedDate = DateTime.Now,
                        Login = context.Logins.Where(u => u.LoginId == userDTO.LoginId).Single<Login>(),
                        Domain = context.Domains.Where(d => d.DomainId == userDTO.DomainId).Single<Domain>()
                    };
                    context.LoginDomainUsers.Add(user);
                    context.SaveChanges();
                }
                else
                { 
                    Console.WriteLine("Modified user "+user.UserId+ ": "+userDTO.LoginId+" | "+user.DomainId+" | "+user.Environment+" | "+user.SysAdmin);
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
                    //Console.WriteLine("Modified user "+user.UserId);
                    context.SaveChanges();
                }
                
                return Ok(userDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{loginID}+{userID}+{domainID}+{env}")]
        public IActionResult DeleteUser(int loginID, string userID, int domainID, int env)
        {
            try
            {
                var context = new MasterContext();

                // Retrieve the user to delete from your data source (if it doesn't exist doesn't do anything, just return)
#pragma warning disable CS8600 // Conversion de littéral ayant une valeur null ou d'une éventuelle valeur null en type non-nullable.
                LoginDomainUser user = context.LoginDomainUsers.Where(u => u.LoginId == loginID && u.DomainId == domainID
                        && u.Environment == env && u.UserId == userID && u.SysAdmin == true).SingleOrDefault<LoginDomainUser>();
#pragma warning restore CS8600 // Conversion de littéral ayant une valeur null ou d'une éventuelle valeur null en type non-nullable.

                if(user != null)
                { 
                    context.LoginDomainUsers.Remove(user);
                    Console.WriteLine("Deleted user "+user.UserId);
                    context.SaveChanges();
                    var response = "Deleted user "+userID+ ": "+loginID+" | "+domainID+" | "+env;
                    return Ok(response);
                }
                else
                {
                    Console.WriteLine("Unable to delete "+userID+ ": "+loginID+" | "+domainID+" | "+env);
                    var response = "Unable to find user "+userID+ ": "+loginID+" | "+domainID+" | "+env;
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}