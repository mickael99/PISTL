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
    /***************************************************************************************/
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

            var domains = context.Domains;
            return Ok(domains);
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

            return Ok(new { domains = context.Domains, newDomainId = domain.DomainId });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /***************************************************************************************/
    /// <summary>
    /// Updates a domain with the specified ID.
    /// </summary>
    /// <param name="id">The ID of the domain to update.</param>
    /// <param name="model">The updated domain model.</param>
    /// <returns>The updated domain if successful, or an error message if not.</returns>
    [HttpPut("{id}")]
    public IActionResult PutDomain(int id, [FromBody] DomainModel model)
    {
        Console.WriteLine("===============> PUT /api/domain");
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

            return Ok(new { domains = context.Domains, domain = existingDomain });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /***************************************************************************************/
    /// <summary>
    /// Deletes a domain with the specified ID.
    /// </summary>
    /// <param name="id">The ID of the domain to delete.</param>
    /// <returns>The remaining domains if successful, or an error message if not.</returns>
    [HttpDelete("{id}")]
    public IActionResult DeleteDomain(int id)
    {
        try
        {
            var context = new DatContext();

            //delete login domains
            var loginDomainToDelete = context.LoginDomainUsers.Where(l => l.DomainId == id).ToList();
            context.LoginDomainUsers.RemoveRange(loginDomainToDelete);

            //delete domain
            var domainToDelete = context.Domains.FirstOrDefault(d => d.DomainId == id);

            if (domainToDelete == null)
                return NotFound($"Domain with ID {id} not found.");

            context.Domains.Remove(domainToDelete);
            context.SaveChanges();

            Console.WriteLine("===============>");

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

    public class DomainModel
    {
        public required string Name { get; set; }
        public int? DomainId { get; set; }
        public byte[]? Logo { get; set; }
        public string? PathLogo { get; set; }
        public string? Edition { get; set; }
        public Boolean? IsSsoEnabled { get; set; }
        public string? Comment { get; set; }
        public string? ParentCompany { get; set; }
    }
}
/***************************************************************************************/
/***************************************************************************************/
/***************************************************************************************/