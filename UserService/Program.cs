using FluentValidation;
using FluentValidation.AspNetCore;
using UserService.Database;
using UserService.Repositories;
using UserService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();


var connectionString = builder.Configuration.GetConnectionString("PostgresDb");
builder.Services.AddSingleton<IUserRepository>(provider => new UserRepository(connectionString));
await DatabaseInitializer.InitializeAsync(connectionString);

var app = builder.Build();

app.MapGrpcService<UserApiService>();
app.MapGet("/", () => "UserService is running. Use a gRPC client to interact with the API.");


await app.RunAsync("http://*:5002");
