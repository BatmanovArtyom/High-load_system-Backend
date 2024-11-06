using RateLimiter.Writer.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();

builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

builder.Services.AddSingleton<DatabaseInitializer>();

var app = builder.Build();

await app.RunAsync("http://*:5001");