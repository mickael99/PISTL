global using DAT_project.API.Models;
global using Microsoft.EntityFrameworkCore;
using DAT_project.API.Repositories.Implementation;
using DAT_project.API.Repositories.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<DatdbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DAT_projectConnectionString"));
}) ;

builder.Services.AddScoped<ILoginRepository, LoginRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseCors(options =>
{
    options.AllowAnyHeader();
    options.AllowAnyOrigin();
    options.AllowAnyMethod();
});

app.UseAuthorization();

app.MapRazorPages();

app.Run();
