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

using UserService.Kafka;

class Program
{
    static async Task Main(string[] args)
    {
        var kafkaProducer = new KafkaEventProducer("localhost:9092", "user-events");
        var scheduler = new EventScheduler(kafkaProducer);

        Console.WriteLine("Введите команду:");
        Console.WriteLine("1. add <userId> <endpoint> <rpm> - добавить или изменить задание.");
        Console.WriteLine("2. update <userId> <endpoint> <rpm> - изменить параметры задания.");
        Console.WriteLine("3. stop <userId> - остановить отправку событий для пользователя.");
        Console.WriteLine("4. exit - завершить программу.");

        while (true)
        {
            var input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
                continue;

            var parts = input.Split(' ');

            switch (parts[0])
            {
                case "add":
                case "update":
                    if (parts.Length < 4 || !int.TryParse(parts[1], out int userId) || !int.TryParse(parts[3], out int rpm))
                    {
                        Console.WriteLine("Неверный формат команды. Используйте: add <userId> <endpoint> <rpm>");
                        continue;
                    }

                    var endpoint = parts[2];
                    scheduler.AddOrUpdateUserEvent(new UserEventConfig
                    {
                        UserId = userId,
                        Endpoint = endpoint,
                        Rpm = rpm
                    });
                    Console.WriteLine($"Задание для пользователя {userId} обновлено: endpoint={endpoint}, rpm={rpm}");
                    break;

                case "stop":
                    if (parts.Length < 2 || !int.TryParse(parts[1], out userId))
                    {
                        Console.WriteLine("Неверный формат команды. Используйте: stop <userId>");
                        continue;
                    }

                    scheduler.RemoveUserEvent(userId);
                    Console.WriteLine($"Отправка событий для пользователя {userId} остановлена.");
                    break;

                case "exit":
                    Console.WriteLine("Завершение программы...");
                    return;

                default:
                    Console.WriteLine("Неизвестная команда. Попробуйте снова.");
                    break;
            }
        }
    }
}