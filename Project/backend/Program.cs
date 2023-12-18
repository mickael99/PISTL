using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Project.Models;



/***************************************************************************************/
/***************************************************************************************/
/***************************************************************************************/
namespace DataBaseFirstDemo
{
  class Program
  {
    /***************************************************************************************/
    static void Main(string[] args)
    {
      // addLogin("simadaniel@hotmail.com", "STL");
      // addDomain("STL");

      CreateHostBuilder(args).Build().Run();

    }

    /***************************************************************************************/
    /// <summary>
    /// Provides a mechanism for configuring and building an instance of <see cref="IHost"/>.
    /// </summary>
    /// <param name="args">The command-line arguments.</param>
    /// <returns>An instance of <see cref="IHostBuilder"/>.</returns>
    public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder =>
        {
          webBuilder.UseUrls("http://localhost:5050");
          webBuilder.UseStartup<Startup>();
        });

    /***************************************************************************************/
    /// <summary>
    /// Creates a password hash and salt using HMACSHA512 algorithm.
    /// </summary>
    /// <param name="password">The password to be hashed.</param>
    /// <param name="passwordHash">The resulting password hash.</param>
    /// <param name="passwordSalt">The resulting password salt.</param>
    /// 
    private static void _create_password_hash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
      using (var hmac = new System.Security.Cryptography.HMACSHA512())
      {
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
      }
    }

    /***************************************************************************************/
    /// <summary>
    /// Adds a new login with the specified email and password to the database.
    /// </summary>
    /// <param name="email">The email of the login.</param>
    /// <param name="password">The password of the login.</param>
    static void addLogin(string email, string password)
    {
      byte[] passwordHash, passwordSalt;
      _create_password_hash(password, out passwordHash, out passwordSalt);
      var newLogin = new Login
      {
        Email = email,
        Name = "Daniel",
        Password = passwordHash,
        PasswordSalt = passwordSalt,
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

    /***************************************************************************************/
    /// <summary>
    /// Adds a new domain to the database.
    /// </summary>
    /// <param name="domainName">The name of the domain.</param>
    static void addDomain(string domainName)
    {
      var newDomain = new Domain
      {
        Name = domainName,
        Logo = Encoding.UTF8.GetBytes("web"),
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
/***************************************************************************************/
/***************************************************************************************/
/***************************************************************************************/

