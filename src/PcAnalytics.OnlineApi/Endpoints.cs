using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
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

            group.MapPost("sensorInputs", AddSensorInputAsync);
            group.MapGet("computers", GetComputersAsync);
            group.MapGet("computer/{computerId}/hardware", GetHardwareAsync);

            return group;
        }

        static async Task AddSensorInputAsync(HttpRequest request,
                                                    [FromBody] IEnumerable<SensorInput> input,
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

                if (computerId == 0)
                {
                    var entity = await dbContext.Computers.AddAsync(new() { Identifier = serial }, cancellationToken);
                    await dbContext.SaveChangesAsync(cancellationToken);
                    computerId = entity.Entity.Id;
                }

                var dbHardwares = await dbContext.GetHardwaresAsync(computerId, input, cancellationToken: cancellationToken);
                var types = await dbContext.GetSensorTypesAsync(computerId, input, cancellationToken);
                var sensorGroups = await dbContext.GetSensorGroupsAsync(dbHardwares, input, cancellationToken);

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
            else throw new Exception("No serial header found");
        }

        static async Task<IEnumerable<Computer>> GetComputersAsync(AppDbContext dbContext, CancellationToken cancellationToken = default)
            => await dbContext.Computers.ToListAsync(cancellationToken);

        static async Task<IEnumerable<Hardware>> GetHardwareAsync(AppDbContext dbContext, int computerId, CancellationToken cancellationToken = default) 
            => await dbContext.Hardwares
                                .Include(h => h.SensorGroups)
                                .Where(h => h.ComputerId == computerId)
                                .ToListAsync(cancellationToken);

    }
}
