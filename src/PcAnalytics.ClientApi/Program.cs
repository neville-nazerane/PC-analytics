using LibreHardwareMonitor.Hardware.Motherboard;
using LibreHardwareMonitor.Hardware;
using PcAnalytics.ClientApi.Service;
using PcAnalytics.ClientApi;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

var builder = WebApplication.CreateBuilder(args);


var configs = builder.Configuration;
var services = builder.Services;

var fullConfigPath = Path.GetFullPath("../configs.json");
configs.AddJsonFile(fullConfigPath, true, true);

//services.AddSingleton(p =>
//{
//    return new StorageService(configs["storagePath"] ?? throw new Exception("storage path not configured"));
//});

var dbLocation = Path.Combine(configs["storagePath"] ?? throw new Exception("storage path not configured"), "localstore.db");
services.AddDbContext<AppDbContext>(o => o.UseSqlite($"Data Source={dbLocation}"));
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


await SetupAsync(app.Services);

await app.RunAsync();


async Task SetupAsync(IServiceProvider serviceProvider)
{
    await using var scope = serviceProvider.CreateAsyncScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
}

