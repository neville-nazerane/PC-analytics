using Microsoft.EntityFrameworkCore;
using PcAnalytics.Models;
using PcAnalytics.Models.Entities;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcAnalytics.ServerLogic
{


    public class AppDbContext : DbContext
    {

        public DbSet<Computer> Computers { get; set; }

        public DbSet<Hardware> Hardwares { get; set; }

        public DbSet<SensorGroup> SensorGroups { get; set; }

        public DbSet<SensorType> SensorTypes { get; set; }

        public DbSet<Record> Records { get; set; }




        public async Task<IEnumerable<Hardware>> GetHardwaresAsync(int computerId,
                                                                        IEnumerable<IncomingSensorInput> input,
                                                                        CancellationToken cancellationToken = default)
        {
            var hardwareNames = input.Select(i => i.HardwareName).Distinct().ToImmutableArray();

            var query = Hardwares
                            .AsNoTracking()
                            .Where(h => h.ComputerId == computerId && hardwareNames.Contains(h.Name));

            var existingNames = await query.Select(h => h.Name)
                                           .ToListAsync(cancellationToken);

            var missing = hardwareNames.Except(existingNames).ToList();



            if (missing.Count > 0)
            {
                foreach (var name in missing)
                {
                    var hw = new Hardware
                    {
                        ComputerId = computerId,
                        Name = name
                    };
                    await Hardwares.AddAsync(hw, cancellationToken);
                }
                await SaveChangesAsync(cancellationToken);
            }

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<SensorGroup>> GetSensorGroupsAsync(int computerId,
                                                                         IEnumerable<Hardware> hardwares,
                                                                         IEnumerable<IncomingSensorInput> input,
                                                                         CancellationToken cancellationToken = default)
        {

            var groups = input.Select(i => new
            {
                i.SensorGroupName,
                HardwareId = hardwares.Single(h => h.Name == i.SensorGroupName).Id
            })
                .ToImmutableArray();

            var groupNames = input.Select(i => i.SensorGroupName).Distinct().ToImmutableArray();

            var query = SensorGroups
                                .AsNoTracking()
                                .Where(g => groups.Any(i => g.Name == i.SensorGroupName && g.HardwareId == i.HardwareId));

            var existingGroups = await query.ToListAsync(cancellationToken);

            var missing = groups
                            .Where(g => !existingGroups.Any(e => e.Name == g.SensorGroupName && e.HardwareId == g.HardwareId))
                            .ToList();


            if (missing.Count > 0)
            {
                foreach (var grp in missing)
                {
                    var group = new SensorGroup
                    {
                        Name = grp.SensorGroupName,
                        HardwareId = grp.HardwareId
                    };
                    await SensorGroups.AddAsync(group, cancellationToken);
                }
                await SaveChangesAsync(cancellationToken);
            }

            return await query.ToListAsync(cancellationToken);

        }

        public async Task<IEnumerable<SensorType>> GetSensorTypesAsync(int computerId,
                                                                IEnumerable<IncomingSensorInput> input,
                                                                CancellationToken cancellationToken = default)
        {
            var typeNames = input.Select(i => i.SensorType).Distinct().ToImmutableArray();

            var query = SensorTypes
                            .AsNoTracking()
                            .Where(t => typeNames.Contains(t.Name));

            var existingNames = await query.Select(t => t.Name)
                                           .ToListAsync(cancellationToken);

            var missing = typeNames.Except(existingNames).ToList();

            if (missing.Count > 0)
            {
                foreach (var name in missing)
                {
                    var type = new SensorType
                    {
                        Name = name
                    };
                    await SensorTypes.AddAsync(type, cancellationToken);
                }
                await SaveChangesAsync(cancellationToken);
            }

            return await query.ToListAsync(cancellationToken);
        }




    }
}
