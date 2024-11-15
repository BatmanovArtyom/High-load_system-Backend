using RateLimiter.Reader.Controllers;
using RateLimiter.Reader.Database;
using RateLimiter.Reader.DomainService;
using RateLimiter.Reader.Repository;


var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("ConnectionStrings"));

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddSingleton<IRateLimitRepository, RateLimitRepository>();
builder.Services.AddHostedService<ReaderService>();
builder.Services.AddSingleton<RateLimitLoader>();
builder.Services.AddSingleton<RateLimitWatcher>();
builder.Services.AddSingleton<DatabaseInitializer>();
builder.Services.AddSingleton<RateLimitMemoryStore>();

builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<ReaderController>();

app.MapGet("/", () => "gRPC service is running. Use a gRPC client to connect.");

await app.RunAsync("http://*:5000");