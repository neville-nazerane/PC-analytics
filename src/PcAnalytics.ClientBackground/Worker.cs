using LibreHardwareMonitor.Hardware;
using LibreHardwareMonitor.Hardware.Motherboard;
using Microsoft.VisualBasic;
using PcAnalytics.ClientBackground.Services;
using PcAnalytics.Models;

namespace PcAnalytics.ClientBackground;

public class Worker(ILogger<Worker> logger,
                    IConfiguration configuration,
                    IServiceProvider serviceProvider) : BackgroundService
{

    private static readonly TimeSpan interval = TimeSpan.FromSeconds(5);
    
    private readonly ILogger<Worker> _logger = logger;
    private readonly IConfiguration _configuration = configuration;
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        var computer = new Computer
        {
            IsCpuEnabled = true,
            IsGpuEnabled = true,
            IsMemoryEnabled = true,
            IsMotherboardEnabled = true,
            IsControllerEnabled = true
        };
        computer.Open();


        await using (var scope = _serviceProvider.CreateAsyncScope())
        {
            var client = scope.ServiceProvider.GetRequiredService<LocalApiConsumer>();
            //client.SetComputerSerial(serial);
        }


        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Logging next...");

            var incomings = new List<SensorInput>();

            foreach (var hw in computer.Hardware)
            {
                hw.Update();

                var createdOn = DateTime.UtcNow;

                foreach (var sensor in hw.Sensors)
                {
                    incomings.Add(new()
                    {
                        CreatedOn = createdOn,
                        HardwareName = hw.Name,
                        SensorGroupName = sensor.Name,
                        SensorType = sensor.SensorType.ToString(),
                        Value = sensor.Value,
                    });
                }
            }


            _logger.LogInformation("Done recording next...");


            await Task.Delay(interval, stoppingToken);
        }
    }
}
