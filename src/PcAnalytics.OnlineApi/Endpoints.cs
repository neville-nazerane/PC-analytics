using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using PcAnalytics.Models;
using PcAnalytics.Models.Entities;
using PcAnalytics.ServerLogic;
using System.Collections.Immutable;
using System.Threading;

namespace PcAnalytics.OnlineApi
{
    public static class Endpoints
    {

        public static IEndpointConventionBuilder MapEndpoints(this IEndpointRouteBuilder endpoints)
        {
            var group = endpoints.MapGroup("");

            group.MapPost("incoming", AddIncomingAsync);

            return group;
        }

        public static async Task AddIncomingAsync(HttpRequest request,
                                                  IEnumerable<IncomingSensorInput> input,
                                                  AppDbContext dbContext,
                                                  CancellationToken cancellationToken = default)
        {

            if (request.Headers.TryGetValue("computerSerial", out var computerIds))
            {
                string serial = computerIds.First() ?? throw new Exception("No computer Id");

                int computerId = await dbContext.Computers
                                                .Where(c => c.Identifier == serial)
                                                .Select(c => c.Id)
                                                .SingleOrDefaultAsync(cancellationToken: cancellationToken);

                var dbHardwares = await dbContext.GetHardwaresAsync(computerId, input, cancellationToken: cancellationToken);
                var types = await dbContext.GetSensorTypesAsync(computerId, input, cancellationToken);
                var sensorGroups = await dbContext.GetSensorGroupsAsync(computerId, dbHardwares, input, cancellationToken);

                var hardwareDict = dbHardwares.ToDictionary(h => h.Name, h => h.Id);
                var typesDict = types.ToDictionary(t => t.Name, t => t.Id);

                foreach (var item in input)
                {
                    var sensorGroupId = sensorGroups.Single(s => s.Name == item.SensorGroupName && s.HardwareId == hardwareDict[item.HardwareName])
                                                    .Id;

                    var entity = new Record
                    {
                        CreatedOn = item.CreatedOn,
                        Value = item.Value,
                        SensorGroupId = sensorGroupId,
                        SensorTypeId = typesDict[item.SensorType]
                    };

                    await dbContext.AddAsync(entity, cancellationToken);
                }

                await dbContext.SaveChangesAsync(cancellationToken);

            }
        }


    }
}
