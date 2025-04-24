
using Microsoft.EntityFrameworkCore;
using PcAnalytics.ServerLogic;

namespace PcAnalytics.OnlineBackground;

public class Worker(ILogger<Worker> logger, IServiceProvider serviceProvider) : BackgroundService
{

    private static readonly TimeSpan RetainTime = TimeSpan.FromHours(5);


    private readonly ILogger<Worker> _logger = logger;
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            await using (var scope = _serviceProvider.CreateAsyncScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var timestamp = DateTime.UtcNow - RetainTime;

                await db.Records.Where(r => r.CreatedOn < timestamp)
                                .ExecuteDeleteAsync(cancellationToken: stoppingToken);
            }

            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }
}
