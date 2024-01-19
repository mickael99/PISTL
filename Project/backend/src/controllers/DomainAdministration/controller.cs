using System;
using Project.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;

[Route("api/domain")]
[ApiController]
public class DomainAdministrationController : ControllerBase
{
    [HttpGet]
    public IActionResult GetDomains()
    {
        try
        {
            // get all domains
            var context = new MasterContext();
            var domains = context.Domains;
            return Ok(domains);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public IActionResult PostDomain([FromBody] DomainModel model)
    {
        Console.WriteLine("===============> POST /api/domain");
        try
        {
            var context = new MasterContext();
            Domain domain = addDomain(model.DomainName, model.CreatedBy, model.Edition);
            context.Domains.Add(domain);
            context.SaveChanges();
            return Ok(context.Domains);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // create a domain to add to the database
    static Domain addDomain(string domainName, string createdBy, string edition)
    {
        var newDomain = new Domain
        {
            Name = domainName,
            Logo = Encoding.UTF8.GetBytes("www.html.am/images/samples/remarkables_queenstown_new_zealand-300x225.jpg"),
            Edition = edition,
            IsSsoEnabled = true,
            Comment = "No comment",
            ParentCompany = "No parent company",
            CreatedDate = DateTime.Now,
            CreatedBy = createdBy,
            ModifiedDate = DateTime.Now,
            ModifiedBy = "Daniel"
        };

        return newDomain;
    }

    public class DomainModel
    {
        public required string DomainName { get; set; }
        public required string CreatedBy { get; set; }
        public required string Edition { get; set; }
    }
}
