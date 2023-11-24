using System;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Project.Models;


namespace DataBaseFirstDemo
{
  class Program
  {
    static void Main(string[] args)
    {
      // addLogin("toto@toto.com", "Toto");
      // addDomain("STL");

      CreateHostBuilder(args).Build().Run();

    }
    public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder =>
        {
          webBuilder.UseUrls("http://localhost:5050");
          webBuilder.UseStartup<Startup>();
        });

    // create a new method to add a new login to the database  
    static void addLogin(string email, string name)
    {
      var newLogin = new Login
      {
        Email = email,
        Name = name,
        Password = Encoding.UTF8.GetBytes("123456"),
        PasswordSalt = "123456",
        PasswordModifiedDate = DateTime.Now,
        PasswordExpirationDate = DateTime.Now.AddDays(30),
        InvalidAttemptCount = 3,
        ResetPasswordEndDate = DateTime.Now.AddDays(1),
        ResetPasswordKey = "654321",
        ResetPasswordSentCount = 1,
        ResetPasswordInvalidAttemptCount = 1,
        LastLoginDate = DateTime.Now,
        TermsAccepted = true,
        Datenabled = true,
        Phone = "123456789",
        BlockedReason = "No reason",
        CreatedDate = DateTime.Now,
        CreatedBy = "Daniel",
        ModifiedDate = DateTime.Now,
        ModifiedBy = "Daniel"
      };
      var context = new MasterContext();
      context.Logins.Add(newLogin);
      context.SaveChanges();
    }

    // create a domain to add to the database
    static void addDomain(string domainName)
    {
      var newDomain = new Domain
      {
        Name = domainName,
        Logo = Encoding.UTF8.GetBytes("https://icons8.com/icon/sq6U5EJQHvHy/design"),
        Edition = "Enterprise",
        IsSsoEnabled = true,
        Comment = "No comment",
        ParentCompany = "No parent company",
        CreatedDate = DateTime.Now,
        CreatedBy = "Daniel",
        ModifiedDate = DateTime.Now,
        ModifiedBy = "Daniel"
      };
      var context = new MasterContext();
      context.Domains.Add(newDomain);
      context.SaveChanges();
    }
  }
}


