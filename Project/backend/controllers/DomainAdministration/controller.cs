using System;
using Project.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;

[Route("api/domain")]
[ApiController]
public class DomainAdministrationController : ControllerBase {
    [HttpGet]
    public IActionResult GetDomains() {
        try {
            // get all domains
            var context = new MasterContext();
            var domains = context.Domains.ToList();
            return Ok(domains);
        } catch (Exception ex){
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public IActionResult PostDomain([FromBody] DomainModel model) {
        Console.WriteLine("===============> POST /api/domain");
        try {
            var context = new MasterContext();
            Domain domain = addDomain(model.Name, model.Logo, model.Edition, model.isSsoEnabled,
                                        model.Comment, model.ParentCompany);
            context.Domains.Add(domain);
            context.SaveChanges();
            return Ok(context.Domains);
        } catch (Exception ex) {
            return BadRequest(ex.Message);
        }
    } 

    [HttpPut("{id}")]
    public IActionResult PutDomain(int id, [FromBody] DomainModel model) {
        Console.WriteLine("===============> POST /api/domain");
        try {
            var context = new MasterContext();
            var existingDomain = context.Domains.FirstOrDefault(d => d.DomainId == id);

            if (existingDomain == null)
                return NotFound($"Domain with ID {id} not found.");
            
            existingDomain.Name = model.Name;
            existingDomain.Logo = Encoding.UTF8.GetBytes(model.Logo); 
            existingDomain.Edition = model.Edition;
            existingDomain.IsSsoEnabled = model.isSsoEnabled;
            existingDomain.Comment = model.Comment;
            existingDomain.ParentCompany = model.ParentCompany;
            context.SaveChanges();

            return Ok(context.Domains);
        } catch(Exception ex) {
            return BadRequest(ex.Message);
        }
    }

    // create a domain to add to the database
    static Domain addDomain
        (string name, 
         string logo,
         string edition,
         Boolean isSsoEnabled,
         string comment,
         string parentCompany) {

        var newDomain = new Domain {
            Name = name,
            Logo = Encoding.UTF8.GetBytes(logo),
            Edition = edition,
            IsSsoEnabled = isSsoEnabled,
            Comment = comment,
            ParentCompany = parentCompany
        };

        return newDomain;
    }

    public class DomainModel {
        public string Name { get; set; }
        public string Logo { get; set; }
        public string Edition { get; set; }
        public Boolean isSsoEnabled { get; set; }
        public string Comment { get; set; }
        public string ParentCompany { get; set; }
    }
}