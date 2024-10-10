using FluentValidation;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Logging.ClearProviders();
builder.Logging.AddConsole(); 

var app = builder.Build();

await app.RunAsync("http://*:5002");