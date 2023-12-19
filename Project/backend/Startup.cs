using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Project.Interface;
using Project.Repository;
using Project.Data;
using Microsoft.Extensions.Configuration;

public class Startup 
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin", builder =>
            {
                builder.WithOrigins("http://localhost:4200")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        services.AddDbContext<DatabaseContext>(options =>
            options.UseSqlServer("Server=LAPTOP-C49R77JJ/SQLEXPRESS;Database=master;TrustServerCertificate=true;Trusted_Connection=True;"));
            
        services.AddScoped<IDatabaseRepository, DatabaseRepository>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) 
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseCors("AllowSpecificOrigin"); 

        app.UseStaticFiles();

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });
    }
}

