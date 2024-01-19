using Project.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;

/***************************************************************************************/
/***************************************************************************************/
/***************************************************************************************/
[Route("api/domain")]
[ApiController]
public class DomainAdministrationController : ControllerBase
{
    /// <summary>
    /// Retrieves the domains with their associated environments from the database.
    /// </summary>
    /// <returns>An IActionResult representing the result of the operation.</returns>
    [HttpGet]
    public IActionResult GetDomains() // TODO add JWT
    {
        try
        {
            var context = new DatContext();

            var domainsWithEnvironments = context.Domains
            .GroupJoin(
                context.DomainEnvironments,
                domain => domain.DomainId,
                environment => environment.DomainId,
                (domain, environments) => new { Domain = domain, Environments = environments.DefaultIfEmpty() }
            )
            .SelectMany(
                result => result.Environments,
                (result, environment) => new { Domain = result.Domain, Environment = environment }
            )
            .ToList();

            var mappedData = domainsWithEnvironments.GroupBy(
                pair => pair.Domain,
                pair => pair.Environment,
                (domain, environments) => new DomainModel
                {
                    Name = domain.Name,
                    Logo = domain.Logo,
                    Edition = domain.Edition,
                    IsSsoEnabled = domain.IsSsoEnabled,
                    Comment = domain.Comment,
                    ParentCompany = domain.ParentCompany,
                    // Environments = environments.Select(environment => new EnvironmentModel
                    // {
                    //     Environment = environment.Environment,
                    //     BpwebServerId = environment.BpwebServerId,
                    //     EaidatabaseId = environment.EaidatabaseId,
                    //     SsrsserverId = environment.SsrsserverId,
                    //     TableauServerId = environment.TableauServerId,
                    //     EaiftpserverId = environment.EaiftpserverId,
                    //     IsBp5Enabled = environment.IsBp5Enabled,
                    //     BpdatabaseId = environment.BpdatabaseId
                    // }).ToList()
                }
            ).ToList();

            return Ok(new { mappedData }); ;
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /***************************************************************************************/
    /// <summary>
    /// Creates a new domain and adds it to the database.
    /// </summary>
    /// <param name="model">The domain model containing the necessary information for creating the domain.</param>
    /// <returns>An IActionResult representing the result of the operation.</returns>
    [HttpPost]
    public IActionResult PostDomain([FromBody] DomainModel model)
    {
        try
        {
            var context = new DatContext();
            Console.WriteLine("=> POST /api/domain");

            Domain domain = addDomain(model.Name, model.Logo ?? new byte[0], model.Edition ?? string.Empty, model.IsSsoEnabled ?? false,
                                        model.Comment ?? string.Empty, model.ParentCompany ?? string.Empty);
            context.Domains.Add(domain);
            context.SaveChanges();

            // List<DomainEnvironment> environments = addDomainEnvironments(domain.DomainId, model.Environments);

            // foreach (DomainEnvironment e in environments)
            //     context.DomainEnvironments.Add(e);
            // context.SaveChanges();

            return Ok(new { domains = context.Domains, newDomainId = domain.DomainId });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /***************************************************************************************/
    [HttpPut("{id}")]
    public IActionResult PutDomain(int id, [FromBody] DomainModel model)
    {
        Console.WriteLine("===============> POST /api/domain");
        try
        {
            var context = new DatContext();
            var existingDomain = context.Domains.FirstOrDefault(d => d.DomainId == id);

            if (existingDomain == null)
                return NotFound($"Domain with ID {id} not found.");

            existingDomain.Name = model.Name;
            existingDomain.Logo = model.Logo;
            existingDomain.Edition = model.Edition;
            existingDomain.IsSsoEnabled = model.IsSsoEnabled ?? false;
            existingDomain.Comment = model.Comment;
            existingDomain.ParentCompany = model.ParentCompany;
            context.SaveChanges();

            return Ok(context.Domains);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteDomain(int id)
    {
        try
        {
            var context = new DatContext();
            var domainToDelete = context.Domains.FirstOrDefault(d => d.DomainId == id);

            if (domainToDelete == null)
                return NotFound($"Domain with ID {id} not found.");

            context.Domains.Remove(domainToDelete);
            context.SaveChanges();

            var remainingDomains = context.Domains.ToList();
            return Ok(remainingDomains);
        }
        catch (Exception ex)
        {
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
         string parentCompany)
    {

        var newDomain = new Domain
        {
            Name = name,
            Logo = logo,
            Edition = edition,
            IsSsoEnabled = isSsoEnabled,
            Comment = comment,
            ParentCompany = parentCompany
        };

        return newDomain;
    }

    static List<DomainEnvironment> addDomainEnvironments(int domainId, List<EnvironmentModel> environmentModel)
    {
        List<DomainEnvironment> environments = new List<DomainEnvironment>();

        foreach (EnvironmentModel e in environmentModel)
        {
            var environmentToAdd = new DomainEnvironment
            {
                DomainId = e.DomainId,
                Environment = e.Environment,
                BpwebServerId = e.BpwebServerId,
                BpdatabaseId = e.BpdatabaseId,
                EaidatabaseId = e.EaidatabaseId,
                SsrsserverId = e.SsrsserverId,
                TableauServerId = e.TableauServerId,
                EaiftpserverId = e.EaiftpserverId,
                IsBp5Enabled = e.IsBp5Enabled
            };
            environments.Add(environmentToAdd);
        }
        return environments;
    }

    public class DomainModel
    {
        public required string Name { get; set; }
        public byte[]? Logo { get; set; }
        public string? PathLogo { get; set; }
        public string? Edition { get; set; }
        public Boolean? IsSsoEnabled { get; set; }
        public string? Comment { get; set; }
        public string? ParentCompany { get; set; }
        public List<EnvironmentModel>? Environments { get; set; }
    }

    public class EnvironmentModel
    {
        public int DomainId { get; set; }
        public int Environment { get; set; }
        public int BpwebServerId { get; set; }
        public int? BpdatabaseId { get; set; }
        public int? EaidatabaseId { get; set; }
        public int? SsrsserverId { get; set; }
        public int? TableauServerId { get; set; }
        public int? EaiftpserverId { get; set; }
        public bool IsBp5Enabled { get; set; }
    }
}
/***************************************************************************************/
/***************************************************************************************/
/***************************************************************************************/
