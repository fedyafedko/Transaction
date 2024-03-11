using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Transaction.BLL.Services;
using Transaction.BLL.Services.Interfaces;
using Transaction.Common.Configs;
using Transaction.DAL.EF;
using Transaction.Seeding.Extentions;
using Transaction.Utility;

var builder = WebApplication.CreateBuilder(args);
var defaultConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//Configs
builder.Services.Configure<ExcelConfig>(builder.Configuration.GetSection("Excel"));

//Routing
builder.Services.AddControllers(options => options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer())));

// Configure Entity Framework Core DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(defaultConnectionString));

// Configure IDbConnection using Dapper
builder.Services.AddScoped<IDbConnection>(serviceProvider =>
    new SqlConnection(defaultConnectionString));

//Services
builder.Services.AddScoped<ITransactionService, TransactionService>();

// Seeding
builder.Services.AddSeeding();

builder.Services.AddControllers();
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

await app.ApplySeedingAsync();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
