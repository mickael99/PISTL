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
          webBuilder.UseUrls("http://*:5050");
          webBuilder.UseStartup<Startup>();
        });
  }
}
/***************************************************************************************/
/***************************************************************************************/
/***************************************************************************************/
