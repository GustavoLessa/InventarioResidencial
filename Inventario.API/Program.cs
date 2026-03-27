using Inventario.Infrastructure.Context;
using Inventario.Domain.Interfaces;
using Inventario.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Inventario.Application.Interfaces;
using Inventario.Application.Services;
using FluentValidation;
using Inventario.Application.Validations;
using Inventario.API.Middleware;
using Inventario.Application.Mappings;

var builder = WebApplication.CreateBuilder(args);

// 1. Configurar a String de Conexão do Supabase (PostgreSQL)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseLazyLoadingProxies() // Adicione esta linha!
    .UseNpgsql(connectionString, 
        b => b.MigrationsAssembly("Inventario.Infrastructure"))); // Define onde as migrations serão salvas

// 2. Registrar os Repositórios e o Unit of Work
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IItemInventarioRepository, ItemInventarioRepository>();
builder.Services.AddScoped<ILocalRepository, LocalRepository>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IItemAppService, ItemAppService>();

builder.Services.AddControllers();
// ... resto do código (Swagger, etc)
// var builder = WebApplication.CreateBuilder(args);

// // Add services to the container.
// // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddValidatorsFromAssemblyContaining<CreateItemInventarioValidator>();
builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

// ATENÇÃO: O middleware de exceção deve ser um dos primeiros!
app.UseMiddleware<ExceptionMiddleware>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapControllers();   
app.UseHttpsRedirection();

// var summaries = new[]
// {
//     "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
// };

// app.MapGet("/weatherforecast", () =>
// {
//     var forecast =  Enumerable.Range(1, 5).Select(index =>
//         new WeatherForecast
//         (
//             DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//             Random.Shared.Next(-20, 55),
//             summaries[Random.Shared.Next(summaries.Length)]
//         ))
//         .ToArray();
//     return forecast;
// })
// .WithName("GetWeatherForecast")
// .WithOpenApi();

app.Run();

// record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
// {
//     public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
// }
