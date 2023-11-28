using System;
using Project.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;

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
            var domains = context.Domains;
            var users = context.LoginDomainUsers;
            var logins = context.Logins;
            // TODO: Add your logic here

            return Ok(new{domains, users, logins});
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}