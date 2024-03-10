using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Transaction.Common.Configs;
using Transaction.DAL.EF;
using Transaction.Seeding.Extentions;

var builder = WebApplication.CreateBuilder(args);

//Configs
builder.Services.Configure<ConnectionStringsConfig>(builder.Configuration.GetSection("ConnectionStrings"));
builder.Services.Configure<ExcelConfig>(builder.Configuration.GetSection("Excel"));


// DbContext
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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
