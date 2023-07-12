using AutoMapper;
using EmployeeApi.DAL.Interfaces;
using EmployeeApi.DAL.Repositories;
using EmployeeApi.Data;
using EmployeeApi.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<EmployeeContext>(
    options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("EmployeeContext"));
    });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IEmployeeRepository, EmployeeRepository>();

//builder.Services.AddAutoMapper(typeof(EmployeeProfile));
builder.Services.AddAutoMapper(m => m.AddProfile<EmployeeProfile>(), typeof(EmployeeProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();

app.Run();
