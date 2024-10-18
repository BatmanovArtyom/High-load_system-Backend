// using FluentValidation;
// using FluentValidation.AspNetCore;
// using UserService.Database;
// using UserService.Repositories;
// using UserService.Services;
//
// var builder = WebApplication.CreateBuilder(args);
//
// builder.Services.AddGrpc();
// builder.Services.AddFluentValidationAutoValidation();
// builder.Services.AddValidatorsFromAssemblyContaining<Program>();
// builder.Logging.ClearProviders();
// builder.Logging.AddConsole();
//
// var connectionString = builder.Configuration.GetConnectionString("PostgresDb");
//
// await DatabaseInitializer.InitializeAsync(connectionString);
//
// var app = builder.Build();
// await app.RunAsync("http://*:5002");

using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using UserService.Database;
using UserService.Repositories;
using UserService.Services;

var builder = WebApplication.CreateBuilder(args);

// Добавление gRPC и FluentValidation
builder.Services.AddGrpc();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// Настройка логирования
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Получение строки подключения к базе данных
var connectionString = builder.Configuration.GetConnectionString("PostgresDb");

// Добавление сервисов
builder.Services.AddSingleton<IUserRepository>(provider => new UserRepository(connectionString));

// Инициализация базы данных (создание таблиц, индексов и функций/процедур)
await DatabaseInitializer.InitializeAsync(connectionString);

var app = builder.Build();

// Настройка маршрутов для gRPC
app.MapGrpcService<UserApiService>();

// Стандартные маршруты (для проверки gRPC-запросов)
app.MapGet("/", () => "UserService is running. Use a gRPC client to interact with the API.");

// Запуск приложения
await app.RunAsync("http://*:5002");
