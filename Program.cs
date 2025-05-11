using Microsoft.EntityFrameworkCore;
using ProductswebAPI.Models;
using ProductswebAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Configure Kestrel to listen on all interfaces at port 80
builder.WebHost.UseUrls("http://0.0.0.0:80");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("dsc")));

builder.Services.AddControllers(); // Add support for controllers
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Enable Swagger in both Development and Production
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers(); 

app.Run();
