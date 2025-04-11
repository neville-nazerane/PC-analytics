using PcAnalytics.ClientApi.Service;
using PcAnalytics.Models;

namespace PcAnalytics.ClientApi
{
    public static class Endpoints
    {

        public static IEndpointConventionBuilder MapEndpoints(this IEndpointRouteBuilder builder)
        {
            var group = builder.MapGroup("");

            group.MapPost("sensorInputs", AddSensorInputsAsync);
            group.MapGet("sensorInputs", GetLocalSensorInputsAsync);
            group.MapPost("sensorInputs/upload", UploadInputsAsync);

            return group;
        }

        public static async Task UploadInputsAsync(StorageService storageService,
                                             OnlineConsumer onlineConsumer,
                                                CancellationToken cancellationToken = default)
        {
            var toUpload = new List<SensorInput>();
            var items = storageService.ReadSensorInputsAsync(cancellationToken);

            bool stoppedInbetween = false;

            await foreach (var item in items)
            {
                toUpload.Add(item);
                if (toUpload.Count > 200)
                {
                    stoppedInbetween = true;
                    break;
                }
            }

            await onlineConsumer.UploadAsync(toUpload, cancellationToken);

        }

        public static Task AddSensorInputsAsync(IEnumerable<SensorInput> inputs,
                                                StorageService storageService,
                                                CancellationToken cancellationToken = default)
            => storageService.StoreAsync(inputs, cancellationToken);

        public static IAsyncEnumerable<SensorInput> GetLocalSensorInputsAsync(StorageService storageService,
                                                                              CancellationToken cancellationToken = default)
            => storageService.ReadSensorInputsAsync(cancellationToken);

    }
}
