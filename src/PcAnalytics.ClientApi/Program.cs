using LibreHardwareMonitor.Hardware.Motherboard;
using LibreHardwareMonitor.Hardware;
using PcAnalytics.ClientApi.Service;
using PcAnalytics.ClientApi;

var builder = WebApplication.CreateBuilder(args);


var configs = builder.Configuration;
var services = builder.Services;

services.AddSingleton(p =>
{
    return new StorageService(configs["storagePath"] ?? throw new Exception("storage path not configured"));
});
services.AddHttpClient<OnlineConsumer>(c =>
{
    c.BaseAddress = new(configs["onlineEndpoint"] ?? throw new Exception("online endpoint not configured"));
});
var infoService = new InfoService();
infoService.Init();
services.AddSingleton(infoService);

var app = builder.Build();

app.MapGet("/", () => "Hello Client World!");
app.MapEndpoints();

await app.RunAsync();


