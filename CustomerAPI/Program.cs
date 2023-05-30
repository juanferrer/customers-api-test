using Microsoft.EntityFrameworkCore;
using CustomerAPI.Models;
using System.Configuration;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// DB
//builder.Services.AddDbContext<CustomerContext>(options => options.UseInMemoryDatabase("Customers"));
builder.Services.AddDbContext<CustomerContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString(nameof(CustomerContext))));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
