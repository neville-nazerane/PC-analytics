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

        public static Task UploadInputsAsync(StorageService storageService,
                                                   OnlineConsumer onlineConsumer,
                                                   CancellationToken cancellationToken = default)
        {
            var toUpload = new List<SensorInput>();

            return storageService.ReadAndRemoveSensorInputsAsync(input =>
                                           {
                                               if (toUpload.Count < 200)
                                               {
                                                   toUpload.Add(input);
                                                   return true;
                                               }
                                               return false;
                                           }, async () =>
                                           {
                                               try
                                               {
                                                   await onlineConsumer.UploadAsync(toUpload, cancellationToken);
                                                   return true;
                                               }
                                               catch
                                               {
                                                   return false;
                                               }
                                           }, cancellationToken);
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
