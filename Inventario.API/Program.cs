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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Supabase;

using System.Text;

var builder = WebApplication.CreateBuilder(args);
var jwtSecret = builder.Configuration["Supabase:JwtSecret"];
var key = Encoding.ASCII.GetBytes(jwtSecret);

// Pegue a URL e a Anon Key (essa você pega no painel do Supabase, mesma tela do JWT Secret)
var supabaseUrl = builder.Configuration["Supabase:Url"];
var supabaseKey = builder.Configuration["Supabase:AnonKey"]; // Adicione esta chave no appsettings.json

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
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Inventário API", Version = "v1" });

    // Configura a definição do esquema de segurança (JWT)
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "JWT Authorization header usando o esquema Bearer. \r\n\r\n Digite 'Bearer' [espaço] e depois o seu token.\r\n\r\nExemplo: \"Bearer 12345abcdef\""
    });

    // Aplica a segurança globalmente no Swagger
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
builder.Services.AddValidatorsFromAssemblyContaining<CreateItemInventarioValidator>();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false; // Em prod, mude para true
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false, // O Supabase às vezes varia o issuer, por enquanto deixamos false
        ValidateAudience = false
    };
});
builder.Services.AddScoped(_ => 
    new Client(supabaseUrl, supabaseKey, new SupabaseOptions { AutoConnectRealtime = true }));


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
