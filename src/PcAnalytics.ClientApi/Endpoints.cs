using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PcAnalytics.ClientApi.Service;
using PcAnalytics.Models;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace PcAnalytics.ClientApi
{
    public static class Endpoints
    {

        public static IEndpointConventionBuilder MapEndpoints(this IEndpointRouteBuilder builder)
        {
            var group = builder.MapGroup("");

            group.MapPost("sensorInputs", AddSensorInputsAsync);
            group.MapGet("sensorInputs", GetLocalSensorInputsAsync);
            group.MapPost("sensorInputs/uploadStored", UploadInputsAsync);

            return group;
        }

        public static async Task UploadInputsAsync(AppDbContext dbContext,
                                                   OnlineConsumer onlineConsumer,
                                                   CancellationToken cancellationToken = default)
        {
            var count = await dbContext.SensorInputs.CountAsync(cancellationToken: cancellationToken);

            for (int i = 0; i < count / 200; i++)
            {
                var items = await dbContext.SensorInputs
                                           .OrderBy(i => i.CreatedOn)
                                           .Take(200)
                                           .ToListAsync(cancellationToken: cancellationToken);

                await onlineConsumer.UploadAsync(items, cancellationToken);

                dbContext.SensorInputs.RemoveRange(items);
                await dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public static async Task AddSensorInputsAsync([FromBody] IEnumerable<SensorInput> inputs,
                                                      AppDbContext dbContext,
                                                      CancellationToken cancellationToken = default)
        {
            await dbContext.SensorInputs.AddRangeAsync(inputs, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public static async IAsyncEnumerable<SensorInput> GetLocalSensorInputsAsync(AppDbContext dbContext,
                                                                                    [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var res = dbContext.SensorInputs.AsAsyncEnumerable();
            await foreach (var item in res)
            {
                yield return item;
                cancellationToken.ThrowIfCancellationRequested();
            }
        }
    }
}
