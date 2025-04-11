using PcAnalytics.ClientBackground;
using PcAnalytics.ClientBackground.Services;

var builder = Host.CreateApplicationBuilder(args);

var configs = builder.Configuration;
var services = builder.Services;

services.AddHttpClient<OnlineConsumer>(c =>
{
    c.BaseAddress = new(configs["onlineEndpoint"] ?? throw new Exception("online endpoint not configured"));
});

services.AddHostedService<Worker>();


var host = builder.Build();


await host.RunAsync();
