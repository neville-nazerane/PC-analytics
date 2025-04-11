using PcAnalytics.ClientApi.Service;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


var configs = builder.Configuration;
var services = builder.Services;


services.AddHttpClient<OnlineConsumer>(c =>
{
    c.BaseAddress = new(configs["onlineEndpoint"] ?? throw new Exception("online endpoint not configured"));
});

app.MapGet("/", () => "Hello Client World!");

await app.RunAsync();
