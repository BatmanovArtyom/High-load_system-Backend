using FluentValidation;
using FluentValidation.AspNetCore;
using UserService.Controller;
using UserService.Database;
using UserService.Domain_Service;
using UserService.Mapping;
using UserService.Models;
using UserService.Repositories;
using UserService.Validators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddScoped<UserServiceDomain>();
builder.Services.AddScoped<UserApiService>();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();


var connectionString = builder.Configuration.GetConnectionString("PostgresDb");
builder.Services.AddSingleton<IUserRepository>(provider => new UserRepository(connectionString, provider.GetRequiredService<IUserMapping>()));
builder.Services.AddSingleton<IUserMapping, UserMapping>();
builder.Services.AddSingleton<IValidator<User>, CreateUserValidator>();
builder.Services.AddSingleton<IValidator<User>, UpdateUserValidator>();
await DatabaseInitializer.InitializeAsync(connectionString);
var app = builder.Build();

app.MapGrpcService<UserApiService>();
app.MapGet("/", () => "UserService is running. Use a gRPC client to interact with the API.");


await app.RunAsync("http://*:5002");
