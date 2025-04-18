using LibreHardwareMonitor.Hardware.Motherboard;
using LibreHardwareMonitor.Hardware;
using PcAnalytics.ClientApi.Service;
using PcAnalytics.ClientApi;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


var configs = builder.Configuration;
var services = builder.Services;

//services.AddSingleton(p =>
//{
//    return new StorageService(configs["storagePath"] ?? throw new Exception("storage path not configured"));
//});
services.AddDbContext<AppDbContext>(o => o.UseSqlite($"Data Source={configs["storagePath"]}/localstore.db"));
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

