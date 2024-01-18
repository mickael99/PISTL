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
            var context = new MasterContext();

            var domainsWithEnvironments = context.Domains
                .Join(
                    context.DomainEnvironments,
                    domain => domain.DomainId,
                    environment => environment.DomainId,
                    (domain, environment) => new { Domain = domain, Environment = environment }
                )
                .ToList();

            var mappedData = domainsWithEnvironments.GroupBy(
                pair => pair.Domain,
                pair => pair.Environment, 
                (domain, environments) => new DomainModel {
                    Name = domain.Name,
                    Logo = domain.Logo,
                    Edition = domain.Edition,
                    IsSsoEnabled = domain.IsSsoEnabled,
                    Comment = domain.Comment,
                    ParentCompany = domain.ParentCompany,
                    Environments = environments.Select(environment => new EnvironmentModel {
                        EnvironmentId = environment.DomainEnvironmentId,
                        DomainId = environment.DomainId,
                        Environment = environment.Environment,
                        BpwebServerId = environment.BpwebServerId,
                        EaiDatabaseId = environment.EaiDatabaseId,
                        SsrsServerId = environment.SsrsServerId,
                        TableauServerId = environment.TableauServerId,
                        EaiftpServerId = environment.EaiftpServerId,
                        IsBp5Enabled = environment.IsBp5Enabled,
                        BpDatabaseId = environment.BpDatabaseId
                    }).ToList()
                }
            ).ToList();

            return Ok(mappedData);
        } 
        catch (Exception ex) {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public IActionResult PostDomain([FromBody] DomainModel model) {
        Console.WriteLine("===============> POST /api/domain");
        try {
            var context = new MasterContext();
            Domain domain = addDomain(model.Name, model.Logo, model.Edition, model.IsSsoEnabled,
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
            existingDomain.Logo = model.Logo; 
            existingDomain.Edition = model.Edition;
            existingDomain.IsSsoEnabled = model.IsSsoEnabled;
            existingDomain.Comment = model.Comment;
            existingDomain.ParentCompany = model.ParentCompany;
            context.SaveChanges();

            return Ok(context.Domains);
        } catch(Exception ex) {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteDomain(int id) {
        try {
            var context = new MasterContext();
            var domainToDelete = context.Domains.FirstOrDefault(d => d.DomainId == id);

            if (domainToDelete == null)
                return NotFound($"Domain with ID {id} not found.");

            context.Domains.Remove(domainToDelete);
            context.SaveChanges();

            var remainingDomains = context.Domains.ToList();
            return Ok(remainingDomains);
        } catch (Exception ex) {
            return BadRequest(ex.Message);
        }
    }

    // create a domain to add to the database
    static Domain addDomain
        (string name, 
         byte[] logo,
         string edition,
         Boolean isSsoEnabled,
         string comment,
         string parentCompany) {

        var newDomain = new Domain {
            Name = name,
            Logo = logo,
            Edition = edition,
            IsSsoEnabled = isSsoEnabled,
            Comment = comment,
            ParentCompany = parentCompany
        };

        return newDomain;
    }

    public class DomainModel {
        public string Name { get; set; }
        public byte[] Logo { get; set; }
        public string Edition { get; set; }
        public Boolean IsSsoEnabled { get; set; }
        public string Comment { get; set; }
        public string ParentCompany { get; set; }
        public List<EnvironmentModel> Environments { get; set; }
    }

    public class EnvironmentModel {
        public int EnvironmentId {get; set; }
        public int DomainId { get; set; }
        public int Environment { get; set; }
        public int BpwebServerId { get; set; }
        public int? BpDatabaseId { get; set; }
        public int? EaiDatabaseId { get; set; }
        public int? SsrsServerId { get; set; }
        public int? TableauServerId { get; set; }
        public int? EaiftpServerId { get; set; }
        public bool IsBp5Enabled { get; set; }
    }
}

