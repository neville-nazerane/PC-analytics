using PcAnalytics.ClientBackground;
using PcAnalytics.ClientBackground.Services;

var builder = Host.CreateApplicationBuilder(args);
var configs = builder.Configuration;
if (builder.Environment.IsDevelopment())
    configs.AddUserSecrets("analytics client");

var fullConfigPath = Path.GetFullPath("../configs.json");
configs.AddJsonFile(fullConfigPath, true, true);

var services = builder.Services;

services.AddHttpClient<LocalApiConsumer>(c =>
{
    c.BaseAddress = new(configs["localEndpoint"] ?? throw new Exception("local endpoint not configured"));
});

services.AddHostedService<Worker>();


var host = builder.Build();


await host.RunAsync();
