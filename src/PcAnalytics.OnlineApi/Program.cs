using PcAnalytics.OnlineApi;
using PcAnalytics.ServerLogic.Utils;

var builder = WebApplication.CreateBuilder(args);

var configs = builder.Configuration;
var services = builder.Services;

services.AddLogicServices(configs);

var app = builder.Build();

app.MapGet("/", () => "Hello PC World!");

app.MapEndpoints();

await app.RunAsync();
