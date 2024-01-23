using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Project.Controllers.Login;

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
      // Utils.AddLogin("simadaniel@hotmail.com", "STL");
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
    // private static void _create_password_hash(string password, out byte[] passwordHash, out string passwordSalt)
    // {
    //   using (var hmac = new System.Security.Cryptography.HMACSHA512())
    //   {
    //     passwordSalt = hmac.Key;
    //     passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
    //   }
    // }
  }
}
/***************************************************************************************/
/***************************************************************************************/
/***************************************************************************************/
