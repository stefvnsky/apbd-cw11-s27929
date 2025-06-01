using EFCodeFirst.Data;
using EFCodeFirst.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container. Kontrolery
builder.Services.AddControllers();

//konfiguracja kontekstu bazy , polaczenie z baza
//ConnectionString jest pobierany z appsettings.json, oczywiscie nalego go tam tez ustawic

builder.Services.AddDbContext<DatabaseContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

//wstrzykiwanie zależności
//https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection

builder.Services.AddScoped<IDbService, DbService>();

var app = builder.Build();

//configure the HTTP request pipeline

app.UseAuthorization();
app.MapControllers();
app.Run();