using Project.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;

[Route("api/domainEnvironment")]
[ApiController]
public class DomainEnvironmentController : ControllerBase {
    
    [HttpGet]
    public IActionResult GetDomainEnvironments()  {
        try {
            var context = new MasterContext();
            Console.WriteLine("=> GET /api/domainEnvironment");

            var domainEnvironments = context.DomainEnvironments;

            return Ok(domainEnvironments);
        } catch (Exception ex) {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public IActionResult PostDomainEnvironment([FromBody] List<EnvironmentModel> models) {
        try {
            var context = new MasterContext();
            Console.WriteLine("=> POST /api/domainEnvironment");
           
           foreach(EnvironmentModel model in models) {
                DomainEnvironment domainEnvironment = new DomainEnvironment {
                    DomainId = model.DomainId,
                    Environment = model.Environment,
                    BpwebServerId = model.BpwebServerId,
                    BpdatabaseId = model.BpdatabaseId,
                    EaidatabaseId = model.EaidatabaseId,
                    SsrsserverId = model.SsrsserverId,
                    TableauServerId = model.TableauServerId,
                    EaiftpserverId = model.EaiftpserverId,
                    IsBp5Enabled = model.IsBp5Enabled
                };
                    
                context.DomainEnvironments.Add(domainEnvironment);
                context.SaveChanges();
           }

            return Ok( new { env = context.DomainEnvironments });
        } catch (Exception ex) {
            Console.WriteLine($"Error in PostDomainEnvironment: {ex.ToString()}");
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public IActionResult PutDomainEnvironment(int id, [FromBody] EnvironmentModel model) {
        Console.WriteLine("===============> PUT /api/domainEnvironment");

         try {
            var context = new MasterContext();
            var existingEnvironmentDomain = context.DomainEnvironments.FirstOrDefault(e => e.DomainEnvironmentId == id);

            if (existingEnvironmentDomain == null)
                return NotFound($"DomainEnvironment with ID {id} not found.");

            existingEnvironmentDomain.BpwebServerId = model.BpwebServerId;
            existingEnvironmentDomain.BpdatabaseId = model.BpdatabaseId;
            existingEnvironmentDomain.EaidatabaseId = model.EaidatabaseId;
            existingEnvironmentDomain.SsrsserverId = model.SsrsserverId;
            existingEnvironmentDomain.TableauServerId = model.TableauServerId;
            existingEnvironmentDomain.EaiftpserverId = model.EaiftpserverId;
            existingEnvironmentDomain.IsBp5Enabled = model.IsBp5Enabled;

            context.SaveChanges();

            return Ok(context.DomainEnvironments);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteDomainEnvironment(int id) {
        try{
            var context = new MasterContext();
            Console.WriteLine("===============> DELETE /api/domainEnvironment");
            var domainEnvironmentToDelete = context.DomainEnvironments.FirstOrDefault(e => e.DomainEnvironmentId == id);

            if (domainEnvironmentToDelete == null)
                return NotFound($"Domain with ID {id} not found.");

            context.DomainEnvironments.Remove(domainEnvironmentToDelete);
            context.SaveChanges();

            var remainingDomains = context.DomainEnvironments.ToList();
            return Ok(remainingDomains);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    public class EnvironmentModel {
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