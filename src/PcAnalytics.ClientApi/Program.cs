using PcAnalytics.ClientApi.Service;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


var configs = builder.Configuration;
var services = builder.Services;

services.AddSingleton(p => new StorageService(configs["storagePath"] ?? throw new Exception("storage path not configured")));
services.AddHttpClient<OnlineConsumer>(c =>
{
    c.BaseAddress = new(configs["onlineEndpoint"] ?? throw new Exception("online endpoint not configured"));
});

app.MapGet("/", () => "Hello Client World!");

await app.RunAsync();
