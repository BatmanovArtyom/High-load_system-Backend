// using FluentValidation;
// using FluentValidation.AspNetCore;
// using UserService.Controller;
// using UserService.Database;
// using UserService.Domain_Service;
// using UserService.Mapping;
// using UserService.Models;
// using UserService.Repositories;
// using UserService.Validators;
//
// var builder = WebApplication.CreateBuilder(args);
//
// builder.Services.AddGrpc();
// builder.Services.AddFluentValidationAutoValidation();
// builder.Services.AddScoped<UserServiceDomain>();
// builder.Services.AddScoped<UserApiService>();
// builder.Services.AddValidatorsFromAssemblyContaining<Program>();
//
// builder.Logging.ClearProviders();
// builder.Logging.AddConsole();
//
//
// var connectionString = builder.Configuration.GetConnectionString("PostgresDb");
// builder.Services.AddSingleton<IUserRepository>(provider => new UserRepository(connectionString));
// builder.Services.AddSingleton<IUserMapping, UserMapping>();
// builder.Services.AddSingleton<IValidator<User>, CreateUserValidator>();
// builder.Services.AddSingleton<IValidator<User>, UpdateUserValidator>();
// await DatabaseInitializer.InitializeAsync(connectionString);
// var app = builder.Build();
//
// app.MapGrpcService<UserApiService>();
// app.MapGet("/", () => "UserService is running. Use a gRPC client to interact with the API.");
//
//
// await app.RunAsync("http://*:5002");

using System;
using UserService.Kafka;

class Program
{
    static async Task Main(string[] args)
    {
        var kafkaProducer = new KafkaEventProducer("localhost:9092", "user-events");
        var scheduler = new EventScheduler(kafkaProducer);

        scheduler.AddOrUpdateUserEvent(new UserEventConfig
        {
            UserId = 123,
            Endpoint = "v1.0/users/getById",
            Rpm = 10
        });

        scheduler.AddOrUpdateUserEvent(new UserEventConfig
        {
            UserId = 321,
            Endpoint = "v1.0/users/getById",
            Rpm = 20
        });

        await Task.Delay(10000);

        scheduler.AddOrUpdateUserEvent(new UserEventConfig
        {
            UserId = 321,
            Endpoint = "v1.0/users/update",
            Rpm = 25
        });

        Console.WriteLine("Нажмите любую клавишу для завершения...");
        Console.ReadKey();
    }
}
