using RateLimiter.Writer.Controller;
using RateLimiter.Writer.Database;
using RateLimiter.Writer.DomainService.Service;
using RateLimiter.Writer.Models.mapper;
using RateLimiter.Writer.Repository;


var builder = WebApplication.CreateBuilder(args);


builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("ConnectionStrings"));

builder.Services.AddSingleton<DatabaseInitializer>();
builder.Services.AddScoped<IWriterDomainService, WriterDomainService>();
builder.Services.AddScoped<IRateLimitRepository, RateLimitRepository>();
builder.Services.AddScoped<IRateLimitMapper, RateLimitMapper>();
builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<WriterService>();

app.MapGet("/", () => "gRPC Writer Service is running on http://localhost:5001");

await app.RunAsync("http://*:5001");
