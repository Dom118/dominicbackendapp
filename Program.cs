using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MinimalX.Data;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Identity;

var builder = WebApplication.CreateBuilder(args);

// Set the HTTPS port
// builder.WebHost.UseSetting("https_port", "7073");

builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient", policy =>
    {
        policy.WithOrigins("https://dominicfrontendapp-dgdugkhndkh7e8et.eastus2-01.azurewebsites.net")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("AllowBlazorClient");
app.UseHttpsRedirection();

app.Run();
