using System.Data;
using FluentValidation;
using FluentValidation.AspNetCore;
using Npgsql;
using UserService.Database;

var builder = WebApplication.CreateBuilder(args);

// Строка подключения
string? connectionString = builder.Configuration.GetConnectionString("PostgresDb");
builder.Services.AddScoped<IDbConnection>(sp => new NpgsqlConnection(connectionString));

builder.Services.AddGrpc();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

// Инициализация базы данных
using (var scope = app.Services.CreateScope())
{
    var dbConnection = scope.ServiceProvider.GetRequiredService<IDbConnection>();
    await DatabaseInitializer.InitializeDatabaseAsync(dbConnection);
}

await app.RunAsync("http://*:5002");