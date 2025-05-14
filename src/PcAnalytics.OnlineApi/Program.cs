using Microsoft.AspNetCore.Http.Json;
using PcAnalytics.OnlineApi;
using PcAnalytics.ServerLogic.Utils;

var builder = WebApplication.CreateBuilder(args);

var configs = builder.Configuration;
var services = builder.Services;

builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    options.SerializerOptions.MaxDepth = 4;
});


services.AddLogicServices(configs);

var app = builder.Build();

app.MapGet("/", () => "Hello PC World!");

app.MapEndpoints();

await app.RunAsync();
