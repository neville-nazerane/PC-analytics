using PcAnalytics.OnlineBackground;
using PcAnalytics.ServerLogic.Utils;

var builder = Host.CreateApplicationBuilder(args);

builder.Services
        .AddHostedService<Worker>()
        .AddLogicServices(builder.Configuration);

var host = builder.Build();

await host.RunAsync();
